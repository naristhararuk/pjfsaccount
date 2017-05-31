using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCG.DB.DTO;
using SS.Standard.WorkFlow.Query;
using SCG.DB.Query;
using SCG.eAccounting.Web.Helper;
using Spring.Validation;
using SCG.DB.BLL;
using SS.Standard.Utilities;
using SS.DB.DTO;
using SS.Standard.UI;
using SCG.eAccounting.Query;

namespace SCG.eAccounting.Web.UserControls.FormEditor.SCG.DB
{
    public partial class DbRejectReasonEditor : BaseUserControl
    {
        public IRejectReasonService RejectReasonService { get; set; }
        public IRejectReasonLangService RejectReasonLangService { get; set; }
        public event EventHandler Notify_Ok;
        public event EventHandler Notify_Cancle;
        public string Mode
        {
            get { return this.ViewState["Mode"] == null ? FlagEnum.NewFlag : (string)this.ViewState["Mode"]; }
            set { this.ViewState["Mode"] = value; }
        }
        public int DbRejectReasonID
        {
            get { return this.ViewState["DbRejectReasonID"] == null ? (int)0 : (int)this.ViewState["DbRejectReasonID"]; }
            set { this.ViewState["DbRejectReasonID"] = value; }
        }

        public void ResetValue()
        {
            ctlReasonCode.Text = string.Empty;
            ctlRequestTypeDropdown.SelectedIndex = -1;
            ctlCommentChk.Checked = false;
            ctlConfirmRejectionChk.Checked = false;
            ctlActivechk.Checked = true;
            this.BindRejectReasonLangGridview(DbRejectReasonID);
            ctlUpdatePanelReasonForm.Update();

        }
        public void Initialize(string mode, int id)
        {
            Mode = mode.ToString();
            this.BindDocumentTypeDropdown(ctlRequestTypeDropdown);
            
            DbRejectReasonID = id;
            if (mode.Equals(FlagEnum.EditFlag))
            {
                DbRejectReason rejectReason = ScgDbQueryProvider.DbRejectReasonQuery.FindByIdentity(DbRejectReasonID);
                ctlReasonCode.Text = rejectReason.ReasonCode;
                if (rejectReason.DocumentTypeID != null)
                {
                    try
                    {
                        ctlRequestTypeDropdown.SelectedValue = rejectReason.DocumentTypeID.ToString();
                        this.BindStateEventIDDropdown(ctlStateEventDropdown, ctlRequestTypeDropdown.SelectedValue);
                    }
                    catch (Exception)
                    {
                        ctlRequestTypeDropdown.SelectedIndex = 0;
                    }
                   
                }
                if (rejectReason.WorkFlowStateEventID != null)
                {
                    try
                    {
                        ctlStateEventDropdown.SelectedValue = rejectReason.WorkFlowStateEventID.ToString();
                    }
                    catch (Exception)
                    {
                        ctlStateEventDropdown.SelectedIndex = 0;
                    }
                    
                }
                ctlCommentChk.Checked = rejectReason.RequireComment;
                ctlConfirmRejectionChk.Checked = rejectReason.RequireConfirmReject;
                ctlActivechk.Checked = rejectReason.Active;

                this.BindRejectReasonLangGridview(DbRejectReasonID);

                ctlUpdatePanelReasonForm.Update();
            }
            else if (mode.ToString() == FlagEnum.NewFlag)
            {
                ResetValue();
            }
            this.Show();

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.BindDocumentTypeDropdown(ctlRequestTypeDropdown);
                this.BindStateEventIDDropdown(ctlStateEventDropdown, ctlRequestTypeDropdown.SelectedValue);
            }
        }


        protected void ctlAdd_Click(object sender, ImageClickEventArgs e)
        {

            DbRejectReason rejectReason = new DbRejectReason();
            if (Mode.Equals(FlagEnum.EditFlag))
            {
                rejectReason.ReasonID = DbRejectReasonID;
            }

            rejectReason.ReasonCode = ctlReasonCode.Text;
            if (!ctlRequestTypeDropdown.SelectedValue.Equals("0"))
            {
                rejectReason.DocumentTypeID = UIHelper.ParseInt(ctlRequestTypeDropdown.SelectedValue);
            }
            else
            {
                rejectReason.DocumentTypeID = null;
            }
            if (!ctlStateEventDropdown.SelectedValue.Equals("0"))
            {
                rejectReason.WorkFlowStateEventID = UIHelper.ParseInt(ctlStateEventDropdown.SelectedValue);
            }
            else
            {
                rejectReason.WorkFlowStateEventID = null;
            }
            rejectReason.RequireComment = ctlCommentChk.Checked;
            rejectReason.RequireConfirmReject = ctlConfirmRejectionChk.Checked;
            rejectReason.Active = ctlActivechk.Checked;
            try
            {
                // save or update RejectReason
                if (Mode.Equals(FlagEnum.EditFlag))
                {
                    RejectReasonService.UpdateReason(rejectReason);
                }
                else
                {
                    int reasonID = RejectReasonService.AddReason(rejectReason);
                    rejectReason.ReasonID = reasonID;

                }

                // save or update RejectReasonLang
                IList<DbRejectReasonLang> list = new List<DbRejectReasonLang>();

                foreach (GridViewRow row in ctlReasonLangGrid.Rows)
                {
                    short languageId = UIHelper.ParseShort(ctlReasonLangGrid.DataKeys[row.RowIndex]["LanguageID"].ToString());

                    TextBox Description = row.FindControl("ctlDescription") as TextBox;
                    TextBox Comment = (TextBox)row.FindControl("ctlCommentLang") as TextBox;
                    CheckBox Active = (CheckBox)row.FindControl("ctlActiveLang") as CheckBox;

                    if ((!string.IsNullOrEmpty(Description.Text)) || !string.IsNullOrEmpty(Comment.Text))
                    {
                        DbRejectReasonLang rejectReasonLang = new DbRejectReasonLang();
                        rejectReasonLang.Active = Active.Checked;
                        rejectReasonLang.ReasonDetail = Description.Text;
                        rejectReasonLang.Comment = Comment.Text;
                        rejectReasonLang.Language = new DbLanguage(languageId);
                        rejectReasonLang.Reason = rejectReason;

                        list.Add(rejectReasonLang);
                    }

                }

                RejectReasonLangService.UpdateRejectReasonLang(list,rejectReason.ReasonID);
                Notify_Ok(sender, e);

            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
            }


        }
        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            if (Notify_Cancle != null)
            {
                Notify_Cancle(sender, e);
            }
            this.Hide();

        }

        protected void ctlReasonLangGrid_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow row in ctlReasonLangGrid.Rows)
            {
                TextBox ctrDescription = row.FindControl("ctlDescription") as TextBox;
                TextBox ctrComment = row.FindControl("ctlCommentLang") as TextBox;
                CheckBox active = row.FindControl("ctlActiveLang") as CheckBox;

                if ((string.IsNullOrEmpty(ctrDescription.Text)) && (string.IsNullOrEmpty(ctrComment.Text)))
                {
                    active.Checked = true;
                }
            }
        }

        protected void ctlRequestTypeDropdown_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindStateEventIDDropdown(ctlStateEventDropdown, ctlRequestTypeDropdown.SelectedValue);
            ctlUpdatePanelReasonForm.Update();
        }

        public void Show()
        {
            ctlReasonModalPopupExtender.Show();
            ctlUpdatePanelReasonForm.Update();
        }
        public void Hide()
        {
            ctlReasonModalPopupExtender.Hide();
            ctlUpdatePanelReasonForm.Update();
        }
        public void BindStateEventIDDropdown(DropDownList dropdownList, string documentTypeID)
        {
            dropdownList.DataSource = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindRejectEventAndReason(UserAccount.CurrentLanguageID, UIHelper.ParseInt(documentTypeID));
            dropdownList.DataTextField = "StateEventID";
            dropdownList.DataValueField = "WorkFlowStateEventID";
            dropdownList.DataBind();
            dropdownList.Items.Insert(0, new ListItem("--Please Select--", "0"));

        }
        public void BindDocumentTypeDropdown(DropDownList dropdownList)
        {
            //dropdownList.DataSource = WorkFlowQueryProvider.DocumentTypeQuery.FindAll();
            //dropdownList.DataTextField = "DocumentTypeName";
            //dropdownList.DataValueField = "DocumentTypeID";
            dropdownList.DataSource = ScgeAccountingQueryProvider.SCGDocumentQuery.FindDocumentTypeCriteria();
            dropdownList.DataTextField = "Symbol";
            dropdownList.DataValueField = "ID";
            dropdownList.DataBind();
            dropdownList.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), "0"));

        }
        public void BindRejectReasonLangGridview(int rejectReasonID)
        {
            ctlReasonLangGrid.DataSource = ScgDbQueryProvider.RejectReasonLangQuery.FindReasonLangByReasonId(rejectReasonID);
            ctlReasonLangGrid.DataBind();
        }
    }
}
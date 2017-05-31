using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.DB.Query;
using System.Text;
using SCG.DB.DTO;
using SS.Standard.Utilities;
using SCG.eAccounting.Web.Helper;
using SCG.DB.BLL;
using SS.DB.DTO;
using SS.Standard.WorkFlow.DTO;

namespace SCG.eAccounting.Web.Forms.SCG.DB.Programs
{
    public partial class ManageReason : BasePage
    {
        public IDbReasonService DbReasonService { get; set; }
        public IDbReasonLangService DbReasonLangService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ctlDocumentTypeList.Items.Add(new ListItem("Travel", "T"));
                ctlDocumentTypeList.Items.Add(new ListItem("Advance", "V"));
                ctlDocumentTypeList.Items.Add(new ListItem("Expense", "E"));
                ctlDocumentTypeList.Items.Add(new ListItem("Remittance", "R"));

                ctlDocumentTypeList.DataBind();
            }
        }

        #region FormView
        protected void ctlReasonFormView_DataBound(object sender, EventArgs e)
        {
            DropDownList documentType = ctlReasonFormView.FindControl("ctlDocumentTypeList") as DropDownList;
            documentType.Items.Add(new ListItem("Travel", "T"));
            documentType.Items.Add(new ListItem("Advance", "V"));
            documentType.Items.Add(new ListItem("Expense", "E"));
            documentType.Items.Add(new ListItem("Remittance", "R"));

            ctlDocumentTypeList.DataBind();
            
        }
        protected void ctlReasonFormView_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            DbScgReason reason = new DbScgReason();
            GetReasonInfo(reason);
            try
            {
                DbReasonService.AddReason(reason);
                ctlRejectReasonGridView.DataCountAndBind();
                ctlReasonFormView.ChangeMode(FormViewMode.ReadOnly);
                ClosePopUp();
            }
            catch(ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        protected void ctlReasonFormView_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            short reasonId = UIHelper.ParseShort(ctlReasonFormView.DataKey.Value.ToString());
            DbScgReason reason = new DbScgReason(reasonId);
            GetReasonInfo(reason);
            try
            {
                DbReasonService.UpdateReason(reason);
                ctlRejectReasonGridView.DataCountAndBind();
                ClosePopUp();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        protected void ctlReasonFormView_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                ctlRejectReasonGridView.DataCountAndBind();
                ClosePopUp();
            }
        }
        protected void ctlReasonFormView_ModeChanging(object sender, FormViewModeEventArgs e)
        {

        }
        #endregion

        #region ReasonGridView
        protected void ctlRejectReasonGridView_DataBound(object sender, EventArgs e)
        {
            if(ctlRejectReasonGridView.Rows.Count > 0)
            {
                RegisterScriptForGridView();
                divButton.Visible = true;
            }
        }
        protected void ctlRejectReasonGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            short reasonId;
            if (e.CommandName.Equals("ReasonEdit"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                reasonId = UIHelper.ParseShort(ctlRejectReasonGridView.DataKeys[rowIndex].Value.ToString());

                ctlRejectReasonGridView.EditIndex = rowIndex;
                ctlReasonFormView.PageIndex = (ctlRejectReasonGridView.PageIndex * ctlRejectReasonGridView.PageSize) + rowIndex;
                ctlReasonFormView.ChangeMode(FormViewMode.Edit);
                IList<DbScgReason> list = new List<DbScgReason>();
                list.Add(ScgDbQueryProvider.DbReasonQuery.FindByIdentity(reasonId));
                ctlReasonFormView.DataSource = list;
                ctlReasonFormView.DataBind();
                ctlRejectReasonGridView.DataCountAndBind();
                ctlUpdatePanelReasonForm.Update();
                ctlReasonModalPopupExtender.Show();
                
            }

            if(e.CommandName.Equals("Select"))
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                reasonId = UIHelper.ParseShort(ctlRejectReasonGridView.DataKeys[rowIndex].Value.ToString());
                ctlReasonLangGrid.DataSource = ScgDbQueryProvider.DbReasonLangQuery.FindReasonLangByReasonId(reasonId);
                ctlReasonLangGrid.DataBind();
                ctlSubmit.Visible = true;
                ctlFieldSetDetailGridView.Visible = true;
                ctlDetailCancel.Visible = true;
                ctlRejectReasonGridView.DataCountAndBind();
                ctlUpdatePanelGridViewLang.Update();
            }
        }
        #endregion

        #region ReasonLangGrid
        protected void ctlReasonLangGrid_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow row in ctlReasonLangGrid.Rows)
            {
                TextBox detail = row.FindControl("ctlReasonDetail") as TextBox;
                TextBox comment = row.FindControl("ctlCommentLang") as TextBox;
                CheckBox active = row.FindControl("ctlActiveLang") as CheckBox;

                if ((string.IsNullOrEmpty(detail.Text)) && (string.IsNullOrEmpty(comment.Text)))
                {
                    active.Checked = true;
                }

            }
        }

        #endregion
        #region Button Event
        protected void ctlSearchButton_Click(object sender, EventArgs e)
        {
            ctlRejectReasonGridView.DataCountAndBind();
            divButton.Visible = true;
        }
        protected void ctlSubmit_Click(object sender, ImageClickEventArgs e)
        {
            IList<DbScgReasonLang> list = new List<DbScgReasonLang>();
            short reasonId = UIHelper.ParseShort(ctlRejectReasonGridView.SelectedValue.ToString());
            DbScgReason reason = new DbScgReason(reasonId);

            foreach (GridViewRow row in ctlReasonLangGrid.Rows)
            {
                TextBox detail = row.FindControl("ctlReasonDetail") as TextBox;
                TextBox comment = row.FindControl("ctlCommentLang") as TextBox;
                CheckBox active = row.FindControl("ctlActiveLang") as CheckBox;

                if (!string.IsNullOrEmpty(detail.Text))
                {

                    short languageId = UIHelper.ParseShort(ctlReasonLangGrid.DataKeys[row.RowIndex].Values["LanguageID"].ToString());
                    DbLanguage lang = new DbLanguage(languageId);

                    DbScgReasonLang reasonLang = new DbScgReasonLang();
                    reasonLang.Reason = reason;
                    reasonLang.Language = lang;
                    reasonLang.ReasonDetail = detail.Text;
                    reasonLang.Comment = comment.Text;
                    reasonLang.Active = active.Checked;

                    GetReasonLangInfo(reasonLang);
                    list.Add(reasonLang);
                }

            }
            DbReasonLangService.UpdateReasonLang(list);
            ctlMessage.Message = GetMessage("SaveSuccessFully");
            ctlRejectReasonGridView.DataCountAndBind();
            ctlUpdatePanelGridView.Update();
        }
        protected void ctlDetailCancel_Click(object sender, ImageClickEventArgs e)
        {
            ReasonLangGridViewFinish();
        }
        protected void ctlAddNew_Click(object sender, ImageClickEventArgs e)
        {
            ctlReasonModalPopupExtender.Show();
            ctlRejectReasonGridView.DataCountAndBind();
            ctlUpdatePanelReasonForm.Update();
            ctlReasonFormView.ChangeMode(FormViewMode.Insert);
        }
        protected void ctlDelete_Click(object sender, ImageClickEventArgs e)
        {
            foreach (GridViewRow row in ctlRejectReasonGridView.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
                {
                    try
                    {
                        short id = UIHelper.ParseShort(ctlRejectReasonGridView.DataKeys[row.RowIndex]["ReasonID"].ToString());
                        DbScgReason reason = DbReasonService.FindByIdentity(id);
                        DbReasonService.Delete(reason);
                    }
                    catch (Exception ex)
                    {
                        if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                                "alert('This data is now in use.');", true);
                        }
                        ctlRejectReasonGridView.DataCountAndBind();
                    }
                }
            }

            ReasonLangGridViewFinish();
            ctlRejectReasonGridView.DataCountAndBind();
            ctlUpdatePanelGridView.Update();
            ctlRejectReasonGridView.DataBind();
        }

        #endregion

        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            string documentType = ctlDocumentTypeList.SelectedValue;
            return ScgDbQueryProvider.DbReasonQuery.GetReasonList(documentType, UserAccount.CurrentLanguageID, startRow, pageSize, sortExpression);
        }
        public int RequestCount()
        {
            string documentType = ctlDocumentTypeList.SelectedValue;
            return ScgDbQueryProvider.DbReasonQuery.GetReasonCount(documentType, UserAccount.CurrentLanguageID); ;
        }
        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBox(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlRejectReasonGridView.ClientID + "', '" + ctlRejectReasonGridView.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "validateChkBox", script.ToString(), true);
        }
        public void ClosePopUp()
        {
            ctlReasonFormView.DataSource = null;
            ctlReasonModalPopupExtender.Hide();
            ctlUpdatePanelGridView.Update();
        }
        public void ReasonLangGridViewFinish()
        {
            ctlRejectReasonGridView.SelectedIndex = -1;
            ctlReasonLangGrid.DataSource = null;
            ctlReasonLangGrid.DataBind();
            ctlUpdatePanelGridViewLang.Update();
            ctlSubmit.Visible = false;
            ctlDetailCancel.Visible = false;
            ctlFieldSetDetailGridView.Visible = false;
        }
        private void GetReasonInfo(DbScgReason reason)
        {
            DropDownList documentType = ctlReasonFormView.FindControl("ctlDocumentTypeList") as DropDownList;
            TextBox reasonCode = ctlReasonFormView.FindControl("ctlReasonCode") as TextBox;
            TextBox comment = ctlReasonFormView.FindControl("ctlComment") as TextBox;
            CheckBox active = ctlReasonFormView.FindControl("ctlActive") as CheckBox;

            reason.ReasonCode = reasonCode.Text;
            reason.DocumentTypeID = UIHelper.ParseInt(documentType.SelectedValue);
            reason.Comment = comment.Text;
            reason.Active = active.Checked;

            reason.CreBy = UserAccount.UserID;
            reason.CreDate = DateTime.Now.Date;
            reason.UpdBy = UserAccount.UserID;
            reason.UpdDate = DateTime.Now.Date;
            reason.UpdPgm = ProgramCode;
        }
        private void GetReasonLangInfo(DbScgReasonLang reasonLang)
        {
            reasonLang.CreBy = UserAccount.UserID;
            reasonLang.CreDate = DateTime.Now.Date;
            reasonLang.UpdBy = UserAccount.UserID;
            reasonLang.UpdDate = DateTime.Now.Date;
            reasonLang.UpdPgm = ProgramCode;
        }
    }
}

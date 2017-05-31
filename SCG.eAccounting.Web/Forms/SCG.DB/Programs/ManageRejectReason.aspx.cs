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
using SS.Standard.WorkFlow.Query;
using SCG.eAccounting.Query;
using SCG.eAccounting.Web.UserControls;

namespace SCG.eAccounting.Web.Forms.SCG.DB.Programs
{
    public partial class ManageRejectReason : BasePage
    {
        public IRejectReasonService RejectReasonService { get; set; }
        public IRejectReasonLangService RejectReasonLangService { get; set; }

        private void RefreshGridData(object sender, EventArgs e)
        {
            ctlDbRejectReasonEditor.Hide();
            ctlRejectReasonGridView.DataCountAndBind();
            ctlUpdatePanelGridView.Update();
        }

 
        protected void Page_Load(object sender, EventArgs e)
        {
            ctlDbRejectReasonEditor.Notify_Ok += new EventHandler(RefreshGridData);
            ctlDbRejectReasonEditor.Notify_Cancle += new EventHandler(RefreshGridData);

            if (!Page.IsPostBack)
            {
                this.BindDocumentTypeDropdown(ctlRequestTypeDropdown);
                this.BindStateEventIDDropdown(ctlStateEventIDDropdown, ctlRequestTypeDropdown.SelectedValue);
                ctlUpdatePanelGridView.Update();
            }
        }

        

        #region ReasonGridView
        protected void ctlRejectReasonGridView_DataBound(object sender, EventArgs e)
        {
            if(ctlRejectReasonGridView.Rows.Count > 0)
            {
                divButton.Visible = true;
            }
        }
        protected void ctlRejectReasonGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int reasonId;
            if (e.CommandName.Equals("ReasonEdit"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                reasonId = UIHelper.ParseShort(ctlRejectReasonGridView.DataKeys[rowIndex].Value.ToString());
                ctlRejectReasonGridView.EditIndex = rowIndex;

                //ctlReasonFormView.PageIndex = (ctlRejectReasonGridView.PageIndex * ctlRejectReasonGridView.PageSize) + rowIndex;
                //ctlReasonFormView.ChangeMode(FormViewMode.Edit);
                //IList<DbRejectReason> list = new List<DbRejectReason>();
                //list.Add(ScgDbQueryProvider.RejectReasonQuery.FindByIdentity(reasonId));
                //ctlReasonFormView.DataSource = list;
                //ctlReasonFormView.DataBind();
                //GridView ctlReasonLangGrid = ctlReasonFormView.FindControl("ctlReasonLangGrid") as GridView;
                //ctlReasonLangGrid.DataSource = ScgDbQueryProvider.RejectReasonLangQuery.FindReasonLangByReasonId(reasonId);
                //ctlReasonLangGrid.DataBind();

                ctlRejectReasonGridView.DataCountAndBind();

                ctlDbRejectReasonEditor.Initialize(FlagEnum.EditFlag,reasonId);
                ctlUpdatePanelGridView.Update();
                
            }

            if (e.CommandName.Equals("ReasonDelete"))
            {
                try
                {
                    int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                    reasonId = UIHelper.ParseShort(ctlRejectReasonGridView.DataKeys[rowIndex].Value.ToString());
                    DbRejectReason rejectReason = ScgDbQueryProvider.DbRejectReasonQuery.FindByIdentity(reasonId);
                    RejectReasonService.Delete(rejectReason);
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
                ctlRejectReasonGridView.DataCountAndBind();
                ctlUpdatePanelGridView.Update();
            }
        }
        #endregion


        #region Button Event
        protected void ctlSearchButton_Click(object sender, ImageClickEventArgs e)
        {
            ctlRejectReasonGridView.DataCountAndBind();
            divButton.Visible = true;
        }
        //protected void ctlSubmit_Click(object sender, ImageClickEventArgs e)
        //{
        //    IList<DbRejectReasonLang> list = new List<DbRejectReasonLang>();
        //    short reasonId = UIHelper.ParseShort(ctlRejectReasonGridView.SelectedValue.ToString());
        //    DbRejectReason reason = new DbRejectReason(reasonId);

        //    //foreach (GridViewRow row in ctlReasonLangGrid.Rows)
        //    //{
        //    //    TextBox detail = row.FindControl("ctlReasonDetail") as TextBox;
        //    //    TextBox comment = row.FindControl("ctlCommentLang") as TextBox;
        //    //    CheckBox active = row.FindControl("ctlActiveLang") as CheckBox;

        //    //    if (!string.IsNullOrEmpty(detail.Text))
        //    //    {

        //    //        short languageId = UIHelper.ParseShort(ctlReasonLangGrid.DataKeys[row.RowIndex].Values["LanguageID"].ToString());
        //    //        DbLanguage lang = new DbLanguage(languageId);

        //    //        RejectReasonLang reasonLang = new RejectReasonLang();
        //    //        reasonLang.RejectReason = reason;
        //    //        reasonLang.Language = lang;
        //    //        reasonLang.ReasonDetail = detail.Text;
        //    //        reasonLang.Comment = comment.Text;
        //    //        reasonLang.Active = active.Checked;

        //    //        GetRejectReasonLangInfo(reasonLang);
        //    //        list.Add(reasonLang);
        //    //    }

        //    //}
        //    RejectReasonLangService.UpdateRejectReasonLang(list);
        //    ctlMessage.Message = GetMessage("SaveSuccessFully");
        //    ctlRejectReasonGridView.DataCountAndBind();
        //    ctlUpdatePanelGridView.Update();
        //}

        //protected void ctlDelete_Click(object sender, ImageClickEventArgs e)
        //{
        //    foreach (GridViewRow row in ctlRejectReasonGridView.Rows)
        //    {
        //        if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
        //        {
        //            try
        //            {
        //                int id = UIHelper.ParseShort(ctlRejectReasonGridView.DataKeys[row.RowIndex]["ReasonID"].ToString());
        //                DbRejectReason reason = RejectReasonService.FindByIdentity(id);
        //                RejectReasonService.Delete(reason);
        //            }
        //            catch (Exception ex)
        //            {
        //                if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
        //                {
        //                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
        //                        "alert('This data is now in use.');", true);
        //                }
        //                ctlRejectReasonGridView.DataCountAndBind();
        //            }
        //        }
        //    }

        //    ctlRejectReasonGridView.DataCountAndBind();
        //    ctlUpdatePanelGridView.Update();
        //}

        protected void ctlAddNew_Click(object sender, ImageClickEventArgs e)
        {
            ctlDbRejectReasonEditor.Initialize(FlagEnum.NewFlag,0);
            ctlRejectReasonGridView.DataCountAndBind();
            ctlUpdatePanelGridView.Update();

            
        }
        protected void ctlRequestTypeDropdown_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindStateEventIDDropdown(ctlStateEventIDDropdown, ctlRequestTypeDropdown.SelectedValue);
            ctlUpdatePanelGridView.Update();
        }

        #endregion

        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            DbRejectReason dbRejectReason = this.BuildsRejectReason();
            return ScgDbQueryProvider.RejectReasonQuery.GetRejectReasonList(dbRejectReason, UserAccount.CurrentLanguageID, startRow, pageSize, sortExpression);
        }
        public int RequestCount()
        {
            DbRejectReason dbRejectReason = this.BuildsRejectReason();
            return ScgDbQueryProvider.RejectReasonQuery.GetRejectReasonCount(dbRejectReason, UserAccount.CurrentLanguageID); ;
        }
        private DbRejectReason BuildsRejectReason()
        {
            DbRejectReason rejectReason = new DbRejectReason();
            rejectReason.ReasonCode = ctlReasonCodeTxt.Text;
            rejectReason.WorkFlowStateEventID = UIHelper.ParseInt(ctlStateEventIDDropdown.SelectedValue);
            rejectReason.DocumentTypeID = UIHelper.ParseInt(ctlRequestTypeDropdown.SelectedValue);

            return rejectReason;
        }
        public void BindStateEventIDDropdown(DropDownList dropdownList,string documentTypeID)
        {
            dropdownList.DataSource = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindRejectEventAndReason(UserAccount.CurrentLanguageID,UIHelper.ParseInt(documentTypeID));
            dropdownList.DataTextField = "StateEventID";
            dropdownList.DataValueField = "WorkFlowStateEventID";
            dropdownList.DataBind();
            dropdownList.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), "0"));
            
        }
        public void BindDocumentTypeDropdown(DropDownList dropdownList)
        {
            //dropdownList.DataSource = WorkFlowQueryProvider.DocumentTypeQuery.FindAll();//<--เปลี่ยนquery
            //dropdownList.DataTextField = "DocumentTypeName";
            //dropdownList.DataValueField = "DocumentTypeID";
            dropdownList.DataSource = ScgeAccountingQueryProvider.SCGDocumentQuery.FindDocumentTypeCriteria();
            dropdownList.DataTextField = "Symbol";
            dropdownList.DataValueField = "ID";

            dropdownList.DataBind();
            dropdownList.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), "0"));
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using System.Text;
using SS.SU.BLL;
using SS.SU.Helper;
using System.Web.UI.HtmlControls;


using SS.DB.DTO;
using SS.DB.BLL;
using SS.DB.Query;
using SS.Standard.Utilities;

namespace SCG.eAccounting.Web.Forms.SU.Programs
{
    public partial class Status : BasePage
    {
        #region Properties
        public IDbStatusService DbStatusService { get; set; }
        public IDbStatusLangService DbStatusLangService { get; set; }
        #endregion
       
        #region Override Method

        //protected override void OnPreRender(EventArgs e)
        //{
        //    base.OnPreRender(e);

        //    //Use for form that contain FileUpload Control.
        //    HtmlForm form = this.Page.Form;
        //    if ((form != null) && (form.Enctype.Length == 0))
        //    {
        //        form.Enctype = "multipart/form-data";
        //    }

        //    if (!Page.IsPostBack)
        //    {
        //        ctlStatusGrid.DataCountAndBind();
        //        ctlSubmit.Visible = false;
        //    }
        //}

        #endregion

        #region Load Event

        //protected override void OnPreLoad(EventArgs e)
        //{
        //    base.OnPreLoad(e);
        //    ProgramCode = "Status";
        //}
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                divMessage.Style["display"] = "none";
            if (!Page.IsPostBack)
            {
                ctlStatusGrid.DataCountAndBind();
                
            }
        }

        #endregion

        #region Function
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            short languageId = UserAccount.CurrentLanguageID;
            
            return SsDbQueryProvider.DbStatusQuery.GetStatusList(startRow, pageSize, sortExpression);
        }
        public int RequestCount()
        {
            short languageId = UserAccount.CurrentLanguageID;

            return SsDbQueryProvider.DbStatusQuery.GetCountStatusList();
        }
        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBox(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlStatusGrid.ClientID + "', '" + ctlStatusGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "validateChkBox", script.ToString(), true);
        }
        public void StatusLangGridViewFinish()
        {
            ctlStatusGrid.SelectedIndex = -1;
            ctlStatusLangGrid.DataSource = null;
            ctlStatusLangGrid.DataBind();
            ctlUpdatePanelStatusLangGridView.Update();
            ctlSubmit.Visible = false;
            ctlCancel.Visible = false;
            ctlFieldSetDetailGridView.Visible = false;
        }
        public void ClosePopUp()
        {
            ctlStatusModalPopupExtender.Hide();
            ctlUpdatePanelGridView.Update();
        }
        private void GetDbStatusInfo(DbStatus status)
        {
            TextBox ctlGroupStatus = ctlStatusForm.FindControl("ctlGroupStatus") as TextBox;
            TextBox ctlComment = ctlStatusForm.FindControl("ctlComment") as TextBox;
            CheckBox ctlActive = ctlStatusForm.FindControl("ctlActive") as CheckBox;
            TextBox ctlStatus = ctlStatusForm.FindControl("ctlStatus") as TextBox;

            status.GroupStatus = ctlGroupStatus.Text;
            status.Comment = ctlComment.Text;
            status.Active = ctlActive.Checked;
            status.Status = ctlStatus.Text;

            status.CreBy = UserAccount.UserID;
            status.CreDate = DateTime.Now.Date;
            status.UpdBy = UserAccount.UserID;
            status.UpdDate = DateTime.Now.Date;
            status.UpdPgm = ProgramCode;
        }
        #endregion

        #region Status Gridview
        protected void ctlStatusGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            short statusId;
            if (e.CommandName == "StatusEdit")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                statusId = Convert.ToInt16(ctlStatusGrid.DataKeys[rowIndex].Value.ToString());
                ctlStatusForm.PageIndex = (ctlStatusGrid.PageIndex * ctlStatusGrid.PageSize) + rowIndex;
                ctlStatusForm.ChangeMode(FormViewMode.Edit);
                IList<DbStatus> list = new List<DbStatus>();
                list.Add(DbStatusService.FindByIdentity(statusId));
                ctlStatusForm.DataSource = list;
                ctlStatusForm.DataBind();
                ctlUpdatePanelStatusForm.Update();
                ctlStatusGrid.DataCountAndBind();
                ctlStatusModalPopupExtender.Show();

            }
            if (e.CommandName == "Select")
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                statusId = UIHelper.ParseShort(ctlStatusGrid.DataKeys[rowIndex].Value.ToString());
                ctlStatusLangGrid.DataSource = SsDbQueryProvider.DbStatusLangQuery.FindStatusLangByStatusId(statusId);
                ctlStatusLangGrid.DataBind();
                ctlSubmit.Visible = true;
                ctlFieldSetDetailGridView.Visible = true;
                ctlCancel.Visible = true;
                ctlStatusGrid.DataCountAndBind();
                ctlUpdatePanelStatusLangGridView.Update();
            }
        }
        protected void ctlStatusGrid_DataBound(object sender, EventArgs e)
        {
            if (ctlStatusGrid.Rows.Count > 0)
            {
                RegisterScriptForGridView();
                divButton.Visible = true;
                ctlDelete.OnClientClick = String.Format("return ConfirmDelete('{0}')", ctlStatusGrid.ClientID);
            }
            else
            {

                divButton.Visible = false;
            }
        }
        protected void ctlStatusGrid_PageIndexChanged(object sender, EventArgs e)
        {
            StatusLangGridViewFinish();
        }
        protected void ctlStatusLangGrid_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow row in ctlStatusLangGrid.Rows)
            {
                TextBox ctlStatusDesc = row.FindControl("ctlStatusDesc") as TextBox;
                TextBox comment = row.FindControl("ctlCommentStatusLang") as TextBox;
                CheckBox active = row.FindControl("ctlActiveStatusLang") as CheckBox;

                if ((string.IsNullOrEmpty(ctlStatusDesc.Text)) && (string.IsNullOrEmpty(comment.Text)))
                {

                    active.Checked = true;
                }
            }
        }
        #endregion

        #region Button Event
        protected void ctlAddNew_Click(object sender, ImageClickEventArgs e)
        {
            ctlStatusModalPopupExtender.Show();
            ctlUpdatePanelStatusForm.Update();
            ctlStatusGrid.DataCountAndBind();
            ctlStatusForm.ChangeMode(FormViewMode.Insert);
        }
        protected void ctlDelete_Click(object sender, ImageClickEventArgs e)
        {
            foreach (GridViewRow row in ctlStatusGrid.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
                {
                    try
                    {
                        short id = UIHelper.ParseShort(ctlStatusGrid.DataKeys[row.RowIndex].Value.ToString());
                        DbStatus status = DbStatusService.FindByIdentity(id);
                        DbStatusService.Delete(status);
                    }
                    catch (Exception ex)
                    {
                        if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                                "alert('This data is now in use.');", true);
                            ctlStatusGrid.DataCountAndBind();
                        }
                    }
                }
            }
            StatusLangGridViewFinish();
            ctlStatusGrid.DataCountAndBind();
            ctlUpdatePanelGridView.Update();
        }
        protected void ctlSubmit_Click(object sender, ImageClickEventArgs e)
        {
            IList<DbStatusLang> list = new List<DbStatusLang>();
            short statusId = UIHelper.ParseShort(ctlStatusGrid.SelectedValue.ToString());
            DbStatus status = new DbStatus(statusId);

            foreach (GridViewRow row in ctlStatusLangGrid.Rows)
            {
                TextBox ctlStatusDesc = row.FindControl("ctlStatusDesc") as TextBox;
                TextBox comment = row.FindControl("ctlCommentStatusLang") as TextBox;
                CheckBox active = row.FindControl("ctlActiveStatusLang") as CheckBox;

                if ((!string.IsNullOrEmpty(ctlStatusDesc.Text)) || (!string.IsNullOrEmpty(comment.Text)))
                {

                    short languageId = UIHelper.ParseShort(ctlStatusLangGrid.DataKeys[row.RowIndex].Values["LanguageId"].ToString());
                    DbLanguage lang = new DbLanguage(languageId);

                    DbStatusLang statusLang = new DbStatusLang();
                    statusLang.Status = status;
                    statusLang.Language = lang;
                    statusLang.StatusDesc = ctlStatusDesc.Text;
                    statusLang.Comment = comment.Text;
                    statusLang.Active = active.Checked;

                    statusLang.CreBy = UserAccount.UserID;
                    statusLang.CreDate = DateTime.Now.Date;
                    statusLang.UpdBy = UserAccount.UserID;
                    statusLang.UpdDate = DateTime.Now.Date;
                    statusLang.UpdPgm = ProgramCode;
                    list.Add(statusLang);
                }

            }

            DbStatusLangService.UpdateStatusLang(list);
            divMessage.Style["display"] = "block";
            AlertMessage1.Symbol = "SaveSuccessFully";
            AlertMessage1.MsgType = MessageTypeEnum.AlertMessage.Information;
            AlertMessage1.Show();
            ctlUpdatePanelStatusLangGridView.Update();
        }
        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            StatusLangGridViewFinish();
        }
        #endregion

        #region Status Formview
        protected void ctlStatusForm_DataBound(object sender, EventArgs e)
        {

        }
        protected void ctlStatusForm_ModeChanging(object sender, FormViewModeEventArgs e)
        {

        }
        protected void ctlStatusForm_Inserting(object sender, FormViewInsertEventArgs e)
        {
            DbStatus status = new DbStatus();
            GetDbStatusInfo(status);

            try
            {
                DbStatusService.AddStatus(status);
                ctlStatusGrid.DataCountAndBind();
                ctlStatusForm.ChangeMode(FormViewMode.ReadOnly);
                ClosePopUp();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        protected void ctlStatusForm_Updating(object sender, FormViewUpdateEventArgs e)
        {
            short statusId = UIHelper.ParseShort(ctlStatusForm.DataKey.Value.ToString());
            DbStatus status = new DbStatus(statusId);
            GetDbStatusInfo(status);

            try
            {
                DbStatusService.UpdateStatus(status);
                ctlStatusGrid.DataCountAndBind();
                ClosePopUp();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }

        }
        protected void ctlStatusForm_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                ctlStatusGrid.DataCountAndBind();
                ClosePopUp();
            }
        }
        #endregion
    }
}

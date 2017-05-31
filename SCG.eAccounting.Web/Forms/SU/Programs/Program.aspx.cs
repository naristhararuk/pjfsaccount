using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SS.SU.Query;
using SCG.eAccounting.Web.Helper;
using SS.SU.DTO;
using SS.SU.BLL;
using System.Text;
using System.Globalization;

using SS.DB.DTO;
using SS.DB.BLL;
using SS.DB.Query;
using SS.Standard.Utilities;

namespace SCG.eAccounting.Web.Forms.SU.Programs
{
    public partial class Program : BasePage
    {
        #region Properties
        public ISuProgramService SuProgramService { get; set; }
        public ISuProgramLangService SuProgramLangService { get; set; }
        #endregion

        #region EventLoad
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            //ProgramCode = "Program";
           
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Page.IsPostBack)
            //{
            //    ctlProgramGrid.DataCountAndBind();
            //}
        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            //ctlDelete.Enabled = this.CanDelete;
            //ctlDelete.Visible = this.ctlDelete.Enabled;

            //ctlAddNew.Enabled = this.CanAdd;
            //ctlAddNew.Visible = this.ctlAddNew.Enabled;

            //ctlDelete.Enabled = this.CanDelete;
            //ctlDelete.Visible = this.ctlDelete.Enabled;
        }
        
        #endregion

        #region ProgramGridviewEvent
        protected void ctlProgramGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UserEdit")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                short programId = Convert.ToInt16(ctlProgramGrid.DataKeys[rowIndex].Value);
                ctlProgramForm.PageIndex = (ctlProgramGrid.PageIndex * ctlProgramGrid.PageSize) + rowIndex;
                ctlProgramForm.ChangeMode(FormViewMode.Edit);
                IList<SuProgram> programList = new List<SuProgram>();
                SuProgram program = SuProgramService.FindByIdentity(programId);
                programList.Add(program);

                ctlProgramForm.DataSource = programList;
                ctlProgramForm.DataBind();
                ctlProgramGrid.DataCountAndBind();

                UpdatePanelProgramForm.Update();
                ctlProgramModalPopupExtender.Show();
            }
            if (e.CommandName == "Select")
            {
                
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                short programId = UIHelper.ParseShort(ctlProgramGrid.DataKeys[rowIndex].Value.ToString());
                ctlProgramLanguageGrid.DataSource = SuProgramLangService.FindByProgramId(programId);
                ctlProgramLanguageGrid.DataBind();
                if (ctlProgramLanguageGrid.Rows.Count > 0)
                {
                    ctlSubmit.Visible = true;
                    ctlProgramLangFds.Visible = true;
                    ctlCancel.Visible = true;
                }
                else
                {
                    ctlSubmit.Visible = false;
                    ctlProgramLangFds.Visible = false;
                    ctlCancel.Visible = false ;
                }
                ctlProgramGrid.DataCountAndBind();
                ctlProgramLanguageUpdatePanel.Update();
            }
        }
        protected void ctlProgramGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
        }
        protected void ctlProgramGrid_DataBound(object sender, EventArgs e)
        {
            if (ctlProgramGrid.Rows.Count > 0)
            {
                RegisterScriptForGridView();
                divButton.Visible = true;
                ctlDelete.OnClientClick = String.Format("return ConfirmDelete('{0}')", ctlProgramGrid.ClientID);
            }
            else
            {
                divButton.Visible = false;
            }
        }
        protected void ctlProgramGrid_PageIndexChanged(object sender, EventArgs e)
        {
            ProgramLangGridViewFinish();
        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            return QueryProvider.SuProgramQuery.GetProgramList(new SuProgram(), startRow, pageSize, sortExpression);
        }
        public int RequestCount()
        {
            int count = QueryProvider.SuProgramQuery.CountBySuProgramCriteria(new SuProgram());

            return count;
        }
        #endregion

        #region ProgramLanguage Gridview
        protected void ctlProgramLangGrid_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow row in ctlProgramLanguageGrid.Rows)
            {
                TextBox ctlProgramName = (TextBox)ctlProgramLanguageGrid.Rows[row.RowIndex].FindControl("ctlProgramName");
                TextBox ctlComment = (TextBox)ctlProgramLanguageGrid.Rows[row.RowIndex].FindControl("ctlComment");
                CheckBox ctlActive = (CheckBox)ctlProgramLanguageGrid.Rows[row.RowIndex].FindControl("ctlActive");
                if (string.IsNullOrEmpty(ctlProgramName.Text) && string.IsNullOrEmpty(ctlComment.Text))
                {
                    ctlActive.Checked = true;
                }
            }
        }
        #endregion

        #region ButtonEvent
        protected void ctlAddNew_Click(object sender, ImageClickEventArgs e)
        {
            ctlProgramModalPopupExtender.Show();
            ctlProgramGrid.DataCountAndBind();
            ctlProgramForm.ChangeMode(FormViewMode.Insert);
            UpdatePanelProgramForm.Update();
            
        }
        protected void ctlDelete_Click(object sender, ImageClickEventArgs e)
        {
            foreach (GridViewRow row in ctlProgramGrid.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelectChk")).Checked))
                {
                    try
                    {
                        short programId = Convert.ToInt16(ctlProgramGrid.DataKeys[row.RowIndex].Value);
                        SuProgram program = SuProgramService.FindByIdentity(programId);
                        SuProgramService.Delete(program);
                    }
                    catch (Exception ex)
                    {
                        if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                                "alert('This data is now in use.');", true);
                            ctlProgramGrid.DataCountAndBind();
                            ctlUpdatePanelGridview.Update();
                        }
                    }
                }
            }
            ProgramLangGridViewFinish();
            ctlProgramGrid.DataCountAndBind();
            ctlUpdatePanelGridview.Update();
        }
        protected void ctlSubmit_Click(object sender, EventArgs e)
        {
            IList<SuProgramLang> programLangList = new List<SuProgramLang>();

            SuProgram program = new SuProgram(UIHelper.ParseShort(ctlProgramGrid.SelectedValue.ToString()));
            foreach (GridViewRow row in ctlProgramLanguageGrid.Rows)
            {
                TextBox ctlProgramName = (TextBox)ctlProgramLanguageGrid.Rows[row.RowIndex].FindControl("ctlProgramName");
                TextBox ctlComment = (TextBox)ctlProgramLanguageGrid.Rows[row.RowIndex].FindControl("ctlComment");
                CheckBox ctlActiveChk = (CheckBox)ctlProgramLanguageGrid.Rows[row.RowIndex].FindControl("ctlActive");
                if (!string.IsNullOrEmpty(ctlProgramName.Text) || !string.IsNullOrEmpty(ctlComment.Text))
                {
                    SuProgramLang programLang = new SuProgramLang();
                    DbLanguage Lang = new DbLanguage(UIHelper.ParseShort(ctlProgramLanguageGrid.DataKeys[row.RowIndex].Value.ToString()));
                    programLang.Language = Lang;
                    programLang.Program = program;
                    programLang.ProgramsName = ctlProgramName.Text;
                    programLang.Comment = ctlComment.Text;
                    programLang.Active = ctlActiveChk.Checked;
                    programLang.CreBy = UserAccount.UserID;
                    programLang.CreDate = DateTime.Now;
                    programLang.UpdBy = UserAccount.UserID;
                    programLang.UpdDate = DateTime.Now;
                    programLang.UpdPgm = ProgramCode;
                    programLangList.Add(programLang);
                }
            }
            SuProgramLangService.UpdateProgramLang(programLangList);
            ctlMessage.Message = GetMessage("SaveSuccessFully");
        }
        protected void ctlCancel_Click(object sender, EventArgs e)
        {
            ProgramLangGridViewFinish();
        }
        
        #endregion

        #region ProgramFormviewEvent
        protected void ctlProgramForm_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            e.Cancel = true;
        }
        protected void ctlProgramForm_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                ctlProgramGrid.DataCountAndBind();
                ClosePopUp();
            }
        }
        protected void ctlProgramForm_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            TextBox ctlProgramCode = (TextBox)ctlProgramForm.FindControl("ctlProgramCode");
            TextBox ctlComment = (TextBox)ctlProgramForm.FindControl("ctlComment");
            CheckBox ctlActiveChk = (CheckBox)ctlProgramForm.FindControl("ctlActiveChk");
            
            SuProgram program = new SuProgram();
            program.ProgramCode = ctlProgramCode.Text;
            program.Comment = ctlComment.Text;
            program.Active = ctlActiveChk.Checked;
            program.CreBy = UserAccount.UserID;
            program.CreDate = DateTime.Now;
            program.UpdBy = UserAccount.UserID;
            program.UpdDate = DateTime.Now;
            program.UpdPgm = ProgramCode;
            try
            {
                SuProgramService.AddProgram(program);
                ctlProgramGrid.DataCountAndBind();
                ctlProgramForm.ChangeMode(FormViewMode.ReadOnly);
                ClosePopUp();
            }catch(ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        protected void ctlProgramForm_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            short programId = Convert.ToInt16(ctlProgramForm.DataKey.Value);
            TextBox ctlProgramCode = (TextBox)ctlProgramForm.FindControl("ctlProgramCode");
            TextBox ctlComment = (TextBox)ctlProgramForm.FindControl("ctlComment");
            CheckBox ctlActiveChk = (CheckBox)ctlProgramForm.FindControl("ctlActiveChk");

            SuProgram program = SuProgramService.FindByIdentity(programId);
            program.ProgramCode = ctlProgramCode.Text;
            program.Comment = ctlComment.Text;
            program.Active = ctlActiveChk.Checked;
            program.UpdBy = UserAccount.UserID;
            program.UpdDate = DateTime.Now;
            program.UpdPgm = ProgramCode;
            try
            {
                SuProgramService.UpdateProgram(program);
                ctlProgramGrid.DataCountAndBind();
                ctlProgramForm.ChangeMode(FormViewMode.ReadOnly);
                ClosePopUp();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        protected void ctlProgramForm_DataBound(object sender, EventArgs e)
        {
            if (ctlProgramForm.CurrentMode != FormViewMode.ReadOnly)
            {
                TextBox ctlTxtName = ctlProgramForm.FindControl("ctlProgramCode") as TextBox;
                ctlTxtName.Focus();
            }
        }
        #endregion

        #region Private Function
        public void ClosePopUp()
        {
            ctlProgramModalPopupExtender.Hide();
            ctlUpdatePanelGridview.Update();
        }
        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBox(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlProgramGrid.ClientID + "', '" + ctlProgramGrid.HeaderRow.FindControl("ctlSelectAllChk").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "validateChkBox", script.ToString(), true);
        }
        public void ProgramLangGridViewFinish()
        {

            ctlProgramLanguageGrid.DataSource = null;
            ctlProgramLanguageGrid.DataBind();
            ctlProgramLanguageUpdatePanel.Update();
            ctlSubmit.Visible = false;
            ctlProgramLangFds.Visible = false;
            ctlCancel.Visible = false;
            ctlProgramGrid.SelectedIndex = -1;
        }
        #endregion

        
    }
}

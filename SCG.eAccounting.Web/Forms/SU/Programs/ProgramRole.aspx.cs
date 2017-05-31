using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Text;


using SS.Standard.UI;
using SS.Standard.Security;

using SS.SU.DTO;
using SS.SU.BLL;
using SS.SU.Query;

using SCG.eAccounting.Web.Helper;

using SS.DB.Query;
using SS.DB.DTO;
using SS.DB.BLL;
using SS.Standard.Utilities;

namespace SCG.eAccounting.Web.Forms.SU.Programs
{
    public partial class ProgramRole : BasePage
    {
        #region Define Variable
        List<SuRole> RoleList;
        public ISuRoleService SuRoleService { get; set; }
        public ISuRoleLangService SuRoleLangService { get; set; }
        public ISuProgramRoleService SuProgramRoleService { get; set; }
        public IDbLanguageService DbLanguageService { get; set; }
        #endregion Define Variable

        #region PageLoad



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //ctlRoleGrid.DataCountAndBind();
                //ctlProgramRoleGrid.DataCountAndBind();
                ctlSave.Visible = false;
                divButton.Visible = false;
                ctlFieldSetDetailGridView.Visible = false;
                //ctlUpdatePanelProgramRoleGridView.Update();
            }
            ProgramSearch1.OnObjectLookUpCalling += new BaseUserControl.ObjectLookUpCallingEventHandler(ProgramSearch1_OnObjectLookUpCalling);
            ProgramSearch1.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ProgramSearch1_OnObjectLookUpReturn);
        }
        #endregion

        #region Public Method
        private void RegisterScriptForGridView()
		{
			StringBuilder script = new StringBuilder();
			script.Append("function validateCheckBox(objChk, objFlag) ");
			script.Append("{ ");
			script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
			script.Append("'" + ctlProgramRoleGrid.ClientID + "', '" + ctlProgramRoleGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
			script.Append("} ");

			ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "validateChkBox", script.ToString(), true);
        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            SuRoleLang suRoleLang = new SuRoleLang();
            suRoleLang.Language = DbLanguageService.FindByIdentity(UserAccount.CurrentLanguageID);
            return SuRoleLangService.FindBySuRoleLangCriteria(suRoleLang, startRow, pageSize, sortExpression);
        }
        public Object RequestProgramRoleData(int startRow, int pageSize, string sortExpression)
        {
            object returnValue = null;
            if (ctlRoleGrid.SelectedIndex > -1)
            {
                SuProgramRole suProgramRole = new SuProgramRole();
                suProgramRole.Role = SuRoleService.FindByIdentity(UIHelper.ParseShort(ctlRoleGrid.SelectedDataKey["Roleid"].ToString()));
                returnValue = QueryProvider.SuProgramRoleQuery.FindBySuProgramRole(suProgramRole, UserAccount.CurrentLanguageID, startRow, pageSize, sortExpression);
            }
            return returnValue;
        }
        public int RequestCount()
        {
            SuRoleLang suRoleLang = new SuRoleLang();
            suRoleLang.Language = DbLanguageService.FindByIdentity(UserAccount.CurrentLanguageID);
            int count = SuRoleLangService.CountBySuRoleLangCriteria(suRoleLang);

            return count;
        }
        public int RequestProgramRoleCount()
        {
            int count = 0;
            if (ctlRoleGrid.SelectedIndex > -1)
            {
                SuProgramRole suProgramRole = new SuProgramRole();
                suProgramRole.Role = SuRoleService.FindByIdentity(UIHelper.ParseShort(ctlRoleGrid.SelectedDataKey["Roleid"].ToString()));
                count = QueryProvider.SuProgramRoleQuery.CountBySuProgramRoleCriteria(suProgramRole, UserAccount.CurrentLanguageID);
            }

            return count;
        }
        #endregion

        #region Event
        void ProgramSearch1_OnObjectLookUpCalling(object sender, ObjectLookUpCallingEventArgs e)
        {
            UserControls.LOV.SS.DB.ProgramSearch programSearch = sender as UserControls.LOV.SS.DB.ProgramSearch;
            short roleId = UIHelper.ParseShort(ctlRoleGrid.SelectedValue.ToString());
            short languageId;
            languageId = UserAccount.CurrentLanguageID;
            programSearch.RoleId = roleId.ToString();
            programSearch.LanguageId = languageId.ToString();
        }
        protected void ProgramSearch1_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            IList<SuProgramRole> list = new List<SuProgramRole>();
            IList<SuProgramLang> programLangList = e.ObjectReturn as IList<SuProgramLang>;
            short roleId = UIHelper.ParseShort(ctlRoleGrid.SelectedValue.ToString());

            try
            {
                foreach (SuProgramLang SuProgramLang in programLangList)
                {
                    SuProgramRole suProgramRole = new SuProgramRole();
                    suProgramRole.Role = SuRoleService.FindProxyByIdentity(roleId);
                    suProgramRole.Program = SuProgramLang.Program;
                    suProgramRole.AddState = false;
                    suProgramRole.EditState = false;
                    suProgramRole.DeleteState = false;
                    suProgramRole.DisplayState = false;
                    suProgramRole.Comment = "";
                    suProgramRole.Active = true;
                    suProgramRole.CreBy = UserAccount.UserID;
                    suProgramRole.CreDate = DateTime.Now.Date;
                    suProgramRole.UpdBy = UserAccount.UserID;
                    suProgramRole.UpdDate = DateTime.Now.Date;
                    suProgramRole.UpdPgm = ProgramCode;
                    list.Add(suProgramRole);
                }
                SuProgramRoleService.UpdateProgramRole(list);
                
                ctlProgramRoleGrid.DataCountAndBind();
                ctlUpdatePanelProgramRoleGridView.Update();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        #endregion

        #region Grid
        protected void ctlRoleGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            short roleId;
            short languageId;
            languageId = UserAccount.CurrentLanguageID;
            if (e.CommandName == "Select")
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                roleId = UIHelper.ParseShort(ctlRoleGrid.DataKeys[rowIndex].Value.ToString());
                ctlProgramRoleGrid.DataSource = QueryProvider.SuProgramRoleQuery.FindSuProgramRoleByRoleId(roleId, languageId);
                ctlProgramRoleGrid.DataBind();
                ctlSave.Visible = true;
                ctlUpdatePanelProgramRoleGridView.Update();
                ctlRoleGrid.DataCountAndBind();
                divButton.Visible = true;
                ctlFieldSetDetailGridView.Visible = true;
            }
        }
        protected void ctlProgramRoleGrid_DataBound(object sender, EventArgs e)
        {
            if (ctlProgramRoleGrid.Rows.Count > 0)
            {
                RegisterScriptForGridView();
                ctlDelete.OnClientClick = String.Format("return ConfirmDelete('{0}')", ctlProgramRoleGrid.ClientID);
            }
        }
        #endregion Grid

        #region Button Event
        protected void ctlSave_Click(object sender, ImageClickEventArgs e)
        {
            IList<SuProgramRole> list = new List<SuProgramRole>();
            short roleId = UIHelper.ParseShort(ctlRoleGrid.SelectedValue.ToString());
            //SuProgramRole translate = new SuProgramRole(roleId);
            try
            {
                foreach (GridViewRow row in ctlProgramRoleGrid.Rows)
                {
                    CheckBox addState = row.FindControl("ctlAddState") as CheckBox;
                    CheckBox editState = row.FindControl("ctlEditState") as CheckBox;
                    CheckBox deleteState = row.FindControl("ctlDeleteState") as CheckBox;
                    CheckBox displayState = row.FindControl("ctlDisplayState") as CheckBox;
                    TextBox comment = row.FindControl("ctlCommentProgramRole") as TextBox;
                    CheckBox active = row.FindControl("ctlActiveProgramRole") as CheckBox;

                    short programId = UIHelper.ParseShort(ctlProgramRoleGrid.DataKeys[row.RowIndex].Values["ProgramId"].ToString());
                    SuProgram program = new SuProgram(programId);
                    SuRole role = new SuRole(roleId);

                    SuProgramRole programRole = new SuProgramRole();
                    programRole.Role = role;
                    programRole.Program = program;
                    programRole.AddState = addState.Checked;
                    programRole.EditState = editState.Checked;
                    programRole.DeleteState = deleteState.Checked;
                    programRole.DisplayState = displayState.Checked;
                    programRole.Comment = comment.Text;
                    programRole.Active = active.Checked;
                    programRole.CreBy = UserAccount.UserID;
                    programRole.CreDate = DateTime.Now.Date;
                    programRole.UpdBy = UserAccount.UserID;
                    programRole.UpdDate = DateTime.Now.Date;
                    programRole.UpdPgm = ProgramCode;

                    list.Add(programRole);

                }
                SuProgramRoleService.UpdateProgramRole(list);
                ctlMessage.Message = GetMessage("SaveSuccessFully");
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        protected void ctlDelete_Click(object sender, ImageClickEventArgs e)
        {
            foreach (GridViewRow row in ctlProgramRoleGrid.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
                {
                    try
                    {
                        short roleid = UIHelper.ParseShort(ctlProgramRoleGrid.DataKeys[row.RowIndex].Values["ID"].ToString());
                        SuProgramRole role = SuProgramRoleService.FindByIdentity(roleid);
                        SuProgramRoleService.Delete(role);
                    }
                    catch (Exception ex)
                    {
                        //if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                        //{
                        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                        //        "alert('This data is now in use.');", true);
                        //}
                    }
                }
            }
            ctlProgramRoleGrid.DataCountAndBind();
        }
        protected void ctlAddNew_Click(object sender, ImageClickEventArgs e)
        {
            ctlRoleGrid.DataCountAndBind();
            ProgramSearch1.Show();
        }
        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            ctlProgramRoleGrid.DataSource = null;
            ctlProgramRoleGrid.DataBind();
            ctlUpdatePanelProgramRoleGridView.Update();
            ctlFieldSetDetailGridView.Visible = false;

            ctlRoleGrid.SelectedIndex = -1;
            ctlRoleGrid.DataCountAndBind();
            UpdatePanelGridView.Update();
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using SS.SU.BLL;
using SS.SU.Query;
using SS.SU.DTO;
using SS.Standard.UI;
using SS.Standard.Utilities;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class ProgramInformation : BaseUserControl
    {

        public ISuProgramRoleService SuProgramRoleService { get; set; }
        
        /// <summary>
        /// Current roleID 
        /// </summary>
        public short RoleID
        {
            get { return this.ViewState["RoleID"] == null ? (short)0 : (short)this.ViewState["RoleID"]; }
            set { this.ViewState["RoleID"] = value; }
        }

        public event EventHandler Close_Seek;

        protected void Page_Load(object sender, EventArgs e)
        {
            ctlProgramSearch1.OnObjectLookUpCalling += new BaseUserControl.ObjectLookUpCallingEventHandler(ctlAccountLookup_OnObjectLookUpCalling);
            ctlProgramSearch1.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlAccountLookup_OnObjectLookUpReturn);
        }

        #region This Page Controler
        public void HidePopUp()
        {
            ctlPanelProgramInfo.Visible = false;

        }

        public void ShowPopUp()
        {
            ctlPanelProgramInfo.Visible = true;
            ctlProgramGridView.DataCountAndBind();
            ctlUpdPanelGridView.Update();
        }
        #endregion
      
        #region Script Initialize
        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBoxControl(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlProgramGridView.ClientID + "', '" + ctlProgramGridView.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this, this.Page.GetType(), "validateChkBox", script.ToString(), true);
        }




        protected void Grid_DataBound(object sender, EventArgs e)
        {
            if (ctlProgramGridView.Rows.Count > 0)
            {
                RegisterScriptForGridView();
            }
        }
        #endregion
        IList<SuProgramLang> proGramLangList;
        #region Lookup Event
        protected void ctlAccountLookup_OnObjectLookUpCalling(object sender, ObjectLookUpCallingEventArgs e)
        {
            UserControls.LOV.SS.DB.ProgramSearch lookup = sender as UserControls.LOV.SS.DB.ProgramSearch;

        }
        IList<SuProgramLang> programLangList;
        protected void ctlAccountLookup_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            programLangList = (IList<SuProgramLang>)e.ObjectReturn;

            foreach (SuProgramLang proLang in programLangList)
            {
                SuProgramRole programRole = new SuProgramRole();
                programRole = new SuProgramRole();
                programRole.Active = true;
                programRole.AddState = false;
                programRole.Comment = "";
                programRole.CreBy = UserAccount.UserID;
                programRole.CreDate = DateTime.Now.Date;
                programRole.DeleteState = false;
                programRole.DisplayState = false;
                programRole.EditState = false;
                programRole.Program = new SuProgram();
                programRole.Program.Programid = proLang.ProgramId;
                programRole.Role = new SuRole();
                programRole.Role.RoleID = RoleID;
                programRole.UpdPgm = ProgramCode;
                programRole.UpdDate = DateTime.Now.Date;
                programRole.UpdBy = UserAccount.UserID;
                try
                {
                    SuProgramRoleService.AddProgramRole(programRole);
                }
                catch (ServiceValidationException ex)
                {
                    ValidationErrors.MergeErrors(ex.ValidationErrors);
                }
            }
            ctlProgramGridView.DataCountAndBind();
            RegisterScriptForGridView();
            ctlUpdPanelGridView.Update();

        }
        #endregion

        #region Button Event
        protected void Add_Click(object sender, EventArgs e)
        {
            ctlProgramSearch1.RoleId = RoleID.ToString();
            ctlProgramSearch1.LanguageId = UserAccount.CurrentLanguageID.ToString() ;
            ctlProgramSearch1.Show();
        }
        protected void Delete_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in ctlProgramGridView.Rows)
            {
                //traversal in a row
                if (((CheckBox)(row.Cells[0].Controls[1])).Checked)
                {
                    int rowIndex = row.RowIndex;
                    short programRoleID = short.Parse(ctlProgramGridView.DataKeys[rowIndex].Value.ToString());
                    SuProgramRole programRole = new SuProgramRole();
                    programRole.Id = programRoleID;
                    try
                    {
                        SuProgramRoleService.DeleteProgramRole(programRole);
                    }
                    catch (ServiceValidationException ex)
                    {
                        ValidationErrors.MergeErrors(ex.ValidationErrors);
                    }

                }
            }
            ctlProgramGridView.DataCountAndBind();
            ctlUpdPanelGridView.Update();
        }
        protected void ctlButtonClose_Click(object sender, EventArgs e)
        {
            HidePopUp();
            Close_Seek(sender, e);
        }
         #endregion

        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            SuRole role = new SuRole();
            role.RoleID = RoleID;
            return QueryProvider.SuProgramRoleQuery.FindProgramInfoByRole(role, UserAccount.UserLanguageID,sortExpression);

        }
       
    }
}
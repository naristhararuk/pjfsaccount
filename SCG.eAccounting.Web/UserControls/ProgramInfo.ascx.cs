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
using SS.SU.DTO.ValueObject;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class ProgramInfo : BaseUserControl
    {
        #region Properties
        public ISuProgramRoleService SuProgramRoleService { get; set; }

        public short RoleID
        {
            get { return this.ViewState["RoleID"] == null ? (short)0 : (short)this.ViewState["RoleID"]; }
            set { this.ViewState["RoleID"] = value; }
        }
        #endregion

        #region Event(Useless)
        public event EventHandler Notify_Ok;
        public event EventHandler Notify_Cancel;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            ctlProgramSearch1.OnObjectLookUpCalling += new BaseUserControl.ObjectLookUpCallingEventHandler(ctlAccountLookup_OnObjectLookUpCalling);
            ctlProgramSearch1.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlAccountLookup_OnObjectLookUpReturn);
        }

        #region Page event
        public void Hide()
        {
            ctlPanelProgramInfo.Visible = false;
        }

        private void RefreshGridView()
        {
            ctlProgramGridView.DataCountAndBind();
            ctlUpdPanelGridView.Update();
        }

        public void Show()
        {
            ctlPanelProgramInfo.Visible = true;

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
        IList<ProgramLang> programLangList;
        protected void ctlAccountLookup_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            programLangList = (IList<ProgramLang>)e.ObjectReturn;

            foreach (ProgramLang proLang in programLangList)
            {
                SuProgramRole programRole = new SuProgramRole();
                programRole = new SuProgramRole();
                programRole.Active = true;
                programRole.AddState = false;
                programRole.Comment = "";
                programRole.CreBy = UserAccount.UserID;
                programRole.UpdPgm = ProgramCode;
                programRole.DeleteState = false;
                programRole.DisplayState = false;
                programRole.EditState = false;
                programRole.Program = new SuProgram();
                if (proLang.ProgramId.HasValue)
                    programRole.Program.Programid = proLang.ProgramId.Value;
                programRole.Role = new SuRole();
                programRole.Role.RoleID = RoleID;
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
            RefreshGridView();


        }
        #endregion

        #region Button Event
        protected void Add_Click(object sender, EventArgs e)
        {
            ctlProgramSearch1.RoleId = RoleID.ToString();
            ctlProgramSearch1.LanguageId = UserAccount.CurrentLanguageID.ToString();
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
            RegisterScriptForGridView();
            RefreshGridView();
        }

        protected void ctlButtonClose_Click(object sender, EventArgs e)
        {
            Notify_Ok(sender, e);
            Hide();
        }
        #endregion

        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            SuRole role = new SuRole();
            role.RoleID = RoleID;
            return QueryProvider.SuProgramRoleQuery.FindProgramInfoByRole(role, UserAccount.CurrentLanguageID, sortExpression);

        }

        public void Initialize(short roleID)
        {
            RoleID = roleID;
            RefreshGridView();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;


using SS.Standard.UI;
using SS.Standard.Security;

using SS.SU.DTO;
using SS.SU.BLL;
using SS.SU.Query;

using SCG.eAccounting.Web.Helper;
using SS.SU.DTO.ValueObject;

using SS.DB.DTO;
using SS.DB.BLL;
using SS.DB.Query;
using SS.Standard.Utilities;

namespace SCG.eAccounting.Web.Forms.SU.Programs
{
    public partial class Role : BasePage
    {
        #region Define Variable
        IList<SuRole> RoleList;
        //public string HiddenTag = "style=\"display:none\"";

        public ISuRoleService SuRoleService { get; set; }
        public ISuRoleQuery SuRoleQuery { get; set; }
        public ISuRoleLangService SuRoleLangService { get; set; }
        public IDbLanguageService DbLanguageService { get; set; }

        #endregion Define Variable

        #region Load Event
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            //ProgramCode = "Role";
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
 

        protected void Page_Load(object sender, EventArgs e)
        {
            //BindRoleGrid();
            
            this.CanAdd = true;
            this.CanDelete = true;
            this.CanView = true;
            this.CanEdit = true;
        }
        #endregion Form Event

        

        //#region Datasource
        //protected void SuRoleDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        //{
        //    e.ObjectInstance = QueryProvider.SuRoleQuery;// SuRoleService;
        //}

        //protected void SuRoleDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        //{
        //    SuRole role = e.InputParameters[0] as SuRole;
        //    GetSuRoleInfo(role);
        //}

        //protected void SuRoleDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        //{
        //    SuRole role = e.InputParameters[0] as SuRole;
        //    GetSuRoleInfo(role);
        //}

        //protected void SuRoleDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        //{
        //    //e.InputParameters["languageID"] = UIHelper.ParseShort("1");
        //}
        //#endregion Datasource

        #region Role GridView
        protected void ctlRoleGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "RoleEdit")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                short roleId = UIHelper.ParseShort(ctlRoleGrid.DataKeys[rowIndex].Value.ToString());
                ctlRoleGrid.EditIndex = rowIndex;
                IList<SuRole> roleList = new List<SuRole>();
                SuRole role = SuRoleService.FindByIdentity(roleId);
                roleList.Add(role);

                ctlRoleForm.DataSource = roleList;
                ctlRoleForm.PageIndex = 0;//(ctlRoleGrid.PageIndex * ctlRoleGrid.PageSize) + rowIndex;

                ctlRoleForm.ChangeMode(FormViewMode.Edit);
                ctlRoleForm.DataBind();
                ctlRoleGrid.DataCountAndBind();

                UpdatePanelRoleForm.Update();
                ctlRoleModalPopupExtender.Show();

            }
            if (e.CommandName == "Select")
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                short roleId = UIHelper.ParseShort(ctlRoleGrid.DataKeys[rowIndex].Value.ToString());
                ctlRoleLangGrid.DataSource = SuRoleLangService.FindByRoleId(roleId);
                ctlRoleLangGrid.DataBind();
                if (ctlRoleLangGrid.Rows.Count > 0)
                {
                    ctlSubmit.Visible = true;
                    ctlRoleLangLangFds.Visible = true;
                    ctlCancel.Visible = true;
                }
                else
                {
                    ctlSubmit.Visible = false;
                    ctlRoleLangLangFds.Visible = false;
                    ctlCancel.Visible = false;
                }
                ctlRoleGrid.DataCountAndBind();
                ctlRoleLangUpdatePanel.Update();
            }
        }
        protected void ctlRoleGrid_PageIndexChanged(object sender, EventArgs e)
        {
            RoleLangGridViewFinish();
            
        }

        protected void ctlRoleGrid_DataBound(object sender, EventArgs e)
        {
            if (ctlRoleGrid.Rows.Count > 0)
            { 
                divButton.Visible = true;
                RegisterScriptForGridView();

                ctlDelete.OnClientClick = String.Format("return ConfirmDelete('{0}')", ctlRoleGrid.ClientID);
            }
            else
            {
                divButton.Visible = false;
            }
        }

        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            //return QueryProvider.SuRoleQuery.GetRoleList(new RoleLang(), UserAccount.CurrentLanguageID, startRow, pageSize, sortExpression);
            return null;
        }
        public int RequestCount()
        {
            int count = QueryProvider.SuRoleQuery.CountByRoleCriteria(new SuRole());

            return count;
        }
        #endregion

        #region FormView
        protected void ctlRoleForm_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                ctlRoleGrid.DataCountAndBind();
                ctlRoleModalPopupExtender.Hide();
                UpdatePanelGridView.Update();
            }
        }

        protected void ctlRoleForm_DataBound(object sender, EventArgs e)
        {
            if (ctlRoleForm.CurrentMode == FormViewMode.Insert)
            {
                TextBox ctlRoleName = ctlRoleForm.FindControl("ctlRoleName") as TextBox;
                ctlRoleName.Focus();
            }

            if (ctlRoleForm.CurrentMode.Equals(FormViewMode.Edit))
            {
                Label ctlRoleName = ctlRoleForm.FindControl("ctlRoleName") as Label;
                TextBox ctlComment = ctlRoleForm.FindControl("ctlComment") as TextBox;
                ctlComment.Focus();

                LinkButton ctlRoleNameInGrid = ctlRoleGrid.Rows[ctlRoleGrid.EditIndex].FindControl("ctlRoleName") as LinkButton;

                ctlRoleName.Text = ctlRoleNameInGrid.Text;
            }
        }

        protected void ctlRoleForm_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            SuRole role = new SuRole();
            SuRoleLang roleLang = new SuRoleLang();

            TextBox ctlComment = ctlRoleForm.FindControl("ctlComment") as TextBox;
            CheckBox chkActive = ctlRoleForm.FindControl("chkActive") as CheckBox;
            TextBox ctlRoleName = ctlRoleForm.FindControl("ctlRoleName") as TextBox;

            //role.Comment = ctlComment.Text;
            role.Active = chkActive.Checked;
            role.UpdPgm = ProgramCode;
            role.CreDate = DateTime.Now.Date;
            role.UpdDate = DateTime.Now.Date;
            role.CreBy = UserAccount.UserID;
            role.UpdBy = UserAccount.UserID;

            roleLang.RoleName = ctlRoleName.Text;
            roleLang.Language = DbLanguageService.FindByIdentity(UserAccount.CurrentLanguageID);
            roleLang.Role = role;
            roleLang.Active = role.Active;
            roleLang.CreBy = UserAccount.UserID;
            roleLang.CreDate = DateTime.Now.Date;
            roleLang.UpdPgm = ProgramCode;
            roleLang.UpdBy = UserAccount.UserID;
            roleLang.UpdDate = DateTime.Now.Date;


            try
            {
                // Cancel insert with DataSource.
                //SuRoleService.AddRole(role,roleLang);
                e.Cancel = true;
                ctlRoleGrid.DataCountAndBind();
                ctlRoleModalPopupExtender.Hide();
                UpdatePanelGridView.Update();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            } 
        }
        protected void ctlRoleForm_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            short roleId = UIHelper.ParseShort(ctlRoleForm.DataKey.Value.ToString());
            SuRole role = SuRoleService.FindByIdentity(roleId);
            TextBox ctlComment = ctlRoleForm.FindControl("ctlComment") as TextBox;
            CheckBox chkActive = ctlRoleForm.FindControl("chkActive") as CheckBox;

            //role.Roleid = short.Parse(ctlRoleID.Text);
            //role.Comment = ctlComment.Text;
            role.Active = chkActive.Checked;

            role.UpdPgm = ProgramCode;
            role.UpdDate = DateTime.Now.Date;
            role.CreBy = UserAccount.UserID;
            role.UpdBy = UserAccount.UserID;
            try
            {
                //SuRoleService.UpdateRole(role);

                // Cancel insert with DataSource.
                e.Cancel = true;
                ctlRoleGrid.DataCountAndBind();
                ctlRoleModalPopupExtender.Hide();
                UpdatePanelGridView.Update();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            } 
        }

        protected void ctlRoleForm_ModeChanging(object sender, FormViewModeEventArgs e)
        {

        }
        #endregion FormView

        #region Button Event
        protected void ctlAddNew_Click(object sender, ImageClickEventArgs e)
        {
            ctlRoleForm.DataSource = null;
            ctlRoleForm.ChangeMode(FormViewMode.Insert);
            UpdatePanelRoleForm.Update();
            ctlRoleGrid.DataCountAndBind();
            ctlRoleModalPopupExtender.Show();
        }
        protected void ctlDelete_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in ctlRoleGrid.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && ((CheckBox)row.FindControl("ctlSelect")).Checked)
                {
                    short roleId = UIHelper.ParseShort(ctlRoleGrid.DataKeys[row.RowIndex].Value.ToString());

                    if (!((CheckBox)row.FindControl("chkActive")).Checked)//ไม่ Active
                    {
                        try
                        {
                            SuRole role = SuRoleService.FindProxyByIdentity(roleId);
                            SuRoleService.Delete(role);
                        }
                        catch (Exception ex)
                        {
                            if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                                    "alert('Role ID " + roleId + " is now in use.');", true);
                            }
                            ctlRoleGrid.DataCountAndBind();
                        }
                    }
                    else//Active
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertActiveData",
                                    "alert('Role ID " + roleId + " is now Active.');", true);
                        ctlRoleGrid.DataCountAndBind();
                        UpdatePanelGridView.Update();
                    }
                }
            }
            ctlRoleGrid.DataCountAndBind();
            UpdatePanelGridView.Update();
            RoleLangGridViewFinish();
        }
        protected void ctlSubmit_Click(object sender, EventArgs e)
        {
            IList<SuRoleLang> roleLangList = new List<SuRoleLang>();

            SuRole role = new SuRole(UIHelper.ParseShort(ctlRoleGrid.SelectedValue.ToString()));
            foreach (GridViewRow row in ctlRoleLangGrid.Rows)
            {
                TextBox ctlRoleName = (TextBox)ctlRoleLangGrid.Rows[row.RowIndex].FindControl("ctlRoleName");
                TextBox ctlComment = (TextBox)ctlRoleLangGrid.Rows[row.RowIndex].FindControl("ctlComment");
                CheckBox ctlActiveChk = (CheckBox)ctlRoleLangGrid.Rows[row.RowIndex].FindControl("ctlActive");
                if (!string.IsNullOrEmpty(ctlRoleName.Text) || !string.IsNullOrEmpty(ctlComment.Text))
                {
                    SuRoleLang roleLang = new SuRoleLang();
                    DbLanguage lang = new DbLanguage(UIHelper.ParseShort(ctlRoleLangGrid.DataKeys[row.RowIndex].Value.ToString()));
                    roleLang.Language = lang;
                    roleLang.Role = role;
                    roleLang.RoleName = ctlRoleName.Text;
                    roleLang.Comment = ctlComment.Text;
                    roleLang.Active = ctlActiveChk.Checked;
                    roleLang.CreBy = UserAccount.UserID;
                    roleLang.CreDate = DateTime.Now;
                    roleLang.UpdBy = UserAccount.UserID;
                    roleLang.UpdDate = DateTime.Now;
                    roleLang.UpdPgm = ProgramCode;
                    roleLangList.Add(roleLang);
                }
            }
            SuRoleLangService.UpdateRoleLang(roleLangList);
            ctlMessage.Message = GetMessage("SaveSuccessFully");
        }
        protected void ctlCancel_Click(object sender, EventArgs e)
        {
            RoleLangGridViewFinish();
            
        }
        #endregion

        #region Role Lang Gridview

        protected void ctlRoleLangGrid_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow row in ctlRoleLangGrid.Rows)
            {
                TextBox ctlRoleName = (TextBox)ctlRoleLangGrid.Rows[row.RowIndex].FindControl("ctlRoleName");
                TextBox ctlComment = (TextBox)ctlRoleLangGrid.Rows[row.RowIndex].FindControl("ctlComment");
                CheckBox ctlActive = (CheckBox)ctlRoleLangGrid.Rows[row.RowIndex].FindControl("ctlActive");

                if (string.IsNullOrEmpty(ctlRoleName.Text) && string.IsNullOrEmpty(ctlComment.Text))
                {
                    ctlActive.Checked = true;
                }
            }
        }

        #endregion

        #region Function
        private void SessionUser()
        {
            if (Session["roleList"] == null)
            {
                //RoleList = SuRoleDataSource.Select() as List<SuRole>;
                RoleList = SuRoleQuery.FindAll();

                Session["roleList"] = UIHelper.Serialization<IList<SuRole>>(RoleList);
            }
            else
            {
                RoleList = UIHelper.DeSerialization<IList<SuRole>>(Session["roleList"]);
                Session["roleList"] = UIHelper.Serialization<IList<SuRole>>(RoleList);
            }
        }
        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBox(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlRoleGrid.ClientID + "', '" + ctlRoleGrid.HeaderRow.FindControl("ctlSelectHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "validateChkBox", script.ToString(), true);
        }
        public void RoleLangGridViewFinish()
        {
            ctlRoleLangGrid.DataSource = null;
            ctlRoleLangGrid.DataBind();
            ctlRoleLangUpdatePanel.Update();
            ctlSubmit.Visible = false;
            ctlCancel.Visible = false;
            ctlRoleLangLangFds.Visible = false;
            ctlRoleGrid.SelectedIndex = -1;
        }
        #endregion Methods
        
    }
}

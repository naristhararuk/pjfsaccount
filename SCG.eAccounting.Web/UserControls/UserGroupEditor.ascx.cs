using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using SS.SU.Query;
using System.Text;
using SS.SU.DTO;
using SS.SU.BLL.Implement;
using SS.SU.BLL;
using System.Collections;
using SS.Standard.Utilities;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class UserGroupEditor : BaseUserControl
    {
        public ISuUserRoleService SuUserRoleService { get; set; }
        public ISuUserRoleQuery SuUserleQue { get; set; }
        public long UId
        {
            get { return UIHelper.ParseLong(user.Value); }
            set { user.Value = value.ToString(); }
        }
        public void Initialize(long id)
        {
           UId = id;
            ctlGroupGrid.DataSource=  QueryProvider.SuUserRoleQuery.FindGroupByUserId(UId);
            ctlGroupGrid.DataBind();
            ctlUpdatePanelGroup.Update();
            
            

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ctlGroupLookup.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlGroupLookup_OnObjectLookUpReturn);
        }
        #region group
        protected void ctlAddGroup_Click(object sender, ImageClickEventArgs e)
        {
            ctlGroupLookup.Show(true);
        }
        protected void ctlDeleteGroup_Click(object sender, ImageClickEventArgs e)
        {
            foreach (GridViewRow row in ctlGroupGrid.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
                {
                    try
                    {
                        long userId = UIHelper.ParseLong(ctlGroupGrid.DataKeys[row.RowIndex].Value.ToString());
                        SuUserRole group = QueryProvider.SuUserRoleQuery.FindUserRoleByUserRoleId(userId);
                        SuUserRoleService.DeleteGroup(group);
      
                    }
                    catch (ServiceValidationException ex)
                    {
                        ValidationErrors.MergeErrors(ex.ValidationErrors);
                    }
                    catch (Exception ex)
                    {
                        string exMessage = ex.Message;
                        Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
                        errors.AddError("DeleteGroup.Error", new Spring.Validation.ErrorMessage("CannotDelete"));
                        ValidationErrors.MergeErrors(errors);
                    }
                }
            }
            ctlGroupGrid.DataCountAndBind();
            ctlUpdatePanelGroup.Update();
        }
        protected void ctlCloseGroup_Click(object sender, ImageClickEventArgs e)
        {
            ctlUpdatePanelGroup.Update();
            HideDetail();
        }

        #endregion
        protected void ctlGroupGrid_Databound(object sender, EventArgs e)
        {
            //VOUserProfile criteria = GetSuUserCriteria();
            if (ctlGroupGrid.Rows.Count > 0)
            {
                RegisterScriptForGridView();
            }

        }
        public SuUser GetUser()
        {
            return QueryProvider.SuUserQuery.FindByIdentity(UId);
        }
        protected Object RequestDataGroup(int startRow, int pageSize, string sortExpression)
        {
            return QueryProvider.SuUserRoleQuery.FindGroupByUserId(UId);
        }

        //protected int RequestCount()
        //{
            
            //return QueryProvider.SuUserRoleQuery.
            //return QueryProvider.SuUserRoleQuery.GetCountSuUserRoleSearchResult(UId);
            
        //}

        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBoxControl(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlGroupGrid.ClientID + "', '" + ctlGroupGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "validateChkBox", script.ToString(), true);
        }
        //protected void ctlGroupLookup_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        //{
        //    IList<SuRole> groupList = (List<SuRole>)e.ObjectReturn;

        //    ////Add Approver to SuUserFavorite
        //    ////.....

        //    SuUser user = GetUser();
        //    foreach (SuRole ur in groupList)
        //    {
        //        SuUserRole UserRole = new SuUserRole();
        //        UserRole.Active = true;
        //        UserRole.CreBy = UserAccount.UserID;
        //        UserRole.CreDate = DateTime.Now;
        //        UserRole.Role = ur;
        //        UserRole.UpdBy = UserAccount.UserID;
        //        UserRole.UpdDate = DateTime.Now;
        //        UserRole.UpdPgm = UserAccount.CurrentProgramCode;
        //        UserRole.User = user;

        //        SuUserRoleService.AddFavoriteGroup(UserRole);

        //    }
        //    ctlGroupGrid.DataCountAndBind();
        //    ctlUpdatePanelGroup.Update();

        //}
        protected void ctlGroupLookup_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            IList<SuRole> groupList = (List<SuRole>)e.ObjectReturn;
            ArrayList groupArrList = new ArrayList();
            ////Add Approver to SuUserFavorite
            ////.....

            foreach (GridViewRow row in ctlGroupGrid.Rows)
            {
                Label ctlGroup = (Label)ctlGroupGrid.Rows[row.RowIndex].FindControl("ctlGroup");

                groupArrList.Add(ctlGroup.Text);
            }
            SuUser user = GetUser();
            foreach (SuRole ur in groupList)
            {
                if (!groupArrList.Contains(ur.RoleName))
                {
                    SuUserRole UserRole = new SuUserRole();
                    UserRole.Active = true;
                    UserRole.CreBy = UserAccount.UserID;
                    UserRole.CreDate = DateTime.Now;
                    UserRole.Role = ur;
                    UserRole.UpdBy = UserAccount.UserID;
                    UserRole.UpdDate = DateTime.Now;
                    UserRole.UpdPgm = UserAccount.CurrentProgramCode;
                    UserRole.User = user;

                    SuUserRoleService.AddFavoriteGroup(UserRole);
                }
                
            }
            ctlGroupGrid.DataCountAndBind();
            ctlUpdatePanelGroup.Update();

        }

        public void ShowDetail()
        {
            ctlFieldsetGroup.Visible = true;
            ctlGroupGrid.DataCountAndBind();
            //ctlFieldsetGroup.Style["display"] = "block";
            ctlUpdatePanelGroup.Update();
        }
        public void HideDetail()
        {
            ctlFieldsetGroup.Visible = false;

        }
        private void CheckAddGroup()
        { 
        
        }
    }
}
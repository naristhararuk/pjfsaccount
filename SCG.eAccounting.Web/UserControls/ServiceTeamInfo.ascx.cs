using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using SS.Standard.UI;
using SS.SU.Query;
using SS.SU.DTO;
using SCG.DB.DTO;
using SS.Standard.Utilities;
using SS.SU.BLL;

namespace SCG.eAccounting.Web.UserControls.DropdownList
{
    public partial class ServiceTeamInfo : BaseUserControl
    {
        public short RoleID
        {
            get { return this.ViewState["RoleID"] == null ? (short)0 : (short)this.ViewState["RoleID"]; }
            set { this.ViewState["RoleID"] = value; }
        }

        public ISuRoleServiceService SuRoleServiceService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (!Page.IsPostBack)
            {
                ctlServiceTeam1.ServiceTeamDataBind(RoleID);
                ctlService.DataCountAndBind();
                ProgramCode = "ServiceTeamInfo";
            }
        }

        public event EventHandler Notify_Ok;
        //public event EventHandler Notify_Cancle;

        #region Button Event
        protected void ctlButtonClose_Click(object sender, EventArgs e)
        {
            Notify_Ok(sender, e);
            Hide();
 
        }

        protected void Add_Click(object sender, EventArgs e)
        {
            SuRoleService suRoleService = new SuRoleService();
            suRoleService.RoleID = new SuRole();
            suRoleService.ServiceTeamID = new DbServiceTeam();
            suRoleService.ServiceTeamID.ServiceTeamID = Convert.ToInt64(ctlServiceTeam1.SelectedValue);
            suRoleService.RoleID.RoleID = RoleID;
            suRoleService.Active = true;
            suRoleService.CreBy = UserAccount.UserID;
            suRoleService.UpdBy = UserAccount.UserID;
            suRoleService.UpdPgm = ProgramCode;
            try
            {
                SuRoleServiceService.AddRoleService(suRoleService);

            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
            finally
            {
                RefreshGridData();
            }
            ctlServiceTeam1.ServiceTeamDataBind(RoleID);
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in ctlService.Rows)
            {
                //traversal in a row
                if (((CheckBox)(row.Cells[0].Controls[1])).Checked)
                {
                    int rowIndex = row.RowIndex;
                    short roleServiceID = short.Parse(ctlService.DataKeys[rowIndex].Value.ToString());
                    SuRoleService roleService = new SuRoleService();
                    roleService.RoleServiceID = roleServiceID;
                    try
                    {
                        SuRoleServiceService.DeleteRoleService(roleService);
                    }
                    catch (ServiceValidationException ex)
                    {
                        ValidationErrors.MergeErrors(ex.ValidationErrors);
                    }

                }
            }
            RefreshGridData();
            ctlServiceTeam1.ServiceTeamDataBind(RoleID);
        }
        #endregion

        #region Control Event
        public void Show()
        {
            ctlPanelPBInfo.Visible = true;
            RefreshGridData();
            ctlServiceTeam1.ServiceTeamDataBind(RoleID);
        }

        public void Hide()
        {
            ctlPanelPBInfo.Visible = false;

        }
        
        public void Initialize(short roleID)
        {
            RoleID = roleID;
        }

        private void RefreshGridData()
        {
            ctlService.DataCountAndBind();
            ctlUpdPanelGridView.Update();
        }
        #endregion


        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {

            return QueryProvider.SuRoleServiceQuery.GetServiceTeamInformation(RoleID, sortExpression);
        }

        #region Script Initialize
        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBoxControl(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlService.ClientID + "', '" + ctlService.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "validateChkBox", script.ToString(), true);
        }

        protected void Grid_DataBound(object sender, EventArgs e)
        {
            if (ctlService.Rows.Count > 0)
            {
                RegisterScriptForGridView();
            }
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


//SS.Standard using section.
using SS.Standard.Utilities;
using SS.Standard.UI;
using SS.Standard.Security;

//Related library using section.
using SS.SU.Query;
using SS.SU.DTO.ValueObject;
using SS.SU.BLL;
using SS.SU.DTO;
using SCG.eAccounting.Web.UserControls;
using System.Drawing;


namespace SCG.eAccounting.Web.Forms.SU.Programs
{
    public partial class ManageUserRole : BasePage
    {

        public ISuRoleService SuRoleService { get; set; }

        private void RefreshGridData(object sender,EventArgs e){
            // แก้ไขการกด link ในกริด ที่จะเป็น Editor มาในหน้าเดียวกัน ไม่ต้องทำการ CountAndBind จะทำให้ไม่เกิด Event ในการกด Link ครั้งต่อไป
            ctlGridRole.DataCountAndBind();
            //CtlUserRoleEditor1.HidePopUp();
            ctlUpdPanelGridView.Update();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CtlUserRoleEditor1.Notify_Ok += new EventHandler(RefreshGridData);
            ctlProgramInfo1.Notify_Ok += new EventHandler(RefreshGridData);
            ctlPBInfo1.Notify_Ok += new EventHandler(RefreshGridData);
            ctlServiceTeamInfo1.Notify_Ok += new EventHandler(RefreshGridData);

            if (!Page.IsPostBack)
            {
                ctlGridRole.DataCountAndBind();
                ctlUpdPanelGridView.Update();
            }
        }

        #region BaseGridView Data Providing
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            //role lang for criteria.
            SuRole role = new SuRole();
            role.RoleCode = ctlTxtGroupCode.Text;
            role.RoleName = ctlTxtGroupName.Text;
            return QueryProvider.SuRoleQuery.GetRoleList(role,
                UserAccount.CurrentLanguageID,startRow, pageSize, sortExpression);
        }
      

        public int RequestCount()
        {
            SuRole role = new SuRole();
            
            role.RoleCode = ctlTxtGroupCode.Text;
            role.RoleName = ctlTxtGroupName.Text;
            int count = QueryProvider.SuRoleQuery.CountByRoleCriteria(role);
            return count;
        }
        #endregion

        #region Button Event
        protected void ctlSearch_Click(object sender, EventArgs e)
        {

            ctlGridRole.PageIndex = 1;
            ctlGridRole.DataCountAndBind();
            ctlUpdPanelGridView.Update();

        }


        protected void ctlBtnAdd_Click1(object sender, EventArgs e)
        {

            CtlUserRoleEditor1.Initialize(FlagEnum.NewFlag, 0);
            CtlUserRoleEditor1.ShowPopUp();

        }
        #endregion

        private void InvisibleAllPopUpcontrol()
        {
            ctlServiceTeamInfo1.Hide();
            ctlPBInfo1.Hide();
            ctlProgramInfo1.Hide();
            // แก้ไขการกด link ในกริด ที่จะเป็น Editor มาในหน้าเดียวกัน ไม่ต้องทำการ CountAndBind จะทำให้ไม่เกิด Event ในการกด Link ครั้งต่อไป
            //ctlGridRole.DataCountAndBind();
            ctlUpdPanelGridView.Update();
        }

        protected void ctlGridRole_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            InvisibleAllPopUpcontrol();
            int rowIndex = 0;
   
            if (e.CommandName == "RoleDelete")
            {

                try
                {
                    rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                    short roleID = short.Parse(ctlGridRole.DataKeys[rowIndex].Value.ToString());
                    SuRole role = SuRoleService.FindByIdentity(roleID);
                    SuRoleService.DeleteRole(role);
                }
                catch (ServiceValidationException ex)
                {
                    ValidationErrors.MergeErrors(ex.ValidationErrors);
                }
                catch (Exception ex)
                {
                
                    if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertinusedata",
                            "alert('this data is now in use.');", true);
                    }
                }
                
                ctlGridRole.DataCountAndBind();

            }
            if (e.CommandName == "RoleEdit")
            {
                //find role id for current setup.

                rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                short roleID = short.Parse(ctlGridRole.DataKeys[rowIndex].Value.ToString());
				CtlUserRoleEditor1.Initialize(FlagEnum.EditFlag, roleID);
                CtlUserRoleEditor1.ShowPopUp();

            }
            if (e.CommandName == "PB")
            {
                //find role id for current setup.
                 rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                short roleID = short.Parse(ctlGridRole.DataKeys[rowIndex].Value.ToString());

                ctlPBInfo1.Initialize(roleID);
                ctlPBInfo1.Show();
            }

            if (e.CommandName == "Service")
            {
                //find role id for current setup.
                 rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                short roleID = short.Parse(ctlGridRole.DataKeys[rowIndex].Value.ToString());
                ctlServiceTeamInfo1.Initialize(roleID);
                ctlServiceTeamInfo1.Show();
            }

            if (e.CommandName == "Program")
            {
                //find role id for current setup.
                 rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                short roleID = short.Parse(ctlGridRole.DataKeys[rowIndex].Value.ToString());

                ctlProgramInfo1.Initialize(roleID);
                ctlProgramInfo1.Show();
            }

            ctlGridRole.SelectedIndex = rowIndex;
            ctlUpdPanelGridView.Update();
            // แก้ไขการกด link ในกริด ที่จะเป็น Editor มาในหน้าเดียวกัน ไม่ต้องทำการ CountAndBind จะทำให้ไม่เกิด Event ในการกด Link ครั้งต่อไป
            //ctlGridRole.DataCountAndBind();
            
            
            //try
            //{
            //    int selectIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
            //    //ctlGridRole.Rows[selectIndex].BackColor = Color.WhiteSmoke;

            //}
            //catch (Exception)
            //{

            //}
        }


   }
}

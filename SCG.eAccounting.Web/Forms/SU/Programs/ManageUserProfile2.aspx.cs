using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SS.SU.DTO;
using SS.SU.Query;
using SCG.eAccounting.Web.Helper;
using SS.SU.DTO.ValueObject;
using System.Text;
using SS.SU.BLL;
using SS.SU.BLL.Implement;
using SCG.eAccounting.Web.UserControls;

namespace SCG.eAccounting.Web.Forms.SU.Programs
{
    public partial class ManageUserProfile2 : BasePage
    {
        #region property
        public long UserId { get; set; }
        public ISuUserFavoriteActorService SuUserFavoriteActorService { get; set; }
        public ISuUserService SuUserService { get; set; }
        public ISuUserRoleService SuUserRoleService { get; set; }
        public long UId
        {
            get { return UIHelper.ParseLong(user.Value); }
            set { user.Value = value.ToString(); }
        }
        public SuUser UserObj
        {
            get { return UserObj; }
            set { UserObj = value; }
        }
        #endregion

        #region EventLoad
        //protected override void OnPreLoad(EventArgs e)
        //{
        //    base.OnPreLoad(e);
        //    //if (!Page.IsPostBack)
        //    //{
        //    //    ProgramCode = "ManageUserProfile";
        //    //}

        //}


        private void RefreshPopup(object sender,EventArgs e)
        {
            ctlUpdatePanelInformation.Update();
        }

        private void RefreshGridData(object sender, EventArgs e)
        {
            ctlUserProfileGrid.DataCountAndBind();
            ctlUserGridUpdatePanel.Update();
            //ctlAddEditPopup.HidePopUp();
            HidePopUp();
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            ctlAddEditPopup.Notify_Ok += new EventHandler(RefreshGridData);
            ctlAddEditPopup.Notify_Cancle += new EventHandler(RefreshGridData);
            long uid = UserAccount.UserID;
            if (!Page.IsPostBack)
            {

                ctlUserGroup.DataSource = QueryProvider.SuRoleQuery.FindAll().OrderBy(x => x.RoleName.Trim());
                ctlUserGroup.DataTextField = "RoleName";
                ctlUserGroup.DataValueField = "RoleID";
                ctlUserGroup.DataBind();
                ctlUserGroup.Items.Insert(0, "");

                ctlUserProfileGrid.DataCountAndBind();
            }
        }
        //protected override void OnLoadComplete(EventArgs e)
        //{
        //    base.OnLoadComplete(e);
        //}
        #endregion

        #region User Profile Method Gridview Event
        protected void ctlUserProfile_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex =0;
            InvisibleAllPopUpcontrol();
            
            if (e.CommandName.Equals("Approver"))
            {
                rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                UId = UIHelper.ParseLong(ctlUserProfileGrid.DataKeys[rowIndex].Value.ToString());
                Approver.Initialize(UId);
                Approver.ShowDetail();

                // แก้ไขการกด link ในกริด ที่จะเป็น Editor มาในหน้าเดียวกัน ไม่ต้องทำการ CountAndBind จะทำให้ไม่เกิด Event ในการกด Link ครั้งต่อไป
                //ctlUserProfileGrid.DataCountAndBind();

            }
            if (e.CommandName.Equals("Initiator"))
            {
                rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                UId = UIHelper.ParseLong(ctlUserProfileGrid.DataKeys[rowIndex].Value.ToString());
                Initiator.Initialize(UId);
                Initiator.ShowDetail();
                //Approver.CloseApproverGrid();
            }
            if (e.CommandName.Equals("UserProfileMethodEdit"))
            {
                rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                UId = UIHelper.ParseLong(ctlUserProfileGrid.DataKeys[rowIndex].Value.ToString());
                
                ctlAddEditPopup.Initialize(FlagEnum.EditFlag, UId);
                //ctlAddEditPopup.ShowPopUp();
                ShowPopUp();
            }

            if (e.CommandName.Equals("UserProfileMethodDelete"))
            {
                try
                {
                    rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                    UId = UIHelper.ParseLong(ctlUserProfileGrid.DataKeys[rowIndex].Value.ToString());
                    SuUser userprofile = QueryProvider.SuUserQuery.FindByIdentity(UId);
                    SuUserService.DeleteUser(userprofile);

                }
                catch (Exception ex)
                {
                    if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                            "alert('This data is now in use.');", true);
                        ctlUserProfileGrid.DataCountAndBind();
                    }
                }

                ctlUserProfileGrid.DataCountAndBind();
               
                
            }
            ctlUserProfileGrid.SelectedIndex = rowIndex;
            ctlUserGridUpdatePanel.Update();
            ctlUpdatePanelInformation.Update();
            
        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            VOUserProfile criteria = GetSuUserCriteria();

            return QueryProvider.SuUserQuery.GetUserProfileListByCriteria(criteria, startRow, pageSize, sortExpression);
        }
        public int RequestCount()
        {
            VOUserProfile criteria = GetSuUserCriteria();
            int count = QueryProvider.SuUserQuery.CountUserProfileByCriteria(criteria);
            return count;

        }

        private void InvisibleAllPopUpcontrol()
        {
            Approver.HideDetail();
            Initiator.HideDetail();
        }
        
        #endregion

        #region set value method
        public VOUserProfile GetSuUserCriteria()
        {
            VOUserProfile user_profile = new VOUserProfile();
            user_profile.UserName = ctlUserID.Text;
            if (!String.IsNullOrEmpty(ctlUserGroup.SelectedValue))
            {
                user_profile.UserGroupId = UIHelper.ParseLong(ctlUserGroup.Text);
            }
            user_profile.EmployeeName = ctrEmployeeName.Text;
            return user_profile;
        }
        public SuUser GetUser()
        {
            return QueryProvider.SuUserQuery.FindByIdentity(UId);
        }
        #endregion

        #region button event
        protected void ctlUserProfileSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlUserProfileGrid.DataCountAndBind();
            InvisibleAllPopUpcontrol();
            ctlUserProfileGrid.SelectedIndex = -1;
        }
        protected void ctlAdd_Click(object sender, ImageClickEventArgs e)
        {
            ctlAddEditPopup.Initialize(FlagEnum.NewFlag, 0);
            //ctlAddEditPopup.ShowPopUp();
            ShowPopUp();
        }
        #endregion

        public void HidePopUp()
        {
            ctlUserProfileModalPopupExtender.Hide();
        }
        public void ShowPopUp()
        {
            ctlUserProfileModalPopupExtender.Show();
            //ctlUpdatePanelUserProfileForm.Update();
        }

    }
}

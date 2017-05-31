using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCG.DB.BLL;
using SCG.DB.DAL;
using SCG.DB.DTO;
using SCG.DB.DTO.ValueObject;
using SCG.DB.Query;
using SCG.eAccounting.BLL;
using SCG.eAccounting.DTO;
using SCG.eAccounting.Query;
using SCG.eAccounting.Web.Helper;
using SS.DB.DTO;
using SS.Standard.Security;
using SS.Standard.UI;
using SS.Standard.Utilities;


namespace SCG.eAccounting.Web.Forms.SCG.DB.Programs
{
    public partial class ProfileList : BasePage
    {
        #region Properties

        public IDbProfileListService DbProfileListService { get; set; }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ChangeButtonVisible(true, false, false);

                ctlProfileListGrid.DataCountAndBind();
                ctlUpdatePanelGridview.Update();
                DivManageDataField.Style.Add("display", "none");
            }
        }

        protected void ctlProfileListGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditItem")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                string ItemId = ctlProfileListGrid.DataKeys[rowIndex].Values["Id"].ToString();
                string ItemName = ctlProfileListGrid.DataKeys[rowIndex].Values["ProfileName"].ToString();
                DivManageDataField.Style.Add("display", "block");
                ctlAddItem.Visible = false;
                ctlInputProfileName.Text = ItemName;
                ctlHiddenProfileListId.Value = ItemId;
                ChangeButtonVisible(false, true, true);

                ctlUpdatePanelGridview.Update();
            } if (e.CommandName == "DeleteItem")
            {

                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                string ItemId = ctlProfileListGrid.DataKeys[rowIndex].Values["Id"].ToString();
                DbProfileList profileList = new DbProfileList();
                profileList.Id = new Guid(ItemId);

                IList<DbCompany> result = ScgDbQueryProvider.DbProfileListQuery.FindProfileToUse(new Guid(ItemId));

                if (result.Count != 0)
                {
                    string error = string.Empty;
                    foreach (DbCompany row in result)
                    {
                        if (error != string.Empty)
                            error += "/";

                        error += row.CompanyCode;
                    }
                    this.ValidationErrors.AddError("ProfileError.Error", new Spring.Validation.ErrorMessage("Profile in use by company : " + error));
                }
                else
                {
                    DbProfileListService.RemoveProfileList(profileList);
                    ctlProfileListGrid.DataCountAndBind();
                }
                ctlUpdatePanelGridview.Update();
            }

        }
        protected void ctlProfileListGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void ctlProfileListGrid_DataBound(object sender, EventArgs e)
        {

        }

        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            return ScgDbQueryProvider.DbProfileListQuery.GetProfileList(startRow, pageSize, sortExpression);
        }
        public int RequestCount()
        {
            int count = ScgDbQueryProvider.DbProfileListQuery.CountByProfileCriteria();
            return count;
        }

        #region ButtonEvent
        protected void ctlAddNew_Click(object sender, ImageClickEventArgs e)
        {
            DbProfileList profileList = new DbProfileList();
            profileList.ProfileName = ctlInputProfileName.Text;
            DbProfileListService.AddProfileList(profileList, UserAccount.UserID);
            DivManageDataField.Style.Add("display", "none");
            ctlAddItem.Visible = true;
            ctlProfileListGrid.DataCountAndBind();
            ctlUpdatePanelGridview.Update();
        }

        protected void ctlAddItem_Click(object sender, ImageClickEventArgs e)
        {
            DivManageDataField.Style.Add("display", "block");
            ctlInputProfileName.Text = string.Empty;
            ctlHiddenProfileListId.Value = string.Empty;
            ctlAddItem.Visible = false;
            ChangeButtonVisible(true, false, true);
            ctlUpdatePanelGridview.Update();
        }
        protected void ctlUpdate_Click(object sender, ImageClickEventArgs e)
        {
            DbProfileList profileList = new DbProfileList();
            profileList.ProfileName = ctlInputProfileName.Text;
            string id = ctlHiddenProfileListId.Value.ToString();
            profileList.Id = new Guid(id);
            DbProfileListService.UpdateProfileList(profileList, UserAccount.UserID);
            DivManageDataField.Style.Add("display", "none");
            ctlAddItem.Visible = true;
            ctlProfileListGrid.DataCountAndBind();
            ctlUpdatePanelGridview.Update();
        }
        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            ctlInputProfileName.Text = string.Empty;
            ctlHiddenProfileListId.Value = string.Empty;
            ChangeButtonVisible(true, false, false);
            ctlAddItem.Visible = true;
            DivManageDataField.Style.Add("display", "none");
            ctlProfileListGrid.DataCountAndBind();
            ctlUpdatePanelGridview.Update();
        }

        public void ChangeButtonVisible(bool AddBtn, bool UpdateBtn, bool CancelBtn)
        {
            AddButton.Visible = AddBtn;
            UpdateButton.Visible = UpdateBtn;
            CancelButton.Visible = CancelBtn;
        }

        #endregion

        #region Private Function



        #endregion



    }
}

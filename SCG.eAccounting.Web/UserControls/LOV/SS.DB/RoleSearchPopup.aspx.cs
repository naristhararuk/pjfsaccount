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
using SS.SU.DTO.ValueObject;
using SS.SU.BLL;
using SS.SU.Query;

using SCG.eAccounting.Web.Helper;

using SS.DB.Query;
using SS.DB.DTO;
using SS.DB.BLL;

namespace SCG.eAccounting.Web.UserControls.LOV.SS.DB
{
    public partial class RoleSearchPopup : BasePage
    {
        #region Property
        #region Service Property
        public ISuRoleService SuRoleService { get; set; }
        public IDbLanguageService DbLanguageService { get; set; }
        #endregion

        #region Local Property
        public string UserID
        {
            get { return txtUserId.Text; }
            set { txtUserId.Text = value; }
        }
        public string RoleName
        {
            get { return txtRoleName.Text; }
            set { txtRoleName.Text = value; }
        }
        public string LanguageId
        {
            get { return txtLanguageId.Text; }
            set { txtLanguageId.Text = value; }
        }
        #endregion
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region GridView Event
        protected int ctlRoleGrid_RequestCount()
        {
            short languageId = UIHelper.ParseShort(LanguageId);
            long userId = UIHelper.ParseLong(UserID);

            return QueryProvider.SuRoleLangQuery.FindCountSuRoleLangSearchResult(new SuRoleLangSearchResult(), languageId, RoleName, userId);
        }
        protected object ctlRoleGrid_RequestData(int startRow, int pageSize, string sortExpression)
        {
            short languageId = UIHelper.ParseShort(LanguageId);
            long userId = UIHelper.ParseLong(UserID);

            return QueryProvider.SuRoleLangQuery.GetTranslatedList(new SuRoleLangSearchResult(), languageId, RoleName, userId, startRow, pageSize, sortExpression);
        }
        protected void ctlRoleGrid_DataBound(object sender, EventArgs e)
        {
            if (ctlRoleGrid.Rows.Count > 0)
            {
                spanSaveButton.Visible = true;
                RegisterScriptForGridView();
            }
            else
            {
                spanSaveButton.Visible = false;
            }
        }
        #endregion

        #region Button Event
        protected void ctlSearch_Click(object sender, EventArgs e)
        {
            ctlRoleGrid.DataCountAndBind();
            //this.UpdatePanelGridView.Update();
        }
        protected void ctlCancel_Click(object sender, EventArgs e)
        {
            Hide();
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), this.ClientID + "Cancel", "notifyPopupResult('cancel')", true);

        }
        protected void ctlSubmit_Click(object sender, EventArgs e)
        {
            //IList<SuRole> list = new List<SuRole>();
            string listID = string.Empty;
            foreach (GridViewRow row in ctlRoleGrid.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
                {
                    short id = UIHelper.ParseShort(ctlRoleGrid.DataKeys[row.RowIndex].Values["RoleId"].ToString());
                    //SuRole role = SuRoleService.FindByIdentity(id);
                    listID += id.ToString() + "|";
                }
            }
            if (listID.Length > 0)
                listID.Remove(listID.Length - 1, 1);
            CallOnObjectLookUpReturn(listID);
            Hide();
        }
        #endregion

        #region Private Method
        private void CallOnObjectLookUpReturn(string id)
        {
            Hide();
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), this.ClientID + "notifyPopupResult()", "notifyPopupResult('ok' , '" + id + "')", true);
        }

        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateUserControlCheckBox(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlRoleGrid.ClientID + "', '" + ctlRoleGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "validateUserControlChkBox", script.ToString(), true);
        }
        #endregion

        #region Public Method
        public void Show()
        {
           // CallOnObjectLookUpCalling();
            ctlRoleGrid.DataCountAndBind();
            this.UpdatePanelGridView.Update();

            //this.ModalPopupExtender1.Show();
        }
        public void Hide()
        {
            txtRoleName.Text = "";
            LanguageId = "";
            UserID = "";
            //this.ModalPopupExtender1.Hide();
        }
        #endregion
    }
}

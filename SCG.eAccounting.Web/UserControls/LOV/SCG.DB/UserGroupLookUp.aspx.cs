using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SS.SU.DTO;
using SS.SU.Query;
using System.Text;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class UserGroupLookUp : BasePage
    {
        #region Properties

        public bool IsMultiple
        {
            get { return Convert.ToBoolean(ctlIsMultiple.Text); }
            set { ctlIsMultiple.Text = value.ToString(); }
        }
        public string UserGroupCode
        {
            get { return ctlUserGroupCode.Text; }

            set { ctlUserGroupCode.Text = value; }
        }
        public string UserGroupName
        {
            get { return ctlUserGroupName.Text; }

            set { ctlUserGroupName.Text = value; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IsMultiple = bool.Parse(Request["IsMultiple"].ToString());
                UserGroupCode = Request["UserGroupCode"].ToString();
                UserGroupName = Request["UserGroupName"].ToString();
                Show(IsMultiple);
            }
        }
        #region User Group Gidview

        protected void ctlUserGroupGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Select"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                short userGroupID = Convert.ToInt16(ctlUserGroupGrid.DataKeys[rowIndex].Value);
                CallOnObjectLookUpReturn(userGroupID.ToString());
                Hide();
                //SuRole userGroup = QueryProvider.SuRoleQuery.FindByIdentity(userGroupID);
                //CallOnObjectLookUpReturn(userGroup);
                //this.ModalPopupExtender1.Hide();
                //CancelUserGroupGridBind();
                //UpdatePanelGridView.Update();
            }
        }

        protected void ctlUserGroupGrid_DataBound(object sender, EventArgs e)
        {
            if (ctlUserGroupGrid.Rows.Count > 0)
            {
                RegisterScriptForGridView();
            }
        }

        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            SuRole role = GetUserGroupCriteria();

            return QueryProvider.SuRoleQuery.GetUserGroupList(role, startRow, pageSize, sortExpression);
        }

        public int RequestCount()
        {
            SuRole role = GetUserGroupCriteria();

            return QueryProvider.SuRoleQuery.CountUserGroupByCriteria(role);
        }

        #endregion

        #region Button Event

        protected void ctlSubmit_Click(object sender, EventArgs e)
        {
            //IList<SuRole> userGroupList = new List<SuRole>();
            string listID = string.Empty;
            foreach (GridViewRow row in ctlUserGroupGrid.Rows)
            {
                CheckBox ctlSelectChk = ctlUserGroupGrid.Rows[row.RowIndex].FindControl("ctlSelect") as CheckBox;
                if (ctlSelectChk.Checked)
                {
                    short userGroupID = Convert.ToInt16(ctlUserGroupGrid.DataKeys[row.RowIndex].Value);
                    //SuRole userGroup = QueryProvider.SuRoleQuery.FindByIdentity(userGroupID);
                    listID += userGroupID.ToString() + "|";
                }
            }
            if (listID.Length > 0)
                listID.Remove(listID.Length - 1, 1);
            CallOnObjectLookUpReturn(listID);
            Hide();
        }
        private void CallOnObjectLookUpReturn(string id)
        {
            Hide();
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), this.ClientID + "notifyPopupResult()", "notifyPopupResult('ok' , '" + id + "')", true);
        }
        protected void ctlCancel_Click(object sender, EventArgs e)
        {
            //ctlUserGroupCode.Text = string.Empty;
            //ctlUserGroupName.Text = string.Empty;
            //CancelUserGroupGridBind();
            //this.ModalPopupExtender1.Hide();
            //UpdatePanelGridView.Update();
            Hide();
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), this.ClientID + "Cancel", "notifyPopupResult('cancel')", true);
        }

        protected void ctlSearch_Click(object sender, EventArgs e)
        {
            ctlUserGroupGrid.Visible = true;
            ctlUserGroupGrid.DataCountAndBind();
            //UpdatePanelGridView.Update();
        }

        #endregion

        #region Private Function

        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBox(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlUserGroupGrid.ClientID + "', '" + ctlUserGroupGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "validateChkBox", script.ToString(), true);
        }

        public void Hide()
        {
            ctlUserGroupCode.Text = string.Empty;
            ctlUserGroupName.Text = string.Empty;
            //this.ModalPopupExtender1.Hide();
        }

        public void Show(bool setIsMultiple)
        {
            IsMultiple = setIsMultiple;
            if (IsMultiple)
            {
                ctlUserGroupGrid.Columns[0].Visible = true;
                ctlUserGroupGrid.Columns[1].Visible = false;
                ctlSubmit.Visible = true;
            }
            else
            {
                ctlUserGroupGrid.Columns[0].Visible = false;
                ctlUserGroupGrid.Columns[1].Visible = true;
                ctlSubmit.Visible = false;
            }
            //this.ModalPopupExtender1.Show();
            UpdatePanelGridView.Update();
        }

        public SuRole GetUserGroupCriteria()
        {
            SuRole userGroup = new SuRole();
            userGroup.RoleCode = ctlUserGroupCode.Text;
            userGroup.RoleName = ctlUserGroupName.Text;
            return userGroup;
        }
        public void CancelUserGroupGridBind()
        {
            ctlUserGroupGrid.DataSource = null;
            ctlUserGroupGrid.DataBind();
            ctlUserGroupGrid.Visible = false;
            //UpdatePanelGridView.Update();
        }

        #endregion
    }
}

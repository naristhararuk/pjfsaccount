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
using SS.SU.DTO.ValueObject;
using SCG.eAccounting.Web.Helper;

namespace SCG.eAccounting.Web.UserControls.LOV.SS.DB
{
    public partial class UserLookup : BaseUserControl
    {

        #region Property
        public string UserId
        {
            get { return ctlUserId.Text; }
            set { this.ctlUserId.Text = value; }
        }
        public short LanguageId
        {
            get { return UserAccount.CurrentLanguageID; }
        }
        public string Mode
        {
            get
            {
                if (ViewState["mode"] != null)
                    return ViewState["mode"].ToString();
                else
                    return "Single";
            }
            set { this.ViewState["mode"] = value; }
        }

        #endregion

        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Mode.Equals("Multiple"))
                {
                    ctlUserSearchResultGrid.Columns[0].Visible = true;
                }
                else
                {
                    ctlUserSearchResultGrid.Columns[0].Visible = false;
                }
            }
        }
        #endregion

        #region Public Method
        public string DisplayName(SuUserSearchResult obj)
        {
            return String.Format("{0}", obj.EmployeeName);
        }
        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBoxControl(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlUserSearchResultGrid.ClientID + "', '" + ctlUserSearchResultGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "validateChkBox", script.ToString(), true);
        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            IList<SuUserSearchResult> list = QueryProvider.SuUserQuery.GetUserSearchResultByCriteria(GetCriteria(), LanguageId, startRow, pageSize, sortExpression);
            return list;
        }
        public int RequestCount()
        {
            return QueryProvider.SuUserQuery.GetCountUserSearchResultByCriteria(GetCriteria(), LanguageId);
        }
        public UserCriteria GetCriteria()
        {
            UserCriteria criteria = new UserCriteria();
            if (!string.IsNullOrEmpty(ctlUserId.Text))
            {
                criteria.UserName = ctlUserId.Text;
            }

            criteria.Name = ctlName.Text;
            criteria.CompanyName = ctlCompany.Text;
            criteria.DivisionName = ctlDivision.Text;
            //criteria.Email = email.text;

            return criteria;
        }

        public void Show()
        {
            CallOnObjectLookUpCalling();
            ctlUserSearchResultGrid.DataCountAndBind();
            this.UpdatePanelSearchUser.Update();
            this.UpdatePanelGridView.Update();


            this.ModalPopupExtender1.Show();
        }
        public void Hide()
        {
            ctlUserId.Text = "";
            ctlName.Text = "";
            ctlCompany.Text = "";
            ctlDivision.Text = "";
            this.ModalPopupExtender1.Hide();
        }
        #endregion

        #region LinkButton Event
        protected void ctlSubmit_Click(object sender, EventArgs e)
        {
            IList<SuUserSearchResult> list = new List<SuUserSearchResult>();
            foreach (GridViewRow row in ctlUserSearchResultGrid.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
                {
                    long userId = UIHelper.ParseLong(ctlUserSearchResultGrid.DataKeys[row.RowIndex]["UserID"].ToString());
                    short divId = UIHelper.ParseShort(ctlUserSearchResultGrid.DataKeys[row.RowIndex]["DivisionID"].ToString());
                    short orgId = UIHelper.ParseShort(ctlUserSearchResultGrid.DataKeys[row.RowIndex]["OrganizationID"].ToString());
                    SuUserSearchResult selectedUser = QueryProvider.SuUserQuery.FindUserByUserIdLanguageId(userId, divId, orgId, LanguageId);
                    list.Add(selectedUser);
                }
            }
            CallOnObjectLookUpReturn(list);
            Hide();
        }
        protected void ctlSearch_Click(object sender, EventArgs e)
        {
            //UpdatePanelGridView.Update();
            ctlUserSearchResultGrid.DataCountAndBind();
        }
        protected void ctlCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }
        #endregion

        #region GridView Event
        protected void ctlUserSearchResultGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (Mode.Equals("Single"))
            {
                if (e.CommandName.Equals("SelectUser"))
                {
                    // Retrieve Object Row from GridView Selected Row
                    GridViewRow selectedRow = ((Control)e.CommandSource).NamingContainer as GridViewRow;
                    long userId = UIHelper.ParseLong(ctlUserSearchResultGrid.DataKeys[selectedRow.RowIndex]["UserID"].ToString());
                    short divId = UIHelper.ParseShort(ctlUserSearchResultGrid.DataKeys[selectedRow.RowIndex]["DivisionID"].ToString());
                    short orgId = UIHelper.ParseShort(ctlUserSearchResultGrid.DataKeys[selectedRow.RowIndex]["OrganizationID"].ToString());
                    SuUserSearchResult selectedUser = QueryProvider.SuUserQuery.FindUserByUserIdLanguageId(userId, divId, orgId, LanguageId);
                    // Return Selected Program.
                    CallOnObjectLookUpReturn(selectedUser);
                    // Hide Modal Popup
                    Hide();
                }
            }
        }
        protected void ctlUserSearchResultGrid_DataBound(object sender, EventArgs e)
        {
            if (Mode.Equals("Multiple"))
            {
                if (ctlUserSearchResultGrid.Rows.Count > 0)
                {
                    ctlDivSubmitButton.Visible = true;
                    RegisterScriptForGridView();
                }
                else
                {
                    ctlDivSubmitButton.Visible = false;
                }
            }
        }
        #endregion
    }
}
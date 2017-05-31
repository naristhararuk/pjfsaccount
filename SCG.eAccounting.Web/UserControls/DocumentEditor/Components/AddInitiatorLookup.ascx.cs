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

namespace SCG.eAccounting.Web.UserControls.DocumentEditor.Components
{
    public partial class AddInitiatorLookup : BaseUserControl
    {

        #region Property
        public string UserId
        {
            get { return ctlUserId.Text; }
            set { this.ctlUserId.Text = value; }
        }
        public string UserIdNotIn
        {
            get { return ctlUserIdNotIn.Text; }
            set { this.ctlUserIdNotIn.Text = value; }
            
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
            ctlUserSearchResultGrid.Columns[0].Visible = true;
            //if (!Page.IsPostBack)
            //{
            //    if (Mode.Equals("Multiple"))
            //    {
            //        ctlUserSearchResultGrid.Columns[0].Visible = true;
            //    }
            //    else
            //    {
            //        ctlUserSearchResultGrid.Columns[0].Visible = false;
            //    }
            //}
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
            IList<SuUserSearchResult> list = null;
           
                list = QueryProvider.SuUserQuery.GetInitialtorLookupSearchResultByCriteria(GetCriteria(), UserAccount.CurrentLanguageID, startRow, pageSize, sortExpression);

            


            return list;
        }
        public int RequestCount()
        {
            int count = 0;
            if (!this.DesignMode)
            {
                count = QueryProvider.SuUserQuery.GetCountInitialtorLookupResultByCriteria(GetCriteria(), UserAccount.CurrentLanguageID);
            }
            return count;
        }
        public UserCriteria GetCriteria()
        {
            UserCriteria criteria = new UserCriteria();
            if (!this.DesignMode)
            {
                if (!string.IsNullOrEmpty(ctlUserId.Text))
                {
                    criteria.UserId = UIHelper.ParseLong(ctlUserId.Text);
                }
                criteria.Name = ctlName.Text.Trim().Length < 0 ? string.Empty : ctlName.Text.Trim();
                criteria.CompanyName = ctlCompanyName.Text.Trim().Length < 0 ? string.Empty : ctlCompanyName.Text.Trim();
                criteria.Email = ctlEmail.Text.Trim().Length < 0 ? string.Empty : ctlEmail.Text.Trim();
                criteria.UserIdNOTIN = UserIdNotIn.Trim().Length < 0 ? string.Empty : UserIdNotIn;
            }
            return criteria;
        }

        public void Show()
        {
            if (!DesignMode)
            {
                CallOnObjectLookUpCalling();
                ctlUserSearchResultGrid.DataCountAndBind();
                this.UpdatePanelSearchUser.Update();
                this.UpdatePanelGridView.Update();


                this.ModalPopupExtender1.Show();
            }
        }
        public void Hide()
        {
            if (!DesignMode)
            {
                ctlUserId.Text = "";
                ctlName.Text = "";
                ctlEmail.Text = "";
                ctlCompanyName.Text = "";
                this.ModalPopupExtender1.Hide();
                this.OnObjectIsHide(true);
            }
        }
        #endregion

        #region LinkButton Event
        //protected void ctlSubmit_Click(object sender, EventArgs e)
        //{
            
        //}
        //protected void ctlSearch_Click(object sender, EventArgs e)
        //{
        //    //UpdatePanelGridView.Update();
            
        //}
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
                        // short divId = UIHelper.ParseShort(ctlUserSearchResultGrid.DataKeys[selectedRow.RowIndex]["DivisionID"].ToString());
                        // short orgId = UIHelper.ParseShort(ctlUserSearchResultGrid.DataKeys[selectedRow.RowIndex]["OrganizationID"].ToString());
                        SuUserSearchResult selectedUser = QueryProvider.SuUserQuery.FindUserByUserIdLanguageId(userId);
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

        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlUserSearchResultGrid.DataCountAndBind();
            this.ModalPopupExtender1.Show();
            OnObjectIsHide(false);

        }

        protected void ctlSubmit_Click(object sender, ImageClickEventArgs e)
        {
            IList<SuUserSearchResult> list = new List<SuUserSearchResult>();
            foreach (GridViewRow row in ctlUserSearchResultGrid.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect1")).Checked))
                {
                    long userId = UIHelper.ParseLong(ctlUserSearchResultGrid.DataKeys[row.RowIndex]["UserID"].ToString());
                    //short divId = UIHelper.ParseShort(ctlUserSearchResultGrid.DataKeys[row.RowIndex]["DivisionID"].ToString());
                    //short orgId = UIHelper.ParseShort(ctlUserSearchResultGrid.DataKeys[row.RowIndex]["OrganizationID"].ToString());
                    SuUserSearchResult selectedUser = QueryProvider.SuUserQuery.FindUserByUserIdLanguageId(userId);
                    list.Add(selectedUser);
                }
            }
            CallOnObjectLookUpReturn(list);
            Hide();
        }

       
    }
}
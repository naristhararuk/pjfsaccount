using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using SCG.DB.Query;
using System.Text;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class ExpenseCodeLookup : BasePage
    {
        #region Property
        public bool isMultiple
        {
            get
            {
                if (ViewState["isMultiple"] == null)
                    return false;
                else
                    return bool.Parse(ViewState["isMultiple"].ToString());
            }
            set
            {
                ViewState["isMultiple"] = value;
            }
        }
        //for query in lookup where company id of document
        public long? CompanyIDofDocument
        {
            get
            {
                if (ViewState["CompanyIDofDocument"] != null)
                {
                    return UIHelper.ParseLong(ViewState["CompanyIDofDocument"].ToString());
                }
                else
                {
                    return null;
                }
            }
            set { ViewState["CompanyIDofDocument"] = value; }
        }
        
        public string AccountCode
        {
            get { return ctlAccountCode.Text; }
            set { ctlAccountCode.Text = value; }
        }
        public string WithoutExpenseCode
        {
            get
            {
                if (ViewState["WithoutExpenseCode"] != null)
                    return ViewState["WithoutExpenseCode"].ToString();
                return string.Empty;
            }
            set { ViewState["WithoutExpenseCode"] = value; }
        }

        #endregion Property

        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                isMultiple = bool.Parse(Request["isMultiple"].ToString());
                CompanyIDofDocument = UIHelper.ParseLong(Request["companyID"].ToString());
                WithoutExpenseCode = Request["withoutExpenseCode"];
                this.Show();
            }
        }
        #endregion protected void Page_Load(object sender, EventArgs e)

        #region public Object RequestData(int startRow, int pageSize, string sortExpression)
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            return ScgDbQueryProvider.DbAccountQuery.GetAccountLovList(UserAccount.CurrentLanguageID, startRow, pageSize, sortExpression, ((ctlExpenseGroup.SelectedIndex == 0) ? "" : ctlExpenseGroup.SelectedValue), ctlAccountCode.Text, ctlDescription.Text, !this.CompanyIDofDocument.HasValue ? 0 : this.CompanyIDofDocument.Value, this.WithoutExpenseCode);
        }
        #endregion public Object RequestData(int startRow, int pageSize, string sortExpression)

        #region public int RequestCount()
        public int RequestCount()
        {
            int count = ScgDbQueryProvider.DbAccountQuery.CountByAccountLovCriteria(UserAccount.CurrentLanguageID, ((ctlExpenseGroup.SelectedIndex == 0) ? "" : ctlExpenseGroup.SelectedValue), ctlAccountCode.Text, ctlDescription.Text, !this.CompanyIDofDocument.HasValue ? 0 : this.CompanyIDofDocument.Value, this.WithoutExpenseCode);
            return count;
        }
        #endregion public int RequestCount()

        #region public void SetCombo()
        private void SetCombo()
        {
            ctlExpenseGroup.DataSource = ScgDbQueryProvider.DbExpenseGroupLangQuery.FindExpenseGroupByLangCriteria(UserAccount.CurrentLanguageID);
            ctlExpenseGroup.DataTextField = "Symbol";
            ctlExpenseGroup.DataValueField = "ID";
            ctlExpenseGroup.DataBind();

            ctlExpenseGroup.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), ""));

        }
        #endregion public void SetCombo()

        #region protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlAccountGridView.DataCountAndBind();
            UpdatePanelGridView.Update();
        }
        #endregion protected void ctlSearch_Click(object sender, ImageClickEventArgs e)

        #region protected void ctlAccountGrid_DataBound(object sender, EventArgs e)
        protected void ctlAccountGrid_DataBound(object sender, EventArgs e)
        {
            if (isMultiple && ctlAccountGridView.Rows.Count > 0)
                RegisterScriptForGridView();
        }
        #endregion protected void ctlAccountGrid_DataBound(object sender, EventArgs e)

        #region protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            Hide();
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), this.ClientID + "notifyPopupResult()", "notifyPopupResult('cancel')", true);
        }
        #endregion protected void ctlCancel_Click(object sender, ImageClickEventArgs e)

        #region protected void ctlAccountGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        protected void ctlAccountGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string id = string.Empty;
            if (e.CommandName.Equals("Select"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                long accountId = UIHelper.ParseLong(ctlAccountGridView.DataKeys[rowIndex].Value.ToString());

                //IList<AccountLang> accountLang = ScgDbQueryProvider.DbAccountLangQuery.FindByDbAccountLangKey(accountId, UserAccount.CurrentLanguageID);
                //if (accountLang.Count > 0)
                //NotifyPopUpReturn(new PopUpReturnArgs(PopUpReturnType.OK, accountLang[0]));
                //Hide();
                CallOnObjectLookUpReturn(accountId.ToString());
            }
        }
        #endregion protected void ctlAccountGrid_RowCommand(object sender, GridViewCommandEventArgs e)

        #region protected void ctlSelect_Click(object sender, ImageClickEventArgs e)
        protected void ctlSelect_Click(object sender, ImageClickEventArgs e)
        {
            if (isMultiple)
            {
                //IList<AccountLang> accountList = new List<AccountLang>();
                string listID = string.Empty;
                foreach (GridViewRow row in ctlAccountGridView.Rows)
                {
                    if ((row.RowType == DataControlRowType.DataRow) && ((CheckBox)row.FindControl("ctlSelect")).Checked)
                    {
                        long accountId = UIHelper.ParseLong(ctlAccountGridView.DataKeys[row.RowIndex].Value.ToString());
                        listID += accountId + "|";
                        //Label ctlAccountCodeInGrid = ctlAccountGridView.Rows[row.RowIndex].FindControl("ctlAccountCode") as Label;
                        //Label ctlDescriptionInGrid = ctlAccountGridView.Rows[row.RowIndex].FindControl("ctlDescription") as Label;

                        //AccountLang account = new AccountLang();

                        //account.AccountCode = ctlAccountCodeInGrid.Text;
                        //account.Description = ctlDescriptionInGrid.Text;

                        //accountList.Add(account);
                    }
                }

                CallOnObjectLookUpReturn(listID);
                Hide();
            }
        }
        #endregion protected void ctlSelect_Click(object sender, ImageClickEventArgs e)

        #region private void RegisterScriptForGridView()
        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBox(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlAccountGridView.ClientID + "', '" + ctlAccountGridView.HeaderRow.FindControl("ctlSelectHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "validateChkBox", script.ToString(), true);
        }
        #endregion private void RegisterScriptForGridView()

        #region public void Show()
        public void Show()
        {
            if (!isMultiple)
            {
                ctlAccountGridView.Columns[0].Visible = false;
                ctlAccountGridView.Columns[1].Visible = true;
                ctlSelect.Visible = false;
                ctlLine.Visible = false;
            }
            else
            {
                ctlAccountGridView.Columns[0].Visible = true;
                ctlAccountGridView.Columns[1].Visible = false;
                ctlSelect.Visible = true;
                ctlLine.Visible = true;
            }

            //CallOnObjectLookUpCalling();
            this.SetCombo();
            ctlAccountGridView.DataBind();
            this.UpdatePanelSearchAccount.Update();
            this.UpdatePanelGridView.Update();


            //this.ctlAccountLookupModalPopupExtender.Show();
        }
        #endregion public void Show()

        #region public void Hide()
        public void Hide()
        {
            ctlExpenseGroup.DataSource = null;
            ctlAccountCode.Text = string.Empty;
            ctlDescription.Text = string.Empty;
            UpdatePanelSearchAccount.Update();
            //this.ctlAccountLookupModalPopupExtender.Hide();
        }
        #endregion public void Hide()

        private void CallOnObjectLookUpReturn(string id)
        {
            Hide();  // clear ค่าหน้าจอ
            // ส่งค่ากลับให้ AccountLookup.ascx
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), this.ClientID + "notifyPopupResult()", "notifyPopupResult('ok' , '" + id + "')", true);
        }
    }
}

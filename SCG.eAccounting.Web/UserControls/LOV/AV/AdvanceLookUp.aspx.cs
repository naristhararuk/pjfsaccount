using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using System.Text;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.Query;
using SCG.eAccounting.Web.Helper;

namespace SCG.eAccounting.Web.UserControls.LOV.AV
{
    public partial class AdvanceLookUp : BasePage
    {
        #region Properties
        public bool isMultiple
        {
            get
            {
                return ViewState["IsMultipleReturn"] == null ? false : Convert.ToBoolean(ViewState["IsMultipleReturn"].ToString());
            }
            set { ViewState["IsMultipleReturn"] = value; }
        }
        public bool IsRepOffice
        {
            get
            {
                return ViewState["IsRepOffice"] == null ? false : Convert.ToBoolean(ViewState["IsRepOffice"].ToString());
            }
            set { ViewState["IsRepOffice"] = value; }
        }
        public long? PBID
        {
            get
            {
                return ViewState["PBID"] == null ? 0 : UIHelper.ParseLong(ViewState["PBID"].ToString());
            }
            set { ViewState["PBID"] = value; }
        }
        public short? MainCurrencyID
        {
            get
            {
                return ViewState["MainCurrencyID"] == null ? (short)0 : UIHelper.ParseShort(ViewState["MainCurrencyID"].ToString());
            }
            set { ViewState["MainCurrencyID"] = value; }
        }
        public long? CompanyID
        {
            get
            {
                return ViewState["AdvanceLookupCompanyID"] == null ? 0 : UIHelper.ParseLong(ViewState["AdvanceLookupCompanyID"].ToString());
            }
            set { ViewState["AdvanceLookupCompanyID"] = value; }
        }
        public string AdvanceType
        {
            get
            {
                return ViewState["IsMultiPleReturn"] == null ? null : ViewState["IsMultiPleReturn"].ToString();
            }
            set { ViewState["IsMultiPleReturn"] = value; }
        }
        public long? RequesterID
        {
            get
            {
                return ViewState["AdvanceLookupRequesterID"] == null ? 0 : UIHelper.ParseLong(ViewState["AdvanceLookupRequesterID"].ToString());
            }
            set { ViewState["AdvanceLookupRequesterID"] = value; }
        }
        public long? CurrentUserID
        {
            set { ViewState["CurrentUserID"] = value; }
            get { return ViewState["CurrentUserID"] == null ? 0 : UIHelper.ParseLong(ViewState["CurrentUserID"].ToString()); }
        }

        public string Text
        {
            get
            {
                if (string.IsNullOrEmpty(ctlAdvanceNo.Text) || string.IsNullOrEmpty(ctlDescription.Text))
                {
                    return string.Empty;
                }
                else
                {
                    return ctlAdvanceNo.Text + '-' + ctlDescription.Text;
                }
            }
        }
        public bool Display
        {
            set
            {
                if (value)
                    ctlContainerTable.Style.Add("display", "inline-block");
                else
                    ctlContainerTable.Style.Add("display", "none");
            }
        }

        private bool isRelateWithRemittanceButNotInExpense;
        public bool IsRelateWithRemittanceButNotInExpense
        {
            set
            {
                ViewState["isRelateWithRemittanceButNotInExpense"] = value;
            }
            get
            {
                return ViewState["isRelateWithRemittanceButNotInExpense"] == null ? false : bool.Parse(ViewState["isRelateWithRemittanceButNotInExpense"].ToString());
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                isMultiple = bool.Parse(Request["isMultiple"].ToString());
                CompanyID = long.Parse(Request["CompanyID"].ToString());
                AdvanceType = Request["AdvanceType"].ToString();
                RequesterID = long.Parse(Request["RequesterID"].ToString());
                CurrentUserID = long.Parse(Request["CurrentUser"].ToString());
                IsRepOffice = bool.Parse(Request["IsRepOffice"].ToString());
                PBID = long.Parse(Request["PBID"].ToString());
                MainCurrencyID = short.Parse(Request["MainCurrencyID"].ToString());
                IsRelateWithRemittanceButNotInExpense = bool.Parse(Request["IsRelateWithRemittanceButNotInExpense"].ToString());
                this.Show();
            }
        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            Advance advance = new Advance();
            advance.DocumentNo = ctlAdvanceNo.Text.Trim();
            advance.Description = ctlDescription.Text.Trim();
            advance.AdvanceType = this.AdvanceType;
            advance.CompanyID = this.CompanyID;
            advance.RequesterID = this.RequesterID;
            advance.CurrentUserID = this.CurrentUserID;
            advance.IsRepOffice = this.IsRepOffice;
            if (PBID != 0)
            {
                advance.PBID = PBID;
            }
            if (MainCurrencyID != 0)
            {
                advance.MainCurrencyID = MainCurrencyID;
            }
            IList<Advance> list;
            //if (ViewState["isRelateWithRemittanceButNotInExpense"] == null ? false : bool.Parse(ViewState["isRelateWithRemittanceButNotInExpense"].ToString()))
            if (IsRelateWithRemittanceButNotInExpense)
            {
                list = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAdvanceListRelateWithRemittanceButNotInExpense(advance, startRow, pageSize, sortExpression);
            }
            else
            {
                list = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAdvanceList(advance, startRow, pageSize, sortExpression);
            }

            return list;
        }
        public int RequestCount()
        {
            Advance advance = new Advance();
            advance.DocumentNo = ctlAdvanceNo.Text.Trim();
            advance.Description = ctlDescription.Text.Trim();
            advance.AdvanceType = this.AdvanceType;
            advance.CompanyID = this.CompanyID;
            advance.RequesterID = this.RequesterID;
            advance.CurrentUserID = this.CurrentUserID;
            advance.IsRepOffice = this.IsRepOffice;
            if (PBID != 0)
            {
                advance.PBID = PBID;
            }
            if (MainCurrencyID != 0)
            {
                advance.MainCurrencyID = MainCurrencyID;
            }
            int count = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.CountByAdvanceCriteria(advance);

            return count;
        }
        public void Show()
        {
            if (!isMultiple)
            {
                ctlAdvanceLookupGrid.Columns[0].Visible = false;
                ctlAdvanceLookupGrid.Columns[1].Visible = true;
                ctlSubmit.Visible = false;
                ctlLblLine.Visible = false;
            }
            else
            {
                ctlAdvanceLookupGrid.Columns[0].Visible = true;
                ctlAdvanceLookupGrid.Columns[1].Visible = false;
                ctlSubmit.Visible = true;
                ctlLblLine.Visible = true;
            }

            ctlAdvanceLookupGrid.DataCountAndBind();
            this.UpdatePanelSearchAdvance.Update();
            this.UpdatePanelGridView.Update();
        }
        public void Hide()
        {
            ctlAdvanceNo.Text = string.Empty;
            ctlDescription.Text = string.Empty;
            UpdatePanelSearchAdvance.Update();
        }



        private void CallOnObjectLookUpReturn(string id)
        {
            Hide();
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), this.ClientID + "notifyPopupResult()", "notifyPopupResult('ok' , '" + id + "')", true);
        }

        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            Hide();
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), this.ClientID + "Cancel", "notifyPopupResult('cancel')", true);
        }


        protected void ctlSubmit_Click(object sender, EventArgs e)
        {
            string list = string.Empty;
            foreach (GridViewRow row in ctlAdvanceLookupGrid.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
                {
                    long id = UIHelper.ParseLong(ctlAdvanceLookupGrid.DataKeys[row.RowIndex].Values["AdvanceID"].ToString());
                    list += id.ToString() + "|";


                }
            }
            if (list.Length > 0)
            {
            	list = list.Remove(list.Length - 1, 1);
            }
            CallOnObjectLookUpReturn(list);
            this.UpdatePanelSearchAdvance.Update();
            this.UpdatePanelGridView.Update();
        }

        protected void ctlAdvanceLookupGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Select"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                long advanceID = UIHelper.ParseLong(ctlAdvanceLookupGrid.DataKeys[rowIndex].Value.ToString());
                CallOnObjectLookUpReturn(advanceID.ToString());
                Hide();
            }
        }
        protected void ctlAdvanceLookupGrid_DataBound(object sender, EventArgs e)
        {
            if (isMultiple)
            {
                if (ctlAdvanceLookupGrid.Rows.Count > 0)
                {
                    RegisterScriptForGridView();
                }
            }
        }

        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBox(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlAdvanceLookupGrid.ClientID + "', '" + ctlAdvanceLookupGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "validateChkBox", script.ToString(), true);
        }

        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlAdvanceLookupGrid.DataCountAndBind();
            this.UpdatePanelSearchAdvance.Update();
            this.UpdatePanelGridView.Update();
        }




    }
}

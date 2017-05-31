using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.Query;
using System.Text;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.eAccounting
{
    public partial class CADocumentLookup : BasePage
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
        public long? CompanyID
        {
            get
            {
                return ViewState["MPADocumentLookupCompanyID"] == null ? 0 : UIHelper.ParseLong(ViewState["MPADocumentLookupCompanyID"].ToString());
            }
            set { ViewState["MPADocumentLookupCompanyID"] = value; }
        }
        public long? RequesterID
        {
            get
            {
                return ViewState["MPADocumentLookupRequesterID"] == null ? 0 : UIHelper.ParseLong(ViewState["MPADocumentLookupRequesterID"].ToString());
            }
            set { ViewState["MPADocumentLookupRequesterID"] = value; }
        }
        //public long? CurrentUserID
        //{
        //    set { ViewState["CurrentUserID"] = value; }
        //    get { return ViewState["CurrentUserID"] == null ? 0 : UIHelper.ParseLong(ViewState["CurrentUserID"].ToString()); }
        //}
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                isMultiple = bool.Parse(Request["isMultiple"].ToString());
                CompanyID = long.Parse(Request["CompanyID"].ToString());
                RequesterID = long.Parse(Request["RequesterID"].ToString());
                //CurrentUserID = long.Parse(Request["CurrentUser"].ToString());
                this.Show();
            }
        }

        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            return ScgeAccountingQueryProvider.CADocumentQuery.GetExpensesCAList(CompanyID, RequesterID, startRow, pageSize, sortExpression);
            //return null;
        }
        public int RequestCount()
        {
            int count = 0;

            count = ScgeAccountingQueryProvider.CADocumentQuery.GetExpensesCACount(CompanyID, RequesterID);

            return count;
        }
        public void Show()
        {
            if (!isMultiple)
            {
                ctlCALookupGrid.Columns[0].Visible = false;
                ctlCALookupGrid.Columns[1].Visible = true;
                ctlSubmit.Visible = false;
                ctlLblLine.Visible = false;
            }
            else
            {
                ctlCALookupGrid.Columns[0].Visible = true;
                ctlCALookupGrid.Columns[1].Visible = false;
                ctlSubmit.Visible = true;
                ctlLblLine.Visible = true;
            }

            ctlCALookupGrid.DataCountAndBind();
            this.UpdatePanelGridView.Update();
        }


        private void CallOnObjectLookUpReturn(string id)
        {
            //Hide();
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), this.ClientID + "notifyPopupResult()", "notifyPopupResult('ok' , '" + id + "')", true);
        }

        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            //Hide();
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), this.ClientID + "Cancel", "notifyPopupResult('cancel')", true);
        }


        protected void ctlSubmit_Click(object sender, EventArgs e)
        {
            string list = string.Empty;
            foreach (GridViewRow row in ctlCALookupGrid.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
                {
                    long id = UIHelper.ParseLong(ctlCALookupGrid.DataKeys[row.RowIndex].Values["CADocumentID"].ToString());
                    list += id.ToString() + "|";
                }
            }
            if (list.Length > 0)
            {
                list = list.Remove(list.Length - 1, 1);
            }
            CallOnObjectLookUpReturn(list);
            this.UpdatePanelGridView.Update();
        }

        protected void ctlCALookupGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Select"))
            {
                 int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                 long MPADocumentID = UIHelper.ParseLong(ctlCALookupGrid.DataKeys[rowIndex].Value.ToString());
                 CallOnObjectLookUpReturn(MPADocumentID.ToString());
                //Hide();
            }
        }
        protected void ctlCALookupGrid_DataBound(object sender, EventArgs e)
        {
            if (isMultiple)
            {
                if (ctlCALookupGrid.Rows.Count > 0)
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
            script.Append("'" + ctlCALookupGrid.ClientID + "', '" + ctlCALookupGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "validateChkBox", script.ToString(), true);
        }
    }
}
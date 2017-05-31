using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SS.Standard.UI;
using SS.Standard.Security;
using SCG.eAccounting.Web.Helper;
using SCG.DB.Query;
using SCG.DB.DTO;
using System.Text;


namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class CompanyLookup : BasePage
    {
        #region Properties

        public bool IsMultiple
        {
            get { return Convert.ToBoolean(ctlIsMultiple.Text); }
            set { ctlIsMultiple.Text = value.ToString(); }
        }
        public string CompanyCode
        {
            get { return ctlCompanyCodeCri.Text; }

            set { ctlCompanyCodeCri.Text = value; }
        }
        public string Companyname
        {
            get { return ctlCompanyNameCri.Text; }

            set { ctlCompanyNameCri.Text = value; }
        }
        public bool? FlagActive
        {
            get { return string.IsNullOrEmpty(ctlFlagActive.Value) ? (bool?)null : bool.Parse(ctlFlagActive.Value); }
            set { ctlFlagActive.Value = value.HasValue ? value.ToString() : string.Empty; }
        }
        public bool UseEccOnly // keep value in hidden field
        {
            get { return string.IsNullOrEmpty(ctlFlagUseEccOnly.Value) ? false : bool.Parse(ctlFlagUseEccOnly.Value); }
            set { ctlFlagUseEccOnly.Value = value.ToString(); }
        }
        public bool UseExpOnly // keep value in hidden field
        {
            get { return string.IsNullOrEmpty(ctlFlagUseExpOnly.Value) ? false : bool.Parse(ctlFlagUseExpOnly.Value); }
            set { ctlFlagUseExpOnly.Value = value.ToString(); }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IsMultiple = bool.Parse(Request["isMultiple"].ToString());
                CompanyCode = Request["CompanyCode"].ToString();
                CompanyName = Request["CompanyName"].ToString();
                FlagActive = String.IsNullOrEmpty(Request["FlagActive"].ToString()) ? (bool?)null : bool.Parse(Request["FlagActive"].ToString());
                UseEccOnly = bool.Parse(Request["UseEccOnly"].ToString());

                this.Show();
            }
            if (!Page.IsPostBack)
            {
                ctlCompanyGrid.DataCountAndBind();
            }

        }

        private void RegisterScriptForGridView()
        {
            if (IsMultiple)
            {
                if (ctlCompanyGrid.HeaderRow != null)
                {
                    StringBuilder script = new StringBuilder();
                    script.Append("function validateCheckBox(objChk, objFlag) ");
                    script.Append("{ ");
                    script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");

                    script.Append("'" + ctlCompanyGrid.ClientID + "', '" + ctlCompanyGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");

                    script.Append("} ");

                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "validateChkBox", script.ToString(), true);
                }
            }
        }
        protected void ctlSearch_Click(object sender, EventArgs e)
        {
            ctlCompanyGrid.DataCountAndBind();
            //UpdatePanelGridView.Update();
        }
        protected void ctlSubmit_Click(object sender, EventArgs e)
        {
            //IList<DbCompany> companyList = new List<DbCompany>();
            string listID = string.Empty;
            foreach (GridViewRow row in ctlCompanyGrid.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
                {
                    short companyID = Convert.ToInt16(ctlCompanyGrid.DataKeys[row.RowIndex].Value);
                    listID += companyID.ToString() + "|";
                    
                }
            }
            if(listID.Length>0)
                listID.Remove(listID.Length - 1, 1);
            CallOnObjectLookUpReturn(listID);
            Hide();
        }

        private void CallOnObjectLookUpReturn(string id)
        {
            Hide();
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), this.ClientID + "notifyPopupResult()", "notifyPopupResult('ok' , '" + id + "')", true);
        }

        protected void ctlCompanyGrid_DataBound(object sender, EventArgs e)
        {
            RegisterScriptForGridView();
        }
        protected void ctlCompanyGrid_RowDataBound(Object s, GridViewRowEventArgs e)
        {
            CheckBox chkActive = e.Row.FindControl("ctlChkActive") as CheckBox;
            if (chkActive != null && !chkActive.Checked)
                e.Row.ForeColor = System.Drawing.Color.Red;
        }
        protected void ctlCompanyGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Select"))
            {
                GridViewRow selectedRow = ((Control)e.CommandSource).NamingContainer as GridViewRow;
                long companyID = UIHelper.ParseLong(ctlCompanyGrid.DataKeys[selectedRow.RowIndex]["CompanyID"].ToString());
                CallOnObjectLookUpReturn(companyID.ToString());
                Hide();

                //int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                //long companyID = Convert.ToInt64(ctlCompanyGrid.DataKeys[rowIndex].Value);
                //DbCompany dbCompany = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(companyID);
                //NotifyPopUpReturn(new PopUpReturnArgs(PopUpReturnType.OK, dbCompany));
                //Hide();
                //CancelGridBind();
                //UpdatePanelGridView.Update();
            }
        }

        public void Hide()
        {
            ctlCompanyCodeCri.Text = string.Empty;
            ctlCompanyNameCri.Text = string.Empty;
            //this.ModalPopupExtender1.Hide();
        }
        public void Show()
        {
            if (IsMultiple)
            {
                ctlCompanyGrid.Columns[0].Visible = true;
                ctlCompanyGrid.Columns[1].Visible = false;
                ctlSubmit.Visible = true;
            }
            else
            {
                ctlCompanyGrid.Columns[0].Visible = false;
                ctlCompanyGrid.Columns[1].Visible = true;
                ctlSubmit.Visible = false;
            }
            //CallOnObjectLookUpCalling();
            ctlCompanyGrid.DataCountAndBind();
            //this.ModalPopupExtender1.Show();
            UpdatePanelGridView.Update();
        }






        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            DbCompany company = GetCompanyCriteria();

            return ScgDbQueryProvider.DbCompanyQuery.GetCompanyList(company, UserAccount.CurrentLanguageID, FlagActive, startRow, pageSize, sortExpression);
        }
        public int RequestCount()
        {
            DbCompany company = GetCompanyCriteria();

            int count = ScgDbQueryProvider.DbCompanyQuery.CountCompanyByCriteria(company, UserAccount.CurrentLanguageID, FlagActive);

            return count;
        }

        public DbCompany GetCompanyCriteria()
        {
            DbCompany company = new DbCompany();
            company.CompanyCode = ctlCompanyCodeCri.Text;
            company.CompanyName = ctlCompanyNameCri.Text;

            if(UseEccOnly || UseExpOnly)
            {
                company.UseEcc = UseExpOnly ? false : true;
            }

            return company;
        }



        protected void ctlCancel_Click(object sender, EventArgs e)
        {
            //ctlCompanyCodeCri.Text = string.Empty;
            //ctlCompanyNameCri.Text = string.Empty;
            //UpdatePanelSearchCompany.Update();
            //CancelGridBind();
            //UpdatePanelGridView.Update();
            //this.ModalPopupExtender1.Hide();
            Hide();
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), this.ClientID + "Cancel", "notifyPopupResult('cancel')", true);
        }


        public void CancelGridBind()
        {
            ctlCompanyGrid.DataSource = null;
            ctlCompanyGrid.DataBind();
        }
    }
}

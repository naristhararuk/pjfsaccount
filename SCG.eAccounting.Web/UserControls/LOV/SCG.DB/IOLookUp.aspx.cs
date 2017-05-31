using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCG.eAccounting.Web.Helper;
using SCG.DB.DTO;
using SCG.DB.Query;
using System.Text;
using SS.Standard.UI;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class IOLookUp : BasePage
    {
        #region properties

        public bool isMultiple
        {
            get
            {
                if (ctlMode.Text.Equals("TRUE"))
                    return true;
                else
                    return false;
            }
            set { ctlMode.Text = value.ToString().ToUpper(); }
        }
        public long? CompanyId
        {
            get
            {
                if (string.IsNullOrEmpty(ctlCompanyId.Text))
                    return null;
                return UIHelper.ParseLong(ctlCompanyId.Text);
            }
            set
            {
                if (value.HasValue)
                    ctlCompanyId.Text = value.Value.ToString();
                else
                    ctlCompanyId.Text = string.Empty;
            }
        }
        public long? CostCenterId
        {
            get
            {
                if (string.IsNullOrEmpty(ctlCostCenterId.Text))
                    return null;
                return UIHelper.ParseLong(ctlCostCenterId.Text);
            }
            set
            {
                if (value.HasValue)
                    ctlCostCenterId.Text = value.Value.ToString();
                else
                    ctlCostCenterId.Text = string.Empty;
            }
        }
        public string CompanyCode
        {
            get { return ctlCompanyCode.Text; }
            set { ctlCompanyCode.Text = value; }
        }
        public string CostCenterCode
        {
            get { return ctlCostCenterCode.Text; }
            set { ctlCostCenterCode.Text = value; }
        }
        public string IONumber
        {
            get { return ctlIONumber.Text; }
            set { ctlIONumber.Text = value; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                isMultiple = bool.Parse(Request["isMultiple"].ToString());
                if (Request["CompanyId"].ToString().Length > 0)
                    CompanyId = long.Parse( Request["CompanyId"].ToString());
                if (Request["CostCenterId"].ToString().Length > 0)
                    CostCenterId = long.Parse(Request["CostCenterId"].ToString());
                CompanyCode = Request["CompanyCode"].ToString();
                CostCenterCode = Request["CostCenterCode"].ToString();
                IONumber = Request["IONumber"].ToString();

                this.Show();
            }
        }

        protected void ctlSubmit_Click(object sender, EventArgs e)
        {
            string listID = string.Empty;
            foreach (GridViewRow row in ctlSearchResultGrid.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
                {
                    long id = UIHelper.ParseLong(ctlSearchResultGrid.DataKeys[row.RowIndex]["IOID"].ToString());
                    listID += id.ToString() + "|";

                }
            }
            listID.Remove(listID.Length - 1, 1);
            CallOnObjectLookUpReturn(listID);
            Hide();
        }
        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlSearchResultGrid.DataCountAndBind();
        }

        #region private void Alert()
        private void Alert()
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "DisableDropdown", "function IOComfirm(){return confirm('ConfirmIOExpireDate'); } ", true);
        }
        #endregion private void Alert()

        protected void ctlSearchResultGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (!isMultiple)
                {
                    long ioId = UIHelper.ParseLong(ctlSearchResultGrid.DataKeys[e.Row.RowIndex]["IOID"].ToString());
                    DbInternalOrder selectedIO = ScgDbQueryProvider.DbIOQuery.FindByIdentity(ioId);

                    if (DateTime.Now > selectedIO.ExpireDate)
                    {
                        e.Row.ForeColor = System.Drawing.Color.Red;

                        Alert();
                        ImageButton img = e.Row.FindControl("ctlIOSelect") as ImageButton;
                        img.Attributes.Add("onClick", "return IOComfirm();");
                    }
                }
            }
        }

        protected void ctlSearchResultGrid_DataBound(object sender, EventArgs e)
        {
            if (isMultiple)
            {
                if (ctlSearchResultGrid.Rows.Count > 0)
                {
                    ctlSubmitButton.Visible = true;
                    RegisterScriptForGridView();
                }
                else
                {
                    ctlSubmitButton.Visible = false;
                }

                ctlSearchResultGrid.Columns[0].Visible = true;
                ctlSearchResultGrid.Columns[1].Visible = false;
            }
            else
            {
                ctlSearchResultGrid.Columns[0].Visible = false;
                ctlSearchResultGrid.Columns[1].Visible = true;
            }
        }
        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBoxControl(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlSearchResultGrid.ClientID + "', '" + ctlSearchResultGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "validateChkBox", script.ToString(), true);
        }
        protected void ctlSearchResultGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("SelectIO"))
            {
                GridViewRow selectedRow = ((Control)e.CommandSource).NamingContainer as GridViewRow;
                long ioId = UIHelper.ParseLong(ctlSearchResultGrid.DataKeys[selectedRow.RowIndex]["IOID"].ToString());
                DbInternalOrder selectedIO = ScgDbQueryProvider.DbIOQuery.FindByIdentity(ioId);
                CallOnObjectLookUpReturn(ioId.ToString());
                Hide();

            }
        }

        public void Show()
        {
            ctlSearchResultGrid.DataBind();
            this.UpdatePanelSearch.Update();
            this.UpdatePanelGridView.Update();
        }



        public void Hide()
        {
            ctlIONumber.Text = string.Empty;
            ctlIODescription.Text = string.Empty;
            UpdatePanelSearch.Update();
        }
        public DbInternalOrder GetCriteria()
        {
            DbInternalOrder io = new DbInternalOrder();
            io.IONumber = ctlIONumber.Text;
            io.IOText = ctlIODescription.Text;
            io.CompanyID = UIHelper.ParseLong(ctlCompanyId.Text);
            io.CostCenterCode = ctlCostCenterCode.Text;
            io.CostCenterID = CostCenterId;

            return io;
        }
        protected Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            return ScgDbQueryProvider.DbIOQuery.GetIOList(
                GetCriteria(), startRow, pageSize, sortExpression);
        }

        protected int ctlSearchResultGrid_RequestCount()
        {
            return ScgDbQueryProvider.DbIOQuery.CountByIOCriteria(GetCriteria());
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

    }
}

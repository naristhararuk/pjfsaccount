using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.DB.Query;
using SCG.DB.DTO.ValueObject;
using SCG.DB.DTO;
using System.Text;
using SCG.eAccounting.Web.Helper;
using System.Web.Script.Serialization;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class VendorLookup : BasePage
    {
        #region Properties
        public bool isMultiple
        {
            get
            {
                if (ViewState["isMultiple"] != null)
                    return (bool)ViewState["isMultiple"];
                return false;
            }
            set { ViewState["isMultiple"] = value; }
        }
        public bool excludeOneTime
        {
            get
            {
                if (ViewState["excludeOneTime"] != null)
                    return (bool)ViewState["excludeOneTime"];
                return false;
            }
            set { ViewState["excludeOneTime"] = value; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                isMultiple = bool.Parse(Request["isMultiple"].ToString());
                excludeOneTime = bool.Parse(Request["excludeOneTime"].ToString());
                this.Show();
            }
        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            return ScgDbQueryProvider.DbVendorQuery.GetVendorListByVendorCriteria(GetCriteria(), startRow, pageSize, sortExpression);
        }
        public int RequestCount()
        {
            return ScgDbQueryProvider.DbVendorQuery.CountVendorByVendorCriteria(GetCriteria()); ;
        }
        public VOVendor GetCriteria()
        {
            VOVendor criteria = new VOVendor();
            criteria.VendorBranch = ctlBranch.Text.Trim();
            criteria.VendorTaxCode = ctlTaxId.Text.Trim();
            criteria.VendorCode = ctlVendorCode.Text.Trim();
            criteria.VendorName = ctlVendorName.Text.Trim();
            criteria.ExcludeOneTime = excludeOneTime;
            return criteria;
        }
        public void Show()
        {
            if (!isMultiple)
            {
                ctlVendorGrid.Columns[0].Visible = false;
                ctlVendorGrid.Columns[1].Visible = true;
                ctlSubmit.Visible = false;
                ctlLblLine.Visible = false;
            }
            else
            {
                ctlVendorGrid.Columns[0].Visible = true;
                ctlVendorGrid.Columns[1].Visible = false;
                ctlSubmit.Visible = true;
                ctlLblLine.Visible = true;
            }
            ctlVendorGrid.DataCountAndBind();
            this.UpdatePanelSearchVendor.Update();
            this.UpdatePanelGridView.Update();
        }
        public void Hide()
        {
            ResetControl();
        }

        #region event Click
        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlVendorGrid.DataCountAndBind();
        }

        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            Hide();
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), this.ClientID + "Cancel", "notifyPopupResult('cancel')", true);
        }
        protected void ctlSubmit_Click(object sender, EventArgs e)
        {
            IList<VOVendor> list = new List<VOVendor>();
            foreach (GridViewRow row in ctlVendorGrid.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
                {
                    //long id = UIHelper.ParseShort(ctlVendorGrid.DataKeys[row.RowIndex].Values["VendorID"].ToString());
                    //DbVendor dbVendor = ScgDbQueryProvider.DbVendorQuery.FindByIdentity(id);
                    //list.Add(dbVendor);
                    int rowIndex = row.RowIndex;
                    Label vendorCode = ctlVendorGrid.Rows[rowIndex].FindControl("ctlGridVendorCode") as Label;
                    Label vendorName = ctlVendorGrid.Rows[rowIndex].FindControl("ctlGridVendorName") as Label;
                    Label taxCode = ctlVendorGrid.Rows[rowIndex].FindControl("ctlGridTaxID") as Label;
                    Label street = ctlVendorGrid.Rows[rowIndex].FindControl("ctlStreet") as Label;
                    Label country = ctlVendorGrid.Rows[rowIndex].FindControl("ctlCountry") as Label;
                    Label city = ctlVendorGrid.Rows[rowIndex].FindControl("ctlCity") as Label;
                    Label postalCode = ctlVendorGrid.Rows[rowIndex].FindControl("ctlPostalCode") as Label;
                    Label branch = ctlVendorGrid.Rows[rowIndex].FindControl("ctlGridBranch") as Label;
                    long? vendorID = null;
                    if (ctlVendorGrid.DataKeys[rowIndex].Value != null)
                    {
                        vendorID = UIHelper.ParseLong(ctlVendorGrid.DataKeys[rowIndex].Value.ToString());
                    }

                    VOVendor vendor = new VOVendor();
                    vendor.VendorID = vendorID;
                    vendor.VendorCode = vendorCode.Text;
                    vendor.VendorName = vendorName.Text;
                    vendor.VendorTaxCode = taxCode.Text;
                    vendor.Street = street.Text;
                    vendor.Country = country.Text;
                    vendor.City = city.Text;
                    vendor.PostalCode = postalCode.Text;
                    vendor.VendorBranch = branch.Text;
                    list.Add(vendor);
                }
            }

            JavaScriptSerializer serialize = new JavaScriptSerializer();
            string vendorInfo = serialize.Serialize(list);
            CallOnObjectLookUpReturn(vendorInfo);
        }
        #endregion

        #region event Grid
        protected void ctlVendorGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!isMultiple)
            {
                if (e.CommandName.Equals("Select"))
                {
                    int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                    Label vendorCode = ctlVendorGrid.Rows[rowIndex].FindControl("ctlGridVendorCode") as Label;
                    Label vendorName = ctlVendorGrid.Rows[rowIndex].FindControl("ctlGridVendorName") as Label;
                    Label taxCode = ctlVendorGrid.Rows[rowIndex].FindControl("ctlGridTaxID") as Label;
                    Label street = ctlVendorGrid.Rows[rowIndex].FindControl("ctlStreet") as Label;
                    Label country = ctlVendorGrid.Rows[rowIndex].FindControl("ctlCountry") as Label;
                    Label city = ctlVendorGrid.Rows[rowIndex].FindControl("ctlCity") as Label;
                    Label postalCode = ctlVendorGrid.Rows[rowIndex].FindControl("ctlPostalCode") as Label;
                    Label branch = ctlVendorGrid.Rows[rowIndex].FindControl("ctlGridBranch") as Label;


                    long? vendorID = null;
                    if (ctlVendorGrid.DataKeys[rowIndex].Value != null)
                    {
                        vendorID = UIHelper.ParseLong(ctlVendorGrid.DataKeys[rowIndex].Value.ToString());
                    }

                    VOVendor vendor = new VOVendor();
                    vendor.VendorID = vendorID;
                    vendor.VendorCode = vendorCode.Text;
                    vendor.VendorName = vendorName.Text;
                    vendor.VendorTaxCode = taxCode.Text;
                    vendor.Street = street.Text;
                    vendor.Country = country.Text;
                    vendor.City = city.Text;
                    vendor.PostalCode = postalCode.Text;
                    vendor.VendorBranch = branch.Text;
                    JavaScriptSerializer serialize = new JavaScriptSerializer();
                    string vendorInfo = serialize.Serialize(vendor);
                    CallOnObjectLookUpReturn(vendorInfo);
                }
            }
        }
        protected void ctlVendorGrid_DataBound(object sender, EventArgs e)
        {
            if (isMultiple)
            {
                if (ctlVendorGrid.Rows.Count > 0)
                {
                    RegisterScriptForGridView();
                }
            }
        }
        #endregion

        #region private void RegisterScriptForGridView()
        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBox(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlVendorGrid.ClientID + "', '" + ctlVendorGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "validateChkBox", script.ToString(), true);
        }
        #endregion private void RegisterScriptForGridView()

        public void ResetControl()
        {
            ctlTaxId.Text = string.Empty;
            ctlBranch.Text = string.Empty;
            ctlVendorCode.Text = string.Empty;
            ctlVendorName.Text = string.Empty;
            UpdatePanelSearchVendor.Update();
        }
        private void CallOnObjectLookUpReturn(string vendorInfo)
        {
            Hide();  // clear ค่าหน้าจอ
            // ส่งค่ากลับให้ VendorLookup.ascx
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), this.ClientID + "notifyPopupResult()", "notifyPopupResult('ok' , '" + vendorInfo + "')", true);
        }
    }
}

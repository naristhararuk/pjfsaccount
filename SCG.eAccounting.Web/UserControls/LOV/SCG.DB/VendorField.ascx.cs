using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

using SS.Standard.UI;

using SCG.eAccounting.Web.Helper;
using SCG.DB.Query;
using SCG.DB.DTO;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class VendorField : BaseUserControl
    {
        #region Property
        public string Mode
        {
            get { return ctlMode.Text; }
            set { ctlMode.Text = value; }
        }
        //public string TaxNo
        //{
        //    get { return ctlVendorTaxNo.Text; }
        //    set { ctlVendorTaxNo.Text = value; }
        //}
        public string VendorCode
        {
            get { return ctlVendorCode.Text; }
            set { ctlVendorCode.Text = value; }
        }
        //public string VendorId
        //{
        //    get { return ctlVendorID.Text; }
        //    set { ctlVendorID.Text = value; }
        //}
        #endregion

        #region Page_Load Event
        protected void Page_Load(object sender, EventArgs e)
        {
            Vendor1.OnObjectLookUpCalling += new BaseUserControl.ObjectLookUpCallingEventHandler(Vendor1_OnObjectLookUpCalling);
            Vendor1.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(Vendor1_OnObjectLookUpReturn);

            //long vendorID = UIHelper.ParseLong(VendorId.ToString());

            //DbVendor parameter = new DbVendor();
            //parameter.VendorID = vendorID;
            //parameter.VendorCode = VendorCode;

            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //ctlVendorAutoComplete.ContextKey = serializer.Serialize(parameter);
            //ctlVendorAutoComplete.UseContextKey = true;

        }

        #endregion

        #region Public Method
        public void ChangeMode()
        {
            if ((!string.IsNullOrEmpty(this.Mode)) && (this.Mode.Equals(ModeEnum.ReadWrite)))
            {
                ctlVendorCode.Enabled = true;
                ctlSearch.Visible = true;
            }
            else
            {
                ctlVendorCode.Enabled = false;
                ctlSearch.Visible = false;
            }
        }
        #endregion

        protected void Vendor1_OnObjectLookUpCalling(object sender, ObjectLookUpCallingEventArgs e)
        {
            UserControls.LOV.SCG.DB.VendorLookUp vendorSearch = sender as UserControls.LOV.SCG.DB.VendorLookUp;
            
        }
        protected void Vendor1_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            IList<DbVendor> vendor = (IList<DbVendor>)e.ObjectReturn;
            DbVendor dbVendor = vendor.First<DbVendor>();
            //ctlTaxNo.TaxNo = dbVendor.TaxNo1;
            ctlVendorCode.Text = dbVendor.VendorCode;
            if (dbVendor.TaxNo1 != null && dbVendor.TaxNo2 == null)
            {
                ctlTaxNo.TaxNo = dbVendor.TaxNo1;
                ctlVendorName.Text = dbVendor.VendorName1;
            }
            else if (dbVendor.TaxNo2 != null && dbVendor.TaxNo1 == null)
            {
                ctlTaxNo.TaxNo = dbVendor.TaxNo2;
                ctlVendorName.Text = dbVendor.VendorName2;
            }
            else
            {
                ctlTaxNo.TaxNo = dbVendor.TaxNo2;
                ctlVendorName.Text = dbVendor.VendorName2;
            }
            ctlStreet.Text = dbVendor.Street;
            ctlCity.Text = dbVendor.City;
            ctlCountry.Text = dbVendor.Country;
            ctlPostalCode.Text = dbVendor.PostalCode;
            CallOnObjectLookUpReturn(dbVendor);
            ctlUpdatePanelVendor.Update();
        }

        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            Vendor1.Show();
        }
        protected void ctlTaxNo_NotifyPopupResult(object sender, string action, string result)
        {
            //if (action != "ok") //return;
            IList<DbVendor> iListDbVendor = ScgDbQueryProvider.DbVendorQuery.FindByDbVendor(UIHelper.ParseLong(result));
            DbVendor dbVendor = iListDbVendor.First<DbVendor>();
            ctlVendorCode.Text = dbVendor.VendorCode;
            if (dbVendor.TaxNo1 != null && dbVendor.TaxNo2 == null)
            {
                ctlTaxNo.TaxNo = dbVendor.TaxNo1;
                ctlVendorName.Text = dbVendor.VendorName1;
            }
            else if (dbVendor.TaxNo2 != null && dbVendor.TaxNo1 == null)
            {
                ctlTaxNo.TaxNo = dbVendor.TaxNo2;
                ctlVendorName.Text = dbVendor.VendorName2;
            }
            else
            {
                ctlTaxNo.TaxNo = dbVendor.TaxNo2;
                ctlVendorName.Text = dbVendor.VendorName2;
            }
            
            ctlStreet.Text = dbVendor.Street;
            ctlCity.Text = dbVendor.City;
            ctlCountry.Text = dbVendor.Country;
            ctlPostalCode.Text = dbVendor.PostalCode;
            CallOnObjectLookUpReturn(dbVendor);
        }
        public void ResetValue()
        {
            ctlVendorCode.Text = string.Empty;
            ctlTaxNo.TaxNo = string.Empty;
            ctlVendorName.Text = string.Empty;
            ctlStreet.Text = string.Empty;
            ctlCity.Text = string.Empty;
            ctlCountry.Text = string.Empty;
            ctlPostalCode.Text = string.Empty;
            ctlUpdatePanelVendor.Update();
        }
    }
}
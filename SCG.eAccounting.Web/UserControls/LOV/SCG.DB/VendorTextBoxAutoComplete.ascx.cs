using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

using SS.Standard.UI;

using SCG.DB.DTO;
using SCG.DB.DTO.ValueObject;
using SCG.eAccounting.Web.Helper;


namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class VendorTextBoxAutoComplete : BaseUserControl, IEditorUserControl
    {
        #region Property
        public string TaxNo
        {
            get { return txtTaxNo.Text; }
            set { txtTaxNo.Text = value; }
        }
        public string VendorCode
        {
            get { return ctlVendorCode.Text; }
            set { ctlVendorCode.Text = value; }
        }
        public string VendorId
        {
            get { return ctlVendorID.Text; }
            set { ctlVendorID.Text = value; }
        }
        public string Street
        {
            get { return ctlStreet.Text; }
            set { ctlStreet.Text = value; }
        }
        public string VendorName
        {
            get { return txtVendorName.Text; }
            set { txtVendorName.Text = value; }
        }
        public string City
        {
            get { return ctlCity.Text; }
            set { ctlCity.Text = value; }
        }
        public string Country
        {
            get { return ctlCountry.Text; }
            set { ctlCountry.Text = value; }
        }
        public string PostalCode
        {
            get { return ctlPostal.Text; }
            set { ctlPostal.Text = value; }
        }
        public string Onblur
        {
            get
            {
                if (ViewState["Onblur"] != null)
                    return ViewState["Onblur"].ToString();
                return null;
            }
            set { ViewState["Onblur"] = value; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Onblur))
                {
                    txtTaxNo.Attributes.Remove("onblur");
                    txtTaxNo.Attributes.Add("onblur", string.Format("{0};", Onblur));
                }
            }
            //ctlVendorAutoComplete.BehaviorID = String.Format("VendorAutoCompleteEx{0}", txtTaxNo.ClientID);
            SetAutoCompleteEvent();

            long vendorID = UIHelper.ParseLong(VendorId.ToString());

            DbVendor parameter = new DbVendor();
            //parameter.VendorID      = vendorID;
            //parameter.VendorCode    = VendorCode;
            //parameter.VendorName1   = VendorName;
            //parameter.Street        = Street;
            //parameter.City          = City;
            //parameter.Country       = Country;
            //parameter.PostalCode    = PostalCode;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            //ctlVendorAutoComplete.ContextKey = serializer.Serialize(parameter);
            //ctlVendorAutoComplete.UseContextKey = true;
        }
        public delegate void NotifyPopupResultHandler(object sender, string returnAction, string returnValue);
        public event NotifyPopupResultHandler NotifyPopupResult;

        public delegate void ChangedVendorHandler(object sender, object objectReturn);
        public event ChangedVendorHandler ChangedVendor;

        protected void ctlReturnAction_ValueChanged(object sender, EventArgs e)
        {
            VOVendor vendor = new VOVendor();
            if (!string.IsNullOrEmpty(ctlVendorID.Text))
            {
                vendor.VendorID = UIHelper.ParseLong(ctlVendorID.Text);
            }
            vendor.VendorName = txtVendorName.Text;
            vendor.VendorCode = ctlVendorCode.Text;
            vendor.Street = ctlStreet.Text;
            vendor.City = ctlCity.Text;
            vendor.Country = ctlCountry.Text;
            vendor.PostalCode = ctlPostal.Text;
            vendor.VendorTaxCode = txtTaxNo.Text;

            if (ChangedVendor != null)
                ChangedVendor(this, vendor);
            //NotifyPopupResult(this, ctlReturnAction.Value, ctlReturnValue.Value);
            ctlReturnAction.Value = "";

        }
        private void SetAutoCompleteEvent()
        {
            string ClientId = txtTaxNo.ClientID;
            //ctlVendorAutoComplete.Animations = ctlVendorAutoComplete.Animations.Replace("VendorAutoCompleteEx", ctlVendorAutoComplete.BehaviorID);
            //ctlVendorAutoComplete.OnClientItemSelected = String.Format("{0}_OnSelected", ClientId);
            //ctlVendorAutoComplete.OnClientPopulating = String.Format("{0}_OnPopulating", ClientId);
            //ctlVendorAutoComplete.OnClientPopulated = String.Format("{0}_OnPopulated", ClientId);
            //ctlVendorAutoComplete.OnClientItemOut = String.Format("{0}_OnItemOut", ClientId);
            //ctlVendorAutoComplete.OnClientShowing = String.Format("{0}_OnShowing", ClientId);
            //ctlVendorAutoComplete.OnClientShown = String.Format("{0}_OnShown", ClientId);
            //ctlVendorAutoComplete.OnClientHidden = String.Format("{0}_OnHidden", ClientId);
            //ctlVendorAutoComplete.OnClientHiding = String.Format("{0}_OnHiding", ClientId);
        }
        public void ResetValue()
        {
            VendorId = string.Empty;
            VendorCode = string.Empty;
            VendorName = string.Empty;
            TaxNo = string.Empty;
            Street = string.Empty;
            City = string.Empty;
            Country = string.Empty;
            PostalCode = string.Empty;
        }

        protected void ctlVendorChange_OnVendorChanged(object sender, EventArgs e)
        {
            VOVendor vendor = new VOVendor();
            if (!string.IsNullOrEmpty(ctlVendorID.Text))
            {
                vendor.VendorID = UIHelper.ParseLong(ctlVendorID.Text);
            }
            vendor.VendorName = txtVendorName.Text;
            vendor.VendorCode = ctlVendorCode.Text;
            vendor.Street = ctlStreet.Text;
            vendor.City = ctlCity.Text;
            vendor.Country = ctlCountry.Text;
            vendor.PostalCode = ctlPostal.Text;
            vendor.VendorTaxCode = txtTaxNo.Text;

            if (ChangedVendor != null)
                ChangedVendor(this, vendor);
        }

        #region IEditorUserControl Members

        public bool Display
        {
            set 
            {
                if (value)
                {
                    this.txtTaxNo.Style["display"] = "inline-block";
                }
                else
                {
                    this.txtTaxNo.Style["display"] = "none";
                }
            }
        }

        public string Text
        {
            get 
            {
                return this.txtTaxNo.Text;
            }
        }

        #endregion
    }
}
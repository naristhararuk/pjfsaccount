using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using SS.Standard.UI;

using SCG.DB.DTO;
using SCG.DB.BLL;
using SCG.DB.DTO.ValueObject;
using SCG.DB.Query;
using SCG.eAccounting.Web.Helper;


using SS.Standard.Security;
using System.Web.Script.Serialization;




namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class VendorLookUp : BaseUserControl
    {
        #region Properties
        public bool isMultiple { get; set; }
        public bool excludeOneTime { get; set; }
        #endregion

        #region Load Data
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void Show()
        {
            CallOnObjectLookUpCalling();
            string popupURL = "~/UserControls/LOV/SCG.DB/VendorLookup.aspx?isMultiple={0}&excludeOneTime={1}";
            ctlVendorLookupPopupCaller.URL = String.Format(popupURL, isMultiple, excludeOneTime);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), ctlVendorLookupPopupCaller.ClientID + "_popup()", ctlVendorLookupPopupCaller.ClientID + "_popup('" + ctlVendorLookupPopupCaller.ProcessedURL + "')", true);
        }
        public void Hide()
        {
        }
        #endregion

        protected void ctlVendorLookupPopupCaller_NotifyPopupResult(object sender, string action, string value)
        {
            if (action != "ok") return;
            JavaScriptSerializer serialize = new JavaScriptSerializer();
            object returnValue = new object();
            if (!isMultiple)
            {
                VOVendor vendor = serialize.Deserialize<VOVendor>(value);
                returnValue = vendor;
                //NotifyPopUpReturn(new PopUpReturnArgs(PopUpReturnType.OK, vendor));
            }
            else
            {
                IList<VOVendor> list = serialize.Deserialize<IList<VOVendor>>(value);
                returnValue = list;
                //CallOnObjectLookUpReturn(list);
            }
            CallOnObjectLookUpReturn(returnValue);
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;

using AjaxControlToolkit;

using Spring.Context.Support;

using SCG.DB.Query;
using SCG.DB.DTO.ValueObject;
using SCG.DB.DTO;

using SCG.eAccounting.Web.Helper;


namespace SCG.eAccounting.Web.WebService
{
    /// <summary>
    /// Summary description for DivisionAutoComplete
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class VendorAutoComplete : System.Web.Services.WebService
    {
        [WebMethod]
        public string[] GetVendorList(string prefixText, int count, string contextKey)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            // AutoCompleteParameter is a ValueObject DTO that used in AutoComplete only
            DbVendor parameter = serializer.Deserialize<DbVendor>(contextKey);

            // Retreive Query Object from Spring
            IDbVendorQuery dbVendorQuery = (IDbVendorQuery)ContextRegistry.GetContext().GetObject("DbVendorQuery");

            IList<VOVendor> vendorList = dbVendorQuery.FindByVendorAutoComplete(prefixText);

            List<string> items = new List<string>(vendorList.Count);
            foreach (VOVendor vendor in vendorList)
            {
                string item = AutoCompleteExtender.CreateAutoCompleteItem(String.Format("[{0}]{1}", vendor.VendorTaxCode, vendor.VendorName), serializer.Serialize(vendor));
                items.Add(item);
            }

            return items.ToArray();
        }
    }

}

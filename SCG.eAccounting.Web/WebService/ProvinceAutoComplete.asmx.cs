using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;

using AjaxControlToolkit;
using Spring.Context.Support;

using SS.DB.Query;
using SS.DB.DTO.ValueObject;
using SCG.eAccounting.Web.Helper;
using System.Web.Script.Serialization;
using System.Collections.Generic;

namespace SCG.eAccounting.Web.WebService
{
    /// <summary>
    /// Summary description for ProvinceAutoComplete
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ProvinceAutoComplete : System.Web.Services.WebService
    {
        [WebMethod]
        public string[] GetProvinceList(string prefixText, int count, string contextKey)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            // AutoCompleteParameter is a ValueObject DTO that used in AutoComplete only
            DBProvinceAutoCompleteParameter parameter = serializer.Deserialize<DBProvinceAutoCompleteParameter>(contextKey);

            // Retreive Query Object from Spring
            IDbProvinceQuery suProvinceQuery = (IDbProvinceQuery)ContextRegistry.GetContext().GetObject("DbProvinceQuery");

            IList<DBProvinceLovReturn> provinceList = suProvinceQuery.FindByProvinceAutoComplete(prefixText, parameter.LanguageId, parameter.RegionId);

            List<string> items = new List<string>(provinceList.Count);

            foreach (DBProvinceLovReturn province in provinceList)
            {
                string item = AutoCompleteExtender.CreateAutoCompleteItem(province.ProvinceName, serializer.Serialize(province));
                items.Add(item);
            }

            return items.ToArray();
        }

    }
}

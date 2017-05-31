
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
    public class CountryAutoComplete : System.Web.Services.WebService
    {



        [WebMethod]
        public string[] GetCountryList(string prefixText, int count, string contextKey)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            // AutoCompleteParameter is a ValueObject DTO that used in AutoComplete only
            DbCountryAutoCompleteParameter parameter = serializer.Deserialize<DbCountryAutoCompleteParameter>(contextKey);

            // Retreive Query Object from Spring
            IDbCountryLangQuery dbCountryLangQuery = (IDbCountryLangQuery)ContextRegistry.GetContext().GetObject("DbCountryLangQuery");

            IList<CountryLang> CountryList = dbCountryLangQuery.FindAutoComplete(prefixText, parameter.CountryId ?? -1, parameter.LanguageId ?? -1);

            List<string> items = new List<string>(CountryList.Count);

            foreach (CountryLang Country in CountryList)
            {
                string item = AutoCompleteExtender.CreateAutoCompleteItem(Country.CountryName, serializer.Serialize(Country));
                items.Add(item);
            }

            return items.ToArray();
        }
    }

}

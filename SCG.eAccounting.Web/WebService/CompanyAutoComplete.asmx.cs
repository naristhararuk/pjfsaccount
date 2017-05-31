using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using SCG.DB.DTO;
using SCG.DB.Query;
using Spring.Context.Support;
using AjaxControlToolkit;

namespace SCG.eAccounting.Web.WebService
{
    /// <summary>
    /// Summary description for CompanyAutoComplete
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class CompanyAutoComplete : System.Web.Services.WebService
    {

        [WebMethod]
        public string[] GetCompanyList(string prefixText, int count, string contextKey)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            // AutoCompleteParameter is a ValueObject DTO that used in AutoComplete only
            DbCompany parameter = serializer.Deserialize<DbCompany>(contextKey);

            // Retreive Query Object from Spring
            //IDbCountryLangQuery dbCountryLangQuery = (IDbCountryLangQuery)ContextRegistry.GetContext().GetObject("DbCountryLangQuery");
            //IList<CountryLang> CountryList = dbCountryLangQuery.FindAutoComplete(prefixText, parameter.CountryId ?? -1, parameter.LanguageId ?? -1);

            IDbCompanyQuery dbCompanyQuery = (IDbCompanyQuery)ContextRegistry.GetContext().GetObject("DbCompanyQuery");
            IList<DbCompany> companyList = dbCompanyQuery.FindAutoComplete(prefixText, parameter.UseEcc,parameter.Active);

            List<string> items = new List<string>(companyList.Count);

            foreach (DbCompany company in companyList)
            {
                DbCompany com = new DbCompany();
                com.CompanyCode = company.CompanyCode;
                com.CompanyID = company.CompanyID;
                com.CompanyName = company.CompanyName;
                string item = AutoCompleteExtender.CreateAutoCompleteItem(string.Format("[{0}] {1}", company.CompanyCode, company.CompanyName), serializer.Serialize(com));
                items.Add(item);
            }

            return items.ToArray();
        }
    }
}

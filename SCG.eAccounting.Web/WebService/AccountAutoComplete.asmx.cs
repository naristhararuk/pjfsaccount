using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using SCG.DB.DTO.ValueObject;
using SCG.DB.Query;
using Spring.Context.Support;
using SCG.DB.DTO;
using AjaxControlToolkit;

namespace SCG.eAccounting.Web.WebService
{
    /// <summary>
    /// Summary description for AccountAutoComplete
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class AccountAutoComplete : System.Web.Services.WebService
    {

        [WebMethod]
        public string[] GetAccountList(string prefixText, int count, string contextKey)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            // AutoCompleteParameter is a ValueObject DTO that used in AutoComplete only
            AccountAutoCompleteParameter parameter = serializer.Deserialize<AccountAutoCompleteParameter>(contextKey);

            // Retreive Query Object from Spring
            IDbAccountQuery DbAccountQuery = (IDbAccountQuery)ContextRegistry.GetContext().GetObject("DbAccountQuery");

            // Return type of FindAutoComplete method must be a ValueObject only
            IList<AccountLang> list = DbAccountQuery.FindAutoComplete(prefixText, parameter);

            List<string> items = new List<string>(list.Count);

            foreach (AccountLang ac in list)
            {
                // Serialize ValueObject to JSON String and set it to AutoCompleteItem's Value
                string item = AutoCompleteExtender.CreateAutoCompleteItem(String.Format("[{0}]{1}", ac.AccountCode, ac.AccountName), serializer.Serialize(ac));
                items.Add(item);
            }

            return items.ToArray();
        }
    }
}

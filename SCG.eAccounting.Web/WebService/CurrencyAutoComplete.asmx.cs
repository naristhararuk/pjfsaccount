using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using SS.DB.Query;
using Spring.Context.Support;
using SS.DB.DTO.ValueObject;
using AjaxControlToolkit;

namespace SCG.eAccounting.Web.WebService
{
    /// <summary>
    /// Summary description for CurrencyAutoComplete
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class CurrencyAutoComplete : System.Web.Services.WebService
    {

        [WebMethod]
        public string[] GetCurrencyList(string prefixText, int count, string contextKey)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            // AutoCompleteParameter is a ValueObject DTO that used in AutoComplete only
            CurrencyAutoCompleteParameter parameter = serializer.Deserialize<CurrencyAutoCompleteParameter>(contextKey);

            // Retreive Query Object from Spring
            IDbCurrencyQuery dbCurrencyQuery = (IDbCurrencyQuery)ContextRegistry.GetContext().GetObject("DbCurrencyQuery");

            IList<VOUCurrencySetup> currencyList = dbCurrencyQuery.GetCurrencyListItem(prefixText, parameter);

            List<string> items = new List<string>();

            foreach (VOUCurrencySetup c in currencyList)
            {
                CurrencyAutoCompleteParameter currency = new CurrencyAutoCompleteParameter();
                currency.CurrencyID = c.CurrencyID.Value;
                currency.Symbol = c.Symbol;
                currency.Desc = c.Description;
                string item = AutoCompleteExtender.CreateAutoCompleteItem(String.Format("[{0}]-{1}", c.Symbol, c.Description), serializer.Serialize(currency));
                items.Add(item);
            }

            return items.ToArray();
        }
    }
}

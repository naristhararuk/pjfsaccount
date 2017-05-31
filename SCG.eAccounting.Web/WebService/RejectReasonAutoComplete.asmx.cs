
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
    public class RejectReasonAutoComplete : System.Web.Services.WebService
    {
        [WebMethod]
        public string[] GetReasonList(string prefixText, int count, string contextKey)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            // AutoCompleteParameter is a ValueObject DTO that used in AutoComplete only
            DbReasonAutoCompleteParameter parameter = serializer.Deserialize<DbReasonAutoCompleteParameter>(contextKey);

            // Retreive Query Object from Spring
            IDbReasonLangQuery rejectReasonLangQuery = (IDbReasonLangQuery)ContextRegistry.GetContext().GetObject("DbReasonLangQuery");

            IList<VOReasonLang> RejectList = rejectReasonLangQuery.FindAutoComplete(prefixText, parameter.DocumentTypeCode, parameter.LanguageId ?? -1);

            List<string> items = new List<string>(RejectList.Count);

            foreach (VOReasonLang reject in RejectList)
            {
                string item = AutoCompleteExtender.CreateAutoCompleteItem(reject.ReasonDetail, serializer.Serialize(reject));
                items.Add(item);
            }

            return items.ToArray();
        }
    }

}

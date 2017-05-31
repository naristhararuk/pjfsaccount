using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using SS.SU.DTO.ValueObject;
using SCG.DB.Query;
using Spring.Context.Support;
using AjaxControlToolkit;
using SCG.DB.DTO.ValueObject;
using SCG.DB.DTO;

namespace SCG.eAccounting.Web.WebService
{
    /// <summary>
    /// Summary description for IOAutoComplete
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class IOAutoComplete : System.Web.Services.WebService
    {

        [WebMethod]
        public string[] GetIOList(string prefixText, int count, string contextKey)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            // AutoCompleteParameter is a ValueObject DTO that used in AutoComplete only
            IOAutoCompleteParameter parameter = serializer.Deserialize<IOAutoCompleteParameter>(contextKey);

            // Retreive Query Object from Spring
            IDbIOQuery DbIOQuery = (IDbIOQuery)ContextRegistry.GetContext().GetObject("DbIOQuery");

            // Return type of FindAutoComplete method must be a ValueObject only
            IList<DbInternalOrder> ioList = DbIOQuery.FindAutoComplete(prefixText, parameter);

            List<string> items = new List<string>(ioList.Count);

            foreach (DbInternalOrder io in ioList)
            {
                // Serialize ValueObject to JSON String and set it to AutoCompleteItem's Value
                string item = AutoCompleteExtender.CreateAutoCompleteItem(String.Format("[{0}]{1}",io.IONumber,io.IOText), serializer.Serialize(io));
                items.Add(item);
            }

            return items.ToArray();
        }
    }
}

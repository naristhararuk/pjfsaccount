using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;

using AjaxControlToolkit;

using Spring.Context.Support;

using SCG.DB.Query;
using SCG.DB.DTO.ValueObject;
using NHibernate;
using NHibernate.Transform;
using SCG.eAccounting.Web.Helper;
using SCG.DB.DTO;
using System.Collections.Generic;
using System;


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
    public class LocationAutoComplete : System.Web.Services.WebService
    {

        [WebMethod]
        public string[] GetLocationList(string prefixText, int count, string contextKey)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            // AutoCompleteParameter is a ValueObject DTO that used in AutoComplete only
            Location parameter = serializer.Deserialize<Location>(contextKey);

            // Retreive Query Object from Spring
            IDbLocationQuery DbLocationQuery = (IDbLocationQuery)ContextRegistry.GetContext().GetObject("DbLocationQuery");

            // Return type of FindAutoComplete method must be a ValueObject only
            IList<Location> locationList = DbLocationQuery.FindAutoComplete(prefixText, parameter);

            List<string> items = new List<string>(locationList.Count);
            
            foreach (Location location in locationList)
            {
                // Serialize ValueObject to JSON String and set it to AutoCompleteItem's Value
                Location locate = new Location();
                locate.LocationID = location.LocationID;
                locate.LocationName = location.LocationName.Trim();
                locate.LocationCode = location.LocationCode.Trim();

                string item = AutoCompleteExtender.CreateAutoCompleteItem(String.Format("[{0}]{1}", locate.LocationCode, locate.LocationName), serializer.Serialize(locate));
                items.Add(item);
            }

            return items.ToArray();
        }
    }

}

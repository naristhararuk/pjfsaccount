using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;

using AjaxControlToolkit;

using Spring.Context.Support;

using SS.SU.Query;
using SS.SU.DTO.ValueObject;

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
    public class DivisionAutoComplete : System.Web.Services.WebService
    {
        


        [WebMethod]
        public string[] GetDivisionList(string prefixText, int count, string contextKey)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            // AutoCompleteParameter is a ValueObject DTO that used in AutoComplete only
            AutoCompleteParameter parameter = serializer.Deserialize<AutoCompleteParameter>(contextKey);

            // Retreive Query Object from Spring
            ISuDivisionLangQuery suDivisionQuery = (ISuDivisionLangQuery)ContextRegistry.GetContext().GetObject("SuDivisionLangQuery");

            // Return type of FindAutoComplete method must be a ValueObject only
            IList<SS.SU.DTO.ValueObject.DivisionLang> divisionList = suDivisionQuery.FindAutoComplete(prefixText, parameter.LanguageId ?? -1, parameter.OrganizationId ?? -1);
            
            List<string> items = new List<string>(divisionList.Count);

            foreach (SS.SU.DTO.ValueObject.DivisionLang division in divisionList)
            {
                //Serialize ValueObject to JSON String and set it to AutoCompleteItem's Value
                string item = AutoCompleteExtender.CreateAutoCompleteItem(division.DivisionName, serializer.Serialize(division));
                items.Add(item);
            }

            //return lists;
            return items.ToArray();
        }
    }

}

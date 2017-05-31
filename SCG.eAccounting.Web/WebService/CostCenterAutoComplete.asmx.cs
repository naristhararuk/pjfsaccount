
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
    public class CostCenterAutoComplete : System.Web.Services.WebService
    {


        //private string CostCenterReturn;
        [WebMethod]
        public string[] GetCostCenterList(string prefixText, int count, string contextKey)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            // AutoCompleteParameter is a ValueObject DTO that used in AutoComplete only
            SCGAutoCompleteParameter parameter = serializer.Deserialize<SCGAutoCompleteParameter>(contextKey);

            // Retreive Query Object from Spring
            IDbCostCenterQuery dbCostCenterQuery = (IDbCostCenterQuery)ContextRegistry.GetContext().GetObject("DbCostCenterQuery");

            IList<AutocompleteField> CostCenterList = dbCostCenterQuery.FindByDbCostCenterAutoComplete(prefixText, parameter.CompanyID.Value);

            List<string> items = new List<string>(CostCenterList.Count);
            foreach (AutocompleteField CostCenter in CostCenterList)
            {
                string item = AutoCompleteExtender.CreateAutoCompleteItem(String.Format("[{0}]{1}", CostCenter.Code, CostCenter.Description), serializer.Serialize(CostCenter));
                items.Add(item);
            }

            return items.ToArray();
        }
    }

}

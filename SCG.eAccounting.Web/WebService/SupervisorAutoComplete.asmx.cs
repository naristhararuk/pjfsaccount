using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using SS.SU.DTO.ValueObject;
using SS.SU.Query;
using SS.SU.DTO;
using AjaxControlToolkit;
using Spring.Context.Support;

namespace SCG.eAccounting.Web.WebService
{
    /// <summary>
    /// Summary description for SupervisorAutoComplete
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class SupervisorAutoComplete : System.Web.Services.WebService
    {
        [WebMethod]
        public string[] GetUserList(string prefixText, int count, string contextKey)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            // AutoCompleteParameter is a ValueObject DTO that used in AutoComplete only
            UserAutoCompleteParameter parameter = serializer.Deserialize<UserAutoCompleteParameter>(contextKey);

            // Retreive Query Object from Spring
            ISuUserQuery SuUserQuery = (ISuUserQuery)ContextRegistry.GetContext().GetObject("SuUserQuery");

            // Return type of FindAutoComplete method must be a ValueObject only
            IList<SuUser> userList = SuUserQuery.FindAutoComplete(prefixText, parameter);

            List<string> items = new List<string>(userList.Count);
       
            foreach (SuUser user in userList)
            {
                SuUserParameter u = new SuUserParameter();
                u.UserID = user.Userid;
                u.UserName = user.UserName;
                u.EmployeeCode = user.EmployeeCode;
                u.EmployeeName = user.EmployeeName;
                // Serialize ValueObject to JSON String and set it to AutoCompleteItem's Value
                string item = AutoCompleteExtender.CreateAutoCompleteItem(String.Format("[{0}]{1}", user.UserName, user.EmployeeName), serializer.Serialize(u));
                items.Add(item);
            }

            return items.ToArray();
        }
       
    }
}

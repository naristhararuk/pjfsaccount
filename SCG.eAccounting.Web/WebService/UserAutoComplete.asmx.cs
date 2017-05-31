
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
using NHibernate;
using NHibernate.Transform;
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
    public class UserAutoComplete : System.Web.Services.WebService
    {

        public class Getuser
        {
            public string UserName { get; set; }
            public string Name { get; set; }
        }
     
        [WebMethod]
        public string[] GetUserList(string prefixText, int count, string contextKey)
        {
            /*
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            // AutoCompleteParameter is a ValueObject DTO that used in AutoComplete only
            DbUserAutoCompleteParameter parameter = serializer.Deserialize<DbUserAutoCompleteParameter>(contextKey);

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
             */
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Getuser parameter = serializer.Deserialize<Getuser>(contextKey);

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("UserName", typeof(string));
            dt.Columns.Add("Name", typeof(string));

            System.Data.DataRow dr = dt.NewRow();
            dr["UserName"] = "Admin";
            dr["Name"] = "Admin Administrator";
            dt.Rows.Add(dr);
            //parameter.UserName = "Admin";
            //parameter.Name = "Admin Administrator";
            System.Data.DataRow[] getUser  = dt.Select("Name Like '%" + prefixText + "%'");
            IList<Getuser> getUserList = new List<Getuser>();
            foreach (System.Data.DataRow r in getUser)
            {
                Getuser user = new Getuser();
                user.Name = r["Name"].ToString();
                user.UserName = r["UserName"].ToString();
                getUserList.Add(user);
            }
            
            List<string> items = new List<string>(getUserList.Count);

            foreach (Getuser userList in getUserList)
            {
                string item = AutoCompleteExtender.CreateAutoCompleteItem(userList.Name, serializer.Serialize(userList));
                items.Add(item);
            }
            return items.ToArray();

        }
    }

}

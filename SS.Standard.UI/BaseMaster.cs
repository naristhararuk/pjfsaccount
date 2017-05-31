using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SS.Standard.Security;
//using SS.Standard.Utilities;
using System.IO;

//using SS.Standard.Language;
namespace SS.Standard.UI
{
    
    public class BaseMaster : Spring.Web.UI.MasterPage
    {
        public string ApplicationMode { get { return GetApplicationMode(); } }
        public IUserAccount UserAccount { get; set; }
        public IUserEngine UserEngine { get; set; }

        public int Server_Hour = DateTime.Now.Hour;
        public int Server_Minute = DateTime.Now.Minute;
        public int Server_Second = DateTime.Now.Second;
        public int Server_Day = DateTime.Now.Day;
        public int Server_Month = DateTime.Now.Month;
        public string ProgramCode { get; set; }
        public string CompanyName { get; set; }
        private ObjectStateFormatter _formatter = new ObjectStateFormatter();

        public void ChangeLanguage(short languageID)
        {
            UserAccount.SetLanguage(languageID);
        }
        public void SignOut(long userID)
        {
            UserEngine.SignOut(userID);
        }

        private string GetApplicationMode()
        {
            string applicationMode = string.Empty;
            System.Collections.Specialized.NameValueCollection obj = (System.Collections.Specialized.NameValueCollection)System.Configuration.ConfigurationManager.GetSection("appSettings");
            applicationMode = obj.GetValues("ApplicationMode")[0].ToString();

            return applicationMode;
        }
    }
}

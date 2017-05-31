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
using SS.Standard.Utilities;
using System.IO;
using SS.Standard.Language;
namespace SS.Standard.Base.UI
{
    
    public class BaseMaster : System.Web.UI.MasterPage
    {
        public int Server_Hour = DateTime.Now.Hour;
        public int Server_Minute = DateTime.Now.Minute;
        public int Server_Second = DateTime.Now.Second;
        public int Server_Day = DateTime.Now.Day;
        public int Server_Month = DateTime.Now.Month;
        //public int Server_Year = DateTime.Now.Year;
        //public string DefaultCulture = "th-TH";
        protected string ProgramCode;
        protected string CompanyName = ConfigurationManager.AppSettings["CompanyName"];
        protected bool Authentication = true;
        private ObjectStateFormatter _formatter = new ObjectStateFormatter();
        /* use in BasePage
        protected override void OnLoad(EventArgs e)
        {

            if (Authentication && ProgramCode != "" && ProgramCode != null && ProgramCode != "SignIn")
                UserEngine.Permission(UserEngine.ACL.View, ProgramCode, true);

            base.OnLoad(e);
        }
        public void permission()
        {
            if (Authentication && ProgramCode != "" && ProgramCode != null)
                UserEngine.Permission(UserEngine.ACL.View, ProgramCode, true);

        }
         */
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            //if (ProgramCode != "" && ProgramCode != null)
            //    Translation.TranslatePage(ProgramCode, this.Controls);
        }




    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.DB.Query;
using SS.Standard.UI;

namespace SCG.eAccounting.Web
{
    public partial class LoginError : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie cookieFromApp = System.Web.HttpContext.Current.Request.Cookies["expFromApp"];

            if (cookieFromApp != null)
            {
                string applicationName = "e-Xpense";

                if (cookieFromApp.Value.ToString() == "expArchive")
                    applicationName += " Archive";
                else if (cookieFromApp.Value.ToString() == "ecc")
                    applicationName += " 6.0";
                else
                    applicationName += " 4.7";

                Msg.Text = string.Format("Cannot access {0} System", applicationName);
                linkButtonBack.Text = string.Format("Back To {0}", applicationName);
            }
        }

        protected void Back_Click(object sender, EventArgs e)
        {
            HttpCookie cookieFromApp = System.Web.HttpContext.Current.Request.Cookies["expFromApp"];

            string url = string.Empty;
            if (cookieFromApp != null)
            {
                if (cookieFromApp.Value.ToString() == "expArchive")
                    url = ParameterServices.ArchiveUrl; //Response.Redirect(ParameterServices.ArchiveUrl + "/menu.aspx");
                else if (cookieFromApp.Value.ToString() == "ecc")
                    url = ParameterServices.ECCUrl;     //Response.Redirect(ParameterServices.ECCUrl + "/menu.aspx");
                else
                    url = ParameterServices.eXpenseUrl; //Response.Redirect(ParameterServices.eXpenseUrl + "/menu.aspx");

                //clear cookies
                cookieFromApp.Value = string.Empty;
                System.Web.HttpContext.Current.Response.Cookies.Add(cookieFromApp);

                if (!string.IsNullOrEmpty(url))
                    Response.Redirect(url + "/menu.aspx");
            }
        }
    }
}
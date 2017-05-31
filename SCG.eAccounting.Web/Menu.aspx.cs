using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.Security;
using SS.Standard.UI;

using SS.SU.DTO;
using SS.SU.BLL;
using SS.SU.Query;
using SS.DB.Query;

namespace SCG.eAccounting.Web
{
    public partial class Menu : BasePage
    {
        public IMenuEngineService MenuEngineService { get; set; }
       
        protected void Page_Load(object sender, EventArgs e)
        {
                if (UserAccount.Authentication)
                {
                    bindingMenu();
                    this.UserCulture = new System.Globalization.CultureInfo(UserAccount.CurrentLanguageCode);
                    if (ApplicationMode == "Archive")
                    {
                        divHomeSummary.Style["display"] = "none";
                    }
                }
                else
                {
                    string url = HttpContext.Current.Request.Url.ToString();
                    if (ParameterServices.EnableSSLOnLoginPage && HttpContext.Current.Request.Url.Scheme.Equals(Uri.UriSchemeHttp))
                    {
                        url = url.Replace(Uri.UriSchemeHttp, Uri.UriSchemeHttps);
                    }
                    Response.Redirect(url.Replace(HttpContext.Current.Request.RawUrl, ResolveUrl("~/Login.aspx")));
                }
        }
        public void bindingMenu()
        {
           // MenuEngine.CreateMenu(this.Menu1);
        }
    }
}

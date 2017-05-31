using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using SS.SU.DTO;

using SS.SU.BLL;
using SS.Standard.Security;
using SS.Standard.UI;

using SS.SU.Query;
using SS.DB.Query;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class Menu : BaseUserControl
    {

        public IMenuEngine MenuEngine { get; set; }
        public Orientation SetOrientation { get; set; }
        protected override void OnInit(EventArgs e)
        {
            this.Menu1.Orientation = SetOrientation;
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                MenuEngine.SetApplicationMode(ApplicationMode);
                if (UserAccount!=null && UserAccount.Authentication)
                    MenuEngine.CreateMenu(this.Menu1);
            }
            catch
            {
                string url = HttpContext.Current.Request.Url.ToString();
                if (ParameterServices.EnableSSLOnLoginPage && HttpContext.Current.Request.Url.Scheme.Equals(Uri.UriSchemeHttp))
                {
                    url = url.Replace(Uri.UriSchemeHttp, Uri.UriSchemeHttps);
                }
                Response.Redirect(url.Replace(HttpContext.Current.Request.RawUrl, ResolveUrl("~/Login.aspx")));
            }

        }
    }
}
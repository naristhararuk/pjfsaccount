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
using SCG.eAccounting.Web.Helper;
using SS.DB.Query;

namespace SCG.eAccounting.Web
{
    public partial class ProgramsPages : BaseMaster
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ApplicationMode == "Archive")
            {
                Page.Header.Controls.Add(new LiteralControl("<link rel=\"stylesheet\" type=\"text/css\" href=\"" + ResolveUrl("~/App_Themes/Default/defaultArchive.css") + "\" />"));
            }

            if (Session.IsNewSession)
            {
                string url = HttpContext.Current.Request.Url.ToString();
                if (ParameterServices.EnableSSLOnLoginPage && HttpContext.Current.Request.Url.Scheme.Equals(Uri.UriSchemeHttp))
                {
                    url = url.Replace(Uri.UriSchemeHttp, Uri.UriSchemeHttps);
                }
                Response.Redirect(url.Replace(HttpContext.Current.Request.RawUrl, ResolveUrl("~/Login.aspx")));
            }
        }
        public void BodyTagUnload(string EventName, string FunctionName)
        {
            MasterPageBodyTag.Attributes.Add(EventName, FunctionName);
        }
    }
}

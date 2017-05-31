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

namespace SCG.eAccounting.Web
{
    public partial class LoginPage : BaseMaster
    {
        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ApplicationMode == "Archive")
            {
                Page.Header.Controls.Add(new LiteralControl("<link rel=\"stylesheet\" type=\"text/css\" href=\"" + ResolveUrl("~/App_Themes/Default/defaultArchive.css") + "\" />"));
            }
        }
        #endregion protected void Page_Load(object sender, EventArgs e)
    }
}
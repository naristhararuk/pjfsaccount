using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SS.Standard.UI;
using SS.SU.Helper;

using System.Text;
using System.IO;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.Query;

namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs
{
    public partial class DocumentSecurity : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctlLinkHomePage.Text = "Go to Home.";
            ctlLinkHomePage.NavigateUrl = "~/Menu.aspx";

            ctlClickText.Text = "You have <B>NO</B> permission to<br> view this document.";
        }
    }
}

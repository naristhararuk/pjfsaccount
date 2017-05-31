using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Demo
{
    public partial class ShowDoc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.WriteFile(ResolveUrl("~/Documents/year_holiday.pdf"));
        }
    }
}

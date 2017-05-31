using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SCG.eAccounting.Web.Errors
{
    public partial class _404 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Set status code and message; you could also use the HttpStatusCode enum:
            // System.Net.HttpStatusCode.NotFound
            Response.StatusCode = 404;
            //Response.StatusDescription = "Page not found";
        }
    }
}
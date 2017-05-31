using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using SS.Standard.AlertMsg;

namespace SCG.eAccounting.Web.Messages
{
    public partial class ShowMessage : System.Web.UI.Page
    {
        public string MsgID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["MsgID"] != null)
                MsgID = Request.Params["MsgID"].ToString();
        }
    }
}

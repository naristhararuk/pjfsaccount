using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
////using SS.Standard;
using System.Text;

namespace SCG.eAccounting.Web.Messages
{
    public partial class GetMessage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder JSON = new StringBuilder();
            JSON.Append(" var ObjMessage = { ");
            JSON.AppendFormat(" DialogHeader : '{0}'", "Confirm Delete");
            JSON.AppendFormat(" ,DialogTopic : '{0}'", "Confirm Delete");
            JSON.AppendFormat(" ,DialogMsg : '{0}'", "Do you want to delete?");
            JSON.AppendFormat(" ,DialogSolutions : '{0}'", "-");
            JSON.Append(" } ");
          

         //Response.Write(Provider.MessageDAL.GetShowMessage(Request.Params["MsgID"]).ToString());   
            Response.Write(JSON.ToString());   
        }
    }
}

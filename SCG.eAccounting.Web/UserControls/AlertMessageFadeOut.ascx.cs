using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class AlertMessageFadeOut : System.Web.UI.UserControl
    {
        public string Message { get; set; }
        public string TargetControlID { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            ctlMessage.Text = Message;
            AnimateExtender.Enabled = !ctlMessage.Text.Trim().Length.Equals(0);
        }

        protected override void OnUnload(EventArgs e)
        {
            ctlMessage.Text = string.Empty;
        }
    }
}
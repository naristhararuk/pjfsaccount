using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
namespace SCG.eAccounting.Web.UserControls
{
    [ValidationProperty("TimeValue")]
    [ControlValueProperty("TimeValue")]
    public partial class Time : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        public string TimeValue
        {
            get { return txtTime.Text; }
            set { txtTime.Text = value; }
        }
        public bool ReadOnly
        {
            get { return txtTime.ReadOnly; }
            set { txtTime.ReadOnly = value; }
        }
        public string Text
        {
            get { return txtTime.Text; }
        }
        public bool Display
        {
            set
            {
                if (value)
                    txtTime.Style.Add("display", "inline-block");
                else
                    txtTime.Style.Add("display", "none");
            }
        }

        public DateTime? Value
        {
            get
            {
                DateTime calDate = DateTime.MinValue;
                if (!DateTime.TryParse(txtTime.Text, out calDate))
                {
                    txtTime.Text = string.Empty;
                }
                return UIHelper.ParseDate(txtTime.Text, "HH:mm");
            }
            set
            {
                if (value.HasValue)
                {
                    txtTime.Text = UIHelper.ToDateString(value);
                }
                else
                {
                    txtTime.Text = string.Empty;
                }
            }
        } 

    }
}
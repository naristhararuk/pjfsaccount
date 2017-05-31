using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SCG.eAccounting.Web.Helper;

using SS.Standard.UI;

using System.Threading;
using Spring.Globalization.Resolvers;

namespace SCG.eAccounting.Web.UserControls
{
    [ValidationProperty("DateValue")]
    [ControlValueProperty("DateValue")]
    public partial class Calendar : BaseUserControl, IEditorUserControl
    {
        public static SessionCultureResolver UserCulture { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            CalendarExtender1.Format = SCG.eAccounting.Web.Helper.Constant.CalendarDateFormat;
            txtDate.Attributes.Remove("onfocus");
            txtDate.Attributes.Add("onfocus", "DateFormatControlInit()");
        }

        public string DateValue
        {
            get
            {
                if (!this.Value.HasValue)
                    txtDate.Text = string.Empty;

                return txtDate.Text;
            }
            set
            {
                if (value != string.Empty)
                {
                    DateTime? calDate = UIHelper.ParseDate(value);
                    txtDate.Text = (calDate.HasValue) ? UIHelper.ToDateString(calDate) : string.Empty;
                }
                else
                {
                    txtDate.Text = string.Empty;
                }
            }
        }
        public DateTime? Value
        {
            get
            {
                return UIHelper.ParseDate(txtDate.Text);
            }
            set
            {
                if (value.HasValue)
                    txtDate.Text = UIHelper.ToDateString(value);
                else
                    txtDate.Text = string.Empty;
            }
        }

        public bool ReadOnly
        {
            get { return txtDate.ReadOnly; }
            set
            {
                txtDate.ReadOnly = value;
                if (value)
                    imgDate.Style.Add("display", "none");
                else
                    imgDate.Style.Add("display", string.Empty);
            }
        }

        #region IUserControl Members
        public bool Display
        {
            set
            {
                if (value)
                {
                    ctlCalendarTable.Style["display"] = string.Empty;
                }
                else
                {
                    ctlCalendarTable.Style["display"] = "none";
                }
            }
        }
        public string Text
        {
            get { return txtDate.Text; }
        }
        #endregion
    }
}
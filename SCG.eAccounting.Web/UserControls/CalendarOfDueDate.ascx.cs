using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SS.Standard.UI;
using SS.DB.Query;
using SCG.eAccounting.Web.Helper;
using System.Threading;
namespace SCG.eAccounting.Web.UserControls
{
    [ValidationProperty("DateValue")]
    [ControlValueProperty("DateValue")]
    public partial class CalendarOfDueDate : BaseUserControl, IEditorUserControl
    {
        private int dayOfDueDate = ParameterServices.DueDateOfRemittance;
        private string dueDateControl;
        private string requestRemitControl;
        public string DueDateControl
        {
            get { return dueDateControl; }
            set { dueDateControl = value; }
        }
        public string RequestRemitControl 
        {
            get { return requestRemitControl; }
            set { requestRemitControl = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CalendarExtender1.Format = SCG.eAccounting.Web.Helper.Constant.CalendarDateFormat;
                txtDate.Attributes.Remove("onfocus");
                txtDate.Attributes.Add("onfocus", "DateFormatControlInit()");

                TextBox ctlReqDate = ((TextBox)(((Calendar)this.Parent.FindControl(requestRemitControl)).FindControl("txtDate")));
                Label ctlDueDate = ((Label)this.Parent.FindControl(dueDateControl));
                txtDate.Attributes.Remove("onchange");
                txtDate.Attributes.Add("onchange", string.Format("CalculateReqDateOfRemit('{0}','{1}',{2})", ctlReqDate.ClientID, ctlDueDate.ClientID, dayOfDueDate));
                txtDate.AutoPostBack = false;
            }
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
                    imgDate.Style.Add("display", "");
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

        public string dueDate
        {
            get { return ctlDueDate.Text; }
        }

        public delegate void NotifyDateChangedHandler(object sender, string returnValue);
        public event NotifyDateChangedHandler NotifyDateChanged;

        protected void ctlReturnAction_ValueChanged(object sender, EventArgs e)
        {
            string s = dueDate;
            if (NotifyDateChanged != null)
                NotifyDateChanged(sender, ctlReturnValue.Value);
        }
    }
}
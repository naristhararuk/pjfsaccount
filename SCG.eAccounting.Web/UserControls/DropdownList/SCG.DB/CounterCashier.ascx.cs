using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.DB.Query;
using SCG.DB.DTO.ValueObject;
using SS.Standard.Utilities;

namespace SCG.eAccounting.Web.UserControls.DropdownList.SCG.DB
{
    public partial class CounterCashier : BaseUserControl, IEditorUserControl
    {
        public void ShowDefault()
        {
            ctlCounterCashierDropdown.SelectedIndex = -1;
        }
        public long? GetCashierId()
        {
            if (ctlCounterCashierDropdown.SelectedIndex != -1)
            {
                return  Utilities.ParseLong(ctlCounterCashierDropdown.SelectedValue);
            }
            else
            {
                return null;
            }
        }
        public string SelectedValue
        {
            set { ctlCounterCashierDropdown.SelectedValue = value; }
        }
        public long? PBID
        {
            get { return GetCashierId(); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void BindCounterCashierDropdown(long companyID)
        {
            IList<PaymentTypeListItem> list = this.BindDropdown(companyID);

            if (list.Count > 1)
            {
                ctlCounterCashierDropdown.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), "-1"));
            }
        }
        public void SetValue(long companyID, long pbID)
        {
            this.BindDropdown(companyID);

            ctlCounterCashierDropdown.SelectedValue = pbID.ToString();
        }
        private IList<PaymentTypeListItem> BindDropdown(long companyID)
        {
            IList<PaymentTypeListItem> list = ScgDbQueryProvider.DbPBQuery.GetPbListItem(companyID, UserAccount.CurrentLanguageID);

            ctlCounterCashierDropdown.DataSource = list;
            ctlCounterCashierDropdown.DataTextField = "Text";
            ctlCounterCashierDropdown.DataValueField = "ID";
            ctlCounterCashierDropdown.DataBind();

            return list;
        }
        public bool Display
        {
            set
            {
                if (value)
                    ctlContanerTable.Style.Add("display", "inline-block");
                else
                    ctlContanerTable.Style.Add("display", "none");
            }
        }
        public string Text
        {
            get
            {
                if (ctlCounterCashierDropdown.SelectedItem != null)
                    return ctlCounterCashierDropdown.SelectedItem.Text;
                return string.Empty;
            }
        }
    }
}
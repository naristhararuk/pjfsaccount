using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.DB.Query;
using SCG.DB.DTO;

namespace SCG.eAccounting.Web.UserControls.DropdownList.SCG.DB
{
    public partial class PaymentMethod : BaseUserControl, IEditorUserControl
    {
        public string SelectedValue
        {
            get { return ctlPaymentMethodDropdown.SelectedValue; }
            set { this.ctlPaymentMethodDropdown.SelectedValue = value; }
        }
        public string Text
        {
            get { return ctlPaymentMethodDropdown.SelectedItem.Text; }
        }
        public bool Display
        {
            set
            {
                if (value)
                    ctlPaymentMethodDropdown.Style.Add("display", "inline-block");
                else
                    ctlPaymentMethodDropdown.Style.Add("display", "none");
            }
        }

        public short ComID
        {
            set 
            {
                ViewState["ComID"] = ComID;
            }
            get 
            {
                if (ViewState["ComID"] != null)
                    return (short)ViewState["ComID"];
                else
                    return 0;

            }
        }

        public Unit Width
        {
            set
            {
                ctlPaymentMethodDropdown.Width = value;
            }
            get
            {
                return ctlPaymentMethodDropdown.Width;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {



        }
        public void PaymentMethodBind()
        {
            //ctlPaymentMethodDropdown.DataSource = ScgDbQueryProvider.DbPaymentMethodQuery.FindAll();
            ctlPaymentMethodDropdown.DataSource = ScgDbQueryProvider.DbPaymentMethodQuery.FindPaymentMethodActive();
            ctlPaymentMethodDropdown.DataTextField = "PaymentMethodName";
            ctlPaymentMethodDropdown.DataValueField = "PaymentMethodID";
            ctlPaymentMethodDropdown.DataBind();
        }
        public void PaymentMethodBind(short ComID)
        {
            //ctlPaymentMethodDropdown.DataSource = ScgDbQueryProvider.DbPaymentMethodQuery.FindAll();
            IList<DbPaymentMethod> dbPaymentMethodList = ScgDbQueryProvider.DbPaymentMethodQuery.FindPaymentMethodActive(ComID);
            if (dbPaymentMethodList.Count == 0)
            {
                PaymentMethodBind();
            }
            else
            {
                ctlPaymentMethodDropdown.DataSource = dbPaymentMethodList;
            ctlPaymentMethodDropdown.DataTextField = "PaymentMethodName";
            ctlPaymentMethodDropdown.DataValueField = "PaymentMethodID";
            ctlPaymentMethodDropdown.DataBind();
        }
            
    }
    }
}
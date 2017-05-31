using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCG.DB.Query;
using SS.Standard.UI;
using SS.DB.Query;

namespace SCG.eAccounting.Web.UserControls.DropdownList.SCG.DB
{
    public partial class WHTRateDropdown : BaseUserControl, IEditorUserControl
    {
        public string Text
        {
            get
            {
                if (ctlWHTRateDropdown.SelectedItem != null)
                    return ctlWHTRateDropdown.SelectedItem.Text;
                return string.Empty;
            }
        }
        public bool Display
        {
            set
            {
                if (value)
                    ctlWHTRateDropdown.Style.Add("display", "inline-block");
                else
                    ctlWHTRateDropdown.Style.Add("display", "none");
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public string SelectedValue
        {
            get { return ctlWHTRateDropdown.SelectedValue; }
            set { this.ctlWHTRateDropdown.SelectedValue = value; }
        }

        public void BindDropDown()
        {
            ctlWHTRateDropdown.DataSource = ScgDbQueryProvider.DbWithHoldingTaxQuery.FindAllWHTRateActive();
            ctlWHTRateDropdown.DataTextField = "Text";
            ctlWHTRateDropdown.DataValueField = "ID";
            ctlWHTRateDropdown.DataBind();
        }
    }
}
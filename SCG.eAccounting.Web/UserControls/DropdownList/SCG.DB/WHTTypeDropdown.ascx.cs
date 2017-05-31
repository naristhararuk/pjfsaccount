using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCG.DB.Query;
using SS.Standard.UI;
using SCG.DB.DTO.ValueObject;

namespace SCG.eAccounting.Web.UserControls.DropdownList.SCG.DB
{
    public partial class WHTTypeDropdown : BaseUserControl, IEditorUserControl
    {
        public string Text
        {
            get
            {
                if (ctlWHTTypeDropdown.SelectedItem != null)
                    return ctlWHTTypeDropdown.SelectedItem.Text;
                return string.Empty;
            }
        }
        public bool Display
        {
            set
            {
                if (value)
                    ctlWHTTypeDropdown.Style.Add("display", "inline-block");
                else
                    ctlWHTTypeDropdown.Style.Add("display", "none");
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public string SelectedValue
        {
            get { return ctlWHTTypeDropdown.SelectedValue; }
            set 
            {
                if (value != "0")
                    this.ctlWHTTypeDropdown.SelectedValue = value;
                else
                    this.ctlWHTTypeDropdown.SelectedIndex = 0;
            }
        }

        public void BindDropDown()
        {
            IList<VOWHTType> types = ScgDbQueryProvider.DbWithHoldingTaxTypeQuery.FindAllWHTTypeActive();
            ctlWHTTypeDropdown.DataSource = types;
            ctlWHTTypeDropdown.DataTextField = "Text";
            ctlWHTTypeDropdown.DataValueField = "ID";
            ctlWHTTypeDropdown.DataBind();
            
        }
    }
}
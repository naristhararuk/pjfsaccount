using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCG.DB.Query;
using SS.Standard.Security;
using SS.Standard.UI;

namespace SCG.eAccounting.Web.UserControls.DropdownList.SCG.DB
{
    public partial class StatusDropdown : BaseUserControl, IEditorUserControl
    {
        public string SelectedValue
        {
            get { return ctlStatusDropdown.SelectedValue; }
            set { this.ctlStatusDropdown.SelectedValue = value; }
        }

        public string GroupType
        {
            get { return ctlGroupType.Text; }
            set { this.ctlGroupType.Text = value; }
        }
        public bool Enable
        {
            set { ctlStatusDropdown.Enabled = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        public void StatusBind()
        {
            ctlStatusDropdown.DataSource = ScgDbQueryProvider.SCGDbStatusLangQuery.FindStatusLangCriteria(ctlGroupType.Text,UserAccount.CurrentLanguageID);
            ctlStatusDropdown.DataTextField = "strSymbol";
            ctlStatusDropdown.DataValueField = "strID";
            ctlStatusDropdown.DataBind();
        }

        public void SetDropdown(string strValue)
        {
            ctlStatusDropdown.SelectedValue = strValue;
        }



        #region IEditorUserControl Members
        public bool Display
        {
            set
            {
                if (value)
                    ctlStatusDropdown.Style.Add("display", "inline-block");
                else
                    ctlStatusDropdown.Style.Add("display", "none");
            }
        }
        public string Text
        {
            get {
                if (ctlStatusDropdown.SelectedItem != null)
                {

                    return ctlStatusDropdown.SelectedItem.Text;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        #endregion
    }
}
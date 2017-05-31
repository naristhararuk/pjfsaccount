using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCG.DB.Query;
using SCG.DB.DTO;
using SS.DB.DTO.ValueObject;
using SS.Standard.UI;


namespace SCG.eAccounting.Web.UserControls.DropdownList.SCG.DB
{
    public partial class ServiceTeam : BaseUserControl, IEditorUserControl
    {

        public string Text
        {
            get
            {
                if (ctlServiceTeamDropdown.SelectedItem != null)
                    return ctlServiceTeamDropdown.SelectedItem.Text;
                return string.Empty;
            }
        }
        public bool Display
        {
            set
            {
                if (value)
                    ctlServiceTeamDropdown.Style.Add("display", "inline-block");
                else
                    ctlServiceTeamDropdown.Style.Add("display", "none");
            }
        }
        public string SelectedValue
        {
            get { return ctlServiceTeamDropdown.SelectedValue; }
            set { this.ctlServiceTeamDropdown.SelectedValue = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void ServiceTeamDataBind()
        {
            IList<TranslatedListItem> translatedList = ScgDbQueryProvider.DbServiceTeamQuery.GetAllServiceTeamListItem();
            ctlServiceTeamDropdown.DataSource = translatedList;
            ctlServiceTeamDropdown.DataTextField = "strSymbol";
            ctlServiceTeamDropdown.DataValueField = "strID";
            ctlServiceTeamDropdown.DataBind();
        }

        public void ServiceTeamDataBind(short RoleID)
        {
            IList<TranslatedListItem> translatedList = ScgDbQueryProvider.DbServiceTeamQuery.GetServiceTeamListItemByUserID(RoleID);
            ctlServiceTeamDropdown.DataSource = translatedList;
            ctlServiceTeamDropdown.DataTextField = "strSymbol";
            ctlServiceTeamDropdown.DataValueField = "strID";
            ctlServiceTeamDropdown.DataBind();
        }

        public void ServiceTeamDataBind(long userID, IList<short> userRole)
        {
            IList<TranslatedListItem> translatedList = ScgDbQueryProvider.DbServiceTeamQuery.GetServiceTeamListItemByUserID(userID, userRole);
            ctlServiceTeamDropdown.DataSource = translatedList;
            ctlServiceTeamDropdown.DataTextField = "strSymbol";
            ctlServiceTeamDropdown.DataValueField = "strID";
            ctlServiceTeamDropdown.DataBind();

            ctlServiceTeamDropdown.Items.Insert(0, new ListItem(GetMessage("All_Item"), string.Empty));
        }
    }

}
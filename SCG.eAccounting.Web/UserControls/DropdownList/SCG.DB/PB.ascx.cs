using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCG.DB.Query;
using SCG.DB.DTO;
using SS.Standard.UI;
using SS.DB.DTO.ValueObject;
using SS.SU.DTO;
using SS.SU.Query;



namespace SCG.eAccounting.Web.UserControls.DropdownList.SCG.DB
{
    public partial class PB : BaseUserControl
    {
        public string SelectedValue
        {
            get { return ctlPBDropdown .SelectedValue; }
            set { this.ctlPBDropdown.SelectedValue = value; }
        }

        public void Show()
        { 
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Page.IsPostBack) PBNameBind();


        }
     
        public void PBRoleNameBind(short RoleID)
        {
            DbpbLang pbLang = new DbpbLang();
            pbLang.LanguageID = new SS.DB.DTO.DbLanguage();
            pbLang.LanguageID.Languageid = UserAccount.UserLanguageID;
            IList<SS.DB.DTO.ValueObject.TranslatedListItem> trnslatedList = ScgDbQueryProvider.DbPbLangQuery.GetPBLangByCriteria(pbLang,RoleID);
            ctlPBDropdown.DataSource = trnslatedList;
            ctlPBDropdown.DataTextField = "IDSymbol";
            ctlPBDropdown.DataValueField = "strID";
            ctlPBDropdown.DataBind();
        }

        public void PBRoleNameBind(short languageID, long UserID, IList<short> userRole)
        {
            IList<SS.DB.DTO.ValueObject.TranslatedListItem> trnslatedList = ScgDbQueryProvider.DbPbLangQuery.GetPBLangByCriteria(languageID, UserID, userRole);
            ctlPBDropdown.DataSource = trnslatedList;
            ctlPBDropdown.DataTextField = "IDSymbol";
            ctlPBDropdown.DataValueField = "strID";
            ctlPBDropdown.DataBind();

            ctlPBDropdown.Items.Insert(0, new ListItem(GetMessage("All_Item"), string.Empty));
        }
    }
}
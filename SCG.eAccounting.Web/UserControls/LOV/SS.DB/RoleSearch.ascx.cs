using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using SS.Standard.UI;
using SS.Standard.Security;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.SU.BLL;
using SS.SU.Query;

using SCG.eAccounting.Web.Helper;

using SS.DB.Query;
using SS.DB.DTO;
using SS.DB.BLL;

namespace SCG.eAccounting.Web.UserControls.LOV.SS.DB
{
	public partial class RoleSearch : BaseUserControl
	{
		#region Service Property
		public ISuRoleService SuRoleService { get; set; }
		public IDbLanguageService DbLanguageService { get; set; }
		#endregion
		
		#region Page_Load Event
		protected void Page_Load(object sender, EventArgs e)
		{
            string popupURL = "~/UserControls/LOV/SS.DB/RoleSearchPopup.aspx?UserID={0}&RoleName={1}&LanguageId{2}";
            ctlRoleSearchLookupPopupCaller.URL = string.Format(popupURL, new object[] { UserID, RoleName, LanguageId });

		}
		#endregion	
	
        public void Show()
        {
            CallOnObjectLookUpCalling();
            string popupURL = "~/UserControls/LOV/SS.DB/RoleSearchPopup.aspx?UserID={0}&RoleName={1}&LanguageId{2}";
            ctlRoleSearchLookupPopupCaller.URL = string.Format(popupURL, new object[] { UserID, RoleName, LanguageId });
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), ctlRoleSearchLookupPopupCaller.ClientID + "_popup()", ctlRoleSearchLookupPopupCaller.ClientID + "_popup('" + ctlRoleSearchLookupPopupCaller.ProcessedURL + "')", true);
        }
        public string UserID { get; set; }
        public string RoleName { get; set; }
        public string LanguageId { get; set; }

        protected void ctlRoleSearchLookupPopupCaller_NotifyPopupResult(object sender, string action, string value)
        {
            if (action != "ok") return;

            object returnValue = new object();

            string[] listID = value.Split('|');
            IList<SuRole> roleList = new List<SuRole>();
            foreach (string id in listID)
            {
                SuRole role = SuRoleService.FindByIdentity(UIHelper.ParseShort(id));
                if (role != null)
                {
                    roleList.Add(role);
                }
            }
            returnValue = roleList;
            CallOnObjectLookUpReturn(returnValue);
        }
	}
}
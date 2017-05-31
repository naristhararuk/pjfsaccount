using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SS.SU.DTO;
using SS.SU.Query;
using System.Text;
using SCG.eAccounting.Web.Helper;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class UserGroupLookup : BaseUserControl
    {
        // If Single mode,Send empty string to Show() function and Lookup has return Object SuRole
        // else if Multiple mode,Send "Multiple" to Show() function ext. Show("Multiple")  and Lookup return IList<SuRole>

        protected void Page_Load(object sender, EventArgs e)
        {
            string popupURL = "~/UserControls/LOV/SCG.DB/UserGroupLookUp.aspx?IsMultiple={0}&UserGroupCode={1}&UserGroupName={2}";
            ctlUserGroupLookupPopupCaller.URL = string.Format(popupURL, new object[] { IsMultiple, UserGroupCode, UserGroupName });
        }
        public void Show(bool isMultiple)
        {
            CallOnObjectLookUpCalling();
            string popupURL = "~/UserControls/LOV/SCG.DB/UserGroupLookUp.aspx?IsMultiple={0}&UserGroupCode={1}&UserGroupName={2}";
            ctlUserGroupLookupPopupCaller.URL = string.Format(popupURL, new object[] { isMultiple, UserGroupCode, UserGroupName });
            ctlUserGroupLookupPopupCaller.ReferenceValue = IsMultiple.ToString();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), ctlUserGroupLookupPopupCaller.ClientID + "_popup()", ctlUserGroupLookupPopupCaller.ClientID + "_popup('" + ctlUserGroupLookupPopupCaller.ProcessedURL + "')", true);
        }
        public bool IsMultiple { get; set; }
        public string UserGroupCode { get; set; }
        public string UserGroupName { get; set; }

        protected void ctlUserGroupLookupPopupCaller_NotifyPopupResult(object sender, string action, string value)
        {
            if (action != "ok") return;

            object returnValue = new object();

            if (!IsMultiple)
            {
                SuRole userGroup = QueryProvider.SuRoleQuery.FindByIdentity(UIHelper.ParseShort(value));
                returnValue = userGroup;
                NotifyPopUpReturn(new PopUpReturnArgs(PopUpReturnType.OK, returnValue));
            }
            else
            {
                string[] listID = value.Split('|');
                IList<SuRole> userGroupList = new List<SuRole>();
                foreach (string id in listID)
                {
                    SuRole userGroup = QueryProvider.SuRoleQuery.FindByIdentity(UIHelper.ParseShort(id));
                    if (userGroup != null)
                        userGroupList.Add(userGroup);
                }
                returnValue = userGroupList;
                CallOnObjectLookUpReturn(returnValue);
            }
        }
    }
}
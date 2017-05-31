using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using System.Text;
using SS.SU.DTO;
using SS.SU.Query;
using SS.SU.DTO.ValueObject;
using System.Web.Script.Serialization;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class UserProfileLookup : BaseUserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //string popupURL = "~/UserControls/LOV/SCG.DB/UserProfileLookup.aspx?isMultiple={0}&UserIdNotIn={1}&SearchFavoriteApprover={2}&RequesterID={3}&SearchApprovalFlag={4}";
            //ctlUserProfileLookupPopupCaller.URL = string.Format(popupURL, new object[] { isMultiple, UserIdNotIn, SearchFavoriteApprover, RequesterID, SearchApprovalFlag });

            //}
        }


        public void Show()
        {
            CallOnObjectLookUpCalling();
            string popupURL = "~/UserControls/LOV/SCG.DB/UserProfileLookup.aspx?isMultiple={0}&UserIdNotIn={1}&SearchFavoriteApprover={2}&RequesterID={3}&SearchApprovalFlag={4}";
            ctlUserProfileLookupPopupCaller.URL = string.Format(popupURL, new object[] { isMultiple, UserIdNotIn, SearchFavoriteApprover, RequesterID, SearchApprovalFlag });

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), ctlUserProfileLookupPopupCaller.ClientID + "_popup()", ctlUserProfileLookupPopupCaller.ClientID + "_popup('" + ctlUserProfileLookupPopupCaller.ProcessedURL + "')", true);
        }

        public void Hide()
        {
        }

        public bool isMultiple
        {
            get
            {
                return ViewState["IsMultipleReturn"] == null ? false : Convert.ToBoolean(ViewState["IsMultipleReturn"].ToString());
            }
            set { ViewState["IsMultipleReturn"] = value; }
        }
        public bool SearchApprovalFlag { get; set; }
        public string UserIdNotIn { get; set; }
        public bool SearchFavoriteApprover { get; set; }
        public long? RequesterID { get; set; }


        protected void ctlUserProfileLookupPopupCaller_NotifyPopupResult(object sender, string action, string value)
        {
            if (action != "ok") return;

            if (!isMultiple)
            {
                SuUser user = QueryProvider.SuUserQuery.FindByIdentity(UIHelper.ParseLong(value));
                NotifyPopUpReturn(new PopUpReturnArgs(PopUpReturnType.OK, user));
            }
            else
            {
                string[] listID = value.Split('|');
                IList<SuUser> list = new List<SuUser>();
                foreach (string id in listID)
                {
                    SuUser user = QueryProvider.SuUserQuery.FindByIdentity(UIHelper.ParseLong(id));
                    if (user != null)
                        list.Add(user);
                }
                CallOnObjectLookUpReturn(list);
            }
        }

    }
}
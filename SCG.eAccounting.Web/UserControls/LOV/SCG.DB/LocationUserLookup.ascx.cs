using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.DB.Query;
using SCG.eAccounting.Web.Helper;
using SCG.DB.DTO;
using System.Text;
using SCG.DB.DTO.ValueObject;


namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class LocationUserLookup : BaseUserControl
    {
        public bool isMultiple { get; set; }
        public long? CompanyId
        {
            get
            {
                if (string.IsNullOrEmpty(ctlCompanyId.Value))
                    return null;
                else return UIHelper.ParseLong(ctlCompanyId.Value);
            }
            set
            {
                if (value.HasValue)
                    ctlCompanyId.Value = value.ToString();
                else
                    ctlCompanyId.Value = null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //string popupURL = "~/UserControls/LOV/SCG.DB/LocationUserLookUp.aspx?isMultiple={0}";
            //ctlLocationUserLookupPopupCaller.URL = string.Format(popupURL, new object[] { isMultiple });
        }

        public void Show()
        {
            CallOnObjectLookUpCalling();
            string popupURL = "~/UserControls/LOV/SCG.DB/LocationUserLookUp.aspx?isMultiple={0}&companyId={1}";
            ctlLocationUserLookupPopupCaller.URL = string.Format(popupURL, new object[] { isMultiple, CompanyId });
            ctlLocationUserLookupPopupCaller.ReferenceValue = isMultiple.ToString();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), ctlLocationUserLookupPopupCaller.ClientID + "_popup()", ctlLocationUserLookupPopupCaller.ClientID + "_popup('" + ctlLocationUserLookupPopupCaller.ProcessedURL + "')", true);
        }

        protected void ctlLocationUserLookupPopupCaller_NotifyPopupResult(object sender, string action, string value)
        {
            if (action != "ok") return;

            object returnValue = new object();

            if (!isMultiple)
            {
                DbLocation location = ScgDbQueryProvider.DbLocationQuery.FindByIdentity(UIHelper.ParseLong(value));
                returnValue = location;
                NotifyPopUpReturn(new PopUpReturnArgs(PopUpReturnType.OK, returnValue));
            }
            else
            {
                string[] listID = value.Split('|');
                IList<DbLocation> list = new List<DbLocation>();
                foreach (string id in listID)
                {
                    DbLocation location = ScgDbQueryProvider.DbLocationQuery.FindByIdentity(UIHelper.ParseLong(id));
                    if (location != null)
                        list.Add(location);
                }
                returnValue = list;
                CallOnObjectLookUpReturn(returnValue);
            }
        }
        
    }
}
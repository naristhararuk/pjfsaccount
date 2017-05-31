using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using SS.Standard.UI;
using SS.Standard.Security;
using SCG.eAccounting.Web.Helper;
using SCG.DB.Query;
using SCG.DB.DTO;


namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class CompanyLookUp : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string popupURL = "~/UserControls/LOV/SCG.DB/CompanyLookUp.aspx?IsMultiple={0}&CompanyCode={1}&CompanyName={2}&UseEccOnly={3}&FlagActive={4}";
            ctlCompanyLookupPopupCaller.URL = string.Format(popupURL, new object[] { isMultiple, CompanyCode, CompanyName, UseEccOnly,FlagActive });

        }

        public void Show()
        {
            CallOnObjectLookUpCalling();
            string popupURL = "~/UserControls/LOV/SCG.DB/CompanyLookUp.aspx?isMultiple={0}&CompanyCode={1}&CompanyName={2}&UseEccOnly={3}&FlagActive={4}";
            ctlCompanyLookupPopupCaller.URL = string.Format(popupURL, new object[] { isMultiple, CompanyCode, CompanyName, UseEccOnly, FlagActive });
            ctlCompanyLookupPopupCaller.ReferenceValue = isMultiple.ToString();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), ctlCompanyLookupPopupCaller.ClientID + "_popup()", ctlCompanyLookupPopupCaller.ClientID + "_popup('" + ctlCompanyLookupPopupCaller.ProcessedURL + "')", true);
        }

        public bool isMultiple { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public bool UseEccOnly { get; set; }
        public bool? FlagActive { get; set; }

        protected void ctlCompanyLookupPopupCaller_NotifyPopupResult(object sender, string action, string value)
        {
            if (action != "ok") return;

            object returnValue = new object();

            if (!isMultiple)
            {
                DbCompany company = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(UIHelper.ParseLong(value));
                returnValue = company;
                NotifyPopUpReturn(new PopUpReturnArgs(PopUpReturnType.OK, returnValue));
            }
            else
            {
                string[] listID = value.Split('|');
                IList<DbCompany> companyList = new List<DbCompany>();
                foreach (string id in listID)
                {
                    DbCompany company = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(UIHelper.ParseLong(id));
                    if (company != null)
                        companyList.Add(company);
                }
                returnValue = companyList;
                CallOnObjectLookUpReturn(returnValue);
            }
        }
    }
}
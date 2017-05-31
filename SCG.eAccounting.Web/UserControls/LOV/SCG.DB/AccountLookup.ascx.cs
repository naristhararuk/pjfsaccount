using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.DB.BLL;
using SCG.DB.Query;
using SCG.DB.DTO.ValueObject;
using SCG.eAccounting.Web.Helper;
using SCG.DB.DTO;
using System.Text;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class AccountLookup : BaseUserControl
    {
        #region Property
        public bool isMultiple { get; set; }
        //for query in lookup where company id of document
        public long? CompanyIDofDocument { get; set; }
        public string WithoutExpenseCode { get; set; }

        #endregion Property

        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #endregion protected void Page_Load(object sender, EventArgs e)

        #region public void Show()
        public void Show()
        {
            CallOnObjectLookUpCalling();
            string popupURL = "~/UserControls/LOV/SCG.DB/ExpenseCodeLookup.aspx?isMultiple={0}&companyID={1}&withoutExpenseCode={2}";
            ctlExpenseLookupPopupCaller.URL = string.Format(popupURL, new object[] { isMultiple, CompanyIDofDocument, WithoutExpenseCode });

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), ctlExpenseLookupPopupCaller.ClientID + "_popup()", ctlExpenseLookupPopupCaller.ClientID + "_popup('" + ctlExpenseLookupPopupCaller.ProcessedURL + "')", true);
        }
        #endregion public void Show()

        #region public void Hide()
        public void Hide()
        {
        }
        #endregion public void Hide()

        protected void ctlUserProfileLookupPopupCaller_NotifyPopupResult(object sender, string action, string value)
        {
            if (action != "ok") return;

            if (!isMultiple)
            {
                IList<AccountLang> accountLang = ScgDbQueryProvider.DbAccountLangQuery.FindByDbAccountLangKey(UIHelper.ParseLong(value), UserAccount.CurrentLanguageID);
                if (accountLang.Count > 0)
                    NotifyPopUpReturn(new PopUpReturnArgs(PopUpReturnType.OK, accountLang[0]));
            }
            else
            {
                string[] listID = value.Split('|');
                IList<AccountLang> list = new List<AccountLang>();
                foreach (string id in listID)
                {
                    IList<AccountLang> accountLang = ScgDbQueryProvider.DbAccountLangQuery.FindByDbAccountLangKey(UIHelper.ParseLong(value), UserAccount.CurrentLanguageID);
                    if (accountLang.Count > 0)
                        list.Add(accountLang[0]);
                }
                CallOnObjectLookUpReturn(list);
            }
        }
    }
}
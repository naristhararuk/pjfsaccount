using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.DB.DTO.ValueObject;
using SCG.DB.Query;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class AccountLabelLookup : BaseUserControl
    {
        public long AccountID
        {
            get
            {
                if (ViewState["AccountID"] != null)
                    return (long)ViewState["AccountID"];
                return 0;
            }
            set
            {
                ViewState["AccountID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctlExpenseLookup.OnObjectLookUpCalling += new BaseUserControl.ObjectLookUpCallingEventHandler(ctlExpenseLookup_OnObjectLookUpCalling);
            ctlExpenseLookup.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlExpenseLookup_OnObjectLookUpReturn);

        }
        protected void ctlExpenseLookup_OnObjectLookUpCalling(object sender, ObjectLookUpCallingEventArgs e)
        {
            //UserControls.LOV.SCG.DB.CostCenterLookUp CostCenterSearch = sender as UserControls.LOV.SCG.DB.CostCenterLookUp;
        }
        protected void ctlExpenseLookup_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            AccountLang accountInfo = (AccountLang)e.ObjectReturn;
            AccountID = accountInfo.AccountID;
            ctlExpenseCode.Text = String.Format("{0}-{1}", accountInfo.AccountCode, accountInfo.AccountName);
            ctlUpdatePanelAccountSimple.Update();
        }
        protected void ctlSearchExpenseCode_Click(object sender, ImageClickEventArgs e)
        {
            ctlExpenseLookup.Show();
        }
        public void SetAccountValue(long accId)
        {
            if (accId > 0)
            {
                AccountID = accId;
                IList<AccountLang> acc = ScgDbQueryProvider.DbAccountLangQuery.FindByDbAccountLangKey(accId, UserAccount.CurrentLanguageID);
                if (acc.Count > 0)
                {
                    AccountLang accountInfo = acc.First<AccountLang>();
                    ctlExpenseCode.Text = String.Format("{0}-{1}", accountInfo.AccountCode, accountInfo.AccountName);
                }
            }
            ctlUpdatePanelAccountSimple.Update();
        }
    }
}
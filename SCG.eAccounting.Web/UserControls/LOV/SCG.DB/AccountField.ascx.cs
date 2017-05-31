using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.DB.DTO;
using SCG.DB.Query;
using SCG.eAccounting.Web.Helper;
using SCG.DB.DTO.ValueObject;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class AccountField : BaseUserControl, IEditorUserControl
    {
        #region Property
        public string AccountID
        {
            get { return ctlAccountID.Text; }
            set { ctlAccountID.Text = value; }
        }
        public string Text
        {
            get
            {
                if (string.IsNullOrEmpty(ctlAccountTextBoxAutoComplete.AccountCode) || string.IsNullOrEmpty(ctlAccountName.Text))
                {
                    return string.Empty;
                }
                else
                {
                    return ctlAccountTextBoxAutoComplete.AccountCode + '-' + ctlAccountName.Text;
                }
            }
        }
        public bool Display
        {
            set
            {
                if (value)
                    ctlContainer.Style.Add("display", "inline-block");
                else
                    ctlContainer.Style.Add("display", "none");
            }
        }
        
        public bool DisplayCaption
        {
            set
            {
                if (value)
                    ctlAccountName.Style.Add("display", "inline-block");
                else
                    ctlAccountName.Style.Add("display", "none");
            }
        }
        //for query in lookup where company id of document
        public long? CompanyIDofDocument
        {
            get
            {
                if (ViewState["CompanyIDofDocument"] != null)
                {
                    return UIHelper.ParseLong(ViewState["CompanyIDofDocument"].ToString());
                }
                else
                {
                    return null;
                }
            }
            set
            {
                ViewState["CompanyIDofDocument"] = value;
                ctlAccountTextBoxAutoComplete.CompanyID = value;
            }
        }
        public string WithoutExpenseCode
        {
            get
            {
                if (ViewState["WithoutExpenseCode"] != null)
                    return ViewState["WithoutExpenseCode"].ToString();
                return string.Empty;
            }
            set
            {
                ViewState["WithoutExpenseCode"] = value;
                ctlAccountTextBoxAutoComplete.WithoutExpenseCode = value;
            }
        }
        #endregion Property

        protected void Page_Load(object sender, EventArgs e)
        {
            BaseUserControl ctl = GetCurrentPopUpControl();
            if (ctl is AccountLookup)
            {
                ctl.OnPopUpReturn += new PopUpReturnEventHandler(ctlAccountLookup_OnNotifyPopup);
            }
        }

        #region Lookup Event

        protected void ctlAccountLookup_OnNotifyPopup(object sender, PopUpReturnArgs args)
        {
            if (args.Type.Equals(PopUpReturnType.OK))
            {
                AccountLang accountInfo = (AccountLang)args.Data;
                AccountID = accountInfo.AccountID.ToString();
                ctlAccountTextBoxAutoComplete.AccountCode = accountInfo.AccountCode;
                ctlAccountName.Text = accountInfo.AccountName;
            }
            ctlUpdatePanelAccount.Update();
        }
        #endregion

        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            AccountLookup ctlAccountLookup = LoadPopup<AccountLookup>("~/UserControls/LOV/SCG.DB/AccountLookup.ascx", ctlPopUpUpdatePanel);
            ctlAccountLookup.CompanyIDofDocument = this.CompanyIDofDocument;
            ctlAccountLookup.WithoutExpenseCode = this.WithoutExpenseCode;
            ctlAccountLookup.Show();
        }
        protected void ctlAccountTextBoxAutoComplete_NotifyPopupResult(object sender, string action, string result)
        {
            DbAccount account;
            long accID = 0;

            if (action == "select")
            {
                account = ScgDbQueryProvider.DbAccountQuery.FindByIdentity(UIHelper.ParseLong(result));
                accID = account == null ? 0 : account.AccountID;
                this.BindAccountControl(accID);
                CallOnObjectLookUpReturn(account);
            }
            else if (action == "textchanged")
            {
                if (string.IsNullOrEmpty(result))
                {
                    ResetValue();
                }
                else if (!WithoutExpenseCode.Contains(result))
                {
                    account = ScgDbQueryProvider.DbAccountQuery.FindAccountByAccountCodeExpenseGroup(result, CompanyIDofDocument);
                    accID = account == null ? 0 : account.AccountID;
                    this.BindAccountControl(accID);
                    CallOnObjectLookUpReturn(account);
                }
                else
                {
                    ResetValue();
                }
            }

        }
        #region Public Method
        public void ShowDefault()
        {

        }
        public void BindAccountControl(long accId)
        {
            AccountID = accId.ToString();
            IList<AccountLang> acc = ScgDbQueryProvider.DbAccountLangQuery.FindByDbAccountLangKey(accId, UserAccount.CurrentLanguageID);
            if (acc.Count > 0)
            {
                ctlAccountTextBoxAutoComplete.AccountCode = acc[0].AccountCode;
                ctlAccountName.Text = acc[0].AccountName;
            }
            else
            {
                ResetValue();
            }
            ctlUpdatePanelAccount.Update();
        }
        public void ResetValue()
        {
            ctlAccountTextBoxAutoComplete.AccountCode = string.Empty;
            ctlAccountName.Text = string.Empty;
            AccountID = string.Empty;
            ctlUpdatePanelAccount.Update();
        }
        #endregion
    }
}
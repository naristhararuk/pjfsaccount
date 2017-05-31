using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCG.DB.DTO.ValueObject;
using System.Web.Script.Serialization;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using SCG.DB.DTO;
using SCG.DB.Query;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class AccountTextBoxAutoComplete : BaseUserControl
    {
        #region Property
        public string AccountID
        {
            get
            {
                if (ViewState["AccountID"] != null)
                    return ViewState["AccountID"].ToString();
                else
                    return string.Empty;

            }
            set { ViewState["AccountID"] = value; }
        }
        public string AccountCode
        {
            get { return ctlAccountCode.Text; }
            set { ctlAccountCode.Text = value; }
        }
        public string AccountName
        {
            get { return ctlAccountText.Text; }
            set { ctlAccountText.Text = value; }
        }
        public long? CompanyID
        {
            get
            {
                if (ViewState["CompanyID"] != null)
                {
                    return UIHelper.ParseLong(ViewState["CompanyID"].ToString());
                }
                else
                {
                    return null;
                }
            }
            set { ViewState["CompanyID"] = value; }
        }
        
        public string WithoutExpenseCode
        {
            get
            {
                if (ViewState["WithoutExpenseCode"] != null)
                    return ViewState["WithoutExpenseCode"].ToString();
                return string.Empty;
            }
            set { ViewState["WithoutExpenseCode"] = value; }
        }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            ctlAccountAutoCompleteUpdatePanel.Update();
            ctlAccountTextAutoComplete.BehaviorID = String.Format("AccountAutoCompleteEx{0}", ctlAccountCode.ClientID);
            SetAutoCompleteEvent();

            AccountAutoCompleteParameter parameter = new AccountAutoCompleteParameter();
            parameter.LanguageID = UserAccount.CurrentLanguageID;
            parameter.CompanyID = this.CompanyID;
            parameter.WithoutExpenseCode = this.WithoutExpenseCode;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ctlAccountTextAutoComplete.ContextKey = serializer.Serialize(parameter);
            ctlAccountTextAutoComplete.UseContextKey = true;
        }

        public void SerializeParameter()
        {
            AccountAutoCompleteParameter parameter = new AccountAutoCompleteParameter();
            parameter.LanguageID = UserAccount.CurrentLanguageID;
            parameter.CompanyID = this.CompanyID;
            parameter.WithoutExpenseCode = this.WithoutExpenseCode;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ctlAccountTextAutoComplete.ContextKey = serializer.Serialize(parameter);
            ctlAccountTextAutoComplete.UseContextKey = true;
        }

        public delegate void NotifyPopupResultHandler(object sender, string returnAction, string returnValue);
        public event NotifyPopupResultHandler NotifyPopupResult;


        private void SetAutoCompleteEvent()
        {
            string ClientId = ctlAccountCode.ClientID;
            ctlAccountTextAutoComplete.Animations = ctlAccountTextAutoComplete.Animations.Replace("AccountAutoCompleteEx", ctlAccountTextAutoComplete.BehaviorID);
            ctlAccountTextAutoComplete.OnClientItemSelected = String.Format("{0}_OnSelected", ClientId);
            //ctlAccountTextAutoComplete.OnClientPopulating = String.Format("{0}_OnPopulating", ClientId);
            //ctlAccountTextAutoComplete.OnClientPopulated = String.Format("{0}_OnPopulated", ClientId);
            ctlAccountTextAutoComplete.OnClientItemOut = String.Format("{0}_OnItemOut", ClientId);
            ctlAccountTextAutoComplete.OnClientShowing = String.Format("{0}_OnShowing", ClientId);
            ctlAccountTextAutoComplete.OnClientShown = String.Format("{0}_OnShown", ClientId);
            ctlAccountTextAutoComplete.OnClientHidden = String.Format("{0}_OnHidden", ClientId);
            ctlAccountTextAutoComplete.OnClientHiding = String.Format("{0}_OnHiding", ClientId);
        }

        protected void ctlAccountCode_TextChanged(object sender, EventArgs e)
        {
            if (NotifyPopupResult != null)
            {
                if (!string.IsNullOrEmpty(ctlReturnAction.Value))
                {
                    NotifyPopupResult(this, ctlReturnAction.Value, ctlReturnValue.Value);
                    ctlReturnAction.Value = string.Empty;
                }
                else
                {
                    NotifyPopupResult(this, "textchanged", ctlAccountCode.Text);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(ctlAccountCode.Text))
                {
                    this.ResetControl();
                }
                else
                {
                    DbAccount account = ScgDbQueryProvider.DbAccountQuery.FindAccountByAccountCodeExpenseGroup(ctlAccountCode.Text, CompanyID);
                    if (account != null)
                        this.AccountID = account.AccountID.ToString();
                    else
                        this.ResetControl();
                }
            }
        }

        public void ResetControl()
        {
            ctlAccountCode.Text = string.Empty;
        }
    }
}
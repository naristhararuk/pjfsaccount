using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SS.DB.Query;
using SS.DB.BLL;
using System.Web.Script.Serialization;
using SS.DB.DTO.ValueObject;
using SS.DB.DTO;

namespace SCG.eAccounting.Web.UserControls.Dropdownlist.SS.DB
{
    public partial class CurrencyDropdown : BaseUserControl, IEditorUserControl
    {
        public string SelectedValue
        {
            get { return ViewState["CurrencyID"] == null ? string.Empty : ViewState["CurrencyID"].ToString(); }
            set { this.ViewState["CurrencyID"] = value; }
        }
        public bool IsExpense
        {
            get { return ViewState["IsExpense"] == null ? false : (bool)ViewState["IsExpense"]; }
            set { this.ViewState["IsExpense"] = value; }
        }
        public bool IsAdvanceFR
        {
            get { return ViewState["IsAdvanceFR"] == null ? false : (bool)ViewState["IsAdvanceFR"]; }
            set { this.ViewState["IsAdvanceFR"] = value; }
        }
        public bool Enable
        {
            set
            {
                ctlCurrency.Enabled = value;
                ctlACBtn.Visible = value;
            }
        }

        public int GridViewRowIndex
        {
            get { return (int)ViewState["GridViewRowIndex"]; }
            set { ViewState["GridViewRowIndex"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctlCurrencyTextAutoComplete.BehaviorID = String.Format("CurrencyAutoCompleteEx{0}", ctlCurrency.ClientID);
            SetAutoCompleteEvent();

            CurrencyAutoCompleteParameter parameter = new CurrencyAutoCompleteParameter();
            parameter.LanguageID = UserAccount.CurrentLanguageID;
            parameter.IsExpense = IsExpense;
            parameter.IsAdvanceFR = IsAdvanceFR;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ctlCurrencyTextAutoComplete.ContextKey = serializer.Serialize(parameter);
            ctlCurrencyTextAutoComplete.UseContextKey = true;
        }

        public delegate void NotifyPopupResultHandler(object sender, string returnAction, string returnValue);
        public event NotifyPopupResultHandler NotifyPopupResult;

        public delegate void CurrencyChangedHandler(object sender);
        public event CurrencyChangedHandler NotifyCurrencyChanged;

        private void SetAutoCompleteEvent()
        {
            string ClientId = ctlCurrency.ClientID;
            ctlCurrencyTextAutoComplete.Animations = ctlCurrencyTextAutoComplete.Animations.Replace("CurrencyAutoCompleteEx", ctlCurrencyTextAutoComplete.BehaviorID);
            ctlCurrencyTextAutoComplete.OnClientItemSelected = String.Format("{0}_OnSelected", ClientId);
            //ctlAccountTextAutoComplete.OnClientPopulating = String.Format("{0}_OnPopulating", ClientId);
            //ctlAccountTextAutoComplete.OnClientPopulated = String.Format("{0}_OnPopulated", ClientId);
            ctlCurrencyTextAutoComplete.OnClientItemOut = String.Format("{0}_OnItemOut", ClientId);
            ctlCurrencyTextAutoComplete.OnClientShowing = String.Format("{0}_OnShowing", ClientId);
            ctlCurrencyTextAutoComplete.OnClientShown = String.Format("{0}_OnShown", ClientId);
            ctlCurrencyTextAutoComplete.OnClientHidden = String.Format("{0}_OnHidden", ClientId);
            ctlCurrencyTextAutoComplete.OnClientHiding = String.Format("{0}_OnHiding", ClientId);
        }

        public void BindCurrency(short currencyId)
        {
            VOUCurrencySetup currency = SsDbQueryProvider.DbCurrencyQuery.GetCurrencyLangByCurrencyID(currencyId, UserAccount.CurrentLanguageID);
            if (currency != null)
            {
                SelectedValue = currency.CurrencyID.ToString();
                ctlCurrency.Text = currency.Symbol;
            }
            ctlCurrencyAutoCompleteUpdatePanel.Update();
        }
        protected void ctlCurrency_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ctlReturnAction.Value))
            {
                SelectedValue = ctlReturnValue.Value;
                if (NotifyPopupResult != null) NotifyPopupResult(this, ctlReturnAction.Value, SelectedValue);
                ctlReturnAction.Value = string.Empty;
            }
            else
            {
                DbCurrency currency = SsDbQueryProvider.DbCurrencyQuery.FindByCurrencySymbol(ctlCurrency.Text.ToUpper(), IsExpense, IsAdvanceFR);
                if (string.IsNullOrEmpty(ctlCurrency.Text) || currency == null) ResetControl();
                else SelectedValue = currency.CurrencyID.ToString();
                if (NotifyPopupResult != null) NotifyPopupResult(this, "textChange", ctlCurrency.Text);
            }

            if (NotifyCurrencyChanged != null)
                NotifyCurrencyChanged(this);
        }

        public void ResetControl()
        {
            ctlCurrency.Text = string.Empty;
            SelectedValue = string.Empty;
        }
        public string Text
        {
            get { return ctlCurrency.Text; }
        }
        public bool Display
        {
            set
            {
                if (value)
                    ctlContainerTable.Style.Add("display", "inline-block");
                else
                    ctlContainerTable.Style.Add("display", "none");
            }
        }
    }
}
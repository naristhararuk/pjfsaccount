using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.DB.DTO;
using SS.DB.DTO.ValueObject;
using SS.DB.Query;
using SCG.eAccounting.Web.Helper;
using SCG.DB.DTO;
using SS.DB.BLL;
using SS.Standard.UI;
using SS.Standard.Security;
using SS.Standard.Utilities;


namespace SCG.eAccounting.Web.UserControls
{
    public partial class CurrencySetupEditor : BaseUserControl
    {
        public event EventHandler Notify_Ok;
        public event EventHandler Notify_Cancle;

        public IDbCurrencyLangService DbCurrencyLangService { get; set; }
        public IDbCurrencyService DbCurrencyService { get; set; }

        public string Mode
        {
            get { return this.ViewState["Mode"] == null ? FlagEnum.NewFlag : (string)this.ViewState["Mode"]; }
            set { this.ViewState["Mode"] = value; }
        }
        public short CurrencyID
        {
            get { return this.ViewState["CurrencyID"] == null ? (short)0 : (short)this.ViewState["CurrencyID"]; }
            set { this.ViewState["CurrencyID"] = value; }
        }

        public void ResetValue()
        {
            ctlSymbol.Text = string.Empty;
            ctlActive.Checked = true;
            ctlComment.Text = string.Empty;
            ctlCurrencyEditorGrid.DataCountAndBind();
            ctlCurrencyUpdatePanel.Update();
            ctlCurrencySymbol.Text = string.Empty;
            
        }
        protected void CurrencyEditor_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow row in ctlCurrencyEditorGrid.Rows)
            {
                TextBox ctrDescription = row.FindControl("ctrDescription") as TextBox;
                TextBox ctrComment = row.FindControl("ctrComment") as TextBox;
                CheckBox active = row.FindControl("ctlActive") as CheckBox;
                TextBox ctlMainUnit = row.FindControl("ctlMainUnit") as TextBox;
                TextBox ctlSubUnit = row.FindControl("ctlSubUnit") as TextBox;

                if ((string.IsNullOrEmpty(ctrDescription.Text)) && (string.IsNullOrEmpty(ctrComment.Text)))
                {

                    active.Checked = true;
                }
            }
        }
        public void Initialize(string mode, short? currencyID)
        {
            Mode = mode.ToString();
            if (currencyID.HasValue)
                CurrencyID = currencyID.Value;

            if (mode.Equals(FlagEnum.EditFlag))
            {
                DbCurrency currency = SsDbQueryProvider.DbCurrencyQuery.FindByIdentity(CurrencyID);
                ctlSymbol.Text = currency.Symbol;
                ctlComment.Text = currency.Comment;
                ctlActive.Checked = currency.Active;
                ctlCurrencyEditorGrid.DataCountAndBind();
                ctlCurrencyUpdatePanel.Update();
                ctlCurrencySymbol.Text = currency.CurrencySymbol;
            }
            else if (mode.ToString() == FlagEnum.NewFlag)
            {
                ResetValue();

            }

        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            return SsDbQueryProvider.DbCurrencyLangQuery.FindCurrencyLangByCurrencyID(CurrencyID);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void ctlAdd_Click(object sender, ImageClickEventArgs e)
        {
            DbCurrency currency;

            if (Mode.Equals(FlagEnum.EditFlag))
                currency = SsDbQueryProvider.DbCurrencyQuery.FindByIdentity(CurrencyID);
            else
                currency = new DbCurrency();

            currency.Symbol     = ctlSymbol.Text;
            currency.Active     = ctlActive.Checked;
            currency.CreBy      = UserAccount.UserID;
            currency.CreDate    = DateTime.Now;
            currency.UpdBy      = UserAccount.UserID;
            currency.UpdDate    = DateTime.Now;
            currency.UpdPgm     = UserAccount.CurrentProgramCode;
            currency.Comment    = ctlComment.Text;
            currency.CurrencySymbol = ctlCurrencySymbol.Text;

            try
            {
                DbCurrencyService.SaveOrUpdate(currency);
                
                // save or update currencylang
                IList<DbCurrencyLang> list = new List<DbCurrencyLang>();
                foreach (GridViewRow row in ctlCurrencyEditorGrid.Rows)
                {
                    short       languageId      = UIHelper.ParseShort(ctlCurrencyEditorGrid.DataKeys[row.RowIndex]["LanguageID"].ToString());
                    TextBox     ctrDescription  = row.FindControl("ctrDescription")         as TextBox;
                    TextBox     ctrComment      = row.FindControl("ctrComment")             as TextBox;
                    CheckBox    ctlActiveCl     = row.FindControl("ctlActive")              as CheckBox;
                    TextBox     ctlMainUnit     = row.FindControl("ctlMainUnit")            as TextBox;
                    TextBox     ctlSubUnit      = row.FindControl("ctlSubUnit")             as TextBox;

                    if ((!string.IsNullOrEmpty(ctrDescription.Text)) || (!string.IsNullOrEmpty(ctrComment.Text)))
                    {
                        DbCurrencyLang currencyLang = new DbCurrencyLang();
                        currencyLang.Description    = ctrDescription.Text;
                        currencyLang.Comment        = ctrComment.Text;
                        currencyLang.Active         = ctlActiveCl.Checked;
                        currencyLang.Currency       = currency;
                        currencyLang.CreBy          = UserAccount.UserID;
                        currencyLang.CreDate        = DateTime.Now;
                        currencyLang.UpdBy          = UserAccount.UserID;
                        currencyLang.UpdDate        = DateTime.Now;
                        currencyLang.Language       = new DbLanguage(languageId);
                        currencyLang.UpdPgm         = UserAccount.CurrentProgramCode;
                        currencyLang.MainUnit = ctlMainUnit.Text;
                        currencyLang.SubUnit = ctlSubUnit.Text;
                        list.Add(currencyLang);
                    }

                }

                if (Mode.Equals(FlagEnum.EditFlag))
                    DbCurrencyLangService.UpdateCurrencyLang(list);
                if (Mode.Equals(FlagEnum.NewFlag))
                    DbCurrencyLangService.AddCurrencyLang(list);

                Notify_Ok(sender, e);

            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
           
           
        }
        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            if (Notify_Cancle != null)
            {
                Notify_Cancle(sender, e);
            }
            HidePopUp();

        }
        protected void ctlCurrencySetup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }
        public void HidePopUp()
        {
            ctlCurrencyEditorModalPopupExtender.Hide();
        }
        public void ShowPopUp()
        {
            ctlCurrencyEditorModalPopupExtender.Show();

        }
    }
}
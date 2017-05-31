using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCG.DB.BLL;
using SCG.DB.DTO;
using SS.Standard.Utilities;
using SCG.DB.DAL;
using SCG.eAccounting.Web.Helper;
using SCG.DB.Query;
using SS.Standard.UI;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class CompanyTaxEditor : BaseUserControl
    {
        public IDbCompanyTaxService DbCompanyTaxService { get; set; }

        #region Properties
        public string Mode
        {
            get { return this.ViewState["Mode"] == null ? FlagEnum.NewFlag : (string)this.ViewState["Mode"]; }
            set { this.ViewState["Mode"] = value; }
        }
        public long TaxID
        {
            get { return this.ViewState["TaxID"] == null ? (long)0 : (long)this.ViewState["TaxID"]; }
            set { this.ViewState["TaxID"] = value; }
        }

        public long CompanyTaxID
        {
            get { return this.ViewState["CompanyTaxID"] == null ? (long)0 : (long)this.ViewState["CompanyTaxID"]; }
            set { this.ViewState["CompanyTaxID"] = value; }
        }
        #endregion

        public event EventHandler Notify_Ok;
        public event EventHandler Notify_Cancle;
        protected void Page_Load(object sender, EventArgs e)
        {
            ctlCompanyField.SetWidthTextBox(148);
        }
        public void ResetValue()
        {
            ctlCompanyField.ResetValue();
            ctlRate.Text = string.Empty;
            ctlRateNonDeduct.Text = string.Empty;
            ctlUseParentRate.Checked = false;
            ctlDisable.Checked = false;
            ctlCompanyTaxUpdatePanel.Update();
        }
        public void Initialize(string mode, long Id,long taxID)
        {
            Mode = mode;
            TaxID = taxID;
            CompanyTaxID = Id;
            if (Mode.Equals(FlagEnum.EditFlag) || !Id.Equals(0))
            {
                DbCompanyTax dbCompanyTax = DbCompanyTaxService.FindByIdentity(CompanyTaxID);
                try
                {
                    if (dbCompanyTax.CompanyID != 0)
                    {
                        ctlCompanyField.SetValue(dbCompanyTax.CompanyID);
                        ctlCompanyField.Mode = ModeEnum.Readonly;
                        ctlCompanyField.ChangeMode();
                    }
                }
                catch { }
                ctlRate.Text = dbCompanyTax.Rate.ToString("#,##0.0000");
                ctlRateNonDeduct.Text = dbCompanyTax.RateNonDeduct.ToString("#,##0.0000");
                ctlUseParentRate.Checked = dbCompanyTax.UseParentRate;
                ctlDisable.Checked = dbCompanyTax.Disable;
                UseParentRate();
                ctlCompanyTaxUpdatePanel.Update(); ;
            }
            else if (Mode.Equals(FlagEnum.NewFlag))
            {
                ResetValue();
                ctlCompanyField.Mode = ModeEnum.ReadWrite;
                ctlCompanyField.ChangeMode();
            }

        }
        private void CheckDataValueUpdate(DbCompanyTax ct)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (ct.CompanyID <= 0)
            {
                errors.AddError("CompanyTax.Error", new Spring.Validation.ErrorMessage("RequiredCompany"));
            }
            if (!ctlUseParentRate.Checked)
            {
                if (ct.Rate == 0)
                {
                    errors.AddError("CompanyTax.Error", new Spring.Validation.ErrorMessage("RequiredRate"));
                }
            }
            if (CompanyTaxID == 0)
            {
                if (ScgDbDaoProvider.DbCompanyTaxDao.IsDuplicateCompanyCode(ct))
                {
                    errors.AddError("CompanyTax.Error", new Spring.Validation.ErrorMessage("Duplicate Company Code"));
                }
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
        }
        protected void ctlSave_Click1(object sender, ImageClickEventArgs e)
        {
            DbCompanyTax ct;
            try
            {
                
                if (Mode.Equals(FlagEnum.EditFlag))
                    ct = DbCompanyTaxService.FindByIdentity(CompanyTaxID);
                else
                    ct = new DbCompanyTax();
                ct.TaxID = TaxID;
                ct.Rate = UIHelper.ParseDouble(ctlRate.Text);
                ct.RateNonDeduct = UIHelper.ParseDouble(ctlRateNonDeduct.Text);
                ct.UseParentRate = ctlUseParentRate.Checked;
                ct.Disable = ctlDisable.Checked;
                DbCompany com = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(UIHelper.ParseLong(ctlCompanyField.CompanyID));
                if (com != null)
                {
                    ct.CompanyID = com.CompanyID;
                }

                ct.CreBy = UserAccount.UserID;
                ct.CreDate = DateTime.Now;
                ct.UpdBy = UserAccount.UserID;
                ct.UpdDate = DateTime.Now;
                ct.UpdPgm = UserAccount.CurrentLanguageCode;
                CheckDataValueUpdate(ct);
                ScgDbDaoProvider.DbCompanyTaxDao.SaveOrUpdate(ct);
                Notify_Ok(sender, e);
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
            }

            catch (NullReferenceException)
            {

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
        public void HidePopUp()
        {
            ctlRate.Enabled = true;
            ctlRateNonDeduct.Enabled = true;
            ctlCompanyTaxModalPopupExtender.Hide();
        }     
        public void ShowPopUp()
        {
            ctlCompanyTaxModalPopupExtender.Show();
        }

        protected void ctlUseParentRate_CheckedChanged(object sender, EventArgs e)
        {
            UseParentRate();
        }

        public void UseParentRate()
        {
            if (ctlUseParentRate.Checked)
            {
                ctlRate.Enabled = false;
                ctlRateNonDeduct.Enabled = false;
            }
            else
            {
                ctlRate.Enabled = true;
                ctlRateNonDeduct.Enabled = true;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.DB.BLL;
using SCG.DB.DTO;
using SCG.DB.Query;
using SCG.eAccounting.Web.Helper;
using SS.Standard.Utilities;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class ExpenseCompanyEditor : BaseUserControl
    {
        public IDbAccountCompanyService DbAccountCompanyService { get; set; }

        public event EventHandler Notify_Ok;
        public event EventHandler Notify_Cancle;
        public string Mode
        {
            get { return this.ViewState["Mode"] == null ? FlagEnum.NewFlag : (string)this.ViewState["Mode"]; }
            set { this.ViewState["Mode"] = value; }
        }
        public long AccountID
        {
            get { return this.ViewState["AccountID"] == null ? (long)0 : (long)this.ViewState["AccountID"]; }
            set { this.ViewState["AccountID"] = value; }
        }
        public long ExpenseCompanyID
        {
            get { return this.ViewState["ExpenseCompanyID"] == null ? (long)0 : (long)this.ViewState["ExpenseCompanyID"]; }
            set { this.ViewState["ExpenseCompanyID"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void Initialize(string mode, long accountID, long expenseCompanyID)
        {
            Mode = mode;
            AccountID = accountID;
            ExpenseCompanyID = expenseCompanyID;
            if (mode.Equals(FlagEnum.EditFlag))
            {
                DbAccountCompany db = ScgDbQueryProvider.DbAccountCompanyQuery.FindProxyByIdentity(expenseCompanyID);
                ctlCompanyCode.Text = db.CompanyCode;
                ctlCompanyField.Mode = ModeEnum.Readonly;
                ctlCompanyField.ChangeMode();
                ctlUseParent.Checked = db.UseParent;
                ctlTaxCode.SelectedValue = db.TaxCode.ToString();
                ctlCostCenter.SelectedValue = db.CostCenter.ToString();
                ctlInternalOrder.SelectedValue = db.InternalOrder.ToString();
                ctlSaleOrder.SelectedValue = db.SaleOrder.ToString();
                try
                {
                    if (db.CompanyID != null)
                    {
                        ctlCompanyField.SetValue(db.CompanyID.CompanyID);
                    }
                }
                catch { }
                ctlUpdatePanel.Update();
            }
        }
        public void ResetValue()
        {
            ctlCompanyField.ResetValue();
            ctlUseParent.Checked = false;
            ctlTaxCode.SelectedValue = "0";
            ctlCostCenter.SelectedValue = "0";
            ctlInternalOrder.SelectedValue = "0";
            ctlSaleOrder.SelectedValue = "0";
            ctlUpdatePanel.Update();
        }
        protected void ctlAdd_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                DbAccountCompany accountCom = null;
                if (Mode.Equals(FlagEnum.EditFlag))
                {
                    accountCom = ScgDbQueryProvider.DbAccountCompanyQuery.FindByIdentity(ExpenseCompanyID);
                }

                if (accountCom != null)
                {
                    accountCom.UseParent = ctlUseParent.Checked;
                    accountCom.TaxCode = Convert.ToInt32(ctlTaxCode.SelectedValue);
                    accountCom.CostCenter = Convert.ToInt32(ctlCostCenter.SelectedValue);
                    accountCom.InternalOrder = Convert.ToInt32(ctlInternalOrder.SelectedValue);
                    accountCom.SaleOrder = Convert.ToInt32(ctlInternalOrder.SelectedValue);
                    //DbCompany com = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(UIHelper.ParseLong(ctlCompanyField.CompanyID));
                    //if (com != null)
                    //{

                    //    accountCom.CompanyID = new DbCompany(com.CompanyID);
                    //    accountCom.CompanyCode = com.CompanyCode;
                    //}

                    //DbAccount ac = ScgDbQueryProvider.DbAccountQuery.FindByIdentity(AccountID);
                    //if (ac != null)
                    //{
                    //    accountCom.AccountID = new DbAccount(ac.AccountID);
                    //}
                    accountCom.Active = true;
                    accountCom.CreBy = UserAccount.UserID;
                    accountCom.CreDate = DateTime.Now;
                    accountCom.UpdBy = UserAccount.UserID;
                    accountCom.UpdDate = DateTime.Now;
                    accountCom.UpdPgm = UserAccount.CurrentProgramCode;

                    if (Mode.Equals(FlagEnum.EditFlag))
                    {
                        DbAccountCompanyService.Update(accountCom);
                    }
                }

                if (Notify_Ok != null)
                {
                    Notify_Ok(sender, e);
                }

                HidePopUp();
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
        public void HidePopUp()
        {
            ResetValue();
            ctlExpenseCompanyModalPopupExtender.Hide();
        }
        public void ShowPopUp()
        {
            ctlExpenseCompanyModalPopupExtender.Show();

        }
    }
}
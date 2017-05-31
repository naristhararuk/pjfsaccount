using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using Spring.Validation;
using SS.Standard.Utilities;
using SCG.DB.DTO;
using SCG.DB.BLL;
using SS.DB.DTO;
using SCG.DB.Query;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class AccountLangEditor : BaseUserControl
    {
        public IDbAccountService DbAccountService { get; set; }
        public IDbAccountLangService DbAccountLangService { get; set; }

        public event EventHandler Notify_Ok;
        public event EventHandler Notify_Cancle;



        public string Mode
        {
            get { return this.ViewState["Mode"] == null ? FlagEnum.NewFlag : (string)this.ViewState["Mode"]; }
            set { this.ViewState["Mode"] = value; }
        }
        public long ExpenseGroupID
        {
            get { return this.ViewState["ExpenseGroupID"] == null ? (long)0 : (long)this.ViewState["ExpenseGroupID"]; }
            set { this.ViewState["ExpenseGroupID"] = value; }
        }
        public long AcountID
        {
            get { return this.ViewState["AcountID"] == null ? (long)0 : (long)this.ViewState["AcountID"]; }
            set { this.ViewState["AcountID"] = value; }
        }
        public void ResetValue()
        {
            ctlExpenseCode.Text = string.Empty;
            ctlExpenseCode.Text = string.Empty;
            ctlActive.Checked = false;
            ctlSaveAsDebtor.Checked = false;
            ctlSaveAsVendor.Checked = false;
            ctlspecialGL.Text = string.Empty;
            ctlSAPSpecialGLAssignment.Text = string.Empty;
            ctlDomesticRecommend.Checked = false;
            ctlForeignRecommend.Checked = false;
            ctlTaxCode.SelectedValue = "0";
            ctlCostCenter.SelectedValue = "0";
            ctlInternalOrder.SelectedValue = "0";
            ctlSaleOrder.SelectedValue = "0";
            ctlAccountLangGrid.DataCountAndBind();
            ctlAccountLangUpdatePanel.Update();

        }
        public void Initialize(string mode, long accountID, long expenseGID)
        {
            Mode = mode.ToString();
            ExpenseGroupID = expenseGID;
            AcountID = accountID;
            if (mode.Equals(FlagEnum.EditFlag))
            {
                DbAccount db = ScgDbQueryProvider.DbAccountQuery.FindByIdentity(accountID);
                ctlExpenseCode.Text = db.AccountCode;
                ctlActive.Checked = db.Active;
                ctlSaveAsDebtor.Checked = db.SaveAsDebtor;
                ctlSaveAsVendor.Checked = db.SaveAsVendor;
                ctlspecialGL.Text = db.SAPSpecialGL;
                ctlSAPSpecialGLAssignment.Text = db.SAPSpecialGLAssignment;
                ctlDomesticRecommend.Checked = db.DomesticRecommend;
                ctlForeignRecommend.Checked = db.ForeignRecommend;
                ctlTaxCode.SelectedValue = db.TaxCode.ToString();
                ctlCostCenter.SelectedValue = db.CostCenter.ToString();
                ctlInternalOrder.SelectedValue = db.InternalOrder.ToString();
                ctlSaleOrder.SelectedValue = db.SaleOrder.ToString();
                ctlAccountLangGrid.DataCountAndBind();
                ctlAccountLangUpdatePanel.Update();

            }
            else if (mode.ToString() == FlagEnum.NewFlag)
            {
                ResetValue();
            }

        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            return ScgDbQueryProvider.DbAccountLangQuery.FindAccountLangByAccountID(AcountID);

        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void ExpenseLangEditor_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow row in ctlAccountLangGrid.Rows)
            {
                TextBox ctrDescription = row.FindControl("ctrDescription") as TextBox;
                TextBox ctrComment = row.FindControl("ctrComment") as TextBox;
                CheckBox active = row.FindControl("ctlActive") as CheckBox;

                if ((string.IsNullOrEmpty(ctrDescription.Text)) && (string.IsNullOrEmpty(ctrComment.Text)))
                {

                    active.Checked = true;
                    ctlActive.Checked = true;
                }
            }
        }
        protected void ctlAdd_Click(object sender, ImageClickEventArgs e)
        {

            DbAccount account = new DbAccount();

            if (Mode.Equals(FlagEnum.EditFlag))
            {
                account.AccountID = AcountID;
            }
            account.AccountCode = ctlExpenseCode.Text;
            account.Active = ctlActive.Checked;
            account.CreBy = UserAccount.UserID;
            account.CreDate = DateTime.Now;
            account.DomesticRecommend = ctlDomesticRecommend.Checked;
            account.ExpenseGroupID = ExpenseGroupID;
            account.ForeignRecommend = ctlForeignRecommend.Checked;
            account.SaveAsDebtor = ctlSaveAsDebtor.Checked;
            account.SaveAsVendor = ctlSaveAsVendor.Checked;
            account.SAPSpecialGL = ctlspecialGL.Text;
            account.SAPSpecialGLAssignment = ctlSAPSpecialGLAssignment.Text;
            account.TaxCode = Convert.ToInt32(ctlTaxCode.SelectedValue);
            account.CostCenter = Convert.ToInt32(ctlCostCenter.SelectedValue);
            account.InternalOrder = Convert.ToInt32(ctlInternalOrder.SelectedValue);
            account.SaleOrder = Convert.ToInt32(ctlSaleOrder.SelectedValue);
            account.UpdBy = UserAccount.UserID;
            account.UpdDate = DateTime.Now;
            account.UpdPgm = UserAccount.CurrentProgramCode;

            #region check for exception

            #endregion
            try
            {
                // save or update PB
                if (Mode.Equals(FlagEnum.EditFlag))
                {
                    DbAccountService.UpdateAccount(account);
                }
                else
                {
                    account.AccountID = DbAccountService.AddNewAccount(account);

                }

                // save or update PBlang
                IList<DbAccountLang> list = new List<DbAccountLang>();

                foreach (GridViewRow row in ctlAccountLangGrid.Rows)
                {
                    short languageId = UIHelper.ParseShort(ctlAccountLangGrid.DataKeys[row.RowIndex]["LanguageID"].ToString());

                    TextBox Description = row.FindControl("ctrDescription") as TextBox;
                    TextBox Comment = (TextBox)row.FindControl("ctrComment") as TextBox;
                    CheckBox Active = (CheckBox)row.FindControl("ctlActive") as CheckBox;

                    if ((!string.IsNullOrEmpty(Description.Text)))
                    {
                        DbAccountLang accountLang = new DbAccountLang();
                        accountLang.Active = Active.Checked;
                        accountLang.CreBy = UserAccount.UserID;
                        accountLang.CreDate = DateTime.Now;
                        accountLang.AccountName = Description.Text;
                        accountLang.Comment = Comment.Text;
                        accountLang.LanguageID = new DbLanguage(languageId);
                        accountLang.Account = account;
                        accountLang.UpdBy = UserAccount.UserID;
                        accountLang.UpdDate = DateTime.Now;
                        accountLang.UpdPgm = UserAccount.CurrentLanguageCode;
                        list.Add(accountLang);
                    }

                }

                if (Mode.Equals(FlagEnum.EditFlag))
                {
                    DbAccountLangService.UpdateListAccountLang(list);
                }
                if (Mode.Equals(FlagEnum.NewFlag))
                {
                    DbAccountLangService.AddListAccountLang(list);
                }

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
        public void HidePopUp()
        {
            ctlAccountLangModalPopupExtender.Hide();
        }
        public void ShowPopUp()
        {
            ctlAccountLangModalPopupExtender.Show();

        }
    }
}
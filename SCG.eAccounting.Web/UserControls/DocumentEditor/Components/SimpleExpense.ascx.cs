using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCG.DB.DTO.ValueObject;
using SS.Standard.UI;
using SCG.eAccounting.BLL;
using SCG.DB.Query;
using SCG.eAccounting.Web;
using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.DTO;
using SCG.DB.DTO;
using SS.DB.Query;
using SS.Standard.Utilities;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.DTO.DataSet;
using SS.SU.Query;
using SS.SU.DTO;
namespace SCG.eAccounting.Web.UserControls.DocumentEditor.Components
{
    public partial class SimpleExpense : BaseUserControl, IEditorComponent
    {
        public ITransactionService TransactionService { get; set; }
        public IFnExpenseInvoiceItemService FnExpenseInvoiceItemService { get; set; }
        public IFnExpenseInvoiceService FnExpenseInvoiceService { get; set; }
        private bool refreshHeaderGrid;

        #region Property
        public Guid TransactionId
        {
            get { return (Guid)ViewState[ViewStateName.TransactionID]; }
            set { ViewState[ViewStateName.TransactionID] = value; }
        }
        public long ExpDocumentID
        {
            get { return (long)ViewState[ViewStateName.DocumentID]; }
            set { ViewState[ViewStateName.DocumentID] = value; }
        }
        public string DocumentType
        {
            get { return ViewState["DocumentType"].ToString(); }
            set { ViewState["DocumentType"] = value; }
        }
        public string InitialFlag
        {
            get { return ViewState[ViewStateName.InitialFlag].ToString(); }
            set { ViewState[ViewStateName.InitialFlag] = value; }
        }
        public long CompanyID
        {
            get { return (long)ViewState["CompanyID"]; }
            set { ViewState["CompanyID"] = value; }
        }

        public long CompanyIDRequester
        {
            get { return (long)ViewState["CompanyIDRequester"]; }
            set { ViewState["CompanyIDRequester"] = value; }
        }
        public bool IsRepOffice
        {
            get
            {
                if (ViewState["IsRepOffice"] != null)
                    return (bool)ViewState["IsRepOffice"];
                return false;
            }
            set { ViewState["IsRepOffice"] = value; }
        }
        public string SelectedCurrency
        {
            get
            {
                if (ViewState["SelectedCurrency"] != null)
                    return ViewState["SelectedCurrency"].ToString();
                return "THB";
            }
            set { ViewState["SelectedCurrency"] = value; }
        }

        public short FinalCurrencyID
        {
            get
            {
                if (ViewState["FinalCurrencyID"] != null)
                    return (short)ViewState["FinalCurrencyID"];
                return 0;
            }
            set { ViewState["FinalCurrencyID"] = value; }
        }

        #endregion
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (refreshHeaderGrid)
            {
                if (ctlSimpleExpenseGridView.Visible)
                {
                    ctlSimpleExpenseGridView.DataBind();
                    SetCurrentSimpleExpense();
                    if (Session["CurrentRecommendExpense"] != null)
                    {
                        Session.Remove("CurrentRecommendExpense");
                    }
                    ctlUpdateSimpleExpenseGridView.Update();
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void Initialize(Guid txID, long documentID, string initFlag)
        {
            this.TransactionId = txID;
            this.ExpDocumentID = documentID;
            this.InitialFlag = initFlag;

        }

        protected void ctlSimpleExpenseGridView_RowDatabound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                UserControls.Dropdownlist.SS.DB.CurrencyDropdown currencyDropdown = e.Row.FindControl("ctlCurrencyDropdown") as UserControls.Dropdownlist.SS.DB.CurrencyDropdown;
                currencyDropdown.GridViewRowIndex = e.Row.RowIndex;
                currencyDropdown.BindCurrency(ParameterServices.USDCurrencyID);
                UserControls.LOV.SCG.DB.CostCenterField cost = e.Row.FindControl("ctlCostCenterLabelLookup") as UserControls.LOV.SCG.DB.CostCenterField;
                UserControls.LOV.SCG.DB.AccountField account = e.Row.FindControl("ctlAccountLabelLookup") as UserControls.LOV.SCG.DB.AccountField;
                UserControls.LOV.SCG.DB.IOAutoCompleteLookup io = e.Row.FindControl("ctlIOLabelLookup") as UserControls.LOV.SCG.DB.IOAutoCompleteLookup;
                HiddenField costId = e.Row.FindControl("ctlCostCenter") as HiddenField;
                HiddenField accountId = e.Row.FindControl("ctlAccountID") as HiddenField;

                if (CompanyIDRequester == CompanyID)
                    cost.BindCostCenterControl(UIHelper.ParseLong(costId.Value));
                cost.CompanyId = CompanyID;
                io.CompanyId = CompanyID;
                cost.AccountFieldControlID = account.ID;
                cost.IOFieldControlID = io.ID;
                
                account.WithoutExpenseCode = ParameterServices.AccountMileageExtra + "," + ParameterServices.AccountPerdiem;
                account.BindAccountControl(UIHelper.ParseLong(accountId.Value));
                if (cost.CostCenterId != "")
                {
                    io.CostCenterId = UIHelper.ParseLong(cost.CostCenterId);
                }
                
                account.CompanyIDofDocument = CompanyID;

                TextBox amount = e.Row.FindControl("ctlTxtAmountTHB") as TextBox;
                TextBox currencyAmount = e.Row.FindControl("ctlTxtCurrencyAmount") as TextBox;
                TextBox exchangeRate = e.Row.FindControl("ctlTxtExchangeRate") as TextBox;

                if (this.DocumentType.Equals(ZoneType.Foreign))
                {
                    currencyAmount.Attributes.Remove("onblur");
                    exchangeRate.Attributes.Remove("onblur");
                    currencyAmount.Attributes.Add("onblur", string.Format("calAmountTHB({0}, {1}, {2});", currencyAmount.ClientID, exchangeRate.ClientID, amount.ClientID));
                    exchangeRate.Attributes.Add("onblur", string.Format("calAmountTHB({0}, {1}, {2});", currencyAmount.ClientID, exchangeRate.ClientID, amount.ClientID));

                }
                else
                {
                    amount.ReadOnly = false;
                }

                if (InitialFlag.Equals(FlagEnum.NewFlag))
                {
                    TextBox des = e.Row.FindControl("ctlTxtDescription") as TextBox;
                    TextBox reference = e.Row.FindControl("ctlTxtRefNo") as TextBox;
                    
                    des.Text = string.Empty;
                    amount.Text = string.Empty;
                    reference.Text = string.Empty;
                    currencyAmount.Text = string.Empty;
                    exchangeRate.Text = string.Empty;
                    if (IsRepOffice)
                    {
                        if (this.DocumentType.Equals(ZoneType.Foreign))
                        {
                            if (this.FinalCurrencyID == UIHelper.ParseShort(currencyDropdown.SelectedValue))
                            {
                                exchangeRate.Text = UIHelper.BindExchangeRate("1");
                            }
                        }
                    }
                }
            }
        }

        protected void currencyDropdown_NotifyCurrencyChanged(object sender)
        {
            if (!IsRepOffice) return;

            UserControls.Dropdownlist.SS.DB.CurrencyDropdown currencyDropdown = sender as UserControls.Dropdownlist.SS.DB.CurrencyDropdown;
            if (currencyDropdown != null)
            {
                if (this.FinalCurrencyID != UIHelper.ParseShort(currencyDropdown.SelectedValue))
                {
                    GridViewRow row = ctlSimpleExpenseGridView.Rows[currencyDropdown.GridViewRowIndex];
                    TextBox exchangeRate = row.FindControl("ctlTxtExchangeRate") as TextBox;
                    exchangeRate.Text = string.Empty;
                    ctlUpdateSimpleExpenseGridView.Update();
                }
            }
        }

        public void BindSimpleExpense(string expenseType, long userId, bool refreshHeader)
        {
            this.refreshHeaderGrid = refreshHeader;
            ExpenseRecommend recommand = new ExpenseRecommend();
            recommand.UserID = userId;
            recommand.LanguageID = UserAccount.CurrentLanguageID;

            SuUser requester = QueryProvider.SuUserQuery.FindByIdentity(userId);
            if (requester != null)
            {
                CompanyIDRequester = requester.Company.CompanyID;
            }

            this.DocumentType = expenseType;

            if (expenseType.Equals(ZoneType.Domestic))
            {
                recommand.IsDomesticRecommend = true;
            }
            else if (expenseType.Equals(ZoneType.Foreign))
            {
                recommand.IsForegnRecommend = true;
            }

            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(this.TransactionId);
            ExpenseDataSet.FnExpenseDocumentRow expRow = ds.FnExpenseDocument.FindByExpenseID(ExpDocumentID);

            if (!expRow.IsLocalCurrencyIDNull())
                this.FinalCurrencyID = expRow.LocalCurrencyID;

            ctlSimpleExpenseGridView.DataSource = ScgDbQueryProvider.DbAccountQuery.FindExpenseRecommendByExpenseGroup(recommand);
            ctlSimpleExpenseGridView.DataBind();
            SetCurrentSimpleExpense();
            ctlUpdateSimpleExpenseGridView.Update();
        }
        public void SaveExpenseRecommend()
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            double totalAmount = 0.0;
            IList<FnExpenseInvoiceItem> RecommendList = new List<FnExpenseInvoiceItem>();
            FnExpenseInvoice invoice = new FnExpenseInvoice();
            invoice.Expense = new FnExpenseDocument(this.ExpDocumentID);
            invoice.InvoiceDocumentType = InvoiceType.General;
            invoice.IsVAT = false;
            invoice.IsWHT = false;
            foreach (GridViewRow row in ctlSimpleExpenseGridView.Rows)
            {
                UserControls.LOV.SCG.DB.CostCenterField cost = row.FindControl("ctlCostCenterLabelLookup") as UserControls.LOV.SCG.DB.CostCenterField;
                UserControls.LOV.SCG.DB.AccountField account = row.FindControl("ctlAccountLabelLookup") as UserControls.LOV.SCG.DB.AccountField;
                UserControls.LOV.SCG.DB.IOAutoCompleteLookup io = row.FindControl("ctlIOLabelLookup") as UserControls.LOV.SCG.DB.IOAutoCompleteLookup;
                TextBox des = row.FindControl("ctlTxtDescription") as TextBox;
                TextBox amount = row.FindControl("ctlTxtAmountTHB") as TextBox;
                TextBox reference = row.FindControl("ctlTxtRefNo") as TextBox;
                TextBox exchangeRate = row.FindControl("ctlTxtExchangeRate") as TextBox;

                if (!string.IsNullOrEmpty(amount.Text) && UIHelper.ParseDouble(amount.Text) > 0)
                {
                    FnExpenseInvoiceItem recommend = new FnExpenseInvoiceItem();
                    recommend.Invoice = invoice;

                    if (UIHelper.ParseLong(account.AccountID) > 0)
                        recommend.Account = new DbAccount(UIHelper.ParseLong(account.AccountID));

                    if (UIHelper.ParseLong(cost.CostCenterId) > 0)
                        recommend.CostCenter = new DbCostCenter(UIHelper.ParseLong(cost.CostCenterId));

                    if (UIHelper.ParseLong(io.IOID) > 0)
                        recommend.IO = new DbInternalOrder(UIHelper.ParseLong(io.IOID));

                    if (DocumentType.Equals(ZoneType.Foreign))
                    {
                        UserControls.Dropdownlist.SS.DB.CurrencyDropdown currencyDropdown = row.FindControl("ctlCurrencyDropdown") as UserControls.Dropdownlist.SS.DB.CurrencyDropdown;
                        if (currencyDropdown.SelectedValue == "")
                        {
                            errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("CurrencyIsRequired"));
                            throw new ServiceValidationException(errors);
                        }

                        TextBox currencyAmount = row.FindControl("ctlTxtCurrencyAmount") as TextBox;

                        recommend.CurrencyID = UIHelper.ParseLong(currencyDropdown.SelectedValue);
                        recommend.CurrencyAmount = UIHelper.ParseDouble(currencyAmount.Text);
                        
                        recommend.ExchangeRate = UIHelper.ParseDouble(exchangeRate.Text);
                        if (IsRepOffice)
                        {
                            recommend.LocalCurrencyAmount = recommend.ExchangeRate * (recommend.CurrencyAmount == (double?)0 ? 1 : recommend.CurrencyAmount);
                            totalAmount += recommend.LocalCurrencyAmount.Value;
                        }
                        else
                        {
                            recommend.Amount = recommend.ExchangeRate * (recommend.CurrencyAmount == (double?)0 ? 1 : recommend.CurrencyAmount);
                            totalAmount += recommend.Amount.Value;
                        }
                    }
                    else
                    {
                        if (IsRepOffice)
                        {
                            recommend.LocalCurrencyAmount = UIHelper.ParseDouble(amount.Text);
                            totalAmount += recommend.LocalCurrencyAmount.Value;
                        }
                        else
                        {
                            recommend.Amount = UIHelper.ParseDouble(amount.Text);
                            totalAmount += recommend.Amount.Value;
                        }
                    }

                    recommend.ReferenceNo = reference.Text;
                    recommend.Description = des.Text;

                    recommend.Description = des.Text;
                    recommend.ReferenceNo = reference.Text;
                    RecommendList.Add(recommend);
                }
            }
            try
            {
                if (RecommendList.Count > 0)
                {
                    if (IsRepOffice)
                    {
                        invoice.TotalBaseAmountLocalCurrency = invoice.NetAmountLocalCurrency = invoice.TotalAmountLocalCurrency = totalAmount;
                    }
                    else
                    {
                        invoice.TotalBaseAmount = invoice.NetAmount = invoice.TotalAmount = totalAmount;
                    }
                    long invoiceId = FnExpenseInvoiceService.AddInvoiceOnTransaction(invoice, this.TransactionId);
                    FnExpenseInvoiceItemService.AddRecommendInvoiceItemOnTransaction(invoiceId, DocumentType, RecommendList, this.TransactionId);
                    ctlSimpleExpenseGridView.DataSource = null;
                    ctlSimpleExpenseGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateExpenseSimple(long ComID)
        {
            foreach (GridViewRow row in ctlSimpleExpenseGridView.Rows)
            {
                UserControls.LOV.SCG.DB.CostCenterField cost = row.FindControl("ctlCostCenterLabelLookup") as UserControls.LOV.SCG.DB.CostCenterField;
                UserControls.LOV.SCG.DB.AccountField account = row.FindControl("ctlAccountLabelLookup") as UserControls.LOV.SCG.DB.AccountField;
                UserControls.LOV.SCG.DB.IOAutoCompleteLookup io = row.FindControl("ctlIOLabelLookup") as UserControls.LOV.SCG.DB.IOAutoCompleteLookup;

                cost.CompanyId = ComID;
                account.CompanyIDofDocument = ComID;
                io.CostCenterId = UIHelper.ParseInt(cost.CostCenterId);
                io.CompanyId = ComID;

                ctlUpdateSimpleExpenseGridView.Update();
            }
        }
        //add by tom 01/07/2009, reset cost center when change company.
        public void ResetCostCenter(long ComID)
        {
            foreach (GridViewRow row in ctlSimpleExpenseGridView.Rows)
            {
                UserControls.LOV.SCG.DB.CostCenterField cost = row.FindControl("ctlCostCenterLabelLookup") as UserControls.LOV.SCG.DB.CostCenterField;
                cost.ResetValue();
                cost.CompanyId = ComID;
                ctlUpdateSimpleExpenseGridView.Update();
            }
        }

        protected void ctlSimpleExpenseGridView_Databound(object sender, EventArgs e)
        {
            if (this.DocumentType.Equals(ZoneType.Domestic))
            {
                ctlSimpleExpenseGridView.Columns[4].Visible = false;
                ctlSimpleExpenseGridView.Columns[5].Visible = false;
                ctlSimpleExpenseGridView.Columns[6].Visible = false;
            }
            else if (this.DocumentType.Equals(ZoneType.Foreign))
            {
                ctlSimpleExpenseGridView.Columns[4].Visible = true;
                ctlSimpleExpenseGridView.Columns[5].Visible = true;
                ctlSimpleExpenseGridView.Columns[6].Visible = true;
            }

            if (IsRepOffice)
            {
                ctlSimpleExpenseGridView.Columns[7].HeaderText = string.Format(GetProgramMessage("AmountLocalCurrency"), "(" + SelectedCurrency + ")");
            }
            else
            {
                ctlSimpleExpenseGridView.Columns[7].HeaderText = string.Format(GetProgramMessage("AmountLocalCurrency"), "(" + CurrencySymbol.THB.ToString() + ")");
            }
        }

        public void GetCurrentSimpleExpense()
        {
            IList<RecommendExpenseData> RecommendList = new List<RecommendExpenseData>();

            foreach (GridViewRow row in ctlSimpleExpenseGridView.Rows)
            {
                RecommendExpenseData item = new RecommendExpenseData();

                UserControls.LOV.SCG.DB.CostCenterField cost = row.FindControl("ctlCostCenterLabelLookup") as UserControls.LOV.SCG.DB.CostCenterField;
                UserControls.LOV.SCG.DB.AccountField account = row.FindControl("ctlAccountLabelLookup") as UserControls.LOV.SCG.DB.AccountField;
                UserControls.LOV.SCG.DB.IOAutoCompleteLookup io = row.FindControl("ctlIOLabelLookup") as UserControls.LOV.SCG.DB.IOAutoCompleteLookup;
                TextBox des = row.FindControl("ctlTxtDescription") as TextBox;
                TextBox amount = row.FindControl("ctlTxtCurrencyAmount") as TextBox;
                TextBox reference = row.FindControl("ctlTxtRefNo") as TextBox;

                if (!string.IsNullOrEmpty(cost.CostCenterId))
                {
                    item.CostCenterID = cost.CostCenterId;
                }
                else
                {
                    item.CostCenterID = string.Empty;
                }

                if (!string.IsNullOrEmpty(account.AccountID))
                {
                    item.ExpenseID = account.AccountID;
                }
                else
                {
                    item.ExpenseID = string.Empty;
                }

                if (!string.IsNullOrEmpty(io.IOID))
                {
                    item.InternalID = io.IOID;
                }
                else
                {
                    item.InternalID = string.Empty;
                }

                if (!string.IsNullOrEmpty(des.Text))
                {
                    item.Description = des.Text;
                }
                else
                {
                    item.Description = string.Empty;
                }

                if (!string.IsNullOrEmpty(amount.Text))
                {
                    item.Amount = amount.Text;
                }
                else
                {
                    item.Amount = string.Empty;
                }

                if (!string.IsNullOrEmpty(reference.Text))
                {
                    item.RefNo = reference.Text;
                }
                else
                {
                    item.RefNo = string.Empty;
                }
                RecommendList.Add(item);
            }

            Session["CurrentRecommendExpense"] = RecommendList;
        }

        public void SetCurrentSimpleExpense()
        {
            if (Session["CurrentRecommendExpense"] != null)
            {
                if (InitialFlag.Equals(FlagEnum.NewFlag) && IsRepOffice)
                {
                    IList<RecommendExpenseData> RecommendList = (IList<RecommendExpenseData>)Session["CurrentRecommendExpense"];

                    if (RecommendList.Count == ctlSimpleExpenseGridView.Rows.Count)
                    {
                        for (int i = 0; i < RecommendList.Count; i++)
                        {
                            GridViewRow row = ctlSimpleExpenseGridView.Rows[i];
                            UserControls.LOV.SCG.DB.CostCenterField cost = row.FindControl("ctlCostCenterLabelLookup") as UserControls.LOV.SCG.DB.CostCenterField;
                            UserControls.LOV.SCG.DB.AccountField account = row.FindControl("ctlAccountLabelLookup") as UserControls.LOV.SCG.DB.AccountField;
                            UserControls.LOV.SCG.DB.IOAutoCompleteLookup io = row.FindControl("ctlIOLabelLookup") as UserControls.LOV.SCG.DB.IOAutoCompleteLookup;
                            TextBox des = row.FindControl("ctlTxtDescription") as TextBox;
                            TextBox amount = row.FindControl("ctlTxtCurrencyAmount") as TextBox;
                            TextBox reference = row.FindControl("ctlTxtRefNo") as TextBox;

                            cost.BindCostCenterControl(UIHelper.ParseLong(RecommendList[i].CostCenterID));
                            account.BindAccountControl(UIHelper.ParseLong(RecommendList[i].ExpenseID));
                            io.BindIOControl(UIHelper.ParseLong(RecommendList[i].InternalID));
                            des.Text = RecommendList[i].Description;
                            amount.Text = RecommendList[i].Amount;
                            reference.Text = RecommendList[i].RefNo;
                        }
                    }
                }
            }
        }
    }
}
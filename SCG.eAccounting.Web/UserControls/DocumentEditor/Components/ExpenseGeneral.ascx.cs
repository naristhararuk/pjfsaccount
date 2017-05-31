using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.Web.Helper;

using SS.Standard.UI;
using SCG.eAccounting.BLL;
using SCG.eAccounting.DTO.DataSet;
using System.Data;
using SCG.DB.DTO;
using SCG.DB.DTO.ValueObject;
using SCG.DB.Query;
using SS.SU.Query;
using SS.SU.DTO;
using SS.DB.Query;
using SS.DB.DTO;
using SS.Standard.Utilities;
using SCG.eAccounting.DTO;
using SS.Standard.WorkFlow.Query;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor.Components
{
    public partial class ExpenseGeneral : BaseUserControl, IEditorComponent
    {
        public ITransactionService TransactionService { get; set; }
        public IFnExpenseInvoiceService FnExpenseInvoiceService { get; set; }
        public IFnExpenseDocumentService FnExpenseDocumentService { get; set; }

        #region Property
        public string Width
        {
            set { ctlExpenseGeneral.Width = value; }
        }
        public string Mode
        {
            get { return ctlMode.Text; }
            set { ctlMode.Text = value; }
        }
        public long RequesterID
        {
            get
            {
                if (ViewState["RequesterID"] != null)
                    return UIHelper.ParseLong(ViewState["RequesterID"].ToString());
                return 0;
            }
            set { ViewState["RequesterID"] = value; }
        }
        public long CompanyID
        {
            get
            {
                if (ViewState["CompanyID"] != null)
                    return UIHelper.ParseLong(ViewState["CompanyID"].ToString());
                return 0;
            }
            set { ViewState["CompanyID"] = value; }
        }
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
            get { return ctlType.Value; }
            set { ctlType.Value = value; }
        }
        public string InitialFlag
        {
            get { return ViewState[ViewStateName.InitialFlag].ToString(); }
            set { ViewState[ViewStateName.InitialFlag] = value; }
        }
        public long DocumentID
        {
            get { return (long)ViewState["DocID"]; }
            set { ViewState["DocID"] = value; }
        }
        public string SelectedCurrency
        {
            get
            {
                if (ViewState["SelectedCurrency"] != null)
                    return string.IsNullOrEmpty(ViewState["SelectedCurrency"].ToString()) ? CurrencySymbol.THB.ToString() : ViewState["SelectedCurrency"].ToString();
                return CurrencySymbol.THB.ToString();
            }
            set { ViewState["SelectedCurrency"] = value; }
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

        #endregion

        private const string invoiceURL = "~/UserControls/DocumentEditor/Components/Invoice.aspx?mode=[mode]&txId={0}&expId={1}&docType={2}&docId={3}";
        private const string perdiemURL = "~/UserControls/DocumentEditor/Components/PerdiemPopup.aspx?mode=[mode]&txId={0}&expId={1}&docType={2}&docId={3}&cp={4}";
        private const string MileageURL = "~/UserControls/DocumentEditor/Components/MileageForm.aspx?mode=[mode]&txId={0}&expId={1}&docType={2}";

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //BaseGridView gridview = (BaseGridView)ctlRepeater.FindControl("ctlInvoiceItem"); 
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region Public Method
        public void BindSimpleExpenseGrid(string expenseType, long requesterId, bool refreshHeaderGrid)
        {
            ctlSimpleExpense.IsRepOffice = this.IsRepOffice;
            ctlSimpleExpense.SelectedCurrency = this.SelectedCurrency;
            ctlSimpleExpense.CompanyID = CompanyID;
            if (ctlSimpleExpense.Visible)
            {
                ctlSimpleExpense.BindSimpleExpense(expenseType, requesterId, refreshHeaderGrid);
            }
            else
            {
                BindExpenseGrid();
            }
            ctlUpdatePanelExpenseGeneral.Update();
        }
        public void InitialControl()
        {
            ctlSimpleExpense.Initialize(this.TransactionId, this.ExpDocumentID, InitialFlag);
        }
        public void BindControl()
        {
            SS.Standard.WorkFlow.DTO.WorkFlow workflow = null;
            long workFlowID = 0;

            if (!string.IsNullOrEmpty(Request.Params["wfid"]) && UIHelper.ParseInt(Request.Params["wfid"]) > 0)
            {
                workFlowID = UIHelper.ParseLong(Request.Params["wfid"]);
                workflow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            }

            if (DocumentType.Equals(ZoneType.Foreign) || this.IsRepOffice)
            {
                ctlAddMilage.Style["display"] = "none";
            }
            else
            {
                ctlAddMilage.Style["display"] = string.Empty;
            }

            if (!InitialFlag.Equals(FlagEnum.ViewFlag) && (workflow == null || (workflow != null && !workflow.CurrentState.Name.Equals(WorkFlowStateFlag.Hold))))
            {
                ctlAddGeneralExpense.Enabled = true;
                ctlAddPerdiem.Enabled = true;
                ctlAddMilage.Enabled = true;
            }
            else
            {
                ctlAddGeneralExpense.Enabled = false;
                ctlAddPerdiem.Enabled = false;
                ctlAddMilage.Enabled = false;
            }
            ctlSimpleExpense.DocumentType = this.DocumentType;
            ctlPerdiemPopupCaller.URL = ShowPerdiemPopup(InitialFlag, null);
            ctlInvoicePopupCaller.URL = String.Format(invoiceURL.Replace("[mode]", FlagEnum.NewFlag), this.TransactionId, this.ExpDocumentID, this.DocumentType, this.DocumentID, string.Empty);
            ctlMileagePopupCaller.URL = ShowMileagePopup(InitialFlag);

            ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(this.TransactionId);
            ExpenseDataSet.FnExpenseDocumentRow expenseDocumentRow = expDs.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);
            this.DocumentID = expenseDocumentRow.DocumentID;

            ctlDivExchangeRateLocalInfo.Style["display"] = "none";
            ctlDivExchangeRateMainInfo.Style["display"] = "none";

            if (!InitialFlag.Equals(FlagEnum.NewFlag) && IsRepOffice)
            {
                try
                {
                    if ((workflow != null && workflow.CurrentState.Ordinal >= 5) && (UserAccount.IsAccountant || UserAccount.IsPayment))
                    {
                        string mainCurrencySymbol = string.Empty;
                        string localCurrencySymbol = string.Empty;
                        if (!expenseDocumentRow.IsLocalCurrencyIDNull())
                        {
                            DbCurrency localCurrency = SsDbQueryProvider.DbCurrencyQuery.FindByIdentity(expenseDocumentRow.LocalCurrencyID);
                            localCurrencySymbol = localCurrency.Symbol;

                        }

                        if (!expenseDocumentRow.IsMainCurrencyIDNull())
                        {
                            DbCurrency mainCurrency = SsDbQueryProvider.DbCurrencyQuery.FindByIdentity(expenseDocumentRow.MainCurrencyID);
                            mainCurrencySymbol = mainCurrency.Symbol;
                        }

                        ctlExRateMainCurrencyLabel.Text = GetProgramMessage("DisplayExchangeRateInfo");
                        ctlExchangeRateMain.Text = UIHelper.BindExchangeRate(expenseDocumentRow.IsExchangeRateMainToTHBCurrencyNull() ? string.Empty : expenseDocumentRow.ExchangeRateMainToTHBCurrency.ToString());
                        ctlExchangeRateMainUnit.Text = CurrencySymbol.THB + "/" + mainCurrencySymbol;

                        ctlExRateLocalCurrencyLabel.Text = GetProgramMessage("DisplayExchangeRateInfo");
                        ctlExchangeRateLocal.Text = UIHelper.BindExchangeRate(expenseDocumentRow.IsExchangeRateForLocalCurrencyNull() ? string.Empty : expenseDocumentRow.ExchangeRateForLocalCurrency.ToString());
                        ctlExchangeRateLocalUnit.Text = localCurrencySymbol + "/" + mainCurrencySymbol;

                        ctlDivExchangeRateMainInfo.Style["display"] = "block";
                        if (!expenseDocumentRow.IsMainCurrencyIDNull() && !expenseDocumentRow.IsLocalCurrencyIDNull() && (expenseDocumentRow.MainCurrencyID != expenseDocumentRow.LocalCurrencyID))
                        {
                            ctlDivExchangeRateLocalInfo.Style["display"] = "block";
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void Initialize(Guid txID, long documentID, string initFlag)
        {
            this.TransactionId = txID;
            this.ExpDocumentID = documentID;
            this.InitialFlag = initFlag;

            ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(this.TransactionId);
            ExpenseDataSet.FnExpenseDocumentRow expenseDocumentRow = expDs.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);
            this.DocumentID = expenseDocumentRow.DocumentID;
            this.InitialControl();
            //this.BindControl();
        }
        #endregion

        #region Button Event
        protected void ctlAddGeneralExpense_Click(object sender, ImageClickEventArgs e)
        {
            // ShowExpenseGeneral() 
            if (!ExpenseDocumentEditor.NotifyUpdateExpense()) return;
            ShowPopup(ctlInvoicePopupCaller.PopupScript);
        }
        protected void ctlAddPerdiem_Click(object sender, ImageClickEventArgs e)
        {
            if (!ExpenseDocumentEditor.NotifyUpdateExpense()) return;
            ShowPopup(ctlPerdiemPopupCaller.PopupScript);
        }
        protected void ctlAddMilage_Click(object sender, ImageClickEventArgs e)
        {
            if (!ExpenseDocumentEditor.NotifyUpdateExpense()) return;
            ShowPopup(ctlMileagePopupCaller.PopupScript);
        }
        #endregion

        protected void ctlRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string filter = string.Empty;
            //ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(this.TransactionId);
            //ExpenseDataSet.FnExpenseInvoiceRow row = expDs.FnExpenseInvoice.FindByInvoiceID(UIHelper.ParseLong(e.CommandArgument.ToString()));
            if (e.CommandName.Equals("EditInovice"))
            {
                PopupCaller popupEdit = (PopupCaller)e.Item.FindControl("ctlEditPopupCaller");
                if (ExpenseDocumentEditor.NotifyUpdateExpense())
                {
                    ShowPopup(popupEdit.PopupScript);
                }
            }
            else if (e.CommandName.Equals("DeleteInvoice"))
            {
                ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(this.TransactionId);
                long invoiceID = UIHelper.ParseLong(e.CommandArgument.ToString());
                ExpenseDataSet.FnExpenseInvoiceRow[] invoice = (ExpenseDataSet.FnExpenseInvoiceRow[])expDs.FnExpenseInvoice.Select(string.Format("InvoiceID  = {0}", e.CommandArgument.ToString()));
                if (invoice.Count() > 0)
                {
                    string invoiceType = invoice[0].InvoiceDocumentType;
                    FnExpenseInvoiceService.DeleteInvoiceOnTransaction(invoiceID, this.TransactionId);
                    if (invoiceType.Equals(InvoiceType.General))
                    {
                        string URL = ShowExpenseGeneral(invoiceID);
                        ctlInvoicePopupCaller.URL = URL.Replace("[mode]", FlagEnum.NewFlag);
                    }
                    else if (invoiceType.Equals(InvoiceType.Perdiem))
                    {
                        string URL = ShowPerdiemPopup(FlagEnum.NewFlag, null);
                        ctlPerdiemPopupCaller.URL = URL;
                    }
                }

                BindExpenseGrid();
                ExpenseDocumentEditor.NotifyPaymentDetailChange();
            }
            else if (e.CommandName.Equals("ViewInvoice"))
            {
                PopupCaller popupView = (PopupCaller)e.Item.FindControl("ctlViewPopupCaller");
                ShowPopup(popupView.PopupScript);
            }
        }

        protected void ctlRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            string url = string.Empty;
            if (e.Item.DataItem != null)
            {
                ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(TransactionId);
                ExpenseDataSet.FnExpenseInvoiceRow invoice = (ExpenseDataSet.FnExpenseInvoiceRow)e.Item.DataItem;
                ExpenseDataSet.FnExpenseDocumentRow expenseDocumentRow = expDs.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);

                PopupCaller popupEdit = (PopupCaller)e.Item.FindControl("ctlEditPopupCaller");
                PopupCaller popupView = (PopupCaller)e.Item.FindControl("ctlViewPopupCaller");
                LinkButton edit = (LinkButton)e.Item.FindControl("ctlEdit");
                LinkButton delete = (LinkButton)e.Item.FindControl("ctlDelete");
                LinkButton view = (LinkButton)e.Item.FindControl("ctlView");
                Literal totalBaseAmount = (Literal)e.Item.FindControl("ctlTotalAmount");
                Literal totalNetAmount = (Literal)e.Item.FindControl("ctlNetAmount");

                if (IsRepOffice)
                {
                    totalBaseAmount.Text = UIHelper.BindDecimal(invoice.TotalBaseAmountLocalCurrency.ToString());
                    totalNetAmount.Text = UIHelper.BindDecimal(invoice.NetAmountLocalCurrency.ToString());
                }
                else
                {
                    totalBaseAmount.Text = UIHelper.BindDecimal(invoice.TotalBaseAmount.ToString());
                    totalNetAmount.Text = UIHelper.BindDecimal(invoice.NetAmount.ToString());
                }

                if (!invoice.IsInvoiceDocumentTypeNull() && invoice.InvoiceDocumentType.Equals(InvoiceType.Perdiem))
                {
                    popupEdit.URL = ShowPerdiemPopup(FlagEnum.EditFlag, invoice.InvoiceID);
                    popupView.URL = ShowPerdiemPopup(FlagEnum.ViewFlag, invoice.InvoiceID);
                }
                if (!invoice.IsInvoiceDocumentTypeNull() && invoice.InvoiceDocumentType.Equals(InvoiceType.General))
                {
                    url = ShowExpenseGeneral(invoice.InvoiceID);
                    popupEdit.URL = url.Replace("[mode]", FlagEnum.EditFlag);
                    popupView.URL = url.Replace("[mode]", FlagEnum.ViewFlag);
                }

                // Show Mileage.
                if (!invoice.IsInvoiceDocumentTypeNull() && invoice.InvoiceDocumentType.Equals(InvoiceType.Mileage))
                {
                    popupEdit.URL = ShowMileagePopup(FlagEnum.EditFlag);
                    popupView.URL = ShowMileagePopup(FlagEnum.ViewFlag);

                }

                SS.Standard.WorkFlow.DTO.WorkFlow workflow = SS.Standard.WorkFlow.Query.WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(this.DocumentID);

                if (InitialFlag.Equals(FlagEnum.ViewFlag))
                {
                    edit.Visible = false;
                    delete.Visible = false;
                    view.Visible = true;
                }
                else
                {
                    edit.Visible = true;
                    view.Visible = false;

                    if (!invoice.IsInvoiceDocumentTypeNull() && invoice.InvoiceDocumentType.Equals(InvoiceType.Mileage))
                        delete.Visible = false;
                    else
                        if (workflow != null && workflow.CurrentState.Name.Equals(WorkFlowStateFlag.Hold))
                        {
                            delete.Visible = false;
                        }
                        else
                        {
                            delete.Visible = true;
                        }

                }

                Literal seq = (Literal)e.Item.FindControl("ctlSeq");

                seq.Text += e.Item.ItemIndex + 1;

                BaseGridView gridview = (BaseGridView)e.Item.FindControl("ctlInvoiceItem");

                string filter = String.Format("InvoiceID = {0}", invoice.InvoiceID);
                gridview.DataSource = expDs.FnExpenseInvoiceItem.Select(filter);
                gridview.DataBind();

            }
        }

        public void BindExpenseGrid()
        {
            ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(TransactionId);
            string filter = String.Format("ExpenseID = {0}", this.ExpDocumentID);
            ctlRepeater.DataSource = expDs.FnExpenseInvoice.Select(filter, "CreDate asc");
            ctlRepeater.DataBind();
            ctlSimpleExpense.Visible = false;
            ctlUpdatePanelExpenseInvoice.Update();
            ctlUpdatePanelExpenseGeneral.Update();
        }
        public string DisplayInvoiceDate(Object obj)
        {
            ExpenseDataSet.FnExpenseInvoiceRow row = (ExpenseDataSet.FnExpenseInvoiceRow)obj;
            return Server.HtmlEncode(UIHelper.BindDate(row["InvoiceDate"].ToString()));
        }
        public string DisplayVendor(Object obj)
        {
            ExpenseDataSet.FnExpenseInvoiceRow row = (ExpenseDataSet.FnExpenseInvoiceRow)obj;
            if (!string.IsNullOrEmpty(row["VendorCode"].ToString()) || !string.IsNullOrEmpty(row["VendorName"].ToString()))
            {
                return Server.HtmlEncode(String.Format("{0}-{1}", row["VendorCode"].ToString(), row["VendorName"].ToString()));
            }
            return string.Empty;
        }
        public string DisplayIO(Object obj)
        {
            ExpenseDataSet.FnExpenseInvoiceItemRow row = (ExpenseDataSet.FnExpenseInvoiceItemRow)obj;
            if (!string.IsNullOrEmpty(row["IOID"].ToString()))
            {
                DbInternalOrder io = ScgDbQueryProvider.DbIOQuery.FindByIdentity(row.IOID);
                if (io != null)
                    return Server.HtmlEncode(io.IONumber);
            }
            return string.Empty;
        }
        public string DisplayAccount(Object obj)
        {
            ExpenseDataSet.FnExpenseInvoiceItemRow row = (ExpenseDataSet.FnExpenseInvoiceItemRow)obj;
            if (!string.IsNullOrEmpty(row["AccountID"].ToString()))
            {
                IList<AccountLang> account = ScgDbQueryProvider.DbAccountLangQuery.FindByDbAccountLangKey(row.AccountID, UserAccount.CurrentLanguageID);
                if (account.Count > 0)
                    return Server.HtmlEncode(String.Format("{0}-{1}", account[0].AccountCode, account[0].AccountName));
            }
            return string.Empty;
        }
        public string DisplayCostCenter(Object obj)
        {
            ExpenseDataSet.FnExpenseInvoiceItemRow row = (ExpenseDataSet.FnExpenseInvoiceItemRow)obj;
            if (!string.IsNullOrEmpty(row["CostCenterID"].ToString()))
            {
                DbCostCenter costCenter = ScgDbQueryProvider.DbCostCenterQuery.FindByIdentity(row.CostCenterID);
                if (costCenter != null)
                    return Server.HtmlEncode(costCenter.CostCenterCode);
            }
            return string.Empty;
        }
        public string DisplayCurrency(Object obj)
        {
            ExpenseDataSet.FnExpenseInvoiceItemRow row = (ExpenseDataSet.FnExpenseInvoiceItemRow)obj;
            if (!string.IsNullOrEmpty(row["CurrencyID"].ToString()))
            {
                DbCurrency currency = SsDbQueryProvider.DbCurrencyQuery.FindByIdentity(UIHelper.ParseShort(row["CurrencyID"].ToString()));
                if (currency != null)
                    return Server.HtmlEncode(currency.Symbol);
            }
            return string.Empty;
        }


        public ExpenseDocumentEditor ExpenseDocumentEditor { get; set; }

        public void SaveExpenseRecommend()
        {
            if (ctlSimpleExpense.Visible)
            {
                ctlSimpleExpense.SaveExpenseRecommend();
                FnExpenseDocumentService.CalculateTotalExpense(this.TransactionId, this.ExpDocumentID, IsRepOffice);
                FnExpenseDocumentService.CalculateDifferenceAmount(this.TransactionId, this.ExpDocumentID, IsRepOffice);
                this.BindExpenseGrid();
                ExpenseDocumentEditor.NotifyPaymentDetailChange();
            }
        }

        private string ShowExpenseGeneral(long invoiceId)
        {
            string url;
            ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(TransactionId);
            ExpenseDataSet.FnExpenseDocumentRow expenseDocumentRow = expDs.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);
            ExpenseDataSet.FnExpenseInvoiceRow invoiceRows = expDs.FnExpenseInvoice.FindByInvoiceID(invoiceId);
            if (invoiceRows != null)
            {
                url = String.Format(string.Concat(invoiceURL, "&invId={4}"), this.TransactionId, this.ExpDocumentID, this.DocumentType, expenseDocumentRow.DocumentID, invoiceId);
            }
            else
            {
                url = String.Format(invoiceURL, this.TransactionId, this.ExpDocumentID, this.DocumentType, expenseDocumentRow.DocumentID);
            }
            return url;
        }
        private string ShowPerdiemPopup(string mode, long? invoiceId)
        {
            ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(this.TransactionId);
            long? perdiemId = null;
            string url = String.Format(perdiemURL, this.TransactionId, this.ExpDocumentID, this.DocumentType, this.DocumentID,Request.QueryString["cp"]);
            string filter = string.Format(" ExpenseID = {0} ", this.ExpDocumentID);
            if (invoiceId.HasValue)
                filter += string.Format(" and InvoiceID = {0} ", invoiceId.Value);
            DataRow[] perdiemRows = expDs.FnExpensePerdiem.Select(filter);

            if (perdiemRows.Length > 0)
            {
                perdiemId = perdiemRows[0].Field<long>("ExpensePerdiemID");

                if (mode.Equals(FlagEnum.NewFlag))
                    mode = FlagEnum.EditFlag;
            }
            else
            {
                if (mode.Equals(FlagEnum.EditFlag))
                    mode = FlagEnum.NewFlag;
            }
            if (perdiemId.HasValue)
                url += String.Format("&perdiemId={0}", perdiemId);
            return url.Replace("[mode]", mode);
        }

        private string ShowMileagePopup(string mode)
        {
            ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(this.TransactionId);
            DataRow[] drs = expDs.FnExpenseMileage.Select(string.Format(" ExpenseID = {0}", this.ExpDocumentID));
            ExpenseDataSet.FnExpenseDocumentRow row = expDs.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);
            string url = string.Empty;
            if ((expDs.FnExpenseMileage != null) && (expDs.FnExpenseMileage.Rows.Count > 0))
            {

                if (drs.Length > 0)
                {
                    url = String.Format(string.Concat(MileageURL, "&mileageId={3}&docId={4}&cp={5}"), this.TransactionId, this.ExpDocumentID, this.DocumentType, drs[0]["ExpenseMileageID"], row.DocumentID,Request.QueryString["cp"]);
                }
                else
                {
                    url = String.Format(string.Concat(MileageURL, "&docId={3}&cp={4}"), this.TransactionId, this.ExpDocumentID, this.DocumentType, row.DocumentID,Request.QueryString["cp"]).Replace("[mode]", FlagEnum.NewFlag);
                }
            }
            else
            {
                url = String.Format(string.Concat(MileageURL, "&docId={3}&cp={4}"), this.TransactionId, this.ExpDocumentID, this.DocumentType, row.DocumentID,Request.QueryString["cp"]).Replace("[mode]", FlagEnum.NewFlag);
            }

            return url.Replace("[mode]", mode);
        }

        public void ResetControl()
        {
            ctlSimpleExpense.Visible = true;
            ctlRepeater.DataSource = null;
            ctlRepeater.DataBind();
            ctlUpdatePanelExpenseGeneral.Update();
        }

        public void FilterCostCenterInGeneralExpense(long ComID)
        {
            ctlSimpleExpense.UpdateExpenseSimple(ComID);
        }
        //add by tom 01/07/2009, reset cost center when change company.
        public void ResetCostCenterInGenaralExpense(long ComID)
        {
            ctlSimpleExpense.ResetCostCenter(ComID);
        }

        protected void ctlInvoiceItem_DataBound(object sender, EventArgs e)
        {
            BaseGridView gridview = (BaseGridView)sender;
            if (DocumentType.Equals(ZoneType.Domestic))
            {

                gridview.Columns[5].Visible = false;
                gridview.Columns[4].Visible = false;
                gridview.Columns[6].Visible = false;
            }
            else
            {
                gridview.Columns[5].Visible = true;
                gridview.Columns[4].Visible = true;
                gridview.Columns[6].Visible = true;
            }

            if (IsRepOffice)
            {
                SS.Standard.WorkFlow.DTO.WorkFlow workflow = null;
                if (!string.IsNullOrEmpty(Request.Params["wfid"]) && UIHelper.ParseInt(Request.Params["wfid"]) > 0)
                {
                    long workFlowID = UIHelper.ParseLong(Request.Params["wfid"]);

                    try
                    {
                        workflow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                gridview.Columns[7].Visible = true;  //local currency
                gridview.Columns[7].HeaderText = string.Format(GetMessage("AmountCurrencyHeaderColumn"), "(" + SelectedCurrency + ")");

                ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(TransactionId);

                ExpenseDataSet.FnExpenseDocumentRow expRow = expDs.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);
                if (!expRow.IsMainCurrencyIDNull())
                {
                    DbCurrency mainCurrency = SsDbQueryProvider.DbCurrencyQuery.FindByIdentity(expRow.MainCurrencyID);
                    if (mainCurrency != null)
                    {
                        gridview.Columns[8].HeaderText = string.Format(GetMessage("AmountCurrencyHeaderColumn"), "(" + mainCurrency.Symbol + ")");
                    }
                }

                if ((workflow != null && workflow.CurrentState.Ordinal >= 5) && (UserAccount.IsAccountant || UserAccount.IsPayment))
                {

                    if (!expRow.IsMainCurrencyIDNull() && !expRow.IsLocalCurrencyIDNull() && (expRow.MainCurrencyID == expRow.LocalCurrencyID))
                    {
                        gridview.Columns[8].Visible = false;  //hide main currency
                    }
                    else
                    {
                        gridview.Columns[8].Visible = true;  //show main currency
                    }
                    gridview.Columns[9].Visible = true;  //THB
                }
                else
                {
                    gridview.Columns[8].Visible = false;
                    gridview.Columns[9].Visible = false;
                }
            }
            else
            {
                gridview.Columns[7].Visible = false;
                gridview.Columns[8].Visible = false;
                gridview.Columns[9].Visible = true;
            }
        }
        protected bool ctlPopupCaller_NotifyPopupCalling(object sender)
        {
            return ExpenseDocumentEditor.NotifyUpdateExpense();
        }
        protected void ctlPopupCaller_NotifyPopupResult(object sender, string action, string value)
        {
            if (action != "ok") return;

            BindExpenseGrid();
            ctlMileagePopupCaller.URL = ShowMileagePopup(InitialFlag);
            ctlPerdiemPopupCaller.URL = ShowPerdiemPopup(InitialFlag, null);
            ExpenseDocumentEditor.NotifyPaymentDetailChange();
        }
        private void ShowPopup(string script)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "ShowPopup", script, true);
        }

        public void GetCurrentSimpleExpense()
        {
            ctlSimpleExpense.GetCurrentSimpleExpense();
        }
    }
}

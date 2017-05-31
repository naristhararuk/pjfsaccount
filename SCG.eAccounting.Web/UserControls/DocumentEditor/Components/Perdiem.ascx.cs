using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using SCG.DB.BLL;
using SCG.DB.DTO;
using SCG.DB.DTO.ValueObject;
using SCG.DB.Query;
using SCG.eAccounting.BLL;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.DataSet;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.Query;
using SCG.eAccounting.Web;
using SCG.eAccounting.Web.Helper;
using SS.DB.BLL;
using SS.DB.DTO;
using SS.DB.DTO.ValueObject;
using SS.DB.Query;
using SS.Standard.UI;
using SS.Standard.Utilities;
using SS.SU.DTO;
using SS.Standard.WorkFlow.Query;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor.Components
{
    public partial class Perdiem : BaseUserControl
    {
        #region properties && user event

        public ITransactionService TransactionService { get; set; }
        public IFnExpenseDocumentService FnExpenseDocumentService { get; set; }
        public IFnExpensePerdiemService FnExpensePerdiemService { get; set; }
        public IFnExpensePerdiemItemService FnExpensePerdiemItemService { get; set; }
        public IFnExpensePerdiemDetailService FnExpensePerdiemDetailService { get; set; }
        public IDbCountryService DbCountryService { get; set; }
        public IDbCostCenterService DbCostCenterService { get; set; }
        public IDbAccountService DbAccountService { get; set; }
        public DbCurrency dbCurrency { get; set; }
        #endregion

        #region global variable && const
        #region footer
        private double TotalNetDayFooter { get; set; }
        private double TotalAmountLocalFooter { get; set; }
        private double TotalAmountFooter { get; set; }
        private double TotalFullDayFooter { get; set; }
        private double TotalHalfDayFooter { get; set; }
        private double TotalAmountTHBFooter { get; set; }
        #endregion

        #region global
        private Guid TxId
        {
            get { return (Guid)ViewState[ViewStateName.TransactionID]; }
            set { ViewState[ViewStateName.TransactionID] = value; }
        }
        private Guid ParentTxId
        {
            get { return (Guid)ViewState[ViewStateName.ParentTxID]; }
            set { ViewState[ViewStateName.ParentTxID] = value; }
        }
        private long ExpenseId
        {
            get { return (long)ViewState["ExpenseId"]; }
            set { ViewState["ExpenseId"] = value; }
        }
        private long ExpensePerdiemId
        {
            get { return (long)ViewState["ExpensePerdiemId"]; }
            set { ViewState["ExpensePerdiemId"] = value; }
        }
        protected string Mode
        {
            get
            {
                if (ViewState["PerdiemMode"] != null)
                {
                    return ViewState["PerdiemMode"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set { ViewState["PerdiemMode"] = value; }
        }
        private long CompanyId
        {
            get
            {
                if (ViewState["CompanyId"] != null)
                {
                    return (long)ViewState["CompanyId"];
                }
                else
                {
                    return (long)-1;
                }
            }
            set { ViewState["CompanyId"] = value; }
        }
        private long RequesterId
        {
            get
            {
                if (ViewState["RequesterId"] != null)
                {
                    return (long)ViewState["RequesterId"];
                }
                else
                {
                    return (long)-1;
                }
            }
            set { ViewState["RequesterId"] = value; }
        }
        private long? PBID
        {
            get
            {
                if (ViewState["PBID"] != null)
                {
                    return (long)ViewState["PBID"];
                }
                else
                {
                    return (long)-1;
                }
            }
            set { ViewState["PBID"] = value; }
        }
        private short? MainCurrencyID
        {
            get
            {
                if (ViewState["MainCurrencyID"] != null)
                {
                    return (short)ViewState["MainCurrencyID"];
                }
                else
                {
                    return (short)-1;
                }
            }
            set { ViewState["MainCurrencyID"] = value; }
        }
        private string PerdiemType
        {
            get
            {
                if (ViewState["PerdiemType"] != null)
                {
                    return ViewState["PerdiemType"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set { ViewState["PerdiemType"] = value; }
        }
        public bool isRepOffice
        {
            get
            {
                if (ViewState["isRepOffice"] == null)
                {
                    return true;
                }
                else
                {
                    return (bool)(ViewState["isRepOffice"]);
                }
            }
            set { ViewState["isRepOffice"] = value; }
        }
        #endregion
        #endregion

        #region load method
        protected void Page_Load(object sender, EventArgs e)
        {
            ctlCostCenterField.OnObjectLookUpCalling += new BaseUserControl.ObjectLookUpCallingEventHandler(CostCenterField_OnObjectLookUpCalling);
            ctlCostCenterField.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(CostCenterField_OnObjectLookUpReturn);
        }
        #endregion

        #region userControl method
        protected void CostCenterField_OnObjectLookUpCalling(object sender, ObjectLookUpCallingEventArgs e)
        {

        }
        protected void CostCenterField_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            if (e.ObjectReturn != null)
            {
                DbCostCenter dbCostcenter = e.ObjectReturn as DbCostCenter;
                ctlInternalLookup.CostCenterId = dbCostcenter.CostCenterID;
                ctlInternalLookup.CostCenterId = dbCostcenter.CostCenterID;
            }
        }
        #endregion

        #region grid method
        #region perdiem item grid
        protected void ctlPerdiemGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("EditItem"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                long perdiemItemId = UIHelper.ParseLong(ctlPerdiemGrid.DataKeys[rowIndex].Value.ToString());
                ctlHiddenFieldPerdiemItemId.Value = perdiemItemId.ToString();

                ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
                ExpenseDataSet.FnExpensePerdiemItemRow row = expDS.FnExpensePerdiemItem.FindByPerdiemItemID(perdiemItemId);

                ctlCalendarFromDate.DateValue = UIHelper.ToDateString(row.FromDate);
                ctlTimeFromTime.TimeValue = UIHelper.ToDateString(row.FromTime, "HH:mm");
                ctlCalendarToDate.DateValue = UIHelper.ToDateString(row.ToDate);
                ctlTimeToTime.TimeValue = UIHelper.ToDateString(row.ToTime, "HH:mm");
                ctlTextBoxAdjustedDay.Text = row.AdjustedDay.ToString();

                if (this.PerdiemType == ZoneType.Foreign)
                {
                    if (!row.IsCountryIDNull()) ctlDropDownListCountry.SelectedValue = row.CountryID.ToString();
                }
                if (!row.IsHalfDayNull()) ctlTextBoxHalfDay.Text = row.HalfDay.ToString();
                if (!row.IsRemarkNull()) ctlTextBoxRemark.Text = row.Remark;
                ToggleButtonItem(false);
            }
            else if (e.CommandName.Equals("DeleteItem"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                long itemId = UIHelper.ParseLong(ctlPerdiemGrid.DataKeys[rowIndex].Value.ToString());

                try
                {
                    FnExpensePerdiemService.UpdateExpensePerdiemTransaction(GetExpensePerdiem(), null, TxId);
                    FnExpensePerdiemItemService.DeleteExpensePerdiemItemTransaction(itemId, TxId);
                    BindData();
                }
                catch (ServiceValidationException ex)
                {
                    this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                }
            }

            OnObjectIsHide(false);
        }
        protected void ctlPerdiemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                #region totalDay
                ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
                ExpenseDataSet.FnExpensePerdiemRow perdiemRow = ds.FnExpensePerdiem.FindByExpensePerdiemID(this.ExpensePerdiemId);
                ExpenseDataSet.FnExpenseDocumentRow expRow = ds.FnExpenseDocument.FindByExpenseID(perdiemRow.ExpenseID);

                Literal fromDateItem = e.Row.FindControl("ctlLabelFromDateItem") as Literal;
                Literal fromTimeItem = e.Row.FindControl("ctlLabelFromTimeItem") as Literal;
                Literal toDateItem = e.Row.FindControl("ctlLabelToDateItem") as Literal;
                Literal toTimeItem = e.Row.FindControl("ctlLabelToTimeItem") as Literal;

                Literal totalDayItem = e.Row.FindControl("ctlLabelTotalDayItem") as Literal;

                DateTime fromDateTime = FnExpensePerdiemItemService.ConvertDateTime(UIHelper.ParseDate(fromDateItem.Text), UIHelper.ParseDate(fromTimeItem.Text, "HH:mm"));
                DateTime toDateTime = FnExpensePerdiemItemService.ConvertDateTime(UIHelper.ParseDate(toDateItem.Text), UIHelper.ParseDate(toTimeItem.Text, "HH:mm"));

                totalDayItem.Text = FnExpensePerdiemItemService.CalculateTotalDay(fromDateTime, toDateTime, this.PerdiemType).ToString("#,##0.0");
                #endregion

                #region amount
                Literal netDayItem = e.Row.FindControl("ctlLabelNetDayItem") as Literal;
                Literal amountItem = e.Row.FindControl("ctlLabelAmountItem") as Literal;
                Literal amountMainItem = e.Row.FindControl("ctlLabelMainAmountItem") as Literal;
                double perdiemaRate = UIHelper.ParseDouble(ctlTextboxPerdiemRate.Text);
                if (!isRepOffice)
                {
                    amountItem.Text = FnExpensePerdiemItemService.CalculateAmount(UIHelper.ParseDouble(netDayItem.Text), perdiemaRate).ToString("#,##0.00");
                }
                else
                {
                    amountMainItem.Text = FnExpensePerdiemItemService.CalculateLocalAmount(UIHelper.ParseDouble(netDayItem.Text), perdiemaRate, 1).ToString("#,##0.00");
                }
                #endregion

                #region fullDay

                Literal halfDayItem = e.Row.FindControl("ctlLabelHalfDayItem") as Literal;
                Literal fullDayItem = e.Row.FindControl("ctlLabelFullDayItem") as Literal;

                fullDayItem.Text = FnExpensePerdiemItemService.CalculateFullDay(UIHelper.ParseDouble(netDayItem.Text), UIHelper.ParseDouble(halfDayItem.Text)).ToString("#,##0.00");

                #endregion


                ImageButton editBtn = e.Row.FindControl("ctlEdit") as ImageButton;
                ImageButton deleteBtn = e.Row.FindControl("ctlDelete") as ImageButton;
                ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
                ExpenseDataSet.FnExpenseDocumentRow expenseDocumentRow = expDS.FnExpenseDocument.FindByExpenseID(this.ExpenseId);
                SS.Standard.WorkFlow.DTO.WorkFlow workflow = SS.Standard.WorkFlow.Query.WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(expenseDocumentRow.DocumentRow.DocumentID);
                if (workflow != null)
                {
                    SS.Standard.WorkFlow.DTO.WorkFlow wf = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workflow.WorkFlowID);
                    if (wf != null && wf.CurrentState != null && wf.CurrentState.Name != null)
                    {
                        if (wf.CurrentState.Name.Equals(WorkFlowStateFlag.Hold))
                        {
                            deleteBtn.Visible = false;
                        }
                        else deleteBtn.Visible = true;
                    }
                    else deleteBtn.Visible = true;
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                #region totalFooter
                Literal netDayFooter = e.Row.FindControl("ctlLabelNetDayFooter") as Literal;
                Literal amountFooter = e.Row.FindControl("ctlLabelAmountFooter") as Literal;
                Literal amountLocalFooter = e.Row.FindControl("ctlLabelMainAmountFooter") as Literal;

                netDayFooter.Text = TotalNetDayFooter.ToString("#,##0.0");
                amountFooter.Text = TotalAmountFooter.ToString("#,##0.00");
                amountLocalFooter.Text = TotalAmountLocalFooter.ToString("#,##0.00");
                Literal fullDayFooter = e.Row.FindControl("ctlLabelFullDayFooter") as Literal;
                Literal halfDayFooter = e.Row.FindControl("ctlLabelHalfDayFooter") as Literal;

                fullDayFooter.Text = TotalFullDayFooter.ToString("#,##0.0");
                halfDayFooter.Text = TotalHalfDayFooter.ToString("#,##0.0");

                #endregion
            }
        }
        #endregion

        #region perdiem detail grid
        protected void ctlPerdiemDetailGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("DeleteItem"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                long perdiemDetailItemId = UIHelper.ParseLong(ctlPerdiemDetailGrid.DataKeys[rowIndex].Value.ToString());

                try
                {
                    FnExpensePerdiemService.UpdateExpensePerdiemTransaction(GetExpensePerdiem(), null, TxId);
                    FnExpensePerdiemDetailService.DeleteExpensePerdiemDetailTransaction(perdiemDetailItemId, TxId);
                    BindData();
                    //ctlPanelPerdiemDetail.Visible = true;
                }
                catch (ServiceValidationException ex)
                {
                    this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                }
            }
            OnObjectIsHide(false);
        }



        protected void ctlPerdiemDetailGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal exchangeRateItem = e.Row.FindControl("ctlLabelExchangeRatePerdiemDetailItem") as Literal;
                Literal amountItem = e.Row.FindControl("ctlLabelAmountPerdiemDetailItem") as Literal;
                Literal amountTHBItem = e.Row.FindControl("ctlLabelAmountTHBPerdiemDetailItem") as Literal;
                double tmp = FnExpensePerdiemDetailService.CalculateAmountTHB(UIHelper.ParseDouble(exchangeRateItem.Text), UIHelper.ParseDouble(amountItem.Text));
                TotalAmountTHBFooter += tmp;
                amountTHBItem.Text = tmp.ToString("#,##0.00");
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                Literal amountTHBFooter = e.Row.FindControl("ctlLabelAmountTHBPerdiemDetailFooter") as Literal;
                amountTHBFooter.Text = TotalAmountTHBFooter.ToString("#,##0.00");
            }
        }
        #endregion
        #endregion

        #region button method

        #region perdirm item
        protected void ctlAddItem_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                FnExpensePerdiem perdiem = GetExpensePerdiem();
                FnExpensePerdiemItem perdiemItem = GetExpensePerdiemItem();
                if (perdiemItem.FromDate.HasValue && perdiemItem.FromTime.HasValue && perdiemItem.ToDate.HasValue && perdiemItem.ToTime.HasValue)
                {
                    FnExpensePerdiemService.ValidationDuplicateDateTimeline(perdiem.ExpensePerdiemID, TxId, perdiemItem);
                }
                FnExpensePerdiemService.UpdateExpensePerdiemTransaction(perdiem, perdiemItem.CountryZoneID, TxId);
                FnExpensePerdiemItemService.AddExpensePerdiemItemTransaction(perdiemItem, TxId);
                BindData();
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
            //OnObjectIsHide(false);
        }
        protected void ctlUpdateItem_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                FnExpensePerdiem perdiem = GetExpensePerdiem();
                FnExpensePerdiemItem perdiemItem = GetExpensePerdiemItem();
                FnExpensePerdiemService.ValidationDuplicateDateTimeline(perdiem.ExpensePerdiemID, TxId, perdiemItem);
                FnExpensePerdiemService.UpdateExpensePerdiemTransaction(perdiem, perdiemItem.CountryZoneID, TxId);
                FnExpensePerdiemItemService.UpdateExpensePerdiemItemTransaction(perdiemItem, TxId);
                BindData();
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
            //OnObjectIsHide(false);
        }
        protected void ctlCancelItem_Click(object sender, ImageClickEventArgs e)
        {
            ClearPerdiemItem();
            //OnObjectIsHide(false);
        }
        #endregion

        #region perdiem detail
        protected void ctlImageButtonPerdiemDetail_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                FnExpensePerdiemService.UpdateExpensePerdiemTransaction(GetExpensePerdiem(), null, TxId);
                FnExpensePerdiemDetailService.AddExpensePerdiemDetailTransaction(GetFnExpensePerdiemDetail(), TxId);
                ctlCurrency.ResetControl();
                BindData();
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
            OnObjectIsHide(false);
        }
        #endregion

        #region perdiem
        protected void ctlCommit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                FnExpensePerdiem perdiem = GetExpensePerdiem();
                FnExpensePerdiemItem perdiemitem = GetExpensePerdiemItem();
                bool isCopy;
                if (Request.QueryString["cp"] != null && UIHelper.ParseLong(Request.QueryString["cp"]) == 1)
                {
                    isCopy = true;
                }
                else
                {
                    isCopy = false;
                }
                FnExpensePerdiemService.ValidationSaveDuplicateDateTimeline(TxId, "Perdiem", perdiem.Expense.ExpenseID, isCopy);
                FnExpensePerdiemService.UpdateExpensePerdiemTransaction(GetExpensePerdiem(), null, TxId);
                TransactionService.Commit(TxId);
                //NotifyPopUpReturn(new PopUpReturnArgs(PopUpReturnType.OK, null));
                Hide();
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Submit", "notifyPopupResult('ok');", true);
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                //OnObjectIsHide(false);
            }
        }
        protected void ctlPopupCancel_Click(object sender, ImageClickEventArgs e)
        {
            TransactionService.Rollback(TxId);
            //NotifyPopUpReturn(new PopUpReturnArgs(PopUpReturnType.Cancel, null));
            Hide();
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Close", "notifyPopupResult('cancel');", true);
        }
        protected void ctlViewClose_Click(object sender, ImageClickEventArgs e)
        {
            //NotifyPopUpReturn(new PopUpReturnArgs(PopUpReturnType.Cancel, null));
            Hide();
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Close", "notifyPopupResult('cancel');", true);
        }
        protected void ctlPerdiemDetail_Click(object sender, ImageClickEventArgs e)
        {
            //ctlPanelPerdiemDetail.Visible = !ctlPanelPerdiemDetail.Visible;
            //OnObjectIsHide(false);
        }
        #endregion
        #endregion

        #region public && protected method
        public void Initialize(string initFlag, long? lineItemID, long expenseId, Guid txID)
        {
            this.ParentTxId = txID;
            this.TxId = TransactionService.Begin(ParentTxId);
            this.Mode = initFlag;
            this.ExpenseId = expenseId;
            this.PerdiemType = FnExpenseDocumentService.GetExpenseType(expenseId, TxId);

            switch (initFlag)
            {
                case FlagEnum.NewFlag:

                    FnExpensePerdiem expensePerdiem = new FnExpensePerdiem();
                    expensePerdiem.Expense = new FnExpenseDocument(expenseId);
                    expensePerdiem.UpdPgm = ProgramCode;
                    this.ExpensePerdiemId = FnExpensePerdiemService.AddExpensePerdiemTransaction(expensePerdiem, TxId);
                    break;
                case FlagEnum.EditFlag:
                    this.ExpensePerdiemId = lineItemID.Value;
                    break;
                case FlagEnum.ViewFlag:
                    this.ExpensePerdiemId = lineItemID.Value;
                    break;
                default:
                    break;
            }

            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
            ExpenseDataSet.FnExpenseDocumentRow rowExpDoc = expDS.FnExpenseDocument.FindByExpenseID(expenseId);
            ExpenseDataSet.DocumentRow rowDoc = expDS.Document.FindByDocumentID(rowExpDoc.DocumentID);
            this.CompanyId = rowDoc.CompanyID;
            this.RequesterId = rowDoc.RequesterID;
            long? requesterCompanyID = null;
            if (!rowExpDoc.IsPBIDNull())
            {
                this.PBID = rowExpDoc.PBID;
            }

            isRepOffice = rowExpDoc.IsIsRepOfficeNull() ? false : rowExpDoc.IsRepOffice;

            if (isRepOffice)
            {
                getCurrencySymbol(Convert.ToInt16(rowExpDoc.IsLocalCurrencyIDNull() ? 0 : rowExpDoc.LocalCurrencyID));
                ctlPerdiemRateUnit.Text = string.Format(GetProgramMessage("PerdiemRatePerDayLbl"), dbCurrency == null ? CurrencySymbol.THB.ToString() : dbCurrency.Symbol);

                DbCurrency localCurrency = SsDbQueryProvider.DbCurrencyQuery.FindByIdentity(Convert.ToInt16(rowExpDoc.IsLocalCurrencyIDNull() ? 0 : rowExpDoc.LocalCurrencyID));
                ctlLabelCalculateName7.Text = string.Format(GetProgramMessage("GrandTotalLabel"), localCurrency == null ? CurrencySymbol.THB.ToString() : localCurrency.Symbol);
            }
            else
            {
                ctlPerdiemRateUnit.Text = string.Format(GetProgramMessage("PerdiemRatePerDayLbl"), CurrencySymbol.THB.ToString());
                ctlLabelCalculateName7.Text = string.Format(GetProgramMessage("GrandTotalLabel"), CurrencySymbol.THB.ToString());
            }

            SuUser requester = SS.SU.Query.QueryProvider.SuUserQuery.FindByIdentity(this.RequesterId);
            if (requester.Company != null)
                requesterCompanyID = requester.Company.CompanyID;

            if (initFlag.Equals(FlagEnum.NewFlag))
            {
                if ((!this.PerdiemType.Equals(ZoneType.Foreign) || rowDoc.CompanyID == requesterCompanyID))
                {
                    if (requester != null && requester.CostCenter != null)
                    {
                        ctlCostCenterField.BindCostCenterControl(requester.CostCenter.CostCenterID);
                        ctlInternalLookup.CostCenterId = requester.CostCenter.CostCenterID;
                    }
                    SetDefualtExpenseCode();
                }

            }

            ctlCostCenterField.CompanyId = this.CompanyId;
            ctlExpenseField.CompanyIDofDocument = this.CompanyId;
            ctlInternalLookup.CompanyId = this.CompanyId;

            ctlExpenseField.WithoutExpenseCode = ParameterServices.AccountMileageExtra + ","
               + ParameterServices.AccountPerdiem + "," + ParameterServices.AccountPerdiem_DM;

            BindData();
            ctlUpdatePanelPrediemParentData.Update();
        }
        public void getCurrencySymbol(short currencyid)
        {
            dbCurrency = SsDbQueryProvider.DbCurrencyQuery.FindByIdentity(currencyid);
        }
        public void SetDefualtExpenseCode()
        {
            string expenseCode = string.Empty;

            if (this.PerdiemType.Equals(ZoneType.Domestic))
            {
                expenseCode = ParameterServices.AccountOfficialPerdiem_DM;
            }
            else if (this.PerdiemType.Equals(ZoneType.Foreign))
            {
                expenseCode = ParameterServices.AccountOfficialPerdiem;
            }

            DbAccount account = ScgDbQueryProvider.DbAccountQuery.FindAccountByAccountCodeExpenseGroup(expenseCode, ctlExpenseField.CompanyIDofDocument);
            if (account != null)
            {
                ctlExpenseField.BindAccountControl(account.AccountID);
            }
        }

        public void Show()
        {
            CallOnObjectLookUpCalling();
            UpdatePanel1.Update();
        }

        public void Hide()
        {
            UpdatePanel1.Update();
        }
        protected string DisplayCountry(Object obj)
        {
            ExpenseDataSet.FnExpensePerdiemItemRow row = (ExpenseDataSet.FnExpensePerdiemItemRow)((DataRowView)obj).Row;
            string name = string.Empty;

            if (!row.IsCountryIDNull())
            {
                CountryLang country = ScgDbQueryProvider.DbCountryLangQuery.FindByDbCountryLangKey((short)row.CountryID, UserAccount.CurrentLanguageID);
                if (country != null) name = Server.HtmlEncode(country.CountryName);
            }

            return name;
        }
        protected string DisplayZone(Object obj)
        {
            ExpenseDataSet.FnExpensePerdiemItemRow row = (ExpenseDataSet.FnExpensePerdiemItemRow)((DataRowView)obj).Row;
            string name = string.Empty;

            if (!row.IsCountryZoneIDNull())
            {
                DbZoneResult zone = SsDbQueryProvider.DbZoneLangQuery.FindByDbZoneLangKey((short)row.CountryZoneID, UserAccount.CurrentLanguageID);
                if (zone != null) name = Server.HtmlEncode(zone.ZoneName);
            }

            return name;
        }
        protected string DisplayCurrency(Object obj)
        {
            ExpenseDataSet.FnExpensePerdiemDetailRow row = (ExpenseDataSet.FnExpensePerdiemDetailRow)((DataRowView)obj).Row;
            string name = string.Empty;

            if (!row.IsCurrencyIDNull())
            {
                DbCurrency currency = SsDbQueryProvider.DbCurrencyQuery.FindByIdentity((short)row.CurrencyID);
                if (currency != null) name = Server.HtmlEncode(currency.Symbol);
            }

            return name;
        }
        #endregion

        #region private methid
        private void BindGridView()
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
            DataView view = new DataView(expDS.FnExpensePerdiemItem);

            view.RowFilter = "ExpensePerdiemID=" + this.ExpensePerdiemId;
            view.Sort = "FromDate ASC";

            ctlPerdiemGrid.DataSource = view;
            ctlPerdiemGrid.DataBind();
            UpdatePanelGridView.Update();
        }
        private void BindPerdiemDetailGridView()
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
            DataView view = new DataView(expDS.FnExpensePerdiemDetail);

            view.RowFilter = "ExpensePerdiemID=" + this.ExpensePerdiemId;

            ctlPerdiemDetailGrid.DataSource = view;
            TotalAmountTHBFooter = 0;
            ctlPerdiemDetailGrid.DataBind();
            ctlUpdatePanelPerdiemDetailGrid.Update();
        }
        private void ClearPerdiemItem()
        {
            ToggleButtonItem(true);
            ctlCalendarFromDate.DateValue = string.Empty;
            ctlCalendarToDate.DateValue = string.Empty;
            ctlTimeFromTime.TimeValue = string.Empty;
            ctlTimeToTime.TimeValue = string.Empty;
            ctlTextBoxAdjustedDay.Text = string.Empty;
            ctlTextBoxHalfDay.Text = string.Empty;
            ctlTextBoxRemark.Text = string.Empty;

            #region dropdownlist country
            IList<CountryLang> countryLangList = ScgDbQueryProvider.DbCountryLangQuery.GetAllCountryLangByLang(UserAccount.CurrentLanguageID, RequesterId);
            ctlDropDownListCountry.DataSource = countryLangList;
            ctlDropDownListCountry.DataTextField = "CountryName";
            ctlDropDownListCountry.DataValueField = "CountryID";
            ctlDropDownListCountry.DataBind();

            ctlDropDownListCountry.Items.Insert(0, new ListItem(GetProgramMessage("Other"), string.Empty));
            #endregion
        }
        private void ToggleButtonItem(bool visible)
        {
            ctlAddItem.Visible = visible;
            ctlUpdateItem.Visible = !visible;
            ctlCancelItem.Visible = !visible;
        }
        private FnExpensePerdiem GetExpensePerdiem()
        {
            FnExpensePerdiem perdiem = new FnExpensePerdiem(this.ExpensePerdiemId);
            perdiem.Expense = new FnExpenseDocument(this.ExpenseId);
            string expenseCode = string.Empty;
            if (!string.IsNullOrEmpty(ctlCostCenterField.CostCenterId.Trim()))
            {
                perdiem.CostCenter = new DbCostCenter(UIHelper.ParseLong(ctlCostCenterField.CostCenterId));
            }

            if (!string.IsNullOrEmpty(ctlInternalLookup.IOID.Trim()))
            {
                perdiem.IO = new DbInternalOrder(UIHelper.ParseLong(ctlInternalLookup.IOID));
            }
            perdiem.SaleOrder = ctlTextBoxSaleOrder.Text.Trim();
            perdiem.SaleItem = ctlTextBoxSaleItem.Text.Trim();
            perdiem.Description = ctlTextboxDescription.Text.Trim();
            perdiem.PerdiemRate = UIHelper.ParseDouble(ctlTextboxPerdiemRate.Text.Trim());
            perdiem.ReferenceNo = ctlTextboxReferenceNo.Text.Trim();
            perdiem.UpdPgm = ProgramCode;

            if (this.PerdiemType == ZoneType.Foreign)
            {
                perdiem.PerdiemRateUSD = UIHelper.ParseDouble(ctlTextBoxPerdiemRateNormalZoneUSD.Text);
                perdiem.PerdiemRateUSDHigh = UIHelper.ParseDouble(ctlTextBoxPerdiemRateHighZoneUSD.Text);
                perdiem.PerdiemRateGov = UIHelper.ParseDouble(ctlTextBoxPerdiemRateNormalZoneTHB.Text);
                perdiem.PerdiemRateGovHigh = UIHelper.ParseDouble(ctlTextBoxPerdiemRateHighZoneTHB.Text);
                perdiem.ExchangeRate = UIHelper.ParseDouble(ctlExchangeRate.Text.Trim());
                perdiem.PerdiemRateUSDThaiZone = UIHelper.ParseDouble(ctlPerdiemRateThaiZoneTextBox.Text);
            }

            //Comment By Noom จะ Error ถ้า ctlInternalLookup.CostCenterId เป็น NULL เนื่องมาจาก ไม่ได้เลือกค่า Internal Lookup
            //ซึ้ง perdiem.CostCenter ก็ได้ค่ามาจาก Code ด้านบนอยู่แล้ว
            //perdiem.CostCenter = DbCostCenterService.FindByIdentity((long)ctlInternalLookup.CostCenterId);

            if (!string.IsNullOrEmpty(ctlExpenseField.AccountID))
            {
                perdiem.Account = new DbAccount(UIHelper.ParseLong(ctlExpenseField.AccountID));
            }

            return perdiem;
        }
        private FnExpensePerdiemItem GetExpensePerdiemItem()
        {
            FnExpensePerdiemItem perdiemItem = new FnExpensePerdiemItem();
            perdiemItem.ExpensePerdiem = new FnExpensePerdiem(this.ExpensePerdiemId);

            if (!ctlAddItem.Visible)
            {
                perdiemItem.PerdiemItemID = UIHelper.ParseLong(ctlHiddenFieldPerdiemItemId.Value);
            }
            try
            {
                perdiemItem.FromDate = ctlCalendarFromDate.Value;
                perdiemItem.ToDate = ctlCalendarToDate.Value;
            }
            catch (FormatException fex)
            {
                this.ValidationErrors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("Invalid_Date_Format"));
            }

            perdiemItem.FromTime = ctlTimeFromTime.Value;
            perdiemItem.ToTime = ctlTimeToTime.Value;

            if (this.PerdiemType == ZoneType.Foreign)
            {
                perdiemItem.FromTime = UIHelper.ParseDate("00:00", "HH:mm").Value;
                perdiemItem.ToTime = UIHelper.ParseDate("00:00", "HH:mm").Value;

                if (ctlDropDownListCountry.SelectedIndex == 0)
                {
                    perdiemItem.CountryZoneID = (long)CountryZonePerdiem.NormalZone;
                }
                else
                {
                    DbCountry country = DbCountryService.FindByIdentity(UIHelper.ParseShort(ctlDropDownListCountry.SelectedValue));
                    perdiemItem.CountryID = country == null ? null : (short?)country.CountryID;
                    string countryCode = country == null ? string.Empty : country.CountryCode;
                    if (!isRepOffice)
                    {
                        perdiemItem.CountryZoneID = ScgeAccountingQueryProvider.FnPerdiemProfileCountryQuery.FindCountryZoneID(country.CountryID, RequesterId);
                    }
                    else
                    {
                        if (countryCode == "TH")
                        {
                            perdiemItem.CountryZoneID = (long)CountryZonePerdiem.ThaiZone;
                        }
                        else
                        {
                            perdiemItem.CountryZoneID = ScgeAccountingQueryProvider.FnPerdiemProfileCountryQuery.FindCountryZoneID(country.CountryID, RequesterId);
                        }
                    }
                }
            }

            perdiemItem.HalfDay = UIHelper.ParseDouble(ctlTextBoxHalfDay.Text);
            perdiemItem.Remark = ctlTextBoxRemark.Text.Trim();

            perdiemItem.AdjustedDay = UIHelper.ParseDouble(ctlTextBoxAdjustedDay.Text);
            perdiemItem.UpdPgm = ProgramCode;

            return perdiemItem;
        }

        protected void ctlDropDownListCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            ctlTextBoxRemark.Text = string.Empty;
        }

        private FnExpensePerdiemDetail GetFnExpensePerdiemDetail()
        {
            FnExpensePerdiemDetail perdiemDet = new FnExpensePerdiemDetail();

            perdiemDet.ExpensePerdiem = new FnExpensePerdiem(this.ExpensePerdiemId);
            perdiemDet.Description = ctlTextBoxPerdiemDetailDecription.Text.Trim();
            perdiemDet.CurrencyID = UIHelper.ParseLong(ctlCurrency.SelectedValue);
            perdiemDet.ExchangeRate = UIHelper.ParseDouble(ctlTextBoxExchangeRate.Text);
            perdiemDet.Amount = UIHelper.ParseDouble(ctlTextBoxPerdiemDetailAmount.Text);

            return perdiemDet;
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!Page.IsPostBack)
            {
                ctlPerdiemGrid.DataBind();

            }
        }
        protected void ctlPerdiemGrid_DataBound(object sender, EventArgs e)
        {
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
            ExpenseDataSet.FnExpensePerdiemRow perdiemRow = ds.FnExpensePerdiem.FindByExpensePerdiemID(this.ExpensePerdiemId);
            ExpenseDataSet.FnExpenseDocumentRow expRow = ds.FnExpenseDocument.FindByExpenseID(perdiemRow.ExpenseID);
            if (!expRow.IsLocalCurrencyIDNull())
            {
                getCurrencySymbol(expRow.LocalCurrencyID);
                ctlPerdiemGrid.Columns[7].HeaderText = string.Format(GetProgramMessage("ctlLabelMainAmountItem"), "(" + dbCurrency.Symbol + ")");
                //ctlPerdiemGrid.HeaderRow.Cells[7].Text = string.Format(GetProgramMessage("ctlLabelMainAmountItem"), "(" + dbCurrency.Symbol + ")");
            }
            else
            {
                ctlPerdiemGrid.Columns[7].HeaderText = string.Format(GetProgramMessage("ctlLabelMainAmountItem"), "");
                //ctlPerdiemGrid.HeaderRow.Cells[7].Text = string.Format(GetProgramMessage("ctlLabelMainAmountItem"), "");
            }
            if (this.PerdiemType == ZoneType.Domestic)
            {
                ctlPerdiemGrid.Columns[1].Visible = true;   //From Time
                ctlPerdiemGrid.Columns[3].Visible = true;   // To Time
                if (isRepOffice)
                {
                    ctlPerdiemGrid.Columns[7].Visible = true;   //Amount Main Currency
                    ctlPerdiemGrid.Columns[8].Visible = false;  //Amount THB
                }
                else
                {
                    ctlPerdiemGrid.Columns[7].Visible = false;  //Amount Main Currency
                    ctlPerdiemGrid.Columns[8].Visible = false;  //Amount THB
                }
                ctlPerdiemGrid.Columns[9].Visible = false;  //Full Day
                ctlPerdiemGrid.Columns[10].Visible = false; //Half Day
                ctlPerdiemGrid.Columns[11].Visible = false; //Country Zone
                ctlPerdiemGrid.Columns[12].Visible = false; //Country Name
                ctlPerdiemGrid.Columns[13].Visible = true; //Remark
            }
            else
            {
                ctlPerdiemGrid.Columns[1].Visible = false;
                ctlPerdiemGrid.Columns[3].Visible = false;
                ctlPerdiemGrid.Columns[7].Visible = false;
                ctlPerdiemGrid.Columns[8].Visible = false;
                ctlPerdiemGrid.Columns[9].Visible = true;
                ctlPerdiemGrid.Columns[10].Visible = true;
                ctlPerdiemGrid.Columns[11].Visible = true;
                ctlPerdiemGrid.Columns[12].Visible = true;
                ctlPerdiemGrid.Columns[13].Visible = true;
            }
        }

        private void BindData()
        {
            #region Set && Hide Perdiem Type

            if (this.PerdiemType == ZoneType.Domestic)
            {
                ctlPanelExchangeRate.Visible = false;
                ctlPanelFormTime.Visible = true;
                ctlPanelToTime.Visible = true;
                ctlPanelHalfDay.Visible = false;
                ctlPanelCountry.Visible = false;
                ctlPanelCalculate.Visible = true;
                ctlPanelPerdiemPersonalExpense.Visible = false;
                ctlFieldSetModeManage.Style["width"] = "800px";
                ctlPerdiemRateTR.Visible = true;
                ctlPanelCalculateForeign.Visible = false;
                ctlnoteDomestic.Visible = true;
                ctlNoteForeign.Visible = false;
                ctlremarkDomestic.Visible = true;
                ctlremarkForeign.Visible = false;
            }
            else
            {
                ctlPanelExchangeRate.Visible = true;
                ctlPanelFormTime.Visible = false;
                ctlPanelToTime.Visible = false;
                ctlPanelCountry.Visible = true;
                ctlPanelCalculate.Visible = true;
                ctlPanelPerdiemPersonalExpense.Visible = true;
                ctlFieldSetModeManage.Style["width"] = "800px";
                ctlPerdiemRateTR.Visible = false;
                ctlPanelCalculateDomestic.Visible = false;
                ctlnoteDomestic.Visible = false;
                ctlNoteForeign.Visible = true;
                ctlremarkDomestic.Visible = false;
                ctlremarkForeign.Visible = true;
            }

            #endregion
            #region Set && Hide Perdiem Mode

            ctlViewClose.Visible = false;
            ctlCommit.Visible = true;
            ctlPopupCancel.Visible = true;
            ctlPanelModeManage.Visible = true;
            ctlPerdiemGrid.Columns[14].Visible = true;
            ctlPanelPerdiemInput.Visible = true;
            ctlPerdiemDetailGrid.Columns[5].Visible = true;

            if (this.Mode == FlagEnum.ViewFlag.ToString())
            {
                ctlPanelModeManage.Visible = false;
                ctlPerdiemGrid.Columns[14].Visible = false;
                ctlCommit.Visible = false;
                ctlPopupCancel.Visible = false;
                ctlViewClose.Visible = true;
                ctlPanelPerdiemInput.Visible = false;
                ctlPerdiemDetailGrid.Columns[5].Visible = false;
            }
            #endregion
            #region Clear PerdiemData
            //ctlCostCenterField.CostCenterId = string.Empty;
            //ctlExpenseField.AccountID = string.Empty;
            //ctlInternalLookup.IOID = string.Empty;

            ctlTextBoxSaleOrder.Text = string.Empty;
            ctlTextBoxSaleItem.Text = string.Empty;
            ctlTextboxDescription.Text = string.Empty;
            ctlTextboxPerdiemRate.Text = string.Empty;
            ctlTextboxReferenceNo.Text = string.Empty;
            ctlLabelPerdiemPrivateAmount.Text = string.Empty;

            ctlLabelExchangeRateForPerdiemCalculate.Text = string.Empty;
            ctlTextBoxPerdiemRateNormalZoneUSD.ReadOnly = false;
            ctlTextBoxPerdiemRateNormalZoneTHB.ReadOnly = false;
            ctlTextBoxPerdiemRateHighZoneUSD.ReadOnly = false;
            ctlTextBoxPerdiemRateHighZoneTHB.ReadOnly = false;
            //ctlTextBoxPerdiemRateNormalZoneUSD.Text = string.Empty;
            //ctlTextBoxPerdiemRateNormalZoneTHB.Text = string.Empty;
            //ctlTextBoxPerdiemRateHighZoneUSD.Text = string.Empty;
            //ctlTextBoxPerdiemRateHighZoneTHB.Text = string.Empty;

            ctlLabelTotalFullDayPerdiem.Text = string.Empty;
            ctlLabelFullDayPerdiemRate.Text = string.Empty;
            ctlLabelTotalFullDayPerdiemAmount.Text = string.Empty;
            ctlLabelTotalHalfDayPerdiem.Text = string.Empty;
            ctlLabelHalfDayPerdiemRate.Text = string.Empty;
            ctlLabelTotalHalfDayPerdiemAmount.Text = string.Empty;
            ctlLabelPerdiemTotalAmount.Text = string.Empty;

            ctlLabelTotalFullDayPerdiemHigh.Text = string.Empty;
            ctlLabelFullDayPerdiemRateHigh.Text = string.Empty;
            ctlLabelTotalFullDayPerdiemAmountHigh.Text = string.Empty;
            ctlLabelTotalHalfDayPerdiemHigh.Text = string.Empty;
            ctlLabelHalfDayPerdiemRateHigh.Text = string.Empty;
            ctlLabelTotalHalfDayPerdiemAmountHigh.Text = string.Empty;
            ctlLabelPerdiemTotalAmountHigh.Text = string.Empty;

            //ctlLabelPerdiemGovernmentAmountHighName.Text = "0.00";
            //ctlLabelPerdiemGovernmentAmountName.Text = "0.00";
            ctlLabelPerdiemGovermentAmount.Text = string.Empty;
            ctlLabelAllowance.Text = string.Empty;
            ctlLabelPerdiemPrivateAmount.Text = string.Empty;
            ctlLabelPerdiemTaxAmount.Text = string.Empty;
            ctlPerdiemAllowanceLabel.Text = string.Empty;
            #endregion

            #region Clear PerdiemItem
            ClearPerdiemItem();
            #endregion
            #region Clear PerdiemDetail
            //#region dropdownlist currency
            //ctlDropDownListCurrency.DataSource = SsDbQueryProvider.DbCurrencyQuery.FindAll();
            //ctlDropDownListCurrency.DataTextField = "Symbol";
            //ctlDropDownListCurrency.DataValueField = "CurrencyID";
            //ctlDropDownListCurrency.DataBind();
            //#endregion
            //ctlPanelPerdiemDetail.Visible = false;
            ctlTextBoxPerdiemDetailDecription.Text = string.Empty;
            ctlTextBoxExchangeRate.Text = string.Empty;
            ctlTextBoxPerdiemDetailAmount.Text = string.Empty;
            #endregion
            #region BindData

            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
            DataRow[] perdiemObj = expDS.FnExpensePerdiem.Select("ExpensePerdiemID=" + this.ExpensePerdiemId.ToString());

            if (perdiemObj.Length > 0)
            {
                ExpenseDataSet.FnExpensePerdiemRow row = expDS.FnExpensePerdiem.FindByExpensePerdiemID(this.ExpensePerdiemId);
                ExpenseDataSet.FnExpenseDocumentRow exp = expDS.FnExpenseDocument.FindByExpenseID(row.ExpenseID);
                isRepOffice = exp.IsIsRepOfficeNull() ? false : exp.IsRepOffice;
                #region Perdiem Rate
                SuUser requester = SS.SU.Query.QueryProvider.SuUserQuery.FindByIdentity(this.RequesterId);
                IList<PerdiemRateValObj> prList;
                if (!isRepOffice)
                {
                    prList = ScgeAccountingQueryProvider.FnPerdiemRateQuery.GetPerdiemRateByRequesterID(Convert.ToInt64(requester.Userid));
                }
                else
                {
                    prList = ScgeAccountingQueryProvider.FnPerdiemRateQuery.GetPerdiemRateByRequesterIDForRepOffice(requester.Userid, requester.PersonalGroup);

                }
                foreach (PerdiemRateValObj obj in prList)
                {
                    if (!isRepOffice)
                    {
                        if (this.PerdiemType == ZoneType.Foreign)
                        {
                            if (obj.ZoneID.ToString() == "2")
                            {
                                if (obj.ExtraPerdiemRate > 0)
                                {
                                    ctlTextBoxPerdiemRateHighZoneUSD.ReadOnly = true;
                                    ctlTextBoxPerdiemRateHighZoneUSD.Style["background-color"] = "Silver";
                                    ctlTextBoxPerdiemRateHighZoneUSD.Style["vertical-align"] = "middle";
                                    ctlTextBoxPerdiemRateHighZoneUSD.BorderStyle = BorderStyle.Solid;
                                    ctlTextBoxPerdiemRateHighZoneUSD.BorderWidth = 1;
                                }
                                if (obj.OfficialPerdiemRate > 0)
                                {
                                    ctlTextBoxPerdiemRateHighZoneTHB.ReadOnly = true;
                                    ctlTextBoxPerdiemRateHighZoneTHB.Style["background-color"] = "Silver";
                                    ctlTextBoxPerdiemRateHighZoneTHB.Style["vertical-align"] = "middle";
                                    ctlTextBoxPerdiemRateHighZoneTHB.BorderStyle = BorderStyle.Solid;
                                    ctlTextBoxPerdiemRateHighZoneTHB.BorderWidth = 1;
                                }
                            }
                            else if (obj.ZoneID.ToString() == "1")
                            {
                                if (obj.ExtraPerdiemRate > 0)
                                {
                                    ctlTextBoxPerdiemRateNormalZoneUSD.ReadOnly = true;
                                    ctlTextBoxPerdiemRateNormalZoneUSD.Style["background-color"] = "Silver";
                                    ctlTextBoxPerdiemRateNormalZoneUSD.Style["vertical-align"] = "middle";
                                    ctlTextBoxPerdiemRateNormalZoneUSD.BorderStyle = BorderStyle.Solid;
                                    ctlTextBoxPerdiemRateNormalZoneUSD.BorderWidth = 1;
                                }
                                if (obj.OfficialPerdiemRate > 0)
                                {
                                    ctlTextBoxPerdiemRateNormalZoneTHB.ReadOnly = true;
                                    ctlTextBoxPerdiemRateNormalZoneTHB.Style["background-color"] = "Silver";
                                    ctlTextBoxPerdiemRateNormalZoneTHB.Style["vertical-align"] = "middle";
                                    ctlTextBoxPerdiemRateNormalZoneTHB.BorderStyle = BorderStyle.Solid;
                                    ctlTextBoxPerdiemRateNormalZoneTHB.BorderWidth = 1;
                                }
                            }
                        }
                        else
                        {
                            if (obj.ZoneID == (short)CountryZonePerdiem.DomesticZone && (obj.OfficialPerdiemRate + obj.ExtraPerdiemRate) > 0)
                            {
                                ctlTextboxPerdiemRate.ReadOnly = true;
                                ctlTextboxPerdiemRate.Style["background-color"] = "Silver";
                                ctlTextboxPerdiemRate.Style["vertical-align"] = "middle";
                                ctlTextboxPerdiemRate.BorderStyle = BorderStyle.Solid;
                                ctlTextboxPerdiemRate.BorderWidth = 1;
                            }
                        }
                    }
                    else
                    {
                        if (this.PerdiemType == ZoneType.Foreign)
                        {
                            if (obj.ZoneID.ToString() == "2")
                            {
                                if (obj.StuffPerdiemRate > 0)
                                {
                                    ctlTextBoxPerdiemRateHighZoneUSD.ReadOnly = true;
                                    ctlTextBoxPerdiemRateHighZoneUSD.Style["background-color"] = "Silver";
                                    ctlTextBoxPerdiemRateHighZoneUSD.Style["vertical-align"] = "middle";
                                    ctlTextBoxPerdiemRateHighZoneUSD.BorderStyle = BorderStyle.Solid;
                                    ctlTextBoxPerdiemRateHighZoneUSD.BorderWidth = 1;
                                }
                            }
                            else if (obj.ZoneID.ToString() == "7")
                            {
                                if (obj.StuffPerdiemRate > 0)
                                {
                                    ctlPerdiemRateThaiZoneTextBox.ReadOnly = true;
                                    ctlPerdiemRateThaiZoneTextBox.Style["background-color"] = "Silver";
                                    ctlPerdiemRateThaiZoneTextBox.Style["vertical-align"] = "middle";
                                    ctlPerdiemRateThaiZoneTextBox.BorderStyle = BorderStyle.Solid;
                                    ctlPerdiemRateThaiZoneTextBox.BorderWidth = 1;
                                }
                            }
                            else
                            {
                                if (obj.StuffPerdiemRate > 0)
                                {
                                    ctlTextBoxPerdiemRateNormalZoneUSD.ReadOnly = true;
                                    ctlTextBoxPerdiemRateNormalZoneUSD.Style["background-color"] = "Silver";
                                    ctlTextBoxPerdiemRateNormalZoneUSD.Style["vertical-align"] = "middle";
                                    ctlTextBoxPerdiemRateNormalZoneUSD.BorderStyle = BorderStyle.Solid;
                                    ctlTextBoxPerdiemRateNormalZoneUSD.BorderWidth = 1;
                                }
                            }
                        }
                        else
                        {
                            if (obj.ZoneID == (short)CountryZonePerdiem.DomesticZone)
                            {
                                //if ((obj.OfficialPerdiemRate + obj.ExtraPerdiemRate) > 0)
                                //{
                                    ctlTextboxPerdiemRate.ReadOnly = true;
                                    ctlTextboxPerdiemRate.Style["background-color"] = "Silver";
                                    ctlTextboxPerdiemRate.Style["vertical-align"] = "middle";
                                    ctlTextboxPerdiemRate.BorderStyle = BorderStyle.Solid;
                                    ctlTextboxPerdiemRate.BorderWidth = 1;
                                //}
                            }
                        }
                    }
                }

                #endregion

                if (isRepOffice)
                {
                    ctlPerdiemDetailZone.Visible = false;
                    ctlTextBoxPerdiemRateHighZoneTHB.Visible = false;
                    ctlHighTHBUnit.Visible = false;
                    ctlTextBoxPerdiemRateNormalZoneTHB.Visible = false;
                    ctlNormalTHBUnit.Visible = false;
                    LabelExtender8.Visible = false;
                    LabelExtender10.Visible = false;
                    if (requester.PersonalGroup.Substring(0, 1).Equals(PersonalGroupType.InternationalStaff))
                    {
                        ctlThaiZoneRowVisible.Visible = false;
                        ctlThaiZoneRow.Visible = false;
                    }
                    else
                    {
                        ctlThaiZoneRowVisible.Visible = true;
                        ctlThaiZoneRow.Visible = true;
                    }
                }
                else
                {
                    ctlThaiZoneRow.Visible = false;
                    ctlPerdiemDetailZone.Visible = true;
                    ctlTextBoxPerdiemRateHighZoneTHB.Visible = true;
                    ctlHighTHBUnit.Visible = true;
                    ctlTextBoxPerdiemRateNormalZoneTHB.Visible = true;
                    ctlNormalTHBUnit.Visible = true;
                    LabelExtender8.Visible = true;
                    LabelExtender10.Visible = true;
                    ctlThaiZoneRowVisible.Visible = false;
                }
                if (!exp.IsExchangeRateForUSDAdvanceNull())
                {
                    if (!isRepOffice)
                    {
                        ctlLabelExchangeRateForPerdiemCalculate.Text = UIHelper.BindExchangeRate(exp.ExchangeRateForUSDAdvance.ToString());
                    }
                    else
                    {
                        if (exp.MainCurrencyID == exp.LocalCurrencyID)
                        {
                            ctlLabelExchangeRateForPerdiemCalculate.Text = UIHelper.BindExchangeRate(exp.ExchangeRateForUSDAdvance.ToString());
                        }
                        else
                        {
                            ctlLabelExchangeRateForPerdiemCalculate.Text = string.Empty;
                        }
                    }
                }
                if (!row.IsCostCenterIDNull()) ctlCostCenterField.SetValue(row.CostCenterID);
                if (!row.IsAccountIDNull()) ctlExpenseField.BindAccountControl(row.AccountID);
                if (!row.IsIOIDNull()) ctlInternalLookup.BindIOControl(row.IOID);

                if (!row.IsSaleOrderNull()) ctlTextBoxSaleOrder.Text = row.SaleOrder;
                if (!row.IsSaleItemNull()) ctlTextBoxSaleItem.Text = row.SaleItem;
                if (!row.IsPerdiemRateNull() && row.PerdiemRate > 0)
                {
                    ctlTextboxPerdiemRate.Text = UIHelper.BindDecimal(row.PerdiemRate.ToString());
                }
                //else
                //{
                //    if (!isRepOffice)
                //    {
                //        ctlTextboxPerdiemRate.Text = UIHelper.BindDecimal(row.PerdiemRate.ToString());
                //        //ctlTextboxPerdiemRate.Text = UIHelper.BindDecimal(ParameterServices.PerdiemRate.ToString()); // Set default 300.00 
                //    }
                //}
                if (!row.IsReferenceNoNull()) ctlTextboxReferenceNo.Text = row.ReferenceNo.ToString();
                if (!row.IsTotalFullDayPerdiemNull()) TotalNetDayFooter = (double)row.TotalFullDayPerdiem;
                if (!row.IsPerdiemTotalAmountNull()) TotalAmountFooter = (double)row.PerdiemTotalAmount;
                if (!row.IsPerdiemTotalAmountLocalCurrencyNull()) TotalAmountLocalFooter = (double)row.PerdiemTotalAmountLocalCurrency;
                ctlTextboxDescription.Text = row.Description;

                if (this.PerdiemType == ZoneType.Foreign)
                {
                    double allowance = (double)0;

                    if (!row.IsTotalFullDayPerdiemNull()
                        && !row.IsTotalFullDayPerdiemHighNull()
                        && !row.IsTotalHalfDayPerdiemNull()
                        && !row.IsTotalHalfDayPerdiemHighNull())
                    {
                        TotalNetDayFooter = (double)(row.TotalFullDayPerdiem + row.TotalFullDayPerdiemHigh + row.TotalHalfDayPerdiem + row.TotalHalfDayPerdiemHigh);
                    }

                    if (!row.IsTotalFullDayPerdiemNull()
                         && !row.IsTotalFullDayPerdiemHighNull())
                    {
                        TotalFullDayFooter = (double)(row.TotalFullDayPerdiem + row.TotalFullDayPerdiemHigh);
                    }

                    if (!row.IsTotalHalfDayPerdiemNull()
                         && !row.IsTotalHalfDayPerdiemHighNull())
                    {
                        TotalHalfDayFooter = (double)(row.TotalHalfDayPerdiem + row.TotalHalfDayPerdiemHigh);
                    }

                    if (!row.IsPerdiemPrivateAmountNull())
                    {
                        TotalAmountTHBFooter = (double)row.PerdiemPrivateAmount;
                    }

                    if (!row.IsPerdiemTotalAmountNull()
                        && !row.IsPerdiemTotalAmountHighNull()
                        && !row.IsPerdiemGovernmentAmountNull())
                    {
                        allowance = (double)(row.PerdiemTotalAmount + row.PerdiemTotalAmountHigh - row.PerdiemGovernmentAmount);
                    }

                    if (!row.IsPerdiemRateUSDNull()) ctlTextBoxPerdiemRateNormalZoneUSD.Text = UIHelper.BindDecimal(row.PerdiemRateUSD.ToString());
                    if (!row.IsPerdiemRateUSDHighNull()) ctlTextBoxPerdiemRateHighZoneUSD.Text = UIHelper.BindDecimal(row.PerdiemRateUSDHigh.ToString());
                    if (!row.IsPerdiemRateGovNull()) ctlTextBoxPerdiemRateNormalZoneTHB.Text = UIHelper.BindDecimal(row.PerdiemRateGov.ToString());
                    if (!row.IsPerdiemRateGovHighNull()) ctlTextBoxPerdiemRateHighZoneTHB.Text = UIHelper.BindDecimal(row.PerdiemRateGovHigh.ToString());
                    if (!row.IsPerdiemRateUSDThaiZoneNull()) ctlPerdiemRateThaiZoneTextBox.Text = UIHelper.BindDecimal(row.PerdiemRateUSDThaiZone.ToString());

                    if (!row.IsTotalFullDayPerdiemNull()) ctlLabelTotalFullDayPerdiem.Text = UIHelper.ParseLong(row.TotalFullDayPerdiem.ToString()).ToString();
                    if (!row.IsFullDayPerdiemRateNull()) ctlLabelFullDayPerdiemRate.Text = UIHelper.BindDecimal(row.FullDayPerdiemRate.ToString());
                    if (!row.IsTotalFullDayPerdiemAmountNull()) ctlLabelTotalFullDayPerdiemAmount.Text = UIHelper.BindDecimal(row.TotalFullDayPerdiemAmount.ToString());
                    if (!row.IsTotalHalfDayPerdiemNull()) ctlLabelTotalHalfDayPerdiem.Text = UIHelper.ParseLong(row.TotalHalfDayPerdiem.ToString()).ToString();
                    if (!row.IsHalfDayPerdiemRateNull()) ctlLabelHalfDayPerdiemRate.Text = UIHelper.BindDecimal(row.HalfDayPerdiemRate.ToString());
                    if (!row.IsTotalHalfDayPerdiemAmountNull()) ctlLabelTotalHalfDayPerdiemAmount.Text = UIHelper.BindDecimal(row.TotalHalfDayPerdiemAmount.ToString());
                    if (!row.IsPerdiemTotalAmountNull()) ctlLabelPerdiemTotalAmount.Text = UIHelper.BindDecimal(row.PerdiemTotalAmount.ToString());
                    if (!row.IsTotalFullDayPerdiemHighNull()) ctlLabelTotalFullDayPerdiemHigh.Text = UIHelper.ParseLong(row.TotalFullDayPerdiemHigh.ToString()).ToString();
                    if (!row.IsFullDayPerdiemRateHighNull()) ctlLabelFullDayPerdiemRateHigh.Text = UIHelper.BindDecimal(row.FullDayPerdiemRateHigh.ToString());
                    if (!row.IsTotalFullDayPerdiemAmountHighNull()) ctlLabelTotalFullDayPerdiemAmountHigh.Text = UIHelper.BindDecimal(row.TotalFullDayPerdiemAmountHigh.ToString());
                    if (!row.IsTotalHalfDayPerdiemHighNull()) ctlLabelTotalHalfDayPerdiemHigh.Text = UIHelper.ParseLong(row.TotalHalfDayPerdiemHigh.ToString()).ToString();
                    if (!row.IsHalfDayPerdiemRateHighNull()) ctlLabelHalfDayPerdiemRateHigh.Text = UIHelper.BindDecimal(row.HalfDayPerdiemRateHigh.ToString());

                    if (!row.IsTotalFullDayPerdiemAmountThaiZoneNull()) ctlLabelTotalFullDayPerdiemAmountThai.Text = UIHelper.BindDecimal(row.TotalFullDayPerdiemAmountThaiZone.ToString());
                    if (!row.IsTotalFullDayPerdiemThaiZoneNull()) ctlLabelTotalFullDayPerdiemThai.Text = row.TotalFullDayPerdiemThaiZone.ToString();
                    if (!row.IsTotalHalfDayPerdiemAmountThaiZoneNull()) ctlLabelTotalHalfDayPerdiemAmountThai.Text = UIHelper.BindDecimal(row.TotalHalfDayPerdiemAmountThaiZone.ToString());
                    if (!row.IsTotalHalfDayPerdiemThaiZoneNull()) ctlLabelTotalHalfDayPerdiemThai.Text = row.TotalHalfDayPerdiemThaiZone.ToString();
                    if (!row.IsFullDayPerdiemRateThaiZoneNull()) ctlLabelFullDayPerdiemRateThai.Text = UIHelper.BindDecimal(row.FullDayPerdiemRateThaiZone.ToString());
                    if (!row.IsHalfDayPerdiemRateThaiZoneNull()) ctlLabelHalfDayPerdiemRateThai.Text = UIHelper.BindDecimal(row.HalfDayPerdiemRateThaiZone.ToString());
                    if (!row.IsPerdiemTotalAmountThaiZoneNull()) ctlLabelPerdiemTotalAmountThai.Text = UIHelper.BindDecimal(row.PerdiemTotalAmountThaiZone.ToString());

                    if (!row.IsTotalHalfDayPerdiemAmountHighNull()) ctlLabelTotalHalfDayPerdiemAmountHigh.Text = UIHelper.BindDecimal(row.TotalHalfDayPerdiemAmountHigh.ToString());
                    if (!row.IsPerdiemTotalAmountHighNull()) ctlLabelPerdiemTotalAmountHigh.Text = UIHelper.BindDecimal(row.PerdiemTotalAmountHigh.ToString());
                    //if (!row.IsPerdiemRateGovHighNull()) ctlLabelPerdiemGovernmentAmountHighName.Text = row.PerdiemRateGovHigh.ToString("#,##0.00");
                    //if (!row.IsPerdiemRateGovNull()) ctlLabelPerdiemGovernmentAmountName.Text = row.PerdiemRateGov.ToString("#,##0.00");
                    if (!row.IsPerdiemGovernmentAmountNull()) ctlLabelPerdiemGovermentAmount.Text = UIHelper.BindDecimal(row.PerdiemGovernmentAmount.ToString());
                    if (!row.IsPerdiemPrivateAmountNull()) ctlLabelPerdiemPrivateAmount.Text = UIHelper.BindDecimal(row.PerdiemPrivateAmount.ToString());
                    if (!row.IsPerdiemTaxAmountNull()) ctlLabelPerdiemTaxAmount.Text = UIHelper.BindDecimal(row.PerdiemTaxAmount.ToString());
                    if (!row.IsExchangeRateNull() && row.ExchangeRate > 0)
                    {
                        ctlExchangeRate.Text = UIHelper.BindExchangeRate(row.ExchangeRate.ToString());
                        if (isRepOffice)
                        {
                            ctlLabelExchangeRateForPerdiemCalculate.Text = UIHelper.BindExchangeRate(row.ExchangeRate.ToString());
                        }
                    }
                    else
                    {
                        if (isRepOffice)
                        {
                            if (!exp.IsMainCurrencyIDNull() && exp.MainCurrencyID == ParameterServices.USDCurrencyID)
                            {
                                ctlExchangeRate.Text = UIHelper.BindExchangeRate("1");
                                ctlExchangeRate.ReadOnly = true;
                                ctlExchangeRate.Style["background-color"] = "Silver";
                                ctlExchangeRate.Style["vertical-align"] = "middle";
                                ctlExchangeRate.BorderStyle = BorderStyle.Solid;
                                ctlExchangeRate.BorderWidth = 1;

                                ctlLabelExchangeRateForPerdiemCalculate.Text = string.Empty;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(ctlLabelExchangeRateForPerdiemCalculate.Text) && !UIHelper.ParseDouble(ctlLabelExchangeRateForPerdiemCalculate.Text).Equals(0))
                    {
                        ctlExchangeRate.Enabled = false;
                    }
                    else
                    {
                        ctlExchangeRate.Enabled = true;
                    }
                    ctlLabelAllowance.Text = UIHelper.BindDecimal(allowance.ToString());
                    double totalAmountNormalHigh = UIHelper.ParseDouble(row["PerdiemTotalAmount"].ToString()) + UIHelper.ParseDouble(row["PerdiemTotalAmountHigh"].ToString()) + UIHelper.ParseDouble(row["PerdiemTotalAmountThaiZone"].ToString());
                    ctlPerdiemAllowanceLabel.Text = UIHelper.BindDecimal(totalAmountNormalHigh.ToString());

                }
                else if (this.PerdiemType == ZoneType.Domestic)
                {
                    double allowance = (double)0;

                    if (!row.IsTotalFullDayPerdiemNull() && !row.IsTotalHalfDayPerdiemNull())
                    {
                        TotalNetDayFooter = (double)(row.TotalFullDayPerdiem + row.TotalHalfDayPerdiem);
                    }

                    if (!row.IsTotalFullDayPerdiemNull())
                    {
                        TotalFullDayFooter = (double)(row.TotalFullDayPerdiem);
                    }

                    if (!row.IsTotalHalfDayPerdiemNull())
                    {
                        TotalHalfDayFooter = (double)(row.TotalHalfDayPerdiem);
                    }

                    if (!row.IsPerdiemPrivateAmountNull())
                    {
                        TotalAmountTHBFooter = (double)row.PerdiemPrivateAmount;
                    }

                    if (!row.IsTotalFullDayPerdiemNull()) ctlNetDayDM.Text = UIHelper.ParseDecimal(row.TotalFullDayPerdiem.ToString()).ToString();
                    if (!row.IsFullDayPerdiemRateNull()) ctlRatePerDayDM.Text = UIHelper.BindDecimal(row.FullDayPerdiemRate.ToString());
                    if (!row.IsPerdiemTotalAmountNull())
                    {
                        if (!isRepOffice)
                        {
                            ctlTotalPerdiemAmountDM.Text = UIHelper.BindDecimal(row.PerdiemTotalAmount.ToString());
                        }
                        else
                        {
                            ctlTotalPerdiemAmountDM.Text = UIHelper.BindDecimal(row.PerdiemTotalAmountLocalCurrency.ToString());
                        }
                    }

                    if (!isRepOffice)
                    {

                        if (!row.IsPerdiemTotalAmountNull()
                            && !row.IsPerdiemGovernmentAmountNull())
                        {
                            allowance = (double)(row.PerdiemTotalAmount - row.PerdiemGovernmentAmount);
                        }

                        //Perdiem (1)
                        double totalAmount = UIHelper.ParseDouble(row["PerdiemTotalAmount"].ToString());
                        ctlPerdiemAllowanceDM.Text = UIHelper.BindDecimal(totalAmount.ToString());

                        //Perdiem Goverment (2)
                        if (!row.IsPerdiemGovernmentAmountNull())
                            ctlGovermentAmountDM.Text = UIHelper.BindDecimal(row.PerdiemGovernmentAmount.ToString());

                        //Perdiem Exceeding (1)-(2)
                        ctlExceedAllowanceDM.Text = UIHelper.BindDecimal(allowance.ToString());
                    }
                    else
                    {
                        //Perdiem (1)
                        double totalAmount = UIHelper.ParseDouble(row["PerdiemTotalAmountLocalCurrency"].ToString());
                        ctlPerdiemAllowanceDM.Text = UIHelper.BindDecimal(totalAmount.ToString());

                        //Perdiem Goverment (2)
                        if (!row.IsPerdiemGovernmentAmountNull())
                            ctlGovermentAmountDM.Text = UIHelper.BindDecimal(row.PerdiemGovernmentAmount.ToString());
                    }

                    if (row.PerdiemRate == row.PerdiemRateGov)
                    {
                        ctlGovermentDMRow.Visible = false;
                        ctlExeedAllowanceRow.Visible = false;
                    }
                }
            }

            BindGridView();
            BindPerdiemDetailGridView();
            #endregion
        }
        #endregion
    }
}
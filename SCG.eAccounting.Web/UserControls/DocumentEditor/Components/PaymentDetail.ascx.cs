using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.DTO;
using SCG.DB.DTO;
using SCG.eAccounting.BLL;
using SCG.DB.Query;
using SCG.eAccounting.DTO.DataSet;
using SCG.eAccounting.Query;
using System.Text;
using SS.DB.Query;
using SS.DB.DTO;
using SCG.DB.DTO.ValueObject;
using System.Data;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor.Components
{
    public partial class PaymentDetail : BaseUserControl, IEditorComponent
    {
        public ITransactionService TransactionService { get; set; }
        public IFnExpenseDocumentService FnExpenseDocumentService { get; set; }

        #region Property
        public bool ShowAdvance { get; set; }
        public string GroupType { get; set; }
        private string width = "width:75%";
        public string Width
        {
            set { this.width = value; }
        }
        public string Mode
        {
            get { return ctlMode.Text; }
            set { ctlMode.Text = value; }
        }
        public string DocumentType
        {
            get { return ctlType.Value; }
            set { ctlType.Value = value; }
        }
        public string ServiceTeamID
        {
            get { return ctlDdlServiceTeam.SelectedValue; }
            set { this.ctlDdlServiceTeam.SelectedValue = value; }
        }
        public string PaymentTypeID
        {
            get { return ctlDdlPaymentType.SelectedValue; }
            set { this.ctlDdlPaymentType.SelectedValue = value; }
        }
        public long? CounterCashierID
        {
            get
            {
                if (UIHelper.ParseLong(ctlDdlCounterCashier.SelectedValue) > 0)
                    return UIHelper.ParseLong(ctlDdlCounterCashier.SelectedValue);
                return null;
            }
            set
            {
                if (value.HasValue && value.Value > 0)
                    this.ctlDdlCounterCashier.SelectedValue = value.ToString();
            }
        }

        public long? TempCounterCashierID
        {
            get { return ViewState["TempCounterCashierID"] == null ? 0 : (long)ViewState["TempCounterCashierID"]; }
            set { ViewState["TempCounterCashierID"] = value; }
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
        public string InitialFlag
        {
            get { return ViewState[ViewStateName.InitialFlag].ToString(); }
            set { ViewState[ViewStateName.InitialFlag] = value; }
        }
        public string TotalExpense
        {
            get { return ctlTotalExpense.Text; }
            set { ctlTotalExpense.Text = value; }
        }
        public string TotalAdvance
        {
            get { return ctlTotalAdvance.Text; }
            set { ctlTotalAdvance.Text = value; }
        }
        public string TotalRemitted
        {
            get { return ctlTotalRemitted.Text; }
            set { ctlTotalRemitted.Text = value; }
        }
        public string DifferenceAmount
        {
            get { return ctlDifferenceAmount.Text; }
            set { ctlDifferenceAmount.Text = value; }
        }

        public string DifferenceAmountTHBForRepOffice
        {
            get { return ctlDifferenceAmtTHB.Text; }
            set { ctlDifferenceAmtTHB.Text = value; }
        }

        public long? CompanyID
        {
            get
            {
                if (ViewState["CompanyId"] != null)
                    return UIHelper.ParseLong(ViewState["CompanyId"].ToString());
                return null;
            }
            set { ViewState["CompanyId"] = value; }
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

        public short? LocalCurrencyID
        {
            get
            {
                if (ViewState["LocalCurrencyID"] != null)
                    return (short)ViewState["LocalCurrencyID"];
                return 0;
            }
            set { ViewState["LocalCurrencyID"] = value; }
        }

        public short? MainCurrencyID
        {
            get
            {
                if (ViewState["MainCurrencyID"] != null)
                    return (short)ViewState["MainCurrencyID"];
                return 0;
            }
            set { ViewState["MainCurrencyID"] = value; }
        }

        #endregion Property

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DivTotalSummaryForThailand.DataBind();
                DivTotalSummaryForRepOffice.DataBind();
            }
        }

        #region Public Method
        public void BindCounterCashier(long companyID)
        {
            CompanyID = companyID;
            if (!ctlDdlPaymentType.SelectedValue.Equals(PaymentType.TR))
            {
                IList<PaymentTypeListItem> list = ScgDbQueryProvider.DbPBQuery.GetPbListItem(companyID, UserAccount.UserLanguageID);

                ctlDdlCounterCashier.DataSource = list;
                ctlDdlCounterCashier.DataTextField = "Text";
                ctlDdlCounterCashier.DataValueField = "ID";
                ctlDdlCounterCashier.DataBind();

                if (list.Count > 1)
                {
                    ctlDdlCounterCashier.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), "-1"));
                }
                else
                {
                    TempCounterCashierID = CounterCashierID;
                }
            }
        }
        public void BindServiceTeam(long companyId, string companyCode, long userId, long locationID)
        {
            ctlDdlServiceTeam.DataSource = ScgDbQueryProvider.DbLocationQuery.FindByLocationCompany(companyId, companyCode, userId, locationID);
            ctlDdlServiceTeam.DataTextField = "Text";
            ctlDdlServiceTeam.DataValueField = "ID";
            ctlDdlServiceTeam.DataBind();
            //ถ้ามีค่ามากกว่า 1 rows ให้ขึ้นคำว่า please select แต่มี 1 rows ให้ default เลย
            if (ctlDdlServiceTeam.Items.Count > 1 && !ctlDdlServiceTeam.Items[0].Value.Equals(string.Empty))
            {
                ctlDdlServiceTeam.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
                ctlDdlServiceTeam.SelectedIndex = 0;

                IList<DbServiceTeamLocation> listLocation = ScgDbQueryProvider.DbServiceTeamLocationQuery.FindServiceTeamByLocationID(locationID);
                if (listLocation != null && listLocation.Count > 0)
                    if (ctlDdlServiceTeam.Items.FindByValue(listLocation.First<DbServiceTeamLocation>().ServiceTeamID.ServiceTeamID.ToString()) != null)
                        ctlDdlServiceTeam.SelectedValue = listLocation.First<DbServiceTeamLocation>().ServiceTeamID.ServiceTeamID.ToString();
            }
        }
        public void BindPaymentType()
        {
            GroupType = GroupStatus.PaymentTypeDMT;
            ctlDdlPaymentType.DataSource = ScgDbQueryProvider.SCGDbStatusLangQuery.FindStatusLangCriteria(GroupType, UserAccount.CurrentLanguageID);
            ctlDdlPaymentType.DataTextField = "strSymbol";
            ctlDdlPaymentType.DataValueField = "strID";
            ctlDdlPaymentType.DataBind();

            ctlDdlPaymentType.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
        }
        public void Initialize(Guid txID, long documentID, string initFlag)
        {
            this.TransactionId = txID;
            this.ExpDocumentID = documentID;
            this.InitialFlag = initFlag;
            ctlPaymentDetailTable.Width = this.width;
            ctlMsgSummary.Text = GetProgramMessage("$DiffentialAmount$");
            ctlLblDifferenceAmtLocal.Text = GetProgramMessage("$DiffentialAmount$");
            ctlLblDifferenceAmtMainCurrency.Text = GetProgramMessage("$DiffentialAmount$");
            ctlLblDifferenceAmtTHB.Text = GetProgramMessage("$DiffentialAmount$");

            if (InitialFlag.Equals(FlagEnum.NewFlag))
            {
                ctlTotalAdvance.Visible = false;
                ctlLblTotalAdvance.Visible = false;
                ctlTotalRemitted.Visible = false;
                ctlRemitted.Visible = false;
                ctlMsgSummary.Text = string.Empty;
                ctlDifferenceAmount.Visible = false;

                ctlTotalAdvanceLocal.Visible = false;
                ctlLblTotalAdvanceLocal.Visible = false;
                ctlTotalRemittanceLocal.Visible = false;
                ctlLblTotalRemittanceLocal.Visible = false;
                ctlLblDifferenceAmtLocal.Text = string.Empty;
                ctlDifferenceAmtLocal.Visible = false;

                ctlTotalAdvanceMainCurrency.Visible = false;
                ctlLblTotalAdvanceMainCurrency.Visible = false;
                ctlTotalRemittanceMainCurrency.Visible = false;
                ctlLblTotalRemittanceMainCurrency.Visible = false;
                ctlLblDifferenceAmtMainCurrency.Text = string.Empty;
                ctlDifferenceAmtMainCurrency.Visible = false;

                ctlTotalAdvanceTHB.Visible = false;
                ctlLblTotalAdvanceTHB.Visible = false;
                ctlTotalRemittanceTHB.Visible = false;
                ctlLblTotalRemittanceTHB.Visible = false;
                ctlLblDifferenceAmtTHB.Text = string.Empty;
                ctlDifferenceAmtTHB.Visible = false;

            }
            else
            {
                this.UpdateMessageSummary();
            }
        }
        public void Save(FnExpenseDocument exp)
        {
            exp.PaymentType = ctlDdlPaymentType.SelectedValue;
            //exp.ServiceTeam = new DbServiceTeam();
        }
        public void BindControl()
        {
            if (string.IsNullOrEmpty(DocumentType) || DocumentType.Equals(ZoneType.Domestic))
            {
                ctlRemitted.Style["display"] = "none";
                ctlTotalRemitted.Style["display"] = "none";
            }
            else if (DocumentType.Equals(ZoneType.Foreign))
            {
                ctlRemitted.Style["display"] = "block";
                ctlTotalRemitted.Style["display"] = "block";
            }
            ShowPaymentInfo();
            ShowHideCounterCasheir();
            BindDifferenceAmountSummary();
        }
        public void BindDifferenceAmountSummary()
        {
            FnExpenseDocumentService.CalculateTotalExpense(this.TransactionId, this.ExpDocumentID, IsRepOffice);
            FnExpenseDocumentService.CalculateDifferenceAmount(this.TransactionId, this.ExpDocumentID, IsRepOffice);
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TransactionId);
            if (expDS != null)
            {
                ExpenseDataSet.FnExpenseDocumentRow expRow = expDS.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);

                if (expRow != null)
                {
                    if (!IsRepOffice)
                    {
                        DivTotalSummaryForThailand.Style["display"] = "block";
                        DivTotalSummaryForRepOffice.Style["display"] = "none";

                        ctlTotalExpense.Text = UIHelper.BindDecimalNumberAccountFormat(expRow.TotalExpense.ToString());
                        ctlTotalExpense.ForeColor = expRow.TotalExpense < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;

                        ctlTotalAdvance.Text = UIHelper.BindDecimalNumberAccountFormat((-1 * expRow.TotalAdvance).ToString());
                        ctlTotalAdvance.ForeColor = (-1 * expRow.TotalAdvance) < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;

                        ctlTotalRemitted.Text = UIHelper.BindDecimalNumberAccountFormat(expRow.TotalRemittance.ToString());
                        ctlTotalRemitted.ForeColor = expRow.TotalRemittance < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;

                        ctlDifferenceAmount.Text = UIHelper.BindDecimalNumberAccountFormat(expRow.DifferenceAmount.ToString());
                        ctlDifferenceAmount.ForeColor = expRow.DifferenceAmount < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;
                    }
                    else
                    {
                        DivTotalSummaryForThailand.Style["display"] = "none";
                        DivTotalSummaryForRepOffice.Style["display"] = "block";
                        ctlTHBCurrencyTab.Visible = false;
                        ctlMainCurrencyTab.Visible = false;

                        if (CounterCashierID.HasValue)
                        {
                            Dbpb pb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(CounterCashierID.Value);
                            DbCurrency localCurrency = SsDbQueryProvider.DbCurrencyQuery.FindByIdentity(LocalCurrencyID.HasValue ? LocalCurrencyID.Value : (short)0);
                            DbCurrency mainCurrency = SsDbQueryProvider.DbCurrencyQuery.FindByIdentity(pb.MainCurrencyID.HasValue ? pb.MainCurrencyID.Value : (short)0);

                            if (localCurrency != null)
                            {
                                ctlLocalCurrencyTab.HeaderText = localCurrency.Symbol;
                            }

                            if (mainCurrency != null)
                            {
                                ctlMainCurrencyTab.HeaderText = mainCurrency.Symbol;
                            }
                        }
                        #region Amount LocalCurrency Tab
                        ctlLocalCurrencyTab.Visible = true;

                        ctlTotalExpenseLocal.Text = UIHelper.BindDecimalNumberAccountFormat(expRow.TotalExpenseLocalCurrency.ToString());
                        ctlTotalExpenseLocal.ForeColor = expRow.TotalExpenseLocalCurrency < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;

                        if (!expRow.IsTotalAdvanceLocalCurrencyNull())
                        {
                            ctlTotalAdvanceLocal.Text = UIHelper.BindDecimalNumberAccountFormat((-1 * expRow.TotalAdvanceLocalCurrency).ToString());
                            ctlTotalAdvanceLocal.ForeColor = (-1 * expRow.TotalAdvanceLocalCurrency) < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;
                        }

                        if (!expRow.IsTotalRemittanceLocalCurrencyNull())
                        {
                            ctlTotalRemittanceLocal.Text = UIHelper.BindDecimalNumberAccountFormat(expRow.TotalRemittanceLocalCurrency.ToString());
                            ctlTotalRemittanceLocal.ForeColor = expRow.TotalRemittanceLocalCurrency < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;
                        }

                        if (!expRow.IsDifferenceAmountLocalCurrencyNull())
                        {
                            ctlDifferenceAmtLocal.Text = UIHelper.BindDecimalNumberAccountFormat(expRow.DifferenceAmountLocalCurrency.ToString());
                            ctlDifferenceAmtLocal.ForeColor = expRow.DifferenceAmountLocalCurrency < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;
                        }
                        #endregion

                        SS.Standard.WorkFlow.DTO.WorkFlow workflow = SS.Standard.WorkFlow.Query.WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(expRow.DocumentID);
                        if (workflow != null && workflow.CurrentState.Ordinal >= 5)
                        {
                            if (UserAccount.IsAccountant || UserAccount.IsPayment)
                            {
                                #region Amount MainCurrency Tab
                                if (expRow.LocalCurrencyID != expRow.MainCurrencyID)
                                {
                                    ctlMainCurrencyTab.Visible = true;
                                }
                                ctlTotalExpenseMainCurrency.Text = UIHelper.BindDecimalNumberAccountFormat(expRow.TotalExpenseMainCurrency.ToString());
                                ctlTotalExpenseMainCurrency.ForeColor = expRow.TotalExpenseMainCurrency < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;

                                ctlTotalAdvanceMainCurrency.Text = UIHelper.BindDecimalNumberAccountFormat((-1 * expRow.TotalAdvanceMainCurrency).ToString());
                                ctlTotalAdvanceMainCurrency.ForeColor = (-1 * expRow.TotalAdvanceMainCurrency) < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;

                                ctlTotalRemittanceMainCurrency.Text = UIHelper.BindDecimalNumberAccountFormat(expRow.TotalRemittanceMainCurrency.ToString());
                                ctlTotalRemittanceMainCurrency.ForeColor = expRow.TotalRemittanceMainCurrency < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;

                                ctlDifferenceAmtMainCurrency.Text = UIHelper.BindDecimalNumberAccountFormat(expRow.DifferenceAmountMainCurrency.ToString());
                                ctlDifferenceAmtMainCurrency.ForeColor = expRow.DifferenceAmountMainCurrency < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;
                                #endregion

                                #region Amount THB Currency Tab
                                ctlTHBCurrencyTab.Visible = true;
                                ctlTotalExpenseTHB.Text = UIHelper.BindDecimalNumberAccountFormat(expRow.TotalExpense.ToString());
                                ctlTotalExpenseTHB.ForeColor = expRow.TotalExpense < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;

                                ctlTotalAdvanceTHB.Text = UIHelper.BindDecimalNumberAccountFormat((-1 * expRow.TotalAdvance).ToString());
                                ctlTotalAdvanceTHB.ForeColor = (-1 * expRow.TotalAdvance) < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;

                                ctlTotalRemittanceTHB.Text = UIHelper.BindDecimalNumberAccountFormat(expRow.TotalRemittance.ToString());
                                ctlTotalRemittanceTHB.ForeColor = expRow.TotalRemittance < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;

                                ctlDifferenceAmtTHB.Text = UIHelper.BindDecimalNumberAccountFormat(expRow.DifferenceAmount.ToString());
                                ctlDifferenceAmtTHB.ForeColor = expRow.DifferenceAmount < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;
                                #endregion
                            }
                        }
                    }
                }
            }
        }

        public void ShowHideCounterCasheir()
        {
            if (ctlDdlPaymentType.SelectedItem != null)
            {
                if (ctlDdlPaymentType.SelectedValue == PaymentType.TR)
                {
                    ctlPanelCounterCashier.Style.Add("display", "none");
                    ctlPanelDdlCounterCashier.Style.Add("display", "none");
                }
                else
                {
                    ctlPanelCounterCashier.Style.Add("display", "block");
                    ctlPanelDdlCounterCashier.Style.Add("display", "block");
                }
            }
        }
        #endregion

        public ExpenseDocumentEditor ExpenseDocumentEditor { get; set; }

        protected void ctlLblPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CompanyID.HasValue && CompanyID.Value > 0)
                BindCounterCashier(CompanyID.Value);

            if (ExpenseDocumentEditor != null)
            {
                ExpenseDocumentEditor.NotifyCheckRepOffice();
                ExpenseDocumentEditor.SelectedPaymentMethod();
                ExpenseDocumentEditor.DefaultSupplementary();
            }

            ShowHideCounterCasheir();
        }

        public void UpdateMessageSummary()
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TransactionId);
            if (expDS != null)
            {
                ExpenseDataSet.FnExpenseDocumentRow expRow = expDS.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);

                if (!IsRepOffice)
                {
                    if (expRow.DifferenceAmount > (decimal)0)
                    {
                        ctlMsgSummary.Text = GetProgramMessage("$AdditionalPayFromCompany$");
                        ctlDifferenceAmount.Visible = true;
                    }
                    else if (expRow.DifferenceAmount < (decimal)0)
                    {
                        ctlMsgSummary.Text = GetProgramMessage("$PayBackToCompany$");
                        ctlDifferenceAmount.Visible = true;
                    }
                    else if (expRow.TotalAdvance > 0 && expRow.DifferenceAmount == (decimal)0)
                    {
                        ctlMsgSummary.Text = GetProgramMessage("$CompleteClearing$");
                        ctlDifferenceAmount.Visible = false;
                    }

                    if (expRow.TotalAdvance == (decimal)0)
                    {
                        ctlTotalAdvance.Visible = false;
                        ctlLblTotalAdvance.Visible = false;
                    }
                    else
                    {
                        ctlTotalAdvance.Visible = true;
                        ctlLblTotalAdvance.Visible = true;
                    }
                    if (expRow.TotalRemittance == (decimal)0)
                    {
                        ctlTotalRemitted.Visible = false;
                        ctlRemitted.Visible = false;
                    }
                    else
                    {
                        ctlTotalRemitted.Visible = true;
                        ctlRemitted.Visible = true;
                    }
                }
                else
                {
                    #region Local Currency Summary
                    if (expRow.DifferenceAmountLocalCurrency > (decimal)0)
                    {
                        ctlLblDifferenceAmtLocal.Text = GetProgramMessage("$AdditionalPayFromCompany$");
                        ctlDifferenceAmtLocal.Visible = true;
                    }
                    else if (expRow.DifferenceAmountLocalCurrency < (decimal)0)
                    {
                        ctlLblDifferenceAmtLocal.Text = GetProgramMessage("$PayBackToCompany$");
                        ctlDifferenceAmtLocal.Visible = true;
                    }
                    else if (expRow.TotalAdvanceLocalCurrency > 0 && expRow.DifferenceAmountLocalCurrency == (decimal)0)
                    {
                        ctlLblDifferenceAmtLocal.Text = GetProgramMessage("$CompleteClearing$");
                        ctlDifferenceAmtLocal.Visible = false;
                    }

                    if (expRow.TotalAdvanceLocalCurrency == (decimal)0)
                    {
                        ctlTotalAdvanceLocal.Visible = false;
                        ctlLblTotalAdvanceLocal.Visible = false;
                    }
                    else
                    {
                        ctlTotalAdvanceLocal.Visible = true;
                        ctlLblTotalAdvanceLocal.Visible = true;
                    }
                    if (expRow.TotalRemittanceLocalCurrency == (decimal)0)
                    {
                        ctlTotalRemittanceLocal.Visible = false;
                        ctlLblTotalRemittanceLocal.Visible = false;
                    }
                    else
                    {
                        ctlTotalRemittanceLocal.Visible = true;
                        ctlLblTotalRemittanceLocal.Visible = true;
                    }
                    #endregion

                    #region Main Currency Summary
                    if (expRow.DifferenceAmountMainCurrency > (decimal)0)
                    {
                        ctlLblDifferenceAmtMainCurrency.Text = GetProgramMessage("$AdditionalPayFromCompany$");
                        ctlDifferenceAmtMainCurrency.Visible = true;
                    }
                    else if (expRow.DifferenceAmountMainCurrency < (decimal)0)
                    {
                        ctlLblDifferenceAmtMainCurrency.Text = GetProgramMessage("$PayBackToCompany$");
                        ctlDifferenceAmtMainCurrency.Visible = true;
                    }
                    else if (expRow.TotalAdvanceMainCurrency > 0 && expRow.DifferenceAmountMainCurrency == (decimal)0)
                    {
                        ctlLblDifferenceAmtMainCurrency.Text = GetProgramMessage("$CompleteClearing$");
                        ctlDifferenceAmtMainCurrency.Visible = false;
                    }

                    if (expRow.TotalAdvanceMainCurrency == (decimal)0)
                    {
                        ctlTotalAdvanceMainCurrency.Visible = false;
                        ctlLblTotalAdvanceMainCurrency.Visible = false;
                    }
                    else
                    {
                        ctlTotalAdvanceMainCurrency.Visible = true;
                        ctlLblTotalAdvanceMainCurrency.Visible = true;
                    }
                    if (expRow.TotalRemittanceMainCurrency == (decimal)0)
                    {
                        ctlTotalRemittanceMainCurrency.Visible = false;
                        ctlLblTotalRemittanceMainCurrency.Visible = false;
                    }
                    else
                    {
                        ctlTotalRemittanceMainCurrency.Visible = true;
                        ctlLblTotalRemittanceMainCurrency.Visible = true;
                    }
                    #endregion

                    #region THB Currency Summary
                    if (expRow.DifferenceAmount > (decimal)0)
                    {
                        ctlLblDifferenceAmtTHB.Text = GetProgramMessage("$AdditionalPayFromCompany$");
                        ctlDifferenceAmtTHB.Visible = true;
                    }
                    else if (expRow.DifferenceAmount < (decimal)0)
                    {
                        ctlLblDifferenceAmtTHB.Text = GetProgramMessage("$PayBackToCompany$");
                        ctlDifferenceAmtTHB.Visible = true;
                    }
                    else if (expRow.TotalAdvance > 0 && expRow.DifferenceAmount == (decimal)0)
                    {
                        ctlLblDifferenceAmtTHB.Text = GetProgramMessage("$CompleteClearing$");
                        ctlDifferenceAmtTHB.Visible = false;
                    }

                    if (expRow.TotalAdvance == (decimal)0)
                    {
                        ctlTotalAdvanceTHB.Visible = false;
                        ctlLblTotalAdvanceTHB.Visible = false;
                    }
                    else
                    {
                        ctlTotalAdvanceTHB.Visible = true;
                        ctlLblTotalAdvanceTHB.Visible = true;
                    }
                    if (expRow.TotalRemittance == (decimal)0)
                    {
                        ctlTotalRemittanceTHB.Visible = false;
                        ctlLblTotalRemittanceTHB.Visible = false;
                    }
                    else
                    {
                        ctlTotalRemittanceTHB.Visible = true;
                        ctlLblTotalRemittanceTHB.Visible = true;
                    }
                    #endregion
                }
                ctlUpdatePanelPaymentDetail.Update();
            }
        }

        public void ShowPaymentInfo()
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TransactionId);
            ExpenseDataSet.FnExpenseDocumentRow expRow = expDS.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);
            long tempDocumentID = expRow.DocumentID;
            int status = 2; // FnAutoPayment Success -> Status = 2
            FnAutoPayment payment = ScgeAccountingQueryProvider.FnAutoPaymentQuery.GetFnAutoPaymentSuccessByDocumentID(tempDocumentID, status);
            StringBuilder paymentInfo = new StringBuilder();
            if (payment != null)
            {
                if (ctlDdlPaymentType.SelectedValue.Equals(PaymentType.TR))
                {
                    if (payment.PaymentDate.HasValue)
                        ctlDateLabel.Visible = true;
                    ctlDate.Text = String.Format(" :{0} ,", UIHelper.BindDate(payment.PaymentDate));
                    if (!string.IsNullOrEmpty(payment.ChequeBankName))
                        ctlBankNameLabel.Visible = true;
                    ctlBankName.Text = String.Format(" :{0} ,", payment.ChequeBankName);
                    if (!string.IsNullOrEmpty(payment.PayeeBankAccountNumber))
                        ctlAccountBankNameLabel.Visible = true;
                    ctlAccountBankName.Text = String.Format(" :{0} ", payment.PayeeBankAccountNumber);
                }
                if (ctlDdlPaymentType.SelectedValue.Equals(PaymentType.CQ))
                {
                    if (payment.ChequeDate.HasValue)
                        ctlDateLabel.Visible = true;
                    ctlDate.Text = String.Format(" :{0} ,", UIHelper.BindDate(payment.ChequeDate));
                    if (!string.IsNullOrEmpty(payment.ChequeBankName))
                        ctlBankNameLabel.Visible = true;
                    ctlBankName.Text = String.Format(" :{0} ,", payment.ChequeBankName);
                    if (!string.IsNullOrEmpty(payment.ChequeNumber.Trim()))
                        ctlAccountBankNameLabel.Visible = true;
                    ctlAccountBankName.Text = String.Format(" :{0} ", payment.ChequeNumber);
                }
            }
        }

        protected void ctlDdlCounterCashier_SelectedIndexChanged(object sender, EventArgs e)
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TransactionId);

            DataRow[] advanceList = expDS.FnExpenseAdvance.Select();

            if (ExpenseDocumentEditor != null)
            {
                if (IsRepOffice && advanceList.Count() > 0)
                {
                    ExpenseDocumentEditor.ValidationErrors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("CannotChangeCounterCashier"));
                    CounterCashierID = TempCounterCashierID;
                }
                else
                {
                    ExpenseDocumentEditor.NotifyCheckRepOffice();
                }

                ExpenseDocumentEditor.DefaultSupplementary();
            }
        }
    }
}
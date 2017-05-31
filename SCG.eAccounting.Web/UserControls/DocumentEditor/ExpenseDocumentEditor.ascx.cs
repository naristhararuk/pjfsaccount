using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SS.Standard.Security;
using SS.Standard.UI;
using SS.Standard.Utilities;

using SCG.eAccounting.BLL;
using SCG.eAccounting.DAL;
using SCG.eAccounting.DTO;
using SCG.eAccounting.Query;
using SCG.eAccounting.Web.Helper;

using SS.DB.DTO;
using SS.DB.Query;
using SCG.DB.DTO;
using SS.SU.DTO;
using SS.SU.Query;
using SCG.eAccounting.DTO.DataSet;
using SS.Standard.WorkFlow.Service;
using SS.Standard.WorkFlow.Query;
using SCG.DB.Query;
using SCG.eAccounting.BLL.Implement;
using SCG.eAccounting.SAP.BAPI.Service.Const;
using log4net;
using System.Data;
using SCG.eAccounting.Web.UserControls.DocumentEditor.Components;
using SS.Standard.WorkFlow.DTO;
using SCG.eAccounting.DTO.ValueObject;
using System.Text;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor
{
    public partial class ExpenseDocumentEditor : BaseUserControl, IDocumentEditor
    {
        private static ILog logger = log4net.LogManager.GetLogger(typeof(ExpenseDocumentEditor));
        #region Service Property
        public ISCGDocumentService SCGDocumentService { get; set; }
        public IDocumentAttachmentService DocumentAttachmentService { get; set; }
        public ITransactionService TransactionService { get; set; }
        public IFnExpenseDocumentService FnExpenseDocumentService { get; set; }
        public IFnExpenseInvoiceService FnExpenseInvoiceService { get; set; }
        public IFnExpenseInvoiceItemService FnExpenseInvoiceItemService { get; set; }
        public IFnExpensePerdiemService FnExpensePerdiemService { get; set; }
        public IFnExpensePerdiemItemService FnExpensePerdiemItemService { get; set; }
        public IFnExpenseMileageService FnExpenseMileageService { get; set; }
        public IFnExpenseMileageItemService FnExpenseMileageItemService { get; set; }
        public IWorkFlowService WorkFlowService { get; set; }
        public IFnExpenseAdvanceService FnExpenseAdvanceService { get; set; }
        #endregion

        #region Property
        public string HeaderStatus
        {
            set { ctlFormHeader.Status = value; }
        }
        public string HeaderForm
        {
            set { ctlFormHeader.HeaderForm = value; }
        }
        public Guid TransactionID
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
            get { return ctlType.Text; }
            set { ctlType.Text = value; }
        }
        public string InitialFlag
        {
            get { return ViewState[ViewStateName.InitialFlag].ToString(); }
            set { ViewState[ViewStateName.InitialFlag] = value; }
        }
        public IList<object> VisibleFields
        {
            get { return (IList<object>)ViewState[ViewStateName.VisibleFields]; }
            set { ViewState[ViewStateName.VisibleFields] = value; }
        }
        public IList<object> EditableFields
        {
            get { return (IList<object>)ViewState[ViewStateName.EditableFields]; }
            set { ViewState[ViewStateName.EditableFields] = value; }
        }
        public long TempCompanyID
        {
            get { return ViewState["TempCompanyID"] == null ? 0 : (long)ViewState["TempCompanyID"]; }
            set { ViewState["TempCompanyID"] = value; }
        }
        public bool isNotViewPost
        {
            get
            {
                if (ViewState["isNotViewPost"] == null)
                {
                    return true;
                }
                else
                {
                    return (bool)(ViewState["isNotViewPost"]);
                }
            }
            set { ViewState["isNotViewPost"] = value; }
        }
        public long TempRequesterID
        {
            get { return ViewState["TempRequesterID"] == null ? 0 : (long)ViewState["TempRequesterID"]; }
            set { ViewState["TempRequesterID"] = value; }
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

        #region Overrid Method
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!Page.IsPostBack)
            {
                X.ActiveTab = ctlTabGeneral;
            }
        }
        #endregion

        #region Page_Load Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //                string script = @"<script type='text/javascript'>
                //                    window.onunload = UnLock;
                //                    UnLockParameters['txID']='" + this.TransactionID + "';</script>";
                //                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "addunlockparameter", script, false);

                //ctlFixedAdvanceGrid.DataCountAndBind();
                //CheckShowHideFixedAdvance(this.InitialFlag);
            }

            BaseUserControl ctl = GetCurrentPopUpControl();
            if (ctl is PopupRequireDocumentAttachment)
            {
                ctl.OnPopUpReturn += new BaseUserControl.PopUpReturnEventHandler(ctlWarningRequireAttachmentPopup_OnNotifyPopup);
            }

            // Add if for correct visibility functional of Post Remittance by Anuwat S on 18/05/2009
            if (VisibleFields.Contains(ExpenseFieldGroup.RemittantDetail))
            {
                if (!IsRepOffice && (UIHelper.ParseDouble(ctlPaymentDetail.DifferenceAmount) < 0))
                {
                    ctlPostRemittanceButton.Visible = true;
                    ctlRemittedPostingStatusText.Visible = true;
                    ctlRemittedPostingStatus.Visible = true;
                }
            }
            else
            {
                ctlPostRemittanceButton.Visible = false;
                ctlRemittedPostingStatusText.Visible = false;
                ctlRemittedPostingStatus.Visible = false;
            }

            ctlCompanyField.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlCompanyField_OnObjectReturn);
            ctlRequesterData.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlRequesterData_OnObjectReturn);
            ctlReceiverData.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlReceiverData_OnObjectReturn);
            ctlExpenseGeneral.ExpenseDocumentEditor = this;
            ctlClearingAdvance.ExpenseDocumentEditor = this;
            ctlExpensesMPA.ExpenseDocumentEditor = this;
            ctlPaymentDetail.ExpenseDocumentEditor = this;
            ctlUpdatePanelValidation.Update();
        }

        void OnDsNull()
        {
            if (dsNullHandler != null)
                dsNullHandler();
        }
        #endregion

        protected void ctlCompanyField_OnObjectReturn(object sender, ObjectLookUpReturnArgs e)
        {
            try
            {
                FnExpenseDocumentService.CanChangeCompany(this.TransactionID, this.ExpDocumentID);
                if (e.ObjectReturn != null)
                {
                    DbCompany comp = (DbCompany)e.ObjectReturn;
                    DbCompany company = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(comp.CompanyID);
                    if (company != null)
                    {
                        this.TempCompanyID = company.CompanyID;

                        CheckRepOffice();
                        ctlClearingAdvance.CompanyID = UIHelper.ParseLong(ctlCompanyField.CompanyID);
                        ctlExpenseGeneral.CompanyID = UIHelper.ParseLong(ctlCompanyField.CompanyID);
                        ctlExpensesMPA.CompanyID = UIHelper.ParseLong(ctlCompanyField.CompanyID);
                        //ในการ select ค่าของ service team ต้องเอา ค่า ของ location ไป  where ด้วย ในกรณีที่ company of document = company of requester else ไม่ต้อง  where
                        //ถ้ามีค่ามากกว่า 1 rows ให้ขึ้นคำว่า please select แต่มี 1 rows ให้ default เลย
                        long locationID = 0;
                        if (ctlCompanyField.CompanyID == ctlRequesterData.CompanyID)
                        {
                            SuUser userList = QueryProvider.SuUserQuery.FindByIdentity(UIHelper.ParseLong(ctlRequesterData.UserID));
                            if (userList != null)
                            {
                                if (userList.Location != null)
                                    locationID = userList.Location.LocationID;
                            }
                        }
                        //add parameter location code
                        ctlPaymentDetail.BindServiceTeam(company.CompanyID, string.Empty, UIHelper.ParseLong(ctlRequesterData.UserID), locationID);
                        if (!IsRepOffice)
                        {
                            ctlPaymentDetail.PaymentTypeID = company.PaymentType;
                            ctlPaymentDetail.BindCounterCashier(company.CompanyID);
                            ctlCurrencyDropdown.Items.Clear();
                            ctlCurrencyDropdownDiv.Style["display"] = "none";
                            ctlExpenseGeneral.SelectedCurrency = string.Empty;
                        }
                        else
                        {
                            ctlPaymentDetail.PaymentTypeID = PaymentType.CA;
                            ctlPaymentDetail.BindCounterCashier(UIHelper.ParseLong(ctlCompanyField.CompanyID));
                        }

                        this.DefaultPb();
                        BindCurrencyDropdown();
                        if (IsRepOffice)
                        {
                            //DefaultFinalCurrency();
                            SetLocalCurrency(UIHelper.ParseShort(ctlCurrencyDropdown.SelectedValue));
                            ctlExpenseGeneral.SelectedCurrency = ctlCurrencyDropdown.SelectedItem == null ? string.Empty : ctlCurrencyDropdown.SelectedItem.ToString();
                            ctlPaymentDetail.LocalCurrencyID = UIHelper.ParseShort(ctlCurrencyDropdown.SelectedValue);
                        }
                        ctlExpenseGeneral.FilterCostCenterInGeneralExpense(company.CompanyID);
                        //add by tom 01/07/2009, reset cost center when change company.
                        ctlExpenseGeneral.ResetCostCenterInGenaralExpense(company.CompanyID);
                        ctlExpenseGeneral.BindSimpleExpenseGrid(this.DocumentType, UIHelper.ParseLong(ctlRequesterData.UserID), true);
                        ctlExpenseGeneral.BindControl();
                        ctlPaymentDetail.BindControl();
                        ctlFixedAdvanceGrid.DataCountAndBind();
                        CheckShowHideFixedAdvance(this.InitialFlag);
                        ctlClearingAdvance.BindControl(true);
                        UpdateExpenseTab.Update();
                        ctlPaymentDetail.ShowHideCounterCasheir();
                    }
                    else
                    {
                        ctlFixedAdvanceGrid.DataCountAndBind();
                        ctlFixedAdvance.Visible = false;
                    }
                }
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                ctlCompanyField.SetValue(this.TempCompanyID);
                ctlUpdatePanelValidation.Update();
            }
            catch (Exception ex)
            {
                logger.Error("ExpenseDocumentEditor change company  ", ex);
                throw;
            }
            ctlUpdatePanelApprover.Update();
        }
        protected void ctlRequesterData_OnObjectReturn(object sender, ObjectLookUpReturnArgs e)
        {

            if (!CanChangeRequester())
            {
                string ErrorMessage;
                ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(this.TransactionID);
                DataRow[] mileageRows = expDs.FnExpenseMileage.Select(String.Format("ExpenseID = {0} ", this.ExpDocumentID));

                if (expDs.FnExpenseCA.Count() != 0 || expDs.FnExpenseMPA.Count() != 0)
                {
                    ErrorMessage = "MPA or CA Document Attachment has already exist. Requester can't be change";
                }
                else if (mileageRows.Length > 0)
                {
                    ErrorMessage = "Mileage has already exist. Requester can't be change";
                }
                else
                {
                    ErrorMessage = "DeletePerdiem_BeforeChangingRequester";
                }

                ValidationErrors.AddError("Provider.Error", new Spring.Validation.ErrorMessage(ErrorMessage));
                ctlRequesterData.SetValue(this.TempRequesterID);
                ctlUpdatePanelValidation.Update();
            }
            else
            {
                if (e.ObjectReturn != null)
                {
                    SuUser userInfo = (SuUser)e.ObjectReturn;
                    this.TempRequesterID = userInfo.Userid;
                    CheckRepOffice();

                    ctlReceiverData.SetValue(userInfo.Userid);
                    //ctlPaymentDetail.BindServiceTeam(UIHelper.ParseLong(ctlRequesterData.CompanyID), ctlRequesterData.CompanyCode, UIHelper.ParseLong(ctlRequesterData.UserID));
                    ctlExpenseGeneral.CompanyID = UIHelper.ParseLong(ctlCompanyField.CompanyID);

                    ctlClearingAdvance.RequesterID = UIHelper.ParseLong(ctlRequesterData.UserID);
                    ctlApproverData.ShowDefaultApprover(userInfo.Userid);
                    ctlInitiator.RequesterID = UIHelper.ParseLong(ctlRequesterData.UserID);
                    ctlApproverData.RequesterID = UIHelper.ParseLong(ctlRequesterData.UserID);

                    if (!IsRepOffice)
                    {
                        ctlCurrencyDropdown.Items.Clear();
                        ctlCurrencyDropdownDiv.Style["display"] = "none";
                        ctlExpenseGeneral.SelectedCurrency = string.Empty;
                    }
                    else
                    {
                        ctlPaymentDetail.PaymentTypeID = PaymentType.CA;
                        ctlPaymentDetail.BindCounterCashier(UIHelper.ParseLong(ctlCompanyField.CompanyID));
                    }

                    this.DefaultPb();
                    BindCurrencyDropdown();
                    if (IsRepOffice)
                    {
                        //DefaultFinalCurrency();
                        SetLocalCurrency(UIHelper.ParseShort(ctlCurrencyDropdown.SelectedValue));
                        ctlExpenseGeneral.SelectedCurrency = ctlCurrencyDropdown.SelectedItem == null ? string.Empty : ctlCurrencyDropdown.SelectedItem.ToString();
                        ctlPaymentDetail.LocalCurrencyID = UIHelper.ParseShort(ctlCurrencyDropdown.SelectedValue);
                    }

                    ctlExpenseGeneral.BindSimpleExpenseGrid(DocumentType, userInfo.Userid, true);
                    ctlExpenseGeneral.BindControl();
                    ctlPaymentDetail.BindControl();
                    ctlPaymentDetail.ShowHideCounterCasheir();
                }
            }
            ctlExpensesMPA.RequesterID = this.TempRequesterID;/*n edit*/
            ctlUpdatePanelHeader.Update();
            ctlUpdatePanelApprover.Update();

            ctlFixedAdvanceGrid.DataCountAndBind();
            CheckShowHideFixedAdvance(this.InitialFlag);
        }
        protected void ctlReceiverData_OnObjectReturn(object sender, ObjectLookUpReturnArgs e)
        {
            CheckRequesterAndReceiverIsSamePerson();
        }
        public bool CanChangeRequester()
        {
            bool canChange = true;
            ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(this.TransactionID);

            DataRow[] mileageRows = expDs.FnExpenseMileage.Select(String.Format("ExpenseID = {0} ", this.ExpDocumentID));
            DataRow[] invoiceRows = expDs.FnExpenseInvoice.Select(String.Format("ExpenseID = {0} and InvoiceDocumentType = 'P'", this.ExpDocumentID));

            if (invoiceRows.Length > 0)
            {
                canChange = false;
            }

            if (expDs.FnExpenseCA.Count() != 0 || expDs.FnExpenseMPA.Count() != 0)
            {
                canChange = false;
            }

            if (mileageRows.Length > 0)
            {
                canChange = false;
            }
            return canChange;
        }

        public bool CanChangeFinalCurrency()
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            bool canchange = true;
            ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(this.TransactionID);

            DataRow[] invoiceRows = expDs.FnExpenseInvoice.Select(String.Format("ExpenseID = {0}", this.ExpDocumentID));

            DataRow[] advRows = expDs.FnExpenseAdvance.Select(String.Format("ExpenseID = {0}", this.ExpDocumentID));

            if (invoiceRows.Length > 0)
                canchange = false;
            else if (advRows.Length > 0)
                canchange = false;

            return canchange;
        }

        #region Private Method

        private void InitializeControl()
        {
            try
            {
                ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TransactionID);
                ExpenseDataSet.FnExpenseDocumentRow expRow = expDS.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);
                long tempDocumentID = expRow.DocumentID;

                ctlCompanyField.UseEccOnly = true;
                ctlCompanyField.FlagActive = true;
                ctlPaymentDetail.DocumentType = DocumentType;
                ctlExpenseGeneral.DocumentType = DocumentType;
                ctlClearingAdvance.DocumentType = DocumentType;
                string mode = this.InitialFlag;
                // หา  current state ของเอกสาร
                if (!string.IsNullOrEmpty(Request.Params["wfid"]) && UIHelper.ParseInt(Request.Params["wfid"]) > 0)
                {
                    long workFlowID = UIHelper.ParseLong(Request.Params["wfid"]);

                    SS.Standard.WorkFlow.DTO.WorkFlow wf = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
                    if (wf != null && wf.CurrentState != null && wf.CurrentState.Name != null)
                    {
                        if (wf.CurrentState.Name.Equals(WorkFlowStateFlag.WaitPayment))
                            mode = FlagEnum.ViewFlag;
                        else if (wf.CurrentState.Name.Equals(WorkFlowStateFlag.Hold) && EditableFields.Count == 0)
                        {
                            mode = FlagEnum.ViewFlag;
                        }
                    }
                }

                // **Initialize Header**
                ctlApproverData.DocumentEditor = this;
                ctlFormHeader.Initialize(this.TransactionID, tempDocumentID, this.InitialFlag);     // send SCGDocument.DocumentID for check visible see history
                ctlCreatorData.ControlGroupID = SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.BuActor;
                ctlRequesterData.ControlGroupID = SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.BuActor;
                ctlReceiverData.ControlGroupID = SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.BuActor;
                ctlApproverData.ControlGroupID = SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.BuActor;

                ctlCreatorData.Initialize(this.TransactionID, this.ExpDocumentID, this.InitialFlag);
                ctlRequesterData.Initialize(this.TransactionID, this.ExpDocumentID, this.InitialFlag);
                ctlReceiverData.Initialize(this.TransactionID, this.ExpDocumentID, this.InitialFlag);
                ctlApproverData.Initialize(this.TransactionID, this.ExpDocumentID, this.InitialFlag);

                ctlExpenseGeneral.CompanyID = UIHelper.ParseLong(ctlCompanyField.CompanyID);
                ctlExpenseGeneral.Initialize(this.TransactionID, this.ExpDocumentID, mode);     // ถ้าเป็น wait for payment จะแก้ไข invoice, perdiem, mileage ไม่ได้
                ctlExpensesMPA.Initialize(this.TransactionID, this.ExpDocumentID, mode);
                ctlClearingAdvance.Initialize(this.TransactionID, this.ExpDocumentID, mode);    // ถ้าเป็น wait for payment จะแก้ไข clearing advance ไม่ได้
                ctlAttachment.Initialize(this.TransactionID, this.ExpDocumentID, mode);         // ถ้าเป็น wait for payment จะแก้ไข attachment ไม่ได้
                ctlPaymentDetail.Initialize(this.TransactionID, this.ExpDocumentID, this.InitialFlag);
                ctlInitiator.ControlGroupID = SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Initiator;
                ctlInitiator.Initialize(this.TransactionID, tempDocumentID, this.InitialFlag);
                ctlHistory.Initialize(tempDocumentID);
                ctlExpenseViewByAccount.Initialize(this.TransactionID, this.ExpDocumentID, this.InitialFlag);
            }
            catch (Exception ex)
            {
                logger.Error("Initialize", ex);
                throw;
            }

            ctlCreatorData.DataBind();
            ctlApproverData.DataBind();
            ctlReceiverData.DataBind();
            ctlRequesterData.DataBind();
            ctlFormHeader.DataBind();
        }
        private void BindControl(bool isCopy)
        {
            try
            {
                Guid txID = this.TransactionID;
                long expDocumentID = this.ExpDocumentID;
                ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(txID);
                ExpenseDataSet.FnExpenseDocumentRow expRow = expDS.FnExpenseDocument.FindByExpenseID(expDocumentID);
                ExpenseDataSet.DocumentRow docRow = expDS.Document.FindByDocumentID(expRow.DocumentID);
                System.Data.DataRow[] attRow = expDS.DocumentAttachment.Select("DocumentID=" + expRow.DocumentID);
                if (!isCopy && this.InitialFlag.Equals(FlagEnum.NewFlag))
                {
                    ctlFormHeader.Status = FlagEnum.NewFlag;
                    ctlCompanyField.ShowDefault();
                    ctlCreatorData.ShowDefault();
                    ctlRequesterData.ShowDefault();
                    ctlInitiator.RequesterID = UIHelper.ParseLong(ctlRequesterData.UserID);
                    ctlReceiverData.ShowDefault();
                    ctlApproverData.ShowDefaultApprover(UIHelper.ParseLong(ctlRequesterData.UserID));

                    ctlPaymentDetail.BindPaymentType();

                    CheckRepOffice();
                    //ในการ select ค่าของ service team ต้องเอา ค่า ของ location ไป  where ด้วย ในกรณีที่ company of document = company of requester else ไม่ต้อง  where
                    //ถ้ามีค่ามากกว่า 1 rows ให้ขึ้นคำว่า please select แต่มี 1 rows ให้ default เลย
                    long locationID = 0;
                    if (ctlCompanyField.CompanyID == ctlRequesterData.CompanyID)
                    {
                        SuUser userList = QueryProvider.SuUserQuery.FindByIdentity(UIHelper.ParseLong(ctlRequesterData.UserID));
                        if (userList != null)
                        {
                            if (userList.Location != null)
                                locationID = userList.Location.LocationID;
                        }
                    }

                    //if not rep office -> set default payment type by company
                    if (!IsRepOffice)
                    {
                        long companyId = UIHelper.ParseLong(ctlCompanyField.CompanyID);
                        if (companyId > 0)
                        {
                            DbCompany company = ScgDbQueryProvider.DbCompanyQuery.FindProxyByIdentity(companyId);
                            if (company != null)
                                ctlPaymentDetail.PaymentTypeID = company.PaymentType;
                        }

                        ctlExpenseGeneral.SelectedCurrency = string.Empty;
                    }
                    else
                    {
                        ctlPaymentDetail.PaymentTypeID = PaymentType.CA;

                    }
                    ctlPaymentDetail.BindCounterCashier(UIHelper.ParseLong(ctlCompanyField.CompanyID));
                    DefaultPb();
                    BindCurrencyDropdown();

                    if (IsRepOffice)
                    {
                        // default final currency
                        //DefaultFinalCurrency();
                        if (ctlCurrencyDropdown.SelectedItem != null)
                        {
                            SetLocalCurrency(UIHelper.ParseShort(ctlCurrencyDropdown.SelectedValue));
                            ctlExpenseGeneral.SelectedCurrency = ctlCurrencyDropdown.SelectedItem.ToString();
                            ctlPaymentDetail.LocalCurrencyID = UIHelper.ParseShort(ctlCurrencyDropdown.SelectedValue);
                        }
                    }

                    //add parameter location code
                    ctlPaymentDetail.BindServiceTeam(UIHelper.ParseLong(ctlCompanyField.CompanyID), ctlCompanyField.CompanyCode, UserAccount.UserID, locationID);
                    ctlExpenseGeneral.CompanyID = UIHelper.ParseLong(ctlCompanyField.CompanyID);
                    ctlExpenseGeneral.BindSimpleExpenseGrid(DocumentType, UIHelper.ParseLong(ctlRequesterData.UserID), true);
                    ctlExpenseGeneral.FilterCostCenterInGeneralExpense(UIHelper.ParseLong(ctlCompanyField.CompanyID));
                    ctlExpensesMPA.CompanyID = UIHelper.ParseLong(ctlCompanyField.CompanyID);
                    ctlExpensesMPA.RequesterID = UIHelper.ParseLong(ctlRequesterData.UserID);
                    ctlPaymentDetail.BindControl();
                }
                else
                {
                    this.DocumentType = FnExpenseDocumentService.GetExpenseType(this.ExpDocumentID, this.TransactionID);
                    if (this.DocumentType.Equals(ZoneType.Domestic))
                    {
                        HeaderForm = GetProgramMessage("$DMExpenseForm$");
                    }
                    else
                    {
                        HeaderForm = GetProgramMessage("$FRExpenseForm$");
                    }
                    if (!isCopy)
                    {
                        ctlFormHeader.No = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(expRow.DocumentID).DocumentNo;
                        if (docRow.DocumentDate != DateTime.MinValue)
                            ctlFormHeader.CreateDate = UIHelper.ToDateString(docRow.DocumentDate);
                        ctlFormHeader.Status = ScgeAccountingQueryProvider.SCGDocumentQuery.GetDocumentCurrentStateName(UserAccount.CurrentLanguageID, expRow.DocumentID);
                        ctlCreatorData.SetValue(docRow.CreatorID);
                    }
                    else
                    {
                        ctlFormHeader.Status = FlagEnum.NewFlag;
                        ctlCreatorData.ShowDefault();
                    }

                    ctlMemo.Text = docRow.Memo;
                    ctlCompanyField.SetValue(docRow.CompanyID);
                    ctlDescriptionHeader.Text = docRow.Subject;

                    ctlRequesterData.SetValue(docRow.RequesterID);
                    ctlReceiverData.SetValue(docRow.ReceiverID);
                    ctlApproverData.SetValue(docRow.ApproverID);
                    ctlApproverData.RequesterID = UIHelper.ParseLong(ctlRequesterData.UserID);

                    //ในการ select ค่าของ service team ต้องเอา ค่า ของ location ไป  where ด้วย ในกรณีที่ company of document = company of requester else ไม่ต้อง  where
                    //ถ้ามีค่ามากกว่า 1 rows ให้ขึ้นคำว่า please select แต่มี 1 rows ให้ default เลย
                    long locationID = 0;
                    if (ctlCompanyField.CompanyID == ctlRequesterData.CompanyID)
                    {
                        SuUser userList = QueryProvider.SuUserQuery.FindByIdentity(UIHelper.ParseLong(ctlRequesterData.UserID));
                        if (userList != null)
                        {
                            if (userList.Location != null)
                                locationID = userList.Location.LocationID;
                        }
                    }

                    CheckRepOffice();
                    //add parameter location code
                    try
                    {
                        ctlPaymentDetail.BindServiceTeam(UIHelper.ParseLong(ctlCompanyField.CompanyID), ctlCompanyField.CompanyCode, UIHelper.ParseLong(ctlRequesterData.UserID), locationID);
                        ctlPaymentDetail.ServiceTeamID = expRow.ServiceTeamID.ToString();

                        ctlPaymentDetail.BindPaymentType();
                        ctlPaymentDetail.PaymentTypeID = expRow.PaymentType;

                        ctlPaymentDetail.BindCounterCashier(UIHelper.ParseLong(ctlCompanyField.CompanyID));

                        if (!expRow.IsPBIDNull())
                        {
                            ctlPaymentDetail.CounterCashierID = expRow.PBID;
                            ctlPaymentDetail.TempCounterCashierID = expRow.PBID;
                        }

                    }
                    catch (Exception ex)
                    {
                        logger.Error("BindServiceTeam", ex);
                    }
                    if (InitialFlag.Equals(FlagEnum.EditFlag))
                    {
                        NotifyUpdateExpense();
                    }
                    if (IsRepOffice)
                    {

                        BindCurrencyDropdown();
                        if (!expRow.IsLocalCurrencyIDNull())
                        {
                            ctlCurrencyDropdown.SelectedValue = expRow.LocalCurrencyID.ToString();
                            ctlPaymentDetail.LocalCurrencyID = expRow.LocalCurrencyID;
                            ctlExpenseGeneral.SelectedCurrency = ctlCurrencyDropdown.SelectedItem.ToString();
                            ctlExpenseViewByAccount.SelectedCurrency = ctlCurrencyDropdown.SelectedItem.ToString();
                        }
                    }
                    else
                    {
                        ctlCurrencyDropdown.Items.Clear();
                        ctlCurrencyDropdownDiv.Style["display"] = "none";
                        ctlExpenseGeneral.SelectedCurrency = string.Empty;
                    }

                    ctlExpenseGeneral.CompanyID = UIHelper.ParseLong(ctlCompanyField.CompanyID);
                    ctlExpensesMPA.CompanyID = UIHelper.ParseLong(ctlCompanyField.CompanyID);
                    ctlExpensesMPA.RequesterID = UIHelper.ParseLong(ctlRequesterData.UserID);
                    ctlExpenseGeneral.BindExpenseGrid();
                    ctlPaymentDetail.BindControl();
                    ctlExpenseViewByAccount.BindControl(true);
                    NotifyPaymentDetailChange();
                    if (!string.IsNullOrEmpty(docRow["BranchCode"].ToString().Trim()))
                        ctlBranch.Text = docRow["BranchCode"].ToString();
                    else
                    {
                        if (this.InitialFlag.Equals(FlagEnum.EditFlag))
                            ctlBranch.Text = "0001"; //case edit must be default branch = 0001.[modify by meaw (log#1829)]
                        else
                            ctlBranch.Text = string.Empty;
                    }

                    if (ParameterServices.EditableBusinessArea)
                    {
                        ctlBusinessArea.Enabled = true;
                        ctlBusinessAreaDomesticText.Style["display"] = "block";
                        ctlBusinessArea.Style["display"] = "block";
                        ctlBusinessAreaLabelExtender.Style["display"] = "block";
                    }
                    else
                    {
                        ctlBusinessArea.Enabled = false;
                        ctlBusinessAreaDomesticText.Style["display"] = "none";
                        ctlBusinessArea.Style["display"] = "none";
                        ctlBusinessAreaLabelExtender.Style["display"] = "none";
                    }
                    if (ParameterServices.EditableSupplementary)
                    {
                        ctlSupplementary.Enabled = true;
                    }
                    else
                    {
                        ctlSupplementary.Enabled = false;
                    }

                    if (string.IsNullOrEmpty(docRow["BusinessArea"].ToString()))
                    {
                        if (this.InitialFlag.Equals(FlagEnum.EditFlag))
                        {
                            DbCompany com = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(UIHelper.ParseLong(ctlCompanyField.CompanyID));
                            ctlBusinessArea.Text = com.BusinessArea;
                        }
                    }
                    else
                    {
                        ctlBusinessArea.Text = docRow["BusinessArea"].ToString();
                    }

                    if (string.IsNullOrEmpty(docRow["Supplementary"].ToString()) && !this.InitialFlag.Equals(FlagEnum.ViewFlag))
                    {
                        DefaultSupplementary();
                    }
                    else
                    {
                        ctlSupplementary.Text = docRow["Supplementary"].ToString();
                    }

                    if (!docRow.IsPostingDateNull() && !docRow.PostingDate.Equals(DateTime.MinValue))
                        ctlPostingDateCalendar.DateValue = UIHelper.ToDateString(docRow.PostingDate);
                    else
                    {
                        //case edit must be default postingdate is datetime.now
                        if (this.InitialFlag.Equals(FlagEnum.EditFlag))
                            ctlPostingDateCalendar.DateValue = UIHelper.ToDateString(DateTime.Today);
                        else
                            ctlPostingDateCalendar.DateValue = string.Empty;
                    }

                    if (!docRow.IsBaseLineDateNull() && !docRow.BaseLineDate.Equals(DateTime.MinValue))
                        ctlBaselineDate.DateValue = UIHelper.ToDateString(docRow.BaseLineDate);
                    else
                    {
                        if (this.InitialFlag.Equals(FlagEnum.EditFlag))
                        {
                            ctlBaselineDate.DateValue = UIHelper.ToDateString(DateTime.Today);
                            if (ctlPaymentDetail.PaymentTypeID.Equals(PaymentType.TR))
                                ctlBaselineDate.DateValue = UIHelper.ToDateString(DateTime.Today.AddDays(3));
                        }
                        else
                            ctlBaselineDate.DateValue = string.Empty;
                    }
                    if (!docRow.IsIsVerifyImageNull())
                        ctlVerifyImage.Checked = docRow.IsVerifyImage;

                    ctlPaymentMethod.DataSource = ScgDbQueryProvider.DbCompanyPaymentMethodQuery.GetPaymentMethod(UIHelper.ParseLong(ctlCompanyField.CompanyID));
                    ctlPaymentMethod.DataTextField = "PaymentMethodCode";
                    ctlPaymentMethod.DataValueField = "PaymentMethodID";

                    ctlPaymentMethod.DataBind();
                    //add condition by oum ; case edit must be show please select
                    if (ctlPaymentMethod.Items.Count > 0)
                    {
                        if (this.InitialFlag.Equals(FlagEnum.ViewFlag))
                            ctlPaymentMethod.Items.Insert(0, new ListItem(string.Empty, string.Empty));
                        else
                            ctlPaymentMethod.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
                    }
                    //else // ถ้าไม่มีค่า Default 0001
                    //    ctlBranchDomestic.Text = "0001";
                    if (!docRow.IsPaymentMethodIDNull() && docRow.PaymentMethodID != 0)
                        ctlPaymentMethod.SelectedValue = docRow.PaymentMethodID.ToString();
                    else
                    {
                        if (ctlPaymentMethod.Items.Count > 1)
                        {
                            if (!InitialFlag.Equals(FlagEnum.ViewFlag))
                            {
                                SelectedPaymentMethod();
                            }
                        }
                        else
                        {
                            ctlPaymentMethod.SelectedValue = string.Empty;
                        }
                    }

                    string posting = "N";
                    if (!string.IsNullOrEmpty(docRow["PostingStatus"].ToString()))
                        posting = docRow.PostingStatus;
                    ctlPostingStatus.Text = GetMessage(String.Format("PostingStatus{0}", posting));

                    string remitedPosting = "N";
                    if (!string.IsNullOrEmpty(expRow["RemittancePostingStatus"].ToString()))
                        remitedPosting = expRow.RemittancePostingStatus;
                    ctlRemittedPostingStatus.Text = GetMessage(String.Format("PostingStatus{0}", remitedPosting));

                    if (!expRow.IsBoxIDNull())
                        ctlBoxID.Text = expRow.BoxID;
                }
                ctlExpenseGeneral.BindControl();
                this.TempCompanyID = UIHelper.ParseLong(ctlCompanyField.CompanyID);
                this.TempRequesterID = UIHelper.ParseLong(ctlRequesterData.UserID);
                ctlClearingAdvance.CompanyID = UIHelper.ParseLong(ctlCompanyField.CompanyID);
                ctlClearingAdvance.RequesterID = UIHelper.ParseLong(ctlRequesterData.UserID);
                ctlPaymentDetail.BindDifferenceAmountSummary();

                #region Show/Hide BoxID Detail
                if (VisibleFields.Contains(ExpenseFieldGroup.BoxID))
                    ctlDivReceiveDetail.Style.Add("display", "inline-block");
                else
                    ctlDivReceiveDetail.Style.Add("display", "none");
                #endregion

                #region Show/Hide Verify Detail

                decimal totalExpense = 0;
                decimal totalAdvance = 0;
                decimal totalRemittance = 0;

                if (expRow != null)
                {
                    totalExpense = expRow.IsTotalExpenseNull() ? 0 : expRow.TotalExpense;
                    totalAdvance = expRow.IsTotalAdvanceNull() ? 0 : expRow.TotalAdvance;
                    totalRemittance = expRow.IsTotalRemittanceNull() ? 0 : expRow.TotalRemittance;
                }

                if (!VisibleFields.Contains(ExpenseFieldGroup.VerifyDetail))
                {
                    ctlDivVerifyDetail.Style.Add("display", "none");
                    // ctlDivReceiveDetail.Style.Add("display", "none");
                }
                else
                {
                    ctlDivVerifyDetail.Style.Add("display", "inline-block");
                    //ctlDivReceiveDetail.Style.Add("display", "inline-block");
                    if (InitialFlag.Equals(FlagEnum.ViewFlag))
                    {
                        if (!totalExpense.Equals(0))
                        {
                            ctlViewPostButton.Visible = true;
                            ctlPostingStatusText.Visible = true;
                            ctlPostingStatus.Visible = true;
                        }
                        else
                        {
                            ctlViewPostButton.Visible = false;
                            ctlPostingStatusText.Visible = false;
                            ctlPostingStatus.Visible = false;
                        }

                        if (VisibleFields.Contains(ExpenseFieldGroup.RemittantDetail))
                        {
                            ctlPostRemittanceButton.Visible = true;
                            ctlRemittedPostingStatusText.Visible = true;
                            ctlRemittedPostingStatus.Visible = true;
                        }
                        else
                        {
                            ctlPostRemittanceButton.Visible = false;
                            ctlRemittedPostingStatusText.Visible = false;
                            ctlRemittedPostingStatus.Visible = false;
                        }
                    }
                    else
                    {
                        if (!totalExpense.Equals(0))
                        {
                            ctlViewPostButton.Visible = false;
                            ctlPostingStatusText.Visible = true;
                            ctlPostingStatus.Visible = true;
                        }
                        else
                        {
                            ctlViewPostButton.Visible = false;
                            ctlPostingStatusText.Visible = false;
                            ctlPostingStatus.Visible = false;
                        }
                        ctlPostRemittanceButton.Visible = false;
                        ctlRemittedPostingStatusText.Visible = false;
                        ctlRemittedPostingStatus.Visible = false;
                    }

                    if (attRow.Length == 0)
                        ctlVerifyImage.Visible = false;
                    else
                        ctlVerifyImage.Visible = true;

                    decimal douNetAdvance = totalAdvance - totalRemittance;
                    if (douNetAdvance <= totalExpense)
                    {
                        ctlPostRemittanceButton.Visible = false;
                        ctlRemittedPostingStatusText.Visible = false;
                        ctlRemittedPostingStatus.Visible = false;
                    }
                }
                #endregion

                ctlKBankDetailPanel.Visible = false;
                ctlPayInMethod.Text = string.Empty;
                ctlGLAccount.Text = string.Empty;
                ctlValueDate.Text = string.Empty;
                ctlDivExpRemittanceDetail.Style.Add("display", "none");

                if (VisibleFields.Contains(ExpenseFieldGroup.ExpenseRemittanceDetail))
                {
                    DbCompany comp = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(UIHelper.ParseLong(ctlCompanyField.CompanyID));
                    if (comp.UseSpecialPayIn)
                    {
                        FnExpenseDocument expDoc = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(expRow.DocumentID);
                        if (!string.IsNullOrEmpty(expDoc.ReceivedMethod))
                        {
                            ctlPayInMethod.Text = expDoc.ReceivedMethod;
                            if (expDoc.ReceivedMethod.Equals("Bank"))
                            {
                                string glAccount = expDoc.PayInGLAccount;
                                if (glAccount.Length < 10)
                                {
                                    int length = glAccount.Length;
                                    for (int z = 10; z > length; z--)
                                    {
                                        glAccount = "0" + glAccount;
                                    }
                                }
                                ctlGLAccount.Text = glAccount;
                                ctlValueDate.Text = UIHelper.ToDateString(expDoc.PayInValueDate);
                                ctlKBankDetailPanel.Visible = true;
                            }
                            ctlDivExpRemittanceDetail.Style.Add("display", "inline-block");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Bindcontrol", ex);
                throw;
            }

            UpdateExpenseTab.Update();
            ctlUpdatePanelApprover.Update();
            ctlUpdatePanelViewPost.Update();
            ctlUpdatePanelHeader.Update();
            ctlUpdatePanelMemo.Update();
            //if (this.DocumentType.Equals(ZoneType.Domestic))
            //{
            //    ctlFixedAdvance.Visible = true;
            ctlFixedAdvanceGrid.DataCountAndBind();
            //}
            CheckShowHideFixedAdvance(this.InitialFlag);
            ctlUpdatePanelValidation.Update();
        }

        private void SetDefaultPbForRepOffice()
        {
            ctlPaymentDetail.PaymentTypeID = PaymentType.CA;
            ctlPaymentDetail.BindCounterCashier(UIHelper.ParseLong(ctlCompanyField.CompanyID));
            // set default PB && show/hide currency dropdown
            SuUser requester = QueryProvider.SuUserQuery.FindByIdentity(UIHelper.ParseLong(ctlRequesterData.UserID));
            long requesterLocationId = requester.Location != null ? requester.Location.LocationID : 0;
            DbLocation location = ScgDbQueryProvider.DbLocationQuery.FindProxyByIdentity(requesterLocationId);
            if (location != null)
            {
                if (location.DefaultPBID.HasValue && location.DefaultPBID.Value != 0)
                {
                    if (requester.Company.CompanyID == UIHelper.ParseLong(ctlCompanyField.CompanyID))
                    {
                        ctlPaymentDetail.CounterCashierID = location.DefaultPBID;
                        ctlPaymentDetail.TempCounterCashierID = location.DefaultPBID;
                    }
                    BindCurrencyDropdown();
                    ctlExpenseGeneral.SelectedCurrency = ctlCurrencyDropdown.SelectedItem == null ? string.Empty : ctlCurrencyDropdown.SelectedItem.ToString();
                    ctlPaymentDetail.LocalCurrencyID = UIHelper.ParseShort(ctlCurrencyDropdown.SelectedValue);
                }
            }
        }
        #endregion

        #region IDocumentEditor Members
        public long SaveToDatabase()
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            long expDocumentID = this.ExpDocumentID;
            try
            {
                bool isCopy;
                if (Request.QueryString["cp"] != null && UIHelper.ParseLong(Request.QueryString["cp"]) == 1)
                {
                    isCopy = true;
                }
                else
                {
                    isCopy = false;
                }
                //Validation Data
                FnExpenseMileageItemService.ValdationMileageRateByDataset(this.TransactionID, this.ExpDocumentID, isCopy);
                FnExpenseMileageItemService.SaveExpenseValidationMileageItemOnTransaction(this.TransactionID, this.ExpDocumentID, "Provider", isCopy);
                FnExpensePerdiemService.ValidationSaveDuplicateDateTimeline(this.TransactionID, "Provider", this.ExpDocumentID, isCopy);
                // save ScgDocument and Expense to database
                expDocumentID = FnExpenseDocumentService.SaveExpenseDocument(this.TransactionID, this.ExpDocumentID);
            }
            catch (ServiceValidationException ex)
            {
                errors.MergeErrors(ex.ValidationErrors);
            }
            if (!errors.IsEmpty)
            {
                // If have some validation error then return error.
                throw new ServiceValidationException(errors);
            }
            long workFlowID = 0;
            TransactionService.Commit(this.TransactionID);
            try
            {
                // Get Expense Document
                FnExpenseDocument expDoc = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.FindProxyByIdentity(expDocumentID);
                // Save New WorkFlow.
                if ((expDoc != null) && (expDoc.Document != null))
                {
                    SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.FindProxyByIdentity(expDoc.Document.DocumentID);
                    SS.Standard.WorkFlow.DTO.WorkFlow workFlow = new SS.Standard.WorkFlow.DTO.WorkFlow();
                    workFlow.WorkFlowType = new SS.Standard.WorkFlow.DTO.WorkFlowType(WorkFlowTypeID.ExpenseWorkFlow);
                    workFlow.CurrentState = WorkFlowQueryProvider.WorkFlowStateQuery.FindWorkFlowStateIDByTypeIDAndStateName(WorkFlowTypeID.ExpenseWorkFlow, WorkFlowStateFlag.Draft);
                    workFlow.Description = string.Empty;
                    workFlow.Document = document;
                    workFlow.Active = true;
                    workFlow.CreBy = document.CreBy;
                    workFlow.CreDate = document.CreDate;
                    workFlow.UpdBy = document.UpdBy;
                    workFlow.UpdDate = document.UpdDate;
                    workFlow.UpdPgm = document.UpdPgm;

                    workFlowID = WorkFlowService.CheckExistAndAddNew(workFlow);
                }
            }
            catch (Exception ex)
            {
                //logger.Error("SaveToDataBase", ex);
                throw ex;
            }

            return workFlowID;
        }

        public void Initialize(string initFlag, long? documentID)
        {
            long expDocumentID = 0;
            ExpenseDataSet expenseDS;
            Guid txID = Guid.Empty;
            bool isCopy = false;
            try
            {
                if (initFlag.Equals(FlagEnum.NewFlag) && Request.QueryString["cp"] == null)
                {
                    expenseDS = (ExpenseDataSet)FnExpenseDocumentService.PrepareExpenseDS();
                    txID = TransactionService.Begin(expenseDS);
                    FnExpenseDocument exp = new FnExpenseDocument();
                    exp.ExpenseType = DocumentType;
                    expDocumentID = FnExpenseDocumentService.AddExpenseDocumentTransaction(exp, txID);
                    ViewState.Add("EXPDataSet", expenseDS);
                    //CheckShowHideFixedAdvance();
                }
                else if (((initFlag.Equals(FlagEnum.EditFlag) || (initFlag.Equals(FlagEnum.ViewFlag)))) && ((documentID.HasValue) && (documentID.Value != 0)))
                {
                    expenseDS = (ExpenseDataSet)FnExpenseDocumentService.PrepareExpenseDS(documentID.Value);
                    txID = TransactionService.Begin(expenseDS);

                    if (expenseDS.FnExpenseDocument.Rows.Count > 0)
                    {
                        expDocumentID = UIHelper.ParseLong(expenseDS.FnExpenseDocument.Rows[0]["ExpenseID"].ToString());
                        DocumentType = expenseDS.FnExpenseDocument.Rows[0]["ExpenseType"].ToString();
                    }
                    ViewState.Add("EXPDataSet", expenseDS);
                }
                else if (Request.QueryString["cp"] != null && UIHelper.ParseLong(Request.QueryString["cp"]) == 1)
                {
                    expenseDS = (ExpenseDataSet)FnExpenseDocumentService.PrepareInternalDataToDataset(UIHelper.ParseLong(Request.QueryString["docId"]), true);
                    txID = TransactionService.Begin(expenseDS);
                    isCopy = true;
                    if (expenseDS.FnExpenseDocument.Rows.Count > 0)
                    {
                        expDocumentID = UIHelper.ParseLong(expenseDS.FnExpenseDocument.Rows[0]["ExpenseID"].ToString());
                        DocumentType = expenseDS.FnExpenseDocument.Rows[0]["ExpenseType"].ToString();
                    }
                    ViewState.Add("EXPDataSet", expenseDS);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Initialize prepare dataset", ex);
                throw ex;
            }
            this.TransactionID = txID;
            this.ExpDocumentID = expDocumentID;
            this.InitialFlag = initFlag;

            this.VisibleFields = FnExpenseDocumentService.GetVisibleFields(documentID);
            this.EditableFields = FnExpenseDocumentService.GetEditableFields(documentID);
            CheckRepOffice();
            GetExchangeRateForRepOffice();
            this.InitializeControl();
            this.BindControl(isCopy);

            #region แสดง Tab
            // Edit By Kla 
            // 15/06/2009
            // For Log#760
            // ให้ Tab ที่มีข้อมูลโชว์ขึ้นมา ส่วน Tab ที่ไม่มีข้อมูล ไม่ต้องโชว์
            string strCurrentState = "";
            if (!string.IsNullOrEmpty(Request.Params["wfid"]) && UIHelper.ParseInt(Request.Params["wfid"]) > 0)
            {
                long workFlowID = UIHelper.ParseLong(Request.Params["wfid"]);

                try
                {
                    SS.Standard.WorkFlow.DTO.WorkFlow wf = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
                    if (wf != null && wf.CurrentState != null && wf.CurrentState.Name != null)
                        strCurrentState = wf.CurrentState.Name;
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    throw ex;
                }
            }

            if (DocumentType == "FR")
            {
                ctlExpensesMPA.Visible = false;
            }

            if (strCurrentState == WorkFlowStateFlag.Draft)
            {
                ctlTabMemo.Visible = true;
                ctlTabInitial.Visible = true;
                ctlTabAttachment.Visible = true;
                ctlTabHistory.Visible = true;
                ctlTabViewByAccount.Visible = true;
                ctlTabAdvanceRemittance.Visible = true;

                UpdateExpenseTab.Update();
            }
            else if (this.InitialFlag.Equals(FlagEnum.ViewFlag))
            {
                if (string.IsNullOrEmpty(ctlMemo.Text))
                    ctlTabMemo.Visible = false;
                else
                    ctlTabMemo.Visible = true;

                if (ctlInitiator.IsEmptyData)
                    ctlTabInitial.Visible = false;
                else
                    ctlTabInitial.Visible = true;

                if (ctlAttachment.IsEmptyData)
                    ctlTabAttachment.Visible = false;
                else
                    ctlTabAttachment.Visible = true;

                if (ctlHistory.IsEmptyData)
                    ctlTabHistory.Visible = false;
                else
                    ctlTabHistory.Visible = true;

                if (ctlExpenseViewByAccount.IsEmptyData)
                    ctlTabViewByAccount.Visible = false;
                else
                    ctlTabViewByAccount.Visible = true;

                if (ctlClearingAdvance.IsEmptyData)
                    ctlTabAdvanceRemittance.Visible = false;
                else
                    ctlTabAdvanceRemittance.Visible = true;

                UpdateExpenseTab.Update();
            }
            else
            {
                ctlTabMemo.Visible = true;
                ctlTabInitial.Visible = true;
                ctlTabAttachment.Visible = true;
                ctlTabHistory.Visible = true;
                ctlTabViewByAccount.Visible = true;
                ctlTabAdvanceRemittance.Visible = true;

                UpdateExpenseTab.Update();
            }
            #endregion แสดง Tab

            if (Convert.ToString(ParameterServices.EnableMPA).ToLower() != "true")
            {
                ctlExpensesMPA.Visible = false;
            }
        }

        public void SaveToDataSet()
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            long tempExpDocumentID = this.ExpDocumentID;
            long tempDocumentID = 0;

            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TransactionID);
            if (expDS != null)
            {
                if (IsRepOffice)
                    GetExchangeRateForRepOffice();

                ExpenseDataSet.FnExpenseDocumentRow expRow = expDS.FnExpenseDocument.FindByExpenseID(tempExpDocumentID);
                tempDocumentID = expRow.DocumentID;

                SCGDocument document = new SCGDocument(tempDocumentID);
                this.GetDocument(document);

                try
                {
                    SCGDocumentService.UpdateTransactionDocument(this.TransactionID, document, true, true);
                }
                catch (ServiceValidationException ex)
                {
                    errors.MergeErrors(ex.ValidationErrors);
                }

                try
                {
                    ctlInitiator.Save();
                }
                catch (ServiceValidationException ex)
                {
                    errors.MergeErrors(ex.ValidationErrors);
                }

                FnExpenseDocument exp = new FnExpenseDocument(expRow.ExpenseID);
                exp.Document = document;
                foreach (GridViewRow row in ctlFixedAdvanceGrid.Rows)
                {
                    CheckBox ctlFixedAdvanceSelect = (CheckBox)ctlFixedAdvanceGrid.Rows[row.RowIndex].FindControl("ctlFixedAdvanceSelect");
                    HiddenField ctlGridDocumentId = (HiddenField)ctlFixedAdvanceGrid.Rows[row.RowIndex].FindControl("ctlGridDocumentId");
                    if (ctlFixedAdvanceSelect.Checked == true)
                    {
                        exp.FixedAdvanceDocument = new FixedAdvanceDocument(UIHelper.ParseLong(ctlGridDocumentId.Value));
                    }
                }
                this.GetExpenseDocument(exp);

                try
                {
                    // Save Expense Document to Transaction.
                    FnExpenseDocumentService.UpdateExpenseDocumentTransaction(exp, this.TransactionID);
                }
                catch (ServiceValidationException ex)
                {
                    errors.MergeErrors(ex.ValidationErrors);
                }

                try
                {
                    ctlClearingAdvance.Save();
                }
                catch (ServiceValidationException ex)
                {
                    errors.MergeErrors(ex.ValidationErrors);
                }

                if (!errors.IsEmpty)
                {
                    // If have some validation error then return error.
                    throw new ServiceValidationException(errors);
                }

                try
                {
                    ctlExpenseGeneral.SaveExpenseRecommend();
                }
                catch (ServiceValidationException ex)
                {
                    throw new ServiceValidationException(ex.ValidationErrors);
                }
            }
        }

        public void ShowWarningRequireAttachmentPopup(string message)
        {
            PopupRequireDocumentAttachment ctlWarningPopup = LoadPopup<PopupRequireDocumentAttachment>("~/UserControls/DocumentEditor/Components/PopupRequireDocumentAttachment.ascx", ctlPopUpWarningRequireAttachmentUpdatePanel);
            ctlWarningPopup.ShowPopup(message);
        }

        protected void ctlWarningRequireAttachmentPopup_OnNotifyPopup(object sender, PopUpReturnArgs args)
        {
            if (args.Type == PopUpReturnType.OK)
            {
                X.ActiveTab = ctlTabAttachment;
                //Response.Redirect("DocumentView.aspx?wfid=" + this.WorkFlowID.ToString() + "&requireAttachment=" + RequireDocumentAttachmentFlag.Required);
                UpdateExpenseTab.Update();
            }
        }

        public long Save()
        {
            if (TransactionService.GetDS(this.TransactionID) == null)
            {
                OnDsNull();
            }

            bool success = false;
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            try
            {
                success = NotifyUpdateExpense();
            }
            catch (ServiceValidationException ex)
            {
                errors.MergeErrors(ex.ValidationErrors);
            }

            if (!errors.IsEmpty)
            {
                // If have some validation error then return error.
                throw new ServiceValidationException(errors);
            }

            if (!success) return 0;

            return this.SaveToDatabase();
        }

        public void ResetControl()
        {
            ctlCompanyField.ShowDefault();
            ctlCreatorData.ShowDefault();
            ctlRequesterData.ShowDefault();
            ctlReceiverData.ShowDefault();
            ctlDescriptionHeader.Text = string.Empty;
            ctlExpenseGeneral.ResetControl();
            ctlApproverData.ShowDefault();
            ctlFixedAdvanceGrid.DataSource = null;
            ctlFixedAdvanceGrid.DataBind();
            //ในการ select ค่าของ service team ต้องเอา ค่า ของ location ไป  where ด้วย ในกรณีที่ company of document = company of requester else ไม่ต้อง  where
            //ถ้ามีค่ามากกว่า 1 rows ให้ขึ้นคำว่า please select แต่มี 1 rows ให้ default เลย
            long locationID = 0;
            if (ctlCompanyField.CompanyID == ctlRequesterData.CompanyID)
            {
                SuUser userList = QueryProvider.SuUserQuery.FindByIdentity(UIHelper.ParseLong(ctlRequesterData.UserID));
                if (userList != null)
                {
                    if (userList.Location != null)
                        locationID = userList.Location.LocationID;
                }
            }

            ctlAttachment.ResetControlValue();
            ctlMemo.Text = string.Empty;

        }
        public void RollBackTransaction()
        {
            TransactionService.Rollback(this.TransactionID);

            //Reset Control
            ResetControl();
        }
        public bool IsContainEditableFields(object editableFieldEnum)
        {
            return this.EditableFields.Contains(editableFieldEnum);
        }
        public bool IsContainVisibleFields(object visibleFieldEnum)
        {
            return this.VisibleFields.Contains(visibleFieldEnum);
        }

        private DsNullHandler dsNullHandler;
        event DsNullHandler IDocumentEditor.DsNull
        {
            add
            {
                dsNullHandler += value;
            }
            remove
            {
                dsNullHandler -= value;
            }
        }

        #endregion

        private void GetDocument(SCGDocument document)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (!string.IsNullOrEmpty(ctlCompanyField.CompanyID))
                document.CompanyID = new DbCompany(UIHelper.ParseLong(ctlCompanyField.CompanyID));

            if (!string.IsNullOrEmpty(ctlCreatorData.UserID))
                document.CreatorID = new SuUser(UIHelper.ParseLong(ctlCreatorData.UserID));

            if (!string.IsNullOrEmpty(ctlRequesterData.UserID))
                document.RequesterID = new SuUser(UIHelper.ParseLong(ctlRequesterData.UserID));

            if (!string.IsNullOrEmpty(ctlReceiverData.UserID))
                document.ReceiverID = new SuUser(UIHelper.ParseLong(ctlReceiverData.UserID));

            if (!string.IsNullOrEmpty(ctlApproverData.UserID))
                document.ApproverID = new SuUser(UIHelper.ParseLong(ctlApproverData.UserID));

            if (DocumentType.Equals(ZoneType.Domestic))
            {
                document.DocumentType = new SS.Standard.WorkFlow.DTO.DocumentType(DocumentTypeID.ExpenseDomesticDocument);
            }
            else if (DocumentType.Equals(ZoneType.Foreign))
            {
                document.DocumentType = new SS.Standard.WorkFlow.DTO.DocumentType(DocumentTypeID.ExpenseForeignDocument);
            }
            document.Subject = ctlDescriptionHeader.Text;
            document.Memo = ctlMemo.Text;
            document.Active = true;

            if (!string.IsNullOrEmpty(ctlBranch.Text.Trim()))
                document.BranchCode = ctlBranch.Text;
            else
                document.BranchCode = "0001";

            document.BusinessArea = ctlBusinessArea.Text;
            document.Supplementary = ctlSupplementary.Text;

            if (!string.IsNullOrEmpty(ctlPaymentMethod.SelectedValue))
                document.PaymentMethodID = UIHelper.ParseLong(ctlPaymentMethod.SelectedValue);
            else
            {
                DbCompany comp = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(UIHelper.ParseLong(ctlCompanyField.CompanyID));
                if (comp != null)
                {
                    switch (ctlPaymentDetail.PaymentTypeID)
                    {
                        case PaymentType.CA:
                            if (comp.PaymentMethodPetty != null)
                                document.PaymentMethodID = comp.PaymentMethodPetty.PaymentMethodID;
                            break;

                        case PaymentType.TR:
                            if (comp.PaymentMethodTransfer != null)
                                document.PaymentMethodID = comp.PaymentMethodTransfer.PaymentMethodID;
                            break;

                        case PaymentType.CQ:
                            if (comp.PaymentMethodCheque != null)
                                document.PaymentMethodID = comp.PaymentMethodCheque.PaymentMethodID;
                            break;
                    }
                }
            }

            try
            {
                if (!string.IsNullOrEmpty(ctlPostingDateCalendar.DateValue))
                    document.PostingDate = UIHelper.ParseDate(ctlPostingDateCalendar.DateValue);
                else
                    document.PostingDate = DateTime.Today;

                if (!string.IsNullOrEmpty(ctlBaselineDate.DateValue))
                    document.BaseLineDate = UIHelper.ParseDate(ctlBaselineDate.DateValue);
                else
                {
                    document.BaseLineDate = DateTime.Today;
                    if (ctlPaymentDetail.PaymentTypeID.Equals(PaymentType.TR))
                        document.BaseLineDate = DateTime.Today.AddDays(3);
                }
            }
            catch (FormatException fex)
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Invalid_Date_Format"));
                throw new ServiceValidationException(errors);
            }
            document.IsVerifyImage = ctlVerifyImage.Checked;
            document.ReferenceNo = ctlClearingAdvance.AdvanceNoTemp;
        }

        private void GetExpenseDocument(FnExpenseDocument exp)
        {
            exp.IsRepOffice = IsRepOffice;
            if (!string.IsNullOrEmpty(ctlPaymentDetail.PaymentTypeID))
                exp.PaymentType = ctlPaymentDetail.PaymentTypeID;   // get value from paymentDetail user control 

            if (UIHelper.ParseLong(ctlPaymentDetail.ServiceTeamID) > 0)
                exp.ServiceTeam = new DbServiceTeam(UIHelper.ParseLong(ctlPaymentDetail.ServiceTeamID));   // get value from paymentDetail user control 

            long workFlowID = 0;
            if (!string.IsNullOrEmpty(Request.Params["wfid"]) && UIHelper.ParseInt(Request.Params["wfid"]) > 0)
            {
                workFlowID = UIHelper.ParseLong(Request.Params["wfid"]);
            }

            if (ctlPaymentDetail.CounterCashierID.HasValue && (ctlPaymentDetail.CounterCashierID > 0))
            {
                Dbpb pb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(ctlPaymentDetail.CounterCashierID.Value);
                exp.PB = pb;     // get value from paymentDetail user control 
                if (IsRepOffice)
                {
                    SS.Standard.WorkFlow.DTO.WorkFlow workflow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
                    exp.MainCurrencyID = pb.MainCurrencyID;
                    exp.LocalCurrencyID = UIHelper.ParseShort(ctlCurrencyDropdown.SelectedValue);
                    if (workflow == null || (workflow != null && (workflow.CurrentState.Name == WorkFlowStateFlag.Draft)))
                    {
                        if (exp.MainCurrencyID.Value == exp.LocalCurrencyID.Value)
                        {
                            exp.ExchangeRateForLocalCurrency = 1;
                        }
                        else
                        {
                            exp.ExchangeRateForLocalCurrency = ScgDbQueryProvider.DbPbRateQuery.GetExchangeRate(pb.Pbid, exp.MainCurrencyID.Value, exp.LocalCurrencyID.Value);
                        }

                        DbCurrency currencyThb = SsDbQueryProvider.DbCurrencyQuery.FindByCurrencySymbol(CurrencySymbol.THB.ToString(), false, false);
                        exp.ExchangeRateMainToTHBCurrency = ScgDbQueryProvider.DbPbRateQuery.GetExchangeRate(pb.Pbid, exp.MainCurrencyID.Value, currencyThb.CurrencyID);

                        // check expense refer advance
                        ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TransactionID);
                        ExpenseDataSet.FnExpenseAdvanceRow[] rows = (ExpenseDataSet.FnExpenseAdvanceRow[])expDS.FnExpenseAdvance.Select();
                        if (rows.Count() > 0)
                        {
                            IList<long> advanceIDList = new List<long>();
                            foreach (ExpenseDataSet.FnExpenseAdvanceRow row in rows)
                            {
                                advanceIDList.Add(row.AdvanceID);
                            }
                            exp.ExchangeRateMainToTHBCurrency = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetExchangeRateMainCurrencyToTHB(advanceIDList);
                            exp.ExchangeRateForLocalCurrency = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetExchangeRateLocalCurrencyToMainCurrency(advanceIDList);
                        }
                    }
                }
            }
            if (DocumentType.Equals(ZoneType.Domestic))
            {
                exp.ExpenseType = ZoneType.Domestic;
            }
            else if (DocumentType.Equals(ZoneType.Foreign))
            {
                exp.ExpenseType = ZoneType.Foreign;
            }
        }

        public bool NotifyUpdateExpense()
        {
            try
            {
                SaveToDataSet();

                ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TransactionID);
                ExpenseDataSet.FnExpenseDocumentRow expRow = expDS.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);
                long tempDocumentID = expRow.DocumentID;
                SCGDocument document = new SCGDocument(tempDocumentID);
                this.GetDocument(document);

                FnExpenseDocument exp = new FnExpenseDocument(expRow.ExpenseID);
                exp.Document = document;
                this.GetExpenseDocument(exp);

                FnExpenseDocumentService.UpdateExpenseDocument(this.TransactionID, exp);

                ctlUpdatePanelValidation.Update();
                return true;
            }
            catch (ServiceValidationException ex)
            {
                logger.Error(ex.ToString());
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                ctlUpdatePanelValidation.Update();
                return false;
            }
            catch (Exception ex)
            {
                logger.Error("NotifyUpdateExpense (Error) : " + ex.ToString());
                this.ValidationErrors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("WorkFlow_Error_Problem_Occurred")); //ระบบมีปัญหาในการประมวลผล กรุณาลองอีกครั้งหนึ่ง (reused from workflow)
                return false;
            }
        }

        protected void ctlViewPostButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.isNotViewPost = false;
                ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TransactionID);
                ExpenseDataSet.FnExpenseDocumentRow expRow = expDS.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);
                SCGDocumentService.ValidateVerifyDetail(this.TransactionID, ViewPostDocumentType.Expense, expRow.IsRepOffice);
                ctlViewPost.Initialize(expRow.DocumentID, DocumentKind.Expense);
                ctlViewPost.Show();
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                ctlUpdatePanelValidation.Update();
            }
        }

        protected void ctlPostRemittanceButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.isNotViewPost = false;
                #region ถ้า company เลือกใช้ special pay-in เช็คว่ากด receive แล้วหรือยัง
                DbCompany comp = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(UIHelper.ParseLong(ctlCompanyField.CompanyID));
                if (comp.UseSpecialPayIn)
                {
                    long workFlowID = UIHelper.ParseLong(Request.Params["wfid"]);
                    SCGDocumentService.ValidateRemittanceRecievedMethod(workFlowID);
                }
                #endregion
                ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TransactionID);
                ExpenseDataSet.FnExpenseDocumentRow expRow = expDS.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);
                SCGDocumentService.ValidateVerifyDetail(this.TransactionID, ViewPostDocumentType.Expense, expRow.IsRepOffice);
                ctlViewPost.Initialize(expRow.DocumentID, DocumentKind.ExpenseRemittance);
                ctlViewPost.Show();
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                ctlUpdatePanelValidation.Update();
            }
        }
        public void NotifyPaymentDetailChange()
        {
            ctlPaymentDetail.BindDifferenceAmountSummary();
            ctlPaymentDetail.UpdateMessageSummary();
            ctlUpdatePanelApprover.Update();
        }

        public void Copy(long wfid)
        {
            SS.Standard.WorkFlow.DTO.Document doc = WorkFlowQueryProvider.WorkFlowQuery.GetDocumentByWorkFlowID(wfid);

            if (doc.DocumentType.DocumentTypeID.Equals(DocumentTypeID.ExpenseDomesticDocument))
                Response.Redirect(String.Format("~/Forms/SCG.eAccounting/Programs/ExpenseFormDM.aspx?cp=1&docId={0}", doc.DocumentID));
            else if (doc.DocumentType.DocumentTypeID.Equals(DocumentTypeID.ExpenseForeignDocument))
                Response.Redirect(String.Format("~/Forms/SCG.eAccounting/Programs/ExpenseFormFR.aspx?cp=1&docId={0}", doc.DocumentID));
        }
        public void SelectedPaymentMethod()
        {
            DbCompany comp = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(UIHelper.ParseLong(ctlCompanyField.CompanyID));
            if (comp != null && ctlPaymentMethod.Items.Count > 1)
            {
                switch (ctlPaymentDetail.PaymentTypeID)
                {
                    case PaymentType.CA:
                        if (comp.PaymentMethodPetty != null)
                            ctlPaymentMethod.SelectedValue = comp.PaymentMethodPetty.PaymentMethodID.ToString();
                        break;

                    case PaymentType.TR:
                        if (comp.PaymentMethodTransfer != null)
                            ctlPaymentMethod.SelectedValue = comp.PaymentMethodTransfer.PaymentMethodID.ToString();
                        break;

                    case PaymentType.CQ:
                        if (comp.PaymentMethodCheque != null)
                            ctlPaymentMethod.SelectedValue = comp.PaymentMethodCheque.PaymentMethodID.ToString();
                        break;
                }
            }
            ctlUpdatePanelViewPost.Update();
        }

        public void DefaultSupplementary()
        {
            if (IsContainEditableFields(SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.VerifyDetail))
            {
                if (ctlPaymentDetail.PaymentTypeID == PaymentTypeConst.DomesticCheque)
                {
                    Dbpb pb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(UIHelper.ParseLong(ctlPaymentDetail.CounterCashierID.ToString()));
                    ctlSupplementary.Text = pb == null ? string.Empty : pb.Supplementary;
                }
                else
                {
                    ctlSupplementary.Text = string.Empty;
                }
            }
        }

        public void EnabledViewPostButton(bool IsLock)
        {
            ctlViewPostButton.Enabled = IsLock;
            if (isNotViewPost)
            {
                ctlUpdatePanelViewPost.Update();
            }
        }
        public void EnabledPostRemittanceButton(bool IsLock)
        {
            ctlPostRemittanceButton.Enabled = IsLock;
            if (isNotViewPost)
            {
                ctlUpdatePanelViewPost.Update();
            }
        }

        public void BindCurrencyDropdown()
        {
            if (IsRepOffice)
            {
                if (ctlPaymentDetail.CounterCashierID.HasValue && ctlPaymentDetail.CounterCashierID.Value > 0)
                {
                    ctlCurrencyDropdown.DataSource = ScgDbQueryProvider.DbPBCurrencyQuery.FindPBLocalCurrencyByPBID(ctlPaymentDetail.CounterCashierID.Value, ZoneType.Domestic);
                    ctlCurrencyDropdown.DataTextField = "Symbol";
                    ctlCurrencyDropdown.DataValueField = "CurrencyID";
                    ctlCurrencyDropdown.DataBind();
                    ctlCurrencyDropdownDiv.Style["display"] = "block";
                }
            }
            else
            {
                ctlCurrencyDropdown.Items.Clear();
                ctlCurrencyDropdownDiv.Style["display"] = "none";
            }
            ctlUpdatePanelHeader.Update();
        }

        protected void ctlCurrencyDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsRepOffice)
            {
                if (DocumentType == ZoneType.Foreign && !CanChangeFinalCurrency())
                {
                    ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TransactionID);
                    ExpenseDataSet.FnExpenseDocumentRow expRow = expDS.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);

                    ValidationErrors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Currency can be changed on a blank form only."));
                    ctlCurrencyDropdown.SelectedValue = expRow.LocalCurrencyID.ToString();
                    ctlUpdatePanelValidation.Update();
                }
                else
                {
                    SetLocalCurrency(UIHelper.ParseShort(ctlCurrencyDropdown.SelectedValue));
                    ctlExpenseGeneral.SelectedCurrency = ctlCurrencyDropdown.SelectedItem == null ? string.Empty : ctlCurrencyDropdown.SelectedItem.ToString();
                    ctlPaymentDetail.LocalCurrencyID = UIHelper.ParseShort(ctlCurrencyDropdown.SelectedValue);
                    ctlExpenseViewByAccount.SelectedCurrency = ctlCurrencyDropdown.SelectedItem == null ? string.Empty : ctlCurrencyDropdown.SelectedItem.ToString();

                    if (InitialFlag.Equals(FlagEnum.NewFlag))
                    {
                        ctlExpenseGeneral.GetCurrentSimpleExpense();
                        ctlExpenseGeneral.BindSimpleExpenseGrid(DocumentType, UIHelper.ParseLong(ctlRequesterData.UserID), true);
                    }
                    else
                    {
                        ctlExpenseGeneral.BindExpenseGrid();
                    }
                    ctlClearingAdvance.BindControl(true);
                    ctlExpenseGeneral.BindControl();
                    ctlPaymentDetail.BindControl();
                    ctlExpenseViewByAccount.BindControl(true);

                    UpdateExpenseTab.Update();
                    ctlUpdatePanelApprover.Update();
                }
            }
        }

        public void CheckRepOffice()
        {
            long expDocumentID = this.ExpDocumentID;
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TransactionID);
            ExpenseDataSet.FnExpenseDocumentRow expRow = expDS.FnExpenseDocument.FindByExpenseID(expDocumentID);

            //if (ctlCompanyField.CompanyID == ctlRequesterData.CompanyID)
            //{
            if (InitialFlag.Equals(FlagEnum.NewFlag) && ((expRow.IsIsRepOfficeNull() || !IsRepOffice)))
            {
                SuUser userList = QueryProvider.SuUserQuery.FindByIdentity(UIHelper.ParseLong(ctlRequesterData.UserID));
                if (userList != null && userList.Location != null)
                {
                    if (userList.Location.DefaultPBID.HasValue)
                    {
                        Dbpb pb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(userList.Location.DefaultPBID.Value);
                        if (pb != null)
                        {
                            IsRepOffice = expRow.IsRepOffice = pb.RepOffice;
                        }
                        else
                        {
                            IsRepOffice = expRow.IsRepOffice = false;
                        }
                    }
                    else
                    {
                        IsRepOffice = expRow.IsRepOffice = false;
                    }
                }
                else
                {
                    IsRepOffice = expRow.IsRepOffice = false;
                }
            }
            else
            {
                if (expRow.IsIsRepOfficeNull())
                {
                    expRow.IsRepOffice = false;
                }
                IsRepOffice = expRow.IsRepOffice;
            }
            //}
            //else
            //{
            //    IsRepOffice = expRow.IsRepOffice = false;
            //}

            ctlPaymentDetail.IsRepOffice = this.IsRepOffice;
            ctlExpenseGeneral.IsRepOffice = this.IsRepOffice;
            ctlClearingAdvance.IsRepOffice = this.IsRepOffice;
            ctlExpenseViewByAccount.IsRepOffice = this.IsRepOffice;
        }

        public void DefaultPb()
        {
            if (ctlCompanyField.CompanyID == ctlRequesterData.CompanyID)
            {
                SuUser requester = QueryProvider.SuUserQuery.FindByIdentity(UIHelper.ParseLong(ctlRequesterData.UserID));
                if (requester.Location != null && requester.Location.DefaultPBID.HasValue)
                {
                    if ((!string.IsNullOrEmpty(ctlPaymentDetail.PaymentTypeID) && ctlPaymentDetail.PaymentTypeID != PaymentType.TR))
                    {
                        ctlPaymentDetail.CounterCashierID = requester.Location.DefaultPBID.Value;
                        ctlPaymentDetail.TempCounterCashierID = requester.Location.DefaultPBID.Value;
                        SetMainCurrency();
                    }
                }
            }
        }

        public void GetExchangeRateForRepOffice()
        {
            Guid txID = this.TransactionID;
            long expDocumentID = this.ExpDocumentID;
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(txID);
            ExpenseDataSet.FnExpenseDocumentRow expRow = expDS.FnExpenseDocument.FindByExpenseID(expDocumentID);

            string strCurrentState = "";
            if (!string.IsNullOrEmpty(Request.Params["wfid"]) && UIHelper.ParseInt(Request.Params["wfid"]) > 0)
            {
                long workFlowID = UIHelper.ParseLong(Request.Params["wfid"]);

                try
                {
                    SS.Standard.WorkFlow.DTO.WorkFlow wf = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
                    if (wf != null && wf.CurrentState != null && wf.CurrentState.Name != null)
                        strCurrentState = wf.CurrentState.Name;
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    throw ex;
                }
            }

            if (IsRepOffice && InitialFlag.Equals(FlagEnum.EditFlag) && (strCurrentState.Equals(WorkFlowStateFlag.WaitVerify) || strCurrentState.Equals(WorkFlowStateFlag.Draft)))
            {
                ExpenseDataSet.FnExpenseAdvanceRow[] advList = (ExpenseDataSet.FnExpenseAdvanceRow[])expDS.FnExpenseAdvance.Select(string.Format("ExpenseID = '{0}'", expDocumentID));

                // get exchange rate and re-calculate invoice amount
                //-----find exchange rate (main Exchange Rate)-----
                if (!expRow.IsPBIDNull() && expRow.PBID > 0)
                {
                    if (expRow.MainCurrencyID == expRow.LocalCurrencyID)
                    {
                        expRow.ExchangeRateForLocalCurrency = 1;
                    }
                    else
                    {
                        expRow.ExchangeRateForLocalCurrency = (decimal)ScgDbQueryProvider.DbPbRateQuery.GetExchangeRate(expRow.PBID, expRow.MainCurrencyID, expRow.LocalCurrencyID);
                    }

                    if (advList.Length == 0)
                    {
                        DbCurrency currencyThb = SsDbQueryProvider.DbCurrencyQuery.FindByCurrencySymbol(CurrencySymbol.THB.ToString(), false, false);
                        expRow.ExchangeRateMainToTHBCurrency = (decimal)ScgDbQueryProvider.DbPbRateQuery.GetExchangeRate(expRow.PBID, expRow.MainCurrencyID, currencyThb.CurrencyID);
                    }
                    else
                    {
                        IList<long> advanceIDList = new List<long>();
                        foreach (ExpenseDataSet.FnExpenseAdvanceRow row in advList)
                        {
                            advanceIDList.Add(row.AdvanceID);
                        }
                        expRow.ExchangeRateMainToTHBCurrency = (decimal)ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetExchangeRateMainCurrencyToTHB(advanceIDList);

                        //if (DocumentType == ZoneType.Domestic)
                        //{
                        expRow.ExchangeRateForLocalCurrency = (decimal)ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetExchangeRateLocalCurrencyToMainCurrency(advanceIDList);
                        //}
                    }
                }
            }
        }

        public void NotifyCheckRepOffice()
        {
            long expDocumentID = this.ExpDocumentID;
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TransactionID);
            ExpenseDataSet.FnExpenseDocumentRow expRow = expDS.FnExpenseDocument.FindByExpenseID(expDocumentID);

            if (ctlPaymentDetail.PaymentTypeID == PaymentType.TR || ctlPaymentDetail.PaymentTypeID == PaymentType.CQ)
            {
                if (IsRepOffice)
                {
                    IsRepOffice = expRow.IsRepOffice = false;
                }
            }
            else
            {
                //if (ctlCompanyField.CompanyID == ctlRequesterData.CompanyID)
                //{
                if (ctlPaymentDetail.CounterCashierID.HasValue && ctlPaymentDetail.CounterCashierID.Value > 0)
                {
                    Dbpb pb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(ctlPaymentDetail.CounterCashierID.Value);
                    expRow.IsRepOffice = IsRepOffice = pb.RepOffice;
                }
                else
                {
                    expRow.IsRepOffice = IsRepOffice = false;
                }
                //}
            }

            ctlPaymentDetail.IsRepOffice = this.IsRepOffice;
            ctlExpenseGeneral.IsRepOffice = this.IsRepOffice;
            ctlClearingAdvance.IsRepOffice = this.IsRepOffice;

            if (!IsRepOffice)
            {
                ctlCurrencyDropdown.Items.Clear();
                ctlCurrencyDropdownDiv.Style["display"] = "none";
                ctlExpenseGeneral.SelectedCurrency = string.Empty;
            }

            BindCurrencyDropdown();
            if (IsRepOffice)
            {
                ctlExpenseGeneral.SelectedCurrency = ctlCurrencyDropdown.SelectedItem == null ? string.Empty : ctlCurrencyDropdown.SelectedItem.ToString();
                ctlPaymentDetail.LocalCurrencyID = UIHelper.ParseShort(ctlCurrencyDropdown.SelectedValue);

                if (UIHelper.ParseShort(ctlCurrencyDropdown.SelectedValue) > 0)
                    expRow.LocalCurrencyID = UIHelper.ParseShort(ctlCurrencyDropdown.SelectedValue);

                SetMainCurrency();
            }
            ctlExpenseGeneral.BindSimpleExpenseGrid(this.DocumentType, UIHelper.ParseLong(ctlRequesterData.UserID), true);
            ctlExpenseGeneral.BindControl();
            ctlPaymentDetail.BindControl();
        }

        public void SetLocalCurrency(short localCurrencyID)
        {
            long expDocumentID = this.ExpDocumentID;
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TransactionID);
            ExpenseDataSet.FnExpenseDocumentRow expRow = expDS.FnExpenseDocument.FindByExpenseID(expDocumentID);

            expRow.LocalCurrencyID = localCurrencyID;
        }

        public void SetMainCurrency()
        {
            long expDocumentID = this.ExpDocumentID;
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TransactionID);
            ExpenseDataSet.FnExpenseDocumentRow expRow = expDS.FnExpenseDocument.FindByExpenseID(expDocumentID);

            if (ctlPaymentDetail.CounterCashierID.HasValue)
            {
                Dbpb pb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(ctlPaymentDetail.CounterCashierID.Value);
                if (pb != null && pb.MainCurrencyID.HasValue && pb.MainCurrencyID.Value > 0)
                    expRow.MainCurrencyID = pb.MainCurrencyID.Value;
            }
        }

        //public void DefaultFinalCurrency()
        //{
        //    if (this.DocumentType.Equals(ZoneType.Foreign) && IsRepOffice)
        //    {
        //        long expDocumentID = this.ExpDocumentID;
        //        ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TransactionID);
        //        ExpenseDataSet.FnExpenseDocumentRow expRow = expDS.FnExpenseDocument.FindByExpenseID(expDocumentID);

        //        if (!expRow.IsMainCurrencyIDNull())
        //        {
        //            ctlCurrencyDropdown.SelectedValue = expRow.MainCurrencyID.ToString();
        //            SetLocalCurrency(expRow.MainCurrencyID);
        //        }
        //    }
        //}

        public void CheckRequesterAndReceiverIsSamePerson()
        {
            if (ctlRequesterData.UserID != ctlReceiverData.UserID)
            {
                string script = @"<script  type='text/javascript' language='javascript'>
                alert('" + GetMessage("RequesterAndReceiverShouldBeTheSamePerson") + "');</script>";
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", script, false);
            }
        }

        public bool RequireDocumentAttachment()
        {
            try
            {
                bool requireAttachment = false;

                DbCompany comp = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(UIHelper.ParseLong(ctlCompanyField.CompanyID));
                if (comp != null)
                {
                    requireAttachment = comp.ExpenseRequireAttachment;
                }

                if (IsRepOffice || requireAttachment)
                {
                    DocumentAttachmentService.ValidateDocumentAttachment(this.TransactionID, this.ExpDocumentID);
                }
            }
            catch (ServiceValidationException ex)
            {
                ShowWarningRequireAttachmentPopup(GetMessage("AttachmentIsRequired"));
                return true;
            }

            return false;
        }

        #region RequestDataFixedAdvanceOutstanding(int startRow, int pageSize, string sortExpression)
        public Object RequestDataFixedAdvanceOutstanding(int startRow, int pageSize, string sortExpression)
        {
            long currentWorkFlowID = UIHelper.ParseLong(Request.Params["wfid"]);
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TransactionID);
            ExpenseDataSet.FnExpenseDocumentRow expRow = expDS.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);
            FnExpenseDocument expDoc = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(expRow.DocumentID);
 
            if (expDoc != null)
            {
                IList<FixedAdvanceOutstanding> aa = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.FindFixedAdvanceOutstandingRequestForExpense(UIHelper.ParseLong(ctlRequesterData.UserID), UIHelper.ParseLong(ctlCompanyField.CompanyID), expDoc.ExpenseID, UserAccount.CurrentLanguageID, startRow, pageSize, sortExpression);
                //SS.Standard.WorkFlow.DTO.WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(expRow.DocumentID);
                //if (expDoc.FixedAdvanceDocument != null && InitialFlag.Equals(FlagEnum.ViewFlag))
                if (InitialFlag.Equals(FlagEnum.ViewFlag))
                {
                    if (expDoc.FixedAdvanceDocument == null) return new List<FixedAdvanceOutstanding>();
                    IList<FixedAdvanceOutstanding> bb = aa.Where(t => t.fixedAdvanceDocumentID == expDoc.FixedAdvanceDocument.FixedAdvanceID).ToList();
                    return bb;
                }
                else
                {
                    return aa;
                }
            }
            else
            {
                IList<FixedAdvanceOutstanding> aa = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.FindFixedAdvanceOutstandingRequestForExpense(UIHelper.ParseLong(ctlRequesterData.UserID), UIHelper.ParseLong(ctlCompanyField.CompanyID), currentWorkFlowID, UserAccount.CurrentLanguageID, startRow, pageSize, sortExpression);
                return aa;
            }
        }
        #endregion

        protected void FixedAdvanceOutstandingGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox ctlFixedAdvanceSelect = e.Row.FindControl("ctlFixedAdvanceSelect") as CheckBox;
                FixedAdvanceOutstanding fixRow = e.Row.DataItem as FixedAdvanceOutstanding;
                ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TransactionID);
                ExpenseDataSet.FnExpenseDocumentRow expRow = expDS.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);
                FnExpenseDocument expDoc = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(expRow.DocumentID);
                if (expDoc != null)
                {
                    if (expDoc.FixedAdvanceDocument != null)
                    {
                        if (fixRow.fixedAdvanceDocumentID == expDoc.FixedAdvanceDocument.FixedAdvanceID)
                            ctlFixedAdvanceSelect.Checked = true;
                    }
                }
            }
        }

        #region RequestCountFixedAdvanceOutstanding()
        public int RequestCountFixedAdvanceOutstanding()
        {
            //long currentWorkFlowID = UIHelper.ParseLong(Request.Params["wfid"]);
            //var aa = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.CountFindFixedAdvanceOutstandingRequestForExpense(UIHelper.ParseLong(ctlRequesterData.UserID), UIHelper.ParseLong(ctlCompanyField.CompanyID), currentWorkFlowID, UserAccount.CurrentLanguageID);

            //ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TransactionID);
            //ExpenseDataSet.FnExpenseDocumentRow expRow = expDS.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);
            //FnExpenseDocument expDoc = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(expRow.DocumentID);
            //if (expDoc != null)
            //{
            //    if (InitialFlag.Equals(FlagEnum.ViewFlag))
            //    {
            //        if (expDoc.FixedAdvanceDocument == null) return 0;
            //        return aa;
            //    }
            //    else
            //    {
            //        return aa;
            //    }
            //}
            //else
            //{
            //    return aa;
            //}
            long currentWorkFlowID = UIHelper.ParseLong(Request.Params["wfid"]);
            //return ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.CountFindFixedAdvanceOutstandingRequest(UIHelper.ParseLong(ctlRequesterData.UserID), UIHelper.ParseLong(ctlCompanyField.CompanyID), currentWorkFlowID, UserAccount.CurrentLanguageID);
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TransactionID);
            ExpenseDataSet.FnExpenseDocumentRow expRow = expDS.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);
            FnExpenseDocument expDoc = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(expRow.DocumentID);
            if (expDoc != null)
            {
                SS.Standard.WorkFlow.DTO.WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(expRow.DocumentID);
                if (expDoc.FixedAdvanceDocument != null && InitialFlag.Equals(FlagEnum.ViewFlag))
                {
                    return 1;
                }
                else
                {
                    return ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.CountFindFixedAdvanceOutstandingRequestForExpense(UIHelper.ParseLong(ctlRequesterData.UserID), UIHelper.ParseLong(ctlCompanyField.CompanyID), currentWorkFlowID, UserAccount.CurrentLanguageID);
                    //return ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.CountFindFixedAdvanceOutstandingRequest(UIHelper.ParseLong(ctlRequesterData.UserID), UIHelper.ParseLong(ctlCompanyField.CompanyID), currentWorkFlowID, UserAccount.CurrentLanguageID);
                }
            }
            else
            {
                return ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.CountFindFixedAdvanceOutstandingRequestForExpense(UIHelper.ParseLong(ctlRequesterData.UserID), UIHelper.ParseLong(ctlCompanyField.CompanyID), currentWorkFlowID, UserAccount.CurrentLanguageID);
                //return ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.CountFindFixedAdvanceOutstandingRequest(UIHelper.ParseLong(ctlRequesterData.UserID), UIHelper.ParseLong(ctlCompanyField.CompanyID), currentWorkFlowID, UserAccount.CurrentLanguageID);
            }
        }
        #endregion

        protected void FixedAdvanceGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Select"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                long FixedAdvanceDocumentID = UIHelper.ParseLong(ctlFixedAdvanceGrid.DataKeys[rowIndex].Value.ToString());
                //CallOnObjectLookUpReturn(MPADocumentID.ToString());
                //Hide();
            }
        }

        protected void ctlFixedAdvanceGrid_DataBound(object sender, EventArgs e)
        {
            if (ctlFixedAdvanceGrid.Rows.Count > 0)
            {
                RegisterScriptForGridView();
            }
           
        }

        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBox(objChk) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckFixedAdvance(objChk, ");
            script.Append("'" + ctlFixedAdvanceGrid.ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "validateChkBox", script.ToString(), true);
        }

        public void CheckShowHideFixedAdvance(string flag)
        {
            long currentWorkFlowID = UIHelper.ParseLong(Request.Params["wfid"]);
            string wfstate = string.Empty;
            SS.Standard.WorkFlow.DTO.WorkFlow wf = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(currentWorkFlowID);
            if (wf != null && wf.CurrentState != null && wf.CurrentState.Name != null)
            {
                wfstate = wf.CurrentState.Name;
            }
            int CheckFixedAdvance = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.CountFindFixedAdvanceOutstandingRequestForExpense(UIHelper.ParseLong(ctlRequesterData.UserID), UIHelper.ParseLong(ctlCompanyField.CompanyID), currentWorkFlowID, UserAccount.CurrentLanguageID);
            if (!string.IsNullOrEmpty(ctlCompanyField.CompanyID) && (flag.Equals(FlagEnum.NewFlag) || (flag.Equals(FlagEnum.EditFlag) && wfstate.Equals(WorkFlowStateFlag.Draft))))
            {
                if (CheckFixedAdvance > 0)
                {
                    if (this.DocumentType.Equals(ZoneType.Domestic))
                    {
                        ctlFixedAdvance.Visible = true; AleartMessageFixedAdvance.Visible = true;
                    }
                    else
                    {
                        ctlFixedAdvance.Visible = false; AleartMessageFixedAdvance.Visible = false;
                    }
                }
                else
                {
                    ctlFixedAdvance.Visible = false;
                    AleartMessageFixedAdvance.Visible = false;
                }
            }
            else if (flag.Equals(FlagEnum.ViewFlag))
            {
                ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TransactionID);
                ExpenseDataSet.FnExpenseDocumentRow expRow = expDS.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);
                FnExpenseDocument expDoc = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(expRow.DocumentID);
                if (expDoc.FixedAdvanceDocument != null)
                {
                    ctlFixedAdvance.Visible = true;
                }
                else
                {
                    ctlFixedAdvance.Visible = false;
                }
            }
            else
            {
                ctlFixedAdvance.Visible = false;
                AleartMessageFixedAdvance.Visible = false;
            }

            UpdateExpenseTab.Update();
        }

    }
}

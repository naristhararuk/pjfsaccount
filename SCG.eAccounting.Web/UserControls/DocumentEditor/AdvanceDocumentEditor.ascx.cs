using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using SS.Standard.Security;
using SS.Standard.UI;
using SS.Standard.Utilities;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.Query;

using SCG.eAccounting.BLL;
using SCG.eAccounting.BLL.Implement;
using SCG.eAccounting.DAL;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.DataSet;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.Query;
using SCG.eAccounting.Web.Helper;

using SCG.eAccounting.SAP.BAPI.Service.Const;

using SCG.DB.DTO;
using SCG.DB.Query;

using SS.DB.DTO;
using SS.DB.Query;
using SS.Standard.WorkFlow.Service;
using SS.SU.DTO;
using SS.SU.Query;
using SCG.eAccounting.Query.Hibernate;
using log4net;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor
{
    public partial class AdvanceDocumentEditor : BaseUserControl, IDocumentEditor
    {
        private static ILog logger = log4net.LogManager.GetLogger(typeof(AdvanceDocumentEditor));

        #region local variable

        public IAvAdvanceDocumentService AvAdvanceDocumentService { get; set; }
        public ITransactionService TransactionService { get; set; }
        public IAvAdvanceItemService AvAdvanceItemService { get; set; }
        public IDocumentInitiatorService DocumentInitiatorService { get; set; }
        public IWorkFlowService WorkFlowService { get; set; }
        public ISCGDocumentService SCGDocumentService { get; set; }
        private string documentTypeDMT = "PaymentTypeDMT";
        private string documentTypeFRN = "PaymentTypeFRN";
        private int dueDateOfRemit = ParameterServices.DueDateOfRemittance;
        private int requestDateOfRemit = ParameterServices.RequestDateOfRemittance;
        private int currencyUsd = ParameterServices.USDCurrencyID;
        public double totalPerdiemRateUSD = 0;
        public double totalAmountTHB = 0;
        public double totalAmountMainCurrency = 0;
        public double totalAmountUSD = 0;
        public double totalAmountThbUSD = 0;
        #endregion

        #region Property
        public string HeaderForm
        {
            set { ctlAdvanceFormHeader.HeaderForm = value; }
        }
        public Guid TransactionID
        {
            get { return (Guid)ViewState[ViewStateName.TransactionID]; }
            set { ViewState[ViewStateName.TransactionID] = value; }
        }
        public long DocumentID
        {
            get { return UIHelper.ParseLong(ViewState[ViewStateName.DocumentID].ToString()); }
            set { ViewState[ViewStateName.DocumentID] = value; }
        }
        public string AdvanceType
        {
            get { return ctlAdvanceType.Text; }
            set { ctlAdvanceType.Text = value; }
        }

        public string InitialFlag
        {
            get { return ViewState["InitialFlag"].ToString(); }
            set { ViewState["InitialFlag"] = value; }
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
        public bool isShowFooter
        {
            get
            {
                if (ViewState["isShowFooter"] == null)
                    return true;
                else
                    return (bool)(ViewState["isShowFooter"]);
            }
            set
            {
                ViewState["isShowFooter"] = value;
            }
        }
        public long DocumentIDofTA
        {
            get
            {
                if (ViewState["DocumentIDofTA"] != null)
                    return (long)(ViewState["DocumentIDofTA"]);
                else
                    return 0;
            }
            set
            {
                ViewState["DocumentIDofTA"] = value;
            }
        }
        public long? TADocumentIDView { get; set; }
        public long TaRequesterID
        {
            get
            {
                if (ViewState["TaRequesterID"] != null)
                    return (long)(ViewState["TaRequesterID"]);
                else
                    return 0;
            }
            set { ViewState["TaRequesterID"] = value; }
        }
        public long AvDocumentIDInGrid
        {
            get
            {
                if (ViewState["AvDocumentIDInGrid"] != null)
                    return (long)(ViewState["AvDocumentIDInGrid"]);
                else
                    return 0;
            }
            set { ViewState["AvDocumentIDInGrid"] = value; }
        }
        public long ExpenseDocumentIDInGrid
        {
            get
            {
                if (ViewState["ExpenseDocumentIDInGrid"] != null)
                    return (long)(ViewState["ExpenseDocumentIDInGrid"]);
                else
                    return 0;
            }
            set { ViewState["ExpenseDocumentIDInGrid"] = value; }
        }
        public bool isUpdatePanel
        {
            get
            {
                if (ViewState["isUpdatePanel"] == null)
                    return true;
                else
                    return (bool)(ViewState["isUpdatePanel"]);
            }
            set
            {
                ViewState["isUpdatePanel"] = value;
            }
        }
        public string CurrentStateName
        {
            get
            {
                if (ViewState["currentState"] != null)
                    return ViewState["currentState"].ToString();
                else
                    return string.Empty;
            }
            set { ViewState["currentState"] = value; }
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
        public bool IsRepOffice
        {
            get
            {
                if (ViewState["IsRepOffice"] == null)
                    return false;
                else
                {
                    return (bool)(ViewState["IsRepOffice"]);
                }
            }
            set { ViewState["IsRepOffice"] = value; }
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

                ctlAdvanceTabContainer.ActiveTab = ctlTabGeneral;
                BindTabGeneral();
                BindTabInitial();
                BindTabAttachment();
                BindTabMemo();
                if (Request.Params["wfid"] == null)
                {
                    ctlTANo.Text = string.Empty;
                    ctlNATaNo.Visible = true;
                    ctlNATaNo.Text = "N/A";
                }
                ctlOutstandingGrid.DataCountAndBind();
                ctlUpdatePanelOutstanding.Update();
                //if (this.TADocumentIDView != null && Request.QueryString["requesterID"] != null)
                //{
                //    this.LoadDataFromTA(this.TADocumentIDView.Value, UIHelper.ParseLong(Request.Params["requesterID"].ToString()));
                //}
                hideCounterCashier();
            }


            ctlTALookup.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlTALookup_OnObjectLookUpReturn);
            ctlTALookup.OnObjectLookUpCalling += new BaseUserControl.ObjectLookUpCallingEventHandler(ctlTALookup_OnObjectLookUpCalling);
            ctlCompanyField.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlCompanyField_OnObjectLookUpReturn);
            ctlRequesterData.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlRequesterData_OnObjectLookUpReturn);
            ctlReceiverData.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlReceiverData_OnObjectLookUpReturn);
            ctlRequestDateOfAdvance.NotifyDateChanged += new CalendarOfDueDate.NotifyDateChangedHandler(ctlRequestDateOfAdvance_NotifyDateChanged);
            ctlArrivalDate.NotifyDateChanged += new CalendarOfDueDate.NotifyDateChangedHandler(ctlArrivalDate_NotifyDateChanged);
        }

        void OnDsNull()
        {
            if (dsNullHandler != null)
                dsNullHandler();
        }

        void ctlArrivalDate_NotifyDateChanged(object sender, string returnValue)
        {
            if (!string.IsNullOrEmpty(returnValue))
            {
                ctlDueDateOfRemittance.Text = returnValue;
                ctlRequestDateOfRemittance.DateValue = ctlDueDateOfRemittance.Text;
            }
        }

        protected void ctlRequestDateOfAdvance_NotifyDateChanged(object sender, string returnValue)
        {
            if (!string.IsNullOrEmpty(returnValue))
            {
                ctlDueDateOfRemittance.Text = returnValue;
                // หา  current state ของเอกสาร
                if (!string.IsNullOrEmpty(Request.Params["wfid"]) && UIHelper.ParseInt(Request.Params["wfid"]) > 0)
                {
                    long workFlowID = UIHelper.ParseLong(Request.Params["wfid"]);

                    SS.Standard.WorkFlow.DTO.WorkFlow wf = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
                    if (wf != null && wf.CurrentState != null && !string.IsNullOrEmpty(wf.CurrentState.Name))
                    {
                        if (wf.CurrentState.Name.Equals(WorkFlowStateFlag.WaitVerify) || wf.CurrentState.Name.Equals(WorkFlowStateFlag.Draft))
                        {
                            //RemittanceDate = date of advance + x day (may be 30 day)
                            DateTime RemittanceDate = UIHelper.ParseDate(ctlRequestDateOfAdvance.DateValue).Value.AddDays(requestDateOfRemit);

                            DateTime dueDateOfRemitted = UIHelper.ParseDate(ctlDueDateOfRemittance.Text).Value;
                            DateTime requestdateOfRemitted = UIHelper.ParseDate(ctlRequestDateOfRemittance.DateValue).Value;
                            if (requestdateOfRemitted.CompareTo(dueDateOfRemitted) < 0 || requestdateOfRemitted.CompareTo(RemittanceDate) > 0)
                            {
                                ctlRequestDateOfRemittance.Value = UIHelper.ParseDate(ctlDueDateOfRemittance.Text);
                            }
                        }
                    }
                }
                else
                {
                    ctlRequestDateOfRemittance.Value = UIHelper.ParseDate(ctlDueDateOfRemittance.Text);
                }
            }
        }

        #endregion

        #region Initialize(string initFlag, long? documentID)
        public void Initialize(string initFlag, long? documentID)
        {
            long advanceDocumentID = 0;
            AdvanceDataSet advanceDataset;
            Guid txID = Guid.Empty;
            bool isCopy = false;

            // หา  current state ของเอกสาร
            //string strCurrentState = "";
            if (!string.IsNullOrEmpty(Request.Params["wfid"]) && UIHelper.ParseInt(Request.Params["wfid"]) > 0)
            {
                long workFlowID = UIHelper.ParseLong(Request.Params["wfid"]);

                SS.Standard.WorkFlow.DTO.WorkFlow wf = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
                if (wf != null && wf.CurrentState != null && wf.CurrentState.Name != null)
                    CurrentStateName = wf.CurrentState.Name;
            }

            //ถ้าเป็น document status ตั้งแต่ Verify ขึ้นไป สามารถแก้ AmountTHB ได้
            ctlAdvanceIntGrid.Columns[6].Visible = true;
            this.isShowFooter = true;

            if (initFlag.Equals(FlagEnum.NewFlag) && Request.QueryString["cp"] == null)
            {
                //ถ้าเป็น document ที่สร้างจาก TA ให้เอาค่า TADocumentID ไป prepare dataset
                if (Request.QueryString["taDocumentID"] != null && Request.QueryString["requesterID"] != null)
                    advanceDataset = (AdvanceDataSet)AvAdvanceDocumentService.PrepareDataToDsTA(UIHelper.ParseLong(Request.QueryString["taDocumentID"].ToString()));
                else
                    advanceDataset = (AdvanceDataSet)AvAdvanceDocumentService.PrepareDS();

                txID = TransactionService.Begin(advanceDataset);
                if (Request.QueryString["taDocumentID"] != null && Request.QueryString["requesterID"] != null)
                {
                    advanceDocumentID = UIHelper.ParseLong(advanceDataset.AvAdvanceDocument.Rows[0]["AdvanceID"].ToString());
                    ctlTAIdLookup.Text = Request.QueryString["taDocumentID"].ToString();
                }
                else
                {
                    if (Request.Params["wfid"] == null && AdvanceType.Equals(AdvanceTypeEnum.Domestic))
                    {
                        advanceDocumentID = AvAdvanceDocumentService.AddAdvanceDocument(txID, ZoneType.Domestic);
                    }
                    else
                    {
                        advanceDocumentID = AvAdvanceDocumentService.AddAdvanceDocument(txID, ZoneType.Foreign);

                    }
                }

                ctlUpdatePanelHeader.Update();
            }
            else if (((initFlag.Equals(FlagEnum.EditFlag) || (initFlag.Equals(FlagEnum.ViewFlag)))) && ((documentID.HasValue) && (documentID.Value != 0)))
            {
                advanceDataset = (AdvanceDataSet)AvAdvanceDocumentService.PrepareDS(documentID.Value);
                txID = TransactionService.Begin(advanceDataset);
                AdvanceDataSet advanceDS = (AdvanceDataSet)advanceDataset;

                if (advanceDS.AvAdvanceDocument.Rows.Count > 0)
                {
                    AdvanceType = advanceDS.AvAdvanceDocument.Rows[0]["AdvanceType"].ToString();
                    advanceDocumentID = UIHelper.ParseLong(advanceDS.AvAdvanceDocument.Rows[0]["AdvanceID"].ToString());
                    if (!string.IsNullOrEmpty(advanceDS.AvAdvanceDocument.Rows[0]["IsRepOffice"].ToString()))
                    {
                        IsRepOffice = (bool)advanceDS.AvAdvanceDocument.Rows[0]["IsRepOffice"];
                    }
                    else
                    {
                        IsRepOffice = false;
                    }
                }
                if (initFlag.Equals(FlagEnum.ViewFlag) || CurrentStateName.Equals(WorkFlowStateFlag.WaitPayment) || CurrentStateName.Equals(WorkFlowStateFlag.WaitVerify) || CurrentStateName.Equals(WorkFlowStateFlag.Hold))
                {
                    isShowFooter = false;
                    ctlAdvanceIntGrid.Columns[6].Visible = false;
                }
            }
            else if (Request.QueryString["cp"] != null && UIHelper.ParseLong(Request.QueryString["cp"]) == 1)
            {
                advanceDataset = (AdvanceDataSet)AvAdvanceDocumentService.PrepareInternalDataToDataset(UIHelper.ParseLong(Request.QueryString["docId"]), true);
                txID = TransactionService.Begin(advanceDataset);
                isCopy = true;
                if (advanceDataset.AvAdvanceDocument.Rows.Count > 0)
                {
                    advanceDocumentID = UIHelper.ParseLong(advanceDataset.AvAdvanceDocument.Rows[0]["AdvanceID"].ToString());
                    AdvanceType = advanceDataset.AvAdvanceDocument.Rows[0]["AdvanceType"].ToString();
                    if (AdvanceType.Equals(ZoneType.Domestic))
                    {
                        ctlAdvanceType.Text = AdvanceTypeEnum.Domestic;
                    }
                    else if (AdvanceType.Equals(ZoneType.Foreign))
                        ctlAdvanceType.Text = AdvanceTypeEnum.International;
                }
            }
            this.TransactionID = txID;
            this.DocumentID = advanceDocumentID;
            this.InitialFlag = initFlag;

            if (Request.QueryString["taDocumentID"] == null)
            {
                this.TADocumentIDView = null;
            }
            else
            {
                this.TADocumentIDView = UIHelper.ParseLong(Request.QueryString["taDocumentID"].ToString());
                this.TaRequesterID = UIHelper.ParseLong(Request.QueryString["requesterID"].ToString());
            }
            this.VisibleFields = AvAdvanceDocumentService.GetVisibleFields(documentID);
            this.EditableFields = AvAdvanceDocumentService.GetEditableFields(documentID, this.TADocumentIDView);


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

                if (CurrentStateName.Equals(WorkFlowStateFlag.Draft))
                {
                    ctlTabMemo.Visible = true;
                    ctlTabInitial.Visible = true;
                    ctlTabAttachment.Visible = true;
                    ctlTabHistory.Visible = true;
                    ctlTabOutstanding.Visible = true;

                    UpdatePanelAdvanceTab.Update();
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

                    if (RequestCountOutstanding() <= 0)
                        ctlTabOutstanding.Visible = false;
                    else
                        ctlTabOutstanding.Visible = true;

                    UpdatePanelAdvanceTab.Update();
                }
                else
                {
                    ctlTabMemo.Visible = true;
                    ctlTabInitial.Visible = true;
                    ctlTabAttachment.Visible = true;
                    ctlTabHistory.Visible = true;
                    ctlTabOutstanding.Visible = true;

                    UpdatePanelAdvanceTab.Update();
                }
            #endregion แสดง Tab
            }
        }
        #endregion

        #region LoadDataFromTA()
        public void LoadDataFromTA()
        {

            //ctlTAIdLookup.Text = taDocumentID.ToString();
            //TADocument taDocument = ScgeAccountingQueryProvider.TADocumentQuery.FindByIdentity(UIHelper.ParseLong(ctlTAIdLookup.Text));
            //if (taDocument != null)
            //{
            //    if (!string.IsNullOrEmpty(taDocument.DocumentID.DocumentNo))
            //    {
            //        ctlTANo.Text = taDocument.DocumentID.DocumentNo;
            //    }
            //    else
            //    {
            //        ctlTANo.Text = "N/A";
            //    }
            //    ctlApproverData.SetValue(taDocument.DocumentID.ApproverID.Userid);
            //    if (taDocument.TravelBy == TravellBy.Foreign)
            //    {
            //        ctlArrivalDate.DateValue = UIHelper.ToDateString(taDocument.ToDate);
            //        ctlDueDateOfRemittance.Text = UIHelper.ToDateString(UIHelper.ParseDate(ctlArrivalDate.DateValue).Value.AddDays(dueDateOfRemit));
            //        ctlRequestDateOfRemittance.DateValue = ctlDueDateOfRemittance.Text;

            //    }
            //}
            //ctlRequesterData.SetValue(requesterID);
            //ctlReceiverData.SetValue(requesterID);
            //ctlCreatorData.SetValue(requesterID);
            //ctlUpdatePanelHeader.Update();
        }
        #endregion

        #region Save To Transaction and DataBase
        #region isDuplicateFromTA
        /// <summary>
        /// for check duplicate advance data from ta
        /// </summary>
        /// <param name="taDocumentID"></param>
        /// <param name="requesterID"></param>
        /// <returns></returns>
        private bool isDuplicateFromTA()
        {
            bool isdupTA = false;
            if (Request.QueryString["taDocumentID"] != null)
            {
                this.TADocumentIDView = UIHelper.ParseLong(Request.QueryString["taDocumentID"].ToString());
                if (this.TADocumentIDView != null && this.TADocumentIDView > 0 && this.TaRequesterID != null && this.TaRequesterID > 0)
                {
                    IList<VoAdvanceFromTA> list = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindAdvanceFromTA(this.TADocumentIDView.Value);
                    if (list != null && list.Count > 0)
                    {
                        var checkDup = from advanceList in list
                                       where advanceList.RequesterID == this.TaRequesterID
                                       select advanceList;
                        if (checkDup.Count<VoAdvanceFromTA>() > 0)
                        {
                            isdupTA = true;
                        }
                    }
                }
            }
            return isdupTA;
        }
        #endregion

        #region Save()
        public long Save()
        {
            if (TransactionService.GetDS(this.TransactionID) == null)
            {
                OnDsNull();
            }
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (!isDuplicateFromTA())
            {
                long tempAvDocumentID = this.DocumentID;
                AdvanceDataSet avDocTemp = (AdvanceDataSet)TransactionService.GetDS(this.TransactionID);

                AdvanceDataSet.AvAdvanceDocumentRow docRow = avDocTemp.AvAdvanceDocument.FindByAdvanceID(tempAvDocumentID);
                long tempDocumentID = docRow.DocumentID;

                #region save document
                SCGDocument document = new SCGDocument(tempDocumentID);

                if (!string.IsNullOrEmpty(ctlCompanyField.CompanyID))
                    document.CompanyID = new DbCompany(UIHelper.ParseLong(ctlCompanyField.CompanyID));

                if (!string.IsNullOrEmpty(ctlRequesterData.UserID))
                    document.RequesterID = new SS.SU.DTO.SuUser(UIHelper.ParseLong(ctlRequesterData.UserID));

                if (!string.IsNullOrEmpty(ctlCreatorData.UserID))
                    document.CreatorID = new SS.SU.DTO.SuUser(UIHelper.ParseLong(ctlCreatorData.UserID));

                if (!string.IsNullOrEmpty(ctlReceiverData.UserID))
                    document.ReceiverID = new SS.SU.DTO.SuUser(UIHelper.ParseLong(ctlReceiverData.UserID));

                if (ctlAdvanceType.Text == AdvanceTypeEnum.International)
                    document.DocumentType = new DocumentType(DocumentTypeID.AdvanceForeignDocument);
                else
                    document.DocumentType = new DocumentType(DocumentTypeID.AdvanceDomesticDocument);

                if (!string.IsNullOrEmpty(ctlApproverData.UserID))
                    document.ApproverID = new SS.SU.DTO.SuUser(UIHelper.ParseLong(ctlApproverData.UserID));

                document.Subject = ctlSubject.Text;
                document.Memo = ctlMemo.Text;

                if (ctlAdvanceType.Text == AdvanceTypeEnum.Domestic)
                {
                    if (!string.IsNullOrEmpty(ctlBranchDomestic.Text.Trim()))
                        document.BranchCode = ctlBranchDomestic.Text;
                    else
                        document.BranchCode = "0001";

                    if (!string.IsNullOrEmpty(ctlPaymentMethodDomestic.SelectedValue))
                        document.PaymentMethodID = UIHelper.ParseLong(ctlPaymentMethodDomestic.SelectedValue);
                    else
                    {
                        //IList<SCG.DB.DTO.ValueObject.CompanyPaymentMethodResult> paymentList = new List<SCG.DB.DTO.ValueObject.CompanyPaymentMethodResult>();
                        //paymentList = ScgDbQueryProvider.DbCompanyPaymentMethodQuery.GetPaymentMethod(UIHelper.ParseLong(ctlCompanyField.CompanyID));

                        DbCompany comp = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(UIHelper.ParseLong(ctlCompanyField.CompanyID));
                        if (comp != null)
                        {
                            switch (ctlPaymentType.SelectedValue)
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
                        //ctlPaymentMethodDomestic.SelectedIndex = 1;
                        //document.PaymentMethodID = UIHelper.ParseLong(ctlPaymentMethodDomestic.SelectedValue);

                    }
                    document.BusinessArea = ctlBusinessAreaDomestic.Text;
                    document.Supplementary = ctlSupplementaryDomestic.Text;
                    try
                    {
                        if (!string.IsNullOrEmpty(ctlPostingDateCalendarDomestic.DateValue))
                            document.PostingDate = UIHelper.ParseDate(ctlPostingDateCalendarDomestic.DateValue);
                        else
                            document.PostingDate = DateTime.Today;

                        document.BaseLineDate = UIHelper.ParseDate(ctlRequestDateOfAdvance.Text);
                    }
                    catch (FormatException fex)
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Invalid_Date_Format"));
                        throw new ServiceValidationException(errors);
                    }

                }
                else
                {
                    if (!string.IsNullOrEmpty(ctlBranchForeign.Text.Trim()))
                        document.BranchCode = ctlBranchForeign.Text;
                    else
                        document.BranchCode = "0001";
                    if (!string.IsNullOrEmpty(ctlBankAccountForeign.Text))
                        document.BankAccount = ctlBankAccountForeign.Text;
                    else
                    {
                        if (!IsRepOffice)
                        {
                            document.BankAccount = ParameterServices.ADF_Default_BankAccount;
                        }
                        //else
                        //{
                        //    document.BankAccount = "222290";
                        //}
                    }
                    try
                    {
                        if (!string.IsNullOrEmpty(ctlPostingDateForeign.DateValue))
                            document.PostingDate = UIHelper.ParseDate(ctlPostingDateForeign.DateValue);
                        else
                            document.PostingDate = DateTime.Today;


                        document.BaseLineDate = UIHelper.ParseDate(ctlRequestDateOfAdvanceForeign.Text);
                    }
                    catch (FormatException ex)
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Invalid_Date_Format"));
                        throw new ServiceValidationException(errors);
                    }
                }
                document.Active = true;
                if (!string.IsNullOrEmpty(ctlBusinessAreaForeign.Text))
                {
                    document.BusinessArea = ctlBusinessAreaForeign.Text;
                }
                else
                {
                    document.BusinessArea = null;
                }
                try
                {
                    // Save SCG Document to Transaction.
                    SCGDocumentService.UpdateTransactionDocument(this.TransactionID, document, true, true);
                }
                catch (ServiceValidationException ex)
                {
                    errors.MergeErrors(ex.ValidationErrors);
                }
                #endregion

                #region AvAdvanceDocument
                AvAdvanceDocument avDoc = new AvAdvanceDocument(docRow.AdvanceID);
                avDoc.DocumentID = document;
                if (ctlAdvanceType.Text == AdvanceTypeEnum.International)
                {
                    avDoc.PBID = new Dbpb(UIHelper.ParseLong(ctlCounterCashierInter.SelectedValue));
                    Dbpb dbPb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(UIHelper.ParseLong(ctlCounterCashierInter.SelectedValue));
                    if (dbPb != null && dbPb.MainCurrencyID.HasValue)
                    {
                        avDoc.MainCurrencyID = dbPb.MainCurrencyID;
                    }
                    avDoc.AdvanceType = ZoneType.Foreign;
                    try
                    {
                        if (!string.IsNullOrEmpty(ctlArrivalDate.DateValue))
                        {
                            DateTime dArrivalDate = UIHelper.ParseDate(ctlArrivalDate.DateValue).Value;



                            DateTime? dRequestDate = UIHelper.ParseDate(ctlRequestDateOfAdvanceForeign.DateValue);
                            if (!dRequestDate.HasValue)
                            {
                                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RequestDateofAdvanceIsRequired"));
                                throw new ServiceValidationException(errors);
                            }
                            if (dArrivalDate.CompareTo(dRequestDate) > 0)
                            {
                                avDoc.ArrivalDate = UIHelper.ParseDate(ctlArrivalDate.DateValue);
                            }
                            else
                            {
                                avDoc.ArrivalDate = DateTime.MinValue;
                            }
                        }

                        if (avDoc.ArrivalDate.HasValue)
                            avDoc.DueDateOfRemittance = avDoc.ArrivalDate.Value.AddDays(dueDateOfRemit);
                        avDoc.RequestDateOfRemittance = ctlRequestDateOfRemittance.Value ?? DateTime.Now;
                        avDoc.RequestDateOfAdvance = ctlRequestDateOfAdvanceForeign.Value.HasValue ? ctlRequestDateOfAdvanceForeign.Value.Value : DateTime.MinValue;
                        avDoc.Amount = UIHelper.ParseDouble(ctlTotalAmountTHBLabel.Text);
                        avDoc.MainCurrencyAmount = UIHelper.ParseDouble(ctlTotalAmountMainLabel.Text);

                    }
                    catch (FormatException fex)
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Invalid_Date_Format"));
                        throw new ServiceValidationException(errors);
                    }

                    avDoc.PerDiemExRateUSD = UIHelper.ParseDouble(ctlExchangeRateForPerDiem.Text);

                    if (!string.IsNullOrEmpty(ctlExchangeRateForeign.Text) && UIHelper.ParseDouble(ctlExchangeRateForeign.Text) > 0)
                    {
                        avDoc.ExchangeRateMainToTHBCurrency = UIHelper.ParseDouble(ctlExchangeRateForeign.Text);
                    }

                    if (!string.IsNullOrEmpty(ctlExchangeRateForeign2.Text) && UIHelper.ParseDouble(ctlExchangeRateForeign2.Text) > 0)
                    {
                        avDoc.ExchangeRateForLocalCurrency = UIHelper.ParseDouble(ctlExchangeRateForeign2.Text);
                    }
                }
                else // domestic
                {
                    if (!ctlPaymentType.SelectedValue.Equals(PaymentType.TR) && !ctlPaymentType.SelectedValue.Equals(string.Empty))
                        avDoc.PBID = new Dbpb(UIHelper.ParseLong(ctlCounterCashier.SelectedValue));

                    Dbpb dbPb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(UIHelper.ParseLong(ctlCounterCashier.SelectedValue));

                    if (IsRepOffice)
                    {
                        if (dbPb != null && dbPb.MainCurrencyID.HasValue)
                        {
                            avDoc.MainCurrencyID = dbPb.MainCurrencyID;

                            SS.Standard.WorkFlow.DTO.WorkFlow workflow = SS.Standard.WorkFlow.Query.WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(document.DocumentID);

                            if (workflow == null || (workflow != null && (workflow.CurrentState.Name.Equals(WorkFlowStateFlag.WaitVerify) || workflow.CurrentState.Name.Equals(WorkFlowStateFlag.Draft))))
                            {
                                CalculateExchangeRate();
                            }
                        }

                        avDoc.Amount = UIHelper.ParseDouble(ctlAmountTotalTHBLabel.Text);
                        avDoc.MainCurrencyAmount = UIHelper.ParseDouble(ctlAmountTotalMainCurrency.Text);
                    }
                    avDoc.ServiceTeamID = new DbServiceTeam(UIHelper.ParseLong(ctlServiceTeam.SelectedValue));
                    avDoc.AdvanceType = ZoneType.Domestic;
                    avDoc.ArrivalDate = null;
                    //domestic ต้อง check ว่า ค่าของ amount เกิน วงเงินของ counter cashier หรือเปล่า
                    // query หา dbpb จากการเลือก counterCashier
                    //avDoc.Amount = UIHelper.ParseDouble(ctlAmount.Text);
                    if (!string.IsNullOrEmpty(ctlExchangeRate1.Text) && UIHelper.ParseDouble(ctlExchangeRate1.Text) > 0)
                    {
                        avDoc.ExchangeRateForLocalCurrency = UIHelper.ParseDouble(ctlExchangeRate1.Text);
                    }

                    if (!string.IsNullOrEmpty(ctlExchangeRate2.Text) && UIHelper.ParseDouble(ctlExchangeRate2.Text) > 0)
                    {
                        avDoc.ExchangeRateMainToTHBCurrency = UIHelper.ParseDouble(ctlExchangeRate2.Text);
                    }

                    avDoc.PaymentType = ctlPaymentType.SelectedValue;
                    //if (ctlPaymentType.SelectedValue == PaymentType.CA)//check ว่า paymentType = เงินสดหรือไม่
                    //{
                    //    if (dbPb != null)
                    //    {
                    //        if (!dbPb.RepOffice)
                    //        {
                    //            double pettyCashDb = dbPb.PettyCashLimit != 0 ? dbPb.PettyCashLimit : 0;
                    //            double amountText = !string.IsNullOrEmpty(ctlAmount.Text) ? UIHelper.ParseDouble(ctlAmount.Text) : 0;
                    //            if (amountText > pettyCashDb)
                    //                avDoc.Amount = -1;//เอาไว้ check ค่าใน service ถ้าค่าของ amount มากกว่า pettyCash ให้ alert message เตือน
                    //        }
                    //    }
                    //}
                    if (ctlRequestDateOfAdvance.Value.HasValue)
                    {
                        try
                        {
                            avDoc.RequestDateOfAdvance = ctlRequestDateOfAdvance.Value.Value;
                            avDoc.DueDateOfRemittance = UIHelper.ParseDate(ctlRequestDateOfAdvance.DateValue).Value.AddDays(dueDateOfRemit);

                            DateTime RemittanceDate = UIHelper.ParseDate(ctlRequestDateOfAdvance.DateValue).Value.AddDays(requestDateOfRemit);
                            DateTime RemittanceDateInput = UIHelper.ParseDate(ctlRequestDateOfRemittance.DateValue).Value;
                            DateTime avDate = UIHelper.ParseDate(ctlRequestDateOfAdvance.DateValue).Value;
                            //check ว่า Date of Remittance อยู่ในช่วง ของ days ที่กำหนดหรือไม่
                            //ต้องอยู่ในช่วง Date of Advance และ  date of advance + x day (may be 30 day)
                            if (RemittanceDateInput.CompareTo(avDate) >= 0 && RemittanceDateInput.CompareTo(RemittanceDate) <= 0)
                            {
                                avDoc.RequestDateOfRemittance = RemittanceDateInput;
                            }
                            else
                                avDoc.RequestDateOfRemittance = DateTime.MaxValue;
                        }
                        catch (FormatException fex)
                        {
                            errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Invalid_Date_Format"));
                            throw new ServiceValidationException(errors);
                        }
                    }
                }
                if (!ctlTAIdLookup.Text.Trim().Equals(string.Empty))
                    avDoc.TADocumentID = UIHelper.ParseLong(ctlTAIdLookup.Text);

                avDoc.Reason = ctlReason.Text;
                avDoc.Active = true;
                #endregion

                #region save AvAdvanceItem
                AvAdvanceItem advanceItem = new AvAdvanceItem(this.DocumentID);
                AdvanceDataSet dstItem = (AdvanceDataSet)TransactionService.GetDS(this.TransactionID);
                AdvanceDataSet.AvAdvanceItemDataTable dtItem = dstItem.AvAdvanceItem;
                double totalAmount = 0;
                double totalAmountMain = 0;

                DataRow[] drItem;
                if (ctlAdvanceType.Text == AdvanceTypeEnum.International)
                {
                    if (ctlAdvanceIntGrid.Rows.Count > 0)
                    {
                        #region AvAdvanceItem(Foreign)
                        int countItem = ctlAdvanceIntGrid.Rows.Count;
                        bool isCurrencyUSD = false;//for check cal exchange rate perdiem
                        foreach (GridViewRow row in ctlAdvanceIntGrid.Rows)
                        {
                            UserControls.DropdownList.SCG.DB.StatusDropdown ctlStatusDropdown = (UserControls.DropdownList.SCG.DB.StatusDropdown)ctlAdvanceIntGrid.Rows[row.RowIndex].FindControl("ctlPaymentTypeDropdown");
                            UserControls.Dropdownlist.SS.DB.CurrencyDropdown ctlCurrencyDropdown = (UserControls.Dropdownlist.SS.DB.CurrencyDropdown)ctlAdvanceIntGrid.Rows[row.RowIndex].FindControl("ctlCurrencyDropdown");
                            TextBox ctlAmountGird = (TextBox)ctlAdvanceIntGrid.Rows[row.RowIndex].FindControl("ctlAmount");
                            Label ctlExchangeRate = (Label)ctlAdvanceIntGrid.Rows[row.RowIndex].FindControl("ctlExchangeRate");
                            TextBox ctlAmountTHB = (TextBox)ctlAdvanceIntGrid.Rows[row.RowIndex].FindControl("ctlAmountTHB");
                            TextBox ctlAmountMain = (TextBox)ctlAdvanceIntGrid.Rows[row.RowIndex].FindControl("ctlAmountMain");
                            HiddenField ctlCurrencyID = (HiddenField)ctlAdvanceIntGrid.Rows[row.RowIndex].FindControl("ctlCurrencyID");
                            //HiddenField ctlPaymentTypeID = (HiddenField)ctlAdvanceIntGrid.Rows[row.RowIndex].FindControl("ctlPaymentTypeID");
                            UserControls.DropdownList.SCG.DB.StatusDropdown ctlPaymentTypeID = (UserControls.DropdownList.SCG.DB.StatusDropdown)ctlAdvanceIntGrid.Rows[row.RowIndex].FindControl("ctlPaymentTypeDDL");

                            int advanceItemId = UIHelper.ParseInt(ctlAdvanceIntGrid.DataKeys[row.RowIndex].Value.ToString());

                            drItem = dtItem.Select("PaymentType='" + ctlPaymentTypeID.SelectedValue + "' and CurrencyID= '" + ctlCurrencyID.Value + "' and AdvanceItemID <> '" + advanceItemId + "' ");
                            if (drItem.Length > 0)
                            {
                                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("AdvanceItemIsDuplication"));
                                break;
                            }
                            else
                            {
                                #region save avadvanceItem

                                //คิด perdiem เฉพาะ USD
                                if (ctlCurrencyID.Value == currencyUsd.ToString() && !IsRepOffice)
                                {
                                    totalAmountUSD += UIHelper.ParseDouble(ctlAmountGird.Text);
                                    totalAmountThbUSD += UIHelper.ParseDouble(ctlAmountTHB.Text);
                                    isCurrencyUSD = true;
                                }
                                else if (ctlCurrencyID.Value == currencyUsd.ToString() && IsRepOffice)
                                {
                                    totalAmount += UIHelper.ParseDouble(ctlAmountGird.Text);
                                    totalAmountMain += UIHelper.ParseDouble(ctlAmountMain.Text);
                                    isCurrencyUSD = true;
                                }

                                advanceItem = new AvAdvanceItem(advanceItemId);

                                advanceItem.AdvanceID = new AvAdvanceDocument(this.DocumentID);
                                advanceItem.PaymentType = ctlPaymentTypeID.SelectedValue;
                                advanceItem.CurrencyID = new DbCurrency(UIHelper.ParseShort(ctlCurrencyID.Value));
                                advanceItem.Amount = UIHelper.ParseDouble(ctlAmountGird.Text);
                                //ถ้าเป็นกรณีที่ไม่มี group ของ ExchangeRateForPerDiemCalculation ก็ไม่ต้องคำนวนค่า ExchangeRate, AmountTHB
                                //จะคำนวนค่าได้ก็ต่อเมื่อเป็น verifier ขึ้นไป
                                if (VisibleFields.Contains(AdvanceFieldGroup.ExchangeRateForPerDiemCalculation))
                                {
                                    if (!IsRepOffice)
                                    {
                                        advanceItem.AmountTHB = UIHelper.ParseDouble(ctlAmountTHB.Text);
                                        advanceItem.ExchangeRate = (double)Math.Round((decimal)(advanceItem.AmountTHB / advanceItem.Amount), 5, MidpointRounding.AwayFromZero);//UIHelper.ParseDouble(ctlExchangeRate.Text);
                                    }
                                    else
                                    {
                                        advanceItem.MainCurrencyAmount = UIHelper.ParseDouble(ctlAmountMain.Text);
                                        //advanceItem.ExchangeRate = 0;
                                        //if (advanceItem.Amount != 0)
                                        //{
                                        advanceItem.ExchangeRate = (double)Math.Round((decimal)(advanceItem.MainCurrencyAmount / advanceItem.Amount), 5, MidpointRounding.AwayFromZero);
                                        // }
                                        advanceItem.ExchangeRateTHB = UIHelper.ParseDouble(ctlExchangeRateForeign.Text);
                                        advanceItem.AmountTHB = (double)Math.Round((decimal)(advanceItem.MainCurrencyAmount * advanceItem.ExchangeRateTHB), 2, MidpointRounding.AwayFromZero);
                                    }
                                }
                                else
                                {
                                    if (!IsRepOffice)
                                    {
                                        advanceItem.ExchangeRate = 0;
                                        advanceItem.AmountTHB = 0;
                                    }
                                    else
                                    {

                                        if (avDoc.MainCurrencyID.HasValue)
                                        {
                                            if (countItem == 1 && UIHelper.ParseShort(ctlCurrencyID.Value) == avDoc.MainCurrencyID.Value)
                                            {
                                                GetExchangeRateForRepOffice(ctlAdvanceType.Text, avDoc);
                                                if (!advanceItem.MainCurrencyAmount.HasValue || advanceItem.MainCurrencyAmount.Value == 0)
                                                {
                                                    advanceItem.MainCurrencyAmount = advanceItem.Amount;
                                                }

                                                if (UIHelper.ParseDecimal(advanceItem.MainCurrencyAmount.ToString()) != 0)
                                                {
                                                    advanceItem.ExchangeRate = (double)Math.Round((UIHelper.ParseDecimal(advanceItem.MainCurrencyAmount.Value.ToString()) / UIHelper.ParseDecimal(advanceItem.Amount.ToString())), 5, MidpointRounding.AwayFromZero);
                                                }

                                                decimal amountThb = Math.Round((UIHelper.ParseDecimal(advanceItem.MainCurrencyAmount.Value.ToString()) * UIHelper.ParseDecimal((!avDoc.ExchangeRateMainToTHBCurrency.HasValue ? 0 : avDoc.ExchangeRateMainToTHBCurrency.Value).ToString())), 2, MidpointRounding.AwayFromZero);
                                                advanceItem.AmountTHB = UIHelper.ParseDouble(amountThb.ToString());
                                            }
                                        }
                                    }
                                    //advanceItem.MainCurrencyAmount = 0;
                                }
                                totalAmountTHB += advanceItem.AmountTHB;
                                if (advanceItem.MainCurrencyAmount.HasValue)
                                {
                                    totalAmountMainCurrency += advanceItem.MainCurrencyAmount.Value;
                                }
                                advanceItem.Active = true;
                                #endregion
                                try
                                {
                                    // Get ta document schedule information and save to transaction.
                                    AvAdvanceItemService.UpdateAdvanceItemTransaction(this.TransactionID, advanceItem, IsRepOffice);
                                }
                                catch (ServiceValidationException ex)
                                {
                                    errors.MergeErrors(ex.ValidationErrors);
                                }
                            }
                        }
                        #endregion
                        //คำนวนค่าของ PerdiemRate ถ้าไม่มี Currency= USD ให้ save ค่าของ PerdiemExchangeRateUSD จาก TextBox
                        if (!IsRepOffice)
                        {
                            if (isCurrencyUSD && string.IsNullOrEmpty(Request.QueryString["cp"]))
                            {
                                if (totalAmountUSD != 0)
                                    totalPerdiemRateUSD = totalAmountThbUSD / totalAmountUSD;
                            }
                            else
                            {
                                totalPerdiemRateUSD = UIHelper.ParseDouble(ctlExchangeRateForPerDiem.Text);

                                //กรณีที่ไม่มี USD และไม่ใส่ค่า perdiemRate ต้อง alter เตือนให้ใส่ ค่าด้วย
                                if (VisibleFields.Contains(AdvanceFieldGroup.ExchangeRateForPerDiemCalculation))
                                {
                                    if (string.IsNullOrEmpty(totalPerdiemRateUSD.ToString()) || totalPerdiemRateUSD == 0)
                                    {
                                        totalPerdiemRateUSD = -1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (isCurrencyUSD && string.IsNullOrEmpty(Request.QueryString["cp"]))
                            {
                                totalPerdiemRateUSD = totalAmountMain / totalAmount;
                            }
                            else
                            {
                                if (avDoc.MainCurrencyID.HasValue && (avDoc.MainCurrencyID.Value == (short)currencyUsd))
                                {
                                    totalPerdiemRateUSD = 1;
                                }
                                else
                                {
                                    totalPerdiemRateUSD = UIHelper.ParseDouble(ctlExchangeRateForPerDiem.Text);
                                }
                                //กรณีที่ไม่มี USD และไม่ใส่ค่า perdiemRate ต้อง alter เตือนให้ใส่ ค่าด้วย
                                if (VisibleFields.Contains(AdvanceFieldGroup.ExchangeRateForPerDiemCalculation))
                                {
                                    if (string.IsNullOrEmpty(totalPerdiemRateUSD.ToString()) || totalPerdiemRateUSD == 0)
                                    {
                                        totalPerdiemRateUSD = -1;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("PaymentTypeIsRequired"));
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("CurrencyIsRequired"));
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("AmountIsRequired"));
                        if (VisibleFields.Contains(AdvanceFieldGroup.ExchangeRateForPerDiemCalculation))
                        {
                            errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("AmountTHBRequired"));
                        }
                    }
                }
                else if (ctlAdvanceType.Text == AdvanceTypeEnum.Domestic)
                {
                    #region AvAdvanceItem(Domestic)

                    if (!InitialFlag.Equals(FlagEnum.NewFlag))
                    {
                        long advanceItemID = 0;
                        IList<AvAdvanceItem> itemList = ScgeAccountingQueryProvider.AvAdvanceItemQuery.FindByAvAdvanceItemAdvanceID(this.DocumentID);
                        if (itemList != null && itemList.Count > 0)
                        {
                            advanceItemID = itemList[0].AdvanceItemID;
                            advanceItem = new AvAdvanceItem(advanceItemID);
                        }
                    }
                    advanceItem.AdvanceID = new AvAdvanceDocument(this.DocumentID);
                    if (!ctlPaymentType.SelectedValue.Equals(string.Empty))
                        advanceItem.PaymentType = ctlPaymentType.SelectedValue;

                    if (IsRepOffice)
                    {
                        double amountLocalCurrency = UIHelper.ParseDouble(ctlAmount.Text);
                        advanceItem.Amount = amountLocalCurrency;
                        advanceItem.CurrencyID = new DbCurrency(UIHelper.ParseShort(ctlCurrencyDropdown.SelectedValue));

                        if (avDoc.ExchangeRateForLocalCurrency.HasValue)
                        {
                            advanceItem.ExchangeRate = avDoc.ExchangeRateForLocalCurrency.Value;
                        }

                        if (avDoc.ExchangeRateMainToTHBCurrency.HasValue)
                        {
                            advanceItem.ExchangeRateTHB = avDoc.ExchangeRateMainToTHBCurrency.Value;
                        }

                        advanceItem.AmountTHB = avDoc.Amount;
                        advanceItem.MainCurrencyAmount = avDoc.MainCurrencyAmount;
                        avDoc.LocalCurrencyAmount = amountLocalCurrency;
                    }
                    else
                    {
                        advanceItem.AmountTHB = UIHelper.ParseDouble(ctlAmount.Text);
                        avDoc.Amount = UIHelper.ParseDouble(ctlAmount.Text);
                    }

                    advanceItem.Active = true;

                    try
                    {
                        // Get ta document schedule information and save to transaction.
                        if (InitialFlag.Equals(FlagEnum.NewFlag))
                        {
                            dstItem.AvAdvanceItem.Clear();
                            AvAdvanceItemService.AddAdvanceItem(this.TransactionID, advanceItem);
                        }
                        else
                        {
                            AvAdvanceItemService.UpdateAdvanceItemTransaction(this.TransactionID, advanceItem, IsRepOffice);
                        }
                    }
                    catch (ServiceValidationException ex)
                    {
                        errors.MergeErrors(ex.ValidationErrors);
                    }

                    #endregion
                }
                #endregion

                #region Save Initiator
                try
                {
                    //if (Request.QueryString["taDocumentID"] == null)
                    ctlInitiator.Save();
                }
                catch (ServiceValidationException ex)
                {
                    errors.MergeErrors(ex.ValidationErrors);
                }
                #endregion

                #region save AvAdvanceDocument
                try
                {
                    //เฉพาะ Foreign ได้มาจาก คำนวนค่า perdiemrate and amount 
                    if (ctlAdvanceType.Text == AdvanceTypeEnum.International)
                    {
                        avDoc.Amount = totalAmountTHB;

                        if (IsRepOffice && totalAmountMainCurrency > 0)
                        {
                            avDoc.MainCurrencyAmount = totalAmountMainCurrency;
                        }
                    }


                    avDoc.PerDiemExRateUSD = totalPerdiemRateUSD;
                    AvAdvanceDocumentService.UpdateAvDocumentTransaction(avDoc, TransactionID); // กำลังเซฟ
                }
                catch (ServiceValidationException ex)
                {
                    errors.MergeErrors(ex.ValidationErrors);
                }
                #endregion
            }
            else
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("AdvanceFromTAIsDuplicate"));
            }
            if (!errors.IsEmpty)
            {
                // If have some validation error then return error.
                throw new ServiceValidationException(errors);
            }
            else
            {
                return this.SaveToDatabase();
            }

        }
        #endregion

        #region SaveToDatabase()
        public long SaveToDatabase()
        {
            // Save all table in dataset to database and clear transaction.
            // return document id of AvDocument.

            long avDocumentID = AvAdvanceDocumentService.SaveAvAdvance(this.TransactionID, this.DocumentID);
            TransactionService.Commit(this.TransactionID);

            long workFlowID = 0;
            // Get avDocument
            AvAdvanceDocument avDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindProxyByIdentity(avDocumentID);
            // Save New WorkFlow.
            #region Save WorkFlow
            if ((avDocument != null) && (avDocument.DocumentID != null))
            {
                SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.FindProxyByIdentity(avDocument.DocumentID.DocumentID);
                SS.Standard.WorkFlow.DTO.WorkFlow workFlow = new SS.Standard.WorkFlow.DTO.WorkFlow();

                workFlow.WorkFlowType = new SS.Standard.WorkFlow.DTO.WorkFlowType(WorkFlowTypeID.AdvanceWorkFlowType);
                workFlow.CurrentState = WorkFlowQueryProvider.WorkFlowStateQuery.FindWorkFlowStateIDByTypeIDAndStateName(WorkFlowTypeID.AdvanceWorkFlowType, WorkFlowStateFlag.Draft);

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
            #endregion
            return workFlowID;
        }
        #endregion
        #endregion

        #region InitializeControl()
        private void InitializeControl()
        {
            ctlCompanyField.UseEccOnly = true;
            ctlCompanyField.FlagActive = true;
            AdvanceDataSet advanceDataSet = (AdvanceDataSet)TransactionService.GetDS(this.TransactionID);
            AdvanceDataSet.AvAdvanceDocumentRow row = advanceDataSet.AvAdvanceDocument.FindByAdvanceID(this.DocumentID);
            long tempAdvanceID = row.AdvanceID;
            // หา  current state ของเอกสาร
            string mode = this.InitialFlag;
            if (!string.IsNullOrEmpty(Request.Params["wfid"]) && UIHelper.ParseInt(Request.Params["wfid"]) > 0)
            {
                long workFlowID = UIHelper.ParseLong(Request.Params["wfid"]);

                SS.Standard.WorkFlow.DTO.WorkFlow wf = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
                if (wf != null && wf.CurrentState != null && wf.CurrentState.Name != null)
                {
                    if (wf.CurrentState.Name.Equals(WorkFlowStateFlag.WaitPayment))
                        mode = FlagEnum.ViewFlag;
                }
            }
            ctlAttachment.Initialize(this.TransactionID, this.DocumentID, mode); // ถ้าเป็น wait for payment จะแก้ไข attachment ไม่ได้

            ctlInitiator.ControlGroupID = SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.Initiator;
            if (Request.QueryString["taDocumentID"] == null)
                ctlInitiator.Initialize(this.TransactionID, tempAdvanceID, this.InitialFlag);
            else
            {
                ctlInitiator.Initialize(this.TransactionID, tempAdvanceID, FlagEnum.ViewFlag);
            }

            long tempDocumentID = row.DocumentID;

            ctlHistory.Initialize(tempDocumentID);

            ctlAdvanceFormHeader.Initialize(this.TransactionID, tempDocumentID, this.InitialFlag);      // send SCGDocument.DocumentID for check visible see history

            ctlApproverData.DocumentEditor = this;
            ctlCreatorData.ControlGroupID = SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.BuActor;
            ctlRequesterData.ControlGroupID = SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.AdvanceReferTA;
            ctlReceiverData.ControlGroupID = SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.BuActor;
            ctlApproverData.ControlGroupID = SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.AdvanceReferTA;

            ctlCreatorData.Initialize(this.TransactionID, this.DocumentID, this.InitialFlag);
            ctlRequesterData.Initialize(this.TransactionID, this.DocumentID, this.InitialFlag);
            ctlReceiverData.Initialize(this.TransactionID, this.DocumentID, this.InitialFlag);
            ctlApproverData.Initialize(this.TransactionID, this.DocumentID, this.InitialFlag);

            ctlAdvanceFormHeader.DataBind();
            ctlCreatorData.DataBind();
            ctlApproverData.DataBind();
            ctlReceiverData.DataBind();
            ctlRequesterData.DataBind();
        }
        #endregion

        #region BindControl()
        public void BindControl(bool isCopy)
        {
            if (this.InitialFlag.Equals(FlagEnum.ViewFlag))
            {
                ctlViewPostButtonDomestic.Visible = true;
                ctlViewPostForeign.Visible = true;
                ctlTANoLookup.Visible = false;
                ctlDeleteTA.Visible = false;

            }
            else
            {
                ctlViewPostButtonDomestic.Visible = false;
                ctlViewPostForeign.Visible = false;
                if (CurrentStateName.Equals(WorkFlowStateFlag.WaitPayment))
                {
                    ctlTANoLookup.Visible = false;
                    ctlDeleteTA.Visible = false;
                }
                else
                {
                    ctlTANoLookup.Visible = true;
                    ctlDeleteTA.Visible = true;
                }
            }

            //กรณี foreign ไม่ต้อง set ค่า service team ซ่อนไปเลย
            if (VisibleFields.Contains(AdvanceFieldGroup.ServiceTeam))
            {
                if (ctlAdvanceType.Text.Equals(AdvanceTypeEnum.International))
                    ctlDivServiceTeam.Visible = false;
                else
                    ctlDivServiceTeam.Visible = true;
            }
            else
                ctlDivServiceTeam.Visible = false;
            //check memo
            //if (VisibleFields.Contains(AdvanceFieldGroup.Memo) && EditableFields.Contains(AdvanceFieldGroup.Memo))
            //{
            //    if (this.InitialFlag.Equals(FlagEnum.ViewFlag))
            //        ctlMemo.Enabled = false;
            //    else
            //        ctlMemo.Enabled = true;
            //}
            //else
            //    ctlMemo.Enabled = false;
            //กรณีที่สร้าง advance จาก Ta ห้าม แก้ไข Document No ได้
            if (Request.QueryString["taDocumentID"] != null)
            {
                ctlTANoLookup.Visible = false;
                ctlDeleteTA.Visible = false;
            }
            if (!isCopy && this.InitialFlag.Equals(FlagEnum.NewFlag))
            {
                ctlAdvanceFormHeader.Status = FlagEnum.NewFlag;
                ctlCompanyField.ShowDefault();
                ctlCreatorData.ShowDefault();
                ctlRequesterData.ShowDefault();
                ctlInitiator.RequesterID = UIHelper.ParseLong(ctlRequesterData.UserID);
                ctlReceiverData.ShowDefault();

                // ctlApproverData.ShowDefault();
                ctlApproverData.ShowDefaultApprover(UIHelper.ParseLong(ctlRequesterData.UserID));
                if (Request.QueryString["taDocumentID"] != null)
                {
                    AdvanceDataSet advanceDS = (AdvanceDataSet)TransactionService.GetDS(this.TransactionID);

                    ctlCompanyField.SetValue(UIHelper.ParseLong(advanceDS.Tables["Document"].Rows[0]["CompanyID"].ToString()));
                    ctlCreatorData.SetValue(UIHelper.ParseLong(advanceDS.Tables["Document"].Rows[0]["CreatorID"].ToString()));
                    //ctlRequesterData.SetValue(UIHelper.ParseLong(advanceDS.Tables["Document"].Rows[0]["RequesterID"].ToString()));
                    ctlRequesterData.SetValue(UIHelper.ParseLong(Request.QueryString["requesterID"].ToString()));
                    ctlReceiverData.SetValue(UIHelper.ParseLong(Request.QueryString["requesterID"].ToString()));
                    ctlApproverData.SetValue(UIHelper.ParseLong(advanceDS.Tables["Document"].Rows[0]["ApproverID"].ToString()));
                    if (advanceDS.Tables["AvAdvanceDocument"].Rows[0]["AdvanceType"].ToString().Equals(ZoneType.Foreign))
                    {
                        ctlArrivalDate.DateValue = UIHelper.ToDateString((DateTime)advanceDS.Tables["AvAdvanceDocument"].Rows[0]["ArrivalDate"]);
                        ctlDueDateOfRemittance.Text = UIHelper.ToDateString(UIHelper.ParseDate(ctlArrivalDate.DateValue).Value.AddDays(dueDateOfRemit));
                        ctlRequestDateOfRemittance.DateValue = ctlDueDateOfRemittance.Text;
                    }
                }
                if (ctlServiceTeam.Items.Count > 0)
                    BindServiceTeam();
                CheckRepOffice();
            }
            else
            {
                if (UIHelper.ParseInt(Request.Params["wfid"]) > 0)//กรณีคืนค่ากลับมาหลังจาก save เสร็จ
                {
                    long workFlowID = UIHelper.ParseLong(Request.Params["wfid"]);

                    int documentTypeID = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID).Document.DocumentType.DocumentTypeID;
                    if (documentTypeID == DocumentTypeID.AdvanceDomesticDocument)
                    {
                        ctlAdvanceType.Text = AdvanceTypeEnum.Domestic;
                        HeaderForm = GetProgramMessage("$DMAdvanceForm$");
                    }
                    else
                    {
                        ctlAdvanceType.Text = AdvanceTypeEnum.International;
                        HeaderForm = GetProgramMessage("$FRAdvanceForm$");
                    }


                }

                if (VisibleFields.Contains(AdvanceFieldGroup.ServiceTeam))
                {
                    if (ctlAdvanceType.Text.Equals(AdvanceTypeEnum.International))
                        ctlDivServiceTeam.Visible = false;
                    else
                        ctlDivServiceTeam.Visible = true;
                }
                Guid txID = this.TransactionID;
                long advanceID = this.DocumentID;

                AdvanceDataSet advanceDS = (AdvanceDataSet)TransactionService.GetDS(txID);
                AdvanceDataSet.AvAdvanceDocumentRow advanceRow = advanceDS.AvAdvanceDocument.FindByAdvanceID(advanceID);
                AdvanceDataSet.DocumentRow document = advanceDS.Document.FindByDocumentID(advanceRow.DocumentID);
                #region Bind Header and Footer

                if (!isCopy)
                {
                    ctlAdvanceFormHeader.No = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(advanceRow.DocumentID).DocumentNo;
                    if (document.DocumentDate != DateTime.MinValue)
                        ctlAdvanceFormHeader.CreateDate = UIHelper.ToDateString(document.DocumentDate);
                    ctlAdvanceFormHeader.Status = ScgeAccountingQueryProvider.SCGDocumentQuery.GetDocumentCurrentStateName(UserAccount.CurrentLanguageID, advanceRow.DocumentID);

                    ctlCreatorData.SetValue(document.CreatorID);
                }
                else
                {
                    ctlAdvanceFormHeader.Status = FlagEnum.NewFlag;
                    ctlCreatorData.ShowDefault();
                }

                ctlCompanyField.SetValue(document.CompanyID);
                ctlSubject.Text = document.Subject;
                ctlRequesterData.SetValue(document.RequesterID);
                ctlReceiverData.SetValue(document.ReceiverID);
                ctlApproverData.SetValue(document.ApproverID);
                #endregion

                CheckRepOffice();

                #region Bind General Tab (Domestic/Foreign)
                //query TAdocument for set link button
                if (!advanceRow.IsTADocumentIDNull() && advanceRow.TADocumentID != 0)
                {
                    TADocument taDocument = ScgeAccountingQueryProvider.TADocumentQuery.FindByIdentity(advanceRow.TADocumentID);
                    if (taDocument != null)
                    {
                        ctlTANo.Visible = true;
                        ctlNATaNo.Visible = false;
                        long documentID = taDocument.DocumentID.DocumentID;
                        ctlTANo.Text = ScgeAccountingQueryProvider.SCGDocumentQuery.FindByIdentity(documentID).DocumentNo;
                        this.DocumentIDofTA = documentID;
                        //ctlTAIdLookup.Text = advanceRow.TADocumentID.ToString();
                        ctlTAIdLookup.Text = taDocument.TADocumentID.ToString();//documentID.ToString();
                    }
                    else
                    {
                        ctlTANo.Visible = false;
                        ctlNATaNo.Visible = true;
                        ctlNATaNo.Text = "N/A";
                    }
                }

                if (advanceRow.IsAmountNull())
                {
                    ctlTotalAmountTHBLabel.Text = "0.00";
                }
                else
                {
                    ctlTotalAmountTHBLabel.Text = UIHelper.BindDecimal(advanceRow.Amount.ToString());
                }

                if (!advanceRow.IsMainCurrencyAmountNull())
                {
                    ctlTotalAmountMainLabel.Text = UIHelper.BindDecimal(advanceRow.MainCurrencyAmount.ToString());
                }
                if (!advanceRow.IsExchangeRateMainToTHBCurrencyNull())
                {
                    ctlExchangeRateForeign.Text = UIHelper.BindExchangeRate((advanceRow.ExchangeRateMainToTHBCurrency.ToString()));
                }

                if (!advanceRow.IsExchangeRateForLocalCurrencyNull())
                {
                    ctlExchangeRateForeign2.Text = UIHelper.BindExchangeRate((advanceRow.ExchangeRateForLocalCurrency.ToString()));
                }

                ctlDueDateOfRemittance.Text = UIHelper.ToDateString(advanceRow.DueDateOfRemittance);
                ctlRequestDateOfRemittance.DateValue = UIHelper.ToDateString(advanceRow.RequestDateOfRemittance);
                ctlReason.Text = advanceRow.Reason;
                if (string.IsNullOrEmpty(ctlTANo.Text))
                {
                    ctlTANo.Visible = false;
                    ctlNATaNo.Visible = true;
                    ctlNATaNo.Text = "N/A";
                }

                if (ctlAdvanceType.Text == AdvanceTypeEnum.Domestic)
                {
                    //ctlServiceTeam.SelectedValue = advanceRow.ServiceTeamID.ToString();
                    //ctlAmount.Text = advanceRow.Amount.ToString();
                    ctlRequestDateOfAdvance.DateValue = UIHelper.ToDateString(advanceRow.RequestDateOfAdvance);
                    ctlPaymentType.DataSource = ScgDbQueryProvider.SCGDbStatusLangQuery.FindStatusLangCriteria(documentTypeDMT, UserAccount.CurrentLanguageID);
                    ctlPaymentType.DataTextField = "strSymbol";
                    ctlPaymentType.DataValueField = "strID";
                    ctlPaymentType.DataBind();
                    if (ctlPaymentType.Items.Count > 0)
                    {
                        ctlPaymentType.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
                    }

                    ctlPaymentType.SelectedValue = advanceDS.AvAdvanceItem.Rows[advanceDS.AvAdvanceItem.Count - 1]["PaymentType"].ToString();//ScgeAccountingQueryProvider.AvAdvanceItemQuery.FindByAvAdvanceItemAdvanceID(advanceRow.AdvanceID)[0].PaymentType;

                    if (!advanceRow.IsPBIDNull())
                    {
                        ctlCounterCashier.DataSource = ScgDbQueryProvider.DbPBQuery.GetPbListItem(UIHelper.ParseLong(ctlCompanyField.CompanyID), UserAccount.CurrentLanguageID);
                        ctlCounterCashier.DataBind();
                        if (ctlCounterCashier.Items.Count > 0)
                            ctlCounterCashier.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));

                        ctlCounterCashier.SelectedValue = advanceRow.PBID.ToString();
                    }
                    else
                    {
                        //ctlCounterCashier.DataSource = ScgDbQueryProvider.DbPBQuery.GetPbListItem(UIHelper.ParseLong(ctlCompanyField.CompanyID), UserAccount.CurrentLanguageID);
                        //ctlCounterCashier.DataBind();
                        ctlCounterCashier.DataSource = null;
                        ctlCounterCashier.DataBind();
                        ctlCounterCashier.SelectedIndex = -1;
                    }

                    BindCurrencyDropdown();
                    ctlCurrencyDropdown.SelectedValue = advanceDS.AvAdvanceItem.Rows[advanceDS.AvAdvanceItem.Count - 1]["CurrencyID"].ToString();
                    ShowAmountLabel();

                    if (IsRepOffice)
                    {
                        if (!string.IsNullOrEmpty(advanceDS.AvAdvanceItem.Rows[advanceDS.AvAdvanceItem.Count - 1]["Amount"].ToString()))
                        {
                            ctlAmount.Text = UIHelper.BindDecimal(advanceDS.AvAdvanceItem.Rows[advanceDS.AvAdvanceItem.Count - 1]["Amount"].ToString());
                        }
                        SS.Standard.WorkFlow.DTO.WorkFlow workflow = SS.Standard.WorkFlow.Query.WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(document.DocumentID);

                        if ((workflow != null && (workflow.CurrentState.Name.Equals(WorkFlowStateFlag.WaitVerify) || workflow.CurrentState.Name.Equals(WorkFlowStateFlag.Draft))) && (this.InitialFlag.Equals(FlagEnum.EditFlag)))
                        {
                            CalculateExchangeRate();
                        }
                        else
                        {
                            ctlAmountTotalMainCurrency.Text = UIHelper.BindDecimal(advanceRow.IsMainCurrencyAmountNull() ? "0" : advanceRow.MainCurrencyAmount.ToString());
                            ctlAmountTotalTHBLabel.Text = UIHelper.BindDecimal(advanceRow.IsAmountNull() ? "0" : advanceRow.Amount.ToString());
                            ctlExchangeRate1.Text = UIHelper.BindExchangeRate(advanceRow.IsExchangeRateForLocalCurrencyNull() ? "0" : advanceRow.ExchangeRateForLocalCurrency.ToString());
                            ctlExchangeRate2.Text = UIHelper.BindExchangeRate(advanceRow.IsExchangeRateMainToTHBCurrencyNull() ? "0" : advanceRow.ExchangeRateMainToTHBCurrency.ToString());
                        }

                        if (!advanceRow.IsMainCurrencyIDNull())
                        {
                            DbCurrency mainCurrency = SsDbQueryProvider.DbCurrencyQuery.FindByIdentity(advanceRow.MainCurrencyID);
                            IList<DbPBCurrency> pbCurrency = ScgDbQueryProvider.DbPBCurrencyQuery.FindPBLocalCurrencyByPBID(UIHelper.ParseLong(ctlCounterCashier.SelectedValue), ZoneType.Domestic);
                            var currencyLocal = pbCurrency.Where(t => t.CurrencyID != mainCurrency.CurrencyID).FirstOrDefault();
                            if (currencyLocal != null)
                            {
                                ctlCurrencyExchangeRateLabel1.Text = string.Format("{0}/{1}", currencyLocal.Symbol, mainCurrency.Symbol);
                            }

                            ctlCurrencyExchangeRateLabel2.Text = string.Format("{0}/{1}", CurrencySymbol.THB, mainCurrency.Symbol);
                        }
                    }
                    else
                    {
                        if (advanceRow.IsAmountNull())  // amountTHB???
                        {
                            ctlAmount.Text = "0.00";
                        }
                        else
                        {
                            ctlAmount.Text = UIHelper.BindDecimal(advanceRow.Amount.ToString());
                        }
                    }

                    //hideCounterCashier();
                    //show remark 
                    ctlRemarkDomestic.Visible = true;
                    ctlRemarkForeign.Visible = false;



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

                    ctlServiceTeam.DataSource = ScgDbQueryProvider.DbLocationQuery.FindByLocationCompany(UIHelper.ParseLong(ctlCompanyField.CompanyID), null, UserAccount.UserID, locationID);
                    ctlServiceTeam.DataBind();
                    if (ctlServiceTeam.Items.Count > 1)
                    {
                        ctlServiceTeam.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));

                        if (ctlServiceTeam.Items.FindByValue(advanceRow.ServiceTeamID.ToString()) != null)
                            ctlServiceTeam.SelectedValue = advanceRow.ServiceTeamID.ToString();
                        else
                        {
                            IList<DbServiceTeamLocation> listLocation = ScgDbQueryProvider.DbServiceTeamLocationQuery.FindServiceTeamByLocationID(locationID);
                            if (listLocation != null && listLocation.Count > 0)
                            {
                                if (ctlServiceTeam.Items.FindByValue(listLocation.First<DbServiceTeamLocation>().ServiceTeamID.ToString()) != null)
                                    ctlServiceTeam.SelectedValue = listLocation.First<DbServiceTeamLocation>().ServiceTeamID.ToString();
                            }
                            else
                            {
                                ctlServiceTeam.SelectedIndex = 0;
                            }
                        }

                    }
                    //-----------------
                    
                }
                else // Foreign
                {
                    if (!advanceRow.IsPBIDNull())
                    {
                        ctlCounterCashierInter.DataSource = ScgDbQueryProvider.DbPBQuery.GetPbListItem(UIHelper.ParseLong(ctlCompanyField.CompanyID), UserAccount.CurrentLanguageID);
                        ctlCounterCashierInter.DataBind();

                        // แก้ไขเรื่องการที่มัน error เวลาที่มันไม่มีค่าที่กำหนดได้ครับ
                        try
                        {
                            ctlCounterCashierInter.SelectedValue = advanceRow.PBID.ToString();
                        }
                        catch (Exception ex)
                        {
                            if (logger != null)
                            {
                                logger.Error("AdvanceDocumentEditor.BindControl() : " + ex.ToString());
                            }
                        }
                    }
                    else
                        ctlCounterCashierInter.Text = string.Empty;

                    short mainCurrencyId = 0;
                    short localCurrencyId = 0;
                    long pbid = UIHelper.ParseLong(ctlCounterCashierInter.SelectedValue);
                    Dbpb pb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(pbid);

                    if (pb != null && pb.MainCurrencyID.HasValue)
                    {
                        mainCurrencyId = pb.MainCurrencyID.Value;
                        DbCurrency mainCurrency = SsDbQueryProvider.DbCurrencyQuery.FindByIdentity(mainCurrencyId);
                        ctlExchangeRateCurrencyLabel.Text = string.Format("{0}/{1}", CurrencySymbol.THB, mainCurrency.Symbol);

                        IList<DbPBCurrency> pbCurrency = ScgDbQueryProvider.DbPBCurrencyQuery.FindPBLocalCurrencyByPBID(pb.Pbid, ZoneType.Domestic);
                        var currencyLocal = pbCurrency.Where(t => t.CurrencyID != mainCurrencyId).FirstOrDefault();
                        if (currencyLocal != null)
                        {
                            localCurrencyId = currencyLocal.CurrencyID;
                            DbCurrency currency = SsDbQueryProvider.DbCurrencyQuery.FindCurrencyById(localCurrencyId);
                            ctlExchangeRateCurrencyLabel2.Text = string.Format("{0}/{1}", currency.Symbol, mainCurrency.Symbol);
                        }
                    }

                    if (InitialFlag.Equals(FlagEnum.EditFlag) && (CurrentStateName.Equals(WorkFlowStateFlag.Draft) || CurrentStateName.Equals(WorkFlowStateFlag.WaitVerify)))
                    {
                        if (mainCurrencyId > 0)
                        {
                            DbCurrency currencyTHB = SsDbQueryProvider.DbCurrencyQuery.FindByCurrencySymbol(CurrencySymbol.THB.ToString(), false, false);
                            double exchangRateTHB = ScgDbQueryProvider.DbPbRateQuery.GetExchangeRate(pbid, mainCurrencyId, currencyTHB.CurrencyID);
                            ctlExchangeRateForeign.Text = UIHelper.BindExchangeRate(exchangRateTHB.ToString());
                        }

                        if (localCurrencyId > 0)
                        {
                            double exchangeRateLocal = ScgDbQueryProvider.DbPbRateQuery.GetExchangeRate(pb.Pbid, mainCurrencyId, localCurrencyId);
                            ctlExchangeRateForeign2.Text = UIHelper.BindExchangeRate(exchangeRateLocal.ToString());
                        }
                    }
                    ctlRequestDateOfAdvanceForeign.DateValue = UIHelper.ToDateString(advanceRow.RequestDateOfAdvance);
                    ctlArrivalDate.DateValue = UIHelper.ToDateString((DateTime?)advanceRow.ArrivalDate);
                    totalAmountTHB = 0;
                    totalAmountMainCurrency = 0;
                    ctlAdvanceIntGrid.DataSource = advanceDS.AvAdvanceItem;
                    ctlAdvanceIntGrid.DataBind();

                    if (InitialFlag.Equals(FlagEnum.EditFlag))
                    {
                        ctlTotalAmountTHBLabel.Text = UIHelper.BindDecimal(totalAmountTHB.ToString());
                        ctlTotalAmountMainLabel.Text = UIHelper.BindDecimal(totalAmountMainCurrency.ToString());
                    }


                    if (!advanceRow.IsPerDiemExRateUSDNull())
                        ctlExchangeRateForPerDiem.Text = UIHelper.BindExchangeRate(advanceRow.PerDiemExRateUSD.ToString());

                    ctlRemarkDomestic.Visible = false;
                    ctlRemarkForeign.Visible = true;



                    //if wait for verify then hide footer and hide add/delete button

                }
                #endregion

                #region Bind Memo Tab
                if (!string.IsNullOrEmpty(document.Memo))
                    ctlMemo.Text = document.Memo;
                else
                    ctlMemo.Text = string.Empty;
                #endregion

                #region viewPost
                if (VisibleFields.Contains(AdvanceFieldGroup.VerifyDetail))
                {
                    if (ctlAdvanceType.Text == AdvanceTypeEnum.Domestic)
                    {
                        #region Domestic
                        ctlPaymentMethodDomestic.DataSource = ScgDbQueryProvider.DbCompanyPaymentMethodQuery.GetPaymentMethod(UIHelper.ParseLong(ctlCompanyField.CompanyID));
                        ctlPaymentMethodDomestic.DataTextField = "PaymentMethodCode";
                        ctlPaymentMethodDomestic.DataValueField = "PaymentMethodID";

                        ctlPaymentMethodDomestic.DataBind();

                        if (ctlPaymentMethodDomestic.Items.Count > 0)
                        {
                            if (this.InitialFlag.Equals(FlagEnum.ViewFlag))
                                ctlPaymentMethodDomestic.Items.Insert(0, new ListItem(string.Empty, string.Empty));
                            else
                                ctlPaymentMethodDomestic.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
                        }
                        if (ParameterServices.EditableBusinessArea)
                        {
                            ctlBusinessAreaDomestic.Enabled = true;
                            ctlBusinessAreaDomestic.Style["display"] = "block";
                            ctlBusinessAreaDomesticText.Style["display"] = "block";
                            ctlBusinessAreaDomesticLabelExtender.Style["display"] = "block";
                        }
                        else
                        {
                            ctlBusinessAreaDomestic.Enabled = false;
                            ctlBusinessAreaDomestic.Style["display"] = "none";
                            ctlBusinessAreaDomesticText.Style["display"] = "none";
                            ctlBusinessAreaDomesticLabelExtender.Style["display"] = "none";
                        }
                        if (ParameterServices.EditableSupplementary)
                        {
                            ctlSupplementaryDomestic.Enabled = true;
                        }
                        else
                        {
                            ctlSupplementaryDomestic.Enabled = false;
                        }

                        if (string.IsNullOrEmpty(document.Supplementary) && !this.InitialFlag.Equals(FlagEnum.ViewFlag))
                        {
                            DefaultSupplementary();
                        }
                        else
                        {
                            ctlSupplementaryDomestic.Text = document.Supplementary;
                        }
                        if (string.IsNullOrEmpty(document.BusinessArea))
                        {
                            if (this.InitialFlag.Equals(FlagEnum.EditFlag))
                            {
                                DbCompany com = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(UIHelper.ParseLong(ctlCompanyField.CompanyID));
                                if (!string.IsNullOrEmpty(com.BusinessArea))
                                {
                                    ctlBusinessAreaDomestic.Text = com.BusinessArea;
                                }
                                else
                                {
                                    ctlBusinessAreaDomestic.Text = string.Empty;
                                }
                            }
                            else
                            {
                                ctlBusinessAreaDomestic.Text = string.Empty;
                            }
                        }
                        else
                        {
                            if (!document.IsBusinessAreaNull())
                                ctlBusinessAreaDomestic.Text = document.BusinessArea;
                        }
                        if (!string.IsNullOrEmpty(document.BranchCode.Trim()))
                            ctlBranchDomestic.Text = document.BranchCode;
                        else
                        {
                            if (this.InitialFlag.Equals(FlagEnum.EditFlag))
                                ctlBranchDomestic.Text = "0001"; //case edit must be default branch = 0001. modify by meaw. (log#1829)
                            else
                                ctlBranchDomestic.Text = string.Empty;
                        }

                        if (!document.IsPaymentMethodIDNull() && document.PaymentMethodID != 0)
                            ctlPaymentMethodDomestic.SelectedValue = document.PaymentMethodID.ToString();
                        else
                        {
                            if (ctlPaymentMethodDomestic.Items.Count > 0)
                            {
                                if (VisibleFields.Contains(AdvanceFieldGroup.VerifyDetail) && !this.InitialFlag.Equals(FlagEnum.ViewFlag))
                                {
                                    DbCompany comp = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(UIHelper.ParseLong(ctlCompanyField.CompanyID));
                                    if (comp != null)
                                    {
                                        switch (ctlPaymentType.SelectedValue)
                                        {
                                            case PaymentType.CA:
                                                if (comp.PaymentMethodPetty != null)
                                                    ctlPaymentMethodDomestic.SelectedValue = comp.PaymentMethodPetty.PaymentMethodID.ToString();
                                                break;

                                            case PaymentType.TR:
                                                if (comp.PaymentMethodTransfer != null)
                                                    ctlPaymentMethodDomestic.SelectedValue = comp.PaymentMethodTransfer.PaymentMethodID.ToString();
                                                break;

                                            case PaymentType.CQ:
                                                if (comp.PaymentMethodCheque != null)
                                                    ctlPaymentMethodDomestic.SelectedValue = comp.PaymentMethodCheque.PaymentMethodID.ToString();
                                                break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                ctlPaymentMethodDomestic.SelectedValue = string.Empty;
                            }
                        }

                        if (!document.PostingDate.Equals(DateTime.MinValue))
                            ctlPostingDateCalendarDomestic.DateValue = UIHelper.ToDateString(document.PostingDate);
                        else
                        {
                            if (this.InitialFlag.Equals(FlagEnum.EditFlag))
                                ctlPostingDateCalendarDomestic.DateValue = UIHelper.ToDateString(DateTime.Today);
                            else
                                ctlPostingDateCalendarDomestic.DateValue = string.Empty;
                        }

                        if (!document.BaseLineDate.Equals(DateTime.MinValue))
                            ctlBaselineDate.Text = UIHelper.ToDateString(document.BaseLineDate);
                        else
                        {
                            //modify by meaw. (log#1829)
                            if (this.InitialFlag.Equals(FlagEnum.EditFlag))
                                ctlBaselineDate.Text = UIHelper.ToDateString(advanceRow.RequestDateOfAdvance);
                            else
                                ctlBaselineDate.Text = string.Empty;
                        }

                        if (!string.IsNullOrEmpty(document.PostingStatus))
                            ctlPostingStatusDomestic.Text = GetMessage(string.Format("PostingStatus{0}", document.PostingStatus));
                        else
                            ctlPostingStatusDomestic.Text = GetMessage("PostingStatusN");

                        #endregion
                    }
                    else
                    {
                        #region Foreign
                        if (!string.IsNullOrEmpty(document.BranchCode.Trim()))
                            ctlBranchForeign.Text = document.BranchCode;
                        else
                        {
                            if (this.InitialFlag.Equals(FlagEnum.EditFlag))
                                ctlBranchForeign.Text = "0001";
                            else
                                ctlBranchForeign.Text = string.Empty;
                        }
                        if (!document.PostingDate.Equals(DateTime.MinValue))
                            ctlPostingDateForeign.DateValue = UIHelper.ToDateString(document.PostingDate);
                        else
                        {
                            if (this.InitialFlag.Equals(FlagEnum.EditFlag))
                                ctlPostingDateForeign.DateValue = UIHelper.ToDateString(DateTime.Today);
                            else
                                ctlPostingDateForeign.DateValue = string.Empty;
                        }
                        //else
                        //    ctlPostingDateForeign.DateValue = UIHelper.ToDateString(DateTime.Now);
                        if (!document.BaseLineDate.Equals(DateTime.MinValue))
                            ctlValueDateForeign.Text = UIHelper.ToDateString(document.BaseLineDate);
                        else  // modify by meaw.(log#1829)
                        {
                            if (this.InitialFlag.Equals(FlagEnum.EditFlag))
                                ctlValueDateForeign.Text = UIHelper.ToDateString(advanceRow.RequestDateOfAdvance);
                            else
                                ctlValueDateForeign.Text = string.Empty;
                        }

                        if (!string.IsNullOrEmpty(document.PostingStatus))
                            ctlPostingStatusForeign.Text = GetMessage("PostingStatus" + document.PostingStatus);
                        else
                            ctlPostingStatusForeign.Text = GetMessage("PostingStatusN");

                        if (!string.IsNullOrEmpty(document.BankAccount))
                            ctlBankAccountForeign.Text = document.BankAccount;
                        else
                        {
                            if (this.InitialFlag.Equals(FlagEnum.EditFlag))
                            {
                                if (!IsRepOffice)
                                    ctlBankAccountForeign.Text = ParameterServices.ADF_Default_BankAccount;
                            }
                        }

                        if (ParameterServices.EditableBusinessArea)
                        {
                            ctlBusinessAreaForeign.Enabled = true;
                            ctlBusinessAreaForeign.Style["display"] = "block";
                            ctlBusinessAreaForeignText.Style["display"] = "block";
                            ctlBusinessAreaForeignLabelExtender.Style["display"] = "block";
                        }
                        else
                        {
                            ctlBusinessAreaForeign.Enabled = false;
                            ctlBusinessAreaForeign.Style["display"] = "none";
                            ctlBusinessAreaForeignText.Style["display"] = "none";
                            ctlBusinessAreaForeignLabelExtender.Style["display"] = "none";
                        }

                        if (!document.IsBusinessAreaNull())
                            ctlBusinessAreaForeign.Text = document.BusinessArea;

                        if (string.IsNullOrEmpty(ctlBusinessAreaForeign.Text))
                        {
                            DbCompany com = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(UIHelper.ParseLong(ctlCompanyField.CompanyID));
                            ctlBusinessAreaForeign.Text = com.BusinessArea;
                        }

                        #endregion
                    }
                }
                #endregion
            }
            if (ctlAdvanceType.Text == AdvanceTypeEnum.Domestic)
            {
                CheckShowMessageOutstanding(InitialFlag);
                FixedAdvanceOutstandingGrid.DataCountAndBind();
            }
            ctlOutstandingGrid.DataCountAndBind();
            ctlUpdatePanelApprover.Update();
            hideCounterCashier();


            //this.BindLabelExtender();
            ctlUpdatePanelHeader.Update();
            ctlUpdatePanelGeneral.Update();
            ctlUpdatePanelApprover.Update();
            ctlUpdatePanelOutstanding.Update();
            ctlUpdatePanelInitial.Update();
            ctlUpdatePanelAttachment.Update();
            ctlUpdatePanelMemo.Update();
            ctlUpdatePanelViewPost.Update();
            ctlUpdatePanelValidation.Update();

        }
        #endregion

        #region RollBackTransaction()
        public void RollBackTransaction()
        {
            Guid txID = this.TransactionID;
            TransactionService.Rollback(txID);
            this.ResetControlValue();
        }
        #endregion

        #region ResetControlValue()
        private void ResetControlValue()
        {
            ctlAdvanceFormHeader.Status = string.Empty;
            ctlAdvanceFormHeader.No = string.Empty;
            ctlAdvanceFormHeader.CreateDate = string.Empty;
            ctlCompanyField.ShowDefault();
            ctlSubject.Text = string.Empty;

            ctlCreatorData.ShowDefault();
            ctlRequesterData.ShowDefault();
            ctlApproverData.ShowDefaultApprover(UIHelper.ParseLong(ctlRequesterData.UserID));

            ctlReceiverData.ShowDefault();
            ctlTANo.Text = string.Empty;
            ctlAmount.Text = "0.00";

            ctlRequestDateOfRemittance.DateValue = string.Empty;
            ctlDueDateOfRemittance.Text = string.Empty;
            ctlReason.Text = string.Empty;

            if (ctlAdvanceType.Text == AdvanceTypeEnum.International)
            {
                ctlCounterCashierInter.DataSource = ScgDbQueryProvider.DbPBQuery.GetPbListItem(UIHelper.ParseLong(ctlCompanyField.CompanyID), UserAccount.CurrentLanguageID);
                ctlCounterCashierInter.DataBind();
                if (ctlCounterCashierInter.Items.Count > 1)
                {
                    ctlCounterCashierInter.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
                }
                ctlRequestDateOfAdvanceForeign.DateValue = string.Empty;
                ctlArrivalDate.DateValue = string.Empty;
            }
            else //domestic
            {
                ctlServiceTeam.Items.Clear();

                ctlPaymentType.DataSource = ScgDbQueryProvider.SCGDbStatusLangQuery.FindStatusLangCriteria(documentTypeDMT, UserAccount.CurrentLanguageID);
                ctlPaymentType.DataTextField = "strSymbol";
                ctlPaymentType.DataValueField = "strID";
                ctlPaymentType.DataBind();




                if (ctlPaymentType.Items.Count > 1)
                {
                    ctlPaymentType.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
                }

                if (ctlPaymentType.SelectedValue.Equals(PaymentType.CA) || ctlPaymentType.SelectedValue.Equals(PaymentType.CQ))
                {
                    ctlCounterCashier.DataSource = ScgDbQueryProvider.DbPBQuery.GetPbListItem(UIHelper.ParseLong(ctlCompanyField.CompanyID), UserAccount.CurrentLanguageID);
                    ctlCounterCashier.DataBind();

                    if (ctlCounterCashier.Items.Count > 1)
                    {
                        ctlCounterCashier.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
                    }
                }

                ctlRequestDateOfAdvance.DateValue = string.Empty;
            }

            ctlAdvanceIntGrid.DataSource = null;
            ctlAdvanceIntGrid.DataBind();
            ctlExchangeRateForPerDiem.Text = string.Empty;

            ctlOutstandingGrid.DataSource = null;
            ctlOutstandingGrid.DataBind();
            if (ctlAdvanceType.Text.Equals(AdvanceTypeEnum.Domestic))
            {
                FixedAdvanceOutstandingGrid.DataSource = null;
                FixedAdvanceOutstandingGrid.DataBind();
            }

            //ctlInitiator

            ctlMemo.Text = string.Empty;

        }
        #endregion

        #region Call Lookup
        protected void ctlTALookup_OnObjectLookUpCalling(object sender, ObjectLookUpCallingEventArgs e)
        {
            UserControls.LOV.SCG.DB.TALookup taLookUp = sender as UserControls.LOV.SCG.DB.TALookup;
            if (taLookUp != null)
            {
                if (ctlAdvanceType.Text == AdvanceTypeEnum.International)
                    taLookUp.TravelBy = TravellBy.Foreign;
                else
                    taLookUp.TravelBy = TravellBy.Domestic;
                taLookUp.RequesterID = ctlRequesterData.UserID;
                taLookUp.CompanyID = ctlCompanyField.CompanyID;
            }
        }
        #region ctlTALookup_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        protected void ctlTALookup_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            this.isUpdatePanel = true;

            TADocumentObj taDoc = (TADocumentObj)e.ObjectReturn;
            if (taDoc != null)
            {
                ctlTANo.Text = taDoc.DocumentNo;
                ctlTAIdLookup.Text = taDoc.TADocumentID.Value.ToString();
                this.DocumentIDofTA = taDoc.DocumentID.Value;//เก็บ DocumentID ของ TA เพื่อเอาไป where ใน workFlow แล้วส่งค่าให้ DocumentView

                //default arrival date ด้วย
                if (taDoc.TravelBy == TravellBy.Foreign)
                {
                    ctlArrivalDate.DateValue = UIHelper.ToDateString(taDoc.ToDate);
                    ctlDueDateOfRemittance.Text = UIHelper.ToDateString(UIHelper.ParseDate(ctlArrivalDate.DateValue).Value.AddDays(dueDateOfRemit));
                    ctlRequestDateOfRemittance.DateValue = ctlDueDateOfRemittance.Text;
                }
                if (ctlTANo.Text.Trim().Equals(string.Empty))
                {
                    ctlNATaNo.Text = "N/A";
                    ctlTANo.Visible = false;
                    ctlNATaNo.Visible = true;
                }
                else
                {
                    ctlNATaNo.Visible = false;
                    ctlTANo.Visible = true;
                }
            }
            ctlUpdatePanelGeneral.Update();
        }
        #endregion

        #region ctlCompanyField_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        protected void ctlCompanyField_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            DbCompany company = (DbCompany)e.ObjectReturn;
            // company เปลี่ยนต้อง query Counter cashier ใหม่
            if (company != null)
            {
                CheckRepOffice();
                if (ctlAdvanceType.Text != AdvanceTypeEnum.International)
                {
                    if (company != null)
                    {
                        ctlCounterCashier.DataSource = ScgDbQueryProvider.DbPBQuery.GetPbListItem(company.CompanyID, UserAccount.CurrentLanguageID);
                        ctlCounterCashier.DataBind();
                        if (ctlCounterCashier.Items.Count > 1)
                        {
                            if (!ctlCounterCashier.Items[0].Value.Equals(string.Empty))
                            {
                                ctlCounterCashier.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
                            }
                        }
                        CheckShowMessageOutstanding(InitialFlag);
                        FixedAdvanceOutstandingGrid.DataCountAndBind();
                    }
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
                    ctlServiceTeam.DataSource = ScgDbQueryProvider.DbLocationQuery.FindByLocationCompany(UIHelper.ParseLong(ctlCompanyField.CompanyID), null, UserAccount.UserID, locationID);
                    ctlServiceTeam.DataBind();
                    if (ctlServiceTeam.Items.Count > 1)
                    {
                        ctlServiceTeam.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
                        ctlServiceTeam.SelectedIndex = 0;

                        IList<DbServiceTeamLocation> listLocation = ScgDbQueryProvider.DbServiceTeamLocationQuery.FindServiceTeamByLocationID(locationID);
                        if (listLocation != null && listLocation.Count > 0)
                            if (ctlServiceTeam.Items.FindByValue(listLocation.First<DbServiceTeamLocation>().ServiceTeamID.ServiceTeamID.ToString()) != null)
                                ctlServiceTeam.SelectedValue = listLocation.First<DbServiceTeamLocation>().ServiceTeamID.ServiceTeamID.ToString();

                    }
                    //-----------------
                }
                else
                {
                    if (company != null)
                    {
                        ctlCounterCashierInter.DataSource = ScgDbQueryProvider.DbPBQuery.GetPbListItem(company.CompanyID, UserAccount.CurrentLanguageID);
                        ctlCounterCashierInter.DataBind();
                        if (ctlCounterCashierInter.Items.Count > 1)
                        {
                            if (!ctlCounterCashierInter.Items[0].Value.Equals(string.Empty))
                            {
                                ctlCounterCashierInter.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
                            }
                        }
                    }
                }

                this.DefaultPb();
                if (IsRepOffice)
                {
                    ctlPaymentType.SelectedValue = PaymentType.CA;
                    BindCurrencyDropdown();
                }
                hideCounterCashier();
            }

            ctlOutstandingGrid.DataCountAndBind();
            ctlUpdatePanelGeneral.Update();
            ctlUpdatePanelOutstanding.Update();
            ctlUpdatePanelApprover.Update();
        }
        #endregion

        #region ctlRequesterData_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        protected void ctlRequesterData_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            SuUser user = (SuUser)e.ObjectReturn;
            if (user != null)
            {
                //company เปลี่ยนต้อง query service team ใหม่ 
                //ctlServiceTeam.DataSource = ScgDbQueryProvider.DbLocationQuery.FindByLocationCompany(UIHelper.ParseLong(ctlRequesterData.CompanyID), ctlRequesterData.CompanyCode, user.Userid);
                //ctlServiceTeam.DataBind();
                CheckRepOffice();
                this.DefaultPb();
                if (IsRepOffice)
                {
                    ctlPaymentType.SelectedValue = PaymentType.CA;
                    BindCurrencyDropdown();
                }

                hideCounterCashier();
                ctlReceiverData.SetValue(user.Userid);
                ctlApproverData.ShowDefaultApprover(UIHelper.ParseLong(ctlRequesterData.UserID));
                ctlInitiator.RequesterID = UIHelper.ParseLong(ctlRequesterData.UserID);
                if (ctlAdvanceType.Text == AdvanceTypeEnum.Domestic)
                {
                    BindServiceTeam();
                    CheckShowMessageOutstanding(InitialFlag);
                    FixedAdvanceOutstandingGrid.DataCountAndBind();
                }
            }
           
            ctlOutstandingGrid.DataCountAndBind();
            ctlUpdatePanelOutstanding.Update();
            ctlUpdatePanelGeneral.Update();
            ctlUpdatePanelApprover.Update();
        }
        protected void ctlReceiverData_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            CheckRequesterAndReceiverIsSamePerson();
        }
        #endregion

        #region ctlTANoLookup_Click(object sender, ImageClickEventArgs e)
        protected void ctlTANoLookup_Click(object sender, ImageClickEventArgs e)
        {
            ctlTALookup.Show();
            this.isUpdatePanel = false;
        }
        #endregion
        #endregion

        #region GridView Event
        #region Outstanding Advance Tab
        #region RequestDataOutstanding(int startRow, int pageSize, string sortExpression)
        public Object RequestDataOutstanding(int startRow, int pageSize, string sortExpression)
        {
            IList<int> iListate = new List<int>();
            iListate.Add(AdvanceStateID.Draft);
            iListate.Add(AdvanceStateID.Complete);
            iListate.Add(AdvanceStateID.Cancel);
            int intDocumentType = 0;
            long currentWorkFlowID = UIHelper.ParseLong(Request.Params["wfid"]);
            if (ctlAdvanceType.Text == AdvanceTypeEnum.Domestic)
            {
                intDocumentType = DocumentTypeID.AdvanceDomesticDocument;
                return ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindOutstandingRequest(UIHelper.ParseLong(ctlRequesterData.UserID), currentWorkFlowID, intDocumentType, DocumentTypeID.ExpenseDomesticDocument, iListate, UserAccount.CurrentLanguageID, startRow, pageSize, sortExpression);
            }
            else
            {
                intDocumentType = DocumentTypeID.AdvanceForeignDocument;
                return ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindOutstandingRequest(UIHelper.ParseLong(ctlRequesterData.UserID), currentWorkFlowID, intDocumentType, DocumentTypeID.ExpenseForeignDocument, iListate, UserAccount.CurrentLanguageID, startRow, pageSize, sortExpression);
            }

        }
        #endregion

        #region RequestCountOutstanding()
        public int RequestCountOutstanding()
        {
            IList<int> iListate = new List<int>();
            iListate.Add(AdvanceStateID.Draft);
            iListate.Add(AdvanceStateID.Complete);
            iListate.Add(AdvanceStateID.Cancel);
            int intDocumentType = 0;
            long currentWorkFlowID = UIHelper.ParseLong(Request.Params["wfid"]);

            if (ctlAdvanceType.Text == AdvanceTypeEnum.Domestic)
            {
                intDocumentType = DocumentTypeID.AdvanceDomesticDocument;
                return ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.CountFindOutstandingRequest(UIHelper.ParseLong(ctlRequesterData.UserID), currentWorkFlowID, intDocumentType, DocumentTypeID.ExpenseDomesticDocument, iListate, UserAccount.CurrentLanguageID);
            }
            else
            {
                intDocumentType = DocumentTypeID.AdvanceForeignDocument;
                return ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.CountFindOutstandingRequest(UIHelper.ParseLong(ctlRequesterData.UserID), currentWorkFlowID, intDocumentType, DocumentTypeID.ExpenseForeignDocument, iListate, UserAccount.CurrentLanguageID);
            }
        }
        #endregion

        #region ctlOutstandingGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        protected void ctlOutstandingGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal ctlNo = e.Row.FindControl("ctlNoText") as Literal;
                ctlNo.Text = ((ctlOutstandingGrid.PageSize * ctlOutstandingGrid.CustomPageIndex) + (e.Row.RowIndex + 1)).ToString();
            }
        }

        #endregion ctlOutstandingGrid_RowDataBound(object sender, GridViewRowEventArgs e)

        protected void ctlOutstandingGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ClickAdvanceNo" || e.CommandName == "ClickExpenseNo")
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                AvDocumentIDInGrid = UIHelper.ParseLong(ctlOutstandingGrid.DataKeys[rowIndex].Values["avDocumentID"].ToString());
                ExpenseDocumentIDInGrid = UIHelper.ParseLong(ctlOutstandingGrid.DataKeys[rowIndex].Values["expenseDocumentID"].ToString());

                SS.Standard.WorkFlow.DTO.WorkFlow workFlow = new SS.Standard.WorkFlow.DTO.WorkFlow();
                if (e.CommandName == "ClickAdvanceNo")
                {
                    workFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(AvDocumentIDInGrid);
                }
                else if (e.CommandName == "ClickExpenseNo")
                {
                    workFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(ExpenseDocumentIDInGrid);
                }
                long workFlowID;
                if (workFlow != null)
                    workFlowID = workFlow.WorkFlowID;
                else
                    workFlowID = 0;
                //popup Document View by WorkFlow
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), string.Empty, "window.open('../Programs/DocumentView.aspx?wfid=" + workFlowID.ToString() + "')", true);
            }
        }
        #endregion

        #region General Tab (Foreign)
        #region ctlAdvanceIntGrid_DataBound(object sender, EventArgs e)
        protected void ctlAdvanceIntGrid_DataBound(object sender, EventArgs e)
        {
            //ถ้าเป็น document status ตั้งแต่ Verify ขึ้นไป สามารถแก้ AmountTHB ได้
            //กรณที่ไม่ใช่ verify จะซ่อน column AmountTHB
            if (ctlAdvanceType.Text == AdvanceTypeEnum.Domestic || !VisibleFields.Contains(AdvanceFieldGroup.ExchangeRateForPerDiemCalculation))
            {
                ctlAdvanceIntGrid.Columns[3].Visible = false;
                ctlAdvanceIntGrid.Columns[5].Visible = false;
                ctlAdvanceIntGrid.Columns[4].Visible = false;
                ctlDivInternationalExchangeRateForPerDiem.Visible = false;
            }
            else
            {
                ctlAdvanceIntGrid.Columns[3].Visible = true;
                ctlAdvanceIntGrid.Columns[5].Visible = true;
                if (IsRepOffice)
                {
                    ctlAdvanceIntGrid.Columns[4].Visible = true;
                    if (!UserAccount.IsAccountant && !UserAccount.IsPayment)
                    {
                        ctlAdvanceIntGrid.Columns[5].Visible = false;
                    }
                    //ctlDivInternationalExchangeRateForPerDiem.Visible = false;
                }
                else
                {
                    ctlAdvanceIntGrid.Columns[4].Visible = false;
                    //ctlDivInternationalExchangeRateForPerDiem.Visible = true;
                }
                ctlDivInternationalExchangeRateForPerDiem.Visible = true;
            }

            string mainCurrencySymbol = string.Empty;
            AdvanceDataSet advanceDS = (AdvanceDataSet)TransactionService.GetDS(this.TransactionID);
            AdvanceDataSet.AvAdvanceDocumentRow advanceRow = advanceDS.AvAdvanceDocument.FindByAdvanceID(this.DocumentID);

            if (IsRepOffice)
            {
                if (!advanceRow.IsMainCurrencyIDNull())
                {
                    DbCurrency mainCurrency = SsDbQueryProvider.DbCurrencyQuery.FindByIdentity(advanceRow.MainCurrencyID);
                    if (mainCurrency != null)
                    {
                        mainCurrencySymbol = mainCurrency.Symbol;
                        ctlAdvanceIntGrid.Columns[4].HeaderText = string.Format(GetProgramMessage("AmountCurrency"), mainCurrency.Symbol);
                    }
                }
            }

            ctlExchangeRateForPerDiemText.Text = string.Format("Exchange Rate for per diem calculation ({0}/USD)", string.IsNullOrEmpty(mainCurrencySymbol) ? CurrencySymbol.THB.ToString() : mainCurrencySymbol);
            UserControls.DropdownList.SCG.DB.StatusDropdown ctlStatusDropdownFooter = ((ctlAdvanceIntGrid.Controls[0] as Table).Rows[2].FindControl("ctlPaymentTypeDropdown")) as UserControls.DropdownList.SCG.DB.StatusDropdown;
            UserControls.Dropdownlist.SS.DB.CurrencyDropdown ctlCurrencyFooter = ((ctlAdvanceIntGrid.Controls[0] as Table).Rows[2].FindControl("ctlCurrencyDropdown")) as UserControls.Dropdownlist.SS.DB.CurrencyDropdown;

            if (ctlStatusDropdownFooter != null)
            {
                ctlStatusDropdownFooter.GroupType = GroupStatus.PaymentTypeFRN;
                ctlStatusDropdownFooter.StatusBind();
                ctlStatusDropdownFooter.SetDropdown(string.Empty);
            }

            if (ctlCurrencyFooter != null)
            {
                //set default currency = USD
                ctlCurrencyFooter.BindCurrency((short)currencyUsd);
            }

            //case for verifier
            //check currency if currency usd not allow input perdiem else not usd allow put perdiem
            if (this.InitialFlag.Equals(FlagEnum.EditFlag))
            {
                if (VisibleFields.Contains(AdvanceFieldGroup.ExchangeRateForPerDiemCalculation))
                {
                    if (ctlAdvanceIntGrid.Rows.Count > 0)
                    {
                        foreach (GridViewRow dr in ctlAdvanceIntGrid.Rows)
                        {
                            if (dr.RowType == DataControlRowType.DataRow)
                            {
                                HiddenField ctlCurrencyID = dr.FindControl("ctlCurrencyID") as HiddenField;
                                if (ctlCurrencyID != null)
                                {
                                    if (currencyUsd.Equals(UIHelper.ParseInt(ctlCurrencyID.Value)))
                                    {
                                        ctlExchangeRateForPerDiem.Enabled = false;
                                        ctlExchangeRateForPerDiem.Text = "0.00000";
                                        break;
                                    }
                                    else
                                        ctlExchangeRateForPerDiem.Enabled = true;
                                }
                            }
                        }

                        if (IsRepOffice && !advanceRow.IsMainCurrencyIDNull() && advanceRow.MainCurrencyID == ParameterServices.USDCurrencyID)
                        {
                            ctlExchangeRateForPerDiem.Enabled = false;
                        }
                    }
                }
            }
        }
        #endregion

        #region ctlAdvanceIntGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        protected void ctlAdvanceIntGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Footer)
            {
                UserControls.DropdownList.SCG.DB.StatusDropdown ctlStatusDropdown = e.Row.FindControl("ctlPaymentTypeDropdown") as UserControls.DropdownList.SCG.DB.StatusDropdown;
                UserControls.Dropdownlist.SS.DB.CurrencyDropdown ctlCurrencyDropdown = e.Row.FindControl("ctlCurrencyDropdown") as UserControls.Dropdownlist.SS.DB.CurrencyDropdown;

                //Literal ctlPaymentTypeDropdownLabel = (Literal)(e.Row.FindControl("ctlPaymentTypeLabel"));
                UserControls.DropdownList.SCG.DB.StatusDropdown ctlPaymentTypeDDL = e.Row.FindControl("ctlPaymentTypeDDL") as UserControls.DropdownList.SCG.DB.StatusDropdown;
                Literal ctlCurrencyDropdowLabel = (Literal)(e.Row.FindControl("ctlCurrencyLabel"));

                HiddenField ctlPaymentTypeID = (HiddenField)(e.Row.FindControl("ctlPaymentTypeID"));
                HiddenField ctlCurrencyID = (HiddenField)(e.Row.FindControl("ctlCurrencyID"));
                Label ctlExchangeRate = (Label)(e.Row.FindControl("ctlExchangeRate"));
                TextBox ctlAmount = (TextBox)(e.Row.FindControl("ctlAmount"));
                SCG.eAccounting.Web.CustomControls.LabelExtender ctlAmountLabel = (SCG.eAccounting.Web.CustomControls.LabelExtender)(e.Row.FindControl("ctlCurrencyAmountExtender"));
                TextBox ctlAmountTHB = (TextBox)(e.Row.FindControl("ctlAmountTHB"));
                TextBox ctlAmountMain = (TextBox)(e.Row.FindControl("ctlAmountMain"));

                if (!IsRepOffice)
                {
                    ctlAmountTHB.ReadOnly = false;
                }
                else
                {
                    ctlAmountTHB.ReadOnly = true;
                    ctlAmountTHB.Style["background-color"] = "#CCCCCC";
                    ctlAmountTHB.BorderWidth = 0;
                }

                if ((CurrentStateName.Equals(WorkFlowStateFlag.Draft) || CurrentStateName.Equals(WorkFlowStateFlag.WaitVerify)) && InitialFlag.Equals(FlagEnum.EditFlag))
                {
                    AdvanceDataSet expDs = (AdvanceDataSet)TransactionService.GetDS(this.TransactionID);
                    //AdvanceDataSet.AvAdvanceItemRow advItem = (AdvanceDataSet.AvAdvanceItemRow)e.Row.DataItem;
                    AdvanceDataSet.AvAdvanceDocumentRow advDocumentRow = expDs.AvAdvanceDocument.FindByAdvanceID(this.DocumentID);

                    if (ctlCurrencyID != null && !advDocumentRow.IsMainCurrencyIDNull())
                    {
                        if (IsRepOffice && (UIHelper.ParseShort(ctlCurrencyID.Value) == advDocumentRow.MainCurrencyID))
                        {

                            if (string.IsNullOrEmpty(ctlAmountMain.Text) || UIHelper.ParseDouble(ctlAmountMain.Text) == 0)
                            {
                                ctlAmountMain.Text = UIHelper.BindDecimal(ctlAmount.Text);
                            }
                            ctlAmountMain.ReadOnly = true;
                            ctlAmountMain.Style["background-color"] = "#CCCCCC";
                            ctlAmountMain.BorderWidth = 0;
                        }
                        decimal exchangeRate = 0;
                        if (UIHelper.ParseDecimal(ctlAmount.Text) != 0)
                        {
                            exchangeRate = Math.Round((UIHelper.ParseDecimal(ctlAmountMain.Text) / UIHelper.ParseDecimal(ctlAmount.Text)), 5, MidpointRounding.AwayFromZero);
                        }
                        ctlExchangeRate.Text = UIHelper.BindExchangeRate(exchangeRate.ToString());

                        decimal amountThb = Math.Round((UIHelper.ParseDecimal(ctlAmountMain.Text) * UIHelper.ParseDecimal(ctlExchangeRateForeign.Text)), 2, MidpointRounding.AwayFromZero);
                        ctlAmountTHB.Text = UIHelper.BindDecimal(amountThb.ToString());
                    }

                    totalAmountMainCurrency += UIHelper.ParseDouble(ctlAmountMain.Text);
                    totalAmountTHB += UIHelper.ParseDouble(ctlAmountTHB.Text);
                }
                //คำนวนค่า exchange rate ก็ต่อเมื่อเป็น Verify ขึ้นไป
                if (VisibleFields.Contains(AdvanceFieldGroup.ExchangeRateForPerDiemCalculation))
                {
                    if (!IsRepOffice)
                    {
                        ctlAmount.Attributes.Remove("onblur");
                        ctlAmount.Attributes.Add("onblur", string.Format("calExchangeRate({0}, {1}, {2});", ctlAmount.ClientID, ctlAmountTHB.ClientID, ctlExchangeRate.ClientID));
                        ctlAmountTHB.Attributes.Remove("onblur");
                        if (e.Row.RowType == DataControlRowType.DataRow)
                        {
                            ctlAmountTHB.Attributes.Add("onblur", string.Format("calExchangeRate({0}, {1}, {2});", ctlAmountLabel.ClientID, ctlAmountTHB.ClientID, ctlExchangeRate.ClientID));
                        }
                        else
                        {
                            ctlAmountTHB.Attributes.Add("onblur", string.Format("calExchangeRate({0}, {1}, {2});", ctlAmount.ClientID, ctlAmountTHB.ClientID, ctlExchangeRate.ClientID));
                        }
                    }
                    else
                    {
                        ctlAmount.Attributes.Remove("onblur");
                        ctlAmount.Attributes.Add("onblur", string.Format("calExchangeRateForRepOffice({0},{1},{2},{3},{4});", ctlAmount.ClientID, ctlAmountMain.ClientID, ctlExchangeRate.ClientID, ctlExchangeRateForeign.ClientID, ctlAmountTHB.ClientID));
                        ctlAmountMain.Attributes.Remove("onblur");
                        if (e.Row.RowType == DataControlRowType.DataRow)
                        {
                            ctlAmountMain.Attributes.Add("onblur", string.Format("calExchangeRateForRepOffice({0},{1},{2},{3},{4});", ctlAmountLabel.ClientID, ctlAmountMain.ClientID, ctlExchangeRate.ClientID, ctlExchangeRateForeign.ClientID, ctlAmountTHB.ClientID));
                        }
                        else
                        {
                            ctlAmountMain.Attributes.Add("onblur", string.Format("calExchangeRateForRepOffice({0},{1},{2},{3},{4});", ctlAmount.ClientID, ctlAmountMain.ClientID, ctlExchangeRate.ClientID, ctlExchangeRateForeign.ClientID, ctlAmountTHB.ClientID));
                        }
                    }
                }
                if (ctlStatusDropdown != null)
                {
                    ctlStatusDropdown.GroupType = GroupStatus.PaymentTypeFRN;
                    ctlStatusDropdown.StatusBind();

                    if (ctlPaymentTypeID != null)
                    {
                        ctlPaymentTypeDDL.GroupType = GroupStatus.PaymentTypeFRN;
                        ctlPaymentTypeDDL.StatusBind();
                        ctlPaymentTypeDDL.SetDropdown(ctlPaymentTypeID.Value);
                    }
                }
                if (ctlPaymentTypeID != null)
                {
                    ctlPaymentTypeDDL.GroupType = GroupStatus.PaymentTypeFRN;
                    ctlPaymentTypeDDL.StatusBind();
                    ctlPaymentTypeDDL.SetDropdown(ctlPaymentTypeID.Value);
                }
                if (ctlCurrencyID != null)
                {
                    DbCurrency currency = SsDbQueryProvider.DbCurrencyQuery.FindByIdentity(UIHelper.ParseShort(ctlCurrencyID.Value));
                    IList<SS.DB.DTO.ValueObject.VOUCurrencySetup> currencyLang = SsDbQueryProvider.DbCurrencyLangQuery.FindCurrencyLangByCurrencyID(UIHelper.ParseLong(currency.CurrencyID.ToString()));
                    if (currency != null)
                    {
                        ctlCurrencyDropdowLabel.Text = currency.Symbol + " - " + currencyLang[0].Description;
                    }
                }
                if (ctlCurrencyDropdown != null)
                {
                    if (e.Row.RowType == DataControlRowType.Footer)
                        ctlCurrencyDropdown.BindCurrency((short)currencyUsd);
                    if (ctlCurrencyID != null)
                    {
                        ctlCurrencyDropdown.SelectedValue = ctlCurrencyID.Value;
                    }
                }


            }
        }
        #endregion

        #region ctlAdvanceIntGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        protected void ctlAdvanceIntGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string isNotInput = string.Empty;
            bool isDuplicate = false;
            if (e.CommandName.Equals("AddAdvance"))
            {
                //AdvanceDataSet advanceDs = (AdvanceDataSet)TransactionService.GetDS(this.TransactionID);
                //advanceDs.AvAdvanceItem.Clear();

                UserControls.DropdownList.SCG.DB.StatusDropdown ctlStatusDropdown;
                UserControls.Dropdownlist.SS.DB.CurrencyDropdown ctlCurrencyDropdown;
                TextBox ctlAmount;
                Label ctlExchangeRate;
                TextBox ctlAmountTHB;

                if (ctlAdvanceIntGrid.Rows.Count == 0)
                {
                    ctlStatusDropdown = (UserControls.DropdownList.SCG.DB.StatusDropdown)((ctlAdvanceIntGrid.Controls[0] as Table).Rows[2].FindControl("ctlPaymentTypeDropdown"));
                    ctlCurrencyDropdown = (UserControls.Dropdownlist.SS.DB.CurrencyDropdown)((ctlAdvanceIntGrid.Controls[0] as Table).Rows[2].FindControl("ctlCurrencyDropdown"));
                    ctlAmount = (TextBox)((ctlAdvanceIntGrid.Controls[0] as Table).Rows[2].FindControl("ctlAmount"));
                    ctlExchangeRate = (Label)((ctlAdvanceIntGrid.Controls[0] as Table).Rows[2].FindControl("ctlExchangeRate"));
                    ctlAmountTHB = (TextBox)((ctlAdvanceIntGrid.Controls[0] as Table).Rows[2].FindControl("ctlAmountTHB"));
                }
                else
                {
                    ctlStatusDropdown = (UserControls.DropdownList.SCG.DB.StatusDropdown)ctlAdvanceIntGrid.FooterRow.FindControl("ctlPaymentTypeDropdown");
                    ctlCurrencyDropdown = (UserControls.Dropdownlist.SS.DB.CurrencyDropdown)ctlAdvanceIntGrid.FooterRow.FindControl("ctlCurrencyDropdown");
                    ctlAmount = (TextBox)ctlAdvanceIntGrid.FooterRow.FindControl("ctlAmount");
                    ctlExchangeRate = (Label)ctlAdvanceIntGrid.FooterRow.FindControl("ctlExchangeRate");
                    ctlAmountTHB = (TextBox)ctlAdvanceIntGrid.FooterRow.FindControl("ctlAmountTHB");
                }
                if (VisibleFields.Contains(AdvanceFieldGroup.ExchangeRateForPerDiemCalculation))
                {
                    ctlAmount.Attributes.Remove("onblur");
                    ctlAmountTHB.Attributes.Remove("onblur");
                    ctlAmount.Attributes.Add("onblur", string.Format("calExchangeRate({0}, {1}, {2});", ctlAmount.ClientID, ctlAmountTHB.ClientID, ctlExchangeRate.ClientID));
                    ctlAmountTHB.Attributes.Add("onblur", string.Format("calExchangeRate({0}, {1}, {2});", ctlAmount.ClientID, ctlAmountTHB.ClientID, ctlExchangeRate.ClientID));
                }
                //check ค่าว่าง
                if (ctlAmount.Text.Equals(string.Empty) || UIHelper.ParseDouble(ctlAmount.Text) == 0)
                {
                    //กรณี status = 'New' จะไม่มีการ input exchangerate and amount thb
                    isNotInput = "isNotInput";

                }
                //check ค่าว่างกรณีที่สามารถแก้ไขข้อมูลของ amount thb and exchange rate
                if (VisibleFields.Contains(AdvanceFieldGroup.ExchangeRateForPerDiemCalculation))
                {
                    if (ctlAmount.Text.Equals(string.Empty) && ctlExchangeRate.Text.Equals(string.Empty) && ctlAmountTHB.Text.Equals(string.Empty))
                    {
                        isNotInput = "isNotInput";
                    }
                }
                // check duplicate key
                foreach (GridViewRow dr in ctlAdvanceIntGrid.Rows)
                {
                    if (dr.RowType == DataControlRowType.DataRow)
                    {
                        TextBox ctlAmountUpdate = dr.FindControl("ctlAmount") as TextBox;
                        TextBox ctlAmountTHBUpdate = dr.FindControl("ctlAmountTHB") as TextBox;
                        Label ctlExchangeRateUpdate = dr.FindControl("ctlExchangeRate") as Label;


                        HiddenField ctlPaymentTypeID = dr.FindControl("ctlPaymentTypeID") as HiddenField;
                        HiddenField ctlCurrencyID = dr.FindControl("ctlCurrencyID") as HiddenField;
                        UserControls.DropdownList.SCG.DB.StatusDropdown ctlPaymentTypeDropdown = dr.FindControl("ctlPaymentTypeDropdown") as UserControls.DropdownList.SCG.DB.StatusDropdown;
                        if (ctlPaymentTypeID.Value == ctlStatusDropdown.SelectedValue &&
                            ctlCurrencyID.Value == ctlCurrencyDropdown.SelectedValue)
                        {
                            isDuplicate = true;
                        }
                        long advanceItemID = UIHelper.ParseLong(ctlAdvanceIntGrid.DataKeys[dr.RowIndex].Values["AdvanceItemID"].ToString());

                        AvAdvanceItem advanceItem = new AvAdvanceItem();
                        advanceItem.AdvanceItemID = advanceItemID;
                        advanceItem.Amount = UIHelper.ParseDouble(ctlAmountUpdate.Text);
                        advanceItem.AmountTHB = UIHelper.ParseDouble(ctlAmountTHBUpdate.Text);
                        advanceItem.ExchangeRate = UIHelper.ParseDouble(ctlExchangeRateUpdate.Text);

                        AvAdvanceItemService.updateGrid(this.TransactionID, advanceItem);
                    }
                }

                if (!isDuplicate && !isNotInput.Equals("isNotInput"))
                {
                    AdvanceDataSet advanceDs = (AdvanceDataSet)TransactionService.GetDS(this.TransactionID);
                    AdvanceDataSet.AvAdvanceItemDataTable table = advanceDs.AvAdvanceItem;
                    AvAdvanceItem avAdvanceItem = new AvAdvanceItem();
                    avAdvanceItem.AdvanceID = new AvAdvanceDocument(this.DocumentID);
                    avAdvanceItem.PaymentType = ctlStatusDropdown.SelectedValue;
                    avAdvanceItem.CurrencyID = new DbCurrency(UIHelper.ParseShort(ctlCurrencyDropdown.SelectedValue));
                    avAdvanceItem.Amount = UIHelper.ParseDouble(ctlAmount.Text);
                    avAdvanceItem.ExchangeRate = UIHelper.ParseDouble(ctlExchangeRate.Text);
                    avAdvanceItem.AmountTHB = UIHelper.ParseDouble(ctlAmountTHB.Text);
                    avAdvanceItem.Active = true;
                    avAdvanceItem.CreBy = UserAccount.UserID;
                    avAdvanceItem.CreDate = DateTime.Now;
                    avAdvanceItem.UpdBy = UserAccount.UserID;
                    avAdvanceItem.UpdDate = DateTime.Now;
                    avAdvanceItem.UpdPgm = ProgramCode;

                    AvAdvanceItemService.AddAdvanceItem(this.TransactionID, avAdvanceItem);

                    ctlAdvanceIntGrid.DataSource = table;
                    ctlAdvanceIntGrid.DataBind();
                }
                else
                {
                    if (isDuplicate)
                    {
                        this.ValidationErrors.AddError("Provider2.Error", new Spring.Validation.ErrorMessage("AdvanceItemIsDuplicate"));
                    }
                    if (isNotInput.Equals("isNotInput"))
                    {
                        this.ValidationErrors.AddError("Provider2.Error", new Spring.Validation.ErrorMessage("AmountIsRequire"));
                    }

                }

                #region Update ExchangeRateForPerDiem

                double douAmount = 0;
                double douAmountThbUpdate = 0;
                double douAmountMainUpdate = 0;
                double ExchangeRateForPerDiem = 0;

                foreach (GridViewRow dr in ctlAdvanceIntGrid.Rows)
                {

                    if (dr.RowType == DataControlRowType.DataRow)
                    {
                        TextBox ctlAmountUpdate = dr.FindControl("ctlAmount") as TextBox;
                        TextBox ctlAmountTHBUpdate = dr.FindControl("ctlAmountTHB") as TextBox;
                        TextBox ctlAmountMainUpdate = dr.FindControl("ctlAmountMain") as TextBox;
                        Label ctlExchangeRateUpdate = dr.FindControl("ctlExchangeRate") as Label;
                        HiddenField ctlCurrencyID = dr.FindControl("ctlCurrencyID") as HiddenField;

                        if (ctlCurrencyID.Value == ParameterServices.USDCurrencyID.ToString())
                        {
                            douAmountThbUpdate += UIHelper.ParseDouble(ctlAmountTHBUpdate.Text);
                            douAmount += UIHelper.ParseDouble(ctlAmountUpdate.Text);
                            douAmountMainUpdate += UIHelper.ParseDouble(ctlAmountMainUpdate.Text);
                        }

                    }
                }

                if (!IsRepOffice)
                {
                    if (douAmount > 0)
                    {
                        ExchangeRateForPerDiem = (double)Math.Round((decimal)(douAmountThbUpdate / douAmount));
                        ctlExchangeRateForPerDiem.Text = string.Format("{0:#,##0.00000}", ExchangeRateForPerDiem);
                    }
                    else
                    {
                        ctlExchangeRateForPerDiem.Text = "0.00000";
                    }
                }
                else
                {
                    AdvanceDataSet advanceDs = (AdvanceDataSet)TransactionService.GetDS(this.TransactionID);
                    AdvanceDataSet.AvAdvanceDocumentRow advanceRow = advanceDs.AvAdvanceDocument.FindByAdvanceID(this.DocumentID);
                    if (!advanceRow.IsMainCurrencyIDNull() && advanceRow.MainCurrencyID != ParameterServices.USDCurrencyID)
                    {
                        if (douAmount > 0)
                        {
                            ExchangeRateForPerDiem = (double)Math.Round((decimal)(douAmountMainUpdate / douAmount));
                            ctlExchangeRateForPerDiem.Text = string.Format("{0:#,##0.00000}", ExchangeRateForPerDiem);
                        }
                        else
                        {
                            ctlExchangeRateForPerDiem.Text = "0.00000";
                        }
                    }
                    else
                    {
                        ExchangeRateForPerDiem = 1;
                        ctlExchangeRateForPerDiem.Text = string.Format("{0:#,##0.00000}", ExchangeRateForPerDiem);
                    }
                }

                ctlUpdatePanelValidation.Update();
                ctlUpdatePanelViewPost.Update();
                ctlUpdatePanelGeneral.Update();
                #endregion Update ExchangeRateForPerDiem

                #region
                /*
                AdvanceDataSet.AvAdvanceItemDataTable table = advanceDs.AvAdvanceItem;
                try
                {
                    foreach (GridViewRow row in ctlAdvanceIntGrid.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            UserControls.DropdownList.SCG.DB.StatusDropdown rowPayment = (UserControls.DropdownList.SCG.DB.StatusDropdown)ctlAdvanceIntGrid.Rows[row.RowIndex].FindControl("ctlPaymentTypeDropdown");
                            UserControls.Dropdownlist.SS.DB.CurrencyDropdown rowCurrency = (UserControls.Dropdownlist.SS.DB.CurrencyDropdown)ctlAdvanceIntGrid.Rows[row.RowIndex].FindControl("ctlCurrencyDropdown");
                            TextBox rowAmount = (TextBox)ctlAdvanceIntGrid.Rows[row.RowIndex].FindControl("ctlAmount");
                            TextBox rowExchangeRate = (TextBox)ctlAdvanceIntGrid.Rows[row.RowIndex].FindControl("ctlExchangeRate");
                            TextBox rowAmountTHB = (TextBox)ctlAdvanceIntGrid.Rows[row.RowIndex].FindControl("ctlAmountTHB");
                            //if (table.Rows.Count > 0)
                            //{
                                DataRow[] dr = table.Select("PaymentType='" + rowPayment.SelectedValue + "' AND CurrencyID='" + rowCurrency.SelectedValue + "'");
                                if (dr.Length == 0 && !isNotInput)
                                {
                                    AvAdvanceItem avAdvanceItem = new AvAdvanceItem();
                                    avAdvanceItem.AdvanceID = new AvAdvanceDocument(this.DocumentID);
                                    avAdvanceItem.PaymentType = rowPayment.SelectedValue;
                                    avAdvanceItem.CurrencyID = new DbCurrency(UIHelper.ParseShort(rowCurrency.SelectedValue));
                                    avAdvanceItem.Amount = UIHelper.ParseDouble(rowAmount.Text);
                                    avAdvanceItem.ExchangeRate = UIHelper.ParseDouble(rowExchangeRate.Text);
                                    avAdvanceItem.AmountTHB = UIHelper.ParseDouble(rowAmountTHB.Text);
                                    avAdvanceItem.Active = true;
                                    avAdvanceItem.CreBy = UserAccount.UserID;
                                    avAdvanceItem.CreDate = DateTime.Now;
                                    avAdvanceItem.UpdBy = UserAccount.UserID;
                                    avAdvanceItem.UpdDate = DateTime.Now;
                                    avAdvanceItem.UpdPgm = ProgramCode;

                                    AvAdvanceItemService.AddAdvanceItem(this.TransactionID, avAdvanceItem);
                                }
                                else
                                    break;
                            //}
                        }
                    }
                }
                catch(Exception ex) 
                { throw ex; }
                #region add into DataSet

                if ((ctlAdvanceIntGrid.Controls[0] as Table).Rows.Count == 3 || ctlAdvanceIntGrid.Rows.Count > 0)
                {
                    DataRow[] dr = table.Select("PaymentType='" + ctlStatusDropdown.SelectedValue + "' AND CurrencyID='" + ctlCurrencyDropdown.SelectedValue + "'");
                    if (dr.Length == 0 && !isNotInput)
                     {
                         AvAdvanceItem avAdvanceItem = new AvAdvanceItem();
                         avAdvanceItem.AdvanceID = new AvAdvanceDocument(this.DocumentID);
                         avAdvanceItem.PaymentType = ctlStatusDropdown.SelectedValue;
                         avAdvanceItem.CurrencyID = new DbCurrency(UIHelper.ParseShort(ctlCurrencyDropdown.SelectedValue));
                         avAdvanceItem.Amount = UIHelper.ParseDouble(ctlAmount.Text);
                         avAdvanceItem.ExchangeRate = UIHelper.ParseDouble(ctlExchangeRate.Text);
                         avAdvanceItem.AmountTHB = UIHelper.ParseDouble(ctlAmountTHB.Text);
                         avAdvanceItem.Active = true;
                         avAdvanceItem.CreBy = UserAccount.UserID;
                         avAdvanceItem.CreDate = DateTime.Now;
                         avAdvanceItem.UpdBy = UserAccount.UserID;
                         avAdvanceItem.UpdDate = DateTime.Now;
                         avAdvanceItem.UpdPgm = ProgramCode;

                         AvAdvanceItemService.AddAdvanceItem(this.TransactionID, avAdvanceItem);

                         ctlAdvanceIntGrid.DataSource = table;
                         ctlAdvanceIntGrid.DataBind();
                     }
                }
                #endregion*/
                #endregion
            }
            else if (e.CommandName.Equals("DeleteAdvance"))
            {
                foreach (GridViewRow dr in ctlAdvanceIntGrid.Rows)
                {
                    if (dr.RowType == DataControlRowType.DataRow)
                    {
                        TextBox ctlAmountUpdate = dr.FindControl("ctlAmount") as TextBox;
                        TextBox ctlAmountTHBUpdate = dr.FindControl("ctlAmountTHB") as TextBox;
                        Label ctlExchangeRateUpdate = dr.FindControl("ctlExchangeRate") as Label;
                        long advanceItemIDDK = UIHelper.ParseLong(ctlAdvanceIntGrid.DataKeys[dr.RowIndex].Values["AdvanceItemID"].ToString());

                        AvAdvanceItem advanceItem = new AvAdvanceItem();
                        advanceItem.AdvanceItemID = advanceItemIDDK;
                        advanceItem.Amount = UIHelper.ParseDouble(ctlAmountUpdate.Text);
                        advanceItem.AmountTHB = UIHelper.ParseDouble(ctlAmountTHBUpdate.Text);
                        if (ctlExchangeRateUpdate != null)
                            advanceItem.ExchangeRate = UIHelper.ParseDouble(ctlExchangeRateUpdate.Text);

                        AvAdvanceItemService.updateGrid(this.TransactionID, advanceItem);
                    }
                }
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                long advanceItemID = UIHelper.ParseLong(ctlAdvanceIntGrid.DataKeys[rowIndex].Value.ToString());
                AdvanceDataSet.AvAdvanceItemDataTable table = (AdvanceDataSet.AvAdvanceItemDataTable)AvAdvanceItemService.DeleteAdvanceItemFromTransaction(this.TransactionID, advanceItemID);

                ctlAdvanceIntGrid.DataSource = table;
                ctlAdvanceIntGrid.DataBind();
                ctlUpdatePanelGeneral.Update();

                #region Update ExchangeRateForPerDiem

                double douAmountThb = 0;
                double douAmountThbUpdate = 0;
                double ExchangeRateForPerDiem = 0;

                foreach (GridViewRow dr in ctlAdvanceIntGrid.Rows)
                {

                    if (dr.RowType == DataControlRowType.DataRow)
                    {
                        TextBox ctlAmountUpdate = dr.FindControl("ctlAmount") as TextBox;
                        TextBox ctlAmountTHBUpdate = dr.FindControl("ctlAmountTHB") as TextBox;
                        Label ctlExchangeRateUpdate = dr.FindControl("ctlExchangeRate") as Label;
                        HiddenField ctlCurrencyID = dr.FindControl("ctlCurrencyID") as HiddenField;

                        if (ctlCurrencyID.Value == "34")
                        {
                            douAmountThbUpdate += UIHelper.ParseDouble(ctlAmountTHBUpdate.Text);
                            douAmountThb += UIHelper.ParseDouble(ctlAmountUpdate.Text);
                        }


                    }
                }
                if (douAmountThb > 0)
                {
                    ExchangeRateForPerDiem = (double)Math.Round((decimal)(douAmountThbUpdate / douAmountThb));
                    ctlExchangeRateForPerDiem.Text = string.Format("{0:#,##0.00000}", ExchangeRateForPerDiem);
                }
                else
                {
                    ctlExchangeRateForPerDiem.Text = "0.00000";
                }

                ctlUpdatePanelValidation.Update();
                ctlUpdatePanelViewPost.Update();
                ctlUpdatePanelGeneral.Update();
                #endregion Update ExchangeRateForPerDiem
            }
        }

        #endregion
        #endregion General Tab (Foreign)
        #endregion

        #region BindTabGeneral()
        private void BindTabGeneral()
        {
            short languageID = UserAccount.CurrentLanguageID;

            //if ((!string.IsNullOrEmpty(this.AdvanceType)) && (this.AdvanceType.Equals(AdvanceTypeEnum.International)))
            if (ctlAdvanceType.Text == AdvanceTypeEnum.International)
            {
                ctlDivInternationalGrid.Visible = true;
                ctlDivInternationalArrivalDate.Visible = true;
                if (!IsRepOffice)
                {
                    ctlDivInternationalExchangeRateForPerDiem.Visible = true;
                }
                else
                {
                    AdvanceDataSet advanceDS = (AdvanceDataSet)TransactionService.GetDS(this.TransactionID);
                    AdvanceDataSet.AvAdvanceDocumentRow advanceRow = advanceDS.AvAdvanceDocument.FindByAdvanceID(this.DocumentID);

                    if (!advanceRow.IsMainCurrencyAmountNull())
                    {
                        if (advanceRow.MainCurrencyID == currencyUsd)
                            ctlDivInternationalExchangeRateForPerDiem.Visible = false;
                    }
                }
                ctlDivInterCounterCashier.Visible = true;
                ctlDivViewDetailForeign.Visible = true;
                ctlDivViewDetailDomestic.Visible = false;
                ctlDivDomesticPaymentType.Visible = false;
                ctlDivDomesticAmount.Visible = false;

                ctlDivExchangeRate1.Style.Add("display", "none"); //Main Exchange Rate 
                ctlDivExchangeRate2.Style.Add("display", "none"); //THB Exchange Rate

                if (!Page.IsPostBack)
                {
                    #region unuse
                    //ในการ select ค่าของ service team ต้องเอา ค่า ของ location ไป  where ด้วย ในกรณีที่ company of document = company of requester else ไม่ต้อง  where
                    //ถ้ามีค่ามากกว่า 1 rows ให้ขึ้นคำว่า please select แต่มี 1 rows ให้ default เลย
                    //long locationID = 0;
                    //if (ctlCompanyField.CompanyID == ctlRequesterData.CompanyID)
                    //{
                    //    SuUser userList = QueryProvider.SuUserQuery.FindByIdentity(UIHelper.ParseLong(ctlRequesterData.UserID));
                    //    if (userList != null)
                    //    {
                    //        if (userList.Location != null)
                    //            locationID = userList.Location.LocationID;
                    //    }
                    //}
                    //ctlServiceTeam.DataSource = ScgDbQueryProvider.DbLocationQuery.FindByLocationCompany(UIHelper.ParseLong(ctlCompanyField.CompanyID), null, UserAccount.UserID, locationID);
                    //ctlServiceTeam.DataBind();
                    //if (ctlServiceTeam.Items.Count > 1 && !ctlServiceTeam.Items[0].Value.Equals(string.Empty))
                    //    ctlServiceTeam.Items.Insert(0, new ListItem("--Please Select--", string.Empty));
                    //---------------------------
                    #endregion

                    ctlCounterCashierInter.DataSource = ScgDbQueryProvider.DbPBQuery.GetPbListItem(UIHelper.ParseLong(ctlCompanyField.CompanyID), UserAccount.CurrentLanguageID);
                    ctlCounterCashierInter.DataBind();
                    if (ctlCounterCashierInter.Items.Count > 1)
                    {
                        ctlCounterCashierInter.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
                    }

                    if (InitialFlag.Equals(FlagEnum.NewFlag))
                    {
                        this.DefaultPb();
                    }
                    ctlTotalLabel.Style.Add("display", "none");
                    ctlTotalAmountMainLabel.Style.Add("display", "none");
                    ctlTotalAmountTHBLabel.Style.Add("display", "none");

                    ctlExchangeRateTextLabel.Style.Add("display", "none");
                    ctlExchangeRateForeign.Style.Add("display", "none");
                    ctlExchangeRateCurrencyLabel.Style.Add("display", "none");

                    ctlExchangeRateTextLabel2.Style.Add("display", "none");
                    ctlExchangeRateForeign2.Style.Add("display", "none");
                    ctlExchangeRateCurrencyLabel2.Style.Add("display", "none");

                    ctlDivExchangeRate1.Style.Add("display", "none"); //Main Exchange Rate 
                    ctlDivExchangeRate2.Style.Add("display", "none"); //THB Exchange Rate

                    //hide verify detail
                    if (VisibleFields.Contains(AdvanceFieldGroup.VerifyDetail))
                    {
                        ctlDivViewDetailDomestic.Visible = false;
                        ctlDivViewDetailForeign.Visible = true;

                        if (IsRepOffice)
                        {
                            divBankAccountLabel.Style.Add("display", "none");
                            divBankAccountTextBox.Style.Add("display", "none");

                            if (UserAccount.IsAccountant || UserAccount.IsPayment)
                            {
                                ctlTotalLabel.Style.Add("display", "block");
                                ctlTotalAmountMainLabel.Style.Add("display", "block");
                                ctlTotalAmountTHBLabel.Style.Add("display", "block");

                                ctlExchangeRateTextLabel.Style.Add("display", "block");
                                ctlExchangeRateForeign.Style.Add("display", "block");
                                ctlExchangeRateCurrencyLabel.Style.Add("display", "block");

                                ctlExchangeRateTextLabel2.Style.Add("display", "block");
                                ctlExchangeRateForeign2.Style.Add("display", "block");
                                ctlExchangeRateCurrencyLabel2.Style.Add("display", "block");
                            }
                        }
                    }
                    else
                    {
                        divBankAccountLabel.Style.Add("display", "block");
                        divBankAccountTextBox.Style.Add("display", "block");

                        ctlDivViewDetailForeign.Visible = false;

                        ctlTotalLabel.Style.Add("display", "none");
                        ctlTotalAmountMainLabel.Style.Add("display", "none");
                        ctlTotalAmountTHBLabel.Style.Add("display", "none");

                        ctlExchangeRateTextLabel.Style.Add("display", "none");
                        ctlExchangeRateForeign.Style.Add("display", "none");
                        ctlExchangeRateCurrencyLabel.Style.Add("display", "none");

                        ctlExchangeRateTextLabel2.Style.Add("display", "none");
                        ctlExchangeRateForeign2.Style.Add("display", "none");
                        ctlExchangeRateCurrencyLabel2.Style.Add("display", "none");
                    }

                    ctlRemarkDomestic.Visible = false;
                    ctlRemarkForeign.Visible = true;

                    ctlRequestDateOfAdvance.Visible = false;//for domestic
                    //for foreign ต่างกันที่ว่า ถ้าเป็นของ domestic Request of av ต้องเอา request of av + 14 = duedate
                    //แต่ถ้าเป็นของ foreign จะเอา arrival + 14 = duedate
                    ctlRequestDateOfAdvanceForeign.Visible = true;
                }
                ctlAdvanceIntGrid.DataBind();
            }
            else
            {
                ctlDivDomesticPaymentType.Visible = true;
                ctlDivDomesticAmount.Visible = true;
                ctlDivViewDetailDomestic.Visible = true;

                ctlDivViewDetailForeign.Visible = false;

                ctlDivInterCounterCashier.Visible = false;
                ctlDivInternationalGrid.Visible = false;
                ctlDivInternationalArrivalDate.Visible = false;
                ctlDivInternationalExchangeRateForPerDiem.Visible = false;


                if (!Page.IsPostBack)
                {
                    ctlPaymentType.DataSource = ScgDbQueryProvider.SCGDbStatusLangQuery.FindStatusLangCriteria(documentTypeDMT, languageID);
                    ctlPaymentType.DataTextField = "strSymbol";
                    ctlPaymentType.DataValueField = "strID";
                    //ในการ select ค่าของ service team ต้องเอา ค่า ของ location ไป  where ด้วย ในกรณีที่ company of document = company of requester else ไม่ต้อง  where
                    //ถ้ามีค่ามากกว่า 1 rows ให้ขึ้นคำว่า please select แต่มี 1 rows ให้ default เลย

                    if (!this.InitialFlag.Equals(FlagEnum.ViewFlag))
                    {
                        BindServiceTeam();
                    }
                    //-----------------
                    if (Request.Params["wfid"] == null)
                    {
                        ctlCounterCashier.DataSource = ScgDbQueryProvider.DbPBQuery.GetPbListItem(UIHelper.ParseLong(ctlCompanyField.CompanyID), UserAccount.CurrentLanguageID);
                        ctlCounterCashier.DataBind();
                        if (ctlCounterCashier.Items.Count > 1)
                        {
                            ctlCounterCashier.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
                        }
                    }
                    ctlPaymentType.DataBind();
                    ctlPaymentType.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
                    //ctlDivAmountTotalTHB.Style.Add("display", "none");
                    ctlAmounTHBLabel.Style.Add("display", "none");
                    ctlAmountTotalTHBLabel.Style.Add("display", "none");
                    ctlAmountTHBCurrencyLabel.Style.Add("display", "none");

                    ctlDivExchangeRate1.Style.Add("display", "none"); //Main Exchange Rate 
                    ctlDivExchangeRate2.Style.Add("display", "none"); //THB Exchange Rate

                    if (InitialFlag.Equals(FlagEnum.NewFlag))
                    {
                        this.DefaultPb();
                    }

                    if (IsRepOffice)
                    {
                        ctlPaymentType.SelectedValue = PaymentType.CA;
                        BindCurrencyDropdown();
                    }

                    //hide verify detail
                    if (VisibleFields.Contains(AdvanceFieldGroup.VerifyDetail))
                    {
                        ctlDivViewDetailDomestic.Visible = true;
                        ctlDivViewDetailForeign.Visible = false;
                        if (IsRepOffice && (UserAccount.IsAccountant || UserAccount.IsPayment))
                        {
                            //ctlDivAmountTotalTHB.Style.Add("display", "block");
                            ctlAmounTHBLabel.Style.Add("display", "block");
                            ctlAmountTotalTHBLabel.Style.Add("display", "block");
                            ctlAmountTHBCurrencyLabel.Style.Add("display", "block");

                            ctlDivExchangeRate1.Style.Add("display", "block"); //Main Exchange Rate 
                            ctlDivExchangeRate2.Style.Add("display", "block"); //THB Exchange Rate
                        }
                    }
                    else
                    {
                        ctlDivViewDetailDomestic.Visible = false;
                    }
                    ctlRemarkDomestic.Visible = true;
                    ctlRemarkForeign.Visible = false;

                    ctlRequestDateOfAdvance.Visible = true;//for domestic
                    //for foreign ต่างกันที่ว่า ถ้าเป็นของ domestic Request of av ต้องเอา request of av + 14 = duedate
                    //แต่ถ้าเป็นของ foreign จะเอา arrival + 14 = duedate
                    ctlRequestDateOfAdvanceForeign.Visible = false;
                }
                hideCounterCashier();
            }
            ctlUpdatePanelViewPost.Update();
            ctlUpdatePanelGeneral.Update();
        }
        #endregion
        #region BindTabInitial()
        private void BindTabInitial()
        {
            //ctlInitiator.DocumentID = this.DocumentID;
            //ctlInitiator.BindControl();
            ctlUpdatePanelInitial.Update();
        }
        #endregion
        #region BindTabAttachment()
        private void BindTabAttachment()
        {
            //ctlAttachment.pathName = "~/ImageFiles";
            //ctlAttachment.docId = this.DocumentID;
            ctlAttachment.BindControl();
            ctlUpdatePanelAttachment.Update();
        }
        #endregion
        #region BindTabMemo()
        private void BindTabMemo()
        {
            ctlUpdatePanelMemo.Update();
        }
        #endregion

        #region hide counter cashier when payment type = "เงินโอน"
        private void hideCounterCashier()
        {
            ctlCounterCashier.Style.Remove("display");
            ctlDivCounterCashierDomesticText.Style.Remove("display");
            if (!IsRepOffice)
            {
                //ถ้า Payment Type = เงินโอน ให้ซ่อน Counter Cashier
                if (ctlPaymentType.SelectedValue.Equals(PaymentType.TR) ||
                    ctlPaymentType.SelectedValue.Equals(string.Empty))
                {
                    ctlCounterCashier.Style.Add("display", "none");
                    ctlDivCounterCashierDomesticText.Style.Add("display", "none");
                    ctlCounterCashierExtender.Style.Add("display", "none");
                }
                else
                {
                    ctlCounterCashier.Style.Add("display", "block");
                    ctlDivCounterCashierDomesticText.Style.Add("display", "block");
                }
                ctlCurrencyDropdown.Items.Clear();
                //ctlCurrencyDropdown.DataBind();
                ctlCurrencyDropdown.Style.Add("display", "none");
                ctlCurrencyLabel.Visible = false;
                ctlCurrencyDropdownExtender.Style.Add("display", "none");
            }
            else
            {
                if (ctlPaymentType.SelectedValue.Equals(PaymentType.CA))
                {
                    ctlCurrencyDropdown.Style.Add("display", "block");
                    ctlCurrencyLabel.Visible = true;
                }
                else
                {
                    ctlCurrencyDropdown.DataSource = null;
                    ctlCurrencyDropdown.DataBind();
                    ctlCurrencyDropdown.Style.Add("display", "none");
                    ctlCurrencyLabel.Visible = false;
                    ctlCurrencyDropdownExtender.Style.Add("display", "none");
                }
            }
            ShowAmountLabel();
            if (this.isUpdatePanel)
            {
                ctlUpdatePanelGeneral.Update();
            }
        }
        protected void ctlLblPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DbCompany comp = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(UIHelper.ParseLong(ctlCompanyField.CompanyID));

            // อันเก่าค่ะ
            ctlCounterCashier.Items.Clear();
            if (ctlPaymentType.SelectedValue == PaymentType.TR)
            {
                ctlReceiverData.SetValue(UIHelper.ParseLong(ctlRequesterData.UserID));
            }
            if (ctlPaymentType.SelectedValue != PaymentType.TR)
            {
                ctlCounterCashier.DataSource = ScgDbQueryProvider.DbPBQuery.GetPbListItem(UIHelper.ParseLong(ctlCompanyField.CompanyID), UserAccount.CurrentLanguageID);
                ctlCounterCashier.DataBind();
                ctlPaymentMethodDomestic.DataSource = ScgDbQueryProvider.DbPBQuery.GetPbListItem(UIHelper.ParseLong(ctlCompanyField.CompanyID), UserAccount.CurrentLanguageID);
                if (ctlCounterCashier.Items.Count > 1)
                {
                    ctlCounterCashier.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
                }
            }

            if (ctlPaymentType.SelectedValue == PaymentType.TR || ctlPaymentType.SelectedValue == PaymentType.CQ)
            {
                if (IsRepOffice)
                {
                    AdvanceDataSet advanceDS = (AdvanceDataSet)TransactionService.GetDS(this.TransactionID);
                    AdvanceDataSet.AvAdvanceDocumentRow advanceRow = advanceDS.AvAdvanceDocument.FindByAdvanceID(this.DocumentID);
                    IsRepOffice = advanceRow.IsRepOffice = false;
                }
            }
            else
            {
                if (ctlCompanyField.CompanyID == ctlRequesterData.CompanyID)
                {
                    if (!IsRepOffice)
                    {
                        AdvanceDataSet advanceDS = (AdvanceDataSet)TransactionService.GetDS(this.TransactionID);
                        AdvanceDataSet.AvAdvanceDocumentRow advanceRow = advanceDS.AvAdvanceDocument.FindByAdvanceID(this.DocumentID);

                        SuUser userList = QueryProvider.SuUserQuery.FindByIdentity(UIHelper.ParseLong(ctlRequesterData.UserID));
                        if (userList != null)
                        {
                            if (userList.Location != null)
                            {
                                DbLocation location = ScgDbQueryProvider.DbLocationQuery.FindProxyByIdentity(userList.Location.LocationID);
                                if (location != null && location.DefaultPBID.HasValue)
                                {
                                    Dbpb pb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(location.DefaultPBID.Value);
                                    advanceRow.IsRepOffice = IsRepOffice = pb.RepOffice;
                                }
                                else
                                {
                                    advanceRow.IsRepOffice = IsRepOffice = false;
                                }
                            }
                        }
                    }
                }
            }

            hideCounterCashier();

            DefaultSupplementary();

            //อันที่เพิ่มค่ะ 
            if (comp != null)
            {
                switch (ctlPaymentType.SelectedValue)
                {
                    case PaymentType.CA:
                        if (comp.PaymentMethodPetty != null)
                            ctlPaymentMethodDomestic.SelectedValue = comp.PaymentMethodPetty.PaymentMethodID.ToString();
                        break;

                    case PaymentType.TR:
                        if (comp.PaymentMethodTransfer != null)
                            ctlPaymentMethodDomestic.SelectedValue = comp.PaymentMethodTransfer.PaymentMethodID.ToString();
                        break;

                    case PaymentType.CQ:
                        if (comp.PaymentMethodCheque != null)
                            ctlPaymentMethodDomestic.SelectedValue = comp.PaymentMethodCheque.PaymentMethodID.ToString();
                        break;
                }
            }

            if (ctlPaymentType.SelectedValue == PaymentType.CA)
            {
                if (IsRepOffice)
                {
                    SuUser userList = QueryProvider.SuUserQuery.FindByIdentity(UIHelper.ParseLong(ctlRequesterData.UserID));
                    if (userList != null && userList.Location != null && userList.Location.DefaultPBID != null)
                    {
                        ctlCounterCashier.SelectedValue = userList.Location.DefaultPBID.ToString();
                        BindCurrencyDropdown();
                        ShowAmountLabel();
                    }
                }
            }
            ctlUpdatePanelViewPost.Update();
        }
        #endregion

        #region Button Event
        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            this.RollBackTransaction();
            this.ResetControlValue();
        }

        protected void ctlViewPostButtonDomestic_Click(object sender, EventArgs e)
        {
            try
            {
                this.isNotViewPost = false;
                AdvanceDataSet advDs = (AdvanceDataSet)TransactionService.GetDS(this.TransactionID);
                AdvanceDataSet.AvAdvanceDocumentRow row = advDs.AvAdvanceDocument.FindByAdvanceID(this.DocumentID);
                SCGDocumentService.ValidateVerifyDetail(this.TransactionID, ViewPostDocumentType.AdvanceDomestic, row.IsRepOffice);
                ctlViewPost.Initialize(row.DocumentID, DocumentKind.Advance);
                ctlViewPost.Show();
                ctlUpdatePanelViewPost.Update();
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                ctlUpdatePanelValidation.Update();
            }
        }
        protected void ctlViewPostForeign_Click(object sender, EventArgs e)
        {
            /*check value thaibaht not = 0*/
            if (checkThaiBahtnotZero())
            {
                try
                {
                    this.isNotViewPost = false;
                    AdvanceDataSet advDs = (AdvanceDataSet)TransactionService.GetDS(this.TransactionID);
                    AdvanceDataSet.AvAdvanceDocumentRow row = advDs.AvAdvanceDocument.FindByAdvanceID(this.DocumentID);
                    SCGDocumentService.ValidateVerifyDetail(this.TransactionID, ViewPostDocumentType.AdvanceForeign, row.IsRepOffice);
                    ctlViewPost.Initialize(row.DocumentID, DocumentKind.Advance);
                    ctlViewPost.Show();
                    ctlUpdatePanelViewPost.Update();
                }
                catch (ServiceValidationException ex)
                {
                    this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                    ctlUpdatePanelValidation.Update();
                }
            }
            else
            {
                try
                {
                    SCGDocumentService.MessageValidation("AmountTHBIsRequired");
                }
                catch (ServiceValidationException ex)
                {
                    this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                    ctlUpdatePanelValidation.Update();
                }
            }
        }
        private bool checkThaiBahtnotZero()
        {
            if (ctlAdvanceIntGrid.Rows.Count > 0)
            {
                TextBox ctlAmountTHB = new TextBox();
                ctlAmountTHB.Text = "";
                foreach (GridViewRow row in ctlAdvanceIntGrid.Rows)
                {
                    ctlAmountTHB = (TextBox)ctlAdvanceIntGrid.Rows[row.RowIndex].FindControl("ctlAmountTHB");
                }
                if(ctlAmountTHB.Text != "" && ctlAmountTHB.Text != "0.00")
                {
                    return true;
                }
                else
                {
                    return false;
                    
                }
            }
            else
            {
                return false;
            }
            
        }
        #endregion

        #region IsContainEditableFields(object editableFieldEnum)
        public bool IsContainEditableFields(object editableFieldEnum)
        {
            return this.EditableFields.Contains(editableFieldEnum);
        }
        #endregion

        #region IsContainVisibleFields(object visibleFieldEnum)
        public bool IsContainVisibleFields(object visibleFieldEnum)
        {
            return this.VisibleFields.Contains(visibleFieldEnum);
        }
        #endregion

        #region ctlTANo_Click
        protected void ctlTANo_Click(object sender, EventArgs e)
        {
            if ((!string.IsNullOrEmpty(ctlTANo.Text) && !ctlTANo.Text.Equals("N/A")) && Request.QueryString["taDocumentID"] == null)
            {
                SS.Standard.WorkFlow.DTO.WorkFlow workFlow = new SS.Standard.WorkFlow.DTO.WorkFlow();
                if (DocumentIDofTA != 0)
                {
                    workFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(DocumentIDofTA);
                    long workFlowID;
                    if (workFlow != null)
                        workFlowID = workFlow.WorkFlowID;
                    else
                        workFlowID = 0;
                    //popup Document View by WorkFlow
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), string.Empty, "window.open('../Programs/DocumentView.aspx?wfid=" + workFlowID.ToString() + "')", true);
                }
            }
        }
        #endregion

        #region ctlDeleteTA_Click(object sender, ImageClickEventArgs e)
        protected void ctlDeleteTA_Click(object sender, ImageClickEventArgs e)
        {
            ctlNATaNo.Visible = true;
            ctlTANo.Text = string.Empty;
            ctlNATaNo.Text = "N/A";
            ctlTAIdLookup.Text = string.Empty;
            this.DocumentIDofTA = 0;
            ctlArrivalDate.DateValue = string.Empty;
            ctlDueDateOfRemittance.Text = string.Empty;
            ctlRequestDateOfRemittance.DateValue = string.Empty;
        }
        #endregion

        public void Copy(long wfid)
        {
            SS.Standard.WorkFlow.DTO.Document doc = WorkFlowQueryProvider.WorkFlowQuery.GetDocumentByWorkFlowID(wfid);
            if (doc.DocumentType.DocumentTypeID.Equals(DocumentTypeID.AdvanceDomesticDocument))
                Response.Redirect(String.Format("~/Forms/SCG.eAccounting/Programs/AdvanceDomesticForm.aspx?cp=1&docId={0}", doc.DocumentID));
            else if (doc.DocumentType.DocumentTypeID.Equals(DocumentTypeID.AdvanceForeignDocument))
                Response.Redirect(String.Format("~/Forms/SCG.eAccounting/Programs/AdvanceForeignForm.aspx?cp=1&docId={0}", doc.DocumentID));
        }
        private void BindServiceTeam()
        {
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
            ctlServiceTeam.DataSource = ScgDbQueryProvider.DbLocationQuery.FindByLocationCompany(UIHelper.ParseLong(ctlCompanyField.CompanyID), null, UserAccount.UserID, locationID);
            ctlServiceTeam.DataBind();
            if (ctlServiceTeam.Items.Count > 1)
            {
                ctlServiceTeam.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));

                IList<DbServiceTeamLocation> listLocation = ScgDbQueryProvider.DbServiceTeamLocationQuery.FindServiceTeamByLocationID(locationID);
                if (listLocation != null && listLocation.Count > 0)
                {
                    if (ctlServiceTeam.Items.FindByValue(listLocation.First<DbServiceTeamLocation>().ServiceTeamID.ServiceTeamID.ToString()) != null)
                        ctlServiceTeam.SelectedValue = listLocation.First<DbServiceTeamLocation>().ServiceTeamID.ServiceTeamID.ToString();
                }

            }
        }

        public void EnabledViewPostButton(bool IsLock)
        {
            ctlViewPostButtonDomestic.Enabled = IsLock;
            ctlViewPostForeign.Enabled = IsLock;
            if (isNotViewPost)
            {
                ctlUpdatePanelViewPost.Update();
            }
        }

        public void EnabledPostRemittanceButton(bool IsLock)
        {

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

        protected void ctlCurrencyDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowAmountLabel();
        }

        public void ShowAmountLabel()
        {
            if (IsRepOffice && ctlPaymentType.SelectedValue == PaymentType.CA)
            {
                if (ctlCurrencyDropdown.Items.Count > 0)
                {
                    ctlAmountText.Text = string.Format(GetProgramMessage("AmountCurrency"), ctlCurrencyDropdown.SelectedItem);
                }
            }
            else
            {
                ctlAmountText.Text = string.Format(GetProgramMessage("AmountCurrency"), CurrencySymbol.THB.ToString());
            }
        }

        public void BindCurrencyDropdown()
        {
            ctlCurrencyLabel.Visible = true;
            ctlCurrencyDropdown.Style.Add("display", "block");
            try
            {
                ctlCurrencyDropdown.DataSource = ScgDbQueryProvider.DbPBCurrencyQuery.FindPBLocalCurrencyByPBID(UIHelper.ParseLong(ctlCounterCashier.SelectedValue), ZoneType.Domestic);
                ctlCurrencyDropdown.DataTextField = "Symbol";
                ctlCurrencyDropdown.DataValueField = "CurrencyID";
                ctlCurrencyDropdown.DataBind();
            }
            catch { }
        }

        public void CalculateExchangeRate()
        {
            //-----find exchange rate (main Exchange Rate)-----
            long pbid = UIHelper.ParseLong(ctlCounterCashier.SelectedValue);
            Dbpb pb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(pbid);
            short mainCurrencyId = pb.MainCurrencyID.Value;
            short finalCurreucyId = UIHelper.ParseShort(ctlCurrencyDropdown.SelectedValue);
            short localCurreucyId = 0;

            IList<DbPBCurrency> pbCurrency = ScgDbQueryProvider.DbPBCurrencyQuery.FindPBLocalCurrencyByPBID(pbid, ZoneType.Domestic);
            var currencyLocal = pbCurrency.Where(t => t.CurrencyID != mainCurrencyId).FirstOrDefault();
            if (currencyLocal != null)
            {
                localCurreucyId = currencyLocal.CurrencyID;
            }

            double exchangeRateForLocalCurrency = ScgDbQueryProvider.DbPbRateQuery.GetExchangeRate(pbid, mainCurrencyId, localCurreucyId);

            // ยังขาดตอนเปลี่ยนหน่วยอยู่ ต้องทำ translate lang ก่อน!!!!
            // find exchange rate ( to THB currencyid = 149)
            DbCurrency currencyThb = SsDbQueryProvider.DbCurrencyQuery.FindByCurrencySymbol(CurrencySymbol.THB.ToString(), false, false);

            double exchangRateMainToTHB = ScgDbQueryProvider.DbPbRateQuery.GetExchangeRate(pbid, mainCurrencyId, currencyThb.CurrencyID);


            double amountFinalCurrency = UIHelper.ParseDouble(ctlAmount.Text.ToString());
            double amountTHB = 0;
            double amountMainCurrency = 0;

            if (finalCurreucyId == mainCurrencyId)
            {
                amountMainCurrency = amountFinalCurrency;
                //exchangeRateForLocalCurrency = 1;
            }
            else
            {
                if (exchangeRateForLocalCurrency != 0)
                {
                    if (exchangRateMainToTHB != 0)
                    {
                        amountMainCurrency = (double)(Math.Round((decimal)(amountFinalCurrency / exchangeRateForLocalCurrency), 2, MidpointRounding.AwayFromZero));
                    }
                }
            }

            ctlAmountTotalMainCurrency.Text = UIHelper.BindDecimal(amountMainCurrency.ToString());
            amountTHB = (double)Math.Round((decimal)(amountMainCurrency * exchangRateMainToTHB), 2, MidpointRounding.AwayFromZero);
            ctlAmountTotalTHBLabel.Text = UIHelper.BindDecimal(amountTHB.ToString());

            ctlExchangeRate1.Text = UIHelper.BindExchangeRate(exchangeRateForLocalCurrency.ToString());
            ctlExchangeRate2.Text = UIHelper.BindExchangeRate(exchangRateMainToTHB.ToString());
        }

        public void CheckRepOffice()
        {
            AdvanceDataSet advanceDS = (AdvanceDataSet)TransactionService.GetDS(this.TransactionID);
            AdvanceDataSet.AvAdvanceDocumentRow advanceRow = advanceDS.AvAdvanceDocument.FindByAdvanceID(this.DocumentID);

            if (InitialFlag.Equals(FlagEnum.NewFlag) && ((advanceRow.IsIsRepOfficeNull() || !IsRepOffice)))
            {
                SuUser userList = QueryProvider.SuUserQuery.FindByIdentity(UIHelper.ParseLong(ctlRequesterData.UserID));
                if (userList != null && userList.Location != null)
                {
                    if (userList.Location.DefaultPBID.HasValue)
                    {
                        Dbpb pb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(userList.Location.DefaultPBID.Value);
                        if (pb != null)
                        {
                            IsRepOffice = advanceRow.IsRepOffice = pb.RepOffice;
                        }
                        else
                        {
                            IsRepOffice = advanceRow.IsRepOffice = false;
                        }
                    }
                    else
                    {
                        IsRepOffice = advanceRow.IsRepOffice = false;
                    }
                }
                else
                {
                    IsRepOffice = advanceRow.IsRepOffice = false;
                }
            }
            else
            {
                if (advanceRow.IsIsRepOfficeNull())
                {
                    advanceRow.IsRepOffice = false;
                }
                IsRepOffice = advanceRow.IsRepOffice;
            }
        }

        public void DefaultPb()
        {
            if (ctlCompanyField.CompanyID == ctlRequesterData.CompanyID)
            {
                SuUser requester = QueryProvider.SuUserQuery.FindByIdentity(UIHelper.ParseLong(ctlRequesterData.UserID));
                if (requester.Location != null && requester.Location.DefaultPBID.HasValue)
                {
                    if (ctlAdvanceType.Text == AdvanceTypeEnum.Domestic)
                    {
                        if (!string.IsNullOrEmpty(ctlPaymentType.SelectedValue) && ctlPaymentType.SelectedValue != PaymentType.TR)
                            ctlCounterCashier.SelectedValue = requester.Location.DefaultPBID.Value.ToString();
                    }
                    else
                    {
                        ctlCounterCashierInter.SelectedValue = requester.Location.DefaultPBID.Value.ToString();
                    }
                }
                else
                {
                    if (ctlAdvanceType.Text == AdvanceTypeEnum.Domestic)
                    {
                        if (!string.IsNullOrEmpty(ctlPaymentType.SelectedValue) && ctlPaymentType.SelectedValue != PaymentType.TR)
                            ctlCounterCashier.SelectedIndex = 0;
                    }
                }
            }
            else
            {
                if (ctlAdvanceType.Text == AdvanceTypeEnum.Domestic)
                {
                    if (ctlPaymentType.SelectedValue != PaymentType.TR)
                    {
                        ctlCounterCashier.DataSource = ScgDbQueryProvider.DbPBQuery.GetPbListItem(UIHelper.ParseLong(ctlCompanyField.CompanyID), UserAccount.CurrentLanguageID);
                        ctlCounterCashier.DataBind();

                        if (ctlCounterCashier.Items.Count > 1)
                        {
                            ctlCounterCashier.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
                            ctlCounterCashier.SelectedIndex = 0;
                        }
                        
                    }
                }
            }
        }
        protected void ctlCounterCashierDomestic_SelectedIndexChanged(object sender, EventArgs e)
        {
            AdvanceDataSet advanceDS = (AdvanceDataSet)TransactionService.GetDS(this.TransactionID);
            AdvanceDataSet.AvAdvanceDocumentRow advanceRow = advanceDS.AvAdvanceDocument.FindByAdvanceID(this.DocumentID);

            Dbpb pb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(UIHelper.ParseLong(ctlCounterCashier.SelectedValue));

            IsRepOffice = advanceRow.IsRepOffice = pb.RepOffice;
            BindCurrencyDropdown();
            DefaultSupplementary();
            hideCounterCashier();
        }

        protected void ctlCounterCashierForeign_SelectedIndexChanged(object sender, EventArgs e)
        {
            AdvanceDataSet advanceDS = (AdvanceDataSet)TransactionService.GetDS(this.TransactionID);
            AdvanceDataSet.AvAdvanceDocumentRow advanceRow = advanceDS.AvAdvanceDocument.FindByAdvanceID(this.DocumentID);

            Dbpb pb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(UIHelper.ParseLong(ctlCounterCashierInter.SelectedValue));

            IsRepOffice = advanceRow.IsRepOffice = pb.RepOffice;

            if (!InitialFlag.Equals(FlagEnum.NewFlag))
            {
                ctlAdvanceIntGrid.DataSource = advanceDS.AvAdvanceItem.Select();
                ctlAdvanceIntGrid.DataBind();
            }
        }

        public void GetExchangeRateForRepOffice(string advanceType, AvAdvanceDocument adv)
        {
            Guid txID = this.TransactionID;
            long advDocumentID = this.DocumentID;
            AdvanceDataSet advDS = (AdvanceDataSet)TransactionService.GetDS(txID);
            AdvanceDataSet.AvAdvanceDocumentRow advRow = advDS.AvAdvanceDocument.FindByAdvanceID(advDocumentID);

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

            if (IsRepOffice && ((InitialFlag.Equals(FlagEnum.EditFlag) && (strCurrentState.Equals(WorkFlowStateFlag.WaitVerify) || strCurrentState.Equals(WorkFlowStateFlag.Draft))) || InitialFlag.Equals(FlagEnum.NewFlag)))
            {
                //-----find exchange rate (main Exchange Rate)-----
                if (adv.PBID != null && adv.PBID.Pbid > 0)
                {
                    IList<DbPBCurrency> pbCurrency = ScgDbQueryProvider.DbPBCurrencyQuery.FindPBLocalCurrencyByPBID(adv.PBID.Pbid, ZoneType.Domestic);
                    var currencyLocal = pbCurrency.Where(t => t.CurrencyID != adv.MainCurrencyID).FirstOrDefault();
                    if (currencyLocal != null)
                    {
                        adv.ExchangeRateForLocalCurrency = ScgDbQueryProvider.DbPbRateQuery.GetExchangeRate(adv.PBID.Pbid, adv.MainCurrencyID.Value, currencyLocal.CurrencyID);
                    }

                    DbCurrency currencyThb = SsDbQueryProvider.DbCurrencyQuery.FindByCurrencySymbol(CurrencySymbol.THB.ToString(), false, false);
                    adv.ExchangeRateMainToTHBCurrency = ScgDbQueryProvider.DbPbRateQuery.GetExchangeRate(adv.PBID.Pbid, adv.MainCurrencyID.Value, currencyThb.CurrencyID);
                }
            }
        }

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
            return false;
        }

        public void DefaultSupplementary()
        {
            if (IsContainEditableFields(SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.VerifyDetail))
            {
                if (ctlPaymentType.SelectedValue == PaymentTypeConst.DomesticCheque)
                {
                    Dbpb pb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(UIHelper.ParseLong(ctlCounterCashier.SelectedValue));
                    ctlSupplementaryDomestic.Text = pb == null ? string.Empty : pb.Supplementary;
                }
                else
                {
                    ctlSupplementaryDomestic.Text = string.Empty;
                }
            }
        }

        #region ctlFixedAdvanceOutstandingGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        protected void ctlFixedAdvanceOutstandingGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal ctlNo = e.Row.FindControl("ctlNoText") as Literal;
                ctlNo.Text = ((FixedAdvanceOutstandingGrid.PageSize * FixedAdvanceOutstandingGrid.CustomPageIndex) + (e.Row.RowIndex + 1)).ToString();
            }
        }
        #endregion

        protected void ctlFixedAdvanceOutstandingGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ClickFixedAdvanceAdvanceNo")
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                AvDocumentIDInGrid = UIHelper.ParseLong(FixedAdvanceOutstandingGrid.DataKeys[rowIndex].Values["DocumentID"].ToString());

                SS.Standard.WorkFlow.DTO.WorkFlow workFlow = new SS.Standard.WorkFlow.DTO.WorkFlow();
                if (e.CommandName == "ClickFixedAdvanceAdvanceNo")
                {
                    workFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(AvDocumentIDInGrid);
                }

                long workFlowID;
                if (workFlow != null)
                    workFlowID = workFlow.WorkFlowID;
                else
                    workFlowID = 0;
                //popup Document View by WorkFlow
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), string.Empty, "window.open('../Programs/DocumentView.aspx?wfid=" + workFlowID.ToString() + "')", true);
            }
        }
        #region RequestCountFixedAdvanceOutstanding()
        public int RequestCountFixedAdvanceOutstanding()
        {

            long currentWorkFlowID = UIHelper.ParseLong(Request.Params["wfid"]);
            return ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.CountFindFixedAdvanceOutstandingRequest(UIHelper.ParseLong(ctlRequesterData.UserID), UIHelper.ParseLong(ctlCompanyField.CompanyID), currentWorkFlowID, UserAccount.CurrentLanguageID);
        }
        #endregion

        #region RequestDataFixedAdvanceOutstanding(int startRow, int pageSize, string sortExpression)
        public Object RequestDataFixedAdvanceOutstanding(int startRow, int pageSize, string sortExpression)
        {
            long currentWorkFlowID = UIHelper.ParseLong(Request.Params["wfid"]);
            return ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.FindFixedAdvanceOutstandingRequest(UIHelper.ParseLong(ctlRequesterData.UserID), UIHelper.ParseLong(ctlCompanyField.CompanyID), currentWorkFlowID, UserAccount.CurrentLanguageID, startRow, pageSize, sortExpression);
        }
        #endregion

        private void CheckShowMessageOutstanding(string flag)
        {
            long currentWorkFlowID = UIHelper.ParseLong(Request.Params["wfid"]);
            DateTime curdate = DateTime.Now;
            string CurrentStateName = "";
            SS.Standard.WorkFlow.DTO.WorkFlow wf = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(currentWorkFlowID);
            if (wf != null && wf.CurrentState != null && wf.CurrentState.Name != null)
                CurrentStateName = wf.CurrentState.Name;
            if (wf != null && wf.CurrentState != null && wf.UpdDate != null)
                curdate = wf.UpdDate;
            /*check fixedadvance and advance outstanding*/
            bool hasfixedadvance = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.FindFixedAdvanceOutStandingFromAleart(UIHelper.ParseLong(ctlRequesterData.UserID), UIHelper.ParseLong(ctlCompanyField.CompanyID));
            int hasadvance = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.CountAdvanceForFixedAdvance(UIHelper.ParseLong(ctlCompanyField.CompanyID), UIHelper.ParseLong(ctlRequesterData.UserID), currentWorkFlowID, DocumentTypeID.AdvanceDomesticDocument, curdate);

            /*workflow state IN (1,3,4,5,6) = Draft,WaitInitial,WaitApprove,WaitVerify,WaitApproveVerify*/
            if (flag.Equals(FlagEnum.NewFlag) || CurrentStateName == "Draft" || CurrentStateName == "WaitInitial" || CurrentStateName == "WaitApprove" || CurrentStateName == "WaitVerify" || CurrentStateName == "WaitApproveVerify")
            {
                if (hasadvance > 0 || hasfixedadvance)
                    AleartMessageFixedAdvance.Visible = true;
                else
                    AleartMessageFixedAdvance.Visible = false;
            }
        }
    }
}


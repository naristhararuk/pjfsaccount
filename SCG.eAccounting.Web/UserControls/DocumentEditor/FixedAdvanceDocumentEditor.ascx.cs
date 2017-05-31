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
    public partial class FixedAdvanceDocumentEditor : BaseUserControl, IDocumentEditor
    {
        private static ILog logger = log4net.LogManager.GetLogger(typeof(FixedAdvanceDocumentEditor));

        #region local variable

        public IAvAdvanceDocumentService AvAdvanceDocumentService { get; set; }
        public ITransactionService TransactionService { get; set; }
        public IAvAdvanceItemService AvAdvanceItemService { get; set; }
        public IDocumentInitiatorService DocumentInitiatorService { get; set; }
        public IWorkFlowService WorkFlowService { get; set; }
        public ISCGDocumentService SCGDocumentService { get; set; }
        public IFixedAdvanceDocumentService FixedAdvanceDocumentService { get; set; }
        private string documentTypeDMT = "PaymentTypeDMT";
        //private string documentTypeFRN = "PaymentTypeFRN";
        private int dueDateOfRemit = ParameterServices.DueDateOfRemittance;
        private int requestDateOfRemit = ParameterServices.RequestDateOfRemittance;
        private int currencyUsd = ParameterServices.USDCurrencyID;
        public double totalPerdiemRateUSD = 0;
        public double totalAmountTHB = 0;
        public double totalAmountMainCurrency = 0;
        public double totalAmountUSD = 0;
        public double totalAmountThbUSD = 0;
        private string selectpaymenttype = string.Empty;
        #endregion

        #region Property

        public string HeaderForm
        {
            set { ctlFixedAdvanceFormHeader.HeaderForm = value; }
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
        public string FixedAdvanceType
        {
            get { return ctlFixedAdvanceType.Text; }/*value = Domestic*/
            set { ctlFixedAdvanceType.Text = value; }
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

        public long TempRequesterID
        {
            get { return ViewState["TempRequesterID"] == null ? 0 : (long)ViewState["TempRequesterID"]; }
            set { ViewState["TempRequesterID"] = value; }
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

        #endregion

        #region Page_Load Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ctlAdvanceTabContainer.ActiveTab = ctlTabGeneral;
                BindTabGeneral();
                BindTabInitial();
                BindTabAttachment();
                BindTabMemo();
                ctlOutstandingGrid.DataCountAndBind();
                FixedAdvanceOutstandingGrid.DataCountAndBind();
                ctlUpdatePanelOutstanding.Update();
                hideCounterCashier();
                hideReturnCounterCashier();
            }

            if (ctlEffectiveDateFrom.DateValue != String.Empty && this.InitialFlag.Equals(FlagEnum.NewFlag) && New.Checked == true && ctlRequestDate.DateValue == String.Empty)
            {
                ctlRequestDate.DateValue = ctlEffectiveDateFrom.DateValue;
            }
            ctlCompanyField.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlCompanyField_OnObjectLookUpReturn);
            ctlRequesterData.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlRequesterData_OnObjectLookUpReturn);
            ctlReceiverData.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlReceiverData_OnObjectLookUpReturn);
        }

        void OnDsNull()
        {
            if (dsNullHandler != null)
                dsNullHandler();
        }

        #endregion

        #region Initialize(string initFlag, long? documentID)
        public void Initialize(string initFlag, long? documentID)
        {
            long fixedadvanceDocumentID = 0;
            FixedAdvanceDataSet fixedadvanceDataset;
            Guid txID = Guid.Empty;
            bool isCopy = false;
            this.TempRequesterID = UIHelper.ParseLong(ctlReceiverData.UserID);

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
            this.isShowFooter = true;

            if (initFlag.Equals(FlagEnum.NewFlag) && Request.QueryString["cp"] == null)
            {
                fixedadvanceDataset = (FixedAdvanceDataSet)FixedAdvanceDocumentService.PrepareDS();
                txID = TransactionService.Begin(fixedadvanceDataset);
                if (Request.Params["wfid"] == null)
                {
                    fixedadvanceDocumentID = FixedAdvanceDocumentService.AddFixedAdvanceDocument(txID, ZoneType.Domestic);
                }
                ctlUpdatePanelHeader.Update();
            }
            else if (((initFlag.Equals(FlagEnum.EditFlag) || (initFlag.Equals(FlagEnum.ViewFlag)))) && ((documentID.HasValue) && (documentID.Value != 0)))
            {
                fixedadvanceDataset = (FixedAdvanceDataSet)FixedAdvanceDocumentService.PrepareDS(documentID.Value, false);
                txID = TransactionService.Begin(fixedadvanceDataset);
                FixedAdvanceDataSet fixedadvanceDS = (FixedAdvanceDataSet)fixedadvanceDataset;
                if (fixedadvanceDS.FixedAdvanceDocument.Rows.Count > 0)
                {
                    fixedadvanceDocumentID = UIHelper.ParseLong(fixedadvanceDS.FixedAdvanceDocument.Rows[0]["FixedAdvanceID"].ToString());
                }
            }
            else if (Request.QueryString["cp"] != null && UIHelper.ParseLong(Request.QueryString["cp"]) == 1)
            {
                fixedadvanceDataset = (FixedAdvanceDataSet)FixedAdvanceDocumentService.PrepareInternalDataToDataset(UIHelper.ParseLong(Request.QueryString["docId"]), true);
                txID = TransactionService.Begin(fixedadvanceDataset);
                //FixedAdvanceDataSet fixedadvanceDS = (FixedAdvanceDataSet)fixedadvanceDataset;
                isCopy = true;
                if (fixedadvanceDataset.FixedAdvanceDocument.Rows.Count > 0)
                {
                    fixedadvanceDocumentID = UIHelper.ParseLong(fixedadvanceDataset.FixedAdvanceDocument.Rows[0]["FixedAdvanceID"].ToString());
                }

            }
            this.TransactionID = txID;
            this.DocumentID = fixedadvanceDocumentID;
            this.InitialFlag = initFlag;

            this.VisibleFields = FixedAdvanceDocumentService.GetVisibleFields(documentID);
            this.EditableFields = FixedAdvanceDocumentService.GetEditableFields(documentID);

            this.InitializeControl();
            this.BindControl(isCopy);

            #region แสดง Tab

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
            ctlFixedAdvanceFormHeader.Status = string.Empty;
            ctlFixedAdvanceFormHeader.No = string.Empty;
            ctlFixedAdvanceFormHeader.CreateDate = string.Empty;
            ctlCompanyField.ShowDefault();
            ctlSubject.Text = string.Empty;

            ctlCreatorData.ShowDefault();
            ctlRequesterData.ShowDefault();
            ctlApproverData.ShowDefaultApprover(UIHelper.ParseLong(ctlRequesterData.UserID));

            ctlReceiverData.ShowDefault();
            ctlAmountValue.Text = "0.00";
            ctlServiceTeam.Items.Clear();


            var a = ScgDbQueryProvider.SCGDbStatusLangQuery.FindStatusLangCriteria(documentTypeDMT, UserAccount.CurrentLanguageID);
            var b = a.Where(x => x.strID != PaymentType.CA);
            ctlPaymentType.DataSource = b;
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


            ctlOutstandingGrid.DataSource = null;
            ctlOutstandingGrid.DataBind();
            FixedAdvanceOutstandingGrid.DataSource = null;
            FixedAdvanceOutstandingGrid.DataBind();

            //ctlInitiator
            ctlMemo.Text = string.Empty;

        }
        #endregion

        #region hide counter cashier when payment type = "เงินโอน"
        private void hideCounterCashier()
        {
            /*check = new or adjust*/
            if (New.Checked == true)
            {
                if (ctlPaymentType.SelectedValue.Equals(PaymentType.TR))
                {
                    /*tranfer hide counter cashier*/
                    ctlDivCounterCashierDomesticText.Visible = false;
                }
                else
                {
                    ctlDivCounterCashierDomesticText.Visible = true;
                }
                /*show service team when new fixedadvance*/
                ctlDivServiceTeam.Visible = true;
                ctlDivSupplementFixedAdvance.Visible = true;
            }
            else
            {
                /*Adjust*/
                if (checkpaybackcompany())
                {
                    ctlDivCounterCashierDomesticText.Visible = true;
                    ctlDivServiceTeam.Visible = false;
                    ctlDivSupplementFixedAdvance.Visible = false;
                }
                else
                {
                    if (ctlPaymentType.SelectedValue.Equals(PaymentType.CQ))
                    {
                        ctlDivCounterCashierDomesticText.Visible = true;
                    }
                    else if (ctlPaymentType.SelectedValue.Equals(PaymentType.TR))
                    {
                        ctlDivCounterCashierDomesticText.Visible = false;
                    }
                    else
                    {
                        ctlDivCounterCashierDomesticText.Visible = true;
                    }
                    ctlDivServiceTeam.Visible = true;
                    ctlDivSupplementFixedAdvance.Visible = true;
                }
            }

            if (this.isUpdatePanel)
            {
                ctlUpdatePanelGeneral.Update();
            }
        }

        private bool checkpaybackcompany()
        {
            ctlAmountGetValue.Text = Convert.ToString(UIHelper.ParseDouble(ctlAmountValue.Text) - UIHelper.ParseDouble(ctlPrevionsAmountValue.Text));
            if (UIHelper.ParseDouble(ctlAmountGetValue.Text) > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void hideReturnCounterCashier()
        {
            ReturnCounterCashierDiv.Visible = false;

            //ถ้า Payment Type = เงินโอน ให้ซ่อน Counter Cashier
            //if (ctlReturnPaymentType.SelectedValue.Equals(PaymentType.TR) ||
            //    ctlReturnPaymentType.SelectedValue.Equals(string.Empty))
            //{
            //    ReturnCounterCashierDiv.Visible = false;
            //    ctlReturnCounterCashierExtender.Style.Add("display", "none");
            //}
            //else
            //{
            //    ReturnCounterCashierDiv.Visible = true;
            //}
            //if (this.isUpdatePanel)
            //{
            //    ctlUpdatePanelGeneral.Update();
            //}
            if (!ctlReturnPaymentType.SelectedValue.Equals(string.Empty))
            {
                ReturnCounterCashierDiv.Visible = true;
                ReturnServiceTeamDiv.Visible = false;
            }
        }

        protected void ctlLblPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DbCompany comp = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(UIHelper.ParseLong(ctlCompanyField.CompanyID));
            ctlAmountGetValue.Text = Convert.ToString(UIHelper.ParseDouble(ctlAmountValue.Text) - UIHelper.ParseDouble(ctlPrevionsAmountValue.Text));

            // อันเก่าค่ะ
            ctlCounterCashier.Items.Clear();
            ctlCounterCashier.DataSource = ScgDbQueryProvider.DbPBQuery.GetPbListItem(UIHelper.ParseLong(ctlCompanyField.CompanyID), UserAccount.CurrentLanguageID);
            ctlCounterCashier.DataBind();
            ctlPaymentMethodDomestic.DataSource = ScgDbQueryProvider.DbPBQuery.GetPbListItem(UIHelper.ParseLong(ctlCompanyField.CompanyID), UserAccount.CurrentLanguageID);
            if (ctlCounterCashier.Items.Count > 1)
            {
                ctlCounterCashier.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
            }
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
                        if (ctlRequesterData.UserID != ctlReceiverData.UserID)
                        {
                            string ErrorMessage = "Can't edit reciever on payment type transfer.";
                            ctlReceiverData.SetValue(this.TempRequesterID);
                            if (!string.IsNullOrEmpty(ErrorMessage))
                                ValidationErrors.AddError("Provider.Error", new Spring.Validation.ErrorMessage(ErrorMessage));
                        }
                        break;

                    case PaymentType.CQ:
                        if (comp.PaymentMethodCheque != null)
                            ctlPaymentMethodDomestic.SelectedValue = comp.PaymentMethodCheque.PaymentMethodID.ToString();
                        break;
                }
            }

            hideCounterCashier();
            hideReturnCounterCashier();

            ctlUpdatePanelViewPost.Update();
            ctlUpdatePanelViewPostReturn.Update();
        }
        #endregion

        protected void ctlLblReturnPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DbCompany comp = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(UIHelper.ParseLong(ctlCompanyField.CompanyID));

            // อันเก่าค่ะ
            ctlReturnCounterCashier.Items.Clear();

            //if (ctlReturnPaymentType.SelectedValue != PaymentType.TR)
            //{
            ctlReturnCounterCashier.DataSource = ScgDbQueryProvider.DbPBQuery.GetPbListItem(UIHelper.ParseLong(ctlCompanyField.CompanyID), UserAccount.CurrentLanguageID);
            ctlReturnCounterCashier.DataBind();
            ctlPaymentMethodDomestic.DataSource = ScgDbQueryProvider.DbPBQuery.GetPbListItem(UIHelper.ParseLong(ctlCompanyField.CompanyID), UserAccount.CurrentLanguageID);
            if (ctlReturnCounterCashier.Items.Count > 1)
            {
                ctlReturnCounterCashier.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
            }
            //}


            //อันที่เพิ่มค่ะ 
            if (comp != null)
            {
                switch (ctlReturnPaymentType.SelectedValue)
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
            hideReturnCounterCashier();
            ctlUpdatePanelViewPost.Update();
            ctlUpdatePanelViewPostReturn.Update();
        }

        #region InitializeControl()
        private void InitializeControl()
        {
            ctlCompanyField.UseEccOnly = true; /*filter company ฝั่ง ECC เท่านั้น*/
            ctlCompanyField.FlagActive = true;
            FixedAdvanceDataSet fixedadvanceDataSet = (FixedAdvanceDataSet)TransactionService.GetDS(this.TransactionID);
            FixedAdvanceDataSet.FixedAdvanceDocumentRow row = fixedadvanceDataSet.FixedAdvanceDocument.FindByFixedAdvanceID(this.DocumentID);
            long tempFixedAdvanceID = row.FixedAdvanceID;
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

            ctlInitiator.ControlGroupID = SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.Initiator;
            ctlInitiator.Initialize(this.TransactionID, tempFixedAdvanceID, this.InitialFlag);

            long tempDocumentID = row.DocumentID;

            ctlHistory.Initialize(tempDocumentID);

            ctlFixedAdvanceFormHeader.Initialize(this.TransactionID, tempDocumentID, this.InitialFlag);      // send SCGDocument.DocumentID for check visible see history

            ctlApproverData.DocumentEditor = this;
            ctlCreatorData.ControlGroupID = SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.BuActor;
            ctlRequesterData.ControlGroupID = SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.BuActor;
            ctlReceiverData.ControlGroupID = SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.BuActor;
            ctlApproverData.ControlGroupID = SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.BuActor;

            ctlCreatorData.Initialize(this.TransactionID, this.DocumentID, this.InitialFlag);
            ctlRequesterData.Initialize(this.TransactionID, this.DocumentID, this.InitialFlag);
            ctlReceiverData.Initialize(this.TransactionID, this.DocumentID, this.InitialFlag);
            ctlApproverData.Initialize(this.TransactionID, this.DocumentID, this.InitialFlag);

            ctlFixedAdvanceFormHeader.DataBind();
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
                PostReturn.Visible = true;
            }
            else
            {
                ctlViewPostButtonDomestic.Visible = false;
                PostReturn.Visible = false;
            }
            //if (VisibleFields.Contains(FixedAdvanceFieldGroup.ServiceTeam))
            //{
            //    ctlDivServiceTeam.Visible = true;
            //}
            //else
            //{
            //    ctlDivServiceTeam.Visible = false;
            //}

            if (!isCopy && this.InitialFlag.Equals(FlagEnum.NewFlag))
            {
                ctlFixedAdvanceFormHeader.Status = FlagEnum.NewFlag;
                ctlCompanyField.ShowDefault();
                ctlCreatorData.ShowDefault();
                ctlRequesterData.ShowDefault();
                ctlInitiator.RequesterID = UIHelper.ParseLong(ctlRequesterData.UserID);
                ctlReceiverData.ShowDefault();
                New.Checked = true;
                DivPrevionsAmount.Visible = false;
                DivRefFixedAdvance.Visible = false;
                divreturnMedthod.Visible = false;
                ctlPrevionsAmountValue.Text = "0.00";
                ctlNetReceiveValue.Text = "0.00";
                ctlAmountValue.Text = "0.00";
                DivCtlNetReceive.Visible = false;

                ctlPayBackCompany.Style.Add("display", "none");
                ctlReceiveCashFromCompany.Style.Add("display", "none");
                bindPaymentType(PaymentType.CA, selectpaymenttype);
                // ctlApproverData.ShowDefault();
                ctlApproverData.ShowDefaultApprover(UIHelper.ParseLong(ctlRequesterData.UserID));
            }
            else
            {
                if (UIHelper.ParseInt(Request.Params["wfid"]) > 0)//กรณีคืนค่ากลับมาหลังจาก save เสร็จ
                {
                    long workFlowID = UIHelper.ParseLong(Request.Params["wfid"]);

                    int documentTypeID = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID).Document.DocumentType.DocumentTypeID;

                    HeaderForm = GetProgramMessage("FixedAdvanceFormEditor");
                }

                if (VisibleFields.Contains(FixedAdvanceFieldGroup.ServiceTeam))
                {
                    ctlDivServiceTeam.Visible = true;
                }

                if (VisibleFields.Contains(FixedAdvanceFieldGroup.Return))
                {
                    divreturnMedthod.Visible = true;
                }
                else
                {
                    divreturnMedthod.Visible = false;
                }
                Guid txID = this.TransactionID;
                long fixedadvanceID = this.DocumentID;

                FixedAdvanceDataSet fixedadvanceDS = (FixedAdvanceDataSet)TransactionService.GetDS(txID);
                FixedAdvanceDataSet.FixedAdvanceDocumentRow fixedadvanceRow = fixedadvanceDS.FixedAdvanceDocument.FindByFixedAdvanceID(fixedadvanceID);
                FixedAdvanceDataSet.DocumentRow document = fixedadvanceDS.Document.FindByDocumentID(fixedadvanceRow.DocumentID);
                #region Bind Header and Footer

                if (!isCopy)
                {
                    ctlFixedAdvanceFormHeader.No = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(fixedadvanceRow.DocumentID).DocumentNo;
                    if (document.DocumentDate != DateTime.MinValue)
                        ctlFixedAdvanceFormHeader.CreateDate = UIHelper.ToDateString(document.DocumentDate);
                    ctlFixedAdvanceFormHeader.Status = ScgeAccountingQueryProvider.SCGDocumentQuery.GetDocumentCurrentStateName(UserAccount.CurrentLanguageID, fixedadvanceRow.DocumentID);
                    ctlCreatorData.SetValue(document.CreatorID);
                }
                else
                {
                    ctlFixedAdvanceFormHeader.Status = FlagEnum.NewFlag;
                    ctlCreatorData.ShowDefault();
                }

                ctlCompanyField.SetValue(document.CompanyID);
                ctlSubject.Text = document.Subject;
                ctlRequesterData.SetValue(document.RequesterID);
                ctlReceiverData.SetValue(document.ReceiverID);
                ctlApproverData.SetValue(document.ApproverID);
                #endregion

                //CheckRepOffice();
                #region Bind General Tab (Domestic/Foreign)
                if (fixedadvanceRow.IsAmountNull())
                {
                    ctlAmountValue.Text = "0.00";
                }
                else
                {
                    ctlAmountValue.Text = UIHelper.BindDecimal(fixedadvanceRow.Amount.ToString());
                }

                ctlEffectiveDateFrom.DateValue = UIHelper.ToDateString(fixedadvanceRow.EffectiveFromDate);
                ctlEffectiveDateTo.DateValue = UIHelper.ToDateString(fixedadvanceRow.EffectiveToDate);
                ctlRequestDate.DateValue = UIHelper.ToDateString(fixedadvanceRow.RequestDate);
                this.TempRequesterID = UIHelper.ParseLong(ctlReceiverData.UserID);


                if (!fixedadvanceRow.IsReturnRequestDateNull())
                {
                    ctlReturnRequestDate.DateValue = UIHelper.ToDateString(fixedadvanceRow.ReturnRequestDate);
                }

                if (!String.IsNullOrEmpty(fixedadvanceRow.ReturnPaymentType))
                {
                    //ctlReturnPaymentType.DataSource = ScgDbQueryProvider.SCGDbStatusLangQuery.FindStatusLangCriteria(documentTypeDMT, UserAccount.CurrentLanguageID);
                    var c = ScgDbQueryProvider.SCGDbStatusLangQuery.FindStatusLangCriteria(documentTypeDMT, UserAccount.CurrentLanguageID);
                    var d = c.Where(t => t.strID != PaymentType.CQ);
                    ctlReturnPaymentType.DataSource = d;
                    ctlReturnPaymentType.DataTextField = "strSymbol";
                    ctlReturnPaymentType.DataValueField = "strID";
                    ctlReturnPaymentType.DataBind();

                    //if (ctlPaymentType.Items.Count > 0)
                    //    ctlReturnPaymentType.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));

                    ctlReturnPaymentType.SelectedValue = fixedadvanceRow.ReturnPaymentType.ToString();
                    //ctlServiceTeam.Style.Add("display", "none");
                    ctlDivServiceTeam.Visible = false;
                }
                else
                {
                    //ctlReturnPaymentType.DataSource = ScgDbQueryProvider.SCGDbStatusLangQuery.FindStatusLangCriteria(documentTypeDMT, UserAccount.CurrentLanguageID);
                    var c = ScgDbQueryProvider.SCGDbStatusLangQuery.FindStatusLangCriteria(documentTypeDMT, UserAccount.CurrentLanguageID);
                    var d = c.Where(t => t.strID != PaymentType.CQ);
                    ctlReturnPaymentType.DataSource = d;
                    ctlReturnPaymentType.DataTextField = "strSymbol";
                    ctlReturnPaymentType.DataValueField = "strID";
                    ctlReturnPaymentType.DataBind();

                    //if (ctlReturnPaymentType.Items.Count > 0)
                    //    ctlReturnPaymentType.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
                    ctlReturnPaymentType.SelectedValue = "CA";
                    //ctlServiceTeam.Style.Add("display", "none");
                    ctlDivServiceTeam.Visible = false;
                }

                if (!fixedadvanceRow.IsReturnPBIDNull())
                {
                    ctlReturnCounterCashier.DataSource = ScgDbQueryProvider.DbPBQuery.GetPbListItem(UIHelper.ParseLong(ctlCompanyField.CompanyID), UserAccount.CurrentLanguageID);
                    ctlReturnCounterCashier.DataBind();
                    if (ctlReturnCounterCashier.Items.Count > 0)
                        ctlReturnCounterCashier.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));

                    ctlReturnCounterCashier.SelectedValue = fixedadvanceRow.ReturnPBID.ToString();
                }
                else
                {
                    ctlReturnCounterCashier.DataSource = ScgDbQueryProvider.DbPBQuery.GetPbListItem(UIHelper.ParseLong(ctlCompanyField.CompanyID), UserAccount.CurrentLanguageID);
                    ctlReturnCounterCashier.DataBind();
                    if (ctlReturnCounterCashier.Items.Count > 0)
                        ctlReturnCounterCashier.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
                }

                if (fixedadvanceRow.FixedAdvanceType == 1)
                {
                    New.Checked = true;
                    DivRefFixedAdvance.Visible = false;
                    DivPrevionsAmount.Visible = false;
                    DivCtlNetReceive.Visible = false;
                }
                else if (fixedadvanceRow.FixedAdvanceType == 2)
                {
                    Adjust.Checked = true;
                    DivRefFixedAdvance.Visible = true;
                    DivPrevionsAmount.Visible = true;
                    DivCtlNetReceive.Visible = true;
                }

                if (fixedadvanceRow.NetAmount != 0)
                {
                    ctlNetReceiveValue.Text = UIHelper.BindDecimal(fixedadvanceRow.NetAmount.ToString());
                }
                else
                {
                    ctlNetReceiveValue.Text = "0.00";
                }
                ctlObjectiveValue.Text = fixedadvanceRow.Objective;
                /*check before bind paymenttype dropdown*/
                //var a = ScgDbQueryProvider.SCGDbStatusLangQuery.FindStatusLangCriteria(documentTypeDMT, UserAccount.CurrentLanguageID);
                //var b = a.Where(x => x.strID != PaymentType.CA);
                //ctlPaymentType.DataSource = b;
                //ctlPaymentType.Items.Remove(PaymentType.CA);
                //ctlPaymentType.DataTextField = "strSymbol";
                //ctlPaymentType.DataValueField = "strID";
                //ctlPaymentType.DataBind();
                //if (ctlPaymentType.Items.Count > 0)
                //{
                //    ctlPaymentType.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
                //}
                //ctlPaymentType.SelectedValue = fixedadvanceRow.PaymentType.ToString();

                /*check payment type*/
                if (fixedadvanceRow.PaymentType != null)
                {
                    selectpaymenttype = fixedadvanceRow.PaymentType.ToString();
                    if (fixedadvanceRow.PaymentType.ToString() != "CA")
                    {
                        bindPaymentType(PaymentType.CA, selectpaymenttype);

                    }
                    else
                    {
                        bindPaymentType(PaymentType.CQ, selectpaymenttype);
                    }
                }
                ctlCounterCashier.DataSource = ScgDbQueryProvider.DbPBQuery.GetPbListItem(UIHelper.ParseLong(ctlCompanyField.CompanyID), UserAccount.CurrentLanguageID);
                ctlCounterCashier.DataBind();
                if (ctlCounterCashier.Items.Count > 0)
                {
                    ctlCounterCashier.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
                }
                if (!fixedadvanceRow.IsPBIDNull())
                {
                    ctlCounterCashier.SelectedValue = fixedadvanceRow.PBID.ToString();
                }

                SS.Standard.WorkFlow.DTO.WorkFlow wf = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(fixedadvanceRow.DocumentID);

                if (fixedadvanceRow.FixedAdvanceType != 1)
                {
                    if (!fixedadvanceRow.IsRefFixedAdvanceIDNull())
                    {
                        ctlReferredFixedAdvance.Items.Clear();
                        if (this.InitialFlag.Equals(FlagEnum.EditFlag) && wf.CurrentState.Name != "Draft")
                        {
                            ctlReferredFixedAdvance.DataSource = FixedAdvanceDocumentService.FindFixedAdvanceCanRef(UIHelper.ParseLong(ctlCompanyField.CompanyID), UIHelper.ParseLong(ctlCreatorData.UserID), UIHelper.ParseLong(ctlRequesterData.UserID), DocumentID, DocumentID);
                        }
                        else
                        {
                            ctlReferredFixedAdvance.DataSource = FixedAdvanceDocumentService.FindRefFixedAdvance(UIHelper.ParseLong(ctlCompanyField.CompanyID), UIHelper.ParseLong(ctlCreatorData.UserID), UIHelper.ParseLong(ctlRequesterData.UserID), DocumentID, "All");
                        }
                       
                        ctlReferredFixedAdvance.DataBind();
                        if (ctlReferredFixedAdvance.Items.Count > 1)
                        {
                            ctlReferredFixedAdvance.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
                            ctlReferredFixedAdvance.SelectedValue = fixedadvanceRow.RefFixedAdvanceID.ToString();
                        }
                        
                        FixedAdvanceDocument result = FixedAdvanceDocumentService.FindNetAmount(UIHelper.ParseLong(ctlReferredFixedAdvance.SelectedItem.Value));
                        ctlPrevionsAmountValue.Text = String.Format("{0:#,##0.00}", result.Amount);
                        ctlAmountGetValue.Text = Convert.ToString(UIHelper.ParseDouble(ctlAmountValue.Text) - UIHelper.ParseDouble(ctlPrevionsAmountValue.Text));
                        if (UIHelper.ParseDouble(ctlAmountGetValue.Text) > 0)
                        {
                            ctlNetReceiveValue.Text = String.Format("{0:#,##0.00}", UIHelper.ParseDouble(ctlAmountGetValue.Text));
                            ctlReceiveCashFromCompany.Style.Add("display", "block");
                            ctlPayBackCompany.Style.Add("display", "none");
                            //selectpaymenttype = "TR";
                            bindPaymentType(PaymentType.CA, selectpaymenttype);
                            ctlDivServiceTeam.Visible = true;
                        }
                        else if (UIHelper.ParseDouble(ctlAmountGetValue.Text) < 0)
                        {
                            ctlNetReceiveValue.Text = String.Format("{0:#,##0.00}", UIHelper.ParseDouble(ctlAmountGetValue.Text) * -1);
                            ctlPayBackCompany.Style.Add("display", "block");
                            //selectpaymenttype = "CA";
                            bindPaymentType(PaymentType.CQ, selectpaymenttype);
                            ctlReceiveCashFromCompany.Style.Add("display", "none");
                            ctlDivServiceTeam.Visible = false;
                        }
                        else
                        {
                            ctlNetReceiveValue.Text = ctlAmountGetValue.Text;
                            ctlPayBackCompany.Style.Add("display", "none");
                            ctlReceiveCashFromCompany.Style.Add("display", "none");
                            bindPaymentType(PaymentType.CA, selectpaymenttype);
                            ctlDivServiceTeam.Visible = false;
                        }
                    }
                }
                else
                {
                    CheckShowMessageOutstanding(InitialFlag);
                    ctlAmountGetValue.Text = Convert.ToString(UIHelper.ParseDouble(ctlAmountValue.Text) - UIHelper.ParseDouble(ctlPrevionsAmountValue.Text));
                    if (UIHelper.ParseDouble(ctlAmountGetValue.Text) > 0)
                    {
                        ctlNetReceiveValue.Text = String.Format("{0:#,##0.00}", UIHelper.ParseDouble(ctlAmountGetValue.Text));
                        ctlReceiveCashFromCompany.Style.Add("display", "block");
                        ctlPayBackCompany.Style.Add("display", "none");
                        bindPaymentType(PaymentType.CA, selectpaymenttype);
                        ctlPrevionsAmountValue.Text = "0.00";
                    }
                    else if (UIHelper.ParseDouble(ctlAmountGetValue.Text) < 0)
                    {
                        ctlNetReceiveValue.Text = String.Format("{0:#,##0.00}", UIHelper.ParseDouble(ctlAmountGetValue.Text) * -1);
                        ctlPayBackCompany.Style.Add("display", "block");
                        bindPaymentType(PaymentType.CQ, selectpaymenttype);
                        ctlReceiveCashFromCompany.Style.Add("display", "none");
                        ctlPrevionsAmountValue.Text = "0.00";
                    }
                    else
                    {
                        ctlNetReceiveValue.Text = ctlAmountGetValue.Text;
                        ctlPayBackCompany.Style.Add("display", "none");
                        ctlReceiveCashFromCompany.Style.Add("display", "none");
                        ctlPrevionsAmountValue.Text = "0.00";
                        bindPaymentType(PaymentType.CA, selectpaymenttype);
                    }
                }

                if (fixedadvanceRow.IsAmountNull())  // amountTHB???
                {
                    ctlAmountValue.Text = "0.00";
                }
                else
                {
                    ctlAmountValue.Text = UIHelper.BindDecimal(fixedadvanceRow.Amount.ToString());
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

                    if (ctlServiceTeam.Items.FindByValue(fixedadvanceRow.ServiceTeamID.ToString()) != null)
                        ctlServiceTeam.SelectedValue = fixedadvanceRow.ServiceTeamID.ToString();
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

                /*not use returnserviceteam when return money to company*/
                //ctlReturnServiceTeam.DataSource = ScgDbQueryProvider.DbLocationQuery.FindByLocationCompany(UIHelper.ParseLong(ctlCompanyField.CompanyID), null, UserAccount.UserID, locationID);
                //ctlReturnServiceTeam.DataBind();
                //if (ctlReturnServiceTeam.Items.Count > 1)
                //{
                //    ctlReturnServiceTeam.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
                //    if (!fixedadvanceRow.IsReturnServiceTeamIDNull())
                //        ctlReturnServiceTeam.SelectedValue = fixedadvanceRow.ReturnServiceTeamID.ToString();
                //    else
                //    {
                //        IList<DbServiceTeamLocation> listLocation = ScgDbQueryProvider.DbServiceTeamLocationQuery.FindServiceTeamByLocationID(locationID);
                //        if (listLocation != null && listLocation.Count > 0)
                //        {
                //            if (ctlReturnServiceTeam.Items.FindByValue(listLocation.First<DbServiceTeamLocation>().ServiceTeamID.ToString()) != null)
                //                ctlReturnServiceTeam.SelectedValue = listLocation.First<DbServiceTeamLocation>().ServiceTeamID.ToString();
                //        }
                //        else
                //        {
                //            ctlReturnServiceTeam.SelectedIndex = 0;
                //        }
                //    }
                //}
                //else
                //{
                //    ctlReturnServiceTeam.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
                //    if (!fixedadvanceRow.IsReturnServiceTeamIDNull())
                //        ctlReturnServiceTeam.SelectedValue = fixedadvanceRow.ReturnServiceTeamID.ToString();
                //    else
                //    {
                //        ctlReturnServiceTeam.SelectedIndex = 0;
                //    }
                //}


                #endregion
                #region Bind Memo Tab
                if (!string.IsNullOrEmpty(document.Memo))
                    ctlMemo.Text = document.Memo;
                else
                    ctlMemo.Text = string.Empty;
                #endregion

                #region viewPost
                if (VisibleFields.Contains(FixedAdvanceFieldGroup.VerifyDetail))
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
                            if (VisibleFields.Contains(FixedAdvanceFieldGroup.VerifyDetail) && !this.InitialFlag.Equals(FlagEnum.ViewFlag))
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
                            ctlBaselineDate.Text = UIHelper.ToDateString(fixedadvanceRow.RequestDate);
                        else
                            ctlBaselineDate.Text = string.Empty;
                    }

                    if (!string.IsNullOrEmpty(document.PostingStatus))
                        ctlPostingStatusDomestic.Text = GetMessage(string.Format("PostingStatus{0}", document.PostingStatus));
                    else
                        ctlPostingStatusDomestic.Text = GetMessage("PostingStatusN");


                    if (fixedadvanceRow.NetAmount > 0)
                    {
                        DivBankAccountGL.Visible = false;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(document.BankAccount.Trim()))
                        {
                            BankAccountGL.Text = document.BankAccount;
                        }
                    }

                    /*New Supplementary*/
                    if (string.IsNullOrEmpty(document.Supplementary) )
                    {
                        DefaultSupplementary();
                        
                    }
                    else
                    {
                        ctlSupplementary.Text = document.Supplementary.ToString();
                    }

                    #endregion
                }
                #endregion

                #region ReturnviewPost
                if (VisibleFields.Contains(FixedAdvanceFieldGroup.ClearingReturn))
                {
                    #region Domestic
                    ctlPaymentMethodReturnPost.DataSource = ScgDbQueryProvider.DbCompanyPaymentMethodQuery.GetPaymentMethod(UIHelper.ParseLong(ctlCompanyField.CompanyID));
                    ctlPaymentMethodReturnPost.DataTextField = "PaymentMethodCode";
                    ctlPaymentMethodReturnPost.DataValueField = "PaymentMethodID";

                    ctlPaymentMethodReturnPost.DataBind();

                    if (ctlPaymentMethodReturnPost.Items.Count > 0)
                    {
                        if (this.InitialFlag.Equals(FlagEnum.ViewFlag))
                            ctlPaymentMethodReturnPost.Items.Insert(0, new ListItem(string.Empty, string.Empty));
                        else
                            ctlPaymentMethodReturnPost.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
                    }


                    if (!string.IsNullOrEmpty(fixedadvanceRow.BranchCodeReturn.Trim()))
                        ctlBranchRetunr.Text = fixedadvanceRow.BranchCodeReturn;
                    else
                    {
                        if (this.InitialFlag.Equals(FlagEnum.EditFlag))
                            ctlBranchRetunr.Text = "0001"; //case edit must be default branch = 0001. modify by meaw. (log#1829)
                        else
                            ctlBranchRetunr.Text = string.Empty;
                    }

                    if (!string.IsNullOrEmpty(fixedadvanceRow.FixedAdvanceBankAccount.Trim()))
                    {
                        ctlBankAccount.Text = fixedadvanceRow.FixedAdvanceBankAccount;
                    }

                    if (!fixedadvanceRow.IsPaymentMethodIDReturnNull() && fixedadvanceRow.PaymentMethodIDReturn != 0)
                        ctlPaymentMethodReturnPost.SelectedValue = fixedadvanceRow.PaymentMethodIDReturn.ToString();
                    else
                    {
                        if (ctlPaymentMethodReturnPost.Items.Count > 0)
                        {
                            if (VisibleFields.Contains(FixedAdvanceFieldGroup.ClearingReturn) && !this.InitialFlag.Equals(FlagEnum.ViewFlag))
                            {
                                DbCompany comp = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(UIHelper.ParseLong(ctlCompanyField.CompanyID));
                                if (comp != null)
                                {
                                    switch (ctlPaymentType.SelectedValue)
                                    {
                                        case PaymentType.CA:
                                            if (comp.PaymentMethodPetty != null)
                                                ctlPaymentMethodReturnPost.SelectedValue = comp.PaymentMethodPetty.PaymentMethodID.ToString();
                                            break;

                                        case PaymentType.TR:
                                            if (comp.PaymentMethodTransfer != null)
                                                ctlPaymentMethodReturnPost.SelectedValue = comp.PaymentMethodTransfer.PaymentMethodID.ToString();
                                            break;

                                        case PaymentType.CQ:
                                            if (comp.PaymentMethodCheque != null)
                                                ctlPaymentMethodReturnPost.SelectedValue = comp.PaymentMethodCheque.PaymentMethodID.ToString();
                                            break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            ctlPaymentMethodReturnPost.SelectedValue = string.Empty;
                        }
                    }
                    if (!fixedadvanceRow.PostingDateReturn.Equals(DateTime.MinValue))
                        ctlPostingDateReturn.DateValue = UIHelper.ToDateString(document.PostingDate);
                    else
                    {
                        if (this.InitialFlag.Equals(FlagEnum.EditFlag))
                            ctlPostingDateReturn.DateValue = UIHelper.ToDateString(DateTime.Today);
                        else
                            ctlPostingDateReturn.DateValue = string.Empty;
                    }

                    if (!fixedadvanceRow.BaseLineDateReturn.Equals(DateTime.MinValue))
                        ctlBaselineDateReturn.Text = UIHelper.ToDateString(fixedadvanceRow.BaseLineDateReturn);
                    else
                    {
                        //modify by meaw. (log#1829)
                        if (this.InitialFlag.Equals(FlagEnum.EditFlag))
                            ctlBaselineDateReturn.Text = UIHelper.ToDateString(fixedadvanceRow.ReturnRequestDate);
                        else
                            ctlBaselineDateReturn.Text = string.Empty;
                    }

                    if (!string.IsNullOrEmpty(fixedadvanceRow.PostingStatusReturn))
                        ctlPostingStatusReturn.Text = GetMessage(string.Format("PostingStatus{0}", fixedadvanceRow.PostingStatusReturn));
                    else
                        ctlPostingStatusReturn.Text = GetMessage("PostingStatusN");


                    if (fixedadvanceRow.ReturnPaymentType != "")
                    {
                        DivBankAccountText.Visible = true;
                        DivBankAccountGL.Visible = true;
                    }
                    else
                    {
                        DivBankAccountText.Visible = false;
                        DivBankAccountGL.Visible = false;
                    }
                    #endregion
                }
                #endregion
            }
            
            ctlOutstandingGrid.DataCountAndBind();
            FixedAdvanceOutstandingGrid.DataCountAndBind();
            InVisibleDummyDateFrom();
            hideCounterCashier();
            hideReturnCounterCashier();
            ctlUpdatePanelHeader.Update();
            ctlUpdatePanelGeneral.Update();
            ctlUpdatePanelApprover.Update();
            ctlUpdatePanelOutstanding.Update();
            ctlUpdatePanelInitial.Update();
            ctlUpdatePanelAttachment.Update();
            ctlUpdatePanelMemo.Update();
            ctlUpdatePanelViewPost.Update();
            ctlUpdatePanelValidation.Update();
            ctlUpdatePanelViewPostReturn.Update();
        }

       
        private void bindPaymentType(string condition, string select)
        {
            var a = ScgDbQueryProvider.SCGDbStatusLangQuery.FindStatusLangCriteria(documentTypeDMT, UserAccount.CurrentLanguageID);
            /*เพิ่มcheck company default เป็นอะไร */
            if (select == "CA")
            {
                var b = a.Where(x => x.strID != "CQ");
                ctlPaymentType.DataSource = b;
            }
            else if (select == "CQ")
            {
                var b = a.Where(x => x.strID != "CA");
                ctlPaymentType.DataSource = b;
            }
            else
            {
                var b = a.Where(x => x.strID != condition);
                ctlPaymentType.DataSource = b;
            }

            //ctlPaymentType.Items.Remove(PaymentType.CA);
            ctlPaymentType.DataTextField = "strSymbol";
            ctlPaymentType.DataValueField = "strID";
            ctlPaymentType.DataBind();
            if (ctlPaymentType.Items.Count > 0)
            {
                ctlPaymentType.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
            }
            if (select != "")
            {
                ctlPaymentType.SelectedValue = select;
            }
            else
            {
                ctlPaymentType.SelectedValue = DefaultPayment(Adjust.Checked, checkpaybackcompany());
            }
        }

        private string DefaultPayment(bool AdjustCheck, bool Paybackcompany)
        {
            string defaultpayment = string.Empty;
            if (!string.IsNullOrEmpty(ctlCompanyField.CompanyID))
            {
                DbCompany company = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(UIHelper.ParseLong(ctlCompanyField.CompanyID));
                defaultpayment = company.PaymentType;
            }
            else
            {
                defaultpayment = "TR";
            }

            if (AdjustCheck)
            {
                if (Paybackcompany)
                {
                    return "CA";
                }
                else
                {
                    return defaultpayment;
                }
            }
            else
            {
                return defaultpayment;
            }
        }
        
        public void DefaultSupplementary()
        {
            if (IsContainEditableFields(SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.VerifyDetail))
            {
                if (ctlPaymentType.SelectedValue == PaymentType.CQ)
                {
                    FixedAdvanceDataSet fixedavDocTemp = (FixedAdvanceDataSet)TransactionService.GetDS(this.TransactionID);
                    FixedAdvanceDataSet.FixedAdvanceDocumentRow docRow = fixedavDocTemp.FixedAdvanceDocument.FindByFixedAdvanceID(this.DocumentID);           
                    FixedAdvanceDocument fixedavDoc = new FixedAdvanceDocument(docRow.FixedAdvanceID);

                    Dbpb pb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(fixedavDoc.PBID == null ? 0 : fixedavDoc.PBID.Pbid);
                    ctlSupplementary.Text = pb == null ? string.Empty : pb.Supplementary;
                }
                else
                {
                    ctlSupplementary.Text = string.Empty;
                }
            }
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

            long tempFixedAvDocumentID = this.DocumentID;
            FixedAdvanceDataSet fixedavDocTemp = (FixedAdvanceDataSet)TransactionService.GetDS(this.TransactionID);

            FixedAdvanceDataSet.FixedAdvanceDocumentRow docRow = fixedavDocTemp.FixedAdvanceDocument.FindByFixedAdvanceID(tempFixedAvDocumentID);
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

            if (!string.IsNullOrEmpty(ctlApproverData.UserID))
                document.ApproverID = new SS.SU.DTO.SuUser(UIHelper.ParseLong(ctlApproverData.UserID));

            document.DocumentType = new DocumentType(DocumentTypeID.FixedAdvanceDocument);

            document.Subject = ctlSubject.Text;
            document.Memo = ctlMemo.Text;
            document.BankAccount = BankAccountGL.Text;

            if (!string.IsNullOrEmpty(ctlBranchDomestic.Text.Trim()))
                document.BranchCode = ctlBranchDomestic.Text;
            else
                document.BranchCode = "0001";

            if (!string.IsNullOrEmpty(ctlPaymentMethodDomestic.SelectedValue))
                document.PaymentMethodID = UIHelper.ParseLong(ctlPaymentMethodDomestic.SelectedValue);
            else
            {
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
            }
            if (!string.IsNullOrEmpty(ctlSupplementary.Text.Trim()))
            {
                document.Supplementary = ctlSupplementary.Text.Trim();
            }

            try
            {
                if (!string.IsNullOrEmpty(ctlPostingDateCalendarDomestic.DateValue))
                    document.PostingDate = UIHelper.ParseDate(ctlPostingDateCalendarDomestic.DateValue);
                else
                    document.PostingDate = DateTime.Today;

                document.BaseLineDate = UIHelper.ParseDate(ctlRequestDate.Text);
            }
            catch (FormatException fex)
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Invalid_Date_Format"));
                throw new ServiceValidationException(errors);
            }


            document.Active = true;
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

            #region FixedAdvanceDocument
            FixedAdvanceDocument fixedavDoc = new FixedAdvanceDocument(docRow.FixedAdvanceID);
            fixedavDoc.DocumentID = document;

            if (!ctlCounterCashier.SelectedValue.Equals(string.Empty))
                fixedavDoc.PBID = new Dbpb(UIHelper.ParseLong(ctlCounterCashier.SelectedValue));
            /*alway use PBID when return*/
            if (!ctlReturnCounterCashier.SelectedValue.Equals(string.Empty))
                fixedavDoc.ReturnPBID = new Dbpb(UIHelper.ParseLong(ctlReturnCounterCashier.SelectedValue));
            if (!ctlServiceTeam.SelectedValue.Equals(string.Empty))
                fixedavDoc.ServiceTeamID = new DbServiceTeam(UIHelper.ParseLong(ctlServiceTeam.SelectedValue));
            /*not use ReturnServiceTeam when return*/
            //fixedavDoc.ReturnServiceTeamID = new DbServiceTeam(UIHelper.ParseLong(ctlReturnServiceTeam.SelectedValue));

            fixedavDoc.FixedAdvanceType = New.Checked ? (byte)FixedAdvanceTypeOption.New : (byte)FixedAdvanceTypeOption.Adjust;
            fixedavDoc.PaymentType = ctlPaymentType.SelectedValue;

            fixedavDoc.ReturnPaymentType = ctlReturnPaymentType.SelectedValue;

            fixedavDoc.FixedAdvanceBankAccount = ctlBankAccount.Text;

            fixedavDoc.BranchCodeReturn = ctlBranchRetunr.Text;

            if (!string.IsNullOrEmpty(ctlPaymentMethodReturnPost.SelectedValue))
                fixedavDoc.PaymentMethodIDReturn = UIHelper.ParseLong(ctlPaymentMethodReturnPost.SelectedValue);
            else
            {
                DbCompany comp = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(UIHelper.ParseLong(ctlCompanyField.CompanyID));
                if (comp != null)
                {
                    switch (ctlPaymentMethodReturnPost.SelectedValue)
                    {
                        case PaymentType.CA:
                            if (comp.PaymentMethodPetty != null)
                                fixedavDoc.PaymentMethodIDReturn = comp.PaymentMethodPetty.PaymentMethodID;
                            break;

                        case PaymentType.TR:
                            if (comp.PaymentMethodTransfer != null)
                                fixedavDoc.PaymentMethodIDReturn = comp.PaymentMethodTransfer.PaymentMethodID;
                            break;

                        case PaymentType.CQ:
                            if (comp.PaymentMethodCheque != null)
                                fixedavDoc.PaymentMethodIDReturn = comp.PaymentMethodCheque.PaymentMethodID;
                            break;
                    }
                }
            }
            try
            {
                fixedavDoc.PostingDateReturn = UIHelper.ParseDate(ctlPostingDateReturn.DateValue);
                fixedavDoc.BaseLineDateReturn = UIHelper.ParseDate(ctlBaselineDateReturn.Text);
            }
            catch (FormatException fex)
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Invalid_Date_Format"));
                throw new ServiceValidationException(errors);
            }


            try
            {

                if (ctlRequestDate.Value.HasValue)
                {
                    fixedavDoc.RequestDate = ctlRequestDate.Value.Value;
                }
                if (ctlEffectiveDateFrom.Value.HasValue)
                {
                    fixedavDoc.EffectiveFromDate = ctlEffectiveDateFrom.Value.Value;
                }
                if (ctlEffectiveDateTo.Value.HasValue)
                {
                    fixedavDoc.EffectiveToDate = ctlEffectiveDateTo.Value.Value;
                }
                fixedavDoc.Amount = UIHelper.ParseDouble(ctlAmountValue.Text);
                fixedavDoc.NetAmount = UIHelper.ParseDouble(ctlAmountGetValue.Text);
                fixedavDoc.Objective = ctlObjectiveValue.Text;
                fixedavDoc.RefFixedAdvanceID = UIHelper.ParseLong(ctlReferredFixedAdvance.SelectedValue);
                if (ctlReturnRequestDate.Value.HasValue)
                    fixedavDoc.ReturnRequestDate = ctlReturnRequestDate.Value.Value;
            }
            catch (FormatException fex)
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Invalid_Date_Format"));
                throw new ServiceValidationException(errors);
            }



            fixedavDoc.Active = true;
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

            #region save FixedAdvanceDocument
            try
            {
                FixedAdvanceDocumentService.UpdateFixedDocumentTransaction(fixedavDoc, TransactionID);
            }
            catch (ServiceValidationException ex)
            {
                errors.MergeErrors(ex.ValidationErrors);
            }
            #endregion


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

            long fixedavDocumentID = FixedAdvanceDocumentService.SaveFixedAdvance(this.TransactionID, this.DocumentID);
            TransactionService.Commit(this.TransactionID);

            long workFlowID = 0;
            // Get avDocument
            FixedAdvanceDocument fixedAvDocument = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.FindProxyByIdentity(fixedavDocumentID);
            // Save New WorkFlow.
            #region Save WorkFlow
            if ((fixedAvDocument != null) && (fixedAvDocument.DocumentID != null))
            {
                SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.FindProxyByIdentity(fixedAvDocument.DocumentID.DocumentID);
                SS.Standard.WorkFlow.DTO.WorkFlow workFlow = new SS.Standard.WorkFlow.DTO.WorkFlow();

                workFlow.WorkFlowType = new SS.Standard.WorkFlow.DTO.WorkFlowType(WorkFlowTypeID.FixedAdvanceWorkFlow);
                workFlow.CurrentState = WorkFlowQueryProvider.WorkFlowStateQuery.FindWorkFlowStateIDByTypeIDAndStateName(WorkFlowTypeID.FixedAdvanceWorkFlow, WorkFlowStateFlag.Draft);
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

        #region BindTabGeneral()
        private void BindTabGeneral()
        {
            short languageID = UserAccount.CurrentLanguageID;


            ctlDivDomesticPaymentType.Visible = true;
            ctlDivDomesticAmount.Visible = true;
            ctlDivViewDetailDomestic.Visible = true;
            ctlDivViewDetailDomesticReturn.Visible = true;

            if (!Page.IsPostBack)
            {
                //var a = ScgDbQueryProvider.SCGDbStatusLangQuery.FindStatusLangCriteria(documentTypeDMT, languageID);
                //var b = a.Where(x => x.strID != PaymentType.CA);
                //ctlPaymentType.DataSource = b;
                //ctlPaymentType.DataTextField = "strSymbol";
                //ctlPaymentType.DataValueField = "strID";
                //ctlPaymentType.DataBind();
                //ctlPaymentType.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
                bindPaymentType(PaymentType.CA, selectpaymenttype);
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


                //hide verify detail
                if (VisibleFields.Contains(FixedAdvanceFieldGroup.VerifyDetail))
                {
                    ctlDivViewDetailDomestic.Visible = true;
                }
                else
                {
                    ctlDivViewDetailDomestic.Visible = false;
                }

                //hide verify detail
                if (VisibleFields.Contains(FixedAdvanceFieldGroup.ClearingReturn))
                {
                    ctlDivViewDetailDomesticReturn.Visible = true;
                }
                else
                {
                    ctlDivViewDetailDomesticReturn.Visible = false;
                }

            }
            hideCounterCashier();
            hideReturnCounterCashier();

            ctlUpdatePanelViewPost.Update();
            ctlUpdatePanelViewPostReturn.Update();
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

        #region ctlCompanyField_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        protected void ctlCompanyField_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            DbCompany company = (DbCompany)e.ObjectReturn;
            // company เปลี่ยนต้อง query Counter cashier ใหม่
            if (company != null)
            {
                /*warnning message*/
                CheckShowMessageOutstanding(InitialFlag);
                ctlCounterCashier.DataSource = ScgDbQueryProvider.DbPBQuery.GetPbListItem(company.CompanyID, UserAccount.CurrentLanguageID);
                ctlCounterCashier.DataBind();
                if (ctlCounterCashier.Items.Count > 1)
                {
                    if (!ctlCounterCashier.Items[0].Value.Equals(string.Empty))
                    {
                        ctlCounterCashier.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
                    }
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
                FindRefFixedAdvance();
                hideCounterCashier();
            }
            ctlOutstandingGrid.DataCountAndBind();
            FixedAdvanceOutstandingGrid.DataCountAndBind();
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
                hideCounterCashier();
                hideReturnCounterCashier();
                ctlReceiverData.SetValue(user.Userid);
                this.TempRequesterID = user.Userid;
                ctlApproverData.ShowDefaultApprover(UIHelper.ParseLong(ctlRequesterData.UserID));
                ctlInitiator.RequesterID = UIHelper.ParseLong(ctlRequesterData.UserID);
                if (ctlFixedAdvanceType.Text == AdvanceTypeEnum.Domestic)
                {
                    BindServiceTeam();
                }
                FindRefFixedAdvance();
                /*warnning message*/
                CheckShowMessageOutstanding(InitialFlag);
            }
            ctlOutstandingGrid.DataCountAndBind();
            FixedAdvanceOutstandingGrid.DataCountAndBind();
            ctlUpdatePanelOutstanding.Update();
            ctlUpdatePanelGeneral.Update();
            ctlUpdatePanelApprover.Update();
        }
        //protected void ctlReceiverData_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        //{
        //    CheckRequesterAndReceiverIsSamePerson();
        //}
        #endregion

        protected void ctlReceiverData_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            string ErrorMessage = "";
            //if (ctlPaymentType.SelectedValue == "TR")
            //{
            //    ErrorMessage = "Can't edit reciever on payment type transfer.";
            //    ctlReceiverData.SetValue(this.TempRequesterID);
            //    ctlUpdatePanelValidation.Update();
            //}
            //else
            //{
            if (e.ObjectReturn != null)
            {
                SuUser userInfo = (SuUser)e.ObjectReturn;
                ctlReceiverData.SetValue(userInfo.Userid);
            }
            //}
            if (!string.IsNullOrEmpty(ErrorMessage))
                ValidationErrors.AddError("Provider.Error", new Spring.Validation.ErrorMessage(ErrorMessage));

            ctlUpdatePanelHeader.Update();
            ctlUpdatePanelApprover.Update();
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

        #region IDocumentEditor Members


        public bool IsContainEditableFields(object editableFieldEnum)
        {
            return this.EditableFields.Contains(editableFieldEnum);
        }

        public bool IsContainVisibleFields(object visibleFieldEnum)
        {
            return this.VisibleFields.Contains(visibleFieldEnum);
        }

        public void Copy(long wfid)
        {
            SS.Standard.WorkFlow.DTO.Document doc = WorkFlowQueryProvider.WorkFlowQuery.GetDocumentByWorkFlowID(wfid);
            Response.Redirect(String.Format("~/Forms/SCG.eAccounting/Programs/FixedAdvanceDomesticForm.aspx?cp=1&docId={0}", doc.DocumentID));
        }

        public void EnabledViewPostButton(bool IsLock)
        {
            ctlViewPostButtonDomestic.Enabled = IsLock;
            if (isNotViewPost)
            {
                ctlUpdatePanelViewPost.Update();
            }
        }

        public void EnabledPostRemittanceButton(bool IsLock)
        {
            //throw new NotImplementedException();
        }

        public bool RequireDocumentAttachment()
        {
            return false;
        }

        #endregion

        protected void ctlReferredFixedAdvance_SelectedIndexChanged(object sender, EventArgs e)
        {
            FixedAdvanceDocument result = FixedAdvanceDocumentService.FindNetAmount(UIHelper.ParseLong(ctlReferredFixedAdvance.SelectedItem.Value));
            if (result != null)
            {
                ctlPrevionsAmountValue.Text = String.Format("{0:#,##0.00}", result.Amount);
                ctlEffectiveDateFrom.DateValue = UIHelper.ToDateString(result.EffectiveFromDate);
                ctlEffectiveDateTo.DateValue = UIHelper.ToDateString(result.EffectiveToDate);
                ctlEffectiveDateFrom.ReadOnly = true;
                ctlEffectiveDateTo.ReadOnly = true;
                //ctlEffectiveDateFromDummy.Text = String.Format("{0:M/d/yyyy}", result.EffectiveFromDate);
                //ctlEffectiveDateToDummy.Text = String.Format("{0:M/d/yyyy}", result.EffectiveToDate);
                ctlEffectiveDateFromDummy.Text = String.Format("{0:d/M/yyyy}",ctlEffectiveDateFrom.DateValue);
                ctlEffectiveDateToDummy.Text = String.Format("{0:d/M/yyyy}", ctlEffectiveDateTo.DateValue);


                VisibleDummyDateFrom();

                if (result.PaymentType != null)
                {
                    selectpaymenttype = result.PaymentType.ToString();
                }
                ctlAmountGetValue.Text = Convert.ToString(UIHelper.ParseDouble(ctlAmountValue.Text) - UIHelper.ParseDouble(ctlPrevionsAmountValue.Text));
                if (UIHelper.ParseDouble(ctlAmountGetValue.Text) > 0)
                {
                    ctlNetReceiveValue.Text = String.Format("{0:#,##0.00}", UIHelper.ParseDouble(ctlAmountGetValue.Text));
                    ctlReceiveCashFromCompany.Style.Add("display", "block");
                    ctlPayBackCompany.Style.Add("display", "none");
                    bindPaymentType(PaymentType.CA, selectpaymenttype);
                }
                else
                {
                    ctlNetReceiveValue.Text = String.Format("{0:#,##0.00}", UIHelper.ParseDouble(ctlAmountGetValue.Text) * -1);
                    ctlPayBackCompany.Style.Add("display", "block");
                    bindPaymentType(PaymentType.CQ, "CA");
                    ctlReceiveCashFromCompany.Style.Add("display", "none");
                }
            }
            hideCounterCashier();
            ctlUpdatePanelGeneral.Update();
        }

        protected void Adjust_CheckedChanged(object sender, EventArgs e)
        {
            FindRefFixedAdvance();
            ctlAmountValue.ReadOnly = false;
            InVisibleDummyDateFrom();
            DivRefFixedAdvance.Visible = true;
            DivPrevionsAmount.Visible = true;
            DivCtlNetReceive.Visible = true;
            AleartMessageFixedAdvance.Visible = false;
            ctlUpdatePanelGeneral.Update();

            //ctlServiceTeam.Style.Add("display", "none");
            hideCounterCashier();
            ctlUpdatePanelApprover.Update();
        }

        protected void New_CheckedChanged(object sender, EventArgs e)
        {
            ctlReferredFixedAdvance.Items.Clear();
            ctlEffectiveDateFrom.ReadOnly = false;
            InVisibleDummyDateFrom();
            ctlEffectiveDateTo.ReadOnly = false;
            ctlNetReceiveValue.Text = ctlAmountValue.Text;
            DivRefFixedAdvance.Visible = false;
            DivPrevionsAmount.Visible = false;
            ctlPrevionsAmountValue.Text = "0.00";
            ctlAmountValue.ReadOnly = false;
            DivCtlNetReceive.Visible = false;
            ctlUpdatePanelGeneral.Update();
            //ctlServiceTeam.Style.Add("display", "block");
            bindPaymentType(PaymentType.CA, selectpaymenttype);
            hideCounterCashier();
            ctlDivServiceTeam.Visible = true;
            CheckShowMessageOutstanding(InitialFlag);
            ctlUpdatePanelApprover.Update();
        }

        protected void ctlAmountValue_TextChanged(object sender, EventArgs e)
        {
            ctlAmountGetValue.Text = Convert.ToString(UIHelper.ParseDouble(ctlAmountValue.Text) - UIHelper.ParseDouble(ctlPrevionsAmountValue.Text));
            if (UIHelper.ParseDouble(ctlAmountGetValue.Text) > 0)
            {
                ctlNetReceiveValue.Text = String.Format("{0:#,##0.00}", UIHelper.ParseDouble(ctlAmountGetValue.Text));
                ctlReceiveCashFromCompany.Style.Add("display", "block");
                ctlPayBackCompany.Style.Add("display", "none");
                bindPaymentType(PaymentType.CA, selectpaymenttype);
            }
            else
            {
                ctlNetReceiveValue.Text = String.Format("{0:#,##0.00}", UIHelper.ParseDouble(ctlAmountGetValue.Text) * -1);
                ctlPayBackCompany.Style.Add("display", "block");
                bindPaymentType(PaymentType.CQ, selectpaymenttype);
                ctlReceiveCashFromCompany.Style.Add("display", "none");
            }
            hideCounterCashier();
            ctlUpdatePanelGeneral.Update();
        }

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

        #region RequestDataFixedAdvanceOutstanding(int startRow, int pageSize, string sortExpression)
        public Object RequestDataFixedAdvanceOutstanding(int startRow, int pageSize, string sortExpression)
        {
            long currentWorkFlowID = UIHelper.ParseLong(Request.Params["wfid"]);
            var result = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.FindFixedAdvanceOutstandingRequest(UIHelper.ParseLong(ctlRequesterData.UserID), UIHelper.ParseLong(ctlCompanyField.CompanyID), currentWorkFlowID, UserAccount.CurrentLanguageID, startRow, pageSize, sortExpression);

            return result;
        }
        #endregion

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

        #region RequestDataOutstanding(int startRow, int pageSize, string sortExpression)
        public Object RequestDataOutstanding(int startRow, int pageSize, string sortExpression)
        {
            IList<int> iListate = new List<int>();
            iListate.Add(AdvanceStateID.Draft);
            iListate.Add(AdvanceStateID.Complete);
            iListate.Add(AdvanceStateID.Cancel);
            int intDocumentType = 0;
            long currentWorkFlowID = UIHelper.ParseLong(Request.Params["wfid"]);
            intDocumentType = DocumentTypeID.AdvanceDomesticDocument;
            return ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindOutstandingRequest(UIHelper.ParseLong(ctlRequesterData.UserID), currentWorkFlowID, intDocumentType, DocumentTypeID.ExpenseDomesticDocument, iListate, UserAccount.CurrentLanguageID, startRow, pageSize, sortExpression);
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

            intDocumentType = DocumentTypeID.AdvanceDomesticDocument;
            return ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.CountFindOutstandingRequest(UIHelper.ParseLong(ctlRequesterData.UserID), currentWorkFlowID, intDocumentType, DocumentTypeID.ExpenseDomesticDocument, iListate, UserAccount.CurrentLanguageID);
        }
        #endregion

        #region RequestCountFixedAdvanceOutstanding()
        public int RequestCountFixedAdvanceOutstanding()
        {

            long currentWorkFlowID = UIHelper.ParseLong(Request.Params["wfid"]);
            return ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.CountFindFixedAdvanceOutstandingRequest(UIHelper.ParseLong(ctlRequesterData.UserID), UIHelper.ParseLong(ctlCompanyField.CompanyID), currentWorkFlowID, UserAccount.CurrentLanguageID);
        }
        #endregion

        protected void ctlViewPostButtonDomestic_Click(object sender, EventArgs e)
        {
            try
            {
                Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

                if (string.IsNullOrEmpty(ctlBranchDomestic.Text))
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Branch return is Required"));

                if (string.IsNullOrEmpty(ctlPaymentMethodDomestic.SelectedValue))
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("PaymentMethod Return is Required"));

                if (ctlPostingDateCalendarDomestic.Value == null)
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Postingdate Return is Required"));

                //if (string.IsNullOrEmpty(ctlCounterCashier.SelectedValue))
                //    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Counter Cashier Return is Required"));

                if (!errors.IsEmpty)
                    throw new ServiceValidationException(errors);

                this.isNotViewPost = false;
                FixedAdvanceDataSet advDs = (FixedAdvanceDataSet)TransactionService.GetDS(this.TransactionID);
                FixedAdvanceDataSet.FixedAdvanceDocumentRow row = advDs.FixedAdvanceDocument.FindByFixedAdvanceID(this.DocumentID);
                ctlViewPost.Initialize(row.DocumentID, DocumentKind.FixedAdvance);
                ctlViewPost.Show();
                ctlUpdatePanelViewPost.Update();
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                ctlUpdatePanelValidation.Update();
            }
        }

        protected void ctlViewPostButtonDomesticReturn_Click(object sender, EventArgs e)
        {
            try
            {
                Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

                if (string.IsNullOrEmpty(ctlBranchRetunr.Text))
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Branch retunr is Required"));

                if (string.IsNullOrEmpty(ctlPaymentMethodReturnPost.SelectedValue))
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("PaymentMethod Return is Required"));

                if (ctlPostingDateReturn.Value == null)
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Postingdate Return is Required"));

                if (string.IsNullOrEmpty(ctlReturnCounterCashier.SelectedValue))
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Return Counter Cashier Return is Required"));

                if (ctlReturnPaymentType.SelectedValue == PaymentType.TR)
                {
                    if (string.IsNullOrEmpty(ctlBankAccount.Text))
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Bank Account Return is Required"));
                }

                if (!errors.IsEmpty)
                    throw new ServiceValidationException(errors);

                this.isNotViewPost = false;
                FixedAdvanceDataSet advDs = (FixedAdvanceDataSet)TransactionService.GetDS(this.TransactionID);
                FixedAdvanceDataSet.FixedAdvanceDocumentRow row = advDs.FixedAdvanceDocument.FindByFixedAdvanceID(this.DocumentID);
                //SCGDocumentService.ValidateVerifyDetail(this.TransactionID, ViewPostDocumentType.FixedAdvance, false);
                ctlViewPostReturn.Initialize(row.DocumentID, DocumentKind.FixedAdvanceReturn);
                ctlViewPostReturn.Show();
                ctlUpdatePanelViewPostReturn.Update();
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                ctlUpdatePanelValidation.Update();
            }
        }

        public void FindRefFixedAdvance()
        {
            DbCompany comp = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(UIHelper.ParseLong(ctlCompanyField.CompanyID));
            ctlReferredFixedAdvance.Items.Clear();
            //ctlReferredFixedAdvance.DataSource = FixedAdvanceDocumentService.FindRefFixedAdvance(UIHelper.ParseLong(ctlCompanyField.CompanyID), UIHelper.ParseLong(ctlRequesterData.UserID), DocumentID, "outstandingOnly");
            /*n-edited check can reference*/
            ctlReferredFixedAdvance.DataSource = FixedAdvanceDocumentService.FindFixedAdvanceCanRef(UIHelper.ParseLong(ctlCompanyField.CompanyID), UIHelper.ParseLong(ctlCreatorData.UserID), UIHelper.ParseLong(ctlRequesterData.UserID), DocumentID, DocumentID);
            ctlReferredFixedAdvance.DataBind();
            if (ctlReferredFixedAdvance.Items.Count > 0)
            {
                ctlReferredFixedAdvance.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
            }
        }

        public void VisibleDummyDateFrom()
        {
            ctlEffectiveDateFromDummy.Visible = true;
            ctlEffectiveDateToDummy.Visible = true;
            ctlEffectiveDateTo.Visible = false;
            ctlEffectiveDateFrom.Visible = false;

        }

        public void InVisibleDummyDateFrom()
        {
            ctlEffectiveDateFromDummy.Visible = false;
            ctlEffectiveDateToDummy.Visible = false;
            ctlEffectiveDateTo.Visible = true;
            ctlEffectiveDateFrom.Visible = true;

        }

        private void CheckShowMessageOutstanding(string flag)
        {
            long currentWorkFlowID = UIHelper.ParseLong(Request.Params["wfid"]);
            DateTime curdate =DateTime.Now;
            string CurrentStateName="";
            SS.Standard.WorkFlow.DTO.WorkFlow wf = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(currentWorkFlowID);
            if (wf != null && wf.CurrentState != null && wf.CurrentState.Name != null)
                CurrentStateName = wf.CurrentState.Name;
            if (wf != null && wf.CurrentState != null && wf.UpdDate != null)
                curdate = wf.UpdDate;
            /*check fixedadvance and advance outstanding*/
            bool hasfixedadvance = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.FindFixedAdvanceOutStandingFromAleart(UIHelper.ParseLong(ctlRequesterData.UserID), UIHelper.ParseLong(ctlCompanyField.CompanyID));
            int hasadvance = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.CountAdvanceForFixedAdvance(UIHelper.ParseLong(ctlCompanyField.CompanyID), UIHelper.ParseLong(ctlRequesterData.UserID), currentWorkFlowID, DocumentTypeID.AdvanceDomesticDocument,curdate);

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


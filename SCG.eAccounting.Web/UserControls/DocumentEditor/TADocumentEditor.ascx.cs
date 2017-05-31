using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCG.DB.DTO.ValueObject;
using SCG.DB.Query;
using SCG.eAccounting.BLL;
using SCG.eAccounting.DAL;
using SCG.eAccounting.DTO;
using SCG.eAccounting.Query;
using SCG.eAccounting.Web.Helper;
using SS.DB.DTO;
using SS.DB.Query;
using SS.Standard.Security;
using SS.Standard.UI;
using SS.Standard.Utilities;
using SS.SU.BLL;
using SS.SU.DAL;
using SS.SU.DTO;
using SS.SU.Query;
using SCG.DB.BLL;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.DTO.DataSet;
using SCG.DB.DTO;
using System.Data;
using SS.Standard.WorkFlow.Query;
using SS.Standard.WorkFlow.Service;
using System.Collections;
using SCG.eAccounting.Web.UserControls.LOV.SCG.DB;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor
{
    public partial class TADocumentEditor : BaseUserControl, IDocumentEditor
    {
        #region local variable
        public ISuUserService SuUserService { get; set; }
        public ITADocumentScheduleService TADocumentScheduleService { get; set; }
        public ITADocumentService TADocumentService { get; set; }
        public ITADocumentTravellerService TADocumentTravellerService { get; set; }
        public ITADocumentAdvanceService TADocumentAdvanceService { get; set; }
        public ITransactionService TransactionService { get; set; }
        public ISCGDocumentService SCGDocumentService { get; set; }
        public IAvAdvanceDocumentService AvAdvanceDocumentService { get; set; }
        public IWorkFlowService WorkFlowService { get; set; }
        #endregion local variable

        #region Property
        public string Mode
        {
            get { return ctlMode.Text; }
            set { ctlMode.Text = value; }
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
        public IList<TADocumentObj> TADocumentObj
        {
            get { return (IList<TADocumentObj>)ViewState["TADocumentObj"]; }
            set { ViewState["TADocumentObj"] = value; }
        }
        public IList<SuUser> UserList
        {
            get { return (IList<SuUser>)ViewState["UserList"]; }
            set { ViewState["UserList"] = value; }
        }
        public bool isShowFooter
        {
            get
            {
                if (ViewState["isShowFooter"] == null)
                {
                    return true;
                }
                else
                {
                    return (bool)(ViewState["isShowFooter"]);
                }
            }
            set { ViewState["isShowFooter"] = value; }
        }
        public long RequesterID
        {
            get
            {
                if (ViewState["RequesterID"] == null)
                {
                    return 0;
                }
                else
                {
                    return UIHelper.ParseLong(ViewState["RequesterID"].ToString());
                }
            }
            set { ViewState["RequesterID"] = value; }
        }
        public string AccountCode
        {
            get
            {
                if (ViewState["AccountCode"] == null)
                {
                    return string.Empty;
                }
                else
                {
                    return ViewState["AccountCode"].ToString();
                }
            }
            set { ViewState["AccountCode"] = value; }
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
        #endregion

        #region protected override void Render(HtmlTextWriter writer)
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            if (!IsPostBack)
            {
                this.ScriptManagerJavaScript();
                ctlUpdatePanelGeneral.Update();
            }
        }
        #endregion protected override void Render(HtmlTextWriter writer)

        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Page.IsPostBack)
            //{
            //    //                string script = @"<script type='text/javascript'>
            //    //                    window.onunload = UnLock;
            //    //                    UnLockParameters['txID']='" + this.TransactionID + "';</script>";
            //    //                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "addunlockparameter", script, false);
            //}

          
            ctlCompanyField.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlCompanyField_OnObjectLookUpReturn);
            //ctlCostCenterField.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlCostCenterField_OnObjectLookUpReturn);
            ctlRequesterData.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlRequesterData_OnObjectLookUpReturn);
            ctlUserProfileLookup.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlUserProfileLookup_OnObjectLookUpReturn);
            this.CheckControlReadOnly();
        }
        #endregion protected void Page_Load(object sender, EventArgs e)

        #region public void Initialize(string initFlag, long? documentID)
        public void Initialize(string initFlag, long? documentID)
        {
            TADocumentDataSet taDocumentDS;
            long taDocumentID = 0;
            Guid txID = Guid.Empty;
            bool isCopy = false;

            #region Visible Mode
            ctlAddTraveller.Visible = true;
            ctlRefresh.Enabled = false;
            ctlCreateAdvance.Enabled = false;
            ctlTravellingInfoGrid.Columns[7].Visible = true;
            ctlTravellingScheduleGrid.Columns[7].Visible = true;
            ctlTravellerAdvanceGrid.Columns[9].Visible = false;

            this.isShowFooter = true;
            #endregion Visible Mode

            if (initFlag.Equals(FlagEnum.NewFlag) && Request.QueryString["cp"] == null)
            {
                taDocumentDS = (TADocumentDataSet)TADocumentService.PrepareDS();
                txID = TransactionService.Begin(taDocumentDS);
                taDocumentID = TADocumentService.AddTADocumentTransaction(txID);

                ctlTAFormHeader.Status = FlagEnum.NewFlag;
            }
            else if (((initFlag.Equals(FlagEnum.EditFlag) || (initFlag.Equals(FlagEnum.ViewFlag)))) && ((documentID.HasValue) && (documentID.Value != 0)))
            {
                taDocumentDS = (TADocumentDataSet)TADocumentService.PrepareDS(documentID.Value);
                txID = TransactionService.Begin(taDocumentDS);

                if (taDocumentDS.Document.Rows.Count > 0)
                {
                    taDocumentID = UIHelper.ParseLong(taDocumentDS.TADocument.Rows[0]["TADocumentID"].ToString());
                }

                #region Check Mode ViewFlag
                if (initFlag.Equals(FlagEnum.ViewFlag))
                {
                    #region Visible Mode
                    ctlAddTraveller.Visible = false;
                    ctlTravellingInfoGrid.Columns[7].Visible = false;
                    ctlTravellingScheduleGrid.Columns[7].Visible = false;

                    SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(documentID.Value);

                    if (document.CreatorID.Userid == UserAccount.UserID)
                    {
                        ctlTravellerAdvanceGrid.Columns[9].Visible = true;
                    }
                    this.isShowFooter = false;
                    #endregion Visible Mode

                    #region Bind Dropdownlist Traveller
                    UserList = ScgeAccountingQueryProvider.TADocumentAdvanceQuery.FindUserIDAdvanceByTADocumentID(taDocumentID);
                    this.BindCtlTravellerAdvanceDropdown();
                    #endregion Bind Dropdownlist Traveller

                    if (Request.QueryString["eventFlag"] == PopUpReturnType.Add.ToString())
                    {
                        ctlTabContainer.ActiveTab = ctlTabAdvance;
                    }
                }
                #endregion Check Mode ViewFlag
                else
                {
                    this.ScriptManagerJavaScript();
                }
            }
            else if (Request.QueryString["cp"] != null && UIHelper.ParseLong(Request.QueryString["cp"]) == 1)
            {
                taDocumentDS = (TADocumentDataSet)TADocumentService.PrepareDataToDataset(UIHelper.ParseLong(Request.QueryString["docId"]), true);
                txID = TransactionService.Begin(taDocumentDS);
                isCopy = true;
                if (taDocumentDS.TADocument.Rows.Count > 0)
                {
                    taDocumentID = UIHelper.ParseLong(taDocumentDS.TADocument.Rows[0]["TADocumentID"].ToString());
                }
            }
            this.TransactionID = txID;
            this.DocumentID = taDocumentID;
            this.InitialFlag = initFlag;

            // Define value to each DocumentEditor Property.
            // And store it in viewstate.
            this.VisibleFields = TADocumentService.GetVisibleFields(documentID);
            this.EditableFields = TADocumentService.GetEditableFields(documentID);

            this.InitializeControl();
            this.BindControl(isCopy);

            #region Show Tab
            // Edit By Kla 
            // 15/06/2009
            // For Log#760
            // ให้ Tab ที่มีข้อมูลโชว์ขึ้นมา ส่วน Tab ที่ไม่มีข้อมูล ไม่ต้องโชว์
            string strCurrentState = "";
            if (!string.IsNullOrEmpty(Request.Params["wfid"]) && UIHelper.ParseInt(Request.Params["wfid"]) > 0)
            {
                long workFlowID = UIHelper.ParseLong(Request.Params["wfid"]);

                SS.Standard.WorkFlow.DTO.WorkFlow wf = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
                if (wf != null && wf.CurrentState != null && wf.CurrentState.Name != null)
                    strCurrentState = wf.CurrentState.Name;
            }

            if (strCurrentState == "Draft")
            {
                ctlTabMemo.Visible = true;
                ctlTabInitial.Visible = true;
                ctlTabAttachment.Visible = true;
                ctlTabHistory.Visible = true;
                ctlTabAdvance.Visible = true;

                ctlUpdatePanelTab.Update();
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

                if (ctlTravellerAdvanceGrid.DataSource == null || ctlTravellerAdvanceGrid.Rows.Count <= 0 || (Request.QueryString["fromTASearch"] != null && UIHelper.ParseLong(Request.QueryString["fromTASearch"]) == 1))
                    ctlTabAdvance.Visible = false;
                else
                    ctlTabAdvance.Visible = true;

                ctlUpdatePanelTab.Update();
            }
            else
            {
                ctlTabMemo.Visible = true;
                ctlTabInitial.Visible = true;
                ctlTabAttachment.Visible = true;
                ctlTabHistory.Visible = true;
                ctlTabAdvance.Visible = true;

                ctlUpdatePanelTab.Update();
            }
            #endregion Show Tab
        }
        #endregion public void Initialize(string initFlag, long? documentID)

        void OnDsNull()
        {
            if (dsNullHandler != null)
                dsNullHandler();
        }
        #region public void Save()
        public long Save()
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            TADocumentDataSet taDocumentDS = (TADocumentDataSet)TransactionService.GetDS(this.TransactionID);
            if (taDocumentDS == null)
            {
                OnDsNull();
            }
            TADocumentDataSet.TADocumentRow taDocumentRow = taDocumentDS.TADocument.FindByTADocumentID(this.DocumentID);
            long tempDocumentID = taDocumentRow.DocumentID;

            #region SCGDocument
            SCGDocument scgDocument = new SCGDocument(tempDocumentID);
            if (!string.IsNullOrEmpty(ctlCompanyField.CompanyID))
            {
                scgDocument.CompanyID = new DbCompany(UIHelper.ParseLong(ctlCompanyField.CompanyID));
            }
            scgDocument.CreatorID = new SuUser(UIHelper.ParseLong(ctlCreatorData.UserID));
            if (!string.IsNullOrEmpty(ctlRequesterData.UserID))
            {
                scgDocument.RequesterID = new SuUser(UIHelper.ParseLong(ctlRequesterData.UserID));
            }
            scgDocument.Subject = ctlSubject.Text;
            if (!string.IsNullOrEmpty(ctlApproverData.UserID))
            {
                scgDocument.ApproverID = new SuUser(UIHelper.ParseLong(ctlApproverData.UserID));
            }
            if (ctlDomesticProvinceChk.Checked)
            {
                scgDocument.DocumentType = new SS.Standard.WorkFlow.DTO.DocumentType(DocumentTypeID.TADocumentDomestic);
            }
            else
            {
                scgDocument.DocumentType = new SS.Standard.WorkFlow.DTO.DocumentType(DocumentTypeID.TADocumentForeign);
            }
            scgDocument.Memo = ctlMemo.Text;
            scgDocument.Active = true;

            try
            {
                // Save SCG Document to Transaction.
                SCGDocumentService.UpdateTransactionDocument(this.TransactionID, scgDocument, false, true);
            }
            catch (ServiceValidationException ex)
            {
                errors.MergeErrors(ex.ValidationErrors);
            }
            #endregion SCGDocument

            #region TADocument
            TADocument taDocument = new TADocument(this.DocumentID);

            taDocument.DocumentID = scgDocument;

            try
            {
                if (ctlFromDateCal.Value != null)
                {
                    taDocument.FromDate = ctlFromDateCal.Value.Value;
                }

                if (ctlToDateCal.Value != null)
                {
                    taDocument.ToDate = ctlToDateCal.Value.Value;
                }
            }
            catch (FormatException fex)
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Invalid_Date_Format"));
                throw new ServiceValidationException(errors);
            }

            if (ctlBusinessChk.Checked)
            {
                taDocument.IsBusinessPurpose = true;
            }

            if (ctlTrainingChk.Checked)
            {
                taDocument.IsTrainningPurpose = true;
            }

            if (ctlOtherChk.Checked)
            {
                taDocument.IsOtherPurpose = true;
                taDocument.OtherPurposeDescription = ctlOther.Text;
            }
            else
            {
                taDocument.OtherPurposeDescription = string.Empty;
            }

            if (ctlDomesticProvinceChk.Checked)
            {
                taDocument.TravelBy = TravellBy.Domestic;
                taDocument.Province = ctlProvince.Text;
            }
            else
            {
                taDocument.Province = string.Empty;
            }

            if (ctlAbroadCountryChk.Checked)
            {
                taDocument.TravelBy = TravellBy.Foreign;
                taDocument.Country = ctlCountry.Text;
            }
            else
            {
                taDocument.Country = string.Empty;
            }

            if ((!ctlDomesticProvinceChk.Checked) && (!ctlAbroadCountryChk.Checked))
            {
                taDocument.TravelBy = string.Empty;
            }

            if (ctlByTravellingChk.Checked)
            {
                taDocument.Ticketing = Ticketing.TravellingServiceSection;
            }

            if (ctlByEmployeeChk.Checked)
            {
                taDocument.Ticketing = Ticketing.EmployeeSection;
            }

            if ((!ctlByTravellingChk.Checked) && (!ctlByEmployeeChk.Checked))
            {
                taDocument.Ticketing = string.Empty;
            }

            taDocument.Active = true;

            try
            {
                // Get ta document information and save to transaction.
                TADocumentService.UpdateTADocumentTransaction(this.TransactionID, taDocument);
            }
            catch (ServiceValidationException ex)
            {
                errors.MergeErrors(ex.ValidationErrors);
            }

            if (!errors.IsEmpty)
            {
                throw new ServiceValidationException(errors);
            }

            #endregion TADocument

            #region TADocumentTraveller
            foreach (GridViewRow row in ctlTravellingInfoGrid.Rows)
            {
                HiddenField ctlUserId = (HiddenField)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlUserId");
                Label ctlEmployeeName = (Label)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlEmployeeName");
                TextBox ctlEmployeeNameEng = (TextBox)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlEmployeeNameEng");
                TextBox ctlAirlineMember = (TextBox)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlAirlineMember");
                Label ctlUserName = (Label)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlUserName");
                CostCenterField ctlCostCenter = (CostCenterField)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlCostCenter");
                IOAutoCompleteLookup ctlIO = (IOAutoCompleteLookup)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlIO");
                AccountField ctlAccount = (AccountField)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlAccount");

                int travellerID = UIHelper.ParseInt(ctlTravellingInfoGrid.DataKeys[row.RowIndex].Value.ToString());

                TADocumentTraveller taDocumentTraveller = new TADocumentTraveller(travellerID);

                taDocumentTraveller.TADocumentID = new TADocument(this.DocumentID);
                taDocumentTraveller.UserID = new SuUser(UIHelper.ParseLong(ctlUserId.Value));
                taDocumentTraveller.EmployeeNameEng = ctlEmployeeNameEng.Text;
                taDocumentTraveller.AirLineMember = ctlAirlineMember.Text;

                if (taDocument.Ticketing.Equals(Ticketing.TravellingServiceSection))
                {
                    taDocumentTraveller.CostCenterID = new SCG.DB.DTO.DbCostCenter(UIHelper.ParseLong(ctlCostCenter.CostCenterId));
                    taDocumentTraveller.IOID = new SCG.DB.DTO.DbInternalOrder(UIHelper.ParseLong(ctlIO.IOID));
                    taDocumentTraveller.Account = new SCG.DB.DTO.DbAccount(UIHelper.ParseLong(ctlAccount.AccountID));
                    taDocumentTraveller.UserName = ctlUserName.Text;
                }
                else 
                {
                    taDocumentTraveller.CostCenterID = null;
                    taDocumentTraveller.IOID = null;
                    taDocumentTraveller.Account = null;
                }

                taDocumentTraveller.Active = true;

                try
                {
                    // Get ta document traveller information and save to transaction.
                    TADocumentTravellerService.UpdateTADocumentTravellerTransaction(this.TransactionID, taDocumentTraveller);
                }
                catch (ServiceValidationException ex)
                {
                    errors.MergeErrors(ex.ValidationErrors);
                }
            }
            #endregion TADocumentTraveller

            #region TADocumentSchedule
            if (ctlTravellingScheduleGrid.Rows.Count > 0)
            {
                foreach (GridViewRow row in ctlTravellingScheduleGrid.Rows)
                {
                    UserControls.Calendar ctlDateCal = (UserControls.Calendar)ctlTravellingScheduleGrid.Rows[row.RowIndex].FindControl("ctlDateCal");
                    UserControls.DropdownList.SCG.DB.StatusDropdown ctlStatusDropdown = (UserControls.DropdownList.SCG.DB.StatusDropdown)ctlTravellingScheduleGrid.Rows[row.RowIndex].FindControl("ctlStatusDropdown");
                    TextBox ctlDeparturefrom = (TextBox)ctlTravellingScheduleGrid.Rows[row.RowIndex].FindControl("ctlDeparturefrom");
                    TextBox ctlArrival = (TextBox)ctlTravellingScheduleGrid.Rows[row.RowIndex].FindControl("ctlArrival");
                    UserControls.Time ctlTime = (UserControls.Time)ctlTravellingScheduleGrid.Rows[row.RowIndex].FindControl("ctlTime");
                    TextBox ctlFlightNo = (TextBox)ctlTravellingScheduleGrid.Rows[row.RowIndex].FindControl("ctlFlightNo");
                    TextBox ctlTravellingDetail = (TextBox)ctlTravellingScheduleGrid.Rows[row.RowIndex].FindControl("ctlTravellingDetail");

                    int scheduleID = UIHelper.ParseInt(ctlTravellingScheduleGrid.DataKeys[row.RowIndex].Value.ToString());

                    TADocumentSchedule taDocumentSchedule = new TADocumentSchedule(scheduleID);

                    taDocumentSchedule.TADocumentID = new TADocument(this.DocumentID);
                    if (ctlDateCal.DateValue != null)
                    {
                        taDocumentSchedule.Date = ctlDateCal.Value;
                    }
                    taDocumentSchedule.DepartureFrom = ctlDeparturefrom.Text;
                    taDocumentSchedule.ArrivalAt = ctlArrival.Text;
                    taDocumentSchedule.TravelBy = ctlStatusDropdown.SelectedValue;
                    taDocumentSchedule.Time = ctlTime.TimeValue;
                    taDocumentSchedule.FlightNo = ctlFlightNo.Text;
                    taDocumentSchedule.TravellingDetail = ctlTravellingDetail.Text;
                    taDocumentSchedule.Active = true;

                    try
                    {
                        // Get ta document schedule information and save to transaction.
                        TADocumentScheduleService.UpdateTADocumentScheduleTransaction(this.TransactionID, taDocumentSchedule);
                    }
                    catch (ServiceValidationException ex)
                    {
                        errors.MergeErrors(ex.ValidationErrors);
                    }
                }
            }
            else
            {
                try
                {
                    // Get ta document schedule information and save to transaction.
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("TravellingDateIsRequired"));
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("TravellingDateIsNotBetweenTravellingFromToDate"));
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("DepartureFromIsRequired"));
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("ArrivalAtIsRequired"));
                }
                catch (ServiceValidationException ex)
                {
                    errors.MergeErrors(ex.ValidationErrors);
                }
            }
            #endregion TADocumentSchedule

            try
            {
                ctlInitiator.Save();
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
            else
            {
                return this.SaveToDatabase();
            }
        }
        #endregion public void SaveToTransaction()

        #region public long SaveToDatabase()
        public long SaveToDatabase()
        {
            // Save all table in dataset to database and clear transaction.
            long taDocumentID = TADocumentService.SaveTADocument(this.TransactionID, this.DocumentID);
            // Get ta document
            TADocument taDocument = ScgeAccountingQueryProvider.TADocumentQuery.FindProxyByIdentity(taDocumentID);
            IList<Advance> advanceList = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindAdvanceDocumentByTADocumentID(taDocumentID);

            if (advanceList.Count > 0)
            {
                foreach (Advance adv in advanceList)
                {
                    SCGDocumentService.UpdateAdvanceSCGDocument(taDocument.DocumentID.DocumentID, adv.DocumentID);
                }
            }

            TransactionService.Commit(this.TransactionID);

            #region Work Flow
            long workFlowID = 0;

            // Save New WorkFlow.
            if ((taDocument != null) && (taDocument.DocumentID != null))
            {
                SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.FindProxyByIdentity(taDocument.DocumentID.DocumentID);
                SS.Standard.WorkFlow.DTO.WorkFlow workFlow = new SS.Standard.WorkFlow.DTO.WorkFlow();
                // WorkFlow Type ID = 4 is TA Workflow Image. wait confirm where to store this data.
                workFlow.WorkFlowType = new SS.Standard.WorkFlow.DTO.WorkFlowType(WorkFlowTypeID.TAWorkFlowType);
                // WorkFlow Type ID = 14 is TA Workflow Image. wait confirm where to store this data.
                workFlow.CurrentState = WorkFlowQueryProvider.WorkFlowStateQuery.FindWorkFlowStateIDByTypeIDAndStateName(WorkFlowTypeID.TAWorkFlowType, WorkFlowStateFlag.Draft);
                workFlow.Description = null;
                workFlow.Document = document;
                workFlow.Active = true;
                workFlow.CreBy = document.CreBy;
                workFlow.CreDate = document.CreDate;
                workFlow.UpdBy = document.UpdBy;
                workFlow.UpdDate = document.UpdDate;
                workFlow.UpdPgm = document.UpdPgm;

                workFlowID = WorkFlowService.CheckExistAndAddNew(workFlow);
            }
            #endregion Work Flow

            return workFlowID;
        }
        #endregion public long SaveToDatabase()

        #region private void InitializeControl()
        private void InitializeControl()
        {
            ctlCompanyField.UseEccOnly = true;
            ctlCompanyField.FlagActive = true;
            TADocumentDataSet taDocumentDS = (TADocumentDataSet)TransactionService.GetDS(this.TransactionID);
            TADocumentDataSet.TADocumentRow row = taDocumentDS.TADocument.FindByTADocumentID(this.DocumentID);
            long tempDocumentID = row.DocumentID;

            ctlAttachment.Initialize(this.TransactionID, this.DocumentID, this.InitialFlag);
            ctlInitiator.ControlGroupID = SCG.eAccounting.BLL.Implement.TAFieldGroup.All;
            ctlInitiator.Initialize(this.TransactionID, tempDocumentID, this.InitialFlag);
            ctlTAFormHeader.Initialize(this.TransactionID, tempDocumentID, this.InitialFlag); // send SCGDocument.DocumentID for check visible see history
            ctlHistory.Initialize(tempDocumentID);

            ctlApproverData.DocumentEditor = this;
            ctlCreatorData.ControlGroupID = SCG.eAccounting.BLL.Implement.TAFieldGroup.All;
            ctlRequesterData.ControlGroupID = SCG.eAccounting.BLL.Implement.TAFieldGroup.All;
            ctlApproverData.ControlGroupID = SCG.eAccounting.BLL.Implement.TAFieldGroup.All;

            ctlCreatorData.Initialize(this.TransactionID, this.DocumentID, this.InitialFlag);
            ctlRequesterData.Initialize(this.TransactionID, this.DocumentID, this.InitialFlag);
            ctlApproverData.Initialize(this.TransactionID, this.DocumentID, this.InitialFlag);

            ctlTAFormHeader.DataBind();
            ctlCreatorData.DataBind();
            ctlRequesterData.DataBind();
            ctlApproverData.DataBind();
        }
        #endregion private void InitializeControl()

        #region public void BindControl(bool isCopy)
        public void BindControl(bool isCopy)
        {
            if (!isCopy && this.InitialFlag.Equals(FlagEnum.NewFlag))
            {
                ctlCompanyField.ShowDefault();
                ctlCreatorData.ShowDefault();
                ctlRequesterData.ShowDefault();

                CheckRepOffice();

                ctlInitiator.RequesterID = UIHelper.ParseLong(ctlRequesterData.UserID);
                ctlApproverData.ShowDefaultApprover(UIHelper.ParseLong(ctlRequesterData.UserID));

                ctlTravellingInfoGrid.DataBind();
                ctlTravellingScheduleGrid.DataBind();
                ctlTravellerAdvanceGrid.DataBind();
            }
            else
            {
                Guid txID = this.TransactionID;
                long taDocumentID = this.DocumentID;
                TADocumentDataSet taDocumentDS = (TADocumentDataSet)TransactionService.GetDS(txID);
                TADocumentDataSet.TADocumentRow taDocumentRow = taDocumentDS.TADocument.FindByTADocumentID(this.DocumentID);
                TADocumentDataSet.DocumentRow documentRow = taDocumentDS.Document.FindByDocumentID(taDocumentRow.DocumentID);

                SS.Standard.WorkFlow.DTO.WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(taDocumentRow.DocumentID);

                #region Header & Footer
                if (!isCopy)
                {
                    ctlTAFormHeader.No = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(taDocumentRow.DocumentID).DocumentNo;
                    ctlTAFormHeader.Status = ScgeAccountingQueryProvider.SCGDocumentQuery.GetDocumentCurrentStateName(UserAccount.CurrentLanguageID, taDocumentRow.DocumentID);
                    if (documentRow.DocumentDate != DateTime.MinValue)
                    {
                        ctlTAFormHeader.CreateDate = UIHelper.ToDateString(documentRow.DocumentDate);
                    }
                    ctlCreatorData.SetValue(documentRow.CreatorID);
                }
                else
                {
                    ctlTAFormHeader.Status = FlagEnum.NewFlag;
                    ctlCreatorData.ShowDefault();
                }

                ctlCompanyField.SetValue(documentRow.CompanyID);
                ctlSubject.Text = documentRow.Subject;
                ctlRequesterData.SetValue(documentRow.RequesterID);
                ctlApproverData.SetValue(documentRow.ApproverID);

                CheckRepOffice();
                #endregion Header & Footer

                #region Tab Attachment
                ctlAttachment.BindControl();
                #endregion

                #region Tab Memo
                ctlMemo.Text = documentRow.Memo;
                #endregion Tab Memo

                #region Tab General
                #region Header
                ctlFromDateCal.Value = taDocumentRow.FromDate;
                ctlToDateCal.Value = taDocumentRow.ToDate;
                ctlBusinessChk.Checked = taDocumentRow.IsBusinessPurpose;
                ctlTrainingChk.Checked = taDocumentRow.IsTrainningPurpose;
                ctlOtherChk.Checked = taDocumentRow.IsOtherPurpose;
                ctlOther.Text = taDocumentRow.OtherPurposeDescription;
                if (taDocumentRow.TravelBy.Equals(TravellBy.Domestic))
                {
                    ctlDomesticProvinceChk.Checked = true;
                }
                if (taDocumentRow.TravelBy.Equals(TravellBy.Foreign))
                {
                    ctlAbroadCountryChk.Checked = true;
                }
                ctlProvince.Text = taDocumentRow.Province;
                ctlCountry.Text = taDocumentRow.Country;
                if (taDocumentRow.Ticketing.Equals(Ticketing.TravellingServiceSection))
                {
                    ctlByTravellingChk.Checked = true;
                }
                if (taDocumentRow.Ticketing.Equals(Ticketing.EmployeeSection))
                {
                    ctlByEmployeeChk.Checked = true;
                }
                #endregion Header

                #region ctlTravellingInfoGrid
                ctlTravellingInfoGrid.DataSource = taDocumentDS.TADocumentTraveller;
                ctlTravellingInfoGrid.DataBind();
                #endregion ctlTravellingInfoGrid

                #region ctlTravellingScheduleGrid
                ctlTravellingScheduleGrid.DataSource = taDocumentDS.TADocumentSchedule.Select(t => t).OrderBy(t => t.Date).ToList();
                ctlTravellingScheduleGrid.DataBind();
                #endregion ctlTravellingScheduleGrid
                #endregion Tab General

                #region Tab Advance
                this.BindCtlTravellerAdvanceDropdown();
                if (!isCopy)
                {
                    this.FindAdvanceDocument();
                }
                #endregion Tab Advance

                if (documentRow.CreatorID == UserAccount.UserID)
                {
                    ctlTravellerAdvanceGrid.Columns[9].Visible = true;
                }
                if (InitialFlag.Equals(FlagEnum.ViewFlag) && workFlow.CurrentState.Name.Equals(WorkFlowStateFlag.Draft) && ctlTravellerAdvanceDropdown.Items.Count > 0 && string.IsNullOrEmpty(ctlTAFormHeader.No))
                {
                    ctlCreateAdvance.Enabled = true;
                    ctlRefresh.Enabled = true;
                }
                if (InitialFlag.Equals(FlagEnum.ViewFlag) && !workFlow.CurrentState.Name.Equals(WorkFlowStateFlag.Draft))
                {
                    ctlRefresh.Enabled = false;
                    ctlCreateAdvance.Enabled = false;
                    ctlTravellerAdvanceDropdown.Enabled = false;
                    ctlTravellerAdvanceGrid.Columns[9].Visible = false;
                }
            }
            this.CheckControlReadOnly();
            this.UpdatePanel();
        }
        #endregion public void BindControl()

        #region private void ResetControlValue()
        private void ResetControlValue()
        {
            this.UserList = null;
            ctlTAFormHeader.Status = string.Empty;
            ctlTAFormHeader.No = string.Empty;
            ctlTAFormHeader.CreateDate = string.Empty;
            ctlCompanyField.ShowDefault();
            ctlSubject.Text = string.Empty;
            ctlCreatorData.ShowDefault();
            ctlRequesterData.ShowDefault();
            ctlApproverData.ShowDefault();
            ctlFromDateCal.Value = null;
            ctlToDateCal.Value = null;
            ctlBusinessChk.Checked = false;
            ctlTrainingChk.Checked = false;
            ctlOtherChk.Checked = false;
            ctlOther.Text = string.Empty;
            ctlDomesticProvinceChk.Checked = false;
            ctlProvince.Text = string.Empty;
            ctlAbroadCountryChk.Checked = false;
            ctlCountry.Text = string.Empty;
            ctlByTravellingChk.Checked = false;
            ctlByEmployeeChk.Checked = false;
            ctlTravellingInfoGrid.DataSource = null;
            ctlTravellingInfoGrid.DataBind();
            ctlTravellingScheduleGrid.DataSource = null;
            ctlTravellerAdvanceGrid.DataSource = null;
            ctlTravellerAdvanceGrid.DataBind();
            ctlMemo.Text = string.Empty;

            this.UpdatePanel();
        }
        #endregion private void ResetControlValue()

        #region Grid Event
        #region Function Display Data on Grid
        #region public string DisplayUserID(Object obj)
        public string DisplayUserName(Object obj)
        {
            TADocumentDataSet.TADocumentTravellerRow row = (TADocumentDataSet.TADocumentTravellerRow)((DataRowView)obj).Row;
            SuUser suUser = QueryProvider.SuUserQuery.FindByIdentity(row.UserID);

            if (suUser == null)
                return string.Empty;

            return Server.HtmlEncode(suUser.UserName);
        }
        #endregion public string DisplayUserID(Object obj)

        #region public string DisplayEmployeeName(Object obj)
        public string DisplayEmployeeName(Object obj)
        {
            TADocumentDataSet.TADocumentTravellerRow row = (TADocumentDataSet.TADocumentTravellerRow)((DataRowView)obj).Row;
            SuUser suUser = QueryProvider.SuUserQuery.FindByIdentity(row.UserID);

            if (suUser == null)
                return string.Empty;

            return Server.HtmlEncode(suUser.EmployeeName);
        }
        #endregion public string DisplayEmployeeName(Object obj)
        #endregion Function Display Data on Grid

        #region ctlTravellingInfoGrid Event

        #region protected void ctlTravellingInfoGrid_DataBound(object sender, EventArgs e)
        protected void ctlTravellingInfoGrid_DataBound(object sender, EventArgs e)
        {
            if (ctlByTravellingChk.Checked)
            {
                ctlTravellingInfoGrid.Columns[4].HeaderStyle.CssClass = string.Empty;
                ctlTravellingInfoGrid.Columns[5].HeaderStyle.CssClass = string.Empty;
                ctlTravellingInfoGrid.Columns[6].HeaderStyle.CssClass = string.Empty;

                ctlTravellingInfoGrid.Columns[4].ItemStyle.CssClass = string.Empty;
                ctlTravellingInfoGrid.Columns[5].ItemStyle.CssClass = string.Empty;
                ctlTravellingInfoGrid.Columns[6].ItemStyle.CssClass = string.Empty;
            }
            else
            {
                ctlTravellingInfoGrid.Columns[4].HeaderStyle.CssClass = "hiddenColumn";
                ctlTravellingInfoGrid.Columns[5].HeaderStyle.CssClass = "hiddenColumn";
                ctlTravellingInfoGrid.Columns[6].HeaderStyle.CssClass = "hiddenColumn";

                ctlTravellingInfoGrid.Columns[4].ItemStyle.CssClass = "hiddenColumn";
                ctlTravellingInfoGrid.Columns[5].ItemStyle.CssClass = "hiddenColumn";
                ctlTravellingInfoGrid.Columns[6].ItemStyle.CssClass = "hiddenColumn";
            }

            if (ctlTravellingInfoGrid.Rows.Count == 0)
            {
                TADocumentTraveller taDocumentTraveller = new TADocumentTraveller();

                taDocumentTraveller.TADocumentID = new TADocument(this.DocumentID);
                taDocumentTraveller.UserID = new SuUser(ctlRequesterData.UserAccount.UserID);
                taDocumentTraveller.EmployeeNameEng = string.Empty;
                taDocumentTraveller.AirLineMember = string.Empty;
                taDocumentTraveller.Active = true;

                TADocumentDataSet taDocumentDS = (TADocumentDataSet)TransactionService.GetDS(this.TransactionID);

                if (taDocumentDS != null)
                {
                    TADocumentDataSet.TADocumentTravellerDataTable table = taDocumentDS.TADocumentTraveller;
                    TADocumentTravellerService.AddTADocumentTravellerTransaction(this.TransactionID, taDocumentTraveller);

                    SuUser user = QueryProvider.SuUserQuery.FindProxyByIdentity(taDocumentTraveller.UserID.Userid);

                    if (this.UserList == null)
                    {
                        this.UserList = new List<SuUser>();
                    }
                    UserList.Add(user);

                    //Bind Dropdownlist Tab Advance
                    this.BindCtlTravellerAdvanceDropdown();

                    ctlTravellingInfoGrid.DataSource = table;
                    ctlTravellingInfoGrid.DataBind();
                    ctlUpdatePanelGeneral.Update();
                }
            }
            //add user id to ddl of tab advance on copy mode
            if (Request.QueryString["cp"] != null && UIHelper.ParseLong(Request.QueryString["cp"]) == 1)
            {
                foreach (GridViewRow row in ctlTravellingInfoGrid.Rows)
                {
                    HiddenField ctlUserID = (HiddenField)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlUserId");

                    SuUser user = QueryProvider.SuUserQuery.FindProxyByIdentity(UIHelper.ParseLong(ctlUserID.Value));
                    if (this.UserList == null)
                    {
                        this.UserList = new List<SuUser>();
                    }
                    if (!UserList.Contains(user))
                    {

                        UserList.Add(user);

                        //Bind Dropdownlist Tab Advance
                        this.BindCtlTravellerAdvanceDropdown();
                        ctlUpdatePanelGeneral.Update();
                    }
                }
            }
        }
        #endregion protected void ctlTravellingInfoGrid_DataBound(object sender, EventArgs e)

        #region protected void ctlTravellingInfoGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        protected void ctlTravellingInfoGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("DeleteTravellerInformation"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                int travellerID = UIHelper.ParseInt(ctlTravellingInfoGrid.DataKeys[rowIndex].Value.ToString());

                foreach (GridViewRow row in ctlTravellingInfoGrid.Rows)
                {
                    HiddenField ctlUserID = (HiddenField)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlUserId");
                    TextBox ctlEmployeeNameEng = (TextBox)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlEmployeeNameEng");
                    TextBox ctlAirlineMember = (TextBox)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlAirlineMember");

                    int travellerIDTemp = UIHelper.ParseInt(ctlTravellingInfoGrid.DataKeys[row.RowIndex].Value.ToString());

                    TADocumentTraveller taDocumentTraveller = new TADocumentTraveller(travellerIDTemp);

                    taDocumentTraveller.TADocumentID = new TADocument(this.DocumentID);
                    taDocumentTraveller.UserID = new SuUser(UIHelper.ParseLong(ctlUserID.Value));
                    taDocumentTraveller.EmployeeNameEng = ctlEmployeeNameEng.Text;
                    taDocumentTraveller.AirLineMember = ctlAirlineMember.Text;
                    taDocumentTraveller.Active = true;

                    //Get ta document traveller information and save to transaction.
                    //TADocumentTravellerService.UpdateTADocumentTravellerTransaction(this.TransactionID, taDocumentTraveller);
                    TADocumentTravellerService.ChangeRequesterTraveller(this.TransactionID, taDocumentTraveller);
                }
                TADocumentDataSet.TADocumentTravellerDataTable table = (TADocumentDataSet.TADocumentTravellerDataTable)TADocumentTravellerService.DeleteTADocumentTravellerTransaction(this.TransactionID, travellerID);

                ctlTravellingInfoGrid.DataSource = table;
                ctlTravellingInfoGrid.DataBind();
                ctlUpdatePanelGeneral.Update();

                this.ScriptManagerJavaScript();
            }
        }
        #endregion protected void ctlTravellingInfoGrid_RowCommand(object sender, GridViewCommandEventArgs e)

        #region protected void ctlTravellingInfoGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        protected void ctlTravellingInfoGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField ctlUserID = (HiddenField)e.Row.FindControl("ctlUserId");
                ImageButton ctlDelete = (ImageButton)e.Row.FindControl("ctlDelete");
                CostCenterField ctlCostCenter = (CostCenterField)e.Row.FindControl("ctlCostCenter");
                IOAutoCompleteLookup ctlIO = (IOAutoCompleteLookup)e.Row.FindControl("ctlIO");
                AccountField ctlAccount = (AccountField)e.Row.FindControl("ctlAccount");

                TADocumentDataSet.TADocumentTravellerRow travellerRow = (e.Row.DataItem as System.Data.DataRowView).Row as TADocumentDataSet.TADocumentTravellerRow;

                if (!string.IsNullOrEmpty(travellerRow["CostCenterID"].ToString()))
                    ctlCostCenter.SetValue(travellerRow.CostCenterID);

                if (!string.IsNullOrEmpty(travellerRow["AccountID"].ToString()))
                    ctlAccount.BindAccountControl(travellerRow.AccountID);

                if (!string.IsNullOrEmpty(travellerRow["IOID"].ToString()))
                    ctlIO.BindIOControl(travellerRow.IOID);

                if (ctlRequesterData.UserID.Equals(ctlUserID.Value))
                {
                    ctlDelete.Visible = false;
                }
            }
        }
        #endregion protected void ctlTravellingInfoGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        #endregion ctlTravellingInfoGrid Event

        #region ctlTravellingSchedule Event
        #region protected void ctlTravellingScheduleGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        protected void ctlTravellingScheduleGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            UserControls.DropdownList.SCG.DB.StatusDropdown ctlStatusDropdown = e.Row.FindControl("ctlStatusDropdown") as UserControls.DropdownList.SCG.DB.StatusDropdown;

            HiddenField ctlTravelBy = e.Row.FindControl("ctlTravelBy") as HiddenField;

            if (ctlStatusDropdown != null)
            {
                ctlStatusDropdown.GroupType = GroupStatus.TravelBy;
                ctlStatusDropdown.StatusBind();

                if (ctlTravelBy != null)
                    ctlStatusDropdown.SetDropdown(ctlTravelBy.Value);
            }
        }
        #endregion protected void ctlTravellingScheduleGrid_RowDataBound(object sender, GridViewRowEventArgs e)

        #region protected void ctlTravellingScheduleGrid_DataBound(object sender, EventArgs e)
        protected void ctlTravellingScheduleGrid_DataBound(object sender, EventArgs e)
        {
            if (ctlTravellingScheduleGrid.Rows.Count == 0)
            {
                UserControls.DropdownList.SCG.DB.StatusDropdown ctlStatusDropdownFooter = ((ctlTravellingScheduleGrid.Controls[0] as Table).Rows[2].FindControl("ctlStatusDropdown")) as UserControls.DropdownList.SCG.DB.StatusDropdown;

                if (ctlStatusDropdownFooter != null)
                {
                    ctlStatusDropdownFooter.GroupType = GroupStatus.TravelBy;
                    ctlStatusDropdownFooter.StatusBind();
                    ctlStatusDropdownFooter.SetDropdown(string.Empty);
                }
            }
        }
        #endregion protected void ctlTravellingScheduleGrid_DataBound(object sender, EventArgs e)

        #region protected void ctlTravellingSchedule_RowCommand(object sender, GridViewCommandEventArgs e)
        protected void ctlTravellingSchedule_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            #region AddTravellingSchedule
            if (e.CommandName.Equals("AddTravellingSchedule"))
            {
                UserControls.Calendar ctlDateCal;
                UserControls.DropdownList.SCG.DB.StatusDropdown ctlStatusDropdown;
                TextBox ctlDeparturefrom;
                TextBox ctlArrival;
                UserControls.Time ctlTime;
                TextBox ctlFlightNo;
                TextBox ctlTravellingDetail;

                if (ctlTravellingScheduleGrid.Rows.Count == 0)
                {
                    ctlDateCal = (UserControls.Calendar)((ctlTravellingScheduleGrid.Controls[0] as Table).Rows[2].FindControl("ctlDateCal"));
                    ctlStatusDropdown = (UserControls.DropdownList.SCG.DB.StatusDropdown)((ctlTravellingScheduleGrid.Controls[0] as Table).Rows[2].FindControl("ctlStatusDropdown"));
                    ctlDeparturefrom = (TextBox)((ctlTravellingScheduleGrid.Controls[0] as Table).Rows[2].FindControl("ctlDeparturefrom"));
                    ctlArrival = (TextBox)((ctlTravellingScheduleGrid.Controls[0] as Table).Rows[2].FindControl("ctlArrival"));
                    ctlTime = (UserControls.Time)((ctlTravellingScheduleGrid.Controls[0] as Table).Rows[2].FindControl("ctlTime"));
                    ctlFlightNo = (TextBox)((ctlTravellingScheduleGrid.Controls[0] as Table).Rows[2].FindControl("ctlFlightNo"));
                    ctlTravellingDetail = (TextBox)((ctlTravellingScheduleGrid.Controls[0] as Table).Rows[2].FindControl("ctlTravellingDetail"));
                }
                else
                {
                    ctlDateCal = (UserControls.Calendar)ctlTravellingScheduleGrid.FooterRow.FindControl("ctlDateCal");
                    ctlStatusDropdown = (UserControls.DropdownList.SCG.DB.StatusDropdown)ctlTravellingScheduleGrid.FooterRow.FindControl("ctlStatusDropdown");
                    ctlDeparturefrom = (TextBox)ctlTravellingScheduleGrid.FooterRow.FindControl("ctlDeparturefrom");
                    ctlArrival = (TextBox)ctlTravellingScheduleGrid.FooterRow.FindControl("ctlArrival");
                    ctlTime = (UserControls.Time)ctlTravellingScheduleGrid.FooterRow.FindControl("ctlTime");
                    ctlFlightNo = (TextBox)ctlTravellingScheduleGrid.FooterRow.FindControl("ctlFlightNo");
                    ctlTravellingDetail = (TextBox)ctlTravellingScheduleGrid.FooterRow.FindControl("ctlTravellingDetail");
                }

                TADocumentSchedule taDocumentSchedule = new TADocumentSchedule();

                taDocumentSchedule.TADocumentID = new TADocument(this.DocumentID);

                try
                {
                    if (ctlDateCal.Value != null)
                    {
                        taDocumentSchedule.Date = ctlDateCal.Value;
                    }
                }
                catch (FormatException fex)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Invalid_Date_Format"));
                    this.ValidationErrors.MergeErrors(errors);
                }

                taDocumentSchedule.DepartureFrom = ctlDeparturefrom.Text;
                taDocumentSchedule.ArrivalAt = ctlArrival.Text;
                taDocumentSchedule.TravelBy = ctlStatusDropdown.SelectedValue;
                taDocumentSchedule.Time = ctlTime.TimeValue;
                taDocumentSchedule.FlightNo = ctlFlightNo.Text;
                taDocumentSchedule.TravellingDetail = ctlTravellingDetail.Text;
                taDocumentSchedule.Active = true;

                try
                {
                    TADocumentScheduleService.AddTADocumentScheduleTransaction(this.TransactionID, taDocumentSchedule);
                }
                catch (ServiceValidationException ex)
                {
                    this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                }

                TADocumentDataSet taDocumentDS = (TADocumentDataSet)TransactionService.GetDS(this.TransactionID);
                TADocumentDataSet.TADocumentScheduleDataTable table = taDocumentDS.TADocumentSchedule;

                ctlTravellingScheduleGrid.DataSource = table.Select(t => t).OrderBy(t => t.Date).ToList();
                ctlTravellingScheduleGrid.DataBind();
            }
            #endregion AddTravellingSchedule
            #region DeleteTravellingSchedule
            else if (e.CommandName.Equals("DeleteTravellingSchedule"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                int scheduleID = UIHelper.ParseInt(ctlTravellingScheduleGrid.DataKeys[rowIndex].Value.ToString());

                TADocumentDataSet.TADocumentScheduleDataTable table = (TADocumentDataSet.TADocumentScheduleDataTable)TADocumentScheduleService.DeleteTADocumentScheduleTransaction(this.TransactionID, scheduleID);
                ctlTravellingScheduleGrid.DataSource = table.Where(t => t.RowState != DataRowState.Deleted).Select(t => t).OrderBy(t => t.Date).ToList();
                ctlTravellingScheduleGrid.DataBind();
            }
            #endregion DeleteTravellingSchedule

            this.ScriptManagerJavaScript();

            ctlUpdatePanelValidationSummary.Update();
            ctlUpdatePanelGeneral.Update();
        }
        #endregion protected void ctlTravellingSchedule_RowCommand(object sender, GridViewCommandEventArgs e)
        #endregion ctlTravellingSchedule Event

        #region ctlTravellerAdvanceGrid Event

        protected void ctlTravellerAdvanceGrid_DataBound(object sender, EventArgs e)
        {
            ctlTravellerAdvanceGrid.Columns[7].Visible = false;
            ctlTravellerAdvanceGrid.Columns[8].Visible = true;
            if (IsRepOffice)
            {
                ctlTravellerAdvanceGrid.Columns[7].Visible = true;
                ctlTravellerAdvanceGrid.Columns[8].Visible = false;

                DbCurrency mainCurrency = null;
                if (MainCurrencyID.HasValue)
                {
                    mainCurrency = SsDbQueryProvider.DbCurrencyQuery.FindByIdentity(MainCurrencyID.Value);
                }

                if (mainCurrency != null)
                {
                    ctlTravellerAdvanceGrid.Columns[7].HeaderText = string.Format(GetMessage("AmountCurrencyHeaderColumn"), "(" + mainCurrency.Symbol + ")");
                    ctlTravellerAdvanceGrid.HeaderRow.Cells[7].Text = string.Format(GetMessage("AmountCurrencyHeaderColumn"), "(" + mainCurrency.Symbol + ")");
                }
                else
                {
                    ctlTravellerAdvanceGrid.Columns[7].HeaderText = string.Format(GetMessage("AmountCurrencyHeaderColumn"), "");
                }
            }
        }

        #region protected void ctlTravellerAdvanceGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        protected void ctlTravellerAdvanceGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal ctlNo = e.Row.FindControl("ctlNo") as Literal;
                ctlNo.Text = ((ctlTravellerAdvanceGrid.PageSize * ctlTravellerAdvanceGrid.CustomPageIndex) + (e.Row.RowIndex + 1)).ToString();

                LinkButton ctlAdvanceNoLink = (LinkButton)e.Row.FindControl("ctlAdvanceNoLink");

                if (string.IsNullOrEmpty(ctlAdvanceNoLink.Text))
                {
                    ctlAdvanceNoLink.Text = "N/A";
                }
            }
        }
        #endregion protected void ctlTravellerAdvanceGrid_RowDataBound(object sender, GridViewRowEventArgs e)

        #region protected void ctlTravellerAdvanceGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        protected void ctlTravellerAdvanceGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = 0;
            long taDocumentID = 0;
            long advanceID = 0;
            long documentID = 0;

            #region DeleteTravellerAdvance
            if (e.CommandName.Equals("DeleteTravellerAdvance"))
            {
                rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                taDocumentID = UIHelper.ParseLong(ctlTravellerAdvanceGrid.DataKeys[rowIndex]["TADocumentID"].ToString());
                advanceID = UIHelper.ParseLong(ctlTravellerAdvanceGrid.DataKeys[rowIndex]["AdvanceID"].ToString());
                documentID = UIHelper.ParseLong(ctlTravellerAdvanceGrid.DataKeys[rowIndex]["DocumentID"].ToString());

                AvAdvanceDocument advanceDoc = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindProxyByIdentity(advanceID);
                long userID = 0;
                if (advanceDoc.DocumentID != null)
                {
                    if (advanceDoc.DocumentID.RequesterID != null)
                    {
                        userID = advanceDoc.DocumentID.RequesterID.Userid;
                    }
                }
                //delete data for AvAdvanceDocument.
                //case ; Advance isn't document number
                SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.FindByIdentity(documentID);
                if (scgDocument != null)
                {
                    string advanceDocumentNo = scgDocument.DocumentNo;
                    if (string.IsNullOrEmpty(advanceDocumentNo))
                        SCGDocumentService.DeleteDocumentByDocumentID(documentID);
                    else
                    {
                        AvAdvanceDocumentService.UpdateTAdocumentIDTransaction(advanceID);
                    }
                }

                var items = from taDocumentAdvance in this.TADocumentObj
                            where taDocumentAdvance.TADocumentID == taDocumentID &&
                                    taDocumentAdvance.AdvanceID == advanceID
                            select taDocumentAdvance;

                if ((items != null) && (items.Count<TADocumentObj>() > 0))
                {
                    this.TADocumentObj.Remove(items.First<TADocumentObj>());
                }

                if (this.UserList == null)
                {
                    this.UserList = new List<SuUser>();
                }
                if (userID > 0)
                {
                    SuUser user = QueryProvider.SuUserQuery.FindProxyByIdentity(userID);
                    this.UserList.Add(user);
                }

                this.BindCtlTravellerAdvanceDropdown();

                if (ctlTravellerAdvanceDropdown.Items.Count == 0)
                {
                    ctlCreateAdvance.Enabled = false;
                }
                else
                {
                    ctlCreateAdvance.Enabled = true;
                    ctlRefresh.Enabled = true;
                }

                ctlTravellerAdvanceGrid.DataSource = TADocumentObj;
                ctlTravellerAdvanceGrid.DataBind();
                ctlUpdatePanelGeneral.Update();
            }
            #endregion DeleteTravellerAdvance
            #region LinkTravellerAdvance
            else if (e.CommandName.Equals("LinkTravellerAdvance"))
            {
                rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                taDocumentID = UIHelper.ParseLong(ctlTravellerAdvanceGrid.DataKeys[rowIndex]["TADocumentID"].ToString());
                advanceID = UIHelper.ParseLong(ctlTravellerAdvanceGrid.DataKeys[rowIndex]["AdvanceID"].ToString());

                AvAdvanceDocument avAdvanceDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindByIdentity(advanceID);
                documentID = avAdvanceDocument.DocumentID.DocumentID;

                SS.Standard.WorkFlow.DTO.WorkFlow workFlow = new SS.Standard.WorkFlow.DTO.WorkFlow();
                workFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(documentID);

                long workFlowID;
                if (workFlow != null)
                    workFlowID = workFlow.WorkFlowID;
                else
                    workFlowID = 0;
                //popup Document View by WorkFlow
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), string.Empty, "window.open('../Programs/DocumentView.aspx?wfid=" + workFlowID.ToString() + "')", true);
            }
            #endregion LinkTravellerAdvance
        }
        #endregion protected void ctlTravellerAdvanceGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        #endregion ctlTravellerAdvanceGrid Event
        #endregion Grid Event

        #region Button Event
        #region Tab Advance
        #region protected void ctlCreateAdvance_Click(object sender, EventArgs e)
        protected void ctlCreateAdvance_Click(object sender, EventArgs e)
        {
            ctlCreateAdvance.Enabled = false;

            this.RequesterID = UIHelper.ParseLong(ctlTravellerAdvanceDropdown.SelectedValue);

            if (ctlDomesticProvinceChk.Checked)
            {
                Response.Redirect("~/Forms/SCG.eAccounting/Programs/AdvanceDomesticForm.aspx?" + "&taDocumentID=" + this.DocumentID + "&requesterID=" + this.RequesterID + "");
            }
            else
            {
                Response.Redirect("~/Forms/SCG.eAccounting/Programs/AdvanceForeignForm.aspx?" + "&taDocumentID=" + this.DocumentID + "&requesterID=" + this.RequesterID + "");
            }
        }
        #endregion protected void ctlCreateAdvance_Click(object sender, EventArgs e)

        #region protected void ctlRefreshAdvance_Click(object sender, EventArgs e)
        protected void ctlRefreshAdvance_Click(object sender, EventArgs e)
        {
            this.FindAdvanceDocument();

            if (ctlTravellerAdvanceDropdown.Items.Count == 0)
            {
                ctlCreateAdvance.Enabled = false;
            }
            else
            {
                ctlCreateAdvance.Enabled = true;
                ctlRefresh.Enabled = true;
            }
        }
        #endregion protected void ctlRefreshAdvance_Click(object sender, EventArgs e)
        #endregion Tab Advance
        #endregion Button Event

        #region Lookup User Event
        #region protected void ctlUserProfileLookup_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        protected void ctlUserProfileLookup_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            IList<SuUser> userInfo = (IList<SuUser>)e.ObjectReturn;
            ArrayList userIDArrList = new ArrayList();

            int travellerID = 0;

            if (userInfo != null)
            {
                //Save user id to arraylist for check duplicate user id.
                foreach (GridViewRow row in ctlTravellingInfoGrid.Rows)
                {
                    HiddenField ctlUserID = (HiddenField)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlUserId");
                    TextBox ctlEmployeeNameEng = (TextBox)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlEmployeeNameEng");
                    Label ctlUserName = (Label)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlUserName");
                    TextBox ctlAirlineMember = (TextBox)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlAirlineMember");
                    CostCenterField ctlCostCenter = (CostCenterField)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlCostCenter");
                    AccountField ctlAccount = (AccountField)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlAccount");
                    IOAutoCompleteLookup ctlIO = (IOAutoCompleteLookup)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlIO");

                    userIDArrList.Add(ctlUserID.Value);
                    //Update data on grid when add user id. 
                    if (userIDArrList.Contains(ctlUserID.Value))
                    {
                        travellerID = UIHelper.ParseInt(ctlTravellingInfoGrid.DataKeys[row.RowIndex].Value.ToString());

                        TADocumentTraveller taDocumentTraveller = new TADocumentTraveller(travellerID);

                        taDocumentTraveller.TADocumentID = new TADocument(this.DocumentID);
                        taDocumentTraveller.UserID = new SuUser(UIHelper.ParseLong(ctlUserID.Value));
                        taDocumentTraveller.EmployeeNameEng = ctlEmployeeNameEng.Text;
                        taDocumentTraveller.AirLineMember = ctlAirlineMember.Text;
                        taDocumentTraveller.UserName = ctlUserName.Text;
                        taDocumentTraveller.CostCenterID = new DbCostCenter(UIHelper.ParseLong(ctlCostCenter.CostCenterId));
                        taDocumentTraveller.Account = new DbAccount(UIHelper.ParseLong(ctlAccount.AccountID));
                        taDocumentTraveller.IOID = new DbInternalOrder(UIHelper.ParseLong(ctlIO.IOID));
                        taDocumentTraveller.Active = true;

                        try
                        {
                            // Get ta document traveller information and save to transaction.
                            TADocumentTravellerService.ChangeRequesterTraveller(this.TransactionID, taDocumentTraveller);
                        }
                        catch (ServiceValidationException ex)
                        {
                            errors.MergeErrors(ex.ValidationErrors);

                        }
                    }
                }
                if (!errors.IsEmpty)
                {
                    this.ValidationErrors.MergeErrors(errors);
                }
                else
                {
                    foreach (SuUser user in userInfo)
                    {
                        //check duplicate user id.
                        if (!userIDArrList.Contains(user.Userid.ToString()))
                        {
                            TADocumentTraveller taDocumentTraveller = new TADocumentTraveller();

                            taDocumentTraveller.TADocumentID = new TADocument(this.DocumentID);
                            taDocumentTraveller.UserID = new SuUser(user.Userid);
                            taDocumentTraveller.EmployeeNameEng = string.Empty;
                            taDocumentTraveller.AirLineMember = string.Empty;
                            taDocumentTraveller.Active = true;

                            TADocumentTravellerService.AddTADocumentTravellerTransaction(this.TransactionID, taDocumentTraveller);

                            if (this.UserList == null)
                            {
                                this.UserList = new List<SuUser>();
                            }
                            UserList.Add(user);
                        }
                    }
                }

                //Bind Dropdownlist Tab Advance
                this.BindCtlTravellerAdvanceDropdown();

                TADocumentDataSet taDocumentDS = (TADocumentDataSet)TransactionService.GetDS(this.TransactionID);
                TADocumentDataSet.TADocumentTravellerDataTable table = taDocumentDS.TADocumentTraveller;
                ctlTravellingInfoGrid.DataSource = table;
                ctlTravellingInfoGrid.DataBind();

                this.ScriptManagerJavaScript();
            }
            ctlUpdatePanelGeneral.Update();
            ctlUpdatePanelValidationSummary.Update();
        }
        #endregion protected void ctlUserProfileLookup_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)

        #region protected void ctlUserProfileLookup_Click(object sender, ImageClickEventArgs e)
        protected void ctlUserProfileLookup_Click(object sender, ImageClickEventArgs e)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (ctlByTravellingChk.Checked)
            {
                foreach (GridViewRow row in ctlTravellingInfoGrid.Rows)
                {
                    Label ctlUserName = (Label)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlUserName");
                    CostCenterField ctlCostCenter = (CostCenterField)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlCostCenter");
                    AccountField ctlAccount = (AccountField)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlAccount");
                    if (ctlCostCenter.CostCenterId == string.Empty)
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("CostCenterIsRequired", new object[] { ctlUserName.Text, " " }));
                    }
                    if (ctlAccount.AccountID == string.Empty)
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("ExpenseGroupIsRequired", new object[] { ctlUserName.Text, " " }));
                    }
                }
            }

            if (!errors.IsEmpty)
            {
                this.ValidationErrors.MergeErrors(errors);
                //throw new ServiceValidationException(errors);
                ctlUpdatePanelValidationSummary.Update();
            }
            else
            {
                ctlUserProfileLookup.isMultiple = true;
                ctlUserProfileLookup.Show();
                ctlUpdatePanelValidationSummary.Update();
            }
        }
        #endregion protected void ctlUserProfileLookup_Click(object sender, ImageClickEventArgs e)

        protected void ctlCompanyField_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            if (e.ObjectReturn != null)
            {
                DbCompany comp = (DbCompany)e.ObjectReturn;
                DbCompany company = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(comp.CompanyID);
                //ctlCostCenterField.ResetValue();
                //ctlCostCenterField.CompanyId = company.CompanyID;
                SuUser user = QueryProvider.SuUserQuery.FindByIdentity(UIHelper.ParseLong(ctlRequesterData.UserID));
                //if (user != null)
                //{
                //    if (user.Company != null && (ctlCompanyField.CompanyID == user.Company.CompanyID.ToString()))
                //    {
                //        if (user.CostCenter != null)
                //            ctlCostCenterField.SetValue(user.CostCenter.CostCenterID);
                //    }
                //}
            }
            ctlUpdatePanelGeneral.Update();
        }

        //public void ctlCostCenterField_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        //{
        //    if (e.ObjectReturn != null)
        //    {
        //        DbCostCenter cost = e.ObjectReturn as DbCostCenter;
        //        DbCostCenter dbCostcenter = ScgDbQueryProvider.DbCostCenterQuery.FindByIdentity(cost.CostCenterID);
        //        ctlIOAutoCompleteLookup.CostCenterId = dbCostcenter.CostCenterID;
        //        ctlAccountField.ExpenseGroupType = string.IsNullOrEmpty(cost.CostCenterCode) ? (int?)null : dbCostcenter.CostCenterCode.Substring(3, 1).Equals("0") ? 0 : 1;
        //    }
        //    ctlUpdatePanelGeneral.Update();
        //}

        #region protected void ctlRequesterData_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        protected void ctlRequesterData_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            SuUser user = (SuUser)e.ObjectReturn;
            ArrayList userIDArrList = new ArrayList();
            TADocumentDataSet taDocumentDS = (TADocumentDataSet)TransactionService.GetDS(this.TransactionID);

            int travellerID = 0;

            if (user != null)
            {
                //if (user.Company != null && (ctlCompanyField.CompanyID == user.Company.CompanyID.ToString()))
                //{
                //    if (user.CostCenter != null)
                //        ctlCostCenterField.SetValue(user.CostCenter.CostCenterID);
                //}
                ctlInitiator.RequesterID = user.Userid;
                if (ctlTravellingInfoGrid.Rows.Count > 0)
                {
                    //keep data of user id.
                    ctlApproverData.ShowDefaultApprover(UIHelper.ParseLong(ctlRequesterData.UserID));

                    foreach (GridViewRow row in ctlTravellingInfoGrid.Rows)
                    {
                        HiddenField ctlUserID = (HiddenField)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlUserId");
                        TextBox ctlEmployeeNameEng = (TextBox)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlEmployeeNameEng");
                        TextBox ctlAirlineMember = (TextBox)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlAirlineMember");
                        Label ctlUserName = (Label)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlUserName");
                        CostCenterField ctlCostCenter = (CostCenterField)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlCostCenter");
                        AccountField ctlAccount = (AccountField)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlAccount");
                        IOAutoCompleteLookup ctlIO = (IOAutoCompleteLookup)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlIO");

                        userIDArrList.Add(ctlUserID.Value);

                        //Update data on grid when add user id. 
                        if (userIDArrList.Contains(ctlUserID.Value))
                        {
                            travellerID = UIHelper.ParseInt(ctlTravellingInfoGrid.DataKeys[row.RowIndex].Value.ToString());

                            TADocumentTraveller taDocumentTraveller = new TADocumentTraveller(travellerID);

                            taDocumentTraveller.TADocumentID = new TADocument(this.DocumentID);
                            taDocumentTraveller.UserID = new SuUser(UIHelper.ParseLong(ctlUserID.Value));
                            taDocumentTraveller.EmployeeNameEng = ctlEmployeeNameEng.Text;
                            taDocumentTraveller.AirLineMember = ctlAirlineMember.Text;
                            taDocumentTraveller.UserName = ctlUserName.Text;
                            taDocumentTraveller.CostCenterID = new DbCostCenter(UIHelper.ParseLong(ctlCostCenter.CostCenterId));
                            taDocumentTraveller.Account = new DbAccount(UIHelper.ParseLong(ctlAccount.AccountID));
                            taDocumentTraveller.IOID = new DbInternalOrder(UIHelper.ParseLong(ctlIO.IOID));
                            taDocumentTraveller.Active = true;

                            // Get ta document traveller information and save to transaction.
                            TADocumentTravellerService.ChangeRequesterTraveller(this.TransactionID, taDocumentTraveller);
                        }
                    }
                    //add requester id on grid when requester id not duplicate user id. 
                    if (!userIDArrList.Contains(user.Userid.ToString()))
                    {
                        travellerID = UIHelper.ParseInt(ctlTravellingInfoGrid.DataKeys[0].Value.ToString());

                        TADocumentDataSet.TADocumentTravellerRow row = taDocumentDS.TADocumentTraveller.FindByTravellerID(travellerID);

                        long currentRequesterID = row.UserID;

                        TADocumentTraveller taDocumentTraveller = new TADocumentTraveller(travellerID);

                        taDocumentTraveller.TADocumentID = new TADocument(this.DocumentID);
                        taDocumentTraveller.UserID = new SuUser(user.Userid);
                        taDocumentTraveller.EmployeeNameEng = string.Empty;
                        taDocumentTraveller.AirLineMember = string.Empty;
                        taDocumentTraveller.Active = true;

                        TADocumentTravellerService.ChangeRequesterTraveller(this.TransactionID, taDocumentTraveller);

                        var currentRequester = from users in this.UserList
                                               where users.Userid == currentRequesterID
                                               select users;

                        if ((currentRequester != null) && (currentRequester.Count<SuUser>() > 0))
                        {
                            int requesterIndex = this.UserList.IndexOf(currentRequester.Single<SuUser>());
                            this.UserList.RemoveAt(requesterIndex);

                            if (this.UserList == null)
                            {
                                this.UserList = new List<SuUser>();
                            }
                            this.UserList.Insert(requesterIndex, user);
                        }
                        //Bind Dropdownlist Tab Advance
                        this.BindCtlTravellerAdvanceDropdown();
                    }
                }
            }

            TADocumentDataSet.TADocumentTravellerDataTable table = taDocumentDS.TADocumentTraveller;
            ctlTravellingInfoGrid.DataSource = table;
            ctlTravellingInfoGrid.DataBind();

            ctlUpdatePanelGeneral.Update();
            ctlUpdatePanelApprover.Update();

            this.ScriptManagerJavaScript();
        }
        #endregion protected void ctlRequesterData_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)

        #endregion Lookup User Event

        #region Function
        #region public void RollBackTransaction()
        public void RollBackTransaction()
        {
            Guid txID = this.TransactionID;

            TransactionService.Rollback(txID);
            this.ResetControlValue();
        }
        #endregion public void RollBackTransaction()

        #region public bool IsContainEditableFields(object editableFieldEnum)
        public bool IsContainEditableFields(object editableFieldEnum)
        {
            return this.EditableFields.Contains(editableFieldEnum);
        }
        #endregion public bool IsContainEditableFields(object editableFieldEnum)

        #region public bool IsContainVisibleFields(object visibleFieldEnum)
        public bool IsContainVisibleFields(object visibleFieldEnum)
        {
            return this.VisibleFields.Contains(visibleFieldEnum);
        }
        #endregion public bool IsContainVisibleFields(object visibleFieldEnum)

        #region public void BindCtlTravellerAdvanceDropdown()
        public void BindCtlTravellerAdvanceDropdown()
        {
            ctlTravellerAdvanceDropdown.DataSource = this.UserList;
            ctlTravellerAdvanceDropdown.DataValueField = "UserID";
            ctlTravellerAdvanceDropdown.DataTextField = "UserName";
            ctlTravellerAdvanceDropdown.DataBind();

            ctlUpdatePanelAdvanceTab.Update();
        }
        #endregion public void BindCtlTravellerAdvanceDropdown()

        #region public void FindAdvanceDocument()
        public void FindAdvanceDocument()
        {
            IList<TADocumentObj> taObjList = ScgeAccountingQueryProvider.TADocumentAdvanceQuery.FindAdvanceByTADocumentID(UserAccount.CurrentLanguageID, this.DocumentID);

            foreach (TADocumentObj taDocumentObj in taObjList)
            {
                if (this.TADocumentObj == null)
                {
                    this.TADocumentObj = new List<TADocumentObj>();
                }
                if (!TADocumentObj.Contains(taDocumentObj))
                {
                    TADocumentAdvance taDocumentAdvance = new TADocumentAdvance();

                    taDocumentAdvance.TADocument = new TADocument(this.DocumentID);
                    taDocumentAdvance.Advance = new AvAdvanceDocument(taDocumentObj.AdvanceID.Value);
                    taDocumentAdvance.Active = true;

                    TADocumentAdvanceService.AddTADocumentAdvanceTransaction(this.TransactionID, taDocumentAdvance);

                    TADocumentObj.Add(taDocumentObj);

                    SuUser user = QueryProvider.SuUserQuery.FindProxyByIdentity(taDocumentObj.RequesterID.Value);
                    if (this.UserList == null)
                    {
                        this.UserList = new List<SuUser>();
                    }
                    var currentRequester = from users in this.UserList
                                           where users.Userid == user.Userid
                                           select users;

                    if ((currentRequester != null) && (currentRequester.Count<SuUser>() > 0))
                    {
                        int requesterIndex = this.UserList.IndexOf(currentRequester.Single<SuUser>());
                        this.UserList.RemoveAt(requesterIndex);
                    }
                }
            }
            this.BindCtlTravellerAdvanceDropdown();

            ctlTravellerAdvanceGrid.DataSource = taObjList;
            ctlTravellerAdvanceGrid.DataBind();
            //if (ctlByTravellingChk.Checked)
            //{
            //    ctlDivAddSchedule.Style["display"] = "block";
            //}
            ctlUpdatePanelGeneral.Update();
            ctlUpdatePanelAdvanceTab.Update();
        }
        #endregion public void FindAdvanceDocument()

        #region public void CheckControlReadOnly()
        public void CheckControlReadOnly()
        {
            #region Check ReadOnly
            if (ctlOtherChk.Checked)
            {
                ctlOther.Enabled = true;
            }
            else
            {
                ctlOther.Enabled = false;
            }
            if (ctlDomesticProvinceChk.Checked)
            {
                ctlProvince.Enabled = true;
            }
            else
            {
                ctlProvince.Enabled = false;
            }
            if (ctlAbroadCountryChk.Checked)
            {
                ctlCountry.Enabled = true;
            }
            else
            {
                ctlCountry.Enabled = false;
            }
            //if (ctlByTravellingChk.Checked)
            //{
            //    ctlDivAddSchedule.Style["display"] = "block";
            //}
            //else
            //{
            //    ctlDivAddSchedule.Style["display"] = "none";
            //}
            //ctlDivAddSchedule.Style["display"] = "none";

            /* if TA reference Advance => can't change company (comment by meaw 10-Jul-2009) */
            if (Request.QueryString["cp"] == null || UIHelper.ParseLong(Request.QueryString["cp"]) != 1)
            {
                if (this.RefAdvance())
                    ctlCompanyField.Mode = ModeEnum.Readonly;
                else
                    ctlCompanyField.Mode = ModeEnum.ReadWrite;

                ctlCompanyField.ChangeMode();
            }
            #endregion Check ReadOnly
        }
        #endregion public void CheckControlReadOnly()

        #region public void UpdatePanel()
        public void UpdatePanel()
        {
            ctlUpdatePanelHeader.Update();
            ctlUpdatePanelTab.Update();
            ctlUpdatePanelGeneral.Update();
            ctlUpdatePanelAdvance.Update();
            ctlUpdatePanelApprover.Update();
            ctlUpdatePanelAttachment.Update();
            ctlUpdatePanelInitial.Update();
            ctlUpdatePanelMemo.Update();
            ctlUpdatePanelValidationSummary.Update();
        }
        #endregion public void UpdatePanel()

        #region public void ScriptManagerJavaScript()
        public void ScriptManagerJavaScript()
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "DisableCheckbox", "DisableCheckbox();", true);
            //ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "VisibleControl", "VisibleControl();", true);
        }
        #endregion public void ScriptManagerJavaScript()

        #region protected void ctlOtherChk_OnCheckedChanged(object sender, EventArgs e)
        protected void ctlOtherChk_OnCheckedChanged(object sender, EventArgs e)
        {
            if (!ctlOtherChk.Checked)
            {
                ctlOther.Text = string.Empty;
            }
            this.ScriptManagerJavaScript();
        }
        #endregion protected void ctlOtherChk_OnCheckedChanged(object sender, EventArgs e)

        #region protected void ctlDomesticProvinceChk_OnCheckedChanged(object sender, EventArgs e)
        protected void ctlDomesticProvinceChk_OnCheckedChanged(object sender, EventArgs e)
        {
            ctlCountry.Text = string.Empty;
            ctlProvince.Enabled = true;
            ctlCountry.Enabled = false;

            this.ScriptManagerJavaScript();
        }
        #endregion protected void ctlDomesticProvinceChk_OnCheckedChanged(object sender, EventArgs e)

        #region protected void ctlAbroadCountryChk_OnCheckedChanged(object sender, EventArgs e)
        protected void ctlAbroadCountryChk_OnCheckedChanged(object sender, EventArgs e)
        {
            ctlProvince.Text = string.Empty;
            ctlCountry.Enabled = true;
            ctlProvince.Enabled = false;

            this.ScriptManagerJavaScript();
        }
        #endregion protected void ctlAbroadCountryChk_OnCheckedChanged(object sender, EventArgs e)
        #endregion Function

        #region public void Copy(long wfid)
        public void Copy(long wfid)
        {
            SS.Standard.WorkFlow.DTO.Document doc = WorkFlowQueryProvider.WorkFlowQuery.GetDocumentByWorkFlowID(wfid);

            if (doc.DocumentType.DocumentTypeID.Equals(DocumentTypeID.TADocumentDomestic) || doc.DocumentType.DocumentTypeID.Equals(DocumentTypeID.TADocumentForeign))
            {
                Response.Redirect(String.Format("~/Forms/SCG.eAccounting/Programs/TAForm.aspx?cp=1&docId={0}", doc.DocumentID));
            }
        }
        #endregion public void Copy(long wfid)

        #region public bool RefAdvance()
        public bool RefAdvance()
        {
            IList<VoAdvanceFromTA> advanceList = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindAdvanceFromTA(this.DocumentID);
            if (advanceList.Count > 0)
                return true;
            else
                return false;
        }
        #endregion public bool RefAdvance()

        #region Enable
        public void EnabledViewPostButton(bool IsLock)
        {

        }

        public void EnabledPostRemittanceButton(bool IsLock)
        {

        }
        #endregion Enable

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

        public void CheckRepOffice()
        {
            long taDocumentID = this.DocumentID;
            TADocumentDataSet taDS = (TADocumentDataSet)TransactionService.GetDS(this.TransactionID);
            TADocumentDataSet.TADocumentRow taRow = taDS.TADocument.FindByTADocumentID(taDocumentID);

            IsRepOffice = false;

            if (ctlCompanyField.CompanyID == ctlRequesterData.CompanyID)
            {
                SuUser userList = QueryProvider.SuUserQuery.FindByIdentity(UIHelper.ParseLong(ctlRequesterData.UserID));
                if (userList != null && userList.Location != null && userList.Location.DefaultPBID.HasValue)
                {
                    Dbpb pb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(userList.Location.DefaultPBID.Value);
                    if (pb != null)
                    {
                        IsRepOffice = pb.RepOffice;
                        MainCurrencyID = pb.MainCurrencyID;
                    }
                }
            }
        }

        public bool RequireDocumentAttachment()
        {
            return false;
        }

        protected void ctlByTravellingChk_CheckedChanged(object sender, EventArgs e)
        {
            if (ctlByTravellingChk.Checked)
            {
                ctlTravellingInfoGrid.Columns[4].HeaderStyle.CssClass = string.Empty;
                ctlTravellingInfoGrid.Columns[5].HeaderStyle.CssClass = string.Empty;
                ctlTravellingInfoGrid.Columns[6].HeaderStyle.CssClass = string.Empty;

                ctlTravellingInfoGrid.Columns[4].ItemStyle.CssClass = string.Empty;
                ctlTravellingInfoGrid.Columns[5].ItemStyle.CssClass = string.Empty;
                ctlTravellingInfoGrid.Columns[6].ItemStyle.CssClass = string.Empty;

            }
            else
            {
                ctlTravellingInfoGrid.Columns[4].HeaderStyle.CssClass = "hiddenColumn";
                ctlTravellingInfoGrid.Columns[5].HeaderStyle.CssClass = "hiddenColumn";
                ctlTravellingInfoGrid.Columns[6].HeaderStyle.CssClass = "hiddenColumn";

                ctlTravellingInfoGrid.Columns[4].ItemStyle.CssClass = "hiddenColumn";
                ctlTravellingInfoGrid.Columns[5].ItemStyle.CssClass = "hiddenColumn";
                ctlTravellingInfoGrid.Columns[6].ItemStyle.CssClass = "hiddenColumn";
                foreach (GridViewRow row in ctlTravellingInfoGrid.Rows)
                {
                    CostCenterField ctlCostCenter = (CostCenterField)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlCostCenter");
                    IOAutoCompleteLookup ctlIO = (IOAutoCompleteLookup)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlIO");
                    AccountField ctlAccount = (AccountField)ctlTravellingInfoGrid.Rows[row.RowIndex].FindControl("ctlAccount");
                    ctlCostCenter.ResetValue();
                    ctlIO.ResetValue();
                    ctlAccount.ResetValue();
                }
            }
        }
    }
}

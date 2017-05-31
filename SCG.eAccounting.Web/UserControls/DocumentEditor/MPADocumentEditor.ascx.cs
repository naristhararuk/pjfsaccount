using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using SS.SU.DTO;
using SS.SU.BLL;
using SCG.eAccounting.BLL;
using SS.Standard.WorkFlow.Service;
using log4net;
using SCG.eAccounting.DTO;
using SCG.eAccounting.Query;
using System.Collections;
using SS.Standard.Utilities;
using SS.SU.Query;
using SCG.eAccounting.DTO.DataSet;
using SS.Standard.WorkFlow.Query;
using SCG.DB.DTO;
using System.Data;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor
{
    public partial class MPADocumentEditor : BaseUserControl, IDocumentEditor
    {
        private static ILog logger = log4net.LogManager.GetLogger(typeof(ExpenseDocumentEditor));

        #region local variable
        public ISuUserService SuUserService { get; set; }
        public IMPADocumentService MPADocumentService { get; set; }
        //public ITADocumentTravellerService TADocumentTravellerService { get; set; }
        //public ITADocumentAdvanceService TADocumentAdvanceService { get; set; }
        public ITransactionService TransactionService { get; set; }
        public ISCGDocumentService SCGDocumentService { get; set; }
        //public IAvAdvanceDocumentService AvAdvanceDocumentService { get; set; }
        public IWorkFlowService WorkFlowService { get; set; }
        public IMPAItemService MPAItemService { get; set; }
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
        public long MPADocumentID
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
        //public IList<TADocumentObj> TADocumentObj
        //{
        //    get { return (IList<TADocumentObj>)ViewState["TADocumentObj"]; }
        //    set { ViewState["TADocumentObj"] = value; }
        //}
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

        #endregion

        //#region protected override void Render(HtmlTextWriter writer)
        //protected override void Render(HtmlTextWriter writer)
        //{
        //    base.Render(writer);
        //    if (!IsPostBack)
        //    {
        //        this.ScriptManagerJavaScript();
        //        ctlUpdatePanelGeneral.Update();
        //    }
        //}
        //#endregion protected override void Render(HtmlTextWriter writer)

        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            //ctlCompanyField.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlCompanyField_OnObjectLookUpReturn);

            ctlRequesterData.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlRequesterData_OnObjectLookUpReturn);
            ctlUserProfileLookup.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlUserProfileLookup_OnObjectLookUpReturn);
        }
        #endregion protected void Page_Load(object sender, EventArgs e)

        #region public void Initialize(string initFlag, long? documentID)
        public void Initialize(string initFlag, long? documentID)
        {
            MPADocumentDataSet mpaDocumentDS;
            long mpaDocumentID = 0;
            Guid txID = Guid.Empty;
            bool isCopy = false;

            #region Visible Mode
            ctlAddRequester.Visible = true;
            ctlRequesterInfoGrid.Columns[12].Visible = true;

            this.isShowFooter = true;
            #endregion Visible Mode

            if (initFlag.Equals(FlagEnum.NewFlag) && Request.QueryString["cp"] == null)
            {
                mpaDocumentDS = (MPADocumentDataSet)MPADocumentService.PrepareDS();
                txID = TransactionService.Begin(mpaDocumentDS);
                mpaDocumentID = MPADocumentService.AddMPADocumentTransaction(txID);

                ctlMPAFormHeader.Status = FlagEnum.NewFlag;
            }
            else if (((initFlag.Equals(FlagEnum.EditFlag) || (initFlag.Equals(FlagEnum.ViewFlag)))) && ((documentID.HasValue) && (documentID.Value != 0)))
            {
                mpaDocumentDS = (MPADocumentDataSet)MPADocumentService.PrepareDS(documentID.Value);
                txID = TransactionService.Begin(mpaDocumentDS);

                if (mpaDocumentDS.Document.Rows.Count > 0)
                {
                    mpaDocumentID = UIHelper.ParseLong(mpaDocumentDS.MPADocument.Rows[0]["MPADocumentID"].ToString());
                }

                if (initFlag.Equals(FlagEnum.ViewFlag))
                {
                    #region Visible Mode
                    ctlAddRequester.Visible = false;
                    ctlRequesterInfoGrid.Columns[12].Visible = false;

                    SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(documentID.Value);

                    this.isShowFooter = false;
                    #endregion Visible Mode
                }
            }
            else if (Request.QueryString["cp"] != null && UIHelper.ParseLong(Request.QueryString["cp"]) == 1)
            {
                mpaDocumentDS = (MPADocumentDataSet)MPADocumentService.PrepareDataToDataset(UIHelper.ParseLong(Request.QueryString["docId"]), true);
                txID = TransactionService.Begin(mpaDocumentDS);
                isCopy = true;
                if (mpaDocumentDS.MPADocument.Rows.Count > 0)
                {
                    mpaDocumentID = UIHelper.ParseLong(mpaDocumentDS.MPADocument.Rows[0]["MPADocumentID"].ToString());
                }
            }
            this.TransactionID = txID;
            this.MPADocumentID = mpaDocumentID;
            this.InitialFlag = initFlag;

            // Define value to each DocumentEditor Property.
            // And store it in viewstate.
            this.VisibleFields = MPADocumentService.GetVisibleFields(documentID);
            this.EditableFields = MPADocumentService.GetEditableFields(documentID);

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

                ctlUpdatePanelTab.Update();
            }
            else
            {
                ctlTabMemo.Visible = true;
                ctlTabInitial.Visible = true;
                ctlTabAttachment.Visible = true;
                ctlTabHistory.Visible = true;

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

            MPADocumentDataSet mpaDocumentDS = (MPADocumentDataSet)TransactionService.GetDS(this.TransactionID);
            if (mpaDocumentDS == null)
            {
                OnDsNull();
            }
            MPADocumentDataSet.MPADocumentRow mpaDocumentRow = mpaDocumentDS.MPADocument.FindByMPADocumentID(this.MPADocumentID);
            long tempDocumentID = mpaDocumentRow.DocumentID;

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
            
            scgDocument.DocumentType = new SS.Standard.WorkFlow.DTO.DocumentType(DocumentTypeID.MPADocument);
            
            
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

            #region MPADocument
            MPADocument mpaDocument = new MPADocument(this.MPADocumentID);

            mpaDocument.DocumentID = scgDocument;

            try
            {
                if (ctlStartDateCal.Value != null)
                {
                    mpaDocument.StartDate = ctlStartDateCal.Value.Value;
                }

                if (ctlEndDateCal.Value != null)
                {
                    mpaDocument.EndDate = ctlEndDateCal.Value.Value;
                }
            }
            catch (FormatException fex)
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Invalid_Date_Format"));
                throw new ServiceValidationException(errors);
            }

            mpaDocument.Active = true;

            try
            {
                // Get ta document information and save to transaction.
                MPADocumentService.UpdateMPADocumentTransaction(this.TransactionID, mpaDocument);
            }
            catch (ServiceValidationException ex)
            {
                errors.MergeErrors(ex.ValidationErrors);
            }

            if (!errors.IsEmpty)
            {
                throw new ServiceValidationException(errors);
            }

            #endregion MPADocument

            #region MPAItem
            foreach (GridViewRow row in ctlRequesterInfoGrid.Rows)
            {
                HiddenField ctlUserId = (HiddenField)ctlRequesterInfoGrid.Rows[row.RowIndex].FindControl("ctlUserId");
                TextBox ctlActualAmount = (TextBox)ctlRequesterInfoGrid.Rows[row.RowIndex].FindControl("ctlActualAmount");
                TextBox ctlActualAmountNotExceed = (TextBox)ctlRequesterInfoGrid.Rows[row.RowIndex].FindControl("ctlActualAmountNotExceed");
                TextBox ctlAmountCompanyPackage = (TextBox)ctlRequesterInfoGrid.Rows[row.RowIndex].FindControl("ctlAmountCompanyPackage");
                TextBox ctlTotalAmount = (TextBox)ctlRequesterInfoGrid.Rows[row.RowIndex].FindControl("ctlTotalAmount");
                TextBox ctlMobilePhoneNo = (TextBox)ctlRequesterInfoGrid.Rows[row.RowIndex].FindControl("ctlMobilePhoneNo");
                TextBox ctlMobileBrand = (TextBox)ctlRequesterInfoGrid.Rows[row.RowIndex].FindControl("ctlMobileBrand");
                TextBox ctlMobileModel = (TextBox)ctlRequesterInfoGrid.Rows[row.RowIndex].FindControl("ctlMobileModel");

                long MPAItemID = UIHelper.ParseInt(ctlRequesterInfoGrid.DataKeys[row.RowIndex].Value.ToString());

                MPAItem mpaItem = new MPAItem(MPAItemID);

                mpaItem.UserID = new SuUser(UIHelper.ParseLong(ctlUserId.Value));
                if (!string.IsNullOrEmpty(ctlActualAmount.Text))
                {
                    mpaItem.ActualAmount = Convert.ToDecimal(ctlActualAmount.Text);
                }
                if (!string.IsNullOrEmpty(ctlActualAmountNotExceed.Text))
                {
                    mpaItem.ActualAmountNotExceed = Convert.ToDecimal(ctlActualAmountNotExceed.Text);
                }
                if (!string.IsNullOrEmpty(ctlAmountCompanyPackage.Text))
                {
                    mpaItem.AmountCompanyPackage = Convert.ToDecimal(ctlAmountCompanyPackage.Text);
                }
                if (!string.IsNullOrEmpty(ctlTotalAmount.Text))
                {
                    mpaItem.TotalAmount = Convert.ToDecimal(ctlTotalAmount.Text);
                }
                mpaItem.MobilePhoneNo = ctlMobilePhoneNo.Text;
                mpaItem.MobileBrand = ctlMobileBrand.Text;
                mpaItem.MobileModel = ctlMobileModel.Text;

                try
                {
                    // Get document requester information and save to transaction.
                    MPAItemService.UpdateMPAItemTransaction(this.TransactionID, mpaItem);
                }
                catch (ServiceValidationException ex)
                {
                    errors.MergeErrors(ex.ValidationErrors);
                }
            }
            #endregion MPAItem

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
            long mpaDocumentID = MPADocumentService.SaveMPADocument(this.TransactionID, this.MPADocumentID);
            // Get ta document
            MPADocument mpaDocument = ScgeAccountingQueryProvider.MPADocumentQuery.FindProxyByIdentity(mpaDocumentID);

            TransactionService.Commit(this.TransactionID);

            #region Work Flow
            long workFlowID = 0;

            // Save New WorkFlow.
            if ((mpaDocument != null) && (mpaDocument.DocumentID != null))
            {
                SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.FindProxyByIdentity(mpaDocument.DocumentID.DocumentID);
                SS.Standard.WorkFlow.DTO.WorkFlow workFlow = new SS.Standard.WorkFlow.DTO.WorkFlow();
                // WorkFlow Type ID = 4 is TA Workflow Image. wait confirm where to store this data.
                workFlow.WorkFlowType = new SS.Standard.WorkFlow.DTO.WorkFlowType(WorkFlowTypeID.MPAWorkFlow);
                // WorkFlow Type ID = 14 is TA Workflow Image. wait confirm where to store this data.
                workFlow.CurrentState = WorkFlowQueryProvider.WorkFlowStateQuery.FindWorkFlowStateIDByTypeIDAndStateName(WorkFlowTypeID.MPAWorkFlow, WorkFlowStateFlag.Draft);
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
            MPADocumentDataSet mpaDocumentDS = (MPADocumentDataSet)TransactionService.GetDS(this.TransactionID);
            MPADocumentDataSet.MPADocumentRow row = mpaDocumentDS.MPADocument.FindByMPADocumentID(this.MPADocumentID);
            long tempDocumentID = row.DocumentID;

            ctlAttachment.Initialize(this.TransactionID, this.MPADocumentID, this.InitialFlag);
            ctlInitiator.ControlGroupID = SCG.eAccounting.BLL.Implement.MPAFieldGroup.All;
            ctlInitiator.Initialize(this.TransactionID, tempDocumentID, this.InitialFlag);
            ctlMPAFormHeader.Initialize(this.TransactionID, tempDocumentID, this.InitialFlag); // send SCGDocument.DocumentID for check visible see history
            ctlHistory.Initialize(tempDocumentID);

            ctlApproverData.DocumentEditor = this;
            ctlCreatorData.ControlGroupID = SCG.eAccounting.BLL.Implement.MPAFieldGroup.All;
            ctlRequesterData.ControlGroupID = SCG.eAccounting.BLL.Implement.MPAFieldGroup.All;
            ctlApproverData.ControlGroupID = SCG.eAccounting.BLL.Implement.MPAFieldGroup.All;

            ctlCreatorData.Initialize(this.TransactionID, this.MPADocumentID, this.InitialFlag);
            ctlRequesterData.Initialize(this.TransactionID, this.MPADocumentID, this.InitialFlag);
            ctlApproverData.Initialize(this.TransactionID, this.MPADocumentID, this.InitialFlag);

            ctlMPAFormHeader.DataBind();
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

                ctlInitiator.RequesterID = UIHelper.ParseLong(ctlRequesterData.UserID);
                ctlApproverData.ShowDefaultApprover(UIHelper.ParseLong(ctlRequesterData.UserID));

                ctlRequesterInfoGrid.DataBind();
            }
            else
            {
                Guid txID = this.TransactionID;
                long mpaDocumentID = this.MPADocumentID;
                MPADocumentDataSet mpaDocumentDS = (MPADocumentDataSet)TransactionService.GetDS(txID);
                MPADocumentDataSet.MPADocumentRow mpaDocumentRow = mpaDocumentDS.MPADocument.FindByMPADocumentID(this.MPADocumentID);
                MPADocumentDataSet.DocumentRow documentRow = mpaDocumentDS.Document.FindByDocumentID(mpaDocumentRow.DocumentID);

                SS.Standard.WorkFlow.DTO.WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(mpaDocumentRow.DocumentID);

                #region Header & Footer
                if (!isCopy)
                {
                    ctlMPAFormHeader.No = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(mpaDocumentRow.DocumentID).DocumentNo;
                    ctlMPAFormHeader.Status = ScgeAccountingQueryProvider.SCGDocumentQuery.GetDocumentCurrentStateName(UserAccount.CurrentLanguageID, mpaDocumentRow.DocumentID);
                    if (documentRow.DocumentDate != DateTime.MinValue)
                    {
                        ctlMPAFormHeader.CreateDate = UIHelper.ToDateString(documentRow.DocumentDate);
                    }
                    ctlCreatorData.SetValue(documentRow.CreatorID);
                }
                else
                {
                    ctlMPAFormHeader.Status = FlagEnum.NewFlag;
                    ctlCreatorData.ShowDefault();
                }

                ctlCompanyField.SetValue(documentRow.CompanyID);
                ctlSubject.Text = documentRow.Subject;
                ctlRequesterData.SetValue(documentRow.RequesterID);
                ctlApproverData.SetValue(documentRow.ApproverID);

                #endregion Header & Footer

                #region Tab Attachment
                ctlAttachment.BindControl();
                #endregion

                #region Tab Memo
                ctlMemo.Text = documentRow.Memo;
                #endregion Tab Memo

                #region Tab General
                #region Header
                ctlStartDateCal.Value = mpaDocumentRow.StartDate;
                ctlEndDateCal.Value = mpaDocumentRow.EndDate;
                
                #endregion Header

                #region ctlRequesterInfoGrid
                ctlRequesterInfoGrid.DataSource = mpaDocumentDS.MPAItem;
                ctlRequesterInfoGrid.DataBind();
                #endregion ctlRequesterInfoGrid

                #endregion Tab General
            }
            this.UpdatePanel();
        }
        #endregion public void BindControl()

        #region private void ResetControlValue()
        private void ResetControlValue()
        {
            this.UserList = null;
            ctlMPAFormHeader.Status = string.Empty;
            ctlMPAFormHeader.No = string.Empty;
            ctlMPAFormHeader.CreateDate = string.Empty;
            ctlCompanyField.ShowDefault();
            ctlSubject.Text = string.Empty;
            ctlCreatorData.ShowDefault();
            ctlRequesterData.ShowDefault();
            ctlApproverData.ShowDefault();
            ctlStartDateCal.Value = null;
            ctlEndDateCal.Value = null;
            ctlRequesterInfoGrid.DataSource = null;
            ctlRequesterInfoGrid.DataBind();
            ctlMemo.Text = string.Empty;

            this.UpdatePanel();
        }
        #endregion private void ResetControlValue()

        #region Grid Event
        #region Function Display Data on Grid
        #region public string DisplayUserID(Object obj)
        public string DisplayUserName(Object obj)
        {
            MPADocumentDataSet.MPAItemRow row = (MPADocumentDataSet.MPAItemRow)((DataRowView)obj).Row;
            SuUser suUser = QueryProvider.SuUserQuery.FindByIdentity(row.UserID);

            if (suUser == null)
                return string.Empty;

            return Server.HtmlEncode(suUser.UserName);
        }
        #endregion public string DisplayUserID(Object obj)

        #region public string DisplayEmployeeName(Object obj)
        public string DisplayEmployeeName(Object obj)
        {
            MPADocumentDataSet.MPAItemRow row = (MPADocumentDataSet.MPAItemRow)((DataRowView)obj).Row;
            SuUser suUser = QueryProvider.SuUserQuery.FindByIdentity(row.UserID);

            if (suUser == null)
                return string.Empty;

            return Server.HtmlEncode(suUser.EmployeeName);
        }
        #endregion public string DisplayEmployeeName(Object obj)

        #region public string DisplayPersonalLevel(Object obj)
        public string DisplayPersonalLevel(Object obj)
        {
            MPADocumentDataSet.MPAItemRow row = (MPADocumentDataSet.MPAItemRow)((DataRowView)obj).Row;
            SuUser suUser = QueryProvider.SuUserQuery.FindByIdentity(row.UserID);

            if (suUser == null)
                return string.Empty;

            return Server.HtmlEncode(suUser.PersonalLevel);
        }
        #endregion public string DisplayPersonalLevel(Object obj)


        #region public string DisplaySectionName(Object obj)
        public string DisplaySectionName(Object obj)
        {
            MPADocumentDataSet.MPAItemRow row = (MPADocumentDataSet.MPAItemRow)((DataRowView)obj).Row;
            SuUser suUser = QueryProvider.SuUserQuery.FindByIdentity(row.UserID);

            if (suUser == null)
                return string.Empty;

            return Server.HtmlEncode(suUser.SectionName);
        }
        #endregion public string DisplaySectionName(Object obj)


        #endregion Function Display Data on Grid

        #region ctlRequesterInfoGrid Event

        #region protected void ctlRequesterInfoGrid_DataBound(object sender, EventArgs e)
        protected void ctlRequesterInfoGrid_DataBound(object sender, EventArgs e)
        {
            if (ctlRequesterInfoGrid.Rows.Count == 0)
            {
                MPAItem mpaItem = new MPAItem();

                mpaItem.MPADocumentID = new MPADocument(this.MPADocumentID);
                mpaItem.UserID = new SuUser(ctlRequesterData.UserAccount.UserID);
                //    taDocumentTraveller.EmployeeNameEng = string.Empty;
                //    taDocumentTraveller.AirLineMember = string.Empty;
                //    taDocumentTraveller.Active = true;

                MPADocumentDataSet mpaItemDS = (MPADocumentDataSet)TransactionService.GetDS(this.TransactionID);

                if (mpaItemDS != null)
                {
                    MPADocumentDataSet.MPAItemDataTable table = mpaItemDS.MPAItem;
                    MPAItemService.AddMPAItemTransaction(this.TransactionID, mpaItem);

                    SuUser user = QueryProvider.SuUserQuery.FindProxyByIdentity(mpaItem.UserID.Userid);

                    if (this.UserList == null)
                    {
                        this.UserList = new List<SuUser>();
                    }
                    UserList.Add(user);

                    ctlRequesterInfoGrid.DataSource = table;
                    ctlRequesterInfoGrid.DataBind();
                    ctlUpdatePanelGeneral.Update();
                }
            }
        }
        #endregion protected void ctlRequesterInfoGrid_DataBound(object sender, EventArgs e)

        #region protected void ctlRequesterInfoGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        protected void ctlRequesterInfoGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("DeleteTravellerInformation"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                long MPAItemID = UIHelper.ParseLong(ctlRequesterInfoGrid.DataKeys[rowIndex].Value.ToString());

                foreach (GridViewRow row in ctlRequesterInfoGrid.Rows)
                {
                    HiddenField ctlUserID = (HiddenField)ctlRequesterInfoGrid.Rows[row.RowIndex].FindControl("ctlUserId");
                    TextBox ctlActualAmount = (TextBox)ctlRequesterInfoGrid.Rows[row.RowIndex].FindControl("ctlActualAmount");
                    TextBox ctlActualAmountNotExceed = (TextBox)ctlRequesterInfoGrid.Rows[row.RowIndex].FindControl("ctlActualAmountNotExceed");
                    TextBox ctlAmountCompanyPackage = (TextBox)ctlRequesterInfoGrid.Rows[row.RowIndex].FindControl("ctlAmountCompanyPackage");
                    TextBox ctlTotalAmount = (TextBox)ctlRequesterInfoGrid.Rows[row.RowIndex].FindControl("ctlTotalAmount");
                    TextBox ctlMobilePhoneNo = (TextBox)ctlRequesterInfoGrid.Rows[row.RowIndex].FindControl("ctlMobilePhoneNo");
                    TextBox ctlMobileBrand = (TextBox)ctlRequesterInfoGrid.Rows[row.RowIndex].FindControl("ctlMobileBrand");
                    TextBox ctlMobileModel = (TextBox)ctlRequesterInfoGrid.Rows[row.RowIndex].FindControl("ctlMobileModel");


                    int MPAItemIDTemp = UIHelper.ParseInt(ctlRequesterInfoGrid.DataKeys[row.RowIndex].Value.ToString());

                    MPAItem mpaItem = new MPAItem(MPAItemIDTemp);

                    //        taDocumentTraveller.TADocumentID = new TADocument(this.DocumentID);
                    mpaItem.UserID = new SuUser(UIHelper.ParseLong(ctlUserID.Value));
                    //        taDocumentTraveller.EmployeeNameEng = ctlEmployeeNameEng.Text;
                    //        taDocumentTraveller.AirLineMember = ctlAirlineMember.Text;
                    //        taDocumentTraveller.Active = true;

                    //        //Get ta document traveller information and save to transaction.
                    MPAItemService.ChangeRequesterMAPItem(this.TransactionID, mpaItem);
                }
                MPADocumentDataSet.MPAItemDataTable table = (MPADocumentDataSet.MPAItemDataTable)MPAItemService.DeleteMPAItemTransaction(this.TransactionID, MPAItemID);

                ctlRequesterInfoGrid.DataSource = table;
                ctlRequesterInfoGrid.DataBind();
                ctlUpdatePanelGeneral.Update();
            }
        }
        #endregion protected void ctlRequesterInfoGrid_RowCommand(object sender, GridViewCommandEventArgs e)

        #region protected void ctlRequesterInfoGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        protected void ctlRequesterInfoGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField ctlUserID = (HiddenField)e.Row.FindControl("ctlUserId");
                ImageButton ctlDelete = (ImageButton)e.Row.FindControl("ctlDelete");

                if (ctlRequesterData.UserID.Equals(ctlUserID.Value))
                {
                    ctlDelete.Visible = false;
                }
            }
        }
        #endregion protected void ctlRequesterInfoGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        #endregion ctlRequesterInfoGrid Event

        
        #endregion Grid Event

        
        #region Lookup User Event
        #region protected void ctlUserProfileLookup_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        protected void ctlUserProfileLookup_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            IList<SuUser> userInfo = (IList<SuUser>)e.ObjectReturn;
            ArrayList userIDArrList = new ArrayList();

            int MPAItemID = 0;

            if (userInfo != null)
            {
                //Save user id to arraylist for check duplicate user id.
                foreach (GridViewRow row in ctlRequesterInfoGrid.Rows)
                {
                    HiddenField ctlUserID = (HiddenField)ctlRequesterInfoGrid.Rows[row.RowIndex].FindControl("ctlUserId");
                    TextBox ctlActualAmount = (TextBox)ctlRequesterInfoGrid.Rows[row.RowIndex].FindControl("ctlActualAmount");
                    TextBox ctlActualAmountNotExceed = (TextBox)ctlRequesterInfoGrid.Rows[row.RowIndex].FindControl("ctlActualAmountNotExceed");
                    TextBox ctlAmountCompanyPackage = (TextBox)ctlRequesterInfoGrid.Rows[row.RowIndex].FindControl("ctlAmountCompanyPackage");
                    TextBox ctlTotalAmount = (TextBox)ctlRequesterInfoGrid.Rows[row.RowIndex].FindControl("ctlTotalAmount");
                    TextBox ctlMobilePhoneNo = (TextBox)ctlRequesterInfoGrid.Rows[row.RowIndex].FindControl("ctlMobilePhoneNo");
                    TextBox ctlMobileBrand = (TextBox)ctlRequesterInfoGrid.Rows[row.RowIndex].FindControl("ctlMobileBrand");
                    TextBox ctlMobileModel = (TextBox)ctlRequesterInfoGrid.Rows[row.RowIndex].FindControl("ctlMobileModel");

                    userIDArrList.Add(ctlUserID.Value);

                    //Update data on grid when add user id. 
                    if (userIDArrList.Contains(ctlUserID.Value))
                    {
                        MPAItemID = UIHelper.ParseInt(ctlRequesterInfoGrid.DataKeys[row.RowIndex].Value.ToString());

                        MPAItem mpaItem = new MPAItem(MPAItemID);

                        mpaItem.MPADocumentID = new MPADocument(this.MPADocumentID);
                        mpaItem.UserID = new SuUser(UIHelper.ParseLong(ctlUserID.Value));
                        if (!string.IsNullOrEmpty(ctlActualAmount.Text))
                        {
                            mpaItem.ActualAmount = Convert.ToDecimal(ctlActualAmount.Text);
                        }
                        if (!string.IsNullOrEmpty(ctlActualAmountNotExceed.Text))
                        {
                            mpaItem.ActualAmountNotExceed = Convert.ToDecimal(ctlActualAmountNotExceed.Text);
                        }
                        if (!string.IsNullOrEmpty(ctlAmountCompanyPackage.Text))
                        {
                            mpaItem.AmountCompanyPackage = Convert.ToDecimal(ctlAmountCompanyPackage.Text);
                        }
                        if (!string.IsNullOrEmpty(ctlTotalAmount.Text))
                        {
                            mpaItem.TotalAmount = Convert.ToDecimal(ctlTotalAmount.Text);
                        }
                        mpaItem.MobilePhoneNo = ctlMobilePhoneNo.Text;
                        mpaItem.MobileBrand = ctlMobileBrand.Text;
                        mpaItem.MobileModel = ctlMobileModel.Text;
                        try
                        {
                            // Get ta document traveller information and save to transaction.
                            MPAItemService.ChangeRequesterMAPItem(this.TransactionID, mpaItem);
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
                            //        TADocumentTraveller taDocumentTraveller = new TADocumentTraveller();
                            MPAItem mpaItem = new MPAItem();
                            mpaItem.MPADocumentID = new MPADocument(this.MPADocumentID);
                            mpaItem.UserID = new SuUser(user.Userid);
                            //mapItem.ActualAmount = Convert.ToDecimal(ctlActualAmount.Text);
                            //mapItem.ActualAmountNotExceed = Convert.ToDecimal(ctlActualAmountNotExceed.Text);
                            //mapItem.AmountCompanyPackage = Convert.ToDecimal(ctlAmountCompanyPackage);
                            //mapItem.TotalAmount = Convert.ToDecimal(ctlTotalAmount);
                            //mapItem.MobilePhoneNo = string.Empty;
                            //mapItem.MobileBrand = string.Empty;
                            //mapItem.MobileModel = string.Empty;

                            MPAItemService.AddMPAItemTransaction(this.TransactionID, mpaItem);

                            if (this.UserList == null)
                            {
                                this.UserList = new List<SuUser>();
                            }
                            UserList.Add(user);
                        }
                    }
                }

                MPADocumentDataSet mpaItemDS = (MPADocumentDataSet)TransactionService.GetDS(this.TransactionID);
                MPADocumentDataSet.MPAItemDataTable table = mpaItemDS.MPAItem;
                ctlRequesterInfoGrid.DataSource = table;
                ctlRequesterInfoGrid.DataBind();
            }
            ctlUpdatePanelGeneral.Update();
            ctlUpdatePanelValidationSummary.Update();
        }
        #endregion protected void ctlUserProfileLookup_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)

        #region protected void ctlUserProfileLookup_Click(object sender, ImageClickEventArgs e)
        protected void ctlUserProfileLookup_Click(object sender, ImageClickEventArgs e)
        {
            ctlUserProfileLookup.isMultiple = true;
            ctlUserProfileLookup.Show();
        }
        #endregion protected void ctlUserProfileLookup_Click(object sender, ImageClickEventArgs e)

        #region protected void ctlRequesterData_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        protected void ctlRequesterData_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            SuUser user = (SuUser)e.ObjectReturn;
            ArrayList userIDArrList = new ArrayList();
            MPADocumentDataSet mpaDocumentDS = (MPADocumentDataSet)TransactionService.GetDS(this.TransactionID);

            long MPAItemID = 0;

            if (user != null)
            {
                ctlInitiator.RequesterID = user.Userid;
                if (ctlRequesterInfoGrid.Rows.Count > 0)
                {
                    //keep data of user id.
                    ctlApproverData.ShowDefaultApprover(UIHelper.ParseLong(ctlRequesterData.UserID));

                    foreach (GridViewRow row in ctlRequesterInfoGrid.Rows)
                    {
                        HiddenField ctlUserID = (HiddenField)ctlRequesterInfoGrid.Rows[row.RowIndex].FindControl("ctlUserId");
                        TextBox ctlEmployeeNameEng = (TextBox)ctlRequesterInfoGrid.Rows[row.RowIndex].FindControl("ctlEmployeeNameEng");
                        TextBox ctlAirlineMember = (TextBox)ctlRequesterInfoGrid.Rows[row.RowIndex].FindControl("ctlAirlineMember");

                        userIDArrList.Add(ctlUserID.Value);

                        //Update data on grid when add user id. 
                        if (userIDArrList.Contains(ctlUserID.Value))
                        {
                            MPAItemID = UIHelper.ParseInt(ctlRequesterInfoGrid.DataKeys[row.RowIndex].Value.ToString());

                            MPAItem mpaItem = new MPAItem(MPAItemID);

                            mpaItem.MPADocumentID = new MPADocument(this.MPADocumentID);
                            mpaItem.UserID = new SuUser(UIHelper.ParseLong(ctlUserID.Value));
                            //    taDocumentTraveller.EmployeeNameEng = ctlEmployeeNameEng.Text;
                            //    taDocumentTraveller.AirLineMember = ctlAirlineMember.Text;
                            //    taDocumentTraveller.Active = true;

                            //    // Get ta document traveller information and save to transaction.
                            MPAItemService.ChangeRequesterMAPItem(this.TransactionID, mpaItem);
                        }
                    }
                    //add requester id on grid when requester id not duplicate user id. 
                    if (!userIDArrList.Contains(user.Userid.ToString()))
                    {
                        MPAItemID = UIHelper.ParseInt(ctlRequesterInfoGrid.DataKeys[0].Value.ToString());

                        MPADocumentDataSet.MPAItemRow row = mpaDocumentDS.MPAItem.FindByMPAItemID(MPAItemID);

                        long currentRequesterID = row.UserID;

                        MPAItem mpaItem = new MPAItem(MPAItemID);

                        mpaItem.MPADocumentID = new MPADocument(this.MPADocumentID);
                        mpaItem.UserID = new SuUser(user.Userid);
                        //    taDocumentTraveller.EmployeeNameEng = string.Empty;
                        //    taDocumentTraveller.AirLineMember = string.Empty;
                        //    taDocumentTraveller.Active = true;

                        MPAItemService.ChangeRequesterMAPItem(this.TransactionID, mpaItem);


                    }
                }
            }

            MPADocumentDataSet.MPAItemDataTable table = mpaDocumentDS.MPAItem;
            ctlRequesterInfoGrid.DataSource = table;
            ctlRequesterInfoGrid.DataBind();

            ctlUpdatePanelGeneral.Update();
            ctlUpdatePanelApprover.Update();
        }
        #endregion protected void ctlRequesterData_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)

        #endregion Lookup User Event

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

        #region public void UpdatePanel()
        public void UpdatePanel()
        {
            ctlUpdatePanelHeader.Update();
            ctlUpdatePanelTab.Update();
            ctlUpdatePanelGeneral.Update();
            ctlUpdatePanelApprover.Update();
            ctlUpdatePanelAttachment.Update();
            ctlUpdatePanelInitial.Update();
            ctlUpdatePanelMemo.Update();
            ctlUpdatePanelValidationSummary.Update();
        }
        #endregion public void UpdatePanel()

        #region public void Copy(long wfid)
        public void Copy(long wfid)
        {
            SS.Standard.WorkFlow.DTO.Document doc = WorkFlowQueryProvider.WorkFlowQuery.GetDocumentByWorkFlowID(wfid);

            if (doc.DocumentType.DocumentTypeID.Equals(DocumentTypeID.MPADocument))
            {
                Response.Redirect(String.Format("~/Forms/SCG.eAccounting/Programs/MPAForm.aspx?cp=1&docId={0}", doc.DocumentID));
            }
        }
        #endregion public void Copy(long wfid)

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

        public bool RequireDocumentAttachment()
        {
            return false;
        }
    }
}

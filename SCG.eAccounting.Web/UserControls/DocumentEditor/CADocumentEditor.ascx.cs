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
using SS.DB.DTO.ValueObject;
using SS.SU.Query;
using SCG.eAccounting.DTO.DataSet;
using SS.Standard.WorkFlow.Query;
using SCG.DB.DTO;
using System.Data;
using SCG.eAccounting.Web.UserControls.DocumentEditor.Components;
using SCG.DB.Query;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor
{
    public partial class CADocumentEditor : BaseUserControl, IDocumentEditor
    {
        private static ILog logger = log4net.LogManager.GetLogger(typeof(ExpenseDocumentEditor));

        #region local variable
        public ISuUserService SuUserService { get; set; }
        public ICADocumentService CADocumentService { get; set; }
        public ITransactionService TransactionService { get; set; }
        public ISCGDocumentService SCGDocumentService { get; set; }
        public IWorkFlowService WorkFlowService { get; set; }
        public IDocumentAttachmentService DocumentAttachmentService { get; set; }
        public IDocumentInitiatorService DocumentInitiatorService { get; set; } 
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
        public long CADocumentID
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


        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        #endregion protected void Page_Load(object sender, EventArgs e)

        #region public void Initialize(string initFlag, long? documentID)
        public void Initialize(string initFlag, long? documentID)
        {
            CADocumentDataSet caDocumentDS;
            long caDocumentID = 0;
            Guid txID = Guid.Empty;
            bool isCopy = false;

            //#region Visible Mode
            //ctlAddRequester.Visible = true;

            //this.isShowFooter = true;
            //#endregion Visible Mode

            if (initFlag.Equals(FlagEnum.NewFlag) && Request.QueryString["cp"] == null)
            {
                caDocumentDS = (CADocumentDataSet)CADocumentService.PrepareDS();
                txID = TransactionService.Begin(caDocumentDS);
                caDocumentID = CADocumentService.AddCADocumentTransaction(txID);

                ctlCAFormHeader.Status = FlagEnum.NewFlag;
            }
            else if (((initFlag.Equals(FlagEnum.EditFlag) || (initFlag.Equals(FlagEnum.ViewFlag)))) && ((documentID.HasValue) && (documentID.Value != 0)))
            {
                caDocumentDS = (CADocumentDataSet)CADocumentService.PrepareDS(documentID.Value);
                txID = TransactionService.Begin(caDocumentDS);

                if (caDocumentDS.Document.Rows.Count > 0)
                {
                    caDocumentID = UIHelper.ParseLong(caDocumentDS.CADocument.Rows[0]["CADocumentID"].ToString());
                }
            }
            else if (Request.QueryString["cp"] != null && UIHelper.ParseLong(Request.QueryString["cp"]) == 1)
            {
                caDocumentDS = (CADocumentDataSet)CADocumentService.PrepareDataToDataset(UIHelper.ParseLong(Request.QueryString["docId"]), true);
                txID = TransactionService.Begin(caDocumentDS);
                isCopy = true;
                if (caDocumentDS.CADocument.Rows.Count > 0)
                {
                    caDocumentID = UIHelper.ParseLong(caDocumentDS.CADocument.Rows[0]["CADocumentID"].ToString());
                }
            }
            this.TransactionID = txID;
            this.CADocumentID = caDocumentID;
            this.InitialFlag = initFlag;

            // Define value to each DocumentEditor Property.
            // And store it in viewstate.
            this.VisibleFields = CADocumentService.GetVisibleFields(documentID);
            this.EditableFields = CADocumentService.GetEditableFields(documentID);

            this.InitializeControl();
            this.BindControl(isCopy);

            #region Show Tab
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

            CADocumentDataSet caDocumentDS = (CADocumentDataSet)TransactionService.GetDS(this.TransactionID);
            if (caDocumentDS == null)
            {
                OnDsNull();
            }
            CADocumentDataSet.CADocumentRow caDocumentRow = caDocumentDS.CADocument.FindByCADocumentID(this.CADocumentID);
            long tempDocumentID = caDocumentRow.DocumentID;

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
            
            scgDocument.DocumentType = new SS.Standard.WorkFlow.DTO.DocumentType(DocumentTypeID.CADocument);
            
            
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

            #region CADocument
            CADocument caDocument = new CADocument(this.CADocumentID);

            caDocument.DocumentID = scgDocument;

            if (ctlSomeTime.Checked)
            {
                caDocument.IsTemporary = true;
                try
                {
                    if (ctlStartDateSumtime.Value != null)
                    {
                        caDocument.StartDate = ctlStartDateSumtime.Value.Value;
                    }

                    if (ctlEndDateSumtime.Value != null)
                    {
                        caDocument.EndDate = ctlEndDateSumtime.Value.Value;
                    }
                }
                catch (FormatException fex)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Invalid_Date_Format"));
                    throw new ServiceValidationException(errors);
                }
            }
            else if (ctlRegular.Checked)
            {
                caDocument.IsTemporary = false;
                try
                {
                    if (ctlStartDate.Value != null)
                    {
                        caDocument.StartDate = ctlStartDate.Value.Value;
                    }

                    if (ctlEndDate.Value != null)
                    {
                        caDocument.EndDate = ctlEndDate.Value.Value;
                    }
                }
                catch (FormatException fex)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Invalid_Date_Format"));
                    throw new ServiceValidationException(errors);
                }
            }

            caDocument.CarLicenseNo = ctlCarLicenseNo.Text;
            caDocument.Brand = ctlBrand.Text;
            caDocument.Model = ctlModel.Text;
            
            switch (ctlDropDownListCategory.SelectedValue)
            {
                case "1":
                    caDocument.OwnerType = OwnerMileage.Employee;
                    break;
                case "2":
                    caDocument.OwnerType = OwnerMileage.Company;
                    break;
                default:
                    break;
            }

            switch (ctlDropDownListType.SelectedValue)
            {
                case "1":
                    caDocument.CarType = TypeOfCar.PrivateCar;
                    break;
                case "2":
                    caDocument.CarType = TypeOfCar.MotorCycle;
                    break;
                case "3":
                    caDocument.CarType = TypeOfCar.Pickup;
                    break;
                default:
                    break;
            }

            if (caDocument.IsTemporary)
            {
                if (ctlWorkInArea.Checked || ctlWorkOutOfArea.Checked)
                {
                    if (ctlWorkInArea.Checked)
                    {
                        caDocument.IsWorkArea = true;
                    }
                    else if (ctlWorkOutOfArea.Checked)
                    {
                        caDocument.IsWorkArea = false;
                        caDocument.Remark = ctlWorkOutOfAreatxt.Text;
                    }
                }
                else
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Please Select Work Area."));
                }
            }
            else
            {
                if (ctlWorkInArea.Checked)
                {
                    caDocument.IsWorkArea = true;
                }
                else if (ctlWorkOutOfArea.Checked)
                {
                    caDocument.IsWorkArea = false;
                    caDocument.Remark = ctlWorkOutOfAreatxt.Text;
                }

            }

            caDocument.Active = true;

            try
            {
                // Get ta document information and save to transaction.
                CADocumentService.UpdateCADocumentTransaction(this.TransactionID, caDocument);
            }
            catch (ServiceValidationException ex)
            {
                errors.MergeErrors(ex.ValidationErrors);
            }

            if (!errors.IsEmpty)
            {
                throw new ServiceValidationException(errors);
            }

            #endregion CADocument

            try
            {
                ctlInitiator.Save();
                DocumentInitiatorService.ValidateDocumentInitiator(this.TransactionID, this.CADocumentID);
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
            long caDocumentID = CADocumentService.SaveCADocument(this.TransactionID, this.CADocumentID);
            // Get ta document
            CADocument caDocument = ScgeAccountingQueryProvider.CADocumentQuery.FindProxyByIdentity(caDocumentID);

            TransactionService.Commit(this.TransactionID);

            #region Work Flow
            long workFlowID = 0;

            // Save New WorkFlow.
            if ((caDocument != null) && (caDocument.DocumentID != null))
            {
                SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.FindProxyByIdentity(caDocument.DocumentID.DocumentID);
                SS.Standard.WorkFlow.DTO.WorkFlow workFlow = new SS.Standard.WorkFlow.DTO.WorkFlow();
                // WorkFlow Type ID = 9 is CA Workflow Image. wait confirm where to store this data.
                workFlow.WorkFlowType = new SS.Standard.WorkFlow.DTO.WorkFlowType(WorkFlowTypeID.CAWorkFlow);
                // WorkFlow Type ID = 9 is CA Workflow Image. wait confirm where to store this data.
                workFlow.CurrentState = WorkFlowQueryProvider.WorkFlowStateQuery.FindWorkFlowStateIDByTypeIDAndStateName(WorkFlowTypeID.CAWorkFlow, WorkFlowStateFlag.Draft);
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
            CADocumentDataSet mpaDocumentDS = (CADocumentDataSet)TransactionService.GetDS(this.TransactionID);
            CADocumentDataSet.CADocumentRow row = mpaDocumentDS.CADocument.FindByCADocumentID(this.CADocumentID);
            long tempDocumentID = row.DocumentID;

            ctlAttachment.Initialize(this.TransactionID, this.CADocumentID, this.InitialFlag);
            ctlInitiator.ControlGroupID = SCG.eAccounting.BLL.Implement.CAFieldGroup.All;
            ctlInitiator.Initialize(this.TransactionID, tempDocumentID, this.InitialFlag);
            ctlCAFormHeader.Initialize(this.TransactionID, tempDocumentID, this.InitialFlag); // send SCGDocument.DocumentID for check visible see history
            ctlHistory.Initialize(tempDocumentID);

            ctlApproverData.DocumentEditor = this;
            ctlCreatorData.ControlGroupID = SCG.eAccounting.BLL.Implement.CAFieldGroup.All;
            ctlRequesterData.ControlGroupID = SCG.eAccounting.BLL.Implement.CAFieldGroup.All;
            ctlApproverData.ControlGroupID = SCG.eAccounting.BLL.Implement.CAFieldGroup.All;

            ctlCreatorData.Initialize(this.TransactionID, this.CADocumentID, this.InitialFlag);
            ctlRequesterData.Initialize(this.TransactionID, this.CADocumentID, this.InitialFlag);
            ctlApproverData.Initialize(this.TransactionID, this.CADocumentID, this.InitialFlag);

            ctlCAFormHeader.DataBind();
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
                ctlRegular.Checked = true;
                if (ctlSomeTime.Checked == true)
                {
                    tableRegular.Visible = false;
                }
                if (ctlRegular.Checked == true)
                {
                    sometimeRegular.Visible = false;
                }
                ctlWorkOutOfAreatxt.Enabled = false;

                ctlInitiator.RequesterID = UIHelper.ParseLong(ctlRequesterData.UserID);
                ctlApproverData.ShowDefaultApprover(UIHelper.ParseLong(ctlRequesterData.UserID));
            }
            else
            {
                Guid txID = this.TransactionID;
                long caDocumentID = this.CADocumentID;
                CADocumentDataSet caDocumentDS = (CADocumentDataSet)TransactionService.GetDS(txID);
                CADocumentDataSet.CADocumentRow caDocumentRow = caDocumentDS.CADocument.FindByCADocumentID(this.CADocumentID);
                CADocumentDataSet.DocumentRow documentRow = caDocumentDS.Document.FindByDocumentID(caDocumentRow.DocumentID);

                SS.Standard.WorkFlow.DTO.WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(caDocumentRow.DocumentID);

                #region Header & Footer
                if (!isCopy)
                {
                    ctlCAFormHeader.No = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(caDocumentRow.DocumentID).DocumentNo;
                    ctlCAFormHeader.Status = ScgeAccountingQueryProvider.SCGDocumentQuery.GetDocumentCurrentStateName(UserAccount.CurrentLanguageID, caDocumentRow.DocumentID);
                    if (documentRow.DocumentDate != DateTime.MinValue)
                    {
                        ctlCAFormHeader.CreateDate = UIHelper.ToDateString(documentRow.DocumentDate);
                    }
                    ctlCreatorData.SetValue(documentRow.CreatorID);
                }
                else
                {
                    ctlCAFormHeader.Status = FlagEnum.NewFlag;
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
                if (!caDocumentRow.IsCarLicenseNoNull())
                {
                ctlCarLicenseNo.Text = caDocumentRow.CarLicenseNo;
                }

                if (!caDocumentRow.IsBrandNull())
                {
                    ctlBrand.Text = caDocumentRow.Brand;
                }

                if (!caDocumentRow.IsModelNull())
                {
                    ctlModel.Text = caDocumentRow.Model;
                }
                if (caDocumentRow.IsTemporary)
                {
                    ctlSomeTime.Checked = true;
                    ctlStartDateSumtime.Value = caDocumentRow.StartDate;
                    ctlEndDateSumtime.Value = caDocumentRow.EndDate;
                    if (ctlSomeTime.Checked == true) 
                    {
                        tableRegular.Visible = false;
                    }
                }
                else {
                    ctlRegular.Checked = true;
                    ctlStartDate.Value = caDocumentRow.StartDate;
                    ctlEndDate.Value = caDocumentRow.EndDate;
                    if (ctlRegular.Checked == true)
                    {
                        sometimeRegular.Visible = false;
                    }
                }


                if (!caDocumentRow.IsIsWorkAreaNull())
                {
                    if (!caDocumentRow.IsIsWorkAreaNull() && caDocumentRow.IsWorkArea)
                    {
                        ctlWorkInArea.Checked = true;
                    }
                    else
                    {
                        ctlWorkOutOfArea.Checked = true;
                        if (!caDocumentRow.IsRemarkNull())
                        {
                            ctlWorkOutOfAreatxt.Text = caDocumentRow.Remark;
                        }
                    }
                }
                switch (caDocumentRow.CarType)
                {
                    case "COM":
                        ctlDropDownListCategory.SelectedIndex = UIHelper.ParseShort("1");
                        break;
                    case "EMP":
                        ctlDropDownListCategory.SelectedIndex = UIHelper.ParseShort("2");
                        break;
                    default:
                        break;
                }
                switch (caDocumentRow.OwnerType)
                {
                    case "PRI":
                        ctlDropDownListType.SelectedIndex = UIHelper.ParseShort("1");
                        break;
                    case "PIC":
                        ctlDropDownListType.SelectedIndex = UIHelper.ParseShort("2");
                        break;
                    case "MOT":
                        ctlDropDownListType.SelectedIndex = UIHelper.ParseShort("3");
                        break;
                    default:
                        break;
                }

                #endregion Tab General
            }

            #region Owner
            IList<SS.DB.DTO.ValueObject.TranslatedListItem> translateList1 = new List<SS.DB.DTO.ValueObject.TranslatedListItem>();

            SS.DB.DTO.ValueObject.TranslatedListItem Owner1 = new SS.DB.DTO.ValueObject.TranslatedListItem();
            Owner1.ID = UIHelper.ParseShort("1");
            Owner1.Symbol = "Company";
            translateList1.Add(Owner1);

            SS.DB.DTO.ValueObject.TranslatedListItem Owner2 = new SS.DB.DTO.ValueObject.TranslatedListItem();
            Owner2.ID = UIHelper.ParseShort("2");
            Owner2.Symbol = "Employee";
            translateList1.Add(Owner2);

            ctlDropDownListCategory.DataSource = translateList1;
            ctlDropDownListCategory.DataTextField = "Symbol";
            ctlDropDownListCategory.DataValueField = "Id";
            ctlDropDownListCategory.DataBind();
            ctlDropDownListCategory.SelectedIndex = 0;
            #endregion
            BindTypeOfCar();
            this.UpdatePanel();
        }
        #endregion public void BindControl()

        #region private void ResetControlValue()
        private void ResetControlValue()
        {
            ctlCAFormHeader.Status = string.Empty;
            ctlCAFormHeader.No = string.Empty;
            ctlCAFormHeader.CreateDate = string.Empty;
            ctlCompanyField.ShowDefault();
            ctlSubject.Text = string.Empty;
            ctlCreatorData.ShowDefault();
            ctlRequesterData.ShowDefault();
            ctlApproverData.ShowDefault();
            ctlStartDate.Value = null;
            ctlEndDate.Value = null;
            ctlMemo.Text = string.Empty;

            this.UpdatePanel();
        }
        #endregion private void ResetControlValue()
        
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

            if (doc.DocumentType.DocumentTypeID.Equals(DocumentTypeID.CADocument))
            {
                Response.Redirect(String.Format("~/Forms/SCG.eAccounting/Programs/CAForm.aspx?cp=1&docId={0}", doc.DocumentID));
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
             try
            {
                DocumentAttachmentService.ValidateDocumentAttachment(this.TransactionID, CADocumentID);

            }
            catch (ServiceValidationException ex)
            {
                ShowWarningRequireAttachmentPopup(GetMessage("CAattachmentIsRequired"));
                return true;
            }

            return false;
        }

        public void ShowWarningRequireAttachmentPopup(string message)
        {
            PopupRequireDocumentAttachment ctlWarningPopup = LoadPopup<PopupRequireDocumentAttachment>("~/UserControls/DocumentEditor/Components/PopupRequireDocumentAttachment.ascx", ctlPopUpWarningRequireAttachmentUpdatePanel);
            ctlWarningPopup.ShowPopup(message);
        }

        public void BindTypeOfCar()
        {
            #region Type of Car
            IList<SS.DB.DTO.ValueObject.TranslatedListItem> translateList2 = new List<SS.DB.DTO.ValueObject.TranslatedListItem>();

            SS.DB.DTO.ValueObject.TranslatedListItem TypeOfCar1 = new SS.DB.DTO.ValueObject.TranslatedListItem();
            TypeOfCar1.ID = UIHelper.ParseShort("1");
            TypeOfCar1.Symbol = "Passenger Car";
            translateList2.Add(TypeOfCar1);


            SS.DB.DTO.ValueObject.TranslatedListItem TypeOfCar3 = new SS.DB.DTO.ValueObject.TranslatedListItem();
            TypeOfCar3.ID = UIHelper.ParseShort("2");
            TypeOfCar3.Symbol = "Pick-up";
            translateList2.Add(TypeOfCar3);


            if (ctlDropDownListCategory.SelectedValue.Equals("2"))
            {
                SS.DB.DTO.ValueObject.TranslatedListItem TypeOfCar2 = new SS.DB.DTO.ValueObject.TranslatedListItem();
                TypeOfCar2.ID = UIHelper.ParseShort("3");
                TypeOfCar2.Symbol = "Motorcycle";
                translateList2.Add(TypeOfCar2);
            }


            ctlDropDownListType.DataSource = translateList2;
            ctlDropDownListType.DataTextField = "Symbol";
            ctlDropDownListType.DataValueField = "Id";
            ctlDropDownListType.DataBind();
            ctlDropDownListType.SelectedIndex = 0;
            #endregion
        }

        protected void ctlRegular_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ctlRegular.Checked == true)
            {
                sometimeRegular.Visible = false;
                tableRegular.Visible = true;
            }
            if (ctlSomeTime.Checked == true)
            {
                tableRegular.Visible = false;
                sometimeRegular.Visible = true;
            }
            this.ctlUpdatePanelTab.Update();
        }

        protected void ctlSomeTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ctlRegular.Checked == true)
            {
                sometimeRegular.Visible = false;
                tableRegular.Visible = true;
            }
            if (ctlSomeTime.Checked == true)
            {
                tableRegular.Visible = false;
                sometimeRegular.Visible = true;
            }
            this.ctlUpdatePanelTab.Update();
        }
        protected void ctlDropDownListCategory_SelectedIndexChanged(object sender, EventArgs e) 
        {
            BindTypeOfCar();
        }
    }
}

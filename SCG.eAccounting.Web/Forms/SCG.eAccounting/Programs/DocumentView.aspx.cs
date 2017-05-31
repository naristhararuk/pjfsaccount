using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SS.Standard.UI;
using SS.Standard.WorkFlow.Query;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.DTO.ValueObject;
using SS.Standard.WorkFlow.Service;
using SS.Standard.WorkFlow.EventUserControl;
using SS.Standard.Security;
using SS.Standard.Utilities;

using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.Web.UserControls.DocumentEditor;
using SCG.eAccounting.DTO;
using SCG.eAccounting.Query;
using SCG.eAccounting.BLL;
using SCG.eAccounting.BLL.Implement;
using SCG.eAccounting.BLL.WorkFlowService;
using SCG.eAccounting.Web.UserControls;
using SCG.eAccounting.Web.UserControls.WorkFlow;
using SCG.eAccounting.Web.UserControls.DocumentEditor.Components;
using SCG.eAccounting.SAP.BAPI.Service.Const;

using SS.DB.Query;
using SCG.eAccounting.DTO.ValueObject;
using SS.SU.DTO.ValueObject;
using SS.SU.DTO;
using SS.SU.Query;

namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs
{
    public partial class DocumentView : BasePage
    {
        #region <-- Desing Variable -->
        public IWorkFlowService WorkFlowService { get; set; }
        public ISCGDocumentService SCGDocumentService { get; set; }
        public IDocumentViewLockService DocumentViewLockService { get; set; }
        public IFnExpenseDocumentService FnExpenseDocumentService { get; set; }
        public IAvAdvanceDocumentService AvAdvanceDocumentService { get; set; }

        public IRemittanceWorkFlowService RemittanceWorkFlowService { get; set; }
        public IAdvanceWorkFlowService AdvanceWorkFlowService { get; set; }
        public IAdvanceWorkFlowService AdvanceForeignWorkFlowService { get; set; }
        public IExpenseWorkFlowService ExpenseWorkFlowService { get; set; }
        #endregion <-- Desing Variable -->

        public long WorkFlowID
        {
            get { return ViewState["WorkFlowID"] == null ? -1 : (long)ViewState["WorkFlowID"]; }
            set { ViewState["WorkFlowID"] = value; }
        }

        public long DocumentID
        {
            get { return ViewState["DocumentID"] == null ? -1 : (long)ViewState["DocumentID"]; }
            set { ViewState["DocumentID"] = value; }
        }

        public int WorkFlowType_ID
        {
            get { return ViewState["WorkFlowTypeID"] == null ? -1 : (int)ViewState["WorkFlowTypeID"]; }
            set { ViewState["WorkFlowTypeID"] = value; }
        }

        public int WorkFlowStateEventID
        {
            get { return ViewState["WorkFlowStateEventID"] == null ? -1 : (int)ViewState["WorkFlowStateEventID"]; }
            set { ViewState["WorkFlowStateEventID"] = value; }
        }

        public string DocumentEditorUserControlPath
        {
            get { return ViewState["DocumentEditorUserControlPath"] == null ? null : (string)ViewState["DocumentEditorUserControlPath"]; }
            set { ViewState["DocumentEditorUserControlPath"] = value; }
        }

        public string AuthorizedEventUserControlPath
        {
            get { return ViewState["AuthorizedEventUserControlPath"] == null ? null : (string)ViewState["AuthorizedEventUserControlPath"]; }
            set { ViewState["AuthorizedEventUserControlPath"] = value; }
        }

        public string WorkFlowMonitorUserControlPath
        {
            get { return ViewState["WorkFlowMonitorUserControlPath"] == null ? null : (string)ViewState["WorkFlowMonitorUserControlPath"]; }
            set { ViewState["WorkFlowMonitorUserControlPath"] = value; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            // use for dynamic load
            PopUpUpdatePanel = ctlPopUpUpdatePanel;
        }

        protected void Page_PreLoad(object sender, EventArgs e)
        {
            if (Request.Params["wfid"] != null)
            {
                if (WorkFlowID == -1)
                {
                    WorkFlowID = UIHelper.ParseLong(Request.Params["wfid"].ToString());
                    WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(WorkFlowID);

                    if (workFlow == null)
                        RedirectMenuPage();

                    if (workFlow.WorkFlowType != null)
                    {
                        this.WorkFlowMonitorUserControlPath = workFlow.WorkFlowType.DisplayControlName;
                        WorkFlowType_ID = workFlow.WorkFlowType.WorkFlowTypeID;
                    }
                }

                if (DocumentID == -1)
                {
                    Document document = WorkFlowQueryProvider.WorkFlowQuery.GetDocumentByWorkFlowID(WorkFlowID);

                    DocumentID = document.DocumentID;
                    DocumentEditorUserControlPath = document.DocumentType.UserControlPath;
                }

                if (DocumentEditorUserControlPath != null)
                {
                    LoadControlDocumentEditor(DocumentEditorUserControlPath);
                }


                if (AuthorizedEventUserControlPath != null)
                {
                    LoadAuthorizedEventControl(AuthorizedEventUserControlPath);
                }

                if (WorkFlowMonitorUserControlPath != null)
                {
                    LoadWorkFlowMonitor(WorkFlowID);
                }
            }

        }

        protected override void OnPreRender(EventArgs e)
        {
            CheckCanLock();
            CheckLock();
            base.OnPreRender(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {


            if (!Page.IsPostBack)
            {
                bool isAllow = bool.Parse(ParameterServices.AllowEveryOneViewDocument);
                if (!isAllow)
                {
                    if (!isPermissionOpenDocument())
                        Response.Redirect("DocumentSecurity.aspx");
                }

                LoadDocumentEditor(DocumentID);
                IList<AuthorizedEvent> authorizedEvent = WorkFlowService.RetrieveAuthorizedEvents(WorkFlowID, UserAccount.CurrentLanguageID);
                ctlAuthurizedEvent.DataTextField = "DisplayName";
                ctlAuthurizedEvent.DataValueField = "EventID";
                ctlAuthurizedEvent.DataSource = authorizedEvent;
                ctlAuthurizedEvent.DataBind();

                if (ctlAuthurizedEvent.Items.Count == 0)
                {
                    ctlSubmit.Visible = false;
                }
                else
                {
                    ctlSubmit.Visible = true;


                    if (ctlAuthurizedEvent.Items.Count == 1 && !authorizedEvent[0].DisplayName.ToLower().Equals("withdraw"))
                    {
                        ctlAuthurizedEvent.Items[0].Selected = true;
                        LoadAuthorizedEvent();
                    }
                }
            }

            BaseUserControl ctl = GetCurrentPopUpControl();

            if (ctl is ConfirmSubmit)
            {
                ctl.OnPopUpReturn += new BaseUserControl.PopUpReturnEventHandler(ctlConfirmPopup_OnNotifyPopup);
            }

            #region check document lock
            string script = @"<script  type='text/javascript' language='javascript'>
                UnLockParameters['documentID']='" + DocumentID + "';UnLockParameters['userID']='" + UserAccount.UserID + "';</script>";
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "unloadscript", script, false);
            //this.Master.BodyTagUnload("onunload", "UnLock('" + DocumentID + "','" + UserAccount.UserID + "','');");
            #endregion check document lock
        }

        private void CheckCanLock()
        {
            if (IsUserCanLockDocument())
            {
                this.ctlDocumentViewLock.Visible = true;

            }
            else
            {
                this.ctlDocumentViewLock.Visible = false;
            }
        }
        private bool IsUserCanLockDocument()
        {
            if (UserAccount.IsAccountant || UserAccount.IsPayment)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        private bool isPermissionOpenDocument()
        {
            Document document = WorkFlowQueryProvider.WorkFlowQuery.GetDocumentByWorkFlowID(WorkFlowID);
            SCGDocument docu = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(document.DocumentID);

            if (document.DocumentType != null)
            {
                if (document.DocumentType.DocumentTypeID == DocumentTypeID.ExpenseDomesticDocument || document.DocumentType.DocumentTypeID == DocumentTypeID.ExpenseForeignDocument)
                    return CanViewExpenseDocument(docu);
                if (document.DocumentType.DocumentTypeID == DocumentTypeID.AdvanceDomesticDocument || document.DocumentType.DocumentTypeID == DocumentTypeID.AdvanceForeignDocument)
                    return CanViewAdvanceDocument(docu);
                if (document.DocumentType.DocumentTypeID == DocumentTypeID.TADocumentDomestic || document.DocumentType.DocumentTypeID == DocumentTypeID.TADocumentForeign)
                    return CanViewTADocument(docu);
                if (document.DocumentType.DocumentTypeID == DocumentTypeID.RemittanceDocument)
                    return CanViewRemittanceDocument(docu);
                if (document.DocumentType.DocumentTypeID == DocumentTypeID.MPADocument)
                    return CanViewMPADocument(docu);
                if (document.DocumentType.DocumentTypeID == DocumentTypeID.CADocument)
                    return CanViewCADocument(docu);

                if (document.DocumentType.DocumentTypeID == DocumentTypeID.FixedAdvanceDocument)
                    return CanViewFixedAdvanceDocument(docu);
            }
           
            return false;
        }

        private bool CanViewDocumentGeneral(long documentID)
        {
            SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(documentID);
            if
            (
                UserAccount.UserID == ((document.RequesterID != null) ? document.RequesterID.Userid : 0) ||
                UserAccount.UserID == ((document.CreatorID != null) ? document.CreatorID.Userid : 0) ||
                UserAccount.UserID == ((document.ReceiverID != null) ? document.ReceiverID.Userid : 0) ||
                UserAccount.UserID == ((document.ApproverID != null) ? document.ApproverID.Userid : 0) ||
                isUserInDocumentInitiator(document) ||
                UserAccount.IsAccountant ||
                UserAccount.IsApproveVerifyDocument ||
                UserAccount.IsApproveVerifyPayment ||
                UserAccount.IsCounterCashier ||
                UserAccount.IsPayment ||
                UserAccount.IsReceiveDocument ||
                UserAccount.IsVerifyDocument ||
                UserAccount.IsVerifyPayment
            )
                return true;

            return false;
        }

        private bool CanViewTADocument(SCGDocument document)
        {
            if (CanViewDocumentGeneral(document.DocumentID))
                return true;


            //If(be a traveler in this TA) return true;
            if (isUserInTADocumentTraveller(document))
                return true;

            TADocument taDocument = ScgeAccountingQueryProvider.TADocumentQuery.GetTADocumentByDocumentID(document.DocumentID);

            if (taDocument != null)
            {
                IList<AvAdvanceDocument> advanceList = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindAdvanceReferenceTAByTADocumentID(taDocument.TADocumentID);

                if (advanceList.Count > 0)
                {
                    foreach (AvAdvanceDocument advance in advanceList)
                    {
                        if (CanViewAdvanceDocument(advance.DocumentID))
                            return true;
                    }
                }

                IList<FnExpenseDocument> expenseList = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.FindExpenseReferenceTAByTADocumentID(taDocument.TADocumentID);

                if (expenseList.Count > 0)
                {
                    foreach (FnExpenseDocument expense in expenseList)
                    {
                        if (CanViewExpenseDocument(expense.Document))
                            return true;
                    }
                }

                IList<FnRemittance> remittanceList = ScgeAccountingQueryProvider.FnRemittanceQuery.FindRemittanceReferenceTAByTADocumentID(taDocument.TADocumentID);

                if (remittanceList.Count > 0)
                {
                    foreach (FnRemittance remitted in remittanceList)
                    {
                        if (CanViewRemittanceDocument(remitted.Document))
                            return true;
                    }
                }
            }

            IList<ProgramRole> programTASearchRoleList = QueryProvider.SuProgramRoleQuery.FindSuProgramRoleByProgramCode(ProgramCodeEnum.TASearch);

            if (UserAccount.UserRole != null)
            {
                foreach (UserRoles role in UserAccount.UserRole)
                {
                    var matchRole = from p in programTASearchRoleList
                                    where p.RoleId == role.RoleID
                                    select p;

                    if (matchRole.Count<ProgramRole>() > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CanViewAdvanceDocument(SCGDocument document)
        {
            if (CanViewDocumentGeneral(document.DocumentID))
                return true;

            AvAdvanceDocument advanceDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(document.DocumentID);

            IList<FnExpenseAdvance> expenseList = ScgeAccountingQueryProvider.FnExpenseAdvanceQuery.FindExpenseReferenceAdvanceByAdvanceID(advanceDocument.AdvanceID);

            if (expenseList.Count > 0)
            {
                foreach (FnExpenseAdvance exp in expenseList)
                {
                    if (CanViewExpenseDocument(exp.Expense.Document))
                        return true;
                }
            }

            IList<FnRemittanceAdvance> remittanceList = ScgeAccountingQueryProvider.FnRemittanceAdvanceQuery.FindRemittanceReferenceAdvanceByAdvanceID(advanceDocument.AdvanceID);

            if (remittanceList.Count > 0)
            {
                foreach (FnRemittanceAdvance rmt in remittanceList)
                {
                    if (CanViewRemittanceDocument(rmt.Remittance.Document))
                        return true;
                }
            }

            return false;
        }

        private bool CanViewRemittanceDocument(SCGDocument document)
        {
            if (CanViewDocumentGeneral(document.DocumentID))
                return true;

            return false;
        }

        private bool CanViewExpenseDocument(SCGDocument document)
        {
            if (CanViewDocumentGeneral(document.DocumentID))
                return true;

            return false;
        }

        private bool CanViewMPADocument(SCGDocument document)
        {
            if (CanViewDocumentGeneral(document.DocumentID))
                return true;

            //If(be a requester in this MPA) return true;
            //if (isUserInMPADocumentItem(document))
            //    return true;

            return false;
        }

        private bool CanViewCADocument(SCGDocument document)
        {
            if (CanViewDocumentGeneral(document.DocumentID))
                return true;

            //If(be a requester in this CA) return true;
            //if (isUserInMPADocumentItem(document))
            //    return true;

            return false;
        }

        private bool CanViewFixedAdvanceDocument(SCGDocument document)
        {
            if (CanViewDocumentGeneral(document.DocumentID))
                return true;

            //If(be a requester in this CA) return true;
            //if (isUserInMPADocumentItem(document))
            //    return true;

            return false;
        }
        private bool isUserInDocumentInitiator(Document document)
        {
            IList<DocumentInitiator> doc = ScgeAccountingQueryProvider.DocumentInitiatorQuery.FindDocumentInitiatorByDocumentID_UserID(document.DocumentID, UserAccount.UserID);
            if (doc.Count > 0)
                return true;
            else
                return false;
        }

        private bool isUserInTADocumentTraveller(Document document)
        {
            TADocument taDoc = ScgeAccountingQueryProvider.TADocumentQuery.GetTADocumentByDocumentID(document.DocumentID);

            IList<TADocumentTraveller> doc = ScgeAccountingQueryProvider.TADocumentTravellerQuery.FindTADocumentTravellerByTADocumentIDAndUserID(taDoc.TADocumentID, UserAccount.UserID);
            if (doc.Count > 0)
                return true;
            else
                return false;
        }

        #region Button Event
        #region ReadOnly Button Event
        protected void ctlSubmit_Click(object sender, ImageClickEventArgs e)
        {
            DateTime lockDate = new DateTime();

            if (IsUserCanLockDocument())
            {
                string msgResult = string.Empty;
                bool IsWonerFlag = false;
                bool lockFlag = DocumentViewLockService.IsDocumentLocked(DocumentID, UserAccount.UserID, ref msgResult, ref IsWonerFlag, ref lockDate);

                if (lockFlag)
                {
                    if (IsWonerFlag)
                    {
                        LockEnableMode(true);
                        DoSubmitForm();
                    }
                    else
                    {
                        if (ctlAuthurizedEvent.SelectedItem != null)
                        {
                            ctlAuthurizedEvent.SelectedItem.Selected = false;
                        }
                        //ctlWarningText.Text = ctlMsgLockByOther.Value;
                        ctlWarningText.Text = string.Format(this.GetProgramMessage("$MsgLockByOther$"), msgResult, lockDate.ToString("dd/MM/yyyy HH:mm:ss"));
                        ctlDocumentLockedPopupExtender.Show();
                        UpdatePanelConfirmDocumentLocked.Update();
                        //this.ctlDocumentViewLock.Checked = true;
                        //this.ctlShowDocumentViewLock.Text = string.Format(this.GetProgramMessage("$ctlMsgShowDocumentViewLock$"),msgResult,lockDate);
                        bool lockFlagOnwer = DocumentViewLockService.IsDocumentLocked(DocumentID, UserAccount.UserID, ref msgResult, ref IsWonerFlag, ref lockDate);
                        if (lockFlagOnwer && IsWonerFlag)
                        {
                            DoSubmitForm();
                        }
                    }

                    // this.ctlShowDocumentViewLock.Text = string.Format(this.GetProgramMessage("$ctlMsgShowDocumentViewLock$"), msgResult, lockDate);
                }
                else
                {
                    AutoTryLock();
                    DoSubmitForm();
                }
                ctlDocumentViewLock.Checked = false;
            }
            else
            {
                DoSubmitForm();
            }



        }

        private void DoSubmitForm()
        {
            try
            {
                bool requireAttachment = false;
                if (ctlAuthurizedEvent.SelectedItem != null && ctlAuthurizedEvent.SelectedItem.ToString() == EventName.Send)
                {
                    if (ctlDocumentEditorPanel.Controls.Count >= 1)
                    {
                        IDocumentEditor documentEditor = ctlDocumentEditorPanel.Controls[0] as IDocumentEditor;
                        requireAttachment = documentEditor.RequireDocumentAttachment();
                    }
                }

                if (!requireAttachment)
                {
                    WorkFlowStateEventID = UIHelper.ParseInt(ctlAuthurizedEvent.SelectedValue);
                    if (WorkFlowStateEventID > 0)
                    {
                        NotifyEvent(WorkFlowStateEventID);

                        //เมื่อ submit แล้วจะต้อง unlock
                        DoUnLock();
                        Response.Redirect("SubmitResult.aspx?wfid=" + WorkFlowID);
                    }
                }
                //เมื่อ submit แล้วจะต้อง unlock
                //DoUnLock();
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                //ctlUpdatePanelValidation.Update();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void NotifyEvent(int WorkFlowStateEventID)
        {
            WorkFlowStateEvent workFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(WorkFlowStateEventID);

            object eventData = GetEventData();

            if (eventData == null)
                eventData = new SubmitResponse(WorkFlowStateEventID);

            WorkFlowService.NotifyEvent(WorkFlowID, workFlowStateEvent.Name, eventData);
        }


        protected void ctlPrint_Click(object sender, ImageClickEventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), string.Empty, "document.location.href = '../Reports/ReportTransferPage.aspx?did=" + this.DocumentID.ToString() + "' ;", true);
        }
        protected void ctlDelete_Click(object sender, ImageClickEventArgs e)
        {
            if (this.DocumentID != -1)
            {
                try
                {
                    //SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.FindProxyByIdentity(this.DocumentID);
                    //SCGDocumentService.Delete(document);
                    WorkFlowService.OnDeleteDocument(this.WorkFlowID);
                    //เมื่อ delete แล้วจะต้อง unlock
                    DoUnLock();

                    Response.Redirect("~/Menu.aspx");
                }
                catch (Exception ex) { }
            }
        }
        protected void ctlEdit_Click(object sender, ImageClickEventArgs e)
        {
            DateTime lockDate = new DateTime();
            if (IsUserCanLockDocument())
            {
                string msgResult = string.Empty;
                bool IsWonerFlag = false;
                bool lockFlag = DocumentViewLockService.IsDocumentLocked(DocumentID, UserAccount.UserID, ref msgResult, ref IsWonerFlag, ref lockDate);

                if (lockFlag)
                {
                    if (IsWonerFlag)
                    {
                        LockEnableMode(true);
                        // this.ctlDocumentViewLock.Checked = true;
                        DoEditForm();
                    }
                    else
                    {
                        if (ctlAuthurizedEvent.SelectedItem != null)
                        {
                            ctlAuthurizedEvent.SelectedItem.Selected = false;
                        }
                        //ctlWarningText.Text = ctlMsgLockByOther.Value;
                        ctlWarningText.Text = string.Format(this.GetProgramMessage("$MsgLockByOther$"), msgResult, lockDate.ToString("dd/MM/yyyy HH:mm:ss"));
                        ctlDocumentLockedPopupExtender.Show();
                        UpdatePanelConfirmDocumentLocked.Update();
                        // this.ctlDocumentViewLock.Checked = true;
                        //this.ctlShowDocumentViewLock.Text = string.Format(this.GetProgramMessage("$ctlMsgShowDocumentViewLock$"), msgResult, lockDate);
                        bool lockFlagOnwer = DocumentViewLockService.IsDocumentLocked(DocumentID, UserAccount.UserID, ref msgResult, ref IsWonerFlag, ref lockDate);
                        if (lockFlagOnwer && IsWonerFlag)
                        {
                            DoEditForm();
                        }
                    }

                    // this.ctlShowDocumentViewLock.Text = string.Format(this.GetProgramMessage("$ctlMsgShowDocumentViewLock$"), msgResult, lockDate);
                }
                else
                {
                    AutoTryLock();
                    DoEditForm();
                }

            }
            else
            {
                DoEditForm();
            }
            ctlDocumentViewLock.Checked = false;
        }

        protected void ctlPrintPayIn_Click(object sender, ImageClickEventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), string.Empty, "document.location.href = '../Reports/ReportTransferPage.aspx?did=" + this.DocumentID.ToString() + "&payin=true' ;", true);
        }

        private void DoEditForm()
        {
            if (ctlDocumentEditorPanel.Controls.Count < 1) return;
            IDocumentEditor documentEditor = ctlDocumentEditorPanel.Controls[0] as IDocumentEditor;
            documentEditor.RollBackTransaction();
            documentEditor.Initialize(FlagEnum.EditFlag, DocumentID);

            ctlDivReadOnlyButton.Visible = false;
            ctlDivReadWriteButton.Visible = true;

            ctlToolBarUpdatePanel.Update();

            ctlDocumentEventPanel.Visible = false;
            ctlDocumentEventUpdatePanel.Update();

            ctlDocumentEventUserControlPanel.Visible = false;
            ctlDocumentEventUserControlUpdatePanel.Update();
        }
        #endregion

        #region ReadWrite Button Event
        protected void ctlSave_Click(object sender, ImageClickEventArgs e)
        {
            DateTime lockDate = new DateTime();
            string msgResult = string.Empty;
            bool IsWonerFlag = false;
            bool lockFlag = DocumentViewLockService.IsDocumentLocked(DocumentID, UserAccount.UserID, ref msgResult, ref IsWonerFlag, ref lockDate);

            if (lockFlag)
            {
                if (IsWonerFlag)
                {
                    LockEnableMode(true);
                    //this.ctlDocumentViewLock.Checked = true;
                    DoSaveForm();
                }
                else
                {
                    if (ctlAuthurizedEvent.SelectedItem != null)
                    {
                        ctlAuthurizedEvent.SelectedItem.Selected = false;
                    }
                    //ctlWarningText.Text = ctlMsgLockByOther.Value;
                    ctlWarningText.Text = string.Format(this.GetProgramMessage("$MsgLockByOther$"), msgResult, lockDate.ToString("dd/MM/yyyy HH:mm:ss"));
                    ctlDocumentLockedPopupExtender.Show();
                    UpdatePanelConfirmDocumentLocked.Update();
                    //this.ctlDocumentViewLock.Checked = true;
                    //this.ctlShowDocumentViewLock.Text = string.Format(this.GetProgramMessage("$ctlMsgShowDocumentViewLock$"), msgResult, lockDate);
                    bool lockFlagOnwer = DocumentViewLockService.IsDocumentLocked(DocumentID, UserAccount.UserID, ref msgResult, ref IsWonerFlag, ref lockDate);
                    if (lockFlagOnwer && IsWonerFlag)
                    {
                        DoSaveForm();
                    }
                }

                //this.ctlShowDocumentViewLock.Text = string.Format(this.GetProgramMessage("$ctlMsgShowDocumentViewLock$"), msgResult, lockDate);
            }
            else
            {
                AutoTryLock();
                DoSaveForm();
            }



            ctlDocumentViewLock.Checked = false;
        }

        private void DoSaveForm()
        {
            if (ctlDocumentEditorPanel.Controls.Count < 1) return;
            IDocumentEditor documentEditor = ctlDocumentEditorPanel.Controls[0] as IDocumentEditor;

            try
            {
                if (!documentEditor.RequireDocumentAttachment())
                {
                    long wfid = documentEditor.Save();

                    if (wfid > 0)
                    {
                        //เมื่อ save แล้วจะต้อง unlock
                        DoUnLock();

                        if (checkShowConfrim(wfid))
                        {
                            ShowComfirmPopup();
                        }
                        else
                        {
                            WorkFlowService.ReCalculateWorkFlowPermission(WorkFlowID);

                            Response.Redirect("DocumentView.aspx?wfid=" + WorkFlowID.ToString());
                        }
                    }
                }
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                //ctlUpdatePanelValidation.Update();
            }
        }

        private bool checkShowConfrim(long wfid)
        {
            bool isShowConfrim = true;//สำหรับ show confrim send document to workflow ;ใช้ในกรณีที่เป็น draft ที่ไม่มี document เท่านั้น ยกเว้น create advance from ta
            WorkFlow workflow = WorkFlowQueryProvider.WorkFlowQuery.FindProxyByIdentity(wfid);
            if (workflow != null)
            {
                //find state.name only draft and not documentNo
                WorkFlowState wfState = WorkFlowQueryProvider.WorkFlowStateQuery.FindProxyByIdentity(workflow.CurrentState.WorkFlowStateID);
                if (wfState != null)
                {
                    #region 1. current state of document is only "Draft"
                    if (wfState.Name.Equals(WorkFlowStateFlag.Draft))
                    {
                        #region 2. documentNO is null.
                        string docNo = workflow.Document.DocumentNo;
                        if (string.IsNullOrEmpty(docNo))
                        {
                            #region 3. workflowtype = advance ?
                            if (workflow.WorkFlowType.WorkFlowTypeID.Equals(WorkFlowTypeID.AdvanceWorkFlowType))
                            {
                                //find documentID for find tadocumentID from Advance
                                long docID = workflow.Document.DocumentID;
                                AvAdvanceDocument advanceDoc = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(docID);
                                if (advanceDoc != null)
                                {
                                    //กรณี check ว่าเป็น advance ที่สร้างจาก ta หรือเปล่า ต้อง check ที่ tadocumentID ว่ามี state เป็นอะไร ถ้าเป็น complete แปลว่าสร้างจาก advance แต่ถ้าเป็น TA ที่ไม่ complete แปลว่าเป็นการสร้าง advance จาก TA
                                    #region 4. tadocumentID = null ?
                                    if (advanceDoc.TADocumentID != null)
                                    {
                                        //find current state of TAdocument for check state is complete?
                                        //if yes = create advance document from advance
                                        // else no = create advance document from TA
                                        long tadocID = advanceDoc.TADocumentID.Value;
                                        TADocument taDoc = ScgeAccountingQueryProvider.TADocumentQuery.FindProxyByIdentity(tadocID);
                                        if (taDoc != null)
                                        {
                                            long docIDTa = taDoc.DocumentID.DocumentID;
                                            WorkFlow wfTA = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(docIDTa);
                                            if (wfTA != null)
                                            {
                                                #region 5. currentState = complete ?
                                                int currentState = wfTA.CurrentState.WorkFlowStateID;
                                                WorkFlowState workflowState = WorkFlowQueryProvider.WorkFlowStateQuery.FindProxyByIdentity(currentState);
                                                if (workflowState != null)
                                                {
                                                    if (workflowState.Name.Equals(WorkFlowStateFlag.Complete))
                                                    {
                                                        isShowConfrim = true;
                                                    }
                                                    else
                                                        isShowConfrim = false;
                                                }
                                                #endregion
                                            }
                                        }
                                    }
                                    else
                                        isShowConfrim = true;
                                    #endregion
                                }
                            }
                            else
                                isShowConfrim = true;
                            #endregion
                        }
                        else
                            isShowConfrim = false;
                        #endregion
                    }
                    else
                        isShowConfrim = false;
                    #endregion
                }
            }
            return isShowConfrim;
        }

        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            if (ctlDocumentEditorPanel.Controls.Count < 1) return;
            IDocumentEditor documentEditor = ctlDocumentEditorPanel.Controls[0] as IDocumentEditor;

            documentEditor.RollBackTransaction();
            this.LoadDocumentEditor(DocumentID);

            DoUnLock();

            ctlDivReadOnlyButton.Visible = true;
            ctlDivReadWriteButton.Visible = false;

            ctlToolBarUpdatePanel.Update();

            ctlDocumentEventPanel.Visible = true;
            ctlDocumentEventUpdatePanel.Update();
            Response.Redirect("DocumentView.aspx?wfid=" + WorkFlowID.ToString());
        }
        #endregion
        #endregion

        #region Radio Button Event
        protected void ctlAuthurizedEvent_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime lockDate = new DateTime();
            ClearAuthorizedEventControl();
            AuthorizedEventUserControlPath = null;

            if (ctlAuthurizedEvent.SelectedItem != null)
            {
                if (IsUserCanLockDocument())
                {
                    string msgResult = string.Empty;
                    bool IsWonerFlag = false;
                    bool lockFlag = DocumentViewLockService.IsDocumentLocked(DocumentID, UserAccount.UserID, ref msgResult, ref IsWonerFlag, ref lockDate);

                    if (lockFlag)
                    {
                        if (IsWonerFlag)
                        {
                            LockEnableMode(true);
                            //this.ctlDocumentViewLock.Checked = true;
                            this.ctlShowDocumentViewLock.Style["display"] = "none";
                        }
                        else
                        {

                            //ctlWarningText.Text = ctlMsgLockByOther.Value;
                            ctlWarningText.Text = string.Format(this.GetProgramMessage("$MsgLockByOther$"), msgResult, lockDate.ToString("dd/MM/yyyy HH:mm:ss"));
                            ctlDocumentLockedPopupExtender.Show();
                            UpdatePanelConfirmDocumentLocked.Update();
                            //this.ctlDocumentViewLock.Checked = true;
                            this.ctlShowDocumentViewLock.Style["display"] = "block";
                            this.ctlShowDocumentViewLock.Text = string.Format(this.GetProgramMessage("$ctlMsgShowDocumentViewLock$"), msgResult, lockDate.ToString("dd/MM/yyyy HH:mm:ss"));

                        }
                        //  this.ctlShowDocumentViewLock.Visible = true;
                        //  this.ctlShowDocumentViewLock.Text = string.Format(this.GetProgramMessage("$ctlMsgShowDocumentViewLock$"),msgResult,lockDate);
                    }
                    else
                    {
                        AutoTryLock();
                    }
                    if (ctlAuthurizedEvent.SelectedValue != null)
                    {
                        LoadAuthorizedEvent();
                    }
                }
                else
                {
                    LoadAuthorizedEvent();
                }

            }
            ctlDocumentViewLock.Checked = false;
        }

        private void LoadAuthorizedEvent()
        {
            WorkFlowStateEventID = UIHelper.ParseInt(ctlAuthurizedEvent.SelectedValue);
            WorkFlowStateEvent workFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(WorkFlowStateEventID);
            if (workFlowStateEvent != null && !string.IsNullOrEmpty(workFlowStateEvent.UserControlPath))
            {
                LoadAuthorizedEventControl(workFlowStateEvent.UserControlPath);
                ctlDocumentEventUpdatePanel.Update();
            }
        }
        #endregion

        #region Private Event
        private void LoadWorkFlowMonitor(long workFlowID)
        {
            if (ctlWorkFlowMonitorPanel.Controls.Count == 0)
                ctlWorkFlowMonitorPanel.Controls.Add(LoadControl(WorkFlowMonitorUserControlPath));

            IWorkFlowMonitor workflowMonitor = ctlWorkFlowMonitorPanel.Controls[0] as IWorkFlowMonitor;
            workflowMonitor.Initialize(workFlowID);
        }
        private void LoadControlDocumentEditor(string userControlPath)
        {
            if (DocumentEditorUserControlPath == null)
                DocumentEditorUserControlPath = userControlPath;
            Control control = LoadControl(userControlPath);
            (control as IDocumentEditor).DsNull += NotifyDsNull;
            ctlDocumentEditorPanel.Controls.Add(control);
        }

        void NotifyDsNull()
        {
            Response.Redirect("DocumentView.aspx?wfid=" + WorkFlowID.ToString());
        }

        private void LoadDocumentEditor(long documentID)
        {
            if (ctlDocumentEditorPanel.Controls.Count < 1) return;
            IDocumentEditor documentEditor = ctlDocumentEditorPanel.Controls[0] as IDocumentEditor;

            documentEditor.Initialize(FlagEnum.ViewFlag, documentID);

            this.ctlEdit.Visible = WorkFlowService.CanEditDocument(WorkFlowID);
            this.ctlDelete.Visible = WorkFlowService.CanDeleteDocument(WorkFlowID);
            this.ctlCopy.Visible = WorkFlowService.CanCopyDocument(WorkFlowID);
            this.ctlPrintPayIn.Visible = WorkFlowService.CanPrintPayInDocument(WorkFlowID);
        }
        private void LoadAuthorizedEventControl(string userControlPath)
        {

            Control eventControl = LoadControl(userControlPath);
            eventControl.ID = "AuthorizedEventUserControl";
            ctlDocumentEventUserControlPanel.Controls.Add(eventControl);
            ctlDocumentEventUserControlUpdatePanel.Update();

            if (AuthorizedEventUserControlPath == null)
            {
                AuthorizedEventUserControlPath = userControlPath;
                InitializeAuthorizedEventControl();
            }
        }

        private void InitializeAuthorizedEventControl()
        {
            if (ctlDocumentEventUserControlPanel.Controls.Count < 1) return;
            IEventControl responseControl = ctlDocumentEventUserControlPanel.Controls[0] as IEventControl;
            responseControl.WorkFlowStateEventID = WorkFlowStateEventID;
            responseControl.Initialize(WorkFlowID);
        }
        private void ClearAuthorizedEventControl()
        {
            //AuthorizedEventUserControlPath = null;
            ctlDocumentEventUserControlPanel.Controls.Clear();
            ctlDocumentEventUserControlUpdatePanel.Update();
        }
        private object GetEventData()
        {
            if (ctlDocumentEventUserControlPanel.Controls.Count < 1) return null;
            IEventControl eventControl = ctlDocumentEventUserControlPanel.Controls[0] as IEventControl;
            if (eventControl != null)
                return eventControl.GetEventData(WorkFlowStateEventID);
            else
                return null;
        }
        #endregion

        protected void ctlConfirmPopup_OnNotifyPopup(object sender, PopUpReturnArgs args)
        {
            WorkFlowService.ReCalculateWorkFlowPermission(WorkFlowID);
            if (args.Type.Equals(PopUpReturnType.OK))
            {
                try
                {
                    WorkFlowStateEvent workFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetSendDraftStateEvent(WorkFlowType_ID);
                    WorkFlowStateEventID = workFlowStateEvent.WorkFlowStateEventID;

                    object eventData = GetEventData();

                    if (eventData == null)
                        eventData = new SubmitResponse(WorkFlowStateEventID);

                    WorkFlowService.NotifyEvent(WorkFlowID, workFlowStateEvent.Name, eventData);
                    Response.Redirect("SubmitResult.aspx?wfid=" + WorkFlowID);
                }
                catch (ServiceValidationException ex)
                {
                    this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                    //ctlUpdatePanelValidation.Update();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                if (args.Type.Equals(PopUpReturnType.Cancel))
                {
                    Response.Redirect("DocumentView.aspx?wfid=" + WorkFlowID.ToString());
                }
                else
                {
                    Response.Redirect("DocumentView.aspx?WFID=" + this.WorkFlowID.ToString() + "&eventFlag=" + PopUpReturnType.Add);
                }
            }
        }

        public void ShowComfirmPopup()
        {
            ConfirmSubmit ctlConfirmPopup = LoadPopup<ConfirmSubmit>("~/UserControls/DocumentEditor/Components/ConfirmSubmit.ascx", ctlPopUpUpdatePanel);
            ctlConfirmPopup.ShowConfirm();
        }

        protected void ctlCopy_Click(object sender, ImageClickEventArgs e)
        {
            if (ctlDocumentEditorPanel.Controls.Count < 1) return;
            IDocumentEditor documentEditor = ctlDocumentEditorPanel.Controls[0] as IDocumentEditor;
            documentEditor.Copy(WorkFlowID);
        }
        protected void ctlDocumentViewLock_CheckedChanged(object sender, EventArgs e)
        {
            DateTime lockDate = new DateTime();
            string msgResult = string.Empty;
            bool IsWonerFlag = false;
            bool lockFlag = DocumentViewLockService.IsDocumentLocked(DocumentID, UserAccount.UserID, ref msgResult, ref IsWonerFlag, ref lockDate);

            if (lockFlag)
            {
                if (IsWonerFlag)
                {
                    LockEnableMode(true);
                    //ถ้าเป็นคนที่ lock ไว้แล้วกดออก หมายถึงต้องการ unlock
                    if (!ctlDocumentViewLock.Checked)
                    {
                        ctlWarningText.Text = ctlMsgUnLock.Value;
                        ctlDoLock.Text = "UnLock This Document";
                        ctlDocumentLockedPopupExtender.Show();
                        UpdatePanelConfirmDocumentLocked.Update();
                    }
                }
                else
                {
                    ctlWarningText.Text = ctlMsgLockByOther.Value;
                    //ctlWarningText1.Text = ctlMsgForceLock.Value;
                    ctlDocumentLockedPopupExtender.Show();
                    UpdatePanelConfirmDocumentLocked.Update();
                    //this.ctlDocumentViewLock.Checked = true;
                    // this.ctlShowDocumentViewLock.Text = string.Format(this.GetProgramMessage("$ctlMsgShowDocumentViewLock$"), msgResult, lockDate);
                }
            }
            else
            {
                if (ctlDocumentViewLock.Checked)
                {
                    DoTryLock();
                }
                else
                {
                    ctlWarningText.Text = ctlMsgDocumentAvilable.Value;
                    ctlDoLock.Text = "Lock This Document";
                    ctlDocumentLockedPopupExtender.Show();
                    UpdatePanelConfirmDocumentLocked.Update();
                }

            }

            ctlDocumentViewLock.Checked = false;
        }

        private void CheckLock()
        {
            string lockByEmployeeName = string.Empty;
            bool IsWonerFlag = false;
            DateTime lockDate = new DateTime();
            bool lockFlag = DocumentViewLockService.IsDocumentLocked(DocumentID, UserAccount.UserID, ref lockByEmployeeName, ref IsWonerFlag, ref lockDate);
            if (lockFlag)
            {
                if (IsWonerFlag)
                {
                    if (ctlAuthurizedEvent.SelectedItem != null)
                    {
                        ctlAuthurizedEvent.SelectedItem.Selected = true;
                    }
                    LockEnableMode(true);
                    //this.ctlShowDocumentViewLock.Visible = true;
                    //this.ctlShowDocumentViewLock.Text = string.Format(this.GetProgramMessage("$ctlMsgShowDocumentViewLock$"), lockByEmployeeName, lockDate);
                    this.ctlShowDocumentViewLock.Style["display"] = "none";
                }
                else
                {
                    LockEnableMode(false);
                    //this.ctlShowDocumentViewLock.Visible = true;
                    //this.ctlShowDocumentViewLock.Text = string.Format(this.GetProgramMessage("$ctlMsgShowDocumentViewLock$"), lockByEmployeeName, lockDate);
                    this.ctlShowDocumentViewLock.Style["display"] = "block";

                }
                //this.ctlShowDocumentViewLock.Visible = true;
                this.ctlShowDocumentViewLock.Text = string.Format(this.GetProgramMessage("$ctlMsgShowDocumentViewLock$"), lockByEmployeeName, lockDate.ToString("dd/MM/yyyy HH:mm:ss"));
            }
            else
            {
                LockEnableMode(true);
                //this.ctlShowDocumentViewLock.Visible = false;
                this.ctlShowDocumentViewLock.Style["display"] = "none";
            }
        }

        private void AutoTryLock()
        {
            bool LockFlag = DocumentViewLockService.TryLock(DocumentID, UserAccount.UserID, ctlDocumentViewLock.Checked);
            if (LockFlag)
            {
                LockEnableMode(true);
            }
            else
            {
                LockEnableMode(false);
            }

            ctlDocumentViewLock.Checked = false;
            ctlDocumentLockedPopupExtender.Hide();
            UpdatePanelConfirmDocumentLocked.Update();

            ctlToolBarUpdatePanel.Update();
            ctlDocumentEventUpdatePanel.Update();

        }

        private void DoTryLock()
        {
            try
            {
                bool LockFlag = DocumentViewLockService.TryLock(DocumentID, UserAccount.UserID, ctlDocumentViewLock.Checked);
                if (LockFlag)
                {
                    if (ctlDocumentViewLock.Checked)
                    {
                        LockEnableMode(true);

                    }
                    else
                    {
                        if (ctlAuthurizedEvent.SelectedItem != null)
                        {
                            ctlAuthurizedEvent.SelectedItem.Selected = false;
                            LockEnableMode(false);

                        }
                        else
                        {
                            LockEnableMode(true);

                        }

                    }
                }

                ctlDocumentViewLock.Checked = false;
                ctlDocumentLockedPopupExtender.Hide();
                UpdatePanelConfirmDocumentLocked.Update();
                ctlToolBarUpdatePanel.Update();
            }
            catch (Exception)
            {

                throw;
            }


        }

        private void DoForceLock()
        {
            try
            {
                bool LockFlag = DocumentViewLockService.ForceLock(DocumentID, UserAccount.UserID);
                if (LockFlag)
                {
                    if (ctlAuthurizedEvent.SelectedItem != null)
                    {
                        ctlAuthurizedEvent.SelectedItem.Selected = true;
                    }

                    LockEnableMode(true);
                    // this.ctlDocumentViewLock.Checked = true;
                    //this.ctlShowDocumentViewLock.Text = string.Format(this.GetProgramMessage("$ctlMsgShowDocumentViewLock$"), UserAccount.EmployeeName, DateTime.Now);

                }
                else
                {
                    LockEnableMode(false);
                    // this.ctlDocumentViewLock.Checked = false;
                }

                ctlDocumentViewLock.Checked = false;

                ctlDocumentLockedPopupExtender.Hide();
                UpdatePanelConfirmDocumentLocked.Update();

                ctlToolBarUpdatePanel.Update();
                ctlDocumentEventUpdatePanel.Update();

            }
            catch (Exception ex)
            {
                this.ctlShowDocumentViewLock.Style["display"] = "block";
                this.ctlShowDocumentViewLock.Text = string.Format(this.GetProgramMessage("$ctlMsgShowDocumentViewLock$"), ex.Message, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            }
        }

        public void CancelPopUpLock(object sender, EventArgs e)
        {
            try
            {
                ctlDocumentViewLock.Checked = false;
                ctlDocumentLockedPopupExtender.Hide();
                UpdatePanelConfirmDocumentLocked.Update();

            }
            catch (Exception)
            {

                throw;
            }

        }

        private void LockEnableMode(bool IsLock)
        {
            this.ctlSave.Enabled = IsLock;

            this.ctlEdit.Enabled = IsLock;

            this.ctlDelete.Enabled = IsLock;

            this.ctlSubmit.Enabled = IsLock;

            this.ctlAuthurizedEvent.Enabled = IsLock;

            this.ctlCopy.Enabled = IsLock;

            IDocumentEditor documentEditor = ctlDocumentEditorPanel.Controls[0] as IDocumentEditor;
            documentEditor.EnabledViewPostButton(IsLock);
            documentEditor.EnabledPostRemittanceButton(IsLock);
        }

        public void DoClose(object sender, EventArgs e)
        {
            if (this.ctlDocumentViewLock.Checked)
            {
                this.ctlDocumentViewLock.Checked = false;
            }

            if (ctlAuthurizedEvent.SelectedItem != null)
            {
                if (ctlAuthurizedEvent.SelectedItem.Selected)
                {
                    ctlAuthurizedEvent.SelectedItem.Selected = false;
                }
            }
            ctlDocumentLockedPopupExtender.Hide();
            UpdatePanelConfirmDocumentLocked.Update();

            ctlToolBarUpdatePanel.Update();
            ctlDocumentEventUpdatePanel.Update();

        }
        public void DoLock(object sender, EventArgs e)
        {
            if (ctlDoLock.Text == "Force Lock")
            {
                // Force Lock
                DoForceLock();
            }
            else if (ctlDoLock.Text == "UnLock This Document")
            {
                //Unlock
                DoUnLock();
            }
            else if (ctlDoLock.Text == "Lock This Document")
            {
                DoTryLock();
            }


        }
        public void DoCancel(object sender, EventArgs e)
        {
            if (this.ctlDocumentViewLock.Checked)
            {
                this.ctlDocumentViewLock.Checked = false;
            }

            if (ctlAuthurizedEvent.SelectedItem != null)
            {
                if (ctlAuthurizedEvent.SelectedItem.Selected)
                {
                    ctlAuthurizedEvent.SelectedItem.Selected = false;
                }
            }
            ctlDocumentLockedPopupExtender.Hide();
            UpdatePanelConfirmDocumentLocked.Update();

            ctlToolBarUpdatePanel.Update();
            ctlDocumentEventUpdatePanel.Update();


        }
        private void DoUnLock()
        {
            try
            {
                bool lockFlag = DocumentViewLockService.UnLock(DocumentID, UserAccount.UserID);
                this.ctlShowDocumentViewLock.Style["display"] = "none";
                //this.ctlShowDocumentViewLock.Text = "";
                ctlDocumentLockedPopupExtender.Hide();
                UpdatePanelConfirmDocumentLocked.Update();
                if (ctlAuthurizedEvent.SelectedItem != null)
                {
                    ctlAuthurizedEvent.SelectedItem.Selected = false;

                }
                ctlDocumentViewLock.Checked = false;
                ctlToolBarUpdatePanel.Update();
                ctlDocumentEventUpdatePanel.Update();

            }
            catch (Exception)
            {

                throw;
            }


        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.Query;
using SS.Standard.Data.Linq;
using SCG.eAccounting.DTO;
using SS.DB.Query;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor.Components
{
    public partial class DocumentHeader : BaseUserControl, IEditorComponent
    {
        public SCG.eAccounting.BLL.WorkFlowService.IExpenseWorkFlowService ExpenseWorkFlowService { get; set; }
        public SCG.eAccounting.BLL.WorkFlowService.IAdvanceWorkFlowService AdvanceWorkFlowService { get; set; }
        public SCG.eAccounting.BLL.WorkFlowService.IAdvanceWorkFlowService AdvanceForeignWorkFlowService { get; set; }

        public Guid TransactionId
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
            get { return ViewState[ViewStateName.InitialFlag].ToString(); }
            set { ViewState[ViewStateName.InitialFlag] = value; }
        }
        public string HeaderForm
        {
            set { ctlDocumentHeader.Text = value; }
        }
        public string Status
        {
            get { return ctlStatus.Text; }
            set { ctlStatus.Text = value; }
        }
        public string No
        {
            get { return ctlAdvanceNo.Text; }
            set { ctlAdvanceNo.Text = value; }
        }
        public string CreateDate
        {
            set { ctlDate.Text = value; }
        }
        public string labelNo
        {
            set { ctlAdvanceNoLabel.Text = value; }
        }
        public bool isSeeHistory
        {
            get { return ctlSeeHistory.Visible; }
            set { ctlSeeHistory.Visible = value; }
        }
        public bool isSeeMessage
        {
            get { return ctlSeeMessage.Visible; }
            set { ctlSeeMessage.Visible = value; }
        }

        public void Initialize(Guid txID, long documentID, string initFlag)
        {
            this.TransactionId = txID;
            this.DocumentID = documentID;
            this.InitialFlag = initFlag;
            int messageCode = 0;
            string message = string.Empty;
            bool boolVerify = false;
            bool boolVerifyAndApproveVerify = false;
            bool boolApprove = false;
            bool boolApproveDocument = false;
            bool showWarningChangeAmount = false;

            long workFlowID = long.Parse(Request.QueryString["wfid"] == null ? "0" : Request.QueryString["wfid"].Trim());
            if (workFlowID > 0)
            {
                SS.Standard.WorkFlow.DTO.WorkFlow workflow = SS.Standard.WorkFlow.Query.WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
                SS.Standard.WorkFlow.DTO.Document document = SS.Standard.WorkFlow.Query.WorkFlowQueryProvider.WorkFlowQuery.GetDocumentByWorkFlowID(workFlowID);

                if (document.DocumentType.DocumentTypeID == 1) //AdvanceDomesticDocument
                {
                    boolVerify = AdvanceWorkFlowService.CanVerifyWaitVerify(workFlowID);
                    boolVerifyAndApproveVerify = AdvanceWorkFlowService.CanVerifyAndApproveVerifyWaitVerify(workFlowID);
                    boolApprove = AdvanceWorkFlowService.CanApproveWaitApproveVerify(workFlowID);
                    boolApproveDocument = AdvanceWorkFlowService.CanApproveWaitApprove(workFlowID);
                }
                else if (document.DocumentType.DocumentTypeID == 5) //AdvanceForeignDocument
                {
                    boolVerify = AdvanceForeignWorkFlowService.CanVerifyWaitVerify(workFlowID);
                    boolVerifyAndApproveVerify = AdvanceForeignWorkFlowService.CanVerifyAndApproveVerifyWaitVerify(workFlowID);
                    boolApprove = AdvanceForeignWorkFlowService.CanApproveWaitApproveVerify(workFlowID);
                    boolApproveDocument = AdvanceForeignWorkFlowService.CanApproveWaitApprove(workFlowID);
                }
                else if (document.DocumentType.DocumentTypeID == 3 || document.DocumentType.DocumentTypeID == 7) //ExpenseDomesticDocument, ExpenseForeignDocument
                {
                    boolVerify = ExpenseWorkFlowService.CanVerifyWaitVerify(workFlowID);
                    boolVerifyAndApproveVerify = ExpenseWorkFlowService.CanVerifyAndApproveVerifyWaitVerify(workFlowID);
                    boolApprove = ExpenseWorkFlowService.CanApproveWaitApproveVerify(workFlowID);
                    boolApproveDocument = ExpenseWorkFlowService.CanApproveWaitApprove(workFlowID);

                    if (workflow != null)
                    {
                        showWarningChangeAmount = workflow.CurrentState.Ordinal >= 6 && boolApprove;
                    }
                }
            }

            isSeeHistory = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.isSeeHistoryReject(this.DocumentID);
            ctlSeeHistory.Visible = isSeeHistory;

            #region Warning message Requester and Approver is the same person
            if ((boolVerify || boolVerifyAndApproveVerify || boolApprove) && ((UserAccount.IsApproveVerifyDocument || UserAccount.IsVerifyDocument) || (UserAccount.IsApproveVerifyPayment || UserAccount.IsVerifyPayment)))
            {
                isSeeMessage = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.isSeeMessage(this.DocumentID);
                if (isSeeMessage)
                {
                    ctlSeeMessage.Visible = isSeeMessage;
                    messageCode = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.isMessage(this.DocumentID);
                    if (UserAccount.CurrentLanguageID == 1)
                    {
                        if (messageCode == 1)
                            message = "และผู้เบิกค่าใช้จ่าย";
                        else if (messageCode == 2)
                            message = "และผู้รับเงิน";
                        else if (messageCode == 3)
                            message = ", ผู้เบิกค่าใช้จ่าย และผู้รับเงิน";
                    }
                    else if (UserAccount.CurrentLanguageID == 2)
                    {
                        if (messageCode == 1)
                            message = "and Requester";
                        else if (messageCode == 2)
                            message = "and Receiver";
                        else if (messageCode == 3)
                            message = ", Requester and Receiver";
                    }
                    this.ctlSeeMessage.Text = string.Format(this.GetProgramMessage("$SeeMessage$"), message);
                }
            }
            else
            {
                ctlSeeMessage.Visible = false;
            }
            #endregion
            ctlWarning.Visible = false;
            SCG.eAccounting.DTO.SCGDocument doc = ScgeAccountingQueryProvider.SCGDocumentQuery.FindByIdentity(DocumentID);
            if ((boolVerify || boolVerifyAndApproveVerify || boolApprove || boolApproveDocument) && ((UserAccount.UserID == doc.ApproverID.Userid || UserAccount.IsApproveVerifyDocument || UserAccount.IsVerifyDocument) || (UserAccount.IsApproveVerifyPayment || UserAccount.IsVerifyPayment)))
            {
                if (doc.RequesterID.Userid != doc.ReceiverID.Userid)
                {
                    ctlWarning.Visible = true;
                    ctlWarning.Text = this.GetMessage("RequesterAndReceiverShouldBeTheSamePerson");
                }
            }

            ctlWarningChangeAmount.Visible = false;
            if (ParameterServices.EnableShowWarningMsgAmountHasBeenCorrected)
            {
                if (showWarningChangeAmount && UserAccount.IsApproveVerifyDocument)
                {
                    FnExpenseDocument expenseDocument = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(doc.DocumentID);
                    if (expenseDocument.AmountApproved != null && expenseDocument.TotalExpense != expenseDocument.AmountApproved)
                    {
                        ctlWarningChangeAmount.Visible = true;
                        ctlWarningChangeAmount.Text = this.GetMessage("AmountHasBeenCorrected");
                    }
                }
            }
        }
    }
}
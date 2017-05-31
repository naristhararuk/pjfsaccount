using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using SS.Standard.UI;
using SCG.eAccounting.Query;
using SCG.eAccounting.DTO;
using System.Collections.Generic;
using SCG.eAccounting.SAP.BAPI.Service.Const;
using SCG.eAccounting.SAP.BAPI.Service;
using SCG.eAccounting.SAP;
using SCG.eAccounting.BLL.Implement;
using SCG.eAccounting.BLL;
using SCG.eAccounting.SAP.BAPI.Service.Posting;
using SCG.eAccounting.BLL.WorkFlowService;
using SS.Standard.WorkFlow.Service;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.Query;
using SS.Standard.Security;
using SS.Standard.WorkFlow.DTO.ValueObject;
using SS.Standard.Utilities;

namespace SCG.eAccounting.SAP.BAPI.Service
{
    public class ApproveService
    {
        #region <-- Desing Variable -->
        public ISCGDocumentService SCGDocumentService { get; set; }
        public IFnExpenseDocumentService FnExpenseDocumentService { get; set; }
        public IWorkFlowService WorkFlowService { get; set; }
        public IAvAdvanceDocumentService AvAdvanceDocumentService { get; set; }

        public IRemittanceWorkFlowService RemittanceWorkFlowService { get; set; }
        public IAdvanceWorkFlowService AdvanceWorkFlowService { get; set; }
        public IAdvanceWorkFlowService AdvanceForeignWorkFlowService { get; set; }
        public IExpenseWorkFlowService ExpenseWorkFlowService { get; set; }

        public IUserAccount UserAccount { get; set; }
        #endregion <-- Desing Variable -->

        #region private bool CanApprove()
        private bool CanApprove(long DocID, string DocKind)
        {
            SS.Standard.WorkFlow.DTO.WorkFlow workflow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(DocID);
            bool boolReturn = false;

            if (workflow == null)
            {
                //this.Alert("Don't Found WorkflowID of this DocumentID !!!");
            }
            else if (DocKind == DocumentKind.Advance.ToString())
            {
                AvAdvanceDocument avDoc = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(DocID);
                if (workflow.CurrentState.Name.ToString() != SCG.eAccounting.SAP.BAPI.Service.Const.WorkFlowStatEventNameConst.WaitApproveVerify)
                    boolReturn = false;
                else if (avDoc.AdvanceType == "DM")
                    boolReturn = AdvanceWorkFlowService.CanApproveWaitApproveVerify(workflow.WorkFlowID);
                else
                    boolReturn = AdvanceForeignWorkFlowService.CanApproveWaitApproveVerify(workflow.WorkFlowID);
            }
            else if (DocKind == DocumentKind.Remittance.ToString())
            {
                if (workflow.CurrentState.Name.ToString() != SCG.eAccounting.SAP.BAPI.Service.Const.WorkFlowStatEventNameConst.WaitApproveRemittance)
                    boolReturn = false;
                else
                    boolReturn = RemittanceWorkFlowService.CanApproveWaitApproveRemittance(workflow.WorkFlowID);
            }
            else if (DocKind == DocumentKind.Expense.ToString())
            {
                if (workflow.CurrentState.Name.ToString() != SCG.eAccounting.SAP.BAPI.Service.Const.WorkFlowStatEventNameConst.WaitApproveVerify)
                    boolReturn = false;
                else
                    boolReturn = ExpenseWorkFlowService.CanApproveWaitApproveVerify(workflow.WorkFlowID);
            }
            else if (DocKind == DocumentKind.ExpenseRemittance.ToString())
            {
                if (workflow.CurrentState.Name.ToString() != SCG.eAccounting.SAP.BAPI.Service.Const.WorkFlowStatEventNameConst.WaitRemittance)
                    boolReturn = false;
                else
                    boolReturn = ExpenseWorkFlowService.CanPayWaitRemittance(workflow.WorkFlowID);
            }
            return boolReturn;
        }
        #endregion private bool CanApprove()

        #region private SCG.eAccounting.SAP.BAPI.Service.Posting.PostingService GetPostingService()
        private SCG.eAccounting.SAP.BAPI.Service.Posting.PostingService GetPostingService(string DocKind)
        {
            SCG.eAccounting.SAP.BAPI.Service.Posting.PostingService PostService;
            if (DocKind == DocumentKind.Advance.ToString())
                PostService = new AdvancePostingService();
            else if (DocKind == DocumentKind.Remittance.ToString())
                PostService = new RemittancePostingService();
            else if (DocKind == DocumentKind.Expense.ToString())
                PostService = new ExpensePostingService();
            else if (DocKind == DocumentKind.ExpenseRemittance.ToString())
                PostService = new ExpenseRemittancePostingService();
            else
                PostService = new AdvancePostingService();

            return PostService;
        }
        #endregion private SCG.eAccounting.SAP.BAPI.Service.Posting.PostingService GetPostingService()

        #region public void WorkFlowApprove()
        private void WorkFlowApprove(string DocKind)
        {
            string wfid = System.Web.HttpContext.Current.Request.QueryString["wfid"].ToString();

            if (DocKind == DocumentKind.Advance.ToString())
            {
                IList<WorkFlowStateEvent> workFlowState = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindWorkFlowStateEvent(WorkFlowEventNameConst.Approve, WorkFlowStatEventNameConst.WaitApproveVerify, WorkFlowTypeName.Advance);
                object eventData = new SubmitResponse(workFlowState[0].WorkFlowStateEventID);
                WorkFlowService.NotifyEvent(int.Parse(wfid), WorkFlowEventNameConst.Approve, eventData);
            }
            else if (DocKind == DocumentKind.Remittance.ToString())
            {
                IList<WorkFlowStateEvent> workFlowState = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindWorkFlowStateEvent(WorkFlowEventNameConst.Approve, WorkFlowStatEventNameConst.WaitApproveRemittance, WorkFlowTypeName.Remittance);
                object eventData = new SubmitResponse(workFlowState[0].WorkFlowStateEventID);
                WorkFlowService.NotifyEvent(int.Parse(wfid), WorkFlowEventNameConst.Approve, eventData);
            }
            else if (DocKind == DocumentKind.Expense.ToString())
            {
                IList<WorkFlowStateEvent> workFlowState = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindWorkFlowStateEvent(WorkFlowEventNameConst.Approve, WorkFlowStatEventNameConst.WaitApproveVerify, WorkFlowTypeName.Expense);
                object eventData = new SubmitResponse(workFlowState[0].WorkFlowStateEventID);
                WorkFlowService.NotifyEvent(int.Parse(wfid), WorkFlowEventNameConst.Approve, eventData);
            }
            else if (DocKind == DocumentKind.ExpenseRemittance.ToString())
            {
                IList<WorkFlowStateEvent> workFlowState = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindWorkFlowStateEvent(WorkFlowEventNameConst.Pay, WorkFlowStatEventNameConst.WaitRemittance, WorkFlowTypeName.Expense);
                object eventData = new SubmitResponse(workFlowState[0].WorkFlowStateEventID);
                WorkFlowService.NotifyEvent(int.Parse(wfid), WorkFlowEventNameConst.Pay, eventData);
            }
        }
        #endregion public void WorkFlowApprove()


        #region public void ApprovePosting()
        public bool ApprovePosting(long DocID, DocumentKind DocKind)
        {
            try
            {
                bool isSuccess = true;
                bool ChangeState = false;
                SCGDocument doc = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(DocID);

                if (doc == null)
                {}
                else if (!CanApprove(DocID,DocKind.ToString()))
                {}
                else if (this.GetPostingService(DocKind.ToString()).GetDocumentStatus(DocID, DocKind.ToString()) == "A")
                {}
                else
                {
                    #region Approve
                    IList<BAPIApproveReturn> bapiReturn = this.GetPostingService(DocKind.ToString()).BAPIApprove(DocID, DocKind.ToString(), this.UserAccount.UserID);

                    if (DocKind.ToString() == DocumentKind.ExpenseRemittance.ToString())
                    {
                        FnExpenseDocument docExpense = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(DocID);
                        if (GetPostingService(DocKind.ToString()).GetDocumentStatus(DocID, DocKind.ToString()) == "A")
                        {
                            docExpense.RemittancePostingStatus = "C";
                            FnExpenseDocumentService.SaveOrUpdate(docExpense);
                        }
                        else if (GetPostingService(DocKind.ToString()).GetDocumentStatus(DocID, DocKind.ToString()) == "PP")
                        {
                            docExpense.RemittancePostingStatus = "PP";
                            FnExpenseDocumentService.SaveOrUpdate(docExpense);
                        }
                    }
                    else
                    {
                        if (this.GetPostingService(DocKind.ToString()).GetDocumentStatus(DocID, DocKind.ToString()) == "A")
                        {
                            doc.PostingStatus = "C";
                            SCGDocumentService.SaveOrUpdate(doc);
                        }
                        else if (this.GetPostingService(DocKind.ToString()).GetDocumentStatus(DocID, DocKind.ToString()) == "PP")
                        {
                            doc.PostingStatus = "PP";
                            SCGDocumentService.SaveOrUpdate(doc);
                        }
                    }
                    #endregion Approve

                    #region Call WorkFlow
                    for (int i = 0; i < bapiReturn.Count; i++)
                    {
                        if (bapiReturn[i].ApproveStatus != "S")
                        {
                            isSuccess = false;
                            break;
                        }
                    }
                    if (isSuccess)
                    {
                        ChangeState = true;
                        WorkFlowApprove(DocKind.ToString());
                    }
                    #endregion Call WorkFlow

                    if (ChangeState)
                    {
                        string wfid = System.Web.HttpContext.Current.Request.QueryString["wfid"].ToString();
                        System.Web.HttpContext.Current.Response.Redirect("SubmitResult.aspx?wfid=" + wfid);
                    }
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion public void ApprovePosting()

    }
}

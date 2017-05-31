using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using Spring.Context.Support;

using SS.Standard.Data.NHibernate.Service;
using SS.Standard.WorkFlow.DAL;
using SS.Standard.WorkFlow.Query;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.Service;
using SS.Standard.WorkFlow.EventUserControl;
using SS.Standard.WorkFlow.DTO.ValueObject;
using SS.Standard.Security;
using SS.Standard.Utilities;

using SCG.eAccounting.DTO;
using SCG.eAccounting.BLL.Implement;
using SCG.eAccounting.BLL.WorkFlowService;
using SCG.eAccounting.Query;

using SS.SU.DTO;
using SS.SU.Query;
using SCG.DB.Query;
using SS.DB.Query;
using SCG.eAccounting.DAL;
using SCG.eAccounting.SAP.BAPI.Service;
using SCG.eAccounting.SAP.BAPI.Service.Posting;
using SCG.eAccounting.SAP.BAPI.Service.Const;
using SCG.DB.BLL;

namespace SCG.eAccounting.BLL.Implement.WorkFlowService
{
    public class ExpenseWorkFlowService : GeneralWorkFlowService, IExpenseWorkFlowService
    {
        public IWorkFlowService WorkFlowService { get; set; }
        public IFnExpenseDocumentService FnExpenseDocumentService { get; set; }
        public ParameterServices ParameterServices { get; set; }
        public IAvAdvanceDocumentService AvAdvanceDocumentService { get; set; }
        public IFixedAdvanceDocumentService FixedAdvanceDocumentService { get; set; }
        public IDbBuyingLetterService DbBuyingLetterService { get; set; }
        public IFnExpenseMileageItemService FnExpenseMileageItemService { get; set; }

        #region CanEdit{StateName}

        public virtual bool CanEditDraft(long workFlowID)
        {
            long userID = GetPermitUserDraft(workFlowID);
            return userID == UserAccount.UserID;
        }

        public bool CanEditWaitVerify(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            if (document.PostingStatus != null && document.PostingStatus != PostingStaus.New)
                return false;

            IList<SuRole> permissionRoles = GetPermitRoleVerify(workFlowID);
            return MatchCurrentUserRole(permissionRoles);


        }

        public bool CanEditHold(long workFlowID)
        {
            long userID = GetPermitUserHold(workFlowID);
            return userID == UserAccount.UserID;
        }

        public bool CanEditWaitPayment(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetPermitRoleCounterCashier(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }

        #endregion

        public bool CanPrintPayInWaitRemittance(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);

            long creatorID = scgDocument.CreatorID.Userid;
            long requesterID = scgDocument.RequesterID.Userid;

            return scgDocument.CompanyID.UseSpecialPayIn && (creatorID == UserAccount.UserID || requesterID == UserAccount.UserID);
        }

        protected IList<SuRole> GetCurrentRoles()
        {
            IList<SuRole> currentUserRoles = new List<SuRole>();
            IList<SuUserRole> userRoles = SS.SU.Query.QueryProvider.SuUserRoleQuery.FindUserRoleByUserId(UserAccount.UserID);
            foreach (SuUserRole userRole in userRoles)
            {
                currentUserRoles.Add(userRole.Role);
            }

            return currentUserRoles;
        }

        protected bool MatchCurrentUserRole(IList<SuRole> permissionRoles)
        {
            IList<SuRole> currentUserRoles = GetCurrentRoles();
            foreach (SuRole role in currentUserRoles)
            {
                var matchRole = from p in permissionRoles
                                where p.RoleID == role.RoleID
                                select p;
                if (matchRole.Count<SuRole>() > 0)
                    return true;
            }
            return false;
        }

        #region Can{EventName}{StateName}

        #region CanRejectWaitVerify
        public virtual bool CanRejectWaitVerify(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleRejectWaitVerify(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #region CanVerifyWaitVerify
        public virtual bool CanVerifyWaitVerify(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleVerifyWaitVerify(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #region CanVerifyAndApproveVerifyWaitVerify
        public virtual bool CanVerifyAndApproveVerifyWaitVerify(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleVerifyWaitVerify(workFlowID);
            bool canVerify = MatchCurrentUserRole(permissionRoles);
            if (!canVerify) return false;

            permissionRoles = GetAllowRoleApproveWaitApproveVerify(workFlowID);
            bool canApproveVerify = MatchCurrentUserRole(permissionRoles);
            if (!canApproveVerify) return false;

            return canVerify && canApproveVerify;
        }
        #endregion

        #region CanHoldWaitVerify
        public virtual bool CanHoldWaitVerify(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleHoldWaitVerify(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #region CanReceiveWaitVerify
        public virtual bool CanReceiveWaitVerify(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleReceiveWaitVerify(workFlowID);
            return MatchCurrentUserRole(permissionRoles);

        }
        #endregion

        #region CanReceiveWaitRemittance
        public virtual bool CanReceiveWaitRemittance(long workFlowID)
        {
            if (!IsReceivedDocument(workFlowID))
            {
                IList<SuRole> permissionRoles = GetAllowRoleReceiveWaitVerify(workFlowID);
                return MatchCurrentUserRole(permissionRoles);
            }
            return false;

        }
        #endregion

        #region CanApproveWaitApproveVerify
        public virtual bool CanApproveWaitApproveVerify(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleApproveWaitApproveVerify(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #region CanRejectWaitApproveVerify
        public virtual bool CanRejectWaitApproveVerify(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleRejectWaitApproveVerify(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #region CanVerifyAndApproveVerifyWaitApproveVerify
        public virtual bool CanVerifyAndApproveVerifyWaitApproveVerify(long workFlowID)
        {
            //Not use this state
            return false;
        }
        #endregion

        #region CanHoldWaitApproveVerify
        public virtual bool CanHoldWaitApproveVerify(long workFlowID)
        {
            //Not use this state
            return false;
        }
        #endregion

        #region CanReceiveWaitApproveVerify
        public virtual bool CanReceiveWaitApproveVerify(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleReceiveWaitApproveVerify(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion


        #region CanSendHold
        public virtual bool CanSendHold(long workFlowID)
        {
            long userID = GetAllowUserSendHold(workFlowID);
            return userID == UserAccount.UserID;
        }
        #endregion

        #region CanVerifyHold
        //public virtual bool CanVerifyHold(long workFlowID)
        //{
        //    IList<SuRole> permissionRoles = GetAllowRoleVerifyHold(workFlowID);
        //    return MatchCurrentUserRole(permissionRoles);
        //}
        #endregion

        #region CanRejectHold
        public virtual bool CanRejectHold(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleRejectHold(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #region CanReceiveHold
        public virtual bool CanReceiveHold(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleReceiveHold(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #region CanUnHoldHold
        public virtual bool CanUnHoldHold(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleVerifyHold(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #region CanPayWaitPayment
        public virtual bool CanPayWaitPayment(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRolePayWaitPayment(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #region CanWithdrawWaitPayment
        public virtual bool CanWithdrawWaitPayment(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleWithdrawWaitPayment(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #region CanReceiveWaitPayment
        public virtual bool CanReceiveWaitPayment(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleReceiveWaitPayment(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #region CanReceiveComplete
        public virtual bool CanReceiveComplete(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleReceiveComplete(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #region CanPayWaitPaymentFromSAP
        public virtual bool CanPayWaitPaymentFromSAP(long workFlowID)
        {
            return false;
        }
        #endregion

        #region CanReceiveWaitPaymentFromSAP
        public virtual bool CanReceiveWaitPaymentFromSAP(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleReceiveWaitPaymentFromSAP(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #region CanWithdrawWaitPaymentFromSAP
        public virtual bool CanWithdrawWaitPaymentFromSAP(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleWithdrawWaitPaymentFromSAP(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #region CanPayWaitRemittance
        public virtual bool CanPayWaitRemittance(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRolePayWaitRemittance(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #region CanWithdrawWaitRemittance
        public virtual bool CanWithdrawWaitRemittance(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleWithdrawWaitRemittance(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #region CanApproveWaitApproveRejection
        public virtual bool CanApproveWaitApproveRejection(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleApproveWaitApproveRejection(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #region CanRejectWaitApproveRejection
        public virtual bool CanRejectWaitApproveRejection(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleRejectWaitApproveRejection(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #region CanReceiveWaitApproveRejection
        public virtual bool CanReceiveWaitApproveRejection(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleReceiveWaitApproveRejection(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion


        #region CanApproveWaitReverse
        public virtual bool CanApproveWaitReverse(long workFlowID)
        {
            return true;
        }
        #endregion

        #region CanRejectWaitReverse
        public virtual bool CanRejectWaitReverse(long workFlowID)
        {
            return true;
        }
        #endregion

        #region CanReceiveWaitDocument
        public virtual bool CanReceiveWaitDocument(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleReceiveWaitDocument(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #region CanRejectWaitDocument
        public virtual bool CanRejectWaitDocument(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleRejectWaitDocument(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #endregion

        #region On{EventName}{StateName}

        #region OnCancelDraft
        public string OnCancelDraft(long workFlowID, object eventData)
        {
            string signal = base.OnCancelDraft(workFlowID, eventData);

            try
            {
                WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

                // Add code for clear expense amount on advance when cancel Expense by Anuwat S on 22/06/2009
                FnExpenseDocument expense = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(workFlow.Document.DocumentID);
                if (expense != null)
                {
                    bool isRepOffice = expense.IsRepOffice.HasValue ? expense.IsRepOffice.Value : false;

                    AvAdvanceDocumentService.UpdateClearingAdvance(expense.ExpenseID, 0);
                    if (isRepOffice)
                    {
                        AvAdvanceDocumentService.UpdateClearingAdvanceForRepOffice(expense.ExpenseID, 0);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return signal;
        }
        #endregion


        #region OnSendDraft
        public override string OnSendDraft(long workFlowID, object eventData)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            if (workFlow.Document.DocumentType.DocumentTypeName == "ExpenseDomesticDocument")
            {
                FnExpenseDocument ExpId = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(scgDocument.DocumentID);
                FnExpenseMileageItemService.ValdationMileageRateByDataBase(ExpId.ExpenseID);
            }
            /*TODO validate , save event data to workflow*/
            WorkFlowResponse response = new WorkFlowResponse();
            SubmitResponse submitResponse;
            try
            {
                submitResponse = eventData as SubmitResponse;
                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(submitResponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);


                if (string.IsNullOrEmpty(scgDocument.DocumentNo))
                {
                    //TODO : Generate Document No and Save it
                    int year = DateTime.Now.Year;

                    scgDocument.DocumentNo = DbDocumentRunningService.RetrieveNextDocumentRunningNo(year, scgDocument.DocumentType.DocumentTypeID, scgDocument.CompanyID.CompanyID);
                    scgDocument.DocumentDate = DateTime.Now;
                    ScgeAccountingDaoProvider.SCGDocumentDao.Update(scgDocument);
                }


            }
            catch (Exception)
            {
                throw;
            }

            /*  TODO generate signal string
             *  Case 1 : Creator != Requester != Receiver and Total Advance Amount > some vallue =>SendDraftToWaitAR
             *  Case 2 : Advance Document has initiator => SendDraftToWaitInitator
             *  Case 3 : Advance Document has no initiator => SendDraftToWaitApprove
             */
            string signal = "";
            if (scgDocument.CreatorID.Userid != scgDocument.RequesterID.Userid &&
                //scgDocument.CreatorID.Userid != scgDocument.ReceiverID.Userid &&
                GetTotalDocumentAmount(scgDocument.DocumentID) >= ParameterServices.RequiredRequesterApprovalAmount)
            {
                signal = "SendDraftToWaitAR";
            }
            else
            {
                IList<DocumentInitiator> initiators = ScgeAccountingQueryProvider.DocumentInitiatorQuery.GetNotResponseInitiatorByDocumentID(workFlow.Document.DocumentID);
                if (initiators.Count > 0)
                {
                    signal = "SendDraftToWaitInitator";
                }
                else
                {
                    signal = "SendDraftToWaitApprove";
                }
                if (ParameterServices.EnableEmail02Requester)
                {
                    if (scgDocument.CreatorID.Userid != scgDocument.RequesterID.Userid)
                    {
                        SCGEmailService.SendEmailEM02(response.WorkFlowResponseID, scgDocument.RequesterID.Userid, null);
                    }
                }
            }
            return signal;
        }
        #endregion

        #region override OnApproveWaitApprove
        public override string OnApproveWaitApprove(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            FnExpenseDocument expenseDocument = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(scgDocument.DocumentID);
            try
            {
                WorkFlowResponse response = new WorkFlowResponse();
                int workFlowStateEventId;
                response.WorkFlow = workFlow;

                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;

                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;


                if (eventData is RejectDetailResponse)
                {

                    RejectDetailResponse rejectDetailResponse = eventData as RejectDetailResponse;
                    workFlowStateEventId = rejectDetailResponse.WorkFlowStateEventID;
                    WorkFlowRejectResponse rejectResponse = new WorkFlowRejectResponse();
                    rejectResponse.WorkFlowResponse = response;
                    if (rejectDetailResponse.ReasonID != 0)
                        rejectResponse.Reason = ScgDbQueryProvider.DbRejectReasonQuery.FindProxyByIdentity(rejectDetailResponse.ReasonID);
                    rejectResponse.Remark = rejectDetailResponse.Remark;
                    rejectResponse.Active = true;
                    rejectResponse.CreBy = UserAccount.UserID;
                    rejectResponse.CreDate = DateTime.Now;
                    rejectResponse.UpdBy = UserAccount.UserID;
                    rejectResponse.UpdDate = DateTime.Now;
                    rejectResponse.UpdPgm = UserAccount.CurrentProgramCode;

                    //if (rejectDetailResponse.ResponseMethod != null)
                    response.ResponseMethod = rejectDetailResponse.ResponseMethod.GetHashCode().ToString();
                    response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(workFlowStateEventId);
                    WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);

                    if (rejectDetailResponse.ReasonID != 0 || !string.IsNullOrEmpty(rejectDetailResponse.Remark))
                        WorkFlowDaoProvider.WorkFlowRejectResponseDao.Save(rejectResponse);
                }
                else
                {
                    SubmitResponse submitResponse = eventData as SubmitResponse;
                    workFlowStateEventId = submitResponse.WorkFlowStateEventID;
                    //if (submitResponse.ResponseMethod != null)
                    response.ResponseMethod = submitResponse.ResponseMethod.GetHashCode().ToString();
                    response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(workFlowStateEventId);
                    WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);

                    if (expenseDocument != null)
                    {
                        expenseDocument.AmountApproved = expenseDocument.TotalExpense;
                        expenseDocument.UpdBy = UserAccount.UserID;
                        expenseDocument.UpdDate = DateTime.Now;
                        expenseDocument.UpdPgm = UserAccount.CurrentProgramCode;
                        FnExpenseDocumentService.Update(expenseDocument);
                    }
                }

                scgDocument.ApproveDate = DateTime.Now;
                scgDocument.UpdBy = UserAccount.UserID;
                scgDocument.UpdDate = DateTime.Now;
                scgDocument.UpdPgm = UserAccount.CurrentProgramCode;
                SCGDocumentService.SaveOrUpdate(scgDocument);
                /* // comment old code ที่ยังไม่ต้อง save reason approve
                SubmitResponse submitResponse = eventData as SubmitResponse;

                WorkFlowResponse response = new WorkFlowResponse();
                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(submitResponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                if (submitResponse.ResponseMethod != null)
                    response.ResponseMethod = submitResponse.ResponseMethod.GetHashCode().ToString();
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);
                */

                if (ParameterServices.EnableEmail02Creator || ParameterServices.EnableEmail02Requester || ParameterServices.EnableEmail02Initiator)
                {
                    long sendToUserID = 0;
                    IList<long> ccList = new List<long>();

                    if (ParameterServices.EnableEmail02Creator)
                        sendToUserID = scgDocument.CreatorID.Userid;

                    if (ParameterServices.EnableEmail02Requester)
                    {
                        if (sendToUserID == 0)
                            sendToUserID = scgDocument.RequesterID.Userid;
                        else if (scgDocument.CreatorID.Userid != scgDocument.RequesterID.Userid)
                            ccList.Add(scgDocument.RequesterID.Userid);
                    }

                    if (ParameterServices.EnableEmail02Initiator)
                    {
                        IList<DocumentInitiator> initiators = ScgeAccountingQueryProvider.DocumentInitiatorQuery.GetDocumentInitiatorByDocumentID(workFlow.Document.DocumentID);
                        foreach (DocumentInitiator initiator in initiators)
                        {
                            if (sendToUserID == 0)
                                sendToUserID = initiator.UserID.Userid;
                            else if (initiator.UserID.Userid != sendToUserID && !ccList.Contains(initiator.UserID.Userid))
                                ccList.Add(initiator.UserID.Userid);
                        }
                    }

                    SCGEmailService.SendEmailEM02(response.WorkFlowResponseID, sendToUserID, ccList);
                }
            }
            catch (Exception)
            {
                throw;
            }
            /*  TODO generate signal string
             *  Case 1 (Expense Only): ApproveWaitApproveToWaitDocument
             *  Case 2 : ApproveWaitApproveToWaitVerify
             */

            string signal = "ApproveWaitApproveToWaitVerify";

            IList<DocumentAttachment> attachments = Query.ScgeAccountingQueryProvider.DocumentAttachmentQuery.GetDocumentAttachmentByDocumentID(workFlow.Document.DocumentID);
            if (attachments.Count == 0)
            {
                signal = "ApproveWaitApproveToWaitDocument";
            }

            if (scgDocument.CompanyID.IsVerifyHardCopyOnly != true)
            {
                if (attachments.Count == 0)
                {
                    scgDocument.IsVerifyWithImage = false;
                }
                else
                {
                    scgDocument.IsVerifyWithImage = true;
                }
            }
            else
            {
                scgDocument.IsVerifyWithImage = false;
            }


            return signal;
        }
        #endregion

        #region OnVerifyWaitVerify
        public virtual string OnVerifyWaitVerify(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

            //Validate Posting Status
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            FnExpenseDocument expDoc = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(workFlow.Document.DocumentID);

            if (document.PostingStatus != PostingStaus.Posted && document.PostingStatus != PostingStaus.Complete)
            {
                if (expDoc == null || (expDoc != null && !expDoc.TotalExpense.Equals(0)))
                {
                    errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("Workflow_OnVerifyWaitVerify_Mismatch_PostingStatus"));
                }
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            try
            {
                SubmitResponse submitReponse = eventData as SubmitResponse;

                WorkFlowResponse response = new WorkFlowResponse();
                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(submitReponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                //update VerifiedDate
                document.VerifiedDate = DateTime.Now;
                SCGDocumentService.SaveOrUpdate(document);
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);

                if (expDoc != null && expDoc.AmountBeforeVerify != expDoc.TotalExpense)
                {
                    WorkFlowVerifyResponse verifyResponse = new WorkFlowVerifyResponse();
                    verifyResponse.WorkFlowResponse = response;
                    verifyResponse.AmountBeforeVerify = expDoc.AmountBeforeVerify ?? 0;
                    verifyResponse.AmountVerified = expDoc.TotalExpense;
                    verifyResponse.Active = true;
                    verifyResponse.CreBy = UserAccount.UserID;
                    verifyResponse.CreDate = DateTime.Now;
                    verifyResponse.UpdBy = UserAccount.UserID;
                    verifyResponse.UpdDate = DateTime.Now;
                    verifyResponse.UpdPgm = UserAccount.CurrentProgramCode;
                    WorkFlowDaoProvider.WorkFlowVerifyResponseDao.Save(verifyResponse);
                    SCGDocumentService.SaveOrUpdate(document);

                }

            }
            catch (Exception)
            {
                throw;
            }
            /*TODO generate signal string*/
            string signal = "VerifyWaitVerifyToWaitApproveVerify";

            return signal;
        }
        #endregion

        #region OnRejectWaitVerify
        public string OnRejectWaitVerify(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

            //Validate Posting Status
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            if (scgDocument.PostingStatus != null && scgDocument.PostingStatus != PostingStaus.New)
            {
                errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("Workflow_OnRejectWaitVerify_Mismatch_PostingStatus"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            RejectDetailResponse rejectDetailResponse = eventData as RejectDetailResponse;
            WorkFlowResponse response = new WorkFlowResponse();

            try
            {
                ValidateRejectResponse(rejectDetailResponse);

                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(rejectDetailResponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);

                WorkFlowRejectResponse rejectResponse = new WorkFlowRejectResponse();
                rejectResponse.WorkFlowResponse = response;
                rejectResponse.Reason = ScgDbQueryProvider.DbRejectReasonQuery.FindProxyByIdentity(rejectDetailResponse.ReasonID);
                rejectResponse.NeedApproveRejection = rejectDetailResponse.NeedApproveRejection;
                rejectResponse.Remark = rejectDetailResponse.Remark;
                rejectResponse.Active = true;
                rejectResponse.CreBy = UserAccount.UserID;
                rejectResponse.CreDate = DateTime.Now;
                rejectResponse.UpdBy = UserAccount.UserID;
                rejectResponse.UpdDate = DateTime.Now;
                rejectResponse.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowRejectResponseDao.Save(rejectResponse);
                DbBuyingLetterService.DeleteLetter(workFlow.Document.DocumentID);

                scgDocument.ApproveDate = null;
                scgDocument.UpdBy = UserAccount.UserID;
                scgDocument.UpdDate = DateTime.Now;
                scgDocument.UpdPgm = UserAccount.CurrentProgramCode;
                SCGDocumentService.SaveOrUpdate(scgDocument);

            }
            catch (Exception)
            {
                throw;
            }
            /*  TODO generate signal string
             *  Case 1 : RejectWaitVerifyToWaitApproveRejection
             *  Case 2 : RejectWaitVerifyToDraft
             */
            string signal = string.Empty;
            if (rejectDetailResponse.NeedApproveRejection)
            {
                signal = "RejectWaitVerifyToWaitApproveRejection";
            }
            else
            {
                signal = "RejectWaitVerifyToDraft";

                try
                {
                    FnExpenseDocument expense = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(scgDocument.DocumentID);

                    scgDocument.ReceiveDocumentDate = null;
                    SCGDocumentService.SaveOrUpdate(scgDocument);

                    if (expense != null)
                    {
                        expense.BoxID = string.Empty;
                        ScgeAccountingDaoProvider.FnExpenseDocumentDao.SaveOrUpdate(expense);
                    }
                    SCGEmailService.SendEmailEM04(response.WorkFlowResponseID, scgDocument.CreatorID.Userid);

                    if (ParameterServices.EnableEmail02Requester)
                    {
                        if (scgDocument.CreatorID.Userid != scgDocument.RequesterID.Userid)
                            SCGEmailService.SendEmailEM02(response.WorkFlowResponseID, scgDocument.RequesterID.Userid, null);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return signal;
        }
        #endregion

        #region OnVerifyAndApproveVerifyWaitVerify
        public virtual string OnVerifyAndApproveVerifyWaitVerify(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/

            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

            //Validate Posting Status
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            FnExpenseDocument expDoc = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(workFlow.Document.DocumentID);
            if (scgDocument.PostingStatus != PostingStaus.Complete)
            {
                if (expDoc == null || (expDoc != null && !expDoc.TotalExpense.Equals(0)))
                {
                    errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("Workflow_OnVerifyAndApproveVerifyWaitVerify_Mismatch_PostingStatus"));
                }
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            SubmitResponse submitReponse = eventData as SubmitResponse;
            try
            {
                WorkFlowResponse response = new WorkFlowResponse();
                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(submitReponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);


                //update VerifiedDate & ApproveVerifiedDate
                scgDocument.VerifiedDate = DateTime.Now;
                scgDocument.ApproveVerifiedDate = DateTime.Now;

                SCGDocumentService.SaveOrUpdate(scgDocument);
            }
            catch (Exception)
            {
                throw;
            }
            /*  TODO generate signal string
             *  Case 1 : VerifyAndApproveVerifyWaitVerifyToWaitPayment
             *  Case 2 : VerifyAndApproveVerifyWaitVerifyToWaitPaymentFromSAP
             *  Case 3 : VerifyAndApproveVerifyWaitVerifyToComplete
             *  Case 4 : VerifyAndApproveVerifyWaitVerifyToWaitRemittance
             */
            string signal = "";

            FnExpenseDocument expenseDocument = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(workFlow.Document.DocumentID);
            if (expenseDocument.DifferenceAmount > 0)
            {
                //Pay more in case expense that more than advance
                if (expenseDocument.PaymentType == PaymentType.CA)
                {
                    signal = "VerifyAndApproveVerifyWaitVerifyToWaitPayment";

                    IList<long> ccList = new List<long>();
                    if (scgDocument.CreatorID.Userid != scgDocument.ReceiverID.Userid)
                        ccList.Add(scgDocument.CreatorID.Userid);
                    if (scgDocument.RequesterID.Userid != scgDocument.ReceiverID.Userid)
                        ccList.Add(scgDocument.RequesterID.Userid);

                    SCGEmailService.SendEmailEM05(workFlowID, scgDocument.ReceiverID.Userid, ccList, false);
                    //#2
                    //if (ParameterServices.EnableSMS)
                    // {
                    IList<long> ReciverList = new List<long>();
                    ReciverList.Add(scgDocument.ReceiverID.Userid);

                    SCGSMSService.SendSMS02(workFlowID, scgDocument.DocumentNo.ToString(), ReciverList, false);
                    // }
                }
                else if (expenseDocument.PaymentType == PaymentType.CQ || expenseDocument.PaymentType == PaymentType.TR)
                {
                    signal = "VerifyAndApproveVerifyWaitVerifyToWaitPaymentFromSAP";
                }

            }
            else if (expenseDocument.DifferenceAmount == 0)
            {
                signal = "VerifyAndApproveVerifyWaitVerifyToComplete";

                IList<FnExpenseAdvance> expenseAdvances = ScgeAccountingQueryProvider.FnExpenseAdvanceQuery.FindByExpenseDocumentID(expenseDocument.ExpenseID);
                foreach (FnExpenseAdvance expenseAdvance in expenseAdvances)
                {
                    WorkFlow advanceWorkFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(expenseAdvance.Advance.DocumentID.DocumentID);

                    // Notify Clearing all advance
                    string clearingEventName = "Clearing";
                    WorkFlowStateEvent stateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetByWorkFlowStateID_EventName(advanceWorkFlow.CurrentState.WorkFlowStateID, clearingEventName);
                    WorkFlowService.NotifyEventFromInternal(advanceWorkFlow.WorkFlowID, clearingEventName, new SubmitResponse(stateEvent.WorkFlowStateEventID));
                }
            }
            else if (expenseDocument.DifferenceAmount < 0)
            {
                signal = "VerifyAndApproveVerifyWaitVerifyToWaitRemittance";

                SCGEmailService.SendEmailEM11(expenseDocument.ExpenseID);
            }
            return signal;
        }
        #endregion

        #region OnHoldWaitVerify
        public string OnHoldWaitVerify(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

            //Validate Posting Status
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            if (scgDocument.PostingStatus != null && scgDocument.PostingStatus != PostingStaus.New)
            {
                errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("Workflow_OnHoldWaitVerify_Mismatch_PostingStatus"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            HoldDetailResponse holdDetailResponse = eventData as HoldDetailResponse;

            try
            {
                //ValidateHoldResponse(holdDetailResponse);

                WorkFlowResponse response = new WorkFlowResponse();
                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(holdDetailResponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);

                WorkFlowHoldResponse holdResponse = new WorkFlowHoldResponse();
                holdResponse.WorkFlowResponse = response;
                holdResponse.Remark = holdDetailResponse.Remark;
                holdResponse.IsUnHold = false;
                holdResponse.Active = true;
                holdResponse.CreBy = UserAccount.UserID;
                holdResponse.CreDate = DateTime.Now;
                holdResponse.UpdBy = UserAccount.UserID;
                holdResponse.UpdDate = DateTime.Now;
                holdResponse.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowHoldResponseDao.Save(holdResponse);


                foreach (object holdField in holdDetailResponse.HoldFields)
                {
                    WorkFlowHoldResponseDetail workFlowHoldDetailResponse = new WorkFlowHoldResponseDetail();
                    workFlowHoldDetailResponse.WorkFlowHoldResponse = holdResponse;
                    workFlowHoldDetailResponse.FieldName = holdField.ToString();
                    workFlowHoldDetailResponse.Active = true;
                    workFlowHoldDetailResponse.CreBy = UserAccount.UserID;
                    workFlowHoldDetailResponse.CreDate = DateTime.Now;
                    workFlowHoldDetailResponse.UpdBy = UserAccount.UserID;
                    workFlowHoldDetailResponse.UpdDate = DateTime.Now;
                    workFlowHoldDetailResponse.UpdPgm = UserAccount.CurrentProgramCode;
                    WorkFlowDaoProvider.WorkFlowHoldResponseDetailDao.Save(workFlowHoldDetailResponse);
                }

                if (ParameterServices.EnableEmail02Creator || ParameterServices.EnableEmail02Requester)
                {
                    long sendToUserID = 0;
                    IList<long> ccList = new List<long>();

                    if (ParameterServices.EnableEmail02Creator)
                        sendToUserID = scgDocument.CreatorID.Userid;

                    if (ParameterServices.EnableEmail02Requester)
                    {
                        if (sendToUserID == 0)
                            sendToUserID = scgDocument.RequesterID.Userid;
                        else if (scgDocument.CreatorID.Userid != scgDocument.RequesterID.Userid)
                            ccList.Add(scgDocument.RequesterID.Userid);
                    }

                    SCGEmailService.SendEmailEM02(response.WorkFlowResponseID, sendToUserID, ccList);
                }
            }
            catch (Exception)
            {
                throw;
            }
            /*  TODO generate signal string
             *  Case 1 : HoldWaitVerifyToHold
             */
            string signal = "HoldWaitVerifyToHold";

            return signal;
        }
        #endregion

        #region OnReceiveWaitVerify
        public virtual string OnReceiveWaitVerify(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            ReceiveResponse receiveResponse = eventData as ReceiveResponse;
            ValidateReceiveResponse(receiveResponse);

            try
            {

                WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
                WorkFlowResponse response = new WorkFlowResponse();
                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(receiveResponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);

                FnExpenseDocument expenseDocument = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(workFlow.Document.DocumentID);
                SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
                expenseDocument.BoxID = receiveResponse.BoxID;
                scgDocument.ReceiveDocumentDate = DateTime.Now;

                FnExpenseDocumentService.Update(expenseDocument);
            }
            catch (Exception)
            {
                throw;
            }
            /*  TODO generate signal string
             *  Case 1 : HoldWaitVerifyToHold
             */
            string signal = "ReceiveWaitVerifyToWaitVerify";

            return signal;
        }
        #endregion


        #region OnApproveWaitApproveVerify
        public virtual string OnApproveWaitApproveVerify(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

            //Validate Posting Status
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);

            FnExpenseDocument expenseDocument = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(workFlow.Document.DocumentID);

            if (scgDocument.PostingStatus != PostingStaus.Complete)
            {
                if (expenseDocument == null || (expenseDocument != null && !expenseDocument.TotalExpense.Equals(0)))
                {
                    ExpensePostingService postingService = new ExpensePostingService();
                    IList<BAPIApproveReturn> bapiReturn = postingService.BAPIApprove(scgDocument.DocumentID, DocumentKind.Expense.ToString(), UserAccount.UserID);

                    if (postingService.GetDocumentStatus(scgDocument.DocumentID, DocumentKind.Remittance.ToString()) == "A")
                    {
                        scgDocument.PostingStatus = "C";
                        SCGDocumentService.SaveOrUpdate(scgDocument);
                    }
                    else if (postingService.GetDocumentStatus(scgDocument.DocumentID, DocumentKind.Remittance.ToString()) == "PP")
                    {
                        scgDocument.PostingStatus = "PP";
                        SCGDocumentService.SaveOrUpdate(scgDocument);
                    }

                    bool isSuccess = true;
                    for (int i = 0; i < bapiReturn.Count; i++)
                    {
                        if (bapiReturn[i].ApproveStatus != "S")
                        {
                            isSuccess = false;
                            break;
                        }
                    }

                    if (!isSuccess)
                    {
                        for (int i = 0; i < bapiReturn.Count; i++)
                        {
                            if (bapiReturn[i].ApproveStatus != "S")
                            {
                                string approveStatus = "Error";
                                if (bapiReturn[i].ApproveStatus.ToUpper() == "W")
                                {
                                    approveStatus = "Warning";
                                }

                                for (int j = 0; j < bapiReturn[i].ApproveReturn.Count; j++)
                                {
                                    errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage(approveStatus + ": [DocSeq=" + bapiReturn[i].DOCSEQ + "] -> " + bapiReturn[i].ApproveReturn[j].Message));
                                }
                            }
                        }
                    }
                }
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            try
            {
                SubmitResponse submitResponse = eventData as SubmitResponse;

                WorkFlowResponse response = new WorkFlowResponse();
                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(submitResponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);

                //update ApproveVerifiedDate
                scgDocument.ApproveVerifiedDate = DateTime.Now;
                SCGDocumentService.SaveOrUpdate(scgDocument);
            }
            catch (Exception)
            {
                throw;
            }
            /*  TODO generate signal string
             *  Case 1 ApproveWaitApproveVerifyToWaitPayment
             *  Case 2 ApproveWaitApproveVerifyToWaitPaymentFromSAP
             *  Case 3 ApproveWaitApproveVerifyToWaitRemittance
             *  Case 4 ApproveWaitApproveVerifyToComplete
             */
            string signal = "";

            //FnExpenseDocument expenseDocument = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(workFlow.Document.DocumentID);

            if (expenseDocument.DifferenceAmount > 0)
            {
                //Pay more in case expense that more than advance
                if (expenseDocument.PaymentType == PaymentType.CA)
                {
                    signal = "ApproveWaitApproveVerifyToWaitPayment";

                    IList<long> ccList = new List<long>();
                    if (scgDocument.CreatorID.Userid != scgDocument.ReceiverID.Userid)
                        ccList.Add(scgDocument.CreatorID.Userid);
                    if (scgDocument.RequesterID.Userid != scgDocument.ReceiverID.Userid)
                        ccList.Add(scgDocument.RequesterID.Userid);

                    SCGEmailService.SendEmailEM05(workFlowID, scgDocument.ReceiverID.Userid, ccList, false);

                    //#3
                    // if (ParameterServices.EnableSMS)
                    //  {
                    IList<long> ReciverList = new List<long>();
                    ReciverList.Add(scgDocument.ReceiverID.Userid);

                    SCGSMSService.SendSMS02(workFlowID, scgDocument.RequesterID.Userid.ToString(), ReciverList, false);
                    // }
                }
                else if (expenseDocument.PaymentType == PaymentType.CQ || expenseDocument.PaymentType == PaymentType.TR)
                {
                    signal = "ApproveWaitApproveVerifyToWaitPaymentFromSAP";
                }

            }
            else if (expenseDocument.DifferenceAmount == 0)
            {
                signal = "ApproveWaitApproveVerifyToComplete";

                IList<FnExpenseAdvance> expenseAdvances = ScgeAccountingQueryProvider.FnExpenseAdvanceQuery.FindByExpenseDocumentID(expenseDocument.ExpenseID);
                foreach (FnExpenseAdvance expenseAdvance in expenseAdvances)
                {
                    WorkFlow advanceWorkFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(expenseAdvance.Advance.DocumentID.DocumentID);

                    // Notify Clearing all advance
                    string clearingEventName = "Clearing";
                    WorkFlowStateEvent stateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetByWorkFlowStateID_EventName(advanceWorkFlow.CurrentState.WorkFlowStateID, clearingEventName);
                    WorkFlowService.NotifyEventFromInternal(advanceWorkFlow.WorkFlowID, clearingEventName, new SubmitResponse(stateEvent.WorkFlowStateEventID));
                }
            }
            else if (expenseDocument.DifferenceAmount < 0)
            {
                signal = "ApproveWaitApproveVerifyToWaitRemittance";

                SCGEmailService.SendEmailEM11(expenseDocument.ExpenseID);
            }


            return signal;
        }
        #endregion

        #region OnRejectWaitApproveVerify
        public string OnRejectWaitApproveVerify(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/

            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

            //Validate Posting Status
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            if (document.PostingStatus != PostingStaus.Posted)
            {
                errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("Workflow_OnRejectWaitApproveVerify_Mismatch_PostingStatus"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            RejectDetailResponse rejectDetailResponse = eventData as RejectDetailResponse;

            try
            {

                ValidateRejectResponse(rejectDetailResponse);

                WorkFlowResponse response = new WorkFlowResponse();
                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(rejectDetailResponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);

                WorkFlowRejectResponse rejectResponse = new WorkFlowRejectResponse();
                rejectResponse.WorkFlowResponse = response;
                rejectResponse.Reason = ScgDbQueryProvider.DbRejectReasonQuery.FindProxyByIdentity(rejectDetailResponse.ReasonID);
                rejectResponse.Remark = rejectDetailResponse.Remark;
                rejectResponse.Active = true;
                rejectResponse.CreBy = UserAccount.UserID;
                rejectResponse.CreDate = DateTime.Now;
                rejectResponse.UpdBy = UserAccount.UserID;
                rejectResponse.UpdDate = DateTime.Now;
                rejectResponse.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowRejectResponseDao.Save(rejectResponse);

                document.VerifiedDate = null;
                SCGDocumentService.SaveOrUpdate(document);
            }
            catch (Exception)
            {
                throw;
            }
            /*TODO generate signal string*/
            string signal = "RejectWaitApproveVerifyToWaitVerify";

            return signal;
        }
        #endregion

        #region OnVerifyAndApproveVerifyWaitApproveVerify
        public string OnVerifyAndApproveVerifyWaitApproveVerify(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            /*TODO generate signal string*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            //update ApproveVerifiedDate
            scgDocument.ApproveVerifiedDate = DateTime.Now;
            SCGDocumentService.SaveOrUpdate(scgDocument);
            string signal = "VerifyAndApproveVerifyWaitApproveVerifyToWaitPayment";

            return signal;
        }
        #endregion

        #region OnHoldWaitApproveVerify
        public string OnHoldWaitApproveVerify(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            /*TODO generate signal string*/
            string signal = "HoldWaitApproveVerifyToHold";

            return signal;
        }
        #endregion

        #region OnReceiveWaitApproveVerify
        public virtual string OnReceiveWaitApproveVerify(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            ReceiveResponse receiveResponse = eventData as ReceiveResponse;
            ValidateReceiveResponse(receiveResponse);

            try
            {

                WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);


                WorkFlowResponse response = new WorkFlowResponse();
                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(receiveResponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);

                FnExpenseDocument expenseDocument = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(workFlow.Document.DocumentID);
                SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
                expenseDocument.BoxID = receiveResponse.BoxID;
                FnExpenseDocumentService.Update(expenseDocument);
                scgDocument.ReceiveDocumentDate = DateTime.Now;
            }
            catch (Exception)
            {
                throw;
            }
            /*  TODO generate signal string
             *  Case 1 : HoldWaitVerifyToHold
             */
            string signal = "ReceiveWaitApproveVerifyToWaitApproveVerify";

            return signal;
        }
        #endregion

        #region OnReceiveWaitRemittance
        public virtual string OnReceiveWaitRemittance(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            ReceiveResponse receiveResponse = eventData as ReceiveResponse;
            ValidateReceiveResponse(receiveResponse);

            try
            {

                WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
                WorkFlowResponse response = new WorkFlowResponse();
                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(receiveResponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);

                FnExpenseDocument expenseDocument = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(workFlow.Document.DocumentID);
                SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
                expenseDocument.BoxID = receiveResponse.BoxID;
                FnExpenseDocumentService.Update(expenseDocument);
                scgDocument.ReceiveDocumentDate = DateTime.Now;
            }
            catch (Exception)
            {
                throw;
            }

            string signal = "ReceiveWaitRemittanceToWaitRemittance";

            return signal;
        }
        #endregion
        #region OnSendHold
        public string OnSendHold(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            try
            {
                SubmitResponse submitResponse = eventData as SubmitResponse;

                WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);


                WorkFlowResponse response = new WorkFlowResponse();
                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(submitResponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);
            }
            catch (Exception)
            {
                throw;
            }
            /*TODO generate signal string*/
            string signal = "SendHoldToWaitVerify";

            return signal;
        }
        #endregion

        #region OnVerifyHold
        //public string OnVerifyHold(long workFlowID, object eventData)
        //{
        //    /*TODO validate , save event data to workflow*/
        //    try
        //    {
        //        SubmitResponse submitResponse = eventData as SubmitResponse;

        //        WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);


        //        WorkFlowResponse response = new WorkFlowResponse();
        //        response.WorkFlow = workFlow;
        //        response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(submitResponse.WorkFlowStateEventID);
        //        response.ResponseBy = UserAccount.UserID;
        //        response.ResponseDate = DateTime.Now;
        //        response.Active = true;
        //        response.CreBy = UserAccount.UserID;
        //        response.CreDate = DateTime.Now;
        //        response.UpdBy = UserAccount.UserID;
        //        response.UpdDate = DateTime.Now;
        //        response.UpdPgm = UserAccount.CurrentProgramCode;
        //        WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    /*TODO generate signal string*/
        //    string signal = "VerifyHoldToWaitApproveVerify";

        //    return signal;
        //}
        #endregion

        #region OnRejectHold
        public string OnRejectHold(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);

            RejectDetailResponse rejectDetailResponse = eventData as RejectDetailResponse;
            WorkFlowResponse response = new WorkFlowResponse();

            try
            {
                ValidateRejectResponse(rejectDetailResponse);

                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(rejectDetailResponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);

                WorkFlowRejectResponse rejectResponse = new WorkFlowRejectResponse();
                rejectResponse.WorkFlowResponse = response;
                rejectResponse.Reason = ScgDbQueryProvider.DbRejectReasonQuery.FindProxyByIdentity(rejectDetailResponse.ReasonID);
                rejectResponse.NeedApproveRejection = rejectDetailResponse.NeedApproveRejection;
                rejectResponse.Remark = rejectDetailResponse.Remark;
                rejectResponse.Active = true;
                rejectResponse.CreBy = UserAccount.UserID;
                rejectResponse.CreDate = DateTime.Now;
                rejectResponse.UpdBy = UserAccount.UserID;
                rejectResponse.UpdDate = DateTime.Now;
                rejectResponse.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowRejectResponseDao.Save(rejectResponse);
            }
            catch (Exception)
            {
                throw;
            }
            /*TODO generate signal string*/
            string signal = "RejectHoldToDraft";
            if (rejectDetailResponse.NeedApproveRejection)
            {
                signal = "RejectHoldToWaitApproveRejection";
            }
            else
            {
                signal = "RejectHoldToDraft";

                try
                {
                    Document document = WorkFlowQueryProvider.WorkFlowQuery.GetDocumentByWorkFlowID(workFlowID);
                    FnExpenseDocument expense = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(document.DocumentID);

                    if (expense != null)
                    {
                        expense.BoxID = string.Empty;
                        scgDocument.ReceiveDocumentDate = null;
                        ScgeAccountingDaoProvider.FnExpenseDocumentDao.SaveOrUpdate(expense);
                    }
                    SCGEmailService.SendEmailEM04(response.WorkFlowResponseID, scgDocument.CreatorID.Userid);

                    if (ParameterServices.EnableEmail02Requester)
                    {
                        if (scgDocument.CreatorID.Userid != scgDocument.RequesterID.Userid)
                            SCGEmailService.SendEmailEM02(response.WorkFlowResponseID, scgDocument.RequesterID.Userid, null);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return signal;
        }
        #endregion

        #region OnReceiveHold
        public virtual string OnReceiveHold(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            ReceiveResponse receiveResponse = eventData as ReceiveResponse;
            ValidateReceiveResponse(receiveResponse);
            try
            {

                WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);


                WorkFlowResponse response = new WorkFlowResponse();
                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(receiveResponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);

                FnExpenseDocument expenseDocument = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(workFlow.Document.DocumentID);
                SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
                expenseDocument.BoxID = receiveResponse.BoxID;
                FnExpenseDocumentService.Update(expenseDocument);
                scgDocument.ReceiveDocumentDate = DateTime.Now;
            }
            catch (Exception)
            {
                throw;
            }
            /*TODO generate signal string*/
            string signal = "ReceiveHoldToHold";
            return signal;
        }
        #endregion

        #region OnUnHoldHold
        public string OnUnHoldHold(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            try
            {
                SubmitResponse submitResponse = eventData as SubmitResponse;

                WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);


                WorkFlowResponse response = new WorkFlowResponse();
                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(submitResponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);
            }
            catch (Exception)
            {
                throw;
            }
            /*TODO generate signal string*/
            string signal = "UnHoldHoldToWaitVerify";

            return signal;
        }
        #endregion

        #region OnPayWaitPayment
        public virtual string OnPayWaitPayment(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

            try
            {
                SubmitResponse submitResponse = eventData as SubmitResponse;

                WorkFlowResponse response = new WorkFlowResponse();
                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(submitResponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);
            }
            catch (Exception)
            {
                throw;
            }

            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            scgDocument.PaidDate = DateTime.Now;
            SCGDocumentService.SaveOrUpdate(scgDocument);
            /*TODO generate signal string*/
            string signal = "PayWaitPaymentToComplete";

            FnExpenseDocument expenseDocument = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(workFlow.Document.DocumentID);

            IList<FnExpenseAdvance> expenseAdvances = ScgeAccountingQueryProvider.FnExpenseAdvanceQuery.FindByExpenseDocumentID(expenseDocument.ExpenseID);
            foreach (FnExpenseAdvance expenseAdvance in expenseAdvances)
            {
                WorkFlow advanceWorkFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(expenseAdvance.Advance.DocumentID.DocumentID);

                // Notify Clearing all advance
                string clearingEventName = "Clearing";
                WorkFlowStateEvent stateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetByWorkFlowStateID_EventName(advanceWorkFlow.CurrentState.WorkFlowStateID, clearingEventName);
                WorkFlowService.NotifyEventFromInternal(advanceWorkFlow.WorkFlowID, clearingEventName, new SubmitResponse(stateEvent.WorkFlowStateEventID));
            }

            return signal;
        }
        #endregion

        #region OnWithdrawWaitPayment
        public string OnWithdrawWaitPayment(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            try
            {
                SubmitResponse submitResponse = eventData as SubmitResponse;

                WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);


                WorkFlowResponse response = new WorkFlowResponse();
                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(submitResponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);

                SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
                //clear VerifiedDate & ApproveVerifiedDate
                scgDocument.VerifiedDate = null;
                scgDocument.ApproveVerifiedDate = null;
                scgDocument.PaidDate = null;
                SCGDocumentService.SaveOrUpdate(scgDocument);
            }
            catch (Exception)
            {
                throw;
            }
            /*TODO generate signal string*/
            string signal = "WithdrawWaitPaymentToWaitVerify";

            return signal;
        }
        #endregion

        #region OnReceiveWaitPayment
        public virtual string OnReceiveWaitPayment(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            ReceiveResponse receiveResponse = eventData as ReceiveResponse;
            ValidateReceiveResponse(receiveResponse);

            try
            {
                WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);


                WorkFlowResponse response = new WorkFlowResponse();
                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(receiveResponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);

                FnExpenseDocument expenseDocument = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(workFlow.Document.DocumentID);
                SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
                expenseDocument.BoxID = receiveResponse.BoxID;
                FnExpenseDocumentService.Update(expenseDocument);
                scgDocument.ReceiveDocumentDate = DateTime.Now;
            }
            catch (Exception)
            {
                throw;
            }
            /*TODO generate signal string*/
            string signal = "ReceiveWaitPaymentToWaitPayment";

            return signal;
        }
        #endregion

        #region OnReceiveComplete
        public virtual string OnReceiveComplete(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            ReceiveResponse receiveResponse = eventData as ReceiveResponse;
            ValidateReceiveResponse(receiveResponse);

            try
            {
                WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);


                WorkFlowResponse response = new WorkFlowResponse();
                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(receiveResponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);

                FnExpenseDocument expenseDocument = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(workFlow.Document.DocumentID);
                SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
                expenseDocument.BoxID = receiveResponse.BoxID;
                FnExpenseDocumentService.Update(expenseDocument);
                scgDocument.ReceiveDocumentDate = DateTime.Now;
            }
            catch (Exception)
            {
                throw;
            }
            /*TODO generate signal string*/
            string signal = "ReceiveComplete";

            return signal;
        }
        #endregion

        #region OnPayWaitPaymentFromSAP
        public virtual string OnPayWaitPaymentFromSAP(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);

            try
            {
                SubmitResponse submitResponse = eventData as SubmitResponse;

                WorkFlowResponse response = new WorkFlowResponse();
                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(submitResponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;

                response.ResponseMethod = ResponseMethod.SAP.GetHashCode().ToString();

                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);
            }
            catch (Exception)
            {
                throw;
            }

            scgDocument.PaidDate = DateTime.Now;
            SCGDocumentService.SaveOrUpdate(scgDocument);

            /*TODO generate signal string*/
            string signal = "PayWaitPaymentFromSAPToComplete";

            FnExpenseDocument expenseDocument = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(scgDocument.DocumentID);

            /*Clearing Advance*/
            IList<FnExpenseAdvance> expenseAdvances = ScgeAccountingQueryProvider.FnExpenseAdvanceQuery.FindByExpenseDocumentID(expenseDocument.ExpenseID);
            foreach (FnExpenseAdvance expenseAdvance in expenseAdvances)
            {
                WorkFlow advanceWorkFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(expenseAdvance.Advance.DocumentID.DocumentID);

                // Notify Clearing all advance
                string clearingEventName = "Clearing";
                WorkFlowStateEvent stateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetByWorkFlowStateID_EventName(advanceWorkFlow.CurrentState.WorkFlowStateID, clearingEventName);
                WorkFlowService.NotifyEventFromInternal(advanceWorkFlow.WorkFlowID, clearingEventName, new SubmitResponse(stateEvent.WorkFlowStateEventID));
            }
            /*------------------------*/

            IList<long> ccList = new List<long>();
            if (scgDocument.CreatorID.Userid != scgDocument.ReceiverID.Userid)
                ccList.Add(scgDocument.CreatorID.Userid);
            if (scgDocument.CreatorID.Userid != scgDocument.RequesterID.Userid)
                ccList.Add(scgDocument.RequesterID.Userid);

            if (expenseDocument.PaymentType == PaymentType.CQ)
            {
                SCGEmailService.SendEmailEM05(workFlowID, scgDocument.ReceiverID.Userid, ccList, true);
                //#4
                //if (ParameterServices.EnableSMS)
                //{
                IList<long> receiverList = new List<long>();
                receiverList.Add(scgDocument.ReceiverID.Userid);

                SCGSMSService.SendSMS02(workFlowID, scgDocument.RequesterID.Userid.ToString(), receiverList, true);
                //}

            }
            else if (expenseDocument.PaymentType == PaymentType.TR)
            {
                SCGEmailService.SendEmailEM06(workFlowID, scgDocument.ReceiverID.Userid, ccList);
                //#1
                //if (ParameterServices.EnableSMS)
                //{
                IList<long> receiverList = new List<long>();
                receiverList.Add(scgDocument.ReceiverID.Userid);
                SCGSMSService.SendSMS03(workFlowID, scgDocument.RequesterID.Userid.ToString(), receiverList);
                //}
            }




            return signal;
        }
        #endregion

        #region OnReceiveWaitPaymentFromSAP
        public virtual string OnReceiveWaitPaymentFromSAP(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            ReceiveResponse receiveResponse = eventData as ReceiveResponse;
            ValidateReceiveResponse(receiveResponse);

            try
            {

                WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);


                WorkFlowResponse response = new WorkFlowResponse();
                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(receiveResponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);

                FnExpenseDocument expenseDocument = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(workFlow.Document.DocumentID);
                SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
                expenseDocument.BoxID = receiveResponse.BoxID;
                FnExpenseDocumentService.Update(expenseDocument);
                scgDocument.ReceiveDocumentDate = DateTime.Now;
            }
            catch (Exception)
            {
                throw;
            }
            /*TODO generate signal string*/
            string signal = "ReceiveWaitPaymentFromSAPToWaitPaymentFromSAP";

            return signal;
        }
        #endregion

        #region OnWithdrawWaitPaymentFromSAP
        public virtual string OnWithdrawWaitPaymentFromSAP(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            try
            {
                SubmitResponse submitResponse = eventData as SubmitResponse;

                WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);


                WorkFlowResponse response = new WorkFlowResponse();
                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(submitResponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);

                SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
                //clear VerifiedDate & ApproveVerifiedDate
                scgDocument.VerifiedDate = null;
                scgDocument.ApproveVerifiedDate = null;
                scgDocument.PaidDate = null;
                SCGDocumentService.SaveOrUpdate(scgDocument);
            }
            catch (Exception)
            {
                throw;
            }
            /*TODO generate signal string*/
            string signal = "WithdrawWaitPaymentFromSAPToWaitVerify";

            return signal;
        }
        #endregion

        #region OnPayWaitRemittance
        public virtual string OnPayWaitRemittance(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            long workflowResponseId = 0;
            try
            {
                FnExpenseDocument expenseDocument = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(workFlow.Document.DocumentID);

                if (eventData is SubmitResponse)
                {
                    //Validate Posting Status
                    Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

                    if (expenseDocument.RemittancePostingStatus != PostingStaus.Complete)
                    {
                        errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("Workflow_OnPayWaitRemittance_Mismatch_PostingStatus"));
                    }
                    if (!errors.IsEmpty) throw new ServiceValidationException(errors);

                    SubmitResponse submitResponse = eventData as SubmitResponse;

                    WorkFlowResponse response = new WorkFlowResponse();
                    response.WorkFlow = workFlow;
                    response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(submitResponse.WorkFlowStateEventID);
                    response.ResponseBy = UserAccount.UserID;
                    response.ResponseDate = DateTime.Now;
                    response.Active = true;
                    response.CreBy = UserAccount.UserID;
                    response.CreDate = DateTime.Now;
                    response.UpdBy = UserAccount.UserID;
                    response.UpdDate = DateTime.Now;
                    response.UpdPgm = UserAccount.CurrentProgramCode;
                    WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);
                    workflowResponseId = response.WorkFlowResponseID;

                }
                else
                {
                    ExpenseRemittanceDetailResponse detailResponse = eventData as ExpenseRemittanceDetailResponse;
                    if (detailResponse != null)
                    {
                        ValidateExpenseRemittanceDetailResponse(detailResponse);

                        expenseDocument.ReceivedMethod = detailResponse.ReceivedMethod;
                        if (!string.IsNullOrEmpty(detailResponse.ReceivedMethod) && detailResponse.ReceivedMethod.Equals("Bank"))
                        {
                            expenseDocument.PayInGLAccount = detailResponse.GLAccount;
                            expenseDocument.PayInValueDate = detailResponse.ValueDate;
                            expenseDocument.UpdBy = UserAccount.UserID;
                            expenseDocument.UpdDate = DateTime.Now;
                            expenseDocument.UpdPgm = UserAccount.CurrentProgramCode;
                        }

                        FnExpenseDocumentService.Update(expenseDocument);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            scgDocument.RemittanceDate = DateTime.Now;
            SCGDocumentService.SaveOrUpdate(scgDocument);

            string signal = string.Empty;
            FnExpenseDocument expenseDoc = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(workFlow.Document.DocumentID);
            if (expenseDoc.RemittancePostingStatus != PostingStaus.Complete)
            {
                signal = "PayWaitRemittanceToWaitRemittance";
            }
            else
            {
                signal = "PayWaitRemittanceToComplete";

                IList<FnExpenseAdvance> expenseAdvances = ScgeAccountingQueryProvider.FnExpenseAdvanceQuery.FindByExpenseDocumentID(expenseDoc.ExpenseID);
                foreach (FnExpenseAdvance expenseAdvance in expenseAdvances)
                {
                    WorkFlow advanceWorkFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(expenseAdvance.Advance.DocumentID.DocumentID);

                    // Notify Clearing all advance
                    string clearingEventName = "Clearing";
                    WorkFlowStateEvent stateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetByWorkFlowStateID_EventName(advanceWorkFlow.CurrentState.WorkFlowStateID, clearingEventName);
                    WorkFlowService.NotifyEventFromInternal(advanceWorkFlow.WorkFlowID, clearingEventName, new SubmitResponse(stateEvent.WorkFlowStateEventID));
                }
                if (!string.IsNullOrEmpty(expenseDoc.ReceivedMethod) && expenseDoc.ReceivedMethod.Equals("Bank"))
                {
                    SCGEmailService.SendEmailEM13(expenseDoc.Document.DocumentID);
                }
            }
            return signal;
        }
        #endregion

        #region OnWithdrawWaitRemittance
        public string OnWithdrawWaitRemittance(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            try
            {
                WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

                FnExpenseDocument expenseDocument = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(workFlow.Document.DocumentID);
                expenseDocument.ReceivedMethod = null;
                expenseDocument.PayInGLAccount = string.Empty;
                expenseDocument.PayInValueDate = null;
                expenseDocument.UpdBy = UserAccount.UserID;
                expenseDocument.UpdDate = DateTime.Now;
                expenseDocument.UpdPgm = UserAccount.CurrentProgramCode;
                FnExpenseDocumentService.Update(expenseDocument);

                SubmitResponse submitResponse = eventData as SubmitResponse;

                WorkFlowResponse response = new WorkFlowResponse();
                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(submitResponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);

                SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
                //clear VerifiedDate & ApproveVerifiedDate
                scgDocument.VerifiedDate = null;
                scgDocument.ApproveVerifiedDate = null;
                scgDocument.RemittanceDate = null;
                SCGDocumentService.SaveOrUpdate(scgDocument);
            }
            catch (Exception)
            {
                throw;
            }
            /*TODO generate signal string*/
            string signal = "WithdrawWaitRemittanceToWaitVerify";

            return signal;
        }
        #endregion

        #region OnApproveWaitApproveRejection
        public string OnApproveWaitApproveRejection(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            WorkFlowResponse response = new WorkFlowResponse();

            try
            {
                SubmitResponse submitResponse = eventData as SubmitResponse;

                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(submitResponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);


            }
            catch (Exception)
            {
                throw;
            }
            /*TODO generate signal string*/
            string signal = "ApproveWaitApproveRejectionToDraft";

            try
            {
                Document document = WorkFlowQueryProvider.WorkFlowQuery.GetDocumentByWorkFlowID(workFlowID);
                FnExpenseDocument expense = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(document.DocumentID);

                if (expense != null)
                {
                    expense.BoxID = string.Empty;
                    scgDocument.ReceiveDocumentDate = null;
                    ScgeAccountingDaoProvider.FnExpenseDocumentDao.SaveOrUpdate(expense);
                }
                SCGEmailService.SendEmailEM04(response.WorkFlowResponseID, scgDocument.CreatorID.Userid);

                if (ParameterServices.EnableEmail02Requester)
                {
                    if (scgDocument.CreatorID.Userid != scgDocument.RequesterID.Userid)
                        SCGEmailService.SendEmailEM02(response.WorkFlowResponseID, scgDocument.RequesterID.Userid, null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return signal;
        }
        #endregion

        #region OnRejectWaitApproveRejection
        public string OnRejectWaitApproveRejection(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            try
            {
                RejectDetailResponse rejectDetailResponse = eventData as RejectDetailResponse;

                ValidateRejectResponse(rejectDetailResponse);

                WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);


                WorkFlowResponse response = new WorkFlowResponse();
                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(rejectDetailResponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);

                WorkFlowRejectResponse rejectResponse = new WorkFlowRejectResponse();
                rejectResponse.WorkFlowResponse = response;
                rejectResponse.Reason = ScgDbQueryProvider.DbRejectReasonQuery.FindProxyByIdentity(rejectDetailResponse.ReasonID);
                rejectResponse.NeedApproveRejection = rejectDetailResponse.NeedApproveRejection;
                rejectResponse.Remark = rejectDetailResponse.Remark;
                rejectResponse.Active = true;
                rejectResponse.CreBy = UserAccount.UserID;
                rejectResponse.CreDate = DateTime.Now;
                rejectResponse.UpdBy = UserAccount.UserID;
                rejectResponse.UpdDate = DateTime.Now;
                rejectResponse.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowRejectResponseDao.Save(rejectResponse);

            }
            catch (Exception)
            {
                throw;
            }
            /*TODO generate signal string*/
            string signal = "RejectWaitApproveRejectionToWaitVerify";

            return signal;
        }
        #endregion

        #region OnReceiveWaitApproveRejection
        public virtual string OnReceiveWaitApproveRejection(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            ReceiveResponse receiveResponse = eventData as ReceiveResponse;
            ValidateReceiveResponse(receiveResponse);

            try
            {

                WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);


                WorkFlowResponse response = new WorkFlowResponse();
                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(receiveResponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);

                FnExpenseDocument expenseDocument = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(workFlow.Document.DocumentID);
                SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
                expenseDocument.BoxID = receiveResponse.BoxID;
                FnExpenseDocumentService.Update(expenseDocument);
                scgDocument.ReceiveDocumentDate = DateTime.Now;
            }
            catch (Exception)
            {
                throw;
            }
            /*TODO generate signal string*/
            string signal = "ReceiveWaitApproveRejectionToWaitApproveRejection";

            return signal;
        }
        #endregion


        #region OnApproveWaitReverse
        public string OnApproveWaitReverse(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            try
            {
                SubmitResponse submitResponse = eventData as SubmitResponse;

                WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);


                WorkFlowResponse response = new WorkFlowResponse();
                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(submitResponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);
            }
            catch (Exception)
            {
                throw;
            }
            /*TODO generate signal string*/
            string signal = "ApproveWaitReverseToWaitApproveVerify";

            return signal;
        }
        #endregion

        #region OnRejectWaitReverse
        public string OnRejectWaitReverse(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            try
            {
                RejectDetailResponse rejectDetailResponse = eventData as RejectDetailResponse;

                ValidateRejectResponse(rejectDetailResponse);

                WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);


                WorkFlowResponse response = new WorkFlowResponse();
                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(rejectDetailResponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);

                WorkFlowRejectResponse rejectResponse = new WorkFlowRejectResponse();
                rejectResponse.WorkFlowResponse = response;
                rejectResponse.Reason = ScgDbQueryProvider.DbRejectReasonQuery.FindProxyByIdentity(rejectDetailResponse.ReasonID);
                rejectResponse.NeedApproveRejection = rejectDetailResponse.NeedApproveRejection;
                rejectResponse.Remark = rejectDetailResponse.Remark;
                rejectResponse.Active = true;
                rejectResponse.CreBy = UserAccount.UserID;
                rejectResponse.CreDate = DateTime.Now;
                rejectResponse.UpdBy = UserAccount.UserID;
                rejectResponse.UpdDate = DateTime.Now;
                rejectResponse.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowRejectResponseDao.Save(rejectResponse);
            }
            catch (Exception)
            {
                throw;
            }
            /*TODO generate signal string*/
            string signal = "RejectWaitReverseToWaitVerify";

            return signal;
        }
        #endregion

        #region OnReceiveWaitDocument
        public virtual string OnReceiveWaitDocument(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            ReceiveResponse receiveResponse = eventData as ReceiveResponse;
            ValidateReceiveResponse(receiveResponse);

            try
            {

                WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);


                WorkFlowResponse response = new WorkFlowResponse();
                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(receiveResponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);

                FnExpenseDocument expenseDocument = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(workFlow.Document.DocumentID);
                SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
                expenseDocument.BoxID = receiveResponse.BoxID;
                FnExpenseDocumentService.Update(expenseDocument);
                scgDocument.ReceiveDocumentDate = DateTime.Now;


            }
            catch (Exception)
            {
                throw;
            }
            /*TODO generate signal string*/
            string signal = "ReceiveWaitDocumentToWaitVerify";

            return signal;
        }
        #endregion

        #region OnRejectWaitDocument
        public string OnRejectWaitDocument(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);

            try
            {
                RejectDetailResponse rejectDetailResponse = eventData as RejectDetailResponse;

                ValidateRejectResponse(rejectDetailResponse);

                WorkFlowResponse response = new WorkFlowResponse();
                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(rejectDetailResponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);

                WorkFlowRejectResponse rejectResponse = new WorkFlowRejectResponse();
                rejectResponse.WorkFlowResponse = response;
                rejectResponse.Reason = ScgDbQueryProvider.DbRejectReasonQuery.FindProxyByIdentity(rejectDetailResponse.ReasonID);
                rejectResponse.NeedApproveRejection = rejectDetailResponse.NeedApproveRejection;
                rejectResponse.Remark = rejectDetailResponse.Remark;
                rejectResponse.Active = true;
                rejectResponse.CreBy = UserAccount.UserID;
                rejectResponse.CreDate = DateTime.Now;
                rejectResponse.UpdBy = UserAccount.UserID;
                rejectResponse.UpdDate = DateTime.Now;
                rejectResponse.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowRejectResponseDao.Save(rejectResponse);

                scgDocument.ApproveDate = null;
                scgDocument.UpdBy = UserAccount.UserID;
                scgDocument.UpdDate = DateTime.Now;
                scgDocument.UpdPgm = UserAccount.CurrentProgramCode;
                SCGDocumentService.SaveOrUpdate(scgDocument);

                SCGEmailService.SendEmailEM04(response.WorkFlowResponseID, scgDocument.CreatorID.Userid);

                if (ParameterServices.EnableEmail02Requester)
                {
                    if (scgDocument.CreatorID.Userid != scgDocument.RequesterID.Userid)
                        SCGEmailService.SendEmailEM02(response.WorkFlowResponseID, scgDocument.RequesterID.Userid, null);
                }
            }
            catch (Exception)
            {
                throw;
            }
            /*TODO generate signal string*/
            string signal = "RejectWaitDocumentToDraft";


            return signal;
        }
        #endregion

        #endregion

        #region OnEnter{StateName} , OnExit{StateName}

        public override void OnEnterDraft(long workFlowID)
        {
            base.OnEnterDraft(workFlowID);

            Document document = WorkFlowQueryProvider.WorkFlowQuery.GetDocumentByWorkFlowID(workFlowID);
            FnExpenseDocument expenseDocument = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(document.DocumentID);

            if (expenseDocument != null)
            {
                expenseDocument.AmountApproved = null;
                expenseDocument.AmountBeforeVerify = null;
                expenseDocument.UpdBy = UserAccount.UserID;
                expenseDocument.UpdDate = DateTime.Now;
                expenseDocument.UpdPgm = UserAccount.CurrentProgramCode;
                FnExpenseDocumentService.Update(expenseDocument);
            }
        }

        public void OnEnterWaitVerify(long workFlowID)
        {
            Document document = WorkFlowQueryProvider.WorkFlowQuery.GetDocumentByWorkFlowID(workFlowID);
            FnExpenseDocument expenseDocument = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(document.DocumentID);

            if (expenseDocument != null)
            {
                expenseDocument.AmountBeforeVerify = expenseDocument.TotalExpense;
                expenseDocument.UpdBy = UserAccount.UserID;
                expenseDocument.UpdDate = DateTime.Now;
                expenseDocument.UpdPgm = UserAccount.CurrentProgramCode;
                FnExpenseDocumentService.Update(expenseDocument);
            }
        }
        public void OnExitWaitVerify(long workFlowID)
        {

        }

        public void OnEnterWaitApproveVerify(long workFlowID)
        {

        }
        public void OnExitWaitApproveVerify(long workFlowID)
        {

        }

        public void OnEnterHold(long workFlowID)
        {

        }
        public void OnExitHold(long workFlowID)
        {

        }

        public void OnEnterWaitPayment(long workFlowID)
        {

        }
        public void OnExitWaitPayment(long workFlowID)
        {

        }

        public void OnEnterWaitPaymentFromSAP(long workFlowID)
        {

        }
        public void OnExitWaitPaymentFromSAP(long workFlowID)
        {

        }

        public void OnEnterWaitRemittance(long workFlowID)
        {

        }

        public void OnExitWaitRemittance(long workFlowID)
        {

        }



        public void OnEnterWaitApproveRejection(long workFlowID)
        {

        }
        public void OnExitWaitApproveRejection(long workFlowID)
        {

        }

        public void OnEnterWaitDocument(long workFlowID)
        {

        }
        public void OnExitWaitDocument(long workFlowID)
        {

        }

        public void OnEnterWaitReverse(long workFlowID)
        {

        }
        public void OnExitWaitReverse(long workFlowID)
        {

        }


        #endregion

        #region GetEditableFieldsFor{StateName}

        public virtual IList<object> GetEditableFieldsForDraft(long workFlowID)
        {
            IList<object> editableFieldEnum = new List<object>();

            long permitUserID = GetPermitUserDraft(workFlowID);
            if (UserAccount.UserID == permitUserID)
            {
                editableFieldEnum.Add(ExpenseFieldGroup.CostCenter);
                editableFieldEnum.Add(ExpenseFieldGroup.AccountCode);
                editableFieldEnum.Add(ExpenseFieldGroup.InternalOrder);
                editableFieldEnum.Add(ExpenseFieldGroup.Subject);
                editableFieldEnum.Add(ExpenseFieldGroup.ReferenceNo);
                editableFieldEnum.Add(ExpenseFieldGroup.VAT);
                editableFieldEnum.Add(ExpenseFieldGroup.WHTax);
                editableFieldEnum.Add(ExpenseFieldGroup.Other);
                editableFieldEnum.Add(ExpenseFieldGroup.InvoiceVerifier);
                editableFieldEnum.Add(ExpenseFieldGroup.Initiator);
                editableFieldEnum.Add(ExpenseFieldGroup.Company);
                editableFieldEnum.Add(ExpenseFieldGroup.BuActor);
                editableFieldEnum.Add(ExpenseFieldGroup.CounterCashier);
                editableFieldEnum.Add(ExpenseFieldGroup.PerdiemRate);
                editableFieldEnum.Add(ExpenseFieldGroup.LocalCurrency);
            }
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            if (string.IsNullOrEmpty(workFlow.Document.DocumentNo))
                editableFieldEnum.Add(ExpenseFieldGroup.Company);
            else if (!string.IsNullOrEmpty(workFlow.Document.DocumentNo))
                editableFieldEnum.Remove(ExpenseFieldGroup.Company);

            return editableFieldEnum;
        }

        public virtual IList<object> GetEditableFieldsForHold(long workFlowID)
        {
            IList<object> editableFieldEnum = new List<object>();

            long permitUserID = GetPermitUserHold(workFlowID);
            if (UserAccount.UserID == permitUserID)
            {
                WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

                IList<WorkFlowHoldResponseDetail> holdFields = WorkFlowQueryProvider.WorkFlowHoldResponseDetailQuery.GetHoldFields(workFlowID, FnExpenseConstant.ExpenseHoldStateEventID);
                foreach (WorkFlowHoldResponseDetail holdField in holdFields)
                {
                    editableFieldEnum.Add((ExpenseFieldGroup)Enum.Parse(typeof(ExpenseFieldGroup), holdField.FieldName));
                }
            }
            return editableFieldEnum;
        }

        public virtual IList<object> GetEditableFieldsForWaitVerify(long workFlowID)
        {
            IList<object> editableFieldEnum = new List<object>();
            //TODO: Check UserAccount is verifier?
            IList<SuRole> permissionRoles = GetPermitRoleVerify(workFlowID);
            if (MatchCurrentUserRole(permissionRoles))
            {
                editableFieldEnum.Add(ExpenseFieldGroup.CostCenter);
                editableFieldEnum.Add(ExpenseFieldGroup.AccountCode);
                editableFieldEnum.Add(ExpenseFieldGroup.InternalOrder);
                editableFieldEnum.Add(ExpenseFieldGroup.Subject);
                editableFieldEnum.Add(ExpenseFieldGroup.ReferenceNo);
                editableFieldEnum.Add(ExpenseFieldGroup.VAT);
                editableFieldEnum.Add(ExpenseFieldGroup.WHTax);
                editableFieldEnum.Add(ExpenseFieldGroup.Other);
                editableFieldEnum.Add(ExpenseFieldGroup.VerifyDetail);
                editableFieldEnum.Add(ExpenseFieldGroup.InvoiceVerifier);
                editableFieldEnum.Add(ExpenseFieldGroup.CounterCashier);
            }

            return editableFieldEnum;
        }
        public virtual IList<object> GetEditableFieldsForWaitPayment(long workFlowID)
        {
            IList<object> editableFieldEnum = new List<object>();
            //TODO: Check UserAccount is verifier?
            IList<SuRole> permissionRoles = GetPermitRoleCounterCashier(workFlowID);
            if (MatchCurrentUserRole(permissionRoles))
            {
                editableFieldEnum.Add(ExpenseFieldGroup.CounterCashier);
            }

            return editableFieldEnum;
        }
        #endregion

        #region GetVisibleFieldsFor{StateName}

        public virtual IList<object> GetVisibleFieldsForDraft(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(ExpenseFieldGroup.CostCenter);
            visibleFieldEnum.Add(ExpenseFieldGroup.AccountCode);
            visibleFieldEnum.Add(ExpenseFieldGroup.InternalOrder);
            visibleFieldEnum.Add(ExpenseFieldGroup.Subject);
            visibleFieldEnum.Add(ExpenseFieldGroup.ReferenceNo);
            visibleFieldEnum.Add(ExpenseFieldGroup.VAT);
            visibleFieldEnum.Add(ExpenseFieldGroup.WHTax);
            visibleFieldEnum.Add(ExpenseFieldGroup.Other);
            visibleFieldEnum.Add(ExpenseFieldGroup.Initiator);
            visibleFieldEnum.Add(ExpenseFieldGroup.Company);
            visibleFieldEnum.Add(ExpenseFieldGroup.BuActor);
            visibleFieldEnum.Add(ExpenseFieldGroup.CounterCashier);
            visibleFieldEnum.Add(ExpenseFieldGroup.PerdiemRate);
            visibleFieldEnum.Add(ExpenseFieldGroup.LocalCurrency);

            return visibleFieldEnum;
        }

        public virtual IList<object> GetVisibleFieldsForWaitAR(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(ExpenseFieldGroup.CostCenter);
            visibleFieldEnum.Add(ExpenseFieldGroup.AccountCode);
            visibleFieldEnum.Add(ExpenseFieldGroup.InternalOrder);
            visibleFieldEnum.Add(ExpenseFieldGroup.Subject);
            visibleFieldEnum.Add(ExpenseFieldGroup.ReferenceNo);
            visibleFieldEnum.Add(ExpenseFieldGroup.VAT);
            visibleFieldEnum.Add(ExpenseFieldGroup.WHTax);
            visibleFieldEnum.Add(ExpenseFieldGroup.Other);
            visibleFieldEnum.Add(ExpenseFieldGroup.Initiator);
            visibleFieldEnum.Add(ExpenseFieldGroup.Company);
            visibleFieldEnum.Add(ExpenseFieldGroup.BuActor);
            visibleFieldEnum.Add(ExpenseFieldGroup.CounterCashier);
            visibleFieldEnum.Add(ExpenseFieldGroup.PerdiemRate);
            visibleFieldEnum.Add(ExpenseFieldGroup.LocalCurrency);

            return visibleFieldEnum;
        }

        public virtual IList<object> GetVisibleFieldsForWaitInitial(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(ExpenseFieldGroup.CostCenter);
            visibleFieldEnum.Add(ExpenseFieldGroup.AccountCode);
            visibleFieldEnum.Add(ExpenseFieldGroup.InternalOrder);
            visibleFieldEnum.Add(ExpenseFieldGroup.Subject);
            visibleFieldEnum.Add(ExpenseFieldGroup.ReferenceNo);
            visibleFieldEnum.Add(ExpenseFieldGroup.VAT);
            visibleFieldEnum.Add(ExpenseFieldGroup.WHTax);
            visibleFieldEnum.Add(ExpenseFieldGroup.Other);
            visibleFieldEnum.Add(ExpenseFieldGroup.Initiator);
            visibleFieldEnum.Add(ExpenseFieldGroup.Company);
            visibleFieldEnum.Add(ExpenseFieldGroup.BuActor);
            visibleFieldEnum.Add(ExpenseFieldGroup.CounterCashier);
            visibleFieldEnum.Add(ExpenseFieldGroup.PerdiemRate);
            visibleFieldEnum.Add(ExpenseFieldGroup.LocalCurrency);

            return visibleFieldEnum;
        }

        public virtual IList<object> GetVisibleFieldsForWaitApprove(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(ExpenseFieldGroup.CostCenter);
            visibleFieldEnum.Add(ExpenseFieldGroup.AccountCode);
            visibleFieldEnum.Add(ExpenseFieldGroup.InternalOrder);
            visibleFieldEnum.Add(ExpenseFieldGroup.Subject);
            visibleFieldEnum.Add(ExpenseFieldGroup.ReferenceNo);
            visibleFieldEnum.Add(ExpenseFieldGroup.VAT);
            visibleFieldEnum.Add(ExpenseFieldGroup.WHTax);
            visibleFieldEnum.Add(ExpenseFieldGroup.Other);
            visibleFieldEnum.Add(ExpenseFieldGroup.Initiator);
            visibleFieldEnum.Add(ExpenseFieldGroup.Company);
            visibleFieldEnum.Add(ExpenseFieldGroup.BuActor);
            visibleFieldEnum.Add(ExpenseFieldGroup.CounterCashier);
            visibleFieldEnum.Add(ExpenseFieldGroup.PerdiemRate);
            visibleFieldEnum.Add(ExpenseFieldGroup.LocalCurrency);

            return visibleFieldEnum;
        }

        public virtual IList<object> GetVisibleFieldsForWaitVerify(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(ExpenseFieldGroup.CostCenter);
            visibleFieldEnum.Add(ExpenseFieldGroup.AccountCode);
            visibleFieldEnum.Add(ExpenseFieldGroup.InternalOrder);
            visibleFieldEnum.Add(ExpenseFieldGroup.Subject);
            visibleFieldEnum.Add(ExpenseFieldGroup.ReferenceNo);
            visibleFieldEnum.Add(ExpenseFieldGroup.VAT);
            visibleFieldEnum.Add(ExpenseFieldGroup.WHTax);
            visibleFieldEnum.Add(ExpenseFieldGroup.Other);
            visibleFieldEnum.Add(ExpenseFieldGroup.Initiator);
            visibleFieldEnum.Add(ExpenseFieldGroup.Company);
            visibleFieldEnum.Add(ExpenseFieldGroup.BuActor);
            visibleFieldEnum.Add(ExpenseFieldGroup.CounterCashier);
            visibleFieldEnum.Add(ExpenseFieldGroup.PerdiemRate);
            visibleFieldEnum.Add(ExpenseFieldGroup.LocalCurrency);

            IList<SuRole> permissionRoles = GetPermitRoleVerify(workFlowID);
            if (MatchCurrentUserRole(permissionRoles))
            {
                visibleFieldEnum.Add(ExpenseFieldGroup.VerifyDetail);
                visibleFieldEnum.Add(ExpenseFieldGroup.InvoiceVerifier);
                visibleFieldEnum.Add(ExpenseFieldGroup.BoxID);
            }

            permissionRoles = GetPermitRoleReceive(workFlowID);
            if (MatchCurrentUserRole(permissionRoles))
            {
                if (!visibleFieldEnum.Contains(ExpenseFieldGroup.BoxID))
                    visibleFieldEnum.Add(ExpenseFieldGroup.BoxID);
            }
            return visibleFieldEnum;
        }

        public virtual IList<object> GetVisibleFieldsForWaitApproveVerify(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(ExpenseFieldGroup.CostCenter);
            visibleFieldEnum.Add(ExpenseFieldGroup.AccountCode);
            visibleFieldEnum.Add(ExpenseFieldGroup.InternalOrder);
            visibleFieldEnum.Add(ExpenseFieldGroup.Subject);
            visibleFieldEnum.Add(ExpenseFieldGroup.ReferenceNo);
            visibleFieldEnum.Add(ExpenseFieldGroup.VAT);
            visibleFieldEnum.Add(ExpenseFieldGroup.WHTax);
            visibleFieldEnum.Add(ExpenseFieldGroup.Other);
            visibleFieldEnum.Add(ExpenseFieldGroup.Initiator);
            visibleFieldEnum.Add(ExpenseFieldGroup.Company);
            visibleFieldEnum.Add(ExpenseFieldGroup.BuActor);
            visibleFieldEnum.Add(ExpenseFieldGroup.CounterCashier);
            visibleFieldEnum.Add(ExpenseFieldGroup.PerdiemRate);
            visibleFieldEnum.Add(ExpenseFieldGroup.LocalCurrency);

            IList<SuRole> permissionRoles = GetPermitRoleVerify(workFlowID);
            if (MatchCurrentUserRole(permissionRoles))
            {
                visibleFieldEnum.Add(ExpenseFieldGroup.VerifyDetail);
                visibleFieldEnum.Add(ExpenseFieldGroup.BoxID);
                visibleFieldEnum.Add(ExpenseFieldGroup.InvoiceVerifier);
                visibleFieldEnum.Add(ExpenseFieldGroup.RemittantDetail);
            }

            permissionRoles = GetPermitRoleApproveVerify(workFlowID);
            if (MatchCurrentUserRole(permissionRoles))
            {
                visibleFieldEnum.Add(ExpenseFieldGroup.VerifyDetail);
                visibleFieldEnum.Add(ExpenseFieldGroup.BoxID);
                visibleFieldEnum.Add(ExpenseFieldGroup.InvoiceVerifier);
                visibleFieldEnum.Add(ExpenseFieldGroup.RemittantDetail);
            }

            return visibleFieldEnum;
        }

        public virtual IList<object> GetVisibleFieldsForHold(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(ExpenseFieldGroup.CostCenter);
            visibleFieldEnum.Add(ExpenseFieldGroup.AccountCode);
            visibleFieldEnum.Add(ExpenseFieldGroup.InternalOrder);
            visibleFieldEnum.Add(ExpenseFieldGroup.Subject);
            visibleFieldEnum.Add(ExpenseFieldGroup.ReferenceNo);
            visibleFieldEnum.Add(ExpenseFieldGroup.VAT);
            visibleFieldEnum.Add(ExpenseFieldGroup.WHTax);
            visibleFieldEnum.Add(ExpenseFieldGroup.Other);
            visibleFieldEnum.Add(ExpenseFieldGroup.Initiator);
            visibleFieldEnum.Add(ExpenseFieldGroup.Company);
            visibleFieldEnum.Add(ExpenseFieldGroup.BuActor);
            visibleFieldEnum.Add(ExpenseFieldGroup.CounterCashier);
            visibleFieldEnum.Add(ExpenseFieldGroup.PerdiemRate);
            visibleFieldEnum.Add(ExpenseFieldGroup.LocalCurrency);

            IList<SuRole> permissionRoles = GetPermitRoleVerify(workFlowID);
            if (MatchCurrentUserRole(permissionRoles))
            {
                visibleFieldEnum.Add(ExpenseFieldGroup.VerifyDetail);
                visibleFieldEnum.Add(ExpenseFieldGroup.BoxID);
                visibleFieldEnum.Add(ExpenseFieldGroup.InvoiceVerifier);
            }

            return visibleFieldEnum;
        }

        public virtual IList<object> GetVisibleFieldsForWaitPayment(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(ExpenseFieldGroup.CostCenter);
            visibleFieldEnum.Add(ExpenseFieldGroup.AccountCode);
            visibleFieldEnum.Add(ExpenseFieldGroup.InternalOrder);
            visibleFieldEnum.Add(ExpenseFieldGroup.Subject);
            visibleFieldEnum.Add(ExpenseFieldGroup.ReferenceNo);
            visibleFieldEnum.Add(ExpenseFieldGroup.VAT);
            visibleFieldEnum.Add(ExpenseFieldGroup.WHTax);
            visibleFieldEnum.Add(ExpenseFieldGroup.Other);
            visibleFieldEnum.Add(ExpenseFieldGroup.Initiator);
            visibleFieldEnum.Add(ExpenseFieldGroup.Company);
            visibleFieldEnum.Add(ExpenseFieldGroup.BuActor);
            visibleFieldEnum.Add(ExpenseFieldGroup.CounterCashier);
            visibleFieldEnum.Add(ExpenseFieldGroup.PerdiemRate);
            visibleFieldEnum.Add(ExpenseFieldGroup.LocalCurrency);

            IList<SuRole> permissionRoles = GetPermitRoleVerify(workFlowID);
            if (MatchCurrentUserRole(permissionRoles))
            {
                visibleFieldEnum.Add(ExpenseFieldGroup.VerifyDetail);
                visibleFieldEnum.Add(ExpenseFieldGroup.BoxID);
                visibleFieldEnum.Add(ExpenseFieldGroup.InvoiceVerifier);
            }

            permissionRoles = GetPermitRoleApproveVerify(workFlowID);
            if (MatchCurrentUserRole(permissionRoles))
            {
                visibleFieldEnum.Add(ExpenseFieldGroup.VerifyDetail);
                visibleFieldEnum.Add(ExpenseFieldGroup.BoxID);
                visibleFieldEnum.Add(ExpenseFieldGroup.InvoiceVerifier);
            }

            return visibleFieldEnum;
        }

        public virtual IList<object> GetVisibleFieldsForWaitPaymentFromSAP(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(ExpenseFieldGroup.CostCenter);
            visibleFieldEnum.Add(ExpenseFieldGroup.AccountCode);
            visibleFieldEnum.Add(ExpenseFieldGroup.InternalOrder);
            visibleFieldEnum.Add(ExpenseFieldGroup.Subject);
            visibleFieldEnum.Add(ExpenseFieldGroup.ReferenceNo);
            visibleFieldEnum.Add(ExpenseFieldGroup.VAT);
            visibleFieldEnum.Add(ExpenseFieldGroup.WHTax);
            visibleFieldEnum.Add(ExpenseFieldGroup.Other);
            visibleFieldEnum.Add(ExpenseFieldGroup.Initiator);
            visibleFieldEnum.Add(ExpenseFieldGroup.Company);
            visibleFieldEnum.Add(ExpenseFieldGroup.BuActor);
            visibleFieldEnum.Add(ExpenseFieldGroup.CounterCashier);
            visibleFieldEnum.Add(ExpenseFieldGroup.PerdiemRate);
            visibleFieldEnum.Add(ExpenseFieldGroup.LocalCurrency);

            IList<SuRole> permissionRoles = GetPermitRoleVerify(workFlowID);
            if (MatchCurrentUserRole(permissionRoles))
            {
                visibleFieldEnum.Add(ExpenseFieldGroup.VerifyDetail);
                visibleFieldEnum.Add(ExpenseFieldGroup.BoxID);
                visibleFieldEnum.Add(ExpenseFieldGroup.InvoiceVerifier);
            }

            permissionRoles = GetPermitRoleApproveVerify(workFlowID);
            if (MatchCurrentUserRole(permissionRoles))
            {
                visibleFieldEnum.Add(ExpenseFieldGroup.VerifyDetail);
                visibleFieldEnum.Add(ExpenseFieldGroup.BoxID);
                visibleFieldEnum.Add(ExpenseFieldGroup.InvoiceVerifier);
            }
            return visibleFieldEnum;
        }

        public virtual IList<object> GetVisibleFieldsForWaitRemittance(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(ExpenseFieldGroup.CostCenter);
            visibleFieldEnum.Add(ExpenseFieldGroup.AccountCode);
            visibleFieldEnum.Add(ExpenseFieldGroup.InternalOrder);
            visibleFieldEnum.Add(ExpenseFieldGroup.Subject);
            visibleFieldEnum.Add(ExpenseFieldGroup.ReferenceNo);
            visibleFieldEnum.Add(ExpenseFieldGroup.VAT);
            visibleFieldEnum.Add(ExpenseFieldGroup.WHTax);
            visibleFieldEnum.Add(ExpenseFieldGroup.Other);
            visibleFieldEnum.Add(ExpenseFieldGroup.Initiator);
            visibleFieldEnum.Add(ExpenseFieldGroup.Company);
            visibleFieldEnum.Add(ExpenseFieldGroup.BuActor);
            visibleFieldEnum.Add(ExpenseFieldGroup.CounterCashier);
            visibleFieldEnum.Add(ExpenseFieldGroup.PerdiemRate);
            visibleFieldEnum.Add(ExpenseFieldGroup.LocalCurrency);

            IList<SuRole> permissionRoles = GetPermitRoleCounterCashier(workFlowID);
            if (MatchCurrentUserRole(permissionRoles))
            {
                visibleFieldEnum.Add(ExpenseFieldGroup.VerifyDetail);
                visibleFieldEnum.Add(ExpenseFieldGroup.BoxID);
                visibleFieldEnum.Add(ExpenseFieldGroup.InvoiceVerifier);
                visibleFieldEnum.Add(ExpenseFieldGroup.RemittantDetail);
                visibleFieldEnum.Add(ExpenseFieldGroup.ExpenseRemittanceDetail);
            }

            return visibleFieldEnum;
        }

        public virtual IList<object> GetVisibleFieldsForWaitApproveRejection(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(ExpenseFieldGroup.CostCenter);
            visibleFieldEnum.Add(ExpenseFieldGroup.AccountCode);
            visibleFieldEnum.Add(ExpenseFieldGroup.InternalOrder);
            visibleFieldEnum.Add(ExpenseFieldGroup.Subject);
            visibleFieldEnum.Add(ExpenseFieldGroup.ReferenceNo);
            visibleFieldEnum.Add(ExpenseFieldGroup.VAT);
            visibleFieldEnum.Add(ExpenseFieldGroup.WHTax);
            visibleFieldEnum.Add(ExpenseFieldGroup.Other);
            visibleFieldEnum.Add(ExpenseFieldGroup.Initiator);
            visibleFieldEnum.Add(ExpenseFieldGroup.Company);
            visibleFieldEnum.Add(ExpenseFieldGroup.BuActor);
            visibleFieldEnum.Add(ExpenseFieldGroup.CounterCashier);
            visibleFieldEnum.Add(ExpenseFieldGroup.PerdiemRate);
            visibleFieldEnum.Add(ExpenseFieldGroup.LocalCurrency);

            IList<SuRole> permissionRoles = GetPermitRoleVerify(workFlowID);
            if (MatchCurrentUserRole(permissionRoles))
            {
                visibleFieldEnum.Add(ExpenseFieldGroup.VerifyDetail);
                visibleFieldEnum.Add(ExpenseFieldGroup.BoxID);
                visibleFieldEnum.Add(ExpenseFieldGroup.InvoiceVerifier);
            }

            permissionRoles = GetPermitRoleApproveVerify(workFlowID);
            if (MatchCurrentUserRole(permissionRoles))
            {
                visibleFieldEnum.Add(ExpenseFieldGroup.VerifyDetail);
                visibleFieldEnum.Add(ExpenseFieldGroup.BoxID);
                visibleFieldEnum.Add(ExpenseFieldGroup.InvoiceVerifier);
            }

            return visibleFieldEnum;
        }

        public virtual IList<object> GetVisibleFieldsForWaitDocument(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(ExpenseFieldGroup.CostCenter);
            visibleFieldEnum.Add(ExpenseFieldGroup.AccountCode);
            visibleFieldEnum.Add(ExpenseFieldGroup.InternalOrder);
            visibleFieldEnum.Add(ExpenseFieldGroup.Subject);
            visibleFieldEnum.Add(ExpenseFieldGroup.ReferenceNo);
            visibleFieldEnum.Add(ExpenseFieldGroup.VAT);
            visibleFieldEnum.Add(ExpenseFieldGroup.WHTax);
            visibleFieldEnum.Add(ExpenseFieldGroup.Other);
            visibleFieldEnum.Add(ExpenseFieldGroup.Initiator);
            visibleFieldEnum.Add(ExpenseFieldGroup.Company);
            visibleFieldEnum.Add(ExpenseFieldGroup.BuActor);
            visibleFieldEnum.Add(ExpenseFieldGroup.CounterCashier);
            visibleFieldEnum.Add(ExpenseFieldGroup.PerdiemRate);
            visibleFieldEnum.Add(ExpenseFieldGroup.LocalCurrency);

            return visibleFieldEnum;
        }

        public virtual IList<object> GetVisibleFieldsForComplete(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(ExpenseFieldGroup.CostCenter);
            visibleFieldEnum.Add(ExpenseFieldGroup.AccountCode);
            visibleFieldEnum.Add(ExpenseFieldGroup.InternalOrder);
            visibleFieldEnum.Add(ExpenseFieldGroup.Subject);
            visibleFieldEnum.Add(ExpenseFieldGroup.ReferenceNo);
            visibleFieldEnum.Add(ExpenseFieldGroup.VAT);
            visibleFieldEnum.Add(ExpenseFieldGroup.WHTax);
            visibleFieldEnum.Add(ExpenseFieldGroup.Other);
            visibleFieldEnum.Add(ExpenseFieldGroup.Initiator);
            visibleFieldEnum.Add(ExpenseFieldGroup.Company);
            visibleFieldEnum.Add(ExpenseFieldGroup.BuActor);
            visibleFieldEnum.Add(ExpenseFieldGroup.CounterCashier);
            visibleFieldEnum.Add(ExpenseFieldGroup.PerdiemRate);
            visibleFieldEnum.Add(ExpenseFieldGroup.LocalCurrency);

            IList<SuRole> permissionRoles = GetPermitRoleVerify(workFlowID);
            if (MatchCurrentUserRole(permissionRoles))
            {
                visibleFieldEnum.Add(ExpenseFieldGroup.VerifyDetail);
                visibleFieldEnum.Add(ExpenseFieldGroup.BoxID);
                visibleFieldEnum.Add(ExpenseFieldGroup.InvoiceVerifier);
                visibleFieldEnum.Add(ExpenseFieldGroup.RemittantDetail);
                visibleFieldEnum.Add(ExpenseFieldGroup.ExpenseRemittanceDetail);
            }

            permissionRoles = GetPermitRoleApproveVerify(workFlowID);
            if (MatchCurrentUserRole(permissionRoles))
            {
                visibleFieldEnum.Add(ExpenseFieldGroup.VerifyDetail);
                visibleFieldEnum.Add(ExpenseFieldGroup.BoxID);
                visibleFieldEnum.Add(ExpenseFieldGroup.InvoiceVerifier);
                visibleFieldEnum.Add(ExpenseFieldGroup.RemittantDetail);
                visibleFieldEnum.Add(ExpenseFieldGroup.ExpenseRemittanceDetail);
            }

            return visibleFieldEnum;
        }

        public virtual IList<object> GetVisibleFieldsForCancel(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(ExpenseFieldGroup.CostCenter);
            visibleFieldEnum.Add(ExpenseFieldGroup.AccountCode);
            visibleFieldEnum.Add(ExpenseFieldGroup.InternalOrder);
            visibleFieldEnum.Add(ExpenseFieldGroup.Subject);
            visibleFieldEnum.Add(ExpenseFieldGroup.ReferenceNo);
            visibleFieldEnum.Add(ExpenseFieldGroup.VAT);
            visibleFieldEnum.Add(ExpenseFieldGroup.WHTax);
            visibleFieldEnum.Add(ExpenseFieldGroup.Other);
            visibleFieldEnum.Add(ExpenseFieldGroup.Initiator);
            visibleFieldEnum.Add(ExpenseFieldGroup.Company);
            visibleFieldEnum.Add(ExpenseFieldGroup.BuActor);
            visibleFieldEnum.Add(ExpenseFieldGroup.CounterCashier);
            visibleFieldEnum.Add(ExpenseFieldGroup.PerdiemRate);
            visibleFieldEnum.Add(ExpenseFieldGroup.LocalCurrency);

            return visibleFieldEnum;
        }

        #endregion

        #region GetPermitRole{EventName}

        public virtual IList<SuRole> GetPermitRoleVerify(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            FnExpenseDocument expenseDocument = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(workFlow.Document.DocumentID);

            //Check user role can Verify document
            IList<SuRole> permitRoles = QueryProvider.SuUserRoleQuery.GetRoleVerifyDocument();

            //Check limit verify amount
            var documentPermitRoles = from role in permitRoles
                                      where Math.Abs(expenseDocument.TotalExpense) >= role.VerifyMinLimit
                                            && Math.Abs(expenseDocument.TotalExpense) <= role.VerifyMaxLimit
                                      select role;

            permitRoles = documentPermitRoles.ToList<SuRole>();
            if (permitRoles.Count == 0) return permitRoles;

            //Check Service Team
            IList<SuRole> serviceTeamRoles = QueryProvider.SuRoleServiceQuery.GetRoleByServiceTeamID(expenseDocument.ServiceTeam.ServiceTeamID);
            documentPermitRoles = from role in permitRoles
                                  join serviceTeamRole in serviceTeamRoles
                                  on role.RoleID equals serviceTeamRole.RoleID
                                  select role;

            return documentPermitRoles.ToList<SuRole>();
        }

        public virtual IList<SuRole> GetPermitRoleApproveVerify(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            FnExpenseDocument expenseDocument = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(workFlow.Document.DocumentID);

            //Check user role can ApproveVerify document
            IList<SuRole> permitRoles = QueryProvider.SuUserRoleQuery.GetRoleApproveVerifyDocument();

            //Check limit approve verify amount
            var documentPermitRoles = from role in permitRoles
                                      where Math.Abs(expenseDocument.TotalExpense) >= role.ApproveVerifyMinLimit
                                            && Math.Abs(expenseDocument.TotalExpense) <= role.ApproveVerifyMaxLimit
                                      select role;

            permitRoles = documentPermitRoles.ToList<SuRole>();
            if (permitRoles.Count == 0) return permitRoles;

            //Check Service Team
            IList<SuRole> serviceTeamRoles = QueryProvider.SuRoleServiceQuery.GetRoleByServiceTeamID(expenseDocument.ServiceTeam.ServiceTeamID);
            documentPermitRoles = from role in permitRoles
                                  join serviceTeamRole in serviceTeamRoles
                                  on role.RoleID equals serviceTeamRole.RoleID
                                  select role;

            return documentPermitRoles.ToList<SuRole>();
        }

        public virtual IList<SuRole> GetPermitRoleVerifyAndApproveVerify(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            FnExpenseDocument expenseDocument = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(workFlow.Document.DocumentID);

            //Check user role can Verify document
            IList<SuRole> permitRoles = QueryProvider.SuUserRoleQuery.GetRoleVerifyAndApproveVerifyDocument();

            //Check limit verify amount
            var documentPermitRoles = from role in permitRoles
                                      where Math.Abs(expenseDocument.TotalExpense) >= role.VerifyMinLimit
                                            && Math.Abs(expenseDocument.TotalExpense) <= role.VerifyMaxLimit
                                      select role;

            permitRoles = documentPermitRoles.ToList<SuRole>();
            if (permitRoles.Count == 0) return permitRoles;

            documentPermitRoles = from role in permitRoles
                                  where Math.Abs(expenseDocument.TotalExpense) >= role.ApproveVerifyMinLimit
                                        && Math.Abs(expenseDocument.TotalExpense) <= role.ApproveVerifyMaxLimit
                                  select role;
            permitRoles = documentPermitRoles.ToList<SuRole>();
            if (permitRoles.Count == 0) return permitRoles;

            //Check Service Team
            IList<SuRole> serviceTeamRoles = QueryProvider.SuRoleServiceQuery.GetRoleByServiceTeamID(expenseDocument.ServiceTeam.ServiceTeamID);
            documentPermitRoles = from role in permitRoles
                                  join serviceTeamRole in serviceTeamRoles
                                  on role.RoleID equals serviceTeamRole.RoleID
                                  select role;

            return documentPermitRoles.ToList<SuRole>();
        }

        public virtual IList<SuRole> GetPermitRoleReceive(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            FnExpenseDocument expenseDocument = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(workFlow.Document.DocumentID);

            //Check user role can Receive document
            IList<SuRole> permitRoles = QueryProvider.SuUserRoleQuery.GetRoleReceiveDocument();
            if (permitRoles.Count == 0) return permitRoles;

            //Check Service Team
            IList<SuRole> serviceTeamRoles = QueryProvider.SuRoleServiceQuery.GetRoleByServiceTeamID(expenseDocument.ServiceTeam.ServiceTeamID);
            var documentPermitRoles = from role in permitRoles
                                      join serviceTeamRole in serviceTeamRoles
                                      on role.RoleID equals serviceTeamRole.RoleID
                                      select role;

            return documentPermitRoles.ToList<SuRole>();
        }

        public virtual IList<SuRole> GetPermitRoleWithdraw(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            FnExpenseDocument expenseDocument = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(workFlow.Document.DocumentID);

            //Check user role can Receive document
            IList<SuRole> permitRoles = QueryProvider.SuUserRoleQuery.GetRoleApproveVerifyDocument();
            if (permitRoles.Count == 0) return permitRoles;

            //Check Service Team
            IList<SuRole> serviceTeamRoles = QueryProvider.SuRoleServiceQuery.GetRoleByServiceTeamID(expenseDocument.ServiceTeam.ServiceTeamID);
            var documentPermitRoles = from role in permitRoles
                                      join serviceTeamRole in serviceTeamRoles
                                      on role.RoleID equals serviceTeamRole.RoleID
                                      select role;

            return documentPermitRoles.ToList<SuRole>();
        }

        public virtual IList<SuRole> GetPermitRoleCounterCashier(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            FnExpenseDocument expenseDocument = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(workFlow.Document.DocumentID);

            //Check user role is Counter Cashier
            IList<SuRole> permitRoles = QueryProvider.SuUserRoleQuery.GetRoleCounterCashier();
            if (permitRoles.Count == 0) return permitRoles;

            ////Check Service Team
            //IList<SuRole> serviceTeamRoles = QueryProvider.SuRoleServiceQuery.GetRoleByServiceTeamID(expenseDocument.ServiceTeam.ServiceTeamID);
            //var documentPermitRoles = from role in permitRoles
            //                      join serviceTeamRole in serviceTeamRoles
            //                      on role.RoleID equals serviceTeamRole.RoleID
            //                      select role;

            //permitRoles = documentPermitRoles.ToList<SuRole>();
            //if (permitRoles.Count == 0) return permitRoles;

            //Check PB
            long pbId = 0;
            if (expenseDocument.PB != null)
                pbId = expenseDocument.PB.Pbid;
            IList<SuRole> cashierRoles = QueryProvider.SuRolePBQuery.FindRoleByPBID(pbId);
            var documentPermitRoles = from role in permitRoles
                                      join cashierRole in cashierRoles
                                      on role.RoleID equals cashierRole.RoleID
                                      select role;

            return documentPermitRoles.ToList<SuRole>();
        }
        #endregion

        #region GetAllowRole{EventName}{StateName}

        public virtual IList<SuRole> GetAllowRoleVerifyWaitVerify(long workFlowID)
        {
            return GetPermitRoleVerify(workFlowID);
        }

        public virtual IList<SuRole> GetAllowRoleVerifyAndApproveVerifyWaitVerify(long workFlowID)
        {
            return GetPermitRoleVerifyAndApproveVerify(workFlowID);
        }

        public virtual IList<SuRole> GetAllowRoleRejectWaitVerify(long workFlowID)
        {
            return GetPermitRoleVerify(workFlowID);
        }

        public virtual IList<SuRole> GetAllowRoleHoldWaitVerify(long workFlowID)
        {
            return GetPermitRoleVerify(workFlowID);
        }

        public virtual long GetAllowUserHoldWaitVerify(long workFlowID)
        {
            return GetPermitUserHold(workFlowID);
        }

        public virtual IList<SuRole> GetAllowRoleReceiveWaitVerify(long workFlowID)
        {
            if (!IsReceivedDocument(workFlowID))
                return GetPermitRoleReceive(workFlowID);
            else
                return new List<SuRole>();
        }

        public virtual IList<SuRole> GetAllowRoleApproveWaitApproveVerify(long workFlowID)
        {
            return GetPermitRoleApproveVerify(workFlowID);
        }

        public virtual IList<SuRole> GetAllowRoleRejectWaitApproveVerify(long workFlowID)
        {
            return GetPermitRoleApproveVerify(workFlowID);
        }

        public virtual IList<SuRole> GetAllowRoleReceiveWaitApproveVerify(long workFlowID)
        {
            if (!IsReceivedDocument(workFlowID))
                return GetPermitRoleReceive(workFlowID);
            else
                return new List<SuRole>();
        }

        public virtual long GetAllowUserSendDraft(long workFlowID)
        {
            return GetPermitUserDraft(workFlowID);
        }

        public virtual long GetAllowUserSendHold(long workFlowID)
        {
            return GetPermitUserDraft(workFlowID);
        }

        public virtual IList<SuRole> GetAllowRoleVerifyHold(long workFlowID)
        {
            return GetPermitRoleVerify(workFlowID);
        }

        public virtual IList<SuRole> GetAllowRoleRejectHold(long workFlowID)
        {
            return GetPermitRoleVerify(workFlowID);
        }

        public virtual IList<SuRole> GetAllowRoleReceiveHold(long workFlowID)
        {
            if (!IsReceivedDocument(workFlowID))
                return GetPermitRoleReceive(workFlowID);
            else
                return new List<SuRole>();
        }

        public virtual IList<SuRole> GetAllowRoleUnHoldHold(long workFlowID)
        {
            return GetPermitRoleVerify(workFlowID);
        }

        public virtual IList<SuRole> GetAllowRolePayWaitPayment(long workFlowID)
        {
            return GetPermitRoleCounterCashier(workFlowID);
        }

        public virtual IList<SuRole> GetAllowRoleWithdrawWaitPayment(long workFlowID)
        {
            return GetPermitRoleApproveVerify(workFlowID);
        }

        public virtual IList<SuRole> GetAllowRoleReceiveWaitPayment(long workFlowID)
        {
            if (!IsReceivedDocument(workFlowID))
                return GetPermitRoleReceive(workFlowID);
            else
                return new List<SuRole>();
        }

        public virtual IList<SuRole> GetAllowRoleReceiveComplete(long workFlowID)
        {
            if (!IsReceivedDocument(workFlowID))
                return GetPermitRoleReceive(workFlowID);
            else
                return new List<SuRole>();
        }

        public virtual IList<SuRole> GetAllowRoleReceiveWaitPaymentFromSAP(long workFlowID)
        {
            if (!IsReceivedDocument(workFlowID))
                return GetPermitRoleReceive(workFlowID);
            else
                return new List<SuRole>();
        }

        public virtual IList<SuRole> GetAllowRoleWithdrawWaitPaymentFromSAP(long workFlowID)
        {
            return GetPermitRoleApproveVerify(workFlowID);
        }

        public virtual IList<SuRole> GetAllowRolePayWaitRemittance(long workFlowID)
        {
            return GetPermitRoleCounterCashier(workFlowID);
        }

        public virtual IList<SuRole> GetAllowRoleWithdrawWaitRemittance(long workFlowID)
        {
            return GetPermitRoleApproveVerify(workFlowID);
        }

        public virtual IList<SuRole> GetAllowRoleApproveWaitApproveRejection(long workFlowID)
        {
            return GetPermitRoleApproveVerify(workFlowID);
        }

        public virtual IList<SuRole> GetAllowRoleRejectWaitApproveRejection(long workFlowID)
        {
            return GetPermitRoleApproveVerify(workFlowID);
        }

        public virtual IList<SuRole> GetAllowRoleReceiveWaitApproveRejection(long workFlowID)
        {
            if (!IsReceivedDocument(workFlowID))
                return GetPermitRoleReceive(workFlowID);
            else
                return new List<SuRole>();
        }

        public virtual IList<SuRole> GetAllowRoleReceiveWaitDocument(long workFlowID)
        {
            if (!IsReceivedDocument(workFlowID))
                return GetPermitRoleReceive(workFlowID);
            else
                return new List<SuRole>();
        }

        public virtual IList<SuRole> GetAllowRoleRejectWaitDocument(long workFlowID)
        {
            return GetPermitRoleReceive(workFlowID);
        }

        #endregion


        #region ReCalculatePermissionFor{EventName}{StateName}


        public void ReCalculatePermissionForRejectWaitVerify(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleRejectWaitVerify(workFlowID);
            foreach (SuRole permitRole in permissionRoles)
            {
                WorkFlowStateEventPermission permission = new WorkFlowStateEventPermission();
                permission.WorkFlow = WorkFlowQueryProvider.WorkFlowQuery.FindProxyByIdentity(workFlowID);
                permission.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindProxyByIdentity(workFlowStateEventID);
                permission.RoleID = permitRole.RoleID;
                permission.Active = true;
                permission.CreBy = UserAccount.UserID;
                permission.CreDate = DateTime.Now;
                permission.UpdBy = UserAccount.UserID;
                permission.UpdDate = DateTime.Now;
                permission.UpdPgm = UserAccount.CurrentProgramCode;

                WorkFlowStateEventPermissionService.Save(permission);
            }
        }

        public void ReCalculatePermissionForVerifyWaitVerify(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleVerifyWaitVerify(workFlowID);
            foreach (SuRole permitRole in permissionRoles)
            {
                WorkFlowStateEventPermission permission = new WorkFlowStateEventPermission();
                permission.WorkFlow = WorkFlowQueryProvider.WorkFlowQuery.FindProxyByIdentity(workFlowID);
                permission.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindProxyByIdentity(workFlowStateEventID);
                permission.RoleID = permitRole.RoleID;
                permission.Active = true;
                permission.CreBy = UserAccount.UserID;
                permission.CreDate = DateTime.Now;
                permission.UpdBy = UserAccount.UserID;
                permission.UpdDate = DateTime.Now;
                permission.UpdPgm = UserAccount.CurrentProgramCode;

                WorkFlowStateEventPermissionService.Save(permission);
            }
        }

        public void ReCalculatePermissionForVerifyAndApproveVerifyWaitVerify(long workFlowID, int workFlowStateEventID)
        {
            //TODO : How to get permit roles
            IList<SuRole> permissionRoles = GetAllowRoleVerifyAndApproveVerifyWaitVerify(workFlowID);
            foreach (SuRole permitRole in permissionRoles)
            {
                WorkFlowStateEventPermission permission = new WorkFlowStateEventPermission();
                permission.WorkFlow = WorkFlowQueryProvider.WorkFlowQuery.FindProxyByIdentity(workFlowID);
                permission.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindProxyByIdentity(workFlowStateEventID);
                permission.RoleID = permitRole.RoleID;
                permission.Active = true;
                permission.CreBy = UserAccount.UserID;
                permission.CreDate = DateTime.Now;
                permission.UpdBy = UserAccount.UserID;
                permission.UpdDate = DateTime.Now;
                permission.UpdPgm = UserAccount.CurrentProgramCode;

                WorkFlowStateEventPermissionService.Save(permission);
            }
        }

        public void ReCalculatePermissionForHoldWaitVerify(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleHoldWaitVerify(workFlowID);
            foreach (SuRole permitRole in permissionRoles)
            {
                WorkFlowStateEventPermission permission = new WorkFlowStateEventPermission();
                permission.WorkFlow = WorkFlowQueryProvider.WorkFlowQuery.FindProxyByIdentity(workFlowID);
                permission.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindProxyByIdentity(workFlowStateEventID);
                permission.RoleID = permitRole.RoleID;
                permission.Active = true;
                permission.CreBy = UserAccount.UserID;
                permission.CreDate = DateTime.Now;
                permission.UpdBy = UserAccount.UserID;
                permission.UpdDate = DateTime.Now;
                permission.UpdPgm = UserAccount.CurrentProgramCode;

                WorkFlowStateEventPermissionService.Save(permission);
            }
        }

        public void ReCalculatePermissionForReceiveWaitVerify(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleReceiveWaitVerify(workFlowID);
            foreach (SuRole permitRole in permissionRoles)
            {
                WorkFlowStateEventPermission permission = new WorkFlowStateEventPermission();
                permission.WorkFlow = WorkFlowQueryProvider.WorkFlowQuery.FindProxyByIdentity(workFlowID);
                permission.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindProxyByIdentity(workFlowStateEventID);
                permission.RoleID = permitRole.RoleID;
                permission.Active = true;
                permission.CreBy = UserAccount.UserID;
                permission.CreDate = DateTime.Now;
                permission.UpdBy = UserAccount.UserID;
                permission.UpdDate = DateTime.Now;
                permission.UpdPgm = UserAccount.CurrentProgramCode;

                WorkFlowStateEventPermissionService.Save(permission);
            }
        }

        public void ReCalculatePermissionForApproveWaitApproveVerify(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleApproveWaitApproveVerify(workFlowID);
            foreach (SuRole permitRole in permissionRoles)
            {
                WorkFlowStateEventPermission permission = new WorkFlowStateEventPermission();
                permission.WorkFlow = WorkFlowQueryProvider.WorkFlowQuery.FindProxyByIdentity(workFlowID);
                permission.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindProxyByIdentity(workFlowStateEventID);
                permission.RoleID = permitRole.RoleID;
                permission.Active = true;
                permission.CreBy = UserAccount.UserID;
                permission.CreDate = DateTime.Now;
                permission.UpdBy = UserAccount.UserID;
                permission.UpdDate = DateTime.Now;
                permission.UpdPgm = UserAccount.CurrentProgramCode;

                WorkFlowStateEventPermissionService.Save(permission);
            }
        }

        public void ReCalculatePermissionForRejectWaitApproveVerify(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleRejectWaitApproveVerify(workFlowID);
            foreach (SuRole permitRole in permissionRoles)
            {
                WorkFlowStateEventPermission permission = new WorkFlowStateEventPermission();
                permission.WorkFlow = WorkFlowQueryProvider.WorkFlowQuery.FindProxyByIdentity(workFlowID);
                permission.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindProxyByIdentity(workFlowStateEventID);
                permission.RoleID = permitRole.RoleID;
                permission.Active = true;
                permission.CreBy = UserAccount.UserID;
                permission.CreDate = DateTime.Now;
                permission.UpdBy = UserAccount.UserID;
                permission.UpdDate = DateTime.Now;
                permission.UpdPgm = UserAccount.CurrentProgramCode;

                WorkFlowStateEventPermissionService.Save(permission);
            }
        }


        public void ReCalculatePermissionForHoldWaitApproveVerify(long workFlowID, int workFlowStateEventID)
        {

        }

        public void ReCalculatePermissionForReceiveWaitApproveVerify(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleReceiveWaitApproveVerify(workFlowID);
            foreach (SuRole permitRole in permissionRoles)
            {
                WorkFlowStateEventPermission permission = new WorkFlowStateEventPermission();
                permission.WorkFlow = WorkFlowQueryProvider.WorkFlowQuery.FindProxyByIdentity(workFlowID);
                permission.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindProxyByIdentity(workFlowStateEventID);
                permission.RoleID = permitRole.RoleID;
                permission.Active = true;
                permission.CreBy = UserAccount.UserID;
                permission.CreDate = DateTime.Now;
                permission.UpdBy = UserAccount.UserID;
                permission.UpdDate = DateTime.Now;
                permission.UpdPgm = UserAccount.CurrentProgramCode;

                WorkFlowStateEventPermissionService.Save(permission);
            }
        }

        public void ReCalculatePermissionForVerifyAndApproveVerifyWaitApproveVerify(long workFlowID, int workFlowStateEventID)
        {
            //TODO:How to get permit role on 2 role
        }

        public void ReCalculatePermissionForSendHold(long workFlowID, int workFlowStateEventID)
        {
            long permitUserID = GetAllowUserSendHold(workFlowID);

            WorkFlowStateEventPermission permission = new WorkFlowStateEventPermission();
            permission.WorkFlow = WorkFlowQueryProvider.WorkFlowQuery.FindProxyByIdentity(workFlowID);
            permission.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindProxyByIdentity(workFlowStateEventID);
            permission.UserID = permitUserID;
            permission.Active = true;
            permission.CreBy = UserAccount.UserID;
            permission.CreDate = DateTime.Now;
            permission.UpdBy = UserAccount.UserID;
            permission.UpdDate = DateTime.Now;
            permission.UpdPgm = UserAccount.CurrentProgramCode;

            WorkFlowStateEventPermissionService.Save(permission);

        }

        public void ReCalculatePermissionForVerifyHold(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleVerifyHold(workFlowID);
            foreach (SuRole permitRole in permissionRoles)
            {
                WorkFlowStateEventPermission permission = new WorkFlowStateEventPermission();
                permission.WorkFlow = WorkFlowQueryProvider.WorkFlowQuery.FindProxyByIdentity(workFlowID);
                permission.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindProxyByIdentity(workFlowStateEventID);
                permission.RoleID = permitRole.RoleID;
                permission.Active = true;
                permission.CreBy = UserAccount.UserID;
                permission.CreDate = DateTime.Now;
                permission.UpdBy = UserAccount.UserID;
                permission.UpdDate = DateTime.Now;
                permission.UpdPgm = UserAccount.CurrentProgramCode;

                WorkFlowStateEventPermissionService.Save(permission);
            }
        }

        public void ReCalculatePermissionForRejectHold(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleRejectHold(workFlowID);
            foreach (SuRole permitRole in permissionRoles)
            {
                WorkFlowStateEventPermission permission = new WorkFlowStateEventPermission();
                permission.WorkFlow = WorkFlowQueryProvider.WorkFlowQuery.FindProxyByIdentity(workFlowID);
                permission.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindProxyByIdentity(workFlowStateEventID);
                permission.RoleID = permitRole.RoleID;
                permission.Active = true;
                permission.CreBy = UserAccount.UserID;
                permission.CreDate = DateTime.Now;
                permission.UpdBy = UserAccount.UserID;
                permission.UpdDate = DateTime.Now;
                permission.UpdPgm = UserAccount.CurrentProgramCode;

                WorkFlowStateEventPermissionService.Save(permission);
            }
        }

        public void ReCalculatePermissionForReceiveHold(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleReceiveHold(workFlowID);
            foreach (SuRole permitRole in permissionRoles)
            {
                WorkFlowStateEventPermission permission = new WorkFlowStateEventPermission();
                permission.WorkFlow = WorkFlowQueryProvider.WorkFlowQuery.FindProxyByIdentity(workFlowID);
                permission.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindProxyByIdentity(workFlowStateEventID);
                permission.RoleID = permitRole.RoleID;
                permission.Active = true;
                permission.CreBy = UserAccount.UserID;
                permission.CreDate = DateTime.Now;
                permission.UpdBy = UserAccount.UserID;
                permission.UpdDate = DateTime.Now;
                permission.UpdPgm = UserAccount.CurrentProgramCode;

                WorkFlowStateEventPermissionService.Save(permission);
            }
        }

        public void ReCalculatePermissionForPayWaitPayment(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetAllowRolePayWaitPayment(workFlowID);
            foreach (SuRole permitRole in permissionRoles)
            {
                WorkFlowStateEventPermission permission = new WorkFlowStateEventPermission();
                permission.WorkFlow = WorkFlowQueryProvider.WorkFlowQuery.FindProxyByIdentity(workFlowID);
                permission.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindProxyByIdentity(workFlowStateEventID);
                permission.RoleID = permitRole.RoleID;
                permission.Active = true;
                permission.CreBy = UserAccount.UserID;
                permission.CreDate = DateTime.Now;
                permission.UpdBy = UserAccount.UserID;
                permission.UpdDate = DateTime.Now;
                permission.UpdPgm = UserAccount.CurrentProgramCode;

                WorkFlowStateEventPermissionService.Save(permission);
            }
        }

        public void ReCalculatePermissionForWithdrawWaitPayment(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleWithdrawWaitPayment(workFlowID);
            foreach (SuRole permitRole in permissionRoles)
            {
                WorkFlowStateEventPermission permission = new WorkFlowStateEventPermission();
                permission.WorkFlow = WorkFlowQueryProvider.WorkFlowQuery.FindProxyByIdentity(workFlowID);
                permission.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindProxyByIdentity(workFlowStateEventID);
                permission.RoleID = permitRole.RoleID;
                permission.Active = true;
                permission.CreBy = UserAccount.UserID;
                permission.CreDate = DateTime.Now;
                permission.UpdBy = UserAccount.UserID;
                permission.UpdDate = DateTime.Now;
                permission.UpdPgm = UserAccount.CurrentProgramCode;

                WorkFlowStateEventPermissionService.Save(permission);
            }
        }

        public void ReCalculatePermissionForReceiveWaitPayment(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetAllowRolePayWaitPayment(workFlowID);
            foreach (SuRole permitRole in permissionRoles)
            {
                WorkFlowStateEventPermission permission = new WorkFlowStateEventPermission();
                permission.WorkFlow = WorkFlowQueryProvider.WorkFlowQuery.FindProxyByIdentity(workFlowID);
                permission.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindProxyByIdentity(workFlowStateEventID);
                permission.RoleID = permitRole.RoleID;
                permission.Active = true;
                permission.CreBy = UserAccount.UserID;
                permission.CreDate = DateTime.Now;
                permission.UpdBy = UserAccount.UserID;
                permission.UpdDate = DateTime.Now;
                permission.UpdPgm = UserAccount.CurrentProgramCode;

                WorkFlowStateEventPermissionService.Save(permission);
            }
        }

        public virtual void ReCalculatePermissionForPayWaitPaymentFromSAP(long workFlowID, int workFlowStateEventID)
        {
            //not do anything
        }

        public void ReCalculatePermissionForReceiveWaitPaymentFromSAP(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleReceiveWaitPaymentFromSAP(workFlowID);
            foreach (SuRole permitRole in permissionRoles)
            {
                WorkFlowStateEventPermission permission = new WorkFlowStateEventPermission();
                permission.WorkFlow = WorkFlowQueryProvider.WorkFlowQuery.FindProxyByIdentity(workFlowID);
                permission.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindProxyByIdentity(workFlowStateEventID);
                permission.RoleID = permitRole.RoleID;
                permission.Active = true;
                permission.CreBy = UserAccount.UserID;
                permission.CreDate = DateTime.Now;
                permission.UpdBy = UserAccount.UserID;
                permission.UpdDate = DateTime.Now;
                permission.UpdPgm = UserAccount.CurrentProgramCode;

                WorkFlowStateEventPermissionService.Save(permission);
            }
        }

        public void ReCalculatePermissionForPayWaitRemittance(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetAllowRolePayWaitRemittance(workFlowID);
            foreach (SuRole permitRole in permissionRoles)
            {
                WorkFlowStateEventPermission permission = new WorkFlowStateEventPermission();
                permission.WorkFlow = WorkFlowQueryProvider.WorkFlowQuery.FindProxyByIdentity(workFlowID);
                permission.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindProxyByIdentity(workFlowStateEventID);
                permission.RoleID = permitRole.RoleID;
                permission.Active = true;
                permission.CreBy = UserAccount.UserID;
                permission.CreDate = DateTime.Now;
                permission.UpdBy = UserAccount.UserID;
                permission.UpdDate = DateTime.Now;
                permission.UpdPgm = UserAccount.CurrentProgramCode;

                WorkFlowStateEventPermissionService.Save(permission);
            }
        }

        public void ReCalculatePermissionForWithdrawWaitRemittance(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleWithdrawWaitRemittance(workFlowID);
            foreach (SuRole permitRole in permissionRoles)
            {
                WorkFlowStateEventPermission permission = new WorkFlowStateEventPermission();
                permission.WorkFlow = WorkFlowQueryProvider.WorkFlowQuery.FindProxyByIdentity(workFlowID);
                permission.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindProxyByIdentity(workFlowStateEventID);
                permission.RoleID = permitRole.RoleID;
                permission.Active = true;
                permission.CreBy = UserAccount.UserID;
                permission.CreDate = DateTime.Now;
                permission.UpdBy = UserAccount.UserID;
                permission.UpdDate = DateTime.Now;
                permission.UpdPgm = UserAccount.CurrentProgramCode;

                WorkFlowStateEventPermissionService.Save(permission);
            }
        }

        public void ReCalculatePermissionForApproveWaitApproveRejection(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleApproveWaitApproveRejection(workFlowID);
            foreach (SuRole permitRole in permissionRoles)
            {
                WorkFlowStateEventPermission permission = new WorkFlowStateEventPermission();
                permission.WorkFlow = WorkFlowQueryProvider.WorkFlowQuery.FindProxyByIdentity(workFlowID);
                permission.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindProxyByIdentity(workFlowStateEventID);
                permission.RoleID = permitRole.RoleID;
                permission.Active = true;
                permission.CreBy = UserAccount.UserID;
                permission.CreDate = DateTime.Now;
                permission.UpdBy = UserAccount.UserID;
                permission.UpdDate = DateTime.Now;
                permission.UpdPgm = UserAccount.CurrentProgramCode;

                WorkFlowStateEventPermissionService.Save(permission);
            }
        }

        public void ReCalculatePermissionForRejectWaitApproveRejection(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleRejectWaitApproveVerify(workFlowID);
            foreach (SuRole permitRole in permissionRoles)
            {
                WorkFlowStateEventPermission permission = new WorkFlowStateEventPermission();
                permission.WorkFlow = WorkFlowQueryProvider.WorkFlowQuery.FindProxyByIdentity(workFlowID);
                permission.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindProxyByIdentity(workFlowStateEventID);
                permission.RoleID = permitRole.RoleID;
                permission.Active = true;
                permission.CreBy = UserAccount.UserID;
                permission.CreDate = DateTime.Now;
                permission.UpdBy = UserAccount.UserID;
                permission.UpdDate = DateTime.Now;
                permission.UpdPgm = UserAccount.CurrentProgramCode;

                WorkFlowStateEventPermissionService.Save(permission);
            }
        }

        public void ReCalculatePermissionForReceiveWaitApproveRejection(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleReceiveWaitApproveRejection(workFlowID);
            foreach (SuRole permitRole in permissionRoles)
            {
                WorkFlowStateEventPermission permission = new WorkFlowStateEventPermission();
                permission.WorkFlow = WorkFlowQueryProvider.WorkFlowQuery.FindProxyByIdentity(workFlowID);
                permission.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindProxyByIdentity(workFlowStateEventID);
                permission.RoleID = permitRole.RoleID;
                permission.Active = true;
                permission.CreBy = UserAccount.UserID;
                permission.CreDate = DateTime.Now;
                permission.UpdBy = UserAccount.UserID;
                permission.UpdDate = DateTime.Now;
                permission.UpdPgm = UserAccount.CurrentProgramCode;

                WorkFlowStateEventPermissionService.Save(permission);
            }
        }

        public void ReCalculatePermissionForReceiveWaitDocument(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleReceiveWaitDocument(workFlowID);
            foreach (SuRole permitRole in permissionRoles)
            {
                WorkFlowStateEventPermission permission = new WorkFlowStateEventPermission();
                permission.WorkFlow = WorkFlowQueryProvider.WorkFlowQuery.FindProxyByIdentity(workFlowID);
                permission.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindProxyByIdentity(workFlowStateEventID);
                permission.RoleID = permitRole.RoleID;
                permission.Active = true;
                permission.CreBy = UserAccount.UserID;
                permission.CreDate = DateTime.Now;
                permission.UpdBy = UserAccount.UserID;
                permission.UpdDate = DateTime.Now;
                permission.UpdPgm = UserAccount.CurrentProgramCode;

                WorkFlowStateEventPermissionService.Save(permission);
            }
        }

        public void ReCalculatePermissionForReceiveComplete(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleReceiveComplete(workFlowID);
            foreach (SuRole permitRole in permissionRoles)
            {
                WorkFlowStateEventPermission permission = new WorkFlowStateEventPermission();
                permission.WorkFlow = WorkFlowQueryProvider.WorkFlowQuery.FindProxyByIdentity(workFlowID);
                permission.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindProxyByIdentity(workFlowStateEventID);
                permission.RoleID = permitRole.RoleID;
                permission.Active = true;
                permission.CreBy = UserAccount.UserID;
                permission.CreDate = DateTime.Now;
                permission.UpdBy = UserAccount.UserID;
                permission.UpdDate = DateTime.Now;
                permission.UpdPgm = UserAccount.CurrentProgramCode;

                WorkFlowStateEventPermissionService.Save(permission);
            }
        }

        public void ReCalculatePermissionForRejectWaitDocument(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleRejectWaitDocument(workFlowID);
            foreach (SuRole permitRole in permissionRoles)
            {
                WorkFlowStateEventPermission permission = new WorkFlowStateEventPermission();
                permission.WorkFlow = WorkFlowQueryProvider.WorkFlowQuery.FindProxyByIdentity(workFlowID);
                permission.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindProxyByIdentity(workFlowStateEventID);
                permission.RoleID = permitRole.RoleID;
                permission.Active = true;
                permission.CreBy = UserAccount.UserID;
                permission.CreDate = DateTime.Now;
                permission.UpdBy = UserAccount.UserID;
                permission.UpdDate = DateTime.Now;
                permission.UpdPgm = UserAccount.CurrentProgramCode;

                WorkFlowStateEventPermissionService.Save(permission);
            }
        }


        #endregion

        #region Protected Method

        protected void ValidateHoldResponse(HoldDetailResponse holdDetailResponse)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (holdDetailResponse.HoldFields.Count == 0)
            {
                errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("WorkFlow_ValidateHoldResponse_HoldFieldRequired"));
            }

            if (!errors.IsEmpty) throw new SS.Standard.Utilities.ServiceValidationException(errors);
        }

        protected void ValidateReceiveResponse(ReceiveResponse receiveResponse)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(receiveResponse.BoxID))
            {
                errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("WorkFlow_ValidateReceiveResponse_BoxID_Required"));
            }
            else if (receiveResponse.BoxID.Length != 15)
            {
                errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("WorkFlow_ValidateReceiveResponse_BoxID_MaxLength_15"));
            }

            if (!errors.IsEmpty) throw new SS.Standard.Utilities.ServiceValidationException(errors);
        }

        protected void ValidateExpenseRemittanceDetailResponse(ExpenseRemittanceDetailResponse response)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (response.ReceivedMethod.Equals("Bank"))
            {
                if (string.IsNullOrEmpty(response.GLAccount))
                {
                    errors.AddError("Workflow.Error", new Spring.Validation.ErrorMessage("WorkFlow_ValidateExpenseRemittanceDetailResponse_GLAcc_Required"));
                }
                if (response.ValueDate == null)
                {
                    errors.AddError("Workflow.Error", new Spring.Validation.ErrorMessage("WorkFlow_ValidateExpenseRemittanceDetailResponse_ValueDate_Required"));
                }
            }

            if (!errors.IsEmpty) throw new SS.Standard.Utilities.ServiceValidationException(errors);
        }

        public override double GetTotalDocumentAmount(long documentID)
        {
            return 0;
        }

        private bool IsReceivedDocument(long workFlowID)
        {
            //ถ้าเคย Receive ไปแล้วไม่สามารถ Receive ได้อีก
            //IList<WorkFlowResponse> workFlowResponses = WorkFlowQueryProvider.WorkFlowResponseQuery.FindByWorkFlowID(workFlowID);
            //var responses = from r in workFlowResponses
            //                where r.WorkFlowStateEvent.Name == "Receive"
            //                select r;
            //workFlowResponses = responses.ToList<WorkFlowResponse>();

            //return workFlowResponses.Count > 0;

            Document document = WorkFlowQueryProvider.WorkFlowQuery.GetDocumentByWorkFlowID(workFlowID);
            string boxID = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetBoxIDByDocuemntID(document.DocumentID);

            if (!string.IsNullOrEmpty(boxID))
                return true;
            else
                return false;
        }
        #endregion
    }
}

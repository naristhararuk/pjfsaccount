using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Context.Support;

using SS.Standard.Data.NHibernate.Service;
using SS.Standard.WorkFlow.DAL;
using SS.Standard.WorkFlow.Query;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.Service;
using SS.Standard.WorkFlow.EventUserControl;
using SS.Standard.WorkFlow.DTO.ValueObject;
using SS.Standard.Security;
using SS.Standard.CommunicationService;

using SCG.eAccounting.DTO;
using SCG.eAccounting.BLL.WorkFlowService;
using SCG.eAccounting.Query;

using SS.SU.DTO;
using SS.SU.Query;
using SS.Standard.Utilities;
using SCG.eAccounting.DAL;
using SS.DB.Query;
using SCG.DB.Query;
using SCG.eAccounting.SAP.BAPI.Service;
using SCG.eAccounting.SAP.BAPI.Service.Const;
using SCG.eAccounting.SAP.BAPI.Service.Posting;

namespace SCG.eAccounting.BLL.Implement.WorkFlowService
{
    public class AdvanceWorkFlowService : ExpenseWorkFlowService, IAdvanceWorkFlowService
    {

        public ParameterServices ParameterServices { get; set; }


        #region Can{EventName}{StateName}

        #region CanSendDraft
        public override bool CanSendDraft(long workFlowID)
        {
            //If Advance Document was created from TA, then cannot send this advance.
            //The way to check advance document was created from TA is 
            //TA's current state that referenced in advance does not complete
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            AvAdvanceDocument advanceDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(workFlow.Document.DocumentID);
            if (advanceDocument.TADocumentID.HasValue)
            {
                TADocument taDocument = ScgeAccountingQueryProvider.TADocumentQuery.FindByIdentity(advanceDocument.TADocumentID.Value);
                WorkFlow taWorkFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(taDocument.DocumentID.DocumentID);
                if (taWorkFlow.CurrentState.Name != "Complete")
                    return false;
            }
            return base.CanSendDraft(workFlowID);

        }
        #endregion


        #region CanApproveFromTADraft
        //public bool CanApproveFromTADraft(long workFlowID)
        //{
        //    long permitUserID = GetAllowUserApproveFromTADraft(workFlowID);
        //    return UserAccount.UserID == permitUserID;

        //}
        #endregion

        #region CanApproveWaitTA
        public bool CanApproveWaitTA(long workFlowID)
        {
            //Must be called from Internal Only
            return false;
        }
        #endregion

        #region CanWithdrawWaitTA
        public bool CanWithdrawWaitTA(long workFlowID)
        {
            //Must be called from Internal Only
            return false;
        }
        #endregion

        #region CanWithdrawWaitPaymentFromSAP
        // Use method on ExpenseWorkFlowService
        #endregion

        #region CanClearingOutstanding
        public bool CanClearingOutstanding(long workFlowID)
        {
            return false;
        }
        #endregion

        #endregion

        #region On{EventName}{StateName}

        #region override OnSendDraft
        public override string OnSendDraft(long workFlowID, object eventData)
        {
            //เพราะว่า CanSendDraft จะ return false ถ้า Advance นั้นเกิดจาก TA ที่ยังไม่ Complate ดังนั้น ไม่ต้อง Validate ที่นี่อีก
            //Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            //WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            //AvAdvanceDocument advanceDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(workFlow.Document.DocumentID);
            //if (advanceDocument.TADocumentID != null && advanceDocument.TADocumentID != 0)
            //{
            //    TADocument taDocument = ScgeAccountingQueryProvider.TADocumentQuery.FindByIdentity(advanceDocument.TADocumentID.Value);
            //    WorkFlow taWorkFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(taDocument.DocumentID.DocumentID);
            //    if (taWorkFlow.CurrentState.WorkFlowStateID != TaConstant.TaCompleteStateID)
            //        errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("WorkFlow_OnSendDraft_CannotSubmitAdvance"));
            //}

            //if (!errors.IsEmpty) throw new SS.Standard.Utilities.ServiceValidationException(errors);


            string signal = base.OnSendDraft(workFlowID, eventData);

            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            AvAdvanceDocument advanceDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(workFlow.Document.DocumentID);
            if (advanceDocument.TADocumentID != null && advanceDocument.TADocumentID != 0)
            {
                TADocument taDocument = ScgeAccountingQueryProvider.TADocumentQuery.FindByIdentity(advanceDocument.TADocumentID.Value);
                WorkFlow taWorkFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(taDocument.DocumentID.DocumentID);

                // Comment for step advance from SendDraftToWaitApprove
                // Old version is SendDraftToWaitTA
                if (taWorkFlow.CurrentState.WorkFlowStateID != TaConstant.TaCompleteStateID &&
                        signal != "SendDraftToWaitAR")
                {
                    //signal = "SendDraftToWaitTA";
                    IList<DocumentInitiator> initiators = ScgeAccountingQueryProvider.DocumentInitiatorQuery.GetNotResponseInitiatorByDocumentID(workFlow.Document.DocumentID);
                    if (initiators.Count > 0)
                        signal = "SendDraftToWaitTAInitial";
                    else
                        signal = "SendDraftToWaitTAApprove";
                }
            }


            return signal;
        }
        #endregion

        #region OnApproveWaitAR
        public override string OnApproveWaitAR(long workFlowID, object eventData)
        {
            WorkFlow workFlow;
            try
            {
                SubmitResponse submitResponse = eventData as SubmitResponse;

                workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

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

            /*  TODO generate signal string
             *  Case 1 : ApproveWaitARToWaitInitial
             *  Case 2 : ApproveWaitARToWaitApprove
             *  [CANCEL] Case 3 : ApproveWaitARToWaitTA (if Advance was created from TA)
             *  Case 3 : ApproveWaitARToWaitTAInitial (if Advance was created from TA)
             *  Case 4 : ApproveWaitARToWaitTAApprove (if Advance was created from TA)
             */
            string signal = string.Empty;

            AvAdvanceDocument advanceDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(workFlow.Document.DocumentID);
            if (advanceDocument.TADocumentID != null && advanceDocument.TADocumentID != 0)
            {
                TADocument taDocument = ScgeAccountingQueryProvider.TADocumentQuery.FindByIdentity(advanceDocument.TADocumentID.Value);
                WorkFlow taWorkFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(taDocument.DocumentID.DocumentID);
                if (taWorkFlow.CurrentState.WorkFlowStateID != TaConstant.TaCompleteStateID)
                {
                    //signal = "ApproveWaitARToWaitTA";
                    IList<DocumentInitiator> initiators = ScgeAccountingQueryProvider.DocumentInitiatorQuery.GetNotResponseInitiatorByDocumentID(workFlow.Document.DocumentID);
                    if (initiators.Count > 0)
                        signal = "ApproveWaitARToWaitTAInitial";
                    else
                        signal = "ApproveWaitARToWaitTAApprove";
                }
            }

            if (String.IsNullOrEmpty(signal))
            {
                IList<DocumentInitiator> initiators = ScgeAccountingQueryProvider.DocumentInitiatorQuery.GetNotResponseInitiatorByDocumentID(workFlow.Document.DocumentID);
                if (initiators.Count > 0)
                    signal = "ApproveWaitARToWaitInitial";
                else
                    signal = "ApproveWaitARToWaitApprove";
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
            int workFlowStateEventId;
            WorkFlowResponse response = new WorkFlowResponse();

            AvAdvanceDocument advDoc = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(scgDocument.DocumentID);

            try
            {
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

                    if (rejectDetailResponse.ResponseMethod != null)
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
                    if (submitResponse.ResponseMethod != null)
                        response.ResponseMethod = submitResponse.ResponseMethod.GetHashCode().ToString();

                    response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(workFlowStateEventId);
                    WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);
                }

                //update approve date
                scgDocument.ApproveDate = DateTime.Now;
                scgDocument.UpdBy = UserAccount.UserID;
                scgDocument.UpdDate = DateTime.Now;
                scgDocument.UpdPgm = UserAccount.CurrentProgramCode;
                SCGDocumentService.SaveOrUpdate(scgDocument);

                //save request date of advance when approved
                advDoc.RequestDateOfAdvanceApproved = advDoc.RequestDateOfAdvance;
                advDoc.UpdBy = UserAccount.UserID;
                advDoc.UpdDate = DateTime.Now;
                advDoc.UpdPgm = UserAccount.CurrentProgramCode;
                AvAdvanceDocumentService.SaveOrUpdate(advDoc);

                /* // comment old code ที่ยังไม่ต้อง save reason approve
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

                long sendToUserID = 0;
                IList<long> ccList = new List<long>();

                if (ParameterServices.EnableEmail02Creator || ParameterServices.EnableEmail02Requester || ParameterServices.EnableEmail02Initiator)
                {
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
             *  Case 1 : ApproveWaitApproveToWaitVerify
             */

            string signal = "ApproveWaitApproveToWaitVerify";



            return signal;
        }
        #endregion

        #region OnApproveWaitApproveVerify
        public override string OnApproveWaitApproveVerify(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

            //Validate Posting Status
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            AvAdvanceDocument advanceDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(workFlow.Document.DocumentID);

            if (scgDocument.PostingStatus != PostingStaus.Complete)
            {
                AdvancePostingService postingService = new AdvancePostingService();
                IList<BAPIApproveReturn> bapiReturn = postingService.BAPIApprove(scgDocument.DocumentID, DocumentKind.Advance.ToString(), UserAccount.UserID);

                if (postingService.GetDocumentStatus(scgDocument.DocumentID, DocumentKind.Advance.ToString()) == "A")
                {
                    scgDocument.PostingStatus = "C";
                    SCGDocumentService.SaveOrUpdate(scgDocument);
                }
                else if (postingService.GetDocumentStatus(scgDocument.DocumentID, DocumentKind.Advance.ToString()) == "PP")
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
            }
            catch (Exception)
            {
                throw;
            }

            scgDocument.ApproveVerifiedDate = DateTime.Now;
            SCGDocumentService.SaveOrUpdate(scgDocument);

            /*  TODO generate signal string
             *  Case 1 (Domestic PaymentType = Cash Or Foreign Advance): ApproveWaitApproveVerifyToWaitPayment
             *  Case 2 (Domestic PaymenyType != Cash): ApproveWaitApproveVerifyToWaitPaymentFromSAP
             */
            //AvAdvanceDocument advanceDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(workFlow.Document.DocumentID);
            string signal = "ApproveWaitApproveVerifyToWaitPayment";
            IList<AvAdvanceItem> advanceItems = ScgeAccountingQueryProvider.AvAdvanceItemQuery.FindByAvAdvanceItemAdvanceID(advanceDocument.AdvanceID);
            if (advanceDocument.AdvanceType == ZoneType.Domestic)
            {
                if (advanceItems.Count > 0 && advanceItems[0].PaymentType != PaymentType.CA)
                {
                    signal = "ApproveWaitApproveVerifyToWaitPaymentFromSAP";
                }
                else
                {
                    signal = "ApproveWaitApproveVerifyToWaitPayment";

                    IList<long> ccList = new List<long>();
                    if (scgDocument.CreatorID.Userid != scgDocument.ReceiverID.Userid)
                        ccList.Add(scgDocument.CreatorID.Userid);
                    if (scgDocument.RequesterID.Userid != scgDocument.ReceiverID.Userid)
                        ccList.Add(scgDocument.RequesterID.Userid);

                    SCGEmailService.SendEmailEM05(workFlowID, scgDocument.ReceiverID.Userid, ccList, false);

                    IList<long> ReciverList = new List<long>();
                    ReciverList.Add(scgDocument.ReceiverID.Userid);
                    SCGSMSService.SendSMS02(workFlowID, scgDocument.RequesterID.Userid.ToString(), ReciverList, false);

                }
            }
            else
            {
                signal = "ApproveWaitApproveVerifyToWaitPayment";

                IList<long> ccList = new List<long>();
                if (scgDocument.CreatorID.Userid != scgDocument.ReceiverID.Userid)
                    ccList.Add(scgDocument.CreatorID.Userid);
                if (scgDocument.RequesterID.Userid != scgDocument.ReceiverID.Userid)
                    ccList.Add(scgDocument.RequesterID.Userid);

                SCGEmailService.SendEmailEM05(workFlowID, scgDocument.ReceiverID.Userid, ccList, false);
                IList<long> ReciverList = new List<long>();
                ReciverList.Add(scgDocument.ReceiverID.Userid);
                SCGSMSService.SendSMS02(workFlowID, scgDocument.RequesterID.Userid.ToString(), ReciverList, false);

            }

            return signal;
        }
        #endregion

        #region OnVerifyAndApproveVerifyWaitVerify
        public override string OnVerifyAndApproveVerifyWaitVerify(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

            //Validate Posting Status
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            if (document.PostingStatus != PostingStaus.Complete)
            {
                errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("Workflow_OnVerifyAndApproveVerifyWaitVerify_Mismatch_PostingStatus"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            SubmitResponse submitResponse = eventData as SubmitResponse;
            try
            {
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

            document.VerifiedDate = DateTime.Now;
            document.ApproveVerifiedDate = DateTime.Now;
            SCGDocumentService.SaveOrUpdate(document);

            /*  TODO generate signal string
             *  Case 1 : VerifyAndApproveVerifyWaitVerifyToWaitPayment
             *  Case 2 : VerifyAndApproveVerifyWaitVerifyToWaitPaymentFromSAP
             */
            string signal = "";

            AvAdvanceDocument advanceDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(workFlow.Document.DocumentID);
            if (advanceDocument.AdvanceType == ZoneType.Foreign)
            {
                signal = "VerifyAndApproveVerifyWaitVerifyToWaitPayment";
            }
            else if (advanceDocument.AdvanceType == ZoneType.Domestic)
            {
                IList<AvAdvanceItem> advanceItems = ScgeAccountingQueryProvider.AvAdvanceItemQuery.FindByAvAdvanceItemAdvanceID(advanceDocument.AdvanceID);
                if (advanceItems.Count > 0)
                {
                    if (advanceItems[0].PaymentType == PaymentType.CA)
                    {
                        signal = "VerifyAndApproveVerifyWaitVerifyToWaitPayment";
                    }
                    else if (advanceItems[0].PaymentType == PaymentType.CQ || advanceItems[0].PaymentType == PaymentType.TR)
                    {
                        signal = "VerifyAndApproveVerifyWaitVerifyToWaitPaymentFromSAP";
                    }
                }
            }


            return signal;
        }
        #endregion

        #region OnPayWaitPayment
        public override string OnPayWaitPayment(long workFlowID, object eventData)
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
            string signal = "PayWaitPaymentToOutstanding";

            return signal;
        }
        #endregion

        #region OnWithdrawWaitPaymentFromSAP
        // Use method on ExpenseWorkFlowService
        #endregion

        #region OnPayWaitPaymentFromSAP
        public override string OnPayWaitPaymentFromSAP(long workFlowID, object eventData)
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
            string signal = "PayWaitPaymentFromSAPToOutstanding";

            AvAdvanceDocument advanceDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(scgDocument.DocumentID);
            IList<AvAdvanceItem> advanceItems = ScgeAccountingQueryProvider.AvAdvanceItemQuery.FindByAvAdvanceItemAdvanceID(advanceDocument.AdvanceID);

            IList<long> ccList = new List<long>();

            if (advanceDocument.AdvanceType == ZoneType.Domestic)
            {
                if (advanceItems.Count > 0 && advanceItems[0].PaymentType == PaymentType.CQ)
                {
                    SCGEmailService.SendEmailEM05(workFlowID, scgDocument.ReceiverID.Userid, ccList, true);
                    //#1
                    // (ParameterServices.EnableSMS)
                    // {
                    IList<long> ReciverList = new List<long>();
                    ReciverList.Add(scgDocument.ReceiverID.Userid);
                    SCGSMSService.SendSMS02(workFlowID, scgDocument.RequesterID.Userid.ToString(), ReciverList, true);
                    // }

                }
                else if (advanceItems.Count > 0 && advanceItems[0].PaymentType == PaymentType.TR)
                {
                    if (scgDocument.CreatorID.Userid != scgDocument.ReceiverID.Userid)
                        ccList.Add(scgDocument.CreatorID.Userid);
                    if (scgDocument.CreatorID.Userid != scgDocument.RequesterID.Userid)
                        ccList.Add(scgDocument.RequesterID.Userid);

                    SCGEmailService.SendEmailEM06(workFlowID, scgDocument.ReceiverID.Userid, ccList);

                    //#2
                    // if (ParameterServices.EnableSMS)
                    //{
                    IList<long> ReciverList = new List<long>();
                    ReciverList.Add(scgDocument.ReceiverID.Userid);
                    SCGSMSService.SendSMS03(workFlowID, scgDocument.RequesterID.Userid.ToString(), ReciverList);
                    //}
                }
            }


            return signal;
        }
        #endregion

        #region OnClearingOutstanding
        public string OnClearingOutstanding(long workFlowID, object eventData)
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
            string signal = "ClearingOutstandingToComplete";

            return signal;
        }
        #endregion

        #region OnApproveFromTADraft
        // Not use , use OnApproveWaitTAToWaitVerify
        //public string OnApproveFromTADraft(long workFlowID, object eventData)
        //{
        //    /*TODO validate , save event data to workflow*/
        //    try
        //    {
        //        SubmitResponse submitResponse = eventData as SubmitResponse;

        //        WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
        //        SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);


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

        //        if (string.IsNullOrEmpty(scgDocument.DocumentNo))
        //        {
        //            //TODO : Generate Document No and Save it
        //            int year = DateTime.Now.Year;

        //            scgDocument.DocumentNo = DbDocumentRunningService.RetrieveNextDocumentRunningNo(year, scgDocument.DocumentType.DocumentTypeID, scgDocument.CompanyID.CompanyID);
        //            scgDocument.DocumentDate = DateTime.Now;
        //            ScgeAccountingDaoProvider.SCGDocumentDao.Update(scgDocument);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    /*TODO generate signal string*/
        //    string signal = "ApproveFromTADraftToWaitVerify";

        //    return signal;
        //}
        #endregion

        #region OnApproveWaitTA
        public string OnApproveWaitTA(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            try
            {
                SubmitResponse submitResponse = eventData as SubmitResponse;

                WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
                SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);


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
            /*TODO generate signal string*/
            string signal = "ApproveWaitTAToWaitVerify";

            return signal;
        }
        #endregion

        #region OnWithDrawWaitTA
        public string OnWithdrawWaitTA(long workFlowID, object eventData)
        {
            try
            {
                SubmitResponse submitResponse = eventData as SubmitResponse;

                WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
                SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);


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
            string signal = "WithdrawWaitTAToDraft";

            return signal;
        }
        #endregion

        #region OnApproveWaitTAInitial
        public string OnApproveWaitTAInitial(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            string signal = string.Empty;
            try
            {
                //object[] submitResponse = eventData as object[];

                //WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

                //WorkFlowResponse response = new WorkFlowResponse();
                //response.WorkFlow = workFlow;
                //response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(int.Parse(submitResponse[0].ToString()));
                //response.ResponseBy = UserAccount.UserID;
                //response.ResponseDate = DateTime.Now;
                //if (submitResponse[1] != null)
                //    response.ResponseMethod = submitResponse[1].GetHashCode().ToString();
                //response.Active = true;
                //response.CreBy = UserAccount.UserID;
                //response.CreDate = DateTime.Now;
                //response.UpdBy = UserAccount.UserID;
                //response.UpdDate = DateTime.Now;
                //response.UpdPgm = UserAccount.CurrentProgramCode;
                //WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);

                //if (int.Parse(submitResponse[0].ToString()) == 1)
                //    signal = "ApproveWaitTAInitialToWaitTAInitial";
                //else if (int.Parse(submitResponse[0].ToString()) == 2)
                //    signal = "ApproveWaitTAInitialToWaitTAApprove";

                signal = base.OnApproveWaitInitial(workFlowID, eventData);
                if (signal.Equals("ApproveWaitInitialToWaitInitial"))
                    signal = "ApproveWaitTAInitialToWaitTAInitial";
                else if (signal.Equals("ApproveWaitInitialToWaitApprove"))
                    signal = "ApproveWaitTAInitialToWaitTAApprove";
            }
            catch (Exception)
            {
                throw;
            }
            /*TODO generate signal string*/
            return signal;
        }
        #endregion

        #region OnApproveWaitTAApprove
        public string OnApproveWaitTAApprove(long workFlowID, object eventData)
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
                if (submitResponse.ResponseMethod != null)
                    response.ResponseMethod = submitResponse.ResponseMethod.GetHashCode().ToString();
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);

                //update approve date
                scgDocument.ApproveDate = DateTime.Now;
                scgDocument.UpdBy = UserAccount.UserID;
                scgDocument.UpdDate = DateTime.Now;
                scgDocument.UpdPgm = UserAccount.CurrentProgramCode;
                SCGDocumentService.SaveOrUpdate(scgDocument);

                //save request date of advance when approved
                AvAdvanceDocument advDoc = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(scgDocument.DocumentID);
                advDoc.RequestDateOfAdvanceApproved = advDoc.RequestDateOfAdvance;
                advDoc.UpdBy = UserAccount.UserID;
                advDoc.UpdDate = DateTime.Now;
                advDoc.UpdPgm = UserAccount.CurrentProgramCode;
                AvAdvanceDocumentService.SaveOrUpdate(advDoc);

            }
            catch (Exception)
            {
                throw;
            }
            /*  TODO generate signal string
             *  Case 1 : ApproveWaitApproveToWaitVerify
             */
            return "ApproveWaitTAApproveToWaitVerify";
        }
        #endregion

        #region OnRejectWaitTAInitial
        public string OnRejectWaitTAInitial(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);

            try
            {
                RejectDetailResponse rejectDetailResponse;

                if (eventData is SubmitResponse)
                    rejectDetailResponse = DefaultRejectDetailResponse((eventData as SubmitResponse).WorkFlowStateEventID, (eventData as SubmitResponse).ResponseMethod);
                else
                    rejectDetailResponse = eventData as RejectDetailResponse;

                ValidateRejectResponse(rejectDetailResponse);

                WorkFlowResponse response = new WorkFlowResponse();
                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(rejectDetailResponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                if (rejectDetailResponse.ResponseMethod != null)
                    response.ResponseMethod = rejectDetailResponse.ResponseMethod.GetHashCode().ToString();
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
            }
            catch (Exception)
            {
                throw;
            }

            /*TODO generate signal string*/
            return "RejectWaitTAInitialToDraft";
        }
        #endregion

        #region OnRejectWaitTAApprove
        public string OnRejectWaitTAApprove(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);

            try
            {
                RejectDetailResponse rejectDetailResponse;

                if (eventData is SubmitResponse)
                    rejectDetailResponse = DefaultRejectDetailResponse((eventData as SubmitResponse).WorkFlowStateEventID, (eventData as SubmitResponse).ResponseMethod);
                else
                    rejectDetailResponse = eventData as RejectDetailResponse;

                ValidateRejectResponse(rejectDetailResponse);

                WorkFlowResponse response = new WorkFlowResponse();
                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(rejectDetailResponse.WorkFlowStateEventID);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                if (rejectDetailResponse.ResponseMethod != null)
                    response.ResponseMethod = rejectDetailResponse.ResponseMethod.GetHashCode().ToString();
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
            }
            catch (Exception)
            {
                throw;
            }

            /*TODO generate signal string*/
            return "RejectWaitTAApproveToDraft";
        }
        #endregion

        #region OnWithdrawWaitInitial
        public string OnWithdrawWaitTAInitial(long workFlowID, object eventData)
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
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);
            }
            catch (Exception)
            {
                throw;
            }
            /*TODO generate signal string*/
            return "WithdrawWaitTAInitialToDraft";
        }
        #endregion

        #region OnOverRoleWaitTAInitial
        public string OnOverRoleWaitTAInitial(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow;
            try
            {
                SubmitResponse submitResponse = eventData as SubmitResponse;

                workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);


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
            /*  TODO generate signal string
             *  Case 1 : OverRoleWaitInitialToWaitInitial
             *  Case 2 : OverRoleWaitInitialToWaitApprove
             */
            string signal;
            AvAdvanceDocument avDoc = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(workFlow.Document.DocumentID);
            TADocument taDoc = ScgeAccountingQueryProvider.TADocumentQuery.FindByIdentity(long.Parse(avDoc.TADocumentID.ToString()));
            IList<DocumentInitiator> initiators = ScgeAccountingQueryProvider.DocumentInitiatorQuery.GetNotResponseInitiatorByDocumentID(taDoc.DocumentID.DocumentID);
            if (initiators.Count > 0)
                signal = "OverRoleWaitTAInitialToWaitTAInitial";
            else
                signal = "OverRoleWaitTAInitialToWaitTAApprove";

            return signal;
        }
        #endregion

        #region OnWithdrawWaitTAApprove
        public string OnWithdrawWaitTAApprove(long workFlowID, object eventData)
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
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);
            }
            catch (Exception)
            {
                throw;
            }
            /*TODO generate signal string*/
            return "WithdrawWaitTAApproveToDraft";
        }
        #endregion

        #endregion


        #region OnEnter{StateName} , OnExit{StateName}

        public void OnEnterOutstanding(long workFlowID)
        {

        }
        public void OnExitOutstanding(long workFlowID)
        {

        }

        #endregion

        #region GetEditableFieldsFor{StateName}

        public override IList<object> GetEditableFieldsForDraft(long workFlowID)
        {
            IList<object> editableFieldEnum = new List<object>();

            long permitUserID = GetPermitUserDraft(workFlowID);
            if (UserAccount.UserID == permitUserID)
            {
                editableFieldEnum.Add(AdvanceFieldGroup.Subject);
                editableFieldEnum.Add(AdvanceFieldGroup.PaymentType);
                editableFieldEnum.Add(AdvanceFieldGroup.RequestDateOfAdvance);
                editableFieldEnum.Add(AdvanceFieldGroup.RequestDateOfRemittance);
                editableFieldEnum.Add(AdvanceFieldGroup.Reason);
                editableFieldEnum.Add(AdvanceFieldGroup.Attachment);
                editableFieldEnum.Add(AdvanceFieldGroup.Memo);
                editableFieldEnum.Add(AdvanceFieldGroup.Other);
                editableFieldEnum.Add(AdvanceFieldGroup.ServiceTeam);
                editableFieldEnum.Add(AdvanceFieldGroup.BuActor);
                editableFieldEnum.Add(AdvanceFieldGroup.CounterCashier);
                editableFieldEnum.Add(AdvanceFieldGroup.DomesticAmountTHB);
                editableFieldEnum.Add(AdvanceFieldGroup.ArrivalDate);
                editableFieldEnum.Add(AdvanceFieldGroup.CurrencyAmount);
                editableFieldEnum.Add(AdvanceFieldGroup.CurrencyRepOffice);

                WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
                AvAdvanceDocument advanceDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(workFlow.Document.DocumentID);
                if (advanceDocument.TADocumentID.HasValue)
                {
                    TADocument taDocument = ScgeAccountingQueryProvider.TADocumentQuery.FindByIdentity(advanceDocument.TADocumentID.Value);
                    WorkFlow taWorkFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(taDocument.DocumentID.DocumentID);
                    if (taWorkFlow.CurrentState.Name == "Complete")
                    {
                        editableFieldEnum.Add(AdvanceFieldGroup.Initiator);
                        editableFieldEnum.Add(AdvanceFieldGroup.AdvanceReferTA);
                        if (string.IsNullOrEmpty(workFlow.Document.DocumentNo))
                            editableFieldEnum.Add(AdvanceFieldGroup.Company);
                    }
                }
                else
                {
                    editableFieldEnum.Add(AdvanceFieldGroup.Initiator);
                    editableFieldEnum.Add(AdvanceFieldGroup.AdvanceReferTA);
                    if (string.IsNullOrEmpty(workFlow.Document.DocumentNo))
                        editableFieldEnum.Add(AdvanceFieldGroup.Company);
                }



            }

            return editableFieldEnum;
        }

        public override IList<object> GetEditableFieldsForHold(long workFlowID)
        {
            IList<object> editableFieldEnum = new List<object>();

            long permitUserID = GetPermitUserHold(workFlowID);
            if (UserAccount.UserID == permitUserID)
            {
                WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
                AvAdvanceDocument advanceDoc = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindAdvanceByWorkFlowID(workFlowID);
                IList<WorkFlowHoldResponseDetail> holdFields = WorkFlowQueryProvider.WorkFlowHoldResponseDetailQuery.GetHoldFields(workFlowID, AvAdvanceConstant.AdvanceHoldStateEventID);
                foreach (WorkFlowHoldResponseDetail holdField in holdFields)
                {
                    if (advanceDoc.AdvanceType.Equals(ZoneType.Foreign) && holdField.FieldName.Equals(AdvanceFieldGroup.PaymentType.ToString()))
                    {
                        editableFieldEnum.Add(AdvanceFieldGroup.PaymentTypeFR);
                    }
                    else
                    {
                        editableFieldEnum.Add((AdvanceFieldGroup)Enum.Parse(typeof(AdvanceFieldGroup), holdField.FieldName));
                        if (holdField.FieldName.Equals(AdvanceFieldGroup.PaymentType.ToString()))
                        {
                            editableFieldEnum.Add(AdvanceFieldGroup.CounterCashier);
                        }
                    }
                }
            }
            return editableFieldEnum;
        }

        public override IList<object> GetEditableFieldsForWaitVerify(long workFlowID)
        {
            IList<object> editableFieldEnum = new List<object>();
            IList<SuRole> permissionRoles = GetAllowRoleVerifyWaitVerify(workFlowID);

            if (MatchCurrentUserRole(permissionRoles))
            {
                editableFieldEnum.Add(AdvanceFieldGroup.Subject);
                editableFieldEnum.Add(AdvanceFieldGroup.PaymentType);
                editableFieldEnum.Add(AdvanceFieldGroup.RequestDateOfAdvance);
                //editableFieldEnum.Add(AdvanceFieldGroup.RequestDateOfRemittance);
                //editableFieldEnum.Add(AdvanceFieldGroup.Reason);
                editableFieldEnum.Add(AdvanceFieldGroup.Attachment);
                editableFieldEnum.Add(AdvanceFieldGroup.Memo);
                editableFieldEnum.Add(AdvanceFieldGroup.Other);
                editableFieldEnum.Add(AdvanceFieldGroup.ExchangeRateForPerDiemCalculation);
                editableFieldEnum.Add(AdvanceFieldGroup.VerifyDetail);
                editableFieldEnum.Add(AdvanceFieldGroup.ServiceTeam);
                editableFieldEnum.Add(AdvanceFieldGroup.CounterCashier);
                editableFieldEnum.Add(AdvanceFieldGroup.PaymentTypeFR);
            }
            return editableFieldEnum;
        }
        public override IList<object> GetEditableFieldsForWaitPayment(long workFlowID)
        {
            IList<object> editableFieldEnum = new List<object>();
            IList<SuRole> permissionRoles = GetPermitRoleCounterCashier(workFlowID);

            if (MatchCurrentUserRole(permissionRoles))
            {
                editableFieldEnum.Add(AdvanceFieldGroup.CounterCashier);
            }
            return editableFieldEnum;
        }
        #endregion

        #region GetVisibleFieldsFor{StateName}

        public override IList<object> GetVisibleFieldsForDraft(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(AdvanceFieldGroup.Subject);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentType);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfAdvance);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfRemittance);
            visibleFieldEnum.Add(AdvanceFieldGroup.Reason);
            visibleFieldEnum.Add(AdvanceFieldGroup.Initiator);
            visibleFieldEnum.Add(AdvanceFieldGroup.Attachment);
            visibleFieldEnum.Add(AdvanceFieldGroup.Memo);
            visibleFieldEnum.Add(AdvanceFieldGroup.Other);
            visibleFieldEnum.Add(AdvanceFieldGroup.AdvanceReferTA);
            visibleFieldEnum.Add(AdvanceFieldGroup.ServiceTeam);
            visibleFieldEnum.Add(AdvanceFieldGroup.Company);
            visibleFieldEnum.Add(AdvanceFieldGroup.BuActor);
            visibleFieldEnum.Add(AdvanceFieldGroup.CounterCashier);
            visibleFieldEnum.Add(AdvanceFieldGroup.DomesticAmountTHB);
            visibleFieldEnum.Add(AdvanceFieldGroup.ArrivalDate);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyAmount);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentTypeFR);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyRepOffice);

            return visibleFieldEnum;
        }

        public IList<object> GetVisibleFieldsForWaitTA(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(AdvanceFieldGroup.Subject);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentType);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfAdvance);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfRemittance);
            visibleFieldEnum.Add(AdvanceFieldGroup.Reason);
            visibleFieldEnum.Add(AdvanceFieldGroup.Initiator);
            visibleFieldEnum.Add(AdvanceFieldGroup.Attachment);
            visibleFieldEnum.Add(AdvanceFieldGroup.Memo);
            visibleFieldEnum.Add(AdvanceFieldGroup.Other);
            visibleFieldEnum.Add(AdvanceFieldGroup.AdvanceReferTA);
            visibleFieldEnum.Add(AdvanceFieldGroup.ServiceTeam);
            visibleFieldEnum.Add(AdvanceFieldGroup.Company);
            visibleFieldEnum.Add(AdvanceFieldGroup.BuActor);
            visibleFieldEnum.Add(AdvanceFieldGroup.CounterCashier);
            visibleFieldEnum.Add(AdvanceFieldGroup.DomesticAmountTHB);
            visibleFieldEnum.Add(AdvanceFieldGroup.ArrivalDate);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyAmount);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentTypeFR);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyRepOffice);

            return visibleFieldEnum;
        }

        public IList<object> GetVisibleFieldsForWaitTAInitial(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(AdvanceFieldGroup.Subject);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentType);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfAdvance);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfRemittance);
            visibleFieldEnum.Add(AdvanceFieldGroup.Reason);
            visibleFieldEnum.Add(AdvanceFieldGroup.Initiator);
            visibleFieldEnum.Add(AdvanceFieldGroup.Attachment);
            visibleFieldEnum.Add(AdvanceFieldGroup.Memo);
            visibleFieldEnum.Add(AdvanceFieldGroup.Other);
            visibleFieldEnum.Add(AdvanceFieldGroup.AdvanceReferTA);
            visibleFieldEnum.Add(AdvanceFieldGroup.ServiceTeam);
            visibleFieldEnum.Add(AdvanceFieldGroup.Company);
            visibleFieldEnum.Add(AdvanceFieldGroup.BuActor);
            visibleFieldEnum.Add(AdvanceFieldGroup.CounterCashier);
            visibleFieldEnum.Add(AdvanceFieldGroup.DomesticAmountTHB);
            visibleFieldEnum.Add(AdvanceFieldGroup.ArrivalDate);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyAmount);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentTypeFR);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyRepOffice);

            return visibleFieldEnum;
        }

        public IList<object> GetVisibleFieldsForWaitTAApprove(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(AdvanceFieldGroup.Subject);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentType);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfAdvance);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfRemittance);
            visibleFieldEnum.Add(AdvanceFieldGroup.Reason);
            visibleFieldEnum.Add(AdvanceFieldGroup.Initiator);
            visibleFieldEnum.Add(AdvanceFieldGroup.Attachment);
            visibleFieldEnum.Add(AdvanceFieldGroup.Memo);
            visibleFieldEnum.Add(AdvanceFieldGroup.Other);
            visibleFieldEnum.Add(AdvanceFieldGroup.AdvanceReferTA);
            visibleFieldEnum.Add(AdvanceFieldGroup.ServiceTeam);
            visibleFieldEnum.Add(AdvanceFieldGroup.Company);
            visibleFieldEnum.Add(AdvanceFieldGroup.BuActor);
            visibleFieldEnum.Add(AdvanceFieldGroup.CounterCashier);
            visibleFieldEnum.Add(AdvanceFieldGroup.DomesticAmountTHB);
            visibleFieldEnum.Add(AdvanceFieldGroup.ArrivalDate);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyAmount);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentTypeFR);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyRepOffice);

            return visibleFieldEnum;
        }

        public override IList<object> GetVisibleFieldsForWaitAR(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(AdvanceFieldGroup.Subject);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentType);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfAdvance);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfRemittance);
            visibleFieldEnum.Add(AdvanceFieldGroup.Reason);
            visibleFieldEnum.Add(AdvanceFieldGroup.Initiator);
            visibleFieldEnum.Add(AdvanceFieldGroup.Attachment);
            visibleFieldEnum.Add(AdvanceFieldGroup.Memo);
            visibleFieldEnum.Add(AdvanceFieldGroup.Other);
            visibleFieldEnum.Add(AdvanceFieldGroup.AdvanceReferTA);
            visibleFieldEnum.Add(AdvanceFieldGroup.ServiceTeam);
            visibleFieldEnum.Add(AdvanceFieldGroup.Company);
            visibleFieldEnum.Add(AdvanceFieldGroup.BuActor);
            visibleFieldEnum.Add(AdvanceFieldGroup.CounterCashier);
            visibleFieldEnum.Add(AdvanceFieldGroup.DomesticAmountTHB);
            visibleFieldEnum.Add(AdvanceFieldGroup.ArrivalDate);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyAmount);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentTypeFR);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyRepOffice);

            return visibleFieldEnum;
        }

        public override IList<object> GetVisibleFieldsForWaitInitial(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(AdvanceFieldGroup.Subject);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentType);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfAdvance);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfRemittance);
            visibleFieldEnum.Add(AdvanceFieldGroup.Reason);
            visibleFieldEnum.Add(AdvanceFieldGroup.Initiator);
            visibleFieldEnum.Add(AdvanceFieldGroup.Attachment);
            visibleFieldEnum.Add(AdvanceFieldGroup.Memo);
            visibleFieldEnum.Add(AdvanceFieldGroup.Other);
            visibleFieldEnum.Add(AdvanceFieldGroup.AdvanceReferTA);
            visibleFieldEnum.Add(AdvanceFieldGroup.ServiceTeam);
            visibleFieldEnum.Add(AdvanceFieldGroup.Company);
            visibleFieldEnum.Add(AdvanceFieldGroup.BuActor);
            visibleFieldEnum.Add(AdvanceFieldGroup.CounterCashier);
            visibleFieldEnum.Add(AdvanceFieldGroup.DomesticAmountTHB);
            visibleFieldEnum.Add(AdvanceFieldGroup.ArrivalDate);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyAmount);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentTypeFR);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyRepOffice);

            return visibleFieldEnum;
        }

        public override IList<object> GetVisibleFieldsForWaitApprove(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(AdvanceFieldGroup.Subject);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentType);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfAdvance);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfRemittance);
            visibleFieldEnum.Add(AdvanceFieldGroup.Reason);
            visibleFieldEnum.Add(AdvanceFieldGroup.Initiator);
            visibleFieldEnum.Add(AdvanceFieldGroup.Attachment);
            visibleFieldEnum.Add(AdvanceFieldGroup.Memo);
            visibleFieldEnum.Add(AdvanceFieldGroup.Other);
            visibleFieldEnum.Add(AdvanceFieldGroup.AdvanceReferTA);
            visibleFieldEnum.Add(AdvanceFieldGroup.ServiceTeam);
            visibleFieldEnum.Add(AdvanceFieldGroup.Company);
            visibleFieldEnum.Add(AdvanceFieldGroup.BuActor);
            visibleFieldEnum.Add(AdvanceFieldGroup.CounterCashier);
            visibleFieldEnum.Add(AdvanceFieldGroup.DomesticAmountTHB);
            visibleFieldEnum.Add(AdvanceFieldGroup.ArrivalDate);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyAmount);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentTypeFR);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyRepOffice);

            return visibleFieldEnum;
        }

        public override IList<object> GetVisibleFieldsForWaitVerify(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(AdvanceFieldGroup.Subject);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentType);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfAdvance);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfRemittance);
            visibleFieldEnum.Add(AdvanceFieldGroup.Reason);
            visibleFieldEnum.Add(AdvanceFieldGroup.Initiator);
            visibleFieldEnum.Add(AdvanceFieldGroup.Attachment);
            visibleFieldEnum.Add(AdvanceFieldGroup.Memo);
            visibleFieldEnum.Add(AdvanceFieldGroup.Other);
            visibleFieldEnum.Add(AdvanceFieldGroup.AdvanceReferTA);
            visibleFieldEnum.Add(AdvanceFieldGroup.ExchangeRateForPerDiemCalculation);
            visibleFieldEnum.Add(AdvanceFieldGroup.ServiceTeam);
            visibleFieldEnum.Add(AdvanceFieldGroup.Company);
            visibleFieldEnum.Add(AdvanceFieldGroup.BuActor);
            visibleFieldEnum.Add(AdvanceFieldGroup.CounterCashier);
            visibleFieldEnum.Add(AdvanceFieldGroup.DomesticAmountTHB);
            visibleFieldEnum.Add(AdvanceFieldGroup.ArrivalDate);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyAmount);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentTypeFR);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyRepOffice);

            IList<SuRole> permissionRoles = GetPermitRoleVerify(workFlowID);

            if (MatchCurrentUserRole(permissionRoles))
            {
                visibleFieldEnum.Add(AdvanceFieldGroup.VerifyDetail);
            }
            return visibleFieldEnum;
        }

        public override IList<object> GetVisibleFieldsForWaitApproveVerify(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(AdvanceFieldGroup.Subject);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentType);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfAdvance);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfRemittance);
            visibleFieldEnum.Add(AdvanceFieldGroup.Reason);
            visibleFieldEnum.Add(AdvanceFieldGroup.Initiator);
            visibleFieldEnum.Add(AdvanceFieldGroup.Attachment);
            visibleFieldEnum.Add(AdvanceFieldGroup.Memo);
            visibleFieldEnum.Add(AdvanceFieldGroup.Other);
            visibleFieldEnum.Add(AdvanceFieldGroup.AdvanceReferTA);
            visibleFieldEnum.Add(AdvanceFieldGroup.ServiceTeam);
            visibleFieldEnum.Add(AdvanceFieldGroup.Company);
            visibleFieldEnum.Add(AdvanceFieldGroup.ExchangeRateForPerDiemCalculation);
            visibleFieldEnum.Add(AdvanceFieldGroup.BuActor);
            visibleFieldEnum.Add(AdvanceFieldGroup.CounterCashier);
            visibleFieldEnum.Add(AdvanceFieldGroup.DomesticAmountTHB);
            visibleFieldEnum.Add(AdvanceFieldGroup.ArrivalDate);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyAmount);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentTypeFR);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyRepOffice);

            IList<SuRole> permissionRoles = GetPermitRoleVerify(workFlowID);

            if (MatchCurrentUserRole(permissionRoles))
            {
                visibleFieldEnum.Add(AdvanceFieldGroup.VerifyDetail);
            }

            permissionRoles = GetPermitRoleApproveVerify(workFlowID);

            if (MatchCurrentUserRole(permissionRoles))
            {
                visibleFieldEnum.Add(AdvanceFieldGroup.VerifyDetail);
            }
            return visibleFieldEnum;
        }

        public override IList<object> GetVisibleFieldsForHold(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(AdvanceFieldGroup.Subject);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentType);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfAdvance);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfRemittance);
            visibleFieldEnum.Add(AdvanceFieldGroup.Reason);
            visibleFieldEnum.Add(AdvanceFieldGroup.Initiator);
            visibleFieldEnum.Add(AdvanceFieldGroup.Attachment);
            visibleFieldEnum.Add(AdvanceFieldGroup.Memo);
            visibleFieldEnum.Add(AdvanceFieldGroup.Other);
            visibleFieldEnum.Add(AdvanceFieldGroup.AdvanceReferTA);
            visibleFieldEnum.Add(AdvanceFieldGroup.ServiceTeam);
            visibleFieldEnum.Add(AdvanceFieldGroup.Company);
            visibleFieldEnum.Add(AdvanceFieldGroup.ExchangeRateForPerDiemCalculation);
            visibleFieldEnum.Add(AdvanceFieldGroup.BuActor);
            visibleFieldEnum.Add(AdvanceFieldGroup.CounterCashier);
            visibleFieldEnum.Add(AdvanceFieldGroup.DomesticAmountTHB);
            visibleFieldEnum.Add(AdvanceFieldGroup.ArrivalDate);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyAmount);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentTypeFR);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyRepOffice);

            IList<SuRole> permissionRoles = GetPermitRoleVerify(workFlowID);

            if (MatchCurrentUserRole(permissionRoles))
            {
                visibleFieldEnum.Add(AdvanceFieldGroup.VerifyDetail);
            }
            return visibleFieldEnum;
        }

        public override IList<object> GetVisibleFieldsForWaitPayment(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(AdvanceFieldGroup.Subject);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentType);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfAdvance);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfRemittance);
            visibleFieldEnum.Add(AdvanceFieldGroup.Reason);
            visibleFieldEnum.Add(AdvanceFieldGroup.Initiator);
            visibleFieldEnum.Add(AdvanceFieldGroup.Attachment);
            visibleFieldEnum.Add(AdvanceFieldGroup.Memo);
            visibleFieldEnum.Add(AdvanceFieldGroup.Other);
            visibleFieldEnum.Add(AdvanceFieldGroup.AdvanceReferTA);
            visibleFieldEnum.Add(AdvanceFieldGroup.ServiceTeam);
            visibleFieldEnum.Add(AdvanceFieldGroup.Company);
            visibleFieldEnum.Add(AdvanceFieldGroup.ExchangeRateForPerDiemCalculation);
            visibleFieldEnum.Add(AdvanceFieldGroup.BuActor);
            visibleFieldEnum.Add(AdvanceFieldGroup.CounterCashier);
            visibleFieldEnum.Add(AdvanceFieldGroup.DomesticAmountTHB);
            visibleFieldEnum.Add(AdvanceFieldGroup.ArrivalDate);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyAmount);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentTypeFR);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyRepOffice);

            IList<SuRole> permissionRoles = GetPermitRoleVerify(workFlowID);

            if (MatchCurrentUserRole(permissionRoles))
            {
                visibleFieldEnum.Add(AdvanceFieldGroup.VerifyDetail);
            }

            permissionRoles = GetPermitRoleApproveVerify(workFlowID);

            if (MatchCurrentUserRole(permissionRoles))
            {
                visibleFieldEnum.Add(AdvanceFieldGroup.VerifyDetail);
            }
            return visibleFieldEnum;
        }


        public override IList<object> GetVisibleFieldsForWaitPaymentFromSAP(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(AdvanceFieldGroup.Subject);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentType);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfAdvance);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfRemittance);
            visibleFieldEnum.Add(AdvanceFieldGroup.Reason);
            visibleFieldEnum.Add(AdvanceFieldGroup.Initiator);
            visibleFieldEnum.Add(AdvanceFieldGroup.Attachment);
            visibleFieldEnum.Add(AdvanceFieldGroup.Memo);
            visibleFieldEnum.Add(AdvanceFieldGroup.Other);
            visibleFieldEnum.Add(AdvanceFieldGroup.AdvanceReferTA);
            visibleFieldEnum.Add(AdvanceFieldGroup.ServiceTeam);
            visibleFieldEnum.Add(AdvanceFieldGroup.Company);
            visibleFieldEnum.Add(AdvanceFieldGroup.ExchangeRateForPerDiemCalculation);
            visibleFieldEnum.Add(AdvanceFieldGroup.BuActor);
            visibleFieldEnum.Add(AdvanceFieldGroup.CounterCashier);
            visibleFieldEnum.Add(AdvanceFieldGroup.DomesticAmountTHB);
            visibleFieldEnum.Add(AdvanceFieldGroup.ArrivalDate);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyAmount);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentTypeFR);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyRepOffice);

            IList<SuRole> permissionRoles = GetPermitRoleApproveVerify(workFlowID);

            if (MatchCurrentUserRole(permissionRoles))
            {
                visibleFieldEnum.Add(AdvanceFieldGroup.VerifyDetail);
            }

            return visibleFieldEnum;
        }

        public override IList<object> GetVisibleFieldsForWaitApproveRejection(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(AdvanceFieldGroup.Subject);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentType);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfAdvance);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfRemittance);
            visibleFieldEnum.Add(AdvanceFieldGroup.Reason);
            visibleFieldEnum.Add(AdvanceFieldGroup.Initiator);
            visibleFieldEnum.Add(AdvanceFieldGroup.Attachment);
            visibleFieldEnum.Add(AdvanceFieldGroup.Memo);
            visibleFieldEnum.Add(AdvanceFieldGroup.Other);
            visibleFieldEnum.Add(AdvanceFieldGroup.AdvanceReferTA);
            visibleFieldEnum.Add(AdvanceFieldGroup.ServiceTeam);
            visibleFieldEnum.Add(AdvanceFieldGroup.Company);
            visibleFieldEnum.Add(AdvanceFieldGroup.ExchangeRateForPerDiemCalculation);
            visibleFieldEnum.Add(AdvanceFieldGroup.BuActor);
            visibleFieldEnum.Add(AdvanceFieldGroup.CounterCashier);
            visibleFieldEnum.Add(AdvanceFieldGroup.DomesticAmountTHB);
            visibleFieldEnum.Add(AdvanceFieldGroup.ArrivalDate);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyAmount);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentTypeFR);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyRepOffice);

            IList<SuRole> permissionRoles = GetPermitRoleApproveVerify(workFlowID);

            if (MatchCurrentUserRole(permissionRoles))
            {
                visibleFieldEnum.Add(AdvanceFieldGroup.VerifyDetail);
            }

            permissionRoles = GetPermitRoleApproveVerify(workFlowID);

            if (MatchCurrentUserRole(permissionRoles))
            {
                visibleFieldEnum.Add(AdvanceFieldGroup.VerifyDetail);
            }
            return visibleFieldEnum;
        }

        public override IList<object> GetVisibleFieldsForWaitDocument(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(AdvanceFieldGroup.Subject);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentType);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfAdvance);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfRemittance);
            visibleFieldEnum.Add(AdvanceFieldGroup.Reason);
            visibleFieldEnum.Add(AdvanceFieldGroup.Initiator);
            visibleFieldEnum.Add(AdvanceFieldGroup.Attachment);
            visibleFieldEnum.Add(AdvanceFieldGroup.Memo);
            visibleFieldEnum.Add(AdvanceFieldGroup.Other);
            visibleFieldEnum.Add(AdvanceFieldGroup.AdvanceReferTA);
            visibleFieldEnum.Add(AdvanceFieldGroup.ServiceTeam);
            visibleFieldEnum.Add(AdvanceFieldGroup.Company);
            visibleFieldEnum.Add(AdvanceFieldGroup.ExchangeRateForPerDiemCalculation);
            visibleFieldEnum.Add(AdvanceFieldGroup.BuActor);
            visibleFieldEnum.Add(AdvanceFieldGroup.CounterCashier);
            visibleFieldEnum.Add(AdvanceFieldGroup.DomesticAmountTHB);
            visibleFieldEnum.Add(AdvanceFieldGroup.ArrivalDate);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyAmount);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentTypeFR);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyRepOffice);

            return visibleFieldEnum;
        }


        public IList<object> GetVisibleFieldsForOutstanding(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(AdvanceFieldGroup.Subject);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentType);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfAdvance);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfRemittance);
            visibleFieldEnum.Add(AdvanceFieldGroup.Reason);
            visibleFieldEnum.Add(AdvanceFieldGroup.Initiator);
            visibleFieldEnum.Add(AdvanceFieldGroup.Attachment);
            visibleFieldEnum.Add(AdvanceFieldGroup.Memo);
            visibleFieldEnum.Add(AdvanceFieldGroup.Other);
            visibleFieldEnum.Add(AdvanceFieldGroup.AdvanceReferTA);
            visibleFieldEnum.Add(AdvanceFieldGroup.ServiceTeam);
            visibleFieldEnum.Add(AdvanceFieldGroup.Company);
            visibleFieldEnum.Add(AdvanceFieldGroup.ExchangeRateForPerDiemCalculation);
            visibleFieldEnum.Add(AdvanceFieldGroup.BuActor);
            visibleFieldEnum.Add(AdvanceFieldGroup.CounterCashier);
            visibleFieldEnum.Add(AdvanceFieldGroup.DomesticAmountTHB);
            visibleFieldEnum.Add(AdvanceFieldGroup.ArrivalDate);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyAmount);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentTypeFR);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyRepOffice);

            IList<SuRole> permissionRoles = GetPermitRoleApproveVerify(workFlowID);

            if (MatchCurrentUserRole(permissionRoles))
            {
                visibleFieldEnum.Add(AdvanceFieldGroup.VerifyDetail);
            }

            permissionRoles = GetPermitRoleApproveVerify(workFlowID);

            if (MatchCurrentUserRole(permissionRoles))
            {
                visibleFieldEnum.Add(AdvanceFieldGroup.VerifyDetail);
            }

            return visibleFieldEnum;
        }

        public override IList<object> GetVisibleFieldsForComplete(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(AdvanceFieldGroup.Subject);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentType);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfAdvance);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfRemittance);
            visibleFieldEnum.Add(AdvanceFieldGroup.Reason);
            visibleFieldEnum.Add(AdvanceFieldGroup.Initiator);
            visibleFieldEnum.Add(AdvanceFieldGroup.Attachment);
            visibleFieldEnum.Add(AdvanceFieldGroup.Memo);
            visibleFieldEnum.Add(AdvanceFieldGroup.Other);
            visibleFieldEnum.Add(AdvanceFieldGroup.AdvanceReferTA);
            visibleFieldEnum.Add(AdvanceFieldGroup.ServiceTeam);
            visibleFieldEnum.Add(AdvanceFieldGroup.Company);
            visibleFieldEnum.Add(AdvanceFieldGroup.ExchangeRateForPerDiemCalculation);
            visibleFieldEnum.Add(AdvanceFieldGroup.BuActor);
            visibleFieldEnum.Add(AdvanceFieldGroup.CounterCashier);
            visibleFieldEnum.Add(AdvanceFieldGroup.DomesticAmountTHB);
            visibleFieldEnum.Add(AdvanceFieldGroup.ArrivalDate);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyAmount);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentTypeFR);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyRepOffice);

            IList<SuRole> permissionRoles = GetPermitRoleVerify(workFlowID);

            if (MatchCurrentUserRole(permissionRoles))
            {
                visibleFieldEnum.Add(AdvanceFieldGroup.VerifyDetail);
            }

            permissionRoles = GetPermitRoleApproveVerify(workFlowID);

            if (MatchCurrentUserRole(permissionRoles))
            {
                visibleFieldEnum.Add(AdvanceFieldGroup.VerifyDetail);
            }
            return visibleFieldEnum;
        }

        public override IList<object> GetVisibleFieldsForCancel(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(AdvanceFieldGroup.Subject);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentType);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfAdvance);
            visibleFieldEnum.Add(AdvanceFieldGroup.RequestDateOfRemittance);
            visibleFieldEnum.Add(AdvanceFieldGroup.Reason);
            visibleFieldEnum.Add(AdvanceFieldGroup.Initiator);
            visibleFieldEnum.Add(AdvanceFieldGroup.Attachment);
            visibleFieldEnum.Add(AdvanceFieldGroup.Memo);
            visibleFieldEnum.Add(AdvanceFieldGroup.Other);
            visibleFieldEnum.Add(AdvanceFieldGroup.AdvanceReferTA);
            visibleFieldEnum.Add(AdvanceFieldGroup.ServiceTeam);
            visibleFieldEnum.Add(AdvanceFieldGroup.Company);
            visibleFieldEnum.Add(AdvanceFieldGroup.ExchangeRateForPerDiemCalculation);
            visibleFieldEnum.Add(AdvanceFieldGroup.BuActor);
            visibleFieldEnum.Add(AdvanceFieldGroup.CounterCashier);
            visibleFieldEnum.Add(AdvanceFieldGroup.DomesticAmountTHB);
            visibleFieldEnum.Add(AdvanceFieldGroup.ArrivalDate);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyAmount);
            visibleFieldEnum.Add(AdvanceFieldGroup.PaymentTypeFR);
            visibleFieldEnum.Add(AdvanceFieldGroup.CurrencyRepOffice);

            return visibleFieldEnum;
        }
        #endregion

        #region GetPermitRole{EventName}
        public override IList<SuRole> GetPermitRoleVerify(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            AvAdvanceDocument advanceDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(workFlow.Document.DocumentID);

            //Check user role can Verify document
            IList<SuRole> permitRoles = QueryProvider.SuUserRoleQuery.GetRoleVerifyDocument();

            //Check limit verify amount
            var documentPermitRoles = from role in permitRoles
                                      where advanceDocument.Amount >= role.VerifyMinLimit
                                            && advanceDocument.Amount <= role.VerifyMaxLimit
                                      select role;
            permitRoles = documentPermitRoles.ToList<SuRole>();
            if (permitRoles.Count == 0) return permitRoles;

            //Check Service Team
            IList<SuRole> serviceTeamRoles = QueryProvider.SuRoleServiceQuery.GetRoleByServiceTeamID(advanceDocument.ServiceTeamID.ServiceTeamID);
            documentPermitRoles = from role in permitRoles
                                  join serviceTeamRole in serviceTeamRoles
                                  on role.RoleID equals serviceTeamRole.RoleID
                                  select role;

            return documentPermitRoles.ToList<SuRole>();
        }

        public override IList<SuRole> GetPermitRoleApproveVerify(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            AvAdvanceDocument advanceDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(workFlow.Document.DocumentID);

            //Check user role can ApproveVerify document
            IList<SuRole> permitRoles = QueryProvider.SuUserRoleQuery.GetRoleApproveVerifyDocument();

            //Check limit approve verify amount
            var documentPermitRoles = from role in permitRoles
                                      where advanceDocument.Amount >= role.ApproveVerifyMinLimit
                                            && advanceDocument.Amount <= role.ApproveVerifyMaxLimit
                                      select role;
            permitRoles = documentPermitRoles.ToList<SuRole>();
            if (permitRoles.Count == 0) return permitRoles;

            //Check Service Team
            IList<SuRole> serviceTeamRoles = QueryProvider.SuRoleServiceQuery.GetRoleByServiceTeamID(advanceDocument.ServiceTeamID.ServiceTeamID);
            documentPermitRoles = from role in permitRoles
                                  join serviceTeamRole in serviceTeamRoles
                                  on role.RoleID equals serviceTeamRole.RoleID
                                  select role;

            return documentPermitRoles.ToList<SuRole>();
        }

        public override IList<SuRole> GetPermitRoleVerifyAndApproveVerify(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            AvAdvanceDocument advanceDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(workFlow.Document.DocumentID);

            //Check user role can Verify document
            IList<SuRole> permitRoles = QueryProvider.SuUserRoleQuery.GetRoleVerifyAndApproveVerifyDocument();

            //Check limit verify amount
            var documentPermitRoles = from role in permitRoles
                                      where advanceDocument.Amount >= role.VerifyMinLimit
                                            && advanceDocument.Amount <= role.VerifyMaxLimit
                                      select role;

            permitRoles = documentPermitRoles.ToList<SuRole>();
            if (permitRoles.Count == 0) return permitRoles;

            documentPermitRoles = from role in permitRoles
                                  where advanceDocument.Amount >= role.ApproveVerifyMinLimit
                                        && advanceDocument.Amount <= role.ApproveVerifyMaxLimit
                                  select role;
            permitRoles = documentPermitRoles.ToList<SuRole>();
            if (permitRoles.Count == 0) return permitRoles;

            //Check Service Team
            IList<SuRole> serviceTeamRoles = QueryProvider.SuRoleServiceQuery.GetRoleByServiceTeamID(advanceDocument.ServiceTeamID.ServiceTeamID);
            documentPermitRoles = from role in permitRoles
                                  join serviceTeamRole in serviceTeamRoles
                                  on role.RoleID equals serviceTeamRole.RoleID
                                  select role;

            return documentPermitRoles.ToList<SuRole>();
        }
        public override IList<SuRole> GetPermitRoleReceive(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            AvAdvanceDocument advanceDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(workFlow.Document.DocumentID);

            //Check user role can Receive document
            IList<SuRole> permitRoles = QueryProvider.SuUserRoleQuery.GetRoleReceiveDocument();
            if (permitRoles.Count == 0) return permitRoles;

            //Check Service Team
            IList<SuRole> serviceTeamRoles = QueryProvider.SuRoleServiceQuery.GetRoleByServiceTeamID(advanceDocument.ServiceTeamID.ServiceTeamID);
            var documentPermitRoles = from role in permitRoles
                                      join serviceTeamRole in serviceTeamRoles
                                      on role.RoleID equals serviceTeamRole.RoleID
                                      select role;

            return documentPermitRoles.ToList<SuRole>();
        }

        public override IList<SuRole> GetPermitRoleWithdraw(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            AvAdvanceDocument advanceDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(workFlow.Document.DocumentID);

            //Check user role can Receive document
            IList<SuRole> permitRoles = QueryProvider.SuUserRoleQuery.GetRoleApproveVerifyDocument();
            if (permitRoles.Count == 0) return permitRoles;

            //Check Service Team
            IList<SuRole> serviceTeamRoles = QueryProvider.SuRoleServiceQuery.GetRoleByServiceTeamID(advanceDocument.ServiceTeamID.ServiceTeamID);
            var documentPermitRoles = from role in permitRoles
                                      join serviceTeamRole in serviceTeamRoles
                                      on role.RoleID equals serviceTeamRole.RoleID
                                      select role;

            return documentPermitRoles.ToList<SuRole>();
        }

        public override IList<SuRole> GetPermitRoleCounterCashier(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            AvAdvanceDocument advanceDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(workFlow.Document.DocumentID);

            //Check user role is Counter Cashier
            IList<SuRole> permitRoles = QueryProvider.SuUserRoleQuery.GetRoleCounterCashier();
            if (permitRoles.Count == 0) return permitRoles;

            // Comment By พี่หยวย
            //Check Service Team
            //IList<SuRole> serviceTeamRoles = QueryProvider.SuRoleServiceQuery.GetRoleByServiceTeamID(advanceDocument.ServiceTeamID.ServiceTeamID);
            //var documentPermitRoles = from role in permitRoles
            //                      join serviceTeamRole in serviceTeamRoles
            //                      on role.RoleID equals serviceTeamRole.RoleID
            //                      select role;

            //permitRoles = documentPermitRoles.ToList<SuRole>();
            //if (permitRoles.Count == 0) return permitRoles;

            //Check PB
            IList<SuRole> cashierRoles = QueryProvider.SuRolePBQuery.FindRoleByPBID(advanceDocument.PBID.Pbid);
            var documentPermitRoles = from role in permitRoles
                                      join cashierRole in cashierRoles
                                      on role.RoleID equals cashierRole.RoleID
                                      select role;

            return documentPermitRoles.ToList<SuRole>();
        }
        #endregion

        #region GetPermitUserApproveWaitTA

        //public long GetPermitUserApproveWaitTA(long workFlowID)
        //{
        //    WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
        //    AvAdvanceDocument advanceDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(workFlow.Document.DocumentID);
        //    TADocument taDocument = ScgeAccountingQueryProvider.TADocumentQuery.FindByIdentity(advanceDocument.TADocumentID.Value);


        //    return taDocument.DocumentID.ApproverID.Userid;
        //}

        #endregion

        #region GetAllowRole{EventName}{StateName}

        public IList<SuRole> GetAllowRoleClearingOutstanding(long workFlowID)
        {
            return GetPermitRoleCounterCashier(workFlowID);
        }

        //public long GetAllowUserApproveWaitTA(long workFlowID)
        //{
        //    return GetPermitUserApproveWaitTA(workFlowID);
        //}

        public override IList<SuRole> GetAllowRoleWithdrawWaitPaymentFromSAP(long workFlowID)
        {
            return GetPermitRoleWithdraw(workFlowID);
        }
        #endregion

        #region Protected Method

        public override double GetTotalDocumentAmount(long documentID)
        {
            AvAdvanceDocument advanceDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(documentID);
            return advanceDocument.Amount;
        }

        #endregion
    }
}

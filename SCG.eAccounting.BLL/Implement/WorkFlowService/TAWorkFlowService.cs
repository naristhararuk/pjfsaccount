﻿using System;
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

using SCG.eAccounting.DTO;
using SCG.eAccounting.BLL.WorkFlowService;
using SCG.eAccounting.Query;
using SCG.eAccounting.DAL;
using SCG.DB.Query;
using SS.DB.Query;

namespace SCG.eAccounting.BLL.Implement.WorkFlowService
{
    public class TAWorkFlowService : GeneralWorkFlowService, ITAWorkFlowService
    {
        public IWorkFlowService WorkFlowService { get; set; }
        public IAvAdvanceDocumentService AvAdvanceDocumentService { get; set; }

        #region CanEdit{StateName}
        public bool CanEditDraft(long workFlowID)
        {
            long userID = GetPermitUserDraft(workFlowID);
            return userID == UserAccount.UserID;
        }

        public bool CanCancelComplete(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            return scgDocument.ApproverID.Userid == UserAccount.UserID;
        }
        #endregion

        #region OnSendDraft
        public override string OnSendDraft(long workFlowID, object eventData)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);

            /*TODO validate , save event data to workflow*/
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
                response.UpdPgm = "WorkFlow";
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
             *  Case 1 : Advance Document has initiator => SendDraftToWaitInitator
             *  Case 2 : Advance Document has no initiator => SendDraftToWaitApprove
             */


            string signal = "";

            TADocument taDocument = ScgeAccountingQueryProvider.TADocumentQuery.GetTADocumentByDocumentID(workFlow.Document.DocumentID);

            IList<AvAdvanceDocument> taAdvances = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindByTADocumentID(taDocument.TADocumentID);
            foreach (AvAdvanceDocument advance in taAdvances)
            {
                WorkFlow advanceWorkFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(advance.DocumentID.DocumentID);
                if (advanceWorkFlow.CurrentState.Name == "Draft")
                {
                    // Notify send all draft advance.
                    string sendDraftEventName = "Send";
                    WorkFlowStateEvent stateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetByWorkFlowStateID_EventName(advanceWorkFlow.CurrentState.WorkFlowStateID, sendDraftEventName);
                    WorkFlowService.NotifyEventFromInternal(advanceWorkFlow.WorkFlowID, sendDraftEventName, new SubmitResponse(stateEvent.WorkFlowStateEventID));
                }
            }

            IList<DocumentInitiator> initiators = ScgeAccountingQueryProvider.DocumentInitiatorQuery.GetNotResponseInitiatorByDocumentID(workFlow.Document.DocumentID);
            if (initiators.Count > 0)
            {
                signal = "SendDraftToWaitInitial";
            }
            else
                signal = "SendDraftToWaitApprove";

            if (ParameterServices.EnableEmail02Requester)
            {
                long sendToUserID = 0;
                IList<long> ccList = new List<long>();

                if (scgDocument.CreatorID.Userid != scgDocument.RequesterID.Userid)
                    sendToUserID = scgDocument.RequesterID.Userid;

                IList<TADocumentTraveller> travellers = ScgeAccountingQueryProvider.TADocumentTravellerQuery.FindTADocumentTravellerByTADocumentID(taDocument.TADocumentID);

                if (travellers.Count > 1)
                {
                    foreach (TADocumentTraveller traveller in travellers)
                    {
                        if (sendToUserID == 0)
                            sendToUserID = traveller.UserID.Userid;
                        else if (traveller.UserID.Userid != sendToUserID && !ccList.Contains(traveller.UserID.Userid))
                            ccList.Add(traveller.UserID.Userid);
                    }
                }

                if (sendToUserID != 0)
                    SCGEmailService.SendEmailEM02(response.WorkFlowResponseID, sendToUserID, ccList);
            }

            return signal;
        }
        #endregion

        public override string OnCancelDraft(long workFlowID, object eventData)
        {
            //Cancel all advance that link to TA
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

            TADocument taDocument = ScgeAccountingQueryProvider.TADocumentQuery.GetTADocumentByDocumentID(workFlow.Document.DocumentID);
            //Cancel all advance that link to TA
            IList<AvAdvanceDocument> taAdvances = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindByTADocumentID(taDocument.TADocumentID);
            foreach (AvAdvanceDocument advance in taAdvances)
            {
                WorkFlow advanceWorkFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(advance.DocumentID.DocumentID);
                //Set CurrentState to Cancel

                advanceWorkFlow.CurrentState = WorkFlowQueryProvider.WorkFlowStateQuery.FindWorkFlowStateIDByTypeIDAndStateName(advanceWorkFlow.WorkFlowType.WorkFlowTypeID, "Cancel");
                WorkFlowService.Save(advanceWorkFlow);
                WorkFlowService.ReCalculateWorkFlowPermission(advanceWorkFlow.WorkFlowID);
            }


            return base.OnCancelDraft(workFlowID, eventData);
        }

        #region OnRejectWaitInitial
        public override string OnRejectWaitInitial(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            TADocument taDocument = ScgeAccountingQueryProvider.TADocumentQuery.GetTADocumentByDocumentID(scgDocument.DocumentID);
            RejectDetailResponse rejectDetailResponse;

            try
            {

                if (eventData is SubmitResponse)
                    rejectDetailResponse = DefaultRejectDetailResponse((eventData as SubmitResponse).WorkFlowStateEventID, (eventData as SubmitResponse).ResponseMethod);
                else
                    rejectDetailResponse = eventData as RejectDetailResponse;

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
                response.UpdPgm = "WorkFlow";
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
                rejectResponse.UpdPgm = "WorkFlow";
                WorkFlowDaoProvider.WorkFlowRejectResponseDao.Save(rejectResponse);

                SCGEmailService.SendEmailEM04(response.WorkFlowResponseID, scgDocument.CreatorID.Userid);

                if (ParameterServices.EnableEmail02Requester || ParameterServices.EnableEmail02Initiator)
                {
                    long sendToUserID = 0;
                    IList<long> ccList = new List<long>();

                    if (ParameterServices.EnableEmail02Requester)
                    {
                        if (scgDocument.CreatorID.Userid != scgDocument.RequesterID.Userid)
                            sendToUserID = scgDocument.RequesterID.Userid;

                        IList<TADocumentTraveller> travellers = ScgeAccountingQueryProvider.TADocumentTravellerQuery.FindTADocumentTravellerByTADocumentID(taDocument.TADocumentID);
                        foreach (TADocumentTraveller traveller in travellers)
                        {
                            if (sendToUserID == 0)
                                sendToUserID = traveller.UserID.Userid;
                            else if (traveller.UserID.Userid != sendToUserID && !ccList.Contains(traveller.UserID.Userid))
                                ccList.Add(traveller.UserID.Userid);
                        }
                    }

                    if (ParameterServices.EnableEmail02Initiator)
                    {
                        IList<DocumentInitiator> responsedInitiators = ScgeAccountingQueryProvider.DocumentInitiatorQuery.GetResponsedInitiatorByDocumentID(scgDocument.DocumentID);
                        foreach (DocumentInitiator initiator in responsedInitiators)
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

            // ====================== For Reject all advance that link to TA ======================
            IList<AvAdvanceDocument> taAdvances = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindByTADocumentID(taDocument.TADocumentID);

            // Use state WaitInitial to add into table WorkflowResponse
            string eventName = "Reject";
            WorkFlowStateEvent stateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetByWorkFlowStateID_EventName(AdvanceStateID.WaitInitial, eventName);
            rejectDetailResponse.WorkFlowStateEventID = stateEvent.WorkFlowStateEventID;

            foreach (AvAdvanceDocument advance in taAdvances)
            {
                WorkFlow advanceWorkFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(advance.DocumentID.DocumentID);

                // Notify Reject all Wait TA Initial advance except cancelled advance.
                if (advanceWorkFlow.CurrentState.WorkFlowStateID != AdvanceStateID.Cancel)
                    WorkFlowService.NotifyEventFromInternal(advanceWorkFlow.WorkFlowID, eventName, rejectDetailResponse);
            }
            // ====================== For Reject all advance that link to TA ======================

            /*TODO generate signal string*/
            return "RejectWaitInitialToDraft";
        }
        #endregion

        #region OnWithdrawWaitInitial
        public override string OnWithdrawWaitInitial(long workFlowID, object eventData)
        {
            string signal = base.OnWithdrawWaitInitial(workFlowID, eventData);

            // Withdraw all advance that link to TA
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            TADocument taDocument = ScgeAccountingQueryProvider.TADocumentQuery.GetTADocumentByDocumentID(workFlow.Document.DocumentID);

            // Withdraw all advance that link to TA
            IList<AvAdvanceDocument> taAdvances = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindByTADocumentID(taDocument.TADocumentID);

            // ====================== For Withdraw all advance that link to TA ======================
            // Use state WaitInitial to add into table WorkflowResponse
            string eventName = "Withdraw";
            WorkFlowStateEvent stateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetByWorkFlowStateID_EventName(AdvanceStateID.WaitInitial, eventName);

            foreach (AvAdvanceDocument advance in taAdvances)
            {
                WorkFlow advanceWorkFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(advance.DocumentID.DocumentID);

                // Notify Withdraw all Wait TA Initial advance except cancelled advance.
                if (advanceWorkFlow.CurrentState.WorkFlowStateID != AdvanceStateID.Cancel)
                    WorkFlowService.NotifyEventFromInternal(advanceWorkFlow.WorkFlowID, eventName, new SubmitResponse(stateEvent.WorkFlowStateEventID));
            }
            // ====================== For Withdraw all advance that link to TA ======================

            return signal;
        }
        #endregion

        #region OnApproveWaitInitial
        public override string OnApproveWaitInitial(long workFlowID, object eventData)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);

            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            TADocument taDocument = ScgeAccountingQueryProvider.TADocumentQuery.GetTADocumentByDocumentID(workFlow.Document.DocumentID);
            IList<AvAdvanceDocument> advanceDocuments = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindByTADocumentID(taDocument.TADocumentID);
            IList<long> advanceWorkflowIDs = new List<long>();
            foreach (AvAdvanceDocument advanceDocument in advanceDocuments)
            {
                WorkFlow advanceWorkFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(advanceDocument.DocumentID.DocumentID);

                // Do not approve initial on cancelled advance
                if (advanceWorkFlow.CurrentState.WorkFlowStateID == AdvanceStateID.Cancel)
                    continue;
                else
                    advanceWorkflowIDs.Add(advanceWorkFlow.WorkFlowID);

                // Change to use WaitTAInitial state instead of WaitTA
                if (advanceWorkFlow.CurrentState.WorkFlowStateID != AdvanceStateID.WaitTAInitial)
                {
                    //errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("WorkFlow_OnApproveWaitInitial_AllAdvance_DoesNot_WaitTA"));
                    errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("WorkFlow_OnApproveWaitInitial_AllAdvance_DoesNot_WaitTAInitial"));
                    break;
                }
            }

            if (!errors.IsEmpty) throw new SS.Standard.Utilities.ServiceValidationException(errors);

            string signal = base.OnApproveWaitInitial(workFlowID, eventData);

            // ====================== For Approve all advance that link to TA ======================
            // Use state WaitInitial to add into table WorkflowResponse
            string eventName = "Approve";
            WorkFlowStateEvent stateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetByWorkFlowStateID_EventName(AdvanceStateID.WaitInitial, eventName);
            //SubmitResponse submitResponse = eventData as SubmitResponse;

            //object[] avEventData = new object[3];
            //avEventData[0] = stateEvent.WorkFlowStateEventID;
            //avEventData[1] = submitResponse.ResponseMethod;
            //if (signal.Equals("ApproveWaitInitialToWaitInitial"))
            //    avEventData[2] = 1;
            //else if (signal.Equals("ApproveWaitInitialToWaitApprove"))
            //    avEventData[2] = 2;

            foreach (long advanceWorkFlowID in advanceWorkflowIDs)
            {
                // Notify Approve all Wait TA Initial advance.
                WorkFlowService.NotifyEventFromInternal(advanceWorkFlowID, eventName, eventData);
            }
            // ====================== For Approve all advance that link to TA ======================

            return signal;
        }
        #endregion

        #region OnApproveWaitApprove
        public override string OnApproveWaitApprove(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);

            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            TADocument taDocument = ScgeAccountingQueryProvider.TADocumentQuery.GetTADocumentByDocumentID(workFlow.Document.DocumentID);
            IList<AvAdvanceDocument> advanceDocuments = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindByTADocumentID(taDocument.TADocumentID);
            IList<long> advanceWorkflowIDs = new List<long>();
            foreach (AvAdvanceDocument advanceDocument in advanceDocuments)
            {
                WorkFlow advanceWorkFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(advanceDocument.DocumentID.DocumentID);

                // Do not approve on cancelled advance
                if (advanceWorkFlow.CurrentState.WorkFlowStateID == AdvanceStateID.Cancel)
                    continue;
                else
                    advanceWorkflowIDs.Add(advanceWorkFlow.WorkFlowID);

                // Change to use WaitTAApprove state instead of WaitTA
                if (advanceWorkFlow.CurrentState.WorkFlowStateID != AdvanceStateID.WaitTAApprove)
                {
                    //errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("WorkFlow_OnApproveWaitApprove_AllAdvance_DoesNot_WaitTA"));
                    errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("WorkFlow_OnApproveWaitApprove_AllAdvance_DoesNot_WaitTAApprove"));
                    break;
                }
            }

            if (!errors.IsEmpty) throw new SS.Standard.Utilities.ServiceValidationException(errors);

            try
            {
                ////SubmitResponse submitResponse = eventData as SubmitResponse;
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
                scgDocument.ApproveDate = DateTime.Now;
                scgDocument.UpdBy = UserAccount.UserID;
                scgDocument.UpdDate = DateTime.Now;
                scgDocument.UpdPgm = UserAccount.CurrentProgramCode;
                SCGDocumentService.SaveOrUpdate(scgDocument);
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
                if (ParameterServices.EnableEmail02Creator || ParameterServices.EnableEmail02Requester || ParameterServices.EnableEmail02Initiator)
                {
                    long sendToUserID = 0;
                    IList<long> ccList = new List<long>();

                    if (ParameterServices.EnableEmail02Creator)
                        sendToUserID = scgDocument.CreatorID.Userid;

                    if (ParameterServices.EnableEmail02Requester)
                    {
                        if (sendToUserID == 0)
                        {
                            sendToUserID = scgDocument.RequesterID.Userid;
                        }
                        else if (scgDocument.CreatorID.Userid != scgDocument.RequesterID.Userid)
                        {
                            ccList.Add(scgDocument.RequesterID.Userid);
                        }

                        //Send mail02 to all travellers that doesnot requester
                        IList<TADocumentTraveller> travellers = ScgeAccountingQueryProvider.TADocumentTravellerQuery.FindTADocumentTravellerByTADocumentID(taDocument.TADocumentID);
                        foreach (TADocumentTraveller traveller in travellers)
                        {
                            if (sendToUserID == 0)
                                sendToUserID = traveller.UserID.Userid;
                            else if (traveller.UserID.Userid != sendToUserID && !ccList.Contains(traveller.UserID.Userid))
                                ccList.Add(traveller.UserID.Userid);
                        }
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


            // ====================== For Approve all advance that link to TA ======================
            // Use state WaitApprove to add into table WorkflowResponse
            string eventName = "Approve";
            WorkFlowStateEvent stateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetByWorkFlowStateID_EventName(AdvanceStateID.WaitApprove, eventName);

            foreach (long advanceWorkFlowID in advanceWorkflowIDs)
            {
                // Notify Approve all Wait TA Approve advance.
                WorkFlowService.NotifyEventFromInternal(advanceWorkFlowID, eventName, new SubmitResponse(stateEvent.WorkFlowStateEventID));
            }
            //IList<AvAdvanceDocument> taAdvances = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindByTADocumentID(taDocument.TADocumentID);
            //foreach (AvAdvanceDocument advance in taAdvances)
            //{
            //    WorkFlow advanceWorkFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(advance.DocumentID.DocumentID);

            //    // Notify Clearing all advance
            //    string approveWaitTAEventName = "Approve";
            //    WorkFlowStateEvent stateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetByWorkFlowStateID_EventName(advanceWorkFlow.CurrentState.WorkFlowStateID, approveWaitTAEventName);
            //    WorkFlowService.NotifyEventFromInternal(advanceWorkFlow.WorkFlowID, approveWaitTAEventName, new SubmitResponse(stateEvent.WorkFlowStateEventID));
            //}  
            // ====================== For Approve all advance that link to TA ======================

            /*  TODO generate signal string
             *  Case 1 : ApproveWaitApproveToComplete
             */
            return "ApproveWaitApproveToComplete";
        }
        #endregion

        #region OnRejectWaitApprove
        public override string OnRejectWaitApprove(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            TADocument taDocument = ScgeAccountingQueryProvider.TADocumentQuery.GetTADocumentByDocumentID(scgDocument.DocumentID);
            RejectDetailResponse rejectDetailResponse;

            try
            {

                if (eventData is SubmitResponse)
                    rejectDetailResponse = DefaultRejectDetailResponse((eventData as SubmitResponse).WorkFlowStateEventID, (eventData as SubmitResponse).ResponseMethod);
                else
                    rejectDetailResponse = eventData as RejectDetailResponse;

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
                response.UpdPgm = "WorkFlow";
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
                rejectResponse.UpdPgm = "WorkFlow";
                WorkFlowDaoProvider.WorkFlowRejectResponseDao.Save(rejectResponse);

                SCGEmailService.SendEmailEM04(response.WorkFlowResponseID, scgDocument.CreatorID.Userid);

                if (ParameterServices.EnableEmail02Requester || ParameterServices.EnableEmail02Initiator)
                {
                    long sendToUserID = 0;
                    IList<long> ccList = new List<long>();

                    if (ParameterServices.EnableEmail02Requester)
                    {
                        if (scgDocument.CreatorID.Userid != scgDocument.RequesterID.Userid)
                            sendToUserID = scgDocument.RequesterID.Userid;

                        IList<TADocumentTraveller> travellers = ScgeAccountingQueryProvider.TADocumentTravellerQuery.FindTADocumentTravellerByTADocumentID(taDocument.TADocumentID);
                        foreach (TADocumentTraveller traveller in travellers)
                        {
                            if (sendToUserID == 0)
                                sendToUserID = traveller.UserID.Userid;
                            else if (traveller.UserID.Userid != sendToUserID && !ccList.Contains(traveller.UserID.Userid))
                                ccList.Add(traveller.UserID.Userid);
                        }
                    }

                    if (ParameterServices.EnableEmail02Initiator)
                    {
                        IList<DocumentInitiator> initiators = ScgeAccountingQueryProvider.DocumentInitiatorQuery.GetDocumentInitiatorByDocumentID(scgDocument.DocumentID);
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


            // ====================== For Reject all advance that link to TA ======================
            IList<AvAdvanceDocument> taAdvances = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindByTADocumentID(taDocument.TADocumentID);

            // Use state WaitInitial to add into table WorkflowResponse
            string eventName = "Reject";
            WorkFlowStateEvent stateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetByWorkFlowStateID_EventName(AdvanceStateID.WaitApprove, eventName);
            rejectDetailResponse.WorkFlowStateEventID = stateEvent.WorkFlowStateEventID;

            foreach (AvAdvanceDocument advance in taAdvances)
            {
                WorkFlow advanceWorkFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(advance.DocumentID.DocumentID);

                // Notify Reject all Wait TA Approve advance except cancelled advance.
                if (advanceWorkFlow.CurrentState.WorkFlowStateID != AdvanceStateID.Cancel)
                    WorkFlowService.NotifyEventFromInternal(advanceWorkFlow.WorkFlowID, eventName, rejectDetailResponse);
            }
            // ====================== For Reject all advance that link to TA ======================

            /*TODO generate signal string*/
            return "RejectWaitApproveToDraft";
        }
        #endregion

        #region OnOverRoleWaitInitial
        public override string OnOverRoleWaitInitial(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            /*  TODO generate signal string
             *  Case 1 : OverRoleWaitInitialToWaitInitial
             *  Case 2 : OverRoleWaitInitialToWaitApprove
             */
            string signal = base.OnOverRoleWaitInitial(workFlowID, eventData);

            // ====================== For Overrole all advance that link to TA ======================
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            TADocument taDocument = ScgeAccountingQueryProvider.TADocumentQuery.GetTADocumentByDocumentID(workFlow.Document.DocumentID);

            string eventName = "OverRole";
            IList<AvAdvanceDocument> taAdvances = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindByTADocumentID(taDocument.TADocumentID);

            // Use state WaitInitial to add into table WorkflowResponse
            WorkFlowStateEvent stateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetByWorkFlowStateID_EventName(AdvanceStateID.WaitInitial, eventName);

            foreach (AvAdvanceDocument advance in taAdvances)
            {
                WorkFlow advanceWorkFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(advance.DocumentID.DocumentID);

                // Notify Reject all Wait TA Approve advance except cancelled advance.
                if (advanceWorkFlow.CurrentState.WorkFlowStateID != AdvanceStateID.Cancel)
                    WorkFlowService.NotifyEventFromInternal(advanceWorkFlow.WorkFlowID, eventName, new SubmitResponse(stateEvent.WorkFlowStateEventID));
            }
            // ====================== For Overrole all advance that link to TA ======================

            return signal;
        }
        #endregion

        #region OnWithdrawWaitApprove
        public override string OnWithdrawWaitApprove(long workFlowID, object eventData)
        {
            string signal = base.OnWithdrawWaitApprove(workFlowID, eventData);

            // Withdraw all advance that link to TA
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            TADocument taDocument = ScgeAccountingQueryProvider.TADocumentQuery.GetTADocumentByDocumentID(workFlow.Document.DocumentID);

            // Withdraw all advance that link to TA
            IList<AvAdvanceDocument> taAdvances = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindByTADocumentID(taDocument.TADocumentID);

            // ====================== For Withdraw all advance that link to TA ======================
            // Use state WaitInitial to add into table WorkflowResponse
            string eventName = "Withdraw";
            WorkFlowStateEvent stateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetByWorkFlowStateID_EventName(AdvanceStateID.WaitApprove, eventName);

            foreach (AvAdvanceDocument advance in taAdvances)
            {
                WorkFlow advanceWorkFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(advance.DocumentID.DocumentID);

                // Notify Withdraw all Wait TA Initial advance except cancelled advance.
                if (advanceWorkFlow.CurrentState.WorkFlowStateID != AdvanceStateID.Cancel)
                    WorkFlowService.NotifyEventFromInternal(advanceWorkFlow.WorkFlowID, eventName, new SubmitResponse(stateEvent.WorkFlowStateEventID));
            }
            // ====================== For Withdraw all advance that link to TA ======================

            return signal;
        }
        #endregion

        #region GetEditableFieldsFor{StateName}

        public virtual IList<object> GetEditableFieldsForDraft(long workFlowID)
        {
            IList<object> editableFieldEnum = new List<object>();

            long permitUserID = GetPermitUserDraft(workFlowID);
            if (UserAccount.UserID == permitUserID)
            {
                editableFieldEnum.Add(TAFieldGroup.All);
                editableFieldEnum.Add(TAFieldGroup.Initiator);

                WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
                if (string.IsNullOrEmpty(workFlow.Document.DocumentNo))
                    editableFieldEnum.Add(TAFieldGroup.Company);
            }

            return editableFieldEnum;
        }

        #endregion

        #region GetVisibleFieldsFor{StateName}

        public virtual IList<object> GetVisibleFieldsForDraft(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(TAFieldGroup.All);
            visibleFieldEnum.Add(TAFieldGroup.Company);
            visibleFieldEnum.Add(TAFieldGroup.Initiator);
            return visibleFieldEnum;
        }

        public virtual IList<object> GetVisibleFieldsForWaitInitial(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(TAFieldGroup.All);
            visibleFieldEnum.Add(TAFieldGroup.Company);
            visibleFieldEnum.Add(TAFieldGroup.Initiator);
            return visibleFieldEnum;
        }

        public virtual IList<object> GetVisibleFieldsForWaitApprove(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(TAFieldGroup.All);
            visibleFieldEnum.Add(TAFieldGroup.Company);
            visibleFieldEnum.Add(TAFieldGroup.Initiator);
            return visibleFieldEnum;
        }

        public virtual IList<object> GetVisibleFieldsForComplete(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(TAFieldGroup.All);
            visibleFieldEnum.Add(TAFieldGroup.Company);
            visibleFieldEnum.Add(TAFieldGroup.Initiator);
            return visibleFieldEnum;
        }

        public virtual IList<object> GetVisibleFieldsForCancel(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(TAFieldGroup.All);
            visibleFieldEnum.Add(TAFieldGroup.Company);
            visibleFieldEnum.Add(TAFieldGroup.Initiator);
            return visibleFieldEnum;
        }

        #endregion

        #region OnCancelComplete
        public string OnCancelComplete(long workFlowID, object eventData)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);

            /*TODO validate , save event data to workflow*/
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
                response.UpdPgm = "WorkFlow";
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
             *  Case 1 : MPA Document has initiator => SendDraftToWaitInitator
             *  Case 2 : MPA Document has no initiator => SendDraftToWaitApprove
             */


            string signal = "";

            TADocument taDocument = ScgeAccountingQueryProvider.TADocumentQuery.GetTADocumentByDocumentID(workFlow.Document.DocumentID);

            //IList<DocumentInitiator> initiators = ScgeAccountingQueryProvider.DocumentInitiatorQuery.GetNotResponseInitiatorByDocumentID(workFlow.Document.DocumentID);
            signal = "WithdrawCompleteToCancel";
            if (ParameterServices.EnableEmail02Creator || ParameterServices.EnableEmail02Requester || ParameterServices.EnableEmail02Initiator)
            {
                long sendToUserID = 0;
                IList<long> ccList = new List<long>();

                if (ParameterServices.EnableEmail02Creator)
                    sendToUserID = scgDocument.CreatorID.Userid;

                if (ParameterServices.EnableEmail02Requester)
                {
                    if (sendToUserID == 0)
                    {
                        sendToUserID = scgDocument.RequesterID.Userid;
                    }
                    else if (scgDocument.CreatorID.Userid != scgDocument.RequesterID.Userid)
                    {
                        ccList.Add(scgDocument.RequesterID.Userid);
                    }

                    //Send mail02 to all travellers that doesnot requester
                    IList<TADocumentTraveller> items = ScgeAccountingQueryProvider.TADocumentTravellerQuery.FindTADocumentTravellerByTADocumentID(taDocument.TADocumentID);
                    foreach (TADocumentTraveller item in items)
                    {
                        if (sendToUserID == 0)
                            sendToUserID = item.UserID.Userid;
                        else if (item.UserID.Userid != sendToUserID && !ccList.Contains(item.UserID.Userid))
                            ccList.Add(item.UserID.Userid);
                    }
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
            return signal;
        }
        #endregion

        public void ReCalculatePermissionForCancelComplete(long workFlowID, int workFlowStateEventID)
        {
            //long permitUserID = GetAllowUserSendDraft(workFlowID);
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            long permitUserID = scgDocument.ApproverID.Userid;


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

        public override void OnDeleteDocument(long workFlowID)
        {
            IList<AvAdvanceDocument> avAdvanceDocumentList = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindAdvancDocumentByWorkFlowID(workFlowID);

            if (avAdvanceDocumentList.Count > 0)
            {
                foreach (AvAdvanceDocument avAdvanceDocument in avAdvanceDocumentList)
                {
                    avAdvanceDocument.TADocumentID = null;

                    AvAdvanceDocumentService.Update(avAdvanceDocument);
                }
            }

            base.OnDeleteDocument(workFlowID);
        }
    }
}

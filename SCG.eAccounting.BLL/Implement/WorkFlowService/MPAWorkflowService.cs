using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.BLL.WorkFlowService;
using SS.Standard.WorkFlow.Service;
using SS.Standard.WorkFlow.Query;
using SS.Standard.WorkFlow.DTO;
using SCG.eAccounting.DTO;
using SCG.eAccounting.Query;
using SS.Standard.WorkFlow.DTO.ValueObject;
using SS.Standard.WorkFlow.DAL;
using SCG.eAccounting.DAL;
using SS.DB.Query;
using SCG.DB.Query;

namespace SCG.eAccounting.BLL.Implement.WorkFlowService
{
    public class MPAWorkflowService : GeneralWorkFlowService, IMPAWorkflowService
    {
        public IWorkFlowService WorkFlowService { get; set; }

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

        #region GetEditableFieldsFor{StateName}

        public virtual IList<object> GetEditableFieldsForDraft(long workFlowID)
        {
            IList<object> editableFieldEnum = new List<object>();

            long permitUserID = GetPermitUserDraft(workFlowID);
            if (UserAccount.UserID == permitUserID)
            {
                editableFieldEnum.Add(MPAFieldGroup.All);
                editableFieldEnum.Add(MPAFieldGroup.Initiator);

                WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
                if (string.IsNullOrEmpty(workFlow.Document.DocumentNo))
                    editableFieldEnum.Add(MPAFieldGroup.Company);
            }

            return editableFieldEnum;
        }

        #endregion

        #region GetVisibleFieldsFor{StateName}

        public virtual IList<object> GetVisibleFieldsForDraft(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(MPAFieldGroup.All);
            visibleFieldEnum.Add(MPAFieldGroup.Company);
            visibleFieldEnum.Add(MPAFieldGroup.Initiator);
            return visibleFieldEnum;
        }

        public virtual IList<object> GetVisibleFieldsForWaitInitial(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(MPAFieldGroup.All);
            visibleFieldEnum.Add(MPAFieldGroup.Company);
            visibleFieldEnum.Add(MPAFieldGroup.Initiator);
            return visibleFieldEnum;
        }

        public virtual IList<object> GetVisibleFieldsForWaitApprove(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(MPAFieldGroup.All);
            visibleFieldEnum.Add(MPAFieldGroup.Company);
            visibleFieldEnum.Add(MPAFieldGroup.Initiator);
            return visibleFieldEnum;
        }

        public virtual IList<object> GetVisibleFieldsForComplete(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(MPAFieldGroup.All);
            visibleFieldEnum.Add(MPAFieldGroup.Company);
            visibleFieldEnum.Add(MPAFieldGroup.Initiator);
            return visibleFieldEnum;
        }

        public virtual IList<object> GetVisibleFieldsForCancel(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(MPAFieldGroup.All);
            visibleFieldEnum.Add(MPAFieldGroup.Company);
            visibleFieldEnum.Add(MPAFieldGroup.Initiator);
            return visibleFieldEnum;
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
             *  Case 1 : MPA Document has initiator => SendDraftToWaitInitator
             *  Case 2 : MPA Document has no initiator => SendDraftToWaitApprove
             */


            string signal = "";

            MPADocument mpaDocument = ScgeAccountingQueryProvider.MPADocumentQuery.GetMPADocumentByDocumentID(workFlow.Document.DocumentID);

            IList<DocumentInitiator> initiators = ScgeAccountingQueryProvider.DocumentInitiatorQuery.GetNotResponseInitiatorByDocumentID(workFlow.Document.DocumentID);
            if (initiators.Count > 0)
            {
                signal = "SendDraftToWaitInitial";

                if (ParameterServices.EnableEmail02Requester)
                {
                    long sendToUserID = 0;
                    IList<long> ccList = new List<long>();

                    if (scgDocument.CreatorID.Userid != scgDocument.RequesterID.Userid)
                        sendToUserID = scgDocument.RequesterID.Userid;

                    IList<MPAItem> items = ScgeAccountingQueryProvider.MPAItemQuery.FindMPAItemByMPADocumentID(mpaDocument.MPADocumentID);
                    foreach (MPAItem item in items)
                    {
                        if (sendToUserID == 0)
                            sendToUserID = item.UserID.Userid;
                        else if (item.UserID.Userid != sendToUserID && !ccList.Contains(item.UserID.Userid))
                            ccList.Add(item.UserID.Userid);
                    }

                    SCGEmailService.SendEmailEM02(response.WorkFlowResponseID, sendToUserID, ccList);
                }
            }
            else
                signal = "SendDraftToWaitApprove";

            return signal;
        }
        #endregion

        #region OnRejectWaitInitial
        public override string OnRejectWaitInitial(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            MPADocument mpaDocument = ScgeAccountingQueryProvider.MPADocumentQuery.GetMPADocumentByDocumentID(scgDocument.DocumentID);
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

                        IList<MPAItem> items = ScgeAccountingQueryProvider.MPAItemQuery.FindMPAItemByMPADocumentID(mpaDocument.MPADocumentID);
                        foreach (MPAItem item in items)
                        {
                            if (sendToUserID == 0)
                                sendToUserID = item.UserID.Userid;
                            else if (item.UserID.Userid != sendToUserID && !ccList.Contains(item.UserID.Userid))
                                ccList.Add(item.UserID.Userid);
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

            // Use state WaitInitial to add into table WorkflowResponse
            string eventName = "Reject";
            WorkFlowStateEvent stateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetByWorkFlowStateID_EventName(AdvanceStateID.WaitInitial, eventName);
            rejectDetailResponse.WorkFlowStateEventID = stateEvent.WorkFlowStateEventID;
            return "RejectWaitInitialToDraft";
        }
        #endregion

        #region OnWithdrawWaitInitial
        public override string OnWithdrawWaitInitial(long workFlowID, object eventData)
        {
            string signal = base.OnWithdrawWaitInitial(workFlowID, eventData);

            // Withdraw all advance that link to TA
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            MPADocument mpaDocument = ScgeAccountingQueryProvider.MPADocumentQuery.GetMPADocumentByDocumentID(workFlow.Document.DocumentID);
            string eventName = "Withdraw";
            WorkFlowStateEvent stateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetByWorkFlowStateID_EventName(AdvanceStateID.WaitInitial, eventName);
            return signal;
        }
        #endregion

        #region OnApproveWaitInitial
        public override string OnApproveWaitInitial(long workFlowID, object eventData)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);

            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            MPADocument mpaDocument = ScgeAccountingQueryProvider.MPADocumentQuery.GetMPADocumentByDocumentID(workFlow.Document.DocumentID);
            IList<long> advanceWorkflowIDs = new List<long>();

            if (!errors.IsEmpty) throw new SS.Standard.Utilities.ServiceValidationException(errors);

            string signal = base.OnApproveWaitInitial(workFlowID, eventData);

            // ====================== For Approve all advance that link to TA ======================
            // Use state WaitInitial to add into table WorkflowResponse
            string eventName = "Approve";
            WorkFlowStateEvent stateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetByWorkFlowStateID_EventName(AdvanceStateID.WaitInitial, eventName);
            //SubmitResponse submitResponse = eventData as SubmitResponse;

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
            MPADocument mpaDocument = ScgeAccountingQueryProvider.MPADocumentQuery.GetMPADocumentByDocumentID(workFlow.Document.DocumentID);

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
                        IList<MPAItem> items = ScgeAccountingQueryProvider.MPAItemQuery.FindMPAItemByMPADocumentID(mpaDocument.MPADocumentID);
                        foreach (MPAItem item in items)
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
            }
            catch (Exception)
            {
                throw;
            }


            // ====================== For Approve all advance that link to TA ======================
            // Use state WaitApprove to add into table WorkflowResponse
            string eventName = "Approve";
            WorkFlowStateEvent stateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetByWorkFlowStateID_EventName(AdvanceStateID.WaitApprove, eventName);
            return "ApproveWaitApproveToComplete";
        }
        #endregion

        #region OnRejectWaitApprove
        public override string OnRejectWaitApprove(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            MPADocument mpaDocument = ScgeAccountingQueryProvider.MPADocumentQuery.GetMPADocumentByDocumentID(scgDocument.DocumentID);
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

                        IList<MPAItem> items = ScgeAccountingQueryProvider.MPAItemQuery.FindMPAItemByMPADocumentID(mpaDocument.MPADocumentID);
                        foreach (MPAItem item in items)
                        {
                            if (sendToUserID == 0)
                                sendToUserID = item.UserID.Userid;
                            else if (item.UserID.Userid != sendToUserID && !ccList.Contains(item.UserID.Userid))
                                ccList.Add(item.UserID.Userid);
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

            // Use state WaitInitial to add into table WorkflowResponse
            string eventName = "Reject";
            WorkFlowStateEvent stateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetByWorkFlowStateID_EventName(AdvanceStateID.WaitApprove, eventName);
            rejectDetailResponse.WorkFlowStateEventID = stateEvent.WorkFlowStateEventID;
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
            MPADocument mpaDocument = ScgeAccountingQueryProvider.MPADocumentQuery.GetMPADocumentByDocumentID(workFlow.Document.DocumentID);

            string eventName = "OverRole";
           
            // Use state WaitInitial to add into table WorkflowResponse
            WorkFlowStateEvent stateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetByWorkFlowStateID_EventName(AdvanceStateID.WaitInitial, eventName);

            return signal;
        }
        #endregion

        #region OnWithdrawWaitApprove
        public override string OnWithdrawWaitApprove(long workFlowID, object eventData)
        {
            string signal = base.OnWithdrawWaitApprove(workFlowID, eventData);

            // Withdraw all advance that link to TA
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            MPADocument mpaDocument = ScgeAccountingQueryProvider.MPADocumentQuery.GetMPADocumentByDocumentID(workFlow.Document.DocumentID);

            // ====================== For Withdraw all advance that link to TA ======================
            // Use state WaitInitial to add into table WorkflowResponse
            string eventName = "Withdraw";
            WorkFlowStateEvent stateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetByWorkFlowStateID_EventName(AdvanceStateID.WaitApprove, eventName);

            return signal;
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

            MPADocument mpaDocument = ScgeAccountingQueryProvider.MPADocumentQuery.GetMPADocumentByDocumentID(workFlow.Document.DocumentID);

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
                    IList<MPAItem> items = ScgeAccountingQueryProvider.MPAItemQuery.FindMPAItemByMPADocumentID(mpaDocument.MPADocumentID);
                    foreach (MPAItem item in items)
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

    }
}

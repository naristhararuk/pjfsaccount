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

using SCG.DB.Query;
using SCG.DB.BLL;

using SCG.eAccounting.DTO;
using SCG.eAccounting.BLL.WorkFlowService;
using SCG.eAccounting.Query;
using SCG.eAccounting.DAL;

using SS.DB.Query;
using SCG.DB.DTO;


namespace SCG.eAccounting.BLL.Implement.WorkFlowService
{
    public class GeneralWorkFlowService : IGeneralWorkFlowService
    {
        public IUserAccount UserAccount { get; set; }
        public ISCGEmailService SCGEmailService { get; set; }
        public IDbDocumentRunningService DbDocumentRunningService { get; set; }
        public IWorkFlowStateEventPermissionService WorkFlowStateEventPermissionService { get; set; }
        public IWorkFlowResponseTokenService WorkFlowResponseTokenService { get; set; }
        //public ParameterServices ParameterServices { get; set; }
        public ISCGSMSService SCGSMSService { get; set; }
        public IWorkFlowsmsTokenService WorkFlowsmsTokenService { get; set; }
        public ISCGDocumentService SCGDocumentService { get; set; }

        public virtual void OnDeleteDocument(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.FindProxyByIdentity(workFlow.Document.DocumentID);

            SCGDocumentService.Delete(document);
        }

        #region CanCopy{StateName}

        public bool CanCopyCancel(long workFlowID)
        {
            long creatorID = GetPermitUserCreatorIsCopy(workFlowID);
            long requesterID = GetPermitUserRequesterIsCopy(workFlowID);

            return creatorID == UserAccount.UserID || requesterID == UserAccount.UserID;
        }
        public bool CanCopyComplete(long workFlowID)
        {
            long creatorID = GetPermitUserCreatorIsCopy(workFlowID);
            long requesterID = GetPermitUserRequesterIsCopy(workFlowID);

            return creatorID == UserAccount.UserID || requesterID == UserAccount.UserID;
        }
        public bool CanCopyDraft(long workFlowID)
        {
            long creatorID = GetPermitUserCreatorIsCopy(workFlowID);
            long requesterID = GetPermitUserRequesterIsCopy(workFlowID);

            return creatorID == UserAccount.UserID || requesterID == UserAccount.UserID;
        }
        public bool CanCopyHold(long workFlowID)
        {
            long creatorID = GetPermitUserCreatorIsCopy(workFlowID);
            long requesterID = GetPermitUserRequesterIsCopy(workFlowID);

            return creatorID == UserAccount.UserID || requesterID == UserAccount.UserID;
        }
        public bool CanCopyOutstanding(long workFlowID)
        {
            long creatorID = GetPermitUserCreatorIsCopy(workFlowID);
            long requesterID = GetPermitUserRequesterIsCopy(workFlowID);

            return creatorID == UserAccount.UserID || requesterID == UserAccount.UserID;
        }
        public bool CanCopyWaitApprove(long workFlowID)
        {
            long creatorID = GetPermitUserCreatorIsCopy(workFlowID);
            long requesterID = GetPermitUserRequesterIsCopy(workFlowID);

            return creatorID == UserAccount.UserID || requesterID == UserAccount.UserID;
        }
        public bool CanCopyWaitApproveRejection(long workFlowID)
        {
            long creatorID = GetPermitUserCreatorIsCopy(workFlowID);
            long requesterID = GetPermitUserRequesterIsCopy(workFlowID);

            return creatorID == UserAccount.UserID || requesterID == UserAccount.UserID;
        }
        public bool CanCopyWaitApproveVerify(long workFlowID)
        {
            long creatorID = GetPermitUserCreatorIsCopy(workFlowID);
            long requesterID = GetPermitUserRequesterIsCopy(workFlowID);

            return creatorID == UserAccount.UserID || requesterID == UserAccount.UserID;
        }
        public bool CanCopyWaitAR(long workFlowID)
        {
            long creatorID = GetPermitUserCreatorIsCopy(workFlowID);
            long requesterID = GetPermitUserRequesterIsCopy(workFlowID);

            return creatorID == UserAccount.UserID || requesterID == UserAccount.UserID;
        }
        public bool CanCopyWaitDocument(long workFlowID)
        {
            long creatorID = GetPermitUserCreatorIsCopy(workFlowID);
            long requesterID = GetPermitUserRequesterIsCopy(workFlowID);

            return creatorID == UserAccount.UserID || requesterID == UserAccount.UserID;
        }
        public bool CanCopyWaitInitial(long workFlowID)
        {
            long creatorID = GetPermitUserCreatorIsCopy(workFlowID);
            long requesterID = GetPermitUserRequesterIsCopy(workFlowID);

            return creatorID == UserAccount.UserID || requesterID == UserAccount.UserID;
        }
        public bool CanCopyWaitPayment(long workFlowID)
        {
            long creatorID = GetPermitUserCreatorIsCopy(workFlowID);
            long requesterID = GetPermitUserRequesterIsCopy(workFlowID);

            return creatorID == UserAccount.UserID || requesterID == UserAccount.UserID;
        }
        public bool CanCopyWaitPaymentFromSAP(long workFlowID)
        {
            long creatorID = GetPermitUserCreatorIsCopy(workFlowID);
            long requesterID = GetPermitUserRequesterIsCopy(workFlowID);

            return creatorID == UserAccount.UserID || requesterID == UserAccount.UserID;
        }
        public bool CanCopyWaitRemittance(long workFlowID)
        {
            long creatorID = GetPermitUserCreatorIsCopy(workFlowID);
            long requesterID = GetPermitUserRequesterIsCopy(workFlowID);

            return creatorID == UserAccount.UserID || requesterID == UserAccount.UserID;
        }
        public bool CanCopyWaitReverse(long workFlowID)
        {
            long creatorID = GetPermitUserCreatorIsCopy(workFlowID);
            long requesterID = GetPermitUserRequesterIsCopy(workFlowID);

            return creatorID == UserAccount.UserID || requesterID == UserAccount.UserID;
        }
        public bool CanCopyWaitTA(long workFlowID)
        {
            long creatorID = GetPermitUserCreatorIsCopy(workFlowID);
            long requesterID = GetPermitUserRequesterIsCopy(workFlowID);

            return creatorID == UserAccount.UserID || requesterID == UserAccount.UserID;
        }
        public bool CanCopyWaitTAApprove(long workFlowID)
        {
            long creatorID = GetPermitUserCreatorIsCopy(workFlowID);
            long requesterID = GetPermitUserRequesterIsCopy(workFlowID);

            return creatorID == UserAccount.UserID || requesterID == UserAccount.UserID;
        }
        public bool CanCopyWaitTAInitial(long workFlowID)
        {
            long creatorID = GetPermitUserCreatorIsCopy(workFlowID);
            long requesterID = GetPermitUserRequesterIsCopy(workFlowID);

            return creatorID == UserAccount.UserID || requesterID == UserAccount.UserID;
        }
        public bool CanCopyWaitVerify(long workFlowID)
        {
            long creatorID = GetPermitUserCreatorIsCopy(workFlowID);
            long requesterID = GetPermitUserRequesterIsCopy(workFlowID);

            return creatorID == UserAccount.UserID || requesterID == UserAccount.UserID;
        }

        #endregion 

        #region CanDelete{StateName}
        public bool CanDeleteDraft(long workFlowID)
		{
			WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
			SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);

			return string.IsNullOrEmpty(document.DocumentNo);
		}
		#endregion
        
        #region Can{EventName}{StateName}

        #region CanSendDraft
        public virtual bool CanSendDraft(long workFlowID)
        {
            long userID = GetAllowUserSendDraft(workFlowID);

            if (UserAccount.UserID == userID)
                return true;
            else
                return false;
        }
        #endregion

        #region CanCancelDraft
        public bool CanCancelDraft(long workFlowID)
        {
            long userID = GetAllowUserCancelDraft(workFlowID);

            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);

            if (UserAccount.UserID == userID && !string.IsNullOrEmpty(scgDocument.DocumentNo))
                return true;
            else
                return false;
        }
        #endregion

        #region CanApproveWaitAR
        public bool CanApproveWaitAR(long workFlowID)
        {
            long userID = GetAllowUserApproveWaitAR(workFlowID);

            if (UserAccount.UserID == userID)
                return true;
            else
                return false;
        }
        #endregion

        #region CanRejectWaitAR
        public bool CanRejectWaitAR(long workFlowID)
        {
            long userID = GetAllowUserRejectWaitAR(workFlowID);
            
            if (UserAccount.UserID == userID)
                return true;
            else
                return false;
        }
        #endregion

        #region CanWithdrawWaitAR
        public bool CanWithdrawWaitAR(long workFlowID)
        {
            long userID = GetAllowUserWithdrawWaitAR(workFlowID);
            
            if (UserAccount.UserID == userID)
                return true;
            else
                return false;
        }
        #endregion

        #region CanApproveWaitInitial
        public bool CanApproveWaitInitial(long workFlowID)
        {
            long userID = GetAllowUserApproveWaitInitial(workFlowID);
            
            if (UserAccount.UserID == userID)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region CanRejectWaitInitial
        public bool CanRejectWaitInitial(long workFlowID)
        {
            long userID = GetAllowUserRejectWaitInitial(workFlowID);

            if (UserAccount.UserID == userID)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region CanWithdrawWaitInitial
        public bool CanWithdrawWaitInitial(long workFlowID)
        {
            long userID = GetAllowUserWithdrawWaitInitial(workFlowID);

            if (UserAccount.UserID == userID)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region CanOverRoleWaitInitial
        public bool CanOverRoleWaitInitial(long workFlowID)
        {
            long userID = GetAllowUserOverRoleWaitInitial(workFlowID);

            if (UserAccount.UserID == userID)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region CanApproveWaitApprove
        public bool CanApproveWaitApprove(long workFlowID)
        {
            long userID = GetAllowUserApproveWaitApprove(workFlowID);

            if (UserAccount.UserID == userID)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region CanRejectWaitApprove
        public bool CanRejectWaitApprove(long workFlowID)
        {
            long userID = GetAllowUserRejectWaitApprove(workFlowID);

            if (UserAccount.UserID == userID)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region CanWithdrawWaitApprove
        public bool CanWithdrawWaitApprove(long workFlowID)
        {
            long userID = GetAllowUserWithdrawWaitApprove(workFlowID);

            if (UserAccount.UserID == userID)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region CanOverRoleWaitApprove
        public bool CanOverRoleWaitApprove(long workFlowID)
        {
            //long userID = GetAllowUserOverRoleWaitApprove(workFlowID);

            //if (UserAccount.UserID == userID)
            //{
            //    return true;
            //}

            return false;
        }
        #endregion

        #endregion

        #region On{EventName}{StateName}

        #region OnSendDraft
        public virtual string OnSendDraft(long workFlowID, object eventData)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            
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

        #region OnCancelDraft
        public virtual string OnCancelDraft(long workFlowID, object eventData)
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
            string signal = "CancelDraftToCancel";

            return signal;
        }
        #endregion


        #region OnApproveWaitAR
        public virtual string OnApproveWaitAR(long workFlowID, object eventData)
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
                if(submitResponse.ResponseMethod != null)
                    response.ResponseMethod = submitResponse.ResponseMethod.GetHashCode().ToString();
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
             */
            string signal = string.Empty;
            IList<DocumentInitiator> initiators = ScgeAccountingQueryProvider.DocumentInitiatorQuery.GetNotResponseInitiatorByDocumentID(workFlow.Document.DocumentID);
            if (initiators.Count > 0)
                signal = "ApproveWaitARToWaitInitial";
            else
                signal = "ApproveWaitARToWaitApprove";

            return signal;
        }
        #endregion

        #region OnRejectWaitAR
        public virtual string OnRejectWaitAR(long workFlowID, object eventData)
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

                SCGEmailService.SendEmailEM04(response.WorkFlowResponseID, scgDocument.CreatorID.Userid);
            
            }
            catch (Exception)
            {
                throw;
            }

            /*TODO generate signal string*/
            string signal = "RejectWaitARToDraft";

            return signal;
        }
        #endregion

        #region OnWithdrawWaitAR
        public virtual string OnWithdrawWaitAR(long workFlowID, object eventData)
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
                response.ResponseDate = DateTime.Now;
                response.ResponseBy = UserAccount.UserID;
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);

                if (ParameterServices.EnableEmail02Creator)
                {
                    SCGEmailService.SendEmailEM02(response.WorkFlowResponseID, scgDocument.CreatorID.Userid, null);
                }
            }
            catch (Exception)
            {
                throw;
            }
            /*TODO generate signal string*/
            string signal = "WithdrawWaitARToDraft";

            return signal;
        }
        #endregion


        #region OnApproveWaitInitial
        public virtual string OnApproveWaitInitial(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow;
            try
            {
                SubmitResponse submitResponse = eventData as SubmitResponse;

                workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

                //Flag DoApprove = true
                IList<DocumentInitiator> initators = ScgeAccountingQueryProvider.DocumentInitiatorQuery.FindDocumentInitiatorByDocumentID_UserID(workFlow.Document.DocumentID, UserAccount.UserID);
                if (initators.Count > 0)
                {
                    DocumentInitiator initiator = ScgeAccountingQueryProvider.DocumentInitiatorQuery.FindByIdentity(initators[0].InitiatorID);

                    initiator.DoApprove = true;
                    initiator.UpdBy = UserAccount.UserID;
                    initiator.UpdDate = DateTime.Now;
                    initiator.UpdPgm = UserAccount.CurrentProgramCode;
                    SCG.eAccounting.DAL.ScgeAccountingDaoProvider.DocumentInitiatorDao.Update(initiator);
                }

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
            }
            catch (Exception)
            {
                throw;
            }
            /*  TODO generate signal string
             *  Case 1 : ApproveWaitInitialToWaitInitial
             *  Case 2 : ApproveWaitInitialToWaitApprove
             */
            string signal = string.Empty;
            IList<DocumentInitiator> initiators = ScgeAccountingQueryProvider.DocumentInitiatorQuery.GetNotResponseInitiatorByDocumentID(workFlow.Document.DocumentID);
            if (initiators.Count > 0)
                signal = "ApproveWaitInitialToWaitInitial";
            else
                signal = "ApproveWaitInitialToWaitApprove";

            return signal;
        }
        #endregion

        #region OnRejectWaitInitial
        public virtual string OnRejectWaitInitial(long workFlowID, object eventData)
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

                SCGEmailService.SendEmailEM04(response.WorkFlowResponseID, scgDocument.CreatorID.Userid);

                if (ParameterServices.EnableEmail02Requester || ParameterServices.EnableEmail02Initiator)
                {
                    long sendToUserID = 0;
                    IList<long> ccList = new List<long>();

                    if (ParameterServices.EnableEmail02Requester)
                    {
                        if (scgDocument.CreatorID.Userid != scgDocument.RequesterID.Userid)
                            sendToUserID = scgDocument.RequesterID.Userid;
                    }

                    if (ParameterServices.EnableEmail02Initiator)
                    {
                        IList<DocumentInitiator> approvedInitiators = ScgeAccountingQueryProvider.DocumentInitiatorQuery.GetResponsedInitiatorByDocumentID(workFlow.Document.DocumentID);
                        foreach (DocumentInitiator approvedInitiator in approvedInitiators)
                        {
                            if (sendToUserID == 0)
                                sendToUserID = approvedInitiator.UserID.Userid;
                            else if (approvedInitiator.UserID.Userid != sendToUserID && !ccList.Contains(approvedInitiator.UserID.Userid))
                                ccList.Add(approvedInitiator.UserID.Userid);
                        }
                    }

                    SCGEmailService.SendEmailEM02(response.WorkFlowResponseID, sendToUserID, ccList);
                }
            }
            catch (Exception)
            {
                throw;
            }

            /*TODO generate signal string*/
            string signal = "RejectWaitInitialToDraft";

            return signal;
        }
        #endregion

        #region OnWithdrawWaitInitial
        public virtual string OnWithdrawWaitInitial(long workFlowID, object eventData)
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

                if (ParameterServices.EnableEmail02Requester || ParameterServices.EnableEmail02Initiator)
                {
                    long sendToUserID = 0;
                    IList<long> ccList = new List<long>();

                    if (ParameterServices.EnableEmail02Requester)
                    {
                        if (scgDocument.CreatorID.Userid != scgDocument.RequesterID.Userid)
                            sendToUserID = scgDocument.RequesterID.Userid;
                    }
                    if (ParameterServices.EnableEmail02Initiator)
                    {
                        IList<DocumentInitiator> responsedInitiators = ScgeAccountingQueryProvider.DocumentInitiatorQuery.GetResponsedInitiatorByDocumentID(workFlow.Document.DocumentID);
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
            /*TODO generate signal string*/
            string signal = "WithdrawWaitInitialToDraft";

            return signal;
        }
        #endregion

        #region OnOverRoleWaitInitial
        public virtual string OnOverRoleWaitInitial(long workFlowID, object eventData)
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


                IList<DocumentInitiator> overRoleInitiators = ScgeAccountingQueryProvider.DocumentInitiatorQuery.GetOverRoleInitiatorByDocumentID(workFlow.Document.DocumentID);
                foreach (DocumentInitiator initiator in overRoleInitiators)
                {
                    SCGEmailService.SendEmailEM03(workFlowID, initiator.UserID.Userid);
                }
                SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);

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
            /*  TODO generate signal string
             *  Case 1 : OverRoleWaitInitialToWaitInitial
             *  Case 2 : OverRoleWaitInitialToWaitApprove
             */
            string signal = string.Empty;
            IList<DocumentInitiator> initiators = ScgeAccountingQueryProvider.DocumentInitiatorQuery.GetNotResponseInitiatorByDocumentID(workFlow.Document.DocumentID);
            if (initiators.Count > 0)
                signal = "OverRoleWaitInitialToWaitInitial";
            else
                signal = "OverRoleWaitInitialToWaitApprove";



            return signal;
        }
        #endregion


        #region OnApproveWaitApprove
        public virtual string OnApproveWaitApprove(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            int workFlowStateEventId;
            WorkFlowResponse response = new WorkFlowResponse();
            try
            {
                RejectDetailResponse rejectDetailResponse;

                if (eventData is RejectDetailResponse)
                {

                    rejectDetailResponse = eventData as RejectDetailResponse;
                    workFlowStateEventId = rejectDetailResponse.WorkFlowStateEventID;
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
                    if (rejectDetailResponse.ReasonID != 0 || !string.IsNullOrEmpty(rejectDetailResponse.Remark))
                        WorkFlowDaoProvider.WorkFlowRejectResponseDao.Save(rejectResponse);

                    if (rejectDetailResponse.ResponseMethod != null)
                        response.ResponseMethod = rejectDetailResponse.ResponseMethod.GetHashCode().ToString();
                }
                else
                {
                    SubmitResponse submitResponse = eventData as SubmitResponse;
                    workFlowStateEventId = submitResponse.WorkFlowStateEventID;
                    if (submitResponse.ResponseMethod != null)
                        response.ResponseMethod = submitResponse.ResponseMethod.GetHashCode().ToString();
                }

                response.WorkFlow = workFlow;
                response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(workFlowStateEventId);
                response.ResponseBy = UserAccount.UserID;
                response.ResponseDate = DateTime.Now;
                
                response.Active = true;
                response.CreBy = UserAccount.UserID;
                response.CreDate = DateTime.Now;
                response.UpdBy = UserAccount.UserID;
                response.UpdDate = DateTime.Now;
                response.UpdPgm = UserAccount.CurrentProgramCode;
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);


                scgDocument.ApproveDate = DateTime.Now;
                scgDocument.UpdBy = UserAccount.UserID;
                scgDocument.UpdDate = DateTime.Now;
                scgDocument.UpdPgm = UserAccount.CurrentProgramCode;
                SCGDocumentService.SaveOrUpdate(scgDocument);
                ////SubmitResponse submitResponse = eventData as SubmitResponse;                

                ////WorkFlowResponse response = new WorkFlowResponse();
                ////response.WorkFlow = workFlow;
                ////response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(submitResponse.WorkFlowStateEventID);
                ////response.ResponseBy = UserAccount.UserID;
                ////response.ResponseDate = DateTime.Now;
                ////if (submitResponse.ResponseMethod != null)
                ////    response.ResponseMethod = submitResponse.ResponseMethod.GetHashCode().ToString();
                ////response.Active = true;
                ////response.CreBy = UserAccount.UserID;
                ////response.CreDate = DateTime.Now;
                ////response.UpdBy = UserAccount.UserID;
                ////response.UpdDate = DateTime.Now;
                ////response.UpdPgm = UserAccount.CurrentProgramCode;
                ////WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);
                if (ParameterServices.EnableEmail02Creator || ParameterServices.EnableEmail02Requester || ParameterServices.EnableEmail02Initiator)
                {
                    long sendToUserID = 0;
                    IList<long> ccList = new List<long>();

                    if (ParameterServices.EnableEmail02Creator)
                    {
                        sendToUserID = scgDocument.CreatorID.Userid;
                    }
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

        #region OnRejectWaitApprove
        public virtual string OnRejectWaitApprove(long workFlowID, object eventData)
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

                //ถ้าเป็น Approver ไม่ต้อง validate reject response
                //ValidateRejectResponse(rejectDetailResponse);

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

                SCGEmailService.SendEmailEM04(response.WorkFlowResponseID, scgDocument.CreatorID.Userid);

                if (ParameterServices.EnableEmail02Requester || ParameterServices.EnableEmail02Initiator || ParameterServices.EnableEmail02CC)
                {
                    long sendToUserID = 0;
                    IList<long> ccList = new List<long>();

                    if (ParameterServices.EnableEmail02Requester)
                    {
                        if (scgDocument.CreatorID.Userid != scgDocument.RequesterID.Userid)
                            sendToUserID = scgDocument.RequesterID.Userid;
                    }
                    if (ParameterServices.EnableEmail02Initiator)
                    {
                        IList<DocumentInitiator> initiators = ScgeAccountingQueryProvider.DocumentInitiatorQuery.GetResponsedInitiatorByDocumentID(workFlow.Document.DocumentID);
                        foreach (DocumentInitiator initiator in initiators)
                        {
                            if (sendToUserID == 0)
                                sendToUserID = initiator.UserID.Userid;
                            else if (initiator.UserID.Userid != sendToUserID && !ccList.Contains(initiator.UserID.Userid))
                                ccList.Add(initiator.UserID.Userid);
                        }
                    }
                    if (ParameterServices.EnableEmail02CC)
                    {
                        IList<DocumentInitiator> ccInitiators = ScgeAccountingQueryProvider.DocumentInitiatorQuery.GetCCInitiatorByDocumentID(workFlow.Document.DocumentID);
                        foreach (DocumentInitiator initiator in ccInitiators)
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
            /*TODO generate signal string*/
            string signal = "RejectWaitApproveToDraft";

            return signal;
        }
        #endregion

        #region OnWithdrawWaitApprove
        public virtual string OnWithdrawWaitApprove(long workFlowID, object eventData)
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

                if (ParameterServices.EnableEmail02Requester || ParameterServices.EnableEmail02Initiator || ParameterServices.EnableEmail02Approver)
                {
                    long sendToUserID = 0;
                    IList<long> ccList = new List<long>();

                    if (ParameterServices.EnableEmail02Requester)
                    {
                        if (scgDocument.CreatorID.Userid != scgDocument.RequesterID.Userid)
                            sendToUserID = scgDocument.RequesterID.Userid;
                    }

                    if (ParameterServices.EnableEmail02Approver)
                    {
                        if (sendToUserID == 0)
                            sendToUserID = scgDocument.ApproverID.Userid;
                        else if (scgDocument.ApproverID.Userid != sendToUserID)
                            ccList.Add(scgDocument.ApproverID.Userid);
                    }

                    if (ParameterServices.EnableEmail02Initiator)
                    {
                        IList<DocumentInitiator> initiators = ScgeAccountingQueryProvider.DocumentInitiatorQuery.GetResponsedInitiatorByDocumentID(workFlow.Document.DocumentID);
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
            /*TODO generate signal string*/
            string signal = "WithdrawWaitApproveToDraft";

            return signal;
        }
        #endregion

        #region OnOverRoleWaitApprove
        public virtual string OnOverRoleWaitApprove(long workFlowID, object eventData)
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
            string signal = "OverRoleWaitApproveToWaitApprove";

            return signal;
        }
        #endregion

        #endregion

        #region OnEnter{StateName} , OnExit{StateName}
        public virtual void OnEnterDraft(long workFlowID)
        {
            //Clear flag DoApprove on DocumentInitiators
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

            IList<DocumentInitiator> initiators = ScgeAccountingQueryProvider.DocumentInitiatorQuery.GetDocumentInitiatorByDocumentID(workFlow.Document.DocumentID);
            foreach (DocumentInitiator initiator in initiators)
            {
                initiator.DoApprove = false;
                initiator.IsSkip = false;
                initiator.SkipReason = null;
                SCG.eAccounting.DAL.ScgeAccountingDaoProvider.DocumentInitiatorDao.Update(initiator);
            }

        }
        public void OnExitDraft(long workFlowID)
        {
            
        }

        public void OnEnterWaitAR(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);

            long sendToUserID = scgDocument.RequesterID.Userid;

            string tokenCode = SaveResponseTokenEmail(workFlowID, sendToUserID);
            SCGEmailService.SendEmailEM01(workFlowID, sendToUserID, tokenCode);

            //#1
            SCGSMSService.SendSMS01(workFlowID, sendToUserID);


        }
        public void OnExitWaitAR(long workFlowID)
        {

        }

        public void OnEnterWaitInitial(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            DocumentInitiator nextInitiator = ScgeAccountingQueryProvider.DocumentInitiatorQuery.GetNextResponseInitiatorByDocumentID(workFlow.Document.DocumentID);

            long sendToUserID = nextInitiator.UserID.Userid;

            string tokenCode = SaveResponseTokenEmail(workFlowID, sendToUserID);
            SCGEmailService.SendEmailEM01(workFlowID, sendToUserID, tokenCode);

            //#2
                SCGSMSService.SendSMS01(workFlowID, sendToUserID);


        }
        public void OnExitWaitInitial(long workFlowID)
        {

        }

        public virtual void OnEnterWaitApprove(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);

            long sendToUserID = scgDocument.ApproverID.Userid;

            string tokenCode = SaveResponseTokenEmail(workFlowID, sendToUserID);
            SCGEmailService.SendEmailEM01(workFlowID, sendToUserID, tokenCode);

            //#3
                SCGSMSService.SendSMS01(workFlowID, sendToUserID);


        }
        public void OnExitWaitApprove(long workFlowID)
        {

        }

        public void OnEnterComplete(long workFlowID)
        {

        }
        public void OnExitComplete(long workFlowID)
        {

        }

        public void OnEnterCancel(long workFlowID)
        {

        }
        public void OnExitCancel(long workFlowID)
        {

        }
        #endregion

        #region GetPermitUser{EventName}
        public virtual long GetPermitUserDraft(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            return scgDocument.CreatorID.Userid;
        }

        public virtual long GetPermitUserHold(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            return scgDocument.CreatorID.Userid;
        }

        public virtual long GetPermitUserCreatorIsCopy(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            return scgDocument.CreatorID.Userid;
        }

        public virtual long GetPermitUserRequesterIsCopy(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            return scgDocument.RequesterID.Userid;
        }

        #endregion

        #region GetAllowUser{EventName}{StateName}

        public long GetAllowUserSendDraft(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            return scgDocument.CreatorID.Userid;
        }

        public long GetAllowUserCancelDraft(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            return scgDocument.CreatorID.Userid;
        }

        public long GetAllowUserApproveWaitAR(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            return scgDocument.RequesterID.Userid;
        }

        public long GetAllowUserRejectWaitAR(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            return scgDocument.RequesterID.Userid;
        }

        public long GetAllowUserWithdrawWaitAR(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            return scgDocument.CreatorID.Userid;
        }

        public long GetAllowUserApproveWaitInitial(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            IList<DocumentInitiator> initiators = ScgeAccountingQueryProvider.DocumentInitiatorQuery.GetNotResponseInitiatorByDocumentID(workFlow.Document.DocumentID);
            //User must be the smallest seq no in DocumentInitiators
            return initiators[0].UserID.Userid;
        }

        public long GetAllowUserRejectWaitInitial(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            IList<DocumentInitiator> initiators = ScgeAccountingQueryProvider.DocumentInitiatorQuery.GetNotResponseInitiatorByDocumentID(workFlow.Document.DocumentID);
            //User must be the smallest seq no in DocumentInitiators
            return initiators[0].UserID.Userid;
        }

        public long GetAllowUserWithdrawWaitInitial(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            return scgDocument.CreatorID.Userid;
        }

        public long GetAllowUserOverRoleWaitInitial(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            return scgDocument.CreatorID.Userid;
        }

        public long GetAllowUserApproveWaitApprove(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            return scgDocument.ApproverID.Userid;
        }

        public long GetAllowUserRejectWaitApprove(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            return scgDocument.ApproverID.Userid;
        }

        public long GetAllowUserWithdrawWaitApprove(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            return scgDocument.CreatorID.Userid;
        }

        public long GetAllowUserOverRoleWaitApprove(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            return scgDocument.CreatorID.Userid;
        }

        #endregion

        #region ReCalculatePermissionFor{EventName}{StateName}(long workFlowID)


        public void ReCalculatePermissionForSendDraft(long workFlowID , int workFlowStateEventID)
        {
            long permitUserID = GetAllowUserSendDraft(workFlowID);
            
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

        public void ReCalculatePermissionForCancelDraft(long workFlowID, int workFlowStateEventID)
        {
            long permitUserID = GetAllowUserCancelDraft(workFlowID);

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

        public void ReCalculatePermissionForApproveWaitAR(long workFlowID, int workFlowStateEventID)
        {
            long permitUserID = GetAllowUserApproveWaitAR(workFlowID);

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

        public void ReCalculatePermissionForRejectWaitAR(long workFlowID, int workFlowStateEventID)
        {
            long permitUserID = GetAllowUserRejectWaitAR(workFlowID);

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

        public void ReCalculatePermissionForWithdrawWaitAR(long workFlowID, int workFlowStateEventID)
        {
            long permitUserID = GetAllowUserWithdrawWaitAR(workFlowID);

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

        public void ReCalculatePermissionForWithdrawWaitInitial(long workFlowID, int workFlowStateEventID)
        {
            long permitUserID = GetAllowUserWithdrawWaitInitial(workFlowID);

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

        public void ReCalculatePermissionForOverRoleWaitInitial(long workFlowID, int workFlowStateEventID)
        {
            long permitUserID = GetAllowUserOverRoleWaitInitial(workFlowID);

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

        public void ReCalculatePermissionForApproveWaitInitial(long workFlowID, int workFlowStateEventID)
        {
            long permitUserID = GetAllowUserApproveWaitInitial(workFlowID);

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

        public void ReCalculatePermissionForRejectWaitInitial(long workFlowID, int workFlowStateEventID)
        {
            long permitUserID = GetAllowUserRejectWaitInitial(workFlowID);

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

        public void ReCalculatePermissionForWithdrawWaitApprove(long workFlowID, int workFlowStateEventID)
        {
            long permitUserID = GetAllowUserWithdrawWaitApprove(workFlowID);

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

        public void ReCalculatePermissionForOverRoleWaitApprove(long workFlowID, int workFlowStateEventID)
        {
            long permitUserID = GetAllowUserOverRoleWaitApprove(workFlowID);

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

        public void ReCalculatePermissionForApproveWaitApprove(long workFlowID, int workFlowStateEventID)
        {
            long permitUserID = GetAllowUserApproveWaitApprove(workFlowID);

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

        public void ReCalculatePermissionForRejectWaitApprove(long workFlowID, int workFlowStateEventID)
        {
            long permitUserID = GetAllowUserRejectWaitApprove(workFlowID);

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

        public void ReCalculatePermission(short roleID)
        {
            throw new NotImplementedException();
        }

        public void ReCalculatePermission()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods
        private string SaveResponseTokenEmail(long workFlowID, long userID)
        {
            WorkFlowResponseTokenService.ClearResponseTokenByWorkFlowID(workFlowID, TokenType.Email);

            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindProxyByIdentity(workFlowID);

            string tokenCode = Guid.NewGuid().ToString();
            IList<WorkFlowStateEventPermission> stateEventPermissions = WorkFlowQueryProvider.WorkFlowStateEventPermissionQuery.FindByWorkFlowID_UserID(workFlowID , userID);
            foreach (WorkFlowStateEventPermission stateEventPermission in stateEventPermissions)
            {
                WorkFlowResponseToken responseToken = new WorkFlowResponseToken();
                responseToken.TokenCode = tokenCode;
                responseToken.UserID = stateEventPermission.UserID.Value;
                responseToken.WorkFlow = workFlow;
                responseToken.TokenType = TokenType.Email.ToString();
                responseToken.WorkFlowStateEvent = stateEventPermission.WorkFlowStateEvent;
                responseToken.Active = true;
                responseToken.CreBy = UserAccount.UserID;
                responseToken.CreDate = DateTime.Now;
                responseToken.UpdBy = UserAccount.UserID;
                responseToken.UpdDate = DateTime.Now;
                responseToken.UpdPgm = UserAccount.CurrentProgramCode;

                WorkFlowResponseTokenService.Save(responseToken);
            }

            

            return tokenCode;
        }

        

        protected void ValidateRejectResponse(RejectDetailResponse rejectDetailResponse)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (rejectDetailResponse.ReasonID == null || rejectDetailResponse.ReasonID == 0)
            {
                errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("WorkFlow_ValidateRejectResponse_ReasonRequired"));
            }

            if (!errors.IsEmpty) throw new SS.Standard.Utilities.ServiceValidationException(errors);
        }


        public virtual double GetTotalDocumentAmount(long documentID)
        {
            return 0;
        }
        #endregion

        protected RejectDetailResponse DefaultRejectDetailResponse(int workflowStateEventID , ResponseMethod responseMethod)
        {
            RejectDetailResponse rejectDetailResponse = new RejectDetailResponse();
            
            rejectDetailResponse.ReasonID = SCG.DB.Query.ScgDbQueryProvider.RejectReasonQuery
                .FindByCode(ParameterServices.DefaultRejectReasonCodeFromMail).ReasonID;

            rejectDetailResponse.WorkFlowStateEventID = workflowStateEventID;
            rejectDetailResponse.ResponseMethod = responseMethod;
            return rejectDetailResponse;
        }
    }
}

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

using SCG.eAccounting.DTO;
using SCG.eAccounting.BLL.WorkFlowService;
using SCG.eAccounting.BLL.Implement;
using SCG.eAccounting.Query;
using SCG.eAccounting.DAL;

using SCG.DB.BLL;

using SS.SU.DTO;
using SS.SU.Query;
using SCG.DB.Query;
using SS.Standard.Utilities;
using SS.DB.Query;
using SCG.eAccounting.SAP.BAPI.Service;
using SCG.eAccounting.SAP.BAPI.Service.Posting;
using SCG.eAccounting.SAP.BAPI.Service.Const;


namespace SCG.eAccounting.BLL.Implement.WorkFlowService
{
    public class RemittanceWorkFlowService : IRemittanceWorkFlowService
    {
        public IUserAccount UserAccount { get; set; }
        public ISCGEmailService SCGEmailService { get; set; }
        public IDbDocumentRunningService DbDocumentRunningService { get; set; }
        public IWorkFlowService WorkFlowService { get; set; }
        public IWorkFlowStateEventPermissionService WorkFlowStateEventPermissionService { get; set; }
        public ISCGDocumentService SCGDocumentService { get; set; }
        public IAvAdvanceDocumentService AvAdvanceDocumentService { get; set; }

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
        public bool CanCopyWaitApproveRemittance(long workFlowID)
        {
            long creatorID = GetPermitUserCreatorIsCopy(workFlowID);
            long requesterID = GetPermitUserRequesterIsCopy(workFlowID);

            return creatorID == UserAccount.UserID || requesterID == UserAccount.UserID;
        }
        public bool CanCopyWaitRemittance(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);

            return string.IsNullOrEmpty(document.DocumentNo);
        }

        #endregion

        public bool CanEditDraft(long workFlowID)
        {
            long userID = GetPermitUserDraft(workFlowID);
            return userID == UserAccount.UserID;
        }

        public bool CanEditWaitRemittance(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleVerifyWaitRemittance(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }

        public bool CanDeleteDraft(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);

            return string.IsNullOrEmpty(document.DocumentNo);
        }

        public virtual void DeleteDocument(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.FindProxyByIdentity(workFlow.Document.DocumentID);

            SCGDocumentService.Delete(document);
        }

        #region Can{EventName}{StateName}

        #region CanSendDraft
        public bool CanSendDraft(long workFlowID)
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
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);

            if (UserAccount.UserID == scgDocument.CreatorID.Userid)
                return true;
            else
                return false;
        }
        #endregion

        #region CanVerifyWaitRemittance
        public bool CanVerifyWaitRemittance(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleVerifyWaitRemittance(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #region CanRejectWaitRemittance
        public bool CanRejectWaitRemittance(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleRejectWaitRemittance(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #region CanWithdrawWaitRemittance
        public bool CanWithdrawWaitRemittance(long workFlowID)
        {
            long userID = GetAllowUserWithdrawWaitRemittance(workFlowID);

            if (UserAccount.UserID == userID)
                return true;
            else
                return false;
        }
        #endregion


        #region CanVerifyAndApproveVerifyWaitRemittance
        public bool CanVerifyAndApproveVerifyWaitRemittance(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleVerifyAndApproveVerifyWaitRemittance(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #region CanApproveVerifyWaitApproveRemittance
        public bool CanApproveWaitApproveRemittance(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleApproveWaitApproveRemittance(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #region CanRejectWaitApproveRemittance
        public bool CanRejectWaitApproveRemittance(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleRejectWaitApproveRemittance(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #endregion

        #region On{EventName}{StateName}

        #region OnSendDraft
        public string OnSendDraft(long workFlowID, object eventData)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            WorkFlowResponse response = new WorkFlowResponse();
            /*TODO validate , save event data to workflow*/
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
             *  SendDraftToWaitRemittance
             */
            string signal = "SendDraftToWaitRemittance";

            if (ParameterServices.EnableEmail02Requester)
            {
                if (scgDocument.CreatorID.Userid != scgDocument.RequesterID.Userid)
                    SCGEmailService.SendEmailEM02(response.WorkFlowResponseID, scgDocument.RequesterID.Userid, null);
            }

            return signal;
        }
        #endregion

        #region OnCancelDraft
        public string OnCancelDraft(long workFlowID, object eventData)
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
                response.UpdPgm = "WorkFlow";
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);

                // Add code for clear remittance amount on advance when cancel Remittance by Anuwat S on 22/06/2009
                FnRemittance remittance = ScgeAccountingQueryProvider.FnRemittanceQuery.GetFnRemittanceByDocumentID(workFlow.Document.DocumentID);
                bool isRepOffice = remittance.IsRepOffice.HasValue ? remittance.IsRepOffice.Value : false;

                AvAdvanceDocumentService.UpdateRemittanceAdvance(remittance.RemittanceID, 0, true);
                if (isRepOffice)
                {
                    AvAdvanceDocumentService.UpdateRemittanceAdvanceForRepOffice(remittance.RemittanceID, 0, true);
                }

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

        #region OnRejectWaitRemittance
        public string OnRejectWaitRemittance(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

            //Validate Posting Status
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            if (scgDocument.PostingStatus != null && scgDocument.PostingStatus != PostingStaus.New)
            {
                errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("Workflow_OnRejectWaitRemittance_Mismatch_PostingStatus"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);


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
            string signal = "RejectWaitRemittanceToDraft";

            return signal;
        }
        #endregion

        #region OnWithdrawWaitRemittance
        public string OnWithdrawWaitRemittance(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

            //Validate Posting Status
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            if (scgDocument.PostingStatus != null && scgDocument.PostingStatus != PostingStaus.New)
            {
                errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("Workflow_OnWithdrawWaitRemittance_Mismatch_PostingStatus"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

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
            }
            catch (Exception)
            {
                throw;
            }

            /*TODO generate signal string*/
            string signal = "WithdrawWaitRemittanceToDraft";

            if (ParameterServices.EnableEmail02Requester)
            {
                if (scgDocument.CreatorID.Userid != scgDocument.RequesterID.Userid)
                    SCGEmailService.SendEmailEM02(response.WorkFlowResponseID, scgDocument.RequesterID.Userid, null);
            }
            return signal;
        }
        #endregion

        public string OnVerifyWaitRemittance(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

            //Validate Posting Status
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            if (document.PostingStatus != PostingStaus.Posted && document.PostingStatus != PostingStaus.Complete)
            {
                errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("Workflow_OnVerifyWaitRemittance_Mismatch_PostingStatus"));
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
                response.UpdPgm = "WorkFlow";
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);
            }
            catch (Exception)
            {
                throw;
            }
            /*  TODO generate signal string
             *  Case 1 : VerifyWaitRemittanceToWaitApproveRemittance
             */
            string signal = "VerifyWaitRemittanceToWaitApproveRemittance";

            return signal;
        }

        public string OnVerifyAndApproveVerifyWaitRemittance(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

            //Validate Posting Status
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            if (document.PostingStatus != PostingStaus.Complete)
            {
                errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("Workflow_OnVerifyAndApproveWaitRemittance_Mismatch_PostingStatus"));
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
                response.UpdPgm = "WorkFlow";
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);
            }
            catch (Exception)
            {
                throw;
            }
            /*  TODO generate signal string
             *  Case 1 : VerifyAndApproveVerifyWaitRemittanceToComplete
             */
            string signal = "VerifyAndApproveVerifyWaitRemittanceToComplete";

            this.ClearingAdvance(document.DocumentID);

            return signal;
        }

        public string OnApproveWaitApproveRemittance(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

            //Validate Posting Status
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);

            if (document.PostingStatus != PostingStaus.Complete)
            {
                RemittancePostingService postingService = new RemittancePostingService();
                IList<BAPIApproveReturn> bapiReturn = postingService.BAPIApprove(document.DocumentID, DocumentKind.Remittance.ToString(), UserAccount.UserID);

                if (postingService.GetDocumentStatus(document.DocumentID, DocumentKind.Remittance.ToString()) == "A")
                {
                    document.PostingStatus = "C";
                    SCGDocumentService.SaveOrUpdate(document);
                }
                else if (postingService.GetDocumentStatus(document.DocumentID, DocumentKind.Remittance.ToString()) == "PP")
                {
                    document.PostingStatus = "PP";
                    SCGDocumentService.SaveOrUpdate(document);
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
                response.UpdPgm = "WorkFlow";
                WorkFlowDaoProvider.WorkFlowResponseDao.Save(response);
            }
            catch (Exception)
            {
                throw;
            }
            /*  TODO generate signal string
             *  Case 1 : ApproveWaitApproveRemittanceToComplete
             */
            string signal = "ApproveWaitApproveRemittanceToComplete";

            this.ClearingAdvance(document.DocumentID);

            return signal;
        }

        public string OnRejectWaitApproveRemittance(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

            //Validate Posting Status
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            if (scgDocument.PostingStatus != PostingStaus.Posted)
            {
                errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("Workflow_OnRejectWaitApproveRemittance_Mismatch_PostingStatus"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);


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
                }
            catch (Exception)
            {
                throw;
            }

            /*TODO generate signal string*/
            string signal = "RejectWaitApproveRemittanceToWaitRemittance";

            return signal;
        }

        #endregion

        #region OnEnter{StateName} , OnExit{StateName}
        public void OnEnterDraft(long workFlowID)
        {

        }

        public void OnExitDraft(long workFlowID)
        {

        }

        public void OnEnterWaitRemittance(long workFlowID)
        {

        }

        public void OnExitWaitRemittance(long workFlowID)
        {

        }

        public void OnEnterWaitApproveRemittance(long workFlowID)
        {

        }

        public void OnExitWaitApproveRemittance(long workFlowID)
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
        public long GetPermitUserDraft(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            return scgDocument.CreatorID.Userid;
        }

        #endregion

        #region GetPermitRole{EventName}

        public IList<SuRole> GetPermitRoleVerifyPayment(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            FnRemittance rmtDocument = ScgeAccountingQueryProvider.FnRemittanceQuery.GetFnRemittanceByDocumentID(workFlow.Document.DocumentID);

            IList<SuRole> permitRoles = QueryProvider.SuUserRoleQuery.GetRoleVerifyPayment();
            if (permitRoles.Count == 0) return permitRoles;
            //Check PB
            long pbId = 0;
            if (rmtDocument.PB != null)
                pbId = rmtDocument.PB.Pbid;
            IList<SuRole> cashierRoles = QueryProvider.SuRolePBQuery.FindRoleByPBID(pbId);
            var documentPermitRoles = from role in permitRoles
                                      join cashierRole in cashierRoles
                                      on role.RoleID equals cashierRole.RoleID
                                      select role;

            return documentPermitRoles.ToList<SuRole>();
        }

        public IList<SuRole> GetPermitRoleApproveVerifyPayment(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            FnRemittance rmtDocument = ScgeAccountingQueryProvider.FnRemittanceQuery.GetFnRemittanceByDocumentID(workFlow.Document.DocumentID);

            IList<SuRole> permitRoles = QueryProvider.SuUserRoleQuery.GetRoleApproveVerifyPayment();
            if (permitRoles.Count == 0) return permitRoles;
            //Check PB
            long pbId = 0;
            if (rmtDocument.PB != null)
                pbId = rmtDocument.PB.Pbid;
            IList<SuRole> cashierRoles = QueryProvider.SuRolePBQuery.FindRoleByPBID(pbId);
            var documentPermitRoles = from role in permitRoles
                                      join cashierRole in cashierRoles
                                      on role.RoleID equals cashierRole.RoleID
                                      select role;

            return documentPermitRoles.ToList<SuRole>();
        }

        public IList<SuRole> GetPermitRoleVerifyAndApproveVerifyPayment(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            FnRemittance rmtDocument = ScgeAccountingQueryProvider.FnRemittanceQuery.GetFnRemittanceByDocumentID(workFlow.Document.DocumentID);

            IList<SuRole> permitRoles = QueryProvider.SuUserRoleQuery.GetRoleVerifyAndApproveVerifyPayment();
            if (permitRoles.Count == 0) return permitRoles;
            //Check PB
            long pbId = 0;
            if (rmtDocument.PB != null)
                pbId = rmtDocument.PB.Pbid;
            IList<SuRole> cashierRoles = QueryProvider.SuRolePBQuery.FindRoleByPBID(pbId);
            var documentPermitRoles = from role in permitRoles
                                      join cashierRole in cashierRoles
                                      on role.RoleID equals cashierRole.RoleID
                                      select role;

            return documentPermitRoles.ToList<SuRole>();
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
            return GetPermitUserDraft(workFlowID);
        }

        public long GetAllowUserWithdrawWaitRemittance(long workFlowID)
        {
            return GetPermitUserDraft(workFlowID);
        }

        #endregion

        #region GetAllowRole{EventName}{StateName}

        public IList<SuRole> GetAllowRoleRejectWaitRemittance(long workFlowID)
        {
            return GetPermitRoleVerifyPayment(workFlowID);
        }

        public IList<SuRole> GetAllowRoleVerifyWaitRemittance(long workFlowID)
        {
            return GetPermitRoleVerifyPayment(workFlowID);
        }

        public IList<SuRole> GetAllowRoleVerifyAndApproveVerifyWaitRemittance(long workFlowID)
        {
            return GetPermitRoleVerifyAndApproveVerifyPayment(workFlowID);
        }

        public IList<SuRole> GetAllowRoleApproveWaitApproveRemittance(long workFlowID)
        {
            return GetPermitRoleApproveVerifyPayment(workFlowID);
        }

        public IList<SuRole> GetAllowRoleRejectWaitApproveRemittance(long workFlowID)
        {
            return GetPermitRoleApproveVerifyPayment(workFlowID);
        }
        #endregion

        #region GetEditableFieldsFor{StateName}

        public IList<object> GetEditableFieldsForDraft(long workFlowID)
        {
            IList<object> editableFieldEnum = new List<object>();

            long userID = GetAllowUserSendDraft(workFlowID);
            if (UserAccount.UserID == userID)
            {
                editableFieldEnum.Add(RemittanceFieldGroup.All);
                editableFieldEnum.Add(RemittanceFieldGroup.FullClearing);

                WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
                if (string.IsNullOrEmpty(workFlow.Document.DocumentNo))
                    editableFieldEnum.Add(RemittanceFieldGroup.Company);
            }

            return editableFieldEnum;
        }

        public IList<object> GetEditableFieldsForWaitRemittance(long workFlowID)
        {
            IList<object> editableFieldEnum = new List<object>();

            IList<SuRole> permissionRoles = GetAllowRoleVerifyWaitRemittance(workFlowID);
            if (MatchCurrentUserRole(permissionRoles))
            {
                editableFieldEnum.Add(RemittanceFieldGroup.FullClearing);
                editableFieldEnum.Add(RemittanceFieldGroup.VerifyDetail);
            }

            return editableFieldEnum;
        }

        #endregion

        #region GetVisibleFieldsFor{StateName}

        public IList<object> GetVisibleFieldsForDraft(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(RemittanceFieldGroup.All);
            visibleFieldEnum.Add(RemittanceFieldGroup.Company);
            visibleFieldEnum.Add(RemittanceFieldGroup.FullClearing);

            return visibleFieldEnum;
        }

        public IList<object> GetVisibleFieldsForWaitRemittance(long workFlowID)
        {
            List<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(RemittanceFieldGroup.All);
            visibleFieldEnum.Add(RemittanceFieldGroup.Company);
            visibleFieldEnum.Add(RemittanceFieldGroup.FullClearing);

            IList<SuRole> permissionRoles = GetPermitRoleVerifyPayment(workFlowID);
            if (MatchCurrentUserRole(permissionRoles))
            {
                visibleFieldEnum.Add(RemittanceFieldGroup.VerifyDetail);
            }

            return visibleFieldEnum;
        }

        public IList<object> GetVisibleFieldsForWaitApproveRemittance(long workFlowID)
        {
            List<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(RemittanceFieldGroup.All);
            visibleFieldEnum.Add(RemittanceFieldGroup.Company);
            visibleFieldEnum.Add(RemittanceFieldGroup.FullClearing);

            IList<SuRole> permissionRoles = GetPermitRoleVerifyPayment(workFlowID);
            if (MatchCurrentUserRole(permissionRoles))
            {
                visibleFieldEnum.Add(RemittanceFieldGroup.VerifyDetail);
            }

            permissionRoles = GetPermitRoleApproveVerifyPayment(workFlowID);
            if (MatchCurrentUserRole(permissionRoles))
            {
                visibleFieldEnum.Add(RemittanceFieldGroup.VerifyDetail);
            }

            return visibleFieldEnum;
        }

        public IList<object> GetVisibleFieldsForCancel(long workFlowID)
        {
            IList<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(RemittanceFieldGroup.All);
            visibleFieldEnum.Add(RemittanceFieldGroup.Company);
            visibleFieldEnum.Add(RemittanceFieldGroup.FullClearing);

            return visibleFieldEnum;
        }

        public IList<object> GetVisibleFieldsForComplete(long workFlowID)
        {
            List<object> visibleFieldEnum = new List<object>();
            visibleFieldEnum.Add(RemittanceFieldGroup.All);
            visibleFieldEnum.Add(RemittanceFieldGroup.Company);
            visibleFieldEnum.Add(RemittanceFieldGroup.FullClearing);

            IList<SuRole> permissionRoles = GetPermitRoleVerifyPayment(workFlowID);
            if (MatchCurrentUserRole(permissionRoles))
            {
                visibleFieldEnum.Add(RemittanceFieldGroup.VerifyDetail);
            }

            permissionRoles = GetPermitRoleApproveVerifyPayment(workFlowID);
            if (MatchCurrentUserRole(permissionRoles))
            {
                visibleFieldEnum.Add(RemittanceFieldGroup.VerifyDetail);
            }

            return visibleFieldEnum;
        }

        #endregion

        #region Private Methods

        protected void ValidateRejectResponse(RejectDetailResponse rejectDetailResponse)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (rejectDetailResponse.ReasonID == null || rejectDetailResponse.ReasonID == 0)
            {
                errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("WorkFlow_ValidateRejectResponse_ReasonRequired"));
            }

            if (!errors.IsEmpty) throw new SS.Standard.Utilities.ServiceValidationException(errors);
        }

        private void ClearingAdvance(long documentID)
        {
            FnRemittance remittanceDocument = ScgeAccountingQueryProvider.FnRemittanceQuery.GetFnRemittanceByDocumentID(documentID);
            if (remittanceDocument.IsFullClearing)
            {
                IList<FnRemittanceAdvance> remittanceAdvances = ScgeAccountingQueryProvider.FnRemittanceAdvanceQuery.FindRemittanceAdvanceByRemittanceID(remittanceDocument.RemittanceID);
                foreach (FnRemittanceAdvance remittanceAdvance in remittanceAdvances)
                {
                    WorkFlow advanceWorkFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(remittanceAdvance.Advance.DocumentID.DocumentID);

                    // Notify Clearing all advance
                    string clearingEventName = "Clearing";
                    WorkFlowStateEvent stateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetByWorkFlowStateID_EventName(advanceWorkFlow.CurrentState.WorkFlowStateID, clearingEventName);
                    WorkFlowService.NotifyEventFromInternal(advanceWorkFlow.WorkFlowID, clearingEventName, new SubmitResponse(stateEvent.WorkFlowStateEventID));
                }
            }
        }
        #endregion

        #region ReCalculatePermissionFor{EventName}{StateName}


        public void ReCalculatePermissionForSendDraft(long workFlowID, int workFlowStateEventID)
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

        public void ReCalculatePermissionForRejectWaitRemittance(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleRejectWaitRemittance(workFlowID);
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
            long permitUserID = GetAllowUserWithdrawWaitRemittance(workFlowID);

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

        public void ReCalculatePermissionForVerifyWaitRemittance(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleVerifyWaitRemittance(workFlowID);
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

        public void ReCalculatePermissionForVerifyAndApproveVerifyWaitRemittance(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleVerifyAndApproveVerifyWaitRemittance(workFlowID);
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

        public void ReCalculatePermissionForApproveWaitApproveRemittance(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleApproveWaitApproveRemittance(workFlowID);
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

        public void ReCalculatePermissionForRejectWaitApproveRemittance(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleRejectWaitApproveRemittance(workFlowID);
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

    }
}

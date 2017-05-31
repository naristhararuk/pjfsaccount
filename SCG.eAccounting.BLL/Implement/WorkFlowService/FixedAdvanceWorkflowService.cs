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
using SS.SU.Query;
using SS.SU.DTO;
using SS.Standard.Utilities;

namespace SCG.eAccounting.BLL.Implement.WorkFlowService
{
    public class FixedAdvanceWorkflowService : ExpenseWorkFlowService, IFixedAdvanceWorkflowService
    {
        #region CanWithdrawWaitPaymentFromSAP
        public override bool CanWithdrawWaitPaymentFromSAP(long workFlowID)
        {  
            IList<SuRole> permissionRoles = GetPermitRoleApproveVerify(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion CanWithdrawWaitPaymentFromSAP

        #region GetEditableFieldsFor{StateName}
        public override bool CanEditDraft(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);

            return UserAccount.UserID == scgDocument.CreatorID.Userid || UserAccount.UserID == scgDocument.RequesterID.Userid;
        }
        public override IList<object> GetEditableFieldsForDraft(long workFlowID)
        {
            IList<object> editableFields = new List<object>();

            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);

            //long permitUserID = GetPermitUserDraft(workFlowID);
            //if (UserAccount.UserID == permitUserID)
            /*creater or requester canedit*/
            if(UserAccount.UserID == scgDocument.CreatorID.Userid || UserAccount.UserID == scgDocument.RequesterID.Userid)
            {
                editableFields.Add(FixedAdvanceFieldGroup.PaymentType);
                editableFields.Add(FixedAdvanceFieldGroup.Initiator);
                editableFields.Add(FixedAdvanceFieldGroup.Attachment);
                editableFields.Add(FixedAdvanceFieldGroup.Memo);
                editableFields.Add(FixedAdvanceFieldGroup.Other);
                editableFields.Add(FixedAdvanceFieldGroup.RequestDate);
                editableFields.Add(FixedAdvanceFieldGroup.ServiceTeam);
                editableFields.Add(FixedAdvanceFieldGroup.BuActor);
                editableFields.Add(FixedAdvanceFieldGroup.CounterCashier);
                editableFields.Add(FixedAdvanceFieldGroup.Subject);

                if (string.IsNullOrEmpty(workFlow.Document.DocumentNo))
                    editableFields.Add(FixedAdvanceFieldGroup.Company);
            }

            return editableFields;
        }

        public override IList<object> GetVisibleFieldsForDraft(long workFlowID)
        {
            IList<object> visibleFields = new List<object>();
            visibleFields.Add(FixedAdvanceFieldGroup.PaymentType);
            visibleFields.Add(FixedAdvanceFieldGroup.Initiator);
            visibleFields.Add(FixedAdvanceFieldGroup.Attachment);
            visibleFields.Add(FixedAdvanceFieldGroup.Memo);
            visibleFields.Add(FixedAdvanceFieldGroup.Other);
            visibleFields.Add(FixedAdvanceFieldGroup.RequestDate);
            visibleFields.Add(FixedAdvanceFieldGroup.Company);
            visibleFields.Add(FixedAdvanceFieldGroup.ServiceTeam);
            visibleFields.Add(FixedAdvanceFieldGroup.BuActor);
            visibleFields.Add(FixedAdvanceFieldGroup.CounterCashier);
            visibleFields.Add(FixedAdvanceFieldGroup.Subject);
            return visibleFields;
        }

        public override IList<object> GetVisibleFieldsForWaitApprove(long workFlowID)
        {
            IList<object> visibleFields = new List<object>();
            visibleFields.Add(FixedAdvanceFieldGroup.PaymentType);
            visibleFields.Add(FixedAdvanceFieldGroup.Initiator);
            visibleFields.Add(FixedAdvanceFieldGroup.Attachment);
            visibleFields.Add(FixedAdvanceFieldGroup.Memo);
            visibleFields.Add(FixedAdvanceFieldGroup.Other);
            visibleFields.Add(FixedAdvanceFieldGroup.RequestDate);
            visibleFields.Add(FixedAdvanceFieldGroup.Company);
            visibleFields.Add(FixedAdvanceFieldGroup.ServiceTeam);
            visibleFields.Add(FixedAdvanceFieldGroup.BuActor);
            visibleFields.Add(FixedAdvanceFieldGroup.CounterCashier);
            visibleFields.Add(FixedAdvanceFieldGroup.Subject);
            return visibleFields;
        }

        public override IList<object> GetVisibleFieldsForWaitInitial(long workFlowID)
        {
            IList<object> visibleFields = new List<object>();
            visibleFields.Add(FixedAdvanceFieldGroup.PaymentType);
            visibleFields.Add(FixedAdvanceFieldGroup.Initiator);
            visibleFields.Add(FixedAdvanceFieldGroup.Attachment);
            visibleFields.Add(FixedAdvanceFieldGroup.Memo);
            visibleFields.Add(FixedAdvanceFieldGroup.Other);
            visibleFields.Add(FixedAdvanceFieldGroup.RequestDate);
            visibleFields.Add(FixedAdvanceFieldGroup.Company);
            visibleFields.Add(FixedAdvanceFieldGroup.ServiceTeam);
            visibleFields.Add(FixedAdvanceFieldGroup.BuActor);
            visibleFields.Add(FixedAdvanceFieldGroup.CounterCashier);
            visibleFields.Add(FixedAdvanceFieldGroup.Subject);
            return visibleFields;
        }

        public override IList<object> GetVisibleFieldsForWaitVerify(long workFlowID)
        {
            IList<object> visibleFields = new List<object>();
            visibleFields.Add(FixedAdvanceFieldGroup.PaymentType);
            visibleFields.Add(FixedAdvanceFieldGroup.Initiator);
            visibleFields.Add(FixedAdvanceFieldGroup.Attachment);
            visibleFields.Add(FixedAdvanceFieldGroup.Memo);
            visibleFields.Add(FixedAdvanceFieldGroup.Other);
            visibleFields.Add(FixedAdvanceFieldGroup.RequestDate);
            visibleFields.Add(FixedAdvanceFieldGroup.Company);
            visibleFields.Add(FixedAdvanceFieldGroup.ServiceTeam);
            visibleFields.Add(FixedAdvanceFieldGroup.BuActor);
            visibleFields.Add(FixedAdvanceFieldGroup.Subject);
            visibleFields.Add(FixedAdvanceFieldGroup.CounterCashier);
            /*N-Edited*/
            IList<SuRole> permissionRoles = GetPermitRoleVerify(workFlowID);
            IList<SuRole> permissionRoles2 = GetPermitRoleCounterCashier(workFlowID);
            if (MatchCurrentUserRole(permissionRoles) || MatchCurrentUserRole(permissionRoles2))
            {
                visibleFields.Add(FixedAdvanceFieldGroup.VerifyDetail);
            }
            return visibleFields;
        }

        public override IList<object> GetEditableFieldsForWaitVerify(long workFlowID)
        {
            IList<object> editableFields = new List<object>();
            editableFields.Add(FixedAdvanceFieldGroup.VerifyDetail);
            /*เพิ่มแก้ไขตอนVerify*/
            editableFields.Add(FixedAdvanceFieldGroup.PaymentType);
            editableFields.Add(FixedAdvanceFieldGroup.CounterCashier);
            editableFields.Add(FixedAdvanceFieldGroup.ServiceTeam);
            editableFields.Add(FixedAdvanceFieldGroup.Attachment);
            editableFields.Add(FixedAdvanceFieldGroup.Memo);
            editableFields.Add(FixedAdvanceFieldGroup.RequestDate);

            //editableFields.Add(FixedAdvanceFieldGroup.Subject);
            //editableFields.Add(FixedAdvanceFieldGroup.Initiator);
            //editableFields.Add(FixedAdvanceFieldGroup.BuActor);

            //editableFields.Add(FixedAdvanceFieldGroup.Other);



            //editableFields.Add(FixedAdvanceFieldGroup.Return);
            //editableFields.Add(FixedAdvanceFieldGroup.ClearingReturn);


            //WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            //if (string.IsNullOrEmpty(workFlow.Document.DocumentNo))
            //    editableFields.Add(FixedAdvanceFieldGroup.Company);


            return editableFields;
        }

        public override IList<object> GetVisibleFieldsForWaitApproveVerify(long workFlowID)
        {
            IList<object> visibleFields = new List<object>();
            visibleFields.Add(FixedAdvanceFieldGroup.PaymentType);
            visibleFields.Add(FixedAdvanceFieldGroup.Initiator);
            visibleFields.Add(FixedAdvanceFieldGroup.Attachment);
            visibleFields.Add(FixedAdvanceFieldGroup.Memo);
            visibleFields.Add(FixedAdvanceFieldGroup.Other);
            visibleFields.Add(FixedAdvanceFieldGroup.RequestDate);
            visibleFields.Add(FixedAdvanceFieldGroup.Company);
            visibleFields.Add(FixedAdvanceFieldGroup.ServiceTeam);
            visibleFields.Add(FixedAdvanceFieldGroup.BuActor);
            visibleFields.Add(FixedAdvanceFieldGroup.Subject);
            visibleFields.Add(FixedAdvanceFieldGroup.CounterCashier);
            /*N-Edited*/
            IList<SuRole> permissionRoles = GetPermitRoleVerify(workFlowID);
            IList<SuRole> permissionRoles2 = GetPermitRoleCounterCashier(workFlowID);
            if (MatchCurrentUserRole(permissionRoles) || MatchCurrentUserRole(permissionRoles2))
            {
                visibleFields.Add(FixedAdvanceFieldGroup.VerifyDetail);
            }
            return visibleFields;
        }

        public IList<object> GetVisibleFieldsForWaitApproveVerifyReturnOutstanding(long workFlowID)
        {
            IList<object> visibleFields = new List<object>();
            visibleFields.Add(FixedAdvanceFieldGroup.PaymentType);
            visibleFields.Add(FixedAdvanceFieldGroup.Initiator);
            visibleFields.Add(FixedAdvanceFieldGroup.Attachment);
            visibleFields.Add(FixedAdvanceFieldGroup.Memo);
            visibleFields.Add(FixedAdvanceFieldGroup.Other);
            visibleFields.Add(FixedAdvanceFieldGroup.RequestDate);
            visibleFields.Add(FixedAdvanceFieldGroup.Company);
            visibleFields.Add(FixedAdvanceFieldGroup.ServiceTeam);
            visibleFields.Add(FixedAdvanceFieldGroup.BuActor);
            visibleFields.Add(FixedAdvanceFieldGroup.Subject);
            visibleFields.Add(FixedAdvanceFieldGroup.CounterCashier);
            /*N-Edited*/
            IList<SuRole> permissionRoles = GetPermitRoleVerify(workFlowID);
            IList<SuRole> permissionRoles2 = GetPermitRoleCounterCashier(workFlowID);
            if (MatchCurrentUserRole(permissionRoles) || MatchCurrentUserRole(permissionRoles2))
            {
                visibleFields.Add(FixedAdvanceFieldGroup.VerifyDetail);
            }
            return visibleFields;
        }

        public IList<object> GetVisibleFieldsForWaitApproveVerifyReturnComplete(long workFlowID)
        {
            IList<object> visibleFields = new List<object>();
            visibleFields.Add(FixedAdvanceFieldGroup.PaymentType);
            visibleFields.Add(FixedAdvanceFieldGroup.Initiator);
            visibleFields.Add(FixedAdvanceFieldGroup.Attachment);
            visibleFields.Add(FixedAdvanceFieldGroup.Memo);
            visibleFields.Add(FixedAdvanceFieldGroup.Other);
            visibleFields.Add(FixedAdvanceFieldGroup.RequestDate);
            visibleFields.Add(FixedAdvanceFieldGroup.Company);
            visibleFields.Add(FixedAdvanceFieldGroup.ServiceTeam);
            visibleFields.Add(FixedAdvanceFieldGroup.BuActor);
            visibleFields.Add(FixedAdvanceFieldGroup.Subject);
            visibleFields.Add(FixedAdvanceFieldGroup.Return);
            visibleFields.Add(FixedAdvanceFieldGroup.CounterCashier);
            /*N-Edited*/
            IList<SuRole> permissionRoles = GetPermitRoleVerify(workFlowID);
            IList<SuRole> permissionRoles2 = GetPermitRoleCounterCashier(workFlowID);
            if (MatchCurrentUserRole(permissionRoles) || MatchCurrentUserRole(permissionRoles2))
            {
                visibleFields.Add(FixedAdvanceFieldGroup.VerifyDetail);
                visibleFields.Add(FixedAdvanceFieldGroup.ClearingReturn);
            }
            return visibleFields;
        }

        public override IList<object> GetVisibleFieldsForWaitPaymentFromSAP(long workFlowID)
        {
            IList<object> visibleFields = new List<object>();
            visibleFields.Add(FixedAdvanceFieldGroup.PaymentType);
            visibleFields.Add(FixedAdvanceFieldGroup.Initiator);
            visibleFields.Add(FixedAdvanceFieldGroup.Attachment);
            visibleFields.Add(FixedAdvanceFieldGroup.Memo);
            visibleFields.Add(FixedAdvanceFieldGroup.Other);
            visibleFields.Add(FixedAdvanceFieldGroup.RequestDate);
            visibleFields.Add(FixedAdvanceFieldGroup.Company);
            visibleFields.Add(FixedAdvanceFieldGroup.ServiceTeam);
            visibleFields.Add(FixedAdvanceFieldGroup.BuActor);
            visibleFields.Add(FixedAdvanceFieldGroup.Subject);
            visibleFields.Add(FixedAdvanceFieldGroup.CounterCashier);
            /*N-Edited*/
            IList<SuRole> permissionRoles = GetPermitRoleVerify(workFlowID);
            IList<SuRole> permissionRoles2 = GetPermitRoleCounterCashier(workFlowID);
            if (MatchCurrentUserRole(permissionRoles) || MatchCurrentUserRole(permissionRoles2))
            {
                visibleFields.Add(FixedAdvanceFieldGroup.VerifyDetail);
            }
            return visibleFields;
        }

        public IList<object> GetVisibleFieldsForDeprecated(long workFlowID)
        {
            IList<object> visibleFields = new List<object>();
            visibleFields.Add(FixedAdvanceFieldGroup.PaymentType);
            visibleFields.Add(FixedAdvanceFieldGroup.Initiator);
            visibleFields.Add(FixedAdvanceFieldGroup.Attachment);
            visibleFields.Add(FixedAdvanceFieldGroup.Memo);
            visibleFields.Add(FixedAdvanceFieldGroup.Other);
            visibleFields.Add(FixedAdvanceFieldGroup.RequestDate);
            visibleFields.Add(FixedAdvanceFieldGroup.Company);
            visibleFields.Add(FixedAdvanceFieldGroup.ServiceTeam);
            visibleFields.Add(FixedAdvanceFieldGroup.BuActor);
            visibleFields.Add(FixedAdvanceFieldGroup.Subject);
            visibleFields.Add(FixedAdvanceFieldGroup.CounterCashier);
            /*N-Edited*/
            IList<SuRole> permissionRoles = GetPermitRoleVerify(workFlowID);
            IList<SuRole> permissionRoles2 = GetPermitRoleCounterCashier(workFlowID);
            if (MatchCurrentUserRole(permissionRoles) || MatchCurrentUserRole(permissionRoles2))
            {
                visibleFields.Add(FixedAdvanceFieldGroup.VerifyDetail);
            }
            return visibleFields;
        }

        public IList<object> GetVisibleFieldsForOutstanding(long workFlowID)
        {
            IList<object> visibleFields = new List<object>();
            visibleFields.Add(FixedAdvanceFieldGroup.PaymentType);
            visibleFields.Add(FixedAdvanceFieldGroup.Initiator);
            visibleFields.Add(FixedAdvanceFieldGroup.Attachment);
            visibleFields.Add(FixedAdvanceFieldGroup.Memo);
            visibleFields.Add(FixedAdvanceFieldGroup.Other);
            visibleFields.Add(FixedAdvanceFieldGroup.RequestDate);
            visibleFields.Add(FixedAdvanceFieldGroup.Company);
            visibleFields.Add(FixedAdvanceFieldGroup.ServiceTeam);
            visibleFields.Add(FixedAdvanceFieldGroup.BuActor);
            visibleFields.Add(FixedAdvanceFieldGroup.Subject);
            visibleFields.Add(FixedAdvanceFieldGroup.Return);
            visibleFields.Add(FixedAdvanceFieldGroup.CounterCashier);
            /*N-Edited*/
            IList<SuRole> permissionRoles = GetPermitRoleVerify(workFlowID);
            IList<SuRole> permissionRoles2 = GetPermitRoleCounterCashier(workFlowID);
            if (MatchCurrentUserRole(permissionRoles) || MatchCurrentUserRole(permissionRoles2))
            {
                visibleFields.Add(FixedAdvanceFieldGroup.VerifyDetail);
            }
            return visibleFields;
        }

        public bool CanEditOutstanding(long workFlowID)
        {
            //long userID = GetPermitUserDraft(workFlowID); 
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            IList<SuRole> permissionRoles = GetPermitRoleCounterCashier(workFlowID);
            return scgDocument.CreatorID.Userid == UserAccount.UserID || scgDocument.RequesterID.Userid == UserAccount.UserID || MatchCurrentUserRole(permissionRoles);
        }

        public bool CanEditWaitReturnComplete(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetPermitRoleCounterCashier(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }

        public bool CanEditWaitReturn(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetPermitRoleCounterCashier(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }

        public IList<object> GetEditableFieldsForOutstanding(long workFlowID)
        {
            IList<object> editableFields = new List<object>();

            /*เพิ่มสิทธิ์ แก้ไขให้ CounterCashier*/
            IList<SuRole> permissionRoles = GetPermitRoleCounterCashier(workFlowID);
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
                //long permitUserID = GetPermitUserDraft(workFlowID);
                if (scgDocument.CreatorID.Userid == UserAccount.UserID || scgDocument.RequesterID.Userid == UserAccount.UserID || MatchCurrentUserRole(permissionRoles))
                {
                    editableFields.Add(FixedAdvanceFieldGroup.Return);
                    if (string.IsNullOrEmpty(workFlow.Document.DocumentNo))
                        editableFields.Add(FixedAdvanceFieldGroup.Company);
                }

            return editableFields;
        }

        public IList<object> GetEditableFieldsForWaitReturn(long workFlowID)
        {
            IList<object> editableFields = new List<object>();
            editableFields.Add(FixedAdvanceFieldGroup.VerifyDetail);
            /*เพิ่มแก้ไขตอนลดยอด*/
            editableFields.Add(FixedAdvanceFieldGroup.PaymentType);
            editableFields.Add(FixedAdvanceFieldGroup.CounterCashier);
            editableFields.Add(FixedAdvanceFieldGroup.ServiceTeam);
            editableFields.Add(FixedAdvanceFieldGroup.Attachment);
            editableFields.Add(FixedAdvanceFieldGroup.Memo);
            editableFields.Add(FixedAdvanceFieldGroup.RequestDate);
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            if (string.IsNullOrEmpty(workFlow.Document.DocumentNo))
            {
                editableFields.Add(FixedAdvanceFieldGroup.Company);
            }
            return editableFields;
        }

        public IList<object> GetVisibleFieldsForWaitReturn(long workFlowID)
        {
            IList<object> visibleFields = new List<object>();
            visibleFields.Add(FixedAdvanceFieldGroup.PaymentType);
            visibleFields.Add(FixedAdvanceFieldGroup.Initiator);
            visibleFields.Add(FixedAdvanceFieldGroup.Attachment);
            visibleFields.Add(FixedAdvanceFieldGroup.Memo);
            visibleFields.Add(FixedAdvanceFieldGroup.Other);
            visibleFields.Add(FixedAdvanceFieldGroup.RequestDate);
            visibleFields.Add(FixedAdvanceFieldGroup.Company);
            visibleFields.Add(FixedAdvanceFieldGroup.ServiceTeam);
            visibleFields.Add(FixedAdvanceFieldGroup.BuActor);
            visibleFields.Add(FixedAdvanceFieldGroup.Subject);
            visibleFields.Add(FixedAdvanceFieldGroup.CounterCashier);
            /*N-Edited*/
            //IList<SuRole> permissionRoles = GetPermitRoleVerify(workFlowID);
            IList<SuRole> permissionRoles2 = GetPermitRoleCounterCashier(workFlowID);
            if (MatchCurrentUserRole(permissionRoles2))
            {
                visibleFields.Add(FixedAdvanceFieldGroup.VerifyDetail);
            }
            return visibleFields;
        }

        public IList<object> GetEditableFieldsForWaitVerifyReturnComplete(long workFlowID)
        {
            IList<object> editableFields = new List<object>();

            //long permitUserID = GetPermitUserDraft(workFlowID);
            //if (UserAccount.UserID == permitUserID)
            //{
                editableFields.Add(FixedAdvanceFieldGroup.ClearingReturn);
                WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
                if (string.IsNullOrEmpty(workFlow.Document.DocumentNo))
                    editableFields.Add(FixedAdvanceFieldGroup.Company);
            //}
            return editableFields;
        }

        public IList<object> GetEditableFieldsForWaitReturnComplete(long workFlowID)
        {
            IList<object> editableFields = new List<object>();

            editableFields.Add(FixedAdvanceFieldGroup.ClearingReturn);
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            if (string.IsNullOrEmpty(workFlow.Document.DocumentNo))
                editableFields.Add(FixedAdvanceFieldGroup.Company);

            return editableFields;
        }

        public IList<object> GetVisibleFieldsForWaitReturnComplete(long workFlowID)
        {
            IList<object> visibleFields = new List<object>();
            visibleFields.Add(FixedAdvanceFieldGroup.PaymentType);
            visibleFields.Add(FixedAdvanceFieldGroup.Initiator);
            visibleFields.Add(FixedAdvanceFieldGroup.Attachment);
            visibleFields.Add(FixedAdvanceFieldGroup.Memo);
            visibleFields.Add(FixedAdvanceFieldGroup.Other);
            visibleFields.Add(FixedAdvanceFieldGroup.RequestDate);
            visibleFields.Add(FixedAdvanceFieldGroup.Company);
            visibleFields.Add(FixedAdvanceFieldGroup.ServiceTeam);
            visibleFields.Add(FixedAdvanceFieldGroup.BuActor);
            visibleFields.Add(FixedAdvanceFieldGroup.Subject);
            visibleFields.Add(FixedAdvanceFieldGroup.Return);
            visibleFields.Add(FixedAdvanceFieldGroup.CounterCashier);
            /*N-Edited*/
            IList<SuRole> permissionRoles = GetPermitRoleVerify(workFlowID);
            IList<SuRole> permissionRoles2 = GetPermitRoleCounterCashier(workFlowID);
            if (MatchCurrentUserRole(permissionRoles) || MatchCurrentUserRole(permissionRoles2))
            {
                visibleFields.Add(FixedAdvanceFieldGroup.VerifyDetail);
                visibleFields.Add(FixedAdvanceFieldGroup.ClearingReturn);
            }
            return visibleFields;
        }

        public IList<object> GetVisibleFieldsForWaitVerifyReturnComplete(long workFlowID)
        {
            IList<object> visibleFields = new List<object>();
            visibleFields.Add(FixedAdvanceFieldGroup.PaymentType);
            visibleFields.Add(FixedAdvanceFieldGroup.Initiator);
            visibleFields.Add(FixedAdvanceFieldGroup.Attachment);
            visibleFields.Add(FixedAdvanceFieldGroup.Memo);
            visibleFields.Add(FixedAdvanceFieldGroup.Other);
            visibleFields.Add(FixedAdvanceFieldGroup.RequestDate);
            visibleFields.Add(FixedAdvanceFieldGroup.Company);
            visibleFields.Add(FixedAdvanceFieldGroup.ServiceTeam);
            visibleFields.Add(FixedAdvanceFieldGroup.BuActor);
            visibleFields.Add(FixedAdvanceFieldGroup.Subject);
            visibleFields.Add(FixedAdvanceFieldGroup.Return);
            visibleFields.Add(FixedAdvanceFieldGroup.CounterCashier);
            /*N-Edited*/
            IList<SuRole> permissionRoles = GetPermitRoleVerify(workFlowID);
            IList<SuRole> permissionRoles2 = GetPermitRoleCounterCashier(workFlowID);
            if (MatchCurrentUserRole(permissionRoles) || MatchCurrentUserRole(permissionRoles2))
            {
                visibleFields.Add(FixedAdvanceFieldGroup.VerifyDetail);
                visibleFields.Add(FixedAdvanceFieldGroup.ClearingReturn);
            }

            return visibleFields;
        }

        public override IList<object> GetVisibleFieldsForComplete(long workFlowID)
        {
            IList<object> visibleFields = new List<object>();
            visibleFields.Add(FixedAdvanceFieldGroup.PaymentType);
            visibleFields.Add(FixedAdvanceFieldGroup.Initiator);
            visibleFields.Add(FixedAdvanceFieldGroup.Attachment);
            visibleFields.Add(FixedAdvanceFieldGroup.Memo);
            visibleFields.Add(FixedAdvanceFieldGroup.Other);
            visibleFields.Add(FixedAdvanceFieldGroup.RequestDate);
            visibleFields.Add(FixedAdvanceFieldGroup.Company);
            visibleFields.Add(FixedAdvanceFieldGroup.ServiceTeam);
            visibleFields.Add(FixedAdvanceFieldGroup.BuActor);
            visibleFields.Add(FixedAdvanceFieldGroup.CounterCashier);
            visibleFields.Add(FixedAdvanceFieldGroup.Subject);
            visibleFields.Add(FixedAdvanceFieldGroup.Return);

            IList<SuRole> permissionRoles = GetPermitRoleVerify(workFlowID);
            IList<SuRole> permissionRoles2 = GetPermitRoleCounterCashier(workFlowID);
            if (MatchCurrentUserRole(permissionRoles) || MatchCurrentUserRole(permissionRoles2))
            {
                visibleFields.Add(FixedAdvanceFieldGroup.VerifyDetail);
                visibleFields.Add(FixedAdvanceFieldGroup.ClearingReturn);
            }

            return visibleFields;
        }

        public override IList<object> GetVisibleFieldsForWaitApproveRejection(long workFlowID)
        {
            IList<object> visibleFields = new List<object>();
            visibleFields.Add(FixedAdvanceFieldGroup.PaymentType);
            visibleFields.Add(FixedAdvanceFieldGroup.Initiator);
            visibleFields.Add(FixedAdvanceFieldGroup.Attachment);
            visibleFields.Add(FixedAdvanceFieldGroup.Memo);
            visibleFields.Add(FixedAdvanceFieldGroup.Other);
            visibleFields.Add(FixedAdvanceFieldGroup.RequestDate);
            visibleFields.Add(FixedAdvanceFieldGroup.Company);
            visibleFields.Add(FixedAdvanceFieldGroup.ServiceTeam);
            visibleFields.Add(FixedAdvanceFieldGroup.BuActor);
            visibleFields.Add(FixedAdvanceFieldGroup.CounterCashier);
            visibleFields.Add(FixedAdvanceFieldGroup.Subject);
            return visibleFields;
        }

        #endregion
        public override bool CanHoldWaitVerify(long workFlowID)
        {
            return false;
        }

        #region CanPayWaitPaymentFromSAP
        public override bool CanPayWaitPaymentFromSAP(long workFlowID)
        {
            /*hide pay radio*/
            return false;
        }
        #endregion
        #region CanRejectWaitReturnToDraft And CanRejectWaitReturnCompleteToOutstanding
        public bool CanRejectWaitReturn(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetPermitRoleCounterCashier(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        public bool CanRejectWaitReturnComplete(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetPermitRoleCounterCashier(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #region CanReturnOutstanding
        public bool CanReturnOutstanding(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            /*requestor and creator can return*/
            return scgDocument.CreatorID.Userid == UserAccount.UserID || scgDocument.RequesterID.Userid == UserAccount.UserID;
        }
        #endregion

        #region CanCancelOutstanding
        public bool CanCancelOutstanding(long workFlowID)
        {
            return false;
        }
        #endregion

        /*checkเพิ่ม ถ้าเป็น state waitreturn*/
        #region CanVerifyWaitVerify
        public override bool CanVerifyWaitVerify(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            if (workFlow.CurrentState.Name == "WaitReturn")
            {
                IList<SuRole> permissionRoles = GetPermitRoleCounterCashier(workFlowID);
                return MatchCurrentUserRole(permissionRoles);
            }
            else
            {
                IList<SuRole> permissionRoles = GetAllowRoleVerifyWaitVerify(workFlowID);
                return MatchCurrentUserRole(permissionRoles);
            }
        }
        public override IList<SuRole> GetAllowRoleApproveWaitApproveVerify(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            if (workFlow.CurrentState.Name == "WaitReturn" || workFlow.CurrentState.Name == "WaitReturnComplete")
            {
                IList<SuRole> permissionRoles = GetPermitRoleCounterCashier(workFlowID);
                return permissionRoles;
            }
            else
            {
                return GetPermitRoleApproveVerify(workFlowID);
            }
        }
        //public override bool CanVerifyAndApproveVerifyWaitVerify(long workFlowID)
        //{
        //    WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
        //    if (workFlow.CurrentState.Name == "WaitReturn")
        //    {
        //        IList<SuRole> permissionRoles = GetPermitRoleCounterCashier(workFlowID);
        //        bool canVerifyAndApprove = MatchCurrentUserRole(permissionRoles);
        //        return canVerifyAndApprove;
        //    }
        //    else
        //    {
        //        IList<SuRole> permissionRoles = GetAllowRoleVerifyWaitVerify(workFlowID);
        //        bool canVerify = MatchCurrentUserRole(permissionRoles);
        //        if (!canVerify) return false;

        //        permissionRoles = GetAllowRoleApproveWaitApproveVerify(workFlowID);
        //        bool canApproveVerify = MatchCurrentUserRole(permissionRoles);
        //        if (!canApproveVerify) return false;
        //        return canVerify && canApproveVerify;
        //    }

        //}
        #endregion

        #region CanVerifyWaitVerifyReturnComplete
        public bool CanVerifyWaitVerifyReturnComplete(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleVerifyWaitVerify(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #region CanRejectWaitVerifyReturnComplete
        public bool CanRejectWaitVerifyReturnComplete(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleRejectWaitVerify(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        public bool CanEditWaitVerifyReturnComplete(long workFlowID)
        {
            long userID = GetPermitUserDraft(workFlowID);
            return userID == UserAccount.UserID;
        }

        public virtual long GetAllowUserReturnWaitReturn(long workFlowID)
        {
            return GetPermitUserReturn(workFlowID);
        }

        public virtual long GetPermitUserReturn(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            return scgDocument.CreatorID.Userid;
        }

        #region CanClearingWaitReturn
        public bool CanClearingWaitReturn(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetPermitRoleCounterCashier(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #region CanClearingWaitReturnCompelete
        public bool CanClearingWaitReturnComplete(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetPermitRoleCounterCashier(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #region override OnSendDraft
        public override string OnSendDraft(long workFlowID, object eventData)
        {
            string signal = base.OnSendDraft(workFlowID, eventData);
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            FixedAdvanceDocument fixadvDoc = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.GetFixedAdvanceByDocumentID(workFlow.Document.DocumentID);
            if (fixadvDoc.FixedAdvanceType == 2 && fixadvDoc.RefFixedAdvanceID == null)
            {
                if (fixadvDoc.RefFixedAdvanceID == null)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RefFixedAdvanceIsRequired"));
                }
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            IList<DocumentInitiator> initiators = ScgeAccountingQueryProvider.DocumentInitiatorQuery.GetNotResponseInitiatorByDocumentID(workFlow.Document.DocumentID);
            if (initiators.Count > 0)
                signal = "SendDraftToWaitInitator";
            else
                signal = "SendDraftToWaitApprove";
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
            FixedAdvanceDocument fixadvDoc = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.GetFixedAdvanceByDocumentID(scgDocument.DocumentID);

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
                //advDoc.RequestDateOfAdvanceApproved = advDoc.RequestDateOfAdvance;
                fixadvDoc.UpdBy = UserAccount.UserID;
                fixadvDoc.UpdDate = DateTime.Now;
                fixadvDoc.UpdPgm = UserAccount.CurrentProgramCode;
                FixedAdvanceDocumentService.SaveOrUpdate(fixadvDoc);


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
            string signal;
            if (fixadvDoc.FixedAdvanceType == 2 && fixadvDoc.NetAmount < 0)
                signal = "ApproveWaitApproveToWaitReturn";
            else
                signal = "ApproveWaitApproveToWaitVerify";

            return signal;
        }
        #endregion

        #region OnVerifyWaitVerify
        public override string OnVerifyWaitVerify(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

            //Validate Posting Status
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            FnExpenseDocument expDoc = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(workFlow.Document.DocumentID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            FixedAdvanceDocument fixadvDoc = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.GetFixedAdvanceByDocumentID(scgDocument.DocumentID);

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
            string signal;
            //if (fixadvDoc.FixedAdvanceType == 2 && fixadvDoc.NetAmount < 0)
            //    signal = "VerifyWaitVerifyToWaitApproveVerifyReturnOutstanding";
            //else
            signal = "VerifyWaitVerifyToWaitApproveVerify";

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

            FixedAdvanceDocument fixedAdvanceDocument = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.GetFixedAdvanceByDocumentID(workFlow.Document.DocumentID);

            if (scgDocument.PostingStatus != PostingStaus.Complete)
            {

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

            //if (expenseDocument.DifferenceAmount > 0)
            //{
            //Pay more in case expense that more than advance
            if (fixedAdvanceDocument.PaymentType == PaymentType.CA)
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
                // }
            }
            else if (fixedAdvanceDocument.PaymentType == PaymentType.CQ || fixedAdvanceDocument.PaymentType == PaymentType.TR)
            {
                signal = "ApproveWaitApproveVerifyToWaitPaymentFromSAP";
            }
            //}
            return signal;
        }
        #endregion

        #region OnApproveWaitApproveVerifyReturnOutstanding
        public string OnApproveWaitApproveVerifyReturnOutstanding(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

            //Validate Posting Status
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);

            FixedAdvanceDocument fixedAdvanceDocument = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.GetFixedAdvanceByDocumentID(workFlow.Document.DocumentID);

            if (scgDocument.PostingStatus != PostingStaus.Complete)
            {
                //if (expenseDocument == null || (expenseDocument != null && !expenseDocument.TotalExpense.Equals(0)))
                //{
                //    ExpensePostingService postingService = new ExpensePostingService();
                //    IList<BAPIApproveReturn> bapiReturn = postingService.BAPIApprove(scgDocument.DocumentID, DocumentKind.Expense.ToString(), UserAccount.UserID);

                //    if (postingService.GetDocumentStatus(scgDocument.DocumentID, DocumentKind.Remittance.ToString()) == "A")
                //    {
                //        scgDocument.PostingStatus = "C";
                //        SCGDocumentService.SaveOrUpdate(scgDocument);
                //    }
                //    else if (postingService.GetDocumentStatus(scgDocument.DocumentID, DocumentKind.Remittance.ToString()) == "PP")
                //    {
                //        scgDocument.PostingStatus = "PP";
                //        SCGDocumentService.SaveOrUpdate(scgDocument);
                //    }

                //    bool isSuccess = true;
                //    for (int i = 0; i < bapiReturn.Count; i++)
                //    {
                //        if (bapiReturn[i].ApproveStatus != "S")
                //        {
                //            isSuccess = false;
                //            break;
                //        }
                //    }

                //    if (!isSuccess)
                //    {
                //        for (int i = 0; i < bapiReturn.Count; i++)
                //        {
                //            if (bapiReturn[i].ApproveStatus != "S")
                //            {
                //                string approveStatus = "Error";
                //                if (bapiReturn[i].ApproveStatus.ToUpper() == "W")
                //                {
                //                    approveStatus = "Warning";
                //                }

                //                for (int j = 0; j < bapiReturn[i].ApproveReturn.Count; j++)
                //                {
                //                    errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage(approveStatus + ": [DocSeq=" + bapiReturn[i].DOCSEQ + "] -> " + bapiReturn[i].ApproveReturn[j].Message));
                //                }
                //            }
                //        }
                //    }
                //}
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
            signal = "ApproveWaitApproveVerifyReturnOutstandingToWaitReturn";
            return signal;
        }
        #endregion

        #region OnApproveWaitApproveVerifyReturnComplete
        public string OnApproveWaitApproveVerifyReturnComplete(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

            //Validate Posting Status
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            SCG.eAccounting.BLL.Implement.FixedAdvanceDocumentService fixedAdvanceService = new FixedAdvanceDocumentService();
            FixedAdvanceDocument fixedAdvanceDocument = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.GetFixedAdvanceByDocumentID(workFlow.Document.DocumentID);

            if (fixedAdvanceDocument.PostingStatusReturn != PostingStaus.Complete)
            {
                //if (expenseDocument == null || (expenseDocument != null && !expenseDocument.TotalExpense.Equals(0)))
                //{
                //    ExpensePostingService postingService = new ExpensePostingService();
                //    IList<BAPIApproveReturn> bapiReturn = postingService.BAPIApprove(scgDocument.DocumentID, DocumentKind.Expense.ToString(), UserAccount.UserID);

                //  if (postingService.GetDocumentStatus(scgDocument.DocumentID, DocumentKind.Remittance.ToString()) == "A")
                //    {
                fixedAdvanceService.UpdatePostingStatusFixedAdvanceDocument(fixedAdvanceDocument.DocumentID.DocumentID, "C");
                //    }
                //    else if (postingService.GetDocumentStatus(scgDocument.DocumentID, DocumentKind.Remittance.ToString()) == "PP")
                //    {
                //        scgDocument.PostingStatus = "PP";
                //        SCGDocumentService.SaveOrUpdate(scgDocument);
                //    }

                //    bool isSuccess = true;
                //    for (int i = 0; i < bapiReturn.Count; i++)
                //    {
                //        if (bapiReturn[i].ApproveStatus != "S")
                //        {
                //            isSuccess = false;
                //            break;
                //        }
                //    }

                //    if (!isSuccess)
                //    {
                //        for (int i = 0; i < bapiReturn.Count; i++)
                //        {
                //            if (bapiReturn[i].ApproveStatus != "S")
                //            {
                //                string approveStatus = "Error";
                //                if (bapiReturn[i].ApproveStatus.ToUpper() == "W")
                //                {
                //                    approveStatus = "Warning";
                //                }

                //                for (int j = 0; j < bapiReturn[i].ApproveReturn.Count; j++)
                //                {
                //                    errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage(approveStatus + ": [DocSeq=" + bapiReturn[i].DOCSEQ + "] -> " + bapiReturn[i].ApproveReturn[j].Message));
                //                }
                //            }
                //        }
                //    }
                //}
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
            signal = "ApproveWaitApproveVerifyReturnCompleteToWaitReturnComplete";
            return signal;
        }
        #endregion

        #region OnRejectWaitApproveVerifyReturnOutstanding
        public string OnRejectWaitApproveVerifyReturnOutstanding(long workFlowID, object eventData)
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
            string signal = "RejectWaitApproveVerifyReturnOutstandingToWaitVerify";

            return signal;
        }
        #endregion

        #region OnRejectWaitApproveVerifyReturnComplete
        public string OnRejectWaitApproveVerifyReturnComplete(long workFlowID, object eventData)
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
            string signal = "RejectWaitApproveVerifyReturnCompleteToWaitVerifyReturnComplete";

            return signal;
        }
        #endregion

        #region OnRejectWaitReturnToDraft And OnRejectWaitReturnCompleteToOutstanding
        public string OnRejectWaitReturn(long workFlowID, object eventData)
        {
            //*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

            //Validate Posting Status
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            if (document.PostingStatus == PostingStaus.Posted || document.PostingStatus == PostingStaus.Complete)
            {
                errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("Workflow_OnRejectWaitReturnToDraft_Mismatch_PostingStatus"));
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
            string signal = "RejectWaitReturnToDraft";

            return signal;
        }

        public string OnRejectWaitReturnComplete(long workFlowID, object eventData)
        {
            //*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

            SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            FixedAdvanceDocument fixadvDoc = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.GetFixedAdvanceByDocumentID(scgDocument.DocumentID);

            //Validate Posting Status
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (fixadvDoc.PostingStatusReturn == PostingStaus.Posted || fixadvDoc.PostingStatusReturn == PostingStaus.Complete)
            {
                errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("Workflow_OnRejectWaitReturnCompleteToOutstanding_Mismatch_PostingStatus"));
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

            string signal = "RejectWaitReturnCompleteToOutstanding";

            return signal;
        }
        #endregion

        #region OnReturnOutstanding
        public string OnReturnOutstanding(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

            //Validate Posting Status
            //Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            FixedAdvanceDocument fixedDoc = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.GetFixedAdvanceByDocumentID(workFlow.Document.DocumentID);
            ValidateReturnOutStanding(fixedDoc);
            //if (!errors.IsEmpty) throw new ServiceValidationException(errors);

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
            string signal = "ReturnOutstandingToWaitReturnComplete";

            return signal;
        }
        #endregion

        #region OnVerifyWaitVerifyReturnComplete
        public string OnVerifyWaitVerifyReturnComplete(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            FnExpenseDocument expDoc = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(workFlow.Document.DocumentID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            FixedAdvanceDocument fixadvDoc = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.GetFixedAdvanceByDocumentID(scgDocument.DocumentID);

            ValidateClearing(fixadvDoc);

            //Validate Posting Status
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

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
            string signal;

            signal = "VerifyWaitVerifyReturnCompleteToWaitApproveVerifyReturnComplete";

            return signal;
        }
        #endregion

        public override void OnEnterWaitApprove(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);

            long sendToUserID = scgDocument.ApproverID.Userid;

            string tokenCode = SaveResponseTokenEmail(workFlowID, sendToUserID);
            SCGEmailService.SendEmailEM01(workFlowID, sendToUserID, tokenCode);
            //#3
            //SCGSMSService.SendSMS01(workFlowID, sendToUserID);

        }

        private string SaveResponseTokenEmail(long workFlowID, long userID)
        {
            WorkFlowResponseTokenService.ClearResponseTokenByWorkFlowID(workFlowID, TokenType.Email);

            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindProxyByIdentity(workFlowID);

            string tokenCode = Guid.NewGuid().ToString();
            IList<WorkFlowStateEventPermission> stateEventPermissions = WorkFlowQueryProvider.WorkFlowStateEventPermissionQuery.FindByWorkFlowID_UserID(workFlowID, userID);
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

        protected void ValidateReturnOutStanding(FixedAdvanceDocument fixedDoc)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (fixedDoc.ReturnPaymentType == "CQ")
                errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("Return payment can't receiv payment type cheque."));

            if (fixedDoc.ReturnRequestDate == null)
                errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("Return request date is required."));

            if (string.IsNullOrEmpty(fixedDoc.ReturnPaymentType))
                errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("Return payment type is required."));

            //if (fixedDoc.ReturnServiceTeamID == null)
            //    errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("Return service teamID is required."));

            if (!errors.IsEmpty) throw new SS.Standard.Utilities.ServiceValidationException(errors);
        }

        protected void ValidateClearing(FixedAdvanceDocument fixedDoc)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (fixedDoc.PostingStatusReturn != "P")
                errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("Can't clearing if posting status return not Posting"));

            if (!errors.IsEmpty) throw new SS.Standard.Utilities.ServiceValidationException(errors);
        }

        public string OnCancelOutstanding(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

            //Validate Posting Status
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);

            //FixedAdvanceDocument fixedAdvanceDocument = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.GetFixedAdvanceByDocumentID(workFlow.Document.DocumentID);

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
            string signal = "ReturnOutstandingToDeprecated";

            return signal;
        }

        #region OnClearingWaitReturn
        public string OnClearingWaitReturn(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);

            if (!errors.IsEmpty) throw new SS.Standard.Utilities.ServiceValidationException(errors);

            FixedAdvanceDocument fixedAdvanceDocument = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.GetFixedAdvanceByDocumentID(workFlow.Document.DocumentID);

            if (scgDocument.PostingStatus == PostingStaus.New || String.IsNullOrEmpty(scgDocument.PostingStatus))
            {
                errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("Can't clearing if posting status return not Posting"));
            }
            else if (scgDocument.PostingStatus == PostingStaus.Posted)
            {
                errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("Can't clearing if posting status not Complete"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            
            string signal;
            if (scgDocument.PostingStatus != PostingStaus.Complete)
            {
                signal = "ClearingWaitReturnToWaitReturn";
            }
            else
            {
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
                ////Change Status//////
                FixedAdvanceDocument pFixedAdvanceDocument = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.GetFixedAdvanceByFixedAdvanceID((long)fixedAdvanceDocument.RefFixedAdvanceID);

                WorkFlow fixedAdvanceWorkFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(pFixedAdvanceDocument.DocumentID.DocumentID);

                // Notify Clearing all advance
                string returnEventName = "Cancel";
                WorkFlowStateEvent stateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetByWorkFlowStateID_EventName(fixedAdvanceWorkFlow.CurrentState.WorkFlowStateID, returnEventName);
                WorkFlowService.NotifyEventFromInternal(fixedAdvanceWorkFlow.WorkFlowID, returnEventName, new SubmitResponse(stateEvent.WorkFlowStateEventID));

                signal = "ClearingWaitReturnToOutstanding";
            }
            return signal;
        }
        #endregion

        #region OnReceiveWaitReturn
        public string OnReceiveWaitReturn(long workFlowID, object eventData)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);

            if (!errors.IsEmpty) throw new SS.Standard.Utilities.ServiceValidationException(errors);

            FixedAdvanceDocument fixedAdvanceDocument = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.GetFixedAdvanceByDocumentID(workFlow.Document.DocumentID);

            //if (scgDocument.PostingStatus != PostingStaus.Complete)
            //{
            if (scgDocument.PostingStatus == PostingStaus.New || scgDocument.PostingStatus == null)
                errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("Can't clearing if posting status return not Posting"));
            //}
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            try
            {
                WorkFlowResponse response = new WorkFlowResponse();
                response.WorkFlow = workFlow;
                //response.WorkFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(receiveResponse.WorkFlowStateEventID);
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
                //expenseDocument.BoxID = receiveResponse.BoxID;
                FnExpenseDocumentService.Update(expenseDocument);
                scgDocument.ReceiveDocumentDate = DateTime.Now;
            }
            catch (Exception)
            {
                throw;
            }

            string signal = "ClearingWaitReturnToWaitReturn";

            return signal;
        }
        #endregion


        #region OnClearingWaitReturnComplete
        public string OnClearingWaitReturnComplete(long workFlowID, object eventData)
        {
            /*TODO validate , save event data to workflow*/
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);

            FixedAdvanceDocument fixedDoc = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.GetFixedAdvanceByDocumentID(workFlow.Document.DocumentID);
            //ValidateClearing(fixedDoc);

            //Validate Posting Status
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
            if (fixedDoc.PostingStatusReturn == PostingStaus.New || String.IsNullOrEmpty(fixedDoc.PostingStatusReturn))
            {
                errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("Can't clearing if posting status return not Posting"));
                if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            }
            else if (fixedDoc.PostingStatusReturn == PostingStaus.Posted)
            {
                errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("Can't clearing if posting status not Complete"));
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
            string signal;

            ////Change Status//////
            if (fixedDoc.PostingStatusReturn != PostingStaus.Complete)
            {
                signal = "ClearingWaitReturnCompleteToWaitReturnComplete";
            }
            else
            {
                signal = "ClearingWaitReturnToComplete";
            }

            return signal;
        }
        #endregion

        public override IList<SuRole> GetPermitRoleCounterCashier(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            FixedAdvanceDocument fixedAdvanceDocument = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.GetFixedAdvanceByDocumentID(workFlow.Document.DocumentID);

            //Check user role is Counter Cashier
            IList<SuRole> permitRoles = QueryProvider.SuUserRoleQuery.GetRoleCounterCashier();
            if (permitRoles.Count == 0) return permitRoles;

            //Check PB
            long pbId = 0;
            if ((workFlow.CurrentState.Name == "WaitReturnComplete") || (workFlow.CurrentState.Name == "Complete"))
            {
                if (fixedAdvanceDocument.ReturnPBID != null)
                    pbId = fixedAdvanceDocument.ReturnPBID.Pbid;
                IList<SuRole> cashierRoles = QueryProvider.SuRolePBQuery.FindRoleByPBID(pbId);
                var documentPermitRoles = from role in permitRoles
                                          join cashierRole in cashierRoles
                                          on role.RoleID equals cashierRole.RoleID
                                          select role;

                return documentPermitRoles.ToList<SuRole>();
            }
            else if (workFlow.CurrentState.Name == "WaitReturn")
            {

                if (fixedAdvanceDocument.PBID != null)
                    pbId = fixedAdvanceDocument.PBID.Pbid;
                IList<SuRole> cashierRoles = QueryProvider.SuRolePBQuery.FindRoleByPBID(pbId);
                var documentPermitRoles = from role in permitRoles
                                          join cashierRole in cashierRoles
                                          on role.RoleID equals cashierRole.RoleID
                                          select role;
                return documentPermitRoles.ToList<SuRole>();
            }
            return new List<SuRole>();



        }

        public override IList<SuRole> GetPermitRoleVerifyAndApproveVerify(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            FixedAdvanceDocument fixedAdvanceDocument = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.GetFixedAdvanceByDocumentID(workFlow.Document.DocumentID);

            //Check user role can Verify document
            IList<SuRole> permitRoles = QueryProvider.SuUserRoleQuery.GetRoleVerifyAndApproveVerifyDocument();

            //Check limit verify amount
            var documentPermitRoles = from role in permitRoles
                                      where Math.Abs(fixedAdvanceDocument.NetAmount) >= role.VerifyMinLimit
                                            && Math.Abs(fixedAdvanceDocument.NetAmount) <= role.VerifyMaxLimit
                                      select role;

            permitRoles = documentPermitRoles.ToList<SuRole>();
            if (permitRoles.Count == 0) return permitRoles;

            documentPermitRoles = from role in permitRoles
                                  where Math.Abs(fixedAdvanceDocument.NetAmount) >= role.ApproveVerifyMinLimit
                                        && Math.Abs(fixedAdvanceDocument.NetAmount) <= role.ApproveVerifyMaxLimit
                                  select role;
            permitRoles = documentPermitRoles.ToList<SuRole>();
            if (permitRoles.Count == 0) return permitRoles;

            //Check Service Team
            IList<SuRole> serviceTeamRoles = QueryProvider.SuRoleServiceQuery.GetRoleByServiceTeamID(fixedAdvanceDocument.ServiceTeamID.ServiceTeamID);
            documentPermitRoles = from role in permitRoles
                                  join serviceTeamRole in serviceTeamRoles
                                  on role.RoleID equals serviceTeamRole.RoleID
                                  select role;

            return documentPermitRoles.ToList<SuRole>();
        }

        public override IList<SuRole> GetPermitRoleApproveVerify(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            FixedAdvanceDocument fixedAdvanceDocument = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.GetFixedAdvanceByDocumentID(workFlow.Document.DocumentID);


            //Check user role can ApproveVerify document
            IList<SuRole> permitRoles = QueryProvider.SuUserRoleQuery.GetRoleApproveVerifyDocument();

            //Check limit approve verify amount
            var documentPermitRoles = from role in permitRoles
                                      where Math.Abs(fixedAdvanceDocument.NetAmount) >= role.ApproveVerifyMinLimit
                                            && Math.Abs(fixedAdvanceDocument.NetAmount) <= role.ApproveVerifyMaxLimit
                                      select role;

            permitRoles = documentPermitRoles.ToList<SuRole>();
            if (permitRoles.Count == 0) return permitRoles;

            //Check Service Team
            IList<SuRole> serviceTeamRoles = QueryProvider.SuRoleServiceQuery.GetRoleByServiceTeamID(fixedAdvanceDocument.ServiceTeamID.ServiceTeamID);
            documentPermitRoles = from role in permitRoles
                                  join serviceTeamRole in serviceTeamRoles
                                  on role.RoleID equals serviceTeamRole.RoleID
                                  select role;

            return documentPermitRoles.ToList<SuRole>();
        }

        #region CanApproveWaitApproveVerifyReturnOutstanding
        public bool CanApproveWaitApproveVerifyReturnOutstanding(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleApproveWaitApproveVerify(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #region CanApproveWaitApproveVerifyReturnComplete
        public bool CanApproveWaitApproveVerifyReturnComplete(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleApproveWaitApproveVerify(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #region CanRejectWaitApproveVerifyReturnComplete
        public bool CanRejectWaitApproveVerifyReturnComplete(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleApproveWaitApproveVerify(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        #region CanRejectWaitApproveVerifyReturnOutstanding
        public bool CanRejectWaitApproveVerifyReturnOutstanding(long workFlowID)
        {
            IList<SuRole> permissionRoles = GetAllowRoleApproveWaitApproveVerify(workFlowID);
            return MatchCurrentUserRole(permissionRoles);
        }
        #endregion

        public override IList<SuRole> GetPermitRoleVerify(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            FixedAdvanceDocument fixedAdvanceDocument = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.GetFixedAdvanceByDocumentID(workFlow.Document.DocumentID);

            //Check user role can Verify document
            IList<SuRole> permitRoles = QueryProvider.SuUserRoleQuery.GetRoleVerifyDocument();

            //Check limit verify amount
            var documentPermitRoles = from role in permitRoles
                                      where Math.Abs(fixedAdvanceDocument.NetAmount) >= role.VerifyMinLimit
                                            && Math.Abs(fixedAdvanceDocument.NetAmount) <= role.VerifyMaxLimit
                                      select role;

            permitRoles = documentPermitRoles.ToList<SuRole>();
            if (permitRoles.Count == 0) return permitRoles;

            //Check Service Team
            if (fixedAdvanceDocument.ServiceTeamID == null)
            {
                return new List<SuRole>();
            }
            else
            {
                IList<SuRole> serviceTeamRoles = QueryProvider.SuRoleServiceQuery.GetRoleByServiceTeamID(fixedAdvanceDocument.ServiceTeamID.ServiceTeamID);
                documentPermitRoles = from role in permitRoles
                                      join serviceTeamRole in serviceTeamRoles
                                      on role.RoleID equals serviceTeamRole.RoleID
                                      select role; 
                return documentPermitRoles.ToList<SuRole>();
            }
        }

        public override void ReCalculatePermissionForPayWaitPaymentFromSAP(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetPermitRoleVerify(workFlowID);
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

        public void ReCalculatePermissionForReturnOutstanding(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetPermitRoleVerify(workFlowID);
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

        public void ReCalculatePermissionForApproveWaitApproveVerifyReturnOutstanding(long workFlowID, int workFlowStateEventID)
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

        public void ReCalculatePermissionForRejectWaitApproveVerifyReturnOutstanding(long workFlowID, int workFlowStateEventID)
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

        public void ReCalculatePermissionForApproveWaitApproveVerifyReturnComplete(long workFlowID, int workFlowStateEventID)
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

        public void ReCalculatePermissionForRejectWaitApproveVerifyReturnComplete(long workFlowID, int workFlowStateEventID)
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

        public void ReCalculatePermissionForClearingWaitReturn(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetPermitRoleCounterCashier(workFlowID);
            //IList<SuRole> permissionRoles = GetPermitRoleVerify(workFlowID);
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

        public void ReCalculatePermissionForClearingWaitReturnComplete(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetPermitRoleCounterCashier(workFlowID);
            //IList<SuRole> permissionRoles = GetPermitRoleVerify(workFlowID);
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
            FixedAdvanceDocument fixedAdvanceDocument = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.GetFixedAdvanceByDocumentID(scgDocument.DocumentID);
            /*TODO generate signal string*/
            if (fixedAdvanceDocument.RefFixedAdvanceID != null)
            {
                ////Change Status//////
                FixedAdvanceDocument pFixedAdvanceDocument = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.GetFixedAdvanceByFixedAdvanceID((long)fixedAdvanceDocument.RefFixedAdvanceID);

                WorkFlow fixedAdvanceWorkFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(pFixedAdvanceDocument.DocumentID.DocumentID);

                // Notify Clearing all advance
                string returnEventName = "Cancel";
                WorkFlowStateEvent stateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetByWorkFlowStateID_EventName(fixedAdvanceWorkFlow.CurrentState.WorkFlowStateID, returnEventName);
                WorkFlowService.NotifyEventFromInternal(fixedAdvanceWorkFlow.WorkFlowID, returnEventName, new SubmitResponse(stateEvent.WorkFlowStateEventID));
            }
            //////////////////////////////////////////////////Email//////////////////////////////////////////////////

            IList<long> ccList = new List<long>();
            if (scgDocument.CreatorID.Userid != scgDocument.ReceiverID.Userid)
                ccList.Add(scgDocument.CreatorID.Userid);
            if (scgDocument.CreatorID.Userid != scgDocument.RequesterID.Userid)
                ccList.Add(scgDocument.RequesterID.Userid);

            if (fixedAdvanceDocument.PaymentType == PaymentType.CQ)
            {
                SCGEmailService.SendEmailEM05(workFlowID, scgDocument.ReceiverID.Userid, ccList, true);
                IList<long> receiverList = new List<long>();
                receiverList.Add(scgDocument.ReceiverID.Userid);
                SCGSMSService.SendSMS02(workFlowID, scgDocument.RequesterID.Userid.ToString(), receiverList, true);
            }
            else if (fixedAdvanceDocument.PaymentType == PaymentType.TR)
            {
                Console.WriteLine("------------------------------------");
                SCGEmailService.SendEmailEM06(workFlowID, scgDocument.ReceiverID.Userid, ccList);
                IList<long> receiverList = new List<long>();
                receiverList.Add(scgDocument.ReceiverID.Userid);
                SCGSMSService.SendSMS03(workFlowID, scgDocument.RequesterID.Userid.ToString(), receiverList);
            }
            return signal;
        }
        #endregion

        public void ReCalculatePermissionForVerifyWaitVerifyReturnComplete(long workFlowID, int workFlowStateEventID)
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

        #region CanReceiveWaitReturn
        // public bool CanReceiveWaitReturn(long workFlowID)
        // {
        ////     IList<SuRole> permissionRoles = GetAllowUserRoleClearingComplete(workFlowID);
        //     return MatchCurrentUserRole(permissionRoles);
        // }
        #endregion

        public void ReCalculatePermissionForRejectWaitVerifyReturnComplete(long workFlowID, int workFlowStateEventID)
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

        #region ReCalculatePermissionForRejectWaitReturnToDraft and ReCalculatePermissionForRejectWaitReturnCompleteToOutstanding
        public void ReCalculatePermissionForRejectWaitReturn(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetPermitRoleCounterCashier(workFlowID);
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
        public void ReCalculatePermissionForRejectWaitReturnComplete(long workFlowID, int workFlowStateEventID)
        {
            IList<SuRole> permissionRoles = GetPermitRoleCounterCashier(workFlowID);
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

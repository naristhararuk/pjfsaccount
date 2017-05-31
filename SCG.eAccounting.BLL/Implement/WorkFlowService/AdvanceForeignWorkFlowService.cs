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

namespace SCG.eAccounting.BLL.Implement.WorkFlowService
{
    public class AdvanceForeignWorkFlowService : AdvanceWorkFlowService
    {
        #region GetVisibleFieldsFor{StateName}

        public override IList<object> GetVisibleFieldsForDraft(long workFlowID)
        {
            IList<object> visibleFieldEnum = base.GetVisibleFieldsForDraft(workFlowID);
            if(visibleFieldEnum.Contains(AdvanceFieldGroup.ServiceTeam)) 
                visibleFieldEnum.Remove(AdvanceFieldGroup.ServiceTeam);
            return visibleFieldEnum;
        }

        public IList<object> GetVisibleFieldsForWaitTA(long workFlowID)
        {
            IList<object> visibleFieldEnum = base.GetVisibleFieldsForWaitTA(workFlowID);
            if (visibleFieldEnum.Contains(AdvanceFieldGroup.ServiceTeam))
                visibleFieldEnum.Remove(AdvanceFieldGroup.ServiceTeam);
            return visibleFieldEnum;
        }

        public override IList<object> GetVisibleFieldsForWaitAR(long workFlowID)
        {
            IList<object> visibleFieldEnum = base.GetVisibleFieldsForWaitAR(workFlowID);
            if(visibleFieldEnum.Contains(AdvanceFieldGroup.ServiceTeam)) 
                visibleFieldEnum.Remove(AdvanceFieldGroup.ServiceTeam);
            return visibleFieldEnum;
        }

        public override IList<object> GetVisibleFieldsForWaitInitial(long workFlowID)
        {
            IList<object> visibleFieldEnum = base.GetVisibleFieldsForWaitInitial(workFlowID);
            if(visibleFieldEnum.Contains(AdvanceFieldGroup.ServiceTeam)) 
                visibleFieldEnum.Remove(AdvanceFieldGroup.ServiceTeam);
            return visibleFieldEnum;
        }

        public override IList<object> GetVisibleFieldsForWaitApprove(long workFlowID)
        {
            IList<object> visibleFieldEnum = base.GetVisibleFieldsForWaitApprove(workFlowID);
            if(visibleFieldEnum.Contains(AdvanceFieldGroup.ServiceTeam)) 
                visibleFieldEnum.Remove(AdvanceFieldGroup.ServiceTeam);
            return visibleFieldEnum;
        }

        public override IList<object> GetVisibleFieldsForWaitVerify(long workFlowID)
        {
            IList<object> visibleFieldEnum = base.GetVisibleFieldsForWaitVerify(workFlowID);
            if(visibleFieldEnum.Contains(AdvanceFieldGroup.ServiceTeam)) 
                visibleFieldEnum.Remove(AdvanceFieldGroup.ServiceTeam);
            return visibleFieldEnum;
        }

        public override IList<object> GetVisibleFieldsForWaitApproveVerify(long workFlowID)
        {
           IList<object> visibleFieldEnum = base.GetVisibleFieldsForWaitApproveVerify(workFlowID);
            if(visibleFieldEnum.Contains(AdvanceFieldGroup.ServiceTeam)) 
                visibleFieldEnum.Remove(AdvanceFieldGroup.ServiceTeam);
            return visibleFieldEnum;
        }

        public override IList<object> GetVisibleFieldsForHold(long workFlowID)
        {
            IList<object> visibleFieldEnum = base.GetVisibleFieldsForHold(workFlowID);
            if(visibleFieldEnum.Contains(AdvanceFieldGroup.ServiceTeam)) 
                visibleFieldEnum.Remove(AdvanceFieldGroup.ServiceTeam);
            return visibleFieldEnum;
        }

        public override IList<object> GetVisibleFieldsForWaitPayment(long workFlowID)
        {
            IList<object> visibleFieldEnum = base.GetVisibleFieldsForWaitPayment(workFlowID);
            if(visibleFieldEnum.Contains(AdvanceFieldGroup.ServiceTeam)) 
                visibleFieldEnum.Remove(AdvanceFieldGroup.ServiceTeam);

            if (visibleFieldEnum.Contains(AdvanceFieldGroup.VerifyDetail))
                visibleFieldEnum.Remove(AdvanceFieldGroup.VerifyDetail);

            return visibleFieldEnum;
        }


        public override IList<object> GetVisibleFieldsForWaitPaymentFromSAP(long workFlowID)
        {
            IList<object> visibleFieldEnum = base.GetVisibleFieldsForWaitPaymentFromSAP(workFlowID);
            if(visibleFieldEnum.Contains(AdvanceFieldGroup.ServiceTeam)) 
                visibleFieldEnum.Remove(AdvanceFieldGroup.ServiceTeam);

            if (visibleFieldEnum.Contains(AdvanceFieldGroup.VerifyDetail))
                visibleFieldEnum.Remove(AdvanceFieldGroup.VerifyDetail);

            return visibleFieldEnum;
        }

        public override IList<object> GetVisibleFieldsForWaitApproveRejection(long workFlowID)
        {
            IList<object> visibleFieldEnum = base.GetVisibleFieldsForWaitApproveRejection(workFlowID);
            if(visibleFieldEnum.Contains(AdvanceFieldGroup.ServiceTeam)) 
                visibleFieldEnum.Remove(AdvanceFieldGroup.ServiceTeam);
            return visibleFieldEnum;
        }

        public override IList<object> GetVisibleFieldsForWaitDocument(long workFlowID)
        {
            IList<object> visibleFieldEnum = base.GetVisibleFieldsForWaitDocument(workFlowID);
            if(visibleFieldEnum.Contains(AdvanceFieldGroup.ServiceTeam)) 
                visibleFieldEnum.Remove(AdvanceFieldGroup.ServiceTeam);
            return visibleFieldEnum;
        }


        public IList<object> GetVisibleFieldsForOutstanding(long workFlowID)
        {
            IList<object> visibleFieldEnum = base.GetVisibleFieldsForOutstanding(workFlowID);
            if(visibleFieldEnum.Contains(AdvanceFieldGroup.ServiceTeam)) 
                visibleFieldEnum.Remove(AdvanceFieldGroup.ServiceTeam);

            if (visibleFieldEnum.Contains(AdvanceFieldGroup.VerifyDetail)) 
                visibleFieldEnum.Remove(AdvanceFieldGroup.VerifyDetail);
            
            return visibleFieldEnum;
        }

        public override IList<object> GetVisibleFieldsForComplete(long workFlowID)
        {
            IList<object> visibleFieldEnum = base.GetVisibleFieldsForComplete(workFlowID);
            if(visibleFieldEnum.Contains(AdvanceFieldGroup.ServiceTeam)) 
                visibleFieldEnum.Remove(AdvanceFieldGroup.ServiceTeam);

            if (visibleFieldEnum.Contains(AdvanceFieldGroup.VerifyDetail))
                visibleFieldEnum.Remove(AdvanceFieldGroup.VerifyDetail);
            
            return visibleFieldEnum;
        }

        #endregion

        #region GetRole{EventName}
        public override IList<SuRole> GetPermitRoleVerify(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            AvAdvanceDocument advanceDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(workFlow.Document.DocumentID);

            //Check user role can Verify document
            IList<SuRole> permitRoles = QueryProvider.SuUserRoleQuery.GetRoleVerifyPayment();
            if (permitRoles.Count == 0) return permitRoles;

            //Check Service Team
            //IList<SuRole> serviceTeamRoles = QueryProvider.SuRoleServiceQuery.GetRoleByServiceTeamID(advanceDocument.ServiceTeamID.ServiceTeamID);
            //var documentPermitRoles = from role in permitRoles
            //                      join serviceTeamRole in serviceTeamRoles
            //                      on role.RoleID equals serviceTeamRole.RoleID
            //                      select role;

            //return documentPermitRoles.ToList<SuRole>();

            //Check PB
            long pbId = 0;
            if (advanceDocument.PBID != null)
                pbId = advanceDocument.PBID.Pbid;
            IList<SuRole> cashierRoles = QueryProvider.SuRolePBQuery.FindRoleByPBID(pbId);
            var documentPermitRoles = from role in permitRoles
                                      join cashierRole in cashierRoles
                                      on role.RoleID equals cashierRole.RoleID
                                      select role;

            return documentPermitRoles.ToList<SuRole>();
        }

        public override IList<SuRole> GetPermitRoleApproveVerify(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            AvAdvanceDocument advanceDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(workFlow.Document.DocumentID);

            //Check user role can ApproveVerify document
            IList<SuRole> permitRoles = QueryProvider.SuUserRoleQuery.GetRoleApproveVerifyPayment();
            if (permitRoles.Count == 0) return permitRoles;

            //Check Service Team
            //IList<SuRole> serviceTeamRoles = QueryProvider.SuRoleServiceQuery.GetRoleByServiceTeamID(advanceDocument.ServiceTeamID.ServiceTeamID);
            //var documentPermitRoles = from role in permitRoles
            //                      join serviceTeamRole in serviceTeamRoles
            //                      on role.RoleID equals serviceTeamRole.RoleID
            //                      select role;

            //return documentPermitRoles.ToList<SuRole>();

            //Check PB
            long pbId = 0;
            if (advanceDocument.PBID != null)
                pbId = advanceDocument.PBID.Pbid;
            IList<SuRole> cashierRoles = QueryProvider.SuRolePBQuery.FindRoleByPBID(pbId);
            var documentPermitRoles = from role in permitRoles
                                      join cashierRole in cashierRoles
                                      on role.RoleID equals cashierRole.RoleID
                                      select role;

            return documentPermitRoles.ToList<SuRole>();
        }

        public override IList<SuRole> GetPermitRoleVerifyAndApproveVerify(long workFlowID)
        {
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            AvAdvanceDocument advanceDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(workFlow.Document.DocumentID);

            //Check user role can Verify document
            IList<SuRole> permitRoles = QueryProvider.SuUserRoleQuery.GetRoleVerifyAndApproveVerifyPayment();

            //Check Service Team
            //IList<SuRole> serviceTeamRoles = QueryProvider.SuRoleServiceQuery.GetRoleByServiceTeamID(advanceDocument.ServiceTeamID.ServiceTeamID);
            //var documentPermitRoles = from role in permitRoles
            //                      join serviceTeamRole in serviceTeamRoles
            //                      on role.RoleID equals serviceTeamRole.RoleID
            //                      select role;

            //return documentPermitRoles.ToList<SuRole>();

            //Check PB
            long pbId = 0;
            if (advanceDocument.PBID != null)
                pbId = advanceDocument.PBID.Pbid;
            IList<SuRole> cashierRoles = QueryProvider.SuRolePBQuery.FindRoleByPBID(pbId);
            var documentPermitRoles = from role in permitRoles
                                      join cashierRole in cashierRoles
                                      on role.RoleID equals cashierRole.RoleID
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
            //IList<SuRole> serviceTeamRoles = QueryProvider.SuRoleServiceQuery.GetRoleByServiceTeamID(advanceDocument.ServiceTeamID.ServiceTeamID);
            //var documentPermitRoles = from role in permitRoles
            //                          join serviceTeamRole in serviceTeamRoles
            //                          on role.RoleID equals serviceTeamRole.RoleID
            //                          select role;

            //return documentPermitRoles.ToList<SuRole>();
            //Check PB
            long pbId = 0;
            if (advanceDocument.PBID != null)
                pbId = advanceDocument.PBID.Pbid;
            IList<SuRole> cashierRoles = QueryProvider.SuRolePBQuery.FindRoleByPBID(pbId);
            var documentPermitRoles = from role in permitRoles
                                      join cashierRole in cashierRoles
                                      on role.RoleID equals cashierRole.RoleID
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

            ////Check Service Team
            //IList<SuRole> serviceTeamRoles = QueryProvider.SuRoleServiceQuery.GetRoleByServiceTeamID(advanceDocument.ServiceTeamID.ServiceTeamID);
            //var documentPermitRoles = from role in permitRoles
            //                          join serviceTeamRole in serviceTeamRoles
            //                          on role.RoleID equals serviceTeamRole.RoleID
            //                          select role;

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

        #region GetTotalDocumentAmount
        public override double GetTotalDocumentAmount(long documentID)
        {
            return 0;
        }
        #endregion
    }
}

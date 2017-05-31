using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.BLL.WorkFlowService
{
    public interface IGeneralWorkFlowService
    {
        long GetPermitUserDraft(long workFlowID);
        long GetPermitUserHold(long workFlowID);

        long GetAllowUserSendDraft(long workFlowID);
        long GetAllowUserCancelDraft(long workFlowID);

        long GetAllowUserApproveWaitAR(long workFlowID);
        long GetAllowUserRejectWaitAR(long workFlowID);
        long GetAllowUserWithdrawWaitAR(long workFlowID);

        long GetAllowUserApproveWaitInitial(long workFlowID);
        long GetAllowUserRejectWaitInitial(long workFlowID);
        long GetAllowUserWithdrawWaitInitial(long workFlowID);
        long GetAllowUserOverRoleWaitInitial(long workFlowID);

        long GetAllowUserApproveWaitApprove(long workFlowID);
        long GetAllowUserRejectWaitApprove(long workFlowID);
        long GetAllowUserWithdrawWaitApprove(long workFlowID);
        long GetAllowUserOverRoleWaitApprove(long workFlowID);

        void OnDeleteDocument(long workFlowID);

        bool CanSendDraft(long workFlowID);
        bool CanCancelDraft(long workFlowID);

        bool CanApproveWaitAR(long workFlowID);
        bool CanRejectWaitAR(long workFlowID);
        bool CanWithdrawWaitAR(long workFlowID);

        bool CanApproveWaitInitial(long workFlowID);
        bool CanRejectWaitInitial(long workFlowID);
        bool CanWithdrawWaitInitial(long workFlowID);
        bool CanOverRoleWaitInitial(long workFlowID);

        bool CanApproveWaitApprove(long workFlowID);
        bool CanRejectWaitApprove(long workFlowID);
        bool CanWithdrawWaitApprove(long workFlowID);
        bool CanOverRoleWaitApprove(long workFlowID);

        string OnSendDraft(long workFlowID, object eventData);
        string OnCancelDraft(long workFlowID, object eventData);

        string OnApproveWaitAR(long workFlowID, object eventData);
        string OnRejectWaitAR(long workFlowID, object eventData);
        string OnWithdrawWaitAR(long workFlowID, object eventData);

        string OnWithdrawWaitInitial(long workFlowID, object eventData);
        string OnOverRoleWaitInitial(long workFlowID, object eventData);
        string OnApproveWaitInitial(long workFlowID, object eventData);
        string OnRejectWaitInitial(long workFlowID, object eventData);

        string OnWithdrawWaitApprove(long workFlowID, object eventData);
        string OnOverRoleWaitApprove(long workFlowID, object eventData);
        string OnApproveWaitApprove(long workFlowID, object eventData);
        string OnRejectWaitApprove(long workFlowID, object eventData);

		bool CanDeleteDraft(long workFlowID);

        void OnEnterDraft(long workFlowID);
        void OnExitDraft(long workFlowID);

        void OnEnterWaitAR(long workFlowID);
        void OnExitWaitAR(long workFlowID);

        void OnEnterWaitInitial(long workFlowID);
        void OnExitWaitInitial(long workFlowID);

        void OnEnterWaitApprove(long workFlowID);
        void OnExitWaitApprove(long workFlowID);

        void OnEnterComplete(long workFlowID);
        void OnExitComplete(long workFlowID);

        void OnEnterCancel(long workFlowID);
        void OnExitCancel(long workFlowID);

        void ReCalculatePermissionForSendDraft(long workFlowID , int workFlowStateEventID);
        void ReCalculatePermissionForCancelDraft(long workFlowID, int workFlowStateEventID);

        void ReCalculatePermissionForApproveWaitAR(long workFlowID, int workFlowStateEventID);
        void ReCalculatePermissionForRejectWaitAR(long workFlowID, int workFlowStateEventID);
        void ReCalculatePermissionForWithdrawWaitAR(long workFlowID, int workFlowStateEventID);

        void ReCalculatePermissionForWithdrawWaitInitial(long workFlowID, int workFlowStateEventID);
        void ReCalculatePermissionForOverRoleWaitInitial(long workFlowID, int workFlowStateEventID);
        void ReCalculatePermissionForApproveWaitInitial(long workFlowID, int workFlowStateEventID);
        void ReCalculatePermissionForRejectWaitInitial(long workFlowID, int workFlowStateEventID);

        void ReCalculatePermissionForWithdrawWaitApprove(long workFlowID, int workFlowStateEventID);
        void ReCalculatePermissionForOverRoleWaitApprove(long workFlowID, int workFlowStateEventID);
        void ReCalculatePermissionForApproveWaitApprove(long workFlowID, int workFlowStateEventID);
        void ReCalculatePermissionForRejectWaitApprove(long workFlowID, int workFlowStateEventID);

        void ReCalculatePermission(short roleID);
        void ReCalculatePermission();

        #region CanCopy
        bool CanCopyCancel(long workFlowID);
        bool CanCopyComplete(long workFlowID);
        bool CanCopyDraft(long workFlowID);
        bool CanCopyHold(long workFlowID);
        bool CanCopyOutstanding(long workFlowID);
        bool CanCopyWaitApprove(long workFlowID);
        bool CanCopyWaitApproveRejection(long workFlowID);
        bool CanCopyWaitApproveVerify(long workFlowID);
        bool CanCopyWaitAR(long workFlowID);
        bool CanCopyWaitDocument(long workFlowID);
        bool CanCopyWaitInitial(long workFlowID);
        bool CanCopyWaitPayment(long workFlowID);
        bool CanCopyWaitPaymentFromSAP(long workFlowID);
        bool CanCopyWaitRemittance(long workFlowID);
        bool CanCopyWaitReverse(long workFlowID);
        bool CanCopyWaitTA(long workFlowID);
        bool CanCopyWaitTAApprove(long workFlowID);
        bool CanCopyWaitTAInitial(long workFlowID);
        bool CanCopyWaitVerify(long workFlowID);
        #endregion 
    }
}

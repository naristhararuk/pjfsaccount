using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.SU.DTO;

namespace SCG.eAccounting.BLL.WorkFlowService
{
    public interface IRemittanceWorkFlowService
    {
        #region GetAllowUser{EventName}{StateName}
        long GetAllowUserSendDraft(long workFlowID);
        long GetAllowUserWithdrawWaitRemittance(long workFlowID);
        #endregion

        #region GetAllowRole{EventName}{StateName}
        IList<SuRole> GetAllowRoleVerifyWaitRemittance(long workFlowID);
        IList<SuRole> GetAllowRoleVerifyAndApproveVerifyWaitRemittance(long workFlowID);
        IList<SuRole> GetAllowRoleRejectWaitRemittance(long workFlowID);
        
        IList<SuRole> GetAllowRoleApproveWaitApproveRemittance(long workFlowID);
        IList<SuRole> GetAllowRoleRejectWaitApproveRemittance(long workFlowID);
        
        #endregion

        #region GetEditableFieldsFor{StateName}

        IList<object> GetEditableFieldsForDraft(long workFlowID);
        IList<object> GetEditableFieldsForWaitRemittance(long workFlowID);
        
        #endregion

        #region GetVisibleFieldsFor{StateName}

        IList<object> GetVisibleFieldsForDraft(long workFlowID);
        IList<object> GetVisibleFieldsForWaitRemittance(long workFlowID);
        IList<object> GetVisibleFieldsForWaitApproveRemittance(long workFlowID);
        IList<object> GetVisibleFieldsForComplete(long workFlowID);
        IList<object> GetVisibleFieldsForCancel(long workFlowID);
        #endregion

        bool CanEditDraft(long workFlowID);
        bool CanEditWaitRemittance(long workFlowID);
        bool CanDeleteDraft(long workFlowID);

        void DeleteDocument(long workFlowID);

        #region Can{EventName}{StateName}
        bool CanSendDraft(long workFlowID);
        bool CanCancelDraft(long workFlowID);

        bool CanVerifyWaitRemittance(long workFlowID);
        bool CanVerifyAndApproveVerifyWaitRemittance(long workFlowID);
        bool CanRejectWaitRemittance(long workFlowID);
        bool CanWithdrawWaitRemittance(long workFlowID);

        bool CanApproveWaitApproveRemittance(long workFlowID);
        bool CanRejectWaitApproveRemittance(long workFlowID);
        #endregion

        #region On{EventName}{StateName}
        string OnSendDraft(long workFlowID, object eventData);
        string OnCancelDraft(long workFlowID, object eventData);
        
        string OnVerifyWaitRemittance(long workFlowID, object eventData);
        string OnVerifyAndApproveVerifyWaitRemittance(long workFlowID, object eventData);
        string OnRejectWaitRemittance(long workFlowID, object eventData);
        string OnWithdrawWaitRemittance(long workFlowID, object eventData);

        string OnApproveWaitApproveRemittance(long workFlowID, object eventData);
        string OnRejectWaitApproveRemittance(long workFlowID, object eventData);
        #endregion

        #region OnEnter/Exit{StateName}
        void OnEnterDraft(long workFlowID);
        void OnExitDraft(long workFlowID);

        void OnEnterWaitRemittance(long workFlowID);
        void OnExitWaitRemittance(long workFlowID);

        void OnEnterWaitApproveRemittance(long workFlowID);
        void OnExitWaitApproveRemittance(long workFlowID);

        void OnEnterComplete(long workFlowID);
        void OnExitComplete(long workFlowID);
        void OnEnterCancel(long workFlowID);
        void OnExitCancel(long workFlowID);

        #endregion

        void ReCalculatePermissionForSendDraft(long workFlowID, int workFlowStateEventID);
        void ReCalculatePermissionForVerifyWaitRemittance(long workFlowID, int workFlowStateEventID);
        void ReCalculatePermissionForVerifyAndApproveVerifyWaitRemittance(long workFlowID, int workFlowStateEventID);
        void ReCalculatePermissionForRejectWaitRemittance(long workFlowID, int workFlowStateEventID);
        void ReCalculatePermissionForWithdrawWaitRemittance(long workFlowID, int workFlowStateEventID);

        void ReCalculatePermissionForApproveWaitApproveRemittance(long workFlowID, int workFlowStateEventID);
        void ReCalculatePermissionForRejectWaitApproveRemittance(long workFlowID, int workFlowStateEventID);

        #region CanCopy
        bool CanCopyCancel(long workFlowID);
        bool CanCopyComplete(long workFlowID);
        bool CanCopyDraft(long workFlowID);
        bool CanCopyWaitApproveRemittance(long workFlowID);
        bool CanCopyWaitRemittance(long workFlowID);
        #endregion 

    }
}

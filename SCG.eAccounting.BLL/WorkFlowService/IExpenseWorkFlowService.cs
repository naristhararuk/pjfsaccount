using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.SU.DTO;

namespace SCG.eAccounting.BLL.WorkFlowService
{
    public interface IExpenseWorkFlowService : IGeneralWorkFlowService
    {
        #region CanEdit{StateName}
        bool CanEditDraft(long workFlowID);
        bool CanEditWaitVerify(long workFlowID);
        bool CanEditHold(long workFlowID);
        bool CanEditWaitPayment(long workFlowID);
        #endregion

        #region GetPermitRole
        IList<SuRole> GetPermitRoleVerify(long workFlowID);
        IList<SuRole> GetPermitRoleApproveVerify(long workFlowID);
        IList<SuRole> GetPermitRoleReceive(long workFlowID);
        IList<SuRole> GetPermitRoleWithdraw(long workFlowID);
        IList<SuRole> GetPermitRoleCounterCashier(long workFlowID);
        #endregion

        #region GetAllowRole{EventName}{StateName}

        IList<SuRole> GetAllowRoleVerifyWaitVerify(long workFlowID);
        IList<SuRole> GetAllowRoleRejectWaitVerify(long workFlowID);
        IList<SuRole> GetAllowRoleHoldWaitVerify(long workFlowID);
        IList<SuRole> GetAllowRoleReceiveWaitVerify(long workFlowID);

        IList<SuRole> GetAllowRoleApproveWaitApproveVerify(long workFlowID);
        IList<SuRole> GetAllowRoleRejectWaitApproveVerify(long workFlowID);
        IList<SuRole> GetAllowRoleReceiveWaitApproveVerify(long workFlowID);

        long GetAllowUserSendDraft(long workFlowID);
        long GetAllowUserSendHold(long workFlowID);

        IList<SuRole> GetAllowRoleVerifyHold(long workFlowID);
        IList<SuRole> GetAllowRoleRejectHold(long workFlowID);
        IList<SuRole> GetAllowRoleReceiveHold(long workFlowID);

        IList<SuRole> GetAllowRolePayWaitPayment(long workFlowID);
        IList<SuRole> GetAllowRoleWithdrawWaitPayment(long workFlowID);
        IList<SuRole> GetAllowRoleReceiveWaitPayment(long workFlowID);
        IList<SuRole> GetAllowRoleReceiveComplete(long workFlowID);

        IList<SuRole> GetAllowRoleReceiveWaitPaymentFromSAP(long workFlowID);
        IList<SuRole> GetAllowRoleWithdrawWaitPaymentFromSAP(long workFlowID);

        IList<SuRole> GetAllowRoleWithdrawWaitRemittance(long workFlowID);
        IList<SuRole> GetAllowRolePayWaitRemittance(long workFlowID);
        
        IList<SuRole> GetAllowRoleApproveWaitApproveRejection(long workFlowID);
        IList<SuRole> GetAllowRoleRejectWaitApproveRejection(long workFlowID);
        IList<SuRole> GetAllowRoleReceiveWaitApproveRejection(long workFlowID);

        IList<SuRole> GetAllowRoleReceiveWaitDocument(long workFlowID);
        IList<SuRole> GetAllowRoleRejectWaitDocument(long workFlowID);


        #endregion

        #region GetEditableFieldsFor{StateName}

        IList<object> GetEditableFieldsForDraft(long workFlowID);
        IList<object> GetEditableFieldsForHold(long workFlowID);
        IList<object> GetEditableFieldsForWaitVerify(long workFlowID);
        IList<object> GetEditableFieldsForWaitPayment(long workFlowID);

        #endregion

        #region GetVisibleFieldsFor{StateName}
        
        IList<object> GetVisibleFieldsForDraft(long workFlowID);
        IList<object> GetVisibleFieldsForWaitAR(long workFlowID);
        IList<object> GetVisibleFieldsForWaitInitial(long workFlowID);
        IList<object> GetVisibleFieldsForWaitApprove(long workFlowID);
        IList<object> GetVisibleFieldsForWaitVerify(long workFlowID);
        IList<object> GetVisibleFieldsForWaitApproveVerify(long workFlowID);
        IList<object> GetVisibleFieldsForHold(long workFlowID);
        IList<object> GetVisibleFieldsForWaitPayment(long workFlowID);
        IList<object> GetVisibleFieldsForWaitPaymentFromSAP(long workFlowID);
        IList<object> GetVisibleFieldsForWaitRemittance(long workFlowID);
        IList<object> GetVisibleFieldsForWaitApproveRejection(long workFlowID);
        IList<object> GetVisibleFieldsForWaitDocument(long workFlowID);
        IList<object> GetVisibleFieldsForComplete(long workFlowID);
        IList<object> GetVisibleFieldsForCancel(long workFlowID);
        #endregion

        #region Can{EventName}{StateName}
        
        bool CanRejectWaitVerify(long workFlowID);
        bool CanVerifyWaitVerify(long workFlowID);
        bool CanVerifyAndApproveVerifyWaitVerify(long workFlowID);
        bool CanHoldWaitVerify(long workFlowID);
        bool CanReceiveWaitVerify(long workFlowID);


        bool CanApproveWaitApproveVerify(long workFlowID);
        bool CanRejectWaitApproveVerify(long workFlowID);
        bool CanVerifyAndApproveVerifyWaitApproveVerify(long workFlowID);
        bool CanHoldWaitApproveVerify(long workFlowID);
        bool CanReceiveWaitApproveVerify(long workFlowID);

        bool CanSendHold(long workFlowID);
        //bool CanVerifyHold(long workFlowID);
        bool CanRejectHold(long workFlowID);
        bool CanReceiveHold(long workFlowID);
        bool CanUnHoldHold(long workFlowID);

        bool CanPayWaitPayment(long workFlowID);
        bool CanWithdrawWaitPayment(long workFlowID);
        bool CanReceiveWaitPayment(long workFlowID);
        bool CanReceiveComplete(long workFlowID);

        bool CanPayWaitPaymentFromSAP(long workFlowID);
        bool CanReceiveWaitPaymentFromSAP(long workFlowID);
        bool CanWithdrawWaitPaymentFromSAP(long workFlowID);

        bool CanWithdrawWaitRemittance(long workFlowID); 
        bool CanPayWaitRemittance(long workFlowID);

        bool CanPrintPayInWaitRemittance(long workFlowID);
        
        bool CanApproveWaitApproveRejection(long workFlowID);
        bool CanRejectWaitApproveRejection(long workFlowID);
        bool CanReceiveWaitApproveRejection(long workFlowID);

        bool CanApproveWaitReverse(long workFlowID);
        bool CanRejectWaitReverse(long workFlowID);

        bool CanReceiveWaitDocument(long workFlowID);
        bool CanRejectWaitDocument(long workFlowID);
        bool CanReceiveWaitRemittance(long workFlowID);

        #endregion

        #region On{EventName}{StateName}
        
        string OnRejectWaitVerify(long workFlowID, object eventData);
        string OnVerifyWaitVerify(long workFlowID, object eventData);
        string OnVerifyAndApproveVerifyWaitVerify(long workFlowID, object eventData);
        string OnHoldWaitVerify(long workFlowID, object eventData);
        string OnReceiveWaitVerify(long workFlowID, object eventData);

        string OnApproveWaitApproveVerify(long workFlowID, object eventData);
        string OnRejectWaitApproveVerify(long workFlowID, object eventData);
        string OnVerifyAndApproveVerifyWaitApproveVerify(long workFlowID, object eventData);
        string OnReceiveWaitApproveVerify(long workFlowID, object eventData);

        string OnSendHold(long workFlowID, object eventData);
        //string OnVerifyHold(long workFlowID, object eventData);
        string OnRejectHold(long workFlowID, object eventData);
        string OnReceiveHold(long workFlowID, object eventData);
        string OnUnHoldHold(long workFlowID, object eventData);

        string OnPayWaitPayment(long workFlowID, object eventData);
        string OnWithdrawWaitPayment(long workFlowID, object eventData);
        string OnReceiveWaitPayment(long workFlowID, object eventData);
        string OnReceiveComplete(long workFlowID, object eventData);

        string OnPayWaitPaymentFromSAP(long workFlowID, object eventData);
        string OnReceiveWaitPaymentFromSAP(long workFlowID, object eventData);
        string OnWithdrawWaitPaymentFromSAP(long workFlowID, object eventData);

        string OnWithdrawWaitRemittance(long workFlowID, object eventData);
        string OnPayWaitRemittance(long workFlowID, object eventData);
        
        string OnApproveWaitApproveRejection(long workFlowID, object eventData);
        string OnRejectWaitApproveRejection(long workFlowID, object eventData);
        string OnReceiveWaitApproveRejection(long workFlowID, object eventData);

        string OnApproveWaitReverse(long workFlowID, object eventData);
        string OnRejectWaitReverse(long workFlowID, object eventData);

        string OnReceiveWaitDocument(long workFlowID, object eventData);
        string OnRejectWaitDocument(long workFlowID, object eventData);
        string OnReceiveWaitRemittance(long workFlowID, object eventData);
        #endregion

        #region OnEnter{StateName}
        void OnEnterWaitVerify(long workFlowID);
        void OnExitWaitVerify(long workFlowID);

        void OnEnterWaitApproveVerify(long workFlowID);
        void OnExitWaitApproveVerify(long workFlowID);

        void OnEnterHold(long workFlowID);
        void OnExitHold(long workFlowID);

        void OnEnterWaitPayment(long workFlowID);
        void OnExitWaitPayment(long workFlowID);

        void OnEnterWaitPaymentFromSAP(long workFlowID);
        void OnExitWaitPaymentFromSAP(long workFlowID);

        void OnEnterWaitRemittance(long workFlowID);
        void OnExitWaitRemittance(long workFlowID);

        void OnEnterWaitApproveRejection(long workFlowID);
        void OnExitWaitApproveRejection(long workFlowID);

        void OnEnterWaitReverse(long workFlowID);
        void OnExitWaitReverse(long workFlowID);

        void OnEnterWaitDocument(long workFlowID);
        void OnExitWaitDocument(long workFlowID);
        #endregion

        #region ReCalculatePermissionFor{EventName}{StateName}

        void ReCalculatePermissionForRejectWaitVerify(long workFlowID , int workFlowStateEventID);
        void ReCalculatePermissionForVerifyWaitVerify(long workFlowID , int workFlowStateEventID);
        void ReCalculatePermissionForVerifyAndApproveVerifyWaitVerify(long workFlowID , int workFlowStateEventID);
        void ReCalculatePermissionForHoldWaitVerify(long workFlowID , int workFlowStateEventID);
        void ReCalculatePermissionForReceiveWaitVerify(long workFlowID , int workFlowStateEventID);

        void ReCalculatePermissionForApproveWaitApproveVerify(long workFlowID , int workFlowStateEventID);
        void ReCalculatePermissionForRejectWaitApproveVerify(long workFlowID , int workFlowStateEventID);
        void ReCalculatePermissionForVerifyAndApproveVerifyWaitApproveVerify(long workFlowID , int workFlowStateEventID);
        void ReCalculatePermissionForHoldWaitApproveVerify(long workFlowID , int workFlowStateEventID);
        void ReCalculatePermissionForReceiveWaitApproveVerify(long workFlowID , int workFlowStateEventID);
        void ReCalculatePermissionForReceiveComplete(long workFlowID, int workFlowStateEventID);

        void ReCalculatePermissionForSendHold(long workFlowID , int workFlowStateEventID);
        void ReCalculatePermissionForVerifyHold(long workFlowID , int workFlowStateEventID);
        void ReCalculatePermissionForRejectHold(long workFlowID , int workFlowStateEventID);
        void ReCalculatePermissionForReceiveHold(long workFlowID , int workFlowStateEventID);

        void ReCalculatePermissionForPayWaitPayment(long workFlowID , int workFlowStateEventID);
        void ReCalculatePermissionForWithdrawWaitPayment(long workFlowID , int workFlowStateEventID);
        void ReCalculatePermissionForReceiveWaitPayment(long workFlowID , int workFlowStateEventID);

        void ReCalculatePermissionForPayWaitPaymentFromSAP(long workFlowID , int workFlowStateEventID);
        void ReCalculatePermissionForReceiveWaitPaymentFromSAP(long workFlowID , int workFlowStateEventID);

        void ReCalculatePermissionForWithdrawWaitRemittance(long workFlowID, int workFlowStateEventID);
        void ReCalculatePermissionForPayWaitRemittance(long workFlowID, int workFlowStateEventID);
        
        void ReCalculatePermissionForApproveWaitApproveRejection(long workFlowID , int workFlowStateEventID);
        void ReCalculatePermissionForRejectWaitApproveRejection(long workFlowID , int workFlowStateEventID);
        void ReCalculatePermissionForReceiveWaitApproveRejection(long workFlowID , int workFlowStateEventID);

        
        void ReCalculatePermissionForReceiveWaitDocument(long workFlowID , int workFlowStateEventID);
        void ReCalculatePermissionForRejectWaitDocument(long workFlowID , int workFlowStateEventID);

        #endregion
    }
}

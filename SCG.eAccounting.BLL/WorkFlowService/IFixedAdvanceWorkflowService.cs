using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.BLL.WorkFlowService
{
    public interface IFixedAdvanceWorkflowService : IExpenseWorkFlowService
    {
        //bool CanEditDraft(long workFlowID);
        IList<object> GetVisibleFieldsForOutstanding(long workFlowID);
        bool CanReturnOutstanding(long workFlowID);
        void ReCalculatePermissionForReturnOutstanding(long workFlowID, int workFlowStateEventID);
        string OnReturnOutstanding(long workFlowID, object eventData);
        IList<object> GetVisibleFieldsForWaitReturn(long workFlowID);
        bool CanClearingWaitReturn(long workFlowID);
        void ReCalculatePermissionForClearingWaitReturn(long workFlowID, int workFlowStateEventID);
        string OnClearingWaitReturn(long workFlowID, object eventData);
        bool CanCancelOutstanding(long workFlowID);
        string OnCancelOutstanding(long workFlowID, object eventData);
        IList<object> GetVisibleFieldsForDeprecated(long workFlowID);
        IList<object> GetEditableFieldsForOutstanding(long workFlowID);
        bool CanEditOutstanding(long workFlowID);
        string OnClearingWaitReturnComplete(long workFlowID, object eventData);
        IList<object> GetVisibleFieldsForWaitReturnComplete(long workFlowID);
        bool CanClearingWaitReturnComplete(long workFlowID);
        void ReCalculatePermissionForClearingWaitReturnComplete(long workFlowID, int workFlowStateEventID);
        IList<object> GetEditableFieldsForWaitReturn(long workFlowID);
        IList<object> GetEditableFieldsForWaitReturnComplete(long workFlowID);
        bool CanEditWaitReturnComplete(long workFlowID);
        string OnApproveWaitApproveVerifyReturnOutstanding(long workFlowID, object eventData);
        string OnRejectWaitApproveVerifyReturnOutstanding(long workFlowID, object eventData);
        bool CanEditWaitReturn(long workFlowID);
        bool CanApproveWaitApproveVerifyReturnOutstanding(long workFlowID);
        bool CanRejectWaitApproveVerifyReturnOutstanding(long workFlowID);
        void ReCalculatePermissionForApproveWaitApproveVerifyReturnOutstanding(long workFlowID, int workFlowStateEventID);
        void ReCalculatePermissionForRejectWaitApproveVerifyReturnOutstanding(long workFlowID, int workFlowStateEventID);
        IList<object> GetVisibleFieldsForWaitApproveVerifyReturnOutstanding(long workFlowID);
        IList<object> GetEditableFieldsForWaitVerifyReturnComplete(long workFlowID);
        IList<object> GetVisibleFieldsForWaitVerifyReturnComplete(long workFlowID);
        bool CanVerifyWaitVerifyReturnComplete(long workFlowID);
        bool CanRejectWaitVerifyReturnComplete(long workFlowID);
        string OnVerifyWaitVerifyReturnComplete(long workFlowID, object eventData);
        void ReCalculatePermissionForVerifyWaitVerifyReturnComplete(long workFlowID, int workFlowStateEventID);
        void ReCalculatePermissionForRejectWaitVerifyReturnComplete(long workFlowID, int workFlowStateEventID);
        bool CanEditWaitVerifyReturnComplete(long workFlowID);
        string OnApproveWaitApproveVerifyReturnComplete(long workFlowID, object eventData);
        IList<object> GetVisibleFieldsForWaitApproveVerifyReturnComplete(long workFlowID);
        void ReCalculatePermissionForApproveWaitApproveVerifyReturnComplete(long workFlowID, int workFlowStateEventID);
        void ReCalculatePermissionForRejectWaitApproveVerifyReturnComplete(long workFlowID, int workFlowStateEventID);
        bool CanApproveWaitApproveVerifyReturnComplete(long workFlowID);
        bool CanRejectWaitApproveVerifyReturnComplete(long workFlowID);
        string OnRejectWaitApproveVerifyReturnComplete(long workFlowID, object eventData);
        string OnReceiveWaitReturn(long workFlowID, object eventData);

        bool CanRejectWaitReturn(long workFlowID);
        bool CanRejectWaitReturnComplete(long workFlowID);
        string OnRejectWaitReturn(long workFlowID, object eventData);
        string OnRejectWaitReturnComplete(long workFlowID, object eventData);
        void ReCalculatePermissionForRejectWaitReturn(long workFlowID, int workFlowStateEventID);
        void ReCalculatePermissionForRejectWaitReturnComplete(long workFlowID, int workFlowStateEventID);
        //bool CanReceiveWaitReturn(long workFlowID);
    }
}

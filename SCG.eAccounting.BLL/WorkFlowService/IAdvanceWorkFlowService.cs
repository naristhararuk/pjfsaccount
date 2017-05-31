using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.SU.DTO;

namespace SCG.eAccounting.BLL.WorkFlowService
{
    public interface IAdvanceWorkFlowService : IExpenseWorkFlowService
    {

        #region GetPermitUser
        //long GetPermitUserApproveWaitTA(long workFlowID);
        #endregion

        #region GetPermitRole
        IList<SuRole> GetPermitRoleVerify(long workFlowID);
        IList<SuRole> GetPermitRoleApproveVerify(long workFlowID);
        IList<SuRole> GetPermitRoleReceive(long workFlowID);
        IList<SuRole> GetPermitRoleCounterCashier(long workFlowID);
        #endregion

        #region GetAllowRole{EventName}{StateName}
        IList<SuRole> GetAllowRoleClearingOutstanding(long workFlowID);
        //long GetAllowUserApproveWaitTA(long workFlowID);
        #endregion


        
        //bool CanApproveFromTADraft(long workFlowID);
        bool CanApproveWaitTA(long workFlowID);
        bool CanWithdrawWaitTA(long workFlowID);

        bool CanClearingOutstanding(long workFlowID);




        //string OnApproveFromTADraft(long workFlowID, object eventData);
        string OnApproveWaitTA(long workFlowID, object eventData);
        string OnWithdrawWaitTA(long workFlowID, object eventData);
        string OnClearingOutstanding(long workFlowID, object eventData);
        string OnApproveWaitTAInitial(long workFlowID, object eventData);
        string OnApproveWaitTAApprove(long workFlowID, object eventData);
        string OnRejectWaitTAInitial(long workFlowID, object eventData);
        string OnRejectWaitTAApprove(long workFlowID, object eventData);
        string OnWithdrawWaitTAInitial(long workFlowID, object eventData);
        string OnWithdrawWaitTAApprove(long workFlowID, object eventData);
        string OnOverRoleWaitTAInitial(long workFlowID, object eventData);
        

        void OnEnterOutstanding(long workFlowID);
        void OnExitOutstanding(long workFlowID);

        IList<object> GetVisibleFieldsForOutstanding(long workFlowID);
        //IList<object> GetVisibleFieldsForComplete(long workFlowID);
        IList<object> GetVisibleFieldsForWaitTA(long workFlowID);
        IList<object> GetVisibleFieldsForWaitTAApprove(long workFlowID);
        IList<object> GetVisibleFieldsForWaitTAInitial(long workFlowID);
    }
}

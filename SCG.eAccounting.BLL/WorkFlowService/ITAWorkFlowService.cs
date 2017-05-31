using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.BLL.WorkFlowService
{
    public interface ITAWorkFlowService : IGeneralWorkFlowService
    {

        #region CanEdit{StateName}
        bool CanEditDraft(long workFlowID);
        bool CanCancelComplete(long workFlowID);
        #endregion

        string OnCancelComplete(long workFlowID, object eventData);
        void ReCalculatePermissionForCancelComplete(long workFlowID, int workFlowStateEventID);

        IList<object> GetEditableFieldsForDraft(long workFlowID);
        
        IList<object> GetVisibleFieldsForDraft(long workFlowID);
        IList<object> GetVisibleFieldsForWaitInitial(long workFlowID);
        IList<object> GetVisibleFieldsForWaitApprove(long workFlowID);
        IList<object> GetVisibleFieldsForComplete(long workFlowID);
        IList<object> GetVisibleFieldsForCancel(long workFlowID);
    }
}

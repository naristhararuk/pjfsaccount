using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SS.Standard.WorkFlow.DTO.ValueObject;
using SS.Standard.WorkFlow.DTO;

namespace SS.Standard.WorkFlow.Service
{
    public interface IWorkFlowService : IService<WorkFlow.DTO.WorkFlow, long>
    {
        bool CanCopyDocument(long workFlowID);
        bool CanEditDocument(long workFlowID);
        bool CanDeleteDocument(long workFlowID);
        void OnDeleteDocument(long workFlowID);
        bool CanPrintPayInDocument(long workFlowID);
        IList<AuthorizedEvent> RetrieveAuthorizedEvents(long workFlowID , short languageID);
        void NotifyEvent(long workFlowID, string eventName, object eventData);
        void UpdateState(long workFlowID, WorkFlow.DTO.WorkFlowState newState);
        long CheckExistAndAddNew(SS.Standard.WorkFlow.DTO.WorkFlow domain);
        IList<object> GetVisibleFields(long workFlowID);
        IList<object> GetEditableFields(long workFlowID);

        void ReCalculateWorkFlowPermission(long workFlowID);
        void ReCalculateWorkFlowPermission(short roleID);
        void ReCalculateWorkFlowPermission();

        void NotifyEventFromToken(string tokenCode, long userID, int workFlowStateEventID, TokenType tokenType);
        void NotifyEventFromSMSToken(string tokenCode, long userID, int workFlowStateEventID, TokenType tokenType);
        void NotifyEventFromInternal(long workFlowID, string eventName, object eventData);
        
        
    }
}

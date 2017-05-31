using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Query;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.DTO.ValueObject;

using NHibernate;
using SCG.DB.DTO.ValueObject;

namespace SS.Standard.WorkFlow.Query
{
    public interface IWorkFlowStateEventQuery : IQuery<WorkFlowStateEvent, int> 
    {
        IList<WorkFlowStateEvent> FindByWorkFlowStateEventByStateID(int workFlowStateID);
        IList<WorkFlowStateEventWithLang> FindByWorkFlowStateEventByStateID(int workFlowStateID, short languageID);
        WorkFlowStateEvent GetByWorkFlowStateID_EventName(int workFlowStateID, string eventName);
        WorkFlowStateEvent GetSendDraftStateEvent(int workFlowTypeID);
        IList<VORejectReasonLang> FindRejectEventAndReason(short languageId, int documentTypeID);
        string GetTranslatedEventName(int workFlowStateEventID, short languageID);

        IList<WorkFlowStateEvent> FindWorkFlowStateEvent(string WorkFlowStateName, string WorkFlowStateEventName, int WorkFlowTypeID);
    }
}

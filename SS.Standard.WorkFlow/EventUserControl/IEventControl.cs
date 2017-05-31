using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Standard.WorkFlow.EventUserControl
{
    public interface IEventControl
    {
        void Initialize(long workFlowID);
        object GetEventData(int workFlowStateEventID);
        int WorkFlowStateEventID {get;set;}
    }
}

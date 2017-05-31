using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Query;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.DTO.ValueObject;

using NHibernate;

namespace SS.Standard.WorkFlow.Query
{
    public interface IWorkFlowStateQuery : IQuery<WorkFlowState, int> 
    {
        WorkFlowState FindWorkFlowStateIDByTypeIDAndStateName(int workFlowTypeID, string Name);
        WorkFlowState GetCurrentWorkFlowStateByWorkFolwID(long workFlowID);
    }
}

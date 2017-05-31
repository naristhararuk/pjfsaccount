using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SS.Standard.WorkFlow.DTO.ValueObject;

namespace SS.Standard.WorkFlow.Service
{
    public interface IWorkFlowStateService : IService<WorkFlow.DTO.WorkFlowState, int>
    {
        
    }
}

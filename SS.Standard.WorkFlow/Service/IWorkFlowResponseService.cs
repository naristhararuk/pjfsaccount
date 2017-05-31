using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SS.Standard.WorkFlow.DTO.ValueObject;

namespace SS.Standard.WorkFlow.Service
{
    public interface IWorkFlowResponseService : IService<WorkFlow.DTO.WorkFlowResponse, long>
    {
        void ResetActiveResponse(long workflowID, int nextOrdinal);
    }
}

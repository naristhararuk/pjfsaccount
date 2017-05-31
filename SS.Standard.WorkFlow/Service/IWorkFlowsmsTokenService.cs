using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;


using System.Data;
using SS.Standard.WorkFlow.Query;
using SS.Standard.WorkFlow.DTO;

namespace SS.Standard.WorkFlow.Service
{
    public interface IWorkFlowsmsTokenService : IService<WorkFlowsmsToken, long>
    {
        long GetRunning();
        string GetSMSTokenCode(long workFlowID);
    }
}

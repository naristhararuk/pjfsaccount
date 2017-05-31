using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

namespace SS.Standard.WorkFlow.Service
{
    public interface IWorkFlowStateEventPermissionService : IService<DTO.WorkFlowStateEventPermission , long>
    {
        void ClearOldPermission(long workFlowID);

    }
}

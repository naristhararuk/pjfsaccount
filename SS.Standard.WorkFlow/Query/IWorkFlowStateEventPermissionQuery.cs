using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Query;

namespace SS.Standard.WorkFlow.Query
{
    public interface IWorkFlowStateEventPermissionQuery : IQuery<DTO.WorkFlowStateEventPermission , long>
    {
        IList<DTO.WorkFlowStateEventPermission> FindByWorkFlowID(long workFlowID);
        IList<DTO.WorkFlowStateEventPermission> FindByWorkFlowID_UserID(long workFlowID , long userID);
    }
}

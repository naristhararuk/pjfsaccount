using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;

using SS.Standard.WorkFlow.DTO;
using NHibernate;

namespace SS.Standard.WorkFlow.Query.Hibernate
{
    public class WorkFlowStateEventPermissionQuery : NHibernateQueryBase<WorkFlowStateEventPermission , long> , IWorkFlowStateEventPermissionQuery
    {
        #region IWorkFlowStateEventPermissionQuery Members

        public IList<WorkFlowStateEventPermission> FindByWorkFlowID(long workFlowID)
        {
            IQuery query = GetCurrentSession().CreateQuery("from WorkFlowStateEventPermission wf where wf.WorkFlow.WorkFlowID = :WorkFlowID and wf.Active = '1'");
            query.SetLockMode("wf", LockMode.None);

            return query.SetInt64("WorkFlowID", workFlowID)
                .List<WorkFlowStateEventPermission>();
        }

        public IList<WorkFlowStateEventPermission> FindByWorkFlowID_UserID(long workFlowID, long userID)
        {
            return GetCurrentSession().CreateQuery("from WorkFlowStateEventPermission where WorkFlowID = :WorkFlowID and UserID = :UserID and active = '1'")
                .SetInt64("WorkFlowID", workFlowID)
                .SetInt64("UserID", userID)
                .List<WorkFlowStateEventPermission>();
        }

        #endregion
    }
}

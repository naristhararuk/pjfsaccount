using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SS.Standard.WorkFlow.DAL;
using SS.Standard.WorkFlow.Query;

namespace SS.Standard.WorkFlow.Service.Implement
{
    public class WorkFlowStateEventPermissionService : ServiceBase<DTO.WorkFlowStateEventPermission , long> , IWorkFlowStateEventPermissionService
    {
        public override IDao<DTO.WorkFlowStateEventPermission, long> GetBaseDao()
        {
            return WorkFlowDaoProvider.WorkFlowStateEventPermissionDao;
        }

        #region IWorkFlowStateEventPermissionService Members

        public void ClearOldPermission(long workFlowID)
        {
            //IList<DTO.WorkFlowStateEventPermission> permissions = WorkFlowQueryProvider.WorkFlowStateEventPermissionQuery.FindByWorkFlowID(workFlowID);
            //foreach (DTO.WorkFlowStateEventPermission permission in permissions)
            //{
            //    //WorkFlowDaoProvider.WorkFlowStateEventPermissionDao.Delete(permission);
            //}
            WorkFlowDaoProvider.WorkFlowStateEventPermissionDao.DeleteWorkFlowStateEventPermission(workFlowID);
        }

        #endregion
    }
}

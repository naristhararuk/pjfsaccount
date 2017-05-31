using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using NHibernate;

namespace SS.Standard.WorkFlow.DAL.Hibernate
{
    public class WorkFlowStateEventPermissionDao : NHibernateDaoBase<DTO.WorkFlowStateEventPermission, long>, IWorkFlowStateEventPermissionDao
    {
        public void DeleteWorkFlowStateEventPermission(long workFlowID)
        {
            #region Old Code
            //GetCurrentSession()
            //   .Delete("from WorkFlowStateEventPermission where WorkFlowID = :WorkFlowID "
            //    , new object[] { workFlowID }
            //    , new NHibernate.Type.IType[] { NHibernateUtil.Int64 });
            #endregion

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(" delete from WorkFlowStateEventPermission where WorkFlowID = :WorkFlowID");
            query.SetInt64("WorkFlowID", workFlowID);
            query.AddScalar("Count", NHibernateUtil.Int32).UniqueResult();
        }
    }
}

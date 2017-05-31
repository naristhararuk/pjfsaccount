using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;

using SCG.DB.DTO;
using SCG.DB.DAL;
using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SCG.DB.DAL.Hibernate
{
    public partial class DbCostCenterDao : NHibernateDaoBase<DbCostCenter, long>, IDbCostCenterDao
    {
        public bool IsDuplicateCostCenterCode(DbCostCenter costCenter)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("select count(1) as CostCenterCount ");
            sqlBuilder.Append("from dbcostcenter ");
            sqlBuilder.Append("where costcentercode = :costcentercode ");
            sqlBuilder.Append("and costcenterid <> :costcenterid ");
            QueryParameterBuilder param = new QueryParameterBuilder();
            param.AddParameterData("costcentercode", typeof(string), costCenter.CostCenterCode);
            param.AddParameterData("costcenterid", typeof(long), costCenter.CostCenterID);
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            param.FillParameters(query);
            query.AddScalar("CostCenterCount", NHibernateUtil.Int32);
            int i = (int)query.UniqueResult();
            if (i > 0)
            {
                return true;
            }
            return false;
        }

        public void SyncNewCostCenter()
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery("exec [dbo].[SyncNewCostCenterData]");
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }

        public void SyncUpdateCostCenter(string costCenterCode)
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery("exec [dbo].[SyncUpdateCostCenterData] :costCenterCode ");
            query.SetString("costCenterCode", costCenterCode);
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }

        public void SyncDeleteCostCenter(string costCenterCode)
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery("exec [dbo].[SyncDeleteCostCenterData] :costCenterCode ");
            query.SetString("costCenterCode", costCenterCode);
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }
    }
}

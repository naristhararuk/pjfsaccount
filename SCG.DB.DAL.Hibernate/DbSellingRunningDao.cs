using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SCG.DB.DTO;
using SCG.DB.DAL;

namespace SCG.DB.DAL.Hibernate
{
    public partial class DbSellingRunningDao : NHibernateDaoBase<DbSellingRunning, long>, IDbSellingRunningDao
    {
        public DbSellingRunningDao()
        { 
        
        }

        public void InsertData(DbSellingRunning sellingRunning) 
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("   INSERT INTO DbSellingRunning(CompanyID,Year,RunningNo,CreBy,CreDate,UpdBy,UpdDate)   ");
            sql.Append("    VALUES ( :companyID, :year,:runningNo ,:creBy , :creDate , :updBy , :updDate ) ");

            QueryParameterBuilder param = new QueryParameterBuilder();
            param.AddParameterData("companyID", typeof(long), sellingRunning.CompanyID);
            param.AddParameterData("year", typeof(int), sellingRunning.Year);
            param.AddParameterData("runningNo", typeof(long), sellingRunning.RunningNo);
            param.AddParameterData("creBy", typeof(DateTime), sellingRunning.CreBy);
            param.AddParameterData("creDate", typeof(DateTime), sellingRunning.CreDate);
            param.AddParameterData("updBy", typeof(long), sellingRunning.UpdBy);
            param.AddParameterData("updDate", typeof(DateTime), sellingRunning.UpdDate);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            param.FillParameters(query);
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }
    }
}
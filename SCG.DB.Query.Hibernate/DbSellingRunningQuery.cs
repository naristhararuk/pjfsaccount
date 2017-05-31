using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using SS.Standard.Data.NHibernate.QueryCreator;
using SS.Standard.Data.NHibernate.QueryDao;
using SCG.DB.DTO;
using NHibernate.Transform;

namespace SCG.DB.Query.Hibernate
{
    public class DbSellingRunningQuery : NHibernateQueryBase<DbSellingRunning, long>, IDbSellingRunningQuery
    {
        public DbSellingRunning GetSellingRunningByCompanyCode_Year(long companyID, int year)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(" SELECT b.RunningID , b.CompanyID , b.RunningNo , b.Year , b.CreBy , b.CreDate FROM DbSellingRunning b ");
            sql.Append(" where b.CompanyID = "+companyID+ " and b.YEAR = " + year);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            //QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            //parameterBuilder.AddParameterData("companyID", typeof(long), companyID);
            //parameterBuilder.AddParameterData("year", typeof(int), year);
            //parameterBuilder.FillParameters(query);

            query.AddScalar("RunningID", NHibernateUtil.Int64);
            query.AddScalar("CompanyID", NHibernateUtil.Int64);
            query.AddScalar("RunningNo", NHibernateUtil.Int64);
            query.AddScalar("Year", NHibernateUtil.Int32);
            query.AddScalar("CreBy", NHibernateUtil.Int64);
            query.AddScalar("CreDate", NHibernateUtil.DateTime);

            DbSellingRunning buyingRunning = query.SetResultTransformer(Transformers.AliasToBean(typeof(DbSellingRunning))).UniqueResult<DbSellingRunning>();

            return buyingRunning;
        }
    }
}

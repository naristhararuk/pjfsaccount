using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SCG.DB.DTO;
using SCG.DB.Query;
using SCG.DB.DTO.ValueObject;
using NHibernate.Expression;

namespace SCG.DB.Query.Hibernate
{
    public class DbBuyingRunningQuery : NHibernateQueryBase<DbBuyingRunning, long>, IDbBuyingRunningQuery
    {
        public DbBuyingRunning GetBuyingRunningByCompanyCode_Year(string companyCode, int year)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(" SELECT b.RunningID , c.CompanyID , b.RunningNo , b.Year , b.CreBy , b.CreDate FROM DbBuyingRunning b ");
            sql.Append(" INNER JOIN DbCompany c ");
            sql.Append(" ON c.CompanyCode = :companyCode and YEAR = :year ");
            sql.Append(" AND c.CompanyID = b.CompanyID ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("companyCode", typeof(string), companyCode);
            parameterBuilder.AddParameterData("year", typeof(int), year);
            parameterBuilder.FillParameters(query);

            query.AddScalar("RunningID", NHibernateUtil.Int64);
            query.AddScalar("CompanyID", NHibernateUtil.Int64);
            query.AddScalar("RunningNo", NHibernateUtil.Int64);
            query.AddScalar("Year", NHibernateUtil.Int32);
            query.AddScalar("CreBy", NHibernateUtil.Int64);
            query.AddScalar("CreDate", NHibernateUtil.DateTime);

            DbBuyingRunning buyingRunning =  query.SetResultTransformer(Transformers.AliasToBean(typeof(DbBuyingRunning))).UniqueResult<DbBuyingRunning>();
            
            return buyingRunning;
        }

    }
}

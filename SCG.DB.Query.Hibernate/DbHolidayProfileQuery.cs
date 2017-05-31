using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.QueryDao;
using SCG.DB.DTO;
using NHibernate;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SCG.DB.Query.Hibernate
{
    public class DbHolidayProfileQuery : NHibernateQueryBase<DbHolidayProfile, Int32>, IDbHolidayProfileQuery
    {
        public int CountHolidayProfile(int? year, string type)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbHolidayProfileQuery, "FindHolidayProfileByCriteria", new object[] { year, string.Empty, true, type });
        }
        public IList<DbHolidayProfile> GetHolidayProfile(int? year, int startRow, int pageSize, string sortExpression, string type)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<DbHolidayProfile>(ScgDbQueryProvider.DbHolidayProfileQuery, "FindHolidayProfileByCriteria", new object[] { year, sortExpression, false, type }, startRow, pageSize, sortExpression);
        }
        public ISQLQuery FindHolidayProfileByCriteria(int? year, string sortExpression, bool isCount, string type)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder paramBuilder = new QueryParameterBuilder();
            if (!isCount)
            {
                sqlBuilder.Append(" select Id,Year,IsApprove,Type,UpdBy, UpdDate, CreBy,CreDate,UpdPgm,RowVersion");
            }
            else
            {
                sqlBuilder.Append(" select count(*) as Count ");
            }

            sqlBuilder.Append("  from DbHolidayProfile where 1=1  ");

            if (year > 0)
            {
                sqlBuilder.Append(" and Year like :year ");
                paramBuilder.AddParameterData("year", typeof(Int32), year);
            }
            sqlBuilder.Append(" and Type like :type ");
            paramBuilder.AddParameterData("type", typeof(string), type);
            if (!string.IsNullOrEmpty(sortExpression))
            {

                sqlBuilder.Append(String.Format(" order by {0} ", sortExpression));
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            paramBuilder.FillParameters(query);
            if (isCount)
            {
                query.AddScalar("Count", NHibernateUtil.Int32);
            }
            else
            {
                query.AddScalar("Id", NHibernateUtil.Int32);
                query.AddScalar("Year", NHibernateUtil.Int32);
                query.AddScalar("IsApprove", NHibernateUtil.Boolean);
                query.AddScalar("Type", NHibernateUtil.String);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(DbHolidayProfile)));
            }
            return query;
        }
        public IList<Int32> GetYear(string type)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select Year ");
            sqlBuilder.Append(" from DbHolidayProfile WHERE Type = :type  Order By Year");
           ISQLQuery query =  GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
           query.SetString("type", type);
           query.AddScalar("Year", NHibernateUtil.Int32);
           IList<DbHolidayProfile> year = query.SetResultTransformer(Transformers.AliasToBean(typeof(DbHolidayProfile))).List<DbHolidayProfile>();
           return year.Select(t => t.Year).ToList();
        }
        
    }
}

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
    public class DbHolidayQuery : NHibernateQueryBase<DbHoliday, Int32>, IDbHolidayQuery
    {
        public int CountHoliday(int holidayProfileId)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbHolidayQuery, "FindHolidayByCriteria", new object[] { holidayProfileId, string.Empty, true });
        }
        public IList<DbHoliday> GetHoliday(int holidayProfileId, int startRow, int pageSize, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<DbHoliday>(ScgDbQueryProvider.DbHolidayQuery, "FindHolidayByCriteria", new object[] { holidayProfileId, sortExpression, false }, startRow, pageSize, sortExpression);
        }
        public ISQLQuery FindHolidayByCriteria(int holidayProfileId, string sortExpression, bool isCount)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder paramBuilder = new QueryParameterBuilder();
            if (!isCount)
            {
                sqlBuilder.Append(" select Id,HolidayProfileId,Date,Description,UpdBy, UpdDate, CreBy,CreDate,UpdPgm,RowVersion");
            }
            else
            {
                sqlBuilder.Append(" select count(*) as Count ");
            }

            sqlBuilder.Append("  from DbHoliday where 1=1  ");
            sqlBuilder.Append(" and HolidayProfileId = :holidayProfileId ");
            paramBuilder.AddParameterData("holidayProfileId", typeof(Int32), holidayProfileId);
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
                query.AddScalar("HolidayProfileId", NHibernateUtil.Int32);
                query.AddScalar("Date", NHibernateUtil.Date);
                query.AddScalar("Description", NHibernateUtil.String);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(DbHoliday)));
            }
            return query;
        }
    }
}

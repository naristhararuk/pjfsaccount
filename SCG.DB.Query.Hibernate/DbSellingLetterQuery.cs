using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;
using SCG.DB.DTO;


namespace SCG.DB.Query.Hibernate
{
    public class DbSellingLetterQuery : NHibernateQueryBase<DbSellingLetter, long>, IDbSellingLetterQuery
    {
        public long GetLetterIDByLetterNo(string letterNo) 
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(" SELECT TOP 1 l.LetterID FROM DbBuyingLetterDetail l ");
            sql.Append(" WHERE l.LetterNo = :letterNo ");
            sql.Append(" ORDER BY l.LetterID DESC ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("letterNo", typeof(string), letterNo);
            parameterBuilder.FillParameters(query);

            query.AddScalar("LetterID", NHibernateUtil.Int64);

            return query.UniqueResult<long>();
        }
    }
}

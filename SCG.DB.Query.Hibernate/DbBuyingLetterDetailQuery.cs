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
    public class DbBuyingLetterDetailQuery : NHibernateQueryBase<DbBuyingLetterDetail, long>, IDbBuyingLetterDetailQuery
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

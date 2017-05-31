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
    public class DbBuyingLetterQuery : NHibernateQueryBase<DbBuyingLetter, long>, IDbBuyingLetterQuery
    {
        public DbBuyingLetter FindLetterByDocumentID(long documentID)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(" SELECT LetterID,DocumentID FROM DbBuyingLetter where DocumentID = :documentID");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("documentID", typeof(Int64), documentID);
            parameterBuilder.FillParameters(query);

            query.AddScalar("LetterID", NHibernateUtil.Int64);
            query.AddScalar("DocumentID", NHibernateUtil.Int64);

            DbBuyingLetter buyingLetter = query.SetResultTransformer(Transformers.AliasToBean(typeof(DbBuyingLetter))).UniqueResult<DbBuyingLetter>();

            return buyingLetter;
        }

    }
}

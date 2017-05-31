using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;
using SS.Standard.Utilities;
using SS.Standard.Security;

using SS.DB.DTO;
using SS.DB.Query;
using SS.DB.DTO.ValueObject;

using NHibernate;
using NHibernate.Transform;

using SS.SU.DTO;

namespace SS.DB.Query.Hibernate
{
    public class DbRegionLangQuery : NHibernateQueryBase<DbRegionLang, long>, IDbRegionLangQuery
    {
        public IList<SS.SU.DTO.TranslatedListItem> FindRegionByLangCriteria(short languageId)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT ");
            sqlBuilder.Append("     DbRegionLang.RegionID       AS Id   ,");
            sqlBuilder.Append("     DbRegionLang.RegionName     AS Text  ");
            sqlBuilder.Append(" FROM ");
            sqlBuilder.Append("     DbRegionLang ");
            sqlBuilder.Append(" WHERE ");
            sqlBuilder.Append("     DbRegionLang.LanguageID   = :LanguageId ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("LanguageId", typeof(Int16), languageId);
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("Id", NHibernateUtil.Int16);
            query.AddScalar("Text", NHibernateUtil.String);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(SS.SU.DTO.TranslatedListItem)));

            return query.List<SS.SU.DTO.TranslatedListItem>();
        }
    }
}

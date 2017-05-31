using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;

using SS.DB.DTO;
using SS.DB.Query;
using SS.Standard.Data.NHibernate.QueryCreator;
using NHibernate;
using NHibernate.Transform;
using SS.DB.DTO.ValueObject;

namespace SS.DB.Query.Hibernate
{
    public class DbStatusLangQuery : NHibernateQueryBase<DbStatusLang, short>, IDbStatusLangQuery
    {

        #region IDbStatusLangQuery Members

        public IList<StatusLang> FindStatusLangByStatusId(short statusId)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select l.LanguageId , l.LanguageName , sl.StatusId,");
            sqlBuilder.Append(" sl.StatusDesc , sl.Comment as Comment, sl.Active as Active");
            sqlBuilder.Append(" from DbLanguage l left join DbStatusLang sl on sl.LanguageId = l.LanguageId");
            sqlBuilder.Append(" and sl.StatusId = :StatusId");
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("StatusId", typeof(short), statusId);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);
            query.AddScalar("LanguageId", NHibernateUtil.Int16)
                .AddScalar("LanguageName", NHibernateUtil.String)
                .AddScalar("StatusID", NHibernateUtil.Int16)
                .AddScalar("StatusDesc", NHibernateUtil.String)
                .AddScalar("Comment", NHibernateUtil.String)
                .AddScalar("Active", NHibernateUtil.Boolean);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(DB.DTO.ValueObject.StatusLang))).List<StatusLang>();
        }

        #endregion
    }
}

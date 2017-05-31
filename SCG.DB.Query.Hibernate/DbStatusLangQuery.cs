using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.QueryDao;
using SCG.DB.DTO;
using NHibernate;
using SS.Standard.Data.NHibernate.QueryCreator;
using NHibernate.Transform;
using SCG.DB.DTO.ValueObject;
using SS.DB.DTO.ValueObject;

namespace SCG.DB.Query.Hibernate
{
    public class SCGDbStatusLangQuery : NHibernateQueryBase<SCGDbStatusLang, long>, IDbStatusLangQuery
    {
        public IList<TranslatedListItem> FindStatusLangCriteria(string groupStatus, short languageId)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT ");
            sqlBuilder.Append("     DbStatus.Status         AS strID, ");
            sqlBuilder.Append("     DbStatusLang.StatusDesc AS strSymbol ");
            sqlBuilder.Append(" FROM DbStatus ");
            sqlBuilder.Append("     INNER JOIN DbStatusLang ON ");
            sqlBuilder.Append("     DbStatus.StatusID = DbStatusLang.StatusID ");
            sqlBuilder.Append(" WHERE ");
            sqlBuilder.Append("     DbStatus.GroupStatus      = :GroupStatus AND ");
            sqlBuilder.Append("     DbStatusLang.LanguageID   = :LanguageId AND ");
            sqlBuilder.Append("     DbStatus.Active = 1 ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("GroupStatus", typeof(string), groupStatus);
            queryParameterBuilder.AddParameterData("LanguageId", typeof(Int16), languageId);
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("strID", NHibernateUtil.String);
            query.AddScalar("strSymbol", NHibernateUtil.String);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(TranslatedListItem)));

            return query.List<TranslatedListItem>();
        }

        public string GetStatusLang(string groupStatus, int languageID, string status)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT DbStatusLang.StatusDesc as StatusDesc ");
            sqlBuilder.Append(" FROM DbStatusLang INNER JOIN ");
            sqlBuilder.Append(" DbStatus ON DbStatusLang.StatusID = DbStatus.StatusID ");
            sqlBuilder.Append(" WHERE DbStatus.Status = :Status and ");
            sqlBuilder.Append(" DbStatusLang.LanguageID = :LanguageID and ");
            sqlBuilder.Append(" DbStatus.GroupStatus = :GroupStatus ");

            return GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString())
                 .AddScalar("StatusDesc", NHibernateUtil.String)
                 .SetString("Status", status)
                 .SetInt32("LanguageID", languageID)
                 .SetString("GroupStatus", groupStatus)
                 .UniqueResult<string>();
        }
    }
}

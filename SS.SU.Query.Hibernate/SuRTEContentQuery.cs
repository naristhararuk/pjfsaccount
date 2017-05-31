using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.SU.Query;

using NHibernate;
using NHibernate.Transform;
using NHibernate.Expression;

using SS.Standard.Security;

namespace SS.SU.Query.Hibernate
{
    public class SuRTEContentQuery : NHibernateQueryBase<SuRTEContent, short>, ISuRTEContentQuery
    {
        public IList<SuRTEContentSearchResult> FindSuRTEContentByNodeId(short nodeId)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine(" select l.LanguageId as LanguageId, l.LanguageName as LanguageName, ");
            sqlBuilder.AppendLine(" c.Id as contentId, c.Header as Header, c.Content as Content, c.Comment as Comment, c.Active as Active");
            sqlBuilder.AppendLine(" from DbLanguage l left join SuRTEContent c on c.LanguageId = l.LanguageId and c.NodeId = :NodeId");
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("NodeId", typeof(short), nodeId);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);
            query.AddScalar("LanguageId", NHibernateUtil.Int16)
                .AddScalar("LanguageName", NHibernateUtil.String)
                .AddScalar("ContentId", NHibernateUtil.Int16)
                .AddScalar("Header", NHibernateUtil.String)
                .AddScalar("Content", NHibernateUtil.String)
                .AddScalar("Comment", NHibernateUtil.String)
                .AddScalar("Active", NHibernateUtil.Boolean);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(SU.DTO.ValueObject.SuRTEContentSearchResult))).List<SuRTEContentSearchResult>();
        }

        public IList<SuRTEContentSearchResult> FindSuRTEContentByContentIdLanguageId(short contentId, short languageId)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine(" select c.LanguageId as LanguageId, c.Id as contentId, c.Header as Header, c.Content as Content, c.Comment as Comment, c.Active as Active");
            sqlBuilder.AppendLine(" from SuRTEContent c where c.Id = :ContentId and c.LanguageId = :LanguageId");
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("LanguageId", typeof(short), languageId);
            parameterBuilder.AddParameterData("ContentId", typeof(short), contentId);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);
            query.AddScalar("LanguageId", NHibernateUtil.Int16)
                .AddScalar("ContentId", NHibernateUtil.Int16)
                .AddScalar("Header", NHibernateUtil.String)
                .AddScalar("Content", NHibernateUtil.String)
                .AddScalar("Comment", NHibernateUtil.String)
                .AddScalar("Active", NHibernateUtil.Boolean);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(SU.DTO.ValueObject.SuRTEContentSearchResult))).List<SuRTEContentSearchResult>();
        }
    }
}

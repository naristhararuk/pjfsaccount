using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;

using SS.SU.DTO;
using SS.SU.Query;
using SS.Standard.Data.NHibernate.QueryCreator;
using NHibernate;
using NHibernate.Transform;
using SS.SU.DTO.ValueObject;

namespace SS.SU.Query.Hibernate
{
    public class SuGlobalTranslateLangQuery : NHibernateQueryBase<SuGlobalTranslateLang, long>, ISuGlobalTranslateLangQuery
    {
        public IList<SuGlobalTranslateLang> FindByTranslateId(long translateId)
        {
            IList<SuGlobalTranslateLang> list = GetCurrentSession().CreateQuery("from SuGlobalTranslateLang as gt where gt.Translate.Translateid = :TranslateId")
                .SetInt64("TranslateId", translateId).List<SuGlobalTranslateLang>();

            return list;
        }
        public IList<GlobalTranslateLang> FindTranslateLangByTranslateId(long translateId)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("select l.LanguageId as LanguageId, l.LanguageName as LanguageName, tl.TranslateId as TranslateId,");
            sqlBuilder.Append(" tl.Id as TranslateLangId, tl.TranslateWord as TranslateWord, tl.Comment as Comment, tl.Active as Active");
            sqlBuilder.Append(" from DbLanguage l left join SuGlobalTranslateLang tl on tl.LanguageId = l.LanguageId and tl.translateId = :TranslateId");
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("TranslateId", typeof(long), translateId);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);
            query.AddScalar("LanguageId", NHibernateUtil.Int16)
                .AddScalar("LanguageName", NHibernateUtil.String)
                .AddScalar("TranslateId", NHibernateUtil.Int64)
                .AddScalar("TranslateLangId", NHibernateUtil.Int16)
                .AddScalar("TranslateWord", NHibernateUtil.String)
                .AddScalar("Comment", NHibernateUtil.String)
                .AddScalar("Active", NHibernateUtil.Boolean);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(SU.DTO.ValueObject.GlobalTranslateLang))).List<GlobalTranslateLang>();
        }
    }
}

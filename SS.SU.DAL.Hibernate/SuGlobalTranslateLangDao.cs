using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DAL;
using NHibernate.Expression;
using SS.SU.DTO.ValueObject;
using NHibernate;
using SS.Standard.Data.NHibernate.QueryCreator;
using NHibernate.Transform;

namespace SS.SU.DAL.Hibernate
{
    public partial class SuGlobalTranslateLangDao : NHibernateDaoBase<SuGlobalTranslateLang, long>, ISuGlobalTranslateLangDao
    {
        public IList<SuGlobalTranslateLang> FindByTranslateId(long translateId)
        {
            IList<SuGlobalTranslateLang> list = GetCurrentSession().CreateQuery("from SuGlobalTranslateLang as gt where gt.Translate.TranslateId = :TranslateId")
                .SetInt64("TranslateId", translateId).List<SuGlobalTranslateLang>();

            return list;
        }
        public bool IsDuplicateLanguage(long translateId, short languageId)
        {
            IList<SuGlobalTranslateLang> list = GetCurrentSession().CreateQuery("from SuGlobalTranslateLang tl where tl.Translate.TranslateId = :TranslateId and tl.Language.LanguageId = : LanguageId")
                .SetInt64("TranslateId", translateId)
                .SetInt16("LanguageId", languageId)
                .List<SuGlobalTranslateLang>();

            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }
        public void DeleteByTranslatIdLanguageId(long translateId, short languageId)
        {
            GetCurrentSession()
                .Delete("from SuGlobalTranslateLang tl where tl.Translate.TranslateId = :TranslateId and tl.Language.Languageid = :LanguageId"
                , new object[] { translateId, languageId }
                , new NHibernate.Type.IType[] { NHibernateUtil.Int64, NHibernateUtil.Int16 });
        }
        //public IList<GlobalTranslateLang> FindTranslateLangByTranslateId(long translateId)
        //{
        //    StringBuilder sqlBuilder = new StringBuilder();
        //    sqlBuilder.Append("select l.LanguageId as LanguageId, l.LanguageName as LanguageName, tl.TranslateId as TranslateId,");
        //    sqlBuilder.Append(" tl.Id as TranslateLangId, tl.TranslateWord as TranslateWord, tl.Comment as Comment, tl.Active as Active");
        //    sqlBuilder.Append(" from SuLanguage l left join SuGlobalTranslateLang tl on tl.LanguageId = l.LanguageId");
        //    sqlBuilder.Append(" where tl.TranslateId = :TraslateId");
        //    QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
        //    parameterBuilder.AddParameterData("TranslateId", typeof(long), translateId);

        //    ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
        //    parameterBuilder.FillParameters(query);
        //    query.AddScalar("LanguageId", NHibernateUtil.Int16)
        //        .AddScalar("LanguageName", NHibernateUtil.String)
        //        .AddScalar("TranslateId", NHibernateUtil.Int64)
        //        .AddScalar("TranslateLangId", NHibernateUtil.Int16)
        //        .AddScalar("TranslateWord", NHibernateUtil.String)
        //        .AddScalar("Comment", NHibernateUtil.String)
        //        .AddScalar("Active", NHibernateUtil.Boolean);

        //    return query.SetResultTransformer(Transformers.AliasToBean(typeof(SU.DTO.ValueObject.GlobalTranslateLang))).List<GlobalTranslateLang>();
        //}
    }
}

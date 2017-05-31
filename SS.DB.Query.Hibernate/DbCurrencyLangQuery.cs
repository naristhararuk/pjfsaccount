using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SS.Standard.Data.NHibernate.QueryDao;

using SS.DB.DTO;
using SS.DB.DTO.ValueObject;
using SS.DB.Query;
using NHibernate;
using NHibernate.Transform;
using SS.Standard.Security;
using SS.Standard.Data.NHibernate.QueryCreator;
namespace SS.DB.Query.Hibernate
{
    public class DbCurrencyLangQuery : NHibernateQueryBase<DbCurrencyLang, long>, IDbCurrencyLangQuery
    {
        public IList<VOUCurrencySetup> FindCurrencyLangByCurrencyID(long cid)
        {
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append(" SELECT    DbLanguage.LanguageID as LanguageID , DbLanguage.LanguageName as LanguageName ,");
            sqlBuilder.Append(" DbCurrencyLang.CurrencyLangID as CurrencyLangID , DbCurrencyLang.LanguageID AS CLanguageID , ");
            sqlBuilder.Append(" DbCurrencyLang.CurrencyID as CurrencyID , DbCurrencyLang.Description as Description , ");
            sqlBuilder.Append(" DbCurrencyLang.Comment as Comment , DbCurrencyLang.Active as LangActive ,");
            sqlBuilder.Append(" DbCurrencyLang.MainUnit as MainUnit,DbCurrencyLang.SubUnit as SubUnit ");
            sqlBuilder.Append(" FROM  DbLanguage LEFT JOIN ");
            sqlBuilder.Append(" DbCurrencyLang ON DbLanguage.LanguageID = DbCurrencyLang.LanguageID and DbCurrencyLang.CurrencyID= :CID ");
            queryParameterBuilder.AddParameterData("CID", typeof(long), cid);
           

          

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("CurrencyID", NHibernateUtil.Int16)
                 .AddScalar("LangActive", NHibernateUtil.Boolean)
                 .AddScalar("Description", NHibernateUtil.String)
                 .AddScalar("LanguageID", NHibernateUtil.Int16)
                 .AddScalar("Comment", NHibernateUtil.String)
                 .AddScalar("CLanguageID", NHibernateUtil.Int16)
                 .AddScalar("CurrencyLangID", NHibernateUtil.Int64)
                 .AddScalar("LanguageName", NHibernateUtil.String)
                 .AddScalar("MainUnit", NHibernateUtil.String)
                 .AddScalar("SubUnit", NHibernateUtil.String);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(VOUCurrencySetup)));


            return query.SetResultTransformer(Transformers.AliasToBean(typeof(VOUCurrencySetup))).List<VOUCurrencySetup>(); ;

        }

        public IList<DbCurrencyLang> FindCurrencyLangByCID(short id)
        {
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append(" SELECT DbCurrencyLang.CurrencyLangID as CurrencyLangID  ");
            sqlBuilder.Append(" FROM  DbCurrencyLang WHERE DbCurrencyLang.CurrencyID = :id ");
            queryParameterBuilder.AddParameterData("id", typeof(short), id);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("CurrencyLangID", NHibernateUtil.Int64);


            query.SetResultTransformer(Transformers.AliasToBean(typeof(DbCurrencyLang)));


            return query.SetResultTransformer(Transformers.AliasToBean(typeof(DbCurrencyLang))).List<DbCurrencyLang>(); ;
        }
    }
}

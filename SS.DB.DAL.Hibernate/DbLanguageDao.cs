using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.Dao;

using SS.DB.DTO;
using SS.DB.DAL;
using NHibernate;
using NHibernate.Expression;
using SS.Standard.Data.NHibernate.QueryCreator;

using SS.DB.DTO.ValueObject;

namespace SS.DB.DAL.Hibernate
{
    public partial class DbLanguageDao : NHibernateDaoBase<DbLanguage, short>, IDbLanguageDao
    {
        public DbLanguageDao()
        {
        }
        public IList<Language> FindByDbLanguageCriteria(DbLanguage language)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Select  l.LanguageName ");
            sql.Append("from DbLanguage as l ");
            sql.Append("where l.LanguageName = :LanguageName ");
            
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("LanguageName", typeof(String), language.LanguageName);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("LanguageName", NHibernateUtil.String);;

            IList<SS.DB.DTO.ValueObject.Language> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(SS.DB.DTO.ValueObject.Language)))
                .List<SS.DB.DTO.ValueObject.Language>();

            return list;
        }
    }
}

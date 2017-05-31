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
using NHibernate.Expression;

namespace SS.DB.Query.Hibernate
{
    public class DbZoneLangQuery : NHibernateQueryBase<DbZoneLang, long>, IDbZoneLangQuery  
    {
        //public ICriteria FindZoneLangById(short Id)
        //{
        //    ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(DbZoneLang), "zl");
        //    criteria.Add(Expression.Eq("zl.Zone.ZoneID", Id));
        //    IList<DbZoneLang> dbZoneLangList = criteria.List<DbZoneLang>();
        //    foreach (DbZoneLang dbZoneLangRate in dbZoneLangList)
        //    {
        //        Console.WriteLine(dbZoneLangRate.Language);
        //        Console.WriteLine(dbZoneLangRate.Zone);
        //    }
        //    return criteria;
        //}
        //public IList<DbZoneLang> GetZoneLangList(short Id, int firstResult, int maxResult, string sortExpression)
        //{
        //    return NHibernateQueryHelper.FindPagingByCriteria<DbZoneLang>(SsDbQueryProvider.DbExchangeRateQuery, "FindZoneLangById", new object[] { Id }, firstResult, maxResult, sortExpression);
        //}
        //public int CountZoneLangByCriteria(short Id)
        //{
        //    return NHibernateQueryHelper.CountByCriteria(SsDbQueryProvider.DbExchangeRateQuery, "FindZoneLangById", new object[] { Id });
        //}

        public DbZoneResult FindByDbZoneLangKey(short zoneID, short languageId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select zl.ZoneID as ZoneID, zl.ZoneName as ZoneName, l.LanguageID as LanguageId, l.LanguageName as LanguageName, zl.Comment as Comment, zl.Active as Active ");
            sql.Append(" from DbZone as z  left join DbZonelang as zl on z.ZoneID = zl.ZoneID and zl.LanguageID = :LanguageID");
            sql.Append(" left join DbLanguage l on l.LanguageId = :LanguageID ");
            sql.Append(" where  zl.ZoneID = :ZoneID ");          

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("LanguageID", typeof(Int16), languageId);
            queryParameterBuilder.AddParameterData("ZoneID", typeof(short), zoneID);
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("ZoneID", NHibernateUtil.Int16);
            query.AddScalar("ZoneName", NHibernateUtil.String);                   
            query.AddScalar("LanguageID", NHibernateUtil.Int16);
            query.AddScalar("LanguageName", NHibernateUtil.String);        
            query.AddScalar("Comment", NHibernateUtil.String);
            query.AddScalar("Active", NHibernateUtil.Boolean);            

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(DbZoneResult))).List<DbZoneResult>().ElementAt<DbZoneResult>(0);
        }
    }
}

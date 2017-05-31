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

namespace SCG.DB.Query.Hibernate
{
    public class DbCountryLangQuery : NHibernateQueryBase<DbCountryLang, long>, IDbCountryLangQuery
    {
        #region ISQLQuery FindByDbCountryLangQuery(DbCountryLang countryLang, string countryId, short languageId)
        public ISQLQuery FindByDbCountryLangQuery(DbCountryLang countryLang, string countryCode, short languageId, bool isCount)
        {
            ISQLQuery query;
            if (!isCount)
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select cl.CountryID as CountryID,c.CountryCode as CountryCode,cl.CountryName as CountryName ,cl.LanguageID as LanguageId");
                sql.Append(" from DbCountry as c ");
                sql.Append("left join DbCountrylang as cl on cl.CountryID = c.CountryID and cl.LanguageID = :LanguageID ");
                sql.Append("where ");
                if (!countryCode.Trim().Equals(string.Empty))
                    sql.Append("cl.CountryCode = :CountryCode and ");
                sql.Append(" cl.CountryName like :CountryName");

                 query = GetCurrentSession().CreateSQLQuery(sql.ToString());
                QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
                queryParameterBuilder.AddParameterData("LanguageID", typeof(Int16), languageId);
                if (!countryCode.ToString().Trim().Equals(string.Empty))
                    queryParameterBuilder.AddParameterData("CountryCode", typeof(string), countryCode);
                queryParameterBuilder.AddParameterData("CountryName", typeof(string), "%"+countryLang.CountryName+"%");
                queryParameterBuilder.FillParameters(query);
                query.AddScalar("LanguageId", NHibernateUtil.Int16);
                query.AddScalar("CountryID", NHibernateUtil.Int16);
                query.AddScalar("CountryCode", NHibernateUtil.String);
                query.AddScalar("CountryName", NHibernateUtil.String);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(CountryLang)));
            }
            else
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select count(*) as count ");
                sql.Append("from DbCountry as c ");
                sql.Append("left join DbCountrylang as cl on cl.CountryID = c.CountryID and cl.LanguageId = :LanguageId ");
                sql.Append("where ");
                if (!countryCode.Trim().Equals(string.Empty))
                    sql.Append("cl.CountryCode = :CountryCode and ");
                sql.Append(" cl.CountryName = :CountryName"); ;

                query = GetCurrentSession().CreateSQLQuery(sql.ToString());
                QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
                queryParameterBuilder.AddParameterData("LanguageId", typeof(Int16), languageId);
                if (!countryCode.ToString().Trim().Equals(string.Empty))
                    queryParameterBuilder.AddParameterData("CountryCode", typeof(string), countryCode);
                queryParameterBuilder.AddParameterData("CountryName", typeof(string), countryLang.CountryName);
                queryParameterBuilder.FillParameters(query);
                query.AddScalar("count", NHibernateUtil.Int16);
            }
            return query;
        }
        public IList<CountryLang> FindByDbCountryLang(DbCountryLang criteria, string countryCode, short languageId, int firstResult, int maxResults, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<CountryLang>(ScgDbQueryProvider.DbCountryLangQuery, "FindByDbCountryLangQuery", new object[] { criteria, countryCode, languageId,false }, firstResult, maxResults, sortExpression);
        }

        public int CountByDbCountryLangCriteria(DbCountryLang criteria, string countryCode, short languageId)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbCountryLangQuery, "FindByDbCountryLangQuery", new object[] { criteria, countryCode, languageId, true});
        }
        public IList<CountryLang> FindAutoComplete(string CountryName, short countryId, short languageId)
        {
            CountryName = "%" + CountryName + "%";

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("select l.LanguageId as LanguageId, l.LanguageName as LanguageName, cl.CountryId as CountryId,c.CountryCode as CountryCode, ");
            sqlBuilder.Append(" cl.CountryName as CountryName, cl.Comment as Comment,cl.Active as Active");
            sqlBuilder.Append(" from DbCountry C  left join DbCountryLang cl  on cl.CountryId = c.CountryId and cl.LanguageId = :LanguageId ");
            sqlBuilder.Append(" left join DbLanguage l on l.LanguageId = :LanguageId ");
            sqlBuilder.Append(" where cl.CountryName Like :CountryName");
            //sqlBuilder.Append(" and c.CountryId = :CountryId");
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("LanguageId", typeof(short), languageId);
            parameterBuilder.AddParameterData("CountryName", typeof(string), CountryName);
            //parameterBuilder.AddParameterData("CountryId", typeof(short), countryId);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);
            query.AddScalar("LanguageId", NHibernateUtil.Int16)
                .AddScalar("LanguageName", NHibernateUtil.String)
                .AddScalar("CountryID", NHibernateUtil.Int16)
                .AddScalar("CountryCode", NHibernateUtil.String)
                .AddScalar("CountryName", NHibernateUtil.String)
                .AddScalar("Comment", NHibernateUtil.String)
                .AddScalar("Active", NHibernateUtil.Boolean);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(CountryLang))).List<CountryLang>();
        }
        #endregion

        #region DbCountryLang FindByDbCountryLangKey(string countryCode, short languageId)
        public CountryLang FindByDbCountryLangKey(short countryID, short languageId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select cl.CountryID as CountryID,c.CountryCode as CountryCode,cl.CountryName as CountryName ,cl.LanguageID as LanguageId");
            sql.Append(" from DbCountry as c ");
            sql.Append("left join DbCountrylang as cl on cl.CountryID = c.CountryID and cl.LanguageID = :LanguageID ");
            sql.Append("where ");
            sql.Append("cl.CountryID = :CountryID ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("LanguageID", typeof(Int16), languageId);
            queryParameterBuilder.AddParameterData("CountryID", typeof(string), countryID);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("LanguageId", NHibernateUtil.Int16);
            query.AddScalar("CountryID", NHibernateUtil.Int16);
            query.AddScalar("CountryCode", NHibernateUtil.String);
            query.AddScalar("CountryName", NHibernateUtil.String);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(CountryLang))).List<CountryLang>().ElementAt<CountryLang>(0);
        }
        #endregion

        public IList<CountryLang> GetAllCountryLangByLang(short languageId, long requesterId)
        {           
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("select cl.CountryId as CountryId,c.CountryCode as CountryCode, ");
            sqlBuilder.Append(" cl.CountryName as CountryName, cl.Comment as Comment,cl.Active as Active");
            sqlBuilder.Append(" from DbCountry c  left join DbCountryLang cl  on c.CountryId = cl.CountryId and cl.LanguageId = :LanguageId ");
            sqlBuilder.Append(" inner join FnPerdiemProfileCountry prCountry on prCountry.CountryID = c.CountryID ");
            sqlBuilder.Append(" inner join FnPerdiemProfile pf on pf.PerdiemProfileID = prCountry.PerdiemProfileID ");
            sqlBuilder.Append(" inner join FnPerdiemProfileCompany Pc on Pf.PerdiemProfileID = Pc.PerdiemProfileID ");
            sqlBuilder.Append(" inner join SuUser u on u.CompanyID = Pc.CompanyID ");
            sqlBuilder.Append(" where  c.Active = 1 and u.UserID = :RequesterID  ");
            sqlBuilder.Append(" order by  prCountry.ZoneID desc, cl.CountryName asc ");

            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("LanguageId", typeof(short), languageId);
            parameterBuilder.AddParameterData("RequesterID", typeof(long), requesterId);
       
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);          
           
            //query.AddScalar("LanguageId", NHibernateUtil.Int16);
            //query.AddScalar("LanguageName", NHibernateUtil.String);
            query.AddScalar("CountryID", NHibernateUtil.Int16);
            query.AddScalar("CountryCode", NHibernateUtil.String);
            query.AddScalar("CountryName", NHibernateUtil.String);
            query.AddScalar("Comment", NHibernateUtil.String);
            query.AddScalar("Active", NHibernateUtil.Boolean);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(CountryLang))).List<CountryLang>();
        }
        public IList<CountryLang> FindCountryLangByCountryID(short cID)
        {
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append(" SELECT    DbLanguage.LanguageID as LanguageId , DbLanguage.LanguageName as LanguageName ,");
            sqlBuilder.Append(" DbCountryLang.ID as ID , DbCountryLang.LanguageID AS LanguageId , ");
            sqlBuilder.Append(" DbCountryLang.CountryID as CountryID  ,DbCountryLang.CountryName as CountryName, DbCountryLang.Comment as Comment , ");
            sqlBuilder.Append(" DbCountryLang.Active as Active ");
            sqlBuilder.Append(" FROM  DbLanguage LEFT JOIN  ");
            sqlBuilder.Append(" DbCountryLang ON DbLanguage.LanguageID = DbCountryLang.LanguageID and DbCountryLang.CountryID= :CID ");
            queryParameterBuilder.AddParameterData("CID", typeof(short), cID);




            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("LanguageId", NHibernateUtil.Int16)
                 .AddScalar("LanguageName", NHibernateUtil.String)
                 .AddScalar("ID", NHibernateUtil.Int16)
                 .AddScalar("LanguageId", NHibernateUtil.Int16)
                 .AddScalar("CountryID", NHibernateUtil.Int16)
                 .AddScalar("CountryName",NHibernateUtil.String)
                 .AddScalar("Comment", NHibernateUtil.String)
                 .AddScalar("Active", NHibernateUtil.Boolean);

            query.SetResultTransformer(Transformers.AliasToBean(typeof(VOPB)));


            return query.SetResultTransformer(Transformers.AliasToBean(typeof(CountryLang))).List<CountryLang>(); ;

        }
    }
}

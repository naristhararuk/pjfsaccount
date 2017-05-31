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
    public class DbCountryQuery : NHibernateQueryBase<DbCountry, short>, IDbCountryQuery
    {
        #region public ISQLQuery FindByCountryCriteria(DbCountry country, bool isCount, short languageId, string sortExpression)
        public ISQLQuery FindByCountryCriteria(DbCountry country, bool isCount, short languageId, string sortExpression)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {
                sqlBuilder.Append(" SELECT ");
                sqlBuilder.Append("     DbCountry.CountryID           AS CountryID       ,");
                sqlBuilder.Append("     DbCountry.CountryCode           AS CountryCode       ,");
                sqlBuilder.Append("     DbCountry.Comment          AS Comment     ,");
                sqlBuilder.Append("     DbCountry.Active           AS Active      ,");
                sqlBuilder.Append("     DbCountryLang.CountryName     AS CountryName    ");
                sqlBuilder.Append(" FROM DbCountry ");
                sqlBuilder.Append("     LEFT JOIN DbCountryLang ON ");
                sqlBuilder.Append("         DbCountry.CountryID           =  DbCountryLang.CountryID AND ");
                sqlBuilder.Append("         DbCountryLang.LanguageID   = :LanguageId ");
                sqlBuilder.Append(" WHERE DbCountry.CountryCode like :CountryCode ");
                sqlBuilder.Append(" AND DbCountry.Comment like :Comment ");
                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine("ORDER BY DbCountry.CountryCode,DbCountryLang.CountryName,DbCountry.Comment,DbCountry.Active");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));

            }
            else
            {
                sqlBuilder.Append(" SELECT COUNT(CountryID) AS CountryCount FROM DbCountry ");
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            if (!isCount)
            {
                QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
                queryParameterBuilder.AddParameterData("LanguageId", typeof(Int16), languageId);
                queryParameterBuilder.AddParameterData("CountryCode", typeof(string), string.Format("%{0}%", country.CountryCode));
                queryParameterBuilder.AddParameterData("Comment", typeof(string), string.Format("%{0}%", country.Comment));
                queryParameterBuilder.FillParameters(query);
                query.AddScalar("CountryID", NHibernateUtil.Int16);
                query.AddScalar("CountryCode", NHibernateUtil.String);
                query.AddScalar("Comment", NHibernateUtil.String);
                query.AddScalar("Active", NHibernateUtil.Boolean);
                query.AddScalar("CountryName", NHibernateUtil.String);
                //query.AddScalar("ProgramName", NHibernateUtil.String);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(CountryLang)));
            }
            else
            {
                query.AddScalar("CountryCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            return query;
        }
        #endregion public ISQLQuery FindByBankCriteria(CountryLang country, bool isCount, short languageId, string sortExpression)

        #region public IList<CountryLang> GetBankList(CountryLang country , short languageId, int firstResult, int maxResult, string sortExpression)
        public IList<CountryLang> GetCountryList(DbCountry country, short languageId, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<CountryLang>(ScgDbQueryProvider.DbCountryQuery, "FindByCountryCriteria", new object[] { country, false, languageId, sortExpression }, firstResult, maxResult, sortExpression);
        }
        #endregion public IList<CountryLang> GetCountryList(CountryLang country , short languageId, int firstResult, int maxResult, string sortExpression)

        #region public int CountByBankCriteria(CountryLang country)
        public int CountByCountryCriteria(DbCountry country)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbCountryQuery, "FindByCountryCriteria", new object[] { country, true, Convert.ToInt16(0), string.Empty });
        }
        #endregion public int CountByCountryCriteria(CountryLang country)

        public IList<CountryLang> FindCountry(short languageId)
        {
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append(" SELECT ");
            sqlBuilder.Append("     DbCountry.CountryID           AS CountryID       ,");
            sqlBuilder.Append("     DbCountry.CountryCode           AS CountryCode       ,");
            sqlBuilder.Append("     DbCountryLang.CountryName     AS CountryName    ");
            sqlBuilder.Append(" FROM DbCountry ");
            sqlBuilder.Append("     LEFT JOIN DbCountryLang ON ");
            sqlBuilder.Append("         DbCountry.CountryID           =  DbCountryLang.CountryID AND ");
            sqlBuilder.Append("         DbCountryLang.LanguageID   = :LanguageId ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("LanguageId", typeof(Int16), languageId);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("CountryID", NHibernateUtil.Int16);
            query.AddScalar("CountryCode", NHibernateUtil.String);
            query.AddScalar("CountryName", NHibernateUtil.String);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(CountryLang)));
            return query.List<CountryLang>();
        }
    }
}

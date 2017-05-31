using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;

using SS.DB.DTO;
using SS.DB.Query;
using NHibernate;
using NHibernate.Transform;
using SS.Standard.Security;
using SS.SU.Helper;
using SS.SU.DTO;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SS.DB.Query.Hibernate
{
    public class DbLanguageQuery : NHibernateQueryBase<DbLanguage, short>, IDbLanguageQuery
    {
        #region IDbLanguageQuery Members
        public IList<DbLanguageSearchResult> GetTranslatedList()
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" SELECT LanguageId,LanguageName,LanguageCode,ImagePath,Comment,Active ");
            strQuery.AppendLine(" FROM DbLanguage ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());

            //query.SetInt16("languageID", languageID);
            query.AddScalar("Languageid", NHibernateUtil.Int16);
            query.AddScalar("LanguageName", NHibernateUtil.String);
            query.AddScalar("LanguageCode", NHibernateUtil.String);
            query.AddScalar("ImagePath", NHibernateUtil.String);
            query.AddScalar("Comment", NHibernateUtil.String);
            query.AddScalar("Active", NHibernateUtil.Boolean);

            IList<DbLanguageSearchResult> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(DbLanguageSearchResult))).List<DbLanguageSearchResult>();

            return list;


        }

        #endregion

        #region ISuUserQuery Members


        public IList<SuUserGridView> FindUserAll()
        {
            throw new NotImplementedException();
        }

        public DbLanguage FindLanguageByIdentity(short lid)
        {
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append(" SELECT     LanguageID, LanguageName ");
            sqlBuilder.Append(" FROM         DbLanguage ");

            StringBuilder whereClauseBuilder = new StringBuilder();
            //whereClauseBuilder.Append("  and db.Active = 1 ");

            if (lid != 0)
            {
                whereClauseBuilder.Append(" and LanguageID= :LID ");
                queryParameterBuilder.AddParameterData("LID", typeof(short), lid);
            }

            sqlBuilder.Append(String.Format(" where 1=1 {0} ", whereClauseBuilder.ToString()));

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("Languageid", NHibernateUtil.Int16)
                   .AddScalar("LanguageName", NHibernateUtil.String);
                  


            query.SetResultTransformer(Transformers.AliasToBean(typeof(DbLanguage)));


            IList<DbLanguage> currencyList = query.SetResultTransformer(Transformers.AliasToBean(typeof(DbLanguage))).List<DbLanguage>(); ;

            if (currencyList.Count > 0)
            {
                return currencyList[0];
            }
            else
            {
                return null;
            }


        }
        public List<DbLanguage> FindAllList()
        {
            IList<DbLanguage> language;
            if (System.Web.HttpContext.Current.Application[ApplicationEnum.WebApplication.DataLanguage.ToString()] == null)
            {
                ISQLQuery query = GetCurrentSession().CreateSQLQuery("SELECT * FROM DbLanguage");
                query.AddEntity(typeof(DbLanguage));
                language = query.List<DbLanguage>();
                System.Web.HttpContext.Current.Application[ApplicationEnum.WebApplication.DataLanguage.ToString()] = language;
            }
            else
            {
                language = System.Web.HttpContext.Current.Application[ApplicationEnum.WebApplication.DataLanguage.ToString()] as IList<DbLanguage>;
            }
            return language.ToList<DbLanguage>();
        }

        public IList<DbLanguageSearchResult> GetLanguageList(short languageID, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<DbLanguageSearchResult>(SsDbQueryProvider.DbLanguageQuery, "FindDbLanguageSearchResult", new object[] { languageID, sortExpression, false }, firstResult, maxResult, sortExpression);
        }

        public int GetCountLanguageList(short languageID)
        {
            return NHibernateQueryHelper.CountByCriteria(SsDbQueryProvider.DbLanguageQuery, "FindDbLanguageSearchResult", new object[] { languageID, string.Empty, true });
        }


        public ISQLQuery FindDbLanguageSearchResult(short languageID , string sortExpression , bool isCount)
        {
            StringBuilder strQuery = new StringBuilder();
            ISQLQuery query;
            if (isCount)
            {
                strQuery.AppendLine(" SELECT count(*) as Count");
                strQuery.AppendLine(" FROM DbLanguage ");

                query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
                query.AddScalar("Count", NHibernateUtil.Int32);
            }
            else
            {
                strQuery.AppendLine(" SELECT LanguageId,LanguageName,LanguageCode,ImagePath,Comment,Active");
                strQuery.AppendLine(" FROM DbLanguage ");

                if (string.IsNullOrEmpty(sortExpression))
                {
                    strQuery.AppendLine(" ORDER BY LanguageId,LanguageName,LanguageCode,ImagePath,Comment,Active ");
                }
                else
                {
                    strQuery.AppendLine(string.Format(" ORDER BY {0}", sortExpression));
                }

                query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
                query.AddScalar("Languageid", NHibernateUtil.Int16);
                query.AddScalar("LanguageName", NHibernateUtil.String);
                query.AddScalar("LanguageCode", NHibernateUtil.String);
                query.AddScalar("ImagePath", NHibernateUtil.String);
                query.AddScalar("Comment", NHibernateUtil.String);
                query.AddScalar("Active", NHibernateUtil.Boolean);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(DbLanguageSearchResult)));
            }
            
            return query;
        }
        public int FindCountDbLanguageSearchResult(DbLanguageSearchResult languageSearchResult, short languageID)
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" SELECT Count(*) as Count ");
            strQuery.AppendLine(" FROM DbLanguage ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
            query.AddScalar("Count", NHibernateUtil.Int32);

            return Convert.ToInt32(query.UniqueResult());
        }
        #endregion

        public List<DbLanguage> FindAllListLanguage()
        {
            IList<DbLanguage> list = GetCurrentSession().CreateQuery("from DbLanguage lang where lang.Active = 1 ")
                  .List<DbLanguage>();
           
           return list.ToList<DbLanguage>();
            
        }
    }
}

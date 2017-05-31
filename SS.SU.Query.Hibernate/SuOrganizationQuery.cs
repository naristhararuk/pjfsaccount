using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.Query;

using SS.SU.DTO;
using SS.SU.Query;

namespace SS.SU.Query.Hibernate
{
    public class SuOrganizationQuery : NHibernateQueryBase<SuOrganization, short>, ISuOrganizationQuery
    {

        #region ISuOrganizationQuery Members (Old)
        //public IList<SuOrganization_SuOrganizationLang> FindByLanguageId(int languageId)
        //{
        //    StringBuilder strQuery = new StringBuilder();
        //    strQuery.AppendLine(" SELECT org.OrganizationID,orgLang.OrganizationName FROM SuOrganization org ");
        //    strQuery.AppendLine(" INNER JOIN SuOrganizationLang orgLang ");
        //    strQuery.AppendLine(" ON org.OrganizationID = orgLang.OrganizationID ");
        //    strQuery.AppendLine(" WHERE orgLang.LanguageID = :LanguageID ");
        //    strQuery.AppendLine(" ORDER BY orgLang.OrganizationName ");

        //    return GetCurrentSession().CreateSQLQuery(strQuery.ToString())
        //            .AddEntity(typeof(SuOrganization_SuOrganizationLang))
        //            .SetInt32("LanguageID", languageId)
        //            .List<SuOrganization_SuOrganizationLang>();
        //}
        #endregion
        
        #region ISuOrganizationQuery Members
        public IList<TranslatedListItem> GetTranslatedList(short languageID)
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" SELECT org.OrganizationID as Id , orgLang.OrganizationName as Text FROM SuOrganization org ");
            strQuery.AppendLine(" INNER JOIN SuOrganizationLang orgLang ");
            strQuery.AppendLine(" ON org.OrganizationID = orgLang.OrganizationID ");
            strQuery.AppendLine(" WHERE orgLang.LanguageID = :LanguageID ");
            strQuery.AppendLine(" ORDER BY orgLang.OrganizationName ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
            query.SetInt16("LanguageID", languageID==short.Parse("0")?short.Parse("1"):languageID);
            query.AddScalar("Id", NHibernateUtil.Int16);
            query.AddScalar("Text", NHibernateUtil.String);
            IList<TranslatedListItem> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(TranslatedListItem))).List<TranslatedListItem>();
            return list;
        }
		
		public IList<SuOrganizationSearchResult> GetOrganizationList(short languageID, int firstResult, int maxResult, string sortExpression)
		{
			return NHibernateQueryHelper.FindPagingByCriteria<SuOrganizationSearchResult>(QueryProvider.SuOrganizationQuery, "FindSuOrganizationSearchResult", new object[] { languageID, sortExpression, false }, firstResult, maxResult, sortExpression);
		}
		public int GetCountOrganizationList(short languageID)
		{
			return NHibernateQueryHelper.CountByCriteria(QueryProvider.SuOrganizationQuery, "FindSuOrganizationSearchResult", new object[] { languageID, string.Empty, true });
		}
		public ISQLQuery FindSuOrganizationSearchResult(short languageID, string sortExpression, bool isCount)
		{
			StringBuilder strQuery = new StringBuilder();
			ISQLQuery query;
			
			if (isCount)
			{
				strQuery.AppendLine(" SELECT Count(*) as Count ");
				strQuery.AppendLine(" FROM SuOrganization as Organization ");
				strQuery.AppendLine(" INNER JOIN SuOrganizationLang as OrganizationLang ");
				strQuery.AppendLine(" ON OrganizationLang.Organizationid = Organization.Organizationid AND OrganizationLang.Languageid = :languageID ");
				strQuery.AppendLine(" INNER JOIN DbLanguage as Language ");
				strQuery.AppendLine(" ON Language.Languageid = :languageID ");

				query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
				query.SetInt16("languageID", languageID);
				query.AddScalar("Count", NHibernateUtil.Int32);
			}
			else
			{
				strQuery.AppendLine(" SELECT org.OrganizationID as OrganizationId, orgLang.OrganizationName as OrganizationName ");
				strQuery.AppendLine(" , lang.LanguageID as LanguageId, lang.LanguageName as LanguageName ");
				strQuery.AppendLine(" , org.Comment as Comment, org.Active as Active ");
				strQuery.AppendLine(" FROM SuOrganization org ");
				strQuery.AppendLine(" INNER JOIN DbLanguage lang ");
				strQuery.AppendLine(" ON lang.LanguageID = :languageID ");
				strQuery.AppendLine(" INNER JOIN SuOrganizationLang orgLang ");
				strQuery.AppendLine(" ON orgLang.OrganizationID = org.OrganizationID AND orgLang.LanguageID = lang.LanguageID ");
				
				if (string.IsNullOrEmpty(sortExpression))
				{
					strQuery.AppendLine(" ORDER BY org.OrganizationId, orgLang.OrganizationName, org.Comment, org.Active ");
				}
				else
				{
					strQuery.AppendLine(string.Format(" ORDER BY {0}", sortExpression));
				}

				query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
				query.SetInt16("languageID", languageID);
				query.AddScalar("OrganizationId", NHibernateUtil.Int16);
				query.AddScalar("OrganizationName", NHibernateUtil.String);
				query.AddScalar("LanguageId", NHibernateUtil.Int16);
				query.AddScalar("LanguageName", NHibernateUtil.String);
				query.AddScalar("Comment", NHibernateUtil.String);
				query.AddScalar("Active", NHibernateUtil.Boolean);
				query.SetResultTransformer(Transformers.AliasToBean(typeof(SuOrganizationSearchResult)));
			}

			return query;
		}
		#endregion
	}
}

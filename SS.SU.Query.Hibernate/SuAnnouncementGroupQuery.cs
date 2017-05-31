using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.QueryDao;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.SU.Query;

namespace SS.SU.Query.Hibernate
{
	public class SuAnnouncementGroupQuery : NHibernateQueryBase<SuAnnouncementGroup, short>, ISuAnnouncementGroupQuery
	{
		#region ISuAnnouncementGroupQuery Members
		public IList<TranslatedListItem> GetTranslatedList(short languageId)
		{
			StringBuilder strQuery = new StringBuilder();
			strQuery.AppendLine(" SELECT ag.AnnouncementGroupID as Id , agl.AnnouncementGroupName as Text FROM SuAnnouncementGroup ag ");
			strQuery.AppendLine(" INNER JOIN SuAnnouncementGroupLang agl ");
			strQuery.AppendLine(" ON agl.AnnouncementGroupID = ag.AnnouncementGroupID ");
			strQuery.AppendLine(" WHERE agl.LanguageID = :LanguageID ");
			strQuery.AppendLine(" ORDER BY agl.AnnouncementGroupName ");

			ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
			query.SetInt16("LanguageID", languageId);
			query.AddScalar("Id", NHibernateUtil.Int16);
			query.AddScalar("Text", NHibernateUtil.String);
			IList<TranslatedListItem> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(TranslatedListItem))).List<TranslatedListItem>();
			return list;
		}
		
		public IList<SuAnnouncementGroupSearchResult> GetSuAnnouncementGroupSearchResultList(short languageID, int firstResult, int maxResult, string sortExpression)
		{
			return NHibernateQueryHelper.FindPagingByCriteria<SuAnnouncementGroupSearchResult>(QueryProvider.SuAnnouncementGroupQuery, "FindSuAnnouncementGroupSearchResult", new object[] { languageID, sortExpression, false }, firstResult, maxResult, sortExpression);
		}
		public int GetCountSuAnnouncementGroupSearchResult(short languageID)
		{
			return NHibernateQueryHelper.CountByCriteria(QueryProvider.SuAnnouncementGroupQuery, "FindSuAnnouncementGroupSearchResult", new object[] { languageID, string.Empty, true });
		}
		public ISQLQuery FindSuAnnouncementGroupSearchResult(short languageID, string sortExpression, bool isCount)
		{
			StringBuilder strQuery = new StringBuilder();
			ISQLQuery query;
			
			if (isCount)
			{
				strQuery.AppendLine(" SELECT Count(*) as Count ");
				strQuery.AppendLine(" FROM SuAnnouncementGroup as AnnouncementGroup ");
				strQuery.AppendLine(" INNER JOIN SuAnnouncementGroupLang as AnnouncementGroupLang ");
				strQuery.AppendLine(" ON AnnouncementGroupLang.AnnouncementGroupId = AnnouncementGroup.AnnouncementGroupId AND AnnouncementGroupLang.Languageid = :languageID ");
				strQuery.AppendLine(" INNER JOIN DbLanguage as Language ");
				strQuery.AppendLine(" ON Language.Languageid = :languageID ");

				query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
				query.SetInt16("languageID", languageID);
				query.AddScalar("Count", NHibernateUtil.Int32);
			}
			else
			{
				strQuery.AppendLine(" SELECT ag.AnnouncementGroupID as AnnouncementGroupid, agl.AnnouncementGroupName as AnnouncementGroupName ");
				strQuery.AppendLine(" , ag.DisplayOrder as DisplayOrder, ag.ImagePath as ImagePath ");
				strQuery.AppendLine(" , lang.LanguageID as LanguageId, lang.LanguageName as LanguageName ");
				strQuery.AppendLine(" , ag.Comment as Comment, ag.Active as Active ");
				strQuery.AppendLine(" FROM SuAnnouncementGroup ag ");
				strQuery.AppendLine(" INNER JOIN DbLanguage lang ");
				strQuery.AppendLine(" ON lang.LanguageID = :languageID ");
				strQuery.AppendLine(" INNER JOIN SuAnnouncementGroupLang agl ");
				strQuery.AppendLine(" ON agl.AnnouncementGroupID = ag.AnnouncementGroupID AND agl.LanguageID = lang.LanguageID ");

				if (string.IsNullOrEmpty(sortExpression))
				{
					strQuery.AppendLine(" ORDER BY ag.AnnouncementGroupId, agl.AnnouncementGroupName, ag.DisplayOrder, ag.ImagePath, ag.Comment, ag.Active ");
				}
				else
				{
					strQuery.AppendLine(string.Format(" ORDER BY {0}", sortExpression));
				}

				query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
				query.SetInt16("languageID", languageID);
				query.AddScalar("AnnouncementGroupid", NHibernateUtil.Int16);
				query.AddScalar("AnnouncementGroupName", NHibernateUtil.String);
				query.AddScalar("DisplayOrder", NHibernateUtil.Int16);
				query.AddScalar("ImagePath", NHibernateUtil.String);
				query.AddScalar("LanguageId", NHibernateUtil.Int16);
				query.AddScalar("LanguageName", NHibernateUtil.String);
				query.AddScalar("Comment", NHibernateUtil.String);
				query.AddScalar("Active", NHibernateUtil.Boolean);
				query.SetResultTransformer(Transformers.AliasToBean(typeof(SuAnnouncementGroupSearchResult)));
			}

			return query;
		}
		#endregion
	}
}

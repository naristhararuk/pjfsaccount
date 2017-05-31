using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.SU.Query;

using NHibernate;
using NHibernate.Transform;

namespace SS.SU.Query.Hibernate
{
	public class SuAnnouncementQuery : NHibernateQueryBase<SuAnnouncement, short>, ISuAnnouncementQuery
	{
		#region ISuAnnouncementQuery Members
        //public IList<SuAnnouncementSearchResult> GetTranslatedList(SuAnnouncementSearchResult criteria, short languageID, short announcementGroupId, int firstResult, int maxResult, string sortExpression)
        //{
        //    return NHibernateQueryHelper.FindPagingByCriteria<SuAnnouncementSearchResult>(QueryProvider.SuAnnouncementQuery, "FindSuAnnouncementSearchResult", new object[] { criteria, languageID, announcementGroupId }, firstResult, maxResult, sortExpression);
        //}
		//public ISQLQuery FindSuAnnouncementSearchResult(SuAnnouncementSearchResult announcementSearchResult, short languageID, short announcementGroupId)
        //{
        //    StringBuilder strQuery = new StringBuilder();
        //    strQuery.AppendLine(" SELECT a.AnnouncementID as Announcementid, al.AnnouncementHeader as AnnouncementHeader ");
        //    strQuery.AppendLine(" , a.EffectiveDate as EffectiveDate, a.LastDisplayDate as LastDisplayDate ");
        //    strQuery.AppendLine(" , lang.LanguageID as LanguageId ");
        //    strQuery.AppendLine(" , a.Comment as Comment, a.Active as Active ");
        //    strQuery.AppendLine(" FROM SuAnnouncement a ");
        //    strQuery.AppendLine(" INNER JOIN DbLanguage lang ");
        //    strQuery.AppendLine(" ON lang.LanguageID = :languageID ");
        //    strQuery.AppendLine(" INNER JOIN SuAnnouncementLang al ");
        //    strQuery.AppendLine(" ON al.AnnouncementID = a.AnnouncementID AND al.LanguageID = lang.LanguageID ");
        //    strQuery.AppendLine(" WHERE a.AnnouncementGroupID = :announcementGroupId ");

        //    ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
        //    query.SetInt16("languageID", languageID);
        //    query.SetInt16("announcementGroupId", announcementGroupId);
        //    query.AddScalar("Announcementid", NHibernateUtil.Int16);
        //    query.AddScalar("AnnouncementHeader", NHibernateUtil.String);
        //    query.AddScalar("EffectiveDate", NHibernateUtil.DateTime);
        //    query.AddScalar("LastDisplayDate", NHibernateUtil.DateTime);
        //    query.AddScalar("LanguageId", NHibernateUtil.Int16);
        //    query.AddScalar("Comment", NHibernateUtil.String);
        //    query.AddScalar("Active", NHibernateUtil.Boolean);
        //    query.SetResultTransformer(Transformers.AliasToBean(typeof(SuAnnouncementSearchResult)));

        //    return query;
        //}
        //public int FindCountSuAnnouncementSearchResult(SuAnnouncementSearchResult announcementSearchResult, short languageID, short announcementGroupId)
        //{
        //    StringBuilder strQuery = new StringBuilder();
        //    strQuery.AppendLine(" SELECT Count(*) as Count ");
        //    strQuery.AppendLine(" FROM SuAnnouncement as Announcement ");
        //    strQuery.AppendLine(" INNER JOIN SuAnnouncementLang as AnnouncementLang ");
        //    strQuery.AppendLine(" ON AnnouncementLang.AnnouncementId = Announcement.AnnouncementId AND AnnouncementLang.Languageid = :languageID ");
        //    strQuery.AppendLine(" INNER JOIN DbLanguage as Language ");
        //    strQuery.AppendLine(" ON Language.Languageid = :languageID ");
        //    strQuery.AppendLine(" WHERE Announcement.AnnouncementGroupID = :announcementGroupId ");

        //    ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
        //    query.SetInt16("languageID", languageID);
        //    query.SetInt16("announcementGroupId", announcementGroupId);
        //    query.AddScalar("Count", NHibernateUtil.Int32);

        //    return Convert.ToInt32(query.UniqueResult());
        //}
		#endregion

		#region ISuAnnouncementQuery Members
		public IList<SuAnnouncementSearchResult> GetAnnouncementList(short languageID, short announcementGroupId, int firstResult, int maxResult, string sortExpression)
		{
			return NHibernateQueryHelper.FindByCriteria<SuAnnouncementSearchResult>(QueryProvider.SuAnnouncementQuery, "FindSuAnnouncementSearchResult", new object[] { languageID, announcementGroupId, sortExpression, false });
		}
		public int GetCountAnnouncement(short languageID, short announcementGroupId)
		{
			return NHibernateQueryHelper.CountByCriteria(QueryProvider.SuAnnouncementQuery, "FindSuAnnouncementSearchResult", new object[] {languageID, announcementGroupId, string.Empty, true });
		}
		public ISQLQuery FindSuAnnouncementSearchResult(short languageID, short announcementGroupId, string sortExpression, bool isCount)
		{
			StringBuilder strQuery = new StringBuilder();
			ISQLQuery query;

			if (isCount)
			{
				strQuery.AppendLine(" SELECT Count(*) as Count ");
				strQuery.AppendLine(" FROM SuAnnouncement as Announcement ");
				strQuery.AppendLine(" INNER JOIN SuAnnouncementLang as AnnouncementLang ");
				strQuery.AppendLine(" ON AnnouncementLang.AnnouncementId = Announcement.AnnouncementId AND AnnouncementLang.Languageid = :languageID ");
				strQuery.AppendLine(" INNER JOIN DbLanguage as Language ");
				strQuery.AppendLine(" ON Language.Languageid = :languageID ");
				strQuery.AppendLine(" WHERE Announcement.AnnouncementGroupID = :announcementGroupId ");

				query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
				query.SetInt16("languageID", languageID);
				query.SetInt16("announcementGroupId", announcementGroupId);
				query.AddScalar("Count", NHibernateUtil.Int32);
			}
			else
			{
				strQuery.AppendLine(" SELECT a.AnnouncementID as Announcementid, al.AnnouncementHeader as AnnouncementHeader ");
				strQuery.AppendLine(" , a.EffectiveDate as EffectiveDate, a.LastDisplayDate as LastDisplayDate ");
				strQuery.AppendLine(" , lang.LanguageID as LanguageId ");
				strQuery.AppendLine(" , a.Comment as Comment, a.Active as Active ");
				strQuery.AppendLine(" FROM SuAnnouncement a ");
				strQuery.AppendLine(" INNER JOIN DbLanguage lang ");
				strQuery.AppendLine(" ON lang.LanguageID = :languageID ");
				strQuery.AppendLine(" INNER JOIN SuAnnouncementLang al ");
				strQuery.AppendLine(" ON al.AnnouncementID = a.AnnouncementID AND al.LanguageID = lang.LanguageID ");
				strQuery.AppendLine(" WHERE a.AnnouncementGroupID = :announcementGroupId ");

				if (string.IsNullOrEmpty(sortExpression))
				{
					strQuery.AppendLine(" ORDER BY a.AnnouncementID, al.AnnouncementHeader, a.EffectiveDate, a.LastDisplayDate, a.Comment, a.Active ");
				}
				else
				{
					strQuery.AppendLine(" ORDER BY " + sortExpression);
				}

				query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
				query.SetInt16("languageID", languageID);
				query.SetInt16("announcementGroupId", announcementGroupId);
				query.AddScalar("Announcementid", NHibernateUtil.Int16);
				query.AddScalar("AnnouncementHeader", NHibernateUtil.String);
				query.AddScalar("EffectiveDate", NHibernateUtil.DateTime);
				query.AddScalar("LastDisplayDate", NHibernateUtil.DateTime);
				query.AddScalar("LanguageId", NHibernateUtil.Int16);
				query.AddScalar("Comment", NHibernateUtil.String);
				query.AddScalar("Active", NHibernateUtil.Boolean);
				query.SetResultTransformer(Transformers.AliasToBean(typeof(SuAnnouncementSearchResult)));
			}
			
			return query;
		}
		#endregion
	}
}

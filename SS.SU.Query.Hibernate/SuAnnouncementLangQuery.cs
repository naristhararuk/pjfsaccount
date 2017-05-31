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
	public class SuAnnouncementLangQuery : NHibernateQueryBase<SuAnnouncementLang, long>, ISuAnnouncementLangQuery
	{
		#region ISuAnnouncementLangQuery Members
		public IList<SuAnnouncementLang> FindByAnnouncementId(short announcementId)
		{
			IList<SuAnnouncementLang> list = GetCurrentSession()
				.CreateQuery("FROM SuAnnouncementLang as al where al.Announcement.Announcementid = :announcementId")
				.SetInt16("announcementId", announcementId).List<SuAnnouncementLang>();

			return list;
		}
		public IList<AnnouncementLang> FindAnnouncementLangByAnnouncementId(short announcementId)
		{
			StringBuilder sqlBuilder = new StringBuilder();
			sqlBuilder.Append("SELECT l.LanguageId as LanguageId, l.LanguageName as LanguageName, al.AnnouncementId as AnnouncementId, ");
			sqlBuilder.Append(" al.Id as AnnouncementLangId, al.AnnouncementHeader as AnnouncementHeader, al.Comment as Comment, al.Active as Active");
			sqlBuilder.Append(" FROM DbLanguage l LEFT JOIN SuAnnouncementLang al on al.LanguageId = l.LanguageId and al.AnnouncementId = :announcementId");

			QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
			parameterBuilder.AddParameterData("announcementId", typeof(short), announcementId);

			ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
			parameterBuilder.FillParameters(query);
			query.AddScalar("LanguageId", NHibernateUtil.Int16)
				.AddScalar("LanguageName", NHibernateUtil.String)
				.AddScalar("AnnouncementId", NHibernateUtil.Int16)
				.AddScalar("AnnouncementLangId", NHibernateUtil.Int64)
				.AddScalar("AnnouncementHeader", NHibernateUtil.String)
				.AddScalar("Comment", NHibernateUtil.String)
				.AddScalar("Active", NHibernateUtil.Boolean);

			return query.SetResultTransformer(Transformers.AliasToBean(typeof(AnnouncementLang))).List<AnnouncementLang>();
		}
        public IList<SuAnnouncementLang> FindByAnnouncementLanguageId(short languageId, short announcementGroupId)
        {
            DateTime dateTime = DateTime.Now.Date;
            StringBuilder sqlBuilder = new StringBuilder();
			sqlBuilder.AppendLine("FROM SuAnnouncementLang as al ");
			sqlBuilder.AppendLine("where al.Announcement.AnnouncementGroup.AnnouncementGroupid = :announcementGroupId ");
			sqlBuilder.AppendLine("and al.Language.Languageid = :languageId ");
			sqlBuilder.AppendLine("and al.Announcement.EffectiveDate <= :dateTime ");
			sqlBuilder.AppendLine("and al.Announcement.LastDisplayDate >= :dateTime ");
			sqlBuilder.AppendLine("order by al.CreDate desc ");
            
            IList<SuAnnouncementLang> list = GetCurrentSession()
                .CreateQuery(sqlBuilder.ToString())
                .SetInt16("announcementGroupId", announcementGroupId)
                .SetInt16("languageId", languageId)
                .SetDateTime("dateTime", dateTime).List<SuAnnouncementLang>();


			foreach (SuAnnouncementLang announcementLang in list)
			{
				Console.WriteLine(announcementLang.Announcement.AnnouncementGroup);
			}
            
            return list;
        }
		#endregion

        #region ISuAnnouncementLangQuery Members


        public IList<AnnouncementLang> FindByDateAnnouncementLangId(short announcementId, short languageId)
        {
            DateTime dateTime = DateTime.Now.Date;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("select agl.AnnouncementGroupName,al.Announcementid,AnnouncementHeader,AnnouncementBody,AnnouncementFooter,ImagePath");
            sqlBuilder.Append(" from SuAnnouncementLang al inner join SuAnnouncement a ");
            sqlBuilder.Append(" on a.AnnouncementID = al.AnnouncementID  inner join ");
            sqlBuilder.Append(" SuAnnouncementGroupLang agl on a.AnnouncementGroupID = agl.AnnouncementGroupID");
            sqlBuilder.Append(" inner join SuAnnouncementGroup ag on agl.AnnouncementGroupID = ag.AnnouncementGroupID");
            sqlBuilder.Append(" where al.Announcementid = :announcementId and al.Languageid = :languageId");
            sqlBuilder.Append(" and agl.Languageid = :languageId and a.EffectiveDate <= :dateTime ");

            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("announcementId", typeof(short), announcementId);
            parameterBuilder.AddParameterData("languageId", typeof(short), languageId);
            parameterBuilder.AddParameterData("dateTime", typeof(DateTime), dateTime);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);
            query.AddScalar("AnnouncementGroupName", NHibernateUtil.String)
                .AddScalar("AnnouncementId", NHibernateUtil.Int16)
                .AddScalar("AnnouncementHeader", NHibernateUtil.String)
                .AddScalar("AnnouncementBody", NHibernateUtil.String)
                .AddScalar("AnnouncementFooter", NHibernateUtil.String)
                .AddScalar("ImagePath", NHibernateUtil.String);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(AnnouncementLang))).List<AnnouncementLang>();
        }

        public IList<SuAnnouncementLang> FindAnnouncementByLangId(short languageId, short announcementGroupId)
        {
            DateTime dateTime = DateTime.Now.Date;
            StringBuilder sqlBuilder = new StringBuilder();
			sqlBuilder.AppendLine("FROM SuAnnouncementLang as al ");
			sqlBuilder.AppendLine("where al.Announcement.AnnouncementGroup.AnnouncementGroupid = :announcementGroupId ");
			sqlBuilder.AppendLine("and al.Language.Languageid = :languageId ");
			sqlBuilder.AppendLine("order by al.CreDate DESC");
            
            IList<SuAnnouncementLang> list = GetCurrentSession()
                .CreateQuery(sqlBuilder.ToString())
                .SetInt16("languageId", languageId)
                .SetInt16("announcementGroupId", announcementGroupId)
                .List<SuAnnouncementLang>();


			foreach (SuAnnouncementLang announcementLang in list)
			{
				Console.WriteLine(announcementLang.Announcement.AnnouncementGroup);
			}
            
            return list;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.SU.DAL;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

namespace SS.SU.DAL.Hibernate
{
	public class SuAnnouncementLangDao : NHibernateDaoBase<SuAnnouncementLang, long>, ISuAnnouncementLangDao
	{	
		#region ISuAnnouncementLangDao Members
		public IList<SuAnnouncementLang> FindByAnnouncementAndLanguage(short announcementId, short languageId)
		{
			IList<SuAnnouncementLang> list = GetCurrentSession()
				.CreateQuery("FROM SuAnnouncementLang al WHERE al.Announcement.Announcementid = :AnnouncementId AND al.Language.Languageid = :LanguageId ")
				.SetInt16("AnnouncementId", announcementId)
				.SetInt16("LanguageId", languageId)
				.List<SuAnnouncementLang>();

			return list;
		}
		public IList<SuAnnouncementLang> FindByAnnouncementId(short announcementId)
		{
			IList<SuAnnouncementLang> list = GetCurrentSession()
				.CreateQuery("FROM SuAnnouncementLang al WHERE al.Announcement.Announcementid = :AnnouncementId ")
				.SetInt16("AnnouncementId", announcementId).List<SuAnnouncementLang>();

			return list;
		}
		public void DeleteByCriteria(short announcementId)
		{
			StringBuilder deleteCommand = new StringBuilder();
			deleteCommand.AppendLine("FROM SuAnnouncementLang al ");
			deleteCommand.AppendLine("WHERE al.Announcement.Announcementid = :announcementId ");
			//deleteCommand.AppendLine("AND agl.Language.Languageid = :languageId ");

			GetCurrentSession()
				.Delete(deleteCommand.ToString()
				, new object[] { announcementId }
				, new NHibernate.Type.IType[] { NHibernateUtil.Int16 });
		}
		public void DeleteByCriteria(short announcementId, short languageId)
		{
			StringBuilder deleteCommand = new StringBuilder();
			deleteCommand.AppendLine("FROM SuAnnouncementLang al ");
			deleteCommand.AppendLine("WHERE al.Announcement.Announcementid = :announcementId ");
			deleteCommand.AppendLine("AND al.Language.Languageid = :languageId ");

			GetCurrentSession()
				.Delete(deleteCommand.ToString()
				, new object[] { announcementId, languageId }
				, new NHibernate.Type.IType[] { NHibernateUtil.Int16, NHibernateUtil.Int16 });
		}
		#endregion
	}
}

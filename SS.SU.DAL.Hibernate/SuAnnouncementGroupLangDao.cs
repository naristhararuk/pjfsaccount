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
	public class SuAnnouncementGroupLangDao : NHibernateDaoBase<SuAnnouncementGroupLang, short>, ISuAnnouncementGroupLangDao
	{
		#region ISuAnnouncementGroupLangDao Members
		public IList<SuAnnouncementGroupLang> FindByAnnouncementGroupId(short announcementGroupId)
		{
			IList<SuAnnouncementGroupLang> list = GetCurrentSession()
				.CreateQuery("FROM SuAnnouncementGroupLang agl WHERE agl.AnnouncementGroup.AnnouncementGroupid = :AnnouncementGroupId")
				.SetInt64("AnnouncementGroupId", announcementGroupId).List<SuAnnouncementGroupLang>();

			return list;
		}
		public bool IsDuplicate(short announcementGroupId, short languageId)
		{
			IList<SuAnnouncementGroupLang> list = GetCurrentSession()
				.CreateQuery("FROM SuAnnouncementGroupLang agl where agl.AnnouncementGroup.AnnouncementGroupid = :AnnouncementGroupId AND agl.Language.LanguageId = : LanguageId")
				.SetInt64("AnnouncementGroupId", announcementGroupId)
				.SetInt16("LanguageId", languageId)
				.List<SuAnnouncementGroupLang>();

			if (list.Count > 0)
			{
				return true;
			}
			return false;
		}
		public void DeleteByCriteria(short announcementGroupId)
		{
			StringBuilder deleteCommand = new StringBuilder();
			deleteCommand.AppendLine("FROM SuAnnouncementGroupLang agl ");
			deleteCommand.AppendLine("WHERE agl.AnnouncementGroup.AnnouncementGroupid = :announcementGroupId ");
			//deleteCommand.AppendLine("AND agl.Language.Languageid = :languageId ");
			
			GetCurrentSession()
				.Delete(deleteCommand.ToString()
				, new object[] { announcementGroupId }
				, new NHibernate.Type.IType[] { NHibernateUtil.Int16 });
		}
		#endregion
	}
}

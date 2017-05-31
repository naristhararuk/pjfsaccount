using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;

namespace SS.SU.DAL
{
	public interface ISuAnnouncementLangDao : IDao<SuAnnouncementLang, long>
	{
		IList<SuAnnouncementLang> FindByAnnouncementAndLanguage(short announcementId, short languageId);
		IList<SuAnnouncementLang> FindByAnnouncementId(short announcementId);
		void DeleteByCriteria(short announcementId);
		void DeleteByCriteria(short announcementId, short languageId);
	}
}

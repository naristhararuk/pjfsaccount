using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;

namespace SS.SU.BLL
{
	public interface ISuAnnouncementLangService : IService<SuAnnouncementLang, long>
	{
		IList<SuAnnouncementLang> FindByAnnouncementAndLanguage(short announcementId, short languageId);
		IList<SuAnnouncementLang> FindByAnnouncementId(short announcementId);
		void UpdateAnnouncementLang(IList<SuAnnouncementLang> announcementLangList);
		void SaveAnnouncementLang(SuAnnouncementLang announcementLang);
		void InsertAnnouncementLang(SuAnnouncementLang announcementLang);
		void UpdateAnnouncementLang(SuAnnouncementLang announcementLang);
	}
}

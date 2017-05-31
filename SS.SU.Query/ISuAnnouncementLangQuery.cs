using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;
using SS.Standard.Data.NHibernate.Query;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

namespace SS.SU.Query
{
	public interface ISuAnnouncementLangQuery : IQuery<SuAnnouncementLang, long>
	{
		IList<SuAnnouncementLang> FindByAnnouncementId(short announcementId);
		IList<AnnouncementLang> FindAnnouncementLangByAnnouncementId(short announcementId);
        IList<SuAnnouncementLang> FindByAnnouncementLanguageId(short languageId, short announcementGroupId);
        IList<AnnouncementLang> FindByDateAnnouncementLangId(short announcementId, short languageId);
        IList<SuAnnouncementLang> FindAnnouncementByLangId(short languageId, short announcementGroupId);
	}
}

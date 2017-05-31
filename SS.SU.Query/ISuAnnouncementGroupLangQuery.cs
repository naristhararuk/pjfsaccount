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
	public interface ISuAnnouncementGroupLangQuery : IQuery<SuAnnouncementGroupLang, short>
	{
		IList<SuAnnouncementGroupLang> FindByAnnouncementGroupId(short announcementGroupId);
		IList<AnnouncementGroupLang> FindAnnouncementGroupLangByAnnouncementGroupId(short announcementGroupId);
        IList<SuAnnouncementGroupLang> FindByAnnouncementLanguageId(short languageId);
        IList<SuAnnouncementGroupLang> FindByAnnouncementGroupIdAndLanguageId(short languageId, short announcementGroupId);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;

namespace SS.SU.BLL
{
	public interface ISuAnnouncementGroupLangService : IService<SuAnnouncementGroupLang, short>
	{
		IList<SuAnnouncementGroupLang> FindByAnnouncementGroupId(short announcementGroupId);
		void UpdateAnnouncementGroupLang(IList<SuAnnouncementGroupLang> announcementGroupLangList);
	}
}

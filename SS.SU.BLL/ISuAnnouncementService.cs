using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;

namespace SS.SU.BLL
{
	public interface ISuAnnouncementService : IService<SuAnnouncement, short>
	{
		IList<SuAnnouncement> FindBySuAnnouncementCriteria(SuAnnouncement announcementCriteria, int firstResult, int maxResults, string sortExpression);
		int CountBySuAnnouncementCriteria(SuAnnouncement announcementCriteria);
		short AddAnnouncement(SuAnnouncement announcement, SuAnnouncementLang announcementLang);
		void UpdateAnnouncement(SuAnnouncement announcement);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;

using NHibernate;

namespace SS.SU.DAL
{
	public interface ISuAnnouncementDao : IDao<SuAnnouncement, short>
	{
		ICriteria FindBySuAnnouncementCriteria(SuAnnouncement announcement);
	}
}

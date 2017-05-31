using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DAL;

using NHibernate;
using NHibernate.Expression;

namespace SS.SU.DAL.Hibernate
{
	public class SuAnnouncementDao : NHibernateDaoBase<SuAnnouncement, short>, ISuAnnouncementDao
	{

		#region ISuAnnouncementDao Members
		public ICriteria FindBySuAnnouncementCriteria(SuAnnouncement announcement)
		{
			ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(SuAnnouncement), "Announcement");
			criteria.Add(Expression.Eq("Announcement.AnnouncementGroup.AnnouncementGroupid", announcement.AnnouncementGroup.AnnouncementGroupid));
			return criteria;
		}
		#endregion
	}
}

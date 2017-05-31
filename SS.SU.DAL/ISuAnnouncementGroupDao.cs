using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

using NHibernate;

namespace SS.SU.DAL
{
	public interface ISuAnnouncementGroupDao : IDao<SuAnnouncementGroup, short>
	{
		ICriteria FindBySuAnnouncementGroupCriteria(SuAnnouncementGroup announcementGroup);
		IList<AnnouncementGroup> GetTranslatedList(short languageId);
	}
}

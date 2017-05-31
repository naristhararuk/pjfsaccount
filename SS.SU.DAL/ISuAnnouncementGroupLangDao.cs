using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;

namespace SS.SU.DAL
{
	public interface ISuAnnouncementGroupLangDao : IDao<SuAnnouncementGroupLang, short>
	{
		IList<SuAnnouncementGroupLang> FindByAnnouncementGroupId(short announcementGroupId);
		bool IsDuplicate(short announcementGroupId, short languageId);
		void DeleteByCriteria(short announcementGroupId);
	}
}
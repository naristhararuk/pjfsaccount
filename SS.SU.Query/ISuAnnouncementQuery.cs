using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;

using SS.Standard.Data.NHibernate.Service;
using SS.Standard.Data.NHibernate.Query;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

namespace SS.SU.Query
{
	public interface ISuAnnouncementQuery : IQuery<SuAnnouncement, short>
	{
		IList<SuAnnouncementSearchResult> GetAnnouncementList(short languageID, short announcementGroupId, int firstResult, int maxResult, string sortExpression);
		ISQLQuery FindSuAnnouncementSearchResult(short languageID, short announcementGroupId, string sortExpression, bool isCount);
		int GetCountAnnouncement(short languageID, short announcementGroupId);
	}
}

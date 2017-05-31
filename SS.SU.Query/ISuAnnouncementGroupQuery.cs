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
	public interface ISuAnnouncementGroupQuery : IQuery<SuAnnouncementGroup, short>
	{
		IList<TranslatedListItem> GetTranslatedList(short languageId);
		
		IList<SuAnnouncementGroupSearchResult> GetSuAnnouncementGroupSearchResultList(short languageID, int firstResult, int maxResult, string sortExpression);
		int GetCountSuAnnouncementGroupSearchResult(short languageID);
		ISQLQuery FindSuAnnouncementGroupSearchResult(short languageID, string sortExpression, bool isCount);
	}
}

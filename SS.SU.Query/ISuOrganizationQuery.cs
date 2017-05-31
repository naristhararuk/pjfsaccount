using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;

using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.Query;

using SS.SU.DTO;

namespace SS.SU.Query
{
    public interface ISuOrganizationQuery : IQuery<SuOrganization, short>
    {
        IList<TranslatedListItem> GetTranslatedList(short languageID);
		IList<SuOrganizationSearchResult> GetOrganizationList(short languageID, int firstResult, int maxResult, string sortExpression);
		int GetCountOrganizationList(short languageID);
		ISQLQuery FindSuOrganizationSearchResult(short languageID, string sortExpression, bool isCount);
    }
}

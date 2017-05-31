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
    public interface ISuRoleLangQuery : IQuery<SuRoleLang, short>
    {
		IList<SuRoleLangSearchResult> GetTranslatedList(SuRoleLangSearchResult criteria, short languageID, string roleName, long userID, int firstResult, int maxResult, string sortExpression);
		ISQLQuery FindSuRoleLangSearchResult(SuRoleLangSearchResult roleLangSearchResult, short languageID, string roleName, long userID);
		int FindCountSuRoleLangSearchResult(SuRoleLangSearchResult roleLangSearchResult, short languageID, string roleName, long userID);
    }
}

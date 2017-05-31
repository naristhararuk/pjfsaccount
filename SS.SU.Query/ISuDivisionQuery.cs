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
    public interface ISuDivisionQuery : IQuery<SuDivision, short>   //, ISimpleMasterQuery
    {
        IList<TranslatedListItem> GetTranslatedList(short languageID);
        IList<SuDivisionSearchResult> GetDivisionList(short languageID, short organizationId, int firstResult, int maxResult, string sortExpression);
        ISQLQuery FindSuDivisionSearchResult(short languageID, short organizationId, string sortExpression, bool isCount);
        int GetCountDivisionList(short languageID, short organizationId);

    }
}

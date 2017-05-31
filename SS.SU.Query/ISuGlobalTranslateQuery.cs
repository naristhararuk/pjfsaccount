using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;
using SS.Standard.Data.NHibernate.Query;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using NHibernate;

namespace SS.SU.Query
{
    public interface ISuGlobalTranslateQuery : IQuery<SuGlobalTranslate, long>
    {
        GlobalTranslate ResolveMessage(string translateSymbol, string languageCode);
        GlobalTranslate ResolveMessage(string programCode, string translateSymbol, string languageCode);
        IList<GlobalTranslate> LoadProgramResources(string programCode, string languageCode);
        IList<TranslateSearchResult> GetTranslatedList(SuGlobalTranslate criteria, short languageID, int firstResult, int maxResult, string sortExpression);
        ISQLQuery FindGlobalTranslateSearchResult(SuGlobalTranslate criteria, short languageId, string sortExpression, bool isCount);
        int GetCountTranslatedList(SuGlobalTranslate criteria, short languageId);
        string GetResolveControl(string programCode, string translateControl, string languageCode);
    }
}

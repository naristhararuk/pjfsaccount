using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;

using SS.Standard.Data.NHibernate.Service;
using SS.Standard.Data.NHibernate.Query;

using SS.DB.DTO;
using SS.DB.DTO.ValueObject;

namespace SS.DB.Query
{
    public interface IDbLanguageQuery : IQuery<DbLanguage, short>
    {
        DbLanguage FindLanguageByIdentity(short lid);
        new IList<DbLanguage> FindAll();
        new DbLanguage FindByIdentity(short id);
        List<DbLanguage> FindAllList();
        IList<DbLanguageSearchResult> GetLanguageList(short languageID, int firstResult, int maxResult, string sortExpression);
        ISQLQuery FindDbLanguageSearchResult(short languageID, string sortExpression, bool isCount);
        int GetCountLanguageList(short languageID);
        List<DbLanguage> FindAllListLanguage();
    }
}

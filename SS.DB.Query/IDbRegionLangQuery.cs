using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Query;

using SS.DB.DTO;
using NHibernate;
using SS.SU.DTO;


namespace SS.DB.Query
{
    public interface IDbRegionLangQuery : IQuery<DbRegionLang, long>
    {
        IList<TranslatedListItem> FindRegionByLangCriteria(short languageId);
    }
}

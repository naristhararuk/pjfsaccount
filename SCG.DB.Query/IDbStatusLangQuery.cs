using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using SCG.DB.DTO;
using SS.Standard.Data.Query;
using SCG.DB.DTO.ValueObject;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;
using SS.DB.DTO.ValueObject;

namespace SCG.DB.Query
{
    public interface IDbStatusLangQuery : IQuery<SCGDbStatusLang, long>
    {
        IList<TranslatedListItem> FindStatusLangCriteria(string groupStatus, short languageId);
        string GetStatusLang(string groupStatus, int languageID, string status);
    }
}

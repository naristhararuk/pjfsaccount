using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Query;

using SS.DB.DTO;
using NHibernate;
using SS.DB.DTO.ValueObject;

namespace SS.DB.Query
{
    public interface IDbStatusLangQuery : IQuery<DbStatusLang, short>
    {
        IList<StatusLang> FindStatusLangByStatusId(short statusId);
    }
}

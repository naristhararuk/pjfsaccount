using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.DB.DTO;
using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;
using SCG.DB.DTO.ValueObject;

namespace SCG.DB.Query
{
    public interface IDbReasonLangQuery : IQuery<DbScgReasonLang, long>
    {
        IList<VOReasonLang> FindAutoComplete(string reasonDetail , string documentType, short languageId);
        IList<VOReasonLang> FindReasonLangByReasonId(short reasonId);
    }
}

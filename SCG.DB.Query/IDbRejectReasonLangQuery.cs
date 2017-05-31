using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;
using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;

using NHibernate;

using SCG.DB.DTO;

namespace SCG.DB.Query
{
    public interface IDbRejectReasonLangQuery : IQuery<DbRejectReasonLang ,long>
    {
    }
}

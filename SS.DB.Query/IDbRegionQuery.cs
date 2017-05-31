using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Query;

using SS.DB.DTO;
using SS.DB.DTO.ValueObject;
using NHibernate;

namespace SS.DB.Query
{
    public interface IDbRegionQuery : IQuery<DbRegion, short>
    {
    }
}

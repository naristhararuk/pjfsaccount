using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;
using SS.Standard.Utilities;
using SS.Standard.Security;

using SS.DB.DTO;
using SS.DB.Query;
using SS.DB.DTO.ValueObject;

using NHibernate;
using NHibernate.Transform;

namespace SS.DB.Query.Hibernate
{
    public class DbRegionQuery : NHibernateQueryBase<DbRegion, short>, IDbRegionQuery
    {
        
    }
}

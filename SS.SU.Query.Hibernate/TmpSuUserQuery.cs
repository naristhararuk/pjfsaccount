using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.QueryDao;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.SU.Query;

namespace SS.SU.Query.Hibernate
{
    public class TmpSuUserQuery : NHibernateQueryBase<TmpSuUser, long>, ITmpSuUserQuery
    {
    }
}

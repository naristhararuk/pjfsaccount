using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SCG.GL.DTO;
using SCG.GL.Query;
using SCG.GL.DTO.ValueObject;

namespace SCG.GL.Query.Hibernate
{
    public class GlAccountLangQuery : NHibernateQueryBase<GlAccountLang, long>, IGlAccountLangQuery
    {
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SCG.FN.DAL;
using SCG.FN.DTO;
using SCG.FN.DTO.ValueObject;

namespace SCG.FN.DAL.Hibernate
{
    public class FnExpensePerdiemItemDao : NHibernateDaoBase<FnExpensePerdiemItem, long>, IFnExpensePerdiemItemDao
    {
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SCG.eAccounting.DTO;
using SCG.eAccounting.Query;

namespace SCG.eAccounting.Query.Hibernate
{
    public class FnExpensePerdiemItemQuery : NHibernateQueryBase<FnExpensePerdiemItem, long>, IFnExpensePerdiemItemQuery
	{
        public IList<FnExpensePerdiemItem> GetPerdiemItemByPerdiemID(long perdiemId)
        {
            return GetCurrentSession().CreateQuery(" from FnExpensePerdiemItem item where item.ExpensePerdiem.ExpensePerdiemID = :PerdiemID")
                .SetInt64("PerdiemID", perdiemId)
                .List<FnExpensePerdiemItem>();
        }
	}
}

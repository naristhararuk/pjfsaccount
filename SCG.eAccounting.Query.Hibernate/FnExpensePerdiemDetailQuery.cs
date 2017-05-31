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
    public class FnExpensePerdiemDetailQuery : NHibernateQueryBase<FnExpensePerdiemDetail, long>, IFnExpensePerdiemDetailQuery
	{
        public IList<FnExpensePerdiemDetail> GetPerdiemDetailByPerdiemID(long perdiemId)
        {
            return GetCurrentSession().CreateQuery(" from FnExpensePerdiemDetail item where ExpensePerdiemID = :PerdiemID ")
                .SetInt64("PerdiemID", perdiemId)
                .List<FnExpensePerdiemDetail>();
        }
	}
}

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
    public class FnExpenseMileageInvoiceQuery : NHibernateQueryBase<FnExpenseMileageInvoice, long>, IFnExpenseMileageInvoiceQuery
	{
        public IList<FnExpenseMileageInvoice> GetMileageInvoiceByMileageID(long mileageId)
        {
            return GetCurrentSession().CreateQuery(" from FnExpenseMileageInvoice where ExpenseMileageID = :MileageID and Active = 1")
                .SetInt64("MileageID", mileageId)
                .List<FnExpenseMileageInvoice>();
        }
	}
}

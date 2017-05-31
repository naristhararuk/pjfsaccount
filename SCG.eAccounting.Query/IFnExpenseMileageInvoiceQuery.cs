using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SCG.eAccounting.DTO;

using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;

namespace SCG.eAccounting.Query
{
    public interface IFnExpenseMileageInvoiceQuery : IQuery<FnExpenseMileageInvoice, long>
	{
        IList<FnExpenseMileageInvoice> GetMileageInvoiceByMileageID(long mileageId);
	}
}

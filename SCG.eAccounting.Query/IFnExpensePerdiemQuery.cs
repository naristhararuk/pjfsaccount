using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SCG.eAccounting.DTO;

using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;
using SCG.eAccounting.DTO.ValueObject;
using SS.Standard.WorkFlow.DTO;

namespace SCG.eAccounting.Query
{
    public interface IFnExpensePerdiemQuery : IQuery<FnExpensePerdiem, long>
	{
        IList<FnExpensePerdiem> GetPerdiemByInvoiceID(long invoiceId);
        IList<FnExpensePerdiem> GetPerdiemByExpenseID(long expenseId);
        Document CheckDateLength(long RequesterId, DateTime date, long expenseId);
        ValidatePrediem GetPerdiemItemDate(long RequesterId, DateTime date, long expenseId);
	}
}

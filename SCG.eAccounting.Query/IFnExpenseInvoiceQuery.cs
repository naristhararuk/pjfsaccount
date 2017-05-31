using System.Linq;
using System.Text;

using SCG.eAccounting.DTO;

using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;
using SCG.eAccounting.DTO.ValueObject;
using NHibernate;
using System.Collections.Generic;

namespace SCG.eAccounting.Query
{
    public interface IFnExpenseInvoiceQuery : IQuery<FnExpenseInvoice, long>
    {
        double FindExpenseInvoiceByDocumentID(long documentID);
        IList<InvoiceDataForEmail> FindInvoiceDataByExpenseID(long expenseID);
        IList<FnExpenseInvoice> GetInvoiceByExpenseID(long expenseId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SCG.eAccounting.DTO;

using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;
using SCG.eAccounting.DTO.ValueObject;

namespace SCG.eAccounting.Query
{
    public interface IFnExpenseInvoiceItemQuery : IQuery<FnExpenseInvoiceItem, long>
	{
        IList<FnExpenseInvoiceItem> GetInvoiceItemByInvoiceID(long invoiceId);
        IList<VOInvoiceItem> GetInvoiceItemListByExpenseID(long expenseId, short languageId, int firstResult, int maxResult, string sortExpression);
        int CountInvoiceItemList(long expenseId, short languageId);
        ISQLQuery FindInvoiceItemByExpenseID(long expenseId, short languageId, bool isCount, string sortExpression);
	}
}

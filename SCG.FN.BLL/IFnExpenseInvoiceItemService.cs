using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;
using SCG.FN.DTO;
using SCG.FN.DTO.ValueObject;

namespace SCG.FN.BLL
{
    public interface IFnExpenseInvoiceItemService : IService<FnExpenseInvoiceItem, long>
    {
        void AddDomesticInvoiceItemOnTransaction(FnExpenseInvoiceItem item, Guid txId);
        void UpdateDomesticInvoiceItemOnTransaction(FnExpenseInvoiceItem item, Guid txId);
        void DeleteItemOnTransaction(Guid txId, long itemId);

        void AddBeginRowInvoiceItemOnTransaction(FnExpenseInvoiceItem item, Guid txId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.DTO.DataSet;

namespace SCG.eAccounting.BLL
{
    public interface IFnExpenseInvoiceItemService : IService<FnExpenseInvoiceItem, long>
    {
		void AddInvoiceItemOnTransaction(FnExpenseInvoiceItem item, Guid txId, string expenseType);
		void UpdateInvoiceItemOnTransaction(FnExpenseInvoiceItem item, Guid txId, string expenseType);
		void UpdateNonDeductAmountInvoiceItem(Guid txID, long invoiceId, double rateNonDeduct);
        void DeleteItemOnTransaction(Guid txId, long itemId);
        //void AddBeginRowInvoiceItemOnTransaction(FnExpenseInvoiceItem item, Guid txId);
		void PrepareDataToDataset(ExpenseDataSet ds, long invoiceId);
        void SavExpenseInvoiceItem(Guid txId, long invoiceId);
		void AddRecommendInvoiceItemOnTransaction(long invoiceId, string expenseType, IList<FnExpenseInvoiceItem> recommendList, Guid txId);
        void AddMileageInvoiceItem(FnExpenseInvoiceItem item, Guid txId);
        void ValidateInvoiceItem(Guid txId, long expenseId);
        void UpdateInvoiceItemByInvoiceID(Guid txID, long invoiceId, FnExpenseDocument expenseDocument);
        double ReCalculateInvoiceItem(Guid txId, long invoiceId, string expenseType);
	}
}

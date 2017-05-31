using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Utilities;

using SCG.FN.DAL;
using SCG.FN.DTO;
using SCG.FN.BLL;
using SCG.FN.DTO.ValueObject;
using System.Data;
using SCG.eAccounting.BLL;
using SCG.FN.DTO.DataSet;

namespace SCG.FN.BLL.Implement
{
    public partial class FnExpenseInvoiceService : ServiceBase<FnExpenseInvoice, long>, IFnExpenseInvoiceService
    {
        public ITransactionService TransactionService { get; set; }

        public override IDao<FnExpenseInvoice, long> GetBaseDao()
        {
            return DaoProvider.FnExpenseInvoiceDao;
        }
        public long AddInvoiceOnTransaction(FnExpenseInvoice invoice, Guid txId)
        {
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpenseInvoiceRow row = ds.FnExpenseInvoice.NewFnExpenseInvoiceRow();

            ds.FnExpenseInvoice.AddFnExpenseInvoiceRow(row);
            
            return row.InvoiceID;
        }
        public long AddBeginRowInvoiceOnTransaction(FnExpenseInvoice invoice, Guid txId)
        {
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpenseInvoiceRow row = ds.FnExpenseInvoice.NewFnExpenseInvoiceRow();

            row.InvoiceType = invoice.InvoiceType;

            row.Active = invoice.Active;
            row.CreBy = invoice.CreBy;
            row.CreDate = invoice.CreDate;
            row.UpdBy = invoice.UpdBy;
            row.UpdDate = invoice.UpdDate;
            row.UpdPgm = invoice.UpdPgm;

            ds.FnExpenseInvoice.AddFnExpenseInvoiceRow(row);

            return row.InvoiceID;
        }
        public void DeleteInvoiceOnTransaction(FnExpenseInvoice invoice, Guid txId)
        {
            ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpenseInvoiceRow row = expDs.FnExpenseInvoice.FindByInvoiceID(invoice.InvoiceID);
            expDs.FnExpenseInvoice.RemoveFnExpenseInvoiceRow(row);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Utilities;

using SCG.eAccounting.DAL;
using SCG.eAccounting.DTO;
using SCG.eAccounting.BLL;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.DTO.DataSet;
using SCG.eAccounting.Query;
using SS.Standard.Security;
using System.Data;

namespace SCG.eAccounting.BLL.Implement
{
    public partial class FnExpenseMileageInvoiceService : ServiceBase<FnExpenseMileageInvoice, long>, IFnExpenseMileageInvoiceService
    {
        public IUserAccount UserAccount { get; set; }
        public ITransactionService TransactionService { get; set; }

        public override IDao<FnExpenseMileageInvoice, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.FnExpenseMileageInvoiceDao;
        }

        public void AddMileageInvoice(Guid txID, long mileageId, long invoiceId)
        {
            ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(txID);
            ExpenseDataSet.FnExpenseMileageInvoiceRow row = expDs.FnExpenseMileageInvoice.NewFnExpenseMileageInvoiceRow();

            row.InvoiceID = invoiceId;
            row.ExpenseMileageID = mileageId;

            row.Active = true;
            row.CreBy = UserAccount.UserID;
            row.CreDate = DateTime.Now;
            row.UpdBy = UserAccount.UserID;
            row.UpdDate = DateTime.Now;
            row.UpdPgm = UserAccount.CurrentProgramCode;

            expDs.FnExpenseMileageInvoice.AddFnExpenseMileageInvoiceRow(row);
        }

        public void DeleteMileageInvoice(Guid txID, long mileageId)
        {
            ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(txID);
            string filter = String.Format("ExpenseMileageID = {0}", mileageId);
            DataRow[] rows = expDs.FnExpenseMileageInvoice.Select(filter);

            foreach (DataRow row in rows)
            {
                row.Delete();
                long invoiceId = row.Field<long>("InvoiceID");
                DataRow invoiceRow = expDs.FnExpenseInvoice.FindByInvoiceID(invoiceId);
                invoiceRow.Delete();
            }
        }

        public void SaveMileageInvoice(Guid txID, long mileageId)
        {
            ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(txID);
            ScgeAccountingDaoProvider.FnExpenseMileageInvoiceDao.Persist(expDs.FnExpenseMileageInvoice);
        }

        public void PrepareDataToDataset(ExpenseDataSet ds, long mileageId)
        {
            IList<FnExpenseMileageInvoice> mileageInvoiceList = ScgeAccountingQueryProvider.FnExpenseMileageInvoiceQuery.GetMileageInvoiceByMileageID(mileageId);

            foreach (FnExpenseMileageInvoice mileageInvoice in mileageInvoiceList)
            {
                // Set data to invoice row in Dataset.
                ExpenseDataSet.FnExpenseMileageInvoiceRow row = ds.FnExpenseMileageInvoice.NewFnExpenseMileageInvoiceRow();

                row.MileageInvoiceID = mileageInvoice.MileageInvoiceID;

                if(mileageInvoice.ExpenseMileage != null)
                row.ExpenseMileageID = mileageInvoice.ExpenseMileage.ExpenseMileageID;

                if (mileageInvoice.Invoice != null)
                    row.MileageInvoiceID = mileageInvoice.Invoice.InvoiceID;

                row.Active = mileageInvoice.Active;
                row.CreBy = mileageInvoice.CreBy;
                row.CreDate = mileageInvoice.CreDate;
                row.UpdBy = mileageInvoice.UpdBy;
                row.UpdDate = mileageInvoice.UpdDate;
                row.UpdPgm = mileageInvoice.UpdPgm;

                // Add mileage invoice row to documentDataset.
                ds.FnExpenseMileageInvoice.AddFnExpenseMileageInvoiceRow(row);
            }
        }
    }
}

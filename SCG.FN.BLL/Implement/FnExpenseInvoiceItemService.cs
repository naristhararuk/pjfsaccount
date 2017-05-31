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
using SCG.FN.DTO.DataSet;
using SCG.eAccounting.BLL;
using System.Web;

namespace SCG.FN.BLL.Implement
{
    public partial class FnExpenseInvoiceItemService : ServiceBase<FnExpenseInvoiceItem, long>, IFnExpenseInvoiceItemService
    {
        public ITransactionService TransactionService { get; set; }
        public override IDao<FnExpenseInvoiceItem, long> GetBaseDao()
        {
            return DaoProvider.FnExpenseInvoiceItemDao;
        }

        public void AddDomesticInvoiceItemOnTransaction(FnExpenseInvoiceItem item, Guid txId)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (item.CostCenter == null)
                errors.AddError("InvoiceItem.Error", new Spring.Validation.ErrorMessage("RequiredCostCenter"));
            if (item.Account == null)
                errors.AddError("InvoiceItem.Error", new Spring.Validation.ErrorMessage("RequiredAccountCode"));
            if (item.IO == null)
                errors.AddError("InvoiceItem.Error", new Spring.Validation.ErrorMessage("RequiredIO"));
            if (!item.Amount.HasValue)
                errors.AddError("InvoiceItem.Error", new Spring.Validation.ErrorMessage("RequiredAmount"));

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpenseInvoiceItemRow row = ds.FnExpenseInvoiceItem.NewFnExpenseInvoiceItemRow();

            //row.ExpenseID = item.Expense.ExpenseID;
            //row.InvoiceID = item.Invoice.InvoiceID;
            row.CostCenterID = item.CostCenter.CostCenterID;
            row.AccountID = item.Account.AccountID;
            row.IOID = item.IO.IOID;

            if (item.Amount.HasValue)
                row.Amount = (decimal)item.Amount.Value;
            //if (item.ExchangeRate.HasValue)
            //    row.ExchangeRate = (decimal)item.ExchangeRate;

            row.Description = item.Description;
            row.ReferenceNo = item.ReferenceNo;
            row.CreBy = item.CreBy;
            row.CreDate = item.CreDate;
            row.UpdBy = item.UpdBy;
            row.UpdDate = item.UpdDate;
            row.UpdPgm = item.UpdPgm;
            row.Active = true;

            ds.FnExpenseInvoiceItem.AddFnExpenseInvoiceItemRow(row);
        }
        public void UpdateDomesticInvoiceItemOnTransaction(FnExpenseInvoiceItem item, Guid txId)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (item.CostCenter == null)
                errors.AddError("InvoiceItem.Error", new Spring.Validation.ErrorMessage("RequiredCostCenter"));
            if (item.Account == null)
                errors.AddError("InvoiceItem.Error", new Spring.Validation.ErrorMessage("RequiredAccountCode"));
            if (item.IO == null)
                errors.AddError("InvoiceItem.Error", new Spring.Validation.ErrorMessage("RequiredIO"));
            if (!item.Amount.HasValue)
                errors.AddError("InvoiceItem.Error", new Spring.Validation.ErrorMessage("RequiredAmount"));

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpenseInvoiceItemRow row = ds.FnExpenseInvoiceItem.FindByInvoiceItemID(item.InvoiceItemID);

            //row.ExpenseID = item.Expense.ExpenseID;
            //row.InvoiceID = item.Invoice.InvoiceID;
            row.CostCenterID = item.CostCenter.CostCenterID;
            row.AccountID = item.Account.AccountID;
            row.IOID = item.IO.IOID;

            if (item.Amount.HasValue)
                row.Amount = (decimal)item.Amount.Value;
            //if (item.ExchangeRate.HasValue)
            //    row.ExchangeRate = (decimal)item.ExchangeRate;

            row.Description = item.Description;
            row.ReferenceNo = item.ReferenceNo;
            row.CreBy = item.CreBy;
            row.CreDate = item.CreDate;
            row.UpdBy = item.UpdBy;
            row.UpdDate = item.UpdDate;
            row.UpdPgm = item.UpdPgm;
            row.Active = true;

            ds.FnExpenseInvoiceItem.AddFnExpenseInvoiceItemRow(row);
        }
        public void DeleteItemOnTransaction(Guid txId, long itemId)
        {
            ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpenseInvoiceItemRow row = expDs.FnExpenseInvoiceItem.FindByInvoiceItemID(itemId);
            expDs.FnExpenseInvoiceItem.RemoveFnExpenseInvoiceItemRow(row);

        }
        public void AddBeginRowInvoiceItemOnTransaction(FnExpenseInvoiceItem item, Guid txId)
        {
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpenseInvoiceItemRow row = ds.FnExpenseInvoiceItem.NewFnExpenseInvoiceItemRow();

            row.InvoiceID = item.Invoice.InvoiceID;

            row.CreBy = item.CreBy;
            row.CreDate = item.CreDate;
            row.UpdBy = item.UpdBy;
            row.UpdDate = item.UpdDate;
            row.UpdPgm = item.UpdPgm;
            row.Active = true;

            ds.FnExpenseInvoiceItem.AddFnExpenseInvoiceItemRow(row);
        }
    }
}

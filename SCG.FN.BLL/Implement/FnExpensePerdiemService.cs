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
    public partial class FnExpensePerdiemService : ServiceBase<FnExpensePerdiem, long>, IFnExpensePerdiemService
    {
        public ITransactionService TransactionService { get; set; }

        public override IDao<FnExpensePerdiem, long> GetBaseDao()
        {
            return DaoProvider.FnExpensePerdiemDao;
        }
        public long AddBeginRowExpensePerdiemOnTransaction(FnExpensePerdiem expensePerdiem, Guid txId)
        {
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpensePerdiemRow row = ds.FnExpensePerdiem.NewFnExpensePerdiemRow();

            ds.FnExpensePerdiem.AddFnExpensePerdiemRow(row);

            return row.ExpensePerdiemID;
        }
        public long UpdateBeginRowExpensePerdiemOnTransaction(FnExpensePerdiem expensePerdiem, Guid txId)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (expensePerdiem.CostCenterID == null)
                errors.AddError("Perdiem.Error", new Spring.Validation.ErrorMessage("RequiredCostCenter"));
            if (expensePerdiem.AccountID == null)
                errors.AddError("Perdiem.Error", new Spring.Validation.ErrorMessage("RequiredAccountID"));
            if (expensePerdiem.Description == null)
                errors.AddError("Perdiem.Error", new Spring.Validation.ErrorMessage("RequiredDescription"));
            if (!expensePerdiem.PerdiemRate.HasValue)
                errors.AddError("Perdiem.Error", new Spring.Validation.ErrorMessage("RequiredPerdiemRate"));
            if (expensePerdiem.PerdiemRate.Equals ((decimal)0))
                errors.AddError("Perdiem.Error", new Spring.Validation.ErrorMessage("RequiredPerdiemRateOverZero"));

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpensePerdiemRow row = ds.FnExpensePerdiem.FindByExpensePerdiemID(expensePerdiem.ExpensePerdiemID);

            row.BeginEdit();
            row.InvoiceID = expensePerdiem.InvoiceID.InvoiceID;
            row.CostCenterID = expensePerdiem.CostCenterID.CostCenterID;
            row.AccountID = expensePerdiem.AccountID.AccountID;

            if (expensePerdiem.IOID != null)
            {
                row.IOID = expensePerdiem.IOID.IOID;
            }            
            
            row.Description = expensePerdiem.Description;
            row.PerdiemRate = expensePerdiem.PerdiemRate.Value;
            row.ReferenceNo = expensePerdiem.ReferenceNo;

            row.CreBy = expensePerdiem.CreBy;
            row.CreDate = expensePerdiem.CreDate;
            row.UpdBy = expensePerdiem.UpdBy;
            row.UpdDate = expensePerdiem.UpdDate;
            row.UpdPgm = expensePerdiem.UpdPgm;
            row.Active = true;
            row.EndEdit();
            row.AcceptChanges();

            return row.ExpensePerdiemID;
        }
    }
}

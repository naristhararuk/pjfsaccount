using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SCG.eAccounting.DAL;
using SCG.eAccounting.DTO;
using SCG.eAccounting.BLL;
using SCG.eAccounting.DTO.DataSet;
using SCG.eAccounting.DTO.ValueObject;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Security;
using SS.Standard.Utilities;
using SCG.eAccounting.Query;

namespace SCG.eAccounting.BLL.Implement
{
    public class FnExpensePerdiemDetailService : ServiceBase<FnExpensePerdiemDetail, long>, IFnExpensePerdiemDetailService
    {
        #region properties
        public ITransactionService TransactionService { get; set; }
        public IFnExpensePerdiemService FnExpensePerdiemService { get; set; }
        public IUserAccount UserAccount { get; set; }
        #endregion

        #region method
        public override IDao<FnExpensePerdiemDetail, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.FnExpensePerdiemDetailDao;
        }
        public void AddExpensePerdiemDetailTransaction(FnExpensePerdiemDetail perdiemDetail, Guid txId)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (perdiemDetail.Description.Length.Equals(0))
                errors.AddError("PerdiemDetail.Error", new Spring.Validation.ErrorMessage("RequiredDescription"));
            if (perdiemDetail.ExchangeRate.Equals((double)0))
                errors.AddError("PerdiemDetail.Error", new Spring.Validation.ErrorMessage("RequiredExchangeRate"));
            if (perdiemDetail.Amount.Equals((double)0))
                errors.AddError("PerdiemDetail.Error", new Spring.Validation.ErrorMessage("RequiredAmount"));            

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpensePerdiemDetailRow row = ds.FnExpensePerdiemDetail.NewFnExpensePerdiemDetailRow();

            row.BeginEdit();
            row.ExpensePerdiemID = perdiemDetail.ExpensePerdiem.ExpensePerdiemID;
            row.Description = perdiemDetail.Description;
            row.CurrencyID = perdiemDetail.CurrencyID;
            row.ExchangeRate = perdiemDetail.ExchangeRate;
            row.Amount = perdiemDetail.Amount;
            row.EndEdit();

            ds.FnExpensePerdiemDetail.AddFnExpensePerdiemDetailRow(row);
            FnExpensePerdiemService.UpdateExpensePerdiemCalculateTransaction(perdiemDetail.ExpensePerdiem.ExpensePerdiemID, txId);
        }    
        public void DeleteExpensePerdiemDetailTransaction(long perdiemDetailId, Guid txId)
        {
            ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpensePerdiemDetailRow row = expDs.FnExpensePerdiemDetail.FindByExpensePerdiemDetailID(perdiemDetailId);
            long expensePerdiemId = row.ExpensePerdiemID;
            row.Delete();
            FnExpensePerdiemService.UpdateExpensePerdiemCalculateTransaction(expensePerdiemId, txId);
        }
        #endregion

        #region Calculate
        public double CalculateAmountTHB(double exchangeRate, double amount)
        {
            return (double)Math.Round((decimal)(exchangeRate * amount), 2, MidpointRounding.AwayFromZero);
        }        
        #endregion    
   
        public void PrepareDataToDataset(ExpenseDataSet ds, long perdiemId)
        {
            IList<FnExpensePerdiemDetail> perdiemDetailList = ScgeAccountingQueryProvider.FnExpensePerdiemDetailQuery.GetPerdiemDetailByPerdiemID(perdiemId);

            foreach (FnExpensePerdiemDetail detail in perdiemDetailList)
            {
                // Set data to perdiem item row in Dataset.
                ExpenseDataSet.FnExpensePerdiemDetailRow itemRow = ds.FnExpensePerdiemDetail.NewFnExpensePerdiemDetailRow();

                itemRow.ExpensePerdiemDetailID = detail.ExpensePerdiemDetailID;

                if (detail.ExpensePerdiem != null)
                    itemRow.ExpensePerdiemID = detail.ExpensePerdiem.ExpensePerdiemID;

                itemRow.Description = detail.Description;
                itemRow.CurrencyID = detail.CurrencyID;
                itemRow.Amount = detail.Amount;
                itemRow.ExchangeRate = detail.ExchangeRate;

                // Add perdiem detail row to documentDataset.
                ds.FnExpensePerdiemDetail.AddFnExpensePerdiemDetailRow(itemRow);
            }
        }
    }
}

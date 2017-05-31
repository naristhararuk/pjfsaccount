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
    public interface IFnExpensePerdiemItemService : IService<FnExpensePerdiemItem , long>
    {
        void AddExpensePerdiemItemTransaction(FnExpensePerdiemItem item, Guid txId);
        void UpdateExpensePerdiemItemTransaction(FnExpensePerdiemItem item, Guid txId);
        void DeleteExpensePerdiemItemTransaction(long itemId, Guid txId);

        #region Calculate
        DateTime ConvertDateTime(DateTime? dd, DateTime? tt);
        double CalculateTotalDay(DateTime fromDateTime, DateTime toDateTime, string perdiemType);
        double CalculateNetDay(double toTalDay, double adjustedDay);
        double CalculateAmount(double netDay, double perdiemRate);
        double CalculateLocalAmount(double netDay, double perdiemRate, double exchangeRateLocalCurrency);
        double CalculateFullDay(double netDay, double halfDay);
        double CalculateTotalFullDay(long expensePerdiemID, Guid txId);
        double CalculateTotalHalfDay(long expensePerdiemID, Guid txId);
        #endregion
       
        void PrepareDataToDataset(ExpenseDataSet ds, long perdiemId);
    }
}

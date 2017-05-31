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
    public interface IFnExpensePerdiemService : IService<FnExpensePerdiem,long>
    {
        long AddExpensePerdiemTransaction(FnExpensePerdiem expensePerdiem, Guid txId);
        void UpdateExpensePerdiemTransaction(FnExpensePerdiem expensePerdiem, long? itemCountryZone, Guid txId);
        void UpdateExpensePerdiemCalculateTransaction(long expensePerdiemID, Guid txId);
        
        void PrepareDataToDataset(ExpenseDataSet ds, long expenseId);
        void SaveExpensePerdiem(Guid txId);
        void ValidationDuplicateDateTimeline(long expensePerdiemID, Guid txId, FnExpensePerdiemItem perdiemItem);
        void ValidationSaveDuplicateDateTimeline(Guid txId, string showError,long expId, bool isCopy);
    }
}

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
    public interface IFnExpensePerdiemDetailService : IService<FnExpensePerdiemDetail, long>
    {
        void AddExpensePerdiemDetailTransaction(FnExpensePerdiemDetail perdiemDetail, Guid txId);
        void DeleteExpensePerdiemDetailTransaction(long perdiemDetailId, Guid txId);

        double CalculateAmountTHB(double exchangeRate, double amount);
        void PrepareDataToDataset(ExpenseDataSet ds, long perdiemId);
    }
}

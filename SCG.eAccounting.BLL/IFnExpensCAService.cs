using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using SCG.eAccounting.DTO;
using System.Data;
using SCG.eAccounting.DTO.DataSet;

namespace SCG.eAccounting.BLL
{
    public interface IFnExpensCAService : IService<FnExpenseCA, long>
    {
        void PrepareDataToDataset(ExpenseDataSet ds, long expenseId);
    }
}

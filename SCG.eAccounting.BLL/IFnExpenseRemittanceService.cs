using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;
using System.Data;
using SCG.eAccounting.DTO.DataSet;

namespace SCG.eAccounting.BLL
{
    public interface IFnExpenseRemittanceService : IService<FnExpenseRemittance, long>
    {
        void SaveExpenseRemittance(Guid txID, long expDocumentID);
        void PrepareDataToDataset(ExpenseDataSet ds, long expenseId);
    }
}

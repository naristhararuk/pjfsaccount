using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Security;
using SS.Standard.Utilities;
using SS.Standard.WorkFlow.Service;
using SS.Standard.WorkFlow.Query;

using SCG.DB.DTO;
using SS.DB.Query;

using SCG.eAccounting.DAL;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.DataSet;
using SCG.eAccounting.BLL;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.Query;

namespace SCG.eAccounting.BLL.Implement
{
    public partial class FnExpenseRemittanceService : ServiceBase<FnExpenseRemittance, long>, IFnExpenseRemittanceService
    {
        public IUserAccount UserAccount { get; set; }
        public ITransactionService TransactionService { get; set; }
        public ISCGDocumentService SCGDocumentService { get; set; }
        public IWorkFlowService WorkFlowService { get; set; }
        public IFnExpenseInvoiceService FnExpenseInvoiceService { get; set; }
        public IFnExpensePerdiemService FnExpensePerdiemService { get; set; }

        public override IDao<FnExpenseRemittance, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.FnExpenseRemittanceDao;
        }

        public void SaveExpenseRemittance(Guid txID, long expDocumentID)
        {
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txID);

            ScgeAccountingDaoProvider.FnExpenseRemittanceDao.Persist(ds.FnExpenseRemittance);
        }
        public void PrepareDataToDataset(ExpenseDataSet ds, long expenseId)
        {
            IList<FnExpenseRemittance> remittedList = ScgeAccountingQueryProvider.FnExpenseRemittanceQuery.GetExpenseRemittanceByExpenseID(expenseId);

            foreach (FnExpenseRemittance remitted in remittedList)
            {
                // Set data to invoice row in Dataset.
                ExpenseDataSet.FnExpenseRemittanceRow expRemittedRow = ds.FnExpenseRemittance.NewFnExpenseRemittanceRow();

                expRemittedRow.ExpenseRemittanceID = remitted.ExpenseRemittanceID;

                if (remitted.Expense != null)
                    expRemittedRow.ExpenseID = remitted.Expense.ExpenseID;

                if (remitted.Remittance != null)
                    expRemittedRow.RemittanceID = remitted.Remittance.RemittanceID;

                expRemittedRow.Active = remitted.Active;
                expRemittedRow.CreBy = remitted.CreBy;
                expRemittedRow.CreDate = remitted.CreDate;
                expRemittedRow.UpdBy = remitted.UpdBy;
                expRemittedRow.UpdDate = remitted.UpdDate;
                expRemittedRow.UpdPgm = remitted.UpdPgm;

                // Add expense remittance row to documentDataset.
                ds.FnExpenseRemittance.AddFnExpenseRemittanceRow(expRemittedRow);
            }
        }
    }
   
}

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
    public partial class FnExpenseAdvanceService : ServiceBase<FnExpenseAdvance, long>, IFnExpenseAdvanceService
    {
        public IUserAccount UserAccount { get; set; }
        public ITransactionService TransactionService { get; set; }
        public ISCGDocumentService SCGDocumentService { get; set; }
        public IWorkFlowService WorkFlowService { get; set; }
        public IFnExpenseInvoiceService FnExpenseInvoiceService { get; set; }
        public IFnExpensePerdiemService FnExpensePerdiemService { get; set; }

        public override IDao<FnExpenseAdvance, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.FnExpenseAdvanceDao;
        }

        public void SaveExpenseAdvance(Guid txID, long expDocumentID)
        {
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txID);

            ScgeAccountingDaoProvider.FnExpenseAdvanceDao.Persist(ds.FnExpenseAdvance);
        }

        public void PrepareDataToDataset(ExpenseDataSet ds, long expenseId)
        {
            IList<FnExpenseAdvance> advList = ScgeAccountingQueryProvider.FnExpenseAdvanceQuery.FindByExpenseDocumentID(expenseId);

            foreach (FnExpenseAdvance adv in advList)
            {
                // Set data to invoice row in Dataset.
                ExpenseDataSet.FnExpenseAdvanceRow expAdvRow = ds.FnExpenseAdvance.NewFnExpenseAdvanceRow();

                expAdvRow.FnExpenseAdvanceID = adv.FnExpenseAdvanceID;

                if (adv.Expense != null)
                    expAdvRow.ExpenseID = adv.Expense.ExpenseID;

                if (adv.Advance != null)
                    expAdvRow.AdvanceID = adv.Advance.AdvanceID;

                expAdvRow.Active = adv.Active;
                expAdvRow.CreBy = adv.CreBy;
                expAdvRow.CreDate = adv.CreDate;
                expAdvRow.UpdBy = adv.UpdBy;
                expAdvRow.UpdDate = adv.UpdDate;
                expAdvRow.UpdPgm = adv.UpdPgm;

                // Add expense remittance row to documentDataset.
                ds.FnExpenseAdvance.AddFnExpenseAdvanceRow(expAdvRow);
            }
        }
    }
   
}

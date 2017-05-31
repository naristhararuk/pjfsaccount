using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.DTO.DataSet;
using SCG.eAccounting.Query;
using SCG.eAccounting.DTO;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Data.NHibernate.Dao;
using SCG.eAccounting.DAL;

namespace SCG.eAccounting.BLL.Implement
{
    public class FnExpensCAService : ServiceBase<FnExpenseCA, long>, IFnExpensCAService
    {
        public override IDao<FnExpenseCA, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.ExpensesCADao;
        }

        public void PrepareDataToDataset(ExpenseDataSet ds, long expenseId)
        {
            IList<FnExpenseCA> ExpCAList = ScgeAccountingQueryProvider.CADocumentQuery.FindByExpenseDocumentID(expenseId);

            foreach (FnExpenseCA expCA in ExpCAList)
            {
                // Set data to invoice row in Dataset.
                ExpenseDataSet.FnExpenseCARow expAdvRow = ds.FnExpenseCA.NewFnExpenseCARow();

                expAdvRow.FnExpenseCAID = expCA.FnExpenseCAID;

                if (expCA.ExpenseID != null)
                    expAdvRow.ExpenseID = expCA.ExpenseID.ExpenseID;

                if (expCA.CADocumentID != null)
                    expAdvRow.CADocumentID = (long)expCA.CADocumentID;

                expAdvRow.Active = expCA.Active;
                expAdvRow.CreBy = expCA.CreBy;
                expAdvRow.CreDate = expCA.CreDate;
                expAdvRow.UpdBy = expCA.UpdBy;
                expAdvRow.UpdDate = expCA.UpdDate;
                expAdvRow.UpdPgm = expCA.UpdPgm;

                // Add expense remittance row to documentDataset.
                ds.FnExpenseCA.AddFnExpenseCARow(expAdvRow);
            }
        }
    }
}

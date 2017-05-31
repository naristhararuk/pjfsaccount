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
    public class FnExpensMPAService : ServiceBase<FnExpenseMPA, long>, IFnExpensMPAService
    {
        public override IDao<FnExpenseMPA, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.ExpensesMPADao;
        }

        public void PrepareDataToDataset(ExpenseDataSet ds, long expenseId)
        {
            IList<FnExpenseMPA> ExpMPAList = ScgeAccountingQueryProvider.FnExpensMPAQuery.FindByExpenseDocumentID(expenseId);

            foreach (FnExpenseMPA expMPA in ExpMPAList)
            {
                // Set data to invoice row in Dataset.
                ExpenseDataSet.FnExpenseMPARow expAdvRow = ds.FnExpenseMPA.NewFnExpenseMPARow();

                expAdvRow.FnExpenseMPAID = expAdvRow.FnExpenseMPAID;

                if (expMPA.ExpenseID != null)
                    expAdvRow.ExpenseID = expMPA.ExpenseID.ExpenseID;

                if (expMPA.MPADocumentID != null)
                    expAdvRow.MPADocumentID = (long)expMPA.MPADocumentID;

                expAdvRow.Active = expMPA.Active;
                expAdvRow.CreBy = expMPA.CreBy;
                expAdvRow.CreDate = expMPA.CreDate;
                expAdvRow.UpdBy = expMPA.UpdBy;
                expAdvRow.UpdDate = expMPA.UpdDate;
                expAdvRow.UpdPgm = expMPA.UpdPgm;

                // Add expense remittance row to documentDataset.
                ds.FnExpenseMPA.AddFnExpenseMPARow(expAdvRow);
            }
        }
    }
}

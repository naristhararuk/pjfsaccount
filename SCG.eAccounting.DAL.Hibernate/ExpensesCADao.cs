using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SCG.eAccounting.DTO.ValueObject;
using System.Data;
using SCG.eAccounting.DTO;

namespace SCG.eAccounting.DAL.Hibernate
{
    public class ExpensesCADao : NHibernateDaoBase<FnExpenseCA, long>, IExpensesCADao
    {
        public long Persist(DataTable dtExpenseCA)
        {
            NHibernateAdapter<FnExpenseCA, long> adapter = new NHibernateAdapter<FnExpenseCA, long>();
            adapter.UpdateChange(dtExpenseCA, ScgeAccountingDaoProvider.ExpensesCADao);

            //return dtExpenseAdvance.Rows[0].Field<long>(dtExpenseAdvance.Columns["FnExpenseAdvanceID"]);
            return 0;
        }
    }
}

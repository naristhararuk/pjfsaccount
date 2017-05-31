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
    public class ExpensesMPADao : NHibernateDaoBase<FnExpenseMPA, long>, IExpensesMPADao
    {
        public long Persist(DataTable dtExpenseMPA)
        {
            NHibernateAdapter<FnExpenseMPA, long> adapter = new NHibernateAdapter<FnExpenseMPA, long>();
            adapter.UpdateChange(dtExpenseMPA, ScgeAccountingDaoProvider.ExpensesMPADao);

            //return dtExpenseAdvance.Rows[0].Field<long>(dtExpenseAdvance.Columns["FnExpenseAdvanceID"]);
            return 0;
        }
    }
}

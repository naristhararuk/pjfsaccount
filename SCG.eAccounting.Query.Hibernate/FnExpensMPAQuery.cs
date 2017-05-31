using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.QueryDao;
using SCG.eAccounting.DTO;

namespace SCG.eAccounting.Query.Hibernate
{
    public class FnExpensMPAQuery : NHibernateQueryBase<FnExpenseMPA, long>, IFnExpensMPAQuery
    {
        public IList<FnExpenseMPA> FindByExpenseDocumentID(long expenseDocumentID)
        {
            return GetCurrentSession().CreateQuery("from FnExpenseMPA where ExpenseID = :ExpenseDocumentID and active = '1'")
                .SetInt64("ExpenseDocumentID", expenseDocumentID)
                .List<FnExpenseMPA>();
        }
    }
}

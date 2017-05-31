using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SCG.eAccounting.DTO;
using SCG.eAccounting.Query;

namespace SCG.eAccounting.Query.Hibernate
{
    public class FnExpenseRemittanceQuery : NHibernateQueryBase<FnExpenseRemittance, long>, IFnExpenseRemittanceQuery
    {
        #region IFnExpenseRemittanceQuery Members

        public IList<FnExpenseRemittance> GetExpenseRemittanceByExpenseID(long expenseId)
        {
            return GetCurrentSession().CreateQuery("from FnExpenseRemittance where ExpenseID = :ExpenseDocumentID and Active = 1")
                .SetInt64("ExpenseDocumentID", expenseId)
                .List<FnExpenseRemittance>();
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;

using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;

namespace SCG.eAccounting.Query
{
    public interface IFnExpenseAdvanceQuery : IQuery<FnExpenseAdvance, long>
    {
        IList<FnExpenseAdvance> FindByExpenseDocumentID(long expenseDocumentID);
        IList<AdvanceData> FindByExpenseDocumentIDForUpdateClearingAdvance(long expenseDocumentID);
        AdvanceData FindExpenseDocumentNoByAdvanceDocumentID(long advanceID);
        IList<FnExpenseAdvance> FindExpenseReferenceAdvanceByAdvanceID(long advanceID);
    }
}

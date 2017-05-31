using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.DTO;
using SS.Standard.Data.Query.NHibernate;
using SCG.eAccounting.DTO.ValueObject;

namespace SCG.eAccounting.Query
{
    public interface ICADocumentQuery : IQuery<CADocument, long>
    {
        CADocument GetCADocumentByDocumentID(long documentID);
        IList<ExpenseCA> GetExpensesCAList(long? CompanyID, long? RequesterID, int startRow, int pageSize, string sortExpression);
        int GetExpensesCACount(long? CompanyID, long? RequesterID);
        IList<ExpenseCA> FindByCADocumentID(long CADocmentID);
        IList<ExpenseCA> FindByExpenseCAID(IList<long> expenseCAIdList);
        IList<FnExpenseCA> FindByExpenseDocumentID(long expenseDocumentID);
    }
}

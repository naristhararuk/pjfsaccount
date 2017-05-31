using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.DTO;
using SS.Standard.Data.Query.NHibernate;
using SCG.eAccounting.DTO.ValueObject;
using NHibernate.Mapping;

namespace SCG.eAccounting.Query
{
    public interface IMPADocumentQuery : IQuery<MPADocument, long>
    {
        MPADocument GetMPADocumentByDocumentID(long documentID);
        IList<ExpensesMPA> GetExpensesMPAList(long? CompanyID, long? RequesterID, long? CurrentUserID, int startRow, int pageSize, string sortExpression);
        int GetExpensesMPACount(long? CompanyID, long? RequesterID, long? CurrentUserID);
        IList<ExpensesMPA> FindByMPADocumentID(long MPADocmentID);
        IList<ExpensesMPA> FindByExpenseMPAID(IList<long> expenseMPAIdList);
    }
}

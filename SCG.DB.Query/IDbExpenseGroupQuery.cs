using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.Query.NHibernate;
using SCG.DB.DTO;
using NHibernate;
using SCG.DB.DTO.ValueObject;

namespace SCG.DB.Query
{
    public interface IDbExpenseGroupQuery : IQuery<DbExpenseGroup, long>
    {
        ISQLQuery FindExpenseGroupByCriteria(VOExpenseGroup criteria, bool isCount, string sortExpression);
        IList<VOExpenseGroup> GetExpenseGroupByCriteria(VOExpenseGroup criteria, int firstResult, int maxResult, string sortExpression);
        int CountExpenseGroupByCriteria(VOExpenseGroup criteria);
    }
}

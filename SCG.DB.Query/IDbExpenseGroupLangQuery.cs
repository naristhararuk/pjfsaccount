using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.DB.DTO;
using SS.DB.DTO.ValueObject;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;
using SCG.DB.DTO.ValueObject;

namespace SCG.DB.Query
{
    public interface IDbExpenseGroupLangQuery : IQuery<DbExpenseGroupLang, long>
    {
        IList<TranslatedListItem> FindExpenseGroupByLangCriteria(short languageId);
        ISQLQuery FindExpenseGroupByCriteria(VOExpenseGroup criteria, short languageId, bool isCount, string sortExpression);
        IList<VOExpenseGroup> GetExpenseGroupList(VOExpenseGroup criteria, short languageId, int firstResult, int maxResult, string sortExpression);
        int GetExpenseGroupCount(VOExpenseGroup criteria, short languageId);
        IList<VOExpenseGroup> FindExpenseGroupAllLanguage(long expenseGroupId);
        IList<VOExpenseGroup> FindExpenseGroupLang(long epLangId);
    }
}

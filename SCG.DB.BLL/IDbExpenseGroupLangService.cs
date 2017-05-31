using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.DB.DTO;
using SS.Standard.Data.NHibernate.Service;

namespace SCG.DB.BLL
{
    public interface IDbExpenseGroupLangService : IService<DbExpenseGroupLang, long>
    {
        long AddExpenseGroupLang(DbExpenseGroupLang expenseGroup);
        void UpdateExpenseGroupLang(IList<DbExpenseGroupLang> expenseGroup);
        void AddExpenseGroupLang(IList<DbExpenseGroupLang> expenseGroup);
    }
}

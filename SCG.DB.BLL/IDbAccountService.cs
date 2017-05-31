using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using SCG.DB.DTO;

namespace SCG.DB.BLL
{
    public interface IDbAccountService : IService<DbAccount, long>
    {
        void AddAccount(DbAccount account);
        void UpdateAccount(DbAccount account);
        long FindAccountId(string accountCode);
        long AddNewAccount(DbAccount dbAccount);
        void DeleteAccount(DbAccount account);
        string GetAccountCodeExpMapping(string accountCode, string expenseGroupType);
    }
}

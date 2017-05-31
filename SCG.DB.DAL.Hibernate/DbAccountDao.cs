using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SCG.DB.DTO;
using SCG.DB.DAL;
using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SCG.DB.DAL.Hibernate
{
    public partial class DbAccountDao : NHibernateDaoBase<DbAccount, long>, IDbAccountDao
    {
        public DbAccountDao()
        {

        }
        public bool IsDuplicateAccountCode(DbAccount account)
        {
            IList<DbAccount> list = GetCurrentSession().CreateQuery("from DbAccount a where a.AccountID <> :AccountID and a.AccountCode = :AccountCode")
                  .SetInt64("AccountID", account.AccountID)
                  .SetString("AccountCode", account.AccountCode)
                  .List<DbAccount>();
            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }
        public long FindAccountId(string accountCode)
        {
            IList<DbAccount> list = GetCurrentSession().CreateQuery("from DbAccount a where a.AccountCode = :AccountCode")
                  .SetString("AccountCode", accountCode)
                  .List<DbAccount>();
            long accountId = -1;
            if (list.Count > 0)
            {
                accountId = list.ElementAt<DbAccount>(0).AccountID;
            }
            return accountId;
        }

        public bool FindByAccountCode(long accountId, string accountCode, long expenseGroupId)
        {
            IList<DbAccount> list = new List<DbAccount>();
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine(" FROM DbAccount ac ");
            sqlBuilder.AppendLine(" WHERE ac.AccountCode = :accountCode and ac.AccountID != :accountId and ac.ExpenseGroupID = :expenseGroupId ");

            IQuery query = GetCurrentSession().CreateQuery(sqlBuilder.ToString());
            query.SetString("accountCode", accountCode);
            query.SetInt64("accountId", accountId);
            query.SetInt64("expenseGroupId", expenseGroupId);

            list = query.List<DbAccount>();
            if (list.Count > 0)
                return true;
            else
                return false;

        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.DB.DTO;
using SS.Standard.Data.NHibernate.Dao;
using NHibernate;

namespace SCG.DB.DAL.Hibernate
{
    public partial class DbExpenseGroupDao : NHibernateDaoBase<DbExpenseGroup, long>, IDbExpenseGroupDao
    {

        public bool FindByExpenseGroupCode(long expGroupId, string expengroupCode)
        {
            IList<DbExpenseGroup> list = new List<DbExpenseGroup>();
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine(" FROM DbExpenseGroup ex ");
            sqlBuilder.AppendLine(" WHERE ex.ExpenseGroupCode = :ExpenseGroupCode and ex.ExpenseGroupID != :expGroupId ");

            IQuery query = GetCurrentSession().CreateQuery(sqlBuilder.ToString());
            query.SetString("ExpenseGroupCode", expengroupCode);
            query.SetInt64("expGroupId", expGroupId);
            list = query.List<DbExpenseGroup>();
            if (list.Count > 0)
                return true;
            else
                return false;
          
        }
    }
}

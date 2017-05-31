using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SCG.DB.DTO;

namespace SCG.DB.DAL
{
    public interface IDbExpenseGroupDao : IDao<DbExpenseGroup, long>
    {
        bool FindByExpenseGroupCode(long expGroupId, string expengroupCode);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.DB.DTO;
using SS.Standard.Data.NHibernate.Dao;
using NHibernate;

namespace SS.DB.DAL
{
    public interface IDbStatusDao : IDao<DbStatus, short>
    {
        bool IsDuplicate(DbStatus status);
        ICriteria FindByDbStatusCriteria(DbStatus status);
        IList<DbStatus> FindByStatusCriteria(DbStatus status);
    }
}

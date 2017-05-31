using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.DB.DTO;
using SS.Standard.Data.NHibernate.Dao;

namespace SCG.DB.DAL
{
    public interface IDbSapInstanceDao : IDao<DbSapInstance, string>  
    {
        bool IsDuplicateCode(DbSapInstance sapinstance);
    }
}

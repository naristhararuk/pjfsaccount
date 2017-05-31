using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.DB.DTO;
using SS.Standard.Data.NHibernate.Service;

namespace SCG.DB.BLL
{
    public interface IDbSapInstanceService : IService<DbSapInstance, string>
    {
        void AddSapInstance(DbSapInstance sapinstance);
        void UpdateSapInstance(DbSapInstance sapinstance);
    }
}

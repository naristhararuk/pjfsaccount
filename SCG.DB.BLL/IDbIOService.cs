using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using SCG.DB.DTO;

namespace SCG.DB.BLL
{
    public interface IDbIOService : IService<DbInternalOrder, long>
    {
        void AddIO(DbInternalOrder io);
        void UpdateIO(DbInternalOrder io);
        void DeleteIO(DbInternalOrder io);
        bool IsExistIO(DbInternalOrder io);
    }
}

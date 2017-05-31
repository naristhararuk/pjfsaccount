using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SCG.DB.DTO;

namespace SCG.DB.DAL
{
    public interface IDbIODao : IDao<DbInternalOrder, long>  
    {
        bool FindByIONumber(long IOID, string IONumber);
        void SyncNewIO();
        void SyncUpdateIO(string iONumber);
        void SyncDeleteIO(string iONumber);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SCG.DB.DTO;

namespace SCG.DB.DAL
{
    public interface IDbLocationDao : IDao<DbLocation, long>
    {
        bool IsDuplicateLocationCode(DbLocation location);
        void SyncNewLocation();
        void SyncUpdateLocation(string locationCode);
        void SyncDeleteLocation(string locationCode);
    }
}

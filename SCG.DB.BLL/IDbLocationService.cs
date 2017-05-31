using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.DB.DTO;

namespace SCG.DB.BLL
{
    public interface IDbLocationService : IService<DbLocation, long>
    {
        void UpdateLocation(DbLocation location);
        void AddLocation(DbLocation location);
        long AddNewLocation(DbLocation location);
        void DeleteLocation(DbLocation location);
        void UpdateLocationToExp(DbLocation location);
    }
}

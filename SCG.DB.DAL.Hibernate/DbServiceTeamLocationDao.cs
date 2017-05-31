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
    public partial class DbServiceTeamLocationDao : NHibernateDaoBase<DbServiceTeamLocation, long>, IDbServiceTeamLocationDao
    {
        public bool IsDuplicateLocationID(DbServiceTeamLocation serviceTeamLocation)
        {
            IList<DbServiceTeamLocation> list = GetCurrentSession().CreateQuery(" from DbServiceTeamLocation as dbService  where dbService.ServiceTeamID.ServiceTeamID = :ServiceTeamID and dbService.LocationID.LocationID = :LocationID ")
                //.SetInt64("ServiceTeamLocationID", serviceTeamLocation.ServiceTeamLocationID)
                  .SetInt64("ServiceTeamID", serviceTeamLocation.ServiceTeamID.ServiceTeamID)
                  .SetInt64("LocationID", serviceTeamLocation.LocationID.LocationID)
                  .List<DbServiceTeamLocation>();

            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }

        public void DeleteServiceTeamLocation(long locationId)
        {
            GetCurrentSession()
                .Delete("from DbServiceTeamLocation sl where sl.LocationID.LocationID= :LocationID "
                , new object[] { locationId }
                , new NHibernate.Type.IType[] { NHibernateUtil.Int64 });
        }
    }
}

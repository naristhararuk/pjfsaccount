using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SCG.DB.DTO;
using SCG.DB.BLL;
using SCG.DB.DAL;
using SS.Standard.Utilities;
using SCG.DB.DTO.ValueObject;
using SCG.DB.Query;

namespace SCG.DB.BLL.Implement
{
    public partial class DbServiceTeamLocationService : ServiceBase<DbServiceTeamLocation, long>, IDbServiceTeamLocationService
    {
        public override IDao<DbServiceTeamLocation, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbServiceTeamLocationDao;
        }
        public void AddServiceTeamLocationList(IList<DbServiceTeamLocation> serviceTeamLocationList)
        {
            foreach (DbServiceTeamLocation serviceTeamLocation in serviceTeamLocationList)
            {
                if (serviceTeamLocation.LocationID != null)
                {
                    IList<DbServiceTeamLocation> list = ScgDbQueryProvider.DbServiceTeamLocationQuery.FindServiceTeamByLocationID(serviceTeamLocation.LocationID.LocationID);
                    if (list.Count > 0)
                        ScgDbDaoProvider.DbServiceTeamLocationDao.DeleteServiceTeamLocation(serviceTeamLocation.LocationID.LocationID);
                }
                AddServiceTeamLocation(serviceTeamLocation);
            }
        }

        private void AddServiceTeamLocation(DbServiceTeamLocation serviceTeamLocation)
        {
            #region Validate

            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (ScgDbDaoProvider.DbServiceTeamLocationDao.IsDuplicateLocationID(serviceTeamLocation))
            {
                errors.AddError("ServiceTeamLocation.Error", new Spring.Validation.ErrorMessage("UniqueServiceTeamLocation_LocationID"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            #endregion

            ScgDbDaoProvider.DbServiceTeamLocationDao.Save(serviceTeamLocation);
        }
    }
}

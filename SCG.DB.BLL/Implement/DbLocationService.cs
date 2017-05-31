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

namespace SCG.DB.BLL.Implement
{
    public partial class DbLocationService : ServiceBase<DbLocation, long>, IDbLocationService
    {
        public override IDao<DbLocation, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbLocationDao;
        }
        public long AddNewLocation(DbLocation location)
        {
            #region Validate DbCompany
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (string.IsNullOrEmpty(location.LocationCode))
            {
                errors.AddError("Location.Error", new Spring.Validation.ErrorMessage("Location Code Required"));
            }
            if (ScgDbDaoProvider.DbLocationDao.IsDuplicateLocationCode(location))
            {
                errors.AddError("Location.Error", new Spring.Validation.ErrorMessage("UniqueLocationCode"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);


            #endregion
           long id= ScgDbDaoProvider.DbLocationDao.Save(location);
           ScgDbDaoProvider.DbLocationDao.SyncNewLocation();
           return id;
        }
        public void AddLocation(DbLocation location)
        {
            #region Validate DbCompany
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (string.IsNullOrEmpty(location.LocationCode))
            {
                errors.AddError("Location.Error", new Spring.Validation.ErrorMessage("Location Code Required"));
            }
            if (ScgDbDaoProvider.DbLocationDao.IsDuplicateLocationCode(location))
            {
                errors.AddError("Location.Error", new Spring.Validation.ErrorMessage("UniqueLocationCode"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);


            #endregion
            ScgDbDaoProvider.DbLocationDao.Save(location);
            ScgDbDaoProvider.DbLocationDao.SyncNewLocation();
        }

        public void UpdateLocation(DbLocation location)
        {
            #region Validate DbCompany
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (string.IsNullOrEmpty(location.LocationCode))
            {
                errors.AddError("Location.Error", new Spring.Validation.ErrorMessage("Location Code Required"));
            }
            if (ScgDbDaoProvider.DbLocationDao.IsDuplicateLocationCode(location))
            {
                errors.AddError("Location.Error", new Spring.Validation.ErrorMessage("UniqueLocationCode"));
            }
           
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);


            #endregion
            ScgDbDaoProvider.DbLocationDao.Update(location);
        }
        public void UpdateLocationToExp(DbLocation location)
        {
            ScgDbDaoProvider.DbLocationDao.SyncUpdateLocation(location.LocationCode);
        }
        public void DeleteLocation(DbLocation location)
        {
            ScgDbDaoProvider.DbLocationDao.Delete(location);
            ScgDbDaoProvider.DbLocationDao.SyncDeleteLocation(location.LocationCode);
        }
        //public short FindCountryId(string countryCode)
        //{
        //    return  ScgDbDaoProvider.DbCountryDao.FindCountryId(countryCode);
        //}
    }
}

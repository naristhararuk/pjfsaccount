using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service.Implement;
using SCG.DB.DTO;
using SS.Standard.Data.NHibernate.Dao;
using SCG.DB.DAL;
using SS.Standard.Utilities;

namespace SCG.DB.BLL.Implement
{
    public partial class DbHolidayProfileService : ServiceBase<DbHolidayProfile, Int32>, IDbHolidayProfileService
    {
        public override IDao<DbHolidayProfile, Int32> GetBaseDao()
        {
            return ScgDbDaoProvider.DbHolidayProfileDao;
        }
        public void AddHolidayProfile(DbHolidayProfile holidayProfile)
        {
            #region Validate DbHolidayProfile
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            
            if (ScgDbDaoProvider.DbHolidayProfileDao.IsDuplicateHolidayProfile(holidayProfile))
            {
                errors.AddError("HolidayProfile.Error", new Spring.Validation.ErrorMessage("UniqueHolidayProfile"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            #endregion

            ScgDbDaoProvider.DbHolidayProfileDao.Save(holidayProfile);
        }
        public void UpdateHolidayProfile(DbHolidayProfile holidayProfile)
        {
            ScgDbDaoProvider.DbHolidayProfileDao.Update(holidayProfile);
        }
        public void Copy(int YearFrom, int YearTo,string type)
        {
            ScgDbDaoProvider.DbHolidayProfileDao.Copy(YearFrom, YearTo, type);
        }
        public bool CheckDay(DateTime checkDay,string type)
        {
            return ScgDbDaoProvider.DbHolidayProfileDao.CheckDay(checkDay, type);
        }
    }
}

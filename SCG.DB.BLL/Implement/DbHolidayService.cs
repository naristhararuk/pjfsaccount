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
    public partial class DbHolidayService : ServiceBase<DbHoliday, Int32>, IDbHolidayService
    {
        public override IDao<DbHoliday, Int32> GetBaseDao()
        {
            return ScgDbDaoProvider.DbHolidayDao;
        }
        public void AddHoliday(DbHoliday holiday)
        {
            #region Validate DbHolidayProfile
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (ScgDbDaoProvider.DbHolidayDao.IsDuplicateHoliday(holiday))
            {
                errors.AddError("Holiday.Error", new Spring.Validation.ErrorMessage("UniqueHoliday"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            #endregion

            ScgDbDaoProvider.DbHolidayDao.Save(holiday);
        }
        public void UpdateHoliday(DbHoliday holiday)
        {
            ScgDbDaoProvider.DbHolidayDao.Update(holiday);
        }
        public void DeleteHolidayProfile(int holidayProfileId)
        {
            ScgDbDaoProvider.DbHolidayDao.DeleteHolidayProfile(holidayProfileId);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SCG.DB.DTO;
using NHibernate;

namespace SCG.DB.DAL.Hibernate
{
    public partial class DbHolidayDao : NHibernateDaoBase<DbHoliday, Int32>, IDbHolidayDao
    {
        public DbHolidayDao()
        {
        }
        public bool IsDuplicateHoliday(DbHoliday holiday)
        {
            IList<DbHoliday> list = GetCurrentSession().CreateQuery("from DbHoliday h where h.HolidayProfileId = :HolidayProfileId AND  h.Date = :Date")
                   .SetInt32("HolidayProfileId", holiday.HolidayProfileId)
                  .SetDateTime("Date", holiday.Date.Date)
                  .List<DbHoliday>();
            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }
        public void DeleteHolidayProfile(int holidayProfileId)
        {
            GetCurrentSession().Delete("from DbHoliday h where h.HolidayProfileId = :HolidayProfileId ", new object[] { holidayProfileId }, new NHibernate.Type.IType[] { NHibernateUtil.Int32 });
        }
    }
}

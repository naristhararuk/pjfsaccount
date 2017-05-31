using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SCG.DB.DTO;

namespace SCG.DB.DAL
{
    public interface IDbHolidayDao : IDao<DbHoliday, Int32>
    {
        bool IsDuplicateHoliday(DbHoliday holiday);
        void DeleteHolidayProfile(int holidayProfileId);
    }
}

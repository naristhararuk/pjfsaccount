using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using SCG.DB.DTO;

namespace SCG.DB.BLL
{
    public interface IDbHolidayService : IService<DbHoliday, Int32>
    {
        void AddHoliday(DbHoliday holiday);
        void UpdateHoliday(DbHoliday holiday);
        void DeleteHolidayProfile(int holidayProfileId);
    }
}

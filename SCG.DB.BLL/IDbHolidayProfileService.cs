using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using SCG.DB.DTO;

namespace SCG.DB.BLL
{
    public interface IDbHolidayProfileService : IService<DbHolidayProfile, Int32>
    {
        void AddHolidayProfile(DbHolidayProfile holidayProfile);
        void UpdateHolidayProfile(DbHolidayProfile holidayProfile);
        void Copy(int YearFrom,int YearTo,string type);
        bool CheckDay(DateTime checkDay,string type);
    }
}

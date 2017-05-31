using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SCG.DB.DTO;

namespace SCG.DB.DAL
{
    public interface IDbHolidayProfileDao : IDao<DbHolidayProfile, Int32>
    {
        bool IsDuplicateHolidayProfile(DbHolidayProfile holidayProfile);
        void Copy(int YearFrom, int YearTo,string type);
        bool CheckDay(DateTime checkDay,string type);
    }
}

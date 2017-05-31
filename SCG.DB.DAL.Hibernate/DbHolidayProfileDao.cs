using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.DB.DTO;
using SS.Standard.Data.NHibernate.Dao;
using NHibernate;

namespace SCG.DB.DAL.Hibernate
{
    public partial class DbHolidayProfileDao : NHibernateDaoBase<DbHolidayProfile, Int32>, IDbHolidayProfileDao
    {
        public DbHolidayProfileDao()
        {
        }
        public bool IsDuplicateHolidayProfile(DbHolidayProfile holidayProfile)
        {
            IList<DbHolidayProfile> list = GetCurrentSession().CreateQuery("from DbHolidayProfile hp where hp.Year = :Year AND hp.Type = :Type ")
                  .SetInt32("Year", holidayProfile.Year)
                  .SetString("Type",holidayProfile.Type)
                  .List<DbHolidayProfile>();
            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }
        public void Copy(int YearFrom, int YearTo,string type)
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery("exec [dbo].[CopyHolidayProfile] :yearFrom , :yearTo , :type ");
            query.SetInt32("yearFrom", YearFrom);
            query.SetInt32("yearTo", YearTo);
            query.SetString("type", type);
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }
        public bool CheckDay(DateTime checkDay,string type)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT count(*) as Count FROM DbHolidayProfile  hp INNER JOIN DbHoliday h ON hp.Id = h.HolidayProfileId where hp.IsApprove = 1 AND hp.Year = :Year AND h.Date = :Date AND hp.Type = :Type");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
              query.SetInt32("Year", checkDay.Year);
              query.SetDateTime("Date", checkDay.Date);
              query.SetString("Type", type);
            query.AddScalar("Count", NHibernateUtil.Int32);
            int count = query.UniqueResult<Int32>();
            if (count > 0 || (checkDay.DayOfWeek == DayOfWeek.Saturday || checkDay.DayOfWeek == DayOfWeek.Sunday))
            {
                return true;
            }
            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.DB.DTO;
using SS.Standard.Data.NHibernate.Query;

namespace SCG.DB.Query
{
    public interface IDbHolidayProfileQuery : IQuery<DbHolidayProfile, Int32>
    {
        IList<DbHolidayProfile> GetHolidayProfile(int? year, int startRow, int pageSize, string sortExpression,string type);
        int CountHolidayProfile(int? year,string type);
        IList<Int32> GetYear(string type);
    }
}

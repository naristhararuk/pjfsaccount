using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Query;
using SCG.DB.DTO;

namespace SCG.DB.Query
{
    public interface IDbHolidayQuery : IQuery<DbHoliday, Int32>
    {
        IList<DbHoliday> GetHoliday(int holidayProfileId, int startRow, int pageSize, string sortExpression);
        int CountHoliday(int holidayProfileId);
    }
}

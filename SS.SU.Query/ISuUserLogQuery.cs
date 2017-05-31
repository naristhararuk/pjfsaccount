using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;

using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using System.Collections;
using SS.SU.DTO;

namespace SS.SU.Query
{
    public interface ISuUserLogQuery : IQuery<SuUserLog, long>
    {
        IList<SuUserLog> FindSuUserLogByUserIdSessionID(string userName , string sessionId);
        object GetUserLogList(string dateFrom, string dateTo, string p, int startRow, int pageSize, string sortExpression);


        ISQLQuery FindSuUserLoginLogSearchResult(DateTime? fromDate, DateTime? toDate, string UserName, string Status, string sortExpression, bool isCount);
        IList<SuUserLoginLog> FindSuUserLoginLogListQuery(DateTime? fromDate, DateTime? toDate, string UserName, string Status, int firstResult, int maxResult, string sortExpression);
        int GetCountUserLoginLoglist(DateTime? fromDate, DateTime? toDate, string UserName, string Status);

        //int GetCountUserLoginLogList(string dateFrom, string dateTo, string p, string p_4);
    }

  
}

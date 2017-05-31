using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;

namespace SS.SU.Query
{
    public interface ISuSmsLogQuery : IQuery<SuSmsLog, long> 
    {
        IList<SuSmsLogSearchResult> GetSmsLogList(DateTime? fromDate, DateTime? toDate, string phoneNo, int firstResult, int maxResult, string sortExpression);
        ISQLQuery FindSuSmsLogSearchResult(DateTime? fromDate, DateTime? toDate, string phoneNo, string sortExpression, bool isCount);
        int GetCountSmsLogList(DateTime? fromDate, DateTime? toDate, string phoneNo);
        SuSmsLog FindBySendMsgSMID(string SMID);
        SuSmsLog FindByID(long smsLogID);
    }
}

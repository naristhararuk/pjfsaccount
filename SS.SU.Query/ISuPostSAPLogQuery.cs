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
    public interface ISuPostSAPLogQuery : IQuery<SuPostSAPLog, long> 
    {
        IList<SuPostSAPLogSearchResult> GetPostSAPLogList(string requestNo, string date, string status, int firstResult, int maxResult, string sortExpression);
        ISQLQuery FindSuPostSAPLogSearchResult(string requestNo, string date, string status, string sortExpression, bool isCount);
        int GetCountPostSAPLogList(string requestNo, string date, string status);
    }
}

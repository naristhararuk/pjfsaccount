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
    public interface ISuImageToSAPLogQuery : IQuery<SuImageTosapLog, long> 
    {
        IList<SuImageToSAPLogSearchResult> GetImageToSAPLogList(string requestNo, DateTime? date, string status, int firstResult, int maxResult, string sortExpression);
        ISQLQuery FindSuImageToSAPLogSearchResult(string requestNo, DateTime? date, string status, string sortExpression, bool isCount);
        int GetCountImageToSAPLogList(string requestNo, DateTime? date, string status);
    }
}

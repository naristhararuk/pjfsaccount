using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Query;
using SS.Standard.WorkFlow.DTO;

using NHibernate;
using SS.Standard.WorkFlow.DTO.ValueObject;

namespace SS.Standard.WorkFlow.Query
{
    public interface IWorkFlowResponseQuery : IQuery<DTO.WorkFlowResponse, long> 
    {
        IList<DTO.WorkFlowResponse> FindByWorkFlowID(long workFlowID);
        ISQLQuery FindWorkFlowResponseByCriteria(long workFlowID, short languageID, bool isCount, string sortExpression);
        IList<WorkFlowSearchResult> GetWorkFlowList(long workFlowID, short languageID, int firstResult, int maxResult, string sortExpression);
        int CountWorkFlowByCriteria(long workFlowID, short languageID);
        WorkFlowResponseSearchResult GetWorkFlowResponseAndEventAndReasonByWFResponseID(long workFlowResponseID, short languageID);
        DateTime? GetApproveVerifyDateTime(long wfid);
        DateTime? GetApproveDateTime(long wfid);
    }
}

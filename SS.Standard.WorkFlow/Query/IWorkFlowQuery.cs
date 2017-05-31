using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Query;
using SS.Standard.WorkFlow.DTO;

using NHibernate;

namespace SS.Standard.WorkFlow.Query
{
    public interface IWorkFlowQuery : IQuery<DTO.WorkFlow, long> 
    {
        Document GetDocumentByWorkFlowID(long workFlowID);
        SS.Standard.WorkFlow.DTO.WorkFlow GetWorkFlowByDocumentID(long documentID);
        IList<SS.Standard.WorkFlow.DTO.WorkFlow> GetAllActiveWorkFlow();
        DTO.WorkFlow FindByIdentityWithUpdateLock(long workFlowID);
    }
}

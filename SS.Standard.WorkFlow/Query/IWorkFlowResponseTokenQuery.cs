using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Query;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.DTO.ValueObject;

using NHibernate;

namespace SS.Standard.WorkFlow.Query
{
    public interface IWorkFlowResponseTokenQuery : IQuery<WorkFlowResponseToken , long>
    {
        IList<WorkFlowResponseToken> FindByTokenCode(string tokenCode);
        WorkFlowResponseToken GetByTokenCode_WorkFlowStateEventID(string tokenCode , int workFlowStateEventID);
        IList<WorkFlowResponseToken> FindByWorkFlowID(long workflowID);
        IList<WorkFlowResponseToken> FindByWorkFlowIDAndType(long workflowID, TokenType tokenType);
    }
}

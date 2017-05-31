using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;
using SS.Standard.WorkFlow.DTO;

namespace SS.Standard.WorkFlow.Service
{
    public interface IWorkFlowResponseTokenService : IService<DTO.WorkFlowResponseToken , long>
    {
        void ClearResponseToken(string tokenCode);
        void ClearResponseTokenByWorkFlowID(long workflowID);
        void ClearResponseTokenByWorkFlowID(long workflowID,TokenType tokenType);
    }
}

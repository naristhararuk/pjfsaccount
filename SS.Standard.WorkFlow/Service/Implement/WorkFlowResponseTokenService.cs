using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SS.Standard.WorkFlow.DAL;
using SS.Standard.WorkFlow.Query;
using SS.Standard.WorkFlow.DTO;

namespace SS.Standard.WorkFlow.Service.Implement
{
    public class WorkFlowResponseTokenService : ServiceBase<DTO.WorkFlowResponseToken , long> , IWorkFlowResponseTokenService
    {
        public override IDao<DTO.WorkFlowResponseToken, long> GetBaseDao()
        {
            return WorkFlowDaoProvider.WorkFlowResponseTokenDao;
        }

        public void ClearResponseToken(string tokenCode)
        {
            IList<DTO.WorkFlowResponseToken> responseTokens = WorkFlowQueryProvider.WorkFlowResponseTokenQuery.FindByTokenCode(tokenCode);
            foreach (DTO.WorkFlowResponseToken responseToken in responseTokens)
            {
                this.Delete(responseToken);
            }
        }

        public void ClearResponseTokenByWorkFlowID(long workflowID)
        {
            IList<DTO.WorkFlowResponseToken> responseTokens = WorkFlowQueryProvider.WorkFlowResponseTokenQuery.FindByWorkFlowID(workflowID);
            foreach (DTO.WorkFlowResponseToken responseToken in responseTokens)
            {
                this.Delete(responseToken);
            }
        }

        public void ClearResponseTokenByWorkFlowID(long workflowID,TokenType tokenType)
        {
            IList<DTO.WorkFlowResponseToken> responseTokens = WorkFlowQueryProvider.WorkFlowResponseTokenQuery.FindByWorkFlowIDAndType(workflowID,tokenType);
            foreach (DTO.WorkFlowResponseToken responseToken in responseTokens)
            {
                this.Delete(responseToken);
            }
        }

    }
}

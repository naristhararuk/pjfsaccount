using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;

using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.DTO.ValueObject;

using NHibernate;
using NHibernate.Transform;
using NHibernate.Expression;

namespace SS.Standard.WorkFlow.Query.Hibernate
{
    public class WorkFlowResponseTokenQuery : NHibernateQueryBase<WorkFlowResponseToken, long>, IWorkFlowResponseTokenQuery
    {
        #region IWorkFlowResponseTokenQuery Members

        public IList<WorkFlowResponseToken> FindByTokenCode(string tokenCode)
        {
            return GetCurrentSession().CreateQuery("from WorkFlowResponseToken where TokenCode = :TokenCode and Active = '1'")
               .SetString("TokenCode", tokenCode)
               .List<WorkFlowResponseToken>();
        }
        
        public WorkFlowResponseToken GetByTokenCode_WorkFlowStateEventID(string tokenCode, int workFlowStateEventID)
        {
            return GetCurrentSession().CreateQuery("from WorkFlowResponseToken where TokenCode = :TokenCode and WorkFlowStateEventID = :WorkFlowStateEventID and Active = '1'")
               .SetString("TokenCode", tokenCode)
               .SetInt32("WorkFlowStateEventID" , workFlowStateEventID)
               .UniqueResult<WorkFlowResponseToken>();
        }
    
        public IList<WorkFlowResponseToken> FindByWorkFlowID(long workflowID)
        {
            return GetCurrentSession().CreateQuery("from WorkFlowResponseToken where WorkFlowID = :WorkFlowID")
               .SetInt64("WorkFlowID", workflowID)
               .List<WorkFlowResponseToken>();
        }

        public IList<WorkFlowResponseToken> FindByWorkFlowIDAndType(long workflowID,TokenType tokenType)
        {
            return GetCurrentSession().CreateQuery("from WorkFlowResponseToken where WorkFlowID = :WorkFlowID AND TokenType = :TokenType")
               .SetInt64("WorkFlowID", workflowID)
               .SetString("TokenType", tokenType.ToString())
               .List<WorkFlowResponseToken>();
        }

        #endregion
    }
}

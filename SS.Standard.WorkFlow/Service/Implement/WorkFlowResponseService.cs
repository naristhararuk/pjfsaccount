using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Context.Support;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SS.Standard.WorkFlow.DAL;
using SS.Standard.WorkFlow.Query;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.DTO.ValueObject;

namespace SS.Standard.WorkFlow.Service.Implement
{
    public class WorkFlowResponseService : ServiceBase<DTO.WorkFlowResponse, long>, IWorkFlowResponseService
    {
        public override IDao<DTO.WorkFlowResponse, long> GetBaseDao()
        {
            return WorkFlowDaoProvider.WorkFlowResponseDao;
        }
        public void ResetActiveResponse(long workflowID, int nextOrdinal)
        {
            WorkFlowDaoProvider.WorkFlowResponseDao.ResetActiveWorkFlowResponse(workflowID, nextOrdinal);
        }
    }
}

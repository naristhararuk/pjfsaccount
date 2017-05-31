using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Spring.Context.Support;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SS.Standard.WorkFlow.DAL;
using SS.Standard.WorkFlow.Query;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.DTO.ValueObject;

namespace SS.Standard.WorkFlow.Service.Implement
{
    public class WorkFlowStateService : ServiceBase<DTO.WorkFlowState, int>, IWorkFlowStateService
    {
        public override IDao<DTO.WorkFlowState, int> GetBaseDao()
        {
            return WorkFlowDaoProvider.WorkFlowStateDao;
        }
    }
}

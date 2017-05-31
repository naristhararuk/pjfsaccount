using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

namespace SS.Standard.WorkFlow.DAL.Hibernate
{
    public class WorkFlowHoldResponseDao : NHibernateDaoBase<DTO.WorkFlowHoldResponse, long>, IWorkFlowHoldResponseDao
    {
    }
}

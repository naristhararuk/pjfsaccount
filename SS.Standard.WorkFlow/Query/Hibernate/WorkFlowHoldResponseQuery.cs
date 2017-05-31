using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;

using SS.Standard.WorkFlow.DTO;

using NHibernate;
using NHibernate.Transform;
using NHibernate.Expression;

namespace SS.Standard.WorkFlow.Query.Hibernate
{
    public class WorkFlowHoldResponseQuery : NHibernateQueryBase<DTO.WorkFlowHoldResponse, long>, IWorkFlowHoldResponseQuery
    {

    }
}

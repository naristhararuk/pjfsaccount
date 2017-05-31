using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.QueryCreator;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.Query;


namespace SS.Standard.WorkFlow.Query.Hibernate
{
    public class WorkFlowsmsTokenQuery : NHibernateQueryBase<WorkFlowsmsToken, long>, IWorkFlowsmsTokenQuery
    {

    }
}

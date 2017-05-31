using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using NHibernate;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.Data.NHibernate.Query;

namespace SS.Standard.WorkFlow.Query
{
    public interface IWorkFlowsmsTokenQuery : IQuery<WorkFlowsmsToken, long>
    {
    }
}

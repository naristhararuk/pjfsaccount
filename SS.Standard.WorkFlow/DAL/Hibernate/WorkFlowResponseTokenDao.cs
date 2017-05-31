using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

namespace SS.Standard.WorkFlow.DAL.Hibernate
{
    public class WorkFlowResponseTokenDao : NHibernateDaoBase<DTO.WorkFlowResponseToken , long>, IWorkFlowResponseTokenDao
    {
    }
}

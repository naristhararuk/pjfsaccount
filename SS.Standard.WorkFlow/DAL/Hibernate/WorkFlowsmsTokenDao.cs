using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SS.Standard.WorkFlow.DTO;

using System.Data;


namespace SS.Standard.WorkFlow.DAL.Hibernate
{
    public partial class WorkFlowsmsTokenDao : NHibernateDaoBase<WorkFlowsmsToken, long>, IWorkFlowsmsTokenDao
    {
        public WorkFlowsmsTokenDao()
        { 
        
        }
    }
}

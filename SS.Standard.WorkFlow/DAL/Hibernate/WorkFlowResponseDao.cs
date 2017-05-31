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
    public class WorkFlowResponseDao : NHibernateDaoBase<DTO.WorkFlowResponse, long>, IWorkFlowResponseDao
    {
        public void ResetActiveWorkFlowResponse(long wfid, int nextOrdinal)
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(@"update WorkFlowResponse set Active = 0 from WorkFlowResponse a 
                inner join WorkFlowStateEvent b on a.WorkFlowStateEventID = b.WorkFlowStateEventID
                inner join WorkFlowState c on b.WorkFlowStateID = c.WorkFlowStateID
                where a.WorkFlowID = :WorkFlowID and a.Active = 1 and c.Ordinal >= :NextOrdinal ");
            query.SetInt64("WorkFlowID", wfid)
                .SetInt32("NextOrdinal", nextOrdinal);
            query.AddScalar("Count", NHibernateUtil.Int32).UniqueResult();
        }
    }
}

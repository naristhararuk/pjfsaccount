using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;

using SS.Standard.WorkFlow.DTO;

using NHibernate;
using NHibernate.Transform;

namespace SS.Standard.WorkFlow.Query.Hibernate
{
    public class WorkFlowStateTransitionQuery : NHibernateQueryBase<WorkFlowStateTransition, int>, IWorkFlowStateTransitionQuery
    {
        #region IWorkFlowStateTransitionQuery Members

        public WorkFlowState GetNextState(int currentStateID, string signal)
        {
            WorkFlowStateTransition transition = GetCurrentSession().CreateQuery("from WorkFlowStateTransition where CurrentStateID = :CurrentStateID and Signal = :Signal and Active='1'")
                .SetInt32("CurrentStateID", currentStateID)
                .SetString("Signal", signal)
                .UniqueResult<WorkFlowStateTransition>();

            if (transition != null)
                return transition.NextState;
            else
                return null;
        }

        #endregion
    }
}

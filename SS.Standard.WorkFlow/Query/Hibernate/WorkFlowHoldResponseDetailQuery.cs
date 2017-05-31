using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;
using SS.Standard.WorkFlow.DTO;

using NHibernate;
using NHibernate.Transform;
using NHibernate.Expression;


namespace SS.Standard.WorkFlow.Query.Hibernate
{
    public class WorkFlowHoldResponseDetailQuery : NHibernateQueryBase<DTO.WorkFlowHoldResponseDetail, long>, IWorkFlowHoldResponseDetailQuery
    {
        #region IWorkFlowHoldResponseDetailQuery Members

        public IList<WorkFlowHoldResponseDetail> GetHoldFields(long workFlowID, int holdStateID)
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine("SELECT wfhrd.* ");
            strQuery.AppendLine("FROM   WorkFlowResponse as wfr ");
            strQuery.AppendLine("INNER JOIN WorkFlowHoldResponse as wfhr on wfr.WorkFlowResponseID = wfhr.WorkFlowResponseID AND wfhr.Active = 1 ");
            strQuery.AppendLine("INNER JOIN WorkFlowHoldResponseDetail as wfhrd on wfhr.WorkFlowHoldResponseID = wfhrd.WorkFlowHoldResponseID AND wfhrd.Active = 1 ");
            strQuery.AppendLine("WHERE	wfr.WorkFlowID = :WorkFlowID ");
            strQuery.AppendLine("AND	wfr.ResponseDate = (SELECT TOP 1 ResponseDate FROM WorkFlowResponse WHERE WorkFlowID = :WorkFlowID AND WorkFlowStateEventID = :WorkFlowStateEventID ORDER BY ResponseDate DESC) ");


            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
            
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("WorkFlowID", typeof(long), workFlowID);
            queryParameterBuilder.AddParameterData("WorkFlowStateEventID", typeof(long), holdStateID);
            queryParameterBuilder.FillParameters(query);

            return query.AddEntity(typeof(WorkFlowHoldResponseDetail)).List<WorkFlowHoldResponseDetail>();
        }

        #endregion
    }
}

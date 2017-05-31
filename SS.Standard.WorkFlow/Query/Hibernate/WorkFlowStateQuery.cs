using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;

using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.DTO.ValueObject;

using NHibernate;
using NHibernate.Transform;
using NHibernate.Expression;
using SS.Standard.Data.NHibernate.QueryCreator;


namespace SS.Standard.WorkFlow.Query.Hibernate
{
    public class WorkFlowStateQuery : NHibernateQueryBase<WorkFlowState, int>, IWorkFlowStateQuery
    {
        public WorkFlowState FindWorkFlowStateIDByTypeIDAndStateName(int workFlowTypeID, string Name)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * ");
            sql.Append("FROM WorkFlowState as w ");
            //sql.Append("LEFT JOIN WorkFlowStateLang as wl on wl.WorkFlowStateID = w.WorkFlowStateID ");
            //sql.Append("Where wl.LanguageID =:LanguageID AND w.WorkFlowTypeID =:WorkFlowTypeID AND wl.DisplayName =:DisplayName ");
            sql.Append("Where w.WorkFlowTypeID =:WorkFlowTypeID AND w.Name =:Name ");

            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());

            //queryParameterBuilder.AddParameterData("LanguageID",typeof(long),languageID);
            queryParameterBuilder.AddParameterData("WorkFlowTypeID",typeof(int),workFlowTypeID);
            queryParameterBuilder.AddParameterData("Name",typeof(string),Name);

            queryParameterBuilder.FillParameters(query);

            
            return query.AddEntity(typeof(WorkFlowState)).UniqueResult<WorkFlowState>();
        }

        public WorkFlowState GetCurrentWorkFlowStateByWorkFolwID(long workFlowID)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT WT.* ");
            sql.Append("FROM WorkFlow as W , WorkFlowState as WT ,  ");
            sql.Append("Where ");
            sql.Append("    W.CurrentState  = WT.WorkFlowStateID AND ");
            sql.Append("    W.WorkFlowID    = :workFlowID ");

            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            queryParameterBuilder.AddParameterData("workFlowID", typeof(long), workFlowID);
            queryParameterBuilder.FillParameters(query);

            return query.AddEntity(typeof(WorkFlowState)).UniqueResult<WorkFlowState>();
        }

    }
}

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
using SCG.DB.DTO.ValueObject;
using SS.Standard.Data.NHibernate.QueryCreator;


namespace SS.Standard.WorkFlow.Query.Hibernate
{
    public class WorkFlowStateEventQuery : NHibernateQueryBase<WorkFlowStateEvent, int>, IWorkFlowStateEventQuery
    {
        #region IWorkFlowStateEventQuery Members

        public IList<WorkFlowStateEvent> FindByWorkFlowStateEventByStateID(int workFlowStateID)
        {
            return GetCurrentSession().CreateQuery("from WorkFlowStateEvent where WorkFlowStateID = :WorkFlowStateID and active = '1'")
                .SetInt32("WorkFlowStateID", workFlowStateID)
                .List<WorkFlowStateEvent>();
        }


        public IList<WorkFlowStateEventWithLang> FindByWorkFlowStateEventByStateID(int workFlowStateID, short languageID)
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" SELECT wse.WorkFlowStateEventID as EventID , wse.Name , wsel.DisplayName , wse.UserControlPath ");
            strQuery.AppendLine(" FROM WorkFlowState ws ");
            strQuery.AppendLine(" INNER JOIN WorkFlowStateEvent wse ");
            strQuery.AppendLine(" ON ws.WorkFlowStateID = wse.WorkFlowStateID ");
            strQuery.AppendLine(" INNER JOIN WorkFlowStateEventLang wsel ");
            strQuery.AppendLine(" ON wse.WorkFlowStateEventID = wsel.WorkFlowStateEventID AND wsel.LanguageID = :LanguageID ");
            strQuery.AppendLine(" WHERE ws.WorkFlowStateID = :WorkFlowStateID");
            strQuery.AppendLine(" AND ws.Active = '1'");
            strQuery.AppendLine(" AND wse.Active = '1'");
            strQuery.AppendLine(" ORDER BY ws.Name ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
            query.SetInt16("LanguageID", languageID);
            query.SetInt32("WorkFlowStateID", workFlowStateID);
            query.AddScalar("EventID", NHibernateUtil.Int64);
            query.AddScalar("Name", NHibernateUtil.String);
            query.AddScalar("DisplayName", NHibernateUtil.String);
            query.AddScalar("UserControlPath", NHibernateUtil.String);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(WorkFlowStateEventWithLang)));
            return query.List<WorkFlowStateEventWithLang>();
        }

        public WorkFlowStateEvent GetByWorkFlowStateID_EventName(int workFlowStateID, string eventName)
        {
            return GetCurrentSession().CreateQuery("from WorkFlowStateEvent where WorkFlowStateID = :WorkFlowStateID and Name = :EventName and active = '1'")
                .SetInt32("WorkFlowStateID", workFlowStateID)
                .SetString("EventName", eventName)
                .UniqueResult<WorkFlowStateEvent>();
        }

        public WorkFlowStateEvent GetSendDraftStateEvent(int workFlowTypeID)
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" select workflowstateevent.* ");
            strQuery.AppendLine(" from workflowstateevent  ");
            strQuery.AppendLine(" inner join workflowstate ");
            strQuery.AppendLine(" on workflowstate.workflowstateid = workflowstateevent.workflowstateid ");
            strQuery.AppendLine(" where workflowstate.workflowtypeid = :workFlowTypeID ");
            strQuery.AppendLine(" and workflowstate.name = 'Draft' ");
            strQuery.AppendLine(" and workflowstateevent.name = 'Send' ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
            query.SetInt32("workFlowTypeID", workFlowTypeID);
            query.AddEntity(typeof(WorkFlowStateEvent));
            IList<WorkFlowStateEvent> list = query.List<WorkFlowStateEvent>();
            if (list != null)
                return list[0];
            else
                return null;
        }
        #endregion

        public IList<WorkFlowStateEvent> FindWorkFlowStateEvent(string WorkFlowStateName, string WorkFlowStateEventName, int WorkFlowTypeID)
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" SELECT t2.WorkFlowStateEventID AS WorkFlowStateEventID ");
            strQuery.AppendLine(" FROM WorkFlowState t1 ");
            strQuery.AppendLine(" INNER JOIN WorkFlowStateEvent t2 ");
            strQuery.AppendLine(" ON t1.WorkFlowStateID = t2.WorkFlowStateID            ");
            strQuery.AppendLine(" WHERE t1.[Name]           = :WorkFlowStateEventName        ");
            strQuery.AppendLine(" AND   t2.[Name]           = :WorkFlowStateName   ");
            strQuery.AppendLine(" AND   t1.WorkFlowTypeID   = :WorkFlowTypeID           ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
            query.SetString("WorkFlowStateName",        WorkFlowStateName);
            query.SetString("WorkFlowStateEventName",   WorkFlowStateEventName);
            query.SetInt32("WorkFlowTypeID", WorkFlowTypeID);

            query.AddScalar("WorkFlowStateEventID", NHibernateUtil.Int32);

            query.SetResultTransformer(Transformers.AliasToBean(typeof(WorkFlowStateEvent)));
            return query.List<WorkFlowStateEvent>();
        }

        public IList<VORejectReasonLang> FindRejectEventAndReason(short languageId,int documentTypeID)
        {
            StringBuilder strQuery = new StringBuilder();
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            strQuery.AppendLine("select wfse.workflowstateeventid as WorkFlowStateEventID, wfsl.displayname + ' - ' + wfsel.displayname as StateEventID from WorkFlowStateEvent wfse ");
            strQuery.AppendLine("inner join WorkflowState wfs ");
            strQuery.AppendLine("on wfse.WorkFlowstateid = wfs.workflowstateid ");
            strQuery.AppendLine("inner join workflowstatelang wfsl ");
            strQuery.AppendLine("on wfs.workflowstateid = wfsl.workflowstateid ");
            strQuery.AppendLine("and wfsl.languageid =:LanguageID ");
            strQuery.AppendLine("inner join workflowstateeventlang wfsel ");
            strQuery.AppendLine("on wfse.workflowstateeventid = wfsel.workflowstateeventid ");
            strQuery.AppendLine("and wfsel.languageid =:LanguageID ");
        	strQuery.AppendLine("inner join WorkFlowTypeDocumentType a ");
            strQuery.AppendLine("on a.WorkFlowTypeID = wfs.WorkFlowTypeID ");
            //add state wait appove for add reason of approve state
            //strQuery.AppendLine("where wfse.name = 'Reject' ");
            strQuery.AppendLine("where (wfse.name = 'Reject' ");
            strQuery.AppendLine("or  (wfse.name = 'approve' and wfs.name = 'WaitApprove')) "); 
            strQuery.AppendLine(" and a.DocumentTypeID = :DocumentTypeID ");
            strQuery.AppendLine("order by StateEventID ");
                

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
            
            parameterBuilder.AddParameterData("LanguageID", typeof(int), languageId);
            parameterBuilder.AddParameterData("DocumentTypeID", typeof(int), documentTypeID);
            parameterBuilder.FillParameters(query);

            query.AddScalar("WorkFlowStateEventID", NHibernateUtil.Int32)
                    .AddScalar("StateEventID", NHibernateUtil.String);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(VORejectReasonLang))).List<VORejectReasonLang>();
        }

        class LanguageTranslation
        {
            public string Local { get; set; }
            public string Translate { get; set; }
        }

        public string GetTranslatedEventName(int workFlowStateEventID, short languageID)
        {
            string querystring = @"
                select t1.name as Local, t2.displayName as Translate 
                from workflowstateevent t1 
                    left join workflowstateeventlang t2 
                        on t1.workflowstateeventid = t2.workflowstateeventid
                            and t2.active = 1
                where t1.workflowstateeventid = :WorkflowStateEventID
                    and t2.languageid = :LanguageID";


            ISQLQuery query = GetCurrentSession().CreateSQLQuery(querystring);

            query.SetParameter("WorkflowStateEventID", workFlowStateEventID);
            query.SetParameter("LanguageID", languageID);
            query.AddScalar("Local", NHibernateUtil.String);
            query.AddScalar("Translate", NHibernateUtil.String);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(LanguageTranslation)));
            LanguageTranslation t =
                    query.UniqueResult<LanguageTranslation>();

            return String.IsNullOrEmpty(t.Translate) ? t.Local : t.Translate;
        }
    }
}

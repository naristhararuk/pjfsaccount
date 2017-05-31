using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;

using SS.Standard.WorkFlow.DTO;

using NHibernate;
using NHibernate.Transform;
using NHibernate.Expression;
using SS.Standard.Data.NHibernate.QueryCreator;
using SS.Standard.WorkFlow.DTO.ValueObject;

namespace SS.Standard.WorkFlow.Query.Hibernate
{
    public class WorkFlowResponseQuery : NHibernateQueryBase<DTO.WorkFlowResponse, long>, IWorkFlowResponseQuery
    {

        #region IWorkFlowResponseQuery Members

        public IList<WorkFlowResponse> FindByWorkFlowID(long workFlowID)
        {
            IQuery query = GetCurrentSession().CreateQuery("from WorkFlowResponse workFlowResponse where workFlowResponse.WorkFlow.WorkFlowID = :WorkFlowID and workFlowResponse.Active = '1'");
            query.SetLockMode("workFlowResponse", LockMode.None);

            return query.SetInt64("WorkFlowID", workFlowID)
                .List<WorkFlowResponse>();
        }

        #endregion

        public ISQLQuery FindWorkFlowResponseByCriteria(long workFlowID,short languageID, bool isCount, string sortExpression)
        {
            StringBuilder sqlBuilder        = new StringBuilder();
            StringBuilder sqlBuilderDetail  = new StringBuilder();

            sqlBuilderDetail.Append(" from ( SELECT wr.WorkFlowID AS WorkFlowID, wr.WorkFlowStateEventID , wr.ResponseDate AS ResponseDate , u.EmployeeName AS Name , wfsel.DisplayName AS Response, ");
            sqlBuilderDetail.Append("Case When (isnull(wr.ResponseMethod,'') = '') or (wr.ResponseMethod = '0') Then 'Web'  When wr.ResponseMethod = '1' Then 'Email' When wr.ResponseMethod = '2' Then 'SMS' When wr.ResponseMethod = '3' Then 'SAP' else '' end as ResponseMethod , ");
            sqlBuilderDetail.Append("(isnull(wr.Remark,'') +' '+ isnull(rl.ReasonDetail,'') +' '+ isnull(wfrr.Remark,'') + ' ' + isnull(wfhr.Remark,'')) AS Description, wfvr.AmountBeforeVerify, wfvr.AmountVerified ");
            sqlBuilderDetail.Append("FROM WorkFlowResponse  AS wr with(nolock) ");
            sqlBuilderDetail.Append("INNER JOIN SuUser AS u with(nolock) ON wr.ResponseBy = u.UserID ");
            sqlBuilderDetail.Append("INNER JOIN WorkFlowStateEvent AS wfse with(nolock) ON wr.WorkFlowStateEventID = wfse.WorkFlowStateEventID ");
            sqlBuilderDetail.Append("INNER JOIN WorkFlowStateEventLang AS wfsel with(nolock) ON wfsel.WorkFlowStateEventID = wfse.WorkFlowStateEventID AND wfsel.LanguageID =:LanguageID ");
            sqlBuilderDetail.Append("LEFT OUTER JOIN WorkFlowRejectResponse AS wfrr with(nolock) ON wfrr.WorkFlowResponseID = wr.WorkFlowResponseID ");
            sqlBuilderDetail.Append("LEFT OUTER JOIN WorkFlowHoldResponse AS wfhr with(nolock) ON wfhr.WorkFlowResponseID = wr.WorkFlowResponseID ");
            sqlBuilderDetail.Append("LEFT JOIN DbRejectReason AS r with(nolock)  ON r.ReasonID = wfrr.ReasonID ");
            sqlBuilderDetail.Append("LEFT JOIN DbRejectReasonLang AS rl with(nolock) ON rl.ReasonID = r.ReasonID AND rl.LanguageID =:LanguageID ");
            sqlBuilderDetail.Append("LEFT JOIN WorkFlowVerifyResponse AS wfvr with(nolock) ON wfvr.WorkFlowResponseID = wr.WorkFlowResponseID ");
            sqlBuilderDetail.Append("WHERE wr.WorkFlowID =:WorkFlowID ");
            sqlBuilderDetail.Append(") t "); 

            if (!isCount)
            {
                sqlBuilder.Append(" SELECT WorkFlowID , WorkFlowStateEventID, ResponseDate , Name , Response ,ResponseMethod, Description, AmountBeforeVerify, AmountVerified ");
                sqlBuilder.Append(sqlBuilderDetail.ToString());

                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine("ORDER BY ResponseDate desc "); 
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));
            }
            else
            {
                sqlBuilder.Append("SELECT  COUNT(WorkFlowID) AS CountWorkFlowResponse ");
                sqlBuilder.Append(sqlBuilderDetail.ToString());
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("WorkFlowID",typeof(long), workFlowID);
            queryParameterBuilder.AddParameterData("LanguageID", typeof(short),languageID);
            queryParameterBuilder.FillParameters(query);

            if (!isCount)
            {
                query.AddScalar("WorkFlowID", NHibernateUtil.Int64);
                query.AddScalar("WorkFlowStateEventID", NHibernateUtil.Int64);
                query.AddScalar("ResponseDate", NHibernateUtil.DateTime);
                query.AddScalar("Name", NHibernateUtil.String);
                query.AddScalar("Response", NHibernateUtil.String);
                query.AddScalar("Description", NHibernateUtil.String);
                query.AddScalar("ResponseMethod", NHibernateUtil.String);
                query.AddScalar("AmountBeforeVerify", NHibernateUtil.Double);
                query.AddScalar("AmountVerified", NHibernateUtil.Double);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(WorkFlowSearchResult)));
            }
            else
            {
                query.AddScalar("CountWorkFlowResponse", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            return query;
        }

        public IList<WorkFlowSearchResult> GetWorkFlowList(long workFlowID,short languageID, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<WorkFlowSearchResult>(WorkFlowQueryProvider.WorkFlowResponseQuery, "FindWorkFlowResponseByCriteria", new object[] { workFlowID,languageID, false, sortExpression }, firstResult, maxResult, sortExpression);
        }

        public int CountWorkFlowByCriteria(long workFlowID, short languageID)
        {
            return NHibernateQueryHelper.CountByCriteria(WorkFlowQueryProvider.WorkFlowResponseQuery, "FindWorkFlowResponseByCriteria", new object[] { workFlowID,languageID, true, string.Empty });
        }

        public WorkFlowResponseSearchResult GetWorkFlowResponseAndEventAndReasonByWFResponseID(long workFlowResponseID,short languageID)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select b.remark AS RemarkReject,f.remark AS RemarkHold,c.reasondetail AS ReasonName,d.displayname AS ResponseEventName,s.EmployeeName AS ResponseBy ,doc.DocumentID as DocumentID, we.Name AS EventName ");
            sql.Append(" from workflowresponse a ");
            sql.Append(" left outer join workflowrejectresponse b on a.workflowresponseid = b.workflowresponseid ");
            sql.Append(" left outer join WorkFlowHoldResponse f on a.workflowresponseid = f.workflowresponseid ");
            sql.Append(" left join workFlow w on w.workFlowID = a.WorkFlowID ");
            sql.Append(" left join document doc on doc.documentID = w.documentID ");
            sql.Append(" left join Suuser s on a.ResponseBy = s.UserID ");
            sql.Append(" left outer join dbrejectreasonlang c on c.reasonid = b.reasonid and c.languageid =:LanguageID ");
            sql.Append(" left outer join workflowstateeventlang d on d.workflowstateeventid = a.workflowstateeventid and d.languageid =:LanguageID ");
            sql.Append(" left outer join workflowstateevent we on we.workflowstateeventid = a.workflowstateeventid ");
            sql.Append("where a.workflowresponseid =:WorkFlowResponseID ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("WorkFlowResponseID", typeof(long), workFlowResponseID);
            queryParameterBuilder.AddParameterData("LanguageID", typeof(short), languageID);
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("DocumentID", NHibernateUtil.Int64);
            query.AddScalar("ResponseEventName",NHibernateUtil.String);
            //query.AddScalar("Remark",NHibernateUtil.String);
            query.AddScalar("RemarkReject", NHibernateUtil.String);
            query.AddScalar("RemarkHold", NHibernateUtil.String);
            query.AddScalar("ReasonName",NHibernateUtil.String);
            query.AddScalar("ResponseBy", NHibernateUtil.String);
            query.AddScalar("EventName", NHibernateUtil.String);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(WorkFlowResponseSearchResult))).UniqueResult <WorkFlowResponseSearchResult>();
        }

        public DateTime? GetApproveVerifyDateTime(long wfid)
        {
            string sql = @"select max(wfr.ResponseDate) as ResponseDate from  workflowresponse wfr
                inner join WorkflowStateEvent wfse on wfse.WorkflowStateEventID = wfr.WorkflowStateEventID and wfse.Name = 'Approve'
                inner join WorkflowState ws on ws.WorkflowStateID = wfse.WorkflowStateID and ws.WorkflowStateID = 31
                where wfr.WorkflowID = :WorkFlowID AND wfr.Active = 1";

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql);
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("WorkFlowID", typeof(long), wfid);
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("ResponseDate", NHibernateUtil.DateTime);

            object approveVerifyDate = query.UniqueResult();
            if (approveVerifyDate != null)
            {
                return (DateTime?)approveVerifyDate;
            }
            return null;
        }

        public DateTime? GetApproveDateTime(long wfid)
        {
            string sql = @"select max(wfr.ResponseDate) as ResponseDate from  workflowresponse wfr
                inner join WorkflowStateEvent wfse on wfse.WorkflowStateEventID = wfr.WorkflowStateEventID and wfse.Name = 'Approve'
                inner join WorkflowState ws on ws.WorkflowStateID = wfse.WorkflowStateID and ws.WorkflowStateID = 29
                where wfr.WorkflowID = :WorkFlowID AND wfr.Active = 1";

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql);
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("WorkFlowID", typeof(long), wfid);
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("ResponseDate", NHibernateUtil.DateTime);

            object approveVerifyDate = query.UniqueResult();
            if (approveVerifyDate != null)
            {
                return (DateTime?)approveVerifyDate;
            }
            return null;
        }

    }
}

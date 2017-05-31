using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SCG.DB.DTO;
using SCG.DB.Query;
using SCG.DB.DTO.ValueObject;

namespace SCG.DB.Query.Hibernate
{
    public class DbRejectReasonQuery : NHibernateQueryBase<DbRejectReason, int>, IDbRejectReasonQuery
    {
        public IList<VORejectReasonLang> FindRejectReasonByDocTypeIDStateIDAndLanguageID(int documentTypeID, int WorkflowStateID, short languageID)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT r.ReasonID AS ReasonID , r.ReasonCode AS ReasonCode , rl.ReasonDetail AS ReasonDetail ");
            sql.AppendLine("FROM DbRejectReason AS r ");
            sql.AppendLine("INNER JOIN DbRejectReasonLang AS rl ON rl.ReasonID = r.ReasonID ");
            sql.AppendLine("WHERE ISNULL(r.DocumentTypeID, :documentTypeID) = :documentTypeID AND rl.LanguageID = :languageID AND r.Active = 1 ");
            sql.AppendLine("AND (r.WorkFlowStateEventID IS NULL OR r.WorkFlowStateEventID IN (SELECT WorkFlowStateEventID FROM WorkFlowStateEvent WHERE WorkFlowStateEvent.WorkflowStateID = :WorkflowStateID)) ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());

            QueryParameterBuilder queryParameter = new QueryParameterBuilder();
            queryParameter.AddParameterData("documentTypeID", typeof(int), documentTypeID);
            queryParameter.AddParameterData("WorkflowStateID", typeof(int), WorkflowStateID);
            queryParameter.AddParameterData("languageID", typeof(int), languageID);
            queryParameter.FillParameters(query);

            query.AddScalar("ReasonID", NHibernateUtil.Int32);
            query.AddScalar("ReasonCode", NHibernateUtil.String);
            query.AddScalar("ReasonDetail", NHibernateUtil.String);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(VORejectReasonLang))).List<VORejectReasonLang>();
        }


        public IList<VORejectReasonLang> FindRejectReasonByDocTypeIDStateEventIDAndLanguageID(int documentTypeID, int workflowStateEventID, short languageID)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT r.ReasonID AS ReasonID , r.ReasonCode AS ReasonCode , rl.ReasonDetail AS ReasonDetail ");
            sql.AppendLine("FROM DbRejectReason AS r ");
            sql.AppendLine("INNER JOIN DbRejectReasonLang AS rl ON rl.ReasonID = r.ReasonID ");
            sql.AppendLine("WHERE ISNULL(r.DocumentTypeID, :documentTypeID) = :documentTypeID AND rl.LanguageID = :languageID AND r.Active = 1 ");
            sql.AppendLine("AND (r.WorkFlowStateEventID IS NULL OR r.WorkFlowStateEventID = :WorkflowStateEventID) order by ReasonCode asc");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());

            QueryParameterBuilder queryParameter = new QueryParameterBuilder();
            queryParameter.AddParameterData("documentTypeID", typeof(int), documentTypeID);
            queryParameter.AddParameterData("WorkflowStateEventID", typeof(int), workflowStateEventID);
            queryParameter.AddParameterData("languageID", typeof(int), languageID);
            queryParameter.FillParameters(query);

            query.AddScalar("ReasonID", NHibernateUtil.Int32);
            query.AddScalar("ReasonCode", NHibernateUtil.String);
            query.AddScalar("ReasonDetail", NHibernateUtil.String);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(VORejectReasonLang))).List<VORejectReasonLang>();
        }
    
    }

}

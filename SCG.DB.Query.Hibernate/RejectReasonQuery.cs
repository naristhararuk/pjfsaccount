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
    public class RejectReasonQuery : NHibernateQueryBase<DbRejectReason, int>, IRejectReasonQuery
    {
        public DbRejectReason FindByCode(string code)
        {
            return GetCurrentSession()
                .CreateCriteria(typeof(DbRejectReason))
                .Add(NHibernate.Expression.Expression.Eq("ReasonCode", code))
                .UniqueResult<DbRejectReason>();
        }


        public ISQLQuery FindReasonByDocumentTypeLanguageId(DbRejectReason dbRejectReason, short languageId, bool isCount, string sortExpression)
        {
            StringBuilder strQuery = new StringBuilder();
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();

            ISQLQuery query;

            if (isCount)
            {
                strQuery.Append("select count(*) as Count ");
                strQuery.Append("from DbRejectReason r ");
                strQuery.Append("left join DocumentType d on d.DocumentTypeID = r.DocumentTypeID ");
                strQuery.Append("left join DbRejectReasonLang rl on r.ReasonID = rl.ReasonID and rl.LanguageID =:LanguageID ");
                strQuery.Append("left join WorkFlowStateEvent wfse On wfse.WorkFlowStateEventID = r.WorkFlowStateEventID ");
                strQuery.Append("left join WorkflowState wfs ");
                strQuery.Append("on wfse.WorkFlowstateid = wfs.workflowstateid ");
                strQuery.Append("left join workflowstatelang wfsl ");
                strQuery.Append("on wfs.workflowstateid = wfsl.workflowstateid ");
                strQuery.Append("and wfsl.languageid =:LanguageID ");
                strQuery.Append("left join workflowstateeventlang wfsel ");
                strQuery.Append("on wfse.workflowstateeventid = wfsel.workflowstateeventid ");
                strQuery.Append("and wfsel.languageid =:LanguageID ");
                strQuery.Append("Where 1=1 ");

                parameterBuilder.AddParameterData("LanguageID", typeof(short), languageId);
                if (!string.IsNullOrEmpty(dbRejectReason.ReasonCode))
                {
                    strQuery.Append("AND r.ReasonCode Like :ReasonCode ");
                    parameterBuilder.AddParameterData("ReasonCode", typeof(string), string.Format("%{0}%", dbRejectReason.ReasonCode));
                }
                if (dbRejectReason.DocumentTypeID != 0)
                {
                    strQuery.Append("AND r.DocumentTypeID =:DocumentTypeID ");
                    parameterBuilder.AddParameterData("DocumentTypeID", typeof(int), dbRejectReason.DocumentTypeID);
                }
                if (dbRejectReason.WorkFlowStateEventID != 0)
                {
                    strQuery.Append("AND r.WorkFlowStateEventID =:WorkFlowStateEventID ");
                    parameterBuilder.AddParameterData("WorkFlowStateEventID", typeof(int), dbRejectReason.WorkFlowStateEventID);
                }

                query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
                parameterBuilder.FillParameters(query);
                query.AddScalar("Count", NHibernateUtil.Int32);
            }
            else
            {
                strQuery.Append("select  r.ReasonID as ReasonID, r.ReasonCode as ReasonCode, d.DocumentTypeName as DocumentTypeCode, wfsl.displayname + ' - ' + wfsel.displayname AS StateEventID ,case when r.RequireComment is null then 'false' else r.RequireComment end As RequireComment ,case when r.RequireConfirmReject is null then 'false' else r.RequireConfirmReject end AS RequireConfirmReject, r.Active as Active ");
                strQuery.Append("from DbRejectReason r ");
                strQuery.Append("left join DocumentType d on d.DocumentTypeID = r.DocumentTypeID ");
                strQuery.Append("left join DbRejectReasonLang rl on r.ReasonID = rl.ReasonID and rl.LanguageID =:LanguageID ");
                strQuery.Append("left join WorkFlowStateEvent wfse On wfse.WorkFlowStateEventID = r.WorkFlowStateEventID ");
                strQuery.Append("left join WorkflowState wfs ");
                strQuery.Append("on wfse.WorkFlowstateid = wfs.workflowstateid ");
                strQuery.Append("left join workflowstatelang wfsl ");
                strQuery.Append("on wfs.workflowstateid = wfsl.workflowstateid ");
                strQuery.Append("and wfsl.languageid =:LanguageID ");
                strQuery.Append("left join workflowstateeventlang wfsel ");
                strQuery.Append("on wfse.workflowstateeventid = wfsel.workflowstateeventid ");
                strQuery.Append("and wfsel.languageid =:LanguageID ");
                strQuery.Append("Where 1=1 ");

                parameterBuilder.AddParameterData("LanguageID", typeof(short), languageId);
                if (!string.IsNullOrEmpty(dbRejectReason.ReasonCode))
                {
                    strQuery.Append("AND r.ReasonCode Like :ReasonCode ");
                    parameterBuilder.AddParameterData("ReasonCode", typeof(string), string.Format("%{0}%", dbRejectReason.ReasonCode));
                }
                if (dbRejectReason.DocumentTypeID != 0)
                {
                    strQuery.Append("AND r.DocumentTypeID =:DocumentTypeID ");
                    parameterBuilder.AddParameterData("DocumentTypeID", typeof(int), dbRejectReason.DocumentTypeID);
                }
                if (dbRejectReason.WorkFlowStateEventID != 0)
                {
                    strQuery.Append("AND r.WorkFlowStateEventID =:WorkFlowStateEventID ");
                    parameterBuilder.AddParameterData("WorkFlowStateEventID", typeof(int), dbRejectReason.WorkFlowStateEventID);
                }
                if (string.IsNullOrEmpty(sortExpression))
                {
                    strQuery.Append(" Order by r.ReasonID, r.ReasonCode ");
                }
                else
                {
                    strQuery.Append(String.Format(" order by {0} ", sortExpression));
                }

                query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
                parameterBuilder.FillParameters(query);
                query.AddScalar("ReasonID", NHibernateUtil.Int32)
                    .AddScalar("ReasonCode", NHibernateUtil.String)
                    .AddScalar("DocumentTypeCode", NHibernateUtil.String)
                    .AddScalar("StateEventID", NHibernateUtil.String)
                    .AddScalar("RequireComment", NHibernateUtil.Boolean)
                    .AddScalar("RequireConfirmReject", NHibernateUtil.Boolean)
                    .AddScalar("Active", NHibernateUtil.Boolean);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(VORejectReasonLang)));
            }

            return query;
        }
        public IList<VORejectReasonLang> GetRejectReasonList(DbRejectReason dbRejectReason, short languageId, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<VORejectReasonLang>(
                ScgDbQueryProvider.RejectReasonQuery, "FindReasonByDocumentTypeLanguageId",
                new object[] { dbRejectReason, languageId, false, sortExpression }
                , firstResult, maxResult, sortExpression);
        }
        public int GetRejectReasonCount(DbRejectReason dbRejectReason, short languageId)
        {
            return NHibernateQueryHelper.CountByCriteria(
                ScgDbQueryProvider.RejectReasonQuery,
                "FindReasonByDocumentTypeLanguageId",
                new object[] { dbRejectReason, languageId, true, string.Empty });
        }
    }
}

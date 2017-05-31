using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;

using SS.Standard.WorkFlow.DTO;

using NHibernate;
using NHibernate.Transform;
using NHibernate.Expression;

namespace SS.Standard.WorkFlow.Query.Hibernate
{
    public class WorkFlowQuery : NHibernateQueryBase<DTO.WorkFlow, long>, IWorkFlowQuery
    {
        #region IDocumentQuery Members

        public Document GetDocumentByWorkFlowID(long workFlowID)
        {
            DTO.WorkFlow workFlow = FindByIdentity(workFlowID);
            //Console.WriteLine(workFlow.Document.DocumentType.ToString());
            return workFlow.Document;
        }

        public SS.Standard.WorkFlow.DTO.WorkFlow GetWorkFlowByDocumentID(long documentID)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.AppendLine(" FROM WorkFlow wf ");
            queryBuilder.AppendLine(" WHERE wf.Document.DocumentID = :documentID ");
            queryBuilder.AppendLine(" AND Active = '1' ");

            IList<WorkFlow.DTO.WorkFlow> list = GetCurrentSession().CreateQuery(queryBuilder.ToString()).SetInt64("documentID", documentID).List<WorkFlow.DTO.WorkFlow>();

            if (list.Count > 0)
            {
                return list[0];
            }
            else
            {
                return null;
            }
        }

        public IList<DTO.WorkFlow> GetAllActiveWorkFlow()
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine("SELECT wf.*");
            strQuery.AppendLine("FROM   WorkFlow wf ");
            strQuery.AppendLine("INNER JOIN WorkFlowState wfs ON wf.CurrentState = wfs.WorkFlowStateID AND wf.WorkFlowTypeID = wfs.WorkFlowTypeID ");
            strQuery.AppendLine("WHERE  wfs.Name NOT IN ('Complete' , 'Cancel') ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());

            query.AddEntity(typeof(DTO.WorkFlow));
            return query.List<DTO.WorkFlow>();
        }

        public DTO.WorkFlow FindByIdentityWithUpdateLock(long workFlowID)
        {
            DTO.WorkFlow workflow = FindByIdentity(workFlowID);
            GetCurrentSession().Refresh(workflow, LockMode.Upgrade);

            return workflow;
        }
        #endregion
    }
}

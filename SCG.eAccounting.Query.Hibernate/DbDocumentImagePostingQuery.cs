using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.QueryDao;
using SCG.eAccounting.DTO;
using NHibernate;
using NHibernate.Expression;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SCG.eAccounting.Query.Hibernate
{
    public class DbDocumentImagePostingQuery : NHibernateQueryBase<DbDocumentImagePosting,long>,IDbDocumentImagePostingQuery
    {
        

        #region IDbDocumentImagePostingQuery Members

        public IList<DbDocumentImagePosting> GetDocumentImagePostingByStatusCode(string status)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(DbDocumentImagePosting), "dip");
            criteria.Add(Expression.Eq("dip.Status", status.ToString()));
            return criteria.List<DbDocumentImagePosting>();
        }

        public DbDocumentImagePosting GetDocumentImagePostingByImageID(string ImageID)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(DbDocumentImagePosting), "dip");
            criteria.Add(Expression.Eq("dip.ImageDocID", ImageID));
            return criteria.UniqueResult<DbDocumentImagePosting>();
        }
        public long GetWorkflowIdByDocumentID(long documentID)
        {
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("select wf.workflowid ");
            sqlBuilder.Append("from [document] doc ");
            sqlBuilder.Append("inner join workflow wf ");
            sqlBuilder.Append("on wf.documentid = doc.documentid ");
            sqlBuilder.Append("where doc.documentid = :documentID ");

            QueryParameterBuilder paramBuilder = new QueryParameterBuilder();
            paramBuilder.AddParameterData("documentID", typeof(long),documentID);
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            paramBuilder.FillParameters(query);
            return query.UniqueResult<long>();

        }
        #endregion
    }
}

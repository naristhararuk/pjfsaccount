using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.QueryDao;
using SCG.eAccounting.DTO;
using SS.Standard.Data.NHibernate.QueryCreator;
using SCG.eAccounting.DTO.ValueObject;
using NHibernate;
using NHibernate.Transform;
using NHibernate.Expression;

namespace SCG.eAccounting.Query.Hibernate
{
    public class CADocumentQuery : NHibernateQueryBase<CADocument, long>, ICADocumentQuery
    {
        #region public CADocument GetCADocumentByDocumentID(long documentID)
        public CADocument GetCADocumentByDocumentID(long documentID)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(CADocument), "t");
            criteria.Add(Expression.Eq("t.DocumentID.DocumentID", documentID));

            return criteria.UniqueResult<CADocument>();
        }
        #endregion public CADocument GetCADocumentByDocumentID(long documentID)

        public IList<ExpenseCA> GetExpensesCAList(long? CompanyID, long? RequesterID , int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<ExpenseCA>(ScgeAccountingQueryProvider.CADocumentQuery, "GetExpenseCAListQuery", new object[] { CompanyID, RequesterID, false, sortExpression }, firstResult, maxResult, sortExpression);
        }

        public int GetExpensesCACount(long? CompanyID, long? RequesterID)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgeAccountingQueryProvider.CADocumentQuery, "GetExpenseCAListQuery", new object[] { CompanyID, RequesterID, true, string.Empty });
        }

        public ISQLQuery GetExpenseCAListQuery(long? CompanyID, long? RequesterID, bool isCount, string sortExpression)
        {
            StringBuilder sql = new StringBuilder();
            if (!isCount)
            {
                sql.AppendLine("Select doc.DocumentNo, doc.Subject, doc.DocumentDate, caDoc.CADocumentID ");
            }
            else
            {
                sql.AppendLine(" SELECT COUNT(*) ExpensesCount ");
            }
            sql.AppendLine(" from Document AS doc ");
            sql.AppendLine(" LEFT JOIN CADocument AS caDoc ");
            sql.AppendLine(" ON doc.DocumentID = caDoc.DocumentID ");

            sql.AppendLine(" WHERE doc.CompanyID = :CompanyID ");
            sql.AppendLine(" AND doc.RequesterID = :RequesterID ");
            sql.AppendLine(" AND GETDATE() Between caDoc.StartDate AND caDoc.EndDate ");
            sql.AppendLine(" AND doc.CacheCurrentStateName = 'Complete'");

            if (!isCount)
            {
                if (string.IsNullOrEmpty(sortExpression))
                    sql.AppendLine(" ORDER BY doc.DocumentNo");
                else
                    sql.AppendLine(string.Format(" ORDER BY {0}", sortExpression));
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.SetParameter("CompanyID", Convert.ToString(CompanyID));
            query.SetParameter("RequesterID", Convert.ToString(RequesterID));


            if (!isCount)
            {
                QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
                queryParameterBuilder.FillParameters(query);
                query.AddScalar("DocumentNo", NHibernateUtil.String);
                query.AddScalar("Subject", NHibernateUtil.String);
                query.AddScalar("DocumentDate", NHibernateUtil.Date);
                query.AddScalar("CADocumentID", NHibernateUtil.Int64);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(ExpenseCA))).List<ExpenseCA>();
            }
            else
            {
                query.AddScalar("ExpensesCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            return query;
        }

        public IList<ExpenseCA> FindByCADocumentID(long CADocmentID)
        {
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.AppendLine("Select distinct doc.DocumentNo, doc.Subject, doc.DocumentDate, caDoc.CADocumentID ");
            sqlBuilder.AppendLine(" from Document AS doc ");
            sqlBuilder.AppendLine(" LEFT JOIN CADocument AS caDoc ");
            sqlBuilder.AppendLine(" ON doc.DocumentID = caDoc.DocumentID ");

            sqlBuilder.AppendLine(" WHERE caDoc.CADocumentID = :CADocmentID ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            //query.SetParameterList("MPADocmentID", Convert.ToString(MPADocmentID));
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("CADocmentID", typeof(long), CADocmentID);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("DocumentNo", NHibernateUtil.String);
            query.AddScalar("Subject", NHibernateUtil.String);
            query.AddScalar("DocumentDate", NHibernateUtil.Date);
            query.AddScalar("CADocumentID", NHibernateUtil.Int64);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(ExpenseCA))).List<ExpenseCA>();
        }

        public IList<ExpenseCA> FindByExpenseCAID(IList<long> expensesCAIdList)
        {
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.AppendLine("Select distinct doc.DocumentNo, doc.Subject, doc.DocumentDate, caDoc.CADocumentID , w.WorkflowID ");
            sqlBuilder.AppendLine(" from Document AS doc ");
            sqlBuilder.AppendLine(" LEFT JOIN CADocument AS caDoc ");
            sqlBuilder.AppendLine(" ON doc.DocumentID = caDoc.DocumentID ");
            sqlBuilder.AppendLine(" LEFT JOIN Workflow w ON w.DocumentID = doc.DocumentID ");
            sqlBuilder.AppendLine(" WHERE caDoc.CADocumentID IN (:expensesCAIdList) ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.SetParameterList("expensesCAIdList", expensesCAIdList);
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("DocumentNo", NHibernateUtil.String);
            query.AddScalar("Subject", NHibernateUtil.String);
            query.AddScalar("DocumentDate", NHibernateUtil.Date);
            query.AddScalar("CADocumentID", NHibernateUtil.Int64);
            query.AddScalar("WorkflowID", NHibernateUtil.Int64);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(ExpenseCA))).List<ExpenseCA>();
        }

        public IList<FnExpenseCA> FindByExpenseDocumentID(long expenseDocumentID)
        {
            return GetCurrentSession().CreateQuery("from FnExpenseCA where ExpenseID = :ExpenseDocumentID and active = '1'")
                .SetInt64("ExpenseDocumentID", expenseDocumentID)
                .List<FnExpenseCA>();
        }
    }
}

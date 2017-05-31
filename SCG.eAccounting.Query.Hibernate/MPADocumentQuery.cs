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
using SCG.eAccounting.DTO.DataSet;

namespace SCG.eAccounting.Query.Hibernate
{
    public class MPADocumentQuery : NHibernateQueryBase<MPADocument, long>, IMPADocumentQuery
    {
        #region public MPADocument GetMPADocumentByDocumentID(long documentID)
        public MPADocument GetMPADocumentByDocumentID(long documentID)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(MPADocument), "t");
            criteria.Add(Expression.Eq("t.DocumentID.DocumentID", documentID));

            return criteria.UniqueResult<MPADocument>();
        }
        #endregion public MPADocument GetMPADocumentByDocumentID(long documentID)

        public IList<ExpensesMPA> GetExpensesMPAList(long? CompanyID, long? RequesterID, long? CurrentUserID, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<ExpensesMPA>(ScgeAccountingQueryProvider.MPADocumentQuery, "GetExpensesMPAListQuery", new object[] { CompanyID, RequesterID, CurrentUserID,false, sortExpression }, firstResult, maxResult, sortExpression);
        }

        public int GetExpensesMPACount(long? CompanyID, long? RequesterID, long? CurrentUserID)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgeAccountingQueryProvider.MPADocumentQuery, "GetExpensesMPAListQuery", new object[] { CompanyID, RequesterID, CurrentUserID, true, string.Empty });
        }

        public ISQLQuery GetExpensesMPAListQuery(long? CompanyID, long? RequesterID, long? CurrentUserID, bool isCount, string sortExpression)
        {
            StringBuilder sql = new StringBuilder();
            if (!isCount)
            {
                sql.AppendLine("Select doc.DocumentNo, doc.Subject, doc.DocumentDate, mpaDoc.MPADocumentID ");
            }
            else {
                sql.AppendLine(" SELECT COUNT(*) ExpensesCount ");
            }
            sql.AppendLine(" from Document AS doc ");
            sql.AppendLine(" LEFT JOIN MPADocument AS mpaDoc ");
            sql.AppendLine(" ON doc.DocumentID = mpaDoc.DocumentID ");
            sql.AppendLine(" LEFT JOIN MPAItem AS mpaItem ");
            sql.AppendLine(" ON mpaItem.MPADocumentID = mpaDoc.MPADocumentID ");

            sql.AppendLine(" WHERE doc.CompanyID = :CompanyID ");
            sql.AppendLine(" AND mpaItem.UserID = :RequesterID ");
            sql.AppendLine(" AND GETDATE() Between mpaDoc.StartDate AND mpaDoc.EndDate ");
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
                query.AddScalar("MPADocumentID", NHibernateUtil.Int64);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(ExpensesMPA))).List<ExpensesMPA>();
            }
            else
            {
                query.AddScalar("ExpensesCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            return query;
        }

        public IList<ExpensesMPA> FindByMPADocumentID(long MPADocmentID)
        {
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.AppendLine("Select distinct doc.DocumentNo, doc.Subject, doc.DocumentDate, mpaDoc.MPADocumentID ");
            sqlBuilder.AppendLine(" from Document AS doc ");
            sqlBuilder.AppendLine(" LEFT JOIN MPADocument AS mpaDoc ");
            sqlBuilder.AppendLine(" ON doc.DocumentID = mpaDoc.DocumentID ");
            sqlBuilder.AppendLine(" LEFT JOIN MPAItem AS mpaItem ");
            sqlBuilder.AppendLine(" ON mpaItem.MPADocumentID = mpaDoc.MPADocumentID ");

            sqlBuilder.AppendLine(" WHERE mpaDoc.MPADocumentID = :MPADocmentID ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            //query.SetParameterList("MPADocmentID", Convert.ToString(MPADocmentID));
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("MPADocmentID", typeof(long), MPADocmentID);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("DocumentNo", NHibernateUtil.String);
            query.AddScalar("Subject", NHibernateUtil.String);
            query.AddScalar("DocumentDate", NHibernateUtil.Date);
            query.AddScalar("MPADocumentID", NHibernateUtil.Int64);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(ExpensesMPA))).List<ExpensesMPA>();
        }

        public IList<ExpensesMPA> FindByExpenseMPAID(IList<long> expensesMPAIdList)
        {
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.AppendLine("Select distinct doc.DocumentNo, doc.Subject, doc.DocumentDate, mpaDoc.MPADocumentID , w.WorkflowID ");
            sqlBuilder.AppendLine(" from Document AS doc ");
            sqlBuilder.AppendLine(" LEFT JOIN MPADocument AS mpaDoc ");
            sqlBuilder.AppendLine(" ON doc.DocumentID = mpaDoc.DocumentID ");
            sqlBuilder.AppendLine(" LEFT JOIN MPAItem AS mpaItem ");
            sqlBuilder.AppendLine(" ON mpaItem.MPADocumentID = mpaDoc.MPADocumentID ");
            sqlBuilder.AppendLine(" LEFT JOIN Workflow w ON w.DocumentID = doc.DocumentID ");
            sqlBuilder.AppendLine(" WHERE mpaItem.MPADocumentID IN (:expensesMPAIdList) ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.SetParameterList("expensesMPAIdList", expensesMPAIdList);
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("DocumentNo", NHibernateUtil.String);
            query.AddScalar("Subject", NHibernateUtil.String);
            query.AddScalar("DocumentDate", NHibernateUtil.Date);
            query.AddScalar("MPADocumentID", NHibernateUtil.Int64);
            query.AddScalar("WorkflowID", NHibernateUtil.Int64);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(ExpensesMPA))).List<ExpensesMPA>();
        }
    }
}

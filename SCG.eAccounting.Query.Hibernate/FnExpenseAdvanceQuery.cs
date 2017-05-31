using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.Query;

namespace SCG.eAccounting.Query.Hibernate
{
    public class FnExpenseAdvanceQuery : NHibernateQueryBase<FnExpenseAdvance, long>, IFnExpenseAdvanceQuery
    {
        #region IFnExpenseAdvanceQuery Members

        public IList<FnExpenseAdvance> FindByExpenseDocumentID(long expenseDocumentID)
        {
            return GetCurrentSession().CreateQuery("from FnExpenseAdvance where ExpenseID = :ExpenseDocumentID and active = '1'")
                .SetInt64("ExpenseDocumentID", expenseDocumentID)
                .List<FnExpenseAdvance>();
        }

        public IList<AdvanceData> FindByExpenseDocumentIDForUpdateClearingAdvance(long expenseDocumentID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT     f.AdvanceID, [Document].DocumentDate AS RequestDateOfAdvance, f.UpdBy, f.UpdPgm AS ProgramCode ");
            sqlBuilder.Append(" FROM         FnExpenseAdvance AS f INNER JOIN ");
            sqlBuilder.Append("           AvAdvanceDocument ON f.AdvanceID = AvAdvanceDocument.AdvanceID INNER JOIN ");
            sqlBuilder.Append("           [Document] ON AvAdvanceDocument.DocumentID = [Document].DocumentID ");
            sqlBuilder.Append(" WHERE f.ExpenseID = :ExpenseDocumentID AND f.Active = '1' ");
            sqlBuilder.Append(" ORDER BY [Document].DocumentDate ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.SetParameter("ExpenseDocumentID", expenseDocumentID);
            query.AddScalar("AdvanceID", NHibernateUtil.Int64);
            // Use RequestDateOfAdvance to store document date
            query.AddScalar("RequestDateOfAdvance", NHibernateUtil.DateTime);
            query.AddScalar("UpdBy", NHibernateUtil.Int64);
            query.AddScalar("ProgramCode", NHibernateUtil.String);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(AdvanceData))).List<AdvanceData>();
        }

        public AdvanceData FindExpenseDocumentNoByAdvanceDocumentID(long advanceID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine(" SELECT  doc.DocumentNo, wf.WorkFlowID AS AdvanceID");
            sqlBuilder.AppendLine(" FROM    FnExpenseAdvance AS expAv INNER JOIN ");
            sqlBuilder.AppendLine("                       FnExpenseDocument AS exp ON expAv.ExpenseID = exp.ExpenseID ");
            sqlBuilder.AppendLine("                       INNER JOIN [Document] AS doc ON exp.DocumentID = doc.DocumentID ");
            sqlBuilder.AppendLine("                       INNER JOIN WorkFlow AS wf ON wf.DocumentID = doc.DocumentID ");
            sqlBuilder.AppendLine(" INNER JOIN workflowstate wfs on wf.CurrentState = wfs.WorkflowStateID and wfs.Name <>'Cancel' ");
            sqlBuilder.AppendLine(" WHERE     (expAv.AdvanceID = :advanceID) ");

            //add by tom 01/10/2009, clearing advance from Expense and Remittance
            sqlBuilder.AppendLine(" UNION ALL ");
            sqlBuilder.AppendLine(" SELECT  doc.DocumentNo, wf.WorkFlowID AS AdvanceID ");
            sqlBuilder.AppendLine(" FROM FnRemittanceAdvance rv ");
            sqlBuilder.AppendLine(" INNER JOIN FnRemittance rd ");
            sqlBuilder.AppendLine(" ON rv.RemittanceID = rd.RemittanceID ");
            sqlBuilder.AppendLine(" INNER JOIN Document doc ");
            sqlBuilder.AppendLine(" ON doc.DocumentID = rd.DocumentID ");
            sqlBuilder.AppendLine(" INNER JOIN WorkFlow AS wf ON wf.DocumentID = doc.DocumentID ");
            sqlBuilder.AppendLine(" INNER JOIN workflowstate wfs on wf.CurrentState = wfs.WorkflowStateID and wfs.Name <>'Cancel' ");
            sqlBuilder.AppendLine(" WHERE rv.AdvanceID = :advanceID ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.SetParameter("advanceID", advanceID);
            query.AddScalar("DocumentNo", NHibernateUtil.String);
            query.AddScalar("AdvanceID", NHibernateUtil.Int64);

            query.SetResultTransformer(Transformers.AliasToBean(typeof(AdvanceData)));
            IList<AdvanceData> advance = query.List<AdvanceData>();

            if (advance != null && advance.Count > 0)
            {
                return advance[0];
            }
            else
            {
                return null;
            }

            //return query.SetResultTransformer(Transformers.AliasToBean(typeof(AdvanceData))).List<AdvanceData>()[0];
        }

        public IList<FnExpenseAdvance> FindExpenseReferenceAdvanceByAdvanceID(long advanceID)
        {
            return GetCurrentSession().CreateQuery(" from FnExpenseAdvance where AdvanceID = :AdvanceID and Active=1 ")
                .SetInt64("AdvanceID", advanceID)
                .List<FnExpenseAdvance>();
        }
        #endregion
    }
}

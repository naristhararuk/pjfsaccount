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
using SCG.eAccounting.Query;
using SCG.eAccounting.DTO.ValueObject;
using SS.Standard.WorkFlow.DTO;

namespace SCG.eAccounting.Query.Hibernate
{
    public class FnExpensePerdiemQuery : NHibernateQueryBase<FnExpensePerdiem, long>, IFnExpensePerdiemQuery
	{
        public IList<FnExpensePerdiem> GetPerdiemByInvoiceID(long invoiceId)
        {
            return GetCurrentSession().CreateQuery(" from FnExpensePerdiem p where p.Invoice.InvoiceID = :InvoiceID")
                .SetInt64("InvoiceID", invoiceId)
                .List<FnExpensePerdiem>();
        }

        public IList<FnExpensePerdiem> GetPerdiemByExpenseID(long expenseId)
        {
            return GetCurrentSession().CreateQuery(" from FnExpensePerdiem p where p.Expense.ExpenseID = :ExpenseID")
                .SetInt64("ExpenseID", expenseId)
                .List<FnExpensePerdiem>();
        }

        public Document CheckDateLength(long RequesterId, DateTime date, long expenseId)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select a.DocumentNo , a.DocumentID from Document a ");
            sqlBuilder.Append(" inner join [dbo].[FnExpenseDocument] b on a.DocumentID = b.DocumentID ");
            sqlBuilder.Append(" inner join [dbo].[FnExpensePerdiem] c on b.ExpenseID = c.ExpenseID ");
            sqlBuilder.Append(" inner join [dbo].[FnExpensePerdiemItem] d on c.ExpensePerdiemID = d.ExpensePerdiemID ");
            sqlBuilder.Append(" where a.CacheCurrentStateName <> 'Cancel' and a.RequesterID = :RequesterID ");
            sqlBuilder.Append(" AND (d.FromTime <= :date AND d.ToTime > :date) and b.ExpenseID <> :expenseId ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("RequesterID", typeof(long),RequesterId);
            queryParameterBuilder.AddParameterData("date", typeof(DateTime), date);
            queryParameterBuilder.AddParameterData("expenseId", typeof(long), expenseId);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("DocumentNo", NHibernateUtil.String);
            query.AddScalar("DocumentID", NHibernateUtil.Int64);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(Document))).List<Document>().FirstOrDefault();
        }

        public ValidatePrediem GetPerdiemItemDate(long RequesterId, DateTime date, long expenseId)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select Top(1)d.ToTime as ToDate , d.FromTime as FromDate,a.DocumentNo from Document a ");
            sqlBuilder.Append(" inner join [dbo].[FnExpenseDocument] b on a.DocumentID = b.DocumentID ");
            sqlBuilder.Append(" inner join [dbo].[FnExpensePerdiem] c on b.ExpenseID = c.ExpenseID ");
            sqlBuilder.Append(" inner join [dbo].[FnExpensePerdiemItem] d on c.ExpensePerdiemID = d.ExpensePerdiemID ");
            sqlBuilder.Append(" where a.CacheCurrentStateName <> 'Cancel' and a.RequesterID = :RequesterID and d.FromTime > :date and b.ExpenseID <> :expenseId ");
            sqlBuilder.Append(" Order By d.FromTime ASC");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("RequesterID", typeof(long), RequesterId);
            queryParameterBuilder.AddParameterData("date", typeof(DateTime), date);
            queryParameterBuilder.AddParameterData("expenseId", typeof(long), expenseId);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("FromDate", NHibernateUtil.DateTime);
            query.AddScalar("ToDate", NHibernateUtil.DateTime);
            query.AddScalar("DocumentNo", NHibernateUtil.String);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(ValidatePrediem))).List<ValidatePrediem>().FirstOrDefault();
        }
    }
}

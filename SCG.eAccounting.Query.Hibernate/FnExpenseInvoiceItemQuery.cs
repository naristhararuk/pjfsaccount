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

namespace SCG.eAccounting.Query.Hibernate
{
    public class FnExpenseInvoiceItemQuery : NHibernateQueryBase<FnExpenseInvoiceItem, long>, IFnExpenseInvoiceItemQuery
    {
        public IList<FnExpenseInvoiceItem> GetInvoiceItemByInvoiceID(long invoiceId)
        {
            return GetCurrentSession().CreateQuery(" from FnExpenseInvoiceItem item where item.Invoice.InvoiceID = :InvoiceID")
                .SetInt64("InvoiceID", invoiceId)
                .List<FnExpenseInvoiceItem>();
        }

        public ISQLQuery FindInvoiceItemByExpenseID(long expenseId, short languageId, bool isCount, string sortExpression)
        {
            StringBuilder sqlBuilder = new StringBuilder();

            if (!isCount)
            {
                sqlBuilder.Append("select ExpenseCode, CostCenterCode, IONumber, Amount, LocalCurrencyAmount, MainCurrencyAmount  from (");
            }
            else
            {
                sqlBuilder.Append(" select count(*) as Count from (");
            }

            sqlBuilder.Append(" select (ac.AccountCode + '-' + acl.AccountName) as ExpenseCode, cost.CostCenterCode as CostCenterCode, i.IONumber as IONumber, ");
            sqlBuilder.Append(" sum(case when ISNULL(inv.IsVAT, 0) = 0 then ISNULL(item.Amount, 0) else ISNULL(item.Amount, 0) + ISNULL(item.NonDeductAmount, 0) end) as Amount, ");
            sqlBuilder.Append(" sum(ISNULL(item.LocalCurrencyAmount, 0)) as LocalCurrencyAmount, ");
            sqlBuilder.Append(" sum(ISNULL(item.MainCurrencyAmount, 0)) as MainCurrencyAmount ");
            sqlBuilder.Append(" from FnExpenseInvoice inv with (nolock) ");
            sqlBuilder.Append(" inner join FnExpenseInvoiceItem item with (nolock) on inv.InvoiceID = item.InvoiceID ");
            sqlBuilder.Append(" left join DbAccount ac with (nolock) on item.AccountID = ac.AccountID ");
            sqlBuilder.Append(" inner join DbAccountLang acl with (nolock) on ac.AccountID = acl.AccountID and acl.LanguageID = :LanguageID ");
            sqlBuilder.Append(" left join DbInternalOrder i with (nolock) on i.IOID = item.IOID ");
            sqlBuilder.Append(" inner join DbCostCenter cost with (nolock) on cost.CostCenterID = item.CostCenterID ");
            sqlBuilder.Append(" left join DbCurrency currency with (nolock) on currency.CurrencyID = item.CurrencyID ");
            sqlBuilder.Append(" where item.Active = 1 and inv.ExpenseID = :ExpenseID ");
            sqlBuilder.Append(" group by ac.AccountCode,acl.AccountName,cost.CostCenterCode,i.IONumber )t ");

            if (!isCount)
            {
                if (!string.IsNullOrEmpty(sortExpression))
                {
                    sqlBuilder.Append(String.Format(" order by {0} ", sortExpression));
                }
                else
                {
                    sqlBuilder.Append(" order by ExpenseCode, CostCenterCode, IONumber ");
                }
            }


            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("LanguageID", typeof(short), languageId);
            parameterBuilder.AddParameterData("ExpenseID", typeof(long), expenseId);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);

            if (!isCount)
            {
                query.AddScalar("ExpenseCode", NHibernateUtil.String)
                    .AddScalar("CostCenterCode", NHibernateUtil.String)
                    .AddScalar("IONumber", NHibernateUtil.String)
                    //.AddScalar("Description", NHibernateUtil.String)
                    .AddScalar("Amount", NHibernateUtil.Double)
                    .AddScalar("LocalCurrencyAmount", NHibernateUtil.Double)
                    .AddScalar("MainCurrencyAmount", NHibernateUtil.Double);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(VOInvoiceItem)));
            }
            else
            {
                query.AddScalar("Count", NHibernateUtil.Int32);
            }
            return query;
        }

        public IList<VOInvoiceItem> GetInvoiceItemListByExpenseID(long expenseId, short languageId, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<VOInvoiceItem>(
                ScgeAccountingQueryProvider.FnExpenseInvoiceItemQuery,
                "FindInvoiceItemByExpenseID", new object[] { expenseId, languageId, false, sortExpression },
                firstResult, maxResult, sortExpression
                );
        }
        public int CountInvoiceItemList(long expenseId, short languageId)
        {
            return NHibernateQueryHelper.CountByCriteria(
                ScgeAccountingQueryProvider.FnExpenseInvoiceItemQuery,
                "FindInvoiceItemByExpenseID",
                new object[] { expenseId, languageId, true, string.Empty }
                );
        }
    }
}

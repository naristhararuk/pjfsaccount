using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

using SCG.FN.DTO;
using SCG.FN.DTO.ValueObject;

using SS.DB.DTO;

namespace SCG.FN.Query.Hibernate
{
    public class FnCashierQuery : NHibernateQueryBase<FnCashier, short>, IFnCashierQuery
    {
        public ISQLQuery FindCashierByOrganization(short organizationId, short languageId, bool isCount, string sortExpression)
        {
            ISQLQuery query;
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            if (isCount)
            {
                sqlBuilder.Append(" SELECT count(*) as Count");
                sqlBuilder.Append(" FROM FnCashier cashier ");
                sqlBuilder.Append(" inner join FnCashierLang cl on cashier.CashierID = cl.CashierID and cl.LanguageID = :LanguageID ");
                sqlBuilder.Append(" where cashier.OrganizationID = :OrganizationID ");
            }
            else
            {
                sqlBuilder.Append(" SELECT cashier.CashierID as CashierId, cashier.CashierCode as CashierCode, cashier.CashierLevel, cashier.Active as Active, ");
                sqlBuilder.Append(" cashier.DivisionID as DivisionId, div.DivisionName as DivisionName ");
                sqlBuilder.Append(" FROM FnCashier cashier ");
                sqlBuilder.Append(" inner join FnCashierLang cl on cashier.CashierID = cl.CashierID and cl.LanguageID = :LanguageID ");
                sqlBuilder.Append(" inner join SuDivisionLang div on div.DivisionID = cashier.DivisionID and div.LanguageID = :LanguageID ");
                sqlBuilder.Append(" where cashier.OrganizationID = :OrganizationID ");
                if (string.IsNullOrEmpty(sortExpression))
                {
                    sqlBuilder.Append(" order by cashier.CashierID, cashier.CashierCode, cashier.CashierLevel, cashier.Active ");
                }
                else
                {
                    sqlBuilder.Append(String.Format(" order by {0} ", sortExpression));
                }
            }

            parameterBuilder.AddParameterData("LanguageID", typeof(short), languageId);
            parameterBuilder.AddParameterData("OrganizationID", typeof(short), organizationId);

            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);
            if (isCount)
            {
                query.AddScalar("Count", NHibernateUtil.Int32);
            }
            else
            {
                query.AddScalar("CashierId", NHibernateUtil.Int16)
                    .AddScalar("CashierCode", NHibernateUtil.String)
                    .AddScalar("CashierLevel", NHibernateUtil.String)
                    .AddScalar("DivisionId", NHibernateUtil.Int16)
                    .AddScalar("DivisionName", NHibernateUtil.String)
                    .AddScalar("Active", NHibernateUtil.Boolean);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(FnCashierSearchResult)));
            }
            return query;
        }

        public IList<FnCashierSearchResult> GetCashierList(short organizationId, short languageId, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<FnCashierSearchResult>(FnQueryProvider.FnCashierQuery, "FindCashierByOrganization", new object[] { organizationId, languageId, false, sortExpression }, firstResult, maxResult, sortExpression);
        }
        public int GetCashierCount(short organizationId, short languageId)
        {
            return NHibernateQueryHelper.CountByCriteria(FnQueryProvider.FnCashierQuery, "FindCashierByOrganization", new object[] { organizationId, languageId, true, string.Empty });
        }
    }
}

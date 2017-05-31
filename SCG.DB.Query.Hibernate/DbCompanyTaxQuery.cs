using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.QueryDao;
using SCG.DB.DTO;
using SS.Standard.Data.NHibernate.QueryCreator;
using NHibernate;
using NHibernate.Transform;
using SCG.DB.DTO.ValueObject;

namespace SCG.DB.Query.Hibernate
{
    public class DbCompanyTaxQuery : NHibernateQueryBase<DbCompanyTax, long> , IDbCompanyTaxQuery
    {
        public ISQLQuery FindByCompanyTaxCriteria(DbCompanyTax taxID, bool isCount, string sortExpression)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {
                sqlBuilder.Append(" SELECT ");
                sqlBuilder.Append("     DbCompanyTax.ID           AS ID       ,");
                sqlBuilder.Append("     DbCompanyTax.TaxID           AS TaxID       ,");
                sqlBuilder.Append("     DbCompanyTax.CompanyID           AS CompanyID       ,");
                sqlBuilder.Append("     DbCompany.CompanyCode           AS CompanyCode       ,");
                sqlBuilder.Append("     DbCompany.CompanyName           AS CompanyName       ,");
                sqlBuilder.Append("     DbCompanyTax.Rate         AS Rate       ,");
                sqlBuilder.Append("     DbCompanyTax.RateNonDeduct          AS RateNonDeduct     ,");
                sqlBuilder.Append("     DbCompanyTax.UseParentRate           AS UseParentRate ,     ");
                sqlBuilder.Append("     DbCompanyTax.Disable           AS Disable      ");
                sqlBuilder.Append(" FROM DbCompanyTax ");
                sqlBuilder.Append(" INNER JOIN DbCompany ON DbCompanyTax.CompanyID = DbCompany.CompanyID");
                sqlBuilder.Append(" WHERE DbCompanyTax.TaxID = :taxID "); 
                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine("ORDER BY DbCompany.CompanyCode");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));

            }
            else
            {
                sqlBuilder.Append(" SELECT COUNT(TaxID) AS CompanyTaxCount FROM DbCompanyTax ");
                sqlBuilder.Append(" WHERE DbCompanyTax.TaxID = :taxID ");
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("taxID", typeof(string), taxID.TaxID);
            queryParameterBuilder.FillParameters(query);
            if (!isCount)
            {
                query.AddScalar("ID", NHibernateUtil.Int64);
                query.AddScalar("TaxID", NHibernateUtil.Int64);
                query.AddScalar("CompanyID", NHibernateUtil.Int64);
                query.AddScalar("CompanyCode", NHibernateUtil.String);
                query.AddScalar("CompanyName", NHibernateUtil.String);
                query.AddScalar("Rate", NHibernateUtil.Double);
                query.AddScalar("RateNonDeduct", NHibernateUtil.Double);
                query.AddScalar("UseParentRate", NHibernateUtil.Boolean);
                query.AddScalar("Disable", NHibernateUtil.Boolean);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(CompanyTaxRate)));
            }
            else
            {
                query.AddScalar("CompanyTaxCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            return query;
        }

        public IList<CompanyTaxRate> GetCompanyTaxList(DbCompanyTax taxID, int firstResult, int maxResult, string sortExpression)
        {
            ISQLQuery queryy = FindByCompanyTaxCriteria(taxID, false, sortExpression);
            IList<CompanyTaxRate> CompanyTaxList = queryy.List<CompanyTaxRate>();
            return CompanyTaxList;
        }

        public int FindCount(DbCompanyTax taxID)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbCompanyTaxQuery, "FindByCompanyTaxCriteria", new object[] { taxID, true, string.Empty });
        }
    }
}

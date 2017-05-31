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
    public class DbTaxQuery : NHibernateQueryBase<DbTax, long>, IDbTaxQuery
    {
        #region public ISQLQuery FindByTaxCriteria(DbTax tax, bool isCount, string sortExpression)
        public ISQLQuery FindByTaxCriteria(DbTax tax, bool isCount, string sortExpression)
        {
            string taxCode = "";
            string description = "";
            if (tax != null)
            {
                taxCode = "%" + tax.TaxCode + "%";
                description = "%" + tax.TaxName + "%";
            }
            else
            {
                taxCode = "%%";
                description = "%%";
            }
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {
                sqlBuilder.Append(" SELECT ");
                sqlBuilder.Append("     DbTax.TaxID           AS TaxID       ,");
                sqlBuilder.Append("     DbTax.TaxCode           AS TaxCode       ,");
                sqlBuilder.Append("     DbTax.TaxName         AS TaxName       ,");
                sqlBuilder.Append("     DbTax.GL          AS GL     ,");
                sqlBuilder.Append("     DbTax.Rate           AS Rate      ,");
                sqlBuilder.Append("     DbTax.ApplyAllCompany           AS ApplyAllCompany ,     ");
                sqlBuilder.Append("     DbTax.RateNonDeduct           AS RateNonDeduct ,     ");
                sqlBuilder.Append("     DbTax.Active           AS Active      ");
                sqlBuilder.Append(" FROM DbTax ");
                sqlBuilder.Append(" WHERE DbTax.TaxCode like :taxCode ");
                sqlBuilder.Append(" AND DbTax.TaxName like :description ");
                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine("ORDER BY DbTax.TaxCode");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));

            }
            else
            {
                sqlBuilder.Append(" SELECT COUNT(TaxID) AS TaxCount FROM DbTax ");
                sqlBuilder.Append(" WHERE DbTax.TaxCode like :taxCode ");
                sqlBuilder.Append(" AND DbTax.TaxName like :description ");
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("taxCode", typeof(string), taxCode);
            queryParameterBuilder.AddParameterData("description", typeof(string), description);
            queryParameterBuilder.FillParameters(query);
            if (!isCount)
            {
                query.AddScalar("TaxID", NHibernateUtil.Int64);
                query.AddScalar("TaxCode", NHibernateUtil.String);
                query.AddScalar("TaxName", NHibernateUtil.String);
                query.AddScalar("GL", NHibernateUtil.String);
                query.AddScalar("Rate", NHibernateUtil.Double);
                query.AddScalar("RateNonDeduct", NHibernateUtil.Double);
                query.AddScalar("ApplyAllCompany", NHibernateUtil.Boolean);
                query.AddScalar("Active", NHibernateUtil.Boolean);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(DbTax)));
            }
            else
            {
                query.AddScalar("TaxCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }
           return query;
        }
        #endregion public ISQLQuery FindByBankCriteria(CountryLang country, bool isCount, short languageId, string sortExpression)

        public IList<DbTax> GetTaxList(DbTax tax, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<DbTax>(ScgDbQueryProvider.DbTaxQuery, "FindByTaxCriteria", new object[] { tax, false, sortExpression }, firstResult, maxResult, sortExpression);
        }

        public int CountByTaxCriteria(DbTax tax)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbTaxQuery, "FindByTaxCriteria", new object[] { tax, true,  string.Empty });
        }
        public DbTax FindbyTaxCode(string taxCode)
        {
            return GetCurrentSession().CreateQuery(" from DbTax where Active = 1 and TaxCode = :TaxCode")
                .SetString("TaxCode", taxCode)
                .UniqueResult<DbTax>();
        }
        public IList<DbTax> GetTaxCodeActive()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select TaxCode,TaxID ");
            sqlBuilder.Append(" from DbTax ");
            sqlBuilder.Append(" Where DbTax.Active = 1 ");
            sqlBuilder.Append(" and DbTax.ApplyAllCompany = 1 ");
            sqlBuilder.Append(" order by DbTax.TaxCode ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("TaxCode", NHibernateUtil.String)
                .AddScalar("TaxID", NHibernateUtil.Int64);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(DbTax))).List<DbTax>();
        }

        public IList<DbTax> GetTaxCodeActiveByCompany(long companyID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select DbTax.TaxCode,DbTax.TaxID ");
            sqlBuilder.Append(" from DbTax ");
            sqlBuilder.Append(" left join DbCompanyTax on DbTax.TaxID = DbCompanyTax.TaxID and DbCompanyTax.CompanyID = :companyID");
            sqlBuilder.Append(" Where DbTax.Active = 1 ");
            sqlBuilder.Append(" and (ISNULL(DbCompanyTax.Disable,0) <> 1)");
            sqlBuilder.Append(" or (DbTax.ApplyAllCompany = 1 and DbTax.Active = 1)");
            sqlBuilder.Append(" order by DbTax.TaxCode ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("companyID", typeof(string), companyID);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("TaxCode", NHibernateUtil.String)
                .AddScalar("TaxID", NHibernateUtil.Int64);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(DbTax))).List<DbTax>();
    }

        public DbTax GetCompanyTaxRateByCompany(long taxID, long companyID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" Select Case When ISNULL(DbCompanyTax.UseParentRate,1) = 1 then ");
            sqlBuilder.Append(" Convert(varchar(30),DbTax.Rate) ");
            sqlBuilder.Append(" else Convert(varchar(30),DbCompanyTax.Rate) ");
            sqlBuilder.Append(" end as Rate,");
            sqlBuilder.Append(" Case When ISNULL(DbCompanyTax.UseParentRate,1) = 1 then ");
            sqlBuilder.Append(" Convert(varchar(30),DbTax.RateNonDeduct) ");
            sqlBuilder.Append(" else Convert(varchar(30),DbCompanyTax.RateNonDeduct) ");
            sqlBuilder.Append(" end as RateNonDeduct ");
            sqlBuilder.Append(" From DbTax ");
            sqlBuilder.Append(" Left Join DbCompanyTax  ");
            sqlBuilder.Append(" on DbTax.TaxID = DbCompanyTax.TaxID and DbCompanyTax.CompanyID = :companyID");
            sqlBuilder.Append(" Where DbTax.Active = 1  ");
            sqlBuilder.Append(" and (ISNULL(DbCompanyTax.Disable,0) <> 1 or DbTax.ApplyAllCompany = 1) ");
            sqlBuilder.Append(" and DbTax.TaxID = :taxID ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("companyID", typeof(string), companyID);
            queryParameterBuilder.AddParameterData("taxID", typeof(string), taxID);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("Rate", NHibernateUtil.Double)
                .AddScalar("RateNonDeduct", NHibernateUtil.Double);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(DbTax))).UniqueResult<DbTax>();
}
    }
}

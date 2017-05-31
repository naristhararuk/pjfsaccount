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
    public class DbAccountCompanyQuery : NHibernateQueryBase<DbAccountCompany, long>, IDbAccountCompanyQuery
    {
        public IList<VOAccountCompany> FindAccountCompanyByAccountID(long accountID)
        {
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select ac.ID as ID ,ac.AccountID as AccountID, ac.CompanyID as CompanyID, ");
            sqlBuilder.Append(" ac.active as Active , com.CompanyCode as CompanyCode, ");
            sqlBuilder.Append(" com.CompanyName as CompanyName from DbAccountCompany ac ");
            sqlBuilder.Append(" inner join DbCompany com on ac.CompanyID=com.CompanyID ");

            StringBuilder whereClauseBuilder = new StringBuilder();
            whereClauseBuilder.Append(" and ac.active = 1 ");
            if (accountID != 0)
            {
                whereClauseBuilder.Append(" and ac.AccountID= :accountID ");
                queryParameterBuilder.AddParameterData("accountID", typeof(long), accountID);
            }

            if (!string.IsNullOrEmpty(whereClauseBuilder.ToString()))
            {
                sqlBuilder.Append(String.Format(" where 1=1 {0} ", whereClauseBuilder.ToString()));
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("ID", NHibernateUtil.Int64)
                .AddScalar("AccountID", NHibernateUtil.Int64)
                 .AddScalar("CompanyID", NHibernateUtil.Int64)
                  .AddScalar("CompanyName", NHibernateUtil.String)
                   .AddScalar("Active", NHibernateUtil.Boolean)
                   .AddScalar("CompanyCode", NHibernateUtil.String);


            query.SetResultTransformer(Transformers.AliasToBean(typeof(VOAccountCompany)));
            IList<VOAccountCompany> list = query.List<VOAccountCompany>();

            return list;

        }

        public IList<DbAccountCompany> FindAccountByCompanyID(long companyId)
        {
            return GetCurrentSession().CreateQuery(" from DbAccountCompany where Active = 1 and CompanyID = :CompanyID ")
                .SetInt64("CompanyID", companyId)
                .List<DbAccountCompany>();
        }

        public IList<VOAccountCompany> FindAccountCompany(long companyId, long accountId)
        {
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select ac.ID as ID ,ac.AccountID as AccountID, ac.CompanyID as CompanyID, ");
            sqlBuilder.Append(" ac.active as Active , com.CompanyCode as CompanyCode, ");
            sqlBuilder.Append(" com.CompanyName as CompanyName from DbAccountCompany ac ");
            sqlBuilder.Append(" inner join DbCompany com on ac.CompanyID=com.CompanyID ");
            sqlBuilder.Append(" inner join DbAccount a on a.AccountID=ac.AccountID ");

            StringBuilder whereClauseBuilder = new StringBuilder();
            whereClauseBuilder.Append(" and a.Active = 1 ");
            if (accountId != 0)
            {
                whereClauseBuilder.Append(" and ac.AccountID= :AccountID ");
                queryParameterBuilder.AddParameterData("AccountID", typeof(long), accountId);
            }
            if (companyId != 0)
            {
                whereClauseBuilder.Append(" and com.CompanyID= :CompanyID ");
                queryParameterBuilder.AddParameterData("CompanyID", typeof(long), companyId);
            }

            if (!string.IsNullOrEmpty(whereClauseBuilder.ToString()))
            {
                sqlBuilder.Append(String.Format(" where 1=1 {0} ", whereClauseBuilder.ToString()));
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("ID", NHibernateUtil.Int64)
                .AddScalar("AccountID", NHibernateUtil.Int64)
                .AddScalar("CompanyID", NHibernateUtil.Int64)
                .AddScalar("Active", NHibernateUtil.Boolean);


            query.SetResultTransformer(Transformers.AliasToBean(typeof(VOAccountCompany)));
            IList<VOAccountCompany> list = query.List<VOAccountCompany>();

            return list;
        }
    }
}

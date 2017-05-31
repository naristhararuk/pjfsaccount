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
using NHibernate.Expression;

namespace SCG.DB.Query.Hibernate
{
    public class DbCompanyPaymentMethodQuery : NHibernateQueryBase<DbCompanyPaymentMethod, long>, IDbCompanyPaymentMethodQuery
    {

       

        public IList<CompanyPaymentMethodResult> FindCompanyPaymentMethodByCompanyID(long companyID)
        {
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("SELECT cp.CompanyPaymentMethodID as CompanyPaymentMethodID,cp.CompanyID as CompanyID,cp.CompanyCode as CompanyCode, cp.PaymentMethodID as PaymentMethodID , p.PaymentMethodCode as PaymentMethodCode , p.PaymentMethodName as PaymentMethodName ,cp.Active as Active ");
            sqlBuilder.Append("FROM DbCompanyPaymentMethod as cp ");
            sqlBuilder.Append("LEFT JOIN DbPaymentMethod as p on p.PaymentMethodID = cp.PaymentMethodID ");
            //edit 28042009
            //sqlBuilder.Append("WHERE cp.CompanyID =:CompanyID AND cp.Active = 1 and p.Active = 1 ");
            sqlBuilder.Append("WHERE cp.CompanyID =:CompanyID  and p.Active = 1 ");
            sqlBuilder.AppendLine("ORDER BY p.PaymentMethodCode,p.PaymentMethodName ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("CompanyID", typeof(long), companyID);
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("CompanyPaymentMethodID", NHibernateUtil.Int64);
            query.AddScalar("CompanyID", NHibernateUtil.Int64);
            query.AddScalar("CompanyCode", NHibernateUtil.String);
            query.AddScalar("PaymentMethodID", NHibernateUtil.Int64);
            query.AddScalar("PaymentMethodCode", NHibernateUtil.String);
            query.AddScalar("PaymentMethodName", NHibernateUtil.String);
            query.AddScalar("Active", NHibernateUtil.Boolean);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(CompanyPaymentMethodResult))).List<CompanyPaymentMethodResult>();
        }

        public DbCompanyPaymentMethod FindCompanyPaymentMethodByCompanyIdAndPaymentMethodId(long companyId, long paymentMethodId)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(DbCompanyPaymentMethod), "cp");

            criteria.Add(Expression.And(Expression.Eq("cp.Company.CompanyID", companyId), Expression.Eq("cp.PaymentMethod.PaymentMethodID", paymentMethodId)));

            return criteria.UniqueResult<DbCompanyPaymentMethod>();
        }

        public IList<CompanyPaymentMethodResult> GetPaymentMethod(long companyID)
        {
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append(" SELECT DbPaymentMethod.PaymentMethodID as PaymentMethodID, DbPaymentMethod.PaymentMethodName as PaymentMethodName, '[' + DbPaymentMethod.PaymentMethodCode + '] ' +  DbPaymentMethod.PaymentMethodName  as PaymentMethodCode ");
            sqlBuilder.Append(" FROM DbCompanyPaymentMethod  ");
            sqlBuilder.Append(" INNER JOIN DbPaymentMethod  ");
            sqlBuilder.Append(" ON DbCompanyPaymentMethod.PaymentMethodID = DbPaymentMethod.PaymentMethodID ");
            sqlBuilder.Append(" where DbCompanyPaymentMethod.CompanyID= :companyID ");
            //sqlBuilder.Append(" and DbCompany.PaymentType = :paymentType ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("companyID", typeof(long), companyID);
            //queryParameterBuilder.AddParameterData("paymentType", typeof(string), paymentType);
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("PaymentMethodID", NHibernateUtil.Int64);
            query.AddScalar("PaymentMethodName", NHibernateUtil.String);
            query.AddScalar("PaymentMethodCode", NHibernateUtil.String);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(CompanyPaymentMethodResult))).List<CompanyPaymentMethodResult>();
        }

    }
}

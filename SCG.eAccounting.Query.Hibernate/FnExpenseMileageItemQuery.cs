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
    public class FnExpenseMileageItemQuery : NHibernateQueryBase<FnExpenseMileageItem, long>, IFnExpenseMileageItemQuery
	{
        public IList<FnExpenseMileageItem> GetMileageItemByMileageID(long mileageId)
        {
            return GetCurrentSession().CreateQuery(" from FnExpenseMileageItem item where item.ExpenseMileage.ExpenseMileageID = :MileageID")
                .SetInt64("MileageID", mileageId)
                .List<FnExpenseMileageItem>();
        }

        public ValidateMilage GetMileageItemForValidationLeft(long RequesterId, string CarLicenseNo, DateTime travelDate, long expId)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select TOP(1) d.CarMeterStart,d.CarMeterEnd,d.TravelDate,a.DocumentNo from Document a ");
            sqlBuilder.Append(" inner join [dbo].[FnExpenseDocument] b on a.DocumentID = b.DocumentID ");
            sqlBuilder.Append(" inner join [dbo].FnExpenseMileage c on b.ExpenseID = c.ExpenseID ");
            sqlBuilder.Append(" inner join [dbo].FnExpenseMileageItem d on c.ExpenseMileageID = d.ExpenseMileageID ");
            sqlBuilder.Append(" where a.CacheCurrentStateName <> 'Cancel' and c.[Owner] = 'EMP' ");
            sqlBuilder.Append(" and a.RequesterID = :RequesterID and c.CarLicenseNo = :CarLicenseNo and d.TravelDate < :travelDate and b.ExpenseID <> :expId ");
            sqlBuilder.Append(" ORDER BY d.TravelDate DESC ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("RequesterID", typeof(long), RequesterId);
            queryParameterBuilder.AddParameterData("CarLicenseNo", typeof(string), CarLicenseNo);
            queryParameterBuilder.AddParameterData("travelDate", typeof(DateTime), travelDate);
            queryParameterBuilder.AddParameterData("expId", typeof(long), expId);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("CarMeterStart", NHibernateUtil.Double);
            query.AddScalar("CarMeterEnd", NHibernateUtil.Double);
            query.AddScalar("TravelDate", NHibernateUtil.DateTime);
            query.AddScalar("DocumentNo", NHibernateUtil.String);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(ValidateMilage))).List<ValidateMilage>().FirstOrDefault();
        }

        public ValidateMilage GetMileageItemForValidationRight(long RequesterId, string CarLicenseNo, DateTime travelDate, long expId)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select TOP(1) d.CarMeterStart,d.CarMeterEnd,d.TravelDate,a.DocumentNo from Document a ");
            sqlBuilder.Append(" inner join [dbo].[FnExpenseDocument] b on a.DocumentID = b.DocumentID ");
            sqlBuilder.Append(" inner join [dbo].FnExpenseMileage c on b.ExpenseID = c.ExpenseID ");
            sqlBuilder.Append(" inner join [dbo].FnExpenseMileageItem d on c.ExpenseMileageID = d.ExpenseMileageID ");
            sqlBuilder.Append(" where a.CacheCurrentStateName <> 'Cancel' and c.[Owner] = 'EMP' ");
            sqlBuilder.Append(" and a.RequesterID = :RequesterID and c.CarLicenseNo = :CarLicenseNo and d.TravelDate > :travelDate and b.ExpenseID <> :expId ");
            sqlBuilder.Append(" ORDER BY d.TravelDate ASC ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("RequesterID", typeof(long), RequesterId);
            queryParameterBuilder.AddParameterData("CarLicenseNo", typeof(string), CarLicenseNo);
            queryParameterBuilder.AddParameterData("travelDate", typeof(DateTime), travelDate);
            queryParameterBuilder.AddParameterData("expId", typeof(long), expId);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("CarMeterStart", NHibernateUtil.Double);
            query.AddScalar("CarMeterEnd", NHibernateUtil.Double);
            query.AddScalar("TravelDate", NHibernateUtil.DateTime);
            query.AddScalar("DocumentNo", NHibernateUtil.String);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(ValidateMilage))).List<ValidateMilage>().FirstOrDefault();
        }

        public ValidateMilage GetMileageItemForValidationCheckLength(long RequesterId, string CarLicenseNo, DateTime travelDate, long expId, Double meter)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select a.DocumentNo from Document a ");
            sqlBuilder.Append(" inner join [dbo].[FnExpenseDocument] b on a.DocumentID = b.DocumentID ");
            sqlBuilder.Append(" inner join [dbo].FnExpenseMileage c on b.ExpenseID = c.ExpenseID ");
            sqlBuilder.Append(" inner join [dbo].FnExpenseMileageItem d on c.ExpenseMileageID = d.ExpenseMileageID ");
            sqlBuilder.Append(" where a.CacheCurrentStateName <> 'Cancel' and c.[Owner] = 'EMP' ");
            sqlBuilder.Append(" and a.RequesterID = :RequesterID and c.CarLicenseNo = :CarLicenseNo and d.TravelDate = :travelDate and b.ExpenseID <> :expId ");
            sqlBuilder.Append(" and (d.CarMeterStart <= :meter and d.CarMeterEnd > :meter) ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("RequesterID", typeof(long), RequesterId);
            queryParameterBuilder.AddParameterData("CarLicenseNo", typeof(string), CarLicenseNo);
            queryParameterBuilder.AddParameterData("travelDate", typeof(DateTime), travelDate);
            queryParameterBuilder.AddParameterData("expId", typeof(long), expId);
            queryParameterBuilder.AddParameterData("meter", typeof(Double), meter);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("DocumentNo", NHibernateUtil.String);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(ValidateMilage))).List<ValidateMilage>().FirstOrDefault();
        }

        public ValidateMilage GetMileageItemForValidationEquals(long RequesterId, string CarLicenseNo, DateTime travelDate, long expId, Double meter)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select TOP(1) d.CarMeterStart,d.CarMeterEnd,d.TravelDate, a.DocumentNo from Document a ");
            sqlBuilder.Append(" inner join [dbo].[FnExpenseDocument] b on a.DocumentID = b.DocumentID ");
            sqlBuilder.Append(" inner join [dbo].FnExpenseMileage c on b.ExpenseID = c.ExpenseID ");
            sqlBuilder.Append(" inner join [dbo].FnExpenseMileageItem d on c.ExpenseMileageID = d.ExpenseMileageID ");
            sqlBuilder.Append(" where a.CacheCurrentStateName <> 'Cancel' and c.[Owner] = 'EMP' ");
            sqlBuilder.Append(" and a.RequesterID = :RequesterID and c.CarLicenseNo = :CarLicenseNo and d.TravelDate = :travelDate and b.ExpenseID <> :expId and d.CarMeterStart > :meter");
            sqlBuilder.Append(" ORDER BY d.TravelDate ASC ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("RequesterID", typeof(long), RequesterId);
            queryParameterBuilder.AddParameterData("CarLicenseNo", typeof(string), CarLicenseNo);
            queryParameterBuilder.AddParameterData("travelDate", typeof(DateTime), travelDate);
            queryParameterBuilder.AddParameterData("expId", typeof(long), expId);
            queryParameterBuilder.AddParameterData("meter", typeof(Double), meter);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("CarMeterStart", NHibernateUtil.Double);
            query.AddScalar("CarMeterEnd", NHibernateUtil.Double);
            query.AddScalar("TravelDate", NHibernateUtil.DateTime);
            query.AddScalar("DocumentNo", NHibernateUtil.String);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(ValidateMilage))).List<ValidateMilage>().FirstOrDefault();
        }
	}
}

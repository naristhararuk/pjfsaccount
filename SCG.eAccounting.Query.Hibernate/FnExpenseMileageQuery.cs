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

namespace SCG.eAccounting.Query.Hibernate
{
    public class FnExpenseMileageQuery : NHibernateQueryBase<FnExpenseMileage, long>, IFnExpenseMileageQuery
	{
        public IList<FnExpenseMileage> GetMileageByExpenseID(long expenseId)
        {
            return GetCurrentSession().CreateQuery(" from FnExpenseMileage m where m.Expense.ExpenseID = :ExpenseID")
                .SetInt64("ExpenseID", expenseId)
                .List<FnExpenseMileage>();
        }

        public double GetLastMileByCarLicenseNo(string carLicenseNo)
        { 
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select (case when max(item.CarMeterEnd) is null then 0 else max(item.CarMeterEnd) end) as LastMile ");
            sqlBuilder.Append(" from FnExpenseMileage mileage ");
            sqlBuilder.Append(" inner join FnExpenseMileageItem item on mileage.ExpenseMileageID = item.ExpenseMileageID ");
            sqlBuilder.Append(" where mileage.Active = 1 and mileage.CarLicenseNo = :CarLicenseNo ");
            return GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString())
                .AddScalar("LastMile", NHibernateUtil.Double)
                .SetString("CarLicenseNo", carLicenseNo)
                .UniqueResult<double>();
        }
	}
}


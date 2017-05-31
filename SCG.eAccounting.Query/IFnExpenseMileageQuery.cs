using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SCG.eAccounting.DTO;

using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;

namespace SCG.eAccounting.Query
{
    public interface IFnExpenseMileageQuery : IQuery<FnExpenseMileage, long>
	{
        IList<FnExpenseMileage> GetMileageByExpenseID(long expenseId);
        double GetLastMileByCarLicenseNo(string carLicenseNo);
	}
}

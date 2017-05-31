using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SCG.eAccounting.DTO;

using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;
using SCG.eAccounting.DTO.ValueObject;

namespace SCG.eAccounting.Query
{
    public interface IFnExpenseMileageItemQuery : IQuery<FnExpenseMileageItem, long>
	{
        IList<FnExpenseMileageItem> GetMileageItemByMileageID(long mileageId);
        ValidateMilage GetMileageItemForValidationLeft(long RequesterId, string CarLicenseNo, DateTime travelDate, long itemid);
        ValidateMilage GetMileageItemForValidationRight(long RequesterId, string CarLicenseNo, DateTime travelDate, long itemid);
        ValidateMilage GetMileageItemForValidationCheckLength(long RequesterId, string CarLicenseNo, DateTime travelDate, long expId, Double meter);
        ValidateMilage GetMileageItemForValidationEquals(long RequesterId, string CarLicenseNo, DateTime travelDate, long expId, Double meter);
	}
}

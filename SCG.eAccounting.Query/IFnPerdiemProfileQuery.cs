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
    public interface IFnPerdiemProfileQuery : IQuery<FnPerdiemProfile, long>
    {
        IList<FnPerdiemProfile> GetForeignPerdiemRateProfileListByCriteria(ForeignPerdiemRateProfileCriteria criteria, int startRow, int pageSize, string sortExpression);
        int CountForeignPerdiemRateProfile(ForeignPerdiemRateProfileCriteria criteria);
        IList<FnPerdiemProfile> GetPRList();
    }
}

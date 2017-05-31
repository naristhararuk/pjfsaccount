using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;
using SCG.DB.DTO;

namespace SCG.DB.Query
{
    public interface IDbMileageRateRevisionQuery : IQuery<DbMileageRateRevision, Guid>
    {
        ISQLQuery FindByMileageRateRevision(bool isCount);
        IList<DbMileageRateRevision> GetMileageRateRevisionList(int firstResult, int maxResult, string sortExpression);
      //  IList<DbMileageRateRevision> GetMileageRateRevisionListDetail(int firstResult, int maxResult, string sortExpression);
        int CountByMileageRateRevision();
        ISQLQuery FindMieagePermission(long UserID);
        DbMileageRateRevision FindEffectiveDate(Guid Id);
        bool ChekPermission(long UerID);
    }
}

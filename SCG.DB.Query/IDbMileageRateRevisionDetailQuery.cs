using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;
using SCG.DB.DTO;

namespace SCG.DB.Query
{
    public interface IDbMileageRateRevisionDetailQuery : IQuery<DbMileageRateRevisionDetail, Guid>
    {
        ISQLQuery FindByMileageRateRevisionItem(Guid Id, bool isCount);
        IList<DbMileageRateRevisionDetail> GetMileageRateRevisionListItem(Guid Id,int firstResult, int maxResult, string sortExpression);
        int CountByMileageRateRevisionItem(Guid Id);
        IList<DbMileageRateRevisionDetail> FindPositionLevel();
        string FindPositionLevelByCode(string personalLevelCode);
        DbMileageRateRevisionDetail FindMileageRateRevision(Guid mlpID, string code, DateTime date);
        IList<DbMileageRateRevisionDetail> FindForRemoveMileageRateRevisionItem(Guid Id);
    }
}

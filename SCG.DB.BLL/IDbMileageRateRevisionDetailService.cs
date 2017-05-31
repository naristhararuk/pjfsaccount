using System;
using SS.Standard.Data.NHibernate.Service;
using SS.Standard.Data.NHibernate.Dao;
using SCG.DB.DTO;


namespace SCG.DB.BLL
{
    public interface IDbMileageRateRevisionDetailService : IService<DbMileageRateRevisionDetail, Guid>
    {
        void AddMileageRateRevisionItem(DbMileageRateRevisionDetail MileageRateRevision, long UserAccount);
        void RemoveMileageRateRevisionItem(DbMileageRateRevisionDetail MileageRateRevision);
        
    }
}

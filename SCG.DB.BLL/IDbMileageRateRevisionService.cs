using System;
using SS.Standard.Data.NHibernate.Service;
using SS.Standard.Data.NHibernate.Dao;
using SCG.DB.DTO;


namespace SCG.DB.BLL
{
    public interface IDbMileageRateRevisionService : IService<DbMileageRateRevision, Guid>
    {
        void AddMileageRateRevision(DbMileageRateRevision MileageRateRevision, long UserAccount);
        void ApproveMileageRateRevision(DbMileageRateRevision MileageRateRevision, long UserAccount);
        void CancelMileageRateRevision(DbMileageRateRevision MileageRateRevision, long UserAccount);
        void RemoveMileageRateRevision(DbMileageRateRevision MileageRateRevision);
        void UpdateMileageRateRevision(DbMileageRateRevision MileageRateRevision, long UserAccount);
        
    }
}

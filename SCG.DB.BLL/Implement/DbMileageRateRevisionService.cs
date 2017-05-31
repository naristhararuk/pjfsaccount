using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Security;
using System.Data;
using SS.Standard.Utilities;
using SS.Standard.Data.NHibernate.Dao;
using SCG.DB.DAL;
using SCG.DB.DTO;
using SCG.DB.Query;


namespace SCG.DB.BLL.Implement
{
    public class DbMileageRateRevisionService : ServiceBase<DbMileageRateRevision, Guid>, IDbMileageRateRevisionService
    {
        public override IDao<DbMileageRateRevision, Guid> GetBaseDao()
        {
            return ScgDbDaoProvider.DbMileageRateRevisionDao;
        }
        public void AddMileageRateRevision(DbMileageRateRevision MileageRateRevision, long UserAccount)
        {
            MileageRateRevision.Status = 0;
            MileageRateRevision.Active = true;
            MileageRateRevision.CreBy = UserAccount;
            MileageRateRevision.CreDate = DateTime.Now.Date;
            MileageRateRevision.UpdPgm = "MileageRateRevision";
            MileageRateRevision.UpdBy = UserAccount;
            MileageRateRevision.UpdDate = DateTime.Now.Date;

            ScgDbDaoProvider.DbMileageRateRevisionDao.Save(MileageRateRevision);       
        }
        public void ApproveMileageRateRevision(DbMileageRateRevision MileageRateRevision, long UserAccount)
        {
            MileageRateRevision.ApprovedDate = DateTime.Now;
            MileageRateRevision.Active = true;
            MileageRateRevision.UpdPgm = "MileageRateRevision";
            MileageRateRevision.UpdBy = UserAccount;
            MileageRateRevision.UpdDate = DateTime.Now.Date;
            ScgDbDaoProvider.DbMileageRateRevisionDao.SaveOrUpdate(MileageRateRevision);
        }
        public void CancelMileageRateRevision(DbMileageRateRevision MileageRateRevision, long UserAccount) 
        {
            MileageRateRevision.Active = true;
            MileageRateRevision.UpdPgm = "MileageRateRevision";
            MileageRateRevision.UpdBy = UserAccount;
            MileageRateRevision.UpdDate = DateTime.Now.Date;
            ScgDbDaoProvider.DbMileageRateRevisionDao.SaveOrUpdate(MileageRateRevision);
        }

        public void UpdateMileageRateRevision(DbMileageRateRevision MileageRateRevision, long UserAccount)
        {
            MileageRateRevision.Active = true;
            MileageRateRevision.UpdPgm = "MileageRateRevision";
            MileageRateRevision.UpdBy = UserAccount;
            MileageRateRevision.UpdDate = DateTime.Now.Date;
            ScgDbDaoProvider.DbMileageRateRevisionDao.SaveOrUpdate(MileageRateRevision);
        }

        
        public void RemoveMileageRateRevision(DbMileageRateRevision MileageRateRevision) 
        {
            //ScgDbQueryProvider.DbMileageRateRevisionDetailQuery.RemoveMileageRateRevisionItem(MileageRateRevision.Id);
            IList<DbMileageRateRevisionDetail> result = ScgDbQueryProvider.DbMileageRateRevisionDetailQuery.FindForRemoveMileageRateRevisionItem(MileageRateRevision.Id);
            foreach (DbMileageRateRevisionDetail mileageId in result) 
            {
                ScgDbDaoProvider.DbMileageRateRevisionDetailDao.Delete(mileageId);
            }
            ScgDbDaoProvider.DbMileageRateRevisionDao.Delete(MileageRateRevision);
                
        }

        //void RemoveProfileList();
    }
}

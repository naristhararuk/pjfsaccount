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


namespace SCG.DB.BLL.Implement
{
    public class DbMileageRateRevisionDetailService : ServiceBase<DbMileageRateRevisionDetail, Guid>, IDbMileageRateRevisionDetailService
    {
        public override IDao<DbMileageRateRevisionDetail, Guid> GetBaseDao()
        {
            return ScgDbDaoProvider.DbMileageRateRevisionDetailDao;
        }
       
        public void AddMileageRateRevisionItem(DbMileageRateRevisionDetail MileageRateRevision, long UserAccount)
        {

            MileageRateRevision.CreBy = UserAccount;
            MileageRateRevision.Active = true;
            MileageRateRevision.CreDate = DateTime.Now.Date;
            MileageRateRevision.UpdPgm = "MileageRateRevision";
            MileageRateRevision.UpdBy = UserAccount;
            MileageRateRevision.UpdDate = DateTime.Now.Date;

            ScgDbDaoProvider.DbMileageRateRevisionDetailDao.SaveOrUpdate(MileageRateRevision);  
        }
        public void RemoveMileageRateRevisionItem(DbMileageRateRevisionDetail MileageRateRevision)
        {
            ScgDbDaoProvider.DbMileageRateRevisionDetailDao.Delete(MileageRateRevision);
        }

        //void RemoveProfileList();
    }
}

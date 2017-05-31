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
    public class DbProfileListService : ServiceBase<DbProfileList, Guid>, IDbProfileListService
    {
        public override IDao<DbProfileList, Guid> GetBaseDao()
        {
            return ScgDbDaoProvider.DbProfileListDao;
        }
        public void AddProfileList(DbProfileList ProfileList, long UserAccount)
        {
            ProfileList.Id = Guid.Empty;
            ProfileList.Active = true;
            ProfileList.CreBy = UserAccount;
            ProfileList.CreDate = DateTime.Now.Date;
            ProfileList.UpdPgm = "ProfileList";
            ProfileList.UpdBy = UserAccount;
            ProfileList.UpdDate = DateTime.Now.Date;

            ScgDbDaoProvider.DbProfileListDao.Save(ProfileList);         
        }
        public void UpdateProfileList(DbProfileList ProfileList, long UserAccount)
        {
           //ProfileList.Id = Id;
            ProfileList.Active = true;
            ProfileList.CreBy = UserAccount;
            ProfileList.CreDate = DateTime.Now.Date;
            ProfileList.UpdPgm = "ProfileList";
            ProfileList.UpdBy = UserAccount;
            ProfileList.UpdDate = DateTime.Now.Date;
            ScgDbDaoProvider.DbProfileListDao.SaveOrUpdate(ProfileList);
        }

        public void RemoveProfileList(DbProfileList ProfileList) 
        {
            ScgDbDaoProvider.DbProfileListDao.Delete(ProfileList);
        }
        //void RemoveProfileList();
    }
}

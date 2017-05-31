using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Utilities;
using SS.SU.DTO;
using SS.SU.BLL;
using SS.SU.DAL;
using SS.SU.DTO.ValueObject;
namespace SS.SU.BLL.Implement
{
    public partial class TmpSuUserService : ServiceBase<TmpSuUser, long>, ITmpSuUserService
    {
        public override IDao<TmpSuUser, long> GetBaseDao()
        {
            return DaoProvider.TmpSuUserDao;
        }

        #region ITmpSuUserService Members

        public void AddUser(TmpSuUser tmpSuUser)
        {
            DaoProvider.TmpSuUserDao.Save(tmpSuUser);
        }

        public void AddUserList(List<TmpSuUser> tmpSuUserList)
        {
            foreach (TmpSuUser tmpSuUser in tmpSuUserList)
            {
                DaoProvider.TmpSuUserDao.SaveOrUpdate(tmpSuUser);
            }
        }

        public void DeleteAll()
        {
            DaoProvider.TmpSuUserDao.DeleteAll();
        }

        public long FindCostCenter(string costCenter)
        {
            return DaoProvider.TmpSuUserDao.FindCostCenter(costCenter);
        }
        /*public long FindSuperVisor(string userName)
        {
            return DaoProvider.TmpSuUserDao.FindSuperVisor(userName);
        }*/
        public long FindLocation(string location)
        {
            return DaoProvider.TmpSuUserDao.FindLocation(location);
        }

        public IList<NewUserEmail> FindNewUser()
        {
            return DaoProvider.TmpSuUserDao.FindNewUser();
        }
        public void BeforeCommit()
        {
            DaoProvider.TmpSuUserDao.BeforeCommit();
        }
        public void CommitImport(int byId, int RoleId)
        {
            DaoProvider.TmpSuUserDao.CommitImport(byId, RoleId);
        }

        public void AfterCommit(int byId)
        {
            DaoProvider.TmpSuUserDao.AfterCommit(byId);
        }

        public void CommitManualImport(int byId, int RoleId)
        {
            DaoProvider.TmpSuUserDao.CommitManualImport(byId, RoleId);
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.Service.Implement;
using SS.SU.DTO;
using SS.Standard.Security.DAL;

namespace SS.Standard.Security.BLL.Implement
{
    public class UserService : ServiceBase<SuUser, int>, IUserService
    {
        #region IUserService Members

        public bool SignIn(string username, string password)
        {
           return   DaoProvider.UserDao.SignIn(username, password);
        }

        public void AccountLock(int userid)
        {
            throw new NotImplementedException();
        }

        public void UnAccountLock(int userid)
        {
            throw new NotImplementedException();
        }

        public void UpdateFail(string username, int num)
        {
            throw new NotImplementedException();
        }

        public void InsertUserSession(int userid, string sessionid, DateTime currenttime)
        {
            throw new NotImplementedException();
        }

        public void DeleteUserSession(int userid)
        {
            throw new NotImplementedException();
        }

        public void DeleteUserSession(DateTime currenttime)
        {
            throw new NotImplementedException();
        }

        public void SetUserCuerrentLanguage(int languageid)
        {
            throw new NotImplementedException();
        }

        public int UpdateUserSession(int userid, string sessionid, DateTime currenttime)
        {
            throw new NotImplementedException();
        }

        #endregion

        public override SS.Standard.Data.NHibernate.Dao.IDao<SuUser, int> GetBaseDao()
        {
            throw new NotImplementedException();
          //  return DaoProvider.UserDao;
        }
    }
}

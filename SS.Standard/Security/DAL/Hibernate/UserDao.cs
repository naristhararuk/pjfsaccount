using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.SU.DTO;

namespace SS.Standard.Security.DAL.Hibernate
{
    public class UserDao : IUserDao
    {
        #region IDao<SuUser,int> Members

        public bool FindExisting(SS.SU.DTO.SuUser domain)
        {
            throw new NotImplementedException();
        }

        public SS.SU.DTO.SuUser FindByIdentity(int id)
        {
            throw new NotImplementedException();
        }

        public SS.SU.DTO.SuUser FindProxyByIdentity(int id)
        {
            throw new NotImplementedException();
        }

        public IList<SS.SU.DTO.SuUser> FindAll()
        {
            throw new NotImplementedException();
        }

        public int Save(SS.SU.DTO.SuUser domain)
        {
            throw new NotImplementedException();
        }

        public void SaveOrUpdate(SS.SU.DTO.SuUser domain)
        {
            throw new NotImplementedException();
        }

        public void Update(SS.SU.DTO.SuUser domain)
        {
            throw new NotImplementedException();
        }

        public void Delete(SS.SU.DTO.SuUser domain)
        {
            throw new NotImplementedException();
        }

     

        #endregion

        #region IUserDao Members

        public bool SignIn(string username, string password)
        {
            bool flag = false;
            //#1 Encrypt password and compare on db


            //SuUser user = new SuUser();
            //user.UserName = username;
            //user.Password = password;

          //  List<SuUser> f = FindByExample(user);
           


            return flag;
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

        #region IDao<SuUser,int> Members


        public IList<SuUser> FindByExample(SuUser domain)
        {
            throw new NotImplementedException();
        }

        public IList<SuUser> FindByCriteria(SS.Standard.Data.NHibernate.QueryCreator.QueryPartCollector queryPartCollector)
        {
            throw new NotImplementedException();
        }

        public int CountByCriteria(SS.Standard.Data.NHibernate.QueryCreator.QueryPartCollector queryPartCollector)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

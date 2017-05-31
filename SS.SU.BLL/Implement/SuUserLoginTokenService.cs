using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.SU.DTO;
using SS.Standard.Data.NHibernate.Dao;
using SS.SU.DAL;
using SS.Standard.Security;

namespace SS.SU.BLL.Implement
{
    public partial class SuUserLoginTokenService : ServiceBase<SuUserLoginToken, Int64>, ISuUserLoginTokenService
    {
        public IUserAccount UserAccount { get; set; }

        public override IDao<SuUserLoginToken, Int64> GetBaseDao()
        {
            return DaoProvider.SuUserLoginTokenDao;
        }

        public string InsertToken()
        {
            string token = Guid.NewGuid().ToString();
            DaoProvider.SuUserLoginTokenDao.DeleteUserToken(UserAccount.UserName);
            DaoProvider.SuUserLoginTokenDao.Save(new SuUserLoginToken() { UserId = UserAccount.UserID, UserName = UserAccount.UserName, Token = token });

            return token;
        }

        public void DeleteToken(string userName)
        {
                DaoProvider.SuUserLoginTokenDao.DeleteUserToken(userName);
        }

        public SuUserLoginToken CheckUserAndToken(string userName, string token)
        {
            SuUserLoginToken userToken = DaoProvider.SuUserLoginTokenDao.FindAll().Where(t => t.UserName == userName && t.Token == token).FirstOrDefault();
            return userToken;
        }
    }
}

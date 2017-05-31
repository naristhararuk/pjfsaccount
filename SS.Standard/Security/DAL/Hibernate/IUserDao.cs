using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SS.SU.DTO;

namespace SS.Standard.Security.DAL.Hibernate
{
   
    public interface IUserDao : IDao<SuUser, int>
    {

        bool SignIn(string username, string password);
        void AccountLock(int userid);
        void UnAccountLock(int userid);
        void UpdateFail(string username, int num);
        void InsertUserSession(int userid, string sessionid, DateTime currenttime);
        void DeleteUserSession(int userid);
        void DeleteUserSession(DateTime currenttime);
        void SetUserCuerrentLanguage(int languageid);
        int UpdateUserSession(int userid, string sessionid, DateTime currenttime);


    }
   
}

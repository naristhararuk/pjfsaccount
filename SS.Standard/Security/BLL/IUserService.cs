using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.Service;
using SS.SU.DTO;

namespace SS.Standard.Security.BLL
{
 
    public interface IUserService : IService<SuUser, int>
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

        new void Delete(SuUser domain);
        new void SaveOrUpdate(SuUser domain);
        new void Update(SuUser domain);
        new IList<SuUser> FindAll();
        new SuUser FindByIdentity(int id);
        new int Save(SuUser domain);
    }
}

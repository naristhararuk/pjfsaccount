using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using SS.SU.DTO;
using SS.DB.DTO;

namespace SS.SU.BLL
{
    public interface IUserEngineService : IService<SuUser, long>
    {
        DbLanguage GetLanguage(short languageID);
        string SignIn(string userName, string passWord);
        bool SignIn(string userName);
        SuUser ChangePassword(long userID, string oldPassword, string newPassword, string confirmPassword, string programCode);
        SuUser ChangePassword(string userName, string oldPassword, string newPassword, string confirmPassword, string programCode);

        void SignOut(long userID);
        bool ResetFailTime(long userID);
        bool SetFailTime(string userName);
        bool IsLocked(string userName);
        void UnLockAccount(long userID);
        void LockAccount(long userID);
        new void Update(SuUser domain);
        UserSession getUserSessionList(long userID,short languageID);
        SuSession getSuSession(long userID);
        void setUserSession(long userID, string sessionID,DateTime TimeStamp);
        void DeleteUserSession(long userID);
        void UpdateUserSession(long userID,DateTime TimeStamp);
        void SessionTimeOut();
        void SignOutClearSession();
        void InitializeUserEngineService();
        void SaveTosuUserLog(string username,string status,string sessionID);

        int CountSession();
        void RemoveUserDict();
        void DropUsers(object application);
        void SyncUpdateUserData(string userName);
    }
}

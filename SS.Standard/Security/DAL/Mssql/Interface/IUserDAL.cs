using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Standard.Security.Mssql.DAL.Interface
{
    public interface IUserDAL  
    {
        void OpenConnection();
        void CloseConnection();
        void AccountLock(int UserId);
        void UnAccountLock(int UserId);
        void UpdateFail(string UserName, int Num);
        void InsertUserSession(int UserId, string SessionId, DateTime TimeStamp);
        void DeleteUserSession(int UserId);
        void DeleteUserSession(DateTime TimeStamp);
        void setLanguage(int languageID);
        int UpdateUserSession(int UserId, string SessionId, DateTime TimeStamp);
        UserSession SignIn(string UserName, string Pass);


    }
}

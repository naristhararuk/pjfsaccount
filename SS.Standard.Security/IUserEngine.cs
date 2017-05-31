using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.SU.DTO;

namespace SS.Standard.Security
{
    public interface IUserEngine
    {
        bool SignIn(string UserName, string Password);
        bool SignIn(string UserName);
		string CheckSignIn(string UserName, string Password);
		SuUser ChangePassword(long userID, string oldPassword, string newPassword, string confirmPassword, string programCode);
        SuUser ChangePassword(string userName, string oldPassword, string newPassword, string confirmPassword, string programCode);
        void SignOut(long userID);
        void SetLanguage(short languageID);
        bool WindowsIsAuthenticated();
        bool SetWindowsUserAuthenticated();
        void SyncUpdateUserData(string userName);
    }
}

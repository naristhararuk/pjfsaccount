using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

using SS.SU.BLL;
using SS.SU.DTO;
using System.Security.Principal;
using System.Security.Authentication;
using System.Threading;
using System.Web;

using SS.Standard.Security;

namespace SS.Standard.Security.Implement
{
    [Serializable]
    public partial class UserEngine : IUserEngine
    {
        public IUserEngineService UserEngineService { get; set; }
        public IUserAccount UserAccount { get; set; }
       

        public bool SignIn(string UserName, string Password)
        {
            bool flag=false;
            //Signin
            string userSigin = UserEngineService.SignIn(UserName, Password);
			if (userSigin == "success")
				flag = true;

            return flag;
        }
        public bool SignIn(string UserName)
        {
            return UserEngineService.SignIn(UserName);
        }
		public string CheckSignIn(string UserName, string Password)
		{
			return UserEngineService.SignIn(UserName, Password);
		}
        private void SignInWithWindowsAuthen(string UserName)
        {
            
        }
        public void SignOut(long userID)
        {
            UserEngineService.SignOut(userID);
        }
        public void SetLanguage(short languageID)
        {
            UserAccount.SetLanguage(languageID);
        }

        public SuUser ChangePassword(long userID, string oldPassword, string newPassword, string confirmPassword, string programCode)
        {
           return UserEngineService.ChangePassword(userID, oldPassword, newPassword, confirmPassword, programCode);
        }

        public SuUser ChangePassword(string userName, string oldPassword, string newPassword, string confirmPassword, string programCode)
        {
            return UserEngineService.ChangePassword(userName, oldPassword, newPassword, confirmPassword, programCode);
        }

        public void SyncUpdateUserData(string userName)
        {
            UserEngineService.SyncUpdateUserData(userName);
        }

        public bool WindowsIsAuthenticated()
        {
           // WindowsPrincipal wp = new WindowsPrincipal(WindowsIdentity.GetCurrent());

           // string xxx22 = wp.Identity.Name;


            IPrincipal PrincipalPermission = Thread.CurrentPrincipal;

            //System.Security.Principal.WindowsIdentity a = System.Security.Principal.WindowsIdentity.GetCurrent();

           // bool xxx = a.IsAuthenticated;
           // string bbb = a.Name;

           // // Change the security context to the currently logged in user from the nt authority\network service context
           //// System.Security.Principal.WindowsImpersonationContext impersonationContext = ((System.Security.Principal.WindowsIdentity)HttpContext.Current.User.Identity).Impersonate();

           // // Code that needs the current user goes here such as
           // string username = WindowsIdentity.GetCurrent().Name;

            // Revert back to normal context (i.e.  nt authority\network service account)
           // impersonationContext.Undo();


           // impersonationContext.Undo();



            return PrincipalPermission.Identity.IsAuthenticated;

        }
        public bool SetWindowsUserAuthenticated()
        {
            bool flag = false;
            IPrincipal PrincipalPermission = Thread.CurrentPrincipal;
            if (PrincipalPermission.Identity.IsAuthenticated)
            {
                string name = PrincipalPermission.Identity.Name;
                string NameAuthen = name.Split('\\')[1].ToString();
               flag = UserEngineService.SignIn(NameAuthen);
            }
            return flag;
        }
	}
}

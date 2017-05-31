using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System.Net;
using Microsoft.Web.Administration;
using System.Configuration;
using System.Web.SessionState;

using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DAL;
using SS.SU.Query;

using SS.SU.Helper;
using SS.DB.Query;
using SS.DB.DAL;
using SS.DB.DTO;

using System.Security.Principal;
using System.Threading;
using SS.Standard.Utilities;
using System.Web;
using System.DirectoryServices;
using log4net;
using System.Runtime.InteropServices;
using System.DirectoryServices.ActiveDirectory;
using System.Text.RegularExpressions;
using System.DirectoryServices.Protocols;
using System.Collections;

namespace SS.SU.BLL.Implement
{
    public partial class UserEngineService : ServiceBase<SuUser, long>, IUserEngineService
    {
        private IUserEngineDao userEngineDao;
        private ISuSessionDao userSessionDao;
        private IDbLanguageDao DbLanguageDao;
        public ParameterServices ParameterServices { get; set; }
        public ISuUserLogService SuUserLogService { get; set; }
        public ISuUserService SuUserService { get; set; }
        public ISuStatisticService SuStatisticService { get; set; }

        private static Queue globalCatalogCache = Queue.Synchronized(new Queue());

        private static ILog logger = log4net.LogManager.GetLogger(typeof(UserEngineService));

        public override IDao<SuUser, long> GetBaseDao()
        {
            return DaoProvider.UserEngineDao;
        }

        #region IUserService Members
        public UserEngineService()
        {
            userEngineDao = DaoProvider.UserEngineDao;
            userSessionDao = DaoProvider.SuSessionDao;
            DbLanguageDao = SsDbDaoProvider.DbLanguageDao;

        }
        public void setLanguage(short languageID)
        {

            throw new NotImplementedException();
        }


        public void InitializeUserEngineService()
        {
            UserSession us = null;

            us = (UserSession)System.Web.HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
            if (us == null)
            {
                us = new UserSession();
                us.IsAuthenticated = false;
                us.CurrentUserLanguageID = ParameterServices.DefaultLanguage;
                System.Web.HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] = us;
            }
            UserSession = us;

            #region Create Session Counter
            //modify by sirilak 11/05/2010
            try
            {
                if (us.IsAuthenticated)
                {
                    Dictionary<string, DateTime> dict = GetUserDict();
                    if (!dict.ContainsKey(HttpContext.Current.Session.SessionID))
                    {
                        lock (dict)
                        {
                            dict.Add(HttpContext.Current.Session.SessionID, DateTime.Now);
                        }
                    }
                    else
                    {
                        lock (dict)
                        {
                            dict[HttpContext.Current.Session.SessionID] = DateTime.Now;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion
        }

        public UserSession UserSession
        {
            get
            {
                LocalDataStoreSlot slot = Thread.GetNamedDataSlot(SessionEnum.WebSession.UserProfiles.ToString());
                return (UserSession)Thread.GetData(slot);
            }
            set
            {
                LocalDataStoreSlot slot = System.Threading.Thread.GetNamedDataSlot(SessionEnum.WebSession.UserProfiles.ToString());
                System.Threading.Thread.SetData(slot, value);
            }
        }

        string SignIn(SuUser user)
        {
            string strResult = string.Empty;

            if (user == null) return strResult;

            if (!user.IsAdUser && user.PasswordExpiryDate.HasValue)
            {
                if (user.PasswordExpiryDate.Value <= DateTime.Today)
                {
                    strResult = "PasswordExpired";

                    SaveTosuUserLog(user.UserName, "Password Expired", null);
                    return strResult;
                }
            }

            if (!user.IsAdUser && user.ChangePassword)
            {
                strResult = "RequiredChangePassword";
            }

            if (string.IsNullOrEmpty(strResult))
            {
                strResult = "success";
                bool IsPrivilege = SuUserService.IsPrivilege(user);
                Dictionary<string, DateTime> dict = GetUserDict();
                if (System.Web.HttpContext.Current != null)
                {
                    if (!dict.ContainsKey(System.Web.HttpContext.Current.Session.SessionID))
                    {
                        lock (dict)
                        {
                            dict.Add(System.Web.HttpContext.Current.Session.SessionID, DateTime.Now);
                        }
                    }
                }
            }

            UserSession userSession = getUserSessionList(user.Userid, user.Language.Languageid);

            //  Create SessoinID move to setUserSession
            this.DeleteUserSession(user.Userid);

            DateTime TimeStamp = DateTime.Now;
            //string SessoinId1 = SS.Standard.Utilities.Encryption.Md5Hash(user.Userid + TimeStamp.ToString());
            string SessoinId = string.Empty;


            if (System.Web.HttpContext.Current != null)
            {
                SessoinId = System.Web.HttpContext.Current.Session.SessionID;

                //insert & update sessionID
                setUserSession(user.Userid, SessoinId, TimeStamp);

                userSession.SessionID = SessoinId;
            }
            //clear oldsession timeout (move to job)
            //this.SessionTimeOut();



            bool isApproveVerifyPayment = false;
            bool isApproveVerifyDocument = false;

            bool isVerifyDocument = false;
            bool isVerifyPayment = false;
            bool isCounterCashier = false;
            bool isReceiveDocument = false;

            List<UserRoles> userRoles = userSession.UserRole;

            #region Set isApproveVerifyDocument
            //isApproveVerifyDocument
            foreach (UserRoles item in userRoles)
            {
                if (item.ApproveVerifyDocument)
                {
                    isApproveVerifyDocument = item.ApproveVerifyDocument;
                    break;
                }
            }
            userSession.IsApproveVerifyDocument = isApproveVerifyDocument;
            #endregion

            #region Set isApproveVerifyPayment
            foreach (UserRoles item in userRoles)
            {
                if (item.ApproveVerifyPayment)
                {
                    isApproveVerifyPayment = item.ApproveVerifyPayment;
                    break;
                }
            }
            userSession.IsApproveVerifyPayment = isApproveVerifyPayment;

            #endregion

            #region Set isVerifyDocument
            foreach (UserRoles item in userRoles)
            {
                if (item.VerifyDocument)
                {
                    isVerifyDocument = item.VerifyDocument;
                    break;
                }
            }
            userSession.IsVerifyDocument = isVerifyDocument;

            #endregion

            #region Set isVerifyPayment
            foreach (UserRoles item in userRoles)
            {
                if (item.VerifyPayment)
                {
                    isVerifyPayment = item.VerifyPayment;
                    break;
                }
            }
            userSession.IsVerifyPayment = isVerifyPayment;

            #endregion

            #region Set isReceiveDocument
            foreach (UserRoles item in userRoles)
            {
                if (item.ReceiveDocument)
                {
                    isReceiveDocument = item.ReceiveDocument;
                    break;
                }
            }
            userSession.IsReceiveDocument = isReceiveDocument;
            #endregion

            #region Set isCounterCashier
            foreach (UserRoles item in userRoles)
            {
                if (item.CounterCashier)
                {
                    isCounterCashier = item.CounterCashier;
                    break;
                }
            }
            userSession.IsCounterCashier = isCounterCashier;
            #endregion
            userSession.IsAccountant = false;
            userSession.IsPayment = false;
            #region Set IsAccountant
            if (isVerifyDocument || isReceiveDocument || isApproveVerifyDocument)
            {
                userSession.IsAccountant = true;
            }



            #endregion

            #region Set IsPayment
            if (isApproveVerifyPayment || isVerifyPayment || isCounterCashier)
            {
                userSession.IsPayment = true;
            }


            #endregion


            userSession.IsAuthenticated = true;
            UserSession = userSession;

            #region Set IsAdmin
            foreach (UserRoles item in userRoles)
            {
                if (item.RoleID.Equals(1))
                {
                    userSession.IsAdmin = true;
                    break;
                }
            }
            #endregion

            #region Set IsAllowMultipleApprovePayment
            foreach (UserRoles item in userRoles)
            {
                if (item.AllowMultipleApprovePayment)
                {
                    userSession.IsAllowMultipleApprovePayment = item.AllowMultipleApprovePayment;
                    break;
                }
            }
            userSession.IsVerifyPayment = isVerifyPayment;
            #endregion
            #region Set IsAllowMultipleApproveAccountant
            foreach (UserRoles item in userRoles)
            {
                if (item.AllowMultipleApproveAccountant)
                {
                    userSession.IsAllowMultipleApproveAccountant = item.AllowMultipleApproveAccountant;
                    break;
                }
            }
            userSession.IsVerifyPayment = isVerifyPayment;
            #endregion

            // set usersession to HttpContext/Thread
            if (System.Web.HttpContext.Current != null)
            {
                System.Web.HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()] = userSession;


                // Edit By Kookkla
                // For Insert Data To SuUserLog
                System.Web.HttpRequest currentRequest = System.Web.HttpContext.Current.Request;
                string ipAddress = currentRequest.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (ipAddress == null || ipAddress.ToLower() == "unknown")
                    ipAddress = currentRequest.ServerVariables["REMOTE_ADDR"];


                SuUser suUser = new SuUserService().FindByIdentity(userSession.UserID);
                SaveTosuUserLog(user.UserName, "Success", userSession.SessionID);


                SuStatisticService.IncreaseUser();


                //Dictionary<string, DateTime> dict = GetUserDict();

                //if (!SuUserService.IsPrivilege(user))
                //{
                //    if ((dict.Count) > ParameterServices.LimitLoginUserAmount)
                //    {
                //        RemoveUserDict();
                //        if (UserSession.CurrentUserLanguageID.Equals(1))
                //        {
                //            strResult = "ขณะนี้ ผู้ใช้งานบนระบบมีจำนวนเกินกว่าที่กำหนดไว้ กรุณารอสักครู่ และทำการ login ใหม่อีกครั้ง.";
                //        }
                //        else
                //        {
                //            strResult = "The number of concurrent users is exceeds the configured value. Please wait for a while and login again.";
                //        }
                //    }
                //}

            }

            return strResult;
        }

        public string SignIn(string userName, string password)
        {
            SuUser user = QueryProvider.SuUserQuery.GetSuUserByUserName(userName);
            if (user == null)
            {
                SaveTosuUserLog(userName, "Invalid UserName", null);
                return "InvalidUserIdOrPassword";
            }

            Dictionary<string, DateTime> dict = GetUserDict();
            if (!SuUserService.IsPrivilege(user))
            {
                if ((dict.Count) >= ParameterServices.LimitLoginUserAmount)
                {
                    if (UserSession.CurrentUserLanguageID.Equals(1))
                    {
                        return "ขณะนี้ ผู้ใช้งานบนระบบมีจำนวนเกินกว่าที่กำหนดไว้ กรุณารอสักครู่ และทำการ login ใหม่อีกครั้ง.";
                    }
                    else
                    {
                        return "The number of concurrent users is exceeds the configured value. Please wait for a while and login again.";
                    }
                }
            }

            int returnValue = 0;
            if (user.SetFailTime < user.FailTime)
            {
                SaveTosuUserLog(userName, "Account Locked", null);
                return "UserAccountIsLocked";
            }
            else if (user.IsAdUser)
            {
                returnValue = LoginWithActiveDirectory(userName, password);
                if (returnValue == 0)
                {
                    ResetFailTime(user.Userid);
                    return SignIn(user);
                }
                else if (returnValue == 1)
                {
                    return LoginFail(user);
                }
                else if (returnValue == 3)
                {
                    SaveTosuUserLog(userName, "Invalid UserName", null);
                    return "InvalidUserIdOrPassword"; ;
                }
                else if (returnValue == 4)
                {
                    return "UnableToLogin_FoundMultipleAccount";
                }
            }
            else
            {
                if (LoginWithDataBase(user, password))
                {
                    ResetFailTime(user.Userid);
                    return SignIn(user);
                }
                else
                {
                    return LoginFail(user);
                }
            }

            return string.Empty;
        }

        public bool SignIn(string UserName)
        {
            SuUser user = QueryProvider.SuUserQuery.GetSuUserByUserName(UserName);
            return SignIn(user).Equals("success");
        }

        /// <summary>
        /// function for login single password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>0= Success, 1=Login Fail, 3=Cannot found user account,4=Found multiple instance of the specified account</returns>
        public int LoginWithActiveDirectory(string username, string password)
        {
            string distinguishedName = string.Empty;
            string userDomainName = string.Empty;

            IntPtr token = GetLogonToken(ParameterServices.ActiveDirectoryLogin,
                    ParameterServices.ActiveDirectoryPassword, ParameterServices.ServiceAccountDomainName);

            WindowsImpersonationContext impersonateContext;
            impersonateContext = WindowsIdentity.Impersonate(token);

            if (globalCatalogCache.Count == 0)
            {

                try
                {
                    Forest curentForest = Forest.GetCurrentForest();
                    GlobalCatalogCollection catalogs = curentForest.FindAllGlobalCatalogs();
                    foreach (GlobalCatalog catalog in catalogs)
                    {
                        globalCatalogCache.Enqueue(catalog.Name);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                }

                string[] globalCatalogs = ParameterServices.GlobalCatalogs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string catalog in globalCatalogs)
                {
                    globalCatalogCache.Enqueue(catalog);
                }
            }

            object catalogName;
            while (globalCatalogCache.Count != 0)
            {
                catalogName = globalCatalogCache.Peek();
                logger.Info("Global Catalog: " + catalogName);
                DirectoryEntry entry = new DirectoryEntry();
                try
                {
                    entry.Path = string.Format("GC://{0}", catalogName);

                    DirectorySearcher search = new DirectorySearcher(entry);
                    search.Filter = "(SAMAccountName=" + username + ")";
                    search.PropertiesToLoad.Add("distinguishedName");
                    SearchResultCollection results = search.FindAll();
                    int result = results.Count;

                    if (result == 0)
                    {
                        logger.Error("Username :" + username + " => Unable to login. The system cannot found account.");
                        return 3;
                    }
                    else if (result > 1)
                    {
                        logger.Error("Username :" + username + " => Unable to login. The system has found multiple instance of the specified account.");
                        return 4;
                    }
                    else
                    {
                        distinguishedName = results[0].Properties["distinguishedName"][0].ToString();
                        string pattern = "(,dc=([^,]+))+";
                        Match m = Regex.Match(distinguishedName, pattern, RegexOptions.IgnoreCase);

                        for (int i = 0; i < m.Groups[2].Captures.Count; i++)
                        {
                            userDomainName += m.Groups[2].Captures[i].Value;
                            if (i != m.Groups[2].Captures.Count - 1)
                            {
                                userDomainName += ".";
                            }
                        }
                        logger.Info("Domain User : " + userDomainName);
                        break;
                    }

                }
                catch (System.Runtime.InteropServices.COMException ex)
                {
                    globalCatalogCache.Dequeue();
                    logger.Error("Server : " + catalogName + " => " + ex.ToString());
                }
                finally
                {
                    entry.Close();
                }
                break;
            }
            if (globalCatalogCache.Count == 0)
            {
                throw new Exception("No global catalog server functioning.");
            }
            impersonateContext.Undo();
            try
            {
                int returnedToken;
                IntPtr tokenUser;
                if (LogonUser(username, userDomainName, password, 2, 0, out returnedToken)) //Logon type = Interactive
                {
                    // The attempt was successful. Get the token.
                    tokenUser = new IntPtr(returnedToken);

                    WindowsImpersonationContext userImpersonate;
                    userImpersonate = WindowsIdentity.Impersonate(tokenUser);
                    userImpersonate.Undo();
                    return 0;
                }
                logger.Error("Wrong password => Username : " + username + ", Domain : " + userDomainName);
                logger.Info("Username : " + username + ", Password :" + password + ", Domain : " + userDomainName);
                return 1;
            }
            catch (Exception ex)
            {
                logger.Error("Username : " + username + ", Domain: " + userDomainName + " => " + ex.ToString());
                return 1;
            }
        }

        public bool LoginWithDataBase(SuUser user, string passwordInput)
        {
            if (SS.Standard.Utilities.Encryption.Md5Hash(passwordInput) == user.Password)
            {
                return true;
            }
            return false;
        }

        public string LoginFail(SuUser user)
        {
            SetFailTime(user.UserName);
            SaveTosuUserLog(user.UserName, "Wrong Password", null);
            if (user.SetFailTime == user.FailTime)
            {
                SaveTosuUserLog(user.UserName, "Account Locked", null);
                return "LoginFail_UserAccountLocked";
            }
            return "InvalidUserIdOrPassword";
        }
        #endregion

        #region IUserEngineService Members


        public bool ResetFailTime(long userID)
        {
            SuUser user = userEngineDao.FindByIdentity(userID);
            user.FailTime = 0;
            user.UpdDate = DateTime.Now;
            userEngineDao.Update(user);
            return true;

        }

        #endregion

        #region IUserEngineService Members


        public UserSession getUserSessionList(long userID, short languageID)
        {
            return QueryProvider.SuUserQuery.GetUserSessionList(userID, languageID)[0];
        }

        #endregion

        #region IUserEngineService Members


        public bool SetFailTime(string userName)
        {
            SuUser user = QueryProvider.SuUserQuery.GetSuUserByUserName(userName);
            if (user != null)
            {
                user.FailTime++;
                user.UpdDate = DateTime.Now;
                userEngineDao.Update(user);
            }
            return true;
        }

        #endregion

        #region IUserEngineService Members


        public bool IsLocked(string userName)
        {
            return QueryProvider.SuUserQuery.IsLocked(userName);
        }

        #endregion

        #region IUserEngineService Members


        public SuSession getSuSession(long userID)
        {
            return QueryProvider.SuSessionQuery.GetUserSession(userID);
        }

        #endregion

        #region IUserEngineService Members



        public void setUserSession(long userID, string sessionID, DateTime TimeStamp)
        {

            string iPAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (iPAddress == null || iPAddress == "")
            {
                iPAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                if (iPAddress == null || iPAddress == "")
                    iPAddress = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
            }

            SuSession user = QueryProvider.SuSessionQuery.GetUserSession(userID);
            if (user != null)
            {
                user.Sessionid = sessionID;
                user.Ip = iPAddress;
                user.TimeStamp = TimeStamp;
                userSessionDao.Update(user);

            }
            else
            {
                user = new SuSession();
                user.Userid = userID;
                user.Sessionid = sessionID;
                user.Ip = iPAddress;
                user.TimeStamp = TimeStamp;
                try
                {
                    userSessionDao.Save(user);
                }
                catch (Exception)
                {

                }



            }

        }

        #endregion

        #region IUserEngineService Members


        public void DeleteUserSession(long userID)
        {
            SuSession uSession = userSessionDao.FindByIdentity(userID);
            if (uSession != null)
                userSessionDao.Delete(uSession);
        }

        public void UpdateUserSession(long userID, DateTime TimeStamp)
        {
            SuSession user = userSessionDao.FindByIdentity(userID);
            user.TimeStamp = TimeStamp;
            userSessionDao.SaveOrUpdate(user);
        }

        #endregion

        #region IUserEngineService Members


        public void UnLockAccount(long userID)
        {
            SuUser user = userEngineDao.FindByIdentity(userID);
            user.FailTime = 0;
            user.Active = true;
            userEngineDao.SaveOrUpdate(user);
        }

        public void LockAccount(long userID)
        {
            SuUser user = userEngineDao.FindByIdentity(userID);
            // user.FailTime = user.SetFailTime.GetValueOrDefault(0);
            user.Active = false;
            userEngineDao.SaveOrUpdate(user);
        }

        #endregion

        #region IUserEngineService Members


        public void SessionTimeOut()
        {
            int strTimeout = ParameterServices.ApplicationTimeout; // modify by tom 03/03/2009 ParameterServices.ApplicationTimeout; //Comment by Desh 21-Jan-09 QueryProvider.DbParameterQuery.getParameterByGroupNo_SeqNo("1", "5");
            IList<SuSession> userSessionTimeOut = QueryProvider.SuSessionQuery.GetUserSessionTimeOutList(DateTime.Now.AddMinutes(-double.Parse((strTimeout < 0) ? "0" : strTimeout.ToString())));

            if (userSessionTimeOut != null && userSessionTimeOut.Count > 0)
            {
                userSessionDao.Delete(userSessionTimeOut[0]);
            }
        }

        #endregion

        #region IUserEngineService Members


        //public void SignOut(long userID)
        //{
        //    DoSingOut(userID);

        //    ClearSession();
        //}

        public void SignOutClearSession()
        {
            string url = HttpContext.Current.Request.Url.ToString();
            if (ParameterServices.EnableSSLOnLoginPage && HttpContext.Current.Request.Url.Scheme.Equals(Uri.UriSchemeHttp))
            {
                url = url.Replace(Uri.UriSchemeHttp, Uri.UriSchemeHttps);
            }

            RemoveUserDict();

            System.Web.HttpContext.Current.Session.Clear();
            System.Web.HttpContext.Current.ClearError();
            System.Web.HttpContext.Current.Session.Abandon();
            System.Web.HttpContext.Current.Response.Redirect(url.Replace(HttpContext.Current.Request.RawUrl, System.Web.VirtualPathUtility.ToAbsolute("~/Login.aspx")), true);
        }

        public void SignOut(long userID)
        {
            try
            {
                SuUser user = QueryProvider.SuUserQuery.FindByIdentity(userID);

                SuSession session = userSessionDao.FindByIdentity(userID);
                if (user == null) return;

                IList<SuUserLog> suUserLog = QueryProvider.SuUserLogQuery.FindSuUserLogByUserIdSessionID(user.UserName, session.Sessionid);
                if (suUserLog.Count > 0)
                {
                    suUserLog[suUserLog.Count - 1].SignOutDate = DateTime.Now;
                    SuUserLogService.Update(suUserLog[suUserLog.Count - 1]);
                }

                userSessionDao.Delete(session);
            }
            catch { }
        }


        #endregion

        #region IUserEngineService Members

        public DbLanguage GetLanguage(short languageID)
        {
            return DbLanguageDao.FindByIdentity(languageID);
        }

        #endregion

        public SuUser ChangePassword(long userID, string oldPassword, string newPassword, string confirmPassword, string programCode)
        {
            //ยังไม่เสร็จ ตรงนี้จะต้องไป compare จาก db ก่อนนะ
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            SuUser user = userEngineDao.FindByIdentity(userID);
            string encryptOldPassword = SS.Standard.Utilities.Encryption.Md5Hash(oldPassword);

            // Check User.
            if (userID == 0)
            {
                errors.AddError("ChangePassword.Error", new Spring.Validation.ErrorMessage("UserRequired"));
            }

            // Check oldPassword Required.
            if (string.IsNullOrEmpty(oldPassword))
            {
                errors.AddError("ChangePassword.Error", new Spring.Validation.ErrorMessage("OldPasswordRequired"));
            }
            else if (user != null && !user.Password.Equals(encryptOldPassword)) // Check Old Password is true.
            {
                errors.AddError("ChangePassword.Error", new Spring.Validation.ErrorMessage("OldPasswordNotMatch"));
            }

            // Check newPassword required.
            if (string.IsNullOrEmpty(newPassword))
            {
                errors.AddError("ChangePassword.Error", new Spring.Validation.ErrorMessage("NewPasswordRequired"));
            }

            // Check confirmPassword required.
            if (string.IsNullOrEmpty(confirmPassword))
            {
                errors.AddError("ChangePassword.Error", new Spring.Validation.ErrorMessage("ConfirmPasswordRequired"));
            }

            // Check newPassword and confirmPassword is match.
            if (!string.IsNullOrEmpty(newPassword) && !string.IsNullOrEmpty(confirmPassword))
            {
                if (!newPassword.Equals(confirmPassword))
                {
                    errors.AddError("ChangePassword.Error", new Spring.Validation.ErrorMessage("PasswordNotMatch"));
                }
            }

            // Check ChangePassword Not Allow.
            if (user != null && !user.ChangePassword)
            {
                if (user.AllowPasswordChangeDate.HasValue)
                {
                    if (user.AllowPasswordChangeDate.Value > DateTime.Today)
                    {
                        errors.AddError("ChangePassword.Error", new Spring.Validation.ErrorMessage("ChangePasswordNotAllow"));
                    }
                }
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            // Set User password information and Update Current User.
            int strMaxPasswordAge = ParameterServices.MaxPasswordAge; // modify by tom 03/03/2009 ParameterServices.maxPasswordAge;
            int strMinPasswordAge = ParameterServices.MinPasswordAge; // modify by tom 03/03/2009 ParameterServices.minPasswordAge;
            int strMinPasswordLen = ParameterServices.MinPasswordLength; // modify by tom 03/03/2009 ParameterServices.minPasswordLength;
            int strPasswordHistoryCount = ParameterServices.PasswordHistoryCount; // modify by tom 03/03/2009 ParameterServices.passwordHistoryCount;
            string strNotAllowPassword = ParameterServices.NotAllowPassword;


            // Validate not allow password.
            if ((newPassword.Equals(strNotAllowPassword, StringComparison.OrdinalIgnoreCase)) || (newPassword.Equals(user.UserName)))
            {
                errors.AddError("ChangePassword.Error", new Spring.Validation.ErrorMessage("PasswordNotAllow"));
            }
            // Validate password length.
            if (newPassword.Length < strMinPasswordLen)
            {
                errors.AddError("ChangePassword.Error", new Spring.Validation.ErrorMessage("PasswordLengthError"));
            }
            IList<SuPasswordHistory> passwordHistoryList = DaoProvider.SuPasswordHistoryDao.FindPasswordHistoryByUserId(userID);
            string encryptNewPassword = SS.Standard.Utilities.Encryption.Md5Hash(newPassword);
            // Validate History Password.
            if (passwordHistoryList.Count > 0)
            {
                if (passwordHistoryList.Count >= strPasswordHistoryCount)
                {
                    for (int i = 0; i < strPasswordHistoryCount; i++)
                    {
                        if (encryptNewPassword.Equals(passwordHistoryList[i].Password))
                        {
                            errors.AddError("ChangePassword.Error", new Spring.Validation.ErrorMessage("PasswordHistoryNotAllow"));
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < passwordHistoryList.Count; i++)
                    {
                        if (encryptNewPassword.Equals(passwordHistoryList[i].Password))
                        {
                            errors.AddError("ChangePassword.Error", new Spring.Validation.ErrorMessage("PasswordHistoryNotAllow"));
                            break;
                        }
                    }
                }
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            user.Password = encryptNewPassword;
            user.AllowPasswordChangeDate = DateTime.Today.AddDays(strMinPasswordAge);
            user.PasswordExpiryDate = DateTime.Today.AddDays(strMaxPasswordAge);
            user.ChangePassword = false;
            user.UpdBy = user.Userid;
            user.UpdDate = DateTime.Now;
            user.UpdPgm = programCode;
            userEngineDao.SaveOrUpdate(user);

            SuPasswordHistory passwordHistory = new SuPasswordHistory();
            passwordHistory.User = user;
            passwordHistory.ChangeDate = DateTime.Now;
            passwordHistory.Password = encryptNewPassword;
            passwordHistory.Active = true;
            passwordHistory.CreBy = user.Userid;
            passwordHistory.CreDate = DateTime.Now;
            passwordHistory.UpdBy = user.Userid;
            passwordHistory.UpdDate = DateTime.Now;
            passwordHistory.UpdPgm = programCode;

            IList<SuPasswordHistory> oldPasswordHistoryList = DaoProvider.SuPasswordHistoryDao.FindActivePasswordHistoryByUserId(user.Userid);
            // Set flag Active of user passwordHistory to false.
            foreach (SuPasswordHistory passwordHist in oldPasswordHistoryList)
            {
                passwordHist.Active = false;
                passwordHist.UpdBy = user.Userid;
                passwordHist.UpdDate = DateTime.Now;
                passwordHist.UpdPgm = programCode;
                DaoProvider.SuPasswordHistoryDao.SaveOrUpdate(passwordHist);
            }

            // Save new password History.
            DaoProvider.SuPasswordHistoryDao.Save(passwordHistory);

            return user;
        }

        public SuUser ChangePassword(string userName, string oldPassword, string newPassword, string confirmPassword, string programCode)
        {
            //ยังไม่เสร็จ ตรงนี้จะต้องไป compare จาก db ก่อนนะ
            SuUser user = null;
            string encryptOldPassword = SS.Standard.Utilities.Encryption.Md5Hash(oldPassword);
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            //SuUser user = userEngineDao.FindByIdentity(userID);
            if (userName.Length == 0)
            {
                errors.AddError("ChangePassword.Error", new Spring.Validation.ErrorMessage("UserRequired"));

            }
            else if (string.IsNullOrEmpty(oldPassword))   // Check oldPassword Required.
            {
                errors.AddError("ChangePassword.Error", new Spring.Validation.ErrorMessage("OldPasswordRequired"));
            }
            else
            {
                user = QueryProvider.SuUserQuery.GetSuUserByUserName_Password(userName, oldPassword);
            }
            // Check User.
            if (user == null)
            {
                errors.AddError("ChangePassword.Error", new Spring.Validation.ErrorMessage("OldPasswordWrong"));
            }
            else
            {

                // Check newPassword required.
                if (string.IsNullOrEmpty(newPassword))
                {
                    errors.AddError("ChangePassword.Error", new Spring.Validation.ErrorMessage("NewPasswordRequired"));
                }

                if (!user.Password.Equals(encryptOldPassword)) // Check Old Password is true.
                {
                    errors.AddError("ChangePassword.Error", new Spring.Validation.ErrorMessage("OldPasswordNotMatch"));
                }

                // Check confirmPassword required.
                if (string.IsNullOrEmpty(confirmPassword))
                {
                    errors.AddError("ChangePassword.Error", new Spring.Validation.ErrorMessage("ConfirmPasswordRequired"));
                }

                // Check newPassword and confirmPassword is match.
                if (!string.IsNullOrEmpty(newPassword) && !string.IsNullOrEmpty(confirmPassword))
                {
                    if (!newPassword.Equals(confirmPassword))
                    {
                        errors.AddError("ChangePassword.Error", new Spring.Validation.ErrorMessage("PasswordNotMatch"));
                    }
                }

                // Check ChangePassword Not Allow.
                if (!user.ChangePassword)
                {
                    if (user.AllowPasswordChangeDate.HasValue)
                    {
                        if (user.AllowPasswordChangeDate.Value > DateTime.Today)
                        {
                            errors.AddError("ChangePassword.Error", new Spring.Validation.ErrorMessage("ChangePasswordNotAllow"));
                        }
                    }
                }
                if (!errors.IsEmpty) throw new ServiceValidationException(errors);

                // Set User password information and Update Current User.
                int strMaxPasswordAge = ParameterServices.MaxPasswordAge; // modify by tom 03/03/2009 ParameterServices.maxPasswordAge;
                int strMinPasswordAge = ParameterServices.MinPasswordAge; // modify by tom 03/03/2009 ParameterServices.minPasswordAge;
                int strMinPasswordLen = ParameterServices.MinPasswordLength; // modify by tom 03/03/2009 ParameterServices.minPasswordLength;
                int strPasswordHistoryCount = ParameterServices.PasswordHistoryCount; // modify by tom 03/03/2009 ParameterServices.passwordHistoryCount;
                string strNotAllowPassword = ParameterServices.NotAllowPassword;


                // Validate not allow password.
                if ((newPassword.Equals(strNotAllowPassword, StringComparison.OrdinalIgnoreCase)) || (newPassword.Equals(user.UserName)))
                {
                    errors.AddError("ChangePassword.Error", new Spring.Validation.ErrorMessage("PasswordNotAllow"));
                }
                // Validate password length.
                if (newPassword.Length < strMinPasswordLen)
                {
                    errors.AddError("ChangePassword.Error", new Spring.Validation.ErrorMessage("PasswordLengthError"));
                }
                IList<SuPasswordHistory> passwordHistoryList = DaoProvider.SuPasswordHistoryDao.FindPasswordHistoryByUserId(user.Userid);
                string encryptNewPassword = SS.Standard.Utilities.Encryption.Md5Hash(newPassword);
                // Validate History Password.
                if (passwordHistoryList.Count > 0)
                {
                    if (passwordHistoryList.Count >= strPasswordHistoryCount)
                    {
                        for (int i = 0; i < strPasswordHistoryCount; i++)
                        {
                            if (encryptNewPassword.Equals(passwordHistoryList[i].Password))
                            {
                                errors.AddError("ChangePassword.Error", new Spring.Validation.ErrorMessage("PasswordHistoryNotAllow"));
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < passwordHistoryList.Count; i++)
                        {
                            if (encryptNewPassword.Equals(passwordHistoryList[i].Password))
                            {
                                errors.AddError("ChangePassword.Error", new Spring.Validation.ErrorMessage("PasswordHistoryNotAllow"));
                                break;
                            }
                        }
                    }
                }
                if (!errors.IsEmpty) throw new ServiceValidationException(errors);

                user.Password = encryptNewPassword;
                user.AllowPasswordChangeDate = DateTime.Today.AddDays(strMinPasswordAge);
                user.PasswordExpiryDate = DateTime.Today.AddDays(strMaxPasswordAge);
                user.ChangePassword = false;
                user.UpdBy = user.Userid;
                user.UpdDate = DateTime.Now;
                user.UpdPgm = programCode;
                userEngineDao.SaveOrUpdate(user);

                SuPasswordHistory passwordHistory = new SuPasswordHistory();
                passwordHistory.User = user;
                passwordHistory.ChangeDate = DateTime.Now;
                passwordHistory.Password = encryptNewPassword;
                passwordHistory.Active = true;
                passwordHistory.CreBy = user.Userid;
                passwordHistory.CreDate = DateTime.Now;
                passwordHistory.UpdBy = user.Userid;
                passwordHistory.UpdDate = DateTime.Now;
                passwordHistory.UpdPgm = programCode;

                IList<SuPasswordHistory> oldPasswordHistoryList = DaoProvider.SuPasswordHistoryDao.FindActivePasswordHistoryByUserId(user.Userid);
                // Set flag Active of user passwordHistory to false.
                foreach (SuPasswordHistory passwordHist in oldPasswordHistoryList)
                {
                    passwordHist.Active = false;
                    passwordHist.UpdBy = user.Userid;
                    passwordHist.UpdDate = DateTime.Now;
                    passwordHist.UpdPgm = programCode;
                    DaoProvider.SuPasswordHistoryDao.SaveOrUpdate(passwordHist);
                }

                // Save new password History.
                DaoProvider.SuPasswordHistoryDao.Save(passwordHistory);
                
                //signin on system
                SignIn(userName, newPassword);
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);


            return user;
        }

        public void SyncUpdateUserData(string userName)
        {
            //sync update user data to eXpense 4.7
            DaoProvider.SuUserDao.SyncUpdateUser(userName);
        }

        public void SaveTosuUserLog(string UserName, string Status, string SessionID)
        {
            if (System.Web.HttpContext.Current != null)
            {
                System.Web.HttpRequest currentRequest = System.Web.HttpContext.Current.Request;
                string ipAddress = currentRequest.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (ipAddress == null || ipAddress.ToLower() == "unknown")
                    ipAddress = currentRequest.ServerVariables["REMOTE_ADDR"];


                SuUserLog suUserLog = new SuUserLog();

                suUserLog.UserName = UserName;

                suUserLog.IPAddress = ipAddress + "/" + Dns.GetHostName();
                suUserLog.SignInDate = DateTime.Now;
                suUserLog.Status = Status;
                suUserLog.SessionID = SessionID;
                suUserLog.SignOutDate = null;
                SuUserLogService.Save(suUserLog);


            }

        }

        [DllImport("advapi32.dll")]
        public static extern bool LogonUser(string lpszUserName,
        string lpszDomain, string lpszPassword, int dwLogonType,
        int dwLogonProvider, out int phToken);
        public static IntPtr GetLogonToken(string user, string password, string machine)
        {
            int returnedToken;
            IntPtr token;
            // Try to log on.
            if (LogonUser(user, machine, password, 5, 0, out returnedToken))
            {
                // The attempt was successful. Get the token.
                token = new IntPtr(returnedToken);
                return token;
            }

            throw new Exception(String.Format("Unable to logon as {0}\\{1}.", machine, user));
        }

        private Dictionary<string, DateTime> GetUserDict()
        {
            if (System.Web.HttpContext.Current != null)
            {
                if (System.Web.HttpContext.Current.Application["currentuser"] == null)
                {
                    System.Web.HttpContext.Current.Application["currentuser"] = new Dictionary<string, DateTime>();
                }

                return (Dictionary<string, DateTime>)System.Web.HttpContext.Current.Application["currentuser"];
            }

            return new Dictionary<string, DateTime>();
        }

        public int CountSession()
        {
            Dictionary<string, DateTime> dict = GetUserDict();

            return dict == null ? 0 : dict.Count;
        }

        public void RemoveUserDict()
        {
            try
            {
                Dictionary<string, DateTime> dict = GetUserDict();
                if (dict.ContainsKey(System.Web.HttpContext.Current.Session.SessionID))
                {
                    lock (dict)
                    {
                        dict.Remove(System.Web.HttpContext.Current.Session.SessionID);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
        }

        public void DropUsers(object application)
        {
            logger.Info("Start DropUsers -> " + DateTime.Now);
            HttpApplicationState CurrentApplication = (HttpApplicationState)application;
            //if (CurrentApplication != null)
            //{
            Dictionary<string, DateTime> dict = (Dictionary<string, DateTime>)CurrentApplication["currentuser"];

            if (dict != null)
            {
                string[] keys = dict.Keys.ToArray();
                foreach (string sessionID in keys)
                {
                    if (dict[sessionID] < (DateTime.Now.Subtract(new TimeSpan(0, ParameterServices.ApplicationTimeout, 0))))
                    {
                        lock (dict)
                        {
                            dict.Remove(sessionID);
                        }
                        logger.Info("Remove dict -> SessionID : " + sessionID);
                    }
                }
            }
            //}

            logger.Info("End DropUsers -> " + DateTime.Now);
        }
    }

}

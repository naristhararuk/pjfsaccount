using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Configuration;

using System.Collections;
using System.ComponentModel;
using System.Security.Principal;
using SS.Standard.Security;
using SS.SU.Helper;
using SS.SU.BLL;
using SS.SU.DTO;
using SS.DB.Query;
using log4net;
using System.Threading;

namespace SCG.eAccounting.Web
{
    public class Global : System.Web.HttpApplication
    {
        public ISuStatisticService SuStatisticService { get; set; }
        private static ILog logger = log4net.LogManager.GetLogger(typeof(Global));

        protected void Application_Start(object sender, EventArgs e)
        {
            //System.Threading.Timer timer = new Timer(new TimerCallback(DropUsers), HttpContext.Current, 0, 5000);
            Thread threadTimer = new Thread(DropUsers);
            threadTimer.Start();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session.Timeout = ParameterServices.ApplicationTimeout;

            //DbParameter parameter           = new DbParameter();
            //LanguageDescription language    = new LanguageDescription();

            //this.Context.Application.Add("ApplicationTitle", parameter.getDbParameter(1, 1));
            //this.Context.Application.Add("HomeSite", parameter.getDbParameter(1, 2));            
            //this.Context.Application.Add("Permission", parameter.getDbParameter(1, 3));
            //this.Context.Application.Add("AcountLock", parameter.getDbParameter(1, 4));
            //this.Context.Application.Add("SessionTimeOut", parameter.getDbParameter(1, 5));
            //string lang_id = parameter.getDbParameter(1, 6);
            //this.Context.Application.Add("DefaultLanguageId", lang_id);
            //this.Context.Application.Add("DefaultLanguageName", language.GetLanguageName(lang_id));


        }

        protected void Session_End(object sender, EventArgs e)
        {
            // minus user logout
            try
            {
                #region clear user token cookies
                HttpCookie cookieUserToken = System.Web.HttpContext.Current.Request.Cookies["expUserToken"];
                if (cookieUserToken != null)
                {
                    cookieUserToken.Value = string.Empty;
                    System.Web.HttpContext.Current.Response.Cookies.Add(cookieUserToken);
                }

                HttpCookie cookieUserName = System.Web.HttpContext.Current.Request.Cookies["expUserName"];
                if (cookieUserName != null)
                {
                    cookieUserName.Value = string.Empty;
                    System.Web.HttpContext.Current.Response.Cookies.Add(cookieUserName);
                }

                HttpCookie cookieFromApp = System.Web.HttpContext.Current.Request.Cookies["expFromApp"];
                if (cookieFromApp != null)
                {
                    cookieFromApp.Value = string.Empty;
                    System.Web.HttpContext.Current.Response.Cookies.Add(cookieFromApp);
                }
                #endregion

                SS.SU.BLL.IUserEngineService service = (SS.SU.BLL.IUserEngineService)Spring.Context.Support.ContextRegistry.GetContext().GetObject("UserEngineService");

                service.RemoveUserDict();

                IUserAccount account = (IUserAccount)Spring.Context.Support.ContextRegistry.GetContext().GetObject("UserAccount");
                service.SignOut(account.UserID);
            }
            catch (Exception ex)
            {
                logger.Error("Session_End", ex);
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is System.Web.Security.FormsIdentity)
                    {
                        FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
                        FormsAuthenticationTicket ticket = id.Ticket;

                        // Get the stored user-data, in this case, our roles
                        string userData = ticket.UserData;
                        string[] roles = userData.Split(',');
                        HttpContext.Current.User = new GenericPrincipal(id, roles);
                    }
                }
            }
        }

        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            if (Context.Handler is IRequiresSessionState || Context.Handler is IReadOnlySessionState)
            {
                SS.SU.BLL.IUserEngineService service = (SS.SU.BLL.IUserEngineService)Spring.Context.Support.ContextRegistry.GetContext().GetObject("UserEngineService");
                service.InitializeUserEngineService();
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var serverError = Server.GetLastError() as HttpException;

            if (serverError != null)
            {
                int errorCode = serverError.GetHttpCode();

                if (errorCode == 404)
                {
                    Server.ClearError();
                    Server.Transfer("~/Errors/404.aspx");
                }
                else
                {
                    Server.Transfer("~/GenericErrorPage.aspx");
                }
            }
            //if (Server.GetLastError() != null)
            //    Server.Transfer("~/GenericErrorPage.aspx");
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        public void DropUsers()
        {
            while (true)
            {
                try
                {

                    SS.SU.BLL.IUserEngineService service = (SS.SU.BLL.IUserEngineService)Spring.Context.Support.ContextRegistry.GetContext().GetObject("UserEngineService");
                    service.DropUsers(this.Application);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                }
                Thread.Sleep(10000);
            }

        }
    }
}
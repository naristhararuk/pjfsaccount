using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SS.SU.DTO;

using SS.SU.BLL;
using SS.Standard.Security;
using SS.Standard.UI;

using SS.DB.Query;

using System.Threading;
using SS.Standard.Utilities;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class UserInfoWelcome : BaseUserControl
    {
        public IUserEngineService UserEngineService { get; set; }
        public IDbLanguageQuery DbLanguageQuery { get; set; }
        public IDbParameterQuery DbParameterQuery { get; set; }
        public ISuUserService SuUserService { get; set; }
        //public ISuUserLoginTokenService SuUserLoginTokenService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (UserAccount != null && UserAccount.Authentication)
                    SetUserInfo();
            }

            //if (ApplicationMode == "Archive")
            //{
            //    ctlArchiveButton.Visible = false;
            //    ctlBackButton.Visible = true;
            //}
            //else
            //{
            //    ctlArchiveButton.Visible = true;
            //    ctlBackButton.Visible = false;
            //}
        }
        private void SetUserInfo()
        {
            try
            {
                if (UserAccount.CurrentLanguageCode.ToString().Equals("th") || UserAccount.CurrentLanguageCode.ToString().Equals("th-TH") || UserAccount.CurrentLanguageCode.ToString().Equals("THA"))
                    ctlWelcomeText.Text = "สวัสดีคุณ";
                else
                    ctlWelcomeText.Text = "Welcome";

                SuUser user = SuUserService.FindByIdentity(UserAccount.UserID);
                ctl_Loged_FullName_Label.Text = user.EmployeeName;
                if (user.Company != null && user.Company.CompanyName != null)
                    ctlCompanyText.Text = user.Company.CompanyName;
                else
                {
                    if (UserAccount.CurrentLanguageCode.ToString().Equals("th") || UserAccount.CurrentLanguageCode.ToString().Equals("th-TH") || UserAccount.CurrentLanguageCode.ToString().Equals("THA"))
                        ctlCompanyText.Text = "ไม่พบข้อมูลบริษัท";
                    else
                        ctlCompanyText.Text = "Not Found Company";
                }

                if (user.IsAdUser)
                {
                    ctlButtonChangePassword.Style["visibility"] = "hidden";
            	}
                else
                {
                    ctlButtonChangePassword.Style["visibility"] = "visible";
                }
            }
            catch (Exception ex)
            {
                string url = HttpContext.Current.Request.Url.ToString();
                if (ParameterServices.EnableSSLOnLoginPage && HttpContext.Current.Request.Url.Scheme.Equals(Uri.UriSchemeHttp))
                {
                    url = url.Replace(Uri.UriSchemeHttp, Uri.UriSchemeHttps);
                }
                Response.Redirect(url.Replace(HttpContext.Current.Request.RawUrl, ResolveUrl("~/Login.aspx")));
            }
        }

        protected void ctlButtonChangePassword_Click(object sender, EventArgs e)
        {
            //ChangePassword.ShowPopup("UserInfoWelcome",false, UserAccount.UserName);
            ChangePassword ctlChangePassword = LoadPopup<ChangePassword>("~/UserControls/ChangePassword.ascx", ctlPopUpUpdatePanel);
            ctlChangePassword.ShowPopup("UserInfoWelcome", false, UserAccount.UserName);
        }

        protected void ctlLingButtonSignOut_Click(object sender, EventArgs e)
        {
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

                UserEngineService.SignOut(UserAccount.UserID);
                UserEngineService.SignOutClearSession();
            }
            catch
            {
                string url = HttpContext.Current.Request.Url.ToString();
                if (ParameterServices.EnableSSLOnLoginPage && HttpContext.Current.Request.Url.Scheme.Equals(Uri.UriSchemeHttp))
                {
                    url = url.Replace(Uri.UriSchemeHttp, Uri.UriSchemeHttps);
                }
                Response.Redirect(url.Replace(HttpContext.Current.Request.RawUrl, ResolveUrl("~/Login.aspx")));
            }

        }

        //protected void ctlArchiveButton_Click(object sender, EventArgs e)
        //{
        //    string token = SuUserLoginTokenService.InsertToken();
        //    string url = ParameterServices.ArchiveUrl;
        //    //Response.Write("<script>window.open('"+url+"?username="+UserAccount.UserName+"&token="+token+"')</script>");
        //    Response.Redirect(url + "?username=" + UserAccount.UserName + "&token=" + token);
            
        //}

        //protected void ctlBackButton_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect(ParameterServices.UrlLink + "/Menu.aspx");
        //}
    }
}
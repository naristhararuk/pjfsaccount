using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using SS.Standard.UI;
using SS.Standard.Security;

using SS.SU.BLL;
using SS.SU.DTO;
using SS.SU.Query;

using SS.DB.Query;
using SS.DB.DTO;

using SCG.eAccounting.Web.Helper;
using SS.Standard.Utilities;
using SCG.DB.Query;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class LogIn : BaseUserControl
    {
        public IDbParameterQuery DbParameterQuery { get; set; }
        public IDbLanguageQuery DbLanguageQuery { get; set; }
        public ISuUserService SuUserService { get; set; }
        public ISuUserLoginTokenService SuUserLoginTokenService { get; set; }
        public IUserEngine UserEngine { get; set; }

        public string WFID
        {
            set { ViewState["WFID"] = value; }
            get
            {
                if (ViewState["WFID"] == null)
                    return "";
                else
                    return ViewState["WFID"].ToString();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie cookieUserName = System.Web.HttpContext.Current.Request.Cookies["expUserName"];
            HttpCookie cookieUserToken = System.Web.HttpContext.Current.Request.Cookies["expUserToken"];

            if (!string.IsNullOrEmpty(Request.QueryString["wfid"]))
                WFID = Request.QueryString["wfid"].ToString();

            if ((cookieUserName != null && !string.IsNullOrEmpty(cookieUserName.Value.ToString())) && (cookieUserToken != null && !string.IsNullOrEmpty(cookieUserToken.Value.ToString())))
            {
                string url = HttpContext.Current.Request.Url.ToString();
                if (ParameterServices.EnableSSLOnLoginPage && IsHttps())
                    url = url.Replace(Uri.UriSchemeHttps, Uri.UriSchemeHttp);

                // check user & token in SuUserLoginToken and Delete token
                string userName = cookieUserName.Value.ToString();
                string token = cookieUserToken.Value.ToString();
                SuUserLoginToken userToken = SuUserLoginTokenService.CheckUserAndToken(userName, token);

                //clear value from cookies
                cookieUserName.Value = string.Empty;
                cookieUserToken.Value = string.Empty;

                System.Web.HttpContext.Current.Response.Cookies.Add(cookieUserName);
                System.Web.HttpContext.Current.Response.Cookies.Add(cookieUserToken);
                    
                if (userToken != null)
                {
                    SuUserLoginTokenService.DeleteToken(userName);

                    UserEngine.SignIn(userName);

                    if (string.IsNullOrEmpty(WFID))
                    {
                        Response.Redirect(url.Replace(HttpContext.Current.Request.RawUrl, ResolveUrl("~/Menu.aspx")), true);
                    }
                    else
                    {
                        Response.Redirect(url.Replace(HttpContext.Current.Request.RawUrl, ResolveUrl("~/Forms/SCG.eAccounting/Programs/DocumentView.aspx?wfid=" + WFID)), true);
                    }
                }

                // if not match user and token then show error page (create ArchiveLoginError.aspx) 
                Response.Redirect(url.Replace(HttpContext.Current.Request.RawUrl, ResolveUrl("~/LoginError.aspx")), true);

            }
            else
            {
                string url = HttpContext.Current.Request.Url.ToString();
                if (ParameterServices.EnableSSLOnLoginPage && !IsHttps())
                {
                    Response.Redirect(url.Replace(Uri.UriSchemeHttp, Uri.UriSchemeHttps), true);
                }
            }

            ctlPassword.Attributes.Add("OnKeyPress", "enter()");
            if (!IsPostBack)
            {
                ctlUserName.Focus();

                //if (!string.IsNullOrEmpty(Request.QueryString["wfid"]))
                //    WFID = Request.QueryString["wfid"].ToString();
            }
        }
        protected void forgetPassword_Click(object sender, ImageClickEventArgs e)
        {
            //ctlChangePassword.ShowPopup();
            ForgetPassword ctlForgetPassword = LoadPopup<ForgetPassword>("~/UserControls/ForgetPassword.ascx", ctlPopUpUpdatePanel);
            ctlForgetPassword.ShowPopup();
        }


        protected void ctlClose_Click(object sender, EventArgs e)
        {
            ModalPopupExtender1ShowMessage.Hide();
        }
        protected void imgLogin_Click(object sender, ImageClickEventArgs e)
        {
            GetAuthentication();
        }

        private void GetAuthentication()
        {
            string signinStatus = UserEngine.CheckSignIn(ctlUserName.Text.Trim(), ctlPassword.Text.Trim());
            if (signinStatus.Equals("PasswordExpired") || signinStatus.Equals("RequiredChangePassword"))
            {
                //ChangePassword.ShowPopup("Login", true, ctlUserName.Text.Trim());
                ChangePassword ctlChangePassword = LoadPopup<ChangePassword>("~/UserControls/ChangePassword.ascx", ctlPopUpUpdatePanel);
                ctlChangePassword.ShowPopup("Login", true, ctlUserName.Text.Trim());
            }
            else if (signinStatus.Equals("success"))
            {
                bool UseECC = ScgDbQueryProvider.DbCompanyQuery.getUseECCOfComOfUserByUserName(ctlUserName.Text.Trim());
                if (UseECC)
                {
                    string url = HttpContext.Current.Request.Url.ToString();
                    if (ParameterServices.EnableSSLOnLoginPage && IsHttps())
                        url = url.Replace(Uri.UriSchemeHttps, Uri.UriSchemeHttp);
                    if (string.IsNullOrEmpty(WFID))
                    {
                        Response.Redirect(url.Replace(HttpContext.Current.Request.RawUrl, ResolveUrl("~/Menu.aspx")), true);
                    }
                    else
                    {
                        Response.Redirect(url.Replace(HttpContext.Current.Request.RawUrl, ResolveUrl("~/Forms/SCG.eAccounting/Programs/DocumentView.aspx?wfid=" + WFID)), true);
                    }
                }
                else
                {
                    HttpCookie cookieUserToken = System.Web.HttpContext.Current.Request.Cookies["expUserToken"];
                    if (cookieUserToken != null)
                        System.Web.HttpContext.Current.Request.Cookies.Remove("expUserToken");

                    HttpCookie cookieUserName = System.Web.HttpContext.Current.Request.Cookies["expUserName"];
                    if (cookieUserName != null)
                        System.Web.HttpContext.Current.Request.Cookies.Remove("expUserName");

                    string token = SuUserLoginTokenService.InsertToken();
                    string url = ParameterServices.eXpenseUrl;

                    System.Web.HttpContext.Current.Response.Cookies.Add(new HttpCookie("expUserToken", token));
                    System.Web.HttpContext.Current.Response.Cookies.Add(new HttpCookie("expUserName", UserAccount.UserName));

                    //Response.Redirect(url + "?username=" + UserAccount.UserName + "&token=" + token);
                    Response.Redirect(url);
                }
            }
            else
            {
                ctlErrorValidationLabel.Text = GetMessage(signinStatus);
                ModalPopupExtender1ShowMessage.Show();
            }
        }

        // EDIT BY KOOKKLA
        protected void btnClose_Click(object sender, EventArgs e)
        {
            this.ModalPopupExtender1ShowMessage.Hide();
        }

        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            this.ModalPopupExtender1ShowMessage.Hide();
        }

        public bool IsHttps()
        {
            if (HttpContext.Current.Request.Url.Scheme.Equals(Uri.UriSchemeHttps))
                return true;
            else
                return false;
        }
    }
}
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AjaxControlToolkit;
using System.Collections.Generic;

using SS.Standard.UI;
using SS.SU.DTO;
using SS.SU.Query;
using SS.SU.DTO.ValueObject;
using SS.SU.BLL;
using SS.DB.Query;
namespace SCG.eAccounting.Web.UserControls
{
    public partial class LeftMenus : BaseUserControl
    {
        public ISuUserLoginTokenService SuUserLoginTokenService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            setMenuPermission();
        }

        public Accordion menuAccordion
        {
            get { return MenuAccordion; }
            set { MenuAccordion = value; }
        }

        private void setMenuPermission()
        {
            if (ApplicationMode == "Archive")
            {
                ctlDivArchiveLink.Style["display"] = "none";
                ctlDivEXpenseLink.Style["display"] = "block";

                RequestPane.Visible = false;
                ctlDivEmployeeDraftLink.Style["display"] = "none";
                ctlDivEmployeeInboxLink.Style["display"] = "none";

                //Set Permission   Accountant Pane
                if (UserAccount.IsAccountant)
                {
                    AccountantPane.Visible = true;
                    ctlDivAccountantInbox.Style["display"] = "none";
                    ctlDivAccountantHistory.Style["display"] = "block";
                    ctlDivAccountantSearch.Style["display"] = "block";
                    ctlDivAccountantMonitoring.Style["display"] = "block";
                }
                else
                {
                    AccountantPane.Visible = false;
                    ctlDivAccountantInbox.Style["display"] = "none";
                    ctlDivAccountantHistory.Style["display"] = "none";
                    ctlDivAccountantSearch.Style["display"] = "none";
                    ctlDivAccountantMonitoring.Style["display"] = "none";
                }

                //Set Permission Payment Pane
                if (UserAccount.IsPayment)
                {
                    PaymentPane.Visible = true;
                    ctlDivPaymentInbox.Style["display"] = "none";
                    ctlDivPaymentHistory.Style["display"] = "block";
                    ctlDivPaymentSearch.Style["display"] = "block";
                }
                else
                {
                    PaymentPane.Visible = false;
                    ctlDivPaymentInbox.Style["display"] = "none";
                    ctlDivPaymentHistory.Style["display"] = "none";
                    ctlDivPaymentSearch.Style["display"] = "none";
                }
            }
            else
            {
                ctlDivArchiveLink.Style["display"] = "block";
                ctlDivEXpenseLink.Style["display"] = "none";
                //Set Permission   Accountant Pane
                if (UserAccount.IsAccountant)
                {
                    AccountantPane.Visible = true;
                    AccountantInbox.Visible = true;
                    AccountantSearch.Visible = true;
                    AccountantSearch1.Visible = true;
                    AccountantMonitoring.Visible = true;
                }
                else
                {
                    AccountantPane.Visible = false;
                    AccountantInbox.Visible = false;
                    AccountantSearch.Visible = false;
                    AccountantSearch1.Visible = false;
                    AccountantMonitoring.Visible = false;
                }

                //Set Permission Payment Pane
                if (UserAccount.IsPayment)
                {
                    PaymentPane.Visible = true;
                    PaymentInbox.Visible = true;
                    PaymentSearch.Visible = true;
                    PaymentSearch1.Visible = true;
                }
                else
                {
                    PaymentPane.Visible = false;
                    PaymentInbox.Visible = false;
                    PaymentSearch.Visible = false;
                    PaymentSearch1.Visible = false;
                }

                //Disable or Enable MPA and Ca
                if (Convert.ToString(ParameterServices.EnableCA).ToLower() != "true")
                {

                    Car.Style["display"] = "none";
                }
                else {
                    Car.Style["display"] = "block";
                }


                if (Convert.ToString(ParameterServices.EnableMPA).ToLower() != "true")
                {
                    MobilePhone.Style["display"] = "none";
                }
                else
                {
                    MobilePhone.Style["display"] = "block";
                }
 
            }

            IList<ProgramRole> programRoleList = QueryProvider.SuProgramRoleQuery.FindSuProgramRoleByProgramCode(ProgramCodeEnum.Search);
            IList<ProgramRole> programTASearchRoleList = QueryProvider.SuProgramRoleQuery.FindSuProgramRoleByProgramCode(ProgramCodeEnum.TASearch);
            if (UserAccount.UserRole != null)
            {
                foreach (UserRoles role in UserAccount.UserRole)
                {
                    var matchRole = from p in programRoleList
                                    where p.RoleId == role.RoleID
                                    select p;

                    if (matchRole.Count<ProgramRole>() > 0)
                    {
                        Search.Visible = true;
                        break;
                    }
                    else
                    {
                        Search.Visible = false;
                    }
                }

                foreach (UserRoles role in UserAccount.UserRole)
                {
                    var matchRole = from p in programTASearchRoleList
                                    where p.RoleId == role.RoleID
                                    select p;

                    if (matchRole.Count<ProgramRole>() > 0)
                    {
                        ctlTASearchLink.Visible = true;
                        break;
                    }
                    else
                    {
                        ctlTASearchLink.Visible = false;
                    }
                }
            }
        }

        protected void ctlArchiveButton_Click(object sender, EventArgs e)
        {
            HttpCookie cookieUserToken = System.Web.HttpContext.Current.Request.Cookies["expUserToken"];
            if (cookieUserToken != null)
                System.Web.HttpContext.Current.Request.Cookies.Remove("expUserToken");

            HttpCookie cookieUserName = System.Web.HttpContext.Current.Request.Cookies["expUserName"];
            if (cookieUserName != null)
                System.Web.HttpContext.Current.Request.Cookies.Remove("expUserName");

            HttpCookie cookieFromApp = System.Web.HttpContext.Current.Request.Cookies["expFromApp"];
            if (cookieFromApp != null)
                System.Web.HttpContext.Current.Request.Cookies.Remove("expFromApp");

            string token = SuUserLoginTokenService.InsertToken();
            string url = ParameterServices.ArchiveUrl;

            System.Web.HttpContext.Current.Response.Cookies.Add(new HttpCookie("expUserToken", token));
            System.Web.HttpContext.Current.Response.Cookies.Add(new HttpCookie("expUserName", UserAccount.UserName));
            System.Web.HttpContext.Current.Response.Cookies.Add(new HttpCookie("expFromApp", "ecc"));
            
            Response.Redirect(url);
        }

        protected void ctlBackButton_Click(object sender, EventArgs e)
        {
            HttpCookie cookieUserToken = System.Web.HttpContext.Current.Request.Cookies["expUserToken"];
            if (cookieUserToken != null)
                System.Web.HttpContext.Current.Request.Cookies.Remove("expUserToken");

            HttpCookie cookieUserName = System.Web.HttpContext.Current.Request.Cookies["expUserName"];
            if (cookieUserName != null)
                System.Web.HttpContext.Current.Request.Cookies.Remove("expUserName");

            HttpCookie cookieFromApp = System.Web.HttpContext.Current.Request.Cookies["expFromApp"];
            if (cookieFromApp != null)
                System.Web.HttpContext.Current.Request.Cookies.Remove("expFromApp");

            string token = SuUserLoginTokenService.InsertToken();
            string url = ParameterServices.eXpenseUrl;

            System.Web.HttpContext.Current.Response.Cookies.Add(new HttpCookie("expUserToken", token));
            System.Web.HttpContext.Current.Response.Cookies.Add(new HttpCookie("expUserName", UserAccount.UserName));
            System.Web.HttpContext.Current.Response.Cookies.Add(new HttpCookie("expFromApp", "ecc"));

            Response.Redirect(url);
        }

        protected void ctlGoeXpenseButton_Click(object sender, EventArgs e)
        {
            HttpCookie cookieUserToken = System.Web.HttpContext.Current.Request.Cookies["expUserToken"];
            if (cookieUserToken != null)
                System.Web.HttpContext.Current.Request.Cookies.Remove("expUserToken");

            HttpCookie cookieUserName = System.Web.HttpContext.Current.Request.Cookies["expUserName"];
            if (cookieUserName != null)
                System.Web.HttpContext.Current.Request.Cookies.Remove("expUserName");

            HttpCookie cookieFromApp = System.Web.HttpContext.Current.Request.Cookies["expFromApp"];
            if (cookieFromApp != null)
                System.Web.HttpContext.Current.Request.Cookies.Remove("expFromApp");

            string token = SuUserLoginTokenService.InsertToken();
            string url = ParameterServices.eXpenseUrl;

            System.Web.HttpContext.Current.Response.Cookies.Add(new HttpCookie("expUserToken", token));
            System.Web.HttpContext.Current.Response.Cookies.Add(new HttpCookie("expUserName", UserAccount.UserName));
            System.Web.HttpContext.Current.Response.Cookies.Add(new HttpCookie("expFromApp", "ecc"));

            Response.Redirect(url);
        }
    }
}
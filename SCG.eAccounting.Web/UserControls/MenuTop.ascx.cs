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
using SS.Standard.UI;
using SS.SU.BLL;
using SS.SU.DTO.ValueObject;
using System.Collections.Generic;
using SS.DB.Query;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class MenuTop : BaseUserControl
    {
        public ISuRTENodeService SuRTENodeService { get; set; }
        short languageId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                languageId = UserAccount.CurrentLanguageID;

                IList<SuRTENodeSearchResult> listItem = SuRTENodeService.GetRTEContentList(languageId, "MainMenu", 3);
                rptMainMenu.DataSource = listItem;
                rptMainMenu.DataBind();
            }
        }

        protected void rptMainMenu_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            RepeaterItem item = e.Item;
            LinkButton ctlContent = (LinkButton)item.FindControl("ctlContent");
            Label ctlContentID = (Label)item.FindControl("ctlContentID");

            string newWindowUrl = "";
            string url = HttpContext.Current.Request.Url.ToString();
            if (UserAccount != null && UserAccount.Authentication)
            {
                if (ParameterServices.EnableSSLOnLoginPage && HttpContext.Current.Request.Url.Scheme.Equals(Uri.UriSchemeHttps))
                {
                    url = url.Replace(Uri.UriSchemeHttps, Uri.UriSchemeHttp);
                }
                if (ctlContentID.Text == "18" || ctlContent.Text.ToUpper() == "HOME" || ctlContent.Text == "หน้าหลัก")
                    newWindowUrl = url.Replace(HttpContext.Current.Request.RawUrl, ResolveUrl("~/Home.aspx"));
                else
                    newWindowUrl = url.Replace(HttpContext.Current.Request.RawUrl, ResolveUrl("~/Home.aspx?nodeId=" + ctlContentID.Text + "&languageId=" + UserAccount.CurrentLanguageID));
            }
            else
            {
                if (ParameterServices.EnableSSLOnLoginPage && HttpContext.Current.Request.Url.Scheme.Equals(Uri.UriSchemeHttp))
                {
                    url = url.Replace(Uri.UriSchemeHttp, Uri.UriSchemeHttps);
                }
                if (ctlContentID.Text == "18" || ctlContent.Text.ToUpper() == "HOME" || ctlContent.Text == "หน้าหลัก")
                    newWindowUrl = url.Replace(HttpContext.Current.Request.RawUrl, ResolveUrl("~/Login.aspx"));
                else
                    newWindowUrl = url.Replace(HttpContext.Current.Request.RawUrl, ResolveUrl("~/Login.aspx?nodeId=" + ctlContentID.Text + "&languageId=" + UserAccount.CurrentLanguageID));
            }

            Response.Redirect(newWindowUrl, true);
        }

        protected void rptMainMenu_ItemDataBound(object source, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton ctlContent = (LinkButton)item.FindControl("ctlContent");
                Label ctlContentID = (Label)item.FindControl("ctlContentID");

                string newWindowUrl = "";
                string url = HttpContext.Current.Request.Url.ToString();
                if (UserAccount != null && UserAccount.Authentication)
                {
                    if (ParameterServices.EnableSSLOnLoginPage && HttpContext.Current.Request.Url.Scheme.Equals(Uri.UriSchemeHttps))
                    {
                        url = url.Replace(Uri.UriSchemeHttps, Uri.UriSchemeHttp);
                    }
                    if (ctlContentID.Text == "18" || ctlContent.Text.ToUpper() == "HOME" || ctlContent.Text == "หน้าหลัก")
                        newWindowUrl = url.Replace(HttpContext.Current.Request.RawUrl, ResolveUrl("~/Home.aspx"));
                    else
                        newWindowUrl = url.Replace(HttpContext.Current.Request.RawUrl, ResolveUrl("~/Home.aspx?nodeId=" + ctlContentID.Text + "&languageId=" + UserAccount.CurrentLanguageID));
                }
                else
                {
                    if (ParameterServices.EnableSSLOnLoginPage && HttpContext.Current.Request.Url.Scheme.Equals(Uri.UriSchemeHttp))
                    {
                        url = url.Replace(Uri.UriSchemeHttp, Uri.UriSchemeHttps);
                    }
                    if (ctlContentID.Text == "18" || ctlContent.Text.ToUpper() == "HOME" || ctlContent.Text == "หน้าหลัก")
                        newWindowUrl = url.Replace(HttpContext.Current.Request.RawUrl, ResolveUrl("~/Login.aspx"));
                    else
                        newWindowUrl = url.Replace(HttpContext.Current.Request.RawUrl, ResolveUrl("~/Login.aspx?nodeId=" + ctlContentID.Text + "&languageId=" + UserAccount.CurrentLanguageID));
                }

                ctlContent.PostBackUrl = newWindowUrl;
            }
        }
    }
}
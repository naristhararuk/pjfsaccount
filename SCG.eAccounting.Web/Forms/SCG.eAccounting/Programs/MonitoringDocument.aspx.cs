using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.Query;
using SCG.eAccounting.DTO;
using SCG.eAccounting.Web.Helper;
using SS.DB.Query;
using SS.SU.BLL;

namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs
{
    public partial class MonitoringDocument : BasePage
    {
        public ISuUserLoginTokenService SuUserLoginTokenService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ctlMonitoringDocGrid.DataCountAndBind();
            }

        }
        protected int ctlMonitoringDocGrid_RequestCount()
        {
            string comCode = Request.Params["comCode"];
            string colnumber = Request.Params["colnumber"];
            string BuName = Request.Params["BUName"];
            string sortExpression = string.Empty;
            int RowCount = ScgeAccountingQueryProvider.DbMonitoringDocumentQuery.CountMonitoringDocumentQuery(comCode, colnumber, BuName, UserAccount.CurrentLanguageID, sortExpression);
            return RowCount;
        }

        protected IList<DbMonitoringDocument> ctlMonitoringDocGrid_RequestData(int startRow, int pageSize, string sortExpression)
        {
            string comCode = Request.Params["comCode"];
            string colnumber = Request.Params["colnumber"];
            string BuName = Request.Params["BUName"];
            IList<DbMonitoringDocument> data = ScgeAccountingQueryProvider.DbMonitoringDocumentQuery.DataMonitoringDocumentQuery(comCode, colnumber, BuName, UserAccount.CurrentLanguageID, sortExpression, startRow, pageSize);
            return data;
        }

        protected void ctlMonitoringDocGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("PopupDocument"))
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                long workflowID = UIHelper.ParseLong(ctlMonitoringDocGrid.DataKeys[rowIndex]["WorkflowID"].ToString());
                string type = ctlMonitoringDocGrid.DataKeys[rowIndex]["Type"].ToString();
                if (type == "ecc")
                {
                    string urlLink = ResolveUrl("~/Forms/SCG.eAccounting/Programs/DocumentView.aspx?wfid=" + workflowID.ToString());
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open(" + "'" + urlLink + "'" + ",'_blank');", true);
                }
                else if (type == "exp") 
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
                    System.Web.HttpContext.Current.Response.Cookies.Add(new HttpCookie("expFromApp", (ApplicationMode == "Archive" ? "expArchive" : "exp")));

                    string urlLinkEXP = url + "?wfid=" + workflowID.ToString();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open(" + "'" + urlLinkEXP + "'" + ",'_blank');", true);
                }
            }
        }

        protected void ctlMonitoringDocGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string attachment = ctlMonitoringDocGrid.DataKeys[e.Row.RowIndex]["CacheAttachment"].ToString();
                string boxID = ctlMonitoringDocGrid.DataKeys[e.Row.RowIndex]["CacheBoxID"].ToString();
                //IList<DocumentAttachment> documentAttachment = ScgeAccountingQueryProvider.DocumentAttachmentQuery.GetDocumentAttachmentByDocumentID(documentID);
                if (attachment == "True")
                {
                    Image ctlAttach = (Image)e.Row.FindControl("ctlAttach");

                    ctlAttach.Visible = true;
                }

                if (!string.IsNullOrEmpty(boxID))
                {
                    Image ctlFile = (Image)e.Row.FindControl("ctlFile");

                    ctlFile.Visible = true;
                }

                Literal amountLabel = (Literal)e.Row.FindControl("ctlAmount");
                Literal amountLocalCurrencyLabel = (Literal)e.Row.FindControl("ctlAmountLocalCurrency");
                Literal amountMainCurrencyLabel = (Literal)e.Row.FindControl("ctlAmountMainCurrency");

                DbMonitoringDocument data = e.Row.DataItem as DbMonitoringDocument;
                if (data != null)
                {
                    System.Drawing.Color defaultColor = System.Drawing.ColorTranslator.FromHtml("#777777");
                    System.Drawing.Color redColor = System.Drawing.ColorTranslator.FromHtml("#ff0000");

                    e.Row.Cells[9].ForeColor = data.CacheAmountLocalCurrency < 0 ? redColor : defaultColor;
                    e.Row.Cells[10].ForeColor = data.CacheAmountMainCurrency < 0 ? redColor : defaultColor;
                    e.Row.Cells[11].ForeColor = data.CacheAmountTHB < 0 ? redColor : defaultColor;
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCG.DB.Query;
using SCG.DB.DTO.ValueObject;
using SCG.DB.DTO;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.BLL;

namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Reports
{
    public partial class ReimbursementReportOutput : BasePage
    {
        public ISCGDocumentService SCGDocumentService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["pbId"]))
            {
                this.GeneratePDF();
            }
        }

        public void GeneratePDF()
        {
            VOPB voPb = ScgDbQueryProvider.DbPBQuery.GetDescription(UIHelper.ParseLong(Request["pbId"].ToString()), 2);
            string pbName = string.Empty;
            string companyName = string.Empty;
            string pbCode = string.Empty;
            if (voPb != null)
            {
                pbCode = voPb.PBCode;
                pbName = voPb.Description;
                DbCompany dbCompany = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(UIHelper.ParseLong(Request["companyId"].ToString()));
                companyName = dbCompany.CompanyName;
            }
            string markList = string.Empty;
            string unMarkList = string.Empty;
            string maxPaidDate = null;
            string minPaidDate = null;

            string keyCriteria = ApplicationMode + "reimbursementCriteriaCookie_" + UserAccount.UserID;
            string key = ApplicationMode + "reimbursementCookie_" + UserAccount.UserID;
            if (Request.Cookies[key] != null)
            {
                markList = Server.HtmlEncode(Request.Cookies[key]["markList"]);
                unMarkList = Server.HtmlEncode(Request.Cookies[key]["unmarkList"]);
            }

            if (!string.IsNullOrEmpty(Request.Cookies[keyCriteria]["PaidDateFrom"].ToString()))
            {
                minPaidDate = Server.HtmlEncode(Request.Cookies[keyCriteria]["PaidDateFrom"]);
            }
            if (!string.IsNullOrEmpty(Request.Cookies[keyCriteria]["PaidDateTo"].ToString()))
            {
                maxPaidDate = Server.HtmlEncode(Request.Cookies[keyCriteria]["PaidDateTo"]);
            }

            string filename = string.Format("{0}{1}{2}{3}", pbCode, DateTime.Now.Day.ToString("00"), DateTime.Now.Month.ToString("00"), DateTime.Now.Year);
            ReportHelper.FlushReport(this.Page, SCGDocumentService.GenerateReimbursementReport(markList, unMarkList, pbCode, pbName, companyName, UserAccount.UserName, maxPaidDate, minPaidDate), ReportHelper.ReportType.PDF, filename);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SCG.eAccounting.Report;
using SCG.eAccounting.Web.Helper;
using SS.DB.Query;
using SS.Standard.UI;

namespace SCG.eAccounting.Web.Forms.Interface.Programs
{
    public partial class PayrollReportOutput : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int month = int.Parse(Request["intMonth"] == null ? "0" : Request["intMonth"].ToString());
            int year = int.Parse(Request["enddate"] == null ? "0" : Request["intYear"].ToString());
            //Create datetime use to used to parameter values.
            DateTime dt = new DateTime(year, month, 1);
            DateTime startDate = dt.AddDays(14);
            DateTime endDate = dt.AddMonths(-1).AddDays(15);

            List<ReportParameter> rptParam = new List<ReportParameter>();

            ReportParameter paramCompanyID = new ReportParameter();
            paramCompanyID.ParamterName = "CompanyCode";
            paramCompanyID.ParamterValue = Request["CompanyCode"] == null ? string.Empty : Request["CompanyCode"].ToString();
            rptParam.Add(paramCompanyID);

            ReportParameter startdate = new ReportParameter();
            startdate.ParamterName = "startdate";
            startdate.ParamterValue = startDate.ToString("dd/MM/yyyy");
            rptParam.Add(startdate);

            ReportParameter enddate = new ReportParameter();
            enddate.ParamterName = "enddate";
            enddate.ParamterValue = endDate.ToString("dd/MM/yyyy");
            rptParam.Add(enddate);


            // find report file name later
            byte[] results = new FilesGenerator().GetByte(ParameterServices.ReportingURL, ParameterServices.ReportUserName, ParameterServices.ReportPassword, ParameterServices.ReportDomainName, ParameterServices.ReportFolderPath, "PayrollReport", rptParam, FilesGenerator.ExportType.PDF);
            ReportHelper.FlushReport(this.Page, results, ReportHelper.ReportType.PDF);
        }
    }
}

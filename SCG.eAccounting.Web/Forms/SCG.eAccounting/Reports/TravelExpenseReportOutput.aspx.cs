using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SCG.eAccounting.Report;
using SCG.eAccounting.Web.Helper;
using SS.DB.Query;

namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Reports
{
    public partial class TravelExpenseReportOutput : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<ReportParameter> rptParam = new List<ReportParameter>();

            ReportParameter paramUserName = new ReportParameter();
            paramUserName.ParamterName = "UserName";
            paramUserName.ParamterValue = Request["userName"] == null ? string.Empty : Request["userName"].ToString();
            rptParam.Add(paramUserName);

            ReportParameter paramCompany = new ReportParameter();
            paramCompany.ParamterName = "Company";
            paramCompany.ParamterValue = Request["Company"] == null ? "0" : Request["Company"].ToString();
            rptParam.Add(paramCompany);

            ReportParameter paramFromDate = new ReportParameter();
            paramFromDate.ParamterName = "FromDate";
            paramFromDate.ParamterValue = Request["FromDate"] == null ? "01/01/1990" : Request["FromDate"].ToString();
            rptParam.Add(paramFromDate);

            ReportParameter paramToDate = new ReportParameter();
            paramToDate.ParamterName = "ToDate";
            paramToDate.ParamterValue = Request["ToDate"] == null ? "31/12/2050" : Request["ToDate"].ToString();
            rptParam.Add(paramToDate);

            ReportParameter paramFromTravelDate = new ReportParameter();
            paramFromTravelDate.ParamterName = "FromTravelDate";
            paramFromTravelDate.ParamterValue = Request["FromTravelDate"] == null ? "01/01/1990" : Request["FromTravelDate"].ToString();
            rptParam.Add(paramFromTravelDate);

            ReportParameter paramToTravelDate = new ReportParameter();
            paramToTravelDate.ParamterName = "ToTravelDate";
            paramToTravelDate.ParamterValue = Request["ToTravelDate"] == null ? "31/12/2050" : Request["ToTravelDate"].ToString();
            rptParam.Add(paramToTravelDate);

            ReportParameter paramFromTANo = new ReportParameter();
            paramFromTANo.ParamterName = "FromTANo";
            paramFromTANo.ParamterValue = Request["FromTANo"] == null ? string.Empty : Request["FromTANo"].ToString();
            rptParam.Add(paramFromTANo);

            ReportParameter paramToTANo = new ReportParameter();
            paramToTANo.ParamterName = "ToTANo";
            paramToTANo.ParamterValue = Request["ToTANo"] == null ? string.Empty : Request["ToTANo"].ToString();
            rptParam.Add(paramToTANo);

            ReportParameter paramFromTraveller = new ReportParameter();
            paramFromTraveller.ParamterName = "FromTraveller";
            paramFromTraveller.ParamterValue = Request["FromTraveller"] == null ? "0" : Request["FromTraveller"].ToString();
            rptParam.Add(paramFromTraveller);

            ReportParameter paramToTraveller = new ReportParameter();
            paramToTraveller.ParamterName = "ToTraveller";
            paramToTraveller.ParamterValue = Request["ToTraveller"] == null ? "0" : Request["ToTraveller"].ToString();
            rptParam.Add(paramToTraveller);

            ReportParameter paramTAStatus = new ReportParameter();
            paramTAStatus.ParamterName = "TAStatus";
            paramTAStatus.ParamterValue = Request["TAStatus"] == null ? string.Empty : Request["TAStatus"].ToString();
            rptParam.Add(paramTAStatus);

            // find report file name later
            byte[] results = new FilesGenerator().GetByte(ParameterServices.ReportingURL, ParameterServices.ReportUserName, ParameterServices.ReportPassword, ParameterServices.ReportDomainName, ParameterServices.ReportFolderPath, "SummaryOfTravelAndExpenseReport", rptParam, FilesGenerator.ExportType.PDF);
            ReportHelper.FlushReport(this.Page, results, ReportHelper.ReportType.PDF);
        

        }
    }
}

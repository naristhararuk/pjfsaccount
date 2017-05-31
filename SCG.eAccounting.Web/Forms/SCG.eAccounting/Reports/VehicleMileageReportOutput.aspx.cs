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
    public partial class VehicleMileageReportOutput : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<ReportParameter> rptParam = new List<ReportParameter>();

            ReportParameter paramUserName = new ReportParameter();
            paramUserName.ParamterName = "UserName";
            paramUserName.ParamterValue = Request["userName"] == null ? string.Empty : Request["userName"].ToString();
            rptParam.Add(paramUserName);

            ReportParameter paramCompanyID = new ReportParameter();
            paramCompanyID.ParamterName = "CompanyID";
            paramCompanyID.ParamterValue = Request["CompanyID"] == null ? "0" : Request["CompanyID"].ToString();
            rptParam.Add(paramCompanyID);

            ReportParameter paramFromRequesterID = new ReportParameter();
            paramFromRequesterID.ParamterName = "FromRequesterID";
            paramFromRequesterID.ParamterValue = Request["FromRequesterID"] == null ? "0" : Request["FromRequesterID"].ToString();
            rptParam.Add(paramFromRequesterID);

            ReportParameter paramToRequesterID = new ReportParameter();
            paramToRequesterID.ParamterName = "ToRequesterID";
            paramToRequesterID.ParamterValue = Request["ToRequesterID"] == null ? "0" : Request["ToRequesterID"].ToString();
            rptParam.Add(paramToRequesterID);

            ReportParameter paramFromCarRegis = new ReportParameter();
            paramFromCarRegis.ParamterName = "FromCarRegis";
            paramFromCarRegis.ParamterValue = Request["FromCarRegis"] == null ? string.Empty : Request["FromCarRegis"].ToString();
            rptParam.Add(paramFromCarRegis);

            ReportParameter paramToCarRegis = new ReportParameter();
            paramToCarRegis.ParamterName = "ToCarRegis";
            paramToCarRegis.ParamterValue = Request["ToCarRegis"] == null ? string.Empty : Request["ToCarRegis"].ToString();
            rptParam.Add(paramToCarRegis);

            ReportParameter paramFromTANo = new ReportParameter();
            paramFromTANo.ParamterName = "FromTANo";
            paramFromTANo.ParamterValue = Request["FromTANo"] == null ? string.Empty : Request["FromTANo"].ToString();
            rptParam.Add(paramFromTANo);

            ReportParameter paramToTANo = new ReportParameter();
            paramToTANo.ParamterName = "ToTANo";
            paramToTANo.ParamterValue = Request["ToTANo"] == null ? string.Empty : Request["ToTANo"].ToString();
            rptParam.Add(paramToTANo);

            ReportParameter paramDocumentStatus = new ReportParameter();
            paramDocumentStatus.ParamterName = "DocumentStatus";
            paramDocumentStatus.ParamterValue = Request["DocumentStatus"] == null ? string.Empty : Request["DocumentStatus"].ToString();
            rptParam.Add(paramDocumentStatus);

            ReportParameter paramList1 = new ReportParameter();
            paramList1.ParamterName = "ParameterList";
            paramList1.ParamterValue = Request["ParameterList"] == null ? string.Empty : Request["ParameterList"].ToString();
            rptParam.Add(paramList1);

            ReportParameter paramList2 = new ReportParameter();
            paramList2.ParamterName = "ParameterList2";
            paramList2.ParamterValue = Request["ParameterList2"] == null ? string.Empty : Request["ParameterList2"].ToString();
            rptParam.Add(paramList2);

            // find report file name later
            byte[] results = new FilesGenerator().GetByte(ParameterServices.ReportingURL, ParameterServices.ReportUserName, ParameterServices.ReportPassword, ParameterServices.ReportDomainName, ParameterServices.ReportFolderPath, "VehicleMileageReport", rptParam, FilesGenerator.ExportType.PDF);
            ReportHelper.FlushReport(this.Page, results, ReportHelper.ReportType.PDF);
        }
    }
}

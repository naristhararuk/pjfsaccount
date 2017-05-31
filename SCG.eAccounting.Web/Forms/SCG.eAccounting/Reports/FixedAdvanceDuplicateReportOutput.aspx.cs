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
    public partial class FixedAdvanceDuplicateReportOutput : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<ReportParameter> rptParam = new List<ReportParameter>();

            //ReportParameter paramCompanyID = new ReportParameter();
            //paramCompanyID.ParamterName = "companyId";
            //paramCompanyID.ParamterValue = Request["companyId"] == null ? string.Empty : Request["companyId"].ToString();
            //rptParam.Add(paramCompanyID);

            //ReportParameter paramFromRequesterID = new ReportParameter();
            //paramFromRequesterID.ParamterName = "requesterId";
            //paramFromRequesterID.ParamterValue = Request["requesterId"] == null ? string.Empty : Request["requesterId"].ToString();
            //rptParam.Add(paramFromRequesterID);

            //ReportParameter paramFromDate = new ReportParameter();
            //paramFromDate.ParamterName = "fromDate";
            //paramFromDate.ParamterValue = Request["fromDate"] == null ? null : Request["fromDate"].ToString();
            //rptParam.Add(paramFromDate);

            //ReportParameter paramToDate = new ReportParameter();
            //paramToDate.ParamterName = "toDate";
            //paramToDate.ParamterValue = Request["toDate"] == null ? null : Request["toDate"].ToString();
            //rptParam.Add(paramToDate);

            ReportParameter paramgroupId = new ReportParameter();
            paramgroupId.ParamterName = "groupId";
            paramgroupId.ParamterValue = "5";
            rptParam.Add(paramgroupId);

            //ReportParameter paramToRequesterID = new ReportParameter();
            //paramToRequesterID.ParamterName = "ApproverID";
            //paramToRequesterID.ParamterValue = Request["ApproverID"] == null ? "0" : Request["ApproverID"].ToString();
            //rptParam.Add(paramToRequesterID);

            //ReportParameter paramList1 = new ReportParameter();
            //paramList1.ParamterName = "ParameterList";
            //paramList1.ParamterValue = Request["ParameterList"] == null ? string.Empty : Request["ParameterList"].ToString();
            //rptParam.Add(paramList1);


            // find report file name later
            byte[] results = new FilesGenerator().GetByte(ParameterServices.ReportingURL, ParameterServices.ReportUserName, ParameterServices.ReportPassword, ParameterServices.ReportDomainName, ParameterServices.ReportFolderPath, "FixAdvanceDuplicateReportV2", rptParam, FilesGenerator.ExportType.EXCEL);
            ReportHelper.FlushReport(this.Page, results, ReportHelper.ReportType.PDF);
        }
    }
}
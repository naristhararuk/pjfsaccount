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
    public partial class ExpenseStatementReportOutput : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<ReportParameter> rptParam = new List<ReportParameter>();

            ReportParameter paramUserName = new ReportParameter();
            paramUserName.ParamterName = "UserName";
            paramUserName.ParamterValue = Request["userName"] == null ? string.Empty : Request["userName"].ToString();
            rptParam.Add(paramUserName);

            ReportParameter paramFromEmpID = new ReportParameter();
            paramFromEmpID.ParamterName = "FromEmployeeCode";
            paramFromEmpID.ParamterValue = Request["fEmpCode"] == null ? "0" : Request["fEmpCode"].ToString();
            rptParam.Add(paramFromEmpID);

            ReportParameter paramToEmpID = new ReportParameter();
            paramToEmpID.ParamterName = "ToEmployeeCode";
            paramToEmpID.ParamterValue = Request["tEmpCode"] == null ? "0" : Request["tEmpCode"].ToString();
            rptParam.Add(paramToEmpID);

            ReportParameter paramFromDueDate = new ReportParameter();
            paramFromDueDate.ParamterName = "FromDueDate";
            paramFromDueDate.ParamterValue = Request["fDueDate"] == null ? "01/01/1990" : Request["fDueDate"].ToString();
            rptParam.Add(paramFromDueDate);

            ReportParameter paramToDueDate = new ReportParameter();
            paramToDueDate.ParamterName = "ToDueDate";
            paramToDueDate.ParamterValue = Request["tDueDate"] == null ? "31/12/2050" : Request["tDueDate"].ToString();
            rptParam.Add(paramToDueDate);

            ReportParameter paramDocState = new ReportParameter();
            paramDocState.ParamterName = "DocumentStatus";
            paramDocState.ParamterValue = Request["docState"] == null ? "" : Request["docState"].ToString();
            rptParam.Add(paramDocState);

            ReportParameter paramShowParam = new ReportParameter();
            paramShowParam.ParamterName = "ShowParam";
            paramShowParam.ParamterValue = Request["ShowParam"] == null ? "" : Request["ShowParam"].ToString();
            rptParam.Add(paramShowParam);

            // find report file name later
            byte[] results = new FilesGenerator().GetByte(ParameterServices.ReportingURL, ParameterServices.ReportUserName, ParameterServices.ReportPassword, ParameterServices.ReportDomainName, ParameterServices.ReportFolderPath, "EmployeeExpenseStatement", rptParam, FilesGenerator.ExportType.PDF);
            ReportHelper.FlushReport(this.Page, results, ReportHelper.ReportType.PDF);
        }
    }
}

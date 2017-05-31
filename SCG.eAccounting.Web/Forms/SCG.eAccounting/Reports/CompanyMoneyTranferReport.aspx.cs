using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCG.eAccounting.Report;
using SS.Standard.UI;
using SS.DB.Query;
using SCG.eAccounting.Web.Helper;
using Spring.Validation;
using System.Globalization;

namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Reports
{
    public partial class CompanyMoneyTranferReport : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ctlButtonPrint_Click(object sender, EventArgs e)
        {
            DateTimeFormatInfo mmddFormat = new CultureInfo("en-US", false).DateTimeFormat;

            if(string.IsNullOrEmpty(Calendar1.Text)){
                 ValidationErrors.AddError("Export.Error", new ErrorMessage("Fromdate is required."));
            } 
            if(string.IsNullOrEmpty(Calendar2.Text)){
                 ValidationErrors.AddError("Export.Error", new ErrorMessage("Todate is required."));
            }
            if (Calendar1.Text.Length != 0 && Calendar2.Text.Length != 0)
            {
                if (Calendar2.Value.Value < Calendar1.Value.Value)
                {
                    ValidationErrors.AddError("Export.Error", new ErrorMessage("Invalid Year."));
                }
                else
                {
                    List<ReportParameter> rptParam = new List<ReportParameter>();
                    DateTime _Fromdate = Calendar1.Value.Value;
                    DateTime _Todate = Calendar2.Value.Value;

                    ReportParameter paramFromDate = new ReportParameter();
                    paramFromDate.ParamterName = "fromdate";
                    paramFromDate.ParamterValue = _Fromdate.ToString("yyyy-MM-dd", mmddFormat);
                    rptParam.Add(paramFromDate);

                    ReportParameter paramToDate = new ReportParameter();
                    paramToDate.ParamterName = "Todate";
                    paramToDate.ParamterValue = _Todate.ToString("yyyy-MM-dd", mmddFormat);
                    rptParam.Add(paramToDate);

                    ReportParameter paramCompanyCode = new ReportParameter();
                    paramCompanyCode.ParamterName = "CompanyID";
                    if (string.IsNullOrEmpty(CompanyTextboxAutoComplete1.Text))
                    {
                        paramCompanyCode.ParamterValue = " ";
                    }
                    if (!string.IsNullOrEmpty(CompanyTextboxAutoComplete1.Text))
                    {
                        paramCompanyCode.ParamterValue = CompanyTextboxAutoComplete1.CompanyCode.Equals(string.Empty) ? string.Empty : CompanyTextboxAutoComplete1.CompanyCode;
                    }
                    rptParam.Add(paramCompanyCode);

                    byte[] results = new FilesGenerator().GetByte(ParameterServices.ReportingURL, ParameterServices.ReportUserName, ParameterServices.ReportPassword, ParameterServices.ReportDomainName, ParameterServices.ReportFolderPath, "CompanyMoneyTransfer", rptParam, FilesGenerator.ExportType.EXCEL);
                    ReportHelper.FlushReportEXCEL(this.Page, results, "CompanyMoneyTransferReport_[" + _Fromdate.ToString("yyyy-MM-dd", mmddFormat) + "]_[" + _Todate.ToString("yyyy-MM-dd", mmddFormat) + "]");
                }
            }
        }
    }
}

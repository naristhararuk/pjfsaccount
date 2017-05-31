using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using SS.Standard.Security;
using SS.Standard.UI;
using SS.Standard.Utilities;

using SCG.eAccounting.BLL;
using SCG.eAccounting.DAL;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.Query;
using SCG.eAccounting.Web.Helper;

//using SCG.FN.DTO;
//using SCG.FN.Query;

using SS.DB.DTO;
using SS.DB.Query;
using SCG.eAccounting.Web.UserControls;
using SCG.eAccounting.Report;

namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Reports
{
    public partial class FixedAdvanceCompareReport : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FixedAdvanceCompareReport_Viewer.Visible = false;
            FixedAdvanceCompareReportGraph_Viewer.Visible = false;
        }

        //protected void ctlPrint_Click(object sender, ImageClickEventArgs e)
        //{
        //    List<ReportParameter> rptParam = new List<ReportParameter>();

        //    byte[] results = new FilesGenerator().GetByte(ParameterServices.ReportingURL, ParameterServices.ReportUserName, ParameterServices.ReportPassword, ParameterServices.ReportDomainName, ParameterServices.ReportFolderPath, "FixedAdvanceDuplicateReportNew", rptParam, FilesGenerator.ExportType.PDF);
        //    ReportHelper.FlushReport(this.Page, results, ReportHelper.ReportType.EXCEL);
        //}

        protected void ctlPreview_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
                VOFixedAdvanceCompareReport vo = ctlFixedAdvanceCompareCriteria.BindCriteria();

                if (string.IsNullOrEmpty(vo.FromDate.ToString()))
                { errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("From Date is Required")); }

                if (string.IsNullOrEmpty(vo.ToDate.ToString()))
                { errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("To Date is Required")); }

                if (!errors.IsEmpty)
                {
                    throw new ServiceValidationException(errors);
                }
                else 
                {
                    DateTime fromDate = (DateTime)vo.FromDate;
                    DateTime toDate = (DateTime)vo.ToDate;
                    DateTime setYear = fromDate.AddMonths(12);
                    if (setYear < toDate || vo.FromDate > vo.ToDate)
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Period between From date and To Date must not be more than 1 year."));
                    }
                    if (vo.FromDate > vo.ToDate)
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("To Date must not be more than From date."));
                    }
                    if (!errors.IsEmpty) { throw new ServiceValidationException(errors); }

                    List<ReportParameters> rptParam = new List<ReportParameters>();

                    ReportParameters RequesterID = new ReportParameters();
                    RequesterID.Name = "requesterid";
                    RequesterID.Value = vo.RequesterID == string.Empty ? null : vo.RequesterID;
                    rptParam.Add(RequesterID);

                    ReportParameters CurrentUserId = new ReportParameters();
                    CurrentUserId.Name = "currentUserId";
                    CurrentUserId.Value = vo.ApproverID == string.Empty ? null : vo.ApproverID; //Approve = CurrentUserID
                    rptParam.Add(CurrentUserId);

                    ReportParameters DocumentNo = new ReportParameters();
                    DocumentNo.Name = "documentno";
                    DocumentNo.Value = vo.DocumentNo == string.Empty ? null : vo.DocumentNo.ToString();
                    rptParam.Add(DocumentNo);

                    ReportParameters FromDate = new ReportParameters();
                    FromDate.Name = "startDate";
                    FromDate.Value = UIHelper.ToDateString(vo.FromDate, "yyyy-MM-dd");
                    rptParam.Add(FromDate);

                    ReportParameters ToDate = new ReportParameters();
                    ToDate.Name = "endDate";
                    ToDate.Value = UIHelper.ToDateString(vo.ToDate, "yyyy-MM-dd");
                    rptParam.Add(ToDate);

                    //ReportParameters ReportType = new ReportParameters();
                    //ReportType.Name = "ReportType";
                    //ReportType.Value = vo.ReportType == null ? string.Empty : vo.ReportType.ToString();
                    //rptParam.Add(ToDate);

                    if (vo.ReportType == "2")
                    {
                        //Detail
                        FixedAdvanceCompareReportGraph_Viewer.Visible = false;

                        FixedAdvanceCompareReport_Viewer.Visible = true;

                        FixedAdvanceCompareReport_Viewer.InitializeReport();
                        FixedAdvanceCompareReport_Viewer.Parameters = rptParam;
                        FixedAdvanceCompareReport_Viewer.Visible = true;
                        FixedAdvanceCompareReport_Viewer.ShowReport();
                    }
                    else
                    {
                        //Graph
                        FixedAdvanceCompareReport_Viewer.Visible = false;

                        FixedAdvanceCompareReportGraph_Viewer.Visible = true;

                        FixedAdvanceCompareReportGraph_Viewer.InitializeReport();
                        FixedAdvanceCompareReportGraph_Viewer.Parameters = rptParam;
                        FixedAdvanceCompareReportGraph_Viewer.Visible = true;
                        FixedAdvanceCompareReportGraph_Viewer.ShowReport();
                    }
                }
            }
            catch (ServiceValidationException ex)
            {

                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                ctlUpdatePanelValidation.Update();
            }
           
        }
    }
}
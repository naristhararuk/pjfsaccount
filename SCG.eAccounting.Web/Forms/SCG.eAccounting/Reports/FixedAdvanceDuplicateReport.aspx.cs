using System;
using System.Collections.Generic;
using System.Web.UI;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.DTO.ValueObject;
using System.Text;
using SS.Standard.Utilities;

namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Reports
{
    public partial class FixedAdvanceDuplicateReport : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FixedAdvanceDuplicate_Viewer.Visible = false;
        }
        
        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
                VOFixedAdvanceDuplicateReport vo = ctlFixedAdvanceDuplicateCriteria.BindCriteria();
                
                if (string.IsNullOrEmpty(vo.BuCode))
                { errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Business Unit is Required")); }


                if (string.IsNullOrEmpty(vo.CompanyID))
                { errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Company is Required")); }

                if (string.IsNullOrEmpty(vo.FromDate.ToString()))
                { errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("From Date is Required")); }

                if (string.IsNullOrEmpty(vo.ToDate.ToString()))
                { errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("To Date is Required")); }

                if (!errors.IsEmpty) { throw new ServiceValidationException(errors); }
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

                    FixedAdvanceDuplicate_Viewer.Visible = true;
                    List<ReportParameters> rptParam = new List<ReportParameters>();
                    ReportParameters paramCompanyID = new ReportParameters();
                    paramCompanyID.Name = "companyId";
                    paramCompanyID.Value = vo.CompanyID;
                    rptParam.Add(paramCompanyID);

                    ReportParameters paramRequesterID = new ReportParameters();
                    paramRequesterID.Name = "requesterId";
                    paramRequesterID.Value = vo.RequesterID;
                    rptParam.Add(paramRequesterID);

                    ReportParameters paramApproverID = new ReportParameters();
                    paramApproverID.Name = "currentUserId";
                    paramApproverID.Value = vo.CurrenUserID;
                    rptParam.Add(paramApproverID);

                    ReportParameters paramFromDate = new ReportParameters();
                    paramFromDate.Name = "fromDate";
                    paramFromDate.Value = UIHelper.ToDateString(vo.FromDate, "yyyy-MM-dd");
                    rptParam.Add(paramFromDate);

                    ReportParameters paramToDate = new ReportParameters();
                    paramToDate.Name = "toDate";
                    paramToDate.Value = UIHelper.ToDateString(vo.ToDate, "yyyy-MM-dd"); ;
                    rptParam.Add(paramToDate);

                    ReportParameters paramGroupId = new ReportParameters();
                    paramGroupId.Name = "groupId";
                    paramGroupId.Value = vo.BuCode;
                    rptParam.Add(paramGroupId);

                    FixedAdvanceDuplicate_Viewer.InitializeReport();
                    FixedAdvanceDuplicate_Viewer.Parameters = rptParam;
                    FixedAdvanceDuplicate_Viewer.Visible = true;
                    FixedAdvanceDuplicate_Viewer.ShowReport();
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
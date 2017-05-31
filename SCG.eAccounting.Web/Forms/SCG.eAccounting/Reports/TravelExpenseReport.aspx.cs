using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SS.Standard.Security;
using SS.Standard.UI;
using SS.Standard.Utilities;

using SCG.eAccounting.BLL;
using SCG.eAccounting.DAL;
using SCG.eAccounting.DTO;
using SCG.eAccounting.Query;
using SCG.eAccounting.Web.Helper;

//using SCG.FN.DTO;
//using SCG.FN.Query;

using SS.DB.DTO;
using SS.DB.Query;

using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.Web.UserControls;
using SCG.eAccounting.Report;

namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Reports
{
	public partial class TravelExpenseReport : BasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            //ctlTravelExpenseCriteria.
            //ctlPreview.OnClientClick = "ctlPreview_Click";
            //ctlPreview.Click += new ImageClickEventHandler(ctlPreview_Click);
            //ctlPreview.CommandName = "ctlPreview_Click";
            TravelExpenseReport_Viewer.Visible = false;
           
        }

        protected void ctlPrint_Click(object sender, ImageClickEventArgs e)
        {
            VoTravelExpenseReport vo = ctlTravelExpenseCriteria.BindCriteria();
            //StringBuilder scriptBuilder = new StringBuilder("document.location.href = 'TravelExpenseReportOutput.aspx?");
            StringBuilder scriptBuilder = new StringBuilder("window.open('TravelExpenseReportOutput.aspx?");
            scriptBuilder.AppendFormat("UserName={0}", UserAccount.UserName);
            scriptBuilder.AppendFormat("&Company={0}", vo.CompanyID);
            scriptBuilder.AppendFormat("&FromDate={0}", UIHelper.ToDateString(vo.FromDate, "dd/MM/yyyy"));
            scriptBuilder.AppendFormat("&ToDate={0}", UIHelper.ToDateString(vo.ToDate, "dd/MM/yyyy"));
            scriptBuilder.AppendFormat("&FromTravelDate={0}", UIHelper.ToDateString(vo.FromTravelDate, "dd/MM/yyyy"));
            scriptBuilder.AppendFormat("&ToTravelDate={0}", UIHelper.ToDateString(vo.ToTravelDate, "dd/MM/yyyy"));
            scriptBuilder.AppendFormat("&FromTANo={0}", vo.FromTaDocumentNo);
            scriptBuilder.AppendFormat("&ToTANo={0}", vo.ToTaDocumentNo);
            scriptBuilder.AppendFormat("&FromTraveller={0}", vo.FromTraveller);
            scriptBuilder.AppendFormat("&ToTraveller={0}", vo.ToTraveller);
            scriptBuilder.AppendFormat("&TAStatus={0}", vo.TaStatus);
            scriptBuilder.Append("') ;");

            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), string.Empty, scriptBuilder.ToString(), true);

        }
        
        protected void ctlPreview_Click(object sender, ImageClickEventArgs e)
        {
            TravelExpenseReport_Viewer.Visible = true ;
            VoTravelExpenseReport vo = ctlTravelExpenseCriteria.BindCriteria();
            List<ReportParameters> rptParam = new List<ReportParameters>();

            ReportParameters paramUserName = new ReportParameters();
            paramUserName.Name = "UserName";
            paramUserName.Value = UserAccount.UserName;
            rptParam.Add(paramUserName);

            ReportParameters paramCompany = new ReportParameters();
            paramCompany.Name = "Company";
            paramCompany.Value = vo.CompanyID == null ? "0" : vo.CompanyID.ToString();
            rptParam.Add(paramCompany);

            ReportParameters paramFromDate = new ReportParameters();
            paramFromDate.Name = "FromDate";
            paramFromDate.Value = vo.FromDate == null ? string.Empty : UIHelper.ToDateString(vo.FromDate, "dd/MM/yyyy");
            rptParam.Add(paramFromDate);

            ReportParameters paramToDate = new ReportParameters();
            paramToDate.Name = "ToDate";
            paramToDate.Value = vo.ToDate == null ? string.Empty : UIHelper.ToDateString(vo.ToDate, "dd/MM/yyyy");
            rptParam.Add(paramToDate);

            ReportParameters paramFromTravelDate = new ReportParameters();
            paramFromTravelDate.Name = "FromTravelDate";
            paramFromTravelDate.Value = vo.FromTravelDate == null ? string.Empty : UIHelper.ToDateString(vo.FromTravelDate, "dd/MM/yyyy");
            rptParam.Add(paramFromTravelDate);

            ReportParameters paramToTravelDate = new ReportParameters();
            paramToTravelDate.Name = "ToTravelDate";
            paramToTravelDate.Value = vo.ToTravelDate == null ? string.Empty : UIHelper.ToDateString(vo.ToTravelDate, "dd/MM/yyyy");
            rptParam.Add(paramToTravelDate);

            ReportParameters paramFromTANo = new ReportParameters();
            paramFromTANo.Name = "FromTANo";
            paramFromTANo.Value = vo.FromTaDocumentNo == null ? string.Empty : vo.FromTaDocumentNo;
            rptParam.Add(paramFromTANo);

            ReportParameters paramToTANo = new ReportParameters();
            paramToTANo.Name = "ToTANo";
            paramToTANo.Value = vo.ToTaDocumentNo == null ? string.Empty : vo.ToTaDocumentNo;
            rptParam.Add(paramToTANo);

            ReportParameters paramFromTraveller = new ReportParameters();
            paramFromTraveller.Name = "FromTraveller";
            paramFromTraveller.Value = vo.FromTraveller == null ? string.Empty : vo.FromTraveller.ToString();
            rptParam.Add(paramFromTraveller);

            ReportParameters paramToTraveller = new ReportParameters();
            paramToTraveller.Name = "ToTraveller";
            paramToTraveller.Value = vo.ToTraveller == null ? string.Empty : vo.ToTraveller.ToString();
            rptParam.Add(paramToTraveller);

            ReportParameters paramTAStatus = new ReportParameters();
            paramTAStatus.Name = "TAStatus";
            paramTAStatus.Value = vo.TaStatus == null ? string.Empty : vo.TaStatus;
            rptParam.Add(paramTAStatus);

            ReportParameters paramShowParam1 = new ReportParameters();
            paramShowParam1.Name = "ShowParam1";
            paramShowParam1.Value = vo.ShowParam1;
            rptParam.Add(paramShowParam1);

            ReportParameters paramShowParam2 = new ReportParameters();
            paramShowParam2.Name = "ShowParam2";
            paramShowParam2.Value = vo.ShowParam2;
            rptParam.Add(paramShowParam2);

            TravelExpenseReport_Viewer.InitializeReport();
            TravelExpenseReport_Viewer.Parameters = rptParam;
            TravelExpenseReport_Viewer.Visible = true;
            TravelExpenseReport_Viewer.ShowReport();
        }
        
    }
}

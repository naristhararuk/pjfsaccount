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

namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Reports
{
	public partial class VehicleMileageReport : BasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            VehicleMileage_Viewer.Visible = false;
        }
        protected void ctlPrint_Click(object sender, ImageClickEventArgs e)
        {
            VoVehicleMileageReport vo = ctlVehicleMileageCriteria.BindCriteria();
            StringBuilder scriptBuilder = new StringBuilder("window.open('VehicleMileageReportOutput.aspx?");
            scriptBuilder.AppendFormat("UserName={0}", UserAccount.UserName);
            scriptBuilder.AppendFormat("&CompanyID={0}", vo.CompanyID);
            scriptBuilder.AppendFormat("&FromRequesterID={0}", vo.FromRequesterID);
            scriptBuilder.AppendFormat("&ToRequesterID={0}", vo.ToRequesterID);
            scriptBuilder.AppendFormat("&FromCarRegis={0}", vo.FromCarRegist);
            scriptBuilder.AppendFormat("&ToCarRegis={0}", vo.ToCarRegist);
            scriptBuilder.AppendFormat("&FromTANo={0}", vo.FromTaDocumentNo);
            scriptBuilder.AppendFormat("&ToTANo={0}", vo.ToTaDocumentNO);
            scriptBuilder.AppendFormat("&DocumentStatus={0}", vo.DocumentStatus);
            scriptBuilder.AppendFormat("&ParameterList={0}", vo.ParameterList);
            scriptBuilder.AppendFormat("&ParameterList2={0}", vo.ParameterList2);

            scriptBuilder.Append("');");
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), string.Empty, scriptBuilder.ToString(), true);
        }
        protected void ctlPreview_Click(object sender, ImageClickEventArgs e)
        {
            VehicleMileage_Viewer.Visible = true;
            VoVehicleMileageReport vo = ctlVehicleMileageCriteria.BindCriteria();
            List<ReportParameters> rptParam = new List<ReportParameters>();

            ReportParameters paramUserName = new ReportParameters();
            paramUserName.Name = "UserName";
            paramUserName.Value = UserAccount.UserName;
            rptParam.Add(paramUserName);

            ReportParameters paramCompanyID = new ReportParameters();
            paramCompanyID.Name = "CompanyID";
            paramCompanyID.Value = vo.CompanyID == null ? "0" : vo.CompanyID.ToString();
            rptParam.Add(paramCompanyID);

            ReportParameters paramFromRequesterID = new ReportParameters();
            paramFromRequesterID.Name = "FromRequesterID";
            paramFromRequesterID.Value = vo.FromRequesterID == null ? string.Empty : vo.FromRequesterID.ToString();
            rptParam.Add(paramFromRequesterID);

            ReportParameters paramToRequesterID = new ReportParameters();
            paramToRequesterID.Name = "ToRequesterID";
            paramToRequesterID.Value = vo.ToRequesterID == null ? string.Empty : vo.ToRequesterID.ToString();
            rptParam.Add(paramToRequesterID);

            ReportParameters paramFromCarRegis = new ReportParameters();
            paramFromCarRegis.Name = "FromCarRegis";
            paramFromCarRegis.Value = vo.FromCarRegist == null ? string.Empty : vo.FromCarRegist.ToString();
            rptParam.Add(paramFromCarRegis);

            ReportParameters paramToCarRegis = new ReportParameters();
            paramToCarRegis.Name = "ToCarRegis";
            paramToCarRegis.Value = vo.ToCarRegist == null ? string.Empty : vo.ToCarRegist.ToString();
            rptParam.Add(paramToCarRegis);

            ReportParameters paramFromTANo = new ReportParameters();
            paramFromTANo.Name = "FromTANo";
            paramFromTANo.Value = vo.FromTaDocumentNo == null ? string.Empty : vo.FromTaDocumentNo.ToString();
            rptParam.Add(paramFromTANo);

            ReportParameters paramToTANo = new ReportParameters();
            paramToTANo.Name = "ToTANo";
            paramToTANo.Value = vo.ToTaDocumentNO == null ? string.Empty : vo.ToTaDocumentNO.ToString();
            rptParam.Add(paramToTANo);

            ReportParameters paramDocumentStatus = new ReportParameters();
            paramDocumentStatus.Name = "DocumentStatus";
            paramDocumentStatus.Value = vo.DocumentStatus == null ? string.Empty : vo.DocumentStatus.ToString();
            rptParam.Add(paramDocumentStatus);

            ReportParameters paramList1 = new ReportParameters();
            paramList1.Name = "ParameterList";
            paramList1.Value = vo.ParameterList == null ? string.Empty : vo.ParameterList.ToString();
            rptParam.Add(paramList1);

            ReportParameters paramList2 = new ReportParameters();
            paramList2.Name = "ParameterList2";
            paramList2.Value = vo.ParameterList == null ? string.Empty : vo.ParameterList2.ToString();
            rptParam.Add(paramList2);

            VehicleMileage_Viewer.InitializeReport();
            VehicleMileage_Viewer.Parameters = rptParam;
            VehicleMileage_Viewer.Visible = true;
            VehicleMileage_Viewer.ShowReport();

        }
    }
}

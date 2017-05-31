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
using SCG.eAccounting.Query;
using SCG.eAccounting.Web.Helper;

//using SCG.FN.DTO;
//using SCG.FN.Query;

using SS.DB.DTO;
using SS.DB.Query;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.Web.UserControls;

namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Reports
{
	public partial class ExpenseStatementReport : BasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            ExpenseStatement_Viewer.Visible = false;
        }
        protected void ctlPrint_Click(object sender, ImageClickEventArgs e)
        {
            VOExpenseStatementReport vo = ctlExpenseStatementCriteria.BindCriteria();
            //StringBuilder scriptBuilder = new StringBuilder("document.location.href = 'ExpenseStatementReportOutput.aspx?");
            StringBuilder scriptBuilder = new StringBuilder("window.open('ExpenseStatementReportOutput.aspx?");
            scriptBuilder.AppendFormat("userName={0}", UserAccount.UserName);
            scriptBuilder.AppendFormat("&fEmpCode={0}", vo.FromExployeeCode);
            scriptBuilder.AppendFormat("&tEmpCode={0}", vo.ToEmployeeCode);
            scriptBuilder.AppendFormat("&fDueDate={0}", UIHelper.ToDateString(vo.FromDueDate,"dd/MM/yyyy"));
            scriptBuilder.AppendFormat("&tDueDate={0}", UIHelper.ToDateString(vo.ToDueDate, "dd/MM/yyyy"));
            scriptBuilder.AppendFormat("&docState={0}", vo.DocumentStatus);
            scriptBuilder.AppendFormat("&ShowParam={0}", vo.ShowParam);
            scriptBuilder.Append("') ;");
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), string.Empty, scriptBuilder.ToString(), true);
        }
        protected void ctlPreview_Click(object sender, ImageClickEventArgs e)
        {
            ExpenseStatement_Viewer.Visible = true;
            VOExpenseStatementReport vo = ctlExpenseStatementCriteria.BindCriteria();
            List<ReportParameters> rptParam = new List<ReportParameters>();

            ReportParameters paramUserName = new ReportParameters();
            paramUserName.Name = "UserName";
            paramUserName.Value = UserAccount.UserName;
            rptParam.Add(paramUserName);

            ReportParameters paramFromEmpID = new ReportParameters();
            paramFromEmpID.Name = "FromEmployeeCode";
            paramFromEmpID.Value = vo.FromExployeeCode == null ? "0" : vo.FromExployeeCode;
            rptParam.Add(paramFromEmpID);

            ReportParameters paramToEmpID = new ReportParameters();
            paramToEmpID.Name = "ToEmployeeCode";
            paramToEmpID.Value = vo.ToEmployeeCode == null ? "0" : vo.ToEmployeeCode;
            rptParam.Add(paramToEmpID);

            ReportParameters paramFromDueDate = new ReportParameters();
            paramFromDueDate.Name = "FromDueDate";
            paramFromDueDate.Value = vo.FromDueDate == null ? DateTime.MinValue.ToString() : UIHelper.ToDateString(vo.FromDueDate);
            rptParam.Add(paramFromDueDate);

            ReportParameters paramToDueDate = new ReportParameters();
            paramToDueDate.Name = "ToDueDate";
            paramToDueDate.Value = vo.ToDueDate == null ? DateTime.MaxValue.ToString() : UIHelper.ToDateString(vo.ToDueDate);
            rptParam.Add(paramToDueDate);

            ReportParameters paramDocState = new ReportParameters();
            paramDocState.Name = "DocumentStatus";
            paramDocState.Value = string.IsNullOrEmpty(vo.DocumentStatus) ? string.Empty : vo.DocumentStatus;
            rptParam.Add(paramDocState);

            ReportParameters paramShowParam = new ReportParameters();
            paramShowParam.Name = "ShowParam";
            paramShowParam.Value = string.IsNullOrEmpty(vo.ShowParam) ? string.Empty : vo.ShowParam;
            rptParam.Add(paramShowParam);

            ExpenseStatement_Viewer.InitializeReport();
            ExpenseStatement_Viewer.Parameters = rptParam;
            ExpenseStatement_Viewer.Visible = true;
            ExpenseStatement_Viewer.ShowReport();
        }
    }
}

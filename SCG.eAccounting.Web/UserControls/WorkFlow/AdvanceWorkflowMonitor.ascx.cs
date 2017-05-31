using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SS.Standard.UI;

using SS.Standard.WorkFlow;
using SS.Standard.WorkFlow.DTO;
using System.Drawing;

namespace SCG.eAccounting.Web.UserControls.WorkFlow
{
    public partial class AdvanceWorkflowMonitor : BaseUserControl, IWorkFlowMonitor
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Initialize(long wfId)
        {
            SS.Standard.WorkFlow.DTO.WorkFlow workFlow = SS.Standard.WorkFlow.Query.WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(wfId);
            switch (workFlow.CurrentState.Name)
            {
				case "Draft":
				case "WaitAR":
					workflowState.Cells[0].BgColor = Color.LightYellow.Name;
					break;
                case "WaitInitial":
                case "WaitTAInitial":
                case "WaitApprove":
                case "WaitTAApprove":
                    workflowState.Cells[1].BgColor = Color.LightYellow.Name;
                    workflowState.Cells[0].BgColor = Color.LightGreen.Name;
                    break;
                case "WaitVerify":
				case "WaitApproveVerify":
				case "Hold":
				case "WaitApproveRejection":
				case "WaitDocument":
					workflowState.Cells[2].BgColor = Color.LightYellow.Name;
					workflowState.Cells[1].BgColor = Color.LightGreen.Name;
					workflowState.Cells[0].BgColor = Color.LightGreen.Name;
					break;
				case "WaitPayment":
				case "WaitPaymentFromSAP":
				case "WaitRemittance":
					workflowState.Cells[3].BgColor = Color.LightYellow.Name;
					workflowState.Cells[2].BgColor = Color.LightGreen.Name;
					workflowState.Cells[1].BgColor = Color.LightGreen.Name;
					workflowState.Cells[0].BgColor = Color.LightGreen.Name;
					break;
                case "Outstanding":
                    workflowState.Cells[4].BgColor = Color.LightYellow.Name;
                    workflowState.Cells[3].BgColor = Color.LightGreen.Name;
                    workflowState.Cells[2].BgColor = Color.LightGreen.Name;
                    workflowState.Cells[1].BgColor = Color.LightGreen.Name;
                    workflowState.Cells[0].BgColor = Color.LightGreen.Name;
                    break;
				case "Complete":
					workflowState.Cells[4].BgColor = Color.LightGreen.Name;
					workflowState.Cells[3].BgColor = Color.LightGreen.Name;
					workflowState.Cells[2].BgColor = Color.LightGreen.Name;
					workflowState.Cells[1].BgColor = Color.LightGreen.Name;
					workflowState.Cells[0].BgColor = Color.LightGreen.Name;
					break;
            }           
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SS.Standard.WorkFlow.EventUserControl;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.Query;
using SS.Standard.WorkFlow.DTO.ValueObject;
using SCG.eAccounting.Web.Helper;
using SCG.DB.DTO;
using SCG.eAccounting.Query;
using SCG.eAccounting.DTO;
using SCG.DB.Query;

namespace SCG.eAccounting.Web.UserControls.WorkFlow
{
    public partial class ExpenseRemittanceDetail : BaseUserControl, IEventControl
    {

        public long DocumentID
        {
            get { return UIHelper.ParseLong(ViewState[ViewStateName.DocumentID].ToString()); }
            set { ViewState[ViewStateName.DocumentID] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctlBankMethod.CheckedChanged += new EventHandler(this.ReceivedMethodChanged);
            ctlCashierMethod.CheckedChanged += new EventHandler(this.ReceivedMethodChanged);
        }

        public void Initialize(long workFlowID)
        {
            DbCompany comp = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(
                ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(
                    WorkFlowQueryProvider.WorkFlowQuery.GetDocumentByWorkFlowID(
                        workFlowID).DocumentID).CompanyID.CompanyID);
            if (comp.UseSpecialPayIn)
            {
                ctlBankMethod.Enabled = true;
            }
            else
            {
                ctlBankMethod.Enabled = false;
            }
            ctlCashierMethod.Checked = true;
            ctlGLAccount.Text = string.Empty;
            ctlKBankDetailPanel.Visible = false;
        }

        private void ReceivedMethodChanged(object sender, EventArgs e)
        {
            if (ctlBankMethod.Checked)
            {
                long workFlowID = UIHelper.ParseLong(Request["wfid"]);
                DbCompany comp = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(
                ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(
                    WorkFlowQueryProvider.WorkFlowQuery.GetDocumentByWorkFlowID(
                        workFlowID).DocumentID).CompanyID.CompanyID);

                ctlKBankDetailPanel.Visible = true;
                ctlGLAccount.Text = comp.DefaultGLAccount;
                ctlValueDate.Value = DateTime.Today;
            }
            else
            {
                ctlKBankDetailPanel.Visible = false;
                ctlGLAccount.Text = string.Empty;
                ctlValueDate.Value = DateTime.Today;
            }
        }


        #region IEventControl Members
        public object GetEventData(int workFlowStateEventID)
        {
            ExpenseRemittanceDetailResponse returnObj = new ExpenseRemittanceDetailResponse();
            returnObj.WorkFlowStateEventID = workFlowStateEventID;
            if (ctlCashierMethod.Checked)
            {
                returnObj.ReceivedMethod = ctlCashierMethod.Text;
            }
            else
            {
                returnObj.ReceivedMethod = ctlBankMethod.Text;
                returnObj.GLAccount = ctlGLAccount.Text;
                returnObj.ValueDate = ctlValueDate.Value;
            }
            return returnObj;
        }
        #endregion

        #region IEventControl Members

        public int WorkFlowStateEventID { get; set; }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.WorkFlow.EventUserControl;
using SCG.eAccounting.DTO;
using SCG.eAccounting.Query;
using SCG.eAccounting.Web.Helper;
using SS.SU.DTO;
using SS.SU.Query;
using SS.Standard.Utilities;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.BLL;
using SS.Standard.UI;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.Query;

namespace SCG.eAccounting.Web.UserControls.WorkFlow
{
    public partial class OverRole : BaseUserControl, IEventControl
    {
        public int WorkFlowStateEventID { get; set; }
        public IDocumentInitiatorService DocumentInitiatorService { get; set; }
        public long DocumentID
        {
            get { return UIHelper.ParseLong(ViewState[ViewStateName.DocumentID].ToString()); }
            set { ViewState[ViewStateName.DocumentID] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        public void Initialize(long workFlowID)
        {
            Document document = WorkFlowQueryProvider.WorkFlowQuery.GetDocumentByWorkFlowID(workFlowID);
            if (document != null)//Advance DocumentType when 1 is Domestic when 5 is Foreign
            {
                this.DocumentID = document.DocumentID;
                this.BindOverRoleGridview();
            }
        }
        public void SubmitResponse()
        {
            IList<DocumentInitiator> OverroleInitiators = new List<DocumentInitiator>();
            foreach (GridViewRow row in ctlOverRoleGridview.Rows)
            {
                Label ctlInitiatorLabel = (Label)ctlOverRoleGridview.Rows[row.RowIndex].FindControl("ctlInitiatorLabel");
                Label ctlEmailLabel = (Label)ctlOverRoleGridview.Rows[row.RowIndex].FindControl("ctlEmailLabel");
                CheckBox ctlSMSChk = (CheckBox)ctlOverRoleGridview.Rows[row.RowIndex].FindControl("ctlSMSChk");
                CheckBox ctlSkipChk = (CheckBox)ctlOverRoleGridview.Rows[row.RowIndex].FindControl("ctlSkipChk");
                TextBox ctlSkipReasonText = (TextBox)ctlOverRoleGridview.Rows[row.RowIndex].FindControl("ctlSkipReasonText");

                long DocumentInitiatorID = UIHelper.ParseLong(ctlOverRoleGridview.DataKeys[row.RowIndex]["InitiatorID"].ToString());
                long UserID = UIHelper.ParseLong(ctlOverRoleGridview.DataKeys[row.RowIndex]["UserID"].ToString());

                DocumentInitiator documentInitiator = ScgeAccountingQueryProvider.DocumentInitiatorQuery.FindProxyByIdentity(DocumentInitiatorID);

                documentInitiator.IsSkip = ctlSkipChk.Checked;
                documentInitiator.SkipReason = ctlSkipReasonText.Text;
                OverroleInitiators.Add(documentInitiator);
                //DocumentInitiatorService.SaveOrUpdate(documentInitiator);
                UpdatePanelOverRole.Update();
            }
            DocumentInitiatorService.UpdateDocumentInitiatorWhenOverRole(OverroleInitiators);
        }
        protected void ctlOverRoleGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox ctlSkipChk = (CheckBox)e.Row.FindControl("ctlSkipChk");
                long initiatorID = UIHelper.ParseLong(ctlOverRoleGridview.DataKeys[e.Row.RowIndex]["InitiatorID"].ToString());
                DocumentInitiator initialtor = ScgeAccountingQueryProvider.DocumentInitiatorQuery.FindByIdentity(initiatorID);
                if (initialtor.DoApprove && initialtor != null)
                {
                    ctlSkipChk.Enabled = false;
                }
            }
            //Auto increment No. in gridview.
            Label ctlNoLabel = e.Row.FindControl("ctlNoLabel") as Label;
            if (ctlNoLabel != null)
            {
                if (e.Row.RowIndex != -1)
                {
                    ctlNoLabel.Text = ((ctlOverRoleGridview.PageSize * ctlOverRoleGridview.CustomPageIndex) + (e.Row.RowIndex + 1)).ToString();
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                long documentInitiatorID = UIHelper.ParseLong(ctlOverRoleGridview.DataKeys[e.Row.RowIndex]["InitiatorID"].ToString());
                DocumentInitiator documentInitiator = ScgeAccountingQueryProvider.DocumentInitiatorQuery.FindProxyByIdentity(documentInitiatorID);
                if (documentInitiator.DoApprove)
                {
                    CheckBox ctlSkipChk = (CheckBox)e.Row.FindControl("ctlSkipChk");
                    TextBox ctlSkipReasonText = (TextBox)e.Row.FindControl("ctlSkipReasonText");

                    ctlSkipChk.Enabled = false;
                    ctlSkipReasonText.Enabled = false;
                }
            }
        }
        public void BindOverRoleGridview ()
        {
            IList<DocumentInitiatorLang> initiatorList = ScgeAccountingQueryProvider.DocumentInitiatorQuery.GetDocumentInitiatorByDocumentIDAndInitiatorType(this.DocumentID);
            ctlOverRoleGridview.DataSource = initiatorList;
            ctlOverRoleGridview.DataBind();
            UpdatePanelOverRole.Update();
        }
        public object GetEventData(int workFlowStateEventID)
        {
            this.SubmitResponse();
            return null;
        }
    }
}
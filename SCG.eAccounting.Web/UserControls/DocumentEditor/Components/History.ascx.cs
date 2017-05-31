using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.Query;
using SS.Standard.WorkFlow.Query;
using System.Drawing;
using System.Data;
using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.BLL;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor.Components
{
    public partial class History : BaseUserControl
    {
        public ITransactionService TransactionService { get; set; }

        public long WorkFlowID
        {
            get 
            {
                if (ViewState["WorkFlowID"] != null)
                    return (long)ViewState["WorkFlowID"];
                else
                    return -1;
            }
            set { ViewState["WorkFlowID"] = value; }
        }
        public bool IsEmptyData
        {
            get { return (bool)ViewState["IsEmptyData"]; }
            set { ViewState["IsEmptyData"] = value; }
        }

        public string advClearingEventID;

        public Guid TransactionID
		{
			get { return (Guid)ViewState[ViewStateName.TransactionID]; }
			set { ViewState[ViewStateName.TransactionID] = value; }
		}
        public long DocumentID
        {
            get { return UIHelper.ParseLong(ViewState[ViewStateName.DocumentID].ToString()); }
            set { ViewState[ViewStateName.DocumentID] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Initialize(long documentID)
        {
            this.DocumentID = documentID;

            SS.Standard.WorkFlow.DTO.WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(documentID);
            if (workFlow != null)
            {
                this.WorkFlowID = workFlow.WorkFlowID;
                ctlHistoryGridView.DataCountAndBind();
            }
            else
            {
                ctlHistoryGridView.DataBind();
            }

            if (RequestCount() > 0)
                IsEmptyData = false;
            else
                IsEmptyData = true;
        }

        protected void ctlHistoryGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label ctlDescription = (Label)e.Row.FindControl("ctlDescriptionLabel") ;
                Label ctlResponseLabel = (Label)e.Row.FindControl("ctlResponseLabel");
                if (!string.IsNullOrEmpty(ctlDescription.Text.Trim()) && !ctlResponseLabel.Text.Equals("Approve"))
                {
                    e.Row.ForeColor = Color.Red;
                }

                if (ctlResponseLabel.Text.Equals("Verify"))
                {
                    SS.Standard.WorkFlow.DTO.ValueObject.WorkFlowSearchResult data = e.Row.DataItem as SS.Standard.WorkFlow.DTO.ValueObject.WorkFlowSearchResult;
                    if (data != null && data.AmountBeforeVerify.HasValue && data.AmountVerified.HasValue)
                    {
                        eAccounting.DTO.FnExpenseDocument expDoc = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(this.DocumentID);
                        if (expDoc != null && expDoc.AmountApproved != data.AmountVerified)
                        {
                            ctlDescription.Text = string.Format(GetMessage("HistoryDisplayMessageAmountChange"), string.Format("{0:#,##0.00}", expDoc.AmountApproved), string.Format("{0:#,##0.00}", data.AmountVerified));
                        }
                    }
                }

                if (ViewState["advClearingEventID"] == null)
                {
                    advClearingEventID = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetByWorkFlowStateID_EventName(eAccounting.BLL.AdvanceStateID.OutStanding, "Clearing").WorkFlowStateEventID.ToString();
                    ViewState["advClearingEventID"] = advClearingEventID;
                }
                else
                {
                    advClearingEventID = ViewState["advClearingEventID"].ToString();
                }

                HiddenField ctlWorkFlowStateEventID = (HiddenField)e.Row.FindControl("ctlWorkFlowStateEventID");
                if (advClearingEventID.ToString().Equals(ctlWorkFlowStateEventID.Value))
                {
                    long advanceID = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindAdvanceByWorkFlowID(this.WorkFlowID).AdvanceID;
                    // Use AdvanceID to keep WorkflowID
                    eAccounting.DTO.ValueObject.AdvanceData advObj = ScgeAccountingQueryProvider.FnExpenseAdvanceQuery.FindExpenseDocumentNoByAdvanceDocumentID(advanceID);

                    if (advObj != null)
                    {
                        ctlDescription.Text = string.Format("<A HREF='/Forms/SCG.eAccounting/Programs/DocumentView.aspx?wfid={0}'>{1}</A>", advObj.AdvanceID, advObj.DocumentNo);
                    }
                }
            }
        }
        protected void ctlHistoryGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }
        protected void ctlHistoryGridView_DataBound(object sender, EventArgs e)
        {
            if (ctlHistoryGridView.Rows.Count == 0)
            {
            }
        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            return WorkFlowQueryProvider.WorkFlowResponseQuery.GetWorkFlowList(this.WorkFlowID,UserAccount.CurrentLanguageID, startRow, pageSize, sortExpression);
        }
        public int RequestCount()
        {
            return WorkFlowQueryProvider.WorkFlowResponseQuery.CountWorkFlowByCriteria(this.WorkFlowID, UserAccount.CurrentLanguageID);
        }

    }
}
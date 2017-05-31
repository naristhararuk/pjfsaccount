using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.Web.Helper;
using System.Text;
using SCG.eAccounting.Query;
using SCG.eAccounting.DTO;
using System.Linq.Expressions;
using System.Collections;
using SS.Standard.WorkFlow.Service;
using SS.Standard.WorkFlow.Query;
using SCG.eAccounting.BLL;
using SCG.eAccounting.SAP.BAPI.Service.Const;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.DTO.ValueObject;
using System.Web.Script.Serialization;
using SS.Standard.Utilities;
using SCG.DB.Query;

namespace SCG.eAccounting.Web.UserControls.InboxSearchResult
{
    public partial class InboxEmployeeSearchResult : BaseUserControl
    {
        public IWorkFlowService WorkFlowService { get; set; }

        #region Property
        public string Legend
        {
            set { ctlSearchCriteriaHeader.Text = value; }
        }
        public int RowCount
        {
            get
            {
                if (ViewState["RowCount"] != null)
                {
                    return UIHelper.ParseInt(ViewState["RowCount"].ToString());
                }
                else
                {
                    return 0;
                }

                //return ctlInboxEmployeeGrid.Rows.Count;
            }
            set { ViewState["RowCount"] = value; }
        }
        public bool VisibleColumnAttachFile
        {
            get
            {
                if (ViewState["VisibleColumnAttachFile"] != null)
                {
                    return (bool)ViewState["VisibleColumnAttachFile"];
                }
                else
                {
                    return true;
                }
            }
            set { ViewState["VisibleColumnAttachFile"] = value; }  //{ ctlInboxEmployeeGrid.Columns[0].Visible = value; }
        }
        public bool VisibleColumnSelect
        {
            get
            {
                if (ViewState["VisibleColumnSelect"] != null)
                {
                    return (bool)ViewState["VisibleColumnSelect"];
                }
                else
                {
                    return true;
                }
            }
            set { ViewState["VisibleColumnSelect"] = value; }  //{ ctlInboxEmployeeGrid.Columns[1].Visible = value; }
        }
        public bool VisibleColumnDocumentType
        {
            get
            {
                if (ViewState["VisibleColumnDocumentType"] != null)
                {
                    return (bool)ViewState["VisibleColumnDocumentType"];
                }
                else
                {
                    return true;
                }
            }
            set { ViewState["VisibleColumnDocumentType"] = value; }  //{ ctlInboxEmployeeGrid.Columns[3].Visible = value; }
        }
        public bool VisibleColumnCreateDate
        {
            get
            {
                if (ViewState["VisibleColumnCreateDate"] != null)
                {
                    return (bool)ViewState["VisibleColumnCreateDate"];
                }
                else
                {
                    return true;
                }
            }
            set { ViewState["VisibleColumnCreateDate"] = value; }   //{ ctlInboxEmployeeGrid.Columns[4].Visible = value; }
        }
        public bool VisibleColumnReferenceNo
        {
            get
            {
                if (ViewState["VisibleColumnReferenceNo"] != null)
                {
                    return (bool)ViewState["VisibleColumnReferenceNo"];
                }
                else
                {
                    return true;
                }
            }
            set { ViewState["VisibleColumnReferenceNo"] = value; }   //{ ctlInboxEmployeeGrid.Columns[5].Visible = value; }
        }
        public bool VisibleColumnRequesterDate
        {
            get
            {
                if (ViewState["VisibleColumnRequesterDate"] != null)
                {
                    return (bool)ViewState["VisibleColumnRequesterDate"];
                }
                else
                {
                    return true;
                }
            }
            set { ViewState["VisibleColumnRequesterDate"] = value; }   //{ ctlInboxEmployeeGrid.Columns[6].Visible = value; }
        }
        public bool VisibleColumnApproveDate
        {
            get
            {
                if (ViewState["VisibleColumnApproveDate"] != null)
                {
                    return (bool)ViewState["VisibleColumnApproveDate"];
                }
                else
                {
                    return true;
                }
            }
            set { ViewState["VisibleColumnApproveDate"] = value; }   //{ ctlInboxEmployeeGrid.Columns[7].Visible = value; }
        }
        public bool VisibleColumnStatus
        {
            get
            {
                if (ViewState["VisibleColumnStatus"] != null)
                {
                    return (bool)ViewState["VisibleColumnStatus"];
                }
                else
                {
                    return true;
                }
            }
            set { ViewState["VisibleColumnStatus"] = value; }   //{ ctlInboxEmployeeGrid.Columns[8].Visible = value; }
        }

        public bool VisibleApprove
        {
            get
            {
                if (ViewState["VisibleApprove"] != null)
                {
                    return (bool)ViewState["VisibleApprove"];
                }
                else
                {
                    return false;
                }
            }
            set { ViewState["VisibleApprove"] = value; }   //{ ctlApprove.Visible = value; }
        }
        public SearchCriteria Criteria
        {
            get { return (SearchCriteria)ViewState["SearchCriteria"]; }
            set { ViewState["SearchCriteria"] = value; }
        }
        public string StateName { get; set; }
        public bool isShowMsg
        {
            get
            {
                if (ViewState["isShowMsg"] == null)
                {
                    return true;
                }
                else
                {
                    return (bool)(ViewState["isShowMsg"]);
                }
            }
            set { ViewState["isShowMsg"] = value; }
        }
        public IList<SearchResultData> resultList { get; set; }

        public bool IsRepOffice
        {
            get
            {
                if (ViewState["IsRepOffice"] == null)
                    return false;
                else
                {
                    return (bool)(ViewState["IsRepOffice"]);
                }
            }
            set { ViewState["IsRepOffice"] = value; }
        }

        public string SearchType
        {
            get
            {
                if (ViewState["SearchType"] == null)
                    return null;
                else
                {
                    return (string)(ViewState["SearchType"]);
                }
            }
            set { ViewState["SearchType"] = value; }
        }

        #endregion Property
        protected void Page_Init(object sender, EventArgs e)
        {
            ctlApproveStatusSummary.Notify_Ok += new EventHandler(ctlApproveSummaryStatusPopup_NotifyPopupResult);
        }

        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #endregion protected void Page_Load(object sender, EventArgs e)

        #region protected int ctlInboxEmployeeGrid_RequestCount()
        protected int ctlInboxEmployeeGrid_RequestCount()
        {
            SearchCriteria criteria = this.Criteria;
            if (criteria != null)
            {
                if (!string.IsNullOrEmpty(this.StateName))
                    criteria.DocumentStatus = this.StateName;

                this.RowCount = ScgeAccountingQueryProvider.SCGDocumentQuery.CountAccountantPaymentCriteria(Criteria);
            }
            else if (this.resultList != null)
            {
                this.RowCount = CountEmployeeInbox(this.StateName);
            }

            return this.RowCount;
        }
        #endregion protected int ctlInboxEmployeeGrid_RequestCount()

        #region protected object ctlInboxEmployeeGrid_RequestData(int startRow, int pageSize, string sortExpression)
        protected object ctlInboxEmployeeGrid_RequestData(int startRow, int pageSize, string sortExpression)
        {
            SearchCriteria criteria = this.Criteria;

            if (this.resultList != null)
            {
                IList<SearchResultData> result = FindEmployeeInbox(this.StateName);
                if (sortExpression.Length > 0)
                {
                    string[] sort = sortExpression.Split(' ');

                    if (sort.Length == 2 && sort[1] == "DESC")
                    {
                        result = result.OrderByDescending(r => r.GetType().GetProperty(sort[0]).GetValue(r, null)).ToList(); //<SearchResultData, object>(mySortExpression).ToList();
                    }
                    else if (sort.Length == 2 && sort[1] == "ASC")
                    {
                        result = result.OrderBy(r => r.GetType().GetProperty(sort[0]).GetValue(r, null)).ToList();
                    }
                }
                return result
                            .Skip<SearchResultData>(startRow)
                            .Take<SearchResultData>(startRow + pageSize)
                            .ToList();
            }
            else if (criteria != null)
            {
                if (!string.IsNullOrEmpty(this.StateName))
                    criteria.DocumentStatus = this.StateName;

                return ScgeAccountingQueryProvider.SCGDocumentQuery.GetAccountantPaymentCriteriaList(criteria, startRow, pageSize, sortExpression);
            }
            else
            {
                return null;
            }
        }
        #endregion protected object ctlInboxEmployeeGrid_RequestData(int startRow, int pageSize, string sortExpression)

        #region public void BindInboxGridView(SearchCriteria criteria)
        public void BindInboxGridView(SearchCriteria criteria)
        {
            this.Criteria = criteria;

            ctlInboxEmployeeGrid.DataCountAndBind();
            ctlUpdatePanelGridView.Update();
        }
        #endregion public void BindInboxGridView(SearchCriteria criteria)

        #region public void BindEmployeeInboxGridView(IList<SearchResultData> resultList)
        public void BindEmployeeInboxGridView(IList<SearchResultData> resultList, SearchCriteria criteria)
        {
            this.resultList = resultList;
            this.Criteria = criteria;

            ctlInboxEmployeeGrid.DataCountAndBind();
            ctlUpdatePanelGridView.Update();
        }
        #endregion public void BindEmployeeInboxGridView(IList<SearchResultData> resultList)

        #region public IList<SearchResultData> GetGridViewDataList()
        public IList<SearchResultData> GetGridViewDataList()
        {
            IList<SearchResultData> resultList = new List<SearchResultData>();

            foreach (GridViewRow row in ctlInboxEmployeeGrid.Rows)
            {
                SearchResultData result = new SearchResultData();
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox ctlSelect = (CheckBox)ctlInboxEmployeeGrid.Rows[row.RowIndex].FindControl("ctlSelect");

                    if (ctlSelect.Checked)
                    {
                        LinkButton ctlRequestNo = (LinkButton)ctlInboxEmployeeGrid.Rows[row.RowIndex].FindControl("ctlRequestNo");
                        Literal ctlReferenceNo = (Literal)ctlInboxEmployeeGrid.Rows[row.RowIndex].FindControl("ctlReferenceNo");
                        Literal ctlRequestDate = (Literal)ctlInboxEmployeeGrid.Rows[row.RowIndex].FindControl("ctlRequestDate");
                        Literal ctlApproveDate = (Literal)ctlInboxEmployeeGrid.Rows[row.RowIndex].FindControl("ctlApproveDate");
                        Literal ctlStatus = (Literal)ctlInboxEmployeeGrid.Rows[row.RowIndex].FindControl("ctlStatus");
                        Literal ctlSubject = (Literal)ctlInboxEmployeeGrid.Rows[row.RowIndex].FindControl("ctlSubject");
                        Literal ctlCreatorName = (Literal)ctlInboxEmployeeGrid.Rows[row.RowIndex].FindControl("ctlCreatorName");
                        Literal ctlRequestName = (Literal)ctlInboxEmployeeGrid.Rows[row.RowIndex].FindControl("ctlRequestName");
                        Literal ctlAmount = (Literal)ctlInboxEmployeeGrid.Rows[row.RowIndex].FindControl("ctlAmount");
                        Literal ctlReceive = (Literal)ctlInboxEmployeeGrid.Rows[row.RowIndex].FindControl("ReceiveDocumentDate");
                        result.DocumentID = UIHelper.ParseLong(ctlInboxEmployeeGrid.DataKeys[row.RowIndex]["DocumentID"].ToString());
                        result.WorkFlowStateID = UIHelper.ParseInt(ctlInboxEmployeeGrid.DataKeys[row.RowIndex]["WorkFlowStateID"].ToString());

                        if (ctlRequestNo != null)
                        {
                            result.RequestNo = ctlRequestNo.Text;
                        }
                        if (ctlReferenceNo != null)
                        {
                            result.ReferenceNo = ctlReferenceNo.Text;
                        }
                        if (ctlRequestDate != null && !string.IsNullOrEmpty(ctlRequestDate.Text))
                        {
                            result.RequestDate = UIHelper.ParseDate(ctlRequestDate.Text).Value;
                        }
                        if (ctlApproveDate != null && !string.IsNullOrEmpty(ctlApproveDate.Text))
                        {
                            result.ApproveDate = UIHelper.ParseDate(ctlApproveDate.Text).Value;
                        }
                        if (ctlStatus != null)
                        {
                            result.DocumentStatus = ctlStatus.Text;
                        }
                        if (ctlSubject != null)
                        {
                            result.Subject = ctlSubject.Text;
                        }
                        if (ctlCreatorName != null)
                        {
                            result.CreatorName = ctlCreatorName.Text;
                        }
                        if (ctlRequestName != null)
                        {
                            result.RequesterName = ctlRequestName.Text;
                        }
                        if (ctlReceive != null)
                        {
                            result.ReceiveDocumentDate =  UIHelper.ParseDate(ctlReceive.Text).Value;
                        }
              
                        resultList.Add(result);
                    }
                }
            }
            return resultList;
        }
        #endregion public IList<SearchResultData> GetGridViewDataList()

        #region protected void ctlInboxEmployeeGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        protected void ctlInboxEmployeeGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("PopupDocument"))
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                long workflowID = UIHelper.ParseLong(ctlInboxEmployeeGrid.DataKeys[rowIndex]["WorkflowID"].ToString());
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), string.Empty, "location.href='../Programs/DocumentView.aspx?wfid=" + workflowID.ToString() + "'", true);
                Response.Redirect("../Programs/DocumentView.aspx?wfid=" + workflowID.ToString());
            }
        }
        #endregion

        #region protected void ctlInboxEmployeeGrid_DataBound(object sender, EventArgs e)
        protected void ctlInboxEmployeeGrid_DataBound(object sender, EventArgs e)
        {
            IsRepOffice = ScgDbQueryProvider.DbPBQuery.IsRepOffice(UIHelper.ParseLong(UserAccount.UserID.ToString()));
            if (Criteria != null && Criteria.FlagSearch == "Employee")
            {
                if (IsRepOffice)
                {
                    ctlInboxEmployeeGrid.Columns[12].Visible = true;
                    ctlInboxEmployeeGrid.Columns[13].Visible = true;
                }
                else
                {
                    ctlInboxEmployeeGrid.Columns[12].Visible = false;
                    ctlInboxEmployeeGrid.Columns[13].Visible = false;
                }
            }

            ctlInboxEmployeeGrid.Columns[0].Visible = this.VisibleColumnAttachFile;
            ctlInboxEmployeeGrid.Columns[1].Visible = this.VisibleColumnSelect;
            ctlInboxEmployeeGrid.Columns[3].Visible = this.VisibleColumnDocumentType;
            ctlInboxEmployeeGrid.Columns[4].Visible = this.VisibleColumnCreateDate;
            ctlInboxEmployeeGrid.Columns[5].Visible = this.VisibleColumnReferenceNo;
            ctlInboxEmployeeGrid.Columns[6].Visible = this.VisibleColumnRequesterDate;
            ctlInboxEmployeeGrid.Columns[7].Visible = this.VisibleColumnApproveDate;
            ctlInboxEmployeeGrid.Columns[8].Visible = this.VisibleColumnStatus;
            ctlApprove.Visible = this.VisibleApprove;
            if (ctlInboxEmployeeGrid.Rows.Count > 0)
            {
                RegisterScriptForGridView();
            }
        }
        #endregion protected void ctlInboxEmployeeGrid_DataBound(object sender, EventArgs e)

        #region protected void ctlInboxEmployeeGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        protected void ctlInboxEmployeeGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                long documentID = UIHelper.ParseLong(ctlInboxEmployeeGrid.DataKeys[e.Row.RowIndex]["DocumentID"].ToString());
                IList<DocumentAttachment> documentAttachment = ScgeAccountingQueryProvider.DocumentAttachmentQuery.GetDocumentAttachmentByDocumentID(documentID);
                IList<FnExpenseDocument> fnExpenseDocument = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetFnExpenseDocumentByDocumentID(documentID);

                CheckBox ctlSelect = (CheckBox)e.Row.FindControl("ctlSelect");

                ctlSelect.Attributes.Add("onclick", "javascript:" + ctlInboxEmployeeGrid.ClientID + "_validateCheckBox(this, '1') ");

                if (documentAttachment.Count > 0)
                {
                    Image ctlAttach = (Image)e.Row.FindControl("ctlAttach");

                    ctlAttach.Visible = true;
                }
                if (fnExpenseDocument.Count > 0)
                {
                    Image ctlFile = (Image)e.Row.FindControl("ctlFile");

                    ctlFile.Visible = true;
                }

                Literal amountLabel = (Literal)e.Row.FindControl("ctlAmount");
                Literal amountLocalCurrencyLabel = (Literal)e.Row.FindControl("ctlAmountLocalCurrency");
                Literal amountMainCurrencyLabel = (Literal)e.Row.FindControl("ctlAmountMainCurrency");

                SearchResultData data = e.Row.DataItem as SearchResultData;
                if (data != null)
                {
                    System.Drawing.Color defaultColor = System.Drawing.ColorTranslator.FromHtml("#777777");
                    System.Drawing.Color redColor = System.Drawing.ColorTranslator.FromHtml("#ff0000");

                    e.Row.Cells[12].ForeColor = data.AmountLocalCurrency < 0 ? redColor : defaultColor;
                    e.Row.Cells[13].ForeColor = data.AmountMainCurrency < 0 ? redColor : defaultColor;
                    e.Row.Cells[14].ForeColor = data.Amount < 0 ? redColor : defaultColor;
                }
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox ctlHeader = (CheckBox)e.Row.FindControl("ctlHeader");
                ctlHeader.Attributes.Add("onclick", "javascript:" + ctlInboxEmployeeGrid.ClientID + "_validateCheckBox(this, '0') ");
            }
        }
        #endregion protected void ctlInboxEmployeeGrid_RowDataBound(object sender, GridViewRowEventArgs e)

        #region private void RegisterScriptForGridView()
        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function " + ctlInboxEmployeeGrid.ClientID + "_validateCheckBox(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlInboxEmployeeGrid.ClientID + "', '" + ctlInboxEmployeeGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), ctlInboxEmployeeGrid.ClientID + "_validateChkBox", script.ToString(), true);
        }
        #endregion private void RegisterScriptForGridView()

        #region public IList<SearchResultData> FindEmployeeInbox(string stateName)
        public IList<SearchResultData> FindEmployeeInbox(string stateName)
        {
            var result = from SearchResultData searchResult in this.resultList
                         where searchResult.StateName == stateName
                         select searchResult;

            return result.ToList<SearchResultData>();
        }
        #endregion public IList<SearchResultData> FindEmployeeInbox(string stateName)

        #region public int CountEmployeeInbox(string stateName)
        public int CountEmployeeInbox(string stateName)
        {
            var result = from SearchResultData searchResult in this.resultList
                         where searchResult.StateName == stateName
                         select searchResult;

            return result.Count();
        }
        #endregion public int CountEmployeeInbox(string stateName)

        protected void Approve_Click(object sender, EventArgs e)
        {
            this.MultipleApproveVerify();
        }

        private IList<long> GetWorkflowIDForMultipleApprove()
        {
            IList<long> workflowList = new List<long>();
            foreach (GridViewRow row in ctlInboxEmployeeGrid.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox ctlSelect = (CheckBox)ctlInboxEmployeeGrid.Rows[row.RowIndex].FindControl("ctlSelect");

                    if (ctlSelect.Checked)
                    {
                        workflowList.Add(UIHelper.ParseInt(ctlInboxEmployeeGrid.DataKeys[row.RowIndex]["WorkflowID"].ToString()));
                    }
                }
            }
            return workflowList;
        }

        public void MultipleApproveVerify()
        {
            IList<long> workflowList = GetWorkflowIDForMultipleApprove();
            ApproveVerifyStatus approveVerifyStatus = null;
            List<ApproveVerifyStatus> approveVerifyStatusList = new List<ApproveVerifyStatus>();
            foreach (long workflowId in workflowList)
            {
                try
                {
                    SS.Standard.WorkFlow.DTO.WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workflowId);
                    SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(workFlow.Document.DocumentID);
                    approveVerifyStatus = new ApproveVerifyStatus();
                    approveVerifyStatus.DocumentNo = document.DocumentNo;
                    approveVerifyStatus.Subject = document.Subject;

                    int workFlowStateEventID = ScgeAccountingQueryProvider.SCGDocumentQuery.GetWorkStateEvent(workFlow.CurrentState.WorkFlowStateID, WorkFlowEventNameConst.Approve);
                    if (workFlow.WorkFlowType.WorkFlowTypeID == WorkFlowTypeID.AdvanceWorkFlowType)
                    {
                        AvAdvanceDocument advDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(workFlow.Document.DocumentID);
                        approveVerifyStatus.Amount = advDocument.Amount;
                        IList<WorkFlowStateEvent> workFlowState = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindWorkFlowStateEvent(WorkFlowEventNameConst.Approve, WorkFlowStatEventNameConst.WaitApproveVerify, WorkFlowTypeName.Advance);
                        object eventData = new SubmitResponse(workFlowState[0].WorkFlowStateEventID);
                        WorkFlowService.NotifyEvent(workflowId, WorkFlowEventNameConst.Approve, eventData);
                    }
                    else if (workFlow.WorkFlowType.WorkFlowTypeID == WorkFlowTypeID.ExpenseWorkFlow)
                    {
                        FnExpenseDocument expDocument = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(workFlow.Document.DocumentID);
                        approveVerifyStatus.Amount = expDocument.DifferenceAmount;

                        IList<WorkFlowStateEvent> workFlowState = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindWorkFlowStateEvent(WorkFlowEventNameConst.Approve, WorkFlowStatEventNameConst.WaitApproveVerify, WorkFlowTypeName.Expense);
                        object eventData = new SubmitResponse(workFlowState[0].WorkFlowStateEventID);
                        WorkFlowService.NotifyEvent(workflowId, WorkFlowEventNameConst.Approve, eventData);
                    }
                    else if (workFlow.WorkFlowType.WorkFlowTypeID == WorkFlowTypeID.RemittanceWorkFlow)
                    {
                        FnRemittance rmtDocument = ScgeAccountingQueryProvider.FnRemittanceQuery.GetFnRemittanceByDocumentID(workFlow.Document.DocumentID);
                        approveVerifyStatus.Amount = rmtDocument.TotalAmount;

                        IList<WorkFlowStateEvent> workFlowState = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindWorkFlowStateEvent(WorkFlowEventNameConst.Approve, WorkFlowStatEventNameConst.WaitApproveRemittance, WorkFlowTypeName.Remittance);
                        object eventData = new SubmitResponse(workFlowState[0].WorkFlowStateEventID);
                        WorkFlowService.NotifyEvent(workflowId, WorkFlowEventNameConst.Approve, eventData);
                    }

                    approveVerifyStatus.Status = "Success";
                }
                catch (ServiceValidationException ex)
                {
                    approveVerifyStatus.Status = "Error";
                    approveVerifyStatus.Reason = new List<string>();
                    foreach (Spring.Validation.ErrorMessage errorMsg in ex.ValidationErrors.GetErrors("WorkFlow.Error"))
                    {
                        approveVerifyStatus.Reason.Add(errorMsg.Id);
                    }
                    //ctlApproveStatusSummary.ValidationErrors.MergeErrors(ex.ValidationErrors);
                }
                approveVerifyStatusList.Add(approveVerifyStatus);
            }

            if (approveVerifyStatusList.Count > 0)
            {
                ctlApproveStatusSummary.DataList = approveVerifyStatusList.OrderBy(x => x.Status).ThenBy(x => x.DocumentNo).ToList();
                ctlApproveStatusSummary.BindGridView();
                ctlApproveResultSummaryModalPopupExtender.Show();
            }
        }
        protected void ctlApproveSummaryStatusPopup_NotifyPopupResult(object sender, EventArgs args)
        {
            ctlInboxEmployeeGrid.DataCountAndBind();
            ctlUpdatePanelGridView.Update();
            ctlApproveResultSummaryModalPopupExtender.Hide();
        }
    }
}
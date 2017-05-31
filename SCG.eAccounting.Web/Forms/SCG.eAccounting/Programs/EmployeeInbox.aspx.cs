using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.Query;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.BLL;
using SS.Standard.WorkFlow.Service;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.Query;
using SS.Standard.WorkFlow.DTO.ValueObject;
using SS.Standard.Utilities;

namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs
{
    public partial class EmployeeInbox : BasePage
    {
        public IWorkFlowService WorkFlowService { get; set; }

        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ctlInboxEmployeeSearchResultDraft.VisibleColumnAttachFile = false;
                ctlInboxEmployeeSearchResultWaitInitial.VisibleColumnAttachFile = false;
                ctlInboxEmployeeSearchResultWaitApprove.VisibleColumnAttachFile = false;
                ctlInboxemployeeSearchResultHold.VisibleColumnAttachFile = false;
                ctlInboxEmployeeSearchResultWaitAgree.VisibleColumnAttachFile = false;

                ctlInboxEmployeeSearchResultDraft.VisibleColumnCreateDate = false;
                ctlInboxEmployeeSearchResultWaitInitial.VisibleColumnCreateDate = false;
                ctlInboxEmployeeSearchResultWaitApprove.VisibleColumnCreateDate = false;
                ctlInboxemployeeSearchResultHold.VisibleColumnCreateDate = false;
                ctlInboxEmployeeSearchResultWaitAgree.VisibleColumnCreateDate = false;

                ctlInboxEmployeeSearchResultDraft.VisibleColumnDocumentType = false;
                ctlInboxEmployeeSearchResultWaitInitial.VisibleColumnDocumentType = false;
                ctlInboxEmployeeSearchResultWaitApprove.VisibleColumnDocumentType = false;
                ctlInboxemployeeSearchResultHold.VisibleColumnDocumentType = false;
                ctlInboxEmployeeSearchResultWaitAgree.VisibleColumnDocumentType = false;

                ctlInboxEmployeeSearchResultDraft.VisibleColumnStatus = false;
                ctlInboxEmployeeSearchResultWaitInitial.VisibleColumnStatus = false;
                ctlInboxEmployeeSearchResultWaitApprove.VisibleColumnStatus = false;
                ctlInboxemployeeSearchResultHold.VisibleColumnStatus = false;
                ctlInboxEmployeeSearchResultWaitAgree.VisibleColumnStatus = false;

                this.GetCriteriaSession();
                this.SearchCriteria();
                this.FieldSet();
            }
        }
        #endregion protected void Page_Load(object sender, EventArgs e)

        #region Function
        #region public SearchCriteria BuildCriteria()
        public SearchCriteria BuildCriteria()
        {
            SearchCriteria criteria = new SearchCriteria();

            criteria.RequestNo = ctlInboxEmployeeSearchCriteria.RequestNo;
            criteria.RequestDateFrom = ctlInboxEmployeeSearchCriteria.RequestDateFrom;
            criteria.RequestDateTo = ctlInboxEmployeeSearchCriteria.RequestDateTo;
            criteria.CreatorID = UIHelper.ParseLong(ctlInboxEmployeeSearchCriteria.CreatorID);
            criteria.RequesterID = UIHelper.ParseLong(ctlInboxEmployeeSearchCriteria.RequesterID);
            criteria.DocumentTypeID = UIHelper.ParseLong(ctlInboxEmployeeSearchCriteria.RequesterType);
            criteria.UserID = UserAccount.UserID;
            criteria.FlagQuery = "Employee";

            criteria.FlagSearch = "Employee";
            criteria.FlagJoin = "Inbox";
            criteria.Role = string.Empty;
            criteria.LanguageID = UserAccount.CurrentLanguageID;
            return criteria;
        }
        #endregion public SearchCriteria BuildCriteria()

        #region public void GetCriteriaSession()
        public void GetCriteriaSession()
        {
            string pageName = this.GetCurrentPageName();
            string sessionName = pageName + "_" + UserAccount.UserID.ToString();

            if (Session[sessionName] != null)
            {
                SearchCriteria searchCriteria = Session[sessionName] as SearchCriteria;

                ctlInboxEmployeeSearchCriteria.RequestNo = searchCriteria.RequestNo.ToString();
                ctlInboxEmployeeSearchCriteria.RequestDateFrom = searchCriteria.RequestDateFrom.ToString();
                ctlInboxEmployeeSearchCriteria.RequestDateTo = searchCriteria.RequestDateTo.ToString();
                ctlInboxEmployeeSearchCriteria.CreatorID = searchCriteria.CreatorID.ToString();
                ctlInboxEmployeeSearchCriteria.RequesterID = searchCriteria.RequesterID.ToString();
                if (!searchCriteria.DocumentTypeID.ToString().Equals("0"))
                {
                    ctlInboxEmployeeSearchCriteria.SetCombo(); //bind dropdownlist
                    ctlInboxEmployeeSearchCriteria.RequesterType = searchCriteria.DocumentTypeID.ToString();
                }
                ctlInboxEmployeeSearchCriteria.CreatorNameID = searchCriteria.CreatorID.ToString();
                ctlInboxEmployeeSearchCriteria.RequesterNameID = searchCriteria.RequesterID.ToString();
            }
        }
        #endregion public void GetCriteriaSession()

        #region public string GetCurrentPageName()
        public string GetCurrentPageName()
        {
            string sPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
            System.IO.FileInfo oInfo = new System.IO.FileInfo(sPath);
            string sRet = oInfo.Name;
            return sRet;
        }
        #endregion public string GetCurrentPageName()

        //public IList<SearchResultData> FindEmployeeInbox(IList<SearchResultData> criteriaList, string stateName)
        //{
        //    var result = from SearchResultData searchResult in Session["resultList"]
        //                 where searchResult.StateName == stateName
        //                 select searchResult;

        //    return result.ToList<SearchResultData>();
        //}

        #region public void SearchCriteria()
        public void SearchCriteria()
        {
            SearchCriteria criteria = this.BuildCriteria();

            //set criteria to session
            string pageName = this.GetCurrentPageName();
            Session[pageName + "_" + UserAccount.UserID.ToString()] = criteria;

            //ctlInboxEmployeeSearchResultDraft.BindInboxGridView(criteria);
            //ctlInboxEmployeeSearchResultWaitInitial.BindInboxGridView(criteria);
            //ctlInboxEmployeeSearchResultWaitApprove.BindInboxGridView(criteria);
            //ctlInboxemployeeSearchResultHold.BindInboxGridView(criteria);
            //ctlInboxEmployeeSearchResultWaitAgree.BindInboxGridView(criteria);

            IList<SearchResultData> resultList = ScgeAccountingQueryProvider.SCGDocumentQuery.GetAccountantPaymentCriteriaList(criteria);

            ctlInboxEmployeeSearchResultDraft.BindEmployeeInboxGridView(resultList, criteria);
            ctlInboxEmployeeSearchResultWaitInitial.BindEmployeeInboxGridView(resultList, criteria);
            ctlInboxEmployeeSearchResultWaitApprove.BindEmployeeInboxGridView(resultList, criteria);
            ctlInboxemployeeSearchResultHold.BindEmployeeInboxGridView(resultList, criteria);
            ctlInboxEmployeeSearchResultWaitAgree.BindEmployeeInboxGridView(resultList, criteria);

            this.FieldSet();
            ctlUpdatePanel.Update();

            ctlInboxEmployeeSearchResultDraft.Visible = !ctlInboxEmployeeSearchResultDraft.RowCount.Equals(0);
            ctlInboxEmployeeSearchResultWaitInitial.Visible = !ctlInboxEmployeeSearchResultWaitInitial.RowCount.Equals(0);
            ctlInboxEmployeeSearchResultWaitApprove.Visible = !ctlInboxEmployeeSearchResultWaitApprove.RowCount.Equals(0);
            ctlInboxemployeeSearchResultHold.Visible = !ctlInboxemployeeSearchResultHold.RowCount.Equals(0);
            ctlInboxEmployeeSearchResultWaitAgree.Visible = !ctlInboxEmployeeSearchResultWaitAgree.RowCount.Equals(0);

            ctlSendDraft.Visible = ctlInboxEmployeeSearchResultDraft.Visible;
            ctlAcceptWaitInitial.Visible = ctlInboxEmployeeSearchResultWaitInitial.Visible;
            ctlApproveWaitApprove.Visible = ctlInboxEmployeeSearchResultWaitApprove.Visible;
            ctlApproveAgree.Visible = ctlInboxEmployeeSearchResultWaitAgree.Visible;
        }
        #endregion public void SearchCriteria()

        #region public void FieldSet()
        public void FieldSet()
        {
            ctlInboxEmployeeSearchResultDraft.Legend = string.Concat(GetMessage(string.Concat("Reject/Withdraw")), " (" + ctlInboxEmployeeSearchResultDraft.RowCount + ")");
            ctlInboxEmployeeSearchResultWaitInitial.Legend = string.Concat(GetMessage(string.Concat("WS_", WorkFlowStateFlag.WaitInitial)), " (" + ctlInboxEmployeeSearchResultWaitInitial.RowCount + ")");
            ctlInboxEmployeeSearchResultWaitApprove.Legend = string.Concat(GetMessage(string.Concat("WS_", WorkFlowStateFlag.WaitApprove)), " (" + ctlInboxEmployeeSearchResultWaitApprove.RowCount + ")");
            ctlInboxemployeeSearchResultHold.Legend = string.Concat(GetMessage(string.Concat("WS_", WorkFlowStateFlag.Hold)), " (" + ctlInboxemployeeSearchResultHold.RowCount + ")");
            ctlInboxEmployeeSearchResultWaitAgree.Legend = string.Concat(GetMessage(string.Concat("WS_", WorkFlowStateFlag.WaitAgree)), " (" + ctlInboxEmployeeSearchResultWaitAgree.RowCount + ")");
        }
        #endregion public void FieldSet()

        #region public void LoadEvent(string eventName,string stateName)
        public void LoadEvent(string eventName, string stateName)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            try
            {
                IList<SearchResultData> resultList = new List<SearchResultData>();
                long workFlowID;
                int workFlowStateEventID;

                if (stateName.Equals(WorkFlowStateFlag.Draft))
                {
                    resultList = ctlInboxEmployeeSearchResultDraft.GetGridViewDataList();
                }
                if (stateName.Equals(WorkFlowStateFlag.WaitInitial))
                {
                    resultList = ctlInboxEmployeeSearchResultWaitInitial.GetGridViewDataList();
                }
                if (stateName.Equals(WorkFlowStateFlag.WaitApprove))
                {
                    resultList = ctlInboxEmployeeSearchResultWaitApprove.GetGridViewDataList();
                }
                if (stateName.Equals(WorkFlowStateFlag.WaitAgree))
                {
                    resultList = ctlInboxEmployeeSearchResultWaitAgree.GetGridViewDataList();
                }

                foreach (SearchResultData result in resultList)
                {
                    workFlowID = 0;
                    workFlowStateEventID = 0;
                    WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(result.DocumentID);

                    if (workFlow != null)
                    {
                        workFlowID = workFlow.WorkFlowID;
                    workFlowStateEventID = ScgeAccountingQueryProvider.SCGDocumentQuery.GetWorkStateEvent(workFlow.CurrentState.WorkFlowStateID, eventName);

                        object eventData = new SubmitResponse(workFlowStateEventID);
                        WorkFlowStateEvent workFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByIdentity(workFlowStateEventID);
                        if (workFlowStateEvent != null)
                    WorkFlowService.NotifyEvent(workFlowID, workFlowStateEvent.Name, eventData);
                }
            }
            }
            catch (ServiceValidationException ex)
            {
                errors.MergeErrors(ex.ValidationErrors);
            }
            ctlUpdatePanelValidation.Update();
        }
        #endregion public void LoadEvent(string eventName,string stateName)
        #endregion Function

        #region Button Click
        #region protected void ctlSearchCriteria_Click(object sender, EventArgs e)
        protected void ctlSearchCriteria_Click(object sender, EventArgs e)
        {
            this.SearchCriteria();
        }
        #endregion protected void ctlSearchCriteria_Click(object sender, EventArgs e)

        #region protected void ctlSendDraft_Click(object sender, EventArgs e)
        protected void ctlSendDraft_Click(object sender, EventArgs e)
        {
            this.LoadEvent(EventName.Send, WorkFlowStateFlag.Draft);
            this.SearchCriteria();
        }
        #endregion protected void ctlSendDraft_Click(object sender, EventArgs e)

        #region protected void ctlAcceptWaitInitial_Click(object sender, EventArgs e)
        protected void ctlAcceptWaitInitial_Click(object sender, EventArgs e)
        {
            this.LoadEvent(EventName.Approve, WorkFlowStateFlag.WaitInitial);
            this.SearchCriteria();
        }
        #endregion protected void ctlAcceptWaitInitial_Click(object sender, EventArgs e)

        #region protected void ctlApproveWaitApprove_Click(object sender, EventArgs e)
        protected void ctlApproveWaitApprove_Click(object sender, EventArgs e)
        {
            this.LoadEvent(EventName.Approve, WorkFlowStateFlag.WaitApprove);
            this.SearchCriteria();
        }
        #endregion protected void ctlApproveWaitApprove_Click(object sender, EventArgs e)

        #region protected void ctlApproveWaitAgree_Click(object sender, EventArgs e)
        protected void ctlApproveWaitAgree_Click(object sender, EventArgs e)
        {
            this.LoadEvent(EventName.Approve, WorkFlowStateFlag.WaitAgree);
            this.SearchCriteria();
        }
        #endregion protected void ctlApproveWaitAgree_Click(object sender, EventArgs e)
        #endregion Button Click
    }
}

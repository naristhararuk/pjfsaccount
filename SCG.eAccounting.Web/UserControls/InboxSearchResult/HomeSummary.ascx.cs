using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.Query;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.BLL;
using SS.SU.Query;
using SS.SU.DTO;
using SCG.eAccounting.Web.Helper;

namespace SCG.eAccounting.Web.UserControls.InboxSearchResult
{
    public partial class HomeSummary : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ApplicationMode != "Archive")
                {
                    this.BindEmployeeInbox();
                    this.BindAccountantInbox();
                    //this.BindPaymentInbox();

                    this.VisibleAccountant();
                    this.VisiblePayment();
                }
            }
        }

        public string BindEmployeeDraftInbox()
        {
            SearchCriteria criteria = new SearchCriteria();
            string count = "0";

            criteria.UserID = UserAccount.UserID;
            criteria.FlagQuery = "Draft";
            criteria.LanguageID = UserAccount.CurrentLanguageID;

            //IList<SearchResultData> criteriaList = ScgeAccountingQueryProvider.SCGDocumentQuery.FindInboxEmployeeSummaryCriteria(criteria);
            //return criteriaList.Count.ToString();

            IList<SearchResultData> criteriaList = ScgeAccountingQueryProvider.SCGDocumentQuery.FindInboxAccountantPaymentSummaryCriteria(criteria);

            return count = CountEmployeeInbox(criteriaList,WorkFlowStateFlag.Draft, 1);
        }

        public string BindEmployeeUnclearAdvenceInbox()
        {
            //SearchCriteria criteria = new SearchCriteria();

            //criteria.UserID = UserAccount.UserID;
            //criteria.FlagOutstanding = WorkFlowStateFlag.Outstanding;
            //criteria.FlagQuery = "Employee";

            //IList<SearchResultData> criteriaList = ScgeAccountingQueryProvider.SCGDocumentQuery.FindInboxEmployeeSummaryCriteria(criteria);

            //return criteriaList.Count.ToString();

            IList<int> iListate = new List<int>();
            iListate.Add(AdvanceStateID.Draft);
            iListate.Add(AdvanceStateID.Complete);
            iListate.Add(AdvanceStateID.Cancel);
            long currentWorkFlowID = UIHelper.ParseLong(Request.Params["wfid"]);

            int domestic = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.CountFindOutstandingRequest(UserAccount.UserID, currentWorkFlowID, DocumentTypeID.AdvanceDomesticDocument, DocumentTypeID.ExpenseDomesticDocument, iListate, UserAccount.CurrentLanguageID);
            int foreign = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.CountFindOutstandingRequest(UserAccount.UserID, currentWorkFlowID, DocumentTypeID.AdvanceForeignDocument, DocumentTypeID.ExpenseForeignDocument, iListate, UserAccount.CurrentLanguageID);
            int count = domestic + foreign;
            return count.ToString();
        }

        public string BindEmployeeUnclearAdvenceOverdueInbox()
        {
            //SearchCriteria criteria = new SearchCriteria();

            //criteria.UserID = UserAccount.UserID;
            //criteria.FlagOutstanding = WorkFlowStateFlag.OutstandingOverdue;
            //criteria.FlagQuery = "Employee";

            //IList<SearchResultData> criteriaList = ScgeAccountingQueryProvider.SCGDocumentQuery.FindInboxEmployeeSummaryCriteria(criteria);

            //return criteriaList.Count.ToString();

            VOAdvanceOverDueReport vo = new VOAdvanceOverDueReport();

            vo.RequesterID = UserAccount.UserID;
            vo.LanguageID = UserAccount.CurrentLanguageID;
            vo.FromOverDue = 1;

            int count = ScgeAccountingQueryProvider.SCGDocumentQuery.CountByAdvanceReportCriteria(vo);

            return count.ToString();
        }

        public void BindEmployeeInbox()
        {
            //SearchCriteria criteria = new SearchCriteria();

            //criteria.UserID = UserAccount.UserID;
            ////criteria.FlagQuery = "Employee";
            //criteria.LanguageID = UserAccount.CurrentLanguageID;

            ////IList<SearchResultData> criteriaList = ScgeAccountingQueryProvider.SCGDocumentQuery.FindInboxEmployeeSummaryCriteria(criteria);
            //IList<SearchResultData> criteriaList = ScgeAccountingQueryProvider.SCGDocumentQuery.FindInboxAccountantPaymentSummaryCriteria(criteria);

            //ctlTotalInboxEmpSummary.Text = CountTotalAccountantPaymentInbox(criteriaList, 1);  //criteriaList.Count.ToString();
            //ctlRejectWithdrawEmpSummary.Text = CountEmployeeInbox(criteriaList, WorkFlowStateFlag.Draft, 1);
            //ctlWaitforAgreeEmpSummary.Text = CountEmployeeInbox(criteriaList, WorkFlowStateFlag.WaitAgree, 1);
            //ctlWaitforInitialEmpSummary.Text = CountEmployeeInbox(criteriaList, WorkFlowStateFlag.WaitInitial, 1);
            //ctlWaitforApproveEmpSummary.Text = CountEmployeeInbox(criteriaList, WorkFlowStateFlag.WaitApprove, 1);
            //ctlHoldEmpSummary.Text = CountEmployeeInbox(criteriaList, WorkFlowStateFlag.Hold, 1);
            //ctlDraftEmpSummary.Text = this.BindEmployeeDraftInbox();

            ctlTotalUnclearAdvanceEmpSummary.Text = this.BindEmployeeUnclearAdvenceInbox();
            ctlTotalUnclearAdvanceOverdueEmpSummary.Text = this.BindEmployeeUnclearAdvenceOverdueInbox();
        }

        public void BindAccountantInbox()
        {
            SearchCriteria criteria = new SearchCriteria();

            criteria.UserID = UserAccount.UserID;
            //criteria.FlagJoin = "Inbox";
            //criteria.FlagSearch = "Accountant";
            criteria.LanguageID = UserAccount.CurrentLanguageID;

            IList<SearchResultData> criteriaList = ScgeAccountingQueryProvider.SCGDocumentQuery.FindInboxAccountantPaymentSummaryCriteria(criteria);

            //Employee
            ctlTotalInboxEmpSummary.Text = CountTotalAccountantPaymentInbox(criteriaList, 1);  //criteriaList.Count.ToString();
            ctlRejectWithdrawEmpSummary.Text = CountEmployeeInbox(criteriaList, WorkFlowStateFlag.Draft, 1);
            ctlWaitforAgreeEmpSummary.Text = CountEmployeeInbox(criteriaList, WorkFlowStateFlag.WaitAgree, 1);
            ctlWaitforInitialEmpSummary.Text = CountEmployeeInbox(criteriaList, WorkFlowStateFlag.WaitInitial, 1);
            ctlWaitforApproveEmpSummary.Text = CountEmployeeInbox(criteriaList, WorkFlowStateFlag.WaitApprove, 1);
            ctlHoldEmpSummary.Text = CountEmployeeInbox(criteriaList, WorkFlowStateFlag.Hold, 1);
            //ctlDraftEmpSummary.Text = CountEmployeeInbox(criteriaList, WorkFlowStateFlag.Draft, null);
            ctlDraftEmpSummary.Text = ScgeAccountingQueryProvider.SCGDocumentQuery.CountDraftNoDocumentEmployeeCriteria(criteria).ToString();

            //Accountant
            ctlTotalInboxAccSummary.Text = CountTotalAccountantPaymentInbox(criteriaList, 2); //criteriaList.Count.ToString();
            ctlWaitforReceiveAccSummary.Text = CountAccountantPaymentInbox(criteriaList, WorkFlowStateFlag.WaitReceive,2);
            ctlWaitforVerifyAccSummary.Text = CountAccountantPaymentInbox(criteriaList, WorkFlowStateFlag.WaitVerify,2);
            ctlWaitforApproveVerifyAccSummary.Text = CountAccountantPaymentInbox(criteriaList, WorkFlowStateFlag.WaitApproveVerify,2);
            ctlHoldAccSummary.Text = CountAccountantPaymentInbox(criteriaList, WorkFlowStateFlag.Hold,2);

            //Payment
            ctlTotalInboxPaySummary.Text = CountTotalAccountantPaymentInbox(criteriaList, 3); //criteriaList.Count.ToString();
            ctlWaitforVerifyPaySummary.Text = CountAccountantPaymentInbox(criteriaList, WorkFlowStateFlag.WaitVerify, 3);
            ctlWaitforApproveVerifyPaySummary.Text = CountAccountantPaymentInbox(criteriaList, WorkFlowStateFlag.WaitApproveVerify, 3);
            ctlWaitforPaymentPaySummary.Text = CountAccountantPaymentInbox(criteriaList, WorkFlowStateFlag.WaitPayment, 3);
            ctlWaitforRemittancePaySummary.Text = CountAccountantPaymentInbox(criteriaList, WorkFlowStateFlag.WaitRemittance, 3);
            ctlWaitforApproveRemittancePaySummary.Text = CountAccountantPaymentInbox(criteriaList, WorkFlowStateFlag.WaitApproveRemittance, 3);
        }

        public void BindPaymentInbox()
        {
            SearchCriteria criteria = new SearchCriteria();

            criteria.UserID = UserAccount.UserID;
            //criteria.FlagJoin = "Inbox";
            //criteria.FlagSearch = "Payment";
            criteria.LanguageID = UserAccount.CurrentLanguageID;

            IList<SearchResultData> criteriaList = ScgeAccountingQueryProvider.SCGDocumentQuery.FindInboxAccountantPaymentSummaryCriteria(criteria);

            ctlTotalInboxPaySummary.Text = CountTotalAccountantPaymentInbox(criteriaList, 3); //criteriaList.Count.ToString();
            ctlWaitforVerifyPaySummary.Text = CountAccountantPaymentInbox(criteriaList, WorkFlowStateFlag.WaitVerify,3);
            ctlWaitforApproveVerifyPaySummary.Text = CountAccountantPaymentInbox(criteriaList, WorkFlowStateFlag.WaitApproveVerify,3);
            ctlWaitforPaymentPaySummary.Text = CountAccountantPaymentInbox(criteriaList, WorkFlowStateFlag.WaitPayment,3);
            ctlWaitforRemittancePaySummary.Text = CountAccountantPaymentInbox(criteriaList, WorkFlowStateFlag.WaitRemittance,3);
            ctlWaitforApproveRemittancePaySummary.Text = CountAccountantPaymentInbox(criteriaList, WorkFlowStateFlag.WaitApproveRemittance,3);
        }

        public string CountEmployeeInbox(IList<SearchResultData> criteriaList, string documentStatus, int? taskGroup)
        {
            var result = from SearchResultData searchResult in criteriaList
                         where searchResult.DocumentStatus == documentStatus
                         && searchResult.TaskGroup == taskGroup
                         select searchResult;

            //return result.ToList<SearchResultData>().Count.ToString();
            if (result.ToList<SearchResultData>().Count > 0)
                return result.ToList<SearchResultData>()[0].ItemCount.ToString();
            else
                return "0";
        }

        public string CountAccountantPaymentInbox(IList<SearchResultData> criteriaList, string documentStatus , int? taskGroup)
        {
            var result = from SearchResultData searchResult in criteriaList
                         where searchResult.DocumentStatus == documentStatus
                         && searchResult.TaskGroup == taskGroup
                         select searchResult;

            //return result.ToList<SearchResultData>().Count.ToString();
            if (result.ToList<SearchResultData>().Count > 0)
                return result.ToList<SearchResultData>()[0].ItemCount.ToString();
            else
                return "0";
        }

        public string CountTotalAccountantPaymentInbox(IList<SearchResultData> criteriaList, int? taskGroup)
        {
            var result = from SearchResultData searchResult in criteriaList
                         where searchResult.TaskGroup == taskGroup
                         select searchResult.ItemCount;

            return result.Sum().ToString();
        }

        protected void InboxEmployee_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Forms/SCG.eAccounting/Programs/EmployeeInbox.aspx");
        }

        protected void InboxDraft_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Forms/SCG.eAccounting/Programs/Draft.aspx");
        }

        protected void InboxAccountant_Click(object sender, EventArgs e)
        {
            LinkButton L1 = (LinkButton)sender;
            string displayName = L1.CommandArgument;

            Response.Redirect("~/Forms/SCG.eAccounting/Programs/AccountantPaymentInbox.aspx?displayName=" + displayName + "");
        }

        protected void InboxAccountantWaitforReceive_Click(object sender, EventArgs e)
        {
            LinkButton L1 = (LinkButton)sender;
            string displayName = string.Empty;

            if (UserAccount.CurrentLanguageID == 1)
            {
                displayName = ctlWaitforReceiveAcc.Text;
            }
            else
            {
                displayName = L1.CommandArgument;
            }

            Response.Redirect("~/Forms/SCG.eAccounting/Programs/AccountantPaymentInbox.aspx?displayName=" + displayName + "");
        }

        protected void InboxAccountantWaitforVerify_Click(object sender, EventArgs e)
        {
            //LinkButton L1 = (LinkButton)sender;
            string displayName = ctlWaitforVerifyAcc.Text;//L1.CommandArgument;

            Response.Redirect("~/Forms/SCG.eAccounting/Programs/AccountantPaymentInbox.aspx?displayName=" + displayName + "");
        }

        protected void InboxAccountantWaitforApproveVerify_Click(object sender, EventArgs e)
        {
            //LinkButton L1 = (LinkButton)sender;
            string displayName = ctlWaitforApproveVerifyAcc.Text; // L1.CommandArgument;

            Response.Redirect("~/Forms/SCG.eAccounting/Programs/AccountantPaymentInbox.aspx?displayName=" + displayName + "");
        }

        protected void InboxAccountantHold_Click(object sender, EventArgs e)
        {
            //LinkButton L1 = (LinkButton)sender;
            string displayName = ctlHoldAcc.Text; // L1.CommandArgument;

            Response.Redirect("~/Forms/SCG.eAccounting/Programs/AccountantPaymentInbox.aspx?displayName=" + displayName + "");
        }

        protected void InboxPayment_Click(object sender, EventArgs e)
        {
            LinkButton L1 = (LinkButton)sender;
            string displayName = L1.CommandArgument;

            Response.Redirect("~/Forms/SCG.eAccounting/Programs/PaymentInbox.aspx?displayName=" + displayName + "");
        }

        protected void InboxPaymentWaitforVerify_Click(object sender, EventArgs e)
        {
            //LinkButton L1 = (LinkButton)sender;
            string displayName = ctlWaitforVerifyPay.Text; // L1.CommandArgument;

            Response.Redirect("~/Forms/SCG.eAccounting/Programs/PaymentInbox.aspx?displayName=" + displayName + "");
        }

        protected void InboxPaymentWaitforApproveVerify_Click(object sender, EventArgs e)
        {
            //LinkButton L1 = (LinkButton)sender;
            string displayName = ctlWaitforApproveVerifyPay.Text; // L1.CommandArgument;

            Response.Redirect("~/Forms/SCG.eAccounting/Programs/PaymentInbox.aspx?displayName=" + displayName + "");
        }

        protected void InboxPaymentWaitforPayment_Click(object sender, EventArgs e)
        {
            //LinkButton L1 = (LinkButton)sender;
            string displayName = ctlWaitforPaymentPay.Text; // L1.CommandArgument;

            Response.Redirect("~/Forms/SCG.eAccounting/Programs/PaymentInbox.aspx?displayName=" + displayName + "");
        }

        protected void InboxPaymentWaitforRemittance_Click(object sender, EventArgs e)
        {
            //LinkButton L1 = (LinkButton)sender;
            string displayName = ctlWaitforRemittancePay.Text; // L1.CommandArgument;

            Response.Redirect("~/Forms/SCG.eAccounting/Programs/PaymentInbox.aspx?displayName=" + displayName + "");
        }

        protected void InboxPaymentWaitforApproveRemittance_Click(object sender, EventArgs e)
        {
            //LinkButton L1 = (LinkButton)sender;
            string displayName = ctlWaitforApproveRemittancePay.Text; // L1.CommandArgument;

            Response.Redirect("~/Forms/SCG.eAccounting/Programs/PaymentInbox.aspx?displayName=" + displayName + "");
        }

        public void VisibleAccountant()
        {

            if (UserAccount.IsAccountant)
            {
                ctlDivAccountant.Style["display"] = "block";
            }
            else
            {
                ctlDivAccountant.Style["display"] = "none";
            }
        }

        public void VisiblePayment()
        {
            if (UserAccount.IsPayment)
            {
                ctlDivPayment.Style["display"] = "block";
            }
            else
            {
                ctlDivPayment.Style["display"] = "none";
            }
        }
    }
}
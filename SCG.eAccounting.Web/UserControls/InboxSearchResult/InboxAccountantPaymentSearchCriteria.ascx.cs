using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCG.eAccounting.Query;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.Web.Helper;
using SS.Standard.UI;
using System.Collections;
using SS.Standard.Security;
using SS.SU.Query;
using SCG.DB.DTO;
using SCG.DB.Query;
using SS.Standard.WorkFlow.Query;

namespace SCG.eAccounting.Web.UserControls.InboxSearchResult
{
    public partial class InboxAccountantPaymentSearchCriteria : BaseUserControl
    {
        #region Property
        #region public string FlagSearch
        public string FlagSearch
        {
            get
            {
                if (ViewState["FlagSearch"] == null)
                    return string.Empty;
                else
                    return ViewState["FlagSearch"].ToString();
            }

            set { ViewState["FlagSearch"] = value; }
        }
        #endregion
        #region public string FlagJoin
        public string FlagJoin
        {
            get
            {
                if (ViewState["FlagJoin"] == null)
                    return string.Empty;
                else
                    return ViewState["FlagJoin"].ToString();
            }

            set { ViewState["FlagJoin"] = value; }
        }
        #endregion
        #region public string Legend
        public string Legend
        {
            set { ctlInboxEmployeeSearchResult.Legend = value; }
        }
        #endregion
        #region public bool isShowCriteria
        public bool isShowCriteria
        {
            get
            {
                if (ViewState["isShowCriteria"] == null)
                    return false;
                else
                    return bool.Parse(ViewState["isShowCriteria"].ToString());
            }

            set { ViewState["isShowCriteria"] = value; }
        }
        #endregion
        #region public string DisplayName
        public string DisplayName { get; set; }
        #endregion
        #region public string SearchType
        public string SearchType
        {
            get
            {
                if (ViewState["SearchType"] == null)
                    return string.Empty;
                else
                    return ViewState["SearchType"].ToString();
            }

            set { ViewState["SearchType"] = value; }
        }
        #endregion
        #endregion Property

        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                this.SetRequestType();
                this.SetStatus();
                this.SetRole();
                this.GetCriteriaSession();

                if (!string.IsNullOrEmpty(SearchType))
                {
                    //string searchType = Request.QueryString["searchType"];
                    IList<short> userRoleServiceTeamList = new List<short>();
                    IList<short> userRolePbList = new List<short>();

                    if (SearchType.Equals("1")) //admim, employee
                    {
                        userRoleServiceTeamList = QueryProvider.SuUserRoleQuery.GetUserRoleServiceTeamByUserID(0);
                        userRolePbList = QueryProvider.SuUserRoleQuery.GetUserRolePBByUserID(0);

                        ctlServiceTeam.ServiceTeamDataBind(0, userRoleServiceTeamList);
                        ctlPB.PBRoleNameBind(UserAccount.UserLanguageID, 0, userRolePbList);
                    }
                    else if (SearchType.Equals("2")) //accountant inbox, search
                    {
                        userRoleServiceTeamList = QueryProvider.SuUserRoleQuery.GetUserRoleServiceTeamByUserID(UserAccount.UserID);
                        userRolePbList = QueryProvider.SuUserRoleQuery.GetUserRolePBByUserID(0);

                        ctlServiceTeam.ServiceTeamDataBind(UserAccount.UserID, userRoleServiceTeamList);
                        ctlPB.PBRoleNameBind(UserAccount.UserLanguageID, 0, userRolePbList);
                    }
                    else if (SearchType.Equals("3")) // payment inbox, search
                    {
                        userRoleServiceTeamList = QueryProvider.SuUserRoleQuery.GetUserRoleServiceTeamByUserID(0);
                        userRolePbList = QueryProvider.SuUserRoleQuery.GetUserRolePBByUserID(UserAccount.UserID);

                        ctlServiceTeam.ServiceTeamDataBind(0, userRoleServiceTeamList);
                        ctlPB.PBRoleNameBind(UserAccount.UserLanguageID, UserAccount.UserID, userRolePbList);
                    }
                }

                if (FlagJoin.ToLower().Equals("inbox") && ((FlagSearch.ToLower().Equals("accountant")) || FlagSearch.ToLower().Equals("payment")))
                {
                    selecteHrFrom.Visible = true;
                    ImageCheckBox.Visible = true;
                    HardCopyCheckBox.Visible = true;
                    ImageHardCopy.Visible = true;
                    ctlImageOptionDiv.Visible = true;
                }
                else 
                {
                    selecteHrFrom.Visible = false;
                    ImageCheckBox.Visible = false;
                    HardCopyCheckBox.Visible = false;
                    ImageHardCopy.Visible = false;
                    ctlImageOptionDiv.Visible = false;
                }
               
                //show search result when inbox page.
                if (!string.IsNullOrEmpty(Request.QueryString["displayName"]) || (!string.IsNullOrEmpty(FlagJoin) &&
                    ((!FlagJoin.ToLower().Equals("history")) && !FlagJoin.ToLower().Equals("search")) &&
                    (FlagJoin.ToLower().Equals("inbox") && FlagSearch.ToLower().Equals("payment"))) || 
                    (FlagJoin.ToLower().Equals("history") && FlagSearch.ToLower().Equals("employee")))
                {
                    this.DisplayName = Request.QueryString["displayName"];
                
                    this.SearchCriteria();
                    ctlInboxEmployeeSearchResult.isShowMsg = true;
                }
                else
                {
                    ctlInboxEmployeeSearchResult.isShowMsg = false;
                    ctlInboxEmployeeSearchResult.Visible = false;
                }
                if (FlagJoin.ToLower().Equals("inbox") && ((ctlStatus.SelectedValue == "Wait for Approve Verify" || ctlStatus.SelectedValue == "Wait for Approve Remittance") || (ctlStatus.SelectedValue == "รออนุมัติตรวจจ่าย") || (ctlStatus.SelectedValue == "รอการอนุมัติรับเงินคืน")))
                {
                    ctlCheckboxMultipleApprove.Visible = true;
                }
                else
                {
                    ctlCheckboxMultipleApprove.Visible = false;
                    ctlCheckboxMultipleApprove.Checked = false;
                }
                ctlInboxEmployeeSearchResult.VisibleColumnCreateDate = false;
                ctlInboxEmployeeSearchResult.VisibleColumnDocumentType = false;

                collapPanel1.CollapsedText = GetMessage("ShowSearchBox");
                collapPanel1.ExpandedText = GetMessage("HideSearchBox");

                isShowCriteria = false;
                ctlInboxEmployeeSearchResult.DataBind();
                ctlUpdatePanelSearchResult.Update();
            }
            else
            {
                if (FlagJoin.ToLower().Equals("inbox") && ((ctlStatus.SelectedValue == "Wait for Approve Verify" || ctlStatus.SelectedValue == "Wait for Approve Remittance") || (ctlStatus.SelectedValue == "รออนุมัติตรวจจ่าย") || (ctlStatus.SelectedValue == "รอการอนุมัติรับเงินคืน")))
                {
                    ctlCheckboxMultipleApprove.Visible = true;
        }
                else
                {
                    ctlCheckboxMultipleApprove.Visible = false;
                    ctlCheckboxMultipleApprove.Checked = false;
                }
            }
        }
        #endregion protected void Page_Load(object sender, EventArgs e)

        #region Set Dropdownlist
        #region private void SetRequestType()
        private void SetRequestType()
        {
            IList<TranslatedListItem> documentTypeList = ScgeAccountingQueryProvider.SCGDocumentQuery.FindDocumentTypeCriteria();

            var items = from documentTypes in documentTypeList
                        select new TranslatedListItem() { ID = documentTypes.ID, Symbol = GetMessage(string.Concat("DT_", documentTypes.Symbol)) };

            ctlRequestType.DataSource = items.ToList<TranslatedListItem>();
            ctlRequestType.DataTextField = "Symbol";
            ctlRequestType.DataValueField = "ID";
            ctlRequestType.DataBind();

            ctlRequestType.Items.Insert(0, new ListItem(GetMessage("All_Item"), string.Empty));
        }
        #endregion private void SetRequestType()

        #region private void SetStatus()
        private void SetStatus()
        {
            ctlStatus.DataSource = ScgeAccountingQueryProvider.SCGDocumentQuery.FindStatusCriteria(UserAccount.CurrentLanguageID, UIHelper.ParseInt(ctlRequestType.SelectedValue));
            //ctlStatus.DataValueField = "ID"; 
            ctlStatus.DataValueField = "Symbol";
            ctlStatus.DataTextField = "Symbol";
            ctlStatus.DataBind();

            ctlStatus.Items.Insert(0, new ListItem(GetMessage("All_Item"), string.Empty));
        }
        #endregion private void SetStatus()

        #region private void SetRole()
        private void SetRole()
        {
            if (UserAccount.CurrentLanguageID.Equals(1))
            {
                ctlRole.Items.Add(new ListItem("ทั้งหมด", "ALL"));
                ctlRole.Items.Add(new ListItem("ผู้ออกเอกสาร", "Creator"));
                ctlRole.Items.Add(new ListItem("ผู้ขอเบิก", "Requester"));
                ctlRole.Items.Add(new ListItem("ผู้ตรวจรายการ", "Initiator"));
                ctlRole.Items.Add(new ListItem("ผู้อนุมัติ", "Approver"));
            }
            else if (UserAccount.CurrentLanguageID.Equals(2))
            {
                ctlRole.Items.Add(new ListItem("ALL", "ALL"));
                ctlRole.Items.Add(new ListItem("Creator", "Creator"));
                ctlRole.Items.Add(new ListItem("Requester", "Requester"));
                ctlRole.Items.Add(new ListItem("Initiator", "Initiator"));
                ctlRole.Items.Add(new ListItem("Approver", "Approver"));
            }
        }
        #endregion private void SetRole()
        #endregion Set Dropdownlist

        #region protected void ctlSearch_Click(object sender, EventArgs e)
        protected void ctlSearch_Click(object sender, EventArgs e)
        {
            ctlInboxEmployeeSearchResult.isShowMsg = true;
            this.SearchCriteria();
        }
        #endregion protected void ctlSearch_Click(object sender, EventArgs e)

        #region public SearchCriteria BuildCriteria()
        public SearchCriteria BuildCriteria()
        {
            SearchCriteria criteria = new SearchCriteria();
            string role = string.Empty;
            criteria.DocumentTypeID = UIHelper.ParseLong(ctlRequestType.SelectedValue);
            if (!string.IsNullOrEmpty(this.DisplayName))
            {
                criteria.Status = this.DisplayName;
                ctlStatus.SelectedValue = this.DisplayName;
            }
            else
            {
                criteria.Status = ctlStatus.SelectedValue;
            }
            if (!ctlRole.SelectedValue.Equals("ALL"))
            {
                role = ctlRole.SelectedValue;
            }
            criteria.Role = role;
            if (!string.IsNullOrEmpty(ctlCompanyTextboxAutoComplete.CompanyCode))
            {
                DbCompany dbCom = ScgDbQueryProvider.DbCompanyQuery.getDbCompanyByCompanyCode(ctlCompanyTextboxAutoComplete.CompanyCode);
                if (dbCom != null)
                {
                    criteria.CompanyID = dbCom.CompanyID;
                }
            }

            criteria.RequestNo = ctlRequestNo.Text;
            criteria.CreatorID = UIHelper.ParseLong(ctlUserAutoCompleteLookupCreator.EmployeeID);
            criteria.RequesterID = UIHelper.ParseLong(ctlUserAutoCompleteLookupRequester.EmployeeID);
            criteria.Receiver = UIHelper.ParseLong(ctlUserAutoCompleteLookupReceiver.EmployeeID);
            criteria.RequestDateFrom = ctlRequestDateFrom.DateValue;
            criteria.RequestDateTo = ctlRequestDateTo.DateValue;
            criteria.Subject = ctlSubject.Text;
            criteria.AmountFrom = UIHelper.ParseDouble(ctlAmountFrom.Text);
            criteria.AmountTo = UIHelper.ParseDouble(ctlAmountTo.Text);
            criteria.UserID = UserAccount.UserID;
            criteria.FlagQuery = "AccountantPayment";
            criteria.FlagSearch = FlagSearch.ToString();
            criteria.FlagJoin = FlagJoin.ToString();
            criteria.LanguageID = UserAccount.CurrentLanguageID;
            criteria.ReferneceNo = ctlRefenenceNo.Text;
            criteria.ServiceTeamID = UIHelper.ParseLong(ctlServiceTeam.SelectedValue);
            criteria.PBID = UIHelper.ParseLong(ctlPB.SelectedValue);
            criteria.SearchType = this.SearchType;
            criteria.MutipleApprove = ctlCheckboxMultipleApprove.Checked;
            criteria.SelcetseHrFrom = selecteHrFrom.Checked;

            if (ImageCheckBox.Checked)
            {
                criteria.ImageOption.Add(ImageOptionCriteria.Image);
            }
            if (HardCopyCheckBox.Checked)
            {
                criteria.ImageOption.Add(ImageOptionCriteria.HardCopy);
            }
            if (ImageHardCopy.Checked)
            {
                criteria.ImageOption.Add(ImageOptionCriteria.ImageHardCopy);
            }
            return criteria;
        }
        #endregion public SearchCriteria BuildCriteria()

        #region public string GetCurrentPageName()
        public string GetCurrentPageName()
        {
            string sPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
            System.IO.FileInfo oInfo = new System.IO.FileInfo(sPath);
            string sRet = oInfo.Name;
            return sRet;
        }
        #endregion public string GetCurrentPageName()

        #region public void GetCriteriaSession()
        public void GetCriteriaSession()
        {
            string role = "ALL";
            string pageName = this.GetCurrentPageName();
            string sessionName = pageName + "_" + UserAccount.UserID.ToString();

            if (Session[sessionName] != null)
            {
                SearchCriteria searchCriteria = Session[sessionName] as SearchCriteria;

                ctlRequestType.SelectedValue = searchCriteria.DocumentTypeID.ToString();
                if (!string.IsNullOrEmpty(this.DisplayName))
                {
                    ctlStatus.SelectedValue = this.DisplayName;
                }
                else
                {
                    ctlStatus.SelectedValue = searchCriteria.Status.ToString();
                }
                if (!searchCriteria.Role.ToString().Equals(string.Empty))
                {
                    role = searchCriteria.Role.ToString();
                }
                ctlRole.SelectedValue = role;
                ctlCompanyTextboxAutoComplete.CompanyID = searchCriteria.CompanyID.ToString();
                ctlRequestNo.Text = searchCriteria.RequestNo.ToString();
                ctlUserAutoCompleteLookupCreator.EmployeeID = searchCriteria.CreatorID.ToString();
                ctlUserAutoCompleteLookupRequester.EmployeeID = searchCriteria.RequesterID.ToString();
                ctlUserAutoCompleteLookupReceiver.EmployeeID = searchCriteria.Receiver.ToString();
                ctlRequestDateFrom.DateValue = searchCriteria.RequestDateFrom.ToString();
                ctlRequestDateTo.DateValue = searchCriteria.RequestDateTo.ToString();
                ctlSubject.Text = searchCriteria.Subject.ToString();
                ctlAmountFrom.Text = searchCriteria.AmountFrom.ToString();
                ctlAmountTo.Text = searchCriteria.AmountTo.ToString();
                ctlRefenenceNo.Text = searchCriteria.ReferneceNo.ToString();
                FlagSearch = searchCriteria.FlagSearch.ToString();
                FlagJoin = searchCriteria.FlagJoin.ToString();
                ctlUserAutoCompleteLookupCreator.UserNameID = searchCriteria.CreatorID.ToString();
                ctlUserAutoCompleteLookupRequester.UserNameID = searchCriteria.RequesterID.ToString();
                ctlUserAutoCompleteLookupReceiver.UserNameID = searchCriteria.Receiver.ToString();
                ctlCompanyTextboxAutoComplete.CompanyCodeByCompanyID = searchCriteria.CompanyID.ToString();
                if (searchCriteria.PBID != 0)
                    ctlPB.SelectedValue = searchCriteria.PBID.ToString();
                if (searchCriteria.ServiceTeamID != 0)
                    ctlServiceTeam.SelectedValue = searchCriteria.ServiceTeamID.ToString();

                ctlCheckboxMultipleApprove.Checked = searchCriteria.MutipleApprove;
                selecteHrFrom.Checked = searchCriteria.SelcetseHrFrom;

                foreach (ImageOptionCriteria img in searchCriteria.ImageOption)
                {
                    if (img.Equals(ImageOptionCriteria.Image))
                    {
                        ImageCheckBox.Checked = true;
            		}
                    else if (img.Equals(ImageOptionCriteria.HardCopy))
                    {
                        HardCopyCheckBox.Checked = true;
        			}
                    else if (img.Equals(ImageOptionCriteria.ImageHardCopy))
                    {
                        ImageHardCopy.Checked = true;
                    }
                }
            }
        }
        #endregion public void GetCriteriaSession()

        #region public void SearchCriteria()
        public void SearchCriteria()
        {
            SearchCriteria criteria = this.BuildCriteria();

            //set criteria to session
            string pageName = this.GetCurrentPageName();
            Session[pageName + "_" + UserAccount.UserID.ToString()] = criteria;

            ctlInboxEmployeeSearchResult.VisibleApprove = criteria.MutipleApprove;
            ctlInboxEmployeeSearchResult.VisibleColumnSelect = criteria.MutipleApprove;
            ctlInboxEmployeeSearchResult.BindInboxGridView(criteria);
            ctlInboxEmployeeSearchResult.Visible = true;
            ctlUpdatePanelSearchResult.Update();
        }
        #endregion public void SearchCriteria()

        #region protected void ctlRequestType_OnSelectedIndexChanged(object sender, EventArgs e)
        protected void ctlRequestType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ctlCheckboxMultipleApprove.Visible = false;
			ctlCheckboxMultipleApprove.Checked = false;
            this.SetStatus();
        }
        #endregion protected void ctlRequestType_OnSelectedIndexChanged(object sender, EventArgs e)

        #region protected void ctlStatus_OnSelectedIndexChanged(object sender, EventArgs e)
        protected void ctlStatus_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (FlagJoin.ToLower().Equals("inbox") && ((ctlStatus.SelectedValue == "Wait for Approve Verify" || ctlStatus.SelectedValue == "Wait for Approve Remittance") || (ctlStatus.SelectedValue == "รออนุมัติตรวจจ่าย") || (ctlStatus.SelectedValue == "รอการอนุมัติรับเงินคืน")))
            {
                ctlCheckboxMultipleApprove.Visible = true;
            }
            else
            {
                ctlCheckboxMultipleApprove.Visible = false;
                ctlCheckboxMultipleApprove.Checked = false;
            }
        }
        #endregion protected void ctlStatus_OnSelectedIndexChanged(object sender, EventArgs e)

        protected void ctlRefNoOpen_Click(object sender, EventArgs e)
        {
            SearchCriteria criteria = this.BuildCriteria();
            //set criteria to session
            string pageName = this.GetCurrentPageName();
            Session[pageName + "_" + UserAccount.UserID.ToString()] = criteria;

            if (!string.IsNullOrEmpty(ctlRefenenceNo.Text))
            {
                IList<long> docId = ScgeAccountingQueryProvider.SCGDocumentQuery.GetDocumentIDByReferenceNo(ctlRefenenceNo.Text);
                if (docId.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "js", "alert('Reference No. is not found.');", true);
                }
                else if (docId.Count > 1)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "js", "alert('Can not open document because many documents are using this reference No.');", true);
                }
                else
                {
                   SS.Standard.WorkFlow.DTO.WorkFlow wf = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(docId[0]);
                   Response.Redirect("DocumentView.aspx?wfid=" + wf.WorkFlowID.ToString());
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "js", "alert('Please input reference No.');", true);
            }
        }

        protected void ctlReqNoOpenBtn_Click(object sender, EventArgs e)
        {
            SearchCriteria criteria = this.BuildCriteria();
            //set criteria to session
            string pageName = this.GetCurrentPageName();
            Session[pageName + "_" + UserAccount.UserID.ToString()] = criteria;

            if (!string.IsNullOrEmpty(ctlRequestNo.Text))
            {
                IList<long> docId = ScgeAccountingQueryProvider.SCGDocumentQuery.GetDocumentIDByDocumentNo(ctlRequestNo.Text);
                if (docId.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "js", "alert('Request No. is not found.');", true);
                }
                else if (docId.Count > 1)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "js", "alert('Can not open document because many documents are using this reference No.');", true);
                }
                else
                {
                    SS.Standard.WorkFlow.DTO.WorkFlow wf = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(docId[0]);
                    Response.Redirect("DocumentView.aspx?wfid=" + wf.WorkFlowID.ToString());
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "js", "alert('Please input request No.');", true);
            }
        }
    }
}

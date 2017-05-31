using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCG.eAccounting.Web.Helper;
using SS.Standard.UI;
using SCG.eAccounting.DTO.DataSet;
using SCG.eAccounting.BLL;
using SCG.eAccounting.DTO;
using SCG.eAccounting.Query;
using SCG.eAccounting.DTO.ValueObject;
using SS.DB.BLL;
using SS.DB.Query;
using SS.Standard.Data.Linq;
using SS.SU.Query;
using SS.Standard.WorkFlow.Query;
using SS.DB.DTO;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor.Components
{
    public partial class ClearingAdvance : BaseUserControl
    {
        public ITransactionService TransactionService { get; set; }
        public IFnExpenseDocumentService FnExpenseDocumentService { get; set; }
        public IFnExpenseAdvanceService FnExpenseAdvanceService { get; set; }
        //public IFnExpenseRemittanceService FnExpenseRemittanceService { get; set; }
        public IDbParameterService DbParameterService { get; set; }
        #region Property
        public Guid TransactionId
        {
            get { return (Guid)ViewState[ViewStateName.TransactionID]; }
            set { ViewState[ViewStateName.TransactionID] = value; }
        }
        public long ExpDocumentID
        {
            get { return (long)ViewState[ViewStateName.DocumentID]; }
            set { ViewState[ViewStateName.DocumentID] = value; }
        }
        public string DocumentType
        {
            get { return ctlType.Value; }
            set { ctlType.Value = value; }
        }
        public string InitialFlag
        {
            get { return ViewState[ViewStateName.InitialFlag].ToString(); }
            set { ViewState[ViewStateName.InitialFlag] = value; }
        }
        public long CompanyID
        {
            get { return UIHelper.ParseLong(ViewState["CompanyID"].ToString()); }
            set { ViewState["CompanyID"] = value; }
        }
        public long RequesterID
        {
            get { return UIHelper.ParseLong(ViewState["RequesterID"].ToString()); }
            set { ViewState["RequesterID"] = value; }
        }
        public long PBID
        {
            get
            {
                if (ViewState["PBID"] == null)
                {
                    return 0;
                }
                return UIHelper.ParseLong(ViewState["PBID"].ToString());
            }
            set { ViewState["PBID"] = value; }
        }
        public short MainCurrencyID
        {
            get
            {
                if (ViewState["MainCurrencyID"] == null)
                {
                    return 0;
                }
                return UIHelper.ParseShort(ViewState["MainCurrencyID"].ToString());
            }
            set { ViewState["MainCurrencyID"] = value; }
        }
        public bool IsDomestic
        {
            set { ctlControlPanel.Visible = value; }
        }
        public bool IsEmptyData
        {
            get { return (bool)ViewState["IsEmptyData"]; }
            set { ViewState["IsEmptyData"] = value; }
        }

        protected bool IsEmptyAdvance
        {
            get { return (bool)ViewState["IsEmptyAdvance"]; }
            set { ViewState["IsEmptyAdvance"] = value; }
        }
        protected bool IsEmptyRemittance
        {
            get { return (bool)ViewState["IsEmptyRemittance"]; }
            set { ViewState["IsEmptyRemittance"] = value; }
        }
        public string AdvanceNoTemp
        {
            get
            {
                if (ViewState["AdvanceNoTemp"] != null)
                {
                    return ViewState["AdvanceNoTemp"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set { ViewState["AdvanceNoTemp"] = value; }
        }
        public bool IsRepOffice
        {
            get
            {
                if (ViewState["IsRepOffice"] != null)
                    return (bool)ViewState["IsRepOffice"];
                return false;
            }
            set { ViewState["IsRepOffice"] = value; }
        }
        public long WorkFlowID
        {
            get { return UIHelper.ParseLong(ViewState["WorkFlowID"].ToString()); }
            set { ViewState["WorkFlowID"] = value; }
        }
        public string CurrentState
        {
            get
            {
                if (ViewState["CurrentState"] != null)
                {
                    return ViewState["CurrentState"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set { ViewState["CurrentState"] = value; }
        }
        public ExpenseDocumentEditor ExpenseDocumentEditor { get; set; }
        private bool refreshHeaderGrid;

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            ctlTALookup.OnObjectLookUpCalling += new BaseUserControl.ObjectLookUpCallingEventHandler(TA_OnObjectLookUpCalling);
            ctlTALookup.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(TA_OnObjectLookUpReturn);
            ctlAdvanceLookup.OnObjectLookUpCalling += new BaseUserControl.ObjectLookUpCallingEventHandler(Advance_OnObjectLookUpCalling);
            ctlAdvanceLookup.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(Advance_OnObjectLookUpReturn);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (refreshHeaderGrid)
            {
                ctlAdvanceGridView.DataBind();
            }
        }

        public void Initialize(Guid txID, long documentID, string initFlag)
        {
            this.TransactionId = txID;
            this.ExpDocumentID = documentID;
            this.InitialFlag = initFlag;

            this.BindControl(true);

            if (IsEmptyAdvance && IsEmptyRemittance && string.IsNullOrEmpty(ctlTALinkButton.Text))
                IsEmptyData = true;
            else
                IsEmptyData = false;
        }
        #region AdvanceGridview
        protected void ctlAdvanceGridview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal ctlNoLabel = e.Row.FindControl("ctlNoLabel") as Literal;

                ctlNoLabel.Text = ((ctlAdvanceGridView.PageSize * ctlAdvanceGridView.CustomPageIndex) + (e.Row.RowIndex + 1)).ToString();

                //Sum Amount(THB)
                Literal ctlLblAmount = (Literal)e.Row.FindControl("ctlLblAmount");
                //this.SumAdvanceAmount += UIHelper.ParseInt(ctlLblAmount.Text);
                //ctlTotalAdvanceAmount.Text = this.SumAdvanceAmount.ToString();
                if (InitialFlag.Equals(FlagEnum.ViewFlag))
                {
                    ((ImageButton)e.Row.FindControl("ctlDelete")).Visible = false;
                }

                SCG.eAccounting.DTO.ValueObject.Advance advance = e.Row.DataItem as SCG.eAccounting.DTO.ValueObject.Advance;

                if (!string.IsNullOrEmpty(this.AdvanceNoTemp) && !this.AdvanceNoTemp.Contains(advance.AdvanceNo))
                {
                    this.AdvanceNoTemp += ',' + advance.AdvanceNo;
                }
                else
                {
                    this.AdvanceNoTemp = advance.AdvanceNo;
                }
            }
        }
        protected void ctlAdvanceGridview_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("DeleteAdvance"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                long AdvanceID = UIHelper.ParseLong(ctlAdvanceGridView.DataKeys[rowIndex]["AdvanceID"].ToString());
                Literal ctlLblAmount = (Literal)ctlAdvanceGridView.Rows[rowIndex].FindControl("ctlLblAmount");
                double totalAmount = FnExpenseDocumentService.DeleteExpenseAdvanceFromTransaction(this.TransactionId, AdvanceID, Convert.ToDouble(ctlLblAmount.Text));
                FnExpenseDocumentService.SetTotalAdvance(this.TransactionId, 0, totalAmount);
                FnExpenseDocumentService.RefreshRemittance(this.TransactionId, this.ExpDocumentID);
                this.BindAdvanceGridView();
                this.BindRemittanceGridview();
                this.ExpenseDocumentEditor.NotifyPaymentDetailChange();
                this.AdvanceNoTemp = string.Empty;

            }
            else if (e.CommandName.Equals("PopupDocument"))
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                long workflowID = UIHelper.ParseLong(ctlAdvanceGridView.DataKeys[rowIndex]["WorkflowID"].ToString());
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), string.Empty, "window.open('../Programs/DocumentView.aspx?wfid=" + workflowID.ToString() + "')", true);
            }
        }
        protected void ctlAdvanceGridview_DataBound(object sender, EventArgs e)
        {
            ExpenseDataSet expenseDS = (ExpenseDataSet)TransactionService.GetDS(this.TransactionId);
            if (expenseDS != null)
            {
                ExpenseDataSet.FnExpenseDocumentRow row = expenseDS.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);
                SS.Standard.WorkFlow.DTO.WorkFlow wf = null;
                int Ordinal = 0;
                if (Request["wfid"] != null)
                {
                    WorkFlowID = UIHelper.ParseLong(Request["wfid"].ToString());
                    wf = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(WorkFlowID);
                    CurrentState = wf.CurrentState.Name;
                    Ordinal = wf.CurrentState.Ordinal;
                }
                //DbCurrency localCurrency = null;
                DbCurrency mainCurrency = null;
                //if (!row.IsLocalCurrencyIDNull())
                //{
                //    localCurrency = SsDbQueryProvider.DbCurrencyQuery.FindByIdentity(row.LocalCurrencyID);
                //}
                if (!row.IsMainCurrencyIDNull())
                {
                    mainCurrency = SsDbQueryProvider.DbCurrencyQuery.FindByIdentity(row.MainCurrencyID);
                }
                //if (localCurrency != null)
                //{
                //    ctlAdvanceGridView.Columns[4].HeaderText = string.Format(GetMessage("AmountCurrencyHeaderColumn"), "(" + localCurrency.Symbol + ")");
                //}
                //else
                //{
                ctlAdvanceGridView.Columns[4].HeaderText = string.Format(GetMessage("AmountCurrencyHeaderColumn"), "");
                //}

                if (mainCurrency != null)
                {
                    ctlAdvanceGridView.Columns[5].HeaderText = string.Format(GetMessage("AmountCurrencyHeaderColumn"), "(" + mainCurrency.Symbol + ")");
                    ctlAdvanceGridView.HeaderRow.Cells[5].Text = string.Format(GetMessage("AmountCurrencyHeaderColumn"), "(" + mainCurrency.Symbol + ")");
                }
                else
                {
                    ctlAdvanceGridView.Columns[5].HeaderText = string.Format(GetMessage("AmountCurrencyHeaderColumn"), "");
                }

                string expenseType = FnExpenseDocumentService.GetExpenseType(this.ExpDocumentID, this.TransactionId);
                bool isRepOffice = row.IsIsRepOfficeNull() ? false : row.IsRepOffice;

                if (expenseType.Equals(ZoneType.Domestic))
                {
                    if (isRepOffice)
                    {
                        if (CurrentState.Equals(WorkFlowStateFlag.WaitVerify) || Ordinal > 5)
                        {
                            ctlAdvanceGridView.Columns[4].Visible = false;
                            ctlAdvanceGridView.Columns[5].Visible = true;
                            ctlAdvanceGridView.Columns[6].Visible = true;
                        }
                        else
                        {
                            ctlAdvanceGridView.Columns[4].Visible = false;
                            ctlAdvanceGridView.Columns[5].Visible = true;
                            ctlAdvanceGridView.Columns[6].Visible = false;
                        }
                    }
                    else
                    {
                        ctlAdvanceGridView.Columns[4].Visible = false;
                        ctlAdvanceGridView.Columns[5].Visible = false;
                        ctlAdvanceGridView.Columns[6].Visible = true;
                    }
                }
                else
                {
                    if (isRepOffice)
                    {
                        if (CurrentState.Equals(WorkFlowStateFlag.WaitVerify) || Ordinal > 5)
                        {
                            ctlAdvanceGridView.Columns[4].Visible = false;
                            ctlAdvanceGridView.Columns[5].Visible = true;
                            ctlAdvanceGridView.Columns[6].Visible = true;
                        }
                        else
                        {
                            ctlAdvanceGridView.Columns[4].Visible = false;
                            ctlAdvanceGridView.Columns[5].Visible = true;
                            ctlAdvanceGridView.Columns[6].Visible = false;
                        }
                    }
                    else
                    {
                        ctlAdvanceGridView.Columns[4].Visible = false;
                        ctlAdvanceGridView.Columns[5].Visible = false;
                        ctlAdvanceGridView.Columns[6].Visible = true;
                    }
                }
            }

            if (ctlAdvanceGridView.Rows.Count == 0)
            {
                ctlTANoLookup.Enabled = true;
                ctlDeleteTA.Enabled = true;
                //this.ClearRemittanceGridview();
            }
        }
        public void BindAdvanceGridView()
        {
            ExpenseDataSet expenseDS = (ExpenseDataSet)TransactionService.GetDS(this.TransactionId);
            ExpenseDataSet.FnExpenseAdvanceRow[] rows = (ExpenseDataSet.FnExpenseAdvanceRow[])expenseDS.FnExpenseAdvance.Select();

            List<long> advanceIdList = new List<long>();
            foreach (ExpenseDataSet.FnExpenseAdvanceRow row in rows)
            {
                // Prepare list of advance id for query
                advanceIdList.Add(row.AdvanceID);
            }

            if (advanceIdList.Count > 0)
            {
                if (ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindAdvancDocumentByAdvanceIds(advanceIdList).Count > 0)
                    IsEmptyAdvance = false;
                else
                    IsEmptyAdvance = true;

                ctlAdvanceGridView.DataSource = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindAdvancDocumentByAdvanceIds(advanceIdList);
                ctlAdvanceGridView.DataBind();
                if (!this.IsRepOffice)
                {
                    ctlExchangeRateForPerdiem.Text = FnExpenseDocumentService.GetAdvanceExchangeRate(this.TransactionId, ParameterServices.USDCurrencyID).AdvanceExchangeRateAmount.ToString("#,##0.00000");
                }
                else
                {
                    ctlExchangeRateForPerdiem.Text = FnExpenseDocumentService.GetAdvanceExchangeRateRepOffice(this.TransactionId, ParameterServices.USDCurrencyID, this.ExpDocumentID).AdvanceExchangeRateAmount.ToString("#,##0.00000");
                }
            }
            else
            {
                IsEmptyAdvance = true;

                ctlAdvanceGridView.DataSource = null;
                ctlAdvanceGridView.DataBind();
                ctlExchangeRateForPerdiem.Text = "0.00000";
            }

            ctlUpdatePanelExpenseGeneral.Update();
        }
        #endregion

        #region RemittanceGridview
        protected void ctlRemittanceGridview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal ctlNoLabel = e.Row.FindControl("ctlNoLabel") as Literal;
                ctlNoLabel.Text = ((ctlRemittanceGridview.PageSize * ctlRemittanceGridview.CustomPageIndex) + (e.Row.RowIndex + 1)).ToString();

                //Sum Amount(THB)
                //Label ctlLblAmount = (Label)e.Row.FindControl("ctlLblAmount");
                //this.SumAdvanceAmount += UIHelper.ParseInt(ctlLblAmount.Text);
                //ctlTotalAdvanceAmount.Text = this.SumAdvanceAmount.ToString();

            }
        }
        protected void ctlRemittanceGridview_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("DeleteRemittance"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                long RemittanceID = UIHelper.ParseLong(ctlAdvanceGridView.DataKeys[rowIndex]["RemittanceID"].ToString());
                //FnRemittanceAdvanceService.DeleteRemittanceAdvanceFromTransaction(this.TransactionID, AdvanceID, this.DocumentID);
                //this.DeleteAdvanceFromRemittanceItemByPaymentTypeAndCurrency(rowIndex);
                //this.AdvanceList.RemoveAt(rowIndex);
                //this.BindCtlAdvanceGridview(AdvanceList);
            }
            else if (e.CommandName.Equals("PopupDocument"))
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                long workflowID = UIHelper.ParseLong(ctlRemittanceGridview.DataKeys[rowIndex]["WorkflowID"].ToString());
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), string.Empty, "window.open('../Programs/DocumentView.aspx?wfid=" + workflowID.ToString() + "')", true);
            }
        }
        protected void ctlRemittanceGridview_DataBound(object sender, EventArgs e)
        {
            ExpenseDataSet expenseDS = (ExpenseDataSet)TransactionService.GetDS(this.TransactionId);
            ExpenseDataSet.FnExpenseDocumentRow row = expenseDS.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);
            SS.Standard.WorkFlow.DTO.WorkFlow wf = null;
            int Ordinal = 0;
            if (Request["wfid"] != null)
            {
                WorkFlowID = UIHelper.ParseLong(Request["wfid"].ToString());
                wf = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(WorkFlowID);
                CurrentState = wf.CurrentState.Name;
                Ordinal = wf.CurrentState.Ordinal;
            }
            DbCurrency mainCurrency = null;
            if (!row.IsMainCurrencyIDNull())
            {
                mainCurrency = SsDbQueryProvider.DbCurrencyQuery.FindByIdentity(row.MainCurrencyID);
            }
            if (mainCurrency != null)
            {
                ctlRemittanceGridview.Columns[7].HeaderText = string.Format(GetProgramMessage("ctlLblRemittanceMainAmount"), "(" + mainCurrency.Symbol + ")");
            }

            string expenseType = FnExpenseDocumentService.GetExpenseType(this.ExpDocumentID, this.TransactionId);
            bool isRepOffice = row.IsIsRepOfficeNull() ? false : row.IsRepOffice;

            if (!expenseType.Equals(ZoneType.Domestic))
            {
                if (isRepOffice)
                {
                    if (CurrentState.Equals(WorkFlowStateFlag.WaitVerify) || Ordinal > 5)
                    {
                        ctlRemittanceGridview.Columns[7].Visible = true;
                        ctlRemittanceGridview.Columns[8].Visible = true;
                    }
                    else
                    {
                        ctlRemittanceGridview.Columns[7].Visible = true;
                        ctlRemittanceGridview.Columns[8].Visible = false;
                    }
                }
                else
                {
                    ctlRemittanceGridview.Columns[7].Visible = false;
                    ctlRemittanceGridview.Columns[8].Visible = true;
                }
            }
            
        }
        public void BindRemittanceGridview()
        {
            ExpenseDataSet expenseDS = (ExpenseDataSet)TransactionService.GetDS(this.TransactionId);
            ExpenseDataSet.FnExpenseAdvanceRow[] rows = (ExpenseDataSet.FnExpenseAdvanceRow[])expenseDS.FnExpenseAdvance.Select();

            List<long> advanceIdList = new List<long>();
            foreach (ExpenseDataSet.FnExpenseAdvanceRow row in rows)
            {
                // Prepare list of advance id for query
                advanceIdList.Add(row.AdvanceID);
            }

            if (!advanceIdList.Count.Equals(0))
            {
                IList<SCG.eAccounting.DTO.ValueObject.Advance> advDtoList = ScgeAccountingQueryProvider.FnRemittanceAdvanceQuery.FindRemittanceAdvanceAndItemsByAdvanceIDs(advanceIdList);
                IList<long> remittanceIdList = new List<long>();
                foreach (SCG.eAccounting.DTO.ValueObject.Advance advDto in advDtoList)
                {
                    remittanceIdList.Add(advDto.RemittanceID);
                }
                if (remittanceIdList.Count > 0)
                {
                    if (ScgeAccountingQueryProvider.FnRemittanceItemQuery.FindRemittanceItemByRemittanceIds(remittanceIdList, UserAccount.CurrentLanguageID).Count > 0)
                        IsEmptyRemittance = false;
                    else
                        IsEmptyRemittance = true;

                    ctlRemittanceGridview.DataSource = ScgeAccountingQueryProvider.FnRemittanceItemQuery.FindRemittanceItemByRemittanceIds(remittanceIdList, UserAccount.CurrentLanguageID);
                    ctlRemittanceGridview.DataBind();
                }
                else
                {
                    IsEmptyRemittance = true;

                    ctlRemittanceGridview.DataSource = null;
                    ctlRemittanceGridview.DataBind();
                }
            }
            else
            {
                IsEmptyRemittance = true;

                ctlRemittanceGridview.DataSource = null;
                ctlRemittanceGridview.DataBind();
            }
            ctlRemittanceGridview.DataBind();
            ctlUpdatePanelExpenseGeneral.Update();
        }

        protected void ctlTANoLookup_Click(object sender, ImageClickEventArgs e)
        {
            if (!ExpenseDocumentEditor.NotifyUpdateExpense()) return;
            ctlTALookup.Show();
        }
        protected void ctlDeleteTA_Click(object sender, ImageClickEventArgs e)
        {
            ctlTANoLabel.Text = string.Empty;
            this.AdvanceNoTemp = string.Empty;
            ctlAddAdvance.Enabled = true;
            ctlAdvanceGridView.Columns[5].Visible = true;
            ResetTADataZone();
            this.DeleteTAFromExpense();
            this.ClearAdvanceGridview();
            //this.ClearRemittanceGridview();
            ExpenseDocumentEditor.NotifyPaymentDetailChange();
            ctlUpdatePanelExpenseGeneral.Update();
        }
        protected void TA_OnObjectLookUpCalling(object sender, ObjectLookUpCallingEventArgs e)
        {
            UserControls.LOV.SCG.DB.TALookup taLookUp = sender as UserControls.LOV.SCG.DB.TALookup;
            string expenseType = FnExpenseDocumentService.GetExpenseType(this.ExpDocumentID, this.TransactionId);
            if (expenseType.Equals(ZoneType.Domestic))
            {
                taLookUp.TravelBy = TravellBy.Domestic;
            }
            else
            {
                taLookUp.TravelBy = TravellBy.Foreign;
            }
            taLookUp.RequesterID = this.RequesterID.ToString();
            taLookUp.CompanyID = this.CompanyID.ToString();
        }
        protected void TA_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            ctlAddAdvance.Enabled = false;
            TADocumentObj TA = (TADocumentObj)e.ObjectReturn;
            ctlAdvanceGridView.Columns[5].Visible = false;

            // For query only advance that
            // 1. Status = OutStanding
            // 2. Do not used in Expense that has flag <> 'Cancel'
            SCG.eAccounting.DTO.ValueObject.Advance avCriteria = new SCG.eAccounting.DTO.ValueObject.Advance();
            avCriteria.CompanyID = CompanyID;
            avCriteria.RequesterID = RequesterID;
            avCriteria.AdvanceType = FnExpenseDocumentService.GetExpenseType(this.ExpDocumentID, this.TransactionId);
            avCriteria.TADocumentID = TA.TADocumentID;

            FnExpenseDocumentService.SetTA(this.TransactionId, this.ExpDocumentID, avCriteria);

            if (TA.TADocumentID.HasValue)
            {
                ctlTANoLabel.Visible = false;
                ctlTALinkButton.Text = TA.DocumentNo;
                ctlTALinkButton.OnClientClick = "window.open('../Programs/DocumentView.aspx?wfid=" + TA.WorkflowID.ToString() + "')";
                ctlBusinessChk.Checked = TA.IsBusinessPurpose == null ? false : bool.Parse(TA.IsBusinessPurpose.ToString());
                ctlTrainingChk.Checked = TA.IsTrainningPurpose == null ? false : bool.Parse(TA.IsTrainningPurpose.ToString());
                ctlOtherChk.Checked = TA.IsOtherPurpose == null ? false : bool.Parse(TA.IsOtherPurpose.ToString());
                ctlOther.Text = TA.OtherPurposeDescription.ToString();
                ctlFromDateCal.DateValue = UIHelper.ToDateString(TA.FromDate);
                ctlToDateCal.DateValue = UIHelper.ToDateString(TA.ToDate);
                ctlCountry.Text = TA.Country;
            }
            else
            {
                ResetTADataZone();
            }
            this.BindAdvanceGridView();
            this.BindRemittanceGridview();
            ExpenseDocumentEditor.NotifyPaymentDetailChange();
            ctlUpdatePanelExpenseGeneral.Update();
        }
        protected void ResetTADataZone()
        {
            ctlTANoLabel.Visible = true;
            ctlTANoLabel.Text = "N/A";
            ctlTALinkButton.Text = string.Empty;
            ctlTALinkButton.OnClientClick = string.Empty;
            ctlBusinessChk.Checked = false;
            ctlTrainingChk.Checked = false;
            ctlOtherChk.Checked = false;
            ctlOther.Text = string.Empty;
            ctlFromDateCal.DateValue = string.Empty;
            ctlToDateCal.DateValue = string.Empty;
            ctlCountry.Text = string.Empty;
        }
        protected void ctlAddAdvance_Click(object sender, EventArgs e)
        {
            if (!ExpenseDocumentEditor.NotifyUpdateExpense()) return;
            ctlAdvanceLookup.Show();
        }
        protected void Advance_OnObjectLookUpCalling(object sender, ObjectLookUpCallingEventArgs e)
        {
            UserControls.LOV.AV.AdvanceLookup advanceLookUp = sender as UserControls.LOV.AV.AdvanceLookup;
            ExpenseDataSet expenseDS = (ExpenseDataSet)TransactionService.GetDS(this.TransactionId);
            ExpenseDataSet.FnExpenseDocumentRow row = expenseDS.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);

            if (!row.IsMainCurrencyIDNull())
            {
                this.MainCurrencyID = row.MainCurrencyID;
            }

            if (!row.IsPBIDNull())
            {
                this.PBID = row.PBID;
            }

            this.IsRepOffice = row.IsIsRepOfficeNull() ? false : row.IsRepOffice;

            advanceLookUp.CompanyID = CompanyID; // UIHelper.ParseLong(((LOV.SCG.DB.CompanyField)this.Parent.Parent.Parent.Parent.FindControl("ctlCompanyField")).CompanyID);
            advanceLookUp.RequesterID = RequesterID; // UIHelper.ParseLong(((ActorData)this.Parent.Parent.Parent.Parent.FindControl("ctlRequesterData")).UserID);
            advanceLookUp.AdvanceType = FnExpenseDocumentService.GetExpenseType(this.ExpDocumentID, this.TransactionId);
            advanceLookUp.IsRelateWithRemittanceButNotInExpense = true;
            advanceLookUp.PBID = this.PBID;
            advanceLookUp.MainCurrencyID = this.MainCurrencyID;
            advanceLookUp.IsRepOffice = this.IsRepOffice;
            if (!string.IsNullOrEmpty(Request.Params["wfid"]) && UIHelper.ParseInt(Request.Params["wfid"]) > 0)
            {
                long workFlowID = UIHelper.ParseLong(Request.Params["wfid"]);

                SS.Standard.WorkFlow.DTO.WorkFlow wf = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
                if (wf != null && wf.CurrentState != null && wf.CurrentState.Name != null && wf.CurrentState.Name.Equals(WorkFlowStateFlag.Draft))
                {
                    advanceLookUp.CurrentUserID = UserAccount.UserID;
                }
            }
            else
            {
                advanceLookUp.CurrentUserID = UserAccount.UserID;
            }
        }
        protected void Advance_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            ctlTANoLookup.Enabled = false;
            ctlDeleteTA.Enabled = false;
            if (e.ObjectReturn != null)
            {
                IList<SCG.eAccounting.DTO.ValueObject.Advance> advanceList = (IList<SCG.eAccounting.DTO.ValueObject.Advance>)e.ObjectReturn;
                this.PrepareAdvance(advanceList);
                this.BindAdvanceGridView();
                this.BindRemittanceGridview();

                if (!ExpenseDocumentEditor.NotifyUpdateExpense()) return;

                ExpenseDocumentEditor.NotifyPaymentDetailChange();
            }
            ctlUpdatePanelExpenseGeneral.Update();
        }
        #endregion

        public void ClearAdvanceGridview()
        {
            ExpenseDataSet expenseDS = (ExpenseDataSet)TransactionService.GetDS(this.TransactionId);
            ExpenseDataSet.FnExpenseDocumentRow expRow = expenseDS.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);
            ExpenseDataSet.FnExpenseAdvanceRow[] rows = (ExpenseDataSet.FnExpenseAdvanceRow[])expenseDS.FnExpenseAdvance.Select();

            foreach (ExpenseDataSet.FnExpenseAdvanceRow row in rows)
            {
                double totalAmount = FnExpenseDocumentService.DeleteExpenseAdvanceFromTransaction(this.TransactionId, row.AdvanceID, (double)expRow.TotalAdvance);
                FnExpenseDocumentService.SetTotalAdvance(this.TransactionId, 0, totalAmount);
                FnExpenseDocumentService.RefreshRemittance(this.TransactionId, this.ExpDocumentID);
                row.Delete();
            }

            this.BindAdvanceGridView();
        }
        //public void ClearRemittanceGridview()
        //{
        //    ExpenseDataSet expenseDS = (ExpenseDataSet)TransactionService.GetDS(this.TransactionId);
            
        //   if (expenseDS != null)
        //   {
        //        ExpenseDataSet.FnExpenseRemittanceRow[] rows = (ExpenseDataSet.FnExpenseRemittanceRow[])expenseDS.FnExpenseRemittance.Select();
        //        foreach (ExpenseDataSet.FnExpenseRemittanceRow row in rows)
        //        {
        //            row.Delete();
        //        }
        //        this.BindRemittanceGridview();
        //   }
        //}
        public void DeleteTAFromExpense()
        {
            ExpenseDataSet expenseDS = (ExpenseDataSet)TransactionService.GetDS(this.TransactionId);
            if (expenseDS.FnExpenseDocument.Rows.Count > 0)
            {
                expenseDS.FnExpenseDocument[0].SetTADocumentIDNull();
            }
        }
        public void PrepareAdvance(IList<SCG.eAccounting.DTO.ValueObject.Advance> advanceList)
        {
            foreach (SCG.eAccounting.DTO.ValueObject.Advance advance in advanceList)
            {
                AvAdvanceDocument advanceDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindProxyByIdentity(advance.AdvanceID);
                if (advanceDocument.TADocumentID != null && advanceDocument.TADocumentID != 0)
                {
                    // For query only advance that
                    // 1. Status = OutStanding
                    // 2. Do not used in Expense that has flag <> 'Cancel'
                    SCG.eAccounting.DTO.ValueObject.Advance avCriteria = new SCG.eAccounting.DTO.ValueObject.Advance();
                    avCriteria.CompanyID = CompanyID;
                    avCriteria.RequesterID = RequesterID;
                    avCriteria.AdvanceType = FnExpenseDocumentService.GetExpenseType(this.ExpDocumentID, this.TransactionId);
                    advance.ExpenseType = avCriteria.AdvanceType;
                    avCriteria.TADocumentID = advanceDocument.TADocumentID;

                    //Delete all advance and remittance in ExpenseAdvance Datatable. 
                    //กรณีที่มีรายการ advance ที่อ้างอิง TA อยู่ใน advanceList ที่เลือกมา
                    this.ClearAdvanceGridview();
                    //this.ClearRemittanceGridview();

                    FnExpenseDocumentService.SetTA(this.TransactionId, this.ExpDocumentID, avCriteria);

                    TADocument ta = ScgeAccountingQueryProvider.TADocumentQuery.FindByIdentity(advanceDocument.TADocumentID.Value);
                    SS.Standard.WorkFlow.DTO.WorkFlow wf = SS.Standard.WorkFlow.Query.WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(ta.DocumentID.DocumentID);

                    ctlTANoLabel.Visible = false;
                    ctlTALinkButton.Text = ta.DocumentID.DocumentNo;
                    ctlTALinkButton.OnClientClick = "window.open('../Programs/DocumentView.aspx?wfid=" + wf.WorkFlowID.ToString() + "')";
                    ctlBusinessChk.Checked = ta.IsBusinessPurpose;
                    ctlTrainingChk.Checked = ta.IsTrainningPurpose;
                    ctlOtherChk.Checked = ta.IsOtherPurpose;
                    ctlOther.Text = ta.OtherPurposeDescription;
                    ctlCountry.Text = ta.Country;
                    ctlFromDateCal.Value = ta.FromDate;
                    ctlToDateCal.Value = ta.ToDate;
                    ctlAddAdvance.Enabled = false;

                    ctlAdvanceGridView.Columns[7].Visible = false;
                    ctlTANoLookup.Enabled = true;
                    ctlDeleteTA.Enabled = true;

                    break;
                }
                else
                {
                    double totalAdvance = FnExpenseDocumentService.AddExpenseAdvanceToTransaction(this.TransactionId, this.ExpDocumentID, advanceList, 0);
                    FnExpenseDocumentService.SetTotalAdvance(this.TransactionId, 0, totalAdvance);
                    FnExpenseDocumentService.RefreshRemittance(this.TransactionId, this.ExpDocumentID);
                    //break;
                }
            }
        }
        public void Save()
        {
            FnExpenseDocument expenseDocument = new FnExpenseDocument(this.ExpDocumentID);
            if (ctlBusinessChk.Checked)
            {
                expenseDocument.IsBusinessPurpose = true;
            }
            if (ctlTrainingChk.Checked)
            {
                expenseDocument.IsTrainningPurpose = true;
            }
            if (ctlOtherChk.Checked)
            {
                expenseDocument.IsOtherPurpose = true;
                expenseDocument.OtherPurposeDescription = ctlOther.Text;
            }
            if (!string.IsNullOrEmpty(ctlFromDateCal.DateValue))
            {
                expenseDocument.FromDate = UIHelper.ParseDate(ctlFromDateCal.DateValue);
            }
            if (!string.IsNullOrEmpty(ctlToDateCal.DateValue))
            {
                expenseDocument.ToDate = UIHelper.ParseDate(ctlToDateCal.DateValue);
            }
            expenseDocument.Country = ctlCountry.Text;
            //expenseDocument.PersonalLevel = ctlPersonLevel.Text;
            //expenseDocument.ExchangeRateForUSDAdvance = UIHelper.ParseDouble(ctlExchangeRateForPerdiem.Text);
            //expenseDocument.ExchangeRateForUSD = UIHelper.ParseDouble(ctlExchangeRateDeparture.Text);

            FnExpenseDocumentService.UpdateExpenseDocumentAdvanceToTransaction(expenseDocument, this.TransactionId);
            BindControl(true);
        }

        public void BindControl(bool refreshHeader)
        {
            this.refreshHeaderGrid = refreshHeader;
            if (InitialFlag.Equals(FlagEnum.ViewFlag))
            {
                ctlAddAdvance.Enabled = false;
                ctlTANoLookup.Visible = false;
                ctlDeleteTA.Visible = false;
            }
            else
            {
                ctlAddAdvance.Enabled = true;
                ctlTANoLookup.Visible = true;
                ctlDeleteTA.Visible = true;
            }

            if (this.DocumentType.Equals(ZoneType.Domestic))
            {
                this.IsDomestic = true;
                ctlRemittanceGridview.Visible = false;
                ctlTALookup.TravelBy = TravellBy.Domestic;
                ctlControlPanel.Visible = false;
            }
            else
            {
                this.IsDomestic = false;
                ctlRemittanceGridview.Visible = true;
                ctlTALookup.TravelBy = TravellBy.Foreign;
                ctlControlPanel.Visible = true;
            }

            ExpenseDataSet expenseDS = (ExpenseDataSet)TransactionService.GetDS(this.TransactionId);
            ExpenseDataSet.FnExpenseDocumentRow row = expenseDS.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);
            if (!row.IsTADocumentIDNull())
            {
                ctlTANoLookup.Enabled = false;
                TADocument ta = ScgeAccountingQueryProvider.TADocumentQuery.FindByIdentity(row.TADocumentID);
                SS.Standard.WorkFlow.DTO.WorkFlow wf = SS.Standard.WorkFlow.Query.WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(ta.DocumentID.DocumentID);
                ctlAddAdvance.Enabled = false;
                ctlTANoLabel.Visible = false;
                ctlTALinkButton.Text = ta.DocumentID.DocumentNo;
                ctlTALinkButton.OnClientClick = "window.open('../Programs/DocumentView.aspx?wfid=" + wf.WorkFlowID.ToString() + "')";
                ctlBusinessChk.Checked = !(row.IsIsBusinessPurposeNull() || row.IsBusinessPurpose == false);
                ctlTrainingChk.Checked = !(row.IsIsTrainningPurposeNull() || row.IsTrainningPurpose == false);
                ctlOtherChk.Checked = !(row.IsIsOtherPurposeNull() || row.IsOtherPurpose == false);
                ctlOther.Text = row.IsOtherPurposeDescriptionNull() ? String.Empty : row.OtherPurposeDescription;
                ctlCountry.Text = row.IsCountryNull() ? String.Empty : row.Country;

                if (!row.IsFromDateNull())
                    ctlFromDateCal.Value = row.FromDate;
                if (!row.IsToDateNull())
                    ctlToDateCal.Value = row.ToDate;

                ctlPersonLevel.Text = row.IsPersonalLevelNull() ? String.Empty : row.PersonalLevel;
                ctlExchangeRateForPerdiem.Text = row.IsExchangeRateForUSDAdvanceNull() ? String.Empty : row.ExchangeRateForUSDAdvance.ToString();
            }
            else
            {
                ResetTADataZone();
                if (expenseDS.FnExpenseAdvance.Rows.Count > 0)
                {
                    ctlTANoLookup.Enabled = false;
                    ctlDeleteTA.Enabled = false;
                }
            }

            BindAdvanceGridView();
            BindRemittanceGridview();
            ctlUpdatePanelExpenseGeneral.Update();
        }

    }
}
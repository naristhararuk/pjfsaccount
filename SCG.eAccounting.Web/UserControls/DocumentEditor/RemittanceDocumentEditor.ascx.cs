using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.DTO;
using SCG.eAccounting.Query;
using SCG.eAccounting.Web.Helper;
using SCG.DB.DTO;
using System.Data;
using SCG.eAccounting.BLL;
using SS.SU.Query;
using SS.Standard.WorkFlow.Query;
using SS.Standard.Utilities;
using SCG.eAccounting.DTO.DataSet;
using SCG.DB.Query;
using SS.SU.DTO;
using SS.Standard.WorkFlow.Service.Implement;
using SS.DB.Query;
using SS.Standard.WorkFlow.Service;
using SCG.eAccounting.BLL.Implement;
using SCG.eAccounting.SAP.BAPI.Service.Const;
using SS.DB.DTO;
using SCG.DB.DTO.ValueObject;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor
{
    public partial class RemittanceDocumentEditor : BaseUserControl, IDocumentEditor
    {
        #region Properties
        public ITransactionService TransactionService { get; set; }
        public IFnRemittanceService FnRemittanceService { get; set; }
        public IFnRemittanceItemService FnRemittanceItemService { get; set; }
        public IFnRemittanceAdvanceService FnRemittanceAdvanceService { get; set; }
        public ISCGDocumentService SCGDocumentService { get; set; }
        public IWorkFlowService WorkFlowService { get; set; }
        public IAvAdvanceDocumentService AvAdvanceDocumentService { get; set; }
        public DbCurrency dbCurrency { get; set; }

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
        public double SumRemittanceItemAmount
        {
            get { return UIHelper.ParseDouble((ViewState["SumRemittanceItemAmount"] ?? 0.00).ToString()); }
            set { ViewState["SumRemittanceItemAmount"] = value; }
        }
        public double SumAdvanceAmount
        {
            get { return UIHelper.ParseDouble((ViewState["SumAdvanceAmount"] ?? 0.00).ToString()); }
            set { ViewState["SumAdvanceAmount"] = value; }
        }
        public IList<SCG.eAccounting.DTO.ValueObject.Advance> AdvanceList
        {
            get { return (IList<SCG.eAccounting.DTO.ValueObject.Advance>)ViewState["AdvanceList"]; }
            set { ViewState["AdvanceList"] = value; }
        }
        public string InitialFlag
        {
            get { return ViewState[ViewStateName.InitialFlag].ToString(); }
            set { ViewState[ViewStateName.InitialFlag] = value; }
        }
        public string TADocumentID
        {
            get { return ctlTempTADocumentID.Text; }
            set { ctlTempTADocumentID.Text = value; }
        }
        public long DocumentIDofTA
        {
            get
            {
                if (ViewState["DocumentIDofTA"] != null)
                    return (long)(ViewState["DocumentIDofTA"]);
                else
                    return 0;
            }
            set
            {
                ViewState["DocumentIDofTA"] = value;
            }
        }
        public IList<object> VisibleFields
        {
            get { return (IList<object>)ViewState[ViewStateName.VisibleFields]; }
            set { ViewState[ViewStateName.VisibleFields] = value; }
        }
        public IList<object> EditableFields
        {
            get { return (IList<object>)ViewState[ViewStateName.EditableFields]; }
            set { ViewState[ViewStateName.EditableFields] = value; }
        }
        public bool isShowFooter
        {
            get
            {
                if (ViewState["isShowFooter"] == null)
                {
                    return true;
                }
                else
                {
                    return (bool)(ViewState["isShowFooter"]);
                }
            }
            set { ViewState["isShowFooter"] = value; }
        }
        public bool isNotViewPost
        {
            get
            {
                if (ViewState["isNotViewPost"] == null)
                {
                    return true;
                }
                else
                {
                    return (bool)(ViewState["isNotViewPost"]);
                }
            }
            set { ViewState["isNotViewPost"] = value; }
        }

        public long? PBID
        {
            get
            {
                if (ViewState["PBID"] != null)
                    return (long)(ViewState["PBID"]);
                else
                    return 0;
            }
            set
            {
                ViewState["PBID"] = value;
            }
        }

        public short? MainCurrencyID
        {
            get
            {
                if (ViewState["MainCurrencyID"] != null)
                    return (short)(ViewState["MainCurrencyID"]);
                else
                    return 0;
            }
            set
            {
                ViewState["MainCurrencyID"] = value;
            }
        }

        public double SumDivAmount
        {
            get { return UIHelper.ParseDouble((ViewState["SumDivAmount"] ?? 0.00).ToString()); }
            set { ViewState["SumDivAmount"] = value; }
        }
        public long TempCompanyID
        {
            get { return ViewState["TempCompanyID"] == null ? 0 : (long)ViewState["TempCompanyID"]; }
            set { ViewState["TempCompanyID"] = value; }
        }
        public long CountRow
        {
            get { return ViewState["CountRow"] == null ? 0 : (long)ViewState["CountRow"]; }
            set { ViewState["CountRow"] = value; }
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

        public string CurrencySymbol
        {
            get { return ViewState["CurrencySymbol"].ToString(); }
            set { ViewState["CurrencySymbol"] = value; }
        }

        public double SumExchangeRateMaintoTHB
        {
            get { return UIHelper.ParseDouble((ViewState["SumExchangeRateMaintoTHB"] ?? 0.00).ToString()); }
            set { ViewState["SumExchangeRateMaintoTHB"] = value; }
        }
        public void ShowDefault()
        {
            ctlCounterCashierDropdown.SelectedIndex = -1;
        }
        public string SelectedValue
        {
            set { ctlCounterCashierDropdown.SelectedValue = value; }
        }
        public long TempRequesterID
        {
            get { return ViewState["TempRequesterID"] == null ? 0 : (long)ViewState["TempRequesterID"]; }
            set { ViewState["TempRequesterID"] = value; }
        }
        #endregion

        #region Load Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //                string script = @"<script type='text/javascript'>
                //                    window.onunload = UnLock;
                //                    UnLockParameters['txID']='" + this.TransactionID + "';</script>";
                //                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "addunlockparameter", script, false);

                this.SumAdvanceAmount = 0;
                this.SumRemittanceItemAmount = 0;
                this.SumDivAmount = 0;
                this.SumExchangeRateMaintoTHB = 0.00;
                this.CountRow = 0;

                ctlRemittanceItemGrid.DataBind();
                ctlAdvanceGridView.DataBind();
                if (Request.Params["wfid"] == null)
                {
                    ctlTANo.Text = "N/A";
                    //ctlRemittanceFormHeader.DocumentID = this.DocumentID;
                }

                if (!VisibleFields.Contains(RemittanceFieldGroup.VerifyDetail))
                {
                    ctlDivViewDetailForeign.Visible = false;
                }
            }

            ctlTALookup.OnObjectLookUpCalling += new BaseUserControl.ObjectLookUpCallingEventHandler(TA_OnObjectLookUpCalling);
            ctlTALookup.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(TA_OnObjectLookUpReturn);
            ctlAdvanceLookup.OnObjectLookUpCalling += new BaseUserControl.ObjectLookUpCallingEventHandler(Advance_OnObjectLookUpCalling);
            ctlAdvanceLookup.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(Advance_OnObjectLookUpReturn);
            ctlCompaymyField.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(CompaymyField_OnObjectLookUpReturn);
            ctlRequesterData.OnObjectLookUpReturn += new ObjectLookUpReturnEventHandler(ctlRequesterData_OnObjectLookUpReturn);
        }
        #endregion

        #region Manage Data
        public void Initialize(string initFlag, long? documentID)
        {
            long remittanceDocumentID = 0;
            this.SumAdvanceAmount = 0;
            this.CountRow = 0;
            this.SumExchangeRateMaintoTHB = 0.00;
            this.SumRemittanceItemAmount = 0;
            FnRemittanceDataset remittanceDataset;
            Guid txID = Guid.Empty;
            bool isCopy = false;
            this.VisibleFields = FnRemittanceService.GetVisibleFields(documentID);
            this.EditableFields = FnRemittanceService.GetEditableFields(documentID);
            this.isShowFooter = true;
            ctlViewPostForeign.Visible = true;

            if (initFlag.Equals(FlagEnum.NewFlag) && Request.QueryString["cp"] == null)
            {
                remittanceDataset = (FnRemittanceDataset)FnRemittanceService.PrepareDS();
                txID = TransactionService.Begin(remittanceDataset);
                remittanceDocumentID = FnRemittanceService.AddFnRemittanceDocumentTransaction(txID);
                this.isVisibleControlViewMode(true);

                ctlRemittanceFormHeader.Status = FlagEnum.NewFlag;
            }
            else if (((initFlag.Equals(FlagEnum.EditFlag) || (initFlag.Equals(FlagEnum.ViewFlag)))) && ((documentID.HasValue) && (documentID.Value != 0)))
            {
                remittanceDataset = (FnRemittanceDataset)FnRemittanceService.PrepareDS(documentID.Value);
                txID = TransactionService.Begin(remittanceDataset);
                if (remittanceDataset.FnRemittance.Rows.Count > 0)
                {
                    remittanceDocumentID = UIHelper.ParseLong(remittanceDataset.FnRemittance.Rows[0]["RemittanceID"].ToString());
                }
                if (initFlag.Equals(FlagEnum.ViewFlag))
                {
                    this.isShowFooter = false;
                    this.isVisibleControlViewMode(false);
                }
                if (initFlag.Equals(FlagEnum.EditFlag))
                {
                    //check edit field if cannot edit advance,TA,remittanceItem then hide button
                    this.isVisibleControlViewMode(EditableFields.Contains(RemittanceFieldGroup.All));
                    this.isShowFooter = EditableFields.Contains(RemittanceFieldGroup.All);
                    ctlViewPostForeign.Visible = false;
                }
            }
            else if (Request.QueryString["cp"] != null && UIHelper.ParseLong(Request.QueryString["cp"]) == 1)
            {
                remittanceDataset = (FnRemittanceDataset)FnRemittanceService.PrepareDataToDataset(UIHelper.ParseLong(Request.QueryString["docId"]), true);
                txID = TransactionService.Begin(remittanceDataset);
                remittanceDataset.FnRemittanceAdvance.Clear();
                remittanceDataset.FnRemittanceItem.Clear();
                isCopy = true;
                if (remittanceDataset.FnRemittance.Rows.Count > 0)
                {
                    remittanceDocumentID = UIHelper.ParseLong(remittanceDataset.FnRemittance.Rows[0]["RemittanceID"].ToString());
                    //DocumentType = remittanceDataset.FnExpenseDocument.Rows[0]["ExpenseType"].ToString();
                }
            }
            this.ctlUpdatePanelGeneral.Update();
            this.ctlUpdatePanelViewPost.Update();
            this.TransactionID = txID;
            this.DocumentID = remittanceDocumentID;
            this.InitialFlag = initFlag;

            this.InitializeControl();
            this.BindControl(isCopy);

            #region แสดง Tab
            // Edit By Kla 
            // 15/06/2009
            // For Log#760
            // ให้ Tab ที่มีข้อมูลโชว์ขึ้นมา ส่วน Tab ที่ไม่มีข้อมูล ไม่ต้องโชว์
            string strCurrentState = "";
            if (!string.IsNullOrEmpty(Request.Params["wfid"]) && UIHelper.ParseInt(Request.Params["wfid"]) > 0)
            {
                long workFlowID = UIHelper.ParseLong(Request.Params["wfid"]);

                SS.Standard.WorkFlow.DTO.WorkFlow wf = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
                if (wf != null && wf.CurrentState != null && wf.CurrentState.Name != null)
                    strCurrentState = wf.CurrentState.Name;
            }

            if (strCurrentState == "Draft")
            {
                ctlTabMemo.Visible = true;
                ctlTabAttachment.Visible = true;
                ctlTabHistory.Visible = true;

                UpdatePanelRemittanceTab.Update();
            }
            else if (this.InitialFlag.Equals(FlagEnum.ViewFlag))
            {
                if (string.IsNullOrEmpty(ctlMemo.Text))
                    ctlTabMemo.Visible = false;
                else
                    ctlTabMemo.Visible = true;

                if (ctlAttachment.IsEmptyData)
                    ctlTabAttachment.Visible = false;
                else
                    ctlTabAttachment.Visible = true;

                if (ctlHistory.IsEmptyData)
                    ctlTabHistory.Visible = false;
                else
                    ctlTabHistory.Visible = true;

                UpdatePanelRemittanceTab.Update();
            }
            else
            {
                ctlTabMemo.Visible = true;
                ctlTabAttachment.Visible = true;
                ctlTabHistory.Visible = true;

                UpdatePanelRemittanceTab.Update();
            }
            #endregion แสดง Tab
        }
        public long Save()
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            #region Get Document & FnRemittance
            long tempfnRemittanceDocumentID = this.DocumentID;
            long tempDocumentID = 0;
            FnRemittanceDataset fnRemittanceDocumentDS = (FnRemittanceDataset)TransactionService.GetDS(this.TransactionID);
            FnRemittanceDataset.FnRemittanceRow row = fnRemittanceDocumentDS.FnRemittance.FindByRemittanceID(tempfnRemittanceDocumentID);
            tempDocumentID = row.DocumentID;

            SCGDocument document = new SCGDocument(tempDocumentID);
            FnRemittance fnRemittance = new FnRemittance(tempfnRemittanceDocumentID);
            #endregion Get Document & FnRemittance

            #region Update Document & FnRemittance
            if (!string.IsNullOrEmpty(ctlCompaymyField.CompanyID))
            {
                document.CompanyID = new DbCompany(UIHelper.ParseLong(ctlCompaymyField.CompanyID));
            }
            if (!string.IsNullOrEmpty(ctlCreatorData.UserID))
            {
                document.CreatorID = new SuUser(UIHelper.ParseLong(ctlCreatorData.UserID));
            }
            if (!string.IsNullOrEmpty(ctlRequesterData.UserID))
            {
                document.RequesterID = new SuUser(UIHelper.ParseLong(ctlRequesterData.UserID));
            }
            document.DocumentNo = string.Empty;
            document.DocumentType = new SS.Standard.WorkFlow.DTO.DocumentType(DocumentTypeID.RemittanceDocument);
            document.Subject = ctlSubject.Text;
            document.Memo = ctlMemo.Text;
            document.Active = true;
            if (!string.IsNullOrEmpty(ctlBranchForeign.Text.Trim()))
            {
                document.BranchCode = ctlBranchForeign.Text;
            }
            else
            {
                document.BranchCode = "0001";
            }
            document.BusinessArea = ctlBusinessAreaForeign.Text;
            try
            {
                if (!string.IsNullOrEmpty(ctlPostingDateForeign.DateValue))
                {
                    document.PostingDate = UIHelper.ParseDate(ctlPostingDateForeign.DateValue);
                }
                else
                {
                    document.PostingDate = DateTime.Today;
                }
                if (!string.IsNullOrEmpty(ctlReceiveDateForeign.Text))
                {
                    document.BaseLineDate = UIHelper.ParseDate(ctlReceiveDateForeign.Text);
                }
                else
                {
                    document.BaseLineDate = DateTime.Today;
                }
            }
            catch (FormatException fex)
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Invalid_Date_Format"));
                throw new ServiceValidationException(errors);
            }
            if (GetCashierId() != -1)
            {
                fnRemittance.PB = new Dbpb(GetCashierId().Value);
            }
            else
            {
                fnRemittance.PB = null;
            }
            if (MainCurrencyID != 0)
            {
                fnRemittance.MainCurrencyID = Convert.ToInt16(this.MainCurrencyID);
            }

            fnRemittance.IsRepOffice = this.IsRepOffice;
            if (this.AdvanceList != null)
            {
                if (string.IsNullOrEmpty(this.TADocumentID) && this.AdvanceList.Count == 0 || this.TADocumentID.Equals("0") && this.AdvanceList.Count == 0)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("TAOrAddAdvanceAreRequired"));
                }
            }
            else
            {
                if (string.IsNullOrEmpty(this.TADocumentID) || this.TADocumentID.Equals("0"))
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("TAOrAddAdvanceAreRequired"));
                }
            }

            try
            {
                // Save Move to RemittanceService (Prepare DTO Header & Footer)
                FnRemittanceService.UpdateHeaderAndFooterToTransaction(this.TransactionID, document, fnRemittance);
            }
            catch (ServiceValidationException ex)
            {
                errors.MergeErrors(ex.ValidationErrors);
            }
            #endregion Update Document & FnRemittance

            try
            {
                FnRemittanceService.ValidateRemittanceAdvance(this.TransactionID, this.DocumentID);
            }
            catch (ServiceValidationException ex)
            {
                errors.MergeErrors(ex.ValidationErrors);
            }

            #region RemittanceItem
            SaveRemittanceItem();
            #endregion RemittanceItem

            #region Remittance  // Update fnRemittance
            fnRemittance.Document = document;
            fnRemittance.Active = true;
            fnRemittance.IsFullClearing = ctlFullReturnCashChk.Checked;


            if (!string.IsNullOrEmpty(this.TADocumentID))
                fnRemittance.TADocumentID = UIHelper.ParseLong(this.TADocumentID);// Value TADocumentID from TaLookup

            try
            {
                // Get remittance document information and save to transaction.
                FnRemittanceService.UpdateRemittanceDocumentTransaction(this.TransactionID, fnRemittance, false);
            }
            catch (ServiceValidationException ex)
            {
                errors.MergeErrors(ex.ValidationErrors);
            }
            #endregion Remittance

            if (!errors.IsEmpty)
            {
                // If have some validation error then return error.
                ctlValidationSummary.Focus();
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Key", "javascript:window.scrollTo(0,)", true);
                throw new ServiceValidationException(errors);
            }

            ctlUpdatePanelValidate.Update();
            return this.SaveToDatabase();
        }

        public void SaveRemittanceItem()
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            List<FnRemittanceItem> list = new List<FnRemittanceItem>();
            this.SumDivAmount = 0;
            foreach (GridViewRow gridviewRow in ctlRemittanceItemGrid.Rows)
            {
                if (gridviewRow.RowType == DataControlRowType.DataRow)
                {
                    UserControls.DropdownList.SCG.DB.StatusDropdown ctlPaymentTypeDropdown = (UserControls.DropdownList.SCG.DB.StatusDropdown)ctlRemittanceItemGrid.Rows[gridviewRow.RowIndex].FindControl("ctlPaymentTypeDropdown");
                    UserControls.Dropdownlist.SS.DB.CurrencyDropdown ctlCurrencyDropdown = (UserControls.Dropdownlist.SS.DB.CurrencyDropdown)ctlRemittanceItemGrid.Rows[gridviewRow.RowIndex].FindControl("ctlCurrencyDropdown");
                    Literal ctlFCurrencyAdv = (Literal)ctlRemittanceItemGrid.Rows[gridviewRow.RowIndex].FindControl("ctlFCurrencyAdv");
                    Literal ctlExchangeRate = (Literal)ctlRemittanceItemGrid.Rows[gridviewRow.RowIndex].FindControl("ctlExchangRateLabel");
                    TextBox ctlFCurrencyRem = (TextBox)ctlRemittanceItemGrid.Rows[gridviewRow.RowIndex].FindControl("ctlFCurrencyRem");
                    Literal ctlAmountTHB = (Literal)ctlRemittanceItemGrid.Rows[gridviewRow.RowIndex].FindControl("ctlAmountTHB");
                    Literal ctlMainCurrencyAmount = (Literal)ctlRemittanceItemGrid.Rows[gridviewRow.RowIndex].FindControl("ctlMainCurrencyAmount");
                    HiddenField ctlAmountTHBAdvance = (HiddenField)ctlRemittanceItemGrid.Rows[gridviewRow.RowIndex].FindControl("ctlAmountTHBAdvance");
                    HiddenField ctlAmountMainCurrencyAdvance = (HiddenField)ctlRemittanceItemGrid.Rows[gridviewRow.RowIndex].FindControl("ctlAmountMainCurrencyAdvance");
                    HiddenField ctlExchangeRateTHB = (HiddenField)ctlRemittanceItemGrid.Rows[gridviewRow.RowIndex].FindControl("ctlExchangeRateTHB");
                    long remittanceItemID = UIHelper.ParseLong(ctlRemittanceItemGrid.DataKeys[gridviewRow.RowIndex].Value.ToString());

                    FnRemittanceItem remittanceItem = new FnRemittanceItem(remittanceItemID);
                    remittanceItem.Remittance = new FnRemittance(this.DocumentID);
                    remittanceItem.PaymentType = ctlPaymentTypeDropdown.SelectedValue;
                    remittanceItem.Currency = new SS.DB.DTO.DbCurrency(UIHelper.ParseShort(ctlCurrencyDropdown.SelectedValue));
                    remittanceItem.ForeignCurrencyAdvanced = UIHelper.ParseDouble(ctlFCurrencyAdv.Text);
                    remittanceItem.ExchangeRate = UIHelper.ParseDouble(ctlExchangeRate.Text);
                    remittanceItem.ForeignCurrencyRemitted = UIHelper.ParseDouble(ctlFCurrencyRem.Text);
                    remittanceItem.ForeignAmountTHBAdvanced = UIHelper.ParseDouble(ctlAmountTHBAdvance.Value);
                    remittanceItem.ExchangeRateTHB = UIHelper.ParseDouble(ctlExchangeRateToTHBHField.Value);
                    if (!IsRepOffice)
                    {
                        remittanceItem.AmountTHB = (double)Math.Round((decimal)(UIHelper.ParseDouble(ctlExchangeRate.Text) * UIHelper.ParseDouble(ctlFCurrencyRem.Text)), 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        remittanceItem.MainCurrencyAmount = (double)Math.Round((decimal)(UIHelper.ParseDouble(ctlExchangeRate.Text) * UIHelper.ParseDouble(ctlFCurrencyRem.Text)), 2, MidpointRounding.AwayFromZero);
                        remittanceItem.AmountTHB = (double)Math.Round((decimal)((UIHelper.ParseDouble(ctlExchangeRate.Text) * UIHelper.ParseDouble(ctlFCurrencyRem.Text)) * remittanceItem.ExchangeRateTHB), 2, MidpointRounding.AwayFromZero);
                    }
                    remittanceItem.ForeignAmountMainCurrencyAdvanced = UIHelper.ParseDouble(ctlAmountMainCurrencyAdvance.Value);
                    remittanceItem.IsImportFromAdvance = !gridviewRow.FindControl("ctlDelete").Visible;
                    remittanceItem.Active = true;
                    list.Add(remittanceItem);
                }
            }

            try
            {
                FnRemittanceItemService.UpdateRemittanceItemList(this.TransactionID, this.DocumentID, list, ctlFullReturnCashChk.Checked);
            }
            catch (ServiceValidationException ex)
            {
                errors.MergeErrors(ex.ValidationErrors);
            }

            if (!errors.IsEmpty)
            {
                // If have some validation error then return error.
                ctlValidationSummary.Focus();
                throw new ServiceValidationException(errors);
            }

            ctlUpdatePanelValidate.Update();
        }

        public long SaveToDatabase()
        {
            // Save all table in dataset to database and clear transaction.
            // return document id of BudgetDocument.

            long fnRemittanceDocumentID = this.DocumentID;


            fnRemittanceDocumentID = FnRemittanceService.SaveRemittanceDocument(this.TransactionID, this.DocumentID);

            TransactionService.Commit(this.TransactionID);
            long workFlowID = 0;
            FnRemittance fnRemittance = ScgeAccountingQueryProvider.FnRemittanceQuery.FindProxyByIdentity(fnRemittanceDocumentID);
            // Save New WorkFlow.
            if ((fnRemittance != null) && (fnRemittance.Document != null))
            {
                SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.FindProxyByIdentity(fnRemittance.Document.DocumentID);
                SS.Standard.WorkFlow.DTO.WorkFlow workFlow = new SS.Standard.WorkFlow.DTO.WorkFlow();
                workFlow.WorkFlowType = new SS.Standard.WorkFlow.DTO.WorkFlowType(WorkFlowTypeID.RemittanceWorkFlow);
                workFlow.CurrentState = WorkFlowQueryProvider.WorkFlowStateQuery.FindWorkFlowStateIDByTypeIDAndStateName(WorkFlowTypeID.RemittanceWorkFlow, WorkFlowStateFlag.Draft);
                workFlow.Description = string.Empty;
                workFlow.Document = document;
                workFlow.Active = true;
                workFlow.CreBy = document.CreBy;
                workFlow.CreDate = document.CreDate;
                workFlow.UpdBy = document.UpdBy;
                workFlow.UpdDate = document.UpdDate;
                workFlow.UpdPgm = document.UpdPgm;

                workFlowID = WorkFlowService.CheckExistAndAddNew(workFlow);
            }

            return workFlowID;
        }
        public void RollBackTransaction()
        {
            Guid txID = this.TransactionID;
            TransactionService.Rollback(txID);

            this.ResetControlValue();
        }
        private void InitializeControl()
        {
            FnRemittanceDataset remittanceDS = (FnRemittanceDataset)TransactionService.GetDS(this.TransactionID);
            FnRemittanceDataset.FnRemittanceRow row = remittanceDS.FnRemittance.FindByRemittanceID(this.DocumentID);
            long tempDocumentId = row.DocumentID;
            ctlCompaymyField.UseEccOnly = true;
            ctlCompaymyField.FlagActive = true;
            ctlRemittanceFormHeader.Initialize(this.TransactionID, tempDocumentId, this.InitialFlag);   // send SCGDocument.DocumentID for check visible see history
            ctlHistory.Initialize(tempDocumentId);
            ctlAttachment.Initialize(this.TransactionID, this.DocumentID, this.InitialFlag);

            ctlCreatorData.ControlGroupID = SCG.eAccounting.BLL.Implement.RemittanceFieldGroup.All;
            ctlRequesterData.ControlGroupID = SCG.eAccounting.BLL.Implement.RemittanceFieldGroup.All;

            ctlCreatorData.Initialize(this.TransactionID, this.DocumentID, this.InitialFlag);
            ctlRequesterData.Initialize(this.TransactionID, this.DocumentID, this.InitialFlag);
            ctlCreatorData.DataBind();
            ctlRequesterData.DataBind();
            ctlRemittanceFormHeader.DataBind();


        }
        public void BindControl(bool isCopy)
        {
            Guid txID = this.TransactionID;
            long remittanceID = this.DocumentID;

            FnRemittanceDataset remittanceDS = (FnRemittanceDataset)TransactionService.GetDS(txID);
            FnRemittanceDataset.FnRemittanceRow remittance = remittanceDS.FnRemittance.FindByRemittanceID(remittanceID);
            FnRemittanceDataset.DocumentRow document = remittanceDS.Document.FindByDocumentID(remittance.DocumentID);
            if (!isCopy && this.InitialFlag.Equals(FlagEnum.NewFlag))
            {
                ctlCompaymyField.ShowDefault();
                ctlCreatorData.ShowDefault();
                ctlRequesterData.ShowDefault();
                ctlRemittanceFormHeader.Status = FlagEnum.NewFlag;
                ctlSubject.Focus();
                BindCounterCashierDropdown(UIHelper.ParseLong(ctlCompaymyField.CompanyID));
                CheckUserRepOffice();
                SetDefaultPB();
                long locationID = 0;
                if (ctlCompaymyField.CompanyID == ctlRequesterData.CompanyID)
                {
                    SuUser userList = QueryProvider.SuUserQuery.FindByIdentity(UIHelper.ParseLong(ctlRequesterData.UserID));
                    if (userList != null)
                    {
                        if (userList.Location != null)
                            locationID = userList.Location.LocationID;
                    }
                }
                if (IsRepOffice)
                {
                    DbLocation location = ScgDbQueryProvider.DbLocationQuery.FindProxyByIdentity(locationID);
                    this.PBID = location.DefaultPBID.Value;
                    if (location != null)
                    {
                        if (location.DefaultPBID.HasValue && location.DefaultPBID.Value != 0)
                        {
                            SetValue(UIHelper.ParseLong(ctlCompaymyField.CompanyID), location.DefaultPBID.Value);
                        }
                    }
                    Dbpb dbpb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(Convert.ToInt64(PBID));
                    MainCurrencyID = dbpb.MainCurrencyID;
                }
            }
            else
            {
                #region BindHeader&Footer
                if (!isCopy)
                {
                    ctlRemittanceFormHeader.No = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(remittance.DocumentID).DocumentNo;
                    ctlRemittanceFormHeader.Status = ScgeAccountingQueryProvider.SCGDocumentQuery.GetDocumentCurrentStateName(UserAccount.CurrentLanguageID, remittance.DocumentID);
                    if (ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(remittance.DocumentID).DocumentDate != null)
                    {
                        //ctlRemittanceFormHeader.CreateDate = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(remittance.DocumentID).DocumentDate.Value.ToShortDateString();
                        ctlRemittanceFormHeader.CreateDate = UIHelper.ToDateString(document.DocumentDate);
                    }
                    ctlCreatorData.SetValue(document.CreatorID);
                }
                else
                {
                    ctlRemittanceFormHeader.Status = FlagEnum.NewFlag;
                    ctlCreatorData.ShowDefault();
                }
                ctlCompaymyField.SetValue(document.CompanyID);
                ctlSubject.Text = document.Subject;
                ctlRequesterData.SetValue(document.RequesterID);
                long locationID = 0;
                SuUser userList = QueryProvider.SuUserQuery.FindByIdentity(UIHelper.ParseLong(ctlRequesterData.UserID));
                if (userList != null)
                {
                    if (userList.Location != null)
                        locationID = userList.Location.LocationID;
                }
                CheckUserRepOffice();
                if (IsRepOffice)
                {
                    DbLocation location = ScgDbQueryProvider.DbLocationQuery.FindProxyByIdentity(locationID);
                    this.PBID = location.DefaultPBID.Value;
                    if (location != null)
                    {
                        if (location.DefaultPBID.HasValue && location.DefaultPBID.Value != 0)
                        {
                            SetValue(UIHelper.ParseLong(ctlCompaymyField.CompanyID), location.DefaultPBID.Value);
                        }
                    }
                    Dbpb dbpb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(Convert.ToInt64(PBID));
                    MainCurrencyID = dbpb.MainCurrencyID;
                }
                SetValue(UIHelper.ParseLong(ctlCompaymyField.CompanyID), remittance.PBID);
                if (!remittance.IsisFullClearingNull())
                {
                    ctlFullReturnCashChk.Checked = remittance.isFullClearing;
                }

                #endregion

                #region view post
                if (VisibleFields.Contains(RemittanceFieldGroup.VerifyDetail))
                {
                    if (!string.IsNullOrEmpty(document.BranchCode.Trim()))
                    {
                        ctlBranchForeign.Text = document.BranchCode;
                    }
                    //else
                    //{
                    //    ctlBranchForeign.Text = "0001";
                    //}
                    if (ParameterServices.EditableBusinessArea)
                    {
                        ctlBusinessAreaForeign.Enabled = true;
                        ctlBusinessAreaForeign.Style["display"] = "block";
                        ctlBusinessAreaForeignText.Style["display"] = "block";
                        ctlBusinessAreaForeignLabelExtender.Style["display"] = "block";
                    }
                    else
                    {
                        ctlBusinessAreaForeign.Enabled = false;
                        ctlBusinessAreaForeign.Style["display"] = "none";
                        ctlBusinessAreaForeignText.Style["display"] = "none";
                        ctlBusinessAreaForeignLabelExtender.Style["display"] = "none";
                    }
                    ctlBusinessAreaForeign.Text = document.BusinessArea;
                    if (string.IsNullOrEmpty(ctlBusinessAreaForeign.Text))
                    {
                        DbCompany com = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(UIHelper.ParseLong(ctlCompaymyField.CompanyID));
                        ctlBusinessAreaForeign.Text = com.BusinessArea;
                    }
                    if (!document.PostingDate.Equals(DateTime.MinValue))
                    {
                        ctlPostingDateForeign.DateValue = UIHelper.ToDateString(document.PostingDate);
                    }
                    else
                    {
                        //case editflag is default datetime.today
                        if (this.InitialFlag.Equals(FlagEnum.EditFlag))
                            ctlPostingDateForeign.DateValue = UIHelper.ToDateString(DateTime.Today);
                    }


                    if (!document.BaseLineDate.Equals(DateTime.MinValue))
                    {
                        ctlReceiveDateForeign.DateValue = UIHelper.ToDateString(document.BaseLineDate);
                    }
                    else
                    {     
                        if (this.InitialFlag.Equals(FlagEnum.EditFlag))
                        {
                            ctlReceiveDateForeign.DateValue = UIHelper.ToDateString(DateTime.Today);
                        }
                        else
                            ctlReceiveDateForeign.DateValue = string.Empty;
                    }

                    if (string.IsNullOrEmpty(document.PostingStatus))
                    {
                        ctlPostingStatusForeign.Text = GetMessage("PostingStatusN");
                    }
                    else
                    {
                        ctlPostingStatusForeign.Text = GetMessage(string.Format("PostingStatus{0}", document.PostingStatus));
                    }
                }
                #endregion

                #region BindGeneralTAB
                if (!remittance.IsTADocumentIDNull() && remittance.TADocumentID != 0)
                {
                    TADocument tadocument = ScgeAccountingQueryProvider.TADocumentQuery.FindByIdentity(remittance.TADocumentID);
                    ctlTANo.Text = tadocument.DocumentID.DocumentNo;
                    ctlTANo.Visible = true;
                    ctlTANoLbl.Text = string.Empty;
                    this.TADocumentID = remittance.TADocumentID.ToString();
                    this.DocumentIDofTA = tadocument.DocumentID.DocumentID;
                }
                #region BindRemittanceItemGrid
                //ctlRemittanceItemGrid.DataSource = remittanceDS.FnRemittanceItem;
                //ctlRemittanceItemGrid.DataBind();
                this.BindRemittanceItemGridview();
                #endregion

                #region BindAdvanceGrid
                IList<Advance> remittanceAdvanceList = new List<Advance>();
                foreach (FnRemittanceDataset.FnRemittanceAdvanceRow row in remittanceDS.FnRemittanceAdvance.Rows)
                {
                    Advance remittanceAdvance = new Advance();
                    AvAdvanceDocument advance = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindProxyByIdentity(row.AdvanceID);
                    remittanceAdvance.AdvanceID = advance.AdvanceID;
                    remittanceAdvance.DocumentNo = advance.DocumentID.DocumentNo;
                    remittanceAdvance.Description = advance.DocumentID.Subject;
                    remittanceAdvance.RemittanceID = this.DocumentID;
                    SuUser requester = QueryProvider.SuUserQuery.FindProxyByIdentity(advance.DocumentID.RequesterID.Userid);
                    remittanceAdvance.RequesterName = requester.UserName;
                    SuUser receiver = QueryProvider.SuUserQuery.FindProxyByIdentity(advance.DocumentID.ReceiverID.Userid);
                    remittanceAdvance.ReceiverName = receiver.UserName;
                    remittanceAdvance.DueDateOfRemittance = advance.DueDateOfRemittance;
                    remittanceAdvance.RequestDateOfRemittance = advance.RequestDateOfRemittance;
                    AvAdvanceDocument advanceDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindProxyByIdentity(advance.AdvanceID);
                    remittanceAdvance.Amount = advanceDocument.Amount;
                    remittanceAdvance.MainCurrencyAmount = advanceDocument.MainCurrencyAmount;
                    remittanceAdvance.ExchangeRateMainToTHBCurrency = advanceDocument.ExchangeRateMainToTHBCurrency;
                    remittanceAdvanceList.Add(remittanceAdvance);
                }
                this.AdvanceList = remittanceAdvanceList;

                this.BindCtlAdvanceGridview(remittanceAdvanceList);

                if (IsContainVisibleFields(RemittanceFieldGroup.FullClearing))
                {
                    if (this.InitialFlag.Equals(FlagEnum.EditFlag))
                    {
                        this.ShowFullReturnChk();
                    }
                    else if (this.InitialFlag.Equals(FlagEnum.NewFlag) & isCopy)
                    {
                        this.ShowFullReturnChk();
                    }
                    else if (this.InitialFlag.Equals(FlagEnum.ViewFlag))
                    {
                        this.ShowFullReturnChk();
                        ctlFullReturnCashChk.Enabled = IsContainEditableFields(RemittanceFieldGroup.FullClearing) && !this.InitialFlag.Equals(FlagEnum.ViewFlag);
                        //ctlFullReturnCashChk.Style["display"] = "block";
                    }
                }
                else
                {
                    ctlFullReturnCashChk.Style["display"] = "none";
                }
                #endregion

                #endregion

                #region BindMemo
                ctlMemo.Text = document.Memo;
                #endregion

                #region Tab Attachment
                ctlAttachment.BindControl();
                #endregion

                #region BindHistory
                ctlUpdatePanelHistory.Update();
                #endregion
            }
            this.TempCompanyID = UIHelper.ParseLong(ctlCompaymyField.CompanyID);

            ctlCounterCasheirUpdate.Update();
            ctlUpdatePanelAttechment.Update();
            ctlUpdatePanelGeneral.Update();
            ctlUpdatePanelHistory.Update();
            ctlUpdatePanelMemo.Update();
            ctlUpdatePanelHeader.Update();
        }
        #endregion

        #region Lookup Event
        protected void TA_OnObjectLookUpCalling(object sender, ObjectLookUpCallingEventArgs e)
        {
            UserControls.LOV.SCG.DB.TALookup taLookUp = sender as UserControls.LOV.SCG.DB.TALookup;
            taLookUp.TravelBy = TravellBy.Foreign;
            taLookUp.CompanyID = ctlCompaymyField.CompanyID;
            taLookUp.RequesterID = ctlRequesterData.UserID;
        }
        protected void TA_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            TADocumentObj TA = (TADocumentObj)e.ObjectReturn;
            if (ctlTANo.Text == TA.DocumentNo) return; // If select old TA, do nothing
            this.TADocumentID = TA.TADocumentID.Value.ToString();
            this.DocumentIDofTA = TA.DocumentID.Value;//เก็บ DocumentID ของ TA เพื่อเอาไป where ใน workFlow แล้วส่งค่าให้ DocumentView
            if (TA.DocumentNo.Trim().Equals(string.Empty))
            {
                ctlTANo.Text = "N/A";
                ctlTANo.Visible = false;
                ctlTANoLbl.Visible = true;
                ctlDeleteTA.Enabled = false;
            }
            else
            {
                ClearPage();
                ctlTANo.Text = TA.DocumentNo;
                ctlTANo.Visible = true;
                ctlTANoLbl.Visible = false;
                ctlDeleteTA.Enabled = true;
                this.TADocumentID = TA.TADocumentID.Value.ToString();

                // Query only advance that
                // 1. Status = OutStanding
                // 2. Do not used in Remittance that has flag <> 'Cancel'
                // 3. Do not used in Expense that has flag <> 'Cancel'
                Advance avCriteria = new Advance();
                avCriteria.CompanyID = UIHelper.ParseLong(ctlCompaymyField.CompanyID);
                avCriteria.RequesterID = UIHelper.ParseLong(ctlRequesterData.UserID);
                avCriteria.AdvanceType = ZoneType.Foreign;
                avCriteria.TADocumentID = TA.TADocumentID;

                IList<Advance> advanceList = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAdvanceList(avCriteria, 0, 100, string.Empty);
                foreach (Advance advance in advanceList)
                {
                    this.AddRemittanceAdvanceToTransaction(advance);
                }
                this.AdvanceList = advanceList;
                this.BindCtlAdvanceGridview(advanceList);

                // Get complete result for RemitItem
                this.ClearFnRemittanceItemRow();
                if (advanceList.Count > 0)
                {
                    IList<Advance> advList;
                    if (!IsRepOffice)
                    {
                        advList = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.SumAmountByPaymentTypeAndCurrency(GetAdvanceIdListFromAdvanceGrid());
                    }
                    else
                    {
                        advList = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.SumAmountByPaymentTypeAndCurrencyForRepOffice(GetAdvanceIdListFromAdvanceGrid());
                    }
                    AddAdvanceToRemittanceItemByPaymentTypeAndCurrency(advList);
                }
                this.BindRemittanceItemGridview();

                this.ShowFullReturnChk();
                ctlAddAddvance.Enabled = false;
                ctlAdvanceGridView.Columns[7].Visible = false;
                ctlUpdatePanelGeneral.Update();
            }
        }
        protected void ctlTANoLookup_Click(object sender, ImageClickEventArgs e)
        {
            ctlTALookup.Show();
        }
        protected void ctlDeleteTA_Click(object sender, ImageClickEventArgs e)
        {
            ClearPage();
            this.AdvanceList.Clear();
            this.BindRemittanceItemGridview();
            this.BindCtlAdvanceGridview(null);
        }
        protected void ClearPage()
        {
            this.TADocumentID = null;
            ctlTANo.Text = "N/A";
            ctlTANo.Visible = false;
            ctlTANoLbl.Visible = true;
            ctlDeleteTA.Enabled = false;
            ctlAddAddvance.Enabled = true;
            ctlTotalAdvanceDiv.Visible = false;
            ctlTotalRemittanceDiv.Visible = false;
            ctlAdvanceGridView.Columns[7].Visible = true;
            this.ClearFnRemittanceItemRow();
            this.ClearFnRemittanceAdvanceRow();
        }
        protected void CompaymyField_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (this.CanChangeCompanyOrRequester())
            {
                if (e.ObjectReturn != null)
                {
                    DbCompany company = (DbCompany)e.ObjectReturn;
                    this.TempCompanyID = company.CompanyID;
                    BindCounterCashierDropdown(company.CompanyID);
                    CheckUserRepOffice();
                    SetDefaultPB();
                }
            }
            else
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("CannotChangeCompany"));
                this.ValidationErrors.MergeErrors(errors);
                ctlValidationSummary.Focus();
                ctlCompaymyField.SetValue(this.TempCompanyID);
                ctlUpdatePanelValidate.Update();
            }
            ctlUpdatePanelGeneral.Update();
            ctlCounterCasheirUpdate.Update();
        }

        void ctlRequesterData_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (this.CanChangeCompanyOrRequester())
            {
                if (e.ObjectReturn != null)
                {
                    SuUser user = (SuUser)e.ObjectReturn;
                    this.TempRequesterID = user.Userid;
                    BindCounterCashierDropdown(UIHelper.ParseLong(ctlCompaymyField.CompanyID));
                    CheckUserRepOffice();
                    SetDefaultPB();
                }
            }
            else
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("CannotChangeRequester"));
                this.ValidationErrors.MergeErrors(errors);
                ctlValidationSummary.Focus();
                ctlRequesterData.SetValue(this.TempRequesterID);
                ctlUpdatePanelValidate.Update();
            }
            ctlUpdatePanelGeneral.Update();
            ctlCounterCasheirUpdate.Update();
        }

        protected void Advance_OnObjectLookUpCalling(object sender, ObjectLookUpCallingEventArgs e)
        {
            UserControls.LOV.AV.AdvanceLookup advanceLookUp = sender as UserControls.LOV.AV.AdvanceLookup;

            advanceLookUp.CompanyID = UIHelper.ParseLong(ctlCompaymyField.CompanyID);
            advanceLookUp.RequesterID = UIHelper.ParseLong(ctlRequesterData.UserID);
            advanceLookUp.AdvanceType = ZoneType.Foreign;
            advanceLookUp.PBID = this.PBID;
            Dbpb dbpb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(Convert.ToInt64(PBID));
            if (dbpb != null && dbpb.MainCurrencyID.HasValue)
            {
                advanceLookUp.MainCurrencyID = dbpb.MainCurrencyID;
            }
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
            ctlDeleteTA.Enabled = false;
            IList<Advance> advanceList = (IList<Advance>)e.ObjectReturn;
            //IList<Advance> advDocumentList = new List<Advance>();
            if (this.AdvanceList == null)
            {
                this.AdvanceList = new List<Advance>();
            }
            bool neverClearAvGrid = true;
            foreach (Advance advance in advanceList)
            {
                SumExchangeRateMaintoTHB = 0.00;
                AvAdvanceDocument advDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindByIdentity(advance.AdvanceID);
                if (advDocument != null)
                {
                    if (advDocument.TADocumentID != null && advDocument.TADocumentID > 0 && advDocument.DocumentID.RequesterID.Userid == UIHelper.ParseLong(ctlRequesterData.UserID) && advDocument.DocumentID.CompanyID.CompanyID == UIHelper.ParseLong(ctlCompaymyField.CompanyID))
                    {
                        ClearPage();
                        this.AdvanceList.Clear();
                        TADocument TA = ScgeAccountingQueryProvider.TADocumentQuery.FindByIdentity(advDocument.TADocumentID.Value);
                        ctlTANo.Text = TA.DocumentID.DocumentNo;
                        this.TADocumentID = TA.TADocumentID.ToString();
                        this.DocumentIDofTA = TA.TADocumentID;//เก็บ DocumentID ของ TA เพื่อเอาไป where ใน workFlow แล้วส่งค่าให้ DocumentView
                        if (neverClearAvGrid)
                        {
                            neverClearAvGrid = false;
                            this.ClearFnRemittanceAdvanceRow();
                        }
                        if (ctlTANo.Text.Trim().Equals(string.Empty))
                        {
                            ctlTANo.Text = "N/A";
                            ctlTANo.Visible = false;
                            ctlTANoLbl.Visible = true;
                        }
                        else
                        {
                            ctlTANo.Visible = true;
                            ctlTANoLbl.Visible = false;
                            ctlDeleteTA.Enabled = true;
                            ctlAddAddvance.Enabled = false;

                            this.ShowFullReturnChk();
                            ctlAdvanceGridView.Columns[7].Visible = false;
                        }

                        // Query only advance that
                        // 1. Status = OutStanding
                        // 2. Do not used in Remittance that has flag <> 'Cancel'
                        // 3. Do not used in Expense that has flag <> 'Cancel'
                        Advance avCriteria = new Advance();
                        avCriteria.CompanyID = UIHelper.ParseLong(ctlCompaymyField.CompanyID);
                        avCriteria.RequesterID = UIHelper.ParseLong(ctlRequesterData.UserID);
                        avCriteria.AdvanceType = ZoneType.Foreign;
                        avCriteria.TADocumentID = TA.TADocumentID;

                        IList<Advance> advList = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAdvanceList(avCriteria, 0, 100, string.Empty);
                        foreach (Advance adv in advList)
                        {
                            if (!this.AdvanceList.Contains(adv) && adv.RequesterID.ToString().Equals(ctlRequesterData.UserID))
                            {

                                this.AdvanceList.Add(adv);
                                this.AddRemittanceAdvanceToTransaction(adv);
                            }
                        }
                        if (IsRepOffice)
                        {
                            foreach (Advance adv in AdvanceList)
                            {
                                SumExchangeRateMaintoTHB = SumExchangeRateMaintoTHB + adv.ExchangeRateMainToTHBCurrency.GetValueOrDefault();
                            }
                            ctlExchangeRateToTHB.Text = UIHelper.BindExchangeRate((SumExchangeRateMaintoTHB / AdvanceList.Count).ToString());
                            ctlExchangeRateToTHBHField.Value = UIHelper.BindExchangeRate((SumExchangeRateMaintoTHB / AdvanceList.Count).ToString());
                        }
                        break; // Do not working if meet with first TA
                    }
                    else if (!this.AdvanceList.Contains(advance))
                    {
                        this.AdvanceList.Add(advance);
                        this.AddRemittanceAdvanceToTransaction(advance);
                        if (IsRepOffice)
                        {
                            foreach (Advance adv in AdvanceList)
                            {
                                SumExchangeRateMaintoTHB = SumExchangeRateMaintoTHB + adv.ExchangeRateMainToTHBCurrency.GetValueOrDefault();
                            }
                            ctlExchangeRateToTHB.Text = UIHelper.BindExchangeRate((SumExchangeRateMaintoTHB / AdvanceList.Count).ToString());
                            ctlExchangeRateToTHBHField.Value = UIHelper.BindExchangeRate((SumExchangeRateMaintoTHB / AdvanceList.Count).ToString());
                        }
                    }
                }
            }

            this.BindCtlAdvanceGridview(this.AdvanceList);

            // Get complete result for RemitItem
            this.ClearFnRemittanceItemRow();
            if (advanceList.Count > 0)
            {
                IList<Advance> advList;
                if (!IsRepOffice)
                {
                    advList = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.SumAmountByPaymentTypeAndCurrency(GetAdvanceIdListFromAdvanceGrid());
                }
                else
                {
                    advList = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.SumAmountByPaymentTypeAndCurrencyForRepOffice(GetAdvanceIdListFromAdvanceGrid());
                }
                AddAdvanceToRemittanceItemByPaymentTypeAndCurrency(advList);
            }
            this.BindRemittanceItemGridview();

            this.ShowFullReturnChk();

            ctlUpdatePanelGeneral.Update();
        }
        protected IList<long> GetAdvanceIdListFromAdvanceGrid()
        {
            IList<long> advanceIDList = new List<long>();
            foreach (GridViewRow row in ctlAdvanceGridView.Rows)
            {
                advanceIDList.Add(UIHelper.ParseLong(ctlAdvanceGridView.DataKeys[row.RowIndex]["AdvanceID"].ToString()));
            }
            return advanceIDList;
        }
        protected void ctlAdvanceLookup_Click(object sender, EventArgs e)
        {
            ctlAdvanceLookup.Show();
        }

        #endregion

        protected void ctlViewPostForeign_Click(object sender, EventArgs e)
        {
            try
            {
                this.isNotViewPost = false;
                FnRemittanceDataset remittanceDs = (FnRemittanceDataset)TransactionService.GetDS(this.TransactionID);
                FnRemittanceDataset.FnRemittanceRow row = remittanceDs.FnRemittance.FindByRemittanceID(this.DocumentID);
                SCGDocumentService.ValidateVerifyDetail(this.TransactionID, ViewPostDocumentType.Remittance, row.IsRepOffice);
                ctlViewPost.Initialize(row.DocumentID, DocumentKind.Remittance);
                ctlViewPost.Show();
                ctlUpdatePanelViewPost.Update();
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                ctlUpdatePanelValidate.Update();
            }
        }

        #region Remittance Document Gridview
        /// <summary>
        /// For Update Remittance grid every time this page was refresh by Anuwat S.,
        /// This method is recalculate value on RemittanceGrid, also recalculate this.SumRemittanceItemAmount
        /// </summary>
        protected void UpdateRemittanceGrid()
        {
            this.SumRemittanceItemAmount = 0;
            double tmpAmountTHB = 0;
            foreach (GridViewRow gridviewRow in ctlRemittanceItemGrid.Rows)
            {
                Literal ctlExchangeRate = (Literal)ctlRemittanceItemGrid.Rows[gridviewRow.RowIndex].FindControl("ctlExchangRateLabel");
                TextBox ctlFCurrencyRem = (TextBox)ctlRemittanceItemGrid.Rows[gridviewRow.RowIndex].FindControl("ctlFCurrencyRem");
                Literal ctlAmountTHB = (Literal)ctlRemittanceItemGrid.Rows[gridviewRow.RowIndex].FindControl("ctlAmountTHB");

                tmpAmountTHB = UIHelper.ParseDouble(ctlExchangeRate.Text) * UIHelper.ParseDouble(ctlFCurrencyRem.Text);
                ctlAmountTHB.Text = string.Format("{0:#,##0.00}", tmpAmountTHB);

                this.SumRemittanceItemAmount += tmpAmountTHB;
            }
            ctlTotalRemittanceAmountLabel.Text = string.Format("{0:#,##0.00}", this.SumRemittanceItemAmount);
            ShowFullReturnChk();
        }
        protected void ctlRemittanceItemGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (e.CommandName.Equals("AddRemittanceItem"))
            {
                UserControls.DropdownList.SCG.DB.StatusDropdown ctlPaymentTypeDropdown;
                UserControls.Dropdownlist.SS.DB.CurrencyDropdown ctlCurrencyDropdown;
                Literal ctlFCurrencyAdv;
                TextBox ctlExchangeRate;
                TextBox ctlFCurrencyRem;
                Literal ctlAmountTHB;
                HiddenField ctlAmountTHBAdvance;
                Literal ctlMainCurrencyAmount;
                HiddenField ctlAmountMainCurrencyAdvance;
                HiddenField ctlExchangeRateTHB;

                FnRemittanceDataset remittanceDS = (FnRemittanceDataset)TransactionService.GetDS(this.TransactionID);

                SaveRemittanceItem();

                if (ctlRemittanceItemGrid.Rows.Count == 0)
                {
                    ctlPaymentTypeDropdown = (UserControls.DropdownList.SCG.DB.StatusDropdown)((Table)ctlRemittanceItemGrid.Controls[0]).Rows[2].FindControl("ctlPaymentTypeDropdown");
                    ctlCurrencyDropdown = (UserControls.Dropdownlist.SS.DB.CurrencyDropdown)((Table)ctlRemittanceItemGrid.Controls[0]).Rows[2].FindControl("ctlCurrencyDropdown");
                    ctlFCurrencyAdv = (Literal)((Table)ctlRemittanceItemGrid.Controls[0]).Rows[2].FindControl("ctlFCurrencyAdv");
                    ctlExchangeRate = (TextBox)((Table)ctlRemittanceItemGrid.Controls[0]).Rows[2].FindControl("ctlExchangeRate");
                    ctlFCurrencyRem = (TextBox)((Table)ctlRemittanceItemGrid.Controls[0]).Rows[2].FindControl("ctlFCurrencyRem");
                    ctlAmountTHB = (Literal)((Table)ctlRemittanceItemGrid.Controls[0]).Rows[2].FindControl("ctlAmountTHB");
                    ctlAmountTHBAdvance = (HiddenField)((Table)ctlRemittanceItemGrid.Controls[0]).Rows[2].FindControl("ctlAmountTHBAdvance");
                    ctlMainCurrencyAmount = (Literal)((Table)ctlRemittanceItemGrid.Controls[0]).Rows[2].FindControl("ctlMainCurrencyAmount");
                    ctlAmountMainCurrencyAdvance = (HiddenField)((Table)ctlRemittanceItemGrid.Controls[0]).Rows[2].FindControl("ctlAmountMainCurrencyAdvance");
                    ctlExchangeRateTHB = (HiddenField)((Table)ctlRemittanceItemGrid.Controls[0]).Rows[2].FindControl("ctlExchangeRateTHB");
                }
                else
                {
                    ctlPaymentTypeDropdown = (UserControls.DropdownList.SCG.DB.StatusDropdown)ctlRemittanceItemGrid.FooterRow.FindControl("ctlPaymentTypeDropdown");
                    ctlCurrencyDropdown = (UserControls.Dropdownlist.SS.DB.CurrencyDropdown)ctlRemittanceItemGrid.FooterRow.FindControl("ctlCurrencyDropdown");
                    ctlFCurrencyAdv = (Literal)ctlRemittanceItemGrid.FooterRow.FindControl("ctlFCurrencyAdv");
                    ctlExchangeRate = (TextBox)ctlRemittanceItemGrid.FooterRow.FindControl("ctlExchangeRate");
                    ctlFCurrencyRem = (TextBox)ctlRemittanceItemGrid.FooterRow.FindControl("ctlFCurrencyRem");
                    ctlAmountTHB = (Literal)ctlRemittanceItemGrid.FooterRow.FindControl("ctlAmountTHB");
                    ctlAmountTHBAdvance = (HiddenField)((Table)ctlRemittanceItemGrid.Controls[0]).Rows[2].FindControl("ctlAmountTHBAdvance");
                    ctlMainCurrencyAmount = (Literal)ctlRemittanceItemGrid.FooterRow.FindControl("ctlMainCurrencyAmount");
                    ctlAmountMainCurrencyAdvance = (HiddenField)((Table)ctlRemittanceItemGrid.Controls[0]).Rows[2].FindControl("ctlAmountMainCurrencyAdvance");
                    ctlExchangeRateTHB = (HiddenField)((Table)ctlRemittanceItemGrid.Controls[0]).Rows[2].FindControl("ctlExchangeRateTHB");
                }

                FnRemittanceItem remittanceItem = new FnRemittanceItem();
                remittanceItem.Remittance = new FnRemittance(this.DocumentID);
                remittanceItem.Remittance.PB = new Dbpb(this.PBID.Value);
                remittanceItem.PaymentType = ctlPaymentTypeDropdown.SelectedValue;
                remittanceItem.Currency = SsDbQueryProvider.DbCurrencyQuery.FindByIdentity(UIHelper.ParseShort(ctlCurrencyDropdown.SelectedValue));
                remittanceItem.ForeignCurrencyAdvanced = UIHelper.ParseDouble(ctlFCurrencyAdv.Text);
                remittanceItem.ExchangeRate = UIHelper.ParseDouble(ctlExchangeRate.Text);
                remittanceItem.ForeignCurrencyRemitted = UIHelper.ParseDouble(ctlFCurrencyRem.Text);
                remittanceItem.ExchangeRateTHB = UIHelper.ParseDouble(ctlExchangeRateToTHBHField.Value);
                if (!IsRepOffice)
                {
                    remittanceItem.AmountTHB = (double)Math.Round((decimal)(UIHelper.ParseDouble(ctlExchangeRate.Text) * UIHelper.ParseDouble(ctlFCurrencyRem.Text)), 2, MidpointRounding.AwayFromZero);
                }
                else
                {
                    remittanceItem.MainCurrencyAmount = (double)Math.Round((decimal)(UIHelper.ParseDouble(ctlExchangeRate.Text) * UIHelper.ParseDouble(ctlFCurrencyRem.Text)), 2, MidpointRounding.AwayFromZero);
                    remittanceItem.AmountTHB = (double)Math.Round((decimal)((UIHelper.ParseDouble(ctlExchangeRate.Text) * UIHelper.ParseDouble(ctlFCurrencyRem.Text)) * remittanceItem.ExchangeRateTHB), 2, MidpointRounding.AwayFromZero);
                }
                remittanceItem.ForeignAmountMainCurrencyAdvanced = UIHelper.ParseDouble(ctlAmountMainCurrencyAdvance.Value);
                remittanceItem.IsImportFromAdvance = false;
                remittanceItem.Active = true;

                try
                {
                    FnRemittanceItemService.AddFnRemittanceItemTransaction(this.TransactionID, remittanceItem, false, this.SumAdvanceAmount);
                }
                catch (ServiceValidationException ex)
                {
                    this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                }

                this.BindRemittanceItemGridview();
                this.ShowFullReturnChk();
                ctlUpdatePanelGeneral.Update();
                ctlUpdatePanelValidate.Update();
            }
            else if (e.CommandName.Equals("DeleteRemitanceItem"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                long remittanceItemID = UIHelper.ParseLong(ctlRemittanceItemGrid.DataKeys[rowIndex].Value.ToString());
                FnRemittanceDataset remittanceDS = (FnRemittanceDataset)TransactionService.GetDS(this.TransactionID);
                try
                {
                    SaveRemittanceItem();
                    FnRemittanceItemService.DeleteRemittanceItemFromTransaction(this.TransactionID, remittanceItemID, false);
                    this.BindRemittanceItemGridview();
                }
                catch (ServiceValidationException ex)
                {
                    //errors.MergeErrors(ex.ValidationErrors);
                    this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                }
                this.ShowFullReturnChk();
                ctlUpdatePanelValidate.Update();
                ctlUpdatePanelGeneral.Update();
            }
        }
        protected void ctlRemittanceItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            bool IsFromAdvance = false;
            if (!e.Row.RowIndex.Equals(-1))
            {
                long remittanceItemID = UIHelper.ParseLong(ctlRemittanceItemGrid.DataKeys[e.Row.RowIndex].Value.ToString());
                FnRemittanceDataset remittanceDS = (FnRemittanceDataset)TransactionService.GetDS(this.TransactionID);
                FnRemittanceDataset.FnRemittanceItemRow remittanceItemRow = remittanceDS.FnRemittanceItem.FindByRemittanceItemID(remittanceItemID);
                IsFromAdvance = remittanceItemRow.IsImportFromAdvance;
            }

            UserControls.DropdownList.SCG.DB.StatusDropdown ctlPaymentTypeDropdown = (UserControls.DropdownList.SCG.DB.StatusDropdown)e.Row.FindControl("ctlPaymentTypeDropdown");
            UserControls.Dropdownlist.SS.DB.CurrencyDropdown ctlCurrencyDropdown = (UserControls.Dropdownlist.SS.DB.CurrencyDropdown)e.Row.FindControl("ctlCurrencyDropdown");
            HiddenField ctlTempPaymentMethodValue = (HiddenField)e.Row.FindControl("ctlTempPaymentMethodValue");
            HiddenField ctlTempCurrencyValue = (HiddenField)e.Row.FindControl("ctlTempCurrencyValue");
            ImageButton ctlDelete = (ImageButton)e.Row.FindControl("ctlDelete");
            if (ctlDelete != null)
            {
                ctlDelete.Visible = !IsFromAdvance;
            }
            if (ctlPaymentTypeDropdown != null)
            {
                ctlPaymentTypeDropdown.Enable = !IsFromAdvance;
                ctlPaymentTypeDropdown.GroupType = GroupStatus.PaymentTypeFRN;
                ctlPaymentTypeDropdown.StatusBind();
                if (ctlTempPaymentMethodValue != null)
                {
                    ctlPaymentTypeDropdown.SelectedValue = ctlTempPaymentMethodValue.Value;
                }
            }
            if (ctlCurrencyDropdown != null)
            {
                ctlCurrencyDropdown.Enable = !IsFromAdvance;
                if (ctlTempCurrencyValue != null)
                {
                    ctlCurrencyDropdown.BindCurrency(UIHelper.ParseShort(ctlTempCurrencyValue.Value));
                }
            }

            // ======== Add code for calculate on client by Anuwat S on 08/05/2009 ========
            TextBox ctlExchangeRate = (TextBox)e.Row.FindControl("ctlExchangeRate");
            Literal ctlExchangRateLabel = (Literal)e.Row.FindControl("ctlExchangRateLabel");
            TextBox ctlFCurrencyRem = (TextBox)e.Row.FindControl("ctlFCurrencyRem");
            HiddenField ctlDivAmount = (HiddenField)e.Row.FindControl("ctlDivAmount");
            HiddenField ctlExchangeRateTHB = (HiddenField)e.Row.FindControl("ctlExchangeRateTHB");
            HiddenField ctlAmountMainCurrencyAdvance = (HiddenField)e.Row.FindControl("ctlAmountMainCurrencyAdvance");
            Literal ctlFCurrencyAdv = (Literal)e.Row.FindControl("ctlFCurrencyAdv");
            if (ctlFCurrencyRem != null)
            {
                if (UIHelper.ParseDouble(ctlFCurrencyAdv.Text) >= UIHelper.ParseDouble(ctlFCurrencyRem.Text))
                {
                    ctlDivAmount.Value = (UIHelper.ParseDouble(ctlFCurrencyAdv.Text) - UIHelper.ParseDouble(ctlFCurrencyRem.Text)).ToString();  //ยืม - คืน
                }
                else
                {
                    ctlDivAmount.Value = (UIHelper.ParseDouble(ctlFCurrencyRem.Text) - UIHelper.ParseDouble(ctlFCurrencyAdv.Text)).ToString(); //ยืม - คืน
                }

                string scriptCal = string.Empty;
                if (!IsRepOffice)
                {
                    if (ctlExchangeRate != null)
                    {
                        //scriptCal = string.Format("calAmountTHB('{0}', '{1}', '{2}', '{3}', '{4}')", ctlExchangeRate.ClientID, ctlFCurrencyRem.ClientID, ctlAmountTHB.ClientID, string.Empty, ctlFCurrencyAdv.ClientID);
                        scriptCal = string.Format("calAmountTHB('{0}', '{1}', '{2}', '{3}', '{4}')", ctlExchangeRate.ClientID, ctlFCurrencyRem.ClientID, e.Row.ClientID, ctlDivAmount.ClientID, e.Row.ClientID);
                        ctlExchangeRate.Attributes.Add("onblur", scriptCal);
                        ctlFCurrencyRem.Attributes.Add("onblur", scriptCal);
                    }
                    else if (ctlExchangRateLabel != null)
                    {
                        //scriptCal = string.Format("calAmountTHB('{0}', '{1}', '{2}', '{3}', '{4}')", ctlExchangRateLabel.ClientID, ctlFCurrencyRem.ClientID, ctlAmountTHB.ClientID, ctlAmountTHBAdvance.ClientID, ctlFCurrencyAdv.ClientID);
                        scriptCal = string.Format("calAmountTHB('{0}', '{1}', '{2}', '{3}', '{4}')", e.Row.ClientID, ctlFCurrencyRem.ClientID, e.Row.ClientID, ctlDivAmount.ClientID, e.Row.ClientID);
                        ctlFCurrencyRem.Attributes.Add("onblur", scriptCal);
                    }
                }
                else
                {
                    if (ctlExchangeRate != null)
                    {
                        //scriptCal = string.Format("calAmountTHB('{0}', '{1}', '{2}', '{3}', '{4}')", ctlExchangeRate.ClientID, ctlFCurrencyRem.ClientID, ctlAmountTHB.ClientID, string.Empty, ctlFCurrencyAdv.ClientID);
                        scriptCal = string.Format("calAmountTHBForRepOffice('{0}', '{1}', '{2}', '{3}', '{4}','{5}','{6}','{7}')", ctlExchangeRate.ClientID, ctlFCurrencyRem.ClientID, e.Row.ClientID, ctlDivAmount.ClientID, e.Row.ClientID, ctlExchangeRateToTHBHField.ClientID, e.Row.ClientID, ctlAmountMainCurrencyAdvance.ClientID);
                        ctlExchangeRate.Attributes.Add("onblur", scriptCal);
                        ctlFCurrencyRem.Attributes.Add("onblur", scriptCal);
                    }
                    else if (ctlExchangRateLabel != null)
                    {
                        //scriptCal = string.Format("calAmountTHB('{0}', '{1}', '{2}', '{3}', '{4}')", ctlExchangRateLabel.ClientID, ctlFCurrencyRem.ClientID, ctlAmountTHB.ClientID, ctlAmountTHBAdvance.ClientID, ctlFCurrencyAdv.ClientID);
                        scriptCal = string.Format("calAmountTHBForRepOffice('{0}', '{1}', '{2}', '{3}', '{4}','{5}','{6}','{7}')", e.Row.ClientID, ctlFCurrencyRem.ClientID, e.Row.ClientID, ctlDivAmount.ClientID, e.Row.ClientID, ctlExchangeRateToTHBHField.ClientID, e.Row.ClientID, ctlAmountMainCurrencyAdvance.ClientID);
                        ctlFCurrencyRem.Attributes.Add("onblur", scriptCal);
                    }
                }
            }

            if (ctlDivAmount != null)
            {
                this.SumDivAmount += UIHelper.ParseDouble(ctlDivAmount.Value);
            }
            ctlTotalDivAmount.Value = UIHelper.BindDecimal(this.SumDivAmount.ToString());
            // ====== End add code for calculate on client by Anuwat S on 08/05/2009 ======

            //Auto increment No. in gridview.
            Literal ctlNoLabel = e.Row.FindControl("ctlNo") as Literal;
            if (ctlNoLabel != null)
            {
                if (e.Row.RowIndex != -1)
                {
                    ctlNoLabel.Text = ((ctlRemittanceItemGrid.PageSize * ctlRemittanceItemGrid.CustomPageIndex) + (e.Row.RowIndex + 1)).ToString();
                }
            }
        }

        protected void ctlFullReturnCashChk_CheckedChanged(object sender, EventArgs e)
        {
            SaveRemittanceItem();
            this.BindRemittanceItemGridview();
            this.ShowFullReturnChk();
            ctlUpdatePanelGeneral.Update();
            ctlUpdatePanelValidate.Update();
            ctlUpdatePanelGeneral.Update();
        }

        protected void ctlRemittanceItemGrid_DataBound(object sender, EventArgs e)
        {
            //if (ctlRemittanceItemGrid.Rows.Count == 0)
            //{
            //UserControls.DropdownList.SCG.DB.StatusDropdown ctlPaymentTypeDropdown = (UserControls.DropdownList.SCG.DB.StatusDropdown)((Table)ctlRemittanceItemGrid.Controls[0]).Rows[2].FindControl("ctlPaymentTypeDropdown");
            //UserControls.Dropdownlist.SS.DB.CurrencyDropdown ctlCurrencyDropdown = (UserControls.Dropdownlist.SS.DB.CurrencyDropdown)((Table)ctlRemittanceItemGrid.Controls[0]).Rows[2].FindControl("ctlCurrencyDropdown"); ;

            //if (ctlPaymentTypeDropdown != null)
            //{
            //    ctlPaymentTypeDropdown.GroupType = GroupStatus.PaymentTypeFRN;
            //    ctlPaymentTypeDropdown.StatusBind();
            //}
            //if (ctlCurrencyDropdown != null)
            //{
            //    ctlCurrencyDropdown.BindCurrencyDropdown();
            //}

            //ctlTotalRemittanceAmount.Text = this.SumRemittanceItemAmount.ToString();
            //}
            CheckUserRepOffice();
            if (this.IsRepOffice)
            {
                ctlRemittanceItemGrid.Columns[6].Visible = true;
                if (PBID != null && PBID != 0)
                {
                    Dbpb dbpb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(Convert.ToInt64(PBID));
                    getCurrencySymbol(Convert.ToInt16(dbpb.MainCurrencyID));
                    ctlRemittanceItemGrid.Columns[6].HeaderText = string.Format(GetProgramMessage("RemittanceMainCurrencyAmount"), " (" + dbCurrency.Symbol + ")");
                }
                else
                {
                    ctlRemittanceItemGrid.Columns[6].HeaderText = string.Format(GetProgramMessage("RemittanceMainCurrencyAmount"), "");
                }
                if (UserAccount.IsAccountant || UserAccount.IsPayment)
                {
                    if (!VisibleFields.Contains(RemittanceFieldGroup.VerifyDetail))
                    {

                        ctlRemittanceItemGrid.Columns[7].Visible = false;
                    }
                    else
                    {
                        ctlRemittanceItemGrid.Columns[7].Visible = true;
                    }

                }
                else
                {
                    ctlRemittanceItemGrid.Columns[7].Visible = false;
                }
            }
            else
            {
                ctlRemittanceItemGrid.Columns[6].Visible = false;
                ctlRemittanceItemGrid.Columns[7].Visible = true;
            }
        }
        #endregion

        #region AdvanceGridview
        protected void ctlAdvanceGridview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal ctlNoLabel = e.Row.FindControl("ctlNoLabel") as Literal;
                ctlNoLabel.Text = ((ctlAdvanceGridView.PageSize * ctlAdvanceGridView.CustomPageIndex) + (e.Row.RowIndex + 1)).ToString();
                Literal ctlLblAmount;
                CountRow++;
                HiddenField ctlExchangeRateMainToTHBCurrency = (HiddenField)e.Row.FindControl("ctlExchangeRateMainToTHBCurrency");
                //Sum Amount(THB)
                if (!IsRepOffice)
                {
                    ctlLblAmount = (Literal)e.Row.FindControl("ctlLblAmount");
                }
                else
                {
                    ctlLblAmount = (Literal)e.Row.FindControl("ctlLblMainCurrencyAmount");
                }
                this.SumAdvanceAmount += UIHelper.ParseDouble(ctlLblAmount.Text);
                this.SumExchangeRateMaintoTHB += UIHelper.ParseDouble(ctlExchangeRateMainToTHBCurrency.Value);
                ctlTotalAdvanceAmountLabel.Text = UIHelper.BindDecimal(this.SumAdvanceAmount.ToString());
                ctlExchangeRateToTHB.Text = UIHelper.BindExchangeRate((SumExchangeRateMaintoTHB / CountRow).ToString());
                ctlExchangeRateToTHBHField.Value = UIHelper.BindExchangeRate((SumExchangeRateMaintoTHB / CountRow).ToString());

            }
        }
        protected void ctlAdvanceGridview_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("DeleteAdvance"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                long AdvanceID = UIHelper.ParseLong(ctlAdvanceGridView.DataKeys[rowIndex]["AdvanceID"].ToString());
                FnRemittanceAdvanceService.DeleteRemittanceAdvanceFromTransaction(this.TransactionID, AdvanceID, this.DocumentID);
                this.AdvanceList.RemoveAt(rowIndex);
                this.BindCtlAdvanceGridview(AdvanceList);
                // Get complete result for RemitItem
                this.ClearFnRemittanceItemRow();
                if (GetAdvanceIdListFromAdvanceGrid().Count > 0)
                {
                    IList<Advance> advList;
                    if (!IsRepOffice)
                    {
                        advList = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.SumAmountByPaymentTypeAndCurrency(GetAdvanceIdListFromAdvanceGrid());
                    }
                    else
                    {
                        advList = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.SumAmountByPaymentTypeAndCurrencyForRepOffice(GetAdvanceIdListFromAdvanceGrid());
                    }
                    AddAdvanceToRemittanceItemByPaymentTypeAndCurrency(advList);
                }
                this.BindRemittanceItemGridview();
                this.ShowFullReturnChk();
                ctlUpdatePanelGeneral.Update();
            }
            else if (e.CommandName.Equals("AdvanceLink"))
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                long advanceID = UIHelper.ParseLong(ctlAdvanceGridView.DataKeys[rowIndex]["AdvanceID"].ToString());

                AvAdvanceDocument advance = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindByIdentity(advanceID);

                SS.Standard.WorkFlow.DTO.WorkFlow workFlow = new SS.Standard.WorkFlow.DTO.WorkFlow();
                if (advance != null)
                {
                    workFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(advance.DocumentID.DocumentID);
                    long workFlowID;
                    if (workFlow != null)
                        workFlowID = workFlow.WorkFlowID;
                    else
                        workFlowID = 0;
                    //popup Document View by WorkFlow
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), string.Empty, "window.open('../Programs/DocumentView.aspx?wfid=" + workFlowID.ToString() + "')", true);

                    //UpdateRemittanceGrid();
                }

                this.BindCtlAdvanceGridview(AdvanceList);
            }
        }
        protected void ctlAdvanceGridview_DataBound(object sender, EventArgs e)
        {
            //if (ctlAdvanceGridView.Rows.Count == 0)
            //{

            if (this.IsRepOffice)
            {
                ctlAdvanceGridView.Columns[6].Visible = true;
                if (PBID != null && PBID != 0)
                {
                    Dbpb dbpb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(Convert.ToInt64(PBID));
                    getCurrencySymbol(Convert.ToInt16(dbpb.MainCurrencyID));
                    //MainCurrencyID = dbCurrency.CurrencyID;
                    ctlAdvanceGridView.Columns[6].HeaderText = string.Format(GetProgramMessage("MainCurrencyAmountHeaderText"), " (" + dbCurrency.Symbol + ")");
                    ctlExchangeRateToTHBLbl.Text = string.Format(GetProgramMessage("MainCurrencyExchangeRateToTHB"), dbCurrency.Symbol);
                }
                else
                {
                    ctlAdvanceGridView.Columns[6].HeaderText = string.Format(GetProgramMessage("MainCurrencyAmountHeaderText"), "");
                    ctlExchangeRateToTHBLbl.Text = string.Format(GetProgramMessage("MainCurrencyExchangeRateToTHB"), "");
                }
                if (UserAccount.IsAccountant || UserAccount.IsPayment)
                {
                    if (!VisibleFields.Contains(RemittanceFieldGroup.VerifyDetail))
                    {
                        ctlExchangeRateLbl.Visible = false; // false
                        ctlExchangeRateToTHB.Visible = false;
                        ctlExchangeRateToTHBLbl.Visible = false;
                        ctlAdvanceGridView.Columns[7].Visible = false;
                    }
                    else
                    {
                        ctlExchangeRateLbl.Visible = true;
                        ctlExchangeRateToTHB.Visible = true;
                        ctlExchangeRateToTHBLbl.Visible = true;
                        ctlAdvanceGridView.Columns[7].Visible = true;
                    }
                }
                else
                {
                    ctlExchangeRateLbl.Visible = false; // false
                    ctlExchangeRateToTHB.Visible = false;
                    ctlExchangeRateToTHBLbl.Visible = false;
                    ctlAdvanceGridView.Columns[7].Visible = false;
                }
            }
            else
            {
                ctlExchangeRateLbl.Visible = false; // false
                ctlExchangeRateToTHB.Visible = false;
                ctlExchangeRateToTHBLbl.Visible = false;
                ctlAdvanceGridView.Columns[6].Visible = false;
                ctlAdvanceGridView.Columns[7].Visible = true;
            }
            if (ctlAdvanceGridView.Rows.Count == 0)
            {
                ctlTANoLookup.Enabled = true;
                ctlDeleteTA.Enabled = true;
                ctlTotalAdvanceAmountLabel.Text = UIHelper.BindDecimal(this.SumAdvanceAmount.ToString());
                FnRemittanceDataset remittanceDS = (FnRemittanceDataset)TransactionService.GetDS(this.TransactionID);
                if (remittanceDS != null)
                {
                    if (remittanceDS.FnRemittanceAdvance.Count > 0)
                    {
                        this.ClearFnRemittanceAdvanceRow();
                    }
                    if (remittanceDS.FnRemittanceItem.Count > 0)
                    {
                        this.ClearFnRemittanceItemRow();
                    }
                    this.BindRemittanceItemGridview();
                    //this.BindCtlAdvanceGridview(this.AdvanceList);
                }
            }
        }
        #endregion

        protected void ctlTANo_Click(object sender, EventArgs e)
        {
            SS.Standard.WorkFlow.DTO.WorkFlow workFlow = new SS.Standard.WorkFlow.DTO.WorkFlow();
            if (DocumentIDofTA != 0)
            {
                workFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(DocumentIDofTA);
                long workFlowID;
                if (workFlow != null)
                    workFlowID = workFlow.WorkFlowID;
                else
                    workFlowID = 0;
                //popup Document View by WorkFlow
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), string.Empty, "window.open('../Programs/DocumentView.aspx?wfid=" + workFlowID.ToString() + "')", true);
            }
        }

        #region Public Function
        public void BindCtlAdvanceGridview(IList<Advance> advanceList)
        {
            this.SumAdvanceAmount = 0;
            this.CountRow = 0;
            this.SumExchangeRateMaintoTHB = 0.00;
            if (advanceList != null && advanceList.Count != 0)
            {
                ctlAdvanceGridView.DataSource = advanceList;
                ctlTotalAdvanceDiv.Visible = true;
            }
            else
            {
                ctlAdvanceGridView.DataSource = null;
                ctlTotalAdvanceDiv.Visible = false;
                ctlUpdatePanelGeneral.Update();
            }
            ctlAdvanceGridView.DataBind();
        }
        public void BindRemittanceItemGridview()
        {
            //this.SumRemittanceItemAmount = 0;
            //this.SumDivAmount = 0;
            FnRemittanceDataset remiitanceDS = (FnRemittanceDataset)TransactionService.GetDS(this.TransactionID);
            if (remiitanceDS != null)
            {
                FnRemittanceDataset.FnRemittanceRow fnremittanceRow = remiitanceDS.FnRemittance.FindByRemittanceID(this.DocumentID);
                if (!IsRepOffice)
                {
                    ctlTotalRemittanceAmountLabel.Text = UIHelper.BindDecimal(fnremittanceRow != null && !fnremittanceRow.IsTotalAmountNull() ? fnremittanceRow.TotalAmount.ToString() : "0");
                }
                else
                {
                    ctlTotalRemittanceAmountLabel.Text = UIHelper.BindDecimal(fnremittanceRow != null && !fnremittanceRow.IsMainCurrencyAmountNull() ? fnremittanceRow.MainCurrencyAmount.ToString() : "0");
                }
                if (remiitanceDS.FnRemittanceItem.Count != 0)
                {
                    ctlRemittanceItemGrid.DataSource = remiitanceDS.FnRemittanceItem.Select("RemittanceID =" + fnremittanceRow.RemittanceID, "RemittanceItemID DESC");
                    ctlTotalRemittanceDiv.Visible = true;
                }
            }
            else
            {
                ctlRemittanceItemGrid.DataSource = null;
                ctlTotalRemittanceDiv.Visible = false;
                ctlUpdatePanelGeneral.Update();
            }

            ctlRemittanceItemGrid.DataBind();
        }
        public void ClearFnRemittanceItemRow()
        {
            FnRemittanceDataset remittanceDS = (FnRemittanceDataset)TransactionService.GetDS(this.TransactionID);

            foreach (DataRow row in remittanceDS.FnRemittanceItem.Select())
            {
                row.Delete();
            }
        }
        public void ClearFnRemittanceAdvanceRow()
        {
            FnRemittanceDataset remittanceDS = (FnRemittanceDataset)TransactionService.GetDS(this.TransactionID);
            foreach (DataRow row in remittanceDS.FnRemittanceAdvance.Select())
            {
                row.Delete();
            }
        }
        public void AddRemittanceAdvanceToTransaction(Advance advance)
        {
            FnRemittanceAdvance remittanceAdvance = new FnRemittanceAdvance();
            remittanceAdvance.Remittance = new FnRemittance(this.DocumentID);
            remittanceAdvance.Advance = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindProxyByIdentity(advance.AdvanceID);
            remittanceAdvance.Active = true;

            FnRemittanceAdvanceService.AddFnRemittanceAdvanceTransaction(this.TransactionID, remittanceAdvance);
        }
        public void AddAdvanceToRemittanceItemByPaymentTypeAndCurrency(IList<Advance> advList)
        {
            foreach (Advance sumAdvance in advList)
            {
                FnRemittanceItem remittanceItem = new FnRemittanceItem();
                if (sumAdvance.CurrencyID != null)
                {
                    remittanceItem.Currency = new SS.DB.DTO.DbCurrency(sumAdvance.CurrencyID.Value);
                }
                remittanceItem.Remittance = new FnRemittance(this.DocumentID);
                remittanceItem.Remittance.PB = new Dbpb(this.PBID.Value);
                remittanceItem.PaymentType = sumAdvance.PaymentType;
                remittanceItem.ExchangeRate = sumAdvance.ExchangeRate.Value;
                remittanceItem.ForeignCurrencyAdvanced = sumAdvance.ForeignCurrencyAdvanced.Value;
                remittanceItem.AmountTHB = sumAdvance.RemittedAmountTHB.Value;
                remittanceItem.ForeignAmountTHBAdvanced = sumAdvance.RemittedAmountTHB.Value;
                if (sumAdvance.MainCurrencyAmount.HasValue)
                {
                    remittanceItem.MainCurrencyAmount = sumAdvance.MainCurrencyAmount.Value;
                    remittanceItem.ForeignAmountMainCurrencyAdvanced = sumAdvance.MainCurrencyAmount.Value;
                }
                remittanceItem.ExchangeRateTHB = SumExchangeRateMaintoTHB / AdvanceList.Count;
                FnRemittanceItemService.AddFnRemittanceItemTransaction(this.TransactionID, remittanceItem, true, this.SumAdvanceAmount);
            }
        }

        public bool CanChangeCompanyOrRequester()
        {
            FnRemittanceDataset remiitanceDS = (FnRemittanceDataset)TransactionService.GetDS(this.TransactionID);
            DataRow[] rmtAdvanceList = remiitanceDS.FnRemittanceAdvance.Select(string.Format("RemittanceID = '{0}'", this.DocumentID));
            if (rmtAdvanceList.Count() > 0)
            {
                return false;
            }
            return true;
        }

        public bool IsContainEditableFields(object editableFieldEnum)
        {
            return this.EditableFields.Contains(editableFieldEnum);
        }
        public bool IsContainVisibleFields(object visibleFieldEnum)
        {
            return this.VisibleFields.Contains(visibleFieldEnum);
        }
        public void ShowFullReturnChk()
        {
            double totalAdvanceAmountTHB = UIHelper.ParseDouble(ctlTotalAdvanceAmountLabel.Text);
            double totalRemittanceAmountTHB = UIHelper.ParseDouble(ctlTotalRemittanceAmountLabel.Text);
            if ((totalAdvanceAmountTHB != 0 && totalRemittanceAmountTHB != 0) && (totalAdvanceAmountTHB == totalRemittanceAmountTHB) || (this.SumDivAmount == 0))
            {
                ctlFullReturnCashChk.Style["display"] = "block";
            }
            else
            {
                ctlFullReturnCashChk.Style["display"] = "none";
                ctlFullReturnCashChk.Checked = false;
            }
            ctlFullReturnCashChk.Enabled = IsContainEditableFields(RemittanceFieldGroup.FullClearing);
        }
        private void ResetControlValue()
        {
            ctlRemittanceFormHeader.Status = string.Empty;
            ctlRemittanceFormHeader.No = string.Empty;
            ctlCompaymyField.ShowDefault();
            ctlSubject.Text = string.Empty;
            ctlCreatorData.ShowDefault();
            ctlRequesterData.ShowDefault();
            ctlMemo.Text = string.Empty;

            this.TADocumentID = string.Empty;
            ctlTANo.Text = "N/A";
            ctlTANoLookup.Enabled = true;
            ctlDeleteTA.Enabled = true;
            ctlAddAddvance.Enabled = true;
            ctlAttachment.ResetControlValue();

            ctlRemittanceItemGrid.DataSource = null;
            ctlRemittanceItemGrid.DataBind();

            this.BindCtlAdvanceGridview(null);

            ShowDefault();

            ctlTotalAdvanceDiv.Visible = false;
            ctlTotalRemittanceDiv.Visible = false;
            ctlFullReturnCashChk.Style["display"] = "none";

            ctlUpdatePanelHeader.Update();
            ctlUpdatePanelGeneral.Update();
            ctlUpdatePanelMemo.Update();
            ctlCounterCasheirUpdate.Update();
        }
        public void isVisibleControlViewMode(bool visible)
        {
            ctlTANoLookup.Visible = visible;
            ctlAddAddvance.Visible = visible;
            ctlDeleteTA.Visible = visible;
            ctlRemittanceItemGrid.Columns[7].Visible = visible;
            ctlAdvanceGridView.Columns[7].Visible = visible;
        }
        #endregion

        public void Copy(long wfid)
        {
            SS.Standard.WorkFlow.DTO.Document doc = WorkFlowQueryProvider.WorkFlowQuery.GetDocumentByWorkFlowID(wfid);
            if (doc.DocumentType.DocumentTypeID.Equals(DocumentTypeID.RemittanceDocument))
                Response.Redirect(String.Format("~/Forms/SCG.eAccounting/Programs/RemittanceForm.aspx?cp=1&docId={0}", doc.DocumentID));
        }

        public void EnabledViewPostButton(bool IsLock)
        {
            ctlViewPostForeign.Enabled = IsLock;
            if (isNotViewPost)
            {
                ctlUpdatePanelViewPost.Update();
            }
        }

        public void EnabledPostRemittanceButton(bool IsLock)
        {

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!Page.IsPostBack)
            {
                this.SumAdvanceAmount = 0;
                this.CountRow = 0;
                this.SumExchangeRateMaintoTHB = 0.00;
                ctlAdvanceGridView.DataBind();
                ctlRemittanceItemGrid.DataBind();

            }
        }

        private DsNullHandler dsNullHandler;
        event DsNullHandler IDocumentEditor.DsNull
        {
            add
            {
                dsNullHandler += value;
            }
            remove
            {
                dsNullHandler -= value;
            }
        }


        public void CheckUserRepOffice()
        {
            FnRemittanceDataset remittanceDS = (FnRemittanceDataset)TransactionService.GetDS(this.TransactionID);
            FnRemittanceDataset.FnRemittanceRow remittanceRow = remittanceDS.FnRemittance.FindByRemittanceID(this.DocumentID);

            if (InitialFlag.Equals(FlagEnum.NewFlag) && ((remittanceRow.IsIsRepOfficeNull() || !IsRepOffice)))
            {
                SuUser userList = QueryProvider.SuUserQuery.FindByIdentity(UIHelper.ParseLong(ctlRequesterData.UserID));
                if (userList != null && userList.Location != null)
                {
                    if (userList.Location.DefaultPBID.HasValue)
                    {
                        Dbpb pb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(userList.Location.DefaultPBID.Value);
                        IsRepOffice = remittanceRow.IsRepOffice = pb.RepOffice;
                    }
                }
                else
                {
                    IsRepOffice = remittanceRow.IsRepOffice = false;
                }
            }
            else
            {
                if (remittanceRow.IsIsRepOfficeNull())
                {
                    remittanceRow.IsRepOffice = false;
                }
                IsRepOffice = remittanceRow.IsRepOffice;
            }
        }
        public void getCurrencySymbol(short currencyid)
        {
            dbCurrency = SsDbQueryProvider.DbCurrencyQuery.FindByIdentity(currencyid);
        }
        protected void ctlCounterCashierDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            FnRemittanceDataset remittanceDS = (FnRemittanceDataset)TransactionService.GetDS(this.TransactionID);
            FnRemittanceDataset.FnRemittanceRow remittanceRow = remittanceDS.FnRemittance.FindByRemittanceID(this.DocumentID);

            Dbpb pb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(UIHelper.ParseLong(ctlCounterCashierDropdown.SelectedValue));

            this.IsRepOffice = remittanceRow.IsRepOffice = pb.RepOffice;
            this.PBID = pb.Pbid;

        }
        public void BindCounterCashierDropdown(long companyID)
        {
            IList<PaymentTypeListItem> list = this.BindDropdown(companyID);

            if (list.Count > 1)
            {
                ctlCounterCashierDropdown.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), "-1"));
            }
        }
        private IList<PaymentTypeListItem> BindDropdown(long companyID)
        {
            IList<PaymentTypeListItem> list = ScgDbQueryProvider.DbPBQuery.GetPbListItem(companyID, UserAccount.UserLanguageID);

            ctlCounterCashierDropdown.DataSource = list;
            ctlCounterCashierDropdown.DataTextField = "Text";
            ctlCounterCashierDropdown.DataValueField = "ID";
            ctlCounterCashierDropdown.DataBind();

            return list;
        }
        public void SetValue(long companyID, long pbID)
        {
            this.BindDropdown(companyID);

            ctlCounterCashierDropdown.SelectedValue = pbID.ToString();
        }
        public long? GetCashierId()
        {
            if (ctlCounterCashierDropdown.SelectedIndex != -1)
            {
                return Utilities.ParseLong(ctlCounterCashierDropdown.SelectedValue);
            }
            else
            {
                return null;
            }
        }

        public void SetDefaultPB()
        {
            if (!string.IsNullOrEmpty(ctlRequesterData.UserID))
            {
                SuUser user = QueryProvider.SuUserQuery.FindByIdentity(UIHelper.ParseLong(ctlRequesterData.UserID));
                if (user.Location != null && user.Location.DefaultPBID.HasValue)
                    if (ctlCounterCashierDropdown.Items.FindByValue(user.Location.DefaultPBID.Value.ToString()) != null)
                        ctlCounterCashierDropdown.SelectedValue = user.Location.DefaultPBID.Value.ToString();
            }
        }

        public bool RequireDocumentAttachment()
        {
            return false;
        }
    }
}
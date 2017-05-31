using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using SCG.eAccounting.BLL;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.DataSet;
using SCG.eAccounting.Web;
using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.Web.UserControls;
using SS.DB.DTO.ValueObject;
using SS.Standard.UI;
using SS.Standard.Utilities;
using SS.DB.Query;
using SCG.DB.Query;
using SCG.eAccounting.Query;
using SCG.DB.DTO;
using System.Text;
using SS.Standard.WorkFlow.Query;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor.Components
{
    public partial class Mileage : BaseUserControl, IEditorComponent
    {
        private const string InvoiceURL = "~/UserControls/DocumentEditor/Components/Invoice.aspx?mode=[mode]&txId={0}&expId={1}&docType={2}&invType={3}";

        #region service
        public ITransactionService TransactionService { get; set; }
        public IFnExpenseMileageService FnExpenseMileageService { get; set; }
        public IFnExpenseMileageItemService FnExpenseMileageItemService { get; set; }
        public IFnExpenseInvoiceService FnExpenseInvoiceService { get; set; }
        public IFnExpenseDocumentService FnExpenseDocumentService { get; set; }
        public ExpenseDocumentEditor ExpenseDocumentEditor { get; set; }
        //public IFnExpenseMileageInvoiceService FnExpenseMileageInvoiceService { get; set; }

        #endregion

        #region global variable && const
        private const string ASCENDING = " ASC";
        private const string DESCENDING = " DESC";
        private decimal TotalDistanceTotal { get; set; }
        private decimal TotalDistanceFirst100Km { get; set; }
        private decimal TotalAdjusted { get; set; }
        private decimal TotalNetAmount { get; set; }
        public string DocumentType
        {
            get
            {
                if (ViewState[this.ClientID + "_DocumentType"] == null)
                    return ZoneType.Domestic;
                return ViewState[this.ClientID + "_DocumentType"].ToString();
            }
            set { ViewState[this.ClientID + "_DocumentType"] = value; }
        }
        public long ExpDocumentID
        {
            get
            {
                if (ViewState[ViewStateName.DocumentID] == null)
                    return 0;
                return (long)ViewState[ViewStateName.DocumentID];
            }
            set { ViewState[ViewStateName.DocumentID] = value; }
        }
        public long? DocumentID
        {
            get
            {
                if (ViewState[this.ClientID + "_DocumentID"] == null)
                    return null;
                return (long)ViewState[this.ClientID + "_DocumentID"];
            }
            set { ViewState[this.ClientID + "_DocumentID"] = value; }
        }
        public SortDirection GridViewSortDirection
        {
            get
            {
                if (ViewState["sortDirection"] == null)
                    ViewState["sortDirection"] = SortDirection.Ascending;

                return (SortDirection)ViewState["sortDirection"];
            }
            set { ViewState["sortDirection"] = value; }
        }

        public Guid TxId
        {
            get { return (Guid)ViewState[ViewStateName.TransactionID]; }
            set { ViewState[ViewStateName.TransactionID] = value; }
        }
        public Guid ParentTxId
        {
            get { return (Guid)ViewState[ViewStateName.ParentTxID]; }
            set { ViewState[ViewStateName.ParentTxID] = value; }
        }

        public long ExpenseMileageId
        {
            get { return (long)ViewState["ExpenseMileageId"]; }
            set { ViewState["ExpenseMileageId"] = value; }
        }
        public long InvoiceId
        {
            get { return (long)ViewState["InvoiceId"]; }
            set { ViewState["InvoiceId"] = value; }
        }
        public string Mode
        {
            get
            {
                if (ViewState["MileageMode"] != null)
                    return ViewState["MileageMode"].ToString();
                return string.Empty;
            }
            set { ViewState["MileageMode"] = value; }
        }
        #endregion

        #region IEditorComponents Members (Initialize)
        /// <summary>
        ///	First initialize when parent control are initialize.
        ///	Use for Label Extender only.
        /// </summary>
        /// <param name="txID"></param>
        /// <param name="documentID"></param>
        /// <param name="initFlag"></param>
        public void Initialize(Guid txID, long documentID, string initFlag)
        {

        }
        #endregion

        #region load method
        protected void Page_Load(object sender, EventArgs e)
        {
            ctlCostCenterField.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlCostCenterField_OnObjectLookUpReturn);
            ctlCADocumentLookup.OnObjectLookUpCalling += new BaseUserControl.ObjectLookUpCallingEventHandler(ExpensesCA_OnObjectLookUpCalling);
            ctlCADocumentLookup.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ExpensesCA_OnObjectLookUpReturn);
        }
        #endregion
        public void ctlCostCenterField_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            if (e.ObjectReturn != null)
            {
                DbCostCenter cost = e.ObjectReturn as DbCostCenter;
                DbCostCenter dbCostcenter = ScgDbQueryProvider.DbCostCenterQuery.FindByIdentity(cost.CostCenterID);
                ctlIOAutoCompleteLookup.CostCenterId = dbCostcenter.CostCenterID;

            }
            ctlUpdatePanelMileageData.Update();
        }

        protected void ExpensesCA_OnObjectLookUpCalling(object sender, ObjectLookUpCallingEventArgs e)
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
            ExpenseDataSet.FnExpenseDocumentRow expenseDocumentRow = expDS.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);
            long requesterID = expenseDocumentRow.DocumentRow.RequesterID;
            UserControls.LOV.SCG.eAccounting.CALookup expensesCALookUp = sender as UserControls.LOV.SCG.eAccounting.CALookup;
            expensesCALookUp.CompanyID = expenseDocumentRow.DocumentRow.CompanyID;
            expensesCALookUp.RequesterID = expenseDocumentRow.DocumentRow.RequesterID;
            //expensesMPALookUp.CurrentUserID = expenseDocumentRow.DocumentRow.RequesterID;
        }

        protected void ExpensesCA_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            if (e.ObjectReturn != null)
            {
                IList<SCG.eAccounting.DTO.ValueObject.ExpenseCA> list = (IList<SCG.eAccounting.DTO.ValueObject.ExpenseCA>)e.ObjectReturn;
                FnExpenseDocumentService.AddExpenseCAToTransaction(this.TxId, this.ExpDocumentID, list);
                this.BindExpenseCAGridView();
            }
            ctlUpdatePanelExpenseGeneral.Update();
        }

        public void BindExpenseCAGridView()
        {
            ExpenseDataSet expenseDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
            ExpenseDataSet.FnExpenseCARow[] rows = (ExpenseDataSet.FnExpenseCARow[])expenseDS.FnExpenseCA.Select();

            List<long> expenseCAIdList = new List<long>();
            foreach (ExpenseDataSet.FnExpenseCARow row in rows)
            {
                // Prepare list of advance id for query
                expenseCAIdList.Add(row.CADocumentID);
            }

            if (expenseCAIdList.Count > 0)
            {
                ctlExpeseCAGridView.DataSource = ScgeAccountingQueryProvider.CADocumentQuery.FindByExpenseCAID(expenseCAIdList);
                ctlExpeseCAGridView.DataBind();
            }
            else
            {
                ctlExpeseCAGridView.DataSource = null;
                ctlExpeseCAGridView.DataBind();
            }
            ctlUpdatePanelExpenseGeneral.Update();
        }

        protected void ctlctlAddExpenseCA_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("DeleteExpensesCA"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                long mpaDocumentID = UIHelper.ParseLong(ctlExpeseCAGridView.DataKeys[rowIndex]["CADocumentID"].ToString());
                FnExpenseDocumentService.DeleteExpenseCAFromTransaction(this.TxId, mpaDocumentID);
                this.BindExpenseCAGridView();
                //this.ExpenseDocumentEditor.NotifyPaymentDetailChange();
            }
            else if (e.CommandName.Equals("PopupDocument"))
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                long workflowID = UIHelper.ParseLong(ctlExpeseCAGridView.DataKeys[rowIndex]["WorkflowID"].ToString());
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), string.Empty, "window.open('" + ResolveUrl("~/Forms/SCG.eAccounting/Programs/DocumentView.aspx?wfid=") + workflowID.ToString() + "')", true);
            }
        }

        protected void ctlExpeseCAGridView_DataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal ctlNoLabel = e.Row.FindControl("ctlNoLabel") as Literal;

                ctlNoLabel.Text = ((ctlExpeseCAGridView.PageSize * ctlExpeseCAGridView.CustomPageIndex) + (e.Row.RowIndex + 1)).ToString();
                if (Mode.Equals(FlagEnum.ViewFlag))
                {
                    ctlExpeseCAGridView.Columns[4].Visible = false;
                    ctlAddExpenseMPA.Visible = false;
                }
            }
        }

        protected void ctlAddExpenseCA_Click(object sender, EventArgs e)
        {
            //if (!ExpenseDocumentEditor.NotifyUpdateExpense()) return;
            ctlCADocumentLookup.Show();

        }

        #region dropdownlist method
        protected void ctlDropDownListOwner_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindTypeOfCar();
            SetMileageOwnerMode();
            ClearInvoiceMileage();
            BindMileageInvoiceGrid();
            SetDefaultCostCenterExpenseCodeIO();
        }
        protected void ctlDropDownListTypeOfCar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ctlDropDownListOwner.SelectedValue == "2" && ctlDropDownListTypeOfCar.SelectedValue == "3")
            {
                ctlPanelGridInvoice.Visible = true;
                ctlDeclareVAT.Enabled = true;       //if type of car = pickup
                ctlPanelCostcenterExpenseCode.Style["display"] = "none";
            }
            else
            {
                ctlPanelCostcenterExpenseCode.Style["display"] = "block";
                ctlPanelGridInvoice.Visible = false;
                ctlDeclareVAT.Enabled = false;
                ClearInvoiceMileage();
            }
            if (ctlDropDownListTypeOfCar.SelectedValue == "2")
            {
                ctlLabelKmRate.Text = GetProgramMessage("KmRate30Km");
                ctlLabelExceedRate.Text = GetProgramMessage("ExceedRate30Km");
                ctlDistanceFirst100Km.Text = GetProgramMessage("First30Km");
                ctlDistanceExceed100Km.Text = GetProgramMessage("Exceed30Km");
                ctlLabelff.Text = string.Format(GetProgramMessage("LessMileage"), ParameterServices.MotorcycleRateForMileageCalculation);
            }
            else
            {
                ctlLabelKmRate.Text = GetProgramMessage("KmRate100Km");
                ctlLabelExceedRate.Text = GetProgramMessage("ExceedRate100Km");
                ctlDistanceFirst100Km.Text = GetProgramMessage("First100Km");
                ctlDistanceExceed100Km.Text = GetProgramMessage("Exceed100Km");
                ctlLabelff.Text = string.Format(GetProgramMessage("LessMileage"), ParameterServices.OtherRateForMileageCalculation);
            }
            BindGrid();
            BindMileageInvoiceGrid();
            ctlUpdatePanelCalculate.Update();
            ctlUpdatePanelGridInvoice.Update();
        }
        #endregion

        #region public method
        public void Initialize(string mode, long? lineItemID, Guid txID)
        {
            this.ParentTxId = txID;
            this.TxId = TransactionService.Begin(ParentTxId);
            this.Mode = mode;

            switch (mode)
            {
                case FlagEnum.NewFlag:
                    FnExpenseMileage expenseMileage = new FnExpenseMileage();
                    expenseMileage.Expense = new FnExpenseDocument(this.ExpDocumentID);
                    this.ExpenseMileageId = FnExpenseMileageService.AddBeginRowExpenseMileageOnTransaction(expenseMileage, this.TxId);
                    break;
                case FlagEnum.EditFlag:
                    this.ExpenseMileageId = lineItemID.Value;
                    break;
                case FlagEnum.ViewFlag:
                    this.ExpenseMileageId = lineItemID.Value;
                    break;
                default:
                    break;
            }
            BeginSetData();
            if (ctlDropDownListTypeOfCar.SelectedValue == "2")
            {
                ctlLabelKmRate.Text = GetProgramMessage("KmRate30Km");
                ctlLabelExceedRate.Text = GetProgramMessage("ExceedRate30Km");
                ctlDistanceFirst100Km.Text = GetProgramMessage("First30Km");
                ctlDistanceExceed100Km.Text = GetProgramMessage("Exceed30Km");
                ctlLabelff.Text = string.Format(GetProgramMessage("LessMileage"), ParameterServices.MotorcycleRateForMileageCalculation);

            }
            else
            {
                ctlLabelKmRate.Text = GetProgramMessage("KmRate100Km");
                ctlLabelExceedRate.Text = GetProgramMessage("ExceedRate100Km");
                ctlDistanceFirst100Km.Text = GetProgramMessage("First100Km");
                ctlDistanceExceed100Km.Text = GetProgramMessage("Exceed100Km");
                ctlLabelff.Text = string.Format(GetProgramMessage("LessMileage"), ParameterServices.OtherRateForMileageCalculation);
            }

            this.BindExpenseCAGridView();
            BindMileageInvoiceGrid();
            ctlUpdatePanelCalculate.Update();
            ctlUpdatePanelGridInvoice.Update();

            if (Convert.ToString(ParameterServices.EnableCA).ToLower() != "true")
            {
                ctlCAField.Visible = false;
            }
        }
        public void Show()
        {
            CallOnObjectLookUpCalling();

            //this.ModalPopupExtender1.Show();

            this.ctlUpdatePanelMileageData.Update();
            this.ctlUpdatePanelMileageSubData.Update();
            this.ctlUpdatePanelMileageItemData.Update();
            this.ctlUpdatePanelCalculate.Update();
            this.ctlUpdatePanelGridView.Update();
            this.ctlUpdatePanelButtonControl.Update();
            this.ctlUpdatePanelBottom.Update();
            this.ctlUpdatePanelGridInvoice.Update();
        }
        public void Hide(string hideFlag)
        {
            //this.ModalPopupExtender1.Hide();

            this.ctlUpdatePanelMileageData.Update();
            this.ctlUpdatePanelMileageSubData.Update();
            this.ctlUpdatePanelMileageItemData.Update();
            this.ctlUpdatePanelCalculate.Update();
            this.ctlUpdatePanelGridView.Update();
            this.ctlUpdatePanelButtonControl.Update();
            this.ctlUpdatePanelBottom.Update();
            this.ctlUpdatePanelGridInvoice.Update();

            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Close", string.Format("notifyPopupResult('{0}');", hideFlag), true);
        }
        #endregion

        #region usercontrol method
        protected void ctlInvoiceForm_NotifyPopup(object sender, PopUpReturnArgs args)
        {
            if (args.Type == PopUpReturnType.OK)
            {
                BindMileageInvoiceGrid();
                CalculateForCompany();
                ctlUpdatePanelGridInvoice.Update();
            }
        }
        #endregion

        #region button method
        protected void ctlDeclareVAT_Click(object sender, ImageClickEventArgs e)
        {
            //FnExpenseInvoiceService.DeleteMileageInvoice(TxId);
            ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
            ExpenseDataSet.FnExpenseDocumentRow expenseRow = expDs.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);

            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "ShowPopup", ctlDeclareVATPopupCaller.PopupScript, true);
            //ShowInvoicePopup(FlagEnum.NewFlag, null);
        }
        protected void ctlCalculateForEmployee_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                UpdateMileageRate();
                CalculateForEmployee();
                BindGrid();
                this.OnObjectIsHide(false);
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                ctlUpdatePanelBottom.Update();
            }
        }
        protected void ctlCalculateForCompany_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                UpdateMileageRate();
                CalculateForCompany();
                BindGrid();
                this.OnObjectIsHide(false);
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                ctlUpdatePanelBottom.Update();
            }
        }
        protected void ctlAddCommit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
                ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(TxId);

                string filter = String.Format("ExpenseMileageID = {0}", this.ExpenseMileageId);
                DataRow[] rows = expDS.FnExpenseMileageItem.Select(filter);

                if (rows.Length == 0)
                {
                    errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("CannotSaveMileage"));
                    throw new ServiceValidationException(errors);
                }

                if (ctlDropDownListOwner.SelectedValue == "1")
                {
                    UpdateMileageRate();
                    CalculateForEmployee();
                }
                else
                {
                    UpdateMileageRate();
                    CalculateForCompany();
                }

                if (Convert.ToString(ParameterServices.EnableCA).ToLower() == "true")
                {
                    if (expDS.FnExpenseCA.Count() == 0)
                    {
                        errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("Attach CarAuthorization Document"));
                        throw new ServiceValidationException(errors);
                    }

                }

                bool isCopy;
                if (Request.QueryString["cp"] != null && UIHelper.ParseLong(Request.QueryString["cp"]) == 1)
                {
                    isCopy = true;
                }
                else
                {
                    isCopy = false;
                }
                FnExpenseMileage mileage = new FnExpenseMileage(this.ExpenseMileageId);
                //ValidationMileageRateSave();
                //Validation  Mileage Rate override
                if (ctlDropDownListOwner.SelectedValue != "2")
                {
                    if (ctlSelectChangeMileageRate.Checked == true)
                    {
                        if (String.IsNullOrEmpty(ctlCompanyField.CompanyCode))
                        {
                            errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("OverrideCompanyIdIsRequare"));
                            throw new ServiceValidationException(errors);
                        }

                        if (String.IsNullOrEmpty(ctlMileagRateRivitionDropDownList.SelectedValue))
                        {
                            errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("RequiredOverrideLevelRemark"));
                            throw new ServiceValidationException(errors);
                        }

                        if (String.IsNullOrEmpty(ctlObjectiveValue.Text))
                        {
                            errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("RequiredOverrideUserPersonalLevelCode"));
                            throw new ServiceValidationException(errors);
                        }
                    }
                }/*check owner not company*/
                SetExpenseMileageObj(mileage);
                FnExpenseMileageService.UpdateExpenseMileageOnTransaction(mileage, TxId);
                FnExpenseMileageItemService.SaveExpenseValidationMileageItemOnTransaction(TxId, this.ExpDocumentID, "Mileage", isCopy);

                TransactionService.Commit(TxId);
                //NotifyPopUpReturn(new PopUpReturnArgs(PopUpReturnType.OK, null));
                Hide("ok");
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        protected void ctlEditCommit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
                ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(TxId);
                ExpenseDataSet.FnExpenseDocumentRow expId = expDS.FnExpenseDocument.FirstOrDefault();

                string filter = String.Format("ExpenseMileageID = {0}", this.ExpenseMileageId);
                DataRow[] rows = expDS.FnExpenseMileageItem.Select(filter);

                if (rows.Length == 0)
                {
                    errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("CannotSaveMileage"));
                    throw new ServiceValidationException(errors);
                }

                if (ctlDropDownListOwner.SelectedValue == "1")
                {
                    UpdateMileageRate();
                    CalculateForEmployee();
                }
                else
                {
                    UpdateMileageRate();
                    CalculateForCompany();
                }

                bool isCopy;
                if (Request.QueryString["cp"] != null && UIHelper.ParseLong(Request.QueryString["cp"]) == 1)
                {
                    isCopy = true;
                }
                else
                {
                    isCopy = false;
                }
                if (ctlDropDownListOwner.SelectedValue != "2")
                {
                    if (ctlSelectChangeMileageRate.Checked == true)
                    {
                        if (ctlCompanyField == null)
                        {
                            errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("OverrideCompanyIdIsRequare"));
                            throw new ServiceValidationException(errors);
                        }

                        if (String.IsNullOrEmpty(ctlMileagRateRivitionDropDownList.SelectedValue))
                        {
                            errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("RequiredOverrideUserPersonalLevelCode"));
                            throw new ServiceValidationException(errors);
                        }

                        if (String.IsNullOrEmpty(ctlObjectiveValue.Text))
                        {
                            errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("RequiredOverrideLevelRemark"));
                            throw new ServiceValidationException(errors);
                        }
                    }
                } /*check owner not company*/
                FnExpenseMileage mileage = new FnExpenseMileage(this.ExpenseMileageId);
                SetExpenseMileageObj(mileage);
                FnExpenseMileageItemService.SaveExpenseValidationMileageItemOnTransaction(TxId, expId.ExpenseID, "Mileage", isCopy);
                FnExpenseMileageService.UpdateExpenseMileageOnTransaction(mileage, TxId);

                TransactionService.Commit(TxId);
                Hide("ok");
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        protected void ctlPopupCancel_Click(object sender, ImageClickEventArgs e)
        {
            TransactionService.Rollback(TxId);
            //NotifyPopUpReturn(new PopUpReturnArgs(PopUpReturnType.Cancel, null));
            OnObjectIsHide(true);
            Hide("cancel");
        }
        protected void ctlViewClose_Click(object sender, ImageClickEventArgs e)
        {
            OnObjectIsHide(true);
            Hide("cancel");
        }
        protected void ctlAddItem_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                FnExpenseMileage mileage = new FnExpenseMileage(this.ExpenseMileageId);
                SetExpenseMileageObj(mileage);

                FnExpenseMileageItem item = new FnExpenseMileageItem();
                SetExpenseMileageItemObj(item,false);
                item.ExpenseMileage = mileage;
                FnExpenseMileageItemService.AddExpenseValidationMileageItemOnTransaction(item, TxId, this.ExpDocumentID);
                FnExpenseMileageItemService.AddExpenseMileageItemOnTransaction(item, TxId);
                BindGrid();
                CalculateForCompany();
                CalculateForEmployee();
                ClearMileageItemData();
                this.ctlUpdatePanelMileageSubData.Update();
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        protected void ctlUpdateItem_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                FnExpenseMileage mileage = new FnExpenseMileage(this.ExpenseMileageId);
                SetExpenseMileageObj(mileage);

                FnExpenseMileageItem item = new FnExpenseMileageItem();
                item.ExpenseMileageItemID = UIHelper.ParseLong(ctlHiddenFieldMileageItemId.Value);
                SetExpenseMileageItemObj(item,true);
                item.ExpenseMileage = mileage;

                FnExpenseMileageItemService.AddExpenseValidationMileageItemOnTransaction(item, TxId, this.ExpDocumentID);
                FnExpenseMileageItemService.UpdateExpenseMileageItemOnTransaction(item, TxId);
                BindGrid();
                CalculateForEmployee();
                CalculateForCompany();
                ClearMileageItemData();
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        protected void ctlCancelItem_Click(object sender, ImageClickEventArgs e)
        {
            ClearMileageItemData();
        }
        protected void ctlDelete_Click(object sender, ImageClickEventArgs e)
        {
            FnExpenseMileageService.DeleteMileage(this.TxId, this.ExpenseMileageId);
            TransactionService.Commit(TxId);
            //NotifyPopUpReturn(new PopUpReturnArgs(PopUpReturnType.OK, null));
            Hide("ok");
        }
        #endregion

        #region gridview method
        #region mileage grid
        private void BindGrid()
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
            DataView view = new DataView(expDS.FnExpenseMileageItem);

            view.RowFilter = "ExpenseMileageID=" + this.ExpenseMileageId;
            view.Sort = "TravelDate asc";

            ctlMileageGrid.DataSource = view;
            ctlMileageGrid.DataBind();

            //CalculateForEmployee();
            //CalculateForCompany();
        }
        private void SortGridView(string sortExpression, string direction)
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
            DataView view = new DataView(expDS.FnExpenseMileageItem);

            view.RowFilter = "ExpenseMileageID=" + this.ExpenseMileageId;
            view.Sort = sortExpression + direction;

            ctlMileageGrid.DataSource = view;
            ctlMileageGrid.DataBind();
        }
        protected void ctlMileageGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Sort"))
            {
                string sortExpression = e.CommandArgument.ToString();

                if (this.GridViewSortDirection == SortDirection.Ascending)
                {
                    this.GridViewSortDirection = SortDirection.Descending;
                    SortGridView(sortExpression, DESCENDING);
                }
                else
                {
                    this.GridViewSortDirection = SortDirection.Ascending;
                    SortGridView(sortExpression, ASCENDING);
                }
            }
            else if (e.CommandName.Equals("EditItem"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                long expenseMileageItemId = UIHelper.ParseLong(ctlMileageGrid.DataKeys[rowIndex].Value.ToString());

                ctlHiddenFieldMileageItemId.Value = expenseMileageItemId.ToString();
                ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
                ExpenseDataSet.FnExpenseMileageItemRow row = expDS.FnExpenseMileageItem.FindByExpenseMileageItemID(expenseMileageItemId);

                ctlCalendarDate.DateValue = UIHelper.ToDateString(row.TravelDate);
                ctlTextBoxLocationFrom.Text = row.LocationFrom.ToString();
                ctlTextBoxLocationTo.Text = row.LocationTo.ToString();
                ctlTextBoxCarMeterStart.Text = row.CarMeterStart.ToString();
                ctlTextBoxCarMeterEnd.Text = row.CarMeterEnd.ToString();
                if (ctlDropDownListOwner.SelectedValue != "1")
                {
                    ctlTextBoxAdjust.Text = UIHelper.BindDecimal(row.DistanceAdjust.ToString());
                }
                else
                {
                    ctlTextBoxAdjust.Text = string.Empty;
                }
                ChangeModeButtonItem(false);

                BindGrid();
            }
            else if (e.CommandName.Equals("DeleteItem"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                long expenseMileageItemId = UIHelper.ParseLong(ctlMileageGrid.DataKeys[rowIndex].Value.ToString());
                FnExpenseMileageItemService.DeleteExpenseMileageItemOnTransaction(TxId, expenseMileageItemId);

                FnExpenseMileage mileage = new FnExpenseMileage(this.ExpenseMileageId);
                SetExpenseMileageObj(mileage);
                FnExpenseMileageService.UpdateMileageSummary(this.TxId, mileage);
                ClearMileageItemData();

                BindGrid();
            }
        }
        protected void ctlMileageGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal lblItemDistanceTotal = e.Row.FindControl("ctlLabelItemDistanceTotalForEmployee") as Literal;
                Literal lblItemDistanceFirst100Km = e.Row.FindControl("ctlLabelItemDistanceFirst100KmForEmployee") as Literal;
                Literal lblItemAdjusted = e.Row.FindControl("ctlLabelItemAdjustedForCompany") as Literal;

                TotalDistanceTotal += UIHelper.ParseDecimal(lblItemDistanceTotal.Text);
                TotalDistanceFirst100Km += UIHelper.ParseDecimal(lblItemDistanceFirst100Km.Text);
                TotalAdjusted += UIHelper.ParseDecimal(lblItemAdjusted.Text);

                ImageButton editBtn = e.Row.FindControl("ctlEdit") as ImageButton;
                ImageButton deleteBtn = e.Row.FindControl("ctlDelete") as ImageButton;
                ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
                ExpenseDataSet.FnExpenseDocumentRow expenseDocumentRow = expDS.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);
                SS.Standard.WorkFlow.DTO.WorkFlow workflow = SS.Standard.WorkFlow.Query.WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(expenseDocumentRow.DocumentRow.DocumentID);
                if (workflow != null)
                {
                    SS.Standard.WorkFlow.DTO.WorkFlow wf = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workflow.WorkFlowID);
                    if (wf != null && wf.CurrentState != null && wf.CurrentState.Name != null)
                    {
                        if (wf.CurrentState.Name.Equals(WorkFlowStateFlag.Hold))
                        {
                            deleteBtn.Visible = false;
                        }
                        else deleteBtn.Visible = true;
                    }
                    else deleteBtn.Visible = true;
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                Literal lblFooterDistanceTotalForEmployee = e.Row.FindControl("ctlLabelFooterDistanceTotalForEmployee") as Literal;
                Literal lblFooterDistanceFirst100Km = e.Row.FindControl("ctlLabelFooterDistanceFirst100KmForEmployee") as Literal;
                Literal lblFooterDistanceExceed100Km = e.Row.FindControl("ctlLabelFooterDistanceExceed100KmForEmployee") as Literal;

                Literal lblFooterDistanceTotalForCompany = e.Row.FindControl("ctlLabelFooterDistanceTotalForCompany") as Literal;
                Literal lblFooterAdjust = e.Row.FindControl("ctlLabelFooterAdjustedForCompany") as Literal;
                Literal lblFooterNet = e.Row.FindControl("ctlLabelFooterNetForCompany") as Literal;

                lblFooterDistanceTotalForEmployee.Text = TotalDistanceTotal.ToString("#,##0");
                lblFooterDistanceFirst100Km.Text = TotalDistanceFirst100Km.ToString("#,##0");
                lblFooterDistanceExceed100Km.Text = (TotalDistanceTotal - TotalDistanceFirst100Km).ToString("#,##0");

                lblFooterDistanceTotalForCompany.Text = TotalDistanceTotal.ToString("#,##0");
                lblFooterAdjust.Text = TotalAdjusted.ToString("#,##0");
                lblFooterNet.Text = (TotalDistanceTotal - TotalAdjusted).ToString("#,##0");
            }
        }
        #endregion

        #region invoice grid
        private void ClearInvoiceMileage()
        {
            if ((ctlDropDownListOwner.SelectedValue == "2") && (ctlDropDownListTypeOfCar.SelectedValue != "3"))
            {
                //Clear Invoice if change dropdowm owner employee to company
                FnExpenseInvoiceService.DeleteMileageInvoice(TxId);
            }
        }
        private void BindMileageInvoiceGrid()
        {
            // กรณีที่เป็น company && pickup 
            if ((ctlDropDownListOwner.SelectedValue == "2") && (ctlDropDownListTypeOfCar.SelectedValue == "3"))
            {
                ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
                DataView view = new DataView(expDS.FnExpenseInvoice);

                view.RowFilter = "InvoiceDocumentType='" + InvoiceType.Mileage + "'";
                TotalNetAmount = 0;
                ctlInvoiceGrid.DataSource = view;
                ctlInvoiceGrid.DataBind();

                ctlUpdatePanelGridInvoice.Update();
            }

            ctlDeclareVATPopupCaller.URL = string.Format(InvoiceURL, this.TxId, this.ExpDocumentID, this.DocumentType, InvoiceType.Mileage).Replace("[mode]", FlagEnum.NewFlag);
        }
        protected void ctlInvoiceGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("DeleteItem"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                long expenseInvoiceId = UIHelper.ParseLong(ctlInvoiceGrid.DataKeys[rowIndex].Value.ToString());
                FnExpenseInvoiceService.DeleteInvoiceOnTransaction(expenseInvoiceId, TxId);

                BindMileageInvoiceGrid();
                CalculateForCompany();
            }
            else if (e.CommandName.Equals("EditItem"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                PopupCaller ctlEditInvoicePopup = ctlInvoiceGrid.Rows[rowIndex].FindControl("ctlEditInvoicePopup") as PopupCaller;
                long expenseInvoiceId = UIHelper.ParseLong(ctlInvoiceGrid.DataKeys[rowIndex].Value.ToString());

                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "ShowPopupEdit", ctlEditInvoicePopup.PopupScript, true);
            }
        }
        protected void ctlInvoiceGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal lblTotalNetAmount = e.Row.FindControl("ctlNetAmountInvoiceGridItem") as Literal;
                TotalNetAmount += UIHelper.ParseDecimal(lblTotalNetAmount.Text);

                long invoiceID = UIHelper.ParseLong(ctlInvoiceGrid.DataKeys[e.Row.RowIndex].Value.ToString());

                PopupCaller popup = e.Row.FindControl("ctlEditInvoicePopup") as PopupCaller;
                string url = String.Format(string.Concat(InvoiceURL, "&invId={4}&docId={5}"), this.TxId, this.ExpDocumentID, this.DocumentType, InvoiceType.Mileage, invoiceID, this.DocumentID);
                popup.URL = url.Replace("[mode]", FlagEnum.EditFlag);
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                Literal lblFooterNetAmount = e.Row.FindControl("ctlNetAmountInvoiceGridFooter") as Literal;
                lblFooterNetAmount.Text = TotalNetAmount.ToString("#,##0.00");
            }
        }
        #endregion
        #endregion

        #region private method
        private void SetControlByMode()
        {
            switch (Mode)
            {
                case FlagEnum.NewFlag:
                    ctlEditCommit.Visible = false;
                    ctlViewClose.Visible = false;
                    ctlDelete.Visible = false;
                    break;
                case FlagEnum.EditFlag:
                    ctlAddCommit.Visible = false;
                    ctlViewClose.Visible = false;
                    ctlDelete.Visible = true;
                    break;
                case FlagEnum.ViewFlag:
                    ctlDropDownListOwner.Enabled = false;
                    ctlDropDownListTypeOfCar.Enabled = false;
                    ctlTextBoxCarLicenseNo.ReadOnly = true;
                    ctlTextBoxPermissionNo.ReadOnly = true;
                    //ctlTextBoxHomeToOfficeRoundTrip.ReadOnly = true;
                    //ctlTextBoxPrivateUse.ReadOnly = true;
                    ctlTextBoxKmRate.ReadOnly = true;
                    ctlTextBoxExceedingRate.ReadOnly = true;
                    ctlTextBoxTotalAmount.ReadOnly = true;

                    ctlPanelMileageItemData.Visible = false;
                    ctlMileageGrid.Columns[6].Visible = false;
                    ctlCalculateForEmployee.Visible = false;
                    ctlCalculateForCompany.Visible = false;
                    ctlDeclareVAT.Visible = false;
                    ctlInvoiceGrid.Columns[2].Visible = false;
                    ctlAddCommit.Visible = false;
                    ctlEditCommit.Visible = false;
                    ctlViewClose.Visible = false;
                    ctlDelete.Visible = false;
                    break;
                default:
                    break;
            }
        }
        private void BeginSetData()
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
            ExpenseDataSet.FnExpenseMileageRow row = expDS.FnExpenseMileage.FindByExpenseMileageID(this.ExpenseMileageId);

            SetDefaultMileageData();

            if (ctlSelectChangeMileageRate.Checked != true)
            {
                reqCompany.Visible = false;
                reqPersonalLevel.Visible = false;
                reqObjective.Visible = false;
            }

            if (row != null)
            {
                if (row.RowState == DataRowState.Unchanged || (!this.Mode.Equals(FlagEnum.NewFlag)))
                {
                    if (row.TypeOfCar.Equals(TypeOfCar.PrivateCar))
                    {
                        ctlDropDownListTypeOfCar.SelectedValue = "1";
                    }
                    else if (row.TypeOfCar.Equals(TypeOfCar.MotorCycle))
                    {
                        ctlDropDownListTypeOfCar.SelectedValue = "2";
                    }
                    else
                    {
                        ctlDropDownListTypeOfCar.SelectedValue = "3";
                    }

                    ctlTextBoxCarLicenseNo.Text = row.CarLicenseNo;
                    ctlTextBoxPermissionNo.Text = row.PermissionNo;
                    //ctlTextBoxHomeToOfficeRoundTrip.Text = row.HomeToOfficeRoundTrip.ToString();
                    //ctlTextBoxPrivateUse.Text = row.PrivateUse.ToString();

                    if (row.Owner == OwnerMileage.Employee)
                    {
                        ctlDropDownListOwner.SelectedValue = "1";
                        ctlTextBoxKmRate.Text = UIHelper.BindDecimal(row.First100KmRate.ToString());
                        ctlTextBoxExceedingRate.Text = UIHelper.BindDecimal(row.Exceed100KmRate.ToString());
                    }
                    else
                    {
                        ctlDropDownListOwner.SelectedValue = "2";
                        ctlTextBoxTotalAmount.Text = UIHelper.BindDecimal(row.TotalAmount.ToString());
                        ctlLabelReimBurseMentAmount.Text = UIHelper.BindDecimal(row.HelpingAmount.ToString());
                    }

                }
            }
            BindGrid();
            BindMileageInvoiceGrid();
            CalculateForEmployee();
            CalculateForCompany();
            SetMileageOwnerMode();
            SetControlByMode();
        }
        private void SetMileageOwnerMode()
        {
            if (ctlDropDownListOwner.SelectedValue == "1")
            {
                //Owner = Employee
                ctlPanelMileageSubDataForEmployee.Visible = true;
                //ctlPanelMileageSubDataForCompany.Visible = false;
                ctlPanelAdjustedForCompany.Visible = false;
                ctlFieldSetModeManage.Style["width"] = "700px";
                ctlMileageGrid.Columns[4].Visible = true;
                ctlMileageGrid.Columns[5].Visible = false;
                ctlPanelCalculateForEmployee.Visible = true;
                ctlPanelCalculateForCompany.Visible = false;
                ctlDeclareVAT.Visible = false;
                ctlCalculateForEmployee.Visible = true;
                ctlCalculateForCompany.Visible = false;
                ctlPanelGridInvoice.Visible = false;
                ctlPanelCostcenterExpenseCode.Style["display"] = "block";
                divEmployeeDetail.Visible = true;
                divMileageSubDataForCompany.Visible = false;
                ctlUpdatePanelEmployeeDetail.Update();
            }
            else
            {
                //Owner = Company
                ctlPanelMileageSubDataForEmployee.Visible = false;
                //ctlPanelMileageSubDataForCompany.Visible = true;
                ctlPanelAdjustedForCompany.Visible = true;
                ctlFieldSetModeManage.Style["width"] = "800px";
                ctlMileageGrid.Columns[4].Visible = false;
                ctlMileageGrid.Columns[5].Visible = true;
                ctlPanelCalculateForEmployee.Visible = false;
                ctlPanelCalculateForCompany.Visible = true;
                ctlDeclareVAT.Visible = true;
                if (ctlDropDownListTypeOfCar.SelectedValue == "3")
                {
                    ctlDeclareVAT.Enabled = true;
                    ctlPanelGridInvoice.Visible = true;
                    ctlPanelCostcenterExpenseCode.Style["display"] = "none";
                }
                else
                {
                    ctlPanelCostcenterExpenseCode.Style["display"] = "block";
                    ctlDeclareVAT.Enabled = false;
                    ctlPanelGridInvoice.Visible = false;
                }
                ctlCalculateForEmployee.Visible = false;
                ctlCalculateForCompany.Visible = true;
                divEmployeeDetail.Visible = false;
                divMileageSubDataForCompany.Visible = true;
                ctlUpdatePanelEmployeeDetail.Update();
            }
        }

        private void ChangeModeButtonItem(bool visible)
        {
            ctlAddItem.Visible = visible;
            ctlUpdateItem.Visible = !visible;
            ctlCancelItem.Visible = !visible;
        }
        private void SetDefaultMileageData()
        {
            #region clear properties
            ctlDropDownListOwner.Enabled = true;
            ctlDropDownListTypeOfCar.Enabled = true;
            ctlTextBoxCarLicenseNo.ReadOnly = false;
            ctlTextBoxPermissionNo.ReadOnly = false;
            //ctlTextBoxHomeToOfficeRoundTrip.ReadOnly = false;
            //ctlTextBoxPrivateUse.ReadOnly = false;
            ctlTextBoxKmRate.ReadOnly = false;
            ctlTextBoxExceedingRate.ReadOnly = false;
            ctlTextBoxTotalAmount.ReadOnly = false;

            ctlPanelMileageItemData.Visible = true;
            ctlMileageGrid.Columns[6].Visible = true;
            ctlCalculateForEmployee.Visible = true;
            ctlCalculateForCompany.Visible = true;
            ctlDeclareVAT.Visible = true;
            ctlInvoiceGrid.Columns[1].Visible = true;
            ctlAddCommit.Visible = true;
            ctlEditCommit.Visible = true;
            ctlViewClose.Visible = true;
            #endregion

            #region clear mileage item
            ClearMileageItemData();
            #endregion

            #region clear calculate
            ctlLabelCalculateDistanceFirst100Km.Text = string.Empty;
            ctlLabelCalculateFirst100KmRateForDistanceFirst100Km.Text = string.Empty;
            ctlLabelCalculateAmountForDistanceFirst100Km.Text = string.Empty;

            ctlLabelCalculateDistanceExceed.Text = string.Empty;
            ctlLabelCalculateExceedRateForDistanceExceed.Text = string.Empty;
            ctlLabelCalculateAmountForDistanceExceed.Text = string.Empty;

            ctlLabelCalculateTotalAmount.Text = string.Empty;
            ctlLabelCalculateAllowance.Text = string.Empty;
            ctlLabelCalculateOverAllowance.Text = string.Empty;

            ctlLabelCalculateTotalAmountSummary.Text = string.Empty;
            ctlLabelCalculateAllowanceSummary.Text = string.Empty;
            ctlLabelCalculateOverAllowanceSummary.Text = string.Empty;

            ctlLabelReimBurseMentAmount.Text = string.Empty;
            ctlLabelReimBurseMentAmountForInvoice.Text = string.Empty;
            ctlLabelRemaining.Text = string.Empty;
            #endregion

            #region clear mileage
            ctlTextBoxCarLicenseNo.Text = string.Empty;
            ctlTextBoxPermissionNo.Text = string.Empty;
            //ctlTextBoxHomeToOfficeRoundTrip.Text = string.Empty;
            //ctlTextBoxPrivateUse.Text = string.Empty;
            ctlTextBoxExceedingRate.Text = string.Empty;
            ctlTextBoxKmRate.Text = string.Empty;
            ctlTextBoxTotalAmount.Text = string.Empty;
            #endregion

            #region clear costcenter, expense code, internal order data
            ctlCostCenterField.ResetValue();
            ctlAccountField.ResetValue();
            ctlIOAutoCompleteLookup.ResetValue();
            #endregion

            #region set control data
            #region Owner
            IList<TranslatedListItem> translateList1 = new List<TranslatedListItem>();

            TranslatedListItem Owner1 = new TranslatedListItem();
            Owner1.ID = UIHelper.ParseShort("1");
            Owner1.Symbol = "Employee";
            translateList1.Add(Owner1);

            TranslatedListItem Owner2 = new TranslatedListItem();
            Owner2.ID = UIHelper.ParseShort("2");
            Owner2.Symbol = "Company";
            translateList1.Add(Owner2);

            ctlDropDownListOwner.DataSource = translateList1;
            ctlDropDownListOwner.DataTextField = "Symbol";
            ctlDropDownListOwner.DataValueField = "Id";
            ctlDropDownListOwner.DataBind();
            ctlDropDownListOwner.SelectedIndex = 0;


            ctlMileagRateRivitionDropDownList.DataSource = ScgDbQueryProvider.DbMileageRateRevisionDetailQuery.FindPositionLevel();
            ctlMileagRateRivitionDropDownList.DataTextField = "PersonalLevelGroupCode";
            ctlMileagRateRivitionDropDownList.DataValueField = "PersonalLevelGroupCode";
            ctlMileagRateRivitionDropDownList.DataBind();
            if (ctlMileagRateRivitionDropDownList.Items.Count > 0)
                ctlMileagRateRivitionDropDownList.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), String.Empty));
            ctlMileagRateRivitionDropDownList.SelectedIndex = 0;

            #endregion

            this.BindTypeOfCar();   //Type of Car
            this.SetDefaultCostCenterExpenseCodeIO();
            #endregion
        }

        private void SetDefaultCostCenterExpenseCodeIO()
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
            ExpenseDataSet.FnExpenseDocumentRow expenseDocumentRow = expDS.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);
            ExpenseDataSet.FnExpenseMileageRow mileageRow = expDS.FnExpenseMileage.FindByExpenseMileageID(this.ExpenseMileageId);
            long requesterID = expenseDocumentRow.DocumentRow.RequesterID;
            long? requesterCompanyID = null;

            SS.SU.DTO.SuUser requester = SS.SU.Query.QueryProvider.SuUserQuery.FindByIdentity(requesterID);
            if (requester != null && requester.Company != null)
                requesterCompanyID = requester.Company.CompanyID;

            ctlCostCenterField.CompanyId = expenseDocumentRow.DocumentRow.CompanyID;
            ctlAccountField.CompanyIDofDocument = expenseDocumentRow.DocumentRow.CompanyID;

            ctlAccountField.WithoutExpenseCode = ParameterServices.AccountMileageExtra + ","
                + ParameterServices.AccountPerdiem;

            ctlIOAutoCompleteLookup.CompanyId = expenseDocumentRow.DocumentRow.CompanyID;
            defaultcompanyId.Value = Convert.ToString(requester.Company.CompanyID);
            companyText.Text = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(requester.Company.CompanyID).CompanyName;
            ctlPersonalLevelGroup.Text = ScgDbQueryProvider.DbMileageRateRevisionDetailQuery.FindPositionLevelByCode(requester.PersonalLevel);
            ctlTextBoxExceedingRate.ReadOnly = true;
            ctlTextBoxKmRate.ReadOnly = true;


            if (Mode.Equals(FlagEnum.NewFlag))
            {
                overrideMileageRate.Visible = false;
                DivSelectPersonalLevelGroup.Visible = false;
                DivCompanyField.Visible = false;
                DivDropdownListPersonal.Visible = false;
                DivctlObjectiveValue.Visible = false;
                DivObjectiveValue.Visible = false;
                if (requester != null && requester.CostCenter != null && expenseDocumentRow.DocumentRow.CompanyID == requesterCompanyID)
                {
                    ctlCostCenterField.BindCostCenterControl(requester.CostCenter.CostCenterID);
                    ctlIOAutoCompleteLookup.CostCenterId = requester.CostCenter.CostCenterID;
                }

                SCG.DB.DTO.DbAccount account = null;
                if (ctlDropDownListOwner.SelectedValue == "1")
                {
                    account = ScgDbQueryProvider.DbAccountQuery.FindAccountByAccountCodeExpenseGroup(ParameterServices.AccountMileageGov, null);
                }
                else if (ctlDropDownListOwner.SelectedValue == "2" && ctlDropDownListTypeOfCar.SelectedValue != "3")
                {
                    account = ScgDbQueryProvider.DbAccountQuery.FindAccountByAccountCodeExpenseGroup(ParameterServices.AccountMileageCompany, null);
                }
                if (account != null)
                {
                    ctlAccountField.BindAccountControl(account.AccountID);
                }
            }
            else if (Mode.Equals(FlagEnum.EditFlag) || Mode.Equals(FlagEnum.ViewFlag))
            {
                if (!mileageRow.IsCostCenterIDNull())
                    ctlCostCenterField.BindCostCenterControl(mileageRow.CostCenterID);
                if (!mileageRow.IsAccountIDNull())
                    ctlAccountField.BindAccountControl(mileageRow.AccountID);
                if (!mileageRow.IsIOIDNull())
                    ctlIOAutoCompleteLookup.BindIOControl(mileageRow.IOID);

                ctlSelectChangeMileageRate.Checked = mileageRow.IsOverrideLevel;
                if (ctlSelectChangeMileageRate.Checked == true)
                {
                    overrideMileageRate.Visible = true;
                    DivCompanyField.Visible = true;
                    DivDropdownListPersonal.Visible = true;
                    DivctlObjectiveValue.Visible = true;
                    DivSelectPersonalLevelGroup.Visible = true;
                    DivObjectiveValue.Visible = true;
                    companyText.Style["background-color"] = "Silver";
                    ctlPersonalLevelGroup.Style["background-color"] = "Silver";
                }
                else
                {
                    overrideMileageRate.Visible = false;
                    DivDropdownListPersonal.Visible = false;
                    DivctlObjectiveValue.Visible = false;
                    DivCompanyField.Visible = false;
                    DivSelectPersonalLevelGroup.Visible = false;
                    DivObjectiveValue.Visible = false;
                    companyText.Style["background-color"] = "white";
                    ctlPersonalLevelGroup.Style["background-color"] = "white";
                }


                if (mileageRow.IsOverrideLevel)
                {
                    ctlCompanyField.SetValue(mileageRow.OverrideCompanyId);
                    ctlMileagRateRivitionDropDownList.SelectedValue = mileageRow.OverrideUserPersonalLevelCode;
                    ctlObjectiveValue.Text = mileageRow.OverrideLevelRemark;
                }
            }
        }
        public void BindTypeOfCar()
        {
            #region Type of Car
            IList<TranslatedListItem> translateList2 = new List<TranslatedListItem>();

            TranslatedListItem TypeOfCar1 = new TranslatedListItem();
            TypeOfCar1.ID = UIHelper.ParseShort("1");
            TypeOfCar1.Symbol = "Passenger Car";
            translateList2.Add(TypeOfCar1);

            if (ctlDropDownListOwner.SelectedValue.Equals("1"))
            {
                TranslatedListItem TypeOfCar2 = new TranslatedListItem();
                TypeOfCar2.ID = UIHelper.ParseShort("2");
                TypeOfCar2.Symbol = "Motorcycle";
                translateList2.Add(TypeOfCar2);
            }


            TranslatedListItem TypeOfCar3 = new TranslatedListItem();
            TypeOfCar3.ID = UIHelper.ParseShort("3");
            TypeOfCar3.Symbol = "Pick-up";
            translateList2.Add(TypeOfCar3);


            ctlDropDownListTypeOfCar.DataSource = translateList2;
            ctlDropDownListTypeOfCar.DataTextField = "Symbol";
            ctlDropDownListTypeOfCar.DataValueField = "Id";
            ctlDropDownListTypeOfCar.DataBind();
            ctlDropDownListTypeOfCar.SelectedIndex = 0;
            #endregion
        }
        private void ClearMileageItemData()
        {
            ctlCalendarDate.DateValue = string.Empty;
            ctlTextBoxLocationFrom.Text = string.Empty;
            ctlTextBoxLocationTo.Text = string.Empty;
            ctlTextBoxCarMeterStart.Text = string.Empty;
            ctlTextBoxCarMeterEnd.Text = string.Empty;
            ctlTextBoxAdjust.Text = string.Empty;
            ChangeModeButtonItem(true);
        }
        private void SetExpenseMileageObj(FnExpenseMileage expenseMileage)
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
            if (expDS != null)
            {
                long expenseId = UIHelper.ParseLong(expDS.FnExpenseDocument.Rows[0]["ExpenseID"].ToString());

                switch (ctlDropDownListOwner.SelectedValue)
                {
                    case "1":
                        expenseMileage.Owner = OwnerMileage.Employee;
                        expenseMileage.First100KmRate = UIHelper.ParseDouble(ctlTextBoxKmRate.Text.Trim());
                        expenseMileage.Exceed100KmRate = UIHelper.ParseDouble(ctlTextBoxExceedingRate.Text.Trim());
                        expenseMileage.HelpingAmount = UIHelper.ParseDouble(ctlLabelCalculateAllowanceSummary.Text);
                        expenseMileage.OverHelpingAmount = UIHelper.ParseDouble(ctlLabelCalculateOverAllowanceSummary.Text);
                        break;
                    case "2":
                        expenseMileage.Owner = OwnerMileage.Company;
                        expenseMileage.TotalAmount = UIHelper.ParseDouble(ctlTextBoxTotalAmount.Text.Trim());
                        break;
                    default:
                        break;
                }
                switch (ctlDropDownListTypeOfCar.SelectedValue)
                {
                    case "1":
                        expenseMileage.TypeOfCar = TypeOfCar.PrivateCar;
                        break;
                    case "2":
                        expenseMileage.TypeOfCar = TypeOfCar.MotorCycle;
                        break;
                    case "3":
                        expenseMileage.TypeOfCar = TypeOfCar.Pickup;
                        break;
                    default:
                        break;
                }
                expenseMileage.CarLicenseNo = ctlTextBoxCarLicenseNo.Text.Trim();
                expenseMileage.PermissionNo = ctlTextBoxPermissionNo.Text.Trim();

                if (ctlSelectChangeMileageRate.Checked != true)
                {
                    expenseMileage.IsOverrideLevel = false;
                    expenseMileage.CurrentCompanyId = UIHelper.ParseLong(defaultcompanyId.Value);
                    expenseMileage.CurrentUserPersonalLevelCode = ctlPersonalLevelGroup.Text;
                }
                else
                {
                    expenseMileage.IsOverrideLevel = true;
                    expenseMileage.CurrentCompanyId = UIHelper.ParseLong(defaultcompanyId.Value);
                    expenseMileage.CurrentUserPersonalLevelCode = ctlPersonalLevelGroup.Text;
                    expenseMileage.OverrideCompanyId = UIHelper.ParseLong(ctlCompanyField.CompanyID);
                    expenseMileage.OverrideLevelRemark = ctlObjectiveValue.Text;
                    expenseMileage.OverrideUserPersonalLevelCode = ctlMileagRateRivitionDropDownList.SelectedValue;
                }

                expenseMileage.Expense = new FnExpenseDocument(expenseId);

                if (!string.IsNullOrEmpty(ctlCostCenterField.CostCenterId))
                {
                    expenseMileage.CostCenter = new DbCostCenter(UIHelper.ParseLong(ctlCostCenterField.CostCenterId));
                }
                if (!string.IsNullOrEmpty(ctlAccountField.AccountID))
                {
                    expenseMileage.Account = new DbAccount(UIHelper.ParseLong(ctlAccountField.AccountID));
                }
                if (!string.IsNullOrEmpty(ctlIOAutoCompleteLookup.IOID))
                {
                    expenseMileage.IO = new DbInternalOrder(UIHelper.ParseLong(ctlIOAutoCompleteLookup.IOID));
                }
            }
        }
        private void SetExpenseMileageItemObj(FnExpenseMileageItem expenseMileageItem,bool IsEdit)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
            ExpenseDataSet.FnExpenseDocumentRow expenseDocumentRow = expDS.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);
            long requesterID = expenseDocumentRow.DocumentRow.RequesterID;
            SS.SU.DTO.SuUser requester = SS.SU.Query.QueryProvider.SuUserQuery.FindByIdentity(requesterID);
            ExpenseDataSet.FnExpenseMileageRow mileageRow = expDS.FnExpenseMileage.FindByExpenseMileageID(this.ExpenseMileageId);
            expenseMileageItem.ExpenseMileage = new FnExpenseMileage(this.ExpenseMileageId);

            try
            {
                if (!string.IsNullOrEmpty(ctlCalendarDate.DateValue))
                {
                    expenseMileageItem.TravelDate = UIHelper.ParseDate(ctlCalendarDate.DateValue).Value;
                }
            }
            catch (FormatException fex)
            {
                errors.AddError("MileageItem.Error", new Spring.Validation.ErrorMessage("Invalid_Date_Format"));
                throw new ServiceValidationException(errors);
            }
            if (!string.IsNullOrEmpty(ctlTextBoxCarMeterStart.Text.Trim()))
            {
                expenseMileageItem.CarMeterStart = UIHelper.ParseDouble(ctlTextBoxCarMeterStart.Text.Trim());
            }
            if (!string.IsNullOrEmpty(ctlTextBoxCarMeterEnd.Text.Trim()))
            {
                expenseMileageItem.CarMeterEnd = UIHelper.ParseDouble(ctlTextBoxCarMeterEnd.Text.Trim());
            }
            if (!string.IsNullOrEmpty(ctlTextBoxAdjust.Text.Trim()))
            {
                expenseMileageItem.DistanceAdjust = UIHelper.ParseDouble(ctlTextBoxAdjust.Text.Trim());
            }

            if (String.IsNullOrEmpty(ctlCalendarDate.DateValue))
            {
                errors.AddError("MileageItem.Error", new Spring.Validation.ErrorMessage("CalendarDateIsRequiredForMileage"));
                throw new ServiceValidationException(errors);
            }

            if (DateTime.Compare( UIHelper.ParseDate(ctlCalendarDate.DateValue).Value ,DateTime.Now.Date)  > 0)
            {
                errors.AddError("MileageItem.Error", new Spring.Validation.ErrorMessage("PleaseSelectDateIsNotToday"));
                throw new ServiceValidationException(errors);
            }
            
            int rowCount;
            if (IsEdit)
            {
                 rowCount = 1;
            }
            else
            {
                 rowCount = 0;
            }
            if (ctlDropDownListOwner.SelectedValue != "2")
            {
                if (ctlSelectChangeMileageRate.Checked != true)
                {
                    if (requester.Company.MileageProfileId != null)
                    {
                        DbMileageRateRevisionDetail result = ScgDbQueryProvider.DbMileageRateRevisionDetailQuery.FindMileageRateRevision(new Guid(Convert.ToString(requester.Company.MileageProfileId)), ctlPersonalLevelGroup.Text, UIHelper.ParseDate(ctlCalendarDate.DateValue).Value);
                        if (result == null)
                        {
                            errors.AddError("MileageItem.Error", new Spring.Validation.ErrorMessage("CanNotFindMileageRate"));
                            throw new ServiceValidationException(errors);
                        }

                        DataRow[] row = expDS.FnExpenseMileageItem.Select("ExpenseMileageID=" + ExpenseMileageId.ToString());

                        if (row.Count() > rowCount)
                        {
                            foreach (DataRow dr in row)
                            {
                                switch (ctlDropDownListTypeOfCar.SelectedValue)
                                {
                                    case "1":
                                        if (result.CarRate != UIHelper.ParseDouble(ctlTextBoxKmRate.Text) || result.CarRate2 != UIHelper.ParseDouble(ctlTextBoxExceedingRate.Text))
                                        {
                                            errors.AddError("MileageItem.Error", new Spring.Validation.ErrorMessage("MileageRateInvalid", new object[] { String.Format("{0:d/M/yyyy}", dr["TravelDate"]) }));
                                        }
                                        else
                                        {
                                            ctlTextBoxKmRate.Text = Convert.ToString(result.CarRate);
                                            ctlTextBoxExceedingRate.Text = Convert.ToString(result.CarRate2);
                                        }
                                        break;
                                    case "2":
                                        if (result.MotocycleRate != UIHelper.ParseDouble(ctlTextBoxKmRate.Text) || result.MotocycleRate2 != UIHelper.ParseDouble(ctlTextBoxExceedingRate.Text))
                                        {
                                            errors.AddError("MileageItem.Error", new Spring.Validation.ErrorMessage("MileageRateInvalid", new object[] { String.Format("{0:d/M/yyyy}", dr["TravelDate"]) }));
                                        }
                                        else
                                        {
                                            ctlTextBoxKmRate.Text = Convert.ToString(result.MotocycleRate);
                                            ctlTextBoxExceedingRate.Text = Convert.ToString(result.MotocycleRate2);
                                        }
                                        break;
                                    case "3":
                                        if (result.PickUpRate != UIHelper.ParseDouble(ctlTextBoxKmRate.Text) || result.PickUpRate2 != UIHelper.ParseDouble(ctlTextBoxExceedingRate.Text))
                                        {
                                            errors.AddError("MileageItem.Error", new Spring.Validation.ErrorMessage("MileageRateInvalid", new object[] { String.Format("{0:d/M/yyyy}", dr["TravelDate"]) }));
                                        }
                                        else
                                        {
                                            ctlTextBoxKmRate.Text = Convert.ToString(result.PickUpRate);
                                            ctlTextBoxExceedingRate.Text = Convert.ToString(result.PickUpRate2);
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
                        }
                        else
                        {
                            switch (ctlDropDownListTypeOfCar.SelectedValue)
                            {
                                case "1":
                                    ctlTextBoxKmRate.Text = Convert.ToString(result.CarRate);
                                    ctlTextBoxExceedingRate.Text = Convert.ToString(result.CarRate2);
                                    break;
                                case "2":
                                    ctlTextBoxKmRate.Text = Convert.ToString(result.MotocycleRate);
                                    ctlTextBoxExceedingRate.Text = Convert.ToString(result.MotocycleRate2);
                                    break;
                                case "3":
                                    ctlTextBoxKmRate.Text = Convert.ToString(result.PickUpRate);
                                    ctlTextBoxExceedingRate.Text = Convert.ToString(result.PickUpRate2);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    else
                    {
                        errors.AddError("MileageItem.Error", new Spring.Validation.ErrorMessage("CanNotFindMileageRate"));
                        throw new ServiceValidationException(errors);
                    }
                }
                else
                {
                    DbCompany company = SCG.DB.Query.ScgDbQueryProvider.DbCompanyQuery.getDbCompanyByCompanyCode(ctlCompanyField.CompanyCode);
                    if (company == null)
                    {
                        errors.AddError("MileageItem.Error", new Spring.Validation.ErrorMessage("CanNotFindMileageRate"));
                        throw new ServiceValidationException(errors);
                    }
                    if (String.IsNullOrEmpty(ctlObjectiveValue.Text))
                    {
                        errors.AddError("MileageItem.Error", new Spring.Validation.ErrorMessage("ObjectiveNotFound"));
                        throw new ServiceValidationException(errors);
                    }
                    if (company.MileageProfileId == null)
                    {
                        errors.AddError("MileageItem.Error", new Spring.Validation.ErrorMessage("CanNotFindMileageRate"));
                        throw new ServiceValidationException(errors);
                    }

                    DbMileageRateRevisionDetail result = ScgDbQueryProvider.DbMileageRateRevisionDetailQuery.FindMileageRateRevision(new Guid(Convert.ToString(company.MileageProfileId)), Convert.ToString(ctlMileagRateRivitionDropDownList.SelectedValue), UIHelper.ParseDate(ctlCalendarDate.DateValue).Value);

                    if (result == null)
                    {
                        errors.AddError("MileageItem.Error", new Spring.Validation.ErrorMessage("CanNotFindMileageRate"));
                        throw new ServiceValidationException(errors);
                    }

                    DataRow[] row = expDS.FnExpenseMileageItem.Select("ExpenseMileageID=" + ExpenseMileageId.ToString());
                    if (row.Count() + (IsEdit ? 0 : 1) > 1)
                    {
                        foreach (DataRow dr in row)
                        {
                            switch (ctlDropDownListTypeOfCar.SelectedValue)
                            {
                                case "1":
                                    if (result.CarRate != UIHelper.ParseDouble(ctlTextBoxKmRate.Text) || result.CarRate2 != UIHelper.ParseDouble(ctlTextBoxExceedingRate.Text))
                                    {
                                        errors.AddError("MileageItem.Error", new Spring.Validation.ErrorMessage("MileageRateInvalid", new object[] { String.Format("{0:d/M/yyyy}", dr["TravelDate"]) }));
                                    }
                                    else
                                    {
                                        ctlTextBoxKmRate.Text = Convert.ToString(result.CarRate);
                                        ctlTextBoxExceedingRate.Text = Convert.ToString(result.CarRate2);
                                    }
                                    break;
                                case "2":
                                    if (result.MotocycleRate != UIHelper.ParseDouble(ctlTextBoxKmRate.Text) || result.MotocycleRate2 != UIHelper.ParseDouble(ctlTextBoxExceedingRate.Text))
                                    {
                                        errors.AddError("MileageItem.Error", new Spring.Validation.ErrorMessage("MileageRateInvalid", new object[] { String.Format("{0:d/M/yyyy}", dr["TravelDate"]) }));
                                    }
                                    else
                                    {
                                        ctlTextBoxKmRate.Text = Convert.ToString(result.MotocycleRate);
                                        ctlTextBoxExceedingRate.Text = Convert.ToString(result.MotocycleRate2);
                                    }
                                    break;
                                case "3":
                                    if (result.PickUpRate != UIHelper.ParseDouble(ctlTextBoxKmRate.Text) || result.PickUpRate2 != UIHelper.ParseDouble(ctlTextBoxExceedingRate.Text))
                                    {
                                        errors.AddError("MileageItem.Error", new Spring.Validation.ErrorMessage("MileageRateInvalid", new object[] { String.Format("{0:d/M/yyyy}", dr["TravelDate"]) }));
                                    }
                                    else
                                    {
                                        ctlTextBoxKmRate.Text = Convert.ToString(result.PickUpRate);
                                        ctlTextBoxExceedingRate.Text = Convert.ToString(result.PickUpRate2);
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (!errors.IsEmpty) throw new ServiceValidationException(errors);
                    }
                    else
                    {
                        switch (ctlDropDownListTypeOfCar.SelectedValue)
                        {
                            case "1":

                                ctlTextBoxKmRate.Text = Convert.ToString(result.CarRate);
                                ctlTextBoxExceedingRate.Text = Convert.ToString(result.CarRate2);
                                break;
                            case "2":

                                ctlTextBoxKmRate.Text = Convert.ToString(result.MotocycleRate);
                                ctlTextBoxExceedingRate.Text = Convert.ToString(result.MotocycleRate2);

                                break;
                            case "3":

                                ctlTextBoxKmRate.Text = Convert.ToString(result.PickUpRate);
                                ctlTextBoxExceedingRate.Text = Convert.ToString(result.PickUpRate2);

                                break;
                            default:
                                break;
                        }
                    }
                }
            } /*check owner not company*/

            expenseMileageItem.LocationFrom = ctlTextBoxLocationFrom.Text.Trim();
            expenseMileageItem.LocationTo = ctlTextBoxLocationTo.Text.Trim();
            expenseMileageItem.UpdPgm = ProgramCode;
            ctlUpdatePanelMileageSubData.Update();
        }
        private void CalculateForEmployee()
        {
            if (ctlDropDownListOwner.SelectedValue == "1")
            {
                FnExpenseMileage mileage = new FnExpenseMileage(this.ExpenseMileageId);
                SetExpenseMileageObj(mileage);
                FnExpenseMileageService.UpdateMileageSummary(TxId, mileage);

                FnExpenseMileageItemService.UpdateMileageItemByMileageID(TxId, ExpenseMileageId, mileage);

                ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
                ExpenseDataSet.FnExpenseMileageRow row = expDS.FnExpenseMileage.FindByExpenseMileageID(ExpenseMileageId);

                decimal total = TotalDistanceCarMeter();
                decimal first100km = TotalFirst100KmCarMeter();
                decimal exceed100Km = FnExpenseMileageService.CalculateExceed100KmRate(total, first100km);

                decimal first100KmRate = UIHelper.ParseDecimal(ctlTextBoxKmRate.Text);
                decimal exceed100KmRate = UIHelper.ParseDecimal(ctlTextBoxExceedingRate.Text);

                decimal amountFirst100Km = FnExpenseMileageService.CalculateDistanceAmount(first100km, first100KmRate);
                decimal amountExceed100Km = FnExpenseMileageService.CalculateDistanceAmount(exceed100Km, exceed100KmRate);

                //decimal toTalAmount = FnExpenseMileageService.CalculateTotalAmount(amountFirst100Km, amountExceed100Km);
                //decimal helpingAmount = FnExpenseMileageService.CalculateHelpingAmount(total, (decimal)4);
                //decimal overHelpingAmount = FnExpenseMileageService.CalculateOverHelpingAmount(toTalAmount, helpingAmount);

                ctlLabelCalculateDistanceFirst100Km.Text = UIHelper.BindDecimal(first100km.ToString());
                ctlLabelCalculateFirst100KmRateForDistanceFirst100Km.Text = UIHelper.BindDecimal(first100KmRate.ToString());
                ctlLabelCalculateAmountForDistanceFirst100Km.Text = UIHelper.BindDecimal(amountFirst100Km.ToString());

                ctlLabelCalculateDistanceExceed.Text = UIHelper.BindDecimal(exceed100Km.ToString());
                ctlLabelCalculateExceedRateForDistanceExceed.Text = UIHelper.BindDecimal(exceed100KmRate.ToString());
                ctlLabelCalculateAmountForDistanceExceed.Text = UIHelper.BindDecimal(amountExceed100Km.ToString());

                if (row != null)
                {
                    ctlLabelCalculateTotalAmount.Text = UIHelper.BindDecimal(row["TotalAmount"].ToString());
                    ctlLabelCalculateAllowance.Text = UIHelper.BindDecimal(row["HelpingAmount"].ToString());
                    ctlLabelCalculateOverAllowance.Text = UIHelper.BindDecimal(row["OverHelpingAmount"].ToString());

                    ctlLabelCalculateTotalAmountSummary.Text = UIHelper.BindDecimal(row["TotalAmount"].ToString());
                    ctlLabelCalculateAllowanceSummary.Text = UIHelper.BindDecimal(row["HelpingAmount"].ToString());
                    ctlLabelCalculateOverAllowanceSummary.Text = UIHelper.BindDecimal(row["OverHelpingAmount"].ToString());
                }
            }
        }
        private void CalculateForCompany()
        {
            if (ctlDropDownListOwner.SelectedValue == "2")
            {
                FnExpenseMileage mileage = new FnExpenseMileage(this.ExpenseMileageId);
                SetExpenseMileageObj(mileage);
                FnExpenseMileageService.UpdateMileageSummary(TxId, mileage);

                FnExpenseMileageItemService.UpdateMileageItemByMileageID(TxId, ExpenseMileageId, mileage);

                ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
                ExpenseDataSet.FnExpenseMileageRow row = expDS.FnExpenseMileage.FindByExpenseMileageID(ExpenseMileageId);

                decimal totalAmount = UIHelper.ParseDecimal(ctlTextBoxTotalAmount.Text.Trim());
                decimal distanceTotal = TotalDistanceCarMeter();
                decimal adjustTotal = TotalAdjustedCarMeter();
                decimal netDistance = FnExpenseMileageService.CalculateNetDistance(distanceTotal, adjustTotal);
                decimal reimBurseMentAmount = (decimal)0;

                if (row != null)
                {
                    reimBurseMentAmount = UIHelper.ParseDecimal(row["HelpingAmount"].ToString());
                    ctlLabelReimBurseMentAmount.Text = UIHelper.BindDecimal(reimBurseMentAmount.ToString());
                    ctlLabelReimBurseMentAmountForInvoice.Text = UIHelper.BindDecimal(ctlLabelReimBurseMentAmount.Text);
                    ctlLabelRemaining.Text = UIHelper.BindDecimal(FnExpenseMileageService.CalculateRemaining(reimBurseMentAmount, TotalInvoiceNetAmount()).ToString());
                    ctlUpdatePanelGridInvoice.Update();
                }
            }
        }
        private decimal TotalDistanceCarMeter()
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
            DataRow[] row = expDS.FnExpenseMileageItem.Select("ExpenseMileageID=" + ExpenseMileageId.ToString());

            decimal distanceTotal = (decimal)0;

            foreach (DataRow dr in row)
            {
                distanceTotal += Math.Round(UIHelper.ParseDecimal(dr["DistanceTotal"].ToString()), 2, MidpointRounding.AwayFromZero);
            }

            return distanceTotal;
        }
        private decimal TotalFirst100KmCarMeter()
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
            DataRow[] row = expDS.FnExpenseMileageItem.Select("ExpenseMileageID=" + ExpenseMileageId.ToString());

            decimal distanceFirst100Km = (decimal)0;

            foreach (DataRow dr in row)
            {
                distanceFirst100Km += Math.Round(UIHelper.ParseDecimal(dr["DistanceFirst100Km"].ToString()), 2, MidpointRounding.AwayFromZero);
            }

            return distanceFirst100Km;
        }
        private decimal TotalAdjustedCarMeter()
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
            DataRow[] row = expDS.FnExpenseMileageItem.Select("ExpenseMileageID=" + ExpenseMileageId.ToString());

            decimal distanceAdjusted = (decimal)0;

            foreach (DataRow dr in row)
            {
                distanceAdjusted += Math.Round(UIHelper.ParseDecimal("100"), 2, MidpointRounding.AwayFromZero);//dr["DistanceAdjust"].ToString()
            }

            return distanceAdjusted;
        }
        private decimal TotalInvoiceNetAmount()
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
            DataRow[] row = expDS.FnExpenseInvoice.Select("InvoiceDocumentType='" + InvoiceType.Mileage + "'");

            decimal netAmount = (decimal)0;

            foreach (DataRow dr in row)
            {
                netAmount += Math.Round(UIHelper.ParseDecimal(dr["NetAmount"].ToString()), 2, MidpointRounding.AwayFromZero);
            }

            return netAmount;
        }
        #endregion

        protected void ctlCarLicenseNo_TextChanged(object sender, EventArgs e)
        {
            double lastMile = ScgeAccountingQueryProvider.FnExpenseMileageQuery.GetLastMileByCarLicenseNo(ctlTextBoxCarLicenseNo.Text);
            ctlTextBoxCarMeterStart.Text = lastMile.ToString();
            ctlUpdatePanelMileageItemData.Update();
        }

        void ShowInvoicePopup(string mode, long? invoiceId, long documentID)
        {

        }
        protected void ctlMileageGrid_DataBound(object sender, EventArgs e)
        {

            foreach (DataControlFieldHeaderCell item in ctlMileageGrid.HeaderRow.Controls)
            {
                if (ctlDropDownListTypeOfCar.SelectedValue == "2")
                {
                    ((Label)item.FindControl("ctlLabelHeaderDistanceFirst100KmForEmployee")).Text = GetProgramMessage("HeaderFirst30Km");
                    ((Label)item.FindControl("ctlLabelHeaderDistanceExceed100KmForEmployee")).Text = GetProgramMessage("HeaderExceed30Km");
                }
                else
                {
                    ((Label)item.FindControl("ctlLabelHeaderDistanceFirst100KmForEmployee")).Text = GetProgramMessage("HeaderFirst100Km");
                    ((Label)item.FindControl("ctlLabelHeaderDistanceExceed100KmForEmployee")).Text = GetProgramMessage("HeaderExceed100Km");
                }
            }
            ctlUpdatePanelGridView.Update();
        }

        protected void ctlPopupCaller_NotifyPopupResult(object sender, string action, string value)
        {
            if (action != "ok") return;

            BindMileageInvoiceGrid();
            CalculateForCompany();
            ctlUpdatePanelGridInvoice.Update();
        }

        protected void ctlSelectChangeMileageRate_CheckedChanged(object sender, EventArgs e)
        {
            if (ctlSelectChangeMileageRate.Checked == true)
            {
                DivSelectPersonalLevelGroup.Visible = true;
                DivObjectiveValue.Visible = true;
                overrideMileageRate.Visible = true;
                DivCompanyField.Visible = true;
                DivDropdownListPersonal.Visible = true;
                DivctlObjectiveValue.Visible = true;
                companyText.Style["background-color"] = "Silver";
                ctlPersonalLevelGroup.Style["background-color"] = "Silver";
            }
            else
            {
                DivSelectPersonalLevelGroup.Visible = false;
                DivObjectiveValue.Visible = false;
                overrideMileageRate.Visible = false;
                DivCompanyField.Visible = false;
                DivDropdownListPersonal.Visible = false;
                DivctlObjectiveValue.Visible = false;
                companyText.Style["background-color"] = "white";
                ctlPersonalLevelGroup.Style["background-color"] = "white";
            }
        }

        public void UpdateMileageRate()
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
            ExpenseDataSet.FnExpenseDocumentRow expenseDocumentRow = expDS.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);
            long requesterID = expenseDocumentRow.DocumentRow.RequesterID;
            SS.SU.DTO.SuUser requester = SS.SU.Query.QueryProvider.SuUserQuery.FindByIdentity(requesterID);
            DbCompany companyRequester = SCG.DB.Query.ScgDbQueryProvider.DbCompanyQuery.getDbCompanyByCompanyCode(requester.CompanyCode);
            ExpenseDataSet.FnExpenseMileageRow mileageRow = expDS.FnExpenseMileage.FindByExpenseMileageID(this.ExpenseMileageId);
            DataRow[] row = expDS.FnExpenseMileageItem.Select("ExpenseMileageID=" + ExpenseMileageId.ToString());
            if (ctlDropDownListOwner.SelectedValue != "2")
            {
                if (ctlSelectChangeMileageRate.Checked != true)
                {
                    if (companyRequester.MileageProfileId != null)
                    {
                        if (row.Count() > 1)
                        {
                            int count = 1;
                            foreach (DataRow dr in row)
                            {
                                if (count == 1)
                                {
                                    DbMileageRateRevisionDetail result2 = ScgDbQueryProvider.DbMileageRateRevisionDetailQuery.FindMileageRateRevision(new Guid(Convert.ToString(companyRequester.MileageProfileId)), ctlPersonalLevelGroup.Text, (DateTime)dr["TravelDate"]);
                                    if (result2 == null)
                                    {
                                        errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("CanNotFindMileageRate"));
                                        throw new ServiceValidationException(errors);
                                    }
                                    switch (ctlDropDownListTypeOfCar.SelectedValue)
                                    {
                                        case "1":

                                            ctlTextBoxKmRate.Text = Convert.ToString(result2.CarRate);
                                            ctlTextBoxExceedingRate.Text = Convert.ToString(result2.CarRate2);
                                            break;
                                        case "2":

                                            ctlTextBoxKmRate.Text = Convert.ToString(result2.MotocycleRate);
                                            ctlTextBoxExceedingRate.Text = Convert.ToString(result2.MotocycleRate2);

                                            break;
                                        case "3":

                                            ctlTextBoxKmRate.Text = Convert.ToString(result2.PickUpRate);
                                            ctlTextBoxExceedingRate.Text = Convert.ToString(result2.PickUpRate2);

                                            break;
                                        default:
                                            break;
                                    }
                                    count++;
                                    continue;
                                }
                                DbMileageRateRevisionDetail result = ScgDbQueryProvider.DbMileageRateRevisionDetailQuery.FindMileageRateRevision(new Guid(Convert.ToString(companyRequester.MileageProfileId)), ctlPersonalLevelGroup.Text, (DateTime)dr["TravelDate"]);
                                if (result == null)
                                {
                                    errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("CanNotFindMileageRate"));
                                    throw new ServiceValidationException(errors);
                                }
                                switch (ctlDropDownListTypeOfCar.SelectedValue)
                                {
                                    case "1":
                                        if (result.CarRate != UIHelper.ParseDouble(ctlTextBoxKmRate.Text) || result.CarRate2 != UIHelper.ParseDouble(ctlTextBoxExceedingRate.Text))
                                        {
                                            errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("MileageRateInvalid", new object[] { String.Format("{0:d/M/yyyy}", dr["TravelDate"]) }));
                                        }
                                        else
                                        {
                                            ctlTextBoxKmRate.Text = Convert.ToString(result.CarRate);
                                            ctlTextBoxExceedingRate.Text = Convert.ToString(result.CarRate2);
                                        }
                                        break;
                                    case "2":
                                        if (result.MotocycleRate != UIHelper.ParseDouble(ctlTextBoxKmRate.Text) || result.MotocycleRate2 != UIHelper.ParseDouble(ctlTextBoxExceedingRate.Text))
                                        {
                                            errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("MileageRateInvalid", new object[] { String.Format("{0:d/M/yyyy}", dr["TravelDate"]) }));
                                        }
                                        else
                                        {
                                            ctlTextBoxKmRate.Text = Convert.ToString(result.MotocycleRate);
                                            ctlTextBoxExceedingRate.Text = Convert.ToString(result.MotocycleRate2);
                                        }
                                        break;
                                    case "3":
                                        if (result.PickUpRate != UIHelper.ParseDouble(ctlTextBoxKmRate.Text) || result.PickUpRate2 != UIHelper.ParseDouble(ctlTextBoxExceedingRate.Text))
                                        {
                                            errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("MileageRateInvalid", new object[] { String.Format("{0:d/M/yyyy}", dr["TravelDate"]) }));
                                        }
                                        else
                                        {
                                            ctlTextBoxKmRate.Text = Convert.ToString(result.PickUpRate);
                                            ctlTextBoxExceedingRate.Text = Convert.ToString(result.PickUpRate2);
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                if (!errors.IsEmpty)
                                {
                                    throw new ServiceValidationException(errors);
                                }
                            }
                        }
                        else
                        {
                            foreach (DataRow dr in row)
                            {
                                DbMileageRateRevisionDetail result = ScgDbQueryProvider.DbMileageRateRevisionDetailQuery.FindMileageRateRevision(new Guid(Convert.ToString(companyRequester.MileageProfileId)), ctlPersonalLevelGroup.Text, (DateTime)dr["TravelDate"]);
                                if (result == null)
                                {
                                    errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("CanNotFindMileageRate"));
                                    throw new ServiceValidationException(errors);
                                }
                                switch (ctlDropDownListTypeOfCar.SelectedValue)
                                {
                                    case "1":

                                        ctlTextBoxKmRate.Text = Convert.ToString(result.CarRate);
                                        ctlTextBoxExceedingRate.Text = Convert.ToString(result.CarRate2);
                                        break;
                                    case "2":

                                        ctlTextBoxKmRate.Text = Convert.ToString(result.MotocycleRate);
                                        ctlTextBoxExceedingRate.Text = Convert.ToString(result.MotocycleRate2);

                                        break;
                                    case "3":

                                        ctlTextBoxKmRate.Text = Convert.ToString(result.PickUpRate);
                                        ctlTextBoxExceedingRate.Text = Convert.ToString(result.PickUpRate2);

                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    else
                    {
                        errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("CanNotFindMileageRate"));
                        throw new ServiceValidationException(errors);
                    }
                }
                else
                {
                    DbCompany Overridecompany = SCG.DB.Query.ScgDbQueryProvider.DbCompanyQuery.getDbCompanyByCompanyCode(ctlCompanyField.CompanyCode);

                    if (Overridecompany == null)
                    {
                        errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("OverrideCompanyIdIsRequare"));
                        throw new ServiceValidationException(errors);
                    }
                    if (String.IsNullOrEmpty(ctlObjectiveValue.Text))
                    {
                        errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("ObjectiveNotFound"));
                        throw new ServiceValidationException(errors);
                    }
                    if (Overridecompany.MileageProfileId == null)
                    {
                        errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("CanNotFindMileageRate"));
                        throw new ServiceValidationException(errors);
                    }
                    if (row.Count() > 1)
                    {
                        int count = 1;
                        foreach (DataRow dr in row)
                        {
                            if (count == 1)
                            {
                                DbMileageRateRevisionDetail result2 = ScgDbQueryProvider.DbMileageRateRevisionDetailQuery.FindMileageRateRevision(new Guid(Convert.ToString(Overridecompany.MileageProfileId)), Convert.ToString(ctlMileagRateRivitionDropDownList.SelectedValue), (DateTime)dr["TravelDate"]);
                                if (result2 == null)
                                {
                                    errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("CanNotFindMileageRate"));
                                    throw new ServiceValidationException(errors);
                                }
                                switch (ctlDropDownListTypeOfCar.SelectedValue)
                                {
                                    case "1":

                                        ctlTextBoxKmRate.Text = Convert.ToString(result2.CarRate);
                                        ctlTextBoxExceedingRate.Text = Convert.ToString(result2.CarRate2);
                                        break;
                                    case "2":

                                        ctlTextBoxKmRate.Text = Convert.ToString(result2.MotocycleRate);
                                        ctlTextBoxExceedingRate.Text = Convert.ToString(result2.MotocycleRate2);

                                        break;
                                    case "3":

                                        ctlTextBoxKmRate.Text = Convert.ToString(result2.PickUpRate);
                                        ctlTextBoxExceedingRate.Text = Convert.ToString(result2.PickUpRate2);

                                        break;
                                    default:
                                        break;
                                }
                                count++;
                                continue;
                            }

                            DbMileageRateRevisionDetail result = ScgDbQueryProvider.DbMileageRateRevisionDetailQuery.FindMileageRateRevision(new Guid(Convert.ToString(Overridecompany.MileageProfileId)), Convert.ToString(ctlMileagRateRivitionDropDownList.SelectedValue), (DateTime)dr["TravelDate"]);

                            if (result == null)
                            {
                                errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("CanNotFindMileageRate"));
                                throw new ServiceValidationException(errors);
                            }
                            switch (ctlDropDownListTypeOfCar.SelectedValue)
                            {
                                case "1":
                                    if (result.CarRate != UIHelper.ParseDouble(ctlTextBoxKmRate.Text) || result.CarRate2 != UIHelper.ParseDouble(ctlTextBoxExceedingRate.Text))
                                    {
                                        errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("MileageRateInvalid", new object[] { String.Format("{0:d/M/yyyy}", dr["TravelDate"]) }));
                                    }
                                    else
                                    {
                                        ctlTextBoxKmRate.Text = Convert.ToString(result.CarRate);
                                        ctlTextBoxExceedingRate.Text = Convert.ToString(result.CarRate2);
                                    }
                                    break;
                                case "2":
                                    if (result.MotocycleRate != UIHelper.ParseDouble(ctlTextBoxKmRate.Text) || result.MotocycleRate2 != UIHelper.ParseDouble(ctlTextBoxExceedingRate.Text))
                                    {
                                        errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("MileageRateInvalid", new object[] { String.Format("{0:d/M/yyyy}", dr["TravelDate"]) }));
                                    }
                                    else
                                    {
                                        ctlTextBoxKmRate.Text = Convert.ToString(result.MotocycleRate);
                                        ctlTextBoxExceedingRate.Text = Convert.ToString(result.MotocycleRate2);
                                    }
                                    break;
                                case "3":
                                    if (result.PickUpRate != UIHelper.ParseDouble(ctlTextBoxKmRate.Text) || result.PickUpRate2 != UIHelper.ParseDouble(ctlTextBoxExceedingRate.Text))
                                    {
                                        errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("MileageRateInvalid", new object[] { String.Format("{0:d/M/yyyy}", dr["TravelDate"]) }));
                                    }
                                    else
                                    {
                                        ctlTextBoxKmRate.Text = Convert.ToString(result.PickUpRate);
                                        ctlTextBoxExceedingRate.Text = Convert.ToString(result.PickUpRate2);
                                    }
                                    break;
                                default:
                                    break;
                            }
                            if (!errors.IsEmpty)
                            {
                                throw new ServiceValidationException(errors);
                            }
                        }
                    }
                    else
                    {
                        foreach (DataRow dr in row)
                        {
                            DbMileageRateRevisionDetail result = ScgDbQueryProvider.DbMileageRateRevisionDetailQuery.FindMileageRateRevision(new Guid(Convert.ToString(Overridecompany.MileageProfileId)), Convert.ToString(ctlMileagRateRivitionDropDownList.SelectedValue), (DateTime)dr["TravelDate"]);
                            if (result == null)
                            {
                                errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("CanNotFindMileageRate"));
                                throw new ServiceValidationException(errors);
                            }
                            switch (ctlDropDownListTypeOfCar.SelectedValue)
                            {
                                case "1":

                                    ctlTextBoxKmRate.Text = Convert.ToString(result.CarRate);
                                    ctlTextBoxExceedingRate.Text = Convert.ToString(result.CarRate2);
                                    break;
                                case "2":

                                    ctlTextBoxKmRate.Text = Convert.ToString(result.MotocycleRate);
                                    ctlTextBoxExceedingRate.Text = Convert.ToString(result.MotocycleRate2);

                                    break;
                                case "3":

                                    ctlTextBoxKmRate.Text = Convert.ToString(result.PickUpRate);
                                    ctlTextBoxExceedingRate.Text = Convert.ToString(result.PickUpRate2);

                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }/*check owner not company*/
            ctlUpdatePanelMileageSubData.Update();
            ctlUpdatePanelBottom.Update();
        }
    }
}
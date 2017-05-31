using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using SS.Standard.UI;
using SS.Standard.Utilities;

using SCG.eAccounting.BLL;
using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.Web.UserControls.LOV.SCG.DB;

using SS.DB.Query;

using SCG.DB.DTO;
using SCG.DB.Query;
using SCG.DB.DTO.ValueObject;

using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.DTO.DataSet;
using SS.DB.DTO;
using SS.SU.DTO;
using SS.Standard.WorkFlow.Query;
using System.Text.RegularExpressions;
using SCG.DB.Query.Hibernate;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor.Components
{
    public partial class InvoiceForm : BaseUserControl, IEditorComponent
    {
        public delegate void NotifyPopupHandler(object sender, string returnAction);
        public event NotifyPopupHandler NotifyPopup;

        #region Service Property
        public ITransactionService TransactionService { get; set; }
        public IFnExpenseDocumentService FnExpenseDocumentService { get; set; }
        public IFnExpenseInvoiceService FnExpenseInvoiceService { get; set; }
        public IFnExpenseInvoiceItemService FnExpenseInvoiceItemService { get; set; }
        #endregion

        #region Property
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
            get
            {
                if (ViewState[ViewStateName.TransactionID] == null)
                    return Guid.Empty;
                return (Guid)ViewState[ViewStateName.TransactionID];
            }
            set { ViewState[ViewStateName.TransactionID] = value; }
        }
        public Guid ParentTxId
        {
            get { return (Guid)ViewState[ViewStateName.ParentTxID]; }
            set { ViewState[ViewStateName.ParentTxID] = value; }
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
        public long InvoiceId
        {
            get { return (long)ViewState["InvoiceId"]; }
            set { ViewState["InvoiceId"] = value; }
        }
        private string Mode
        {
            get { return ViewState["InvoicMode"].ToString(); }
            set { ViewState["InvoicMode"] = value; }
        }
        public string InitialFlag
        {
            get
            {
                if (ViewState[ViewStateName.InitialFlag] != null)
                    return ViewState[ViewStateName.InitialFlag].ToString();
                return string.Empty;
            }
            set { ViewState[ViewStateName.InitialFlag] = value; }
        }
        public bool IsDomestic
        {
            get
            {
                if (DocumentType.Equals(ZoneType.Domestic) || string.IsNullOrEmpty(DocumentType))
                    return true;
                else
                    return false;
            }
        }
        public string DocumentType
        {
            get
            {
                if (ViewState["DocumentType"] != null)
                    return (string)ViewState["DocumentType"];
                return string.Empty;
            }
            set { ViewState["DocumentType"] = value; }
        }
        public bool IsPeople
        {
            get
            {
                if (ViewState["IsPeople"] == null)
                    return false;
                return (bool)ViewState["IsPeople"];
            }
            set { ViewState["IsPeople"] = value; }
        }
        public string InvoiceDocumentType
        {
            get
            {
                if (ViewState["InvoiceType"] == null)
                    return string.Empty;
                return ViewState["InvoiceType"].ToString();
            }
            set { ViewState["InvoiceType"] = value; }
        }
        public double TotalAmount { get; set; }
        public double TotalAmountLocalCurrency { get; set; }
        public double TotalAmountMainCurrency { get; set; }

        public long InvoiceItemID
        {
            get
            {
                if (ViewState["InvoiceItemID"] == null)
                    return 0;
                return (long)ViewState["InvoiceItemID"];
            }
            set { ViewState["InvoiceItemID"] = value; }
        }

        public long? VendorID
        {
            get
            {
                if (ViewState["VendorID"] == null)
                    return null;
                return UIHelper.ParseLong(ViewState["VendorID"].ToString());
            }
            set { ViewState["VendorID"] = value; }
        }

        public bool LinkControlVisible
        {
            get { return ViewState["LinkControlVisible"] == null ? true : (bool)ViewState["LinkControlVisible"]; }
            set { ViewState["LinkControlVisible"] = value; }
        }
        public bool LinkControlEditable
        {
            get { return ViewState["LinkControlEditable"] == null ? true : (bool)ViewState["LinkControlEditable"]; }
            set { ViewState["LinkControlEditable"] = value; }
        }
        public double TmpBaseAmount
        {
            get
            {
                if (ViewState["TmpBaseAmount"] == null)
                    return 0;
                return (double)ViewState["TmpBaseAmount"];
            }
            set { ViewState["TmpBaseAmount"] = value; }
        }
        #endregion

        #region Constant
        private const string ASCENDING = " ASC";
        private const string DESCENDING = " DESC";
        #endregion

        public bool notCheck { get; set; }
        public bool CheckRender
        {
            get { return Session[this.UniqueID + "CheckRender"] != null ? Session[this.UniqueID + "CheckRender"].ToString().Equals("true") : false; }
            set { Session[this.UniqueID + "CheckRender"] = value.ToString().ToLower(); }
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

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!Page.IsPostBack)
            {
                this.TotalAmount = 0;
                this.TotalAmountLocalCurrency = 0;
                this.TotalAmountMainCurrency = 0;

                ctlInvoiceItemGridview.DataBind();
                ctlUpdatePanelInvoiceForm.Update();
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            CheckRender = true;
            //if (!this.notCheckRender)
            //{


            //    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "yyy", "showHideForm();", true);
            //}
            //ctlUpdateInvoicePanel.Update();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder str = new StringBuilder();
            if (this.CheckRender || !InitialFlag.Equals(FlagEnum.NewFlag))
            {
                str.Append(" function showHideForm()");
                str.Append(" {");
                str.Append(" var showVatCtl = document.getElementById('" + ctlShowVatControl.ClientID + "');");
                str.Append(" var showWHTCtl = document.getElementById('" + ctlShowWHTControl.ClientID + "');");
                //str.Append(" alert(showVatCtl); alert(showWHTCtl);");
                str.Append(" if (showVatCtl != null) ");
                str.Append(" {");
                str.Append(" if (showVatCtl.value == 'showVat') ");
                str.Append(" {");
                str.Append(" document.getElementById('ctlDivSummary').style.display = 'block';");
                str.Append(" document.getElementById('" + ctlInvoiceDiv.ClientID + "').style.display = 'block';");
                str.Append(" }");
                str.Append(" else");
                str.Append(" {");
                str.Append(" document.getElementById('ctlDivSummary').style.display = 'none';");
                str.Append(" document.getElementById('" + ctlInvoiceDiv.ClientID + "').style.display = 'none';");
                str.Append(" }");
                str.Append(" }");
                str.Append(" if (showWHTCtl != null) ");
                str.Append(" {");
                str.Append(" if (showWHTCtl.value == 'showWHT') ");
                str.Append(" {");
                str.Append(" document.getElementById('" + ctlInvoiceDiv.ClientID + "').style.display = 'block';");
                str.Append(" document.getElementById('" + ctlWhtFds.ClientID + "').style.display = 'block';");
                str.Append(" document.getElementById('ctlDivSummary').style.display = 'block';");
                str.Append(" }");
                str.Append(" else");
                str.Append(" {");
                str.Append(" document.getElementById('" + ctlWhtFds.ClientID + "').style.display = 'none';");
                str.Append(" document.getElementById('ctlDivSummary').style.display = 'none';");
                str.Append(" }");
                str.Append(" }");
                // str.Append("alert(showVatCtl.value); alert(showWHTCtl.value);");
                str.Append(" if((showWHTCtl != null && showWHTCtl.value == 'showWHT') || (showVatCtl != null && showVatCtl.value == 'showVat'))");
                str.Append(" { document.getElementById('ctlDivSummary').style.display = 'block'; document.getElementById('" + ctlSummary.ClientID + "').style.display = 'block';}");
                str.Append(" }");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "yyyy", str.ToString(), true);
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "xxx", "showHideForm();", true);

            }
            ctlCostCenterField.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlCostCenterField_OnObjectLookUpReturn);
        }

        public void ctlCostCenterField_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            if (e.ObjectReturn != null)
            {
                DbCostCenter cost = e.ObjectReturn as DbCostCenter;
                DbCostCenter dbCostcenter = ScgDbQueryProvider.DbCostCenterQuery.FindByIdentity(cost.CostCenterID);
                ctlIOAutoCompleteLookup.CostCenterId = dbCostcenter.CostCenterID;

            }
            ctlUpdatePanelInvoiceForm.Update();
        }

        #region DropdownList Event
        protected void ctlCurrency_NotifyPopupResult(object sender, string action, string result)
        {
            this.GetAdvanceExchangeRate();
        }
        #endregion

        #region User Control Event
        protected void ctlVendorAPLookup_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            VOVendor vendor = (VOVendor)e.ObjectReturn;
            if ((vendor != null))
            {
                ctlVendorCodeAP.Text = vendor.VendorCode;
            }
            else
            {
                ctlVendorCodeAP.Text = string.Empty;
            }
            ctlUpdateInvoicePanel.Update();
        }
        protected void ctlVendorLookup_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            VOVendor vendor = (VOVendor)e.ObjectReturn;
            if ((vendor != null))
            {
                ctlVendorCode.Text = vendor.VendorCode;
                ctlVendorBranch.Text = vendor.VendorBranch;
                ctlVendorName.Text = vendor.VendorName;
                VendorID = vendor.VendorID;
                ctlVendorTaxNo.Text = vendor.VendorTaxCode;
                ctlStreet.Text = vendor.Street;
                ctlCity.Text = vendor.City;
                ctlCountry.Text = vendor.Country;
                ctlPostalCode.Text = vendor.PostalCode;

                CheckEditableVendor(vendor);

                CheckPersonalTaxFormat();

                if (InitialFlag == FlagEnum.NewFlag)
                    SetDefaultWHTType();
            }
            else
            {
                ctlVendorTaxNo.Text = string.Empty;
                ctlVendorCode.Text = string.Empty;
                ctlVendorBranch.Text = string.Empty;
                ctlVendorName.Text = string.Empty;
                ctlStreet.Text = string.Empty;
                ctlCity.Text = string.Empty;
                ctlCountry.Text = string.Empty;
                ctlPostalCode.Text = string.Empty;
            }

            ctlUpdateInvoicePanel.Update();
        }
        protected void ctlVendorTaxNo_TextChange(object sender, EventArgs e)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            string taxNo = ctlVendorTaxNo.Text;


            if (!string.IsNullOrEmpty(taxNo))
            {
                if (taxNo.Length != 13)
                {
                    errors.AddError("ValidationError", new Spring.Validation.ErrorMessage("TaxNo_IsInvalid"));
                    resetControl();
                }
                else
                {
                    CheckPersonalTaxFormat();
                    if (IsPeople)
                        ctlVendorBranch.Text = "00000";
                }
            }
            else
            {
                resetControl();
                ctlCountry.Text = "TH";
                ctlVendorCode.Text = ParameterServices.Default_Vendor;
                ctlVendorCode.Enabled = false;
                ctlClearVendor.Visible = false;
            }

            string branch = ctlVendorBranch.Text;
            if (ParameterServices.RequireVendorBranchCode)
            {
                if (string.IsNullOrEmpty(branch))
                {
                    ctlVendorBranch.Focus();
                    errors.AddError("ValidationError", new Spring.Validation.ErrorMessage("VendorBranch_IsRequired"));
                }
            }
            if (!errors.IsEmpty)
            {
                this.ValidationErrors.MergeErrors(errors);
            }
            else
            {
                IList<VOVendor> vender = ScgDbQueryProvider.DbVendorQuery.FindVendorByVendorTaxNO(taxNo, branch);
                if (vender.Count > 0)
                {
                    VOVendor vendor = vender.FirstOrDefault<VOVendor>(t => t.VendorTaxCode.Length == 13);

                    if (vendor == null)
                        vendor = vender.First<VOVendor>();

                    if (vendor.VendorTaxCode.Length == 13)
                    {
                        ctlVendorTaxNo.Text = vendor.VendorTaxCode;
                    }

                    ctlVendorCode.Text = vendor.VendorCode;
                    ctlVendorBranch.Text = vendor.VendorBranch;
                    ctlVendorName.Text = vendor.VendorName;
                    ctlStreet.Text = vendor.Street;
                    ctlCity.Text = vendor.City;
                    ctlCountry.Text = vendor.Country;
                    ctlPostalCode.Text = vendor.PostalCode;
                    this.VendorID = vendor.VendorID;

                    CheckEditableVendor(vendor);
                }
                else
                {
                    resetControl();
                    ctlCountry.Text = "TH";
                    ctlVendorCode.Text = ParameterServices.Default_Vendor;
                    ctlVendorCode.Enabled = false;
                    ctlClearVendor.Visible = false;
                }

                if (InitialFlag == FlagEnum.NewFlag)
                    SetDefaultWHTType();

                ctlUpdateInvoicePanel.Update();
            }

            ctlUpdatePanelInvoiceForm.Update();
        }
        protected void ctlVendorBranch_TextChange(object sender, EventArgs e)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            string taxNo = ctlVendorTaxNo.Text;
            string branch = ctlVendorBranch.Text;

            if (taxNo.Length != 13)
            {
                errors.AddError("ValidationError", new Spring.Validation.ErrorMessage("TaxNo_IsInvalid"));
                resetControl();
            }
            if (!string.IsNullOrEmpty(branch))
            {
                if (branch.Length != 5)
                {
                    errors.AddError("ValidationError", new Spring.Validation.ErrorMessage("VendorBranch_Mustbe_5_digit"));
                    resetControl();
                }
            }

            if (!errors.IsEmpty)
            {
                this.ValidationErrors.MergeErrors(errors);
            }
            else
            {
                IList<VOVendor> vender = ScgDbQueryProvider.DbVendorQuery.FindVendorByVendorTaxNO(taxNo, branch);
                if (vender.Count > 0)
                {
                    VOVendor vendor = vender.FirstOrDefault<VOVendor>(t => t.VendorTaxCode.Length == 13);

                    if (vendor == null)
                        vendor = vender.First<VOVendor>();

                    if (vendor.VendorTaxCode.Length == 13)
                    {
                        ctlVendorTaxNo.Text = vendor.VendorTaxCode;
                    }

                    ctlVendorCode.Text = vendor.VendorCode;
                    ctlVendorBranch.Text = vendor.VendorBranch;
                    ctlVendorName.Text = vendor.VendorName;
                    ctlStreet.Text = vendor.Street;
                    ctlCity.Text = vendor.City;
                    ctlCountry.Text = vendor.Country;
                    ctlPostalCode.Text = vendor.PostalCode;
                    this.VendorID = vendor.VendorID;

                    CheckEditableVendor(vendor);
                }
                else
                {
                    resetBranch();
                    ctlVendorCode.Text = ParameterServices.Default_Vendor;
                    ctlVendorCode.Enabled = false;
                    ctlClearVendor.Visible = false;
                }

                if (InitialFlag == FlagEnum.NewFlag)
                    SetDefaultWHTType();

                ctlUpdateInvoicePanel.Update();
            }

            ctlUpdatePanelInvoiceForm.Update();
        }
        private void resetBranch()
        {
            ctlVendorName.Text = string.Empty;
            this.VendorID = null;
            ctlClearVendor.Visible = true;
        }
        private void resetControl()
        {
            ctlVendorCode.Text = string.Empty;
            ctlVendorName.Text = string.Empty;
            ctlStreet.Text = string.Empty;
            ctlCity.Text = string.Empty;
            ctlCountry.Text = string.Empty;
            ctlPostalCode.Text = string.Empty;
            this.VendorID = null;

            ctlVendorCode.Enabled = true;
            ctlVendorBranch.Enabled = true;
            ctlVendorName.Enabled = true;
            ctlStreet.Enabled = true;
            ctlCity.Enabled = true;
            ctlCountry.Enabled = true;
            ctlPostalCode.Enabled = true;
            ctlClearVendor.Visible = true;
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
            this.ParentTxId = txID;
            this.ExpDocumentID = documentID;
            this.InitialFlag = initFlag;

            this.BindControl();
        }
        /// <summary>
        /// For initialize when call to Add new, Edit, View Invoice & InvoiceItem.
        /// Use for manage with data.
        /// </summary>
        /// <param name="initialFlag"></param>
        /// <param name="lineItemId"></param>
        /// <param name="txId"></param>
        public void Initialize(string initialFlag, long? lineItemId, Guid txId)
        {
            this.InitailizeInvoice(initialFlag, lineItemId, txId, InvoiceType.General);
        }

        public void InitailizeInvoice(string initialFlag, long? lineItemId, Guid txId, string invoiceType)
        {
            this.ParentTxId = txId;
            this.TxId = TransactionService.Begin(ParentTxId);

            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
            ExpenseDataSet.FnExpenseDocumentRow expRow = expDS.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);

            if (expRow != null)
                this.IsRepOffice = expRow.IsIsRepOfficeNull() ? false : expRow.IsRepOffice;

            if (initialFlag.Equals(FlagEnum.NewFlag))
            {
                Mode = FlagEnum.NewFlag.ToString();
                this.InvoiceId = FnExpenseInvoiceService.AddInvoiceOnTransaction(null, this.TxId);
            }
            else if ((initialFlag.Equals(FlagEnum.EditFlag) || initialFlag.Equals(FlagEnum.ViewFlag)) && ((lineItemId.HasValue) && (lineItemId.Value != 0)))
            {
                this.InvoiceId = lineItemId.Value;
            }
            this.InitialFlag = initialFlag;
            this.InvoiceDocumentType = invoiceType;

            Control currentControl = this;
            while (currentControl != null)
            {
                if (currentControl is IDocumentEditor)
                {
                    this.LinkControlVisible = ((IDocumentEditor)currentControl).IsContainVisibleFields(SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.VAT);
                    this.LinkControlEditable = ((IDocumentEditor)currentControl).IsContainEditableFields(SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.VAT);

                    break;
                }
                else
                {
                    currentControl = currentControl.Parent;
                }
            }

            this.BindControl();
        }
        #endregion

        #region Button Event
        protected void ctlVendorSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlVendorLookup.Show();
        }
        protected void ctlVendorCodeAPSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlVendorAPLookup.excludeOneTime = true;
            ctlVendorAPLookup.Show();
        }
        protected void ctlAdd_Click(object sender, ImageClickEventArgs e)
        {
            FnExpenseInvoiceItem item;
            //if (Mode.Equals(FlagEnum.NewFlag))
            //{
            item = new FnExpenseInvoiceItem();
            GetInvoiceItem(item);
            try
            {
                FnExpenseInvoiceItemService.AddInvoiceItemOnTransaction(item, this.TxId, this.DocumentType);
                BindInvoiceItemGridView();
                ctlInvoicefds.Style["display"] = "block";
                ctlRadioDiv.Style["display"] = "block";
                ctlUpdateInvoicePanel.Update();

                if (this.ValidationErrors.IsEmpty)
                {
                    this.ResetInvoiceItemForm();
                    this.BindInvoiceItemForm();
                    ctlUpdatePanelInvoiceForm.Update();
                }
                CalculateInvoice(false);
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
            //}


        }
        protected void ctlUpdateItem_Click(object sender, ImageClickEventArgs e)
        {
            TmpBaseAmount = UIHelper.ParseDouble(ctlBaseAmount.Text);
            FnExpenseInvoiceItem item = new FnExpenseInvoiceItem();
            GetInvoiceItem(item);
            try
            {
                FnExpenseInvoiceItemService.UpdateInvoiceItemOnTransaction(item, this.TxId, this.DocumentType);
                BindInvoiceItemGridView();
                ctlInvoicefds.Style["display"] = "block";
                ctlRadioDiv.Style["display"] = "block";
                ctlUpdateInvoicePanel.Update();
                ctlVendorAP.Style["display"] = "none";
                ctlAddItemButton.Style["display"] = "block";
                ctlEditItemButton.Style["display"] = "none";

                if (this.ValidationErrors.IsEmpty)
                {
                    this.ResetInvoiceItemForm();
                    this.BindInvoiceItemForm();
                    ctlUpdatePanelInvoiceForm.Update();
                }
                CalculateInvoice(false);
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
            }


        }
        protected void ctlCancelEditItem_Click(object sender, ImageClickEventArgs e)
        {
            ctlAddItemButton.Style["display"] = "block";
            ctlEditItemButton.Style["display"] = "none";
            ctlVendorAP.Style["display"] = "none";
            ctlVendorCodeAP.Text = string.Empty;
            if (this.ValidationErrors.IsEmpty)
            {
                this.ResetInvoiceItemForm();
                this.BindInvoiceItemForm();
                ctlUpdatePanelInvoiceForm.Update();
            }
        }

        protected void ctlCalculate_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.ValidateInvoice();
                this.CalculateInvoice(true);
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        protected void ctlSubmit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.ValidateInvoice();
                this.CalculateInvoice(false);
                FnExpenseInvoice inv = GetInvoice();

                // Validate TotalBaseAmount that are equal with summary of all invoice item amount.
                //FnExpenseInvoiceService.ValidateTotalBaseAmount(this.TxId, this.InvoiceId, inv);
                //FnExpenseInvoiceItemService.ReCalculateInvoiceItem(this.TxId, inv.InvoiceID);
                FnExpenseInvoiceService.UpdateInvoiceOnTransaction(inv, this.TxId);
                TransactionService.Commit(this.TxId);

                Hide();
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Submit", "notifyPopupResult('ok');", true);
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                //this.CheckVatAndWht();
            }
        }
        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            RollbackTransaction();
            //NotifyPopUpReturn(new PopUpReturnArgs(PopUpReturnType.Cancel, null));
            Hide();
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Close", "notifyPopupResult('cancel');", true);
        }
        protected void ctlClearVendor_Click(object sender, ImageClickEventArgs e)
        {
            VendorID = null;
            ctlVendorCode.Enabled = false;
            ctlVendorCode.Text = ParameterServices.Default_Vendor;
            ctlVendorBranch.Enabled = true;
            ctlVendorBranch.Text = string.Empty;
            ctlVendorName.Enabled = true;
            ctlStreet.Enabled = true;
            ctlCity.Enabled = true;
            ctlCountry.Enabled = true;
            ctlPostalCode.Enabled = true;
            ctlClearVendor.Visible = true;
        }
        #endregion

        #region GridView Event
        protected void ctlInvoiceItemGridview_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("EditItem") || e.CommandName.Equals("ViewItem"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                long itemId = UIHelper.ParseLong(ctlInvoiceItemGridview.DataKeys[rowIndex].Value.ToString());
                BindEditInvoiceItemControl(itemId);
            }
            if (e.CommandName.Equals("DeleteItem"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                long itemId = UIHelper.ParseLong(ctlInvoiceItemGridview.DataKeys[rowIndex].Value.ToString());
                FnExpenseInvoiceItemService.DeleteItemOnTransaction(this.TxId, itemId);
                ctlHideTotalAmountTHB.Value = string.Empty;
                BindInvoiceItemGridView();
            }
        }
        protected void ctlInvoiceItemGridview_DataBound(object sender, EventArgs e)
        {
            if (IsDomestic)
            {
                ctlInvoiceItemGridview.Columns[4].Visible = false;
                ctlInvoiceItemGridview.Columns[5].Visible = false;
                ctlInvoiceItemGridview.Columns[6].Visible = false;
            }
            else
            {
                ctlInvoiceItemGridview.Columns[4].Visible = true;
                ctlInvoiceItemGridview.Columns[5].Visible = true;
                ctlInvoiceItemGridview.Columns[6].Visible = true;
            }

            if (IsRepOffice)
            {
                long workFlowID = 0;
                if (!string.IsNullOrEmpty(Request.Params["wfid"]) && UIHelper.ParseInt(Request.Params["wfid"]) > 0)
                {
                    workFlowID = UIHelper.ParseLong(Request.Params["wfid"]);
                }
                SS.Standard.WorkFlow.DTO.WorkFlow workflow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
                ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
                ExpenseDataSet.FnExpenseDocumentRow expRow = expDs.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);

                ctlInvoiceItemGridview.Columns[7].Visible = true;  //local currency
                if (!expRow.IsLocalCurrencyIDNull())
                {
                    DbCurrency localCurrency = SsDbQueryProvider.DbCurrencyQuery.FindByIdentity(expRow.LocalCurrencyID);

                    if (localCurrency != null)
                        ctlInvoiceItemGridview.Columns[7].HeaderText = string.Format(GetMessage("AmountCurrencyHeaderColumn"), "(" + localCurrency.Symbol + ")");
                }

                if ((workflow != null && workflow.CurrentState.Ordinal >= 5) && (UserAccount.IsAccountant || UserAccount.IsPayment))
                {
                    if (!expRow.IsMainCurrencyIDNull() && !expRow.IsLocalCurrencyIDNull() && (expRow.MainCurrencyID == expRow.LocalCurrencyID))
                    {
                        DbCurrency mainCurrency = SsDbQueryProvider.DbCurrencyQuery.FindByIdentity(expRow.MainCurrencyID);
                        if (mainCurrency != null)
                        {
                            ctlInvoiceItemGridview.Columns[8].HeaderText = string.Format(GetMessage("AmountCurrencyHeaderColumn"), "(" + mainCurrency.Symbol + ")");
                        }

                        ctlInvoiceItemGridview.Columns[8].Visible = false;  //hide main currency
                    }
                    else
                    {
                        ctlInvoiceItemGridview.Columns[8].Visible = true;  //show main currency
                    }
                    ctlInvoiceItemGridview.Columns[9].Visible = true;  //THB
                }
                else
                {
                    ctlInvoiceItemGridview.Columns[8].Visible = false;
                    ctlInvoiceItemGridview.Columns[9].Visible = false;
                }
            }
            else
            {
                ctlInvoiceItemGridview.Columns[7].Visible = false;
                ctlInvoiceItemGridview.Columns[8].Visible = false;
                ctlInvoiceItemGridview.Columns[9].Visible = true;
            }
        }
        protected void ctlInvoiceItemGridview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton viewBtn = e.Row.FindControl("ctlView") as ImageButton;
                ImageButton editBtn = e.Row.FindControl("ctlEdit") as ImageButton;
                ImageButton deleteBtn = e.Row.FindControl("ctlDelete") as ImageButton;

                if (InitialFlag.Equals(FlagEnum.ViewFlag) && viewBtn != null)
                {
                    viewBtn.Visible = true;
                    editBtn.Visible = false;
                    deleteBtn.Visible = false;
                }
                else
                {
                    viewBtn.Visible = false;
                    editBtn.Visible = true;
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

                Literal amountLabel = e.Row.FindControl("ctlAmountTHBLabel1") as Literal;
                double amountTHB = UIHelper.ParseDouble(amountLabel.Text);
                this.TotalAmount += amountTHB;

                if (IsRepOffice)
                {
                    Literal amountLocalCurrencyLabel = e.Row.FindControl("ctlAmountLocalLabel1") as Literal;
                    double amountLocalCurrency = UIHelper.ParseDouble(amountLocalCurrencyLabel.Text);
                    this.TotalAmountLocalCurrency += amountLocalCurrency;

                    Literal amountMainCurrencyLabel = e.Row.FindControl("ctlAmountMainLabel1") as Literal;
                    double amountMainCurrency = UIHelper.ParseDouble(amountMainCurrencyLabel.Text);
                    this.TotalAmountMainCurrency += amountMainCurrency;
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                Literal total = e.Row.FindControl("ctlTotalAmountTHB") as Literal;
                total.Text = UIHelper.BindDecimal(TotalAmount.ToString());
                ctlHideTotalAmountTHB.Value = UIHelper.BindDecimal(TotalAmount.ToString());


                if (IsRepOffice)
                {
                    Literal totalLocalCurrency = e.Row.FindControl("ctlTotalAmountLocalCurrency") as Literal;
                    totalLocalCurrency.Text = UIHelper.BindDecimal(TotalAmountLocalCurrency.ToString());
                    ctlHideTotalAmountLocalCurrency.Value = UIHelper.BindDecimal(TotalAmountLocalCurrency.ToString());

                    Literal totalMainCurrency = e.Row.FindControl("ctlTotalAmountMainCurrency") as Literal;
                    totalMainCurrency.Text = UIHelper.BindDecimal(TotalAmountMainCurrency.ToString());
                    ctlHideTotalAmountMainCurrency.Value = UIHelper.BindDecimal(TotalAmountMainCurrency.ToString());

                    ctlBaseAmount.Text = UIHelper.BindDecimal(TotalAmountLocalCurrency.ToString());
                }
                else
                {
                    ctlBaseAmount.Text = UIHelper.BindDecimal(TotalAmount.ToString());
                }
                ctlUpdatePanelSummary.Update();
            }
        }
        #endregion

        #region Radio Button Event
        protected void ctlVatRdo_CheckedChanged(object sender, EventArgs e)
        {
            if (ctlVatRdo.Checked)
            {
                ctlSummary.Style["display"] = "inline-block";

                ctlInvoiceDiv.Style["display"] = "inline-block";
            }
            else
            {
                ctlInvoiceDiv.Style["display"] = "none";
            }
            ctlUpdateInvoicePanel.Update();
        }
        protected void ctlNonVatRdo_CheckedChanged(object sender, EventArgs e)
        {
            if ((ctlNonVatRdo.Checked) && !(ctlWhtRdo.Checked))
            {
                ctlSummary.Style["display"] = "none";
                ctlVatAmount.Text = string.Empty;
                ctlWHTAmount.Text = string.Empty;

                ctlInvoiceDiv.Style["display"] = "none";
                ResetInvoiceForm();
            }
            else
            {
                ctlVatAmount.Text = string.Empty;
            }
            ctlUpdatePanelSummary.Update();
            ctlUpdateInvoicePanel.Update();
        }
        protected void ctlWhtRdo_CheckedChanged(object sender, EventArgs e)
        {
            if (ctlWhtRdo.Checked)
            {
                ctlWhtFds.Style["display"] = "inline-block";

                //if (ctlVatRdo.Checked)
                //{
                ctlSummary.Style["display"] = "inline-block";

                //}
                //else
                //{
                //    ctlSummary.Style["display"] = "none";
                //    ctlUpdatePanelSummary.Update();
                //}

                ctlInvoiceDiv.Style["display"] = "inline-block";
            }
            else
            {
                ctlInvoiceDiv.Style["display"] = "none";
            }
            ctlUpdatePanelSummary.Update();
            ctlUpdatePanelWHTSummary.Update();
        }
        protected void ctlNonWhtRdo_CheckedChanged(object sender, EventArgs e)
        {
            if (ctlNonWht.Checked)
            {
                ctlWHTAmount.Text = string.Empty;
                ctlWhtFds.Style["display"] = "none";


                if (ctlVatRdo.Checked)
                {
                    ctlSummary.Style["display"] = "inline-block";
                }
                else
                {
                    ctlVatAmount.Text = string.Empty;
                    ctlInvoiceDiv.Style["display"] = "none";
                    ctlSummary.Style["display"] = "none";
                    ResetInvoiceForm();
                }
            }
            ctlUpdatePanelSummary.Update();
            ctlUpdatePanelWHTSummary.Update();
        }
        #endregion

        #region Public Method
        public void RollbackTransaction()
        {
            TransactionService.Rollback(this.TxId);
            ctlInvoiceItemGridview.DataSource = null;
            ctlInvoiceItemGridview.DataBind();
            ctlRadioDiv.Style["display"] = "none";
            ctlInvoicefds.Style["display"] = "none";
            ctlWhtFds.Style["display"] = "none";
            ctlSummary.Style["display"] = "none";
        }
        public void BindEditInvoiceItemControl(long itemId)
        {
            ctlAddItemButton.Style["display"] = "none";
            ctlEditItemButton.Style["display"] = "block";
            ctlVendorAP.Style["display"] = "none";
            ctlVendorCodeAP.Text = String.Empty;
            InvoiceItemID = itemId;
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
            ExpenseDataSet.FnExpenseDocumentRow expRow = expDS.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);
            ExpenseDataSet.FnExpenseInvoiceItemRow itemRow = expDS.FnExpenseInvoiceItem.FindByInvoiceItemID(InvoiceItemID);
            ctlCostCenterField.ResetValue();
            if (!itemRow.IsCostCenterIDNull())
            {
                ctlCostCenterField.BindCostCenterControl(itemRow.CostCenterID);
            }

            ctlAccountField.ResetValue();
            if (!itemRow.IsAccountIDNull())
                ctlAccountField.BindAccountControl(itemRow.AccountID);

            ctlIOAutoCompleteLookup.ResetValue();
            ctlIOAutoCompleteLookup.CostCenterId = UIHelper.ParseLong(ctlCostCenterField.CostCenterId);
            if (!itemRow.IsIOIDNull())
                ctlIOAutoCompleteLookup.BindIOControl(itemRow.IOID);

            ctlSaleItem.Text = itemRow["SaleItem"].ToString();
            ctlSaleOrder.Text = itemRow["SaleOrder"].ToString();
            ctlDescription.Text = itemRow["Description"].ToString();
            ctlReferanceNo.Text = itemRow["ReferenceNo"].ToString();
            ctlVendorCodeAP.Text = itemRow["VendorCodeAP"].ToString();
            DbAccount db = ScgDbQueryProvider.DbAccountQuery.FindByIdentity(itemRow.AccountID);
            if (db.SaveAsVendor)
            {
                ctlVendorAP.Style["display"] = "block";
            }
            if (DocumentType.Equals(ZoneType.Foreign))
            {
                ctlAmount.Text = UIHelper.BindDecimal(itemRow.CurrencyAmount.ToString());
            }
            else
            {
                if (!IsRepOffice)
                {
                    ctlAmount.Text = UIHelper.BindDecimal(itemRow.Amount.ToString());
                }
                else
                {
                    ctlAmount.Text = UIHelper.BindDecimal(itemRow.LocalCurrencyAmount.ToString());
                }
            }
            if (!string.IsNullOrEmpty(itemRow["CurrencyID"].ToString()))
                ctlCurrency.BindCurrency((short)itemRow.CurrencyID);
            //ctlCurrencyDrowdown.SelectedValue = itemRow.CurrencyID.ToString();

            if (ctlCurrency.SelectedValue == ParameterServices.USDCurrencyID.ToString() && UIHelper.ParseDecimal(expRow["ExchangeRateForUSDAdvance"].ToString()) > 0)
            {
                if (!string.IsNullOrEmpty(itemRow["ExchangeRate"].ToString()))
                {
                    ctlAdvanceExchangeRate.Text = UIHelper.BindExchangeRate(itemRow.ExchangeRate.ToString());
                    ctlExchangeRate.ReadOnly = true;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(itemRow["ExchangeRate"].ToString()))
                {
                    ctlExchangeRate.Text = UIHelper.BindExchangeRate(itemRow.ExchangeRate.ToString());
                    ctlExchangeRate.ReadOnly = false;
                }
            }

            ctlUpdatePanelInvoiceForm.Update();
        }
        public void ChangeModeButton(string mode)
        {
            if (mode.Equals(FlagEnum.ViewFlag))
            {
                ctlAddItemButton.Style["display"] = "none";
                ctlEditItemButton.Style["display"] = "block";
                ctlEditItemButton.Visible = false;
                ctlCalculate.Visible = false;
                ctlSubmit.Visible = false;
            }
            else
            {
                if (mode.Equals(FlagEnum.EditFlag))
                {
                    ctlAddItemButton.Style["display"] = "block";
                    ctlEditItemButton.Style["display"] = "none";
                }
                else
                {
                    ctlAddItemButton.Style["display"] = "block";
                    ctlEditItemButton.Style["display"] = "none";
                }
                ctlEditItemButton.Visible = true;
                ctlCalculate.Visible = true;
                ctlSubmit.Visible = true;
            }
        }
        public void BindInvoiceForm()
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
            ExpenseDataSet.FnExpenseDocumentRow expenseDocumentRow = expDS.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);
            ExpenseDataSet.FnExpenseInvoiceRow row = expDS.FnExpenseInvoice.FindByInvoiceID(InvoiceId);

            ctlInvoicefds.Style["display"] = "block";
            ctlRadioDiv.Style["display"] = "block";


            this.SetVat();
            this.SetWHT();

            ctlInvoiceNo.Text = row.IsInvoiceNoNull() ? string.Empty : row.InvoiceNo.ToString();
            ctlInvoiceDate.Value = row.IsInvoiceDateNull() ? null : (DateTime?)row.InvoiceDate;
            ctlInvoiceDescription.Text = row.IsDescriptionNull() ? string.Empty : row.Description.ToString();
            ctlBranch.Text = row.IsBranchCodeNull() ? string.Empty : row.BranchCode.ToString();
            //Vendor
            if (InitialFlag == FlagEnum.ViewFlag)
                ctlVendorSearch.Visible = false;
            else
                ctlVendorSearch.Visible = this.LinkControlEditable;

            ctlVendorTaxNo.Text = row.IsVendorTaxCodeNull() ? string.Empty : row.VendorTaxCode;
            ctlVendorCode.Text = row.IsVendorCodeNull() ? string.Empty : row.VendorCode;
            ctlVendorBranch.Text = row.IsVendorBranchNull() ? string.Empty : row.VendorBranch;
            ctlVendorName.Text = row.IsVendorNameNull() ? string.Empty : row.VendorName;
            ctlStreet.Text = row.IsStreetNull() ? string.Empty : row.Street;
            ctlCity.Text = row.IsCityNull() ? string.Empty : row.City;
            ctlCountry.Text = row.IsCountryNull() ? string.Empty : row.Country;
            ctlPostalCode.Text = row.IsPostalCodeNull() ? string.Empty : row.PostalCode;
            if (!row.IsVendorIDNull())
            {
                ctlVendorCode.Enabled = false;
                ctlVendorBranch.Enabled = false;
                ctlVendorName.Enabled = false;
                ctlStreet.Enabled = false;
                ctlCity.Enabled = false;
                ctlCountry.Enabled = false;
                ctlPostalCode.Enabled = false;
                ctlClearVendor.Visible = true;
            }
            else
            {
                if (ctlVendorCode.Text.Equals(ParameterServices.Default_Vendor))
                {
                    ctlVendorCode.Enabled = false;
                    ctlClearVendor.Visible = false;
                }
                else
                {
                    ctlVendorCode.Enabled = true;
                    ctlClearVendor.Visible = true;
                }
                ctlVendorName.Enabled = true;
                ctlStreet.Enabled = true;
                ctlCity.Enabled = true;
                ctlCountry.Enabled = true;
                ctlPostalCode.Enabled = true;
            }

            this.BindWHTTypeDropdownControl();

            // WHT & VAT
            if (!row.IsWHTID1Null())
            {
                ctlWhtRateDropDown.SelectedValue = row.WHTID1.ToString();
            }
            else
            {
                DbWithHoldingTax wht = ScgDbQueryProvider.DbWithHoldingTaxQuery.FindWithHoldingTaxByWhtCode(ParameterServices.DefaultWHTCode);
                if (wht != null)
                    ctlWhtRateDropDown.SelectedValue = wht.Whtid.ToString();
            }

            if (!row.IsWHTID2Null())
                ctlWhtRateDropDown2.SelectedValue = row.WHTID2.ToString();

            if (!row.IsWHTAmount1Null())
                ctlWhtAmount1.Text = UIHelper.BindDecimal(row.WHTAmount1.ToString());

            if (!row.IsWHTAmount2Null())
                ctlWhtAmount2.Text = UIHelper.BindDecimal(row.WHTAmount2.ToString());

            if (!row.IsBaseAmount1Null())
                ctlBaseAmount1.Text = UIHelper.BindDecimal(row.BaseAmount1.ToString());

            if (!row.IsBaseAmount2Null())
                ctlBaseAmount2.Text = UIHelper.BindDecimal(row.BaseAmount2.ToString());

            if (!row.IsWHTTypeID1Null())
                ctlWHTTypeDropdown.SelectedValue = row.WHTTypeID1.ToString();
            if (!row.IsWHTTypeID2Null())
                ctlWHTTypeDropDown2.SelectedValue = row.WHTTypeID2.ToString();

            //Summary
            if (!row.IsTotalBaseAmountNull())
                ctlBaseAmount.Text = UIHelper.BindDecimal(row.TotalBaseAmount.ToString());

            if (!row.IsVatAmountNull())
                ctlVatAmount.Text = UIHelper.BindDecimal(row.VatAmount.ToString());

            if (!row.IsWHTAmountNull())
                ctlWHTAmount.Text = UIHelper.BindDecimal(row.WHTAmount.ToString());

            if (!row.IsNetAmountNull())
                ctlNetAmount.Text = UIHelper.BindDecimal(row.NetAmount.ToString());


            if (!row.IsTaxIDNull())
            {
                if (ctlVatRdo.Checked)
                {
                    ctlTaxCodeDropDown.SelectedValue = row.TaxID.ToString();
                }
                else if (InitialFlag == FlagEnum.ViewFlag)
                {
                    ctlTaxCodeDropDown.Items.Insert(0, new ListItem(string.Empty, string.Empty));
                    ctlTaxCodeDropDown.SelectedIndex = 0;
                }
            }
            else
            {
                if (InitialFlag != FlagEnum.ViewFlag)
                {
                    DbCompany company = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(expenseDocumentRow.DocumentRow.CompanyID);
                    if (company.DefaultTaxID.HasValue && company.DefaultTaxID.Value != 0)
                    {
                        ctlTaxCodeDropDown.SelectedValue = company.DefaultTaxID.ToString();
                    }
                    else
                    {
                        DbTax tax = ScgDbQueryProvider.DbTaxQuery.FindbyTaxCode(ParameterServices.DefaultTaxCode);
                        ctlTaxCodeDropDown.SelectedValue = tax.TaxID.ToString();
                    }
                }
                else
                {
                    ctlTaxCodeDropDown.Items.Insert(0, new ListItem(string.Empty, string.Empty));
                    ctlTaxCodeDropDown.SelectedIndex = 0;
                }
            }



            if (UserAccount.IsAccountant || UserAccount.IsPayment)
            {
                ctlVatAmount.ReadOnly = false;
            }
            else
            {
                ctlVatAmount.ReadOnly = true;
            }

            ctlUpdateInvoicePanel.Update();
            ctlUpdatePanelWHTSummary.Update();
            ctlUpdatePanelSummary.Update();
        }
        public void BindInvoiceItemForm()
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
            ExpenseDataSet.FnExpenseDocumentRow expenseDocumentRow = expDS.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);
            long requesterID = expenseDocumentRow.DocumentRow.RequesterID;
            long? requesterCompanyID = null;

            SuUser requester = SS.SU.Query.QueryProvider.SuUserQuery.FindByIdentity(requesterID);
            if (requester != null && requester.Company != null)
                requesterCompanyID = requester.Company.CompanyID;

            ctlCostCenterField.CompanyId = expenseDocumentRow.DocumentRow.CompanyID;
            ctlAccountField.CompanyIDofDocument = expenseDocumentRow.DocumentRow.CompanyID;
            ctlIOAutoCompleteLookup.CompanyId = expenseDocumentRow.DocumentRow.CompanyID;

            ctlAccountField.WithoutExpenseCode = ParameterServices.AccountMileageExtra + "," + ParameterServices.AccountPerdiem;

            if (ParameterServices.EnableInvoiceExcludePerdiemDomesticAccount)
            {
                ctlAccountField.WithoutExpenseCode += "," + ParameterServices.AccountPerdiem_DM;
            }

            if (requester != null && requester.CostCenter != null)
            {
                if (InitialFlag.Equals(FlagEnum.NewFlag))
                {
                    ctlCostCenterField.BindCostCenterControl(requester.CostCenter.CostCenterID);
                    ctlIOAutoCompleteLookup.CostCenterId = requester.CostCenter.CostCenterID;
                }
            }
        }
        public void BindInvoiceItemGridView()
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
            if (InvoiceId != 0)
            {
                string filter = String.Format("InvoiceID = {0}", this.InvoiceId);
                ctlInvoiceItemGridview.DataSource = expDS.FnExpenseInvoiceItem.Select(filter);
            }
            ctlInvoiceItemGridview.DataBind();
        }
        public void ShowForeignField()
        {
            ctlTableInForm.Rows[6].Visible = true;
            ctlTableInForm.Rows[8].Visible = true;
            ctlTableInForm.Rows[9].Visible = true;
        }
        public void ShowDomesticField()
        {
            ctlTableInForm.Rows[6].Visible = false;
            ctlTableInForm.Rows[8].Visible = false;
            ctlTableInForm.Rows[9].Visible = false;
        }
        public void Show()
        {
            CallOnObjectLookUpCalling();
            this.GetAdvanceExchangeRate();
            this.ctlUpdatePanelInvoiceForm.Update();
        }
        public void Hide()
        {
            this.ResetInvoiceItemForm();
            this.ResetInvoiceItemGridView();
            this.ResetVatAndWHT();
            this.ResetInvoiceForm();
            this.ResetWitholdingTaxForm();
            this.ResetSummary();

            ctlUpdatePanelInvoiceForm.Update();
            ctlUpdateInvoicePanel.Update();
            ctlUpdatePanelWHTSummary.Update();
            ctlUpdatePanelSummary.Update();
        }
        public string DisplayCurrency(Object obj)
        {
            ExpenseDataSet.FnExpenseInvoiceItemRow row = (ExpenseDataSet.FnExpenseInvoiceItemRow)obj;
            if (!string.IsNullOrEmpty(row["CurrencyID"].ToString()))
            {
                DbCurrency currency = SsDbQueryProvider.DbCurrencyQuery.FindByIdentity(UIHelper.ParseShort(row["CurrencyID"].ToString()));
                if (currency != null)
                    return Server.HtmlEncode(currency.Symbol);
            }
            return string.Empty;
        }
        public string DisplayIO(Object obj)
        {
            ExpenseDataSet.FnExpenseInvoiceItemRow row = (ExpenseDataSet.FnExpenseInvoiceItemRow)obj;
            if (!string.IsNullOrEmpty(row["IOID"].ToString()))
            {
                DbInternalOrder io = ScgDbQueryProvider.DbIOQuery.FindByIdentity(row.IOID);
                if (io != null)
                    return Server.HtmlEncode(io.IONumber);
            }
            return string.Empty;
        }
        public string DisplayAccount(Object obj)
        {
            ExpenseDataSet.FnExpenseInvoiceItemRow row = (ExpenseDataSet.FnExpenseInvoiceItemRow)obj;
            if (!string.IsNullOrEmpty(row["AccountID"].ToString()))
            {
                IList<AccountLang> account = ScgDbQueryProvider.DbAccountLangQuery.FindByDbAccountLangKey(row.AccountID, UserAccount.CurrentLanguageID);
                if (account.Count > 0)
                    return Server.HtmlEncode(String.Format("{0}-{1}", account[0].AccountCode, account[0].AccountName));
            }
            return string.Empty;
        }
        public string DisplayCostCenter(Object obj)
        {
            ExpenseDataSet.FnExpenseInvoiceItemRow row = (ExpenseDataSet.FnExpenseInvoiceItemRow)obj;
            if (!string.IsNullOrEmpty(row["CostCenterID"].ToString()))
            {
                DbCostCenter costCenter = ScgDbQueryProvider.DbCostCenterQuery.FindByIdentity(row.CostCenterID);
                if (costCenter != null)
                    return Server.HtmlEncode(costCenter.CostCenterCode);
            }
            return string.Empty;
        }
        #endregion

        #region Private Method

        private void BindTaxCodeDropdown()
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
            ExpenseDataSet.FnExpenseDocumentRow expenseDocumentRow = expDS.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);
            ctlTaxCodeDropDown.DataSource = ScgDbQueryProvider.DbTaxQuery.GetTaxCodeActiveByCompany(expenseDocumentRow.DocumentRow.CompanyID);
            ctlTaxCodeDropDown.DataTextField = "TaxCode";
            ctlTaxCodeDropDown.DataValueField = "TaxID";
            ctlTaxCodeDropDown.DataBind();

            DbCompany company = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(expenseDocumentRow.DocumentRow.CompanyID);
            if (company.DefaultTaxID.HasValue && company.DefaultTaxID.Value != 0)
            {
                ctlTaxCodeDropDown.SelectedValue = company.DefaultTaxID.ToString();
            }
            else
            {
                DbTax tax = ScgDbQueryProvider.DbTaxQuery.FindbyTaxCode(ParameterServices.DefaultTaxCode);
                ctlTaxCodeDropDown.SelectedValue = tax.TaxID.ToString();
            }
        }
        private void BindWhtRateDropdownControl()
        {
            ctlWhtRateDropDown.BindDropDown();
            ctlWhtRateDropDown2.BindDropDown();
            DbWithHoldingTax wht = ScgDbQueryProvider.DbWithHoldingTaxQuery.FindWithHoldingTaxByWhtCode(ParameterServices.DefaultWHTCode);
            if (wht != null)
                ctlWhtRateDropDown.SelectedValue = wht.Whtid.ToString();
        }
        private void BindWHTTypeDropdownControl()
        {
            ctlWHTTypeDropdown.BindDropDown();
            ctlWHTTypeDropDown2.BindDropDown();

            ctlUpdatePanelWHTSummary.Update();
        }
        private void GetAdvanceExchangeRate()
        {
            short currencyID = UIHelper.ParseShort(ctlCurrency.SelectedValue);
            InvoiceExchangeRate invoiceExchangeRate = null;

            ctlAdvanceExchangeRate.Style["background-color"] = "Silver";
            ctlAdvanceExchangeRate.Style["vertical-align"] = "middle";
            ctlAdvanceExchangeRate.BorderStyle = BorderStyle.Solid;
            ctlAdvanceExchangeRate.BorderWidth = 1;

            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
            ExpenseDataSet.FnExpenseDocumentRow expenseDocumentRow = expDS.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);

            if (!IsRepOffice)
            {
                invoiceExchangeRate = FnExpenseDocumentService.GetAdvanceExchangeRate(this.TxId, currencyID);
            }
            else
            {
                invoiceExchangeRate = FnExpenseDocumentService.GetAdvanceExchangeRateRepOffice(this.TxId, currencyID, this.ExpDocumentID);
            }

            if (invoiceExchangeRate == null)
            {
                if (IsRepOffice)
                {
                    if (currencyID == expenseDocumentRow.LocalCurrencyID)
                    {
                        ctlExchangeRate.Text = UIHelper.BindExchangeRate("1");
                        ctlExchangeRate.ReadOnly = true;
                    }
                    else if (currencyID == expenseDocumentRow.MainCurrencyID)
                    {
                        double exRate = ScgDbQueryProvider.DbPbRateQuery.GetExchangeRate(expenseDocumentRow.PBID, expenseDocumentRow.MainCurrencyID, expenseDocumentRow.LocalCurrencyID);
                        if (exRate != 0)
                        {
                            ctlExchangeRate.Text = UIHelper.BindExchangeRate(exRate.ToString());
                            ctlExchangeRate.ReadOnly = true;
                        }
                    }
                    else
                    {

                        ctlAdvanceExchangeRate.Text = string.Empty;
                        ctlExchangeRate.Text = string.Empty;
                        ctlExchangeRate.ReadOnly = false;

                    }
                }
                else
                {
                    ctlAdvanceExchangeRate.Text = string.Empty;
                    ctlExchangeRate.Text = string.Empty;
                    ctlExchangeRate.ReadOnly = false;
                }
            }
            else
            {
                if (invoiceExchangeRate.AdvanceExchangeRateAmount == 0)
                {
                    ctlAdvanceExchangeRate.Text = string.Empty;
                    ctlExchangeRate.Text = string.Empty;
                    ctlExchangeRate.ReadOnly = false;
                }
                else
                {
                    if (IsRepOffice && expenseDocumentRow.LocalCurrencyID != expenseDocumentRow.MainCurrencyID)
                    {
                        ctlAdvanceExchangeRate.Text = string.Empty;
                        if (currencyID == expenseDocumentRow.MainCurrencyID)
                        {
                            ctlExchangeRate.Text = UIHelper.BindExchangeRate(expenseDocumentRow.ExchangeRateForLocalCurrency.ToString());
                            ctlExchangeRate.ReadOnly = true;
                        }
                        else if (currencyID == expenseDocumentRow.LocalCurrencyID)
                        {
                            ctlExchangeRate.Text = UIHelper.BindExchangeRate("1");
                            ctlExchangeRate.ReadOnly = true;
                        }
                        else
                        {
                            ctlAdvanceExchangeRate.Text = string.Empty;
                            ctlExchangeRate.Text = string.Empty;
                            ctlExchangeRate.ReadOnly = false;
                        }
                    }
                    else
                    {
                        ctlAdvanceExchangeRate.Text = UIHelper.BindExchangeRate(invoiceExchangeRate.AdvanceExchangeRateAmount.ToString());
                        ctlExchangeRate.Text = string.Empty;
                        ctlExchangeRate.ReadOnly = true;
                    }
                }
            }

            if (!ctlExchangeRate.ReadOnly)
            {
                if (UIHelper.ParseDecimal(ctlExchangeRate.Text) == 0)
                {
                    ctlExchangeRate.Text = string.Empty;
                }
                ctlExchangeRate.ReadOnly = false;
                ctlExchangeRate.BackColor = System.Drawing.Color.Transparent;
            }
            else
            {
                ctlExchangeRate.ReadOnly = true;
                ctlExchangeRate.ForeColor = System.Drawing.Color.Black;
                ctlExchangeRate.BackColor = System.Drawing.Color.Silver;
            }
            // Update Panel.
            ctlUpdatePanelInvoiceForm.Update();
        }
        private void GetInvoiceItem(FnExpenseInvoiceItem item)
        {
            ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
            ExpenseDataSet.FnExpenseDocumentRow expRow = expDs.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);

            item.Invoice = new FnExpenseInvoice(InvoiceId);
            item.Invoice.Expense = new FnExpenseDocument(this.ExpDocumentID);
            item.InvoiceItemID = InvoiceItemID;
            if (!string.IsNullOrEmpty(ctlCostCenterField.CostCenterId))
            {
                item.CostCenter = new DbCostCenter(UIHelper.ParseLong(ctlCostCenterField.CostCenterId));
            }
            if (!string.IsNullOrEmpty(ctlAccountField.AccountID))
            {
                item.Account = new DbAccount(UIHelper.ParseLong(ctlAccountField.AccountID));
            }
            if (!string.IsNullOrEmpty(ctlIOAutoCompleteLookup.IOID))
            {
                item.IO = new DbInternalOrder(UIHelper.ParseLong(ctlIOAutoCompleteLookup.IOID));
            }
            item.VendorCodeAP = ctlVendorCodeAP.Text;
            item.SaleOrder = ctlSaleOrder.Text;
            item.SaleItem = ctlSaleItem.Text;
            item.Description = ctlDescription.Text;

            if (!string.IsNullOrEmpty(ctlAmount.Text))
            {
                if (DocumentType.Equals(ZoneType.Domestic) || string.IsNullOrEmpty(DocumentType))
                {
                    if (!IsRepOffice)
                    {
                        item.Amount = UIHelper.ParseDouble(ctlAmount.Text);
                    }
                    else
                    {
                        item.LocalCurrencyAmount = UIHelper.ParseDouble(ctlAmount.Text);
                    }
                }
                else if (DocumentType.Equals(ZoneType.Foreign))
                {
                    item.ExchangeRate = UIHelper.ParseDouble(ctlAdvanceExchangeRate.Text);
                    if (item.ExchangeRate == (double)0)
                    {
                        double exchangeRate = UIHelper.ParseDouble(ctlExchangeRate.Text);
                        item.ExchangeRate = exchangeRate;
                    }
                    double currencyAmount = UIHelper.ParseDouble(ctlAmount.Text);
                    double amount = (double)Math.Round((decimal)(item.ExchangeRate * currencyAmount).GetValueOrDefault(0), 2, MidpointRounding.AwayFromZero);

                    item.CurrencyAmount = currencyAmount;

                    if (!IsRepOffice)
                    {
                        item.Amount = amount;
                    }
                    else
                    {
                        item.LocalCurrencyAmount = amount;
                    }

                    item.CurrencyID = UIHelper.ParseLong(ctlCurrency.SelectedValue);
                }
            }

            if (IsRepOffice)
            {
                if (expRow.MainCurrencyID == expRow.LocalCurrencyID)
                {
                    item.MainCurrencyAmount = item.LocalCurrencyAmount;
                }
                else
                {
                    if (!expRow.IsExchangeRateForLocalCurrencyNull() && expRow.ExchangeRateForLocalCurrency != 0)
                    {
                        item.MainCurrencyAmount = (double)Math.Round((decimal)((!item.LocalCurrencyAmount.HasValue ? 0 : item.LocalCurrencyAmount.Value) / (double)expRow.ExchangeRateForLocalCurrency), 2, MidpointRounding.AwayFromZero);
                    }
                }

                if (!expRow.IsExchangeRateMainToTHBCurrencyNull())
                {
                    item.Amount = (double)Math.Round((decimal)((!item.MainCurrencyAmount.HasValue ? 0 : item.MainCurrencyAmount.Value) * (double)expRow.ExchangeRateMainToTHBCurrency), 2, MidpointRounding.AwayFromZero);
                }
            }

            item.ReferenceNo = ctlReferanceNo.Text;
        }
        private FnExpenseInvoice GetInvoice()
        {
            ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
            ExpenseDataSet.FnExpenseDocumentRow expRow = expDs.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);

            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            FnExpenseInvoice invoice = new FnExpenseInvoice(InvoiceId);
            invoice.Expense = new FnExpenseDocument(ExpDocumentID);
            invoice.InvoiceDocumentType = InvoiceDocumentType;
            invoice.InvoiceNo = ctlInvoiceNo.Text;
            try
            {
                invoice.InvoiceDate = UIHelper.ParseDate(ctlInvoiceDate.DateValue);
            }
            catch (FormatException fex)
            {
                errors.AddError("ValidationError", new Spring.Validation.ErrorMessage("Invalid_Date_Format"));
                throw new ServiceValidationException(errors);
            }
            invoice.Description = ctlInvoiceDescription.Text;
            invoice.BranchCode = ctlBranch.Text;

            if (VendorID.HasValue)
            {
                invoice.VendorID = VendorID.Value;
            }
            else
            {
                invoice.VendorID = null;
            }

            invoice.VendorTaxCode = ctlVendorTaxNo.Text;
            invoice.VendorCode = ctlVendorCode.Text;
            invoice.VendorBranch = ctlVendorBranch.Text;
            invoice.VendorName = ctlVendorName.Text;
            invoice.Street = ctlStreet.Text;
            invoice.City = ctlCity.Text;
            invoice.Country = ctlCountry.Text;
            invoice.PostalCode = ctlPostalCode.Text;

            if (!ctlVatRdo.Checked && !ctlNonVatRdo.Checked)
                invoice.IsVAT = null;
            else
            {
                invoice.IsVAT = ctlVatRdo.Checked;
                // Set TaxID of invoice.
                if (!ctlTaxCodeDropDown.SelectedValue.Equals("-1"))
                {
                    invoice.TaxID = UIHelper.ParseLong(ctlTaxCodeDropDown.SelectedValue);
                }
            }
            if (!ctlWhtRdo.Checked && !ctlNonWht.Checked)
                invoice.IsWHT = null;
            else
            {
                invoice.IsWHT = ctlWhtRdo.Checked;
                if (!string.IsNullOrEmpty(ctlBaseAmount1.Text) && !UIHelper.ParseDouble(ctlBaseAmount1.Text).Equals(0) && !ctlWHTTypeDropdown.SelectedValue.Equals("-1"))
                {
                    invoice.WHTTypeID1 = UIHelper.ParseLong(ctlWHTTypeDropdown.SelectedValue);
                }
                else
                    invoice.WHTTypeID1 = null;
                if (!string.IsNullOrEmpty(ctlBaseAmount2.Text) && !UIHelper.ParseDouble(ctlBaseAmount2.Text).Equals(0) && !ctlWHTTypeDropDown2.SelectedValue.Equals("-1"))
                {
                    invoice.WHTTypeID2 = UIHelper.ParseLong(ctlWHTTypeDropDown2.SelectedValue);
                }
                else
                    invoice.WHTTypeID2 = null;

            }

            invoice.BaseAmount1 = UIHelper.ParseDouble(ctlBaseAmount1.Text);
            invoice.BaseAmount2 = UIHelper.ParseDouble(ctlBaseAmount2.Text);
            invoice.WHTAmount1 = UIHelper.ParseDouble(ctlWhtAmount1.Text);
            invoice.WHTAmount2 = UIHelper.ParseDouble(ctlWhtAmount2.Text);
            if (!ctlWhtRateDropDown.SelectedValue.Equals("-1"))
            {
                DbWithHoldingTax wht = ScgDbQueryProvider.DbWithHoldingTaxQuery.FindByIdentity(UIHelper.ParseLong(ctlWhtRateDropDown.SelectedValue));
                invoice.WHTID1 = wht.Whtid;
                invoice.WHTRate1 = wht.Rate;
            }
            if (!ctlWhtRateDropDown2.SelectedValue.Equals("-1"))
            {
                DbWithHoldingTax wht = ScgDbQueryProvider.DbWithHoldingTaxQuery.FindByIdentity(UIHelper.ParseLong(ctlWhtRateDropDown2.SelectedValue));
                invoice.WHTID2 = wht.Whtid;
                invoice.WHTRate2 = wht.Rate;
            }

            // Set value of all Amount data.

            invoice.TotalAmount = UIHelper.ParseDouble(ctlHideTotalAmountTHB.Value);	// TotalAmount is summary amount of all InvoiceItem.

            if (IsRepOffice)
            {
                invoice.TotalAmountLocalCurrency = invoice.TotalBaseAmountLocalCurrency = invoice.NetAmountLocalCurrency = UIHelper.ParseDouble(ctlHideTotalAmountLocalCurrency.Value);
                invoice.NetAmountLocalCurrency = UIHelper.ParseDouble(ctlNetAmount.Text);
            }
            else
            {
                invoice.VatAmount = UIHelper.ParseDouble(ctlVatAmount.Text);
                invoice.WHTAmount = UIHelper.ParseDouble(ctlWHTAmount.Text);

                // TotalBaseAmount is TotalAmount + NonDeductAmount >>> get value from ctlBaseAmount.
                invoice.NonDeductAmount = UIHelper.ParseDouble(ctlNonDeductAmount.Value);	// NonDeductAmount is calculate from Total amount when click calculate button.
                invoice.TotalBaseAmount = UIHelper.ParseDouble(ctlBaseAmount.Text);
                invoice.NetAmount = UIHelper.ParseDouble(ctlNetAmount.Text);
            }

            return invoice;
        }
        private void ValidateInvoice()
        {
            Spring.Validation.ValidationErrors error = new Spring.Validation.ValidationErrors();

            FnExpenseInvoice invoice = this.GetInvoice();

            if (ctlInvoiceItemGridview.Rows.Count == 0)
            {
                error.AddError("ValidationError", new Spring.Validation.ErrorMessage("CannotSaveInvoice_NoItem"));
            }
            if (!error.IsEmpty) throw new ServiceValidationException(error);

            if (!IsRepOffice)
            {
                try
                {
                    FnExpenseInvoiceService.ValidateCalculateInvoice(invoice);
                }
                catch (ServiceValidationException ex)
                {
                    throw ex;
                }
            }
        }
        private void CalculateInvoice(bool isCal)
        {
            Spring.Validation.ValidationErrors error = new Spring.Validation.ValidationErrors();
            double baseAmount = UIHelper.ParseDouble(ctlHideTotalAmountTHB.Value);
            double vatAmount = UIHelper.ParseDouble(ctlVatAmount.Text);
            double whtAmount = 0;
            double netAmount = 0;
            double nonDeductAmount = 0;
            double newBaseAmount = baseAmount;

            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
            ExpenseDataSet.FnExpenseDocumentRow expRow = expDS.FnExpenseDocument.FindByExpenseID(this.ExpDocumentID);
            ExpenseDataSet.DocumentRow docRow = expDS.Document.FindByDocumentID(expRow.DocumentID);
            SS.Standard.WorkFlow.DTO.WorkFlow workflow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(docRow.DocumentID);

            // recal invoice item
            if (!IsRepOffice)
            {
                double baseAmountRecalculate = FnExpenseInvoiceItemService.ReCalculateInvoiceItem(this.TxId, this.InvoiceId, this.DocumentType);
                if (baseAmount != baseAmountRecalculate)
                    baseAmount = baseAmountRecalculate;
            }

            // Calculate Vat Amount.
            if (ctlVatRdo.Checked)
            {
                DbTax tax = ScgDbQueryProvider.DbTaxQuery.GetCompanyTaxRateByCompany(UIHelper.ParseLong(ctlTaxCodeDropDown.SelectedValue), expRow.DocumentRow.CompanyID);
                if (vatAmount == 0 || isCal)
                {
                    vatAmount = FnExpenseInvoiceService.CalculateVATAmount(baseAmount, tax == null ? null : (double?)tax.Rate);
                }
                else if (isCal || workflow == null || !(UserAccount.IsAccountant || UserAccount.IsPayment) || (workflow != null && workflow.CurrentState.Name.Equals(WorkFlowStateFlag.Draft)))
                {
                    vatAmount = FnExpenseInvoiceService.CalculateVATAmount(baseAmount, tax == null ? null : (double?)tax.Rate);
                }
                nonDeductAmount = FnExpenseInvoiceService.CalculateVATAverage(baseAmount, tax.RateNonDeduct);

                newBaseAmount += nonDeductAmount;

                // Update all InvoiceItem to set NonDeductAmount.
                FnExpenseInvoiceItemService.UpdateNonDeductAmountInvoiceItem(this.TxId, this.InvoiceId, tax.RateNonDeduct);
            }
            else
            {
                vatAmount = 0;
                nonDeductAmount = 0;
                FnExpenseInvoiceItemService.UpdateNonDeductAmountInvoiceItem(this.TxId, this.InvoiceId, 0);
            }

            ctlNonDeductAmount.Value = nonDeductAmount.ToString();

            // Calculate Witholding Tax.
            if (ctlWhtRdo.Checked)
            {
                double[] baseAmountWHT = new double[2];
                double[] whtRate = new double[2];

                DbWithHoldingTax wht1 = ScgDbQueryProvider.DbWithHoldingTaxQuery.FindByIdentity(UIHelper.ParseLong(ctlWhtRateDropDown.SelectedValue));
                baseAmountWHT[0] = UIHelper.ParseDouble(ctlBaseAmount1.Text);
                whtRate[0] = wht1.Rate;
                if (baseAmountWHT[0] > 0)
                {
                    ctlWhtAmount1.Text = UIHelper.BindDecimal(FnExpenseInvoiceService.CalculateWHTAmount(baseAmountWHT[0], whtRate[0]).ToString());
                }
                // Check for calculate WHT Amount 2.
                if (!string.IsNullOrEmpty(ctlBaseAmount2.Text))
                {
                    DbWithHoldingTax wht2 = ScgDbQueryProvider.DbWithHoldingTaxQuery.FindByIdentity(UIHelper.ParseLong(ctlWhtRateDropDown2.SelectedValue));
                    baseAmountWHT[1] = UIHelper.ParseDouble(ctlBaseAmount2.Text);
                    whtRate[1] = wht2.Rate;
                    ctlWhtAmount2.Text = UIHelper.BindDecimal(FnExpenseInvoiceService.CalculateWHTAmount(baseAmountWHT[1], whtRate[1]).ToString()); //UIHelper.BindDecimal(calWhtAmount[1].ToString());
                }

                double totalBaseAmountWHT = Math.Round(baseAmountWHT[0] + baseAmountWHT[1], 2, MidpointRounding.AwayFromZero);
                if (totalBaseAmountWHT > newBaseAmount)
                {
                    error.AddError("ValidationError", new Spring.Validation.ErrorMessage("TotalBaseAmountWHT_Is_MoreThan"));
                }
                // Calculate WHTAmount.
                whtAmount = FnExpenseInvoiceService.CalculateWHTAmount(baseAmountWHT, whtRate);
            }
            if (ctlNonWht.Checked)
            {
                whtAmount = 0;
                ctlBaseAmount1.Text = "0.00";
                ctlBaseAmount2.Text = "0.00";
                ctlWhtAmount1.Text = "0.00";
                ctlWhtAmount2.Text = "0.00";
                ctlWhtRateDropDown.DataBind();
                ctlWhtRateDropDown2.DataBind();
            }

            if (!error.IsEmpty) throw new ServiceValidationException(error);

            if (!IsRepOffice)
            {
                // Assign value to ctlNetAmount.
                netAmount = FnExpenseInvoiceService.CalculateNetAmount(newBaseAmount, vatAmount, whtAmount);
                ctlNetAmount.Text = UIHelper.BindDecimal(netAmount.ToString());

                // Assign value to ctlVatAmount.
                ctlVatAmount.Text = UIHelper.BindDecimal(vatAmount.ToString());

                // Assign new value to ctlBaseAmount.
                ctlBaseAmount.Text = UIHelper.BindDecimal(newBaseAmount.ToString());

                // Assign value to ctlWHTAmount.
                ctlWHTAmount.Text = UIHelper.BindDecimal(whtAmount.ToString());
            }
            else
            {
                ctlBaseAmount.Text = ctlNetAmount.Text = UIHelper.BindDecimal(ctlHideTotalAmountLocalCurrency.Value.ToString());
            }
            ctlUpdatePanelWHTSummary.Update();
            ctlUpdatePanelSummary.Update();
        }

        private void ShowWHT()
        {
            ctlWhtFds.Style["display"] = "block";
        }
        private void ShowVat()
        {
            double baseAmount = UIHelper.ParseDouble(ctlHideTotalAmountTHB.Value);
            double vatAmount = FnExpenseInvoiceService.CalculateVATAmount(baseAmount, null);

            ctlVatAmount.Text = vatAmount.ToString();

            ctlSummary.Style["display"] = "block";

            ctlUpdatePanelSummary.Update();
        }
        private void BindControl()
        {
            BindInvoiceItemForm();
            BindInvoiceItemGridView();

            this.ChangeModeButton(InitialFlag);
            this.BindWhtRateDropdownControl();
            this.BindWHTTypeDropdownControl();
            BindTaxCodeDropdown();
            if (IsDomestic)
            {
                ShowDomesticField();
            }
            else
            {
                ShowForeignField();
            }
            if (!InitialFlag.Equals(FlagEnum.NewFlag))
            {
                if (InitialFlag.Equals(FlagEnum.ViewFlag))
                    ctlClearVendor.Visible = false;
                else
                    ctlClearVendor.Visible = this.LinkControlEditable;

                BindInvoiceForm();
            }
            else
            {
                ctlInvoicefds.Style["display"] = "none";
                ctlRadioDiv.Style["display"] = "none";
                ctlCountry.Text = "TH";
            }

            //BindCurrencyDropdown();

            ctlUpdatePanelInvoiceForm.Update();

        }
        private void ResetInvoiceItemForm()
        {
            ctlCostCenterField.ResetValue();
            ctlAccountField.ResetValue();
            ctlIOAutoCompleteLookup.ResetValue();

            ctlSaleOrder.Text = string.Empty;
            ctlSaleItem.Text = string.Empty;
            ctlDescription.Text = string.Empty;
            ctlCurrency.ResetControl();
            ctlAmount.Text = string.Empty;
            ctlAdvanceExchangeRate.Text = string.Empty;
            ctlExchangeRate.Text = string.Empty;
            ctlReferanceNo.Text = string.Empty;
            ctlVendorCodeAP.Text = string.Empty;
            ctlExchangeRate.ReadOnly = false;
            ctlExchangeRate.BackColor = System.Drawing.Color.Transparent;
            ctlVendorAP.Style["Display"] = "none";
        }
        private void ResetInvoiceItemGridView()
        {
            ctlInvoiceItemGridview.DataSource = null;
            ctlInvoiceItemGridview.DataBind();
            ctlHideTotalAmountTHB.Value = string.Empty;
        }
        private void ResetVatAndWHT()
        {
            ctlVatRdo.Checked = false;
            ctlNonVatRdo.Checked = false;
            ctlWhtRdo.Checked = false;
            ctlNonWht.Checked = false;
        }
        private void ResetInvoiceForm()
        {
            ctlInvoiceNo.Text = string.Empty;
            ctlInvoiceDate.DateValue = string.Empty;
            ctlInvoiceDescription.Text = string.Empty;
            ctlBranch.Text = string.Empty;
            ctlFLDocument.Text = string.Empty;
            ctlVendorTaxNo.Text = string.Empty;
            ctlVendorCode.Text = string.Empty;
            ctlVendorBranch.Text = string.Empty;
            ctlVendorName.Text = string.Empty;
            ctlStreet.Text = string.Empty;
            ctlCity.Text = string.Empty;
            ctlCountry.Text = string.Empty;
            ctlPostalCode.Text = string.Empty;
            VendorID = null;
        }
        private void ResetWitholdingTaxForm()
        {
            ctlBaseAmount1.Text = string.Empty;
            ctlWhtAmount1.Text = string.Empty;
            ctlWhtAmount2.Text = string.Empty;
            ctlBaseAmount2.Text = string.Empty;
            ctlWhtFds.Style["Display"] = "none";
        }
        private void ResetSummary()
        {
            ctlBaseAmount.Text = string.Empty;
            ctlVatAmount.Text = string.Empty;
            ctlWHTAmount.Text = string.Empty;
            ctlNetAmount.Text = string.Empty;
            ctlSummary.Style["Display"] = "none";
        }
        private void CheckVatAndWht()
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "checkRdoButton", "OnRadioButtonChange();", true);
        }

        private void SetVat()
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
            ExpenseDataSet.FnExpenseInvoiceRow row = expDS.FnExpenseInvoice.FindByInvoiceID(InvoiceId);
            if (!row.IsisVATNull() && row.isVAT)
            {
                ctlVatRdo.Checked = true;
                ctlShowVatControl.Value = "showVat";
            }
            else
            {
                ctlNonVatRdo.Checked = true;
                ctlShowVatControl.Value = "";
            }
        }
        private void SetWHT()
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(this.TxId);
            ExpenseDataSet.FnExpenseInvoiceRow row = expDS.FnExpenseInvoice.FindByInvoiceID(InvoiceId);
            if (!row.IsisWHTNull() && row.isWHT)
            {
                ctlWhtRdo.Checked = true;
                ctlShowWHTControl.Value = "showWHT";
            }
            else
            {
                ctlNonWht.Checked = true;
                ctlShowWHTControl.Value = "";
            }
        }
        #endregion

        private void SetDefaultWHTType()
        {
            IList<VOWHTType> types = ScgDbQueryProvider.DbWithHoldingTaxTypeQuery.FindAllWHTTypeActive();

            var t = from i in types
                    where i.IsPeople == IsPeople
                    select i;
            if (t != null && t.Count<VOWHTType>() > 0)
            {
                ctlWHTTypeDropdown.SelectedValue = t.First<VOWHTType>().ID.ToString();
                ctlWHTTypeDropDown2.SelectedValue = t.First<VOWHTType>().ID.ToString();
            }
        }

        protected void ctlBaseAmount1_OnTextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ctlBaseAmount1.Text))
            {
                ctlWhtAmount1.Text = string.Empty;
            }
        }

        protected void ctlBaseAmount2_OnTextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ctlBaseAmount2.Text))
            {
                ctlWhtAmount2.Text = string.Empty;
            }
        }
        private void CheckEditableVendor(VOVendor vendor)
        {
            if (vendor.VendorID != null)
            {
                ctlVendorCode.Enabled = false;
                ctlVendorBranch.Enabled = false;
                ctlVendorName.Enabled = false;
                ctlStreet.Enabled = false;
                ctlCity.Enabled = false;
                ctlCountry.Enabled = false;
                ctlPostalCode.Enabled = false;
                ctlClearVendor.Visible = true;
            }
            else
            {
                if (vendor.VendorCode.Equals(ParameterServices.Default_Vendor))
                {
                    ctlVendorCode.Enabled = false;
                    ctlClearVendor.Visible = false;
                }
                else
                {
                    ctlVendorCode.Enabled = false;
                    ctlClearVendor.Visible = true;
                }
                ctlVendorBranch.Enabled = true;
                ctlVendorName.Enabled = true;
                ctlStreet.Enabled = true;
                ctlCity.Enabled = true;
                ctlCountry.Enabled = true;
                ctlPostalCode.Enabled = true;
            }
        }

        protected void ctlTaxCodeDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.CalculateInvoice(true);
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }

        private void CheckPersonalTaxFormat()
        {
            Regex personalTaxRegEx = new Regex(ParameterServices.PersonalTaxFormatRegEx);
            Match match = personalTaxRegEx.Match(ctlVendorTaxNo.Text);

            IsPeople = match.Success;
        }
    }

    public struct TaxFlag
    {
        // Use set document status.
        public const string VAT = "VAT";
        public const string NoVAT = "No VAT";
        public const string WHT = "WHT";
        public const string NoWHT = "No WHT";
    }

}

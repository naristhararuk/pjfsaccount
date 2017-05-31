using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.DB.Query;
using SCG.DB.DTO;
using SCG.DB.DTO.ValueObject;
using SCG.eAccounting.Web.Helper;
using SCG.DB.BLL;
using SS.Standard.Utilities;
using SS.DB.Query;
using System.Text;
using SS.DB.DTO;
using SCG.eAccounting.Web.UserControls;
using System.Web.UI.HtmlControls;
using SCG.eAccounting.Query;

namespace SCG.eAccounting.Web.Forms.SCG.DB.Programs
{
    public partial class Company : BasePage
    {
        #region Properties
        public IDbCompanyService DbCompanyService { get; set; }
        public IDbCompanyPaymentMethodService DbCompanyPaymentMethodService { get; set; }
        public IDbLocationService DbLocationService { get; set; }
        public IDbLocationLangService DbLocationLangService { get; set; }
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            ctlLocationEditor.Notify_Ok += new EventHandler(RefreshGridData);
            ctlLocationEditor.Notify_Cancle += new EventHandler(RefreshGridData);
            if (!Page.IsPostBack)
            {
                ctlCompanyGrid.DataCountAndBind();
            }
        }
        #endregion
        public void RefreshGridData(object sender, EventArgs e)
        {


            ctlLocationEditor.HidePopUp();
            ctlUpdatePanelCompanyGridview.Update();
            BindLocationGrid(UIHelper.ParseLong(ctlCompanyIDHidden2.Value), UserAccount.CurrentLanguageID);
            ctlUpdatePanelLocationGrid.Update();


        }
        #region Company Gridview Event
        protected void ctlCompanyGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("CompanyEdit"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                short companyId = Convert.ToInt16(ctlCompanyGrid.DataKeys[rowIndex].Value);
                ctlCompanyForm.PageIndex = (ctlCompanyGrid.PageIndex * ctlCompanyGrid.PageSize) + rowIndex;
                ctlCompanyForm.ChangeMode(FormViewMode.Edit);
                IList<DbCompany> companyList = new List<DbCompany>();
                DbCompany company = DbCompanyService.FindByIdentity(companyId);
                company.PerdiemProfileID = ScgDbQueryProvider.DbCompanyQuery.GetFnPerdiemProfileCompany(company.CompanyID);
                companyList.Add(company);

                ctlCompanyForm.DataSource = companyList;
                ctlCompanyForm.DataBind();


                ctlUpdatePanelCompanyForm.Update();
                ctlCompanyModalPopupExtender.Show();
            }
            if (e.CommandName.Equals("PaymentMethodEdit"))
            {

                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                long companyId = UIHelper.ParseShort(ctlCompanyGrid.DataKeys[rowIndex].Value.ToString());
                ctlCompanyIDHidden.Value = companyId.ToString();

                BindPaymentMethodGridAndDropdown(companyId);
                ctlCompanyGrid.SelectedIndex = rowIndex;

                ctlPaymentMethodGrid.Visible = true;
                ctlPaymentMethodTools.Visible = true;
                ctlPaymentMethodDropdown.Visible = true;
                ctlUpdatePaymentMethod.Visible = true;
                ctlAddPaymentMethod.Visible = true;
                ctlClosePaymentMethod.Visible = true;

                ctlUpdatePanelCompanyGridview.Update();
                ctlUpdatePanelPaymentMethodGrid.Update();
                LocationGridViewClose();//close location grid


            }
            if (e.CommandName.Equals("LocationEdit"))
            {
                ctlLocationCode.Text = string.Empty;
                ctlDescription.Text = string.Empty;
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                long companyId = UIHelper.ParseShort(ctlCompanyGrid.DataKeys[rowIndex].Value.ToString());
                ctlCompanyGrid.SelectedIndex = rowIndex;
                ctlCompanyIDHidden2.Value = companyId.ToString();
                BindLocationGrid(companyId, UserAccount.CurrentLanguageID);
                ctlLocationGrid.Visible = true;
                ctlLocationTools.Visible = true;
                ctlLocationCriteria.Visible = true;
                ctlSearchLocation.Visible = true;
                ctlUpdatePanelCompanyGridview.Update();
                ctlUpdatePanelLocationGrid.Update();
                PaymentMethodGridViewClose();//close payment grid
            }
            if (e.CommandName.Equals("CompanyDelete"))
            {
                try
                {
                    int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                    long companyId = UIHelper.ParseLong(ctlCompanyGrid.DataKeys[rowIndex].Value.ToString());
                    DbCompany company = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(companyId);
                    DbCompanyService.DeleteCompany(company);
                }
                catch (Exception ex)
                {
                    if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                            "alert('This data is now in use.');", true);
                    }
                    ctlCompanyGrid.DataCountAndBind();
                }
                ctlCompanyGrid.DataCountAndBind();
                ctlUpdatePanelCompanyGridview.Update();
            }
        }
        protected void ctlCompanyGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void ctlCompanyGrid_DataBound(object sender, EventArgs e)
        {
            if (ctlCompanyGrid.Rows.Count > 0)
            {
                //RegisterScriptForGridView();
                //divButton.Visible = true;
                //ctlDelete.OnClientClick = String.Format("return ConfirmDelete('{0}')", ctlProgramGrid.ClientID);
            }
            else
            {
                //divButton.Visible = false;
            }
        }
        protected void ctlCompanyGrid_PageIndexChanged(object sender, EventArgs e)
        {
            PaymentMethodGridViewClose();
        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            DbCompany company = GetCompanyCriteria();

            return ScgDbQueryProvider.DbCompanyQuery.GetCompanyList(company,null, startRow, pageSize, sortExpression);
        }
        public int RequestCount()
        {
            DbCompany company = GetCompanyCriteria();

            int count = ScgDbQueryProvider.DbCompanyQuery.CountCompanyByCriteria(company,null);

            return count;
        }
        public DbCompany GetCompanyCriteria()
        {
            DbCompany company = new DbCompany();
            company.CompanyCode = ctlCompanyCodeCri.Text;
            company.CompanyName = ctlCompanyNameCri.Text;
            return company;
        }
        #endregion

        #region Button Event

        #region Company Gridview Buton Event
        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {

            ctlCompanyGrid.DataCountAndBind();
            PaymentMethodGridViewClose();
            LocationGridViewClose();


        }
        protected void ctlAddNew_Click(object sender, ImageClickEventArgs e)
        {
            ctlCompanyModalPopupExtender.Show();
            ctlCompanyForm.ChangeMode(FormViewMode.Insert);
            ctlUpdatePanelCompanyForm.Update();
        }
        #endregion

        #region PaymentMethod Gridview Button Event
        protected void ctlAddPaymentMethod_Click(object sender, ImageClickEventArgs e)
        {

            DbCompanyPaymentMethod companyPaymentMethod = new DbCompanyPaymentMethod();
            DbPaymentMethod paymentMethod = ScgDbQueryProvider.DbPaymentMethodQuery.FindPaymentMethodByID(UIHelper.ParseLong(ctlPaymentMethodDropdown.SelectedValue));
            DbCompany company = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(UIHelper.ParseLong(ctlCompanyIDHidden.Value));

            companyPaymentMethod.Company = company;
            companyPaymentMethod.PaymentMethod = paymentMethod;
            companyPaymentMethod.CompanyCode = company.CompanyCode;
            companyPaymentMethod.Active = true;
            companyPaymentMethod.CreBy = UserAccount.UserID;
            companyPaymentMethod.CreDate = DateTime.Now;
            companyPaymentMethod.UpdBy = UserAccount.UserID;
            companyPaymentMethod.UpdDate = DateTime.Now;
            companyPaymentMethod.UpdPgm = ProgramCode;

            try
            {
                DbCompanyPaymentMethodService.AddCompanyPaymentMethod(companyPaymentMethod);
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
            BindPaymentMethodGridAndDropdown(UIHelper.ParseLong(ctlCompanyIDHidden.Value));
            ctlUpdatePanelPaymentMethodGrid.Update();
        }
        protected void ctlUpdatePaymentMethod_Click(object sender, ImageClickEventArgs e)
        {
            foreach (GridViewRow row in ctlPaymentMethodGrid.Rows)
            {
                CheckBox ctlActive = (CheckBox)ctlPaymentMethodGrid.Rows[row.RowIndex].FindControl("ctlActive");
                if (row.RowType == DataControlRowType.DataRow)
                {
                    long companyPaymentMethodId = UIHelper.ParseLong(ctlPaymentMethodGrid.DataKeys[row.RowIndex]["CompanyPaymentMethodID"].ToString());
                    DbCompanyPaymentMethod companyPaymentMethod = ScgDbQueryProvider.DbCompanyPaymentMethodQuery.FindByIdentity(companyPaymentMethodId);
                    companyPaymentMethod.Active = ctlActive.Checked;
                    DbCompanyPaymentMethodService.UpdateCompanyPaymentMethod(companyPaymentMethod);
                }
            }
            BindPaymentMethodGridAndDropdown(UIHelper.ParseLong(ctlCompanyIDHidden.Value));
        }
        protected void ctlDeletePaymentMethod_Click(object sender, ImageClickEventArgs e)
        {
            foreach (GridViewRow row in ctlPaymentMethodGrid.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
                {
                    try
                    {
                        long companyId = UIHelper.ParseLong(ctlPaymentMethodGrid.DataKeys[row.RowIndex]["CompanyID"].ToString());
                        long paymentMethodId = UIHelper.ParseLong(ctlPaymentMethodGrid.DataKeys[row.RowIndex]["PaymentMethodID"].ToString());
                        DbCompanyPaymentMethod paymentMethod = ScgDbQueryProvider.DbCompanyPaymentMethodQuery.FindCompanyPaymentMethodByCompanyIdAndPaymentMethodId(companyId, paymentMethodId);
                        DbCompanyPaymentMethodService.Delete(paymentMethod);

                    }
                    catch (Exception ex)
                    {
                        if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                                "alert('This data is now in use.');", true);

                            ctlUpdatePanelPaymentMethodGrid.Update();
                        }
                    }
                }
            }
            BindPaymentMethodGridAndDropdown(UIHelper.ParseLong(ctlCompanyIDHidden.Value));
            ctlUpdatePanelPaymentMethodGrid.Update();
        }
        protected void ctlClosePaymentMethod_Click(object sender, ImageClickEventArgs e)
        {
            PaymentMethodGridViewClose();
        }
        #endregion

        #region Location Gridview Button Event
        protected void ctlSearchLocation_Click(object sender, ImageClickEventArgs e)
        {
            BindLocationGrid(UIHelper.ParseLong(ctlCompanyIDHidden2.Value), UserAccount.CurrentLanguageID);

        }
        protected void ctlAddLocation_Click(object sender, ImageClickEventArgs e)
        {
            //ctlLocationFormModalPopupExtender.Show();
            //ctlLocationForm.ChangeMode(FormViewMode.Insert);
            long companyId = UIHelper.ParseLong(ctlCompanyIDHidden2.Value);
            ctlLocationEditor.Initialize(FlagEnum.NewFlag, 0, companyId);
            ctlLocationEditor.ShowPopUp();
            ctlUpdatePanelLocationForm.Update();
        }
        protected void ctlCloseLocation_Click(object sender, ImageClickEventArgs e)
        {
            LocationGridViewClose();
        }
        #endregion

        #endregion

        #region Company Form Event
        protected void ctlCompanyForm_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            e.Cancel = true;
        }
        protected void ctlCompanyForm_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                ctlCompanyGrid.DataCountAndBind();
                CloseCompanyPopUp();
            }
        }
        protected void ctlCompanyForm_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            TextBox ctlCompanyCode = (TextBox)ctlCompanyForm.FindControl("ctlCompanyCode");
            TextBox ctlCompanyName = (TextBox)ctlCompanyForm.FindControl("ctlCompanyName");
            DropDownList ctlPaymentTypeDropdown = (DropDownList)ctlCompanyForm.FindControl("ctlPaymentTypeDropdown");
            UserControls.DropdownList.SCG.DB.PaymentMethod ctlPaymentMethodPettyDropdown = ctlCompanyForm.FindControl("ctlPaymentMedthodPettyDropdown") as UserControls.DropdownList.SCG.DB.PaymentMethod;
            UserControls.DropdownList.SCG.DB.PaymentMethod ctlPaymentMethodTransferDropdown = ctlCompanyForm.FindControl("ctlPaymentMedthodTransferDropdown") as UserControls.DropdownList.SCG.DB.PaymentMethod;
            UserControls.DropdownList.SCG.DB.PaymentMethod ctlPaymentMethodChequeDropdown = ctlCompanyForm.FindControl("ctlPaymentMedthodChequeDropdown") as UserControls.DropdownList.SCG.DB.PaymentMethod;
            DropDownList ctlPerdiemRateProfileDropdown = (DropDownList)ctlCompanyForm.FindControl("ctlPerdiemRateProfileDropdown");
            CheckBox ctlActiveChk = (CheckBox)ctlCompanyForm.FindControl("ctlActiveChk");
            CheckBox ctlAllowImportUserFromEHrChk = (CheckBox)ctlCompanyForm.FindControl("ctlAllowImportUserFromEHrChk");
            DropDownList ctlTaxCodeDropDown = (DropDownList)ctlCompanyForm.FindControl("ctlTaxCodeDropDown");
            TextBox ctlBankName = (TextBox)ctlCompanyForm.FindControl("ctlBankName");
            TextBox ctlBankBranch = (TextBox)ctlCompanyForm.FindControl("ctlBankBranch");
            TextBox ctlAccountNo = (TextBox)ctlCompanyForm.FindControl("ctlAccountNo");
            TextBox ctlAccountType = (TextBox)ctlCompanyForm.FindControl("ctlAccountType");
            TextBox ctlTaxId = (TextBox)ctlCompanyForm.FindControl("ctlTaxId");
            TextBox ctlKBankCode = (TextBox)ctlCompanyForm.FindControl("ctlKBankCode");
            CheckBox ctlUseSpecialPayInChk = (CheckBox)ctlCompanyForm.FindControl("ctlUseSpecialPayInChk");
            TextBox ctlDefaultGLAccount = (TextBox)ctlCompanyForm.FindControl("ctlDefaultGLAccount");
            CheckBox ctlExpenseRequireAttachment = (CheckBox)ctlCompanyForm.FindControl("ctlExpenseRequireAttachment");
            DropDownList ctlBUDropdown = (DropDownList)ctlCompanyForm.FindControl("ctlBUDropdown");
            TextBox ctlBusinessArea = (TextBox)ctlCompanyForm.FindControl("ctlBusinessArea");
            CheckBox ctlRequireBusinessArea = (CheckBox)ctlCompanyForm.FindControl("ctlRequireBusinessArea");
            CheckBox ctlUseEcc = (CheckBox)ctlCompanyForm.FindControl("ctlUseEcc");
			CheckBox ctlIsVerifyHardCopyOnly = (CheckBox)ctlCompanyForm.FindControl("ctlIsVerifyHardCopyOnly");
            DropDownList ctlSAPInstanceDropdown = (DropDownList)ctlCompanyForm.FindControl("ctlSAPInstanceDropdown");
			DropDownList ctlMileageRateRivition = (DropDownList)ctlCompanyForm.FindControl("ctlMileageRateRivition");

            DbCompany company = new DbCompany();
            company.CompanyCode = ctlCompanyCode.Text;
            company.CompanyName = ctlCompanyName.Text;
            company.PaymentType = ctlPaymentTypeDropdown.SelectedValue;
            company.PaymentMethodPetty = ScgDbQueryProvider.DbPaymentMethodQuery.FindByIdentity(UIHelper.ParseLong(ctlPaymentMethodPettyDropdown.SelectedValue));
            company.PaymentMethodTransfer = ScgDbQueryProvider.DbPaymentMethodQuery.FindByIdentity(UIHelper.ParseLong(ctlPaymentMethodTransferDropdown.SelectedValue));
            company.PaymentMethodCheque = ScgDbQueryProvider.DbPaymentMethodQuery.FindByIdentity(UIHelper.ParseLong(ctlPaymentMethodChequeDropdown.SelectedValue));
            company.PerdiemProfileID = UIHelper.ParseLong(ctlPerdiemRateProfileDropdown.SelectedValue);
            company.AllowImportUserFromEHr = ctlAllowImportUserFromEHrChk.Checked;
            company.DefaultTaxID = UIHelper.ParseLong(ctlTaxCodeDropDown.SelectedValue);

            company.Active = ctlActiveChk.Checked;
            company.CreBy = UserAccount.UserID;
            company.CreDate = DateTime.Now;
            company.UpdBy = UserAccount.UserID;
            company.UpdDate = DateTime.Now;
            company.UpdPgm = ProgramCode;
            company.BankName = ctlBankName.Text;
            company.BankBranch = ctlBankBranch.Text;
            company.AccountNo = ctlAccountNo.Text;
            company.AccountType = ctlAccountType.Text;
            company.UseSpecialPayIn = ctlUseSpecialPayInChk.Checked;
            company.TaxId = ctlTaxId.Text;
            company.KBankCode = ctlKBankCode.Text;
            company.DefaultGLAccount = ctlDefaultGLAccount.Text;
            company.ExpenseRequireAttachment = ctlExpenseRequireAttachment.Checked;
            company.BU = ctlBUDropdown.SelectedValue;
            company.UseEcc = ctlUseEcc.Checked;
            company.SapCode = string.IsNullOrEmpty(ctlSAPInstanceDropdown.SelectedValue) ? null : ctlSAPInstanceDropdown.SelectedValue;
            company.BusinessArea = ctlBusinessArea.Text;
            company.RequireBusinessArea = ctlRequireBusinessArea.Checked;
			company.IsVerifyHardCopyOnly = ctlIsVerifyHardCopyOnly.Checked;
            if (ctlMileageRateRivition.SelectedIndex != 0)
                company.MileageProfileId = new Guid(ctlMileageRateRivition.SelectedValue);
            else
                company.MileageProfileId = null;

            try
            {
                DbCompanyService.AddCompany(company);
                ctlCompanyGrid.DataCountAndBind();
                ctlCompanyForm.ChangeMode(FormViewMode.ReadOnly);
                CloseCompanyPopUp();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        protected void ctlCompanyForm_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            short companyId = Convert.ToInt16(ctlCompanyForm.DataKey.Value);
            TextBox ctlCompanyCode = (TextBox)ctlCompanyForm.FindControl("ctlCompanyCode");
            TextBox ctlCompanyName = (TextBox)ctlCompanyForm.FindControl("ctlCompanyName");
            DropDownList ctlPaymentTypeDropdown = (DropDownList)ctlCompanyForm.FindControl("ctlPaymentTypeDropdown");
            UserControls.DropdownList.SCG.DB.PaymentMethod ctlPaymentMethodPettyDropdown = ctlCompanyForm.FindControl("ctlPaymentMedthodPettyDropdown") as UserControls.DropdownList.SCG.DB.PaymentMethod;
            UserControls.DropdownList.SCG.DB.PaymentMethod ctlPaymentMethodTransferDropdown = ctlCompanyForm.FindControl("ctlPaymentMedthodTransferDropdown") as UserControls.DropdownList.SCG.DB.PaymentMethod;
            UserControls.DropdownList.SCG.DB.PaymentMethod ctlPaymentMethodChequeDropdown = ctlCompanyForm.FindControl("ctlPaymentMedthodChequeDropdown") as UserControls.DropdownList.SCG.DB.PaymentMethod;
            DropDownList ctlPerdiemRateProfileDropdown = (DropDownList)ctlCompanyForm.FindControl("ctlPerdiemRateProfileDropdown");
            CheckBox ctlActiveChk = (CheckBox)ctlCompanyForm.FindControl("ctlActiveChk");
            CheckBox ctlAllowImportUserFromEHrChk = (CheckBox)ctlCompanyForm.FindControl("ctlAllowImportUserFromEHrChk");
            DropDownList ctlTaxCodeDropDown = (DropDownList)ctlCompanyForm.FindControl("ctlTaxCodeDropDown");
            TextBox ctlBankName = (TextBox)ctlCompanyForm.FindControl("ctlBankName");
            TextBox ctlBankBranch = (TextBox)ctlCompanyForm.FindControl("ctlBankBranch");
            TextBox ctlAccountNo = (TextBox)ctlCompanyForm.FindControl("ctlAccountNo");
            TextBox ctlAccountType = (TextBox)ctlCompanyForm.FindControl("ctlAccountType");
            TextBox ctlTaxId = (TextBox)ctlCompanyForm.FindControl("ctlTaxId");
            TextBox ctlKBankCode = (TextBox)ctlCompanyForm.FindControl("ctlKBankCode");
            CheckBox ctlUseSpecialPayIn = (CheckBox)ctlCompanyForm.FindControl("ctlUseSpecialPayInChk");
            TextBox ctlDefaultGLAccount = (TextBox)ctlCompanyForm.FindControl("ctlDefaultGLAccount");
            CheckBox ctlExpenseRequireAttachment = (CheckBox)ctlCompanyForm.FindControl("ctlExpenseRequireAttachment");
            DropDownList ctlBUDropdown = (DropDownList)ctlCompanyForm.FindControl("ctlBUDropdown");
            TextBox ctlBusinessArea = (TextBox)ctlCompanyForm.FindControl("ctlBusinessArea");
            CheckBox ctlRequireBusinessArea = (CheckBox)ctlCompanyForm.FindControl("ctlRequireBusinessArea");
            CheckBox ctlUseEcc = (CheckBox)ctlCompanyForm.FindControl("ctlUseEcc");
			CheckBox ctlIsVerifyHardCopyOnly = (CheckBox)ctlCompanyForm.FindControl("ctlIsVerifyHardCopyOnly");
            DropDownList ctlSAPInstanceDropdown = (DropDownList)ctlCompanyForm.FindControl("ctlSAPInstanceDropdown");
			DropDownList ctlMileageRateRivition = (DropDownList)ctlCompanyForm.FindControl("ctlMileageRateRivition");
            DbCompany company = DbCompanyService.FindByIdentity(companyId);
            company.CompanyCode = ctlCompanyCode.Text;
            company.CompanyName = ctlCompanyName.Text;
            company.PaymentType = ctlPaymentTypeDropdown.SelectedValue;
            company.PaymentMethodPetty = ScgDbQueryProvider.DbPaymentMethodQuery.FindByIdentity(UIHelper.ParseLong(ctlPaymentMethodPettyDropdown.SelectedValue));
            company.PaymentMethodTransfer = ScgDbQueryProvider.DbPaymentMethodQuery.FindByIdentity(UIHelper.ParseLong(ctlPaymentMethodTransferDropdown.SelectedValue));
            company.PaymentMethodCheque = ScgDbQueryProvider.DbPaymentMethodQuery.FindByIdentity(UIHelper.ParseLong(ctlPaymentMethodChequeDropdown.SelectedValue));
            company.DefaultTaxID = UIHelper.ParseLong(ctlTaxCodeDropDown.SelectedValue);
            company.AllowImportUserFromEHr = ctlAllowImportUserFromEHrChk.Checked;
            company.BU = ctlBUDropdown.SelectedValue;
            company.SapCode = string.IsNullOrEmpty(ctlSAPInstanceDropdown.SelectedValue) ? null : ctlSAPInstanceDropdown.SelectedValue;
            company.Active = ctlActiveChk.Checked;
            company.CreBy = UserAccount.UserID;
            company.CreDate = DateTime.Now;
            company.UpdBy = UserAccount.UserID;
            company.UpdDate = DateTime.Now;
            company.UpdPgm = ProgramCode;
            company.BankName = ctlBankName.Text;
            company.BankBranch = ctlBankBranch.Text;
            company.AccountNo = ctlAccountNo.Text;
            company.AccountType = ctlAccountType.Text;
            company.UseSpecialPayIn = ctlUseSpecialPayIn.Checked;
            company.TaxId = ctlTaxId.Text;
            company.KBankCode = ctlKBankCode.Text;
            company.DefaultGLAccount = ctlDefaultGLAccount.Text;
            company.ExpenseRequireAttachment = ctlExpenseRequireAttachment.Checked;
            company.BusinessArea = ctlBusinessArea.Text;
            company.RequireBusinessArea = ctlRequireBusinessArea.Checked;
            company.UseEcc = ctlUseEcc.Checked;
            company.PerdiemProfileID = UIHelper.ParseLong(ctlPerdiemRateProfileDropdown.SelectedValue);
			 company.IsVerifyHardCopyOnly = ctlIsVerifyHardCopyOnly.Checked;
            if (ctlMileageRateRivition.SelectedIndex != 0)
                company.MileageProfileId = new Guid(ctlMileageRateRivition.SelectedValue);
            else
                company.MileageProfileId = null;

            try
            {
                DbCompanyService.UpdateCompany(company);
                ctlCompanyGrid.DataCountAndBind();
                ctlCompanyForm.ChangeMode(FormViewMode.ReadOnly);
                CloseCompanyPopUp();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        protected void ctlCompanyForm_DataBound(object sender, EventArgs e)
        {
            if (ctlCompanyForm.CurrentMode != FormViewMode.ReadOnly)
            {
                TextBox ctlCompanyCode = (TextBox)ctlCompanyForm.FindControl("ctlCompanyCode");
                CheckBox ctlActiveCheck = (CheckBox)ctlCompanyForm.FindControl("ctlActiveChk");
                CheckBox ctlAllowImportUserFromEHrChk = (CheckBox)ctlCompanyForm.FindControl("ctlAllowImportUserFromEHrChk");
                TextBox ctlBankName = (TextBox)ctlCompanyForm.FindControl("ctlBankName");
                TextBox ctlBankBranch = (TextBox)ctlCompanyForm.FindControl("ctlBankBranch");
                TextBox ctlAccountNo = (TextBox)ctlCompanyForm.FindControl("ctlAccountNo");
                TextBox ctlAccountType = (TextBox)ctlCompanyForm.FindControl("ctlAccountType");
                TextBox ctlTaxId = (TextBox)ctlCompanyForm.FindControl("ctlTaxId");
                TextBox ctlKBankCode = (TextBox)ctlCompanyForm.FindControl("ctlKBankCode");
                CheckBox ctlUseSpecialPayInChk = (CheckBox)ctlCompanyForm.FindControl("ctlUseSpecialPayInChk");
                TextBox ctlDefaultGLAccount = (TextBox)ctlCompanyForm.FindControl("ctlDefaultGLAccount");
                CheckBox ctlExpenseRequireAttachment = (CheckBox)ctlCompanyForm.FindControl("ctlExpenseRequireAttachment");
                CheckBox ctlIsVerifyHardCopyOnly = (CheckBox)ctlCompanyForm.FindControl("ctlIsVerifyHardCopyOnly");
                DropDownList ctlPaymentTypeDropdown = ctlCompanyForm.FindControl("ctlPayMentTypeDropdown") as DropDownList;

                TextBox ctlBusinessArea = (TextBox)ctlCompanyForm.FindControl("ctlBusinessArea");
                CheckBox ctlRequireBusinessArea = (CheckBox)ctlCompanyForm.FindControl("ctlRequireBusinessArea");
                CheckBox ctlUseEcc = (CheckBox)ctlCompanyForm.FindControl("ctlUseEcc");
                DropDownList ctlSAPInstanceDropdown = (DropDownList)ctlCompanyForm.FindControl("ctlSAPInstanceDropdown");
				DropDownList ctlMileageRateRivition = ctlCompanyForm.FindControl("ctlMileageRateRivition") as DropDownList;
                ctlCompanyCode.Focus();

                ctlPaymentTypeDropdown.DataSource = SsDbQueryProvider.DbStatusQuery.FindPaymentTypeByLang(UserAccount.CurrentLanguageID);
                ctlPaymentTypeDropdown.DataTextField = "StatusDesc";
                ctlPaymentTypeDropdown.DataValueField = "Status";
                ctlPaymentTypeDropdown.DataBind();

                ctlMileageRateRivition.DataSource = ScgDbQueryProvider.DbProfileListQuery.FindAll();
                ctlMileageRateRivition.DataTextField = "ProfileName";
                ctlMileageRateRivition.DataValueField = "Id";
                ctlMileageRateRivition.DataBind();
                ctlMileageRateRivition.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), null));

                UserControls.DropdownList.SCG.DB.PaymentMethod ctlPaymentMethodPettyDropdown = ctlCompanyForm.FindControl("ctlPaymentMedthodPettyDropdown") as UserControls.DropdownList.SCG.DB.PaymentMethod;
                UserControls.DropdownList.SCG.DB.PaymentMethod ctlPaymentMethodTransferDropdown = ctlCompanyForm.FindControl("ctlPaymentMedthodTransferDropdown") as UserControls.DropdownList.SCG.DB.PaymentMethod;
                UserControls.DropdownList.SCG.DB.PaymentMethod ctlPaymentMethodChequeDropdown = ctlCompanyForm.FindControl("ctlPaymentMedthodChequeDropdown") as UserControls.DropdownList.SCG.DB.PaymentMethod;

                DropDownList ctlPerdiemRateProfileDropdown = (DropDownList)ctlCompanyForm.FindControl("ctlPerdiemRateProfileDropdown");
                ctlPerdiemRateProfileDropdown.DataSource = ScgeAccountingQueryProvider.FnPerdiemProfileQuery.GetPRList();
                ctlPerdiemRateProfileDropdown.DataTextField = "PerdiemProfileName";
                ctlPerdiemRateProfileDropdown.DataValueField = "PerdiemProfileID";
                ctlPerdiemRateProfileDropdown.DataBind();
                ctlPerdiemRateProfileDropdown.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));

                DropDownList ctlTaxCodeDropDown = (DropDownList)ctlCompanyForm.FindControl("ctlTaxCodeDropDown");
                ctlTaxCodeDropDown.DataSource = ScgDbQueryProvider.DbTaxQuery.GetTaxCodeActive();
                ctlTaxCodeDropDown.DataTextField = "TaxCode";
                ctlTaxCodeDropDown.DataValueField = "TaxID";
                ctlTaxCodeDropDown.DataBind();

                ctlPaymentMethodPettyDropdown.PaymentMethodBind();
                ctlPaymentMethodTransferDropdown.PaymentMethodBind();
                ctlPaymentMethodChequeDropdown.PaymentMethodBind();

                DropDownList ctlBUDropdown = (DropDownList)ctlCompanyForm.FindControl("ctlBUDropdown");
                ctlBUDropdown.DataSource = ScgDbQueryProvider.DbBuQuery.FindBUALL();
                ctlBUDropdown.DataTextField = "BuName";
                ctlBUDropdown.DataValueField = "BuCode";
                ctlBUDropdown.DataBind();
                ctlBUDropdown.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));

                ctlSAPInstanceDropdown.DataSource = ScgDbQueryProvider.DbSapInstanceQuery.GetSapInstanceList();
                ctlSAPInstanceDropdown.DataTextField = "AliasName";
                ctlSAPInstanceDropdown.DataValueField = "Code";
                ctlSAPInstanceDropdown.DataBind();
                ctlSAPInstanceDropdown.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));

                if (ctlCompanyForm.CurrentMode == FormViewMode.Edit)
                {
                    short companyID = UIHelper.ParseShort(ctlCompanyForm.DataKey.Value.ToString());
                    DbCompany company = DbCompanyService.FindByIdentity(companyID);
                    ctlPaymentMethodPettyDropdown.PaymentMethodBind(companyID);
                    ctlPaymentMethodTransferDropdown.PaymentMethodBind(companyID);
                    ctlPaymentMethodChequeDropdown.PaymentMethodBind(companyID);
                    ctlPerdiemRateProfileDropdown.SelectedValue = company.PerdiemProfileID.ToString();
                    ctlTaxCodeDropDown.SelectedValue = company.DefaultTaxID.ToString();
                    ctlBUDropdown.SelectedValue = company.BU;
                    ctlSAPInstanceDropdown.SelectedValue = company.SapCode;
                    ctlAccountNo.Text = company.AccountNo;
                    ctlAccountType.Text = company.AccountType;
                    ctlBankName.Text = company.BankName;
                    ctlBankBranch.Text = company.BankBranch;

                    ctlUseSpecialPayInChk.Checked = company.UseSpecialPayIn;
                    ctlTaxId.Text = company.TaxId;
                    ctlKBankCode.Text = company.KBankCode;
                    ctlDefaultGLAccount.Text = company.DefaultGLAccount;
                    ctlExpenseRequireAttachment.Checked = company.ExpenseRequireAttachment;
                    ctlIsVerifyHardCopyOnly.Checked = company.IsVerifyHardCopyOnly;
                    ctlBusinessArea.Text = company.BusinessArea;
                    ctlRequireBusinessArea.Checked = company.RequireBusinessArea;
                    if (company.UseEcc.HasValue)
                    {
                        ctlUseEcc.Checked = company.UseEcc.Value;
                    }
                    ctlUseSpecialPayInChk_CheckedChanged(sender, e);
                    ctlUseEcc_CheckedChanged(sender, e);
                    try
                    {
                        ctlPaymentMethodPettyDropdown.SelectedValue = company.PaymentMethodPetty.PaymentMethodID.ToString();
                    }
                    catch (Exception ex1) { }
                    try
                    {
                        ctlPaymentMethodTransferDropdown.SelectedValue = company.PaymentMethodTransfer.PaymentMethodID.ToString();
                    }
                    catch (Exception ex2) { }
                    try
                    {
                        ctlPaymentMethodChequeDropdown.SelectedValue = company.PaymentMethodCheque.PaymentMethodID.ToString();
                    }
                    catch (Exception ex3) { }
                    ctlPaymentTypeDropdown.SelectedValue = company.PaymentType.ToString();
					ctlMileageRateRivition.SelectedValue = company.MileageProfileId.ToString();
                }
                if (ctlCompanyForm.CurrentMode == FormViewMode.Insert)
                {
                    ctlPerdiemRateProfileDropdown.SelectedValue = string.Empty;
                    ctlTaxCodeDropDown.SelectedValue = ParameterServices.DefaultTaxCode;
                    ctlBUDropdown.SelectedValue = string.Empty;
                    ctlSAPInstanceDropdown.SelectedValue = string.Empty;
                    ctlActiveCheck.Checked = true;
                    ctlAllowImportUserFromEHrChk.Checked = true;
                }
            }
        }
        #endregion

        #region Public Function
        public void CloseCompanyPopUp()
        {
            ctlCompanyModalPopupExtender.Hide();
            ctlCompanyForm.ChangeMode(FormViewMode.ReadOnly);
            ctlUpdatePanelCompanyGridview.Update();
        }
        public void CloseLocationPopUp()
        {
            ctlLocationFormModalPopupExtender.Hide();
            ctlLocationForm.ChangeMode(FormViewMode.ReadOnly);
            ctlUpdatePanelLocationGrid.Update();
        }
        public void PaymentMethodGridViewClose()
        {
            ctlPaymentMethodGrid.Visible = false;
            ctlPaymentMethodGrid.DataSource = null;
            ctlPaymentMethodGrid.DataBind();
            ctlUpdatePanelPaymentMethodGrid.Update();
            ctlPaymentMethodDropdown.Visible = false;
            ctlClosePaymentMethod.Visible = false;
            ctlAddPaymentMethod.Visible = false;
            ctlUpdatePaymentMethod.Visible = false;
            ctlPaymentMethodTools.Visible = false;
        }
        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBox(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlPaymentMethodGrid.ClientID + "', '" + ctlPaymentMethodGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "validateChkBox", script.ToString(), true);
        }
        public void LocationGridViewClose()
        {
            ctlLocationGrid.Visible = false;
            ctlLocationGrid.DataSource = null;
            ctlLocationGrid.DataBind();
            ctlUpdatePanelLocationGrid.Update();
            ctlLocationTools.Visible = false;
            ctlLocationCriteria.Visible = false;
            ctlSearchLocation.Visible = false;
        }
        public void BindPaymentMethodGridAndDropdown(long companyId)
        {
            IList<CompanyPaymentMethodResult> CompanyPaymentMethodList = ScgDbQueryProvider.DbCompanyPaymentMethodQuery.FindCompanyPaymentMethodByCompanyID(companyId);
            ctlPaymentMethodGrid.DataSource = CompanyPaymentMethodList;
            ctlPaymentMethodGrid.DataBind();

            IList<string> paymentMethodIDList = new List<string>();
            foreach (CompanyPaymentMethodResult companyPaymentMethod in CompanyPaymentMethodList)
            {
                paymentMethodIDList.Add(companyPaymentMethod.PaymentMethodID.ToString());
            }
            ctlPaymentMethodDropdown.DataSource = ScgDbQueryProvider.DbPaymentMethodQuery.FindPaymentMethodNotAdd(paymentMethodIDList);
            ctlPaymentMethodDropdown.DataTextField = "PaymentMethodCode";
            ctlPaymentMethodDropdown.DataValueField = "PaymentMethodID";
            ctlPaymentMethodDropdown.DataBind();

            ListItem choose = new ListItem();
            choose.Text = "Choose";
            choose.Value = "-1";
            ctlPaymentMethodDropdown.Items.Insert(0, choose);

        }

        public void BindLocationGrid(long companyId, short languageId)
        {
            Location location = new Location();
            location.CompanyID = companyId;
            location.LocationCode = ctlLocationCode.Text;
            location.LocationName = ctlDescription.Text;

            ctlLocationGrid.DataSource = ScgDbQueryProvider.DbLocationQuery.FindLocationByCriteria(location, UserAccount.CurrentLanguageID);
            ctlLocationGrid.DataBind();
        }
        #endregion

        #region PaymentMethod Gridview Event
        protected void ctlPaymentMethodGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }
        protected void ctlPaymentMethodGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void ctlPaymentMethodGrid_DataBound(object sender, EventArgs e)
        {
            if (ctlPaymentMethodGrid.Rows.Count > 0)
            {
                RegisterScriptForGridView();
                ctlDeletePaymentMethod.Visible = true;
                ctlDeletePaymentMethod.OnClientClick = String.Format("return ConfirmDelete('{0}')", ctlPaymentMethodGrid.ClientID);
            }
            else
            {
                ctlDeletePaymentMethod.Visible = false;
            }
        }
        protected void ctlPaymentMethodGrid_PageIndexChanged(object sender, EventArgs e)
        {

        }
        #endregion

        #region Location Gridview Event
        protected void ctlLocationGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void ctlLocationGrid_DataBound(object sender, EventArgs e)
        {
            if (ctlCompanyGrid.Rows.Count > 0)
            {
                //RegisterScriptForGridView();
                //divButton.Visible = true;
                //ctlDelete.OnClientClick = 

                //String.Format("return ConfirmDelete('{0}')", ctlProgramGrid.ClientID);
            }
            else
            {
                //divButton.Visible = false;
            }
        }
        protected void ctlLocationGrid_PageIndexChanged(object sender, EventArgs e)
        {
            PaymentMethodGridViewClose();
        }
        protected void ctlLocationGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("LocationEdit"))
            {
                //int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                //long locationId = UIHelper.ParseShort(ctlLocationGrid.DataKeys[rowIndex].Value.ToString());
                //ctlLocationForm.PageIndex = (ctlLocationGrid.PageIndex * ctlLocationGrid.PageSize) + rowIndex;
                //ctlLocationForm.ChangeMode(FormViewMode.Edit);
                //IList<DbLocation> locationList = new List<DbLocation>();
                //DbLocation location = DbLocationService.FindByIdentity(locationId);
                //locationList.Add(location);

                //ctlLocationForm.DataSource = locationList;
                //ctlLocationForm.DataBind();

                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                long locationId = UIHelper.ParseShort(ctlLocationGrid.DataKeys[rowIndex].Value.ToString());
                long companyId = UIHelper.ParseLong(ctlCompanyIDHidden2.Value);
                ctlLocationEditor.Initialize(FlagEnum.EditFlag, locationId, companyId);
                ctlLocationEditor.ShowPopUp();
                ctlUpdatePanelLocationForm.Update();

                //ctlLocationFormModalPopupExtender.Show();
                //ctlUpdatePanelLocationForm.Update();
            }
            if (e.CommandName.Equals("LocationDelete"))
            {
                try
                {
                    int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                    long locationId = UIHelper.ParseLong(ctlLocationGrid.DataKeys[rowIndex].Value.ToString());
                    DbLocation location = ScgDbQueryProvider.DbLocationQuery.FindByIdentity(locationId);
                    DbLocationService.DeleteLocation(location);
                }
                catch (Exception ex)
                {
                    if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                            "alert('This data is now in use.');", true);

                        BindLocationGrid(UIHelper.ParseLong(ctlCompanyIDHidden2.Value), UserAccount.CurrentLanguageID);
                        ctlUpdatePanelLocationGrid.Update();
                    }
                }
                BindLocationGrid(UIHelper.ParseLong(ctlCompanyIDHidden2.Value), UserAccount.CurrentLanguageID);
                ctlUpdatePanelLocationGrid.Update();
            }
        }
        #endregion

        #region Location Formview Event
        protected void ctlLocationForm_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                ctlCompanyGrid.DataCountAndBind();
                CloseLocationPopUp();
            }
        }
        protected void ctlLocationForm_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            e.Cancel = true;
        }
        protected void ctlLocationForm_DataBound(object sender, EventArgs e)
        {
            GridView ctlLocationLangGridview = (GridView)ctlLocationForm.FindControl("ctlLocationLangGrid");
            TextBox ctlLocationCode = (TextBox)ctlLocationForm.FindControl("ctlLocationCode");
            CheckBox ctlActive = (CheckBox)ctlLocationForm.FindControl("ctlActiveChk");
            ctlLocationCode.Focus();
            long locationID = 0;
            if (ctlLocationForm.CurrentMode == FormViewMode.Edit)
            {
                locationID = UIHelper.ParseShort(ctlLocationForm.DataKey.Value.ToString());
                ctlLocationLangGridview.DataSource = ScgDbQueryProvider.DbLocationQuery.FindLocationLangByLocationID(locationID);
                ctlLocationLangGridview.DataBind();
            }
            if (ctlLocationForm.CurrentMode == FormViewMode.Insert)
            {
                ctlActive.Checked = true;
                ctlLocationLangGridview.DataSource = ScgDbQueryProvider.DbLocationQuery.FindLocationLangByLocationID(locationID);
                ctlLocationLangGridview.DataBind();
            }

        }
        protected void ctlLocationForm_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            TextBox ctlFormLocationCode = (TextBox)ctlLocationForm.FindControl("ctlLocationCode");
            CheckBox ctlFormActive = (CheckBox)ctlLocationForm.FindControl("ctlActiveChk");
            long locationId = UIHelper.ParseLong(ctlLocationForm.DataKey.Value.ToString());
            DbLocation location = DbLocationService.FindByIdentity(locationId);
            location.LocationCode = ctlFormLocationCode.Text;
            DbCompany company = DbCompanyService.FindByIdentity(UIHelper.ParseLong(ctlCompanyIDHidden2.Value));
            location.CompanyID = company;
            location.UpdPgm = ProgramCode;
            location.Active = ctlFormActive.Checked;
            location.UpdBy = UserAccount.UserID;
            location.UpdDate = DateTime.Now;

            IList<DbLocationLang> locationLangList = new List<DbLocationLang>();
            GridView ctlLocationLangGridview = (GridView)ctlLocationForm.FindControl("ctlLocationLangGrid");
            foreach (GridViewRow row in ctlLocationLangGridview.Rows)
            {

                TextBox ctlLocationName = (TextBox)ctlLocationLangGridview.Rows[row.RowIndex].FindControl("ctlDescription");
                TextBox ctlComment = (TextBox)ctlLocationLangGridview.Rows[row.RowIndex].FindControl("ctlComment");
                CheckBox ctlActive = (CheckBox)ctlLocationLangGridview.Rows[row.RowIndex].FindControl("ctlActive");
                if (!string.IsNullOrEmpty(ctlLocationName.Text) || !string.IsNullOrEmpty(ctlComment.Text))
                {
                    short languageId = UIHelper.ParseShort(ctlLocationLangGridview.DataKeys[row.RowIndex].Value.ToString());
                    DbLocationLang locationLang = new DbLocationLang();
                    DbLanguage language = new DbLanguage(languageId);
                    locationLang.LocationID = location;
                    //locationLang.LanguageID = language;
                    locationLang.LocationName = ctlLocationName.Text;
                    locationLang.Comment = ctlComment.Text;
                    locationLang.Active = ctlActive.Checked;
                    locationLang.CreBy = UserAccount.UserID;
                    locationLang.CreDate = DateTime.Now;
                    locationLang.UpdBy = UserAccount.UserID;
                    locationLang.UpdDate = DateTime.Now;
                    locationLang.UpdPgm = ProgramCode;
                    locationLangList.Add(locationLang);

                }
            }

            try
            {
                DbLocationService.UpdateLocation(location);
                DbLocationLangService.UpdateLocationLang(locationLangList);
                ctlLocationForm.ChangeMode(FormViewMode.ReadOnly);
                CloseLocationPopUp();
                BindLocationGrid(UIHelper.ParseLong(ctlCompanyIDHidden2.Value), UserAccount.CurrentLanguageID);
                ctlUpdatePanelLocationForm.Update();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }

        }
        protected void ctlLocationForm_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            TextBox ctlFormLocationCode = (TextBox)ctlLocationForm.FindControl("ctlLocationCode");
            CheckBox ctlFormActive = (CheckBox)ctlLocationForm.FindControl("ctlActiveChk");
            DbLocation location = new DbLocation();
            location.LocationCode = ctlFormLocationCode.Text;
            DbCompany company = DbCompanyService.FindByIdentity(UIHelper.ParseLong(ctlCompanyIDHidden2.Value));
            location.CompanyID = company;
            location.Active = ctlFormActive.Checked;
            location.CreBy = UserAccount.UserID;
            location.CreDate = DateTime.Now;
            location.UpdPgm = ProgramCode;
            location.UpdBy = UserAccount.UserID;
            location.UpdDate = DateTime.Now;

            IList<DbLocationLang> locationLangList = new List<DbLocationLang>();
            GridView ctlLocationLangGridview = (GridView)ctlLocationForm.FindControl("ctlLocationLangGrid");
            foreach (GridViewRow row in ctlLocationLangGridview.Rows)
            {

                TextBox ctlLocationName = (TextBox)ctlLocationLangGridview.Rows[row.RowIndex].FindControl("ctlDescription");
                TextBox ctlComment = (TextBox)ctlLocationLangGridview.Rows[row.RowIndex].FindControl("ctlComment");
                CheckBox ctlActive = (CheckBox)ctlLocationLangGridview.Rows[row.RowIndex].FindControl("ctlActive");
                if (!string.IsNullOrEmpty(ctlLocationName.Text) || !string.IsNullOrEmpty(ctlComment.Text))
                {
                    short languageId = UIHelper.ParseShort(ctlLocationLangGridview.DataKeys[row.RowIndex].Value.ToString());
                    DbLocationLang locationLang = new DbLocationLang();
                    DbLanguage language = new DbLanguage(languageId);
                    locationLang.LocationID = location;
                    //locationLang.LanguageID = language;
                    locationLang.LocationName = ctlLocationName.Text;
                    locationLang.Comment = ctlComment.Text;
                    locationLang.Active = ctlActive.Checked;
                    locationLang.CreBy = UserAccount.UserID;
                    locationLang.CreDate = DateTime.Now;
                    locationLang.UpdBy = UserAccount.UserID;
                    locationLang.UpdDate = DateTime.Now;
                    locationLang.UpdPgm = ProgramCode;
                    locationLangList.Add(locationLang);

                }
            }

            try
            {
                DbLocationService.AddLocation(location);
                DbLocationLangService.UpdateLocationLang(locationLangList);
                ctlLocationForm.ChangeMode(FormViewMode.ReadOnly);
                CloseLocationPopUp();
                BindLocationGrid(UIHelper.ParseLong(ctlCompanyIDHidden2.Value), UserAccount.CurrentLanguageID);
                ctlUpdatePanelLocationForm.Update();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        #endregion

        #region Control Event
        protected void ctlUseSpecialPayInChk_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ctlUseSpecialPayInChk = (CheckBox)ctlCompanyForm.FindControl("ctlUseSpecialPayInChk");
            TextBox ctlTaxId = (TextBox)ctlCompanyForm.FindControl("ctlTaxId");
            TextBox ctlKBankCode = (TextBox)ctlCompanyForm.FindControl("ctlKBankCode");
            TextBox ctlDefaultGLAccount = (TextBox)ctlCompanyForm.FindControl("ctlDefaultGLAccount");
            Panel ctlSpecialPayInPanel = (Panel)ctlCompanyForm.FindControl("ctlSpecialPayInPanel");

            if (!ctlUseSpecialPayInChk.Checked)
            {
                ctlTaxId.Text = string.Empty;
                ctlKBankCode.Text = string.Empty;
                ctlDefaultGLAccount.Text = string.Empty;
                ctlSpecialPayInPanel.Visible = false;
            }
            else
            {
                ctlSpecialPayInPanel.Visible = true;
            }
        }
        protected void ctlUseEcc_CheckedChanged(object sender, EventArgs e)
        {
            Panel ctlSAPInstancePanel = (Panel)ctlCompanyForm.FindControl("ctlSAPInstancePanel");
            CheckBox ctlUseEcc = (CheckBox)ctlCompanyForm.FindControl("ctlUseEcc");
            DropDownList ctlSAPInstanceDropdown = (DropDownList)ctlCompanyForm.FindControl("ctlSAPInstanceDropdown");

            if (!ctlUseEcc.Checked)
            {

                ctlSAPInstancePanel.Visible = false;
                ctlSAPInstanceDropdown.SelectedValue = string.Empty;
            }
            else
            {
                ctlSAPInstancePanel.Visible = true;
            }
        }
        #endregion




    }
}

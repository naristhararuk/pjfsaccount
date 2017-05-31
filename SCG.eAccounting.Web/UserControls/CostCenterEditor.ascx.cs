using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using SCG.DB.DTO;
using SCG.DB.BLL.Implement;
using SCG.DB.BLL;
using Spring.Validation;
using SS.Standard.Utilities;
using SCG.DB.Query;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class CostCenterEditor : BaseUserControl
    {
        public IDbCostCenterService DbCostCenterService { get; set; }
        public IDbCompanyService DbCompanyService { get; set; }

        #region ViewState Variable

        public string Mode
        {
            get { return this.ViewState["Mode"] == null ? FlagEnum.NewFlag : (string)this.ViewState["Mode"]; }
            set { this.ViewState["Mode"] = value; }
        }

        public long CostCenterID
        {
            get { return this.ViewState["CostCenterID"] == null ? (long)0 : (long)this.ViewState["CostCenterID"]; }
            set { this.ViewState["CostCenterID"] = value; }
        }

        public string CostCenterCode
        {
            get { return ViewState["CostCenterCode"] == null ? "" : ViewState["CostCenterCode"].ToString(); }
            set { ViewState["CostCenterCode"] = value; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #region PopUp event


        public event EventHandler Notify_Ok;
        public event EventHandler Notify_Cancel;

        private void ResetValue()
        {
            ctlTextBoxCostCenterCode.Text = string.Empty;
            ctlTextBoxCostCenterCode.Visible = true;
            ctlCostCenterCodeLabel.Text = string.Empty;
            ctlCostCenterCodeLabel.Visible = false;
            ctlTextBoxDescription.Text = string.Empty;
            ctlCompanyTextboxAutoComplete.CompanyCode = string.Empty;
            ctlCalendarValid.DateValue = string.Empty;
            ctlCalendarExpire.DateValue = string.Empty;
            ctlChkLock.Checked = false;
            ctlChkActive.Checked = false;
            ctlBusinessArea.Text = string.Empty;
            ctlProfitCenter.Text = string.Empty;
            ctlUpdatePanel.Update();
           

        }

        public void ShowPopUp()
        {
            ctlModalPopupExtender.Show();
        }


        public void Initial(string mode, long costCenterID)
        {
            ResetValue();
            Mode = mode;
            CostCenterID = costCenterID;
            if (mode == FlagEnum.EditFlag)
            {
                DbCostCenter costCenter = DbCostCenterService.FindByIdentity(costCenterID);
                if ((costCenter.CompanyID != null))
                {
                    DbCompany company = DbCompanyService.FindByIdentity(costCenter.CompanyID.CompanyID);
                    ctlCompanyTextboxAutoComplete.CompanyCode = costCenter.CompanyID.CompanyCode;
                }

                this.CostCenterCode = costCenter.CostCenterCode;
                ctlCostCenterCodeLabel.Text = costCenter.CostCenterCode;
                ctlCostCenterCodeLabel.Visible = true;
                ctlTextBoxCostCenterCode.Visible = false;
                ctlTextBoxDescription.Text = costCenter.Description;

                //ctlCalendarValid.DateValue = costCenter.Valid.ToString(Constant.CalendarDateFormat);
                //ctlCalendarExpire.DateValue = costCenter.Valid.ToString(Constant.CalendarDateFormat);

                ctlCalendarValid.Value = costCenter.Valid;
                ctlCalendarExpire.Value = costCenter.Expire;
                ctlChkLock.Checked = costCenter.ActualPrimaryCosts;
                ctlChkActive.Checked = costCenter.Active;
                ctlBusinessArea.Text = costCenter.BusinessArea;
                ctlProfitCenter.Text = costCenter.ProfitCenter;

            }

        }

        public void HidePopUp()
        {
            ctlModalPopupExtender.Hide();
        }
        #endregion


        #region button
        protected void ctlButtonSave_Click(object sender, EventArgs e)
        {
            if (Mode == FlagEnum.EditFlag)
            {
                DbCostCenter costCenter = DbCostCenterService.FindByIdentity(CostCenterID);

                costCenter.ActualPrimaryCosts = ctlChkLock.Checked;
                costCenter.Active = ctlChkActive.Checked;
                costCenter.CompanyCode = ctlCompanyTextboxAutoComplete.Text;
                costCenter.Description = ctlTextBoxDescription.Text;
                //costCenter.CostCenterCode = ctlTextBoxCostCenterCode.Text;
                costCenter.BusinessArea = ctlBusinessArea.Text;
                costCenter.ProfitCenter = ctlProfitCenter.Text;

                try
                {
                    costCenter.Valid = UIHelper.ParseDate(ctlCalendarValid.DateValue).Value;
                    costCenter.Expire = UIHelper.ParseDate(ctlCalendarExpire.DateValue).Value;
                }
                catch (Exception)
                {
                    Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
                    errors.AddError("CostCenter.Error", new ErrorMessage("Invalid or Null in Datetime fields."));
                    ValidationErrors.MergeErrors(errors);
                    //return;
                }


                costCenter.UpdBy = UserAccount.UserID;
                costCenter.UpdDate = DateTime.Now.Date;
                costCenter.UpdPgm = ProgramCode;

                try
                {
                    CheckDataValueUpdate(costCenter);
                    DbCostCenterService.UpdateCostCenter(costCenter);
                    DbCostCenterService.UpdateCostCenterToExp(costCenter);
                }
                catch (ServiceValidationException ex)
                {
                    ValidationErrors.MergeErrors(ex.ValidationErrors);
                    return;
                }
            }
            if (Mode == FlagEnum.NewFlag)
            {
                DbCostCenter costCenter = new DbCostCenter();
                costCenter.CompanyID = new DbCompany();

                costCenter.ActualPrimaryCosts = ctlChkLock.Checked;
                costCenter.Active = ctlChkActive.Checked;
                costCenter.CompanyCode = ctlCompanyTextboxAutoComplete.Text;
                costCenter.Description = ctlTextBoxDescription.Text;
                costCenter.CostCenterCode = ctlTextBoxCostCenterCode.Text;
                costCenter.BusinessArea = ctlBusinessArea.Text;
                costCenter.ProfitCenter = ctlProfitCenter.Text;

                try
                {
                    costCenter.Valid = UIHelper.ParseDate(ctlCalendarValid.DateValue).Value;
                    costCenter.Expire = UIHelper.ParseDate(ctlCalendarExpire.DateValue).Value;
                }
                catch (Exception)
                {
                    Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
                    errors.AddError("CostCenter.Error", new ErrorMessage("Invalid or Null in Datetime fields."));
                    ValidationErrors.MergeErrors(errors);
                    //return;
                }

                costCenter.UpdBy = UserAccount.UserID;
                costCenter.UpdDate = DateTime.Now.Date;
                costCenter.UpdPgm = ProgramCode;
                costCenter.CreBy = UserAccount.UserID;
                costCenter.CreDate = DateTime.Now.Date;

                try
                {
                    CheckDataValueInsert(costCenter);
                    DbCostCenterService.AddCostCenter(costCenter);
                }
                catch (ServiceValidationException ex)
                {
                    ValidationErrors.MergeErrors(ex.ValidationErrors);
                    return;
                }
            }

            HidePopUp();
            Notify_Ok(sender, e);
        }
        protected void ctlButtonCancel_Click(object sender, EventArgs e)
        {
            HidePopUp();
        }
        #endregion

        private void CheckDataValueInsert(DbCostCenter costCenter)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(costCenter.CostCenterCode))
                errors.AddError("CostCenter.Error", new Spring.Validation.ErrorMessage("RequiredCostCenterCode"));
            if (string.IsNullOrEmpty(costCenter.CompanyCode))
                errors.AddError("CostCenter.Error", new Spring.Validation.ErrorMessage("RequiredCompanyCode"));
            if (ScgDbQueryProvider.DbCostCenterQuery.IsDuplicateCostCenterCode(costCenter))
                errors.AddError("CostCenter.Error", new Spring.Validation.ErrorMessage("DuplicationCostCenterCode"));

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
        }

        private void CheckDataValueUpdate(DbCostCenter costCenter)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(costCenter.CostCenterCode))
                errors.AddError("CostCenter.Error", new Spring.Validation.ErrorMessage("RequiredCostCenterCode"));
            if (string.IsNullOrEmpty(costCenter.CompanyCode))
                errors.AddError("CostCenter.Error", new Spring.Validation.ErrorMessage("RequiredCompanyCode"));
            if ((CostCenterCode.ToString() != ctlTextBoxCostCenterCode.Text) && (ScgDbQueryProvider.DbCostCenterQuery.IsDuplicateCostCenterCode(costCenter)))
                errors.AddError("CostCenter.Error", new Spring.Validation.ErrorMessage("DuplicationCostCenterCode"));

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//Using Related Library
using SS.SU.DTO;
using SS.SU.BLL;
using SS.Standard.UI;
using SS.Standard.Utilities;
using Spring.Validation;
using AjaxControlToolkit;
using Spring.Web.UI.Controls;
using SCG.eAccounting.Web.Helper;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class UserRoleEditor : BaseUserControl
    {

        #region properties

        public string Mode {
            get { return this.ViewState["Mode"] == null ? FlagEnum.NewFlag : (string)this.ViewState["Mode"]; }
            set { this.ViewState["Mode"] = value; } 
        }
        
        public short RoleID
        {
            get { return this.ViewState["RoleID"] == null ? (short)0 : (short)this.ViewState["RoleID"]; }
            set { this.ViewState["RoleID"] = value; }
        }

        public ISuRoleService SuRoleService { get; set; }

        private SuRole role;

        #endregion

        public event EventHandler Notify_Ok;
        public event EventHandler Notify_Cancel;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region PopUp Event

        public void ResetValue()
        {
            ctlTextBoxGroupName.Text = string.Empty;
            ctlTextBoxGroupCode.Text = string.Empty;
            ctlCheckboxActive.Checked = true;
            ctlCheckboxRecieveDoc.Checked = false;
            ctlCheckboxVerifyDoc.Checked = false;
            ctlCheckboxApproveVerifyDoc.Checked = false;
            ctlCheckboxApproveVerifyDoc.Enabled = true;
            ctlTextBoxVerifyMin.Text = string.Empty;
            ctlTextBoxVerifyMax.Text = string.Empty;
            ctlTextBoxApproveVerifyMin.Text = string.Empty;
            ctlTextBoxApproveVerifyMax.Text = string.Empty;
            ctlCheckBoxVerifyPayment.Checked = false;
            ctlCheckBoxApproveVerifyPayment.Checked = false;
            ctlCheckBoxApproveVerifyPayment.Enabled = true;
            ctlCheckBoxCounterCashier.Checked = false;
            ctlCheckboxAllowMultipleApproveAccountant.Enabled = true;
            ctlCheckboxAllowMultipleApproveAccountant.Checked = false;
            ctlCheckBoxAllowMultipleApprovePayment.Enabled = true;
            ctlCheckBoxAllowMultipleApprovePayment.Checked = false;   
            ctlSpecialUseCustomizationLimitAmount.Visible = false;
            ctlLimitAmountForVerifierChange.Text = string.Empty;
            ctlUseCustomizationLimitAmount.Checked = false;
            ctlPanelUserRole.Update();
        }

        public void Initialize(string mode,short roleID)
        {
            Mode = mode.ToString();
            RoleID = roleID;
            if (mode.ToString() == FlagEnum.EditFlag)
            {
                role = SuRoleService.FindByIdentity(roleID);
                ctlTextBoxGroupName.Text = role.RoleName;
                ctlTextBoxGroupCode.Text = role.RoleCode;
                ctlCheckboxActive.Checked = role.Active;
                ctlCheckboxRecieveDoc.Checked = role.ReceiveDocument;
                ctlCheckboxVerifyDoc.Checked = role.VerifyDocument;

                ctlCheckboxApproveVerifyDoc.Checked = role.ApproveVerifyDocument;
                ctlCheckBoxApproveVerifyPayment.Checked = role.ApproveVerifyPayment;
                if (role.VerifyMinLimit ==0)
                    ctlTextBoxVerifyMin.Text = string.Empty;
                else
                    ctlTextBoxVerifyMin.Text = role.VerifyMinLimit.ToString();

                if (role.VerifyMaxLimit == int.MaxValue)
                    ctlTextBoxVerifyMax.Text = string.Empty;
                else
                    ctlTextBoxVerifyMax.Text = role.VerifyMaxLimit.ToString();

                if (role.ApproveVerifyMinLimit == 0)
                    ctlTextBoxApproveVerifyMin.Text = string.Empty;
                else
                    ctlTextBoxApproveVerifyMin.Text = role.ApproveVerifyMinLimit.ToString();

                if (role.ApproveVerifyMaxLimit == int.MaxValue)
                    ctlTextBoxApproveVerifyMax.Text = string.Empty;
                else
                    ctlTextBoxApproveVerifyMax.Text = role.ApproveVerifyMaxLimit.ToString();

                ctlCheckboxAllowMultipleApproveAccountant.Checked = role.AllowMultipleApproveAccountant;
                ctlCheckBoxAllowMultipleApprovePayment.Checked = role.AllowMultipleApprovePayment;
                ctlCheckBoxVerifyPayment.Checked = role.VerifyPayment;
                if (ctlCheckBoxAllowMultipleApprovePayment.Checked)
                {
                    ctlCheckBoxApproveVerifyPayment.Checked = true;
                    ctlCheckBoxApproveVerifyPayment.Enabled = false;
                }
                
                if (ctlCheckboxAllowMultipleApproveAccountant.Checked)
                {
                    ctlCheckboxApproveVerifyDoc.Checked = true;
                    ctlCheckboxApproveVerifyDoc.Enabled = false;
                }
                
                ctlCheckBoxCounterCashier.Checked = role.CounterCashier;
                ctlUseCustomizationLimitAmount.Checked = role.UseCustomizationLimitAmount;
                ctlLimitAmountForVerifierChange.Text = UIHelper.BindDecimal(role.LimitAmountForVerifierChange.ToString());

                if (!ctlUseCustomizationLimitAmount.Checked)
                {
                    ctlLimitAmountForVerifierChange.Text = string.Empty;
                    ctlSpecialUseCustomizationLimitAmount.Visible = false;
                }
                else
                {
                    ctlSpecialUseCustomizationLimitAmount.Visible = true;
                }
                ctlPanelUserRole.Update();
            }
        }

        public void ShowPopUp()
        {
            ctlModalPopupExtender.Show();
        }


        public void HidePopUp()
        {
            ResetValue();
            ctlModalPopupExtender.Hide();
        }   

        #endregion
      
        #region Button Event

        protected void ctlButtonSave_Click(object sender, EventArgs e)
        {
          
            #region create DTO for save Role
            role = new SuRole();
            //role = SuRoleService.FindByIdentity(RoleID);
            role.Active = ctlCheckboxActive.Checked;
            role.UpdBy = UserAccount.UserID;
 
            role.UpdPgm = base.ProgramCode;
            if (Mode == FlagEnum.EditFlag)
            {
                role.CreBy = UserAccount.UserID;
         
            }
            role.RoleCode = ctlTextBoxGroupCode.Text;
            role.RoleName = ctlTextBoxGroupName.Text;
            role.ApproveVerifyDocument = ctlCheckboxApproveVerifyDoc.Checked;
            role.ReceiveDocument = ctlCheckboxRecieveDoc.Checked;
            role.VerifyDocument = ctlCheckboxVerifyDoc.Checked;
            role.ApproveVerifyDocument = ctlCheckboxApproveVerifyDoc.Checked;
            role.VerifyPayment = ctlCheckBoxVerifyPayment.Checked;
            role.ApproveVerifyPayment = ctlCheckBoxApproveVerifyPayment.Checked;
            role.CounterCashier = ctlCheckBoxCounterCashier.Checked;
            role.AllowMultipleApproveAccountant = ctlCheckboxAllowMultipleApproveAccountant.Checked;
            role.AllowMultipleApprovePayment = ctlCheckBoxAllowMultipleApprovePayment.Checked;
            role.UseCustomizationLimitAmount = ctlUseCustomizationLimitAmount.Checked;
            role.LimitAmountForVerifierChange = UIHelper.ParseDouble(ctlLimitAmountForVerifierChange.Text);
            try
            {
                if (!string.IsNullOrEmpty(ctlTextBoxVerifyMax.Text))
                {
                    role.VerifyMaxLimit = UIHelper.ParseDouble(ctlTextBoxVerifyMax.Text);
                }
                else
                {
                    role.VerifyMaxLimit = int.MaxValue;
                }
                if (!string.IsNullOrEmpty(ctlTextBoxVerifyMin.Text))
                {
                    role.VerifyMinLimit = UIHelper.ParseDouble(ctlTextBoxVerifyMin.Text);
                }
                else
                {
                    role.VerifyMinLimit = 0;
                }
                if (!string.IsNullOrEmpty(ctlTextBoxApproveVerifyMax.Text))
                {
                    role.ApproveVerifyMaxLimit = UIHelper.ParseDouble(ctlTextBoxApproveVerifyMax.Text);
                }
                else
                {
                    role.ApproveVerifyMaxLimit = int.MaxValue;
                }
                if (!string.IsNullOrEmpty(ctlTextBoxApproveVerifyMin.Text))
                {
                    role.ApproveVerifyMinLimit = UIHelper.ParseDouble(ctlTextBoxApproveVerifyMin.Text);
                }
                else
                {
                    role.ApproveVerifyMinLimit = 0;
                }
            }
            catch (FormatException)
            {
                Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            errors.AddError("Role.Error",new ErrorMessage("Limit-field area be accept numeric only."));
            ValidationErrors.MergeErrors(errors);
                return;
            }

            #endregion
            try
            {
                if (Mode == FlagEnum.NewFlag)
                {
                    SuRoleService.AddRole(role);
                }

                else if (Mode == FlagEnum.EditFlag)
                {
                    role.RoleID = RoleID;
                    SuRoleService.UpdateRole(role);
                }
                //Auto computer will refresh workflow permission here.
                Notify_Ok(sender, e);
                HidePopUp();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
  

        }

        protected void ctlButtonCancel_Click(object sender, EventArgs e)
        {
            if (Notify_Cancel != null)
            {
                Notify_Cancel(sender, e);
            }
            HidePopUp();

        }

        protected void ctlCheckboxAllowMultipleApproveAccountant_Checked(object sender, EventArgs e)
        {
            if (ctlCheckboxAllowMultipleApproveAccountant.Checked)
            {
                ctlCheckboxApproveVerifyDoc.Checked = true;
                ctlCheckboxApproveVerifyDoc.Enabled = false;
            }
            else
            {
                ctlCheckboxApproveVerifyDoc.Checked = false;
                ctlCheckboxApproveVerifyDoc.Enabled = true;
            }
        }

        protected void ctlCheckBoxAllowMultipleApprovePayment_Checked(object sender, EventArgs e)
        {
            if (ctlCheckBoxAllowMultipleApprovePayment.Checked)
            {
                ctlCheckBoxApproveVerifyPayment.Checked = true;
                ctlCheckBoxApproveVerifyPayment.Enabled = false;
            }
            else
            {
                ctlCheckBoxApproveVerifyPayment.Checked = false;
                ctlCheckBoxApproveVerifyPayment.Enabled = true;
            }
        }

        #endregion


        protected void ctlUseCustomizationLimitAmount_CheckedChanged(object sender, EventArgs e)
        {
            if (!ctlUseCustomizationLimitAmount.Checked)
            {
                ctlLimitAmountForVerifierChange.Text = string.Empty;
                ctlSpecialUseCustomizationLimitAmount.Visible = false;
            }
            else
            {
                ctlSpecialUseCustomizationLimitAmount.Visible = true;
            }
        }

    }
}
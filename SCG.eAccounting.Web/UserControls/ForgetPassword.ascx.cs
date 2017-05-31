using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SS.Standard.UI;
using SS.Standard.Security;

using SS.SU.BLL;
using SS.SU.DTO;
using SS.SU.Query;

using SS.DB.Query;
using SS.DB.DTO;

using SCG.eAccounting.Web.Helper;
using SS.Standard.Utilities;
using SCG.eAccounting.BLL;

namespace SCG.eAccounting.Web.UserControls
{

    public partial class ForgetPassword : BaseUserControl
    {
        public ISuUserService SuUserService { get; set; }
        public ISCGEmailService SCGEmailService { get; set; }
        public ParameterServices ParameterServices { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ctlConfirm_Click(object sender, EventArgs e)
        {
            
            //Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            try
            {
                SuUser user = new SuUser();
                user.UserName = ctlUserName.Text;
                string realPassword = SuUserService.Forgetpassword(ctlUserName.Text);
                user = SuUserService.FindByUserName(ctlUserName.Text).First<SuUser>();

                if (user != null)
                    UserEngine.SyncUpdateUserData(user.UserName);

                SCGEmailService.SendEmailEM12(user.Userid, realPassword.ToString());

                //Alert(GetMessage("ForgetPasswordIsComplete"));
                this.ModalPopupMsg.Show();
                HidePopup();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }

        protected void ctlCancel_Click(object sender, EventArgs e)
        {
            HidePopup();
        }

        public void ShowPopup()
        {
            ModalPopupExtender1.Show();
            UpdatePanelChangePassword.Update();
        }

        public void HidePopup()
        {
            ctlUserName.Text = string.Empty;
            this.ModalPopupExtender1.Hide();
        }

        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            HidePopup();
        }

        #region private void Alert(string Message)
        private void Alert(string Message)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "DisableDropdown", "alert('" + Message + "');", true);
        }
        #endregion private void Alert(string Message)


        protected void imgClose1_Click(object sender, ImageClickEventArgs e)
        {
            this.ModalPopupMsg.Hide();
        }

        protected void ctlBtnClose_Click(object sender, EventArgs e)
        {
            this.ModalPopupMsg.Hide();
        }
    }    
}
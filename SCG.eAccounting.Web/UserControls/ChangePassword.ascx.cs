using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SS.SU.DTO;

using SS.SU.BLL;
using SS.Standard.Security;
using SS.Standard.UI;

using SS.DB.Query;

using System.Threading;
using SS.Standard.Utilities;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class ChangePassword : BaseUserControl
    {
        public IUserEngineService UserEngineService { get; set; }
        public IDbLanguageQuery DbLanguageQuery { get; set; }
        public IDbParameterQuery DbParameterQuery { get; set; }
        public ISuUserService SuUserService { get; set; }



        #region public bool isResponseMenu
        public bool isResponseMenu
        {
            get
            {
                if (string.IsNullOrEmpty(ViewState["isResponseMenu"].ToString()))
                    return false;
                else
                    return bool.Parse(ViewState["isResponseMenu"].ToString());
            }
            set { ViewState["isResponseMenu"] = value; }
        }
        #endregion public bool isResponseMenu

        public string userName
        {
            get
            {
                if (string.IsNullOrEmpty(ViewState["userName"].ToString()))
                    return string.Empty;
                else
                    return ViewState["userName"].ToString();
            }
            set { ViewState["userName"] = value; }
        }


        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #endregion protected void Page_Load(object sender, EventArgs e)


        #region public void ShowPopup()
        public void ShowPopup(string ProgramCode, bool isResponseMenu, string userName)
        {
            //this.ProgramCode    = ProgramCode;
            this.isResponseMenu = isResponseMenu;
            this.userName = userName;

            ctlOldPasswordTextbox.Text = "";
            ctlNewPasswordTextbox.Text = "";
            ctlConfirmPasswordTextbox.Text = "";
            ctlPanelModalPopupExtender.Show();
        }
        #endregion public void ShowPopup()

        #region public void HidePopup()
        public void HidePopup()
        {
            ctlPanelModalPopupExtender.Hide();
        }
        #endregion public void HidePopup()

        #region protected void ctlCancelUpdate_Click(object sender, EventArgs e)
        protected void ctlCancelUpdate_Click(object sender, EventArgs e)
        {
            ctlOldPasswordTextbox.Text = "";
            ctlNewPasswordTextbox.Text = "";
            ctlConfirmPasswordTextbox.Text = "";
            ctlPanelModalPopupExtender.Hide();
        }
        #endregion protected void ctlCancelUpdate_Click(object sender, EventArgs e)

        #region protected void ctlUpdate_Click(object sender, EventArgs e)
        protected void ctlUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                SuUser user;

                if (UserAccount.UserID > 0)
                {
                    user = UserEngine.ChangePassword(UserAccount.UserID, ctlOldPasswordTextbox.Text.Trim(), ctlNewPasswordTextbox.Text.Trim(), ctlConfirmPasswordTextbox.Text.Trim(), ProgramCode);
                }
                else
                {
                    user = UserEngine.ChangePassword(userName, ctlOldPasswordTextbox.Text.Trim(), ctlNewPasswordTextbox.Text.Trim(), ctlConfirmPasswordTextbox.Text.Trim(), ProgramCode);
                }

                if (user != null)
                    UserEngine.SyncUpdateUserData(user.UserName);

                if (isResponseMenu && user != null && user.Userid > 0)
                    Response.Redirect(ResolveUrl("~/Menu.aspx"), true);
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
            }

            if (this.ValidationErrors.IsEmpty)
            {
                ctlPanelModalPopupExtender.Hide();
            }
            else
            {
                vsSummary.Visible = true;
                ctlPanelModalPopupExtender.Show();
            }

        }
        #endregion protected void ctlUpdate_Click(object sender, EventArgs e)

        #region protected void imgClose_Click(object sender, ImageClickEventArgs e)
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            HidePopup();
        }
        #endregion protected void imgClose_Click(object sender, ImageClickEventArgs e)
    }
}
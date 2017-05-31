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

using SS.SU.Query;
using System.Threading;

using SS.DB.Query;
using SS.DB.DTO;
using SS.Standard.Utilities;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class UserInfo : BaseUCTranslations
    {
        public IUserEngineService UserEngineService { get; set; }
        public IDbLanguageQuery DbLanguageQuery { get; set; }
        public IDbParameterQuery DbParameterQuery { get; set; }
        


        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    List<DbLanguage> language = DbLanguageQuery.FindAllList().ToList<DbLanguage>();
            //    ctlDropDownListChangeLanguage.DataSource = language;
            //    ctlDropDownListChangeLanguage.DataTextField = "LanguageName";
            //    ctlDropDownListChangeLanguage.DataValueField = "LanguageID";
            //    ctlDropDownListChangeLanguage.DataBind();
            //    ctlDropDownListChangeLanguage.SelectedValue = UserAccount.CurrentLanguageID.ToString();
            //    SetUserInfo();
            //}
        }
        protected void ctlLingButtonSignOut_Click(object sender, EventArgs e)
        {
            //UserEngineService.SignOut(UserAccount.UserID);
            //UserEngineService.SignOutClearSession();
        }
        protected void ctlButtonChangePassword_Click(object sender, EventArgs e)
        {
            ctlOldPasswordTextbox.Text = "";
            ctlNewPasswordTextbox.Text = "";
            ctlConfirmPasswordTextbox.Text = "";
            //ctlErrorValidationLabel.Text = "";
            ctlPanelModalPopupExtender.Show();
        }
        private void SetUserInfo()
        {
            DbLanguage language = DbLanguageQuery.FindByIdentity(UserAccount.CurrentLanguageID);
            //UserEngine.SetLanguage(language.Languageid);
            //ctlFlagImage.ImageUrl = DbParameterQuery.getParameterByGroupNo_SeqNo("1","7").ToString()+language.ImagePath;
            
            //if (UserAccount.CurrentLanguageCode.ToString().Equals("th") || UserAccount.CurrentLanguageCode.ToString().Equals("th-TH") || UserAccount.CurrentLanguageCode.ToString().Equals("THA"))
           // ctl_Loged_FullName_Label.Text = UserAccount.FirstName + "   "+ UserAccount.LastName;// "อภิเดช วงษา";
           // else
           //     ctl_Loged_FullName_Label.Text = "Apidesh Wongsa";

            //TODO:
            // Change UserAccount.FirstName + "  " + UserAccount.LastName to UserAccount name by language
            
        }
        protected void ctlDropDownListChangeLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            UserAccount.SetLanguage(Helper.UIHelper.ParseShort(ctlDropDownListChangeLanguage.SelectedValue));
            ctlDropDownListChangeLanguage.SelectedValue = UserAccount.CurrentLanguageID.ToString();
            SetCulture();

                Response.Redirect(System.Web.HttpContext.Current.Request.Url.AbsolutePath);

        }


        private string GetCurrentPageName()
        {
            string sPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
            System.IO.FileInfo oInfo = new System.IO.FileInfo(sPath);
            string sRet = oInfo.Name;
            return sRet;
        }

        protected void ctlUpdate_Click(object sender, EventArgs e)
        {
			try
			{
				SuUser user = UserEngine.ChangePassword(UserAccount.UserID, ctlOldPasswordTextbox.Text, ctlNewPasswordTextbox.Text, ctlConfirmPasswordTextbox.Text, ProgramCode);
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
				ctlPanelModalPopupExtender.Show();
			}
			#region Old Code Comment By Pichai C. on 2-Feb-2009
			//if (ctlNewPasswordTextbox.Text != ctlConfirmPasswordTextbox.Text)
			//{
			//    ctlErrorValidationLabel.Text = "New password not same Confirm password";
			//    ctlPanelModalPopupExtender.Show();

			//}
			//else
			//{
			//    SuUser user  = UserEngine.ChangePassword(UserAccount.UserID, ctlOldPasswordTextbox.Text, ctlNewPasswordTextbox.Text, ctlConfirmPasswordTextbox.Text, "UserInfo");
			//    if (flag.Equals("sucessfull"))
			//    {
			//        ctlErrorValidationLabel.Text = "Change password successfull";
			//        Thread.Sleep(1000);
			//        ctlPanelModalPopupExtender.Hide();
			//    }
			//    else
			//    {
			//        ctlErrorValidationLabel.Text = flag;
			//        ctlPanelModalPopupExtender.Show();
			//    }

			//}
			#endregion
        }

        protected void ctlMsgCloseImageButton_Click(object sender, ImageClickEventArgs e)
        {
            ctlPanelModalPopupExtender.Hide();
        }
    }
}
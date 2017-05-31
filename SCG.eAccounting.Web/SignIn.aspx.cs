using System;
using System.Collections;
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
using SS.Standard.Security;
using SS.Standard.UI;
using AjaxControlToolkit;

using SS.SU.DTO;
using SS.SU.BLL;
using SS.SU.Query;


using SCG.eAccounting.Web.Helper;

using SS.DB.Query;
using SS.DB.DTO;
using SS.DB.BLL;

namespace SCG.eAccounting.Web
{
    public partial class SignIn : BasePage
    {
        public IDbParameterQuery DbParameterQuery { get; set; }
        public IDbLanguageQuery DbLanguageQuery { get; set; }
       
        protected void Page_Load(object sender, EventArgs e)
        {
            ctl_Password_Textbox.Attributes.Add("OnKeyPress", "enter()");
            if (!IsPostBack)
            {
                
                ctl_UserName_Textbox.Focus();

                short languageId = UIHelper.ParseShort(DbParameterQuery.getParameterByGroupNo_SeqNo("1", "6"));
                DbLanguage language = DbLanguageQuery.FindByIdentity(languageId);
                this.UserCulture = new System.Globalization.CultureInfo(language.LanguageCode);
            }

            
        }
        protected void x()
        { 
            
        
        }
        protected void OpenPDF()
        {
            Response.Write("xx");
        }
        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if ((String.IsNullOrEmpty(RadioButtonList1.SelectedValue)) == false)
            //{
            //    // Popup result is the selected task
            //    PopupControlExtender.GetProxyForCurrentPopup(this.Page).Commit(RadioButtonList1.SelectedValue);
            //}
            //// Reset the selected item
            //RadioButtonList1.ClearSelection();
        }
        protected void ctl_SignIn_Click(object sender, ImageClickEventArgs e)
        {
            GetAuthentication();
        }
        protected void ctl_Cancel_Click(object sender, ImageClickEventArgs e)
        {
            //ctl_UserName_Validator.Dispose();
            //ctl_Password_Validator.Dispose();
            ctl_UserName_Textbox.Text = string.Empty;
            ctl_Password_Textbox.Text = string.Empty;

        }
        private void GetAuthentication()
        {


           

            if (UserEngine.SignIn(ctl_UserName_Textbox.Text.Trim(), ctl_Password_Textbox.Text.Trim()))
            {
                Response.Redirect(ResolveUrl("Menu.aspx"), true);
            }
            else
            {
                Session["xxx"] = "ss";
                //DialogHeader.Text = "Error";
                DialogOkButton.Focus();
                ModalPopupExtenderMsg.Show();
                ctl_ErrorMessage_Label.Visible = true;
                ctl_ErrorMessage_Label.Text = "$SignInFailed$";
            }
        }
    }
}

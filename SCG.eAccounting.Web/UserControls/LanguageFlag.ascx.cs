using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SS.Standard.UI;

using SS.DB.Query;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class LanguageFlag : BaseUCTranslations
    {

        public ParameterServices ParameterServices { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (UserAccount.CurrentLanguageID == 1)
            {
                CtlThaiFlagButton.Visible = false;
                CtlEnglishFlagButton.Visible = true;
            }
            else
            {
                CtlThaiFlagButton.Visible = true;
                CtlEnglishFlagButton.Visible = false;
            }
        }
     
        protected void CtlEnglishFlagButton_Click(object sender, ImageClickEventArgs e)
        {
            SetLanguage("2");
        }

        protected void CtlThaiFlagButton_Click(object sender, ImageClickEventArgs e)
        {
            SetLanguage("1");
        }
        private void SetLanguage(string languageID)
        {
            short langID = Helper.UIHelper.ParseShort(languageID);
            UserAccount.SetLanguage(langID);
            
            SetCulture();
            Response.Redirect(System.Web.HttpContext.Current.Request.Url.AbsoluteUri);
        }


    }
}
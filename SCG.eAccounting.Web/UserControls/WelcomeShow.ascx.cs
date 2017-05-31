using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SS.SU.BLL;
using SS.SU.DTO.ValueObject;
using SCG.eAccounting.Web.Helper;
using SS.SU.Query;

using SS.DB.DTO;
using SS.DB.Query;
using SS.DB.BLL;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class WelcomeShow : BaseUserControl
    {
        public ISuRTENodeService SuRTENodeService { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            short languageId;
            if (!Page.IsPostBack)
            {
                // How to get language ID
                languageId = UserAccount.CurrentLanguageID;
                SuRTENodeSearchResult rteNode = SuRTENodeService.GetWelcome(languageId, "Welcome");
                if (rteNode != null)
                {
                    ctlHeader.Text = rteNode.Header;
                    ctlContent.Text = rteNode.Content;
                }
                System.Drawing.Image image = System.Drawing.Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "\\App_Themes\\Default\\images\\Welcome.png");
                ctlPanel.Width = image.Width;
                //ctlPanel.Height = image.Height;
            }
        }
    }
}
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SCG.eAccounting.Web.Helper;

using SS.DB.Query;
using SS.DB.DTO;
using SS.DB.BLL;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.SU.BLL;
using SS.SU.Query;

using SS.Standard.Security;
using SS.Standard.UI;

namespace SCG.eAccounting.Web
{
    public partial class Login : BasePage
    {
        public ISuRTENodeService SuRTENodeService { get; set; }
        public IDbParameterQuery DbParameterQuery { get; set; }
        public IDbLanguageQuery DbLanguageQuery { get; set; }
        string nodeId;
        short languageId;

        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            languageId = UserAccount.CurrentLanguageID;
            DbLanguage language = DbLanguageQuery.FindByIdentity(languageId);
            if (language != null)
                this.UserCulture = new System.Globalization.CultureInfo(language.LanguageCode);

            nodeId = Request.QueryString["nodeId"];
            if (nodeId != null)
            {
                dvContent.Visible = true;
                dvWelcomeMsg.Visible = false;
                getRTENode(languageId);
                updPanelShow.Update();
            }
            else
            {
                dvContent.Visible = false;
                dvWelcomeMsg.Visible = true;
                updPanelShow.Update();
            }

            //this.UserCulture = new System.Globalization.CultureInfo(language.LanguageCode);
        }
        #endregion protected void Page_Load(object sender, EventArgs e)

        #region private void getRTENode(short languageId)
        private void getRTENode(short languageId)
        {
            IList<SuRTENodeSearchResult> list = SuRTENodeService.GetRTEContent(languageId, "MainMenu", UIHelper.ParseShort(nodeId));
            if (list.Count > 0)
            {
                ctlHeader.Text = list[0].Header.ToString();
                ctlContent.Text = list[0].Content.ToString();
            }
        }
        #endregion private void getRTENode(short languageId)

    }
}

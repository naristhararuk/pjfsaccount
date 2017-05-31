using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SS.SU.Query;
using SCG.eAccounting.Web.UserControls;
using SS.SU.DTO;
using SS.Standard.Utilities;
using SCG.DB.DTO;
using SCG.DB.Query;
using SS.DB.Query;
using SS.DB.DTO;
using SS.SU.BLL;
using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.BLL;

namespace SCG.eAccounting.Web.Forms.SU.Programs
{
    public partial class UserProfile : BasePage
    {
        public ISuUserService SuUserService { get; set; }
        public ISuPasswordHistoryService SuPasswordHistoryService { get; set; }
        public ParameterServices ParameterServices { get; set; }
        public ISCGEmailService SCGEmailService { get; set; }

        public string Mode
        {
            get
            {
                if (ViewState["mode"] != null)
                    return ViewState["mode"].ToString();
                return string.Empty;
            }
            set { ViewState["mode"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ctlUserProfileEditor.Initialize(FlagEnum.EditFlag, UserAccount.UserID);
                ctlUpdateUserProfilePanel.Update();
            }
            ctlUserProfileEditor.Notify_Cancle += new EventHandler(Notify);
            ctlUserProfileEditor.Notify_Ok += new EventHandler(Notify);
        }
        protected void ctlApproverButton_Click(object sender, EventArgs e)
        {
            ctlApproverEditor.Initialize(UserAccount.UserID);
            ctlApproverEditor.ShowDetail();
            ctlUpdateUserProfilePanel.Update();
        }
        protected void ctlInitiatorButton_Click(object sender, EventArgs e)
        {
            ctlInitiatorEditor.Initialize(UserAccount.UserID);
            ctlInitiatorEditor.ShowDetail();
            ctlUpdateUserProfilePanel.Update();
        }

        protected void Notify(object sender, EventArgs e)
        {
            ctlUserProfileEditor.Initialize(FlagEnum.EditFlag, UserAccount.UserID);
            ctlUpdateUserProfilePanel.Update();
    }
}
}

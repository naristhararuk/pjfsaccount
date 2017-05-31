using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SS.SU.BLL;
using SCG.eAccounting.Web.Helper;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class LoginInformation : BaseUserControl
    {
        public ISuStatisticService SuStatisticService { get; set; }
        public IUserEngineService UserEngineService { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            ctlWebStatisticLabel.Text = "Web Statistic " + DateTime.Now.ToShortDateString();
            ctlYouVisitorNumber.Text = string.Format("{0:#,##0}",SuStatisticService.FindByIdentity(1).StatisticNumber);

            ctlCurrentUserOnline.Text = string.Format("{0:#,##0}", UIHelper.ParseInt((UserEngineService.CountSession()).ToString()));
            ctlTodayUseLogin.Text = string.Format("{0:#,##0}",SuStatisticService.FindByIdentity(2).StatisticNumber);
        }
    }
}
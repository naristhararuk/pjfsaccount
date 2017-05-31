using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SS.SU.BLL;
using SS.SU.Query;
using SS.SU.DTO;

namespace SCG.eAccounting.Web.Forms.SU.Programs
{
    public partial class TestMasterGrid : BasePage
    {
        private ISuMenuQuery suMenuQuery;
        public ISuMenuQuery SuMenuQuery
        {
            set { this.suMenuQuery = value; }
        }
        private ISuMenuService suMenuService;
        public ISuMenuService SuMenuService
        {
            set { this.suMenuService = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SuMenu m = new SuMenu();
            //if (!Page.IsPostBack)
            //{
            m.CreBy = UserAccount.UserID;
            m.CreDate = DateTime.Now.Date;
            m.UpdBy = UserAccount.UserID;
            m.UpdDate = DateTime.Now.Date;
            m.UpdPgm = "TestMasterGrid";
            //}
            ctlMasterGrid1.ObjectQuery = suMenuQuery;
            ctlMasterGrid1.DTO = m;
            ctlMasterGrid1.ObjectService = suMenuService;
        }

    }
}

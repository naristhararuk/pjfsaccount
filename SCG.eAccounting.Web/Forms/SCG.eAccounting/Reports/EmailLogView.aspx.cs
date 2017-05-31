using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SS.Standard.UI;

namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Reports
{
    public partial class EmailLogView : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["isDuplication"] != null)
            {
                ctlEmailLog.isDisplayCriteria = Convert.ToBoolean(Request.Params["isDuplication"]);
            }
            else
            {
                ctlEmailLog.isDisplayCriteria = true;
            }
            if (Request.Params["EmailType"] != null)
            {
                ctlEmailLog.EmailType = Request.Params["EmailType"].ToString();
            }
            if (Request.Params["RequestNo"] != null)
            {
                ctlEmailLog.RequestNo = Request.Params["RequestNo"].ToString();
            }
            ctlUpdatePanelCriteria.Update();
        }
    }
}

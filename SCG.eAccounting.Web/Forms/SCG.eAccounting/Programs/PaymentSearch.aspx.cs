using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using SS.Standard.UI;

namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs
{
    public partial class PaymentSearch : BasePage
    {
        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ctlInboxAccountantPaymentSearchCriteria.DataBind();
                ctlInboxAccountantPaymentSearchCriteria.Legend = GetMessage("WS_SearchResult");
                ctlUpdatePanel.Update();
            }
        }
        #endregion protected void Page_Load(object sender, EventArgs e)
    }
}

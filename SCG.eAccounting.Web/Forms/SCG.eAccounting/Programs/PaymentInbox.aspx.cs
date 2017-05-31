using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;

namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs
{
    public partial class PaymentInbox : BasePage
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

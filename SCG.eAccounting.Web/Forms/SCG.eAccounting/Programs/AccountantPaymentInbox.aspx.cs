using System;
using SS.Standard.UI;

namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs
{
    public partial class AccountantPaymentInbox : BasePage
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

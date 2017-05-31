using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCG.eAccounting.BLL;
using SS.Standard.UI;

namespace SCG.eAccounting.Web.DocumentView
{
    public partial class ForceLock : BasePage
    {
        public IDocumentViewLockService DocumentViewLockService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

            string ctlDocumentID = Request.QueryString["documentID"] == null ? "" : Request.QueryString["documentID"].ToString();
            string ctlUserID = Request.QueryString["userID"] == null ? "" : Request.QueryString["userID"].ToString();
            try
            {
                if (ctlDocumentID.Trim().Length > 0 && ctlUserID.Trim().Length > 0)
                {
                    DocumentViewLockService.ForceLock(SS.Standard.Utilities.Utilities.ParseLong(ctlDocumentID.Trim()), SS.Standard.Utilities.Utilities.ParseLong(ctlUserID.Trim()));
                }
                Response.Write("true");
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

    }
}

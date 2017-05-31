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
    public partial class UnLock : BasePage
    {
        public ITransactionService TransactionService { get; set; }
        public IDocumentViewLockService DocumentViewLockService { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            string ctlDocumentID = Request.Params["documentID"] == null ? "" : Request.Params["documentID"].ToString();
            string ctlUserID = Request.Params["userID"] == null ? "" : Request.Params["userID"].ToString();
            try
            {
                if (!string.IsNullOrEmpty(Request.Params["txID"]))
                {
                    TransactionService.Rollback(new Guid(Request.Params["txID"]));
                }
                if (ctlDocumentID.Trim().Length > 0)
                {
                    DocumentViewLockService.UnLock(SS.Standard.Utilities.Utilities.ParseLong(ctlDocumentID.Trim()), SS.Standard.Utilities.Utilities.ParseLong(ctlUserID.Trim()));
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

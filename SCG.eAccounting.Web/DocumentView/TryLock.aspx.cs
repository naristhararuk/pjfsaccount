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
    public partial class SetLock : BasePage
    {
        public IDocumentViewLockService DocumentViewLockService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            string ctlDocumentID = Request.Params["documentID"] == null ? "" : Request.Params["documentID"].ToString();
            string ctlUserID = Request.Params["userID"] == null ? "" : Request.Params["userID"].ToString();
            string ctlLockFlag = Request.Params["LockFlag"] == null ? "" : Request.Params["LockFlag"].ToString();
            try
            {
                if (ctlDocumentID.Trim().Length > 0 && ctlUserID.Trim().Length>0)
                {
                    DocumentViewLockService.TryLock(SS.Standard.Utilities.Utilities.ParseLong(ctlDocumentID.Trim()), SS.Standard.Utilities.Utilities.ParseLong(ctlUserID.Trim()), SS.Standard.Utilities.Utilities.ParseBool(ctlLockFlag.Trim()));
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

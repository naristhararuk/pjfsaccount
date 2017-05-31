using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class PopupCaller : System.Web.UI.UserControl
    {
        //private string url;
        //private string text;

        public String ReferenceValue
        {
            get { return ViewState["referenceValue"] == null ? "" : ViewState["referenceValue"].ToString(); }
            set { ViewState["referenceValue"] = value; }
        }

        public String URL
        {
            get
            {
                if (ViewState["url"] != null)
                    return ViewState["url"].ToString();
                return string.Empty;
            }
            set { ViewState["url"] = value; }
        }

        public String ProcessedURL
        {
            get { return Page.ResolveUrl(URL + "&_pid=" + ClientID).Replace("\\", "\\\\").Replace("'", "\'"); }
        }

        public String ButtonSkinID
        {
            get { return ViewState["skinID"].ToString(); }
            set { ViewState["skinID"] = value; }
        }
        public bool Hide
        {
            set
            {
                if (value)
                    ctlPopup.Style["display"] = "none";
                else
                    ctlPopup.Style["display"] = "block";
            }
        }
        public String Width
        {
            get { return ViewState["width"] == null ? "800" : ViewState["width"].ToString(); }
            set { ViewState["width"] = value; }
        }
        public string Height
        {
            get { return ViewState["height"] == null ? "600" : ViewState["height"].ToString(); }
            set { ViewState["height"] = value; }
        }
        public delegate void NotifyPopupResultHandler(object sender, string returnAction, string returnValue);
        public event NotifyPopupResultHandler NotifyPopupResult;

        public delegate bool NotifyPopupCallingHandler(object sender);
        public event NotifyPopupCallingHandler NotifyPopupCalling;


        protected void Page_Load(object sender, EventArgs e)
        {
            //ctlPopup.DataBind();
        }
        protected void ctlReturnAction_ValueChanged(object sender, EventArgs e)
        {
            /*
            if (ctlReturnStatus.Value == "0")
            {

                if (Cancel != null) Cancel(sender);
            }
            else
            {

                if (OK != null) OK(sender, ctlReturnValue.Value);
            }*/
            NotifyPopupResult(this, ctlReturnAction.Value, ctlReturnValue.Value);
            ctlReturnAction.Value = "";

        }

        public string PopupScript
        {
            get
            {
                return this.ClientID + "_popup();";
            }
        }

        protected void ctlPopup_Click(object sender, EventArgs e)
        {
            bool noError = true;
            if (NotifyPopupCalling != null)
            {
                noError = NotifyPopupCalling(this);
            }
            if (noError)
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "popup", PopupScript, true);
        }
    }
}
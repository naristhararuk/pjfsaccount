using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.Query;
using SCG.eAccounting.DTO.ValueObject;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.eAccounting
{
    public partial class MRLookUp : BaseUserControl
    {
        #region Properties
        private Guid mRRevitionId;

        public Guid MRRevitionId
        {
            get { return mRRevitionId; }
            set { mRRevitionId = value; }
        }
        
        //public Guid mRRevitionId;
        //public Guid? MRRevitionId
        //{
        //    set { ViewState["MRRevitionId"] = value; }
        //    get { return ViewState["MRRevitionId"] == null ? 0 : UIHelper.ParseLong(ViewState["MRRevitionId"].ToString()); }
        //}
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ctlMRLookUp_NotifyPopupResult(object sender, string action, string value)
        {
            if (action != "ok") return;

            //object returnValue = new object();
            //** get data info after select lookup item         
            //CallOnObjectLookUpReturn(returnValue);
        }

        public void Show()
        {
            CallOnObjectLookUpCalling();
            string popupURL = "~/UserControls/LOV/SCG.eAccounting/MileageRateRevisionLookUp.aspx?MRRevitionId={0}";
            ctlMRLookUpPopupCaller.URL = string.Format(popupURL, new object[] { MRRevitionId });
            //ctlMRFixedAdvanceLookUpPopupCaller.ReferenceValue = MRRevitionId.ToString();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), ctlMRLookUpPopupCaller.ClientID + "_popup()", ctlMRLookUpPopupCaller.ClientID + "_popup('" + ctlMRLookUpPopupCaller.ProcessedURL + "')", true);
        }

        public void Hide()
        {
        }
    }
}
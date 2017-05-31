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
    public partial class MPALookup : BaseUserControl
    {
        #region Properties
        public bool isMultiple
        {
            get
            {
                return ViewState["IsMultipleReturn"] == null ? false : Convert.ToBoolean(ViewState["IsMultipleReturn"].ToString());
            }
            set { ViewState["IsMultipleReturn"] = value; }
        }
        public long? CompanyID
        {
            get
            {
                return ViewState["MPADocumentLookupCompanyID"] == null ? 0 : UIHelper.ParseLong(ViewState["MPADocumentLookupCompanyID"].ToString());
            }
            set { ViewState["MPADocumentLookupCompanyID"] = value; }
        }
        public long? RequesterID
        {
            get
            {
                return ViewState["MPADocumentLookupRequesterID"] == null ? 0 : UIHelper.ParseLong(ViewState["MPADocumentLookupRequesterID"].ToString());
            }
            set { ViewState["MPADocumentLookupRequesterID"] = value; }
        }
        public long? CurrentUserID
        {
            set { ViewState["CurrentUserID"] = value; }
            get { return ViewState["CurrentUserID"] == null ? 0 : UIHelper.ParseLong(ViewState["CurrentUserID"].ToString()); }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void ctlMPADocumentLookupPopupCaller_NotifyPopupResult(object sender, string action, string value)
        {
            if (action != "ok") return;

            object returnValue = new object();
            //** get data info after select lookup item
            if (!isMultiple)
            {
                IList<ExpensesMPA> mpaDocment = ScgeAccountingQueryProvider.MPADocumentQuery.FindByMPADocumentID(UIHelper.ParseLong(value));
                returnValue = mpaDocment;
            }
            else
            {
                string[] listID = value.Split('|');
                IList<ExpensesMPA> list = new List<ExpensesMPA>();
                foreach (string id in listID)
                {
                      IList<ExpensesMPA> mpaDocment = ScgeAccountingQueryProvider.MPADocumentQuery.FindByMPADocumentID(UIHelper.ParseLong(id));
                    if (mpaDocment != null && mpaDocment.Count > 0)
                       list.Add(mpaDocment[0]);
                }
               returnValue = list;
            }
            
            CallOnObjectLookUpReturn(returnValue);

        }

        public void Show()
        {
            CallOnObjectLookUpCalling();
            string popupURL = "~/UserControls/LOV/SCG.eAccounting/MPADocumentLookUp.aspx?isMultiple={0}&CompanyID={1}&RequesterID={2}&CurrentUser={3}";
            ctlMPALookupPopupCaller.URL = string.Format(popupURL, new object[] { isMultiple, CompanyID, RequesterID, CurrentUserID});
            ctlMPALookupPopupCaller.ReferenceValue = isMultiple.ToString();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), ctlMPALookupPopupCaller.ClientID + "_popup()", ctlMPALookupPopupCaller.ClientID + "_popup('" + ctlMPALookupPopupCaller.ProcessedURL + "')", true);
        }

        public void Hide()
        {
        }


    }
}
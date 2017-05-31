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
    public partial class CALookup : BaseUserControl
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
                return ViewState["CADocumentLookupCompanyID"] == null ? 0 : UIHelper.ParseLong(ViewState["CADocumentLookupCompanyID"].ToString());
            }
            set { ViewState["CADocumentLookupCompanyID"] = value; }
        }
        public long? RequesterID
        {
            get
            {
                return ViewState["CADocumentLookupRequesterID"] == null ? 0 : UIHelper.ParseLong(ViewState["CADocumentLookupRequesterID"].ToString());
            }
            set { ViewState["CADocumentLookupRequesterID"] = value; }
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


        protected void ctlCADocumentLookupPopupCaller_NotifyPopupResult(object sender, string action, string value)
        {
            if (action != "ok") return;

            object returnValue = new object();
            //** get data info after select lookup item
            if (!isMultiple)
            {
                IList<ExpenseCA> caDocment = ScgeAccountingQueryProvider.CADocumentQuery.FindByCADocumentID(UIHelper.ParseLong(value));
                returnValue = caDocment;
            }
            else
            {
                string[] listID = value.Split('|');
                IList<ExpenseCA> list = new List<ExpenseCA>();
                foreach (string id in listID)
                {
                      IList<ExpenseCA> caDocment = ScgeAccountingQueryProvider.CADocumentQuery.FindByCADocumentID(UIHelper.ParseLong(id));
                      if (caDocment != null && caDocment.Count > 0)
                          list.Add(caDocment[0]);
                }
               returnValue = list;
            }
            
            CallOnObjectLookUpReturn(returnValue);

        }

        public void Show()
        {
            CallOnObjectLookUpCalling();
            string popupURL = "~/UserControls/LOV/SCG.eAccounting/CADocumentLookUp.aspx?isMultiple={0}&CompanyID={1}&RequesterID={2}";
            ctlCALookupPopupCaller.URL = string.Format(popupURL, new object[] { isMultiple, CompanyID, RequesterID });
            ctlCALookupPopupCaller.ReferenceValue = isMultiple.ToString();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), ctlCALookupPopupCaller.ClientID + "_popup()", ctlCALookupPopupCaller.ClientID + "_popup('" + ctlCALookupPopupCaller.ProcessedURL + "')", true);
        }

        public void Hide()
        {
        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;

using SS.Standard.UI;

using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.Query;
using SCG.eAccounting.DTO;


namespace SCG.eAccounting.Web.UserControls.LOV.AV
{
    public partial class AdvanceLookup : BaseUserControl
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

        public bool IsRepOffice
        {
            get
            {
                return ViewState["IsRepOffice"] == null ? false : Convert.ToBoolean(ViewState["IsRepOffice"].ToString());
            }
            set { ViewState["IsRepOffice"] = value; }
        }
        public long? CompanyID
        {
            get
            {
                return ViewState["AdvanceLookupCompanyID"] == null ? 0 : UIHelper.ParseLong(ViewState["AdvanceLookupCompanyID"].ToString());
            }
            set { ViewState["AdvanceLookupCompanyID"] = value; }
        }
        public long? PBID
        {
            get
            {
                return ViewState["PBID"] == null ? 0 : UIHelper.ParseLong(ViewState["PBID"].ToString());
            }
            set { ViewState["PBID"] = value; }
        }
        public short? MainCurrencyID
        {
            get
            {
                return ViewState["MainCurrencyID"] == null ? (short)0 : UIHelper.ParseShort(ViewState["MainCurrencyID"].ToString());
            }
            set { ViewState["MainCurrencyID"] = value; }
        }
        public string AdvanceType
        {
            get
            {
                return ViewState["IsMultiPleReturn"] == null ? null : ViewState["IsMultiPleReturn"].ToString();
            }
            set { ViewState["IsMultiPleReturn"] = value; }
        }
        public long? RequesterID
        {
            get
            {
                return ViewState["AdvanceLookupRequesterID"] == null ? 0 : UIHelper.ParseLong(ViewState["AdvanceLookupRequesterID"].ToString());
            }
            set { ViewState["AdvanceLookupRequesterID"] = value; }
        }
        public string Text
        {
            get
            {
                if (string.IsNullOrEmpty(ctlAdvanceNo.Text) || string.IsNullOrEmpty(ctlDescription.Text))
                {
                    return string.Empty;
                }
                else
                {
                    return ctlAdvanceNo.Text + '-' + ctlDescription.Text;
                }
            }
        }

        public long? CurrentUserID
        {
            set { ViewState["CurrentUserID"] = value; }
            get { return ViewState["CurrentUserID"] == null ? 0 : UIHelper.ParseLong(ViewState["CurrentUserID"].ToString()); }
        }
        private bool isRelateWithRemittanceButNotInExpense;
        public bool IsRelateWithRemittanceButNotInExpense
        {
            set
            {
                ViewState["isRelateWithRemittanceButNotInExpense"] = value;
            }
            get
            {
                return ViewState["isRelateWithRemittanceButNotInExpense"] == null ? false : bool.Parse(ViewState["isRelateWithRemittanceButNotInExpense"].ToString());
            }
        }
        #endregion

        protected void ctlAdvanceLookupPopupCaller_NotifyPopupResult(object sender, string action, string value)
        {
            if (action != "ok") return;

            object returnValue = new object();
            if (!isMultiple)
            {
                IList<Advance> advances = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindByAdvanceID(UIHelper.ParseLong(value));
                returnValue = advances;
            }
            else
            {
                string[] listID = value.Split('|');
                IList<Advance> list = new List<Advance>();
                foreach (string id in listID)
                {
                    IList<Advance> advances = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindByAdvanceID(UIHelper.ParseLong(id));
                    if (advances != null && advances.Count >0)
                        list.Add(advances[0]);
                }
                returnValue = list;
            }
            // NotifyPopUpReturn(new PopUpReturnArgs(PopUpReturnType.OK, returnValue));
            CallOnObjectLookUpReturn(returnValue);

        }


        public void Show()
        {
            CallOnObjectLookUpCalling();
            string popupURL = "~/UserControls/LOV/AV/AdvanceLookUp.aspx?isMultiple={0}&CompanyID={1}&AdvanceType={2}&RequesterID={3}&IsRelateWithRemittanceButNotInExpense={4}&CurrentUser={5}&PBID={6}&MainCurrencyID={7}&IsRepOffice={8}";
            ctlAdvanceLookupPopupCaller.URL = string.Format(popupURL, new object[] { isMultiple, CompanyID, AdvanceType, RequesterID, IsRelateWithRemittanceButNotInExpense, CurrentUserID,PBID,MainCurrencyID,IsRepOffice });
            ctlAdvanceLookupPopupCaller.ReferenceValue = isMultiple.ToString();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), ctlAdvanceLookupPopupCaller.ClientID + "_popup()", ctlAdvanceLookupPopupCaller.ClientID + "_popup('" + ctlAdvanceLookupPopupCaller.ProcessedURL + "')", true);
        }

        public void Hide()
        {
        }




    }
}
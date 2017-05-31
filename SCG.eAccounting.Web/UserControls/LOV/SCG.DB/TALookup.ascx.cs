using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.DB.BLL;
using SCG.DB.Query;
using SCG.DB.DTO.ValueObject;
using SCG.eAccounting.Web.Helper;
using SCG.DB.DTO;
using System.Text;
using SCG.eAccounting.Query;
using SCG.eAccounting.Query.Hibernate;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class TALookup : BaseUserControl
    {

        public bool isMultiple { get; set; }
        public string CompanyID { get; set; }
        public string RequesterID { get; set; }
        public string TravelBy { get; set; }
        public bool isQueryForAdvance { get; set; }
        public bool isQueryForRemittance { get; set; }
        public bool isQueryForExpense { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            string popupURL = "~/UserControls/LOV/SCG.DB/TALookup.aspx?isMultiple={0}&CompanyID={1}&RequesterID={2}&TravelBy={3}&isQueryForAdvance={4}&isQueryForRemittance={5}&isQueryForExpense={6}";
            ctlTALookupPopupCaller.URL = string.Format(popupURL, new object[] { isMultiple, CompanyID, RequesterID, TravelBy, isQueryForAdvance, isQueryForRemittance, isQueryForExpense });
        }

        public void Show()
        {
            CallOnObjectLookUpCalling();
            string popupURL = "~/UserControls/LOV/SCG.DB/TALookup.aspx?isMultiple={0}&CompanyID={1}&RequesterID={2}&TravelBy={3}&isQueryForAdvance={4}&isQueryForRemittance={5}&isQueryForExpense={6}";
            ctlTALookupPopupCaller.URL = string.Format(popupURL, new object[] { isMultiple, CompanyID, RequesterID, TravelBy, isQueryForAdvance, isQueryForRemittance, isQueryForExpense });
            ctlTALookupPopupCaller.ReferenceValue = isMultiple.ToString();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), ctlTALookupPopupCaller.ClientID + "_popup()", ctlTALookupPopupCaller.ClientID + "_popup('" + ctlTALookupPopupCaller.ProcessedURL + "')", true);
        }

        protected void ctlTALookupPopupCaller_NotifyPopupResult(object sender, string action, string value)
        {
            if (action != "ok") return;

            object returnValue = new object();

            if (!isMultiple)
            {
                IList<TADocumentObj> taDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.FindByDocumentIdentity(UIHelper.ParseLong(value));
                returnValue = taDocument[0];
                CallOnObjectLookUpReturn(returnValue);
            }
            else
            {
                string[] listID = value.Split('|');
                IList<TADocument> list = new List<TADocument>();
                foreach (string id in listID)
                {
                    TADocument taDocument = ScgeAccountingQueryProvider.TADocumentQuery.FindByIdentity(UIHelper.ParseLong(id));
                    if (taDocument != null)
                        list.Add(taDocument);
                }
                returnValue = list;
                CallOnObjectLookUpReturn(returnValue);
            }
        }

    }
}
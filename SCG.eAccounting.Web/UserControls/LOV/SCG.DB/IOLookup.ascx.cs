using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCG.DB.Query;
using SCG.DB.DTO;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using System.Text;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class IOLookup : BaseUserControl
    {

        public bool isMultiple
        {
            get
            {
                if (ViewState["isMultiple"] != null)
                    return (bool)ViewState["isMultiple"];
                return false;
            }
            set { ViewState["isMultiple"] = value; }
        }
        public long? CompanyId
        {
            get
            {
                if (string.IsNullOrEmpty(ctlCompanyId.Text))
                    return null;
                return UIHelper.ParseLong(ctlCompanyId.Text);
            }
            set
            {
                if (value.HasValue)
                    ctlCompanyId.Text = value.Value.ToString();
                else
                    ctlCompanyId.Text = string.Empty;
            }
        }
        public long? CostCenterId
        {
            get
            {
                if (string.IsNullOrEmpty(ctlCostCenterId.Text))
                    return null;
                return UIHelper.ParseLong(ctlCostCenterId.Text);
            }
            set
            {
                if (value.HasValue)
                    ctlCostCenterId.Text = value.Value.ToString();
                else
                    ctlCostCenterId.Text = string.Empty;
            }
        }
        public string CompanyCode
        {
            get { return ctlCompanyCode.Text; }
            set { ctlCompanyCode.Text = value; }
        }
        public string CostCenterCode
        {
            get { return ctlCostCenterCode.Text; }
            set { ctlCostCenterCode.Text = value; }
        }
        public string IONumber
        {
            get { return ctlIONumber.Text; }
            set { ctlIONumber.Text = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void ctlIOLookupPopupCaller_NotifyPopupResult(object sender, string action, string value)
        {
            if (action != "ok") return;

            object returnValue = new object();

            if (!isMultiple)
            {
                DbInternalOrder ioOrder = ScgDbQueryProvider.DbIOQuery.FindByIdentity(UIHelper.ParseLong(value));
                returnValue = ioOrder;
                NotifyPopUpReturn(new PopUpReturnArgs(PopUpReturnType.OK, returnValue));
            }
            else
            {
                string[] listID = value.Split('|');
                IList<DbInternalOrder> list = new List<DbInternalOrder>();
                foreach (string id in listID)
                {
                    DbInternalOrder ioOrder = ScgDbQueryProvider.DbIOQuery.FindByIdentity(UIHelper.ParseLong(id));
                    if (ioOrder != null)
                        list.Add(ioOrder);
                }
                returnValue = list;
                CallOnObjectLookUpReturn(returnValue);
            }
        }
        public void Show()
        {
            CallOnObjectLookUpCalling();
            string popupURL = "~/UserControls/LOV/SCG.DB/IOLookup.aspx?isMultiple={0}&CompanyId={1}&CostCenterId={2}&CompanyCode={3}&CostCenterCode={4}&IONumber={5}";
            ctlIOLookupPopupCaller.URL = string.Format(popupURL, new object[] {  isMultiple, CompanyId, CostCenterId, CompanyCode, CostCenterCode, IONumber });
            ctlIOLookupPopupCaller.ReferenceValue = isMultiple.ToString();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), ctlIOLookupPopupCaller.ClientID + "_popup()", ctlIOLookupPopupCaller.ClientID + "_popup('" + ctlIOLookupPopupCaller.ProcessedURL + "')", true);
        }

        public void Hide()
        {
        }
    }
}
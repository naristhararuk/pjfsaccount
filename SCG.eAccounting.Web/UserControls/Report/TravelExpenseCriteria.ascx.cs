using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.DTO.ValueObject;
using SS.Standard.UI;
using SS.SU.DTO;
using SS.SU.Query;

namespace SCG.eAccounting.Web.UserControls.Report
{
    public partial class TravelExpenseCriteria : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        public VoTravelExpenseReport BindCriteria()
        {
            VoTravelExpenseReport vo = new VoTravelExpenseReport();
            vo.CompanyID = UIHelper.ParseLong(ctlCompanyField.CompanyID);
            vo.FromDate = UIHelper.ParseDate(ctlFromDate.DateValue);
            vo.ToDate = UIHelper.ParseDate(ctlToDate.DateValue);
            vo.FromTravelDate = UIHelper.ParseDate(ctlFromTravelDate.DateValue);
            vo.ToTravelDate = UIHelper.ParseDate(ctlToTravelDate.DateValue);
            vo.FromTaDocumentNo = ctlFromTaNo.TaDocumentNo;
            vo.ToTaDocumentNo = ctlToTaNo.TaDocumentNo;
            SuUser travellerFrom = QueryProvider.SuUserQuery.FindByIdentity(UIHelper.ParseLong(ctlFromTraveller.EmployeeID));
            SuUser travellerTo = QueryProvider.SuUserQuery.FindByIdentity(UIHelper.ParseLong(ctlToTraveller.EmployeeID));
            vo.FromTraveller = travellerFrom == null ? string.Empty : travellerFrom.EmployeeCode;
            vo.ToTraveller = travellerTo == null? string.Empty : travellerTo.EmployeeCode;
            vo.TaStatus = ctlTAStatus.SelectedValue;
            vo.ShowParam1 = string.Format("Company : {0}, TA Date : {1} - {2}, Travel Date : {3} - {4}, TA No. : {5} - {6}", new object[] { ctlCompanyField.CompanyCode,ctlFromDate.DateValue,ctlToDate.DateValue,ctlFromTravelDate.DateValue,ctlToTravelDate.DateValue,ctlFromTaNo.TaDocumentNo,ctlToTaNo.TaDocumentNo});
            vo.ShowParam2 = string.Format("Traveller : {0} - {1}, TA Status : {2}", new object[] { vo.FromTraveller, vo.ToTraveller, ctlTAStatus.SelectedItem.Text });

            return vo;
        }
    }
}
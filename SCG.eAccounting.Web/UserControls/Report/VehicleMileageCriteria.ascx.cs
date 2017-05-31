using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

using SCG.eAccounting.Web.Helper;
using SS.Standard.UI;
using SCG.eAccounting.DTO.ValueObject;
using SS.SU.Query;
using SS.SU.DTO;

namespace SCG.eAccounting.Web.UserControls.Report
{
    public partial class VehicleMileageCriteria : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        public VoVehicleMileageReport BindCriteria()
        {
            VoVehicleMileageReport vo = new VoVehicleMileageReport();
            vo.CompanyID = UIHelper.ParseLong(ctlCompanyField.CompanyID);
            vo.FromCarRegist = ctlFromCarRegis.Text;
            vo.ToCarRegist = ctlToCarRegis.Text;
            vo.FromTaDocumentNo = ctlFromTaNo.TaDocumentNo;
            vo.ToTaDocumentNO = ctlToTaNo.TaDocumentNo;
            vo.DocumentStatus = ctlDocumentStatus.SelectedValue;
            string empFrom=string.Empty;
            string empTo = string.Empty;
            SuUser suUserFrom = QueryProvider.SuUserQuery.FindByIdentity(UIHelper.ParseLong(ctlFromRequesterID.EmployeeID));
            SuUser suUserTo = QueryProvider.SuUserQuery.FindByIdentity(UIHelper.ParseLong(ctlToRequesterID.EmployeeID));
            if(suUserFrom != null)
                empFrom = suUserFrom.EmployeeCode;
            if(suUserTo != null)
                empTo = suUserTo.EmployeeCode;
            vo.FromRequesterID = empFrom;
            vo.ToRequesterID = empTo;
            string paramList = "Company : "+ctlCompanyField.CompanyCode+ ", Employee ID : "+empFrom+" - "+empTo+", Car Registration : "+ctlFromCarRegis.Text+" - "+ctlToCarRegis.Text;
            string paramList2 = "Document No. : " + ctlFromTaNo.TaDocumentNo + " - " + ctlToTaNo.TaDocumentNo + " , Document Status : " + ctlDocumentStatus.Text;
            vo.ParameterList = paramList;
            vo.ParameterList2 = paramList2;
            return vo;
        }
    }
}
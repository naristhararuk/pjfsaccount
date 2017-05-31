using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using SS.Standard.UI;
using SS.Standard.Security;

using SCG.DB.DTO;
using SCG.DB.BLL;
using SCG.DB.DTO.ValueObject;
using SCG.DB.Query;

using SCG.eAccounting.Web.Helper;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
	public partial class CostCenterLookUp : BaseUserControl
	{
		#region Properties
		public bool isMultiple { get; set; }
		public long? CompanyId 
		{
			get 
			{
				if (string.IsNullOrEmpty(ctlCompanyId.Value))
					return null;
				else return UIHelper.ParseLong(ctlCompanyId.Value);
			}
			set 
			{
				if (value.HasValue)
					ctlCompanyId.Value = value.ToString();
				else
					ctlCompanyId.Value = null;
			}
		}
		#endregion

		#region Page_Load Event
		protected void Page_Load(object sender, EventArgs e)
		{

		}
		#endregion

		#region Public Method
		public void Show()
		{
			CallOnObjectLookUpCalling();
			string popupURL = "~/UserControls/LOV/SCG.DB/CostCenterPopup.aspx?isMultiple={0}&companyId={1}";
			ctlCostCenterPopupCaller.URL = string.Format(popupURL, this.isMultiple, this.CompanyId);
			ctlCostCenterPopupCaller.ReferenceValue = isMultiple.ToString();
			ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), ctlCostCenterPopupCaller.ClientID + "_popup()", ctlCostCenterPopupCaller.ClientID + "_popup('" + ctlCostCenterPopupCaller.ProcessedURL + "')", true);
		}
		public void Hide()
		{

		}
		#endregion

		#region PopupCaller Event
		protected void ctlCostCenterPopupCaller_NotifyPopupResult(object sender, string action, string value)
		{
			if (action != "ok") return;

			object returnValue = new object();

			if (!isMultiple)
			{
				//DbCostCenter costCenter = ScgDbQueryProvider.DbCostCenterQuery.FindProxyByIdentity(UIHelper.ParseLong(value));
				//returnValue = costCenter;
				DbCostCenter costCenter = ScgDbQueryProvider.DbCostCenterQuery.FindByIdentity(UIHelper.ParseLong(value));
				IList<DbCostCenter> list = new List<DbCostCenter>();
				list.Add(costCenter);
				returnValue = list;
				NotifyPopUpReturn(new PopUpReturnArgs(PopUpReturnType.OK, returnValue));
			}
			else
			{
				string[] strCostCenterIDList = value.Split('|');
				IList<long> costCenterIDList = new List<long>();
				foreach (string id in strCostCenterIDList)
				{
					costCenterIDList.Add(UIHelper.ParseLong(id));
				}

				if (costCenterIDList.Count > 0)
					returnValue = ScgDbQueryProvider.DbCostCenterQuery.FindByCostCenterIDList(costCenterIDList);

				NotifyPopUpReturn(new PopUpReturnArgs(PopUpReturnType.OK, returnValue));
				//CallOnObjectLookUpReturn(returnValue);
			}
		}
		#endregion
	}
}
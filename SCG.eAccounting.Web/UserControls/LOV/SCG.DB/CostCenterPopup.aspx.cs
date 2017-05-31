using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using SS.Standard.UI;

using SCG.eAccounting.BLL;
using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.Web.UserControls.DocumentEditor;

using SCG.DB.DTO;
using SCG.DB.Query;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
	public partial class CostCenterPopup : BasePage
	{
		#region Properties
		public bool isMultiple { get; set; }
		public string CostCenterCode
		{
			get { return ctlCostCenterCode.Text; }
			set { ctlCostCenterCode.Text = value; }
		}
		public long? CompanyId
		{
			get
			{
				if (string.IsNullOrEmpty(ctlCompanyID.Value))
					return null;
				return UIHelper.ParseLong(ctlCompanyID.Value);
			}
			set
			{
				if (value.HasValue)
					ctlCompanyID.Value = value.Value.ToString();
				else
					ctlCompanyID.Value = string.Empty;
			}
		}
		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				this.isMultiple = bool.Parse(Request["isMultiple"]);
				ctlCompanyID.Value = Request["companyId"];

				this.Show();
			}
		}

		#region GridView Event
		public Object RequestData(int startRow, int pageSize, string sortExpression)
		{
			IList<DbCostCenter> list = ScgDbQueryProvider.DbCostCenterQuery.GetCostCenterList(ctlCostCenterCode.Text.Trim(), ctlDescription.Text.Trim(), UIHelper.ParseLong(ctlCompanyID.Value), startRow, pageSize, sortExpression);
			return list;
		}
		public int RequestCount()
		{
			int count = ScgDbQueryProvider.DbCostCenterQuery.CountByCostCenterCriteria(ctlCostCenterCode.Text.Trim(), ctlDescription.Text.Trim(), UIHelper.ParseLong(ctlCompanyID.Value));
			return count;
		}
		protected void ctlCostCenterGrid_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			if (!isMultiple && e.CommandName.Equals("Select"))
			{
				int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
				long costCenterID = UIHelper.ParseLong(ctlCostCenterGrid.DataKeys[rowIndex].Value.ToString());
				CallOnObjectLookUpReturn(costCenterID.ToString());
				//IList<DbCostCenter> dbCostCenter = ScgDbQueryProvider.DbCostCenterQuery.FindByDbCostCenter(CostCenterID, UIHelper.ParseLong(ctlCompanyID.Value));
				//NotifyPopUpReturn(new PopUpReturnArgs(PopUpReturnType.OK, dbCostCenter));
			}
		}
		protected void ctlCostCenterGrid_DataBound(object sender, EventArgs e)
		{
			if (isMultiple && ctlCostCenterGrid.Rows.Count > 0)
			{
				this.RegisterScriptForGridView();
			}
		}
		#endregion

		public void Show()
		{
			if (!isMultiple)
			{
				ctlCostCenterGrid.Columns[0].Visible = false;
				ctlCostCenterGrid.Columns[1].Visible = true;
				ctlSubmit.Visible = false;
				ctlLblLine.Visible = false;
			}
			else
			{
				ctlCostCenterGrid.Columns[0].Visible = true;
				ctlCostCenterGrid.Columns[1].Visible = false;
				ctlSubmit.Visible = true;
				ctlLblLine.Visible = true;
			}

			ctlCostCenterGrid.DataCountAndBind();
			this.UpdatePanelSearchCostCenter.Update();
			this.UpdatePanelGridView.Update();
		}
		public void Hide()
		{
			ctlCostCenterCode.Text = string.Empty;
			ctlDescription.Text = string.Empty;
			UpdatePanelSearchCostCenter.Update();
		}

		protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
		{
			Hide();
			ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), this.ClientID + "Cancel", "notifyPopupResult('cancel')", true);
		}
		protected void ctlSubmit_Click(object sender, EventArgs e)
		{
			//IList<DbCostCenter> list = new List<DbCostCenter>();
			StringBuilder costCenterIdList = new StringBuilder();
			foreach (GridViewRow row in ctlCostCenterGrid.Rows)
			{
				if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
				{
					long id = UIHelper.ParseLong(ctlCostCenterGrid.DataKeys[row.RowIndex].Values["CostCenterID"].ToString());
					costCenterIdList.AppendFormat("'{0}'|", id.ToString());
					//DbCostCenter dbCostCenter = ScgDbQueryProvider.DbCostCenterQuery.FindByIdentity(id);
					//list.Add(dbCostCenter);
				}
			}
			string costCenterIds = costCenterIdList.ToString();
			CallOnObjectLookUpReturn(costCenterIds.Remove(costCenterIds.LastIndexOf('|')));
		}
		protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
		{
			ctlCostCenterGrid.DataCountAndBind();
			this.UpdatePanelGridView.Update();
		}

		#region Private Method
		private void RegisterScriptForGridView()
		{
			StringBuilder script = new StringBuilder();
			script.Append("function validateCheckBox(objChk, objFlag) ");
			script.Append("{ ");
			script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
			script.Append("'" + ctlCostCenterGrid.ClientID + "', '" + ctlCostCenterGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
			script.Append("} ");

			ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "validateChkBox", script.ToString(), true);
		}
		private void CallOnObjectLookUpReturn(string returnValue)
		{
			Hide();
			ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), this.ClientID + "notifyPopupResult()", string.Format("notifyPopupResult('ok' , '{0}')", returnValue), true);
		}
		#endregion
	}
}

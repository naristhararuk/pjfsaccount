using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using SS.Standard.UI;

using SS.SU.BLL;
using SS.SU.DTO;
using SS.SU.Query;

using SCG.eAccounting.Web.Helper;

using SS.DB.Query;
using SS.DB.DTO;
using SS.DB.BLL;
using SS.Standard.Utilities;

namespace SCG.eAccounting.Web.Forms.SU.Programs
{
	public partial class Organization : BasePage
	{
		#region Property
		public ISuOrganizationService SuOrganizationService { get; set; }
		public ISuOrganizationLangService SuOrganizationLangService { get; set; }
		public IDbLanguageService DbLanguageService { get; set; }
		#endregion

		#region Page_Load Event
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			
			if (!Page.IsPostBack)
			{
				ctlOrganizationGrid.DataCountAndBind();
				ctlCancel.Visible = false;
				fdsDetailGridView.Visible = false;
			}
		}
		protected void Page_Load(object sender, EventArgs e)
		{
			
		}
		#endregion

		#region Button Event
		protected void ctlAddNew_Click(object sender, ImageClickEventArgs e)
		{
			ctlModalPopupExtender1.Show();
			ctlUpdatePanelOrganizationForm.Update();
            ctlOrganizationGrid.DataCountAndBind();
			ctlOrganizationForm.ChangeMode(FormViewMode.Insert);
		}
		protected void ctlDelete_Click(object sender, ImageClickEventArgs e)
		{
			foreach (GridViewRow row in ctlOrganizationGrid.Rows)
			{
				if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
				{
					try
					{
						short organizationId = UIHelper.ParseShort(ctlOrganizationGrid.DataKeys[row.RowIndex].Value.ToString());
						SuOrganization organization = SuOrganizationService.FindProxyByIdentity(organizationId);
						SuOrganizationService.Delete(organization);
					}
					catch (Exception ex)
					{
						string exMessage = ex.Message;
						Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
						errors.AddError("DeleteOrganization.Error", new Spring.Validation.ErrorMessage("CannotDelete"));
						ValidationErrors.MergeErrors(errors);
					}
				}
			}

			ctlOrganizationLangGrid.DataSource = null;
			ctlOrganizationLangGrid.DataBind();
			ctlCancel.Visible = false;
			fdsDetailGridView.Visible = false;
			ctlUpddatePanelOrganizationLangGrid.Update();
			
			ctlOrganizationGrid.SelectedIndex = -1;
			ctlOrganizationGrid.DataCountAndBind();
			ctlUpdatePanelOrganizationGrid.Update();
		}
		protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
		{
			ctlOrganizationLangGrid.DataSource = null;
			ctlOrganizationLangGrid.DataBind();
			ctlUpddatePanelOrganizationLangGrid.Update();
			fdsDetailGridView.Visible = false;

			ctlOrganizationGrid.SelectedIndex = -1;
			ctlOrganizationGrid.DataCountAndBind();
			ctlUpdatePanelOrganizationGrid.Update();
		}
		#endregion
		
		#region GridView Event
		#region ctlOrganizationGrid
		protected int ctlOrganizationGrid_RequestCount()
		{
			short languageId = UserAccount.CurrentLanguageID;
			return QueryProvider.SuOrganizationQuery.GetCountOrganizationList(languageId);
		}
		protected object ctlOrganizationGrid_RequestData(int startRow, int pageSize, string sortExpression)
		{
			short languageId = UserAccount.CurrentLanguageID;
			return QueryProvider.SuOrganizationQuery.GetOrganizationList(languageId, startRow, pageSize, sortExpression);
		}
		protected void ctlOrganizationGrid_DataBound(object sender, EventArgs e)
		{
			if (ctlOrganizationGrid.Rows.Count > 0)
			{
				RegisterScriptForGridView();
			}

			divButton.Visible = true;
			ctlDelete.OnClientClick = String.Format("return ConfirmDelete('{0}');", ctlOrganizationGrid.ClientID);
		}
		protected void ctlOrganizationGrid_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			
			if (e.CommandName == "EditOrganization")
			{
				int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
				short organizationId = UIHelper.ParseShort(ctlOrganizationGrid.DataKeys[rowIndex].Value.ToString());
			
				ctlOrganizationGrid.EditIndex = rowIndex;
			
				IList<SuOrganization> list = new List<SuOrganization>();
				list.Add(SuOrganizationService.FindProxyByIdentity(organizationId));

				ctlOrganizationForm.DataSource = list;
				ctlOrganizationForm.ChangeMode(FormViewMode.Edit);
                ctlOrganizationGrid.DataCountAndBind();

				ctlOrganizationForm.DataBind();
				ctlUpdatePanelOrganizationForm.Update();
				ctlModalPopupExtender1.Show();
			}
			if (e.CommandName == "Select")
			{
				int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
				short organizationId = UIHelper.ParseShort(ctlOrganizationGrid.DataKeys[rowIndex].Value.ToString());

                ctlOrganizationGrid.DataCountAndBind();
				BindOrganizationLangGrid(organizationId);
				ctlUpddatePanelOrganizationLangGrid.Update();
				ctlCancel.Visible = true;
			}
		}
		#endregion

		#region ctlOrganizationLangGrid
		protected void ctlOrganizationLangGrid_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			if (e.CommandName == "Select")
			{
				int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
				short organizationId = UIHelper.ParseShort(ctlOrganizationGrid.SelectedDataKey.Value.ToString());
				short languageId = UIHelper.ParseShort(ctlOrganizationLangGrid.DataKeys[rowIndex]["LanguageId"].ToString());

				IList<SuOrganizationLang> organizationLangList = QueryProvider.SuOrganizationLangQuery.FindByOrganizationAndLanguage(organizationId, languageId);

				if (organizationLangList.Count > 0)
				{
					ctlOrganizationLangForm.DataSource = organizationLangList;
					ctlOrganizationLangForm.ChangeMode(FormViewMode.Edit);
				}
				else
				{
					ctlOrganizationLangForm.DataSource = organizationLangList;
					ctlOrganizationLangForm.ChangeMode(FormViewMode.Insert);
				}

				ctlUpddatePanelOrganizationLangForm.Update();
				ctlOrganizationLangForm.DataBind();
				ctlModalPopupExtender2.Show();
			}
		}
		#endregion
		#endregion

		#region FormView Event
		#region ctlOrganizationForm Event
		protected void ctlOrganizationForm_ItemCommand(object sender, FormViewCommandEventArgs e)
		{
			if (e.CommandName == "Cancel")
			{
                ctlOrganizationGrid.DataCountAndBind();
				ClosePopUpOrganizationForm();
			}
		}
		protected void ctlOrganizationForm_ItemInserting(object sender, FormViewInsertEventArgs e)
		{
			SuOrganization organization = new SuOrganization();
			GetSuOrganizationInfo(organization);
			
			SuOrganizationLang organizationLang = new SuOrganizationLang();
			TextBox ctlTxtOrgName = ctlOrganizationForm.FindControl("ctlTxtOrgName") as TextBox;
			short languageId = UserAccount.CurrentLanguageID;
			organizationLang.Language = DbLanguageService.FindByIdentity(languageId);
			organizationLang.Organization = organization;
			organizationLang.OrganizationName = ctlTxtOrgName.Text;
			organizationLang.Active = organization.Active;
            organizationLang.CreBy = UserAccount.UserID;
			organizationLang.CreDate =DateTime.Now;
			organizationLang.UpdPgm = ProgramCode;
            organizationLang.UpdBy = UserAccount.UserID;
			organizationLang.UpdDate = DateTime.Now;

			try
			{
				SuOrganizationService.AddOrganization(organization, organizationLang);
			}
			catch (ServiceValidationException ex)
			{
				ValidationErrors.MergeErrors(ex.ValidationErrors);
			}

			// Check if no validation error then changemode to default mode.
			if (ValidationErrors.IsEmpty)
			{
				ctlOrganizationForm.ChangeMode(FormViewMode.ReadOnly);
				ClosePopUpOrganizationForm();
				ctlOrganizationGrid.DataCountAndBind();
			}
		}
		protected void ctlOrganizationForm_ItemUpdating(object sender, FormViewUpdateEventArgs e)
		{
			short organizationId = UIHelper.ParseShort(ctlOrganizationForm.DataKey.Value.ToString());
			SuOrganization organization = SuOrganizationService.FindProxyByIdentity(organizationId);
			GetSuOrganizationInfo(organization);

			try
			{
				SuOrganizationService.UpdateOrganization(organization);
			}
			catch (ServiceValidationException ex)
			{
				ValidationErrors.MergeErrors(ex.ValidationErrors);
			}

			// Check if no validation error then changemode to default mode.
			if (ValidationErrors.IsEmpty)
			{
				ctlOrganizationForm.ChangeMode(FormViewMode.ReadOnly);
				ClosePopUpOrganizationForm();
				ctlOrganizationGrid.DataCountAndBind();
			}
		}
		protected void ctlOrganizationForm_ModeChanging(object sender, FormViewModeEventArgs e)
		{

		}
		protected void ctlOrganizationForm_DataBound(object sender, EventArgs e)
		{
			if (ctlOrganizationForm.CurrentMode == FormViewMode.Edit)
			{
				Label ctlTxtOrgName = ctlOrganizationForm.FindControl("ctlTxtOrgName") as Label;
				LinkButton ctlOrganizationName = ctlOrganizationGrid.Rows[ctlOrganizationGrid.EditIndex].FindControl("ctlOrganizationName") as LinkButton;
				
				ctlTxtOrgName.Text = ctlOrganizationName.Text;

				if (ValidationErrors.IsEmpty)
				{
					TextBox ctlTxtComment = ctlOrganizationForm.FindControl("ctlTxtComment") as TextBox;
					ctlTxtComment.Focus();
				}
			}

			if (ctlOrganizationForm.CurrentMode == FormViewMode.Insert)
			{
				if (ValidationErrors.IsEmpty)
				{
					TextBox ctlTxtOrgName = ctlOrganizationForm.FindControl("ctlTxtOrgName") as TextBox;
					ctlTxtOrgName.Focus();
				}
			}
		}
		#endregion

		#region ctlOrganizationLangForm Event
		protected void ctlOrganizationLangForm_ItemCommand(object sender, FormViewCommandEventArgs e)
		{
			if (e.CommandName == "Cancel")
			{
				short organizationId = UIHelper.ParseShort(ctlOrganizationGrid.SelectedDataKey.Value.ToString());

				ClosePopUpOrganizationLangForm();
				BindOrganizationLangGrid(organizationId);
				ctlOrganizationLangGrid.SelectedIndex = -1;
			}
		}
		protected void ctlOrganizationLangForm_ItemInserting(object sender, FormViewInsertEventArgs e)
		{
			short organizationId = UIHelper.ParseShort(ctlOrganizationGrid.SelectedDataKey.Value.ToString());
			SuOrganizationLang organizationLang = new SuOrganizationLang();
			organizationLang.Organization = SuOrganizationService.FindProxyByIdentity(organizationId);
			organizationLang.Language = DbLanguageService.FindProxyByIdentity(UIHelper.ParseShort(ctlOrganizationLangGrid.SelectedDataKey["LanguageId"].ToString()));

			GetSuOrganizationLangInfo(organizationLang);

			try
			{
				SuOrganizationLangService.InsertOrganizationLang(organizationLang);
			}
			catch (ServiceValidationException ex)
			{
				ValidationErrors.MergeErrors(ex.ValidationErrors);
			}

			// Check if no validation error then changemode to default mode.
			if (ValidationErrors.IsEmpty)
			{
				ctlOrganizationLangForm.ChangeMode(FormViewMode.ReadOnly);
				ClosePopUpOrganizationLangForm();
				BindOrganizationLangGrid(organizationId);

				ctlOrganizationGrid.DataCountAndBind();
				ctlUpdatePanelOrganizationGrid.Update();
			}
		}
		protected void ctlOrganizationLangForm_ItemUpdating(object sender, FormViewUpdateEventArgs e)
		{
			short organizationId;
			long organizationLangId = UIHelper.ParseLong(ctlOrganizationLangForm.DataKey["Id"].ToString());
			SuOrganizationLang organizationLang = SuOrganizationLangService.FindProxyByIdentity(organizationLangId);
			organizationId = organizationLang.Organization.Organizationid;

			GetSuOrganizationLangInfo(organizationLang);

			try
			{
				SuOrganizationLangService.UpdateOrganizationLang(organizationLang);
			}
			catch (ServiceValidationException ex)
			{
				ValidationErrors.MergeErrors(ex.ValidationErrors);
			}

			// Check if no validation error then changemode to default mode.
			if (ValidationErrors.IsEmpty)
			{
				ctlOrganizationForm.ChangeMode(FormViewMode.ReadOnly);
				ClosePopUpOrganizationLangForm();
				BindOrganizationLangGrid(organizationId);

				ctlOrganizationGrid.DataCountAndBind();
				ctlUpdatePanelOrganizationGrid.Update();
			}
		}
		protected void ctlOrganizationLangForm_ModeChanging(object sender, FormViewModeEventArgs e)
		{

		}
		protected void ctlOrganizationLangForm_DataBound(object sender, EventArgs e)
		{
			if (ctlOrganizationLangForm.CurrentMode != FormViewMode.ReadOnly)
			{
				RegularExpressionValidator revCtlTxtEmail = ctlOrganizationLangForm.FindControl("revCtlTxtEmail") as RegularExpressionValidator;
				revCtlTxtEmail.ErrorMessage = "Incorrect format.";

				if (ValidationErrors.IsEmpty)
				{
					TextBox ctlTxtOrgName = ctlOrganizationLangForm.FindControl("ctlTxtOrgName") as TextBox;
					ctlTxtOrgName.Focus();
				}
			}
		}
		#endregion
		#endregion

		#region Private Method
		private void ClosePopUpOrganizationForm()
		{
			ctlModalPopupExtender1.Hide();
			ctlUpdatePanelOrganizationGrid.Update();
		}
		private void ClosePopUpOrganizationLangForm()
		{
			ctlModalPopupExtender2.Hide();
			ctlUpddatePanelOrganizationLangGrid.Update();
		}
		private void BindOrganizationLangGrid(short organizationId)
		{
			ctlOrganizationLangGrid.SelectedIndex = -1;
			ctlOrganizationLangGrid.DataSource = QueryProvider.SuOrganizationLangQuery.FindSuOrganizationLangByOrganizationId(organizationId);
			ctlOrganizationLangGrid.DataBind();
			
			fdsDetailGridView.Visible = true;
		}
		private void GetSuOrganizationInfo(SuOrganization organization)
		{
			TextBox ctlTxtComment = ctlOrganizationForm.FindControl("ctlTxtComment") as TextBox;
			CheckBox ctlActive = ctlOrganizationForm.FindControl("ctlActive") as CheckBox;

			organization.Comment = ctlTxtComment.Text;
			organization.Active = ctlActive.Checked;
			organization.UpdPgm = ProgramCode;
            organization.CreBy = UserAccount.UserID;
			organization.CreDate = DateTime.Now;
            organization.UpdBy = UserAccount.UserID;
			organization.UpdDate = DateTime.Now;
		}
		private void GetSuOrganizationLangInfo(SuOrganizationLang organizationLang)
		{
			TextBox ctlTxtOrgName = ctlOrganizationLangForm.FindControl("ctlTxtOrgName") as TextBox;
			TextBox ctlTxtAddress = ctlOrganizationLangForm.FindControl("ctlTxtAddress") as TextBox;
			TextBox ctlTxtProvince = ctlOrganizationLangForm.FindControl("ctlTxtProvince") as TextBox;
			TextBox ctlTxtCountry = ctlOrganizationLangForm.FindControl("ctlTxtCountry") as TextBox;
			TextBox ctlTxtPostal = ctlOrganizationLangForm.FindControl("ctlTxtPostal") as TextBox;
			TextBox ctlTxtTelephone = ctlOrganizationLangForm.FindControl("ctlTxtTelephone") as TextBox;
			TextBox ctlTxtTelephoneExt = ctlOrganizationLangForm.FindControl("ctlTxtTelephoneExt") as TextBox;
			TextBox ctlTxtFax = ctlOrganizationLangForm.FindControl("ctlTxtFax") as TextBox;
			TextBox ctlTxtFaxExt = ctlOrganizationLangForm.FindControl("ctlTxtFaxExt") as TextBox;
			TextBox ctlTxtEmail = ctlOrganizationLangForm.FindControl("ctlTxtEmail") as TextBox;
			TextBox ctlTxtComment = ctlOrganizationLangForm.FindControl("ctlTxtComment") as TextBox;
			CheckBox ctlActive = ctlOrganizationLangForm.FindControl("ctlActive") as CheckBox;

			organizationLang.OrganizationName = ctlTxtOrgName.Text;
			organizationLang.Address = ctlTxtAddress.Text;
			organizationLang.Province = ctlTxtProvince.Text;
			organizationLang.Country = ctlTxtCountry.Text;
			organizationLang.Postal = ctlTxtPostal.Text;
			organizationLang.Organization.Telephone = ctlTxtTelephone.Text;
			organizationLang.Organization.TelephoneExt = ctlTxtTelephoneExt.Text;
			organizationLang.Organization.Fax = ctlTxtFax.Text;
			organizationLang.Organization.FaxExt = ctlTxtFaxExt.Text;
			organizationLang.Organization.Email = ctlTxtEmail.Text;
			organizationLang.Comment = ctlTxtComment.Text;
			organizationLang.Active = ctlActive.Checked;
			organizationLang.UpdPgm = ProgramCode;
            organizationLang.CreBy = UserAccount.UserID;
			organizationLang.CreDate = DateTime.Now;
            organizationLang.UpdBy = UserAccount.UserID;
			organizationLang.UpdDate = DateTime.Now;
			organizationLang.Organization.UpdPgm = ProgramCode;
            organizationLang.Organization.CreBy = UserAccount.UserID;
			organizationLang.Organization.CreDate = DateTime.Now;
            organizationLang.Organization.UpdBy = UserAccount.UserID;
			organizationLang.Organization.UpdDate = DateTime.Now;
		}
		private void RegisterScriptForGridView()
		{
			StringBuilder script = new StringBuilder();
			script.Append("function validateCheckBox(objChk, objFlag) ");
			script.Append("{ ");
			script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
			script.Append("'" + ctlOrganizationGrid.ClientID + "', '" + ctlOrganizationGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
			script.Append("} ");

			ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "validateChkBox", script.ToString(), true);
		}
		#endregion
	}
}

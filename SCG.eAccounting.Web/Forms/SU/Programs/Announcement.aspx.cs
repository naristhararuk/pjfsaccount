using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using FredCK.FCKeditorV2;
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
	public partial class Announcement : BasePage
	{
		#region Property
		public ISuAnnouncementService SuAnnouncementService { get; set; }
		public ISuAnnouncementLangService SuAnnouncementLangService { get; set; }
		public ISuAnnouncementGroupService SuAnnouncementGroupService { get; set; }
		public IDbLanguageService DbLanguageService { get; set; }
		#endregion

		#region Override Method
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			if (!Page.IsPostBack)
			{
				BindAnnouncementGroupDropDown();
				ctlCancel.Visible = false;
				ctlFieldSetDetailGridView.Visible = false;
			}
		}
		#endregion

		#region Page_Load Event
		protected void Page_Load(object sender, EventArgs e)
		{
			
		}
		#endregion
		
		#region Button Click Event
        protected void lnkSearch_Click(object sender, EventArgs e)
        {
            if (ctlDdlAnnouncementGroup.SelectedIndex != 0)
            {
                if (ctlDdlAnnouncementGroup.SelectedValue != "-1")
                {
                    ctlLblAnnouncementGroup.Text = ctlDdlAnnouncementGroup.SelectedItem.Text;
                    hdCtlLblAnnouncementGroupId.Value = ctlDdlAnnouncementGroup.SelectedValue;

                    ctlAnnouncementGrid.DataCountAndBind();
                }

                ctlAnnouncementLangGrid.DataSource = null;
                ctlAnnouncementLangGrid.DataBind();
                ctlCancel.Visible = false;
                ctlFieldSetDetailGridView.Visible = false;
                ctlUpddatePanelAnnouncementLangGrid.Update();
                divButton.Visible = true;

            }
        }
		protected void ctlDelete_Click(object sender, ImageClickEventArgs e)
		{
			foreach (GridViewRow row in ctlAnnouncementGrid.Rows)
			{
				if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
				{
					try
					{
						short announcementId = UIHelper.ParseShort(ctlAnnouncementGrid.DataKeys[row.RowIndex].Value.ToString());
						SuAnnouncement announcement = SuAnnouncementService.FindProxyByIdentity(announcementId);
						SuAnnouncementService.Delete(announcement);
					}
					catch (Exception ex)
					{
						string exMessage = ex.Message;
					}
				}
			}
			
			ctlAnnouncementLangGrid.DataSource = null;
			ctlAnnouncementLangGrid.DataBind();
			ctlCancel.Visible = false;
			ctlFieldSetDetailGridView.Visible = false;
			ctlUpddatePanelAnnouncementLangGrid.Update();
			
			ctlAnnouncementGrid.SelectedIndex = -1;
			ctlAnnouncementGrid.DataCountAndBind();
			ctlUpdatePanelAnnnouncementGrid.Update();
		}
		protected void ctlAddNew_Click(object sender, ImageClickEventArgs e)
		{
			ctlModalPopupExtender1.Show();
			ctlUpdatePanelAnnouncementForm.Update();
            ctlAnnouncementForm.ChangeMode(FormViewMode.Insert);
            ctlAnnouncementGrid.DataCountAndBind();
		}
		protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
		{
			ctlAnnouncementLangGrid.DataSource = null;
			ctlAnnouncementLangGrid.DataBind();
			ctlCancel.Visible = false;
			ctlFieldSetDetailGridView.Visible = false;
			ctlUpddatePanelAnnouncementLangGrid.Update();

			ctlAnnouncementGrid.SelectedIndex = -1;
			ctlAnnouncementGrid.DataCountAndBind();
			ctlUpdatePanelAnnnouncementGrid.Update();
		}
		#endregion

		#region DropdownList Event
		protected void ctlDdlAnnouncementGroup_DataBound(object sender, EventArgs e)
		{
			ctlDdlAnnouncementGroup.Items.Insert(0, new ListItem("", "-1"));
		}
		#endregion
		
		#region GridView Event
		#region ctlAnnouncementGrid Event
		protected void ctlAnnouncementGrid_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			if (e.CommandName == "EditAnnouncement")
			{
				int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
				short announcementId = UIHelper.ParseShort(ctlAnnouncementGrid.DataKeys[rowIndex].Value.ToString());

				ctlAnnouncementGrid.EditIndex = rowIndex;
				
				IList<SuAnnouncement> list = new List<SuAnnouncement>();
				list.Add(SuAnnouncementService.FindProxyByIdentity(announcementId));

				ctlAnnouncementForm.DataSource = list;
				ctlAnnouncementForm.ChangeMode(FormViewMode.Edit);
                ctlModalPopupExtender1.Show();
				ctlUpdatePanelAnnouncementForm.Update();
				ctlAnnouncementForm.DataBind();
                ctlAnnouncementGrid.DataCountAndBind();
				
			}
			if (e.CommandName == "Select")
			{
				int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
				short announcementId = UIHelper.ParseShort(ctlAnnouncementGrid.DataKeys[rowIndex].Value.ToString());
				
				ctlUpddatePanelAnnouncementLangGrid.Update();
				BindAnnouncementLangGrid(announcementId);
                ctlAnnouncementGrid.DataCountAndBind();
				//ctlSubmit.Visible = true;
			}
		}
		protected void ctlAnnouncementGrid_DataBound(object sender, EventArgs e)
		{
			if (ctlAnnouncementGrid.Rows.Count > 0)
			{
				RegisterScriptForGridView();
			}

			divButton.Visible = true;
			ctlDelete.OnClientClick = String.Format("return ConfirmDelete('{0}')", ctlAnnouncementGrid.ClientID);
		}
		protected object RequestData1(int startRow, int pageSize, string sortExpression)
		{
			short languageId = UserAccount.CurrentLanguageID;
			short announcementGroupId = UIHelper.ParseShort(hdCtlLblAnnouncementGroupId.Value);
				
			return QueryProvider.SuAnnouncementQuery.GetAnnouncementList(languageId, announcementGroupId, startRow, pageSize, sortExpression);
		}
		protected int ctlAnnouncementGrid_RequestCount()
		{
			short languageId = UserAccount.CurrentLanguageID;
			short announcementGroupId = UIHelper.ParseShort(hdCtlLblAnnouncementGroupId.Value);
			
			return QueryProvider.SuAnnouncementQuery.GetCountAnnouncement(languageId, announcementGroupId);
		}
		#endregion

		#region ctlAnnouncementLangGrid Event
		protected void ctlAnnouncementLangGrid_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			if (e.CommandName == "Select")
			{
				int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
				short announcementId = UIHelper.ParseShort(ctlAnnouncementGrid.SelectedDataKey.Value.ToString());
				short languageId = UIHelper.ParseShort(ctlAnnouncementLangGrid.DataKeys[rowIndex]["LanguageId"].ToString());
				
				IList<SuAnnouncementLang> announcementLangList = SuAnnouncementLangService.FindByAnnouncementAndLanguage(announcementId, languageId);

				if (announcementLangList.Count > 0)
				{
					ctlAnnouncementLangForm.DataSource = announcementLangList;
					ctlAnnouncementLangForm.ChangeMode(FormViewMode.Edit);
				}
				else
				{
					ctlAnnouncementLangForm.DataSource = announcementLangList;
					ctlAnnouncementLangForm.ChangeMode(FormViewMode.Insert);
				}

				ctlUpddatePanelAnnouncementLangForm.Update();
				ctlAnnouncementLangForm.DataBind();
				ctlModalPopupExtender2.Show();
			}
		}
		#endregion
		#endregion

		#region FormView Event
		#region ctlAnnouncementForm Event
		protected void ctlAnnouncementForm_ItemCommand(object sender, FormViewCommandEventArgs e)
		{
			if (e.CommandName == "Cancel")
			{
				ClosePopUpAnnouncementForm();
                ctlAnnouncementGrid.DataCountAndBind();
			}
		}
		protected void ctlAnnouncementForm_ItemInserting(object sender, FormViewInsertEventArgs e)
		{
			SuAnnouncement announcement = new SuAnnouncement();
			GetSuAnnouncementInfo(announcement);
			
			SuAnnouncementLang announcementLang = new SuAnnouncementLang();
			TextBox ctlTxtHeader = ctlAnnouncementForm.FindControl("ctlTxtHeader") as TextBox;
			short languageId = UserAccount.CurrentLanguageID;
			announcementLang.Language = DbLanguageService.FindByIdentity(languageId);
			announcementLang.Announcement = announcement;
			announcementLang.AnnouncementHeader = ctlTxtHeader.Text;
			announcementLang.Active = announcement.Active;
            announcementLang.CreBy = UserAccount.UserID;
			announcementLang.CreDate = DateTime.Now;
			announcementLang.UpdPgm = ProgramCode;
            announcementLang.UpdBy = UserAccount.UserID;
			announcementLang.UpdDate = DateTime.Now;

			try
			{
				SuAnnouncementService.AddAnnouncement(announcement, announcementLang);
			}
			catch (ServiceValidationException ex)
			{
				ValidationErrors.MergeErrors(ex.ValidationErrors);
			}

			// Check if no validation error then changemode to default mode.
			if (ValidationErrors.IsEmpty)
			{
				ctlAnnouncementForm.ChangeMode(FormViewMode.ReadOnly);
				ClosePopUpAnnouncementForm();
				ctlAnnouncementGrid.DataCountAndBind();
			}
		}
		protected void ctlAnnouncementForm_ItemUpdating(object sender, FormViewUpdateEventArgs e)
		{
			short announcementId = UIHelper.ParseShort(ctlAnnouncementForm.DataKey.Value.ToString());
			SuAnnouncement announcement = SuAnnouncementService.FindProxyByIdentity(announcementId);
			GetSuAnnouncementInfo(announcement);

			try
			{
				SuAnnouncementService.UpdateAnnouncement(announcement);
			}
			catch (ServiceValidationException ex)
			{
				ValidationErrors.MergeErrors(ex.ValidationErrors);
			}

			// Check if no validation error then changemode to default mode.
			if (ValidationErrors.IsEmpty)
			{
				ctlAnnouncementForm.ChangeMode(FormViewMode.ReadOnly);
				ClosePopUpAnnouncementForm();
				ctlAnnouncementGrid.DataCountAndBind();
			}
		}
		protected void ctlAnnouncementForm_ModeChanging(object sender, FormViewModeEventArgs e)
		{

		}
		protected void ctlAnnouncementForm_DataBound(object sender, EventArgs e)
		{
			if (ctlAnnouncementForm.CurrentMode == FormViewMode.Edit)
			{
				Label ctlTxtHeader = ctlAnnouncementForm.FindControl("ctlTxtHeader") as Label;
				LinkButton ctlAnnouncementHeader = ctlAnnouncementGrid.Rows[ctlAnnouncementGrid.EditIndex].FindControl("ctlAnnouncementHeader") as LinkButton;

				ctlTxtHeader.Text = ctlAnnouncementHeader.Text;

				if (ValidationErrors.IsEmpty)
				{
                    //modify by tom 28/01/2009
					//SCG.eAccounting.Web.UserControls.Calendar ctlCalEffectiveDate = ctlAnnouncementForm.FindControl("ctlCalEffectiveDate") as SCG.eAccounting.Web.UserControls.Calendar;
                    UserControls.Calendar ctlCalEffectiveDate = ctlAnnouncementForm.FindControl("ctlCalEffectiveDate") as UserControls.Calendar;
					ctlCalEffectiveDate.Focus();
				}
			}

			if (ctlAnnouncementForm.CurrentMode == FormViewMode.Insert)
			{
				if (ValidationErrors.IsEmpty)
				{
					TextBox ctlTxtHeader = ctlAnnouncementForm.FindControl("ctlTxtHeader") as TextBox;
					ctlTxtHeader.Focus();
				}
			}
		}
		#endregion

		#region ctlAnnouncementLangForm Event
		protected void ctlAnnouncementLangForm_ItemCommand(object sender, FormViewCommandEventArgs e)
		{
			if (e.CommandName == "Cancel")
			{
				short announcementId = UIHelper.ParseShort(ctlAnnouncementGrid.SelectedDataKey.Value.ToString());
				
				ClosePopUpAnnouncementLangForm();
				BindAnnouncementLangGrid(announcementId);
				ctlAnnouncementLangGrid.SelectedIndex = -1;
			}
		}
		protected void ctlAnnouncementLangForm_ItemInserting(object sender, FormViewInsertEventArgs e)
		{
			short announcementId = UIHelper.ParseShort(ctlAnnouncementGrid.SelectedDataKey.Value.ToString());
			SuAnnouncementLang announcementLang = new SuAnnouncementLang();
			announcementLang.Announcement = SuAnnouncementService.FindProxyByIdentity(announcementId);
			announcementLang.Language = DbLanguageService.FindProxyByIdentity(UIHelper.ParseShort(ctlAnnouncementLangGrid.SelectedDataKey["LanguageId"].ToString()));
			
			GetSuAnnouncementLangInfo(announcementLang);

			try
			{
				SuAnnouncementLangService.InsertAnnouncementLang(announcementLang);
			}
			catch (ServiceValidationException ex)
			{
				ValidationErrors.MergeErrors(ex.ValidationErrors);
			}

			// Check if no validation error then changemode to default mode.
			if (ValidationErrors.IsEmpty)
			{
				ctlAnnouncementLangForm.ChangeMode(FormViewMode.ReadOnly);
				ClosePopUpAnnouncementLangForm();
				BindAnnouncementLangGrid(announcementId);

				ctlAnnouncementGrid.DataCountAndBind();
				ctlUpdatePanelAnnnouncementGrid.Update();
			}
		}
		protected void ctlAnnouncementLangForm_ItemUpdating(object sender, FormViewUpdateEventArgs e)
		{
			short announcementId;
			long announcementLangId = UIHelper.ParseLong(ctlAnnouncementLangForm.DataKey["Id"].ToString());
			SuAnnouncementLang announcementLang = SuAnnouncementLangService.FindProxyByIdentity(announcementLangId);
			announcementId = announcementLang.Announcement.Announcementid;
			
			GetSuAnnouncementLangInfo(announcementLang);

			try
			{
				SuAnnouncementLangService.UpdateAnnouncementLang(announcementLang);
			}
			catch (ServiceValidationException ex)
			{
				ValidationErrors.MergeErrors(ex.ValidationErrors);
			}

			// Check if no validation error then changemode to default mode.
			if (ValidationErrors.IsEmpty)
			{
				ctlAnnouncementForm.ChangeMode(FormViewMode.ReadOnly);
				ClosePopUpAnnouncementLangForm();
				BindAnnouncementLangGrid(announcementId);

				ctlAnnouncementGrid.DataCountAndBind();
				ctlUpdatePanelAnnnouncementGrid.Update();
			}
		}
		protected void ctlAnnouncementLangForm_ModeChanging(object sender, FormViewModeEventArgs e)
		{
			
		}
		protected void ctlAnnouncementLangForm_DataBound(object sender, EventArgs e)
		{
			if (ctlAnnouncementLangForm.CurrentMode != FormViewMode.ReadOnly)
			{
				if (ValidationErrors.IsEmpty)
				{
					TextBox ctlTxtHeader = ctlAnnouncementLangForm.FindControl("ctlTxtHeader") as TextBox;
					ctlTxtHeader.Focus();
				}
			}
		}
		#endregion
		#endregion

		#region Private Method
		private void ClosePopUpAnnouncementForm()
		{
			ctlModalPopupExtender1.Hide();
			ctlUpdatePanelAnnnouncementGrid.Update();
            ctlAnnouncementForm.ChangeMode(FormViewMode.ReadOnly);
		}
		private void ClosePopUpAnnouncementLangForm()
		{
			ctlModalPopupExtender2.Hide();
			ctlUpddatePanelAnnouncementLangGrid.Update();
            ctlAnnouncementLangForm.ChangeMode(FormViewMode.ReadOnly);
		}
		private void BindAnnouncementGroupDropDown()
		{
			short languageId = UserAccount.CurrentLanguageID;
			ctlDdlAnnouncementGroup.DataSource = QueryProvider.SuAnnouncementGroupQuery.GetTranslatedList(languageId);
			ctlDdlAnnouncementGroup.DataBind();
		}
		private void BindAnnouncementLangGrid(short announcementId)
		{
			//short announcementId = UIHelper.ParseShort(ctlAnnouncementGrid.SelectedDataKey.Value.ToString());
			ctlAnnouncementLangGrid.SelectedIndex = -1;
			ctlAnnouncementLangGrid.DataSource = QueryProvider.SuAnnouncementLangQuery.FindAnnouncementLangByAnnouncementId(announcementId);
			ctlAnnouncementLangGrid.DataBind();
			ctlCancel.Visible = true;
			ctlFieldSetDetailGridView.Visible = true;
		}
		private void GetSuAnnouncementInfo(SuAnnouncement announcement)
		{
			short announcementGroupId = UIHelper.ParseShort(hdCtlLblAnnouncementGroupId.Value);
			
            //modify by tom 28/01/209
            //SCG.eAccounting.Web.UserControls.Calendar ctlCalEffectiveDate = ctlAnnouncementForm.FindControl("ctlCalEffectiveDate") as SCG.eAccounting.Web.UserControls.Calendar;
            //SCG.eAccounting.Web.UserControls.Calendar ctlCalLastDisplayDate = ctlAnnouncementForm.FindControl("ctlCalLastDisplayDate") as SCG.eAccounting.Web.UserControls.Calendar;
            UserControls.Calendar ctlCalEffectiveDate = ctlAnnouncementForm.FindControl("ctlCalEffectiveDate") as UserControls.Calendar;
            UserControls.Calendar ctlCalLastDisplayDate = ctlAnnouncementForm.FindControl("ctlCalLastDisplayDate") as UserControls.Calendar;
			TextBox ctlTxtComment = ctlAnnouncementForm.FindControl("ctlTxtComment") as TextBox;
			CheckBox ctlActive = ctlAnnouncementForm.FindControl("ctlActive") as CheckBox;

			if (!string.IsNullOrEmpty(ctlCalEffectiveDate.DateValue))
			{
				try
				{
					//DateTime effectiveDate = DateTime.Parse(ctlCalEffectiveDate.DateValue);
					announcement.EffectiveDate = UIHelper.ParseDate(ctlCalEffectiveDate.DateValue).GetValueOrDefault(DateTime.MinValue);
				}
				catch (Exception)
				{
					announcement.EffectiveDate = null;
				}
			}
			else
			{
				announcement.EffectiveDate = DateTime.MinValue;
			}

			if (!string.IsNullOrEmpty(ctlCalLastDisplayDate.DateValue))
			{
				try
				{
					//DateTime lastDisplayDate = DateTime.Parse(ctlCalLastDisplayDate.DateValue);
					announcement.LastDisplayDate = UIHelper.ParseDate(ctlCalLastDisplayDate.DateValue).GetValueOrDefault(DateTime.MinValue);
				}
				catch (Exception)
				{
					announcement.LastDisplayDate = null;
				}
			}
			else
			{
				announcement.LastDisplayDate = DateTime.MinValue;
			}
			
			announcement.AnnouncementGroup = SuAnnouncementGroupService.FindProxyByIdentity(announcementGroupId);
			announcement.Comment = ctlTxtComment.Text;
			announcement.Active = ctlActive.Checked;
			announcement.UpdPgm = ProgramCode;
            announcement.CreBy = UserAccount.UserID;
			announcement.CreDate = DateTime.Now;
            announcement.UpdBy = UserAccount.UserID;
			announcement.UpdDate = DateTime.Now;
		}
		private void GetSuAnnouncementLangInfo(SuAnnouncementLang announcementLang)
		{
			TextBox ctlTxtHeader = ctlAnnouncementLangForm.FindControl("ctlTxtHeader") as TextBox;
			TextBox ctlTxtBody = ctlAnnouncementLangForm.FindControl("ctlTxtBody") as TextBox;
			TextBox ctlTxtFooter = ctlAnnouncementLangForm.FindControl("ctlTxtFooter") as TextBox;
			TextBox ctlTxtComment = ctlAnnouncementLangForm.FindControl("ctlTxtComment") as TextBox;
			CheckBox ctlActive = ctlAnnouncementLangForm.FindControl("ctlActive") as CheckBox;
            FCKeditor ctlFCK = ctlAnnouncementLangForm.FindControl("ctlFCK") as FCKeditor;

			announcementLang.AnnouncementHeader = ctlTxtHeader.Text;
			//announcementLang.AnnouncementBody = ctlTxtBody.Text;
            announcementLang.AnnouncementBody = ctlFCK.Value;
			announcementLang.AnnouncementFooter = ctlTxtFooter.Text;
			announcementLang.Comment = ctlTxtComment.Text;
			announcementLang.Active = ctlActive.Checked;
			announcementLang.UpdPgm = ProgramCode;
            announcementLang.CreBy = UserAccount.UserID;
			announcementLang.CreDate = DateTime.Now;
            announcementLang.UpdBy = UserAccount.UserID;
			announcementLang.UpdDate = DateTime.Now;
		}
		private void RegisterScriptForGridView()
		{
			StringBuilder script = new StringBuilder();
			script.Append("function validateCheckBox(objChk, objFlag) ");
			script.Append("{ ");
			script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
			script.Append("'" + ctlAnnouncementGrid.ClientID + "', '" + ctlAnnouncementGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
			script.Append("} ");

			ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "validateChkBox", script.ToString(), true);
		}
		#endregion
	}
}

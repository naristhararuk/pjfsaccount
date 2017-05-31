using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.IO;

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
	public partial class AnnouncementGroup : BasePage
	{
		#region Property
		public IDbLanguageService DbLanguageService { get; set; }
		public ISuAnnouncementGroupService SuAnnouncementGroupService { get; set; }
		public ISuAnnouncementGroupLangService SuAnnouncementGroupLangService { get; set; }
		#endregion

		#region Override Method
		protected override void OnPreInit(EventArgs e)
		{
			base.OnPreInit(e);
		}
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            
            //Use for form that contain FileUpload Control.
            HtmlForm form = this.Page.Form;
            if ((form != null) && (form.Enctype.Length == 0))
            {
                form.Enctype = "multipart/form-data";
            }

            //if (!Page.IsPostBack)
            //{
            //    ctlAnnouncementGroupGrid.DataCountAndBind();
            //    divDetailGridViewButton.Visible = false;
            //    ctlFieldSetDetailGridView.Visible = false;
            //}
        }
        #endregion

        #region Page_Load Event
        protected void Page_Load(object sender, EventArgs e)
		{
            if (!Page.IsPostBack)
            {
                ctlAnnouncementGroupGrid.DataCountAndBind();
                divDetailGridViewButton.Visible = false;
                ctlFieldSetDetailGridView.Visible = false;
            }
		}
		#endregion

		#region Button Event
		protected void ctlAddNew_Click(object sender, ImageClickEventArgs e)
		{
			ctlModalPopupExtender.Show();
			ctlUpdPanelAnnGrpForm.Update();
            ctlAnnouncementGroupGrid.DataCountAndBind();
			ctlAnnouncementGroupForm.ChangeMode(FormViewMode.Insert);
		}
		protected void ctlDelete_Click(object sender, ImageClickEventArgs e)
		{
			foreach (GridViewRow row in ctlAnnouncementGroupGrid.Rows)
			{
				if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
				{
					try
					{
						//ctlAnnouncementGroupGrid.DeleteRow(row.RowIndex);
						short announcementGroupId = UIHelper.ParseShort(ctlAnnouncementGroupGrid.DataKeys[row.RowIndex].Value.ToString());
						SuAnnouncementGroup announcementGroup = SuAnnouncementGroupService.FindProxyByIdentity(announcementGroupId);
						SuAnnouncementGroupService.Delete(announcementGroup);
					}
					catch (Exception ex)
					{
						string exMessage = ex.Message;
					}
				}
			}
			
			ctlAnnouncementGroupLangGrid.DataSource = null;
			ctlAnnouncementGroupLangGrid.DataBind();
			divDetailGridViewButton.Visible = false;
			ctlFieldSetDetailGridView.Visible = false;
			ctlUpdPanelAnnGrpLang.Update();

			ctlAnnouncementGroupGrid.SelectedIndex = -1;
			ctlAnnouncementGroupGrid.DataCountAndBind();
			ctlUpdPanelAnnGrp.Update();
		}
		protected void ctlSubmit_Click(object sender, EventArgs e)
		{
			IList<SuAnnouncementGroupLang> list = new List<SuAnnouncementGroupLang>();
			short announcementGroupId = UIHelper.ParseShort(ctlAnnouncementGroupGrid.SelectedValue.ToString());
			SuAnnouncementGroup announcementGroup = QueryProvider.SuAnnouncementGroupQuery.FindProxyByIdentity(announcementGroupId);

			foreach (GridViewRow row in ctlAnnouncementGroupLangGrid.Rows)
			{
				TextBox ctlAnnouncementGroupName = row.FindControl("ctlAnnouncementGroupName") as TextBox;
				TextBox ctlComment = row.FindControl("ctlComment") as TextBox;
				CheckBox ctlActive = row.FindControl("ctlActive") as CheckBox;

				if (!string.IsNullOrEmpty(ctlAnnouncementGroupName.Text))
				{
					short languageId = UIHelper.ParseShort(ctlAnnouncementGroupLangGrid.DataKeys[row.RowIndex].Values["LanguageId"].ToString());

					SuAnnouncementGroupLang announcementGroupLang = new SuAnnouncementGroupLang();
					announcementGroupLang.Active = ctlActive.Checked;
					announcementGroupLang.Language = DbLanguageService.FindProxyByIdentity(languageId);
					announcementGroupLang.AnnouncementGroup = announcementGroup;
					announcementGroupLang.Comment = ctlComment.Text;
					announcementGroupLang.AnnouncementGroupName = ctlAnnouncementGroupName.Text;
					announcementGroupLang.CreBy = 0;
					announcementGroupLang.CreDate = DateTime.Now;
					announcementGroupLang.UpdBy = 0;
					announcementGroupLang.UpdDate = DateTime.Now;
					announcementGroupLang.UpdPgm = ProgramCode;

					list.Add(announcementGroupLang);
				}
			}
			
			// Update AnnouncementGroupLang.
			SuAnnouncementGroupLangService.UpdateAnnouncementGroupLang(list);

            ctlMessage.Message = GetMessage("SaveSuccessFully"); ;

			// Set data source of ctlAnnouncementGroupLangGrid.
			//ctlAnnouncementGroupLangGrid.DataSource = null;
			//ctlAnnouncementGroupLangGrid.DataBind();
			
			// Bind ctlAnnouncementGroupGrid without SelectedIndex set.
			//ctlAnnouncementGroupGrid.SelectedIndex = -1;
			//ctlAnnouncementGroupGrid.DataCountAndBind();
			//ctlUpdPanelAnnGrp.Update();
			//ctlSubmit.Visible = false;
		}
		protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
		{
			ctlAnnouncementGroupLangGrid.DataSource = null;
			ctlAnnouncementGroupLangGrid.DataBind();
			divDetailGridViewButton.Visible = false;
			ctlFieldSetDetailGridView.Visible = false;
			ctlUpdPanelAnnGrpLang.Update();
			

			ctlAnnouncementGroupGrid.SelectedIndex = -1;
			ctlAnnouncementGroupGrid.DataCountAndBind();
			ctlUpdPanelAnnGrp.Update();
		}
		#endregion

		#region GridView Event
		protected object RequestData1(int startRow, int pageSize, string sortExpression)
		{
			short languageId = UserAccount.CurrentLanguageID;
			return QueryProvider.SuAnnouncementGroupQuery.GetSuAnnouncementGroupSearchResultList(languageId, startRow, pageSize, sortExpression);
		}
		protected int ctlAnnouncementGroupGrid_RequestCount()
		{
			short languageId = UserAccount.CurrentLanguageID;
			return QueryProvider.SuAnnouncementGroupQuery.GetCountSuAnnouncementGroupSearchResult(languageId);
		}
		protected void ctlAnnouncementGroupGrid_DataBound(object sender, EventArgs e)
		{
			if (ctlAnnouncementGroupGrid.Rows.Count > 0)
			{
				RegisterScriptForGridView();	
			}
			
			divButton.Visible = true;
			ctlDelete.OnClientClick = String.Format("return ConfirmDelete('{0}')", ctlAnnouncementGroupGrid.ClientID);
		}
		protected void ctlAnnouncementGroupGrid_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			short announcementGroupId;
			if (e.CommandName == "EditAnnouncementGroup")
			{
				int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
				announcementGroupId = UIHelper.ParseShort(ctlAnnouncementGroupGrid.DataKeys[rowIndex].Value.ToString());
				ctlAnnouncementGroupGrid.EditIndex = rowIndex;
				
				IList<SuAnnouncementGroup> list = new List<SuAnnouncementGroup>();
				list.Add(SuAnnouncementGroupService.FindProxyByIdentity(announcementGroupId));
				
				ctlAnnouncementGroupForm.DataSource = list;
				ctlAnnouncementGroupForm.ChangeMode(FormViewMode.Edit);
                ctlAnnouncementGroupGrid.DataCountAndBind();
				ctlUpdPanelAnnGrpForm.Update();
				ctlAnnouncementGroupForm.DataBind();
				ctlModalPopupExtender.Show();
			}
			if (e.CommandName == "Select")
			{
				int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
				
				announcementGroupId = UIHelper.ParseShort(ctlAnnouncementGroupGrid.DataKeys[rowIndex].Value.ToString());
				ctlAnnouncementGroupLangGrid.DataSource = QueryProvider.SuAnnouncementGroupLangQuery.FindAnnouncementGroupLangByAnnouncementGroupId(announcementGroupId);
				ctlAnnouncementGroupLangGrid.DataBind();
				divDetailGridViewButton.Visible = true;
				ctlFieldSetDetailGridView.Visible = true;
                ctlAnnouncementGroupGrid.DataCountAndBind();
				ctlUpdPanelAnnGrpLang.Update();
			}
		}

		protected void ctlAnnouncementGroupLangGrid_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if ((e.Row.RowType == DataControlRowType.DataRow))
			{
				TextBox ctlAnnouncementGroupName = e.Row.FindControl("ctlAnnouncementGroupName") as TextBox;
				TextBox ctlComment = e.Row.FindControl("ctlAnnouncementGroupName") as TextBox;
				CheckBox ctlActive = e.Row.FindControl("ctlActive") as CheckBox;

				if (string.IsNullOrEmpty(ctlAnnouncementGroupName.Text.Trim()) && string.IsNullOrEmpty(ctlAnnouncementGroupName.Text.Trim()))
				{
					ctlActive.Checked = true;
				}
			}
		}
		#endregion

		#region FormView Event
		protected void ctlAnnouncementGroupForm_DataBound(object sender, EventArgs e)
		{
			if (ctlAnnouncementGroupForm.CurrentMode == FormViewMode.Insert)
			{
				if (ValidationErrors.IsEmpty)
				{
					TextBox ctlTxtName = ctlAnnouncementGroupForm.FindControl("ctlTxtName") as TextBox;
					ctlTxtName.Focus();
				}
				
				ImageButton ctlInsert = ctlAnnouncementGroupForm.FindControl("ctlInsert") as ImageButton;
				ScriptManager.GetCurrent(this).RegisterPostBackControl(ctlInsert);
			}
			else if (ctlAnnouncementGroupForm.CurrentMode == FormViewMode.Edit)
			{
				// Set AnnouncementGroupName to label ctlAnnouncementGroupName in ctlAnnouncementGruopForm.
				Label ctlTxtName = ctlAnnouncementGroupForm.FindControl("ctlTxtName") as Label;
				LinkButton ctlAnnouncementGroupName = ctlAnnouncementGroupGrid.Rows[ctlAnnouncementGroupGrid.EditIndex].FindControl("ctlAnnouncementGroupName") as LinkButton;
				ctlTxtName.Text = ctlAnnouncementGroupName.Text;

				if (ValidationErrors.IsEmpty)
				{
					TextBox txtDisplayOrder = ctlAnnouncementGroupForm.FindControl("txtDisplayOrder") as TextBox;
					txtDisplayOrder.Focus();
				}
					
				// Set Display Image.
				Label ctlImagePath = ctlAnnouncementGroupGrid.Rows[ctlAnnouncementGroupGrid.EditIndex].FindControl("ctlImagePath") as Label;
				Image ctlImage = ctlAnnouncementGroupForm.FindControl("ctlImage") as Image;
				
				//string filePath = "D:\\DotNetStandard\\main\\program\\NHibernate\\SCG.eAccounting.Web\\ImageFiles\\Announcement\\";
                string filePath = ParameterServices.AnnouncementGoupUploadFilePath;
				string fileName = ctlImagePath.Text;
				
				ctlImage.ImageUrl = filePath + ctlImagePath.Text;
				
				ImageButton ctlUpdate = ctlAnnouncementGroupForm.FindControl("ctlUpdate") as ImageButton;
				ScriptManager.GetCurrent(this).RegisterPostBackControl(ctlUpdate);
			}
		}
		protected void ctlAnnouncementGroupForm_ModeChanging(object sender, FormViewModeEventArgs e)
		{
			
		}
		protected void ctlAnnouncementGroupForm_ItemCommand(object sender, FormViewCommandEventArgs e)
		{
			if (e.CommandName == "Cancel")
			{
                ctlAnnouncementGroupGrid.DataCountAndBind();
				ClosePopUp();
			}
		}
		protected void ctlAnnouncementGroupForm_ItemInserting(object sender, FormViewInsertEventArgs e)
		{
			SuAnnouncementGroup announcementGroup = new SuAnnouncementGroup();
			HttpPostedFile imageFile;
			GetSuAnnouncementGroupInfo(announcementGroup, out imageFile);

			SuAnnouncementGroupLang announcementGroupLang = new SuAnnouncementGroupLang();
			TextBox ctlTxtName = ctlAnnouncementGroupForm.FindControl("ctlTxtName") as TextBox;
			short languageId = UserAccount.CurrentLanguageID;
			announcementGroupLang.Language = DbLanguageService.FindByIdentity(languageId);
			announcementGroupLang.AnnouncementGroup = announcementGroup;
			announcementGroupLang.AnnouncementGroupName = ctlTxtName.Text;
			announcementGroupLang.Active = announcementGroup.Active;
			announcementGroupLang.CreBy = 0;
			announcementGroupLang.CreDate = DateTime.Now;
			announcementGroupLang.UpdPgm = ProgramCode;
			announcementGroupLang.UpdBy = 0;
			announcementGroupLang.UpdDate = DateTime.Now;

			try
			{
				if (imageFile == null)
				{
					SuAnnouncementGroupService.AddAnnouncementGroup(announcementGroup, announcementGroupLang);
				}
				else
				{
					// If imageFile is not null.
					short newAnnouncementGroupId = SuAnnouncementGroupService.AddAnnouncementGroup(announcementGroup, announcementGroupLang, imageFile);
					
					// Get file path from Database DbParameter.
					// Save new file to stored directory.
					//string filePath = "D:\\DotNetStandard\\main\\program\\NHibernate\\SCG.eAccounting.Web\\ImageFiles\\Announcement\\";
                    string filePath = ParameterServices.AnnouncementGoupUploadFilePath;
					filePath = AppDomain.CurrentDomain.BaseDirectory + filePath.Replace("~", string.Empty).Replace("/", "\\");
					if (Directory.Exists(filePath))
					{
						// imagePath contain FileName of each AnnouncementGroup.
						imageFile.SaveAs(filePath + announcementGroup.ImagePath);
					}
					else
					{
						Directory.CreateDirectory(filePath);
						imageFile.SaveAs(filePath + announcementGroup.ImagePath);
					}
				}
			}
			catch (ServiceValidationException ex)
			{
				ValidationErrors.MergeErrors(ex.ValidationErrors);
			}

			// Check if no validation error then changemode to default mode.
			if (ValidationErrors.IsEmpty)
			{
				ctlAnnouncementGroupForm.ChangeMode(FormViewMode.ReadOnly);
				ClosePopUp();
				ctlAnnouncementGroupGrid.DataCountAndBind();
			}
		}
		protected void ctlAnnouncementGroupForm_ItemUpdating(object sender, FormViewUpdateEventArgs e)
		{
			short announcementGroupId = UIHelper.ParseShort(ctlAnnouncementGroupForm.DataKey.Value.ToString());
			SuAnnouncementGroup announcementGroup = SuAnnouncementGroupService.FindProxyByIdentity(announcementGroupId);
			string existingFile = announcementGroup.ImagePath;
			HttpPostedFile imageFile;
			GetSuAnnouncementGroupInfo(announcementGroup, out imageFile);

			try
			{
				if (imageFile == null)
				{
					SuAnnouncementGroupService.UpdateAnnouncementGroup(announcementGroup);
				}
				else
				{
					// if user input new file for announcementGroup.
					SuAnnouncementGroupService.UpdateAnnouncementGroup(announcementGroup, imageFile);

					// Get file path from Database table DbParameter.
					//string filePath = "D:\\DotNetStandard\\main\\program\\NHibernate\\SCG.eAccounting.Web\\ImageFiles\\Announcement\\";
                    string filePath = ParameterServices.AnnouncementGoupUploadFilePath;
					filePath = AppDomain.CurrentDomain.BaseDirectory + filePath.Replace("~", string.Empty).Replace("/", "\\");
					// Check Directory Exist.
					if (Directory.Exists(filePath))
					{
						// Delete if old file exist.
						if (File.Exists(filePath + existingFile))
						{
							File.Delete(filePath + existingFile);
						}

						// Save new file to specific folder.
						// imagePath contain FileName for each announcemntGroup.
						imageFile.SaveAs(filePath + announcementGroup.ImagePath);
					}
					else
					{
						// Store directory does not exist then create new directory.
						Directory.CreateDirectory(filePath);
						imageFile.SaveAs(filePath + announcementGroup.ImagePath);
					}
				}
			}
			catch (ServiceValidationException ex)
			{
				ValidationErrors.MergeErrors(ex.ValidationErrors);
			}

			// Check if no validation error then changemode to default mode.
			if (ValidationErrors.IsEmpty)
			{
				ctlAnnouncementGroupForm.ChangeMode(FormViewMode.ReadOnly);
				ClosePopUp();
				ctlAnnouncementGroupGrid.DataCountAndBind();
			}
		}
		#endregion

		#region Private Method
		private void ClosePopUp()
		{
			hdModalPopupState.Value = "hidden";
			ctlModalPopupExtender.Hide();
			ctlUpdPanelAnnGrp.Update();
		}
		private void GetSuAnnouncementGroupInfo(SuAnnouncementGroup announcementGroup, out HttpPostedFile imageFile)
		{
			TextBox txtDisplayOrder = ctlAnnouncementGroupForm.FindControl("txtDisplayOrder") as TextBox;
			TextBox ctlComment = ctlAnnouncementGroupForm.FindControl("ctlComment") as TextBox;
			CheckBox ctlActive = ctlAnnouncementGroupForm.FindControl("ctlActive") as CheckBox;
			FileUpload ctlImageFile = ctlAnnouncementGroupForm.FindControl("ctlImageFile") as FileUpload;
			
			if (ctlImageFile.HasFile)
			{
				// announcementGroup.ImagePath = save only file name(exclude filepath).
				FileInfo info = new FileInfo(ctlImageFile.PostedFile.FileName);
				string fileName = info.Name.Replace(info.Extension, "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + info.Extension);
				imageFile = ctlImageFile.PostedFile;
				
				announcementGroup.ImagePath = fileName;
			}
			else
			{
				imageFile = null;
			}
			
			announcementGroup.Comment = ctlComment.Text;
			if (!string.IsNullOrEmpty(txtDisplayOrder.Text.Trim()))
			{
				try
				{
					short displayOrder = short.Parse(txtDisplayOrder.Text.Trim());
					announcementGroup.DisplayOrder = displayOrder;
				}
				catch (Exception)
				{
					announcementGroup.DisplayOrder = null;
				}
			}
			else
			{
				announcementGroup.DisplayOrder = 0;
			}
			announcementGroup.Active = ctlActive.Checked;
			announcementGroup.CreBy = 0;
			announcementGroup.CreDate = DateTime.Now;
			announcementGroup.UpdBy = 0;
			announcementGroup.UpdDate = DateTime.Now;
			announcementGroup.UpdPgm = ProgramCode;
		}
		private void RegisterScriptForGridView()
		{
			StringBuilder script = new StringBuilder();
			script.Append("function validateCheckBox(objChk, objFlag) ");
			script.Append("{ ");
			script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
			script.Append("'" + ctlAnnouncementGroupGrid.ClientID + "', '" + ctlAnnouncementGroupGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
			script.Append("} ");

			ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "validateChkBox", script.ToString(), true);
		}
		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Configuration;


using SS.Standard.UI;

using SS.SU.DTO;
using SS.SU.BLL;
using SS.SU.Query;

using SCG.eAccounting.Web.Helper;

using SS.DB.Query;
using SS.DB.DTO;
using SS.DB.BLL;
using SS.Standard.Utilities;

namespace SCG.eAccounting.Web.Forms.SU.Programs
{
    public partial class Language : BasePage
    {
        //List<DbLanguage> LanguageList;
        public IDbLanguageService DbLanguageService { get; set; }
        public IDbLanguageQuery DbLanguageQuery { get; set; }



        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        #region Button Event
        protected void ctlAddNew_Click(object sender, ImageClickEventArgs e)
        {

            ctlLanguageModalPopupExtender.Show();
            ctlLanguageGrid.DataCountAndBind();
            ctlLanguageForm.ChangeMode(FormViewMode.Insert);
            UpdatePanelLanguageForm.Update();
        }
        protected void ctlDelete_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in ctlLanguageGrid.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
                {
                    try
                    {
                        short languageId = UIHelper.ParseShort(ctlLanguageGrid.DataKeys[row.RowIndex].Value.ToString());
                        DbLanguage language = DbLanguageService.FindByIdentity(languageId);

                        DbLanguageService.Delete(language);
                    }
                    catch (Exception ex)
                    {
                        string exMessage = ex.Message;
                    }
                }
            }

            // Bind Grid After Delete User Successful.
            ctlLanguageGrid.DataCountAndBind();
        }
        #endregion

        protected void DbLanguageDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = SsDbQueryProvider.DbLanguageQuery;
        }
        #region Object DataSource Inserting, Updating Event (Not in use now)
        protected void DbLanguageDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            DbLanguage language = e.InputParameters[0] as DbLanguage;
            //GetSuLanguageInfo(language);
        }
        protected void DbLanguageDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            DbLanguage language = e.InputParameters[0] as DbLanguage;
            //GetSuLanguageInfo(language);
        }
        protected void DbLanguageDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            //e.InputParameters["languageID"] = UIHelper.ParseShort("1");
        }
        #endregion

        #region FormView Event
        protected void ctlLanguageForm_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            e.Cancel = true;
        }
        protected void ctlLanguageForm_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                ctlLanguageGrid.DataCountAndBind();
                ClosePopUp();
            }
        }
        protected void ctlLanguageForm_DataBound(object sender, EventArgs e)
        {
            if (ctlLanguageForm.CurrentMode == FormViewMode.Insert)
            {

                TextBox ctlLanguage = ctlLanguageForm.FindControl("ctlLanguage") as TextBox;
                CheckBox chkActive = ctlLanguageForm.FindControl("chkActive") as CheckBox;
                chkActive.Checked = true;
                ctlLanguage.Focus();

                ImageButton ctlInsert = ctlLanguageForm.FindControl("ctlInsert") as ImageButton;
                ScriptManager.GetCurrent(this).RegisterPostBackControl(ctlInsert);
            }
            else if (ctlLanguageForm.CurrentMode == FormViewMode.Edit)
            {

                // Set AnnouncementGroupName to label ctlAnnouncementGroupName in ctlAnnouncementGruopForm.
                Label ctlLanguage = ctlLanguageForm.FindControl("ctlLanguage") as Label;
                Label ctlLanguageName = ctlLanguageGrid.Rows[ctlLanguageGrid.EditIndex].FindControl("ctlLblLanguage") as Label;
                ctlLanguage.Text = ctlLanguageName.Text;

                // Set Display Image.
                Label ctlImagePath = ctlLanguageGrid.Rows[ctlLanguageGrid.EditIndex].FindControl("ctlLblImagePath") as Label;
                Image ctlImage = ctlLanguageForm.FindControl("ctlImage") as Image;

                //string filePath = "D:\\Bow\\Project\\SCG\\main\\program\\NHibernate\\SCG.eAccounting.Web\\ImageFiles\\Flags\\";
                string filePath = ParameterServices.FlagUploadFilePath;
                string fileName = ctlImagePath.Text;

                ctlImage.ImageUrl = filePath + ctlImagePath.Text;

                ImageButton ctlUpdate = ctlLanguageForm.FindControl("ctlUpdate") as ImageButton;
                ScriptManager.GetCurrent(this).RegisterPostBackControl(ctlUpdate);
            }
        }
        protected void ctlLanguageForm_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            DbLanguage language = new DbLanguage();
            HttpPostedFile imageFile;
            GetDbLanguageInfo(language,out imageFile);
            try
            {
                if (imageFile == null)
                {
                    DbLanguageService.AddLanguage(language);
                }
                else
                {
                    // If imageFile is not null.
                    DbLanguageService.AddLanguage(language, imageFile);

                    // Get file path from Database DbParameter.
                    // Save new file to stored directory.
                    //string filePath = "D:\\Bow\\Project\\SCG\\main\\program\\NHibernate\\SCG.eAccounting.Web\\ImageFiles\\Flags\\";
                    string filePath = ParameterServices.FlagUploadFilePath;
                    filePath = AppDomain.CurrentDomain.BaseDirectory + filePath.Replace("~", string.Empty).Replace("/", "\\");
                    if (Directory.Exists(filePath))
                    {
                        // imagePath contain FileName of each AnnouncementGroup.
                        imageFile.SaveAs(filePath + language.ImagePath);
                    }
                    else
                    {
                        Directory.CreateDirectory(filePath);
                        imageFile.SaveAs(filePath + language.ImagePath);
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
                ctlLanguageForm.ChangeMode(FormViewMode.ReadOnly);
                ClosePopUp();
                ctlLanguageGrid.DataCountAndBind();
            }
        }
        protected void ctlLanguageForm_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            short languageId = UIHelper.ParseShort(ctlLanguageForm.DataKey["LanguageID"].ToString());
            DbLanguage language = DbLanguageService.FindByIdentity(languageId);
            string existingFile = language.ImagePath;

            HttpPostedFile imageFile;
            GetDbLanguageInfo(language, out imageFile);

            try
            {
                if (imageFile == null)
                {
                    DbLanguageService.UpdateLanguage(language);
                }
                else
                {
                    // If imageFile is not null.
                    DbLanguageService.UpdateLanguage(language, imageFile);

                    // Get file path from Database DbParameter.
                    // Save new file to stored directory.
                    string filePath = ParameterServices.FlagUploadFilePath;
                    filePath = AppDomain.CurrentDomain.BaseDirectory + filePath.Replace("~", string.Empty).Replace("/", "\\");
                    if (Directory.Exists(filePath))
                    {
                        // imagePath contain FileName of each AnnouncementGroup.
                        imageFile.SaveAs(filePath + language.ImagePath);
                    }
                    else
                    {
                        Directory.CreateDirectory(filePath);
                        imageFile.SaveAs(filePath + language.ImagePath);
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
                ctlLanguageForm.ChangeMode(FormViewMode.ReadOnly);
                ClosePopUp();
                ctlLanguageGrid.DataCountAndBind();
            }
        }
        #endregion

        #region Private Function
        private void ClosePopUp()
        {
            hdModalPopupState.Value = "hidden";
            ctlLanguageModalPopupExtender.Hide();
            UpdatePanelGridView.Update();
        }
        private void GetDbLanguageInfo(DbLanguage language, out HttpPostedFile imageFile)
        {
            //UserControl LanguageEditor1 = ctlLanguageForm.FindControl("LanguageEditor1") as UserControl;
            TextBox ctlLanguageCode = ctlLanguageForm.FindControl("ctlLanguageCode") as TextBox;
            TextBox ctlComment = ctlLanguageForm.FindControl("ctlComment") as TextBox;
            CheckBox chkActive = ctlLanguageForm.FindControl("chkActive") as CheckBox;
            FileUpload ctlImageFile = ctlLanguageForm.FindControl("ctlImageFile") as FileUpload;

            if (ctlImageFile.HasFile)
            {
                // announcementGroup.ImagePath = save only file name(exclude filepath).
                FileInfo info = new FileInfo(ctlImageFile.PostedFile.FileName);
                string fileName = info.Name.Replace(info.Extension, "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + info.Extension);
                //string[] fileNameArray = imageFile.FileName.Split(new string[] { "\\" }, StringSplitOptions.None);
                imageFile = ctlImageFile.PostedFile;

                language.ImagePath = fileName;
            }
            else
            {
                imageFile = null;
            }

            language.Comment = ctlComment.Text;
            if (ctlLanguageForm.CurrentMode.Equals(FormViewMode.Insert))
            {
                TextBox ctlLanguage = ctlLanguageForm.FindControl("ctlLanguage") as TextBox;
                language.LanguageName = ctlLanguage.Text;
            }
            language.LanguageCode = ctlLanguageCode.Text;
            language.Active = chkActive.Checked;
            language.CreBy = UserAccount.UserID;
            language.CreDate = DateTime.Now;
            language.UpdBy = UserAccount.UserID;
            language.UpdDate = DateTime.Now;
            language.UpdPgm = ProgramCode;

        }
        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBox(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlLanguageGrid.ClientID + "', '" + ctlLanguageGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "validateChkBox", script.ToString(), true);
        }
        
        #endregion

        #region GridView Event
        protected object RequestData(int startRow, int pageSize, string sortExpression)
        {
            short languageId = UserAccount.CurrentLanguageID;
            return SsDbQueryProvider.DbLanguageQuery.GetLanguageList(languageId, startRow, pageSize, sortExpression);
        }
        protected int ctlLanguageGrid_RequestCount()
        {
            short languageId = UserAccount.CurrentLanguageID;

            return SsDbQueryProvider.DbLanguageQuery.GetCountLanguageList(languageId);
        }
        protected void ctlLanguageGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UserEdit")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                short languageId = UIHelper.ParseShort(ctlLanguageGrid.DataKeys[rowIndex].Value.ToString());
                ctlLanguageGrid.EditIndex = rowIndex;

                IList<DbLanguage> languageList = new List<DbLanguage>();
                DbLanguage language = DbLanguageService.FindProxyByIdentity(languageId);
                languageList.Add(language);

                ctlLanguageForm.DataSource = languageList;
                ctlLanguageForm.ChangeMode(FormViewMode.Edit);
                ctlLanguageGrid.DataCountAndBind();

                UpdatePanelLanguageForm.Update();
                ctlLanguageForm.DataBind();
                ctlLanguageModalPopupExtender.Show();
            }
        }
        protected void ctlLanguageGrid_DataBound(object sender, EventArgs e)
        {
            if (ctlLanguageGrid.Rows.Count > 0)
            {
                divButton.Visible = true;
                RegisterScriptForGridView();

                ctlDelete.OnClientClick = String.Format("return ConfirmDelete('{0}')", ctlLanguageGrid.ClientID);
            }
            else
            {
                divButton.Visible = false;
            }
        }
        #endregion


    }
}

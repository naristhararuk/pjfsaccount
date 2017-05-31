using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using System.Text;
using SS.SU.BLL;
using SS.SU.Query;
using SCG.eAccounting.Web.Helper;
using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using FredCK.FCKeditorV2;
using SS.DB.BLL;
using System.IO;
using SS.DB.Query;
using System.Web.UI.HtmlControls;
using SS.Standard.Utilities;

namespace SCG.eAccounting.Web.Forms.SU.Programs
{
    public partial class ManageHeader : BasePage
    {
        public ISuRTENodeService SuRTENodeService { get; set; }
        public ISuRTEContentService SuRTEContentService { get; set; }
        public IDbLanguageService DbLanguageService { get; set; }
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
            //    ctlNodeGridView.DataCountAndBind();
            //    ctlCancel.Visible = false;
            //    ctlFieldSetDetailGridView.Visible = false;
            //}
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ctlNodeGridView.DataCountAndBind();
                ctlCancel.Visible = false;
                ctlFieldSetDetailGridView.Visible = false;
            }
        }

        #region ctlNodeGridView Event
        protected void ctlNodeGridView_DataBound(object sender, EventArgs e)
        {
            if (ctlNodeGridView.Rows.Count > 0)
            {
                RegisterScriptForGridView();


            }
            else
            {
            }
        }
        protected void ctlNodeGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            short nodeId;
            if (e.CommandName == "EditNode")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                nodeId = UIHelper.ParseShort(ctlNodeGridView.DataKeys[rowIndex].Value.ToString());

                ctlNodeGridView.EditIndex = rowIndex;
                ctlNodeGridView.PageIndex = (ctlNodeGridView.PageIndex * ctlNodeGridView.PageSize) + rowIndex;
                ctlNodeForm.ChangeMode(FormViewMode.Edit);
                IList<SuRTENode> list = new List<SuRTENode>();
                list.Add(SuRTENodeService.FindByIdentity(nodeId));
                ctlNodeForm.DataSource = list;
                ctlNodeForm.DataBind();
                ctlNodeGridView.DataCountAndBind();
                ctlUpdatePanelNodeForm.Update();
                ctlModalPopupExtender1.Show();
            }
            if (e.CommandName == "Select")
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                nodeId = UIHelper.ParseShort(ctlNodeGridView.DataKeys[rowIndex].Value.ToString());
                BindContentGrid(nodeId);
                ctlNodeGridView.DataCountAndBind();
                ctlUpdPanelContentGrid.Update();
            }
        }


        #endregion

        #region ctlContentGrid Event
        protected void ctlContentGrid_RowDataBound(object sender, EventArgs e)
        {

        }
        protected void ctlContentGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                short nodeId = UIHelper.ParseShort(ctlNodeGridView.SelectedDataKey.Value.ToString());
                short languageId = UIHelper.ParseShort(ctlContentGrid.DataKeys[rowIndex]["LanguageId"].ToString());
                short contentId = UIHelper.ParseShort(ctlContentGrid.DataKeys[rowIndex]["ContentId"].ToString());
                IList<SuRTEContentSearchResult> contentList = QueryProvider.SuRTEContentQuery.FindSuRTEContentByContentIdLanguageId(contentId, languageId);

                if (contentList.Count > 0)
                {
                    ctlContentForm.DataSource = contentList;
                    ctlContentForm.ChangeMode(FormViewMode.Edit);
                }
                else
                {
                    ctlContentForm.DataSource = contentList;
                    ctlContentForm.ChangeMode(FormViewMode.Insert);
                }

                ctlUpddatePanelContentForm.Update();
                ctlContentForm.DataBind();
                ctlModalPopupExtender2.Show();
            }
        }
        #endregion

        #region Button Event

        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            ContentGridViewFinish();
            ctlUpdPanelMasterGrid.Update();
        }

        #endregion

        #region FormView Event
        #region ctlNodeForm Event
        protected void ctlNodeForm_DataBound(object sender, EventArgs e)
        {
            if (ctlNodeForm.CurrentMode == FormViewMode.Insert)
            {
                if (ValidationErrors.IsEmpty)
                {
                    TextBox ctlNodeHeader = ctlNodeForm.FindControl("ctlNodeHeader") as TextBox;
                    ctlNodeHeader.Focus();
                }
                ImageButton ctlInsert = ctlNodeForm.FindControl("ctlInsert") as ImageButton;
                ScriptManager.GetCurrent(this).RegisterPostBackControl(ctlInsert);
            }
            else if (ctlNodeForm.CurrentMode == FormViewMode.Edit)
            {
                // Set AnnouncementGroupName to label ctlAnnouncementGroupName in ctlAnnouncementGruopForm.
                //Label ctlTxtName = ctlNodeForm.FindControl("ctlTxtName") as Label;
                //LinkButton ctlAnnouncementGroupName = ctlGrid.Rows[ctlAnnouncementGroupGrid.EditIndex].FindControl("ctlAnnouncementGroupName") as LinkButton;
                //ctlTxtName.Text = ctlAnnouncementGroupName.Text;

                if (ValidationErrors.IsEmpty)
                {
                    TextBox ctlNodeHeader = ctlNodeForm.FindControl("ctlNodeHeader") as TextBox;
                    ctlNodeHeader.Focus();
                }

                // Set Display Image.
                //Label ctlImagePath = ctlAnnouncementGroupGrid.Rows[ctlAnnouncementGroupGrid.EditIndex].FindControl("ctlImagePath") as Label;
                SuRTENode node = QueryProvider.SuRTENodeQuery.FindByIdentity(UIHelper.ParseShort(ctlNodeForm.DataKey.Value.ToString()));
                Image ctlImage = ctlNodeForm.FindControl("ctlImage") as Image;

                //string filePath = "D:\\DotNetStandard\\main\\program\\NHibernate\\SS.Web\\ImageFiles\\Announcement\\";
                string filePath = ParameterServices.FilePathService;
                string fileName = node.ImagePath;

                ctlImage.ImageUrl = filePath + fileName;

                ImageButton ctlUpdate = ctlNodeForm.FindControl("ctlUpdate") as ImageButton;
                ScriptManager.GetCurrent(this).RegisterPostBackControl(ctlUpdate);
            }
        }
        protected void ctlNodeForm_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            HttpPostedFile imageFile;
            SuRTENode node = SuRTENodeService.FindByIdentity(UIHelper.ParseShort(ctlNodeForm.DataKey.Value.ToString()));
            string existingFile = node.ImagePath;
            GetNodeInfo(node, out imageFile);

            try
            {
                if (imageFile == null)
                {
                    SuRTENodeService.UpdateNode(node);
                }
                else 
                {
                    SuRTENodeService.UpdateNode(node, imageFile);

                    // Get file path from Database table DbParameter.
                    //string filePath = "D:\\DotNetStandard\\main\\program\\NHibernate\\SS.Web\\ImageFiles\\Announcement\\";
                    string filePath = ParameterServices.FilePathService;
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
                        imageFile.SaveAs(filePath + node.ImagePath);
                    }
                    else
                    {
                        // Store directory does not exist then create new directory.
                        Directory.CreateDirectory(filePath);
                        imageFile.SaveAs(filePath + node.ImagePath);
                    }
                }
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
            if (ValidationErrors.IsEmpty)
            {
                ctlNodeForm.ChangeMode(FormViewMode.ReadOnly);
                ClosePopUpNodeForm();
                ctlNodeGridView.DataCountAndBind();
            }
        }
        protected void ctlNodeForm_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            HttpPostedFile imageFile;
            SuRTENode node = new SuRTENode();
            GetNodeInfo(node, out imageFile);

            try
            {
                if (imageFile == null)
                {
                    SuRTENodeService.AddNode(node);
                }
                else
                {
                    SuRTENodeService.AddNode(node, imageFile);

                    // Get file path from Database DbParameter.
                    // Save new file to stored directory.
                    //string filePath = "D:\\DotNetStandard\\main\\program\\NHibernate\\SS.Web\\ImageFiles\\Announcement\\";
                    string filePath = ParameterServices.FilePathService;
                    filePath = AppDomain.CurrentDomain.BaseDirectory + filePath.Replace("~", string.Empty).Replace("/", "\\");
                    if (Directory.Exists(filePath))
                    {
                        // imagePath contain FileName of each AnnouncementGroup.
                        imageFile.SaveAs(filePath + node.ImagePath);
                    }
                    else
                    {
                        Directory.CreateDirectory(filePath);
                        imageFile.SaveAs(filePath + node.ImagePath);
                    }
                }
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
            if (ValidationErrors.IsEmpty)
            {
                ctlNodeForm.ChangeMode(FormViewMode.ReadOnly);
                ClosePopUpNodeForm();
                ctlNodeGridView.DataCountAndBind();
            }
        }
        protected void ctlNodeForm_ModeChanging(object sender, FormViewModeEventArgs e)
        {

        }
        protected void ctlNodeForm_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                ClosePopUpNodeForm();
                ctlNodeGridView.DataCountAndBind();
            }
        }

        #endregion
        #region ctlContentForm Event
        protected void ctlContentForm_DataBound(object sender, EventArgs e)
        {
            if (ctlContentForm.CurrentMode != FormViewMode.ReadOnly)
            {
                if (ValidationErrors.IsEmpty)
                {
                    TextBox ctlHeader = ctlContentForm.FindControl("ctlContentHeader") as TextBox;
                    ctlHeader.Focus();
                }
            }
        }
        protected void ctlContentForm_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            short nodeId = UIHelper.ParseShort(ctlNodeGridView.SelectedDataKey.Value.ToString());
            short contentId = UIHelper.ParseShort(ctlContentForm.DataKey["ContentId"].ToString());
            SuRTEContent content = SuRTEContentService.FindByIdentity(contentId);
            GetContentInfo(content);
            try
            {
                SuRTEContentService.UpdateContent(content);
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }

            if (ValidationErrors.IsEmpty)
            {
                ctlContentForm.ChangeMode(FormViewMode.ReadOnly);
                ClosePopUpContentForm();
                BindContentGrid(nodeId);
            }
        }
        protected void ctlContentForm_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            short nodeId = UIHelper.ParseShort(ctlNodeGridView.SelectedDataKey.Value.ToString());
            short languageId = UIHelper.ParseShort(ctlContentGrid.SelectedDataKey["LanguageId"].ToString());
            SuRTEContent content = new SuRTEContent();
            content.Node = SuRTENodeService.FindProxyByIdentity(nodeId);
            content.Language = DbLanguageService.FindProxyByIdentity(languageId);
            GetContentInfo(content);

            try
            {
                SuRTEContentService.AddContent(content);
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }

            if (ValidationErrors.IsEmpty)
            {
                ctlContentForm.ChangeMode(FormViewMode.ReadOnly);
                ClosePopUpContentForm();
                BindContentGrid(nodeId);
            }
        }
        protected void ctlContentForm_ModeChanging(object sender, FormViewModeEventArgs e)
        {

        }
        protected void ctlContentForm_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                short nodeId = UIHelper.ParseShort(ctlNodeGridView.SelectedDataKey.Value.ToString());
                ClosePopUpContentForm();
                BindContentGrid(nodeId);
            }
        }
        #endregion
        #endregion

        #region Public & Private Method
        public Object RequestData1(int startRow, int pageSize, string sortExpression)
        {
            short languageId = UserAccount.CurrentLanguageID;
            return QueryProvider.SuRTENodeQuery.GetSuRTENodeList(languageId, startRow, pageSize, sortExpression);
        }
        public int RequestCount1()
        {
            short languageId = UserAccount.CurrentLanguageID;
            return QueryProvider.SuRTENodeQuery.GetCountList(languageId);
        }

        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
 

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "validateChkBox", script.ToString(), true);
        }
        private void ClosePopUpNodeForm()
        {
            ctlModalPopupExtender1.Hide();
            ctlUpdPanelMasterGrid.Update();
            ctlNodeForm.ChangeMode(FormViewMode.ReadOnly);
        }
        private void ClosePopUpContentForm()
        {
            ctlModalPopupExtender2.Hide();
            ctlUpdPanelContentGrid.Update();
            ctlContentForm.ChangeMode(FormViewMode.ReadOnly);
        }
        public void ContentGridViewFinish()
        {
            ctlNodeGridView.SelectedIndex = -1;
            ctlContentGrid.DataSource = null;
            ctlContentGrid.DataBind();
            ctlUpdPanelContentGrid.Update();
            ctlCancel.Visible = false;
            ctlFieldSetDetailGridView.Visible = false;
        }

        public void BindContentGrid(short nodeId)
        {
            ctlContentGrid.DataSource = QueryProvider.SuRTEContentQuery.FindSuRTEContentByNodeId(nodeId);
            ctlContentGrid.DataBind();
            ctlCancel.Visible = true;
            ctlFieldSetDetailGridView.Visible = true;
        }

        private void GetContentInfo(SuRTEContent content)
        {
            TextBox ctlHeader = ctlContentForm.FindControl("ctlContentHeader") as TextBox;
            TextBox ctlComment = ctlContentForm.FindControl("ctlContentComment") as TextBox;
            CheckBox ctlActive = ctlContentForm.FindControl("ctlContentActive") as CheckBox;
            FCKeditor ctlFCK = ctlContentForm.FindControl("ctlFCK") as FCKeditor;

            content.Header = ctlHeader.Text;
            content.Content = ctlFCK.Value;
            content.Comment = ctlComment.Text;
            content.Active = ctlActive.Checked;
            content.UpdPgm = ProgramCode;
            content.CreBy = UserAccount.UserID;
            content.CreDate = DateTime.Now;
            content.UpdBy = UserAccount.UserID;
            content.UpdDate = DateTime.Now;
        }

        private void GetNodeInfo(SuRTENode node, out HttpPostedFile imageFile)
        {
            TextBox ctlNodeHeader = ctlNodeForm.FindControl("ctlNodeHeader") as TextBox;
            TextBox ctlNodeOrderNo = ctlNodeForm.FindControl("ctlNodeOrderNo") as TextBox;
            TextBox ctlNodeType = ctlNodeForm.FindControl("ctlNodeType") as TextBox;
            FileUpload ctlImageFile = ctlNodeForm.FindControl("ctlImageFile") as FileUpload;
            TextBox ctlComment = ctlNodeForm.FindControl("ctlNodeComment") as TextBox;
            CheckBox ctlActive = ctlNodeForm.FindControl("ctlNodeActive") as CheckBox;

            if (ctlImageFile.HasFile)
            {
                // announcementGroup.ImagePath = save only file name(exclude filepath).
                FileInfo info = new FileInfo(ctlImageFile.PostedFile.FileName);
                string fileName = info.Name.Replace(info.Extension, "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + info.Extension);
                imageFile = ctlImageFile.PostedFile;

                node.ImagePath = fileName;
            }
            else
            {
                imageFile = null;
            }

            if (!string.IsNullOrEmpty(ctlNodeHeader.Text))
            {
                node.NodeHeaderid = UIHelper.ParseShort(ctlNodeHeader.Text);
            }
            if (!string.IsNullOrEmpty(ctlNodeOrderNo.Text))
            {
                node.NodeOrderNo = UIHelper.ParseShort(ctlNodeOrderNo.Text);
            }
            node.NodeType = ctlNodeType.Text;
            node.Comment = ctlComment.Text;
            node.Active = ctlActive.Checked;
            node.UpdPgm = ProgramCode;
            node.CreBy = UserAccount.UserID;
            node.CreDate = DateTime.Now;
            node.UpdBy = UserAccount.UserID;
            node.UpdDate = DateTime.Now;
        }
        #endregion
    }
}

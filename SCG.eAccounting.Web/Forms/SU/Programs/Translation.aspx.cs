using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SS.SU.BLL;
using SS.SU.DTO;
using SS.SU.Query;
using SCG.eAccounting.Web.Helper;
using System.Text;
using System.Web.UI.HtmlControls;

using SS.DB.DTO;
using SS.DB.BLL;
using SS.DB.Query;
using SS.Standard.Utilities;

namespace SCG.eAccounting.Web.Forms.SU.Programs
{
    public partial class Translation : BasePage
    {
        #region Properties
        public ISuGlobalTranslateService SuGlobalTranslateService { get; set; }
        public ISuGlobalTranslateLangService SuGlobalTranslateLangService { get; set; }
        #endregion
        #region Override Method

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //Use for form that contain FileUpload Control.
            HtmlForm form = this.Page.Form;
            if ((form != null) && (form.Enctype.Length == 0))
            {
                form.Enctype = "multipart/form-data";
            }

            
        }
        #endregion
        #region Load Event
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            //ProgramCode = "SURT03";
            //ProgramCode = "Translation";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ctlGlobalTranslateGrid.DataCountAndBind();
                ctlSubmit.Visible = false;
            }
        }
        #endregion

        #region GlobalTranslate Gridview
        protected void ctlGlobalTranslateGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            long translateId;
            if (e.CommandName == "TranslateEdit")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                translateId = UIHelper.ParseLong(ctlGlobalTranslateGrid.DataKeys[rowIndex].Value.ToString());
                ctlTranslateForm.PageIndex = (ctlGlobalTranslateGrid.PageIndex * ctlGlobalTranslateGrid.PageSize) + rowIndex;
                ctlTranslateForm.ChangeMode(FormViewMode.Edit);
                IList<SuGlobalTranslate> list = new List<SuGlobalTranslate>();
                list.Add(SuGlobalTranslateService.FindByIdentity(translateId));
                ctlTranslateForm.DataSource = list;
                ctlTranslateForm.DataBind();
                ctlUpdatePanelTranslateForm.Update();
                ctlGlobalTranslateGrid.DataCountAndBind();
                ctlTranslateModalPopupExtender.Show();

            }
            if (e.CommandName == "Select")
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                translateId = UIHelper.ParseLong(ctlGlobalTranslateGrid.DataKeys[rowIndex].Value.ToString());
                ctlTranslateLangGrid.DataSource = QueryProvider.SuGlobalTranslateLangQuery.FindTranslateLangByTranslateId(translateId);
                ctlTranslateLangGrid.DataBind();
                ctlSubmit.Visible = true;
                ctlFieldSetDetailGridView.Visible = true;
                ctlCancel.Visible = true;
                ctlGlobalTranslateGrid.DataCountAndBind();
                ctlUpdatePanelTranslateLangGridView.Update();
            }
        }
        protected void ctlGlobalTranslateGrid_DataBound(object sender, EventArgs e)
        {
            if (ctlGlobalTranslateGrid.Rows.Count > 0)
            {
                RegisterScriptForGridView();
                divButton.Visible = true;
                ctlDelete.OnClientClick = String.Format("return ConfirmDelete('{0}')", ctlGlobalTranslateGrid.ClientID);
            }
            else
            {

                divButton.Visible = false;
            }
        }
        protected void ctlGlobalTranslateGrid_PageIndexChanged(object sender, EventArgs e)
        {
            TranslateLangGridViewFinish();
        }
        protected void ctlTranslateLangGrid_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow row in ctlTranslateLangGrid.Rows)
            {
                TextBox translateWord = row.FindControl("ctlTranslateWord") as TextBox;
                TextBox comment = row.FindControl("ctlCommentTranslateLang") as TextBox;
                CheckBox active = row.FindControl("ctlActiveTranslateLang") as CheckBox;

                if ((string.IsNullOrEmpty(translateWord.Text)) && (string.IsNullOrEmpty(comment.Text)))
                {

                    active.Checked = true;
                }
            }
        }

        #endregion

        #region GlobalTranslate Formview
        protected void ctlTranslateForm_DataBound(object sender, EventArgs e)
        {
            TextBox ctlProgramCode = ctlTranslateForm.FindControl("ctlProgramCodeText") as TextBox;

            //DropDownList program = ctlTranslateForm.FindControl("ctlProgramList") as DropDownList;
            //program.DataSource = QueryProvider.SuProgramQuery.FindSuProgramByLanguageId(UserAccount.CurrentLanguageID);
            //program.DataTextField = "ProgramName";
            //program.DataValueField = "ProgramCode";
            //program.DataBind();
            //program.Items.Insert(0, new ListItem("--Please Select--", ""));
            if (ctlTranslateForm.CurrentMode != FormViewMode.ReadOnly)
            {
                TextBox ctlSymbol = ctlTranslateForm.FindControl("ctlSymbol") as TextBox;
                ctlProgramCode.Focus();
            }
            if (ctlTranslateForm.CurrentMode.Equals(FormViewMode.Edit))
            {
                SuGlobalTranslate translate = ctlTranslateForm.DataItem as SuGlobalTranslate;
                if (!string.IsNullOrEmpty(translate.ProgramCode))
                {
                    ctlProgramCode.Text = translate.ProgramCode;
                }
            }

        }
        protected void ctlTranslateForm_ModeChanging(object sender, FormViewModeEventArgs e)
        {

        }
        protected void ctlTranslateForm_Inserting(object sender, FormViewInsertEventArgs e)
        {
            SuGlobalTranslate globalTranslate = new SuGlobalTranslate();
            GetSuGlobalTranslateInfo(globalTranslate);

            try
            {
                SuGlobalTranslateService.AddGlobalTranslate(globalTranslate);
                ctlGlobalTranslateGrid.DataCountAndBind();
                ctlTranslateForm.ChangeMode(FormViewMode.ReadOnly);
                ClosePopUp();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        protected void ctlTranslateForm_Updating(object sender, FormViewUpdateEventArgs e)
        {
            long translateId = UIHelper.ParseLong(ctlTranslateForm.DataKey.Value.ToString());
            SuGlobalTranslate globalTranslate = new SuGlobalTranslate(translateId);
            GetSuGlobalTranslateInfo(globalTranslate);

            try
            {
                SuGlobalTranslateService.UpdateGlobalTranslate(globalTranslate);
                ctlGlobalTranslateGrid.DataCountAndBind();
                ClosePopUp();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }

        }
        protected void ctlTranslateForm_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                ctlGlobalTranslateGrid.DataCountAndBind();
                ClosePopUp();
            }
        }
        #endregion

        #region Button
        protected void ctlAddNew_Click(object sender, ImageClickEventArgs e)
        {
            ctlTranslateModalPopupExtender.Show();
            ctlUpdatePanelTranslateForm.Update();
            ctlGlobalTranslateGrid.DataCountAndBind();
            ctlTranslateForm.ChangeMode(FormViewMode.Insert);
        }
        protected void ctlDelete_Click(object sender, ImageClickEventArgs e)
        {
            foreach (GridViewRow row in ctlGlobalTranslateGrid.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
                {
                    try
                    {
                        long id = UIHelper.ParseLong(ctlGlobalTranslateGrid.DataKeys[row.RowIndex].Value.ToString());
                        SuGlobalTranslate translate = SuGlobalTranslateService.FindByIdentity(id);
                        SuGlobalTranslateService.Delete(translate);
                        //if (ctlGlobalTranslateGrid.SelectedIndex == row.RowIndex)
                        //{
                        //    TranslateLangGridViewFinish();
                        //}
                        //ctlGlobalTranslateGrid.DataCountAndBind();
                        //ctlUpdatePanelGridView.Update();
                    }
                    catch (Exception ex)
                    {
                        if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                                "alert('This data is now in use.');", true);
                            ctlGlobalTranslateGrid.DataCountAndBind();
                        }
                    }
                }
            }
            TranslateLangGridViewFinish();
            ctlGlobalTranslateGrid.DataCountAndBind();
            ctlUpdatePanelGridView.Update();
            //ctlGlobalTranslateGrid.DataBind();
        }
        protected void ctlSubmit_Click(object sender, EventArgs e)
        {
            IList<SuGlobalTranslateLang> list = new List<SuGlobalTranslateLang>();
            long translateId = UIHelper.ParseLong(ctlGlobalTranslateGrid.SelectedValue.ToString());
            SuGlobalTranslate translate = new SuGlobalTranslate(translateId);

            foreach (GridViewRow row in ctlTranslateLangGrid.Rows)
            {
                TextBox translateWord = row.FindControl("ctlTranslateWord") as TextBox;
                TextBox comment = row.FindControl("ctlCommentTranslateLang") as TextBox;
                CheckBox active = row.FindControl("ctlActiveTranslateLang") as CheckBox;

                if ((!string.IsNullOrEmpty(translateWord.Text)) || (!string.IsNullOrEmpty(comment.Text)))
                {

                    short languageId = UIHelper.ParseShort(ctlTranslateLangGrid.DataKeys[row.RowIndex].Values["LanguageId"].ToString());
                    DbLanguage lang = new DbLanguage(languageId);

                    SuGlobalTranslateLang translateLang = new SuGlobalTranslateLang();
                    translateLang.Translate = translate;
                    translateLang.Language = lang;
                    translateLang.TranslateWord = translateWord.Text;
                    translateLang.Comment = comment.Text;
                    translateLang.Active = active.Checked;

                    GetSuGlobalTranslateLangInfo(translateLang);
                    list.Add(translateLang);
                }

            }

            SuGlobalTranslateLangService.UpdateGlobalTranslateLang(list);
            ctlMessage.Message = GetMessage("SaveSuccessFully");
            //ctlUpdatePanelTranslateLangGridView.Update();
        }
        protected void ctlCancel_Click(object sender, EventArgs e)
        {
            TranslateLangGridViewFinish();
        }
        #endregion

        #region Function
        private void GetSuGlobalTranslateInfo(SuGlobalTranslate globalTranslate)
        {
            //DropDownList program = ctlTranslateForm.FindControl("ctlProgramList") as DropDownList;
            TextBox programCode = ctlTranslateForm.FindControl("ctlProgramCodeText") as TextBox;
            TextBox symbol = ctlTranslateForm.FindControl("ctlSymbol") as TextBox;
            TextBox comment = ctlTranslateForm.FindControl("ctlComment") as TextBox;
            CheckBox active = ctlTranslateForm.FindControl("ctlActive") as CheckBox;
            TextBox translateControl = ctlTranslateForm.FindControl("ctlTranslateControl") as TextBox;

            globalTranslate.TranslateSymbol = symbol.Text;
            globalTranslate.Comment = comment.Text;
            globalTranslate.Active = active.Checked;
            globalTranslate.TranslateControl = translateControl.Text;
            
            //SuProgram pro = QueryProvider.SuProgramQuery.FindByIdentity(UIHelper.ParseShort(program.SelectedValue));
            if (!string.IsNullOrEmpty(programCode.Text))
            {
                globalTranslate.ProgramCode = programCode.Text;
            }
            globalTranslate.CreBy = UserAccount.UserID;
            globalTranslate.CreDate = DateTime.Now.Date;
            globalTranslate.UpdBy = UserAccount.UserID;
            globalTranslate.UpdDate = DateTime.Now.Date;
            globalTranslate.UpdPgm = ProgramCode;
        }
        private void GetSuGlobalTranslateLangInfo(SuGlobalTranslateLang translateLang)
        {
            translateLang.CreBy = UserAccount.UserID;
            translateLang.CreDate = DateTime.Now.Date;
            translateLang.UpdBy = UserAccount.UserID;
            translateLang.UpdDate = DateTime.Now.Date;
            translateLang.UpdPgm = ProgramCode;
        }
        public void ClosePopUp()
        {
            ctlTranslateModalPopupExtender.Hide();
            ctlUpdatePanelGridView.Update();
        }
        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBox(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlGlobalTranslateGrid.ClientID + "', '" + ctlGlobalTranslateGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "validateChkBox", script.ToString(), true);
        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            short languageId = UserAccount.CurrentLanguageID;

            return QueryProvider.SuGlobalTranslateQuery.GetTranslatedList(GetTranslateCriteria(), languageId, startRow, pageSize, sortExpression);
        }
        public int RequestCount()
        {
            short languageId = UserAccount.CurrentLanguageID;

            return QueryProvider.SuGlobalTranslateQuery.GetCountTranslatedList(GetTranslateCriteria(), languageId);
        }
        public void TranslateLangGridViewFinish()
        {
            ctlGlobalTranslateGrid.SelectedIndex = -1;
            ctlTranslateLangGrid.DataSource = null;
            ctlTranslateLangGrid.DataBind();
            ctlUpdatePanelTranslateLangGridView.Update();
            ctlSubmit.Visible = false;
            ctlCancel.Visible = false;
            ctlFieldSetDetailGridView.Visible = false;
        }
        #endregion

        protected void ctlTranslateSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlGlobalTranslateGrid.DataCountAndBind();
            ctlUpdatePanelGridView.Update();
        }

        public SuGlobalTranslate GetTranslateCriteria()
        {
            SuGlobalTranslate criteria = new SuGlobalTranslate();
            criteria.ProgramCode = ctlProgramCode.Text;
            criteria.TranslateControl = ctlControlName.Text;
            criteria.TranslateSymbol = ctlSymbol.Text;

            return criteria;
        }
    }
}

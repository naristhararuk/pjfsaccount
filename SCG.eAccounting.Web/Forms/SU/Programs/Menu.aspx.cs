using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SS.SU.BLL;
using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.SU.Query;
using SCG.eAccounting.Web.Helper;
using System.Text;

using SS.DB.DTO;
using SS.DB.BLL;
using SS.DB.Query;
using SS.Standard.Utilities;

namespace SCG.eAccounting.Web.Forms.SU.Programs
{
    public partial class Menu : BasePage
    {
        public ISuMenuService SuMenuService { get; set; }
        public ISuMenuLangService SuMenuLangService { get; set; }
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            //ProgramCode = "Menu";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ctlMenuGrid.DataCountAndBind();
                fdsMenuLang.Visible = false;
            }
        }

        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            //return SuMenuService.FindBySuMenuCriteria(new SuMenu(), startRow, pageSize, sortExpression);

            short languageId = UserAccount.CurrentLanguageID;
            return QueryProvider.SuMenuQuery.GetTranslatedList(languageId, startRow, pageSize, sortExpression);
        }
        public int RequestCount()
        {
            //int count = SuMenuService.CountBySuMenuCriteria(new SuMenu());
            //return count;

            short languageId = UserAccount.CurrentLanguageID;
            return QueryProvider.SuMenuQuery.GetCountMenuList(languageId);
        }
        protected void ctlMenuGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            short menuId;
            if (e.CommandName == "MenuEdit")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                menuId = UIHelper.ParseShort(ctlMenuGrid.DataKeys[rowIndex].Value.ToString());
                ctlMenuForm.PageIndex = (ctlMenuGrid.PageIndex * ctlMenuGrid.PageSize) + rowIndex;
                ctlMenuForm.ChangeMode(FormViewMode.Edit);
                IList<SuMenu> list = new List<SuMenu>();
                list.Add(SuMenuService.FindByIdentity(menuId));

                ctlMenuForm.DataSource = list;
                ctlMenuForm.DataBind();
                ctlMenuGrid.DataCountAndBind();
                ctlUpdatePanelMenuForm.Update();
                ctlMenuModalPopupExtender.Show();

            }
            if (e.CommandName == "Select")
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                menuId = UIHelper.ParseShort(ctlMenuGrid.DataKeys[rowIndex].Value.ToString());
                ctlMenuLangGrid.DataSource = QueryProvider.SuMenuQuery.FindMenuLangByTranslateId(menuId);
                ctlMenuLangGrid.DataBind();
                fdsMenuLang.Visible = true;
                ctlMenuGrid.DataCountAndBind();
                ctlUpdatePanelMenuLangGridView.Update();
            }
        }
        protected void ctlMenuGrid_DataBound(object sender, EventArgs e)
        {
            if (ctlMenuGrid.Rows.Count > 0)
            {
                divButton.Visible = true;
                RegisterScriptForGridView();

                ctlDelete.OnClientClick = String.Format("return ConfirmDelete('{0}')", ctlMenuGrid.ClientID);
            }
            else
            {
                divButton.Visible = false;
            }
        }
        protected void ctlMenuForm_DataBound(object sender, EventArgs e)
        {
            initDropdownList();
            DropDownList ctlProgram = ctlMenuForm.FindControl("ctlProgramID") as DropDownList;
            if (ctlMenuForm.CurrentMode.Equals(FormViewMode.Edit))
            {
                SuMenu menu = ctlMenuForm.DataItem as SuMenu;
                if (menu.Program != null)
                {
                    ctlProgram.SelectedValue = menu.Program.Programid.ToString();
                }
            }

            DropDownList ctlMainMenuId = ctlMenuForm.FindControl("ctlMainMenuID") as DropDownList;
            if (ctlMenuForm.CurrentMode.Equals(FormViewMode.Edit))
            {
                SuMenu menu = ctlMenuForm.DataItem as SuMenu;
                if (!string.IsNullOrEmpty(menu.MenuMainid.ToString()))
                {
                    ctlMainMenuId.SelectedValue = menu.MenuMainid.ToString();
                }
            }
        }
        private void initDropdownList()
        {
            DropDownList ctlProgram = ctlMenuForm.FindControl("ctlProgramID") as DropDownList;
            ctlProgram.DataSource = QueryProvider.SuProgramQuery.FindAll();
            ctlProgram.DataTextField = "ProgramCode";
            ctlProgram.DataValueField = "ProgramId";
            ctlProgram.DataBind();
            ctlProgram.Items.Insert(0, new ListItem("--Please Select--", ""));

            DropDownList ctlMainMenuId = ctlMenuForm.FindControl("ctlMainMenuID") as DropDownList;
            ctlMainMenuId.DataSource = QueryProvider.SuMenuQuery.FindAll();
            ctlMainMenuId.DataTextField = "MenuCode";
            ctlMainMenuId.DataValueField = "MenuId";
            ctlMainMenuId.DataBind();
            ctlMainMenuId.Items.Insert(0, new ListItem("--Please Select--", ""));
        }
        protected void ctlMenuForm_ModeChanging(object sender, FormViewModeEventArgs e)
        {

        }
        protected void ctlMenuForm_Inserting(object sender, FormViewInsertEventArgs e)
        {
            SuMenu menu = new SuMenu();
            GetSuMenuInfo(menu);

            try
            {
                SuMenuService.AddMenu(menu);
                ctlMenuGrid.DataCountAndBind();
                ClosePopUp();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        protected void ctlMenuForm_Updating(object sender, FormViewUpdateEventArgs e)
        {
            short menuId = UIHelper.ParseShort(ctlMenuForm.DataKey.Value.ToString());
            SuMenu menu = QueryProvider.SuMenuQuery.FindProxyByIdentity(menuId);
            GetSuMenuInfo(menu);

            try
            {
                SuMenuService.UpdateMenu(menu);
                ctlMenuGrid.DataCountAndBind();

                ClosePopUp();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }

        }
        protected void ctlAddNew_Click(object sender, ImageClickEventArgs e)
        {
            ctlMenuModalPopupExtender.Show();
            ctlUpdatePanelMenuForm.Update();
            ctlMenuForm.ChangeMode(FormViewMode.Insert);
            ctlMenuGrid.DataCountAndBind();
            //initDropdownList();

            //TextBox ctlMenuCode = ctlMenuForm.FindControl("ctlMenuCode") as TextBox;
            //DropDownList ctlProgramID = ctlMenuForm.FindControl("ctlProgramID") as DropDownList;
            //DropDownList ctlMainMenuID = ctlMenuForm.FindControl("ctlMainMenuID") as DropDownList;
            //TextBox ctlMenuSeq = ctlMenuForm.FindControl("ctlMenuSeq") as TextBox;
            //TextBox ctlComment = ctlMenuForm.FindControl("ctlComment") as TextBox;
            //CheckBox ctlActive = ctlMenuForm.FindControl("ctlActive") as CheckBox;

            //ctlMenuCode.Text = "";
            //ctlMenuSeq.Text = "";
            //ctlComment.Text = "";
            //ctlActive.Checked = true;
            //ctlProgramID.SelectedIndex = 0;
            //ctlMainMenuID.SelectedIndex = 0;

           
        }
        protected void ctlDelete_Click(object sender, ImageClickEventArgs e)
        {
            foreach (GridViewRow row in ctlMenuGrid.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
                {
                    try
                    {
                        short menuId = UIHelper.ParseShort(ctlMenuGrid.DataKeys[row.RowIndex].Value.ToString());
                        SuMenu menu = SuMenuService.FindByIdentity(menuId);
                        SuMenuLangService.DeleteMenuLang(menuId);
                        SuMenuService.Delete(menu);
                        
                    }
                    catch (Exception ex)
                    {
                        Response.Write("Delete Error");
                    }
                }

            }
            ctlMenuGrid.SelectedIndex = -1;
            ctlMenuGrid.DataCountAndBind();
            ctlUpdatePanelGridView.Update();

            ctlMenuLangGrid.DataSource = null;
            ctlMenuLangGrid.DataBind();
            fdsMenuLang.Visible = false;
            ctlUpdatePanelMenuLangGridView.Update();

            
        }
        protected void ctlMenuLangGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowType == DataControlRowType.DataRow))
            {
                TextBox ctlMenuName = e.Row.FindControl("ctlMenuName") as TextBox;
                TextBox ctlCommentMenuLang = e.Row.FindControl("ctlCommentMenuLang") as TextBox;
                CheckBox ctlActive = e.Row.FindControl("ctlActiveMenuLang") as CheckBox;

                if (string.IsNullOrEmpty(ctlMenuName.Text.Trim()) && string.IsNullOrEmpty(ctlCommentMenuLang.Text.Trim()))
                {
                    ctlActive.Checked = true;
                }
            }
        }
        protected void ctlMenuForm_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                ctlMenuGrid.DataCountAndBind();
                ClosePopUp();
            }
        }

        protected void ctlSubmit_Click(object sender, EventArgs e)
        {
            IList<SuMenuLang> list = new List<SuMenuLang>();
            short menuId = UIHelper.ParseShort(ctlMenuGrid.SelectedValue.ToString());
            SuMenu menu = new SuMenu(menuId);

            foreach (GridViewRow row in ctlMenuLangGrid.Rows)
            {
                TextBox ctlMenuName = row.FindControl("ctlMenuName") as TextBox;
                TextBox ctlCommentMenuLang = row.FindControl("ctlCommentMenuLang") as TextBox;
                CheckBox ctlActiveMenuLang = row.FindControl("ctlActiveMenuLang") as CheckBox;

                //if ((!string.IsNullOrEmpty(ctlMenuName.Text)) || (!string.IsNullOrEmpty(ctlCommentMenuLang.Text)))
                //{

                    short languageId = UIHelper.ParseShort(ctlMenuLangGrid.DataKeys[row.RowIndex].Values["LanguageId"].ToString());
                    DbLanguage lang = new DbLanguage(languageId);

                    SuMenuLang menuLang = new SuMenuLang();
                    menuLang.Menu = menu;
                    menuLang.Language = lang;
                    menuLang.MenuName = ctlMenuName.Text;
                    menuLang.Comment = ctlCommentMenuLang.Text;
                    menuLang.Active = ctlActiveMenuLang.Checked;

                    GetSuMenuLangInfo(menuLang);
                    list.Add(menuLang);
                //}

            }

            SuMenuLangService.UpdateMenuLang(list);
            ctlMessage.Message = GetMessage("SaveSuccessFully");
            //ctlUpdatePanelTranslateLangGridView.Update();
        }

        private void GetSuMenuInfo(SuMenu menu)
        {
            //DropDownList program = ctlMenuForm.FindControl("ctlProgramList") as DropDownList;
            TextBox ctlMenuCode = ctlMenuForm.FindControl("ctlMenuCode") as TextBox;
            DropDownList ctlProgramID = ctlMenuForm.FindControl("ctlProgramID") as DropDownList;
            DropDownList ctlMainMenuID = ctlMenuForm.FindControl("ctlMainMenuID") as DropDownList;
            TextBox ctlMenuSeq = ctlMenuForm.FindControl("ctlMenuSeq") as TextBox;
            TextBox ctlComment = ctlMenuForm.FindControl("ctlComment") as TextBox;
            CheckBox ctlActive = ctlMenuForm.FindControl("ctlActive") as CheckBox;

            if (!string.IsNullOrEmpty(ctlProgramID.SelectedValue))
            {
                short programId = UIHelper.ParseShort(ctlProgramID.SelectedValue);
                //modify by tom 28/01/2009
                //menu.Program = SS.SU.Query.QueryProvider.SuProgramQuery.FindProxyByIdentity(programId);
                menu.Program = QueryProvider.SuProgramQuery.FindProxyByIdentity(programId);
            }
            //else
            //{
            //    menu.Programid = 0;
            //}

            if (!string.IsNullOrEmpty(ctlMainMenuID.SelectedValue))
            {
                menu.MenuMainid = UIHelper.ParseShort(ctlMainMenuID.SelectedValue);
            }
            else
            {
                menu.MenuMainid = null;
            }

            menu.MenuCode = ctlMenuCode.Text;
            menu.MenuLevel = QueryProvider.SuMenuQuery.GetMenuLevel(menu.MenuMainid,menu.Menuid);
            menu.MenuSeq = UIHelper.ParseShort(ctlMenuSeq.Text);
            menu.Comment = ctlComment.Text;
            menu.Active = ctlActive.Checked;
            menu.CreBy = UserAccount.UserID;
            menu.CreDate = DateTime.Now.Date;
            menu.UpdBy = UserAccount.UserID;
            menu.UpdDate = DateTime.Now.Date;
            menu.UpdPgm = ProgramCode;
        }

        private void GetSuMenuLangInfo(SuMenuLang menuLang)
        {
            menuLang.CreBy = UserAccount.UserID;
            menuLang.CreDate = DateTime.Now.Date;
            menuLang.UpdBy = UserAccount.UserID;
            menuLang.UpdDate = DateTime.Now.Date;
            menuLang.UpdPgm = ProgramCode;
        }
        public void ClosePopUp()
        {
            ctlMenuForm.ChangeMode(FormViewMode.ReadOnly);
            ctlMenuModalPopupExtender.Hide();
            ctlUpdatePanelGridView.Update();
        }
        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBox(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlMenuGrid.ClientID + "', '" + ctlMenuGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "validateChkBox", script.ToString(), true);
        }

        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            ctlMenuLangGrid.DataSource = null;
            ctlMenuLangGrid.DataBind();
            ctlUpdatePanelMenuLangGridView.Update();
            fdsMenuLang.Visible = false;

            ctlMenuGrid.SelectedIndex = -1;
            ctlMenuGrid.DataCountAndBind();
            ctlUpdatePanelGridView.Update();

            ctlMenuForm.ChangeMode(FormViewMode.ReadOnly);
        }
    }
}

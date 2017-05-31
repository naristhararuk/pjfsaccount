using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SS.SU.DTO;
using SCG.eAccounting.Web.Helper;
using SS.SU.BLL;
using SS.SU.Query;
using System.Text;
using System.Web.UI.HtmlControls;

using SS.DB.Query;
using SS.DB.DTO;
using SS.DB.BLL;
using SS.Standard.Utilities;

namespace SCG.eAccounting.Web.Forms.SU.Programs
{
    public partial class Division : BasePage
    {
        #region Properties
        public ISuOrganizationService SuOrganizationService { get; set; }
		public IDbLanguageService DbLanguageService { get; set; }
        public ISuDivisionService SuDivisionService { get; set; }
        public ISuDivisionLangService SuDivisionLangService { get; set; }
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

            if (!Page.IsPostBack)
            {
                //ctlDivisionGrid.DataCountAndBind();
                //ctlSubmit.Visible = false;
            }
        }
        #endregion

        #region LoadEvent
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            ProgramCode = "Division";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ctlOrganizationList.DataSource = QueryProvider.SuOrganizationLangQuery.FindByLanguageId(UserAccount.CurrentLanguageID);
                ctlOrganizationList.DataTextField = "OrganizationName";
                ctlOrganizationList.DataValueField = "OrganizationID";
                ctlOrganizationList.DataBind();
                ctlOrganizationList.Items.Insert(0, new ListItem("Please Select", "-1"));
            }
        }
        #endregion

        #region Button Event
        protected void ctlSearchButton_Click(object sender, EventArgs e)
        {
            ctlDivisionGrid.DataCountAndBind();
            divButton.Visible = true;
        }
        protected void ctlSubmit_Click(object sender, EventArgs e)
        {
            IList<SuDivisionLang> list = new List<SuDivisionLang>();
            short divisionId = UIHelper.ParseShort(ctlDivisionGrid.SelectedValue.ToString());
            SuDivision div = new SuDivision(divisionId);

            foreach (GridViewRow row in ctlDivisionLangGrid.Rows)
            {
                TextBox divisionName = row.FindControl("ctlDivisionName") as TextBox;
                TextBox comment = row.FindControl("ctlCommentDivisionLang") as TextBox;
                CheckBox active = row.FindControl("ctlActiveDivisionLang") as CheckBox;

                if ((!string.IsNullOrEmpty(divisionName.Text)) || (!string.IsNullOrEmpty(comment.Text)))
                {

                    short languageId = UIHelper.ParseShort(ctlDivisionLangGrid.DataKeys[row.RowIndex].Values["LanguageId"].ToString());
                    DbLanguage lang = new DbLanguage(languageId);

                    SuDivisionLang divisionLang = new SuDivisionLang();
                    divisionLang.Division = div;
                    divisionLang.Language = lang;
                    divisionLang.DivisionName = divisionName.Text;
                    divisionLang.Comment = comment.Text;
                    divisionLang.Active = active.Checked;

                    GetSuDivisionLangInfo(divisionLang);
                    list.Add(divisionLang);
                }

            }
            SuDivisionLangService.UpdateDivisionLang(list);
            ctlMessage.Message = GetMessage("SaveSuccessFully");
			//ctlDivisionLangGrid.DataSource = null;
			//ctlDivisionLangGrid.DataBind();
			//ctlSubmit.Visible = false;

			//ctlDivisionGrid.SelectedIndex = -1;    
            ctlDivisionGrid.DataCountAndBind();
            ctlUpdatePanelGridView.Update();
        }
        protected void ctlDelete_Click(object sender, ImageClickEventArgs e)
        {
            foreach (GridViewRow row in ctlDivisionGrid.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
                {
                    try
                    {
                        //ctlGlobalTranslateGrid.DeleteRow(row.RowIndex);
                        short id = UIHelper.ParseShort(ctlDivisionGrid.DataKeys[row.RowIndex].Value.ToString());
                        SuDivision div = SuDivisionService.FindByIdentity(id);
                        SuDivisionService.Delete(div);
                        //if(ctlDivisionGrid.SelectedIndex == row.RowIndex)
                        //{
                        //    DivisionLangGridViewFinish();
                        //}
                        //ctlDivisionGrid.DataCountAndBind();
                        //ctlUpdatePanelGridView.Update();
                    }
                    catch (Exception ex)
                    {
                        if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                                "alert('This data is now in use.');", true);
                        }
                        ctlDivisionGrid.DataCountAndBind();
                    }
                }
            }
            DivisionLangGridViewFinish();
            ctlDivisionGrid.DataCountAndBind();
            ctlUpdatePanelGridView.Update();
            //ctlDivisionGrid.DataBind();
        }
        protected void ctlAddNew_Click(object sender, ImageClickEventArgs e)
        {
            ctlDivisionModalPopupExtender.Show();
            ctlDivisionGrid.DataCountAndBind();
            ctlUpdatePanelDivisionForm.Update();
            ctlDivisionForm.ChangeMode(FormViewMode.Insert);
        }
        protected void ctlDetailCancel_Click(object sender, ImageClickEventArgs e)
        {
            DivisionLangGridViewFinish();
        }
        #endregion

        #region DivisionGridEvent
        protected void ctlDivisionGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            short divisionId;
            if (e.CommandName == "DivisionEdit")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                divisionId = UIHelper.ParseShort(ctlDivisionGrid.DataKeys[rowIndex].Value.ToString());
                
                ctlDivisionGrid.EditIndex = rowIndex;
                ctlDivisionForm.PageIndex = (ctlDivisionGrid.PageIndex * ctlDivisionGrid.PageSize) + rowIndex;
                ctlDivisionForm.ChangeMode(FormViewMode.Edit);
                IList<SuDivision> list = new List<SuDivision>();
                list.Add(SuDivisionService.FindByIdentity(divisionId));
                ctlDivisionForm.DataSource = list;
                ctlDivisionForm.DataBind();
                ctlDivisionGrid.DataCountAndBind();
                ctlUpdatePanelDivisionForm.Update();
                ctlDivisionModalPopupExtender.Show();

            }
            if (e.CommandName == "Select")
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                divisionId = UIHelper.ParseShort(ctlDivisionGrid.DataKeys[rowIndex].Value.ToString());
                ctlDivisionLangGrid.DataSource = QueryProvider.SuDivisionLangQuery.FindDivisionLangByDivisionId(divisionId);
                ctlDivisionLangGrid.DataBind();
                ctlSubmit.Visible = true;
                ctlFieldSetDetailGridView.Visible = true;
                ctlDetailCancel.Visible = true;
                ctlDivisionGrid.DataCountAndBind();
                ctlUpdatePanelDivisionLangGridView.Update();
            }
        }
        protected void ctlDivisionGrid_DataBound(object sender, EventArgs e)
        {
            if (ctlDivisionGrid.Rows.Count > 0)
            {
                RegisterScriptForGridView();
                divButton.Visible = true;
                ctlDelete.OnClientClick = String.Format("return ConfirmDelete('{0}')", ctlDivisionGrid.ClientID);
            }
            else
            {
                divButton.Visible = false;
            }
        }

        protected void ctlDivisionLangGrid_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow row in ctlDivisionLangGrid.Rows)
            {
                TextBox divisionName = row.FindControl("ctlDivisionName") as TextBox;
                TextBox comment = row.FindControl("ctlCommentDivisionLang") as TextBox;
                CheckBox active = row.FindControl("ctlActiveDivisionLang") as CheckBox;

                if ((string.IsNullOrEmpty(divisionName.Text)) && (string.IsNullOrEmpty(comment.Text)))
                {
                    active.Checked = true;
                }

            }
        }
        protected void ctlDivisionGrid_PageIndexChanged(object sender, EventArgs e)
        {
            DivisionLangGridViewFinish();
        }

        #endregion

        #region DivisionFormEvent
        protected void ctlDivisionForm_DataBound(object sender, EventArgs e)
        {
            DropDownList org = ctlDivisionForm.FindControl("ctlOrganizeList") as DropDownList;
            org.DataSource = QueryProvider.SuOrganizationLangQuery.FindByLanguageId(UIHelper.ParseShort("1"));
            org.DataTextField = "OrganizationName";
            org.DataValueField = "OrganizationID";
            org.DataBind();
            org.Items.Insert(0, new ListItem("Please Select", "-1"));

            if (ctlDivisionForm.CurrentMode == FormViewMode.Insert)
            {
                TextBox ctlDivisionName = ctlDivisionForm.FindControl("ctlDivisionName") as TextBox;
                Label ctlOrganizerLabel = ctlDivisionForm.FindControl("ctlOrganizerLabel") as Label;
                ctlDivisionName.Focus();
                org.Visible = false;
                ctlOrganizerLabel.Text = ctlOrganizationList.SelectedItem.Text;
            }

            if (ctlDivisionForm.CurrentMode.Equals(FormViewMode.Edit))
            {
                TextBox ctlComment = ctlDivisionForm.FindControl("ctlComment") as TextBox;
                ctlComment.Focus();

				LinkButton grdDivisionName = ctlDivisionGrid.Rows[ctlDivisionGrid.EditIndex].FindControl("ctlDivisionName") as LinkButton;
				Label frmDivisionName = ctlDivisionForm.FindControl("ctlDivisionName") as Label;
				
				frmDivisionName.Text = grdDivisionName.Text;
                SuDivision div = ctlDivisionForm.DataItem as SuDivision;
                if (div.Organization != null)
                {
                    org.SelectedValue = div.Organization.Organizationid.ToString();
                }
            }
        }
        protected void ctlDivisionForm_ModeChanging(object sender, FormViewModeEventArgs e)
        {

        }
        protected void ctlDivisionForm_Inserting(object sender, FormViewInsertEventArgs e)
        {
            SuDivision div = new SuDivision();
            SuDivisionLang divLang = new SuDivisionLang();
            GetSuDivisionInfo(div);
            
            short languageId = UserAccount.CurrentLanguageID;
            TextBox ctlDivisionName =ctlDivisionForm.FindControl("ctlDivisionName") as TextBox;
            divLang.DivisionName = ctlDivisionName.Text;
            divLang.Division = div;
            divLang.Language = DbLanguageService.FindByIdentity(languageId);
            divLang.Active = div.Active;
            divLang.CreBy = UserAccount.UserID;
            divLang.CreDate = DateTime.Now;
            divLang.UpdPgm = ProgramCode;
            divLang.UpdBy = UserAccount.UserID;
            divLang.UpdDate = DateTime.Now;

            try
            {
                SuDivisionService.AddDivision(div, divLang);
                ctlDivisionGrid.DataCountAndBind();
                ctlDivisionForm.ChangeMode(FormViewMode.ReadOnly);
                ClosePopUp();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        protected void ctlDivisionForm_Updating(object sender, FormViewUpdateEventArgs e)
        {
            short divisionId = UIHelper.ParseShort(ctlDivisionForm.DataKey.Value.ToString());
            SuDivision div = new SuDivision(divisionId);
            GetSuDivisionInfo(div);

            try
            {
                SuDivisionService.UpdateDivision(div);
                ctlDivisionGrid.DataCountAndBind();
                ClosePopUp();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }

        }
        protected void ctlDivisionForm_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                ctlDivisionGrid.DataCountAndBind();
                ClosePopUp();
            }
        }
        #endregion

        #region PrivateFunction
        private void GetSuDivisionInfo(SuDivision div)
        {
            DropDownList org = ctlDivisionForm.FindControl("ctlOrganizeList") as DropDownList;
            TextBox comment = ctlDivisionForm.FindControl("ctlComment") as TextBox;
            CheckBox active = ctlDivisionForm.FindControl("ctlActive") as CheckBox;

            div.Comment = comment.Text;
            div.Active = active.Checked;
            if (ctlDivisionForm.CurrentMode.Equals(FormViewMode.Insert))
            {
                SuOrganization organize = new SuOrganization(UIHelper.ParseShort(ctlOrganizationList.SelectedValue));
                div.Organization = organize;
            }
            else
            {
                if (UIHelper.ParseShort(org.SelectedValue) != -1)
                {
                    SuOrganization organize = new SuOrganization(UIHelper.ParseShort(org.SelectedValue));
                    div.Organization = organize;
                }
            }
            div.CreBy = UserAccount.UserID;
            div.CreDate = DateTime.Now.Date;
            div.UpdBy = UserAccount.UserID;
            div.UpdDate = DateTime.Now.Date;
            div.UpdPgm = ProgramCode;
        }
        private void GetSuDivisionLangInfo(SuDivisionLang divisionLang)
        {
            divisionLang.CreBy = UserAccount.UserID;
            divisionLang.CreDate = DateTime.Now.Date;
            divisionLang.UpdBy = UserAccount.UserID;
            divisionLang.UpdDate = DateTime.Now.Date;
            divisionLang.UpdPgm = ProgramCode;
        }
        public SuOrganization GetOrganization()
        {
            SuOrganization org = new SuOrganization();
            if (ctlOrganizationList.SelectedIndex > 0)
            {
                org.Organizationid = UIHelper.ParseShort(ctlOrganizationList.SelectedValue);
            }
            return org;
        }
        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBox(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlDivisionGrid.ClientID + "', '" + ctlDivisionGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "validateChkBox", script.ToString(), true);
        }
        public void ClosePopUp()
        {
            ctlDivisionForm.DataSource = null;
            ctlDivisionModalPopupExtender.Hide();
            ctlUpdatePanelGridView.Update();
        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            short languageId = UserAccount.CurrentLanguageID;
            short organizationId = UIHelper.ParseShort(ctlOrganizationList.SelectedValue);

            if (organizationId == -1)
            {
                return null;
            }
            else
            {
                return QueryProvider.SuDivisionQuery.GetDivisionList(languageId, organizationId, startRow, pageSize, sortExpression);
            }

            //return SuDivisionService.FindDivisionByOrganization(GetOrganization(), startRow, pageSize, sortExpression);
        }
        public int RequestCount()
        {
            //int count = SuDivisionService.CountByOrganizationCriteria(GetOrganization());
            //return count;

            short languageId = UserAccount.CurrentLanguageID;
            short organizationId = UIHelper.ParseShort(ctlOrganizationList.SelectedValue);

            if (organizationId == -1)
            {
                return 0;
            }
            else
            {
                return QueryProvider.SuDivisionQuery.GetCountDivisionList(languageId, organizationId);
            }
        }

        public void DivisionLangGridViewFinish()
        {
            ctlDivisionGrid.SelectedIndex = -1;
            ctlDivisionLangGrid.DataSource = null;
            ctlDivisionLangGrid.DataBind();
            ctlUpdatePanelDivisionLangGridView.Update();
            ctlSubmit.Visible = false;
            ctlDetailCancel.Visible = false;
            ctlFieldSetDetailGridView.Visible = false;
        }
        #endregion
    }
}

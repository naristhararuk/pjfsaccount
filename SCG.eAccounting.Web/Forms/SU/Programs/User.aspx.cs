using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using SS.Standard.UI;
//using SS.Standard.Security;

using SS.SU.DTO;
using SS.SU.BLL;
using SS.SU.Query;

using SCG.eAccounting.Web.Helper;
using System.Globalization;

using SS.DB.Query;
using SS.DB.DTO;
using SS.DB.BLL;

namespace SCG.eAccounting.Web.Forms.SU.Programs
{
    public partial class User : BasePage
    {
        List<SuUser> UserList;
        public string HiddenTag = "style=\"display:none\"";

        public ISuUserService SuUserService { get; set; }
        public ISuUserQuery SuUserQuery { get; set; }
        public ISuOrganizationQuery SuOrganizationQuery { get; set; }
        public IDbLanguageQuery DbLanguageQuery { get; set; }
		public ISuDivisionQuery SuDivisionQuery { get; set; }

		#region Load Event
        protected override void OnPreLoad(EventArgs e)
        {
           base.OnPreLoad(e);
           ProgramCode = "SURT17";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //UserAccount.CURRENT_LanguageID 
            this.CanAdd = true;
            this.CanDelete = true;
            this.CanView = true;
            this.CanEdit = true;

            //if (!IsPostBack)
            //{
            //    BindOrganization();
            //    BindLanguage();
            //    BindDivision();
            //}
		}
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            ctlDelete.Enabled = this.CanDelete;
            ctlDelete.Visible = this.ctlDelete.Enabled;

            ctlAddNew.Enabled = this.CanAdd;
            ctlAddNew.Visible = this.ctlAddNew.Enabled;

            ctlDelete.Enabled = this.CanDelete;
            ctlDelete.Visible = this.ctlDelete.Enabled;
        }
        private void SessionUser()
        {
            if (Session["userList"] == null)
            {
                UserList = SuUserDataSource.Select() as List<SuUser>;
                Session["userList"] = UIHelper.Serialization<List<SuUser>>(UserList);
            }
            else
            {
                UserList = UIHelper.DeSerialization<List<SuUser>>(Session["userList"]);
                Session["userList"] = UIHelper.Serialization<List<SuUser>>(UserList);
            }
        }
        #endregion

		#region BindData Function (Not in use now)
        private void BindDivision()
        {
            /*bllDiv DivisionEntity = new bllDiv();
            List<SU_DIVISION> DivisionList = DivisionEntity.ListDiv();
            ddlDivID.DataSource = DivisionList;
            ddlDivID.DataBind();
             */
            //ddlDivID.Items.Insert(0, new ListItem(lblSelectDivision.Text, "0"));
        }
        private void BindOrganization()
        {
            if (ctlUserForm.CurrentMode != FormViewMode.ReadOnly)
            {
                DropDownList ctlOrganization = ctlUserForm.FindControl("ctlOrganization") as DropDownList;
                ctlOrganization.DataSource = SuOrganizationQuery.FindAll();
                ctlOrganization.DataTextField = "SuOrganizationLang.OrganizationName";
                ctlOrganization.DataValueField = "OrganizationID";
                //ctlOrganization.DataBind();
            }
            //bllOrganization OrganizationEntity = new bllOrganization();
            //List<SU_ORGANIZATION> OrganizationList = OrganizationEntity.ListOrg();
            //ddlOrganization.DataSource = OrganizationList;
            //ddlOrganization.DataBind();
            //ddlOrganization.Items.Insert(0, new ListItem(lblSelectOrganization.Text, "0"));
        }
        public void BindLanguage()
        {
            if (ctlUserForm.CurrentMode != FormViewMode.ReadOnly)
            {
                DropDownList ctlLanguage = ctlUserForm.FindControl("ctlLanguage") as DropDownList;
                ctlLanguage.DataSource = DbLanguageQuery.FindAll();
                ctlLanguage.DataTextField = "LanguageID";
                ctlLanguage.DataValueField = "LanguageName";
                //ctlLanguage.DataBind();
            }
            //bllLang LanguageEntity = new bllLang();
            //List<SU_LANG> LanguageList = LanguageEntity.ListLang(true);
            //ddlLanguage.DataSource = LanguageList;
            //ddlLanguage.DataBind();
            //ddlLanguage.Items.Insert(0, new ListItem(lblSelectLanguage.Text, "0"));
        }
        #endregion

		#region GridView Event
        protected void ctlUserGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UserEdit")
            {
				int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
				long userId = UIHelper.ParseLong(ctlUserGrid.DataKeys[rowIndex].Value.ToString());

				IList<SuUser> userList = new List<SuUser>();
				SuUser user = SuUserService.FindByIdentity(userId);
				userList.Add(user);

				ctlUserForm.DataSource = userList;
				ctlUserForm.PageIndex = 0;
				
				ctlUserForm.ChangeMode(FormViewMode.Edit);
				ctlUserForm.DataBind();
				
				UpdatePanelUserForm.Update();
				ctlUserModalPopupExtender.Show();
            }
        }
        protected void ctlUserGrid_DataBound(object sender, EventArgs e)
        {
            if (ctlUserGrid.Rows.Count > 0)
            {
                divButton.Visible = true;
                RegisterScriptForGridView();

				ctlDelete.OnClientClick = String.Format("return ConfirmDelete('{0}')", ctlUserGrid.ClientID);
			}
			else
			{
				divButton.Visible = false;
			}
        }
        #endregion
        
        #region FormView Event
        protected void ctlUserForm_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            ctlUserModalPopupExtender.Hide();
            UpdatePanelGridView.Update();
        }
		protected void ctlUserForm_DataBound(object sender, EventArgs e)
		{
			//BindLanguage();
			//BindOrganization();
		}
		protected void ctlUserForm_ItemInserting(object sender, FormViewInsertEventArgs e)
		{
			SuUser user = new SuUser();
			user = GetSuUserInfo(user);

			SuUserService.Save(user);

			// Cancel insert with DataSource.
			e.Cancel = true;
			ctlUserGrid.DataBind();
		}
		protected void ctlUserForm_ItemUpdating(object sender, FormViewUpdateEventArgs e)
		{
			long userId = UIHelper.ParseLong(ctlUserForm.DataKey["UserId"].ToString());
			SuUser user = SuUserService.FindByIdentity(userId);

			user = GetSuUserInfo(user);

			SuUserService.SaveOrUpdate(user);

			// Cancel insert with DataSource.
			e.Cancel = true;
			ctlUserGrid.DataBind();
		}
		protected void ctlUserForm_ModeChanging(object sender, FormViewModeEventArgs e)
		{

		}
		#endregion

		#region Object DataSource Inserting, Updating Event (Not in use now)
        protected void SuUserDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            SuUser user = e.InputParameters[0] as SuUser;
            GetSuUserInfo(user);
        }
        protected void SuUserDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            SuUser user = e.InputParameters[0] as SuUser;
            
            //modify by tom 28/01/2009
            //SCG.eAccounting.Web.UserControls.Calendar ctlEffDate = ctlUserForm.FindControl("ctlEffDate") as SCG.eAccounting.Web.UserControls.Calendar;
            //SCG.eAccounting.Web.UserControls.Calendar ctlEndDate = ctlUserForm.FindControl("ctlEndDate") as SCG.eAccounting.Web.UserControls.Calendar;
            UserControls.Calendar ctlEffDate = ctlUserForm.FindControl("ctlEffDate") as UserControls.Calendar;
            UserControls.Calendar ctlEndDate = ctlUserForm.FindControl("ctlEndDate") as UserControls.Calendar;
            //user.EffDate = UIHelper.ParseDate(ctlEffDate.DateValue, Helper.Constant.DateFormat,System.Globalization.CultureInfo.CurrentCulture).Value;//, new System.Globalization.CultureInfo("th-TH"));
            //user.EndDate = UIHelper.ParseDate(ctlEndDate.DateValue, Helper.Constant.DateFormat, System.Globalization.CultureInfo.CurrentCulture).Value;//DateTime.Parse(ctlEndDate.DateValue, new System.Globalization.CultureInfo("th-TH"));
			//user.EffDate = UIHelper.ParseDate(ctlEffDate.DateValue, Helper.Constant.DateFormat).Value;
			//user.EndDate = UIHelper.ParseDate(ctlEndDate.DateValue, Helper.Constant.DateFormat).Value;
            
            GetSuUserInfo(user);
        }
        #endregion

		#region Button Event
        protected void ctlAddNew_Click(object sender, ImageClickEventArgs e)
        {
            ctlUserModalPopupExtender.Show();
            UpdatePanelUserForm.Update();
            ctlUserForm.ChangeMode(FormViewMode.Insert);
        }
        protected void ctlDelete_Click(object sender, EventArgs e)
        {
			foreach (GridViewRow row in ctlUserGrid.Rows)
			{
				if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
				{
					try
					{
						long userId = UIHelper.ParseLong(ctlUserGrid.DataKeys[row.RowIndex].Value.ToString());
						SuUser user = SuUserService.FindProxyByIdentity(userId);
						
						SuUserService.Delete(user);
					}
					catch (Exception ex)
					{
						if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
						{
							ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
								"alert('This data is now in use.');", true);
						}
					}
				}
			}
			
			// Bind Grid After Delete User Successful.
			ctlUserGrid.DataBind();
		}
        #endregion

		#region Private Function
		private SuUser GetSuUserInfo(SuUser user)
		{
			TextBox ctlUserName = ctlUserForm.FindControl("ctlUserName") as TextBox;
			TextBox ctlPassword = ctlUserForm.FindControl("ctlPassword") as TextBox;
			TextBox ctlFailTime = ctlUserForm.FindControl("ctlFailTime") as TextBox;
			TextBox ctlComment = ctlUserForm.FindControl("ctlComment") as TextBox;
			DropDownList ctlOrganization = ctlUserForm.FindControl("ctlOrganization") as DropDownList;
			DropDownList ctlDivision = ctlUserForm.FindControl("ctlDivision") as DropDownList;
			DropDownList ctlLanguage = ctlUserForm.FindControl("ctlLanguage") as DropDownList;
			UserControls.Calendar ctlEffDate = ctlUserForm.FindControl("ctlEffDate") as UserControls.Calendar;
			UserControls.Calendar ctlEndDate = ctlUserForm.FindControl("ctlEndDate") as UserControls.Calendar;
			CheckBox ctlChangePassword = ctlUserForm.FindControl("ctlChangePassword") as CheckBox;
			CheckBox chkActive = ctlUserForm.FindControl("chkActive") as CheckBox;
			
			user.UserName = ctlUserName.Text;
			user.Password = "";
			//user.Password = UserEngine.Md5Hash(user.Password);
			//user.EffDate = UIHelper.ParseDate(ctlEffDate.DateValue, Helper.Constant.DateFormat).Value;
			//user.EndDate = UIHelper.ParseDate(ctlEndDate.DateValue, Helper.Constant.DateFormat).Value;
			user.FailTime = UIHelper.ParseShort(ctlFailTime.Text);
			user.SetFailTime = 0;
			user.Comment = ctlComment.Text;
			user.ChangePassword = ctlChangePassword.Checked;
			user.Active = chkActive.Checked;
			//user.Organization = SuOrganizationQuery.FindByIdentity(UIHelper.ParseShort(ctlOrganization.SelectedValue));
			//user.Division = SuDivisionQuery.FindByIdentity(UIHelper.ParseShort(ctlDivision.SelectedValue));
			user.Language = DbLanguageQuery.FindByIdentity(UIHelper.ParseShort(ctlLanguage.SelectedValue));

            user.CreBy = UserAccount.UserID;//null;//UserAccount.UserID;
			user.CreDate = DateTime.Now.Date;
            user.UpdBy = UserAccount.UserID;//"";  //UserAccount.UserID;
			user.UpdDate = DateTime.Now.Date;
			user.UpdPgm = ProgramCode;
			
			return user;
		}
		private void RegisterScriptForGridView()
		{
			StringBuilder script = new StringBuilder();
			script.Append("function validateCheckBox(objChk, objFlag) ");
			script.Append("{ ");
			script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
			script.Append("'" + ctlUserGrid.ClientID + "', '" + ctlUserGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
			script.Append("} ");

			ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "validateChkBox", script.ToString(), true);
		}
		#endregion

		#region Object DataSource Creating Event.
        protected void SuOrganizationDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = QueryProvider.SuOrganizationQuery;
        }
        protected void DbLanguageDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = SsDbQueryProvider.DbLanguageQuery;
        }
		protected void SuDivisionDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
		{
			e.ObjectInstance = QueryProvider.SuDivisionQuery;
		}
		protected void SuUserDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
		{
			e.ObjectInstance = QueryProvider.SuUserQuery;
		}
		#endregion

		#region Object DataSource Selecting Event.
        protected void SuOrganizationDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
			e.InputParameters["languageID"] =  UIHelper.ParseShort("1");
			//e.InputParameters["languageID"] = UIHelper.ParseShort(SS.Standard.Security.UserAccount.CURRENT_LanguageID.Value.ToString());
        }
		protected void SuUserDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
		{
			e.InputParameters["languageID"] = UIHelper.ParseShort("1");
			//e.InputParameters["languageID"] = UIHelper.ParseShort(SS.Standard.Security.UserAccount.CURRENT_LanguageID.Value.ToString());
		}
		protected void SuDivisionDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
		{
			e.InputParameters["languageID"] = UIHelper.ParseShort("1");
			//e.InputParameters["languageID"] = UIHelper.ParseShort(SS.Standard.Security.UserAccount.CURRENT_LanguageID.Value.ToString());
		}
		#endregion

        protected void Button1_Click(object sender, EventArgs e)
        {
            this.UserCulture = new CultureInfo("th");
            Label1.Text = new Spring.Validation.ErrorMessage("TestText").GetMessage(MessageSource);
            
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            this.UserCulture = new CultureInfo("en");
            Label1.Text = new Spring.Validation.ErrorMessage("TestText").GetMessage(MessageSource);
            
        }
    }
}

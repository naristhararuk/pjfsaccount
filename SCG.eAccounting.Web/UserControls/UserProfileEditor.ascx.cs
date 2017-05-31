using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SS.Standard.Utilities;
using SS.SU.DTO.ValueObject;
using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.BLL;
using SCG.DB.Query;
using SCG.DB.DTO;
using SS.DB.Query;
using SS.SU.DTO;
using SS.SU.BLL;
using SS.SU.Query;
using Spring.Validation;
using System.Text.RegularExpressions;


namespace SCG.eAccounting.Web.UserControls
{
    public partial class UserProfileEditor : BaseUserControl
    {
        public event EventHandler Notify_Ok;
        public event EventHandler Notify_Cancle;
        private SuUser user;

        #region Properties
        public ISuUserService SuUserService { get; set; }
        public ISuPasswordHistoryService SuPasswordHistoryService { get; set; }
        public ParameterServices ParameterServices { get; set; }
        public ISCGEmailService SCGEmailService { get; set; }
        public String ChMode
        {
            get { return ctlChangeMode.Value; }
            set { ctlChangeMode.Value = value; }
        }
        public long UserId
        {
            get
            {
                if (!string.IsNullOrEmpty(ctlIdEdit.Value))
                    return UIHelper.ParseLong(ctlIdEdit.Value);
                return 0;
            }
            set { ctlIdEdit.Value = value.ToString(); }
        }
        public string Mode
        {
            get { return this.ViewState["Mode"] == null ? FlagEnum.NewFlag : (string)this.ViewState["Mode"]; }
            set { this.ViewState["Mode"] = value; }
        }
        public long UserID
        {
            get { return this.ViewState["UserID"] == null ? (long)0 : (long)this.ViewState["UserID"]; }
            set { this.ViewState["UserID"] = value; }
        }
        public long CompanyID
        {
            get { return this.ViewState["CompanyID"] == null ? (long)0 : (long)this.ViewState["CompanyID"]; }
            set { this.ViewState["CompanyID"] = value; }
        }
        public bool IsAdmin
        {
            get { return this.ViewState["isAdmin"] == null ? false : (bool)this.ViewState["isAdmin"]; }
            set { this.ViewState["isAdmin"] = value; }
        }
        public int Height
        {
            set { ctlPanelScroll.Height = value; }
        }

        public bool ShowScrollBar
        {
            set
            {
                if (value)
                {
                    ctlPanelScroll.Height = Unit.Pixel(500);
                    ctlPanelScroll.ScrollBars = ScrollBars.Auto;
                }
                else
                {
                    ctlPanelScroll.Height = Unit.Percentage(100);
                    ctlPanelScroll.ScrollBars = ScrollBars.None;
                }
            }
        }
        #endregion

        #region on load event
        protected void ctlCompanyField_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            if (e.ObjectReturn != null)
            {
                DbCompany company = (DbCompany)e.ObjectReturn;
                ctlCompanyField.CompanyID = company.CompanyID.ToString();
                ctlCostCenterField.ResetValue();
                ctlLocationField.ResetValue();
                ctlCostCenterField.CompanyId = company.CompanyID;
                ctlLocationField.CompanyId = company.CompanyID;
                CompanyID = company.CompanyID;

                ctlCostCenterField.ReadOnly = false;
                ctlLocationField.ReadOnly = false;
                ctlCostCenterField.CheckForReadOnly();
                ctlLocationField.CheckForReadOnly();
                ctlLocationField.DisableCompany = true;
                ctlUpdatePanelUserProfileForm.Update();
            }
            //string aaa = ctlUserPassword.Text;
        }
        protected void ctlCostCenterField_OnObjectLookUpCalling(object sender, ObjectLookUpCallingEventArgs e)
        {

        }
        protected void ctlLocationField_OnObjectLookUpCalling(object sender, ObjectLookUpCallingEventArgs e)
        {

        }

        //protected void ctlLocationField_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        //{
        //}
        //protected void ctlCostCenterField_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        //{
        //}


        protected void Page_Load(object sender, EventArgs e)
        {
            ctlCompanyField.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlCompanyField_OnObjectLookUpReturn);
            ctlCostCenterField.OnObjectLookUpCalling += new BaseUserControl.ObjectLookUpCallingEventHandler(ctlCostCenterField_OnObjectLookUpCalling);
            //ctlCostCenterField.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlCostCenterField_OnObjectLookUpReturn);
            ctlLocationField.OnObjectLookUpCalling += new BaseUserControl.ObjectLookUpCallingEventHandler(ctlLocationField_OnObjectLookUpCalling);
            //ctlLocationField.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlLocationField_OnObjectLookUpReturn);

            ctlCostCenterField.SetWidthTextBox(100);
            ctlCompanyField.SetWidthTextBox(100);
            ctlSupervisor.SetWidthTextBox(100);
            ctlLocationField.SetWidthTextBox(100);

            ctlCostCenterField.CompanyId = UIHelper.ParseLong(ctlCompanyField.CompanyID);
            ctlLocationField.CompanyId = UIHelper.ParseLong(ctlCompanyField.CompanyID);

            if (!Page.IsPostBack)
            {
                ctlCmbLanguage.DataSource = SsDbQueryProvider.DbLanguageQuery.FindAllListLanguage();
                ctlCmbLanguage.DataTextField = "LanguageName";
                ctlCmbLanguage.DataValueField = "Languageid";
                ctlCmbLanguage.DataBind();
            }
        }
        
        public void ResetValue()
        {
            ctlUserProfileFromERH.Text = "No";
            ctlUserProfileId.Text = string.Empty;
            ctlUserProfilePeopleId.Text = string.Empty;
            ctlUserProfileEmployeeCode.Text = string.Empty;
            ctlUserProfileEmployeeName.Text = string.Empty;
            ctlUserProfileSectionName.Text = string.Empty;
            ctlUserProfilePersonalLevel.Text = string.Empty;
            ctlUserProfilePersonalLevelDescription.Text = string.Empty;
            ctlUserProfilePersonalGroup.Text = string.Empty;
            ctlUserProfilePersonalGroupDescription.Text = string.Empty;
            ctlUserProfilePositionName.Text = string.Empty;
            ctlUserProfilePhoneNo.Text = string.Empty;
            ctlUserProfileEmail.Text = string.Empty;
            ctlUserProfileApprovalFlag.Checked = false;
            ctlUserProfileActive.Checked = false;
            ctlApproveRecject.Checked = false;
            ctlToReceive.Checked = false;
            ctlVendorCode.Text = string.Empty;

            ctlUserProfileId.Enabled = true;
            ctlUserProfilePeopleId.Enabled = true;
            ctlUserProfileEmployeeCode.Enabled = true;
            ctlUserProfileEmployeeName.Enabled = true;
            ctlUserProfileSectionName.Enabled = true;
            ctlUserProfilePersonalLevel.Enabled = true;
            ctlUserProfilePersonalLevelDescription.Enabled = true;
            ctlUserProfilePersonalGroup.Enabled = true;
            ctlUserProfilePersonalGroupDescription.Enabled = true;
            ctlUserProfilePositionName.Enabled = true;
            ctlUserProfilePhoneNo.Enabled = true;
            ctlUserProfileEmail.Enabled = true;
            ctlUserProfileApprovalFlag.Enabled = true;
            ctlUserProfileActive.Enabled = true;
            ctlApproveRecject.Enabled = true;
            ctlToReceive.Enabled = true;

            ctlUserProfileMobilePhoneNo.Text = string.Empty;
            ctlCompanyField.ResetValue();
            ctlCostCenterField.ResetValue();
            ctlLocationField.ResetValue();
            ctlSupervisor.ResetValue();

            ctlCompanyField.Mode = ModeEnum.ReadWrite;
            ctlCompanyField.ChangeMode();

            ctlCostCenterField.ReadOnly = false;
            ctlCostCenterField.CheckForReadOnly();

            ctlLocationField.ReadOnly = false;
            ctlLocationField.CheckForReadOnly();

            ctlSupervisor.ReadOnly = false;
            ctlSupervisor.CheckForReadOnly();


            // ========== Add code for control password same as Password Policy by Anuwat S on 24/04/2009 ==========
            ctlSetFailTime.Text = string.Empty;
            ctlOriginFailTime.Value = string.Empty;
            ctlChangePassword.Checked = false;
            ctlLockUser.Checked = false;
            // ======== End add code for control password same as Password Policy by Anuwat S on 24/04/2009 ========

            ctlUpdatePanelUserProfileForm.Update();

        }
        public void Initialize(string mode, long userID)
        {
            Mode = mode.ToString();
            UserID = userID;

            if (userID > 0)
            {
                user = QueryProvider.SuUserQuery.FindByIdentity(userID);
                ctlCmbLanguage.SelectedValue = (user.Language != null && user.Language.Languageid != null && user.Language.Languageid.ToString() != "") ? user.Language.Languageid.ToString() : "1";
            }
            if (mode.Equals(FlagEnum.EditFlag) && user != null)
            {
                if (user.FromEHr)
                    ctlUserProfileFromERH.Text = "Yes";
                else
                    ctlUserProfileFromERH.Text = "No";

                ctlUserProfileId.Text = user.UserName;
                ctlUserProfilePeopleId.Text = user.PeopleID;
                ctlUserProfileEmployeeCode.Text = user.EmployeeCode;
                ctlUserProfileEmployeeName.Text = user.EmployeeName;
                ctlUserProfileSectionName.Text = user.SectionName;
                ctlUserProfilePersonalLevel.Text = user.PersonalLevel;
                ctlUserProfilePersonalLevelDescription.Text = user.PersonalDescription;
                ctlUserProfilePersonalGroup.Text = user.PersonalGroup;
                ctlUserProfilePersonalGroupDescription.Text = user.PersonalLevelGroupDescription;
                ctlUserProfilePositionName.Text = user.PositionName;
                ctlUserProfilePhoneNo.Text = user.PhoneNo;
                ctlUserProfileEmail.Text = user.Email;
                ctlUserProfileApprovalFlag.Checked = user.ApprovalFlag;
                ctlUserProfileActive.Checked = user.Active;
                ctlApproveRecject.Checked = user.SMSApproveOrReject;
                ctlToReceive.Checked = user.SMSReadyToReceive;
                ctlUserProfileMobilePhoneNo.Text = user.MobilePhoneNo;
                ctlIsAdUser.Checked = user.IsAdUser;
                ctlVendorCode.Text = user.VendorCode;
                ctlEmailActive.Checked = user.EmailActive;
                
                if (user.IsAdUser)
                {
                    ctlChangePassword.Visible = false;
                }
                else
                {
                    ctlChangePassword.Visible = true;
                }

                if (user.Company != null)
                    ctlCompanyField.SetValue(user.Company.CompanyID);
                if (user.CostCenter != null)
                    ctlCostCenterField.SetValue(user.CostCenter.CostCenterID);
                if (user.Location != null)
                    ctlLocationField.SetValue(user.Location.LocationID);
                ctlSupervisor.SetValue(user.Supervisor);

                ctlCostCenterField.ReadOnly = false;
                ctlLocationField.ReadOnly = false;

                ctlCostCenterField.CheckForReadOnly();
                ctlLocationField.CheckForReadOnly();

                // ========== Add code for control password same as Password Policy by Anuwat S on 23/04/2009 ==========
                ctlSetFailTime.Text = user.SetFailTime.ToString();
                ctlOriginFailTime.Value = user.SetFailTime.ToString();
                if (ctlSetFailTime.Text.Length.Equals(0)) ctlSetFailTime.Text = "0";
                if (ctlOriginFailTime.Value.Length.Equals(0)) ctlOriginFailTime.Value = "0";
                ctlChangePassword.Checked = user.ChangePassword;
                //ctlLockUser.Checked = user.SetFailTime.Equals(user.FailTime);
                ctlLockUser.Checked = user.SetFailTime <= user.FailTime;
                // ======== End add code for control password same as Password Policy by Anuwat S on 23/04/2009 ========

                CheckReadOnlyMode(user);
            }
            else if (mode.Equals(FlagEnum.NewFlag))
            {
                ResetValue();
                ctlCostCenterField.ReadOnly = true;
                ctlLocationField.ReadOnly = true;

                ctlCostCenterField.CheckForReadOnly();
                ctlLocationField.CheckForReadOnly();
                // ========== Add code for control password same as Password Policy by Anuwat S on 23/04/2009 ==========
                divInputPassword.Visible = true;
                ctlResetPassword.Visible = false;
                ctlPasswordStrength.PreferredPasswordLength = ParameterServices.MinPasswordLength;
                ctlSetFailTime.Text = ParameterServices.DefaultLoginFailTime.ToString();
                ctlChangePassword.Checked = true;
                // ======== End add code for control password same as Password Policy by Anuwat S on 23/04/2009 ========
            }

            ctlUpdatePanelUserProfileForm.Update();
        }
        #endregion

        #region button event
        protected void ctlInsert_Click1(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (Mode == FlagEnum.NewFlag)
                {
                    #region Insert
                    user = new SuUser();
                    user.UserName = ctlUserProfileId.Text;
                    user.PeopleID = ctlUserProfilePeopleId.Text;
                    user.SetFailTime = UIHelper.ParseShort(ctlSetFailTime.Text);
                    user.EmployeeCode = ctlUserProfileEmployeeCode.Text;
                    user.EmployeeName = ctlUserProfileEmployeeName.Text;
                    user.SMSApproveOrReject = ctlApproveRecject.Checked;
                    user.SMSReadyToReceive = ctlToReceive.Checked;
                    user.MobilePhoneNo = ctlUserProfileMobilePhoneNo.Text;
                    user.SectionName = ctlUserProfileSectionName.Text;
                    user.PersonalLevel = ctlUserProfilePersonalLevel.Text;
                    user.PersonalDescription = ctlUserProfilePersonalLevelDescription.Text;
                    user.PersonalGroup = ctlUserProfilePersonalGroup.Text;
                    user.PersonalLevelGroupDescription = ctlUserProfilePersonalGroupDescription.Text;
                    user.PositionName = ctlUserProfilePositionName.Text;
                    user.PhoneNo = ctlUserProfilePhoneNo.Text;
                    user.Email = ctlUserProfileEmail.Text;
                    user.ApprovalFlag = ctlUserProfileApprovalFlag.Checked;
                    user.Active = ctlUserProfileActive.Checked;
                    user.Language = new SS.DB.DTO.DbLanguage(UIHelper.ParseShort(ctlCmbLanguage.SelectedValue));
                    user.ChangePassword = ctlChangePassword.Checked;
                    user.IsAdUser = ctlIsAdUser.Checked;
                    user.VendorCode = ctlVendorCode.Text;
                    user.EmailActive = ctlEmailActive.Checked;

                    DbCompany com = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(UIHelper.ParseLong(ctlCompanyField.CompanyID));
                    if (com != null)
                    {
                        user.CompanyCode = com.CompanyCode;
                        user.Company = com;
                    }
                    if (ctlLocationField.LocationID != null)
                    {
                        DbLocation location = ScgDbQueryProvider.DbLocationQuery.FindProxyByIdentity(ctlLocationField.LocationID.Value);
                        if (location != null)
                        {
                            user.LocationCode = location.LocationCode;
                            user.Location = location;                          
                        }
                    }
                    DbCostCenter cost = ScgDbQueryProvider.DbCostCenterQuery.FindByIdentity(UIHelper.ParseLong(ctlCostCenterField.CostCenterId));
                    if (cost != null)
                    {
                        user.CostCenter = cost;
                        user.CostCenterCode = cost.CostCenterCode;
                    }
                    if (ctlSupervisor.UserID != null)
                    {
                        SuUser su = QueryProvider.SuUserQuery.FindProxyByIdentity(ctlSupervisor.UserID.Value);
                        if (su != null)
                            user.Supervisor = su.Userid;

                    }
                    user.UpdBy = UserAccount.UserID;
                    user.UpdDate = DateTime.Now;
                    user.CreBy = UserAccount.UserID;
                    user.CreDate = DateTime.Now;
                    user.UpdPgm = UserAccount.CurrentProgramCode;

                    // ========== Add code for control password same as Password Policy by Anuwat S on 23/04/2009 ==========
                    if (!string.IsNullOrEmpty(ctlUserPassword.Text))
                    {
                        user.Password = Encryption.Md5Hash(ctlUserPassword.Text);
                    }
                    user.AllowPasswordChangeDate    = DateTime.Now.AddDays(ParameterServices.MinPasswordAge);
                    user.PasswordExpiryDate         = DateTime.Now.AddDays(ParameterServices.MaxPasswordAge);

                    if (ctlLockUser.Checked)
                    {
                        user.FailTime = user.SetFailTime;
                    }
                    else
                    {
                        user.FailTime = 0;
                    }

                    // ======== End add code for control password same as Password Policy by Anuwat S on 23/04/2009 ========

                    SuUserService.SaveUserProfile(user);

                    // ========== Add code for control password same as Password Policy by Anuwat S on 23/04/2009 ==========
                    SCGEmailService.SendEmailEM08(user.Userid, ctlUserPassword.Text);
                    #endregion Insert
                }
                else if (Mode == FlagEnum.EditFlag)
                {
                    #region Edit
                    user = QueryProvider.SuUserQuery.FindByIdentity(UserID);
                    user.UserName = ctlUserProfileId.Text;
                    user.PeopleID = ctlUserProfilePeopleId.Text;
                    user.SetFailTime = UIHelper.ParseShort(ctlSetFailTime.Text);
                    user.EmployeeCode = ctlUserProfileEmployeeCode.Text;
                    user.EmployeeName = ctlUserProfileEmployeeName.Text;
                    user.SMSApproveOrReject = ctlApproveRecject.Checked;
                    user.SMSReadyToReceive = ctlToReceive.Checked;
                    user.MobilePhoneNo = ctlUserProfileMobilePhoneNo.Text;
                    user.SectionName = ctlUserProfileSectionName.Text;
                    user.PersonalLevel = ctlUserProfilePersonalLevel.Text;
                    user.PersonalDescription = ctlUserProfilePersonalLevelDescription.Text;
                    user.PersonalGroup = ctlUserProfilePersonalGroup.Text;
                    user.PersonalLevelGroupDescription = ctlUserProfilePersonalGroupDescription.Text;
                    user.PositionName = ctlUserProfilePositionName.Text;
                    user.PhoneNo = ctlUserProfilePhoneNo.Text;
                    user.Email = ctlUserProfileEmail.Text;
                    user.ApprovalFlag = ctlUserProfileApprovalFlag.Checked;
                    user.Active = ctlUserProfileActive.Checked;
                    user.Language = new SS.DB.DTO.DbLanguage(UIHelper.ParseShort(ctlCmbLanguage.SelectedValue));
                    user.ChangePassword = ctlChangePassword.Checked;
                    user.IsAdUser = ctlIsAdUser.Checked;
                    user.VendorCode = ctlVendorCode.Text;
                    user.EmailActive = ctlEmailActive.Checked;

                    DbCompany com = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(UIHelper.ParseLong(ctlCompanyField.CompanyID));
                    if (com != null)
                    {
                        user.CompanyCode = com.CompanyCode;
                        user.Company = com;
                    }
                    if (ctlLocationField.LocationID != null)
                    {
                        DbLocation location = ScgDbQueryProvider.DbLocationQuery.FindProxyByIdentity(ctlLocationField.LocationID.Value);
                        if (location != null)
                        {
                            user.LocationCode = location.LocationCode;
                            user.Location = location;
                        }
                    }
                    DbCostCenter cost = ScgDbQueryProvider.DbCostCenterQuery.FindByIdentity(UIHelper.ParseLong(ctlCostCenterField.CostCenterId));
                    if (cost != null)
                    {
                        user.CostCenter = cost;
                        user.CostCenterCode = cost.CostCenterCode;
                    }
                    if (ctlSupervisor.UserID != null)
                    {
                        SuUser su = QueryProvider.SuUserQuery.FindProxyByIdentity(ctlSupervisor.UserID.Value);
                        if (su != null)
                            user.Supervisor = su.Userid;

                    }
                    user.UpdBy = UserAccount.UserID;
                    user.UpdDate = DateTime.Now;
                    user.CreBy = UserAccount.UserID;
                    user.CreDate = DateTime.Now;

                    user.UpdPgm = UserAccount.CurrentProgramCode;

                    // ========== Add code for control password same as Password Policy by Anuwat S on 23/04/2009 ==========
                    if ((int.Parse(ctlOriginFailTime.Value) <= (user.FailTime) && !ctlLockUser.Checked) ||
                            !user.SetFailTime.Equals(short.Parse(ctlOriginFailTime.Value)))
                        user.FailTime = 0;  // Unlock user
                    else if (ctlLockUser.Checked)
                        user.FailTime = user.SetFailTime;   // Still lock user
                    // ========== Add code for control password same as Password Policy by Anuwat S on 23/04/2009 ==========

                    //SuUserService.SaveOrUpdate(user);
                    SuUserService.UpdateUserProfile(user);

                    #endregion Edit
                }

                

                // ========== Add code for control password same as Password Policy by Anuwat S on 23/04/2009 ==========
                IList<SuPasswordHistory> passwordHistoryList = QueryProvider.SuPasswordHistoryQuery.FindActiveByUserId(user.Userid);
                foreach (SuPasswordHistory passwordHist in passwordHistoryList)
                {
                    passwordHist.Active = false;
                    passwordHist.UpdBy = UserAccount.UserID;
                    passwordHist.UpdDate = DateTime.Now;
                    passwordHist.UpdPgm = ProgramCode;
                    SuPasswordHistoryService.SaveOrUpdate(passwordHist);
                }
                // ========== Add code for control password same as Password Policy by Anuwat S on 23/04/2009 ==========
                if (Notify_Ok != null)
                    Notify_Ok(sender, e);

                Alert(GetMessage("SaveSuccessFully"));
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
                ctlUpdatePanelUserProfileForm.Update();
            }
        }

        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            ResetValue();
            if (Notify_Cancle != null)
            {
                Notify_Cancle(sender, e);
            }
        }

        protected void ctlResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                SuUser user = new SuUser();
                user = SuUserService.FindByIdentity(UserID);
                if (user != null)
                {
                	string realPassword = SuUserService.Forgetpassword(user.UserName);

                	SCGEmailService.SendEmailEM12(user.Userid, realPassword.ToString());
                	ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), string.Empty, string.Format("alert('{0}');", GetProgramMessage("ResetPasswordSuccess")), true);
            	}
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        #endregion

        #region private void Alert(string Message)
        private void Alert(string Message)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "DisableDropdown", "alert('" + Message + "');", true);
        }
        #endregion private void Alert(string Message)

        public void CheckReadOnlyMode(SuUser user)
        {

            if (!IsAdmin)
            {
                divInputPassword.Visible = false;
                ctlResetPassword.Visible = false;

                ctlUserProfileId.Enabled = false;
                ctlUserProfileEmployeeName.Enabled = false;
                ctlUserProfileEmployeeCode.Enabled = false;
                ctlUserProfilePeopleId.Enabled = false;
                ctlCompanyField.Mode = ModeEnum.Readonly;
                ctlCompanyField.ChangeMode();
                ctlLockUser.Enabled = false;
                ctlChangePassword.Enabled = false;
                ctlSetFailTime.Enabled = false;
                ctlUserProfileApprovalFlag.Enabled = false;
                ctlUserProfileActive.Enabled = false;
                ctlVendorCode.Enabled = false;

                //Field from EHr
                ctlUserProfileId.Enabled = false;
                ctlUserProfilePeopleId.Enabled = false;
                ctlUserProfileEmployeeCode.Enabled = false;
                ctlUserProfileEmployeeName.Enabled = false;
                ctlUserProfileSectionName.Enabled = false;
                ctlUserProfilePersonalLevel.Enabled = false;
                ctlUserProfilePersonalLevelDescription.Enabled = false;
                ctlUserProfilePersonalGroup.Enabled = false;
                ctlUserProfilePersonalGroupDescription.Enabled = false;
                ctlUserProfilePositionName.Enabled = false;
                ctlUserProfilePhoneNo.Enabled = false;
                ctlUserProfileEmail.Enabled = false;
                ctlIsAdUser.Enabled = false;

                ctlSupervisor.ReadOnly = true;
                ctlSupervisor.CheckForReadOnly();

                ctlCompanyField.Mode = ModeEnum.Readonly;
                ctlCompanyField.ChangeMode();
                ctlCostCenterField.ReadOnly = true;
                ctlLocationField.ReadOnly = true;

                ctlCostCenterField.CheckForReadOnly();
                ctlLocationField.CheckForReadOnly();
            }
            else
            {
                // If user is admin, hide password zone and display button to reset password
                divInputPassword.Visible = false;
                if (user.IsAdUser)
                {
                    ctlResetPassword.Visible = false;
                }
                else
                {
                	ctlResetPassword.Visible = true;
                }

                ctlIsAdUser.Enabled = true;
                if (user.FromEHr)
                {
                    ctlUserProfileId.Enabled = false;
                    ctlUserProfilePeopleId.Enabled = false;
                    ctlUserProfileEmployeeCode.Enabled = false;
                    ctlUserProfileEmployeeName.Enabled = false;
                    ctlUserProfileSectionName.Enabled = false;
                    ctlUserProfilePersonalLevel.Enabled = false;
                    ctlUserProfilePersonalLevelDescription.Enabled = false;
                    ctlUserProfilePersonalGroup.Enabled = false;
                    ctlUserProfilePersonalGroupDescription.Enabled = false;
                    ctlUserProfilePositionName.Enabled = false;
                    ctlUserProfilePhoneNo.Enabled = false;
                    ctlUserProfileEmail.Enabled = false;

                    ctlSupervisor.ReadOnly = true;
                    ctlSupervisor.CheckForReadOnly();

                    ctlCompanyField.Mode = ModeEnum.Readonly;
                    ctlCompanyField.ChangeMode();
                    ctlCostCenterField.ReadOnly = true;
                    ctlLocationField.ReadOnly = true;

                    ctlCostCenterField.CheckForReadOnly();
                    ctlLocationField.CheckForReadOnly();
                }
            }
        }

        protected void ctlIsAdUser_CheckedChanged(object sender, EventArgs e)
        {

            if (ctlIsAdUser.Checked)
            {
                ctlChangePassword.Checked = false;
                ctlChangePassword.Visible = false;
                if (Mode == FlagEnum.EditFlag)
                {
                    ctlResetPassword.Visible = false;
    			}
                else
                {
                    divInputPassword.Visible = false;
                }
            }
            else
            {
                ctlChangePassword.Visible = true;
                if (Mode == FlagEnum.EditFlag)
                {
                    ctlResetPassword.Visible = true;
                }
                else
                {
                    divInputPassword.Visible = true;
                }
            }
        }
    }
}
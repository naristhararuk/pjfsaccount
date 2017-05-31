//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by NHibernate.
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

using System;
using SS.DB.DTO;
using SCG.DB.DTO;

namespace SS.SU.DTO
{
    /// <summary>
    /// POJO for SuUser. This class is autogenerated
    /// </summary>
    [Serializable]
    public partial class SuUser
    {
        #region Fields

        private long userID;
        private string employeeCode;
        private string companyCode;
        private string costCenterCode;
        private string locationCode;
        private string userName;
        private string password;
        private short setFailTime;
        private short failTime;
        private bool changePassword;
        private DateTime? allowPasswordChangeDate;
        private DateTime? passwordExpiryDate;
        private string peopleID;
        private string employeeName;
        private string sectionName;
        private string personalLevel;
        private string personalDescription;
        private string personalGroup;
        private string personalLevelGroupDescription;
        private string positionName;
        private long supervisor;
        private string phoneNo;
        private string mobilePhoneNo;
        private bool sMSApproveOrReject;
        private bool sMSReadyToReceive;
        private DateTime? hireDate;
        private bool approvalFlag;
        private string email;
        private bool fromEHr;
        private string comment;
        private long updBy;
        private DateTime updDate;
        private long creBy;
        private DateTime creDate;
        private string updPgm;
        private Byte[] rowVersion;
        private bool active;
        private SS.DB.DTO.DbLanguage language;
        private SCG.DB.DTO.DbCompany company;
        private SCG.DB.DTO.DbCostCenter costCenter;
        private SCG.DB.DTO.DbLocation locationID;
        private bool isAdUser;
        private string vendorCode;
        private bool emailActive;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SuUser class
        /// </summary>
        public SuUser()
        {
        }

        public SuUser(long userID)
        {
            this.userID = userID;
        }

        /// <summary>
        /// Initializes a new instance of the SuUser class
        /// </summary>
        /// <param name="employeeCode">Initial <see cref="SuUser.EmployeeCode" /> value</param>
        /// <param name="companyCode">Initial <see cref="SuUser.CompanyCode" /> value</param>
        /// <param name="costCenterCode">Initial <see cref="SuUser.CostCenterCode" /> value</param>
        /// <param name="locationCode">Initial <see cref="SuUser.LocationCode" /> value</param>
        /// <param name="userName">Initial <see cref="SuUser.UserName" /> value</param>
        /// <param name="password">Initial <see cref="SuUser.Password" /> value</param>
        /// <param name="setFailTime">Initial <see cref="SuUser.SetFailTime" /> value</param>
        /// <param name="failTime">Initial <see cref="SuUser.FailTime" /> value</param>
        /// <param name="changePassword">Initial <see cref="SuUser.ChangePassword" /> value</param>
        /// <param name="allowPasswordChangeDate">Initial <see cref="SuUser.AllowPasswordChangeDate" /> value</param>
        /// <param name="passwordExpiryDate">Initial <see cref="SuUser.PasswordExpiryDate" /> value</param>
        /// <param name="peopleID">Initial <see cref="SuUser.PeopleID" /> value</param>
        /// <param name="employeeName">Initial <see cref="SuUser.EmployeeName" /> value</param>
        /// <param name="sectionName">Initial <see cref="SuUser.SectionName" /> value</param>
        /// <param name="personalLevel">Initial <see cref="SuUser.PersonalLevel" /> value</param>
        /// <param name="personalDescription">Initial <see cref="SuUser.PersonalDescription" /> value</param>
        /// <param name="personalGroup">Initial <see cref="SuUser.PersonalGroup" /> value</param>
        /// <param name="personalLevelGroupDescription">Initial <see cref="SuUser.PersonalLevelGroupDescription" /> value</param>
        /// <param name="positionName">Initial <see cref="SuUser.PositionName" /> value</param>
        /// <param name="supervisor">Initial <see cref="SuUser.Supervisor" /> value</param>
        /// <param name="phoneNo">Initial <see cref="SuUser.PhoneNo" /> value</param>
        /// <param name="mobilePhoneNo">Initial <see cref="SuUser.MobilePhoneNo" /> value</param>
        /// <param name="sMSApproveOrReject">Initial <see cref="SuUser.SMSApproveOrReject" /> value</param>
        /// <param name="sMSReadyToReceive">Initial <see cref="SuUser.SMSReadyToReceive" /> value</param>
        /// <param name="hireDate">Initial <see cref="SuUser.HireDate" /> value</param>
        /// <param name="approvalFlag">Initial <see cref="SuUser.ApprovalFlag" /> value</param>
        /// <param name="email">Initial <see cref="SuUser.Email" /> value</param>
        /// <param name="fromEHr">Initial <see cref="SuUser.FromEHr" /> value</param>
        /// <param name="comment">Initial <see cref="SuUser.Comment" /> value</param>
        /// <param name="updBy">Initial <see cref="SuUser.UpdBy" /> value</param>
        /// <param name="updDate">Initial <see cref="SuUser.UpdDate" /> value</param>
        /// <param name="creBy">Initial <see cref="SuUser.CreBy" /> value</param>
        /// <param name="creDate">Initial <see cref="SuUser.CreDate" /> value</param>
        /// <param name="updPgm">Initial <see cref="SuUser.UpdPgm" /> value</param>
        /// <param name="rowVersion">Initial <see cref="SuUser.RowVersion" /> value</param>
        /// <param name="active">Initial <see cref="SuUser.Active" /> value</param>
        /// <param name="language">Initial <see cref="SuUser.Language" /> value</param>
        /// <param name="company">Initial <see cref="SuUser.Company" /> value</param>
        /// <param name="costCenter">Initial <see cref="SuUser.CostCenter" /> value</param>
        /// <param name="locationID">Initial <see cref="SuUser.LocationID" /> value</param>
        public SuUser(string employeeCode, string companyCode, string costCenterCode, string locationCode, string userName, string password, short setFailTime, short failTime, bool changePassword, DateTime? allowPasswordChangeDate, DateTime? passwordExpiryDate, string peopleID, string employeeName, string sectionName, string personalLevel, string personalDescription, string personalGroup, string personalLevelGroupDescription, string positionName, long supervisor, string phoneNo, string mobilePhoneNo, bool sMSApproveOrReject, bool sMSReadyToReceive, DateTime? hireDate, bool approvalFlag, string email, bool fromEHr, string comment, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, Byte[] rowVersion, bool active, SS.DB.DTO.DbLanguage language, SCG.DB.DTO.DbCompany company, SCG.DB.DTO.DbCostCenter costCenter, SCG.DB.DTO.DbLocation locationID)
        {
            this.employeeCode = employeeCode;
            this.companyCode = companyCode;
            this.costCenterCode = costCenterCode;
            this.locationCode = locationCode;
            this.userName = userName;
            this.password = password;
            this.setFailTime = setFailTime;
            this.failTime = failTime;
            this.changePassword = changePassword;
            this.allowPasswordChangeDate = allowPasswordChangeDate;
            this.passwordExpiryDate = passwordExpiryDate;
            this.peopleID = peopleID;
            this.employeeName = employeeName;
            this.sectionName = sectionName;
            this.personalLevel = personalLevel;
            this.personalDescription = personalDescription;
            this.personalGroup = personalGroup;
            this.personalLevelGroupDescription = personalLevelGroupDescription;
            this.positionName = positionName;
            this.supervisor = supervisor;
            this.phoneNo = phoneNo;
            this.mobilePhoneNo = mobilePhoneNo;
            this.sMSApproveOrReject = sMSApproveOrReject;
            this.sMSReadyToReceive = sMSReadyToReceive;
            this.hireDate = hireDate;
            this.approvalFlag = approvalFlag;
            this.email = email;
            this.fromEHr = fromEHr;
            this.comment = comment;
            this.updBy = updBy;
            this.updDate = updDate;
            this.creBy = creBy;
            this.creDate = creDate;
            this.updPgm = updPgm;
            this.rowVersion = rowVersion;
            this.active = active;
            this.language = language;
            this.company = company;
            this.costCenter = costCenter;
            this.locationID = locationID;
        }

        /// <summary>
        /// Minimal constructor for class SuUser
        /// </summary>
        /// <param name="employeeCode">Initial <see cref="SuUser.EmployeeCode" /> value</param>
        /// <param name="userName">Initial <see cref="SuUser.UserName" /> value</param>
        /// <param name="password">Initial <see cref="SuUser.Password" /> value</param>
        /// <param name="setFailTime">Initial <see cref="SuUser.SetFailTime" /> value</param>
        /// <param name="failTime">Initial <see cref="SuUser.FailTime" /> value</param>
        /// <param name="changePassword">Initial <see cref="SuUser.ChangePassword" /> value</param>
        /// <param name="peopleID">Initial <see cref="SuUser.PeopleID" /> value</param>
        /// <param name="employeeName">Initial <see cref="SuUser.EmployeeName" /> value</param>
        /// <param name="email">Initial <see cref="SuUser.Email" /> value</param>
        /// <param name="updBy">Initial <see cref="SuUser.UpdBy" /> value</param>
        /// <param name="updDate">Initial <see cref="SuUser.UpdDate" /> value</param>
        /// <param name="creBy">Initial <see cref="SuUser.CreBy" /> value</param>
        /// <param name="creDate">Initial <see cref="SuUser.CreDate" /> value</param>
        /// <param name="updPgm">Initial <see cref="SuUser.UpdPgm" /> value</param>
        /// <param name="active">Initial <see cref="SuUser.Active" /> value</param>
        public SuUser(string employeeCode, string userName, string password, short setFailTime, short failTime, bool changePassword, string peopleID, string employeeName, string email, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, bool active)
        {
            this.employeeCode = employeeCode;
            this.userName = userName;
            this.password = password;
            this.setFailTime = setFailTime;
            this.failTime = failTime;
            this.changePassword = changePassword;
            this.peopleID = peopleID;
            this.employeeName = employeeName;
            this.email = email;
            this.updBy = updBy;
            this.updDate = updDate;
            this.creBy = creBy;
            this.creDate = creDate;
            this.updPgm = updPgm;
            this.active = active;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the UserID for the current SuUser
        /// </summary>
        public virtual long Userid
        {
            get { return this.userID; }
            set { this.userID = value; }
        }

        /// <summary>
        /// Gets or sets the EmployeeCode for the current SuUser
        /// </summary>
        public virtual string EmployeeCode
        {
            get { return this.employeeCode; }
            set { this.employeeCode = value; }
        }

        /// <summary>
        /// Gets or sets the CompanyCode for the current SuUser
        /// </summary>
        public virtual string CompanyCode
        {
            get { return this.companyCode; }
            set { this.companyCode = value; }
        }

        /// <summary>
        /// Gets or sets the CostCenterCode for the current SuUser
        /// </summary>
        public virtual string CostCenterCode
        {
            get { return this.costCenterCode; }
            set { this.costCenterCode = value; }
        }

        /// <summary>
        /// Gets or sets the LocationCode for the current SuUser
        /// </summary>
        public virtual string LocationCode
        {
            get { return this.locationCode; }
            set { this.locationCode = value; }
        }

        /// <summary>
        /// Gets or sets the UserName for the current SuUser
        /// </summary>
        public virtual string UserName
        {
            get { return this.userName; }
            set { this.userName = value; }
        }

        /// <summary>
        /// Gets or sets the Password for the current SuUser
        /// </summary>
        public virtual string Password
        {
            get { return this.password; }
            set { this.password = value; }
        }

        /// <summary>
        /// Gets or sets the SetFailTime for the current SuUser
        /// </summary>
        public virtual short SetFailTime
        {
            get { return this.setFailTime; }
            set { this.setFailTime = value; }
        }

        /// <summary>
        /// Gets or sets the FailTime for the current SuUser
        /// </summary>
        public virtual short FailTime
        {
            get { return this.failTime; }
            set { this.failTime = value; }
        }

        /// <summary>
        /// Gets or sets the ChangePassword for the current SuUser
        /// </summary>
        public virtual bool ChangePassword
        {
            get { return this.changePassword; }
            set { this.changePassword = value; }
        }

        /// <summary>
        /// Gets or sets the AllowPasswordChangeDate for the current SuUser
        /// </summary>
        public virtual DateTime? AllowPasswordChangeDate
        {
            get { return this.allowPasswordChangeDate; }
            set { this.allowPasswordChangeDate = value; }
        }

        /// <summary>
        /// Gets or sets the PasswordExpiryDate for the current SuUser
        /// </summary>
        public virtual DateTime? PasswordExpiryDate
        {
            get { return this.passwordExpiryDate; }
            set { this.passwordExpiryDate = value; }
        }

        /// <summary>
        /// Gets or sets the PeopleID for the current SuUser
        /// </summary>
        public virtual string PeopleID
        {
            get { return this.peopleID; }
            set { this.peopleID = value; }
        }

        /// <summary>
        /// Gets or sets the EmployeeName for the current SuUser
        /// </summary>
        public virtual string EmployeeName
        {
            get { return this.employeeName; }
            set { this.employeeName = value; }
        }

        /// <summary>
        /// Gets or sets the SectionName for the current SuUser
        /// </summary>
        public virtual string SectionName
        {
            get { return this.sectionName; }
            set { this.sectionName = value; }
        }

        /// <summary>
        /// Gets or sets the PersonalLevel for the current SuUser
        /// </summary>
        public virtual string PersonalLevel
        {
            get { return this.personalLevel; }
            set { this.personalLevel = value; }
        }

        /// <summary>
        /// Gets or sets the PersonalDescription for the current SuUser
        /// </summary>
        public virtual string PersonalDescription
        {
            get { return this.personalDescription; }
            set { this.personalDescription = value; }
        }

        /// <summary>
        /// Gets or sets the PersonalGroup for the current SuUser
        /// </summary>
        public virtual string PersonalGroup
        {
            get { return this.personalGroup; }
            set { this.personalGroup = value; }
        }

        /// <summary>
        /// Gets or sets the PersonalLevelGroupDescription for the current SuUser
        /// </summary>
        public virtual string PersonalLevelGroupDescription
        {
            get { return this.personalLevelGroupDescription; }
            set { this.personalLevelGroupDescription = value; }
        }

        /// <summary>
        /// Gets or sets the PositionName for the current SuUser
        /// </summary>
        public virtual string PositionName
        {
            get { return this.positionName; }
            set { this.positionName = value; }
        }

        /// <summary>
        /// Gets or sets the Supervisor for the current SuUser
        /// </summary>
        public virtual long Supervisor
        {
            get { return this.supervisor; }
            set { this.supervisor = value; }
        }

        /// <summary>
        /// Gets or sets the PhoneNo for the current SuUser
        /// </summary>
        public virtual string PhoneNo
        {
            get { return this.phoneNo; }
            set { this.phoneNo = value; }
        }

        /// <summary>
        /// Gets or sets the MobilePhoneNo for the current SuUser
        /// </summary>
        public virtual string MobilePhoneNo
        {
            get { return this.mobilePhoneNo; }
            set { this.mobilePhoneNo = value; }
        }

        /// <summary>
        /// Gets or sets the SMSApproveOrReject for the current SuUser
        /// </summary>
        public virtual bool SMSApproveOrReject
        {
            get { return this.sMSApproveOrReject; }
            set { this.sMSApproveOrReject = value; }
        }

        /// <summary>
        /// Gets or sets the SMSReadyToReceive for the current SuUser
        /// </summary>
        public virtual bool SMSReadyToReceive
        {
            get { return this.sMSReadyToReceive; }
            set { this.sMSReadyToReceive = value; }
        }

        /// <summary>
        /// Gets or sets the HireDate for the current SuUser
        /// </summary>
        public virtual DateTime? HireDate
        {
            get { return this.hireDate; }
            set { this.hireDate = value; }
        }

        /// <summary>
        /// Gets or sets the ApprovalFlag for the current SuUser
        /// </summary>
        public virtual bool ApprovalFlag
        {
            get { return this.approvalFlag; }
            set { this.approvalFlag = value; }
        }

        /// <summary>
        /// Gets or sets the Email for the current SuUser
        /// </summary>
        public virtual string Email
        {
            get { return this.email; }
            set { this.email = value; }
        }

        /// <summary>
        /// Gets or sets the FromEHr for the current SuUser
        /// </summary>
        public virtual bool FromEHr
        {
            get { return this.fromEHr; }
            set { this.fromEHr = value; }
        }

        /// <summary>
        /// Gets or sets the Comment for the current SuUser
        /// </summary>
        public virtual string Comment
        {
            get { return this.comment; }
            set { this.comment = value; }
        }

        /// <summary>
        /// Gets or sets the UpdBy for the current SuUser
        /// </summary>
        public virtual long UpdBy
        {
            get { return this.updBy; }
            set { this.updBy = value; }
        }

        /// <summary>
        /// Gets or sets the UpdDate for the current SuUser
        /// </summary>
        public virtual DateTime UpdDate
        {
            get { return this.updDate; }
            set { this.updDate = value; }
        }

        /// <summary>
        /// Gets or sets the CreBy for the current SuUser
        /// </summary>
        public virtual long CreBy
        {
            get { return this.creBy; }
            set { this.creBy = value; }
        }

        /// <summary>
        /// Gets or sets the CreDate for the current SuUser
        /// </summary>
        public virtual DateTime CreDate
        {
            get { return this.creDate; }
            set { this.creDate = value; }
        }

        /// <summary>
        /// Gets or sets the UpdPgm for the current SuUser
        /// </summary>
        public virtual string UpdPgm
        {
            get { return this.updPgm; }
            set { this.updPgm = value; }
        }

        /// <summary>
        /// Gets or sets the RowVersion for the current SuUser
        /// </summary>
        public virtual Byte[] RowVersion
        {
            get { return this.rowVersion; }
            set { this.rowVersion = value; }
        }

        /// <summary>
        /// Gets or sets the Active for the current SuUser
        /// </summary>
        public virtual bool Active
        {
            get { return this.active; }
            set { this.active = value; }
        }

        /// <summary>
        /// Gets or sets the Language for the current SuUser
        /// </summary>
        public virtual SS.DB.DTO.DbLanguage Language
        {
            get { return this.language; }
            set { this.language = value; }
        }

        /// <summary>
        /// Gets or sets the Company for the current SuUser
        /// </summary>
        public virtual SCG.DB.DTO.DbCompany Company
        {
            get { return this.company; }
            set { this.company = value; }
        }

        /// <summary>
        /// Gets or sets the CostCenter for the current SuUser
        /// </summary>
        public virtual SCG.DB.DTO.DbCostCenter CostCenter
        {
            get { return this.costCenter; }
            set { this.costCenter = value; }
        }

        /// <summary>
        /// Gets or sets the LocationID for the current SuUser
        /// </summary>
        public virtual SCG.DB.DTO.DbLocation Location
        {
            get { return this.locationID; }
            set { this.locationID = value; }
        }

        /// <summary>
        /// Gets or sets the IsAdUser for the current SuUser
        /// </summary>
        public virtual bool IsAdUser
        {
            get { return this.isAdUser; }
            set { this.isAdUser = value; }
        }

        public virtual string VendorCode
        {
            get { return this.vendorCode; }
            set { this.vendorCode = value; }
        }

        public virtual bool EmailActive
        {
            get { return this.emailActive; }
            set { this.emailActive = value; }
        }

        #endregion
    }
}
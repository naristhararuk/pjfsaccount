using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO
{
	[Serializable]
	public partial class SuUserSearchResult
	{
		#region Fields

		private long userid;
		private string userName;
		private string password;
		private short setFailTime;
		private short failTime;
		private DateTime effDate;
		private DateTime endDate;
		private bool changePassword;
		private string comment;
		//private long updBy;
		//private DateTime updDate;
		//private long creBy;
		//private DateTime creDate;
		//private string updPgm;
		//private Byte[] rowVersion;
		private bool active;
		private short divisionid;
		private string divisionName;
		private short organizationid;
		private string organizationName;
		private short languageid;
		private string languageName;
        private string email;
        private string employeeName;
       // private string lastName;
        private char initiatorType;
        private bool sms;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the Userid for the current SuUser
		/// </summary>
		public virtual long UserID
		{
			get { return this.userid; }
			set { this.userid = value; }
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
		/// Gets or sets the EffDate for the current SuUser
		/// </summary>
		public virtual DateTime EffDate
		{
			get { return this.effDate; }
			set { this.effDate = value; }
		}

		/// <summary>
		/// Gets or sets the EndDate for the current SuUser
		/// </summary>
		public virtual DateTime EndDate
		{
			get { return this.endDate; }
			set { this.endDate = value; }
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
		//public virtual long UpdBy
		//{
		//    get { return this.updBy; }
		//    set { this.updBy = value; }
		//}

		/// <summary>
		/// Gets or sets the UpdDate for the current SuUser
		/// </summary>
		//public virtual DateTime UpdDate
		//{
		//    get { return this.updDate; }
		//    set { this.updDate = value; }
		//}

		/// <summary>
		/// Gets or sets the CreBy for the current SuUser
		/// </summary>
		//public virtual long CreBy
		//{
		//    get { return this.creBy; }
		//    set { this.creBy = value; }
		//}

		/// <summary>
		/// Gets or sets the CreDate for the current SuUser
		/// </summary>
		//public virtual DateTime CreDate
		//{
		//    get { return this.creDate; }
		//    set { this.creDate = value; }
		//}

		/// <summary>
		/// Gets or sets the UpdPgm for the current SuUser
		/// </summary>
		//public virtual string UpdPgm
		//{
		//    get { return this.updPgm; }
		//    set { this.updPgm = value; }
		//}

		/// <summary>
		/// Gets or sets the RowVersion for the current SuUser
		/// </summary>
		//public virtual Byte[] RowVersion
		//{
		//    get { return this.rowVersion; }
		//    set { this.rowVersion = value; }
		//}

		/// <summary>
		/// Gets or sets the Active for the current SuUser
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}

		/// <summary>
		/// Gets or sets the DivisionID for the current SuUser
		/// </summary>
		public virtual short DivisionID
		{
			get { return this.divisionid; }
			set { this.divisionid = value; }
		}

		/// <summary>
		/// Gets or sets the DivisionName for the current SuUser
		/// </summary>
		public virtual string DivisionName
		{
			get { return this.divisionName; }
			set { this.divisionName = value; }
		}

		/// <summary>
		/// Gets or sets the OrganizationID for the current SuUser
		/// </summary>
		public virtual short OrganizationID
		{
			get { return this.organizationid; }
			set { this.organizationid = value; }
		}

		/// <summary>
		/// Gets or sets the OrganizationName for the current SuUser
		/// </summary>
		public virtual string OrganizationName
		{
			get { return this.organizationName; }
			set { this.organizationName = value; }
		}

		/// <summary>
		/// Gets or sets the LanguageID for the current SuUser
		/// </summary>
		public virtual short LanguageID
		{
			get { return this.languageid; }
			set { this.languageid = value; }
		}

		/// <summary>
		/// Gets or sets the LanguageID for the current SuUser
		/// </summary>
		public virtual string LanguageName
		{
			get { return this.languageName; }
			set { this.languageName = value; }
		}
        public virtual string Email
        {
            get { return this.email; }
            set { this.email = value; }
        }
        public virtual string EmployeeName
        {
            get { return this.employeeName; }
            set { this.employeeName = value; }
        }
        //public virtual string LastName
        //{
        //    get { return this.lastName; }
        //    set { this.lastName = value; }
        //}
        public virtual char InitiatorType
        {
            get { return this.initiatorType; }
            set { this.initiatorType = value; }
        }
        public virtual bool SMS
        {
            get { return this.sms; }
            set { this.sms = value; }
        }
		#endregion
	}
}

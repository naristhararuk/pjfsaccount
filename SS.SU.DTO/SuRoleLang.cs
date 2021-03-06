//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by NHibernate.
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

using System;

namespace SS.SU.DTO
{
	/// <summary>
	/// POJO for SuRoleLang. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class SuRoleLang
	{
		#region Fields
		
		private long id;
		private string roleName;
		private string comment;
		private long updBy;
		private DateTime updDate;
		private long creBy;
		private DateTime creDate;
		private string updPgm;
		private Byte[] rowVersion;
		private bool active;
		private SS.SU.DTO.SuRole role;
		private SS.DB.DTO.DbLanguage language;

		#endregion

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the SuRoleLang class
		/// </summary>
		public SuRoleLang()
		{
		}

		public SuRoleLang(long id)
		{
			this.id = id;
		}
	
		/// <summary>
		/// Initializes a new instance of the SuRoleLang class
		/// </summary>
		/// <param name="roleName">Initial <see cref="SuRoleLang.RoleName" /> value</param>
		/// <param name="comment">Initial <see cref="SuRoleLang.Comment" /> value</param>
		/// <param name="updBy">Initial <see cref="SuRoleLang.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="SuRoleLang.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="SuRoleLang.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="SuRoleLang.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="SuRoleLang.UpdPgm" /> value</param>
		/// <param name="rowVersion">Initial <see cref="SuRoleLang.RowVersion" /> value</param>
		/// <param name="active">Initial <see cref="SuRoleLang.Active" /> value</param>
		/// <param name="role">Initial <see cref="SuRoleLang.Role" /> value</param>
		/// <param name="language">Initial <see cref="SuRoleLang.Language" /> value</param>
		public SuRoleLang(string roleName, string comment, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, Byte[] rowVersion, bool active, SS.SU.DTO.SuRole role, SS.DB.DTO.DbLanguage language)
		{
			this.roleName = roleName;
			this.comment = comment;
			this.updBy = updBy;
			this.updDate = updDate;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updPgm = updPgm;
			this.rowVersion = rowVersion;
			this.active = active;
			this.role = role;
			this.language = language;
		}
	
		/// <summary>
		/// Minimal constructor for class SuRoleLang
		/// </summary>
		/// <param name="roleName">Initial <see cref="SuRoleLang.RoleName" /> value</param>
		/// <param name="updBy">Initial <see cref="SuRoleLang.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="SuRoleLang.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="SuRoleLang.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="SuRoleLang.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="SuRoleLang.UpdPgm" /> value</param>
		/// <param name="active">Initial <see cref="SuRoleLang.Active" /> value</param>
		public SuRoleLang(string roleName, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, bool active)
		{
			this.roleName = roleName;
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
		/// Gets or sets the Id for the current SuRoleLang
		/// </summary>
		public virtual long Id
		{
			get { return this.id; }
			set { this.id = value; }
		}
		
		/// <summary>
		/// Gets or sets the RoleName for the current SuRoleLang
		/// </summary>
		public virtual string RoleName
		{
			get { return this.roleName; }
			set { this.roleName = value; }
		}
		
		/// <summary>
		/// Gets or sets the Comment for the current SuRoleLang
		/// </summary>
		public virtual string Comment
		{
			get { return this.comment; }
			set { this.comment = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdBy for the current SuRoleLang
		/// </summary>
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdDate for the current SuRoleLang
		/// </summary>
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreBy for the current SuRoleLang
		/// </summary>
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreDate for the current SuRoleLang
		/// </summary>
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdPgm for the current SuRoleLang
		/// </summary>
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}
		
		/// <summary>
		/// Gets or sets the RowVersion for the current SuRoleLang
		/// </summary>
		public virtual Byte[] RowVersion
		{
			get { return this.rowVersion; }
			set { this.rowVersion = value; }
		}
		
		/// <summary>
		/// Gets or sets the Active for the current SuRoleLang
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		
		/// <summary>
		/// Gets or sets the Role for the current SuRoleLang
		/// </summary>
		public virtual SS.SU.DTO.SuRole Role
		{
			get { return this.role; }
			set { this.role = value; }
		}
		
		/// <summary>
		/// Gets or sets the Language for the current SuRoleLang
		/// </summary>
		public virtual SS.DB.DTO.DbLanguage Language
		{
			get { return this.language; }
			set { this.language = value; }
		}
        public virtual short RoleId
        {
            get
            {
                if (Role != null)
                {
                    return this.Role.RoleID;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (Role != null)
                {
                    this.Role.RoleID = value;
                }
            }
        }
		
		#endregion
	}
}

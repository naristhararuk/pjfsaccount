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
	/// POJO for SuUserProfile. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class SuUserProfile
	{
		#region Fields
		
		private long userid;
		private string comment;
		private long updBy;
		private DateTime updDate;
		private long creBy;
		private DateTime creDate;
		private string updPgm;
		private Byte[] rowVersion;
		private bool active;
		private SS.DB.DTO.DbPrefix prefix;

		#endregion

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the SuUserProfile class
		/// </summary>
		public SuUserProfile()
		{
		}

		public SuUserProfile(long userid)
		{
			this.userid = userid;
		}
	
		/// <summary>
		/// Initializes a new instance of the SuUserProfile class
		/// </summary>
		/// <param name="comment">Initial <see cref="SuUserProfile.Comment" /> value</param>
		/// <param name="updBy">Initial <see cref="SuUserProfile.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="SuUserProfile.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="SuUserProfile.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="SuUserProfile.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="SuUserProfile.UpdPgm" /> value</param>
		/// <param name="rowVersion">Initial <see cref="SuUserProfile.RowVersion" /> value</param>
		/// <param name="active">Initial <see cref="SuUserProfile.Active" /> value</param>
		/// <param name="prefix">Initial <see cref="SuUserProfile.Prefix" /> value</param>
		/// <param name="address">Initial <see cref="SuUserProfile.Address" /> value</param>
		public SuUserProfile(string comment, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, Byte[] rowVersion, bool active, SS.DB.DTO.DbPrefix prefix)
		{
			this.comment = comment;
			this.updBy = updBy;
			this.updDate = updDate;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updPgm = updPgm;
			this.rowVersion = rowVersion;
			this.active = active;
			this.prefix = prefix;
		}
	
		/// <summary>
		/// Minimal constructor for class SuUserProfile
		/// </summary>
		/// <param name="updBy">Initial <see cref="SuUserProfile.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="SuUserProfile.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="SuUserProfile.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="SuUserProfile.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="SuUserProfile.UpdPgm" /> value</param>
		/// <param name="active">Initial <see cref="SuUserProfile.Active" /> value</param>
		public SuUserProfile(long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, bool active)
		{
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
		/// Gets or sets the Userid for the current SuUserProfile
		/// </summary>
		public virtual long Userid
		{
			get { return this.userid; }
			set { this.userid = value; }
		}
		
		/// <summary>
		/// Gets or sets the Comment for the current SuUserProfile
		/// </summary>
		public virtual string Comment
		{
			get { return this.comment; }
			set { this.comment = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdBy for the current SuUserProfile
		/// </summary>
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdDate for the current SuUserProfile
		/// </summary>
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreBy for the current SuUserProfile
		/// </summary>
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreDate for the current SuUserProfile
		/// </summary>
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdPgm for the current SuUserProfile
		/// </summary>
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}
		
		/// <summary>
		/// Gets or sets the RowVersion for the current SuUserProfile
		/// </summary>
		public virtual Byte[] RowVersion
		{
			get { return this.rowVersion; }
			set { this.rowVersion = value; }
		}
		
		/// <summary>
		/// Gets or sets the Active for the current SuUserProfile
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		
		/// <summary>
		/// Gets or sets the Prefix for the current SuUserProfile
		/// </summary>
		public virtual SS.DB.DTO.DbPrefix Prefix
		{
			get { return this.prefix; }
			set { this.prefix = value; }
		}
		
		#endregion
	}
}

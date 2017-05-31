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
	/// POJO for SuRolepb. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class SuRolepb
	{
		#region Fields
		
		private long rolePBID;
		private long updBy;
		private DateTime updDate;
		private long creBy;
		private DateTime creDate;
		private string updPgm;
		private Byte[] rowVersion;
		private bool active;
		private SS.SU.DTO.SuRole roleID;
		private SCG.DB.DTO.Dbpb pBID;

		#endregion

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the SuRolepb class
		/// </summary>
		public SuRolepb()
		{
		}

		public SuRolepb(long rolePBID)
		{
			this.rolePBID = rolePBID;
		}
	
		/// <summary>
		/// Initializes a new instance of the SuRolepb class
		/// </summary>
		/// <param name="updBy">Initial <see cref="SuRolepb.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="SuRolepb.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="SuRolepb.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="SuRolepb.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="SuRolepb.UpdPgm" /> value</param>
		/// <param name="rowVersion">Initial <see cref="SuRolepb.RowVersion" /> value</param>
		/// <param name="active">Initial <see cref="SuRolepb.Active" /> value</param>
		/// <param name="roleID">Initial <see cref="SuRolepb.RoleID" /> value</param>
		/// <param name="pBID">Initial <see cref="SuRolepb.PBID" /> value</param>
		public SuRolepb(long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, Byte[] rowVersion, bool active, SS.SU.DTO.SuRole roleID,SCG.DB.DTO.Dbpb pBID)
		{
			this.updBy = updBy;
			this.updDate = updDate;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updPgm = updPgm;
			this.rowVersion = rowVersion;
			this.active = active;
			this.roleID = roleID;
			this.pBID = pBID;
		}
	
		/// <summary>
		/// Minimal constructor for class SuRolepb
		/// </summary>
		/// <param name="updBy">Initial <see cref="SuRolepb.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="SuRolepb.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="SuRolepb.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="SuRolepb.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="SuRolepb.UpdPgm" /> value</param>
		/// <param name="active">Initial <see cref="SuRolepb.Active" /> value</param>
		public SuRolepb(long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, bool active)
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
		/// Gets or sets the RolePBID for the current SuRolepb
		/// </summary>
		public virtual long RolePBID
		{
			get { return this.rolePBID; }
			set { this.rolePBID = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdBy for the current SuRolepb
		/// </summary>
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdDate for the current SuRolepb
		/// </summary>
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreBy for the current SuRolepb
		/// </summary>
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreDate for the current SuRolepb
		/// </summary>
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdPgm for the current SuRolepb
		/// </summary>
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}
		
		/// <summary>
		/// Gets or sets the RowVersion for the current SuRolepb
		/// </summary>
		public virtual byte[] RowVersion
		{
			get { return this.rowVersion; }
			set { this.rowVersion = value; }
		}
		
		/// <summary>
		/// Gets or sets the Active for the current SuRolepb
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		
		/// <summary>
		/// Gets or sets the RoleID for the current SuRolepb
		/// </summary>
		public virtual SS.SU.DTO.SuRole RoleID
		{
			get { return this.roleID; }
			set { this.roleID = value; }
		}
		
		/// <summary>
		/// Gets or sets the PBID for the current SuRolepb
		/// </summary>
		public virtual SCG.DB.DTO.Dbpb PBID
		{
			get { return this.pBID; }
			set { this.pBID = value; }
		}
		
		#endregion
	}
}
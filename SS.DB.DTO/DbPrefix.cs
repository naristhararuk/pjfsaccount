//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by NHibernate.
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

using System;

namespace SS.DB.DTO
{
	/// <summary>
	/// POJO for DbPrefix. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class DbPrefix
	{
		#region Fields
		
		private short prefixid;
		private string comment;
		private long updBy;
		private DateTime updDate;
		private long creBy;
		private DateTime creDate;
		private string updPgm;
		private Byte[] rowVersion;
		private bool active;

		#endregion

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the DbPrefix class
		/// </summary>
		public DbPrefix()
		{
		}

		public DbPrefix(short prefixid)
		{
			this.prefixid = prefixid;
		}
	
		/// <summary>
		/// Initializes a new instance of the DbPrefix class
		/// </summary>
		/// <param name="comment">Initial <see cref="DbPrefix.Comment" /> value</param>
		/// <param name="updBy">Initial <see cref="DbPrefix.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="DbPrefix.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="DbPrefix.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="DbPrefix.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="DbPrefix.UpdPgm" /> value</param>
		/// <param name="rowVersion">Initial <see cref="DbPrefix.RowVersion" /> value</param>
		/// <param name="active">Initial <see cref="DbPrefix.Active" /> value</param>
		public DbPrefix(string comment, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, Byte[] rowVersion, bool active)
		{
			this.comment = comment;
			this.updBy = updBy;
			this.updDate = updDate;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updPgm = updPgm;
			this.rowVersion = rowVersion;
			this.active = active;
		}
	
		/// <summary>
		/// Minimal constructor for class DbPrefix
		/// </summary>
		/// <param name="updBy">Initial <see cref="DbPrefix.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="DbPrefix.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="DbPrefix.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="DbPrefix.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="DbPrefix.UpdPgm" /> value</param>
		/// <param name="active">Initial <see cref="DbPrefix.Active" /> value</param>
		public DbPrefix(long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, bool active)
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
		/// Gets or sets the Prefixid for the current DbPrefix
		/// </summary>
		public virtual short Prefixid
		{
			get { return this.prefixid; }
			set { this.prefixid = value; }
		}
		
		/// <summary>
		/// Gets or sets the Comment for the current DbPrefix
		/// </summary>
		public virtual string Comment
		{
			get { return this.comment; }
			set { this.comment = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdBy for the current DbPrefix
		/// </summary>
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdDate for the current DbPrefix
		/// </summary>
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreBy for the current DbPrefix
		/// </summary>
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreDate for the current DbPrefix
		/// </summary>
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdPgm for the current DbPrefix
		/// </summary>
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}
		
		/// <summary>
		/// Gets or sets the RowVersion for the current DbPrefix
		/// </summary>
		public virtual Byte[] RowVersion
		{
			get { return this.rowVersion; }
			set { this.rowVersion = value; }
		}
		
		/// <summary>
		/// Gets or sets the Active for the current DbPrefix
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		
		#endregion
	}
}

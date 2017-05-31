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
	/// POJO for DbCity. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class DbCity
	{
		#region Fields
		
		private short cityid;
		private string comment;
		private long updBy;
		private DateTime updDate;
		private long creBy;
		private DateTime creDate;
		private string updPgm;
		private Byte[] rowVersion;
		private bool active;
		private SS.DB.DTO.DbProvince province;

		#endregion

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the DbCity class
		/// </summary>
		public DbCity()
		{
		}

		public DbCity(short cityid)
		{
			this.cityid = cityid;
		}
	
		/// <summary>
		/// Initializes a new instance of the DbCity class
		/// </summary>
		/// <param name="comment">Initial <see cref="DbCity.Comment" /> value</param>
		/// <param name="updBy">Initial <see cref="DbCity.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="DbCity.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="DbCity.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="DbCity.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="DbCity.UpdPgm" /> value</param>
		/// <param name="rowVersion">Initial <see cref="DbCity.RowVersion" /> value</param>
		/// <param name="active">Initial <see cref="DbCity.Active" /> value</param>
		/// <param name="province">Initial <see cref="DbCity.Province" /> value</param>
		public DbCity(string comment, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, Byte[] rowVersion, bool active, SS.DB.DTO.DbProvince province)
		{
			this.comment = comment;
			this.updBy = updBy;
			this.updDate = updDate;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updPgm = updPgm;
			this.rowVersion = rowVersion;
			this.active = active;
			this.province = province;
		}
	
		/// <summary>
		/// Minimal constructor for class DbCity
		/// </summary>
		/// <param name="updBy">Initial <see cref="DbCity.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="DbCity.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="DbCity.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="DbCity.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="DbCity.UpdPgm" /> value</param>
		/// <param name="active">Initial <see cref="DbCity.Active" /> value</param>
		public DbCity(long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, bool active)
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
		/// Gets or sets the Cityid for the current DbCity
		/// </summary>
		public virtual short Cityid
		{
			get { return this.cityid; }
			set { this.cityid = value; }
		}
		
		/// <summary>
		/// Gets or sets the Comment for the current DbCity
		/// </summary>
		public virtual string Comment
		{
			get { return this.comment; }
			set { this.comment = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdBy for the current DbCity
		/// </summary>
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdDate for the current DbCity
		/// </summary>
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreBy for the current DbCity
		/// </summary>
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreDate for the current DbCity
		/// </summary>
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdPgm for the current DbCity
		/// </summary>
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}
		
		/// <summary>
		/// Gets or sets the RowVersion for the current DbCity
		/// </summary>
		public virtual Byte[] RowVersion
		{
			get { return this.rowVersion; }
			set { this.rowVersion = value; }
		}
		
		/// <summary>
		/// Gets or sets the Active for the current DbCity
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		
		/// <summary>
		/// Gets or sets the Province for the current DbCity
		/// </summary>
		public virtual SS.DB.DTO.DbProvince Province
		{
			get { return this.province; }
			set { this.province = value; }
		}
		
		#endregion
	}
}
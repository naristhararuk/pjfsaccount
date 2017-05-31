//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by NHibernate.
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

using System;

namespace SCG.DB.DTO
{
	/// <summary>
	/// POJO for DbDocumentRunning. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class DbDocumentRunning
	{
		#region Fields
		
		private int runningID;
		private int year;
		private long runningNo;
		private bool active;
		private long creBy;
		private DateTime creDate;
		private long updBy;
		private DateTime updDate;
		private string updPgm;
		private byte[] rowVersion;
		private int documentTypeID;
		private SCG.DB.DTO.DbCompany companyID;

		#endregion

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the DbDocumentRunning class
		/// </summary>
		public DbDocumentRunning()
		{
		}

		public DbDocumentRunning(int runningID)
		{
			this.runningID = runningID;
		}
	
		/// <summary>
		/// Initializes a new instance of the DbDocumentRunning class
		/// </summary>
		/// <param name="year">Initial <see cref="DbDocumentRunning.Year" /> value</param>
		/// <param name="runningNo">Initial <see cref="DbDocumentRunning.RunningNo" /> value</param>
		/// <param name="active">Initial <see cref="DbDocumentRunning.Active" /> value</param>
		/// <param name="creBy">Initial <see cref="DbDocumentRunning.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="DbDocumentRunning.CreDate" /> value</param>
		/// <param name="updBy">Initial <see cref="DbDocumentRunning.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="DbDocumentRunning.UpdDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="DbDocumentRunning.UpdPgm" /> value</param>
		/// <param name="rowVersion">Initial <see cref="DbDocumentRunning.RowVersion" /> value</param>
		/// <param name="documentTypeID">Initial <see cref="DbDocumentRunning.DocumentTypeID" /> value</param>
		/// <param name="companyID">Initial <see cref="DbDocumentRunning.CompanyID" /> value</param>
		public DbDocumentRunning(int year, long runningNo, bool active, long creBy, DateTime creDate, long updBy, DateTime updDate, string updPgm, byte[] rowVersion, int documentTypeID, SCG.DB.DTO.DbCompany companyID)
		{
			this.year = year;
			this.runningNo = runningNo;
			this.active = active;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updBy = updBy;
			this.updDate = updDate;
			this.updPgm = updPgm;
			this.rowVersion = rowVersion;
			this.documentTypeID = documentTypeID;
			this.companyID = companyID;
		}
	
		/// <summary>
		/// Minimal constructor for class DbDocumentRunning
		/// </summary>
		/// <param name="active">Initial <see cref="DbDocumentRunning.Active" /> value</param>
		/// <param name="creBy">Initial <see cref="DbDocumentRunning.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="DbDocumentRunning.CreDate" /> value</param>
		/// <param name="updBy">Initial <see cref="DbDocumentRunning.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="DbDocumentRunning.UpdDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="DbDocumentRunning.UpdPgm" /> value</param>
		public DbDocumentRunning(bool active, long creBy, DateTime creDate, long updBy, DateTime updDate, string updPgm)
		{
			this.active = active;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updBy = updBy;
			this.updDate = updDate;
			this.updPgm = updPgm;
		}
		#endregion
	
		#region Properties
		
		/// <summary>
		/// Gets or sets the RunningID for the current DbDocumentRunning
		/// </summary>
		public virtual int RunningID
		{
			get { return this.runningID; }
			set { this.runningID = value; }
		}
		
		/// <summary>
		/// Gets or sets the Year for the current DbDocumentRunning
		/// </summary>
		public virtual int Year
		{
			get { return this.year; }
			set { this.year = value; }
		}
		
		/// <summary>
		/// Gets or sets the RunningNo for the current DbDocumentRunning
		/// </summary>
		public virtual long RunningNo
		{
			get { return this.runningNo; }
			set { this.runningNo = value; }
		}
		
		/// <summary>
		/// Gets or sets the Active for the current DbDocumentRunning
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreBy for the current DbDocumentRunning
		/// </summary>
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreDate for the current DbDocumentRunning
		/// </summary>
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdBy for the current DbDocumentRunning
		/// </summary>
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdDate for the current DbDocumentRunning
		/// </summary>
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdPgm for the current DbDocumentRunning
		/// </summary>
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}
		
		/// <summary>
		/// Gets or sets the RowVersion for the current DbDocumentRunning
		/// </summary>
		public virtual byte[] RowVersion
		{
			get { return this.rowVersion; }
			set { this.rowVersion = value; }
		}
		
		/// <summary>
		/// Gets or sets the DocumentTypeID for the current DbDocumentRunning
		/// </summary>
		public virtual int DocumentTypeID
		{
			get { return this.documentTypeID; }
			set { this.documentTypeID = value; }
		}
		
		/// <summary>
		/// Gets or sets the CompanyID for the current DbDocumentRunning
		/// </summary>
		public virtual SCG.DB.DTO.DbCompany CompanyID
		{
			get { return this.companyID; }
			set { this.companyID = value; }
		}
		
		#endregion
	}
}
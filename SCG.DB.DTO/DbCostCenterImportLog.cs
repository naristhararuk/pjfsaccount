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
	/// POJO for DbCostCenterImportLog. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class DbCostCenterImportLog
	{
		#region Fields
		
		private long iD;
		private string costCenterCode;
		private DateTime? validFrom;
		private DateTime? expireDate;
		private string description;
		private string companyCode;
		private bool active;
		private int errorCode;
		private string message;
		private long line;

		#endregion

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the DbCostCenterImportLog class
		/// </summary>
		public DbCostCenterImportLog()
		{
		}

		public DbCostCenterImportLog(long iD)
		{
			this.iD = iD;
		}
	
		/// <summary>
		/// Initializes a new instance of the DbCostCenterImportLog class
		/// </summary>
		/// <param name="costCenterCode">Initial <see cref="DbCostCenterImportLog.CostCenterCode" /> value</param>
		/// <param name="validFrom">Initial <see cref="DbCostCenterImportLog.ValidFrom" /> value</param>
		/// <param name="expireDate">Initial <see cref="DbCostCenterImportLog.ExpireDate" /> value</param>
		/// <param name="description">Initial <see cref="DbCostCenterImportLog.Description" /> value</param>
		/// <param name="companyCode">Initial <see cref="DbCostCenterImportLog.CompanyCode" /> value</param>
		/// <param name="active">Initial <see cref="DbCostCenterImportLog.Active" /> value</param>
		/// <param name="errorCode">Initial <see cref="DbCostCenterImportLog.ErrorCode" /> value</param>
		/// <param name="message">Initial <see cref="DbCostCenterImportLog.Message" /> value</param>
		/// <param name="line">Initial <see cref="DbCostCenterImportLog.Line" /> value</param>
		public DbCostCenterImportLog(string costCenterCode, DateTime validFrom, DateTime expireDate, string description, string companyCode, bool active, int errorCode, string message, long line)
		{
			this.costCenterCode = costCenterCode;
			this.validFrom = validFrom;
			this.expireDate = expireDate;
			this.description = description;
			this.companyCode = companyCode;
			this.active = active;
			this.errorCode = errorCode;
			this.message = message;
			this.line = line;
		}
	
		/// <summary>
		/// Minimal constructor for class DbCostCenterImportLog
		/// </summary>
		/// <param name="active">Initial <see cref="DbCostCenterImportLog.Active" /> value</param>
		public DbCostCenterImportLog(bool active)
		{
			this.active = active;
		}
		#endregion
	
		#region Properties
		
		/// <summary>
		/// Gets or sets the ID for the current DbCostCenterImportLog
		/// </summary>
		public virtual long ID
		{
			get { return this.iD; }
			set { this.iD = value; }
		}
		
		/// <summary>
		/// Gets or sets the CostCenterCode for the current DbCostCenterImportLog
		/// </summary>
		public virtual string CostCenterCode
		{
			get { return this.costCenterCode; }
			set { this.costCenterCode = value; }
		}
		
		/// <summary>
		/// Gets or sets the ValidFrom for the current DbCostCenterImportLog
		/// </summary>
		public virtual DateTime? ValidFrom
		{
			get { return this.validFrom; }
			set { this.validFrom = value; }
		}
		
		/// <summary>
		/// Gets or sets the ExpireDate for the current DbCostCenterImportLog
		/// </summary>
		public virtual DateTime? ExpireDate
		{
			get { return this.expireDate; }
			set { this.expireDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the Description for the current DbCostCenterImportLog
		/// </summary>
		public virtual string Description
		{
			get { return this.description; }
			set { this.description = value; }
		}
		
		/// <summary>
		/// Gets or sets the CompanyCode for the current DbCostCenterImportLog
		/// </summary>
		public virtual string CompanyCode
		{
			get { return this.companyCode; }
			set { this.companyCode = value; }
		}
		
		/// <summary>
		/// Gets or sets the Active for the current DbCostCenterImportLog
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		
		/// <summary>
		/// Gets or sets the ErrorCode for the current DbCostCenterImportLog
		/// </summary>
		public virtual int ErrorCode
		{
			get { return this.errorCode; }
			set { this.errorCode = value; }
		}
		
		/// <summary>
		/// Gets or sets the Message for the current DbCostCenterImportLog
		/// </summary>
		public virtual string Message
		{
			get { return this.message; }
			set { this.message = value; }
		}
		
		/// <summary>
		/// Gets or sets the Line for the current DbCostCenterImportLog
		/// </summary>
		public virtual long Line
		{
			get { return this.line; }
			set { this.line = value; }
		}
		
		#endregion
	}
}

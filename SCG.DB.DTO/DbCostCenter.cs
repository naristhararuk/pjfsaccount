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
	/// POJO for DbCostCenter. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class DbCostCenter
	{
		#region Fields
		
		private long costCenterID;
		private string costCenterCode;
		private DateTime valid;
		private DateTime expire;
		private string description;
		private bool actualPrimaryCosts;
		private bool active;
		private long creBy;
		private DateTime creDate;
		private long updBy;
		private DateTime updDate;
		private string updPgm;
		private Byte[] rowVersion;
		private SCG.DB.DTO.DbCompany companyID;
		private string companyCode;
        private string businessArea;
        private string profitCenter;

		#endregion

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the DbCostCenter class
		/// </summary>
		public DbCostCenter()
		{
		}

		public DbCostCenter(long costCenterID)
		{
			this.costCenterID = costCenterID;
		}
	
		/// <summary>
		/// Initializes a new instance of the DbCostCenter class
		/// </summary>
		/// <param name="costCenterCode">Initial <see cref="DbCostCenter.CostCenterCode" /> value</param>
		/// <param name="valid">Initial <see cref="DbCostCenter.Valid" /> value</param>
		/// <param name="expire">Initial <see cref="DbCostCenter.Expire" /> value</param>
		/// <param name="description">Initial <see cref="DbCostCenter.Description" /> value</param>
		/// <param name="actualPrimaryCosts">Initial <see cref="DbCostCenter.ActualPrimaryCosts" /> value</param>
		/// <param name="active">Initial <see cref="DbCostCenter.Active" /> value</param>
		/// <param name="creBy">Initial <see cref="DbCostCenter.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="DbCostCenter.CreDate" /> value</param>
		/// <param name="updBy">Initial <see cref="DbCostCenter.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="DbCostCenter.UpdDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="DbCostCenter.UpdPgm" /> value</param>
		/// <param name="rowVersion">Initial <see cref="DbCostCenter.RowVersion" /> value</param>
		/// <param name="companyID">Initial <see cref="DbCostCenter.CompanyID" /> value</param>
		/// <param name="companyCode">Initial <see cref="DbCostCenter.CompanyCode" /> value</param>
		public DbCostCenter(string costCenterCode, DateTime valid, DateTime expire, string description, bool actualPrimaryCosts, bool active, long creBy, DateTime creDate, long updBy, DateTime updDate, string updPgm, Byte[] rowVersion, SCG.DB.DTO.DbCompany companyID, string companyCode,string businessArea,string profitCenter)
		{
			this.costCenterCode = costCenterCode;
			this.valid = valid;
			this.expire = expire;
			this.description = description;
			this.actualPrimaryCosts = actualPrimaryCosts;
			this.active = active;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updBy = updBy;
			this.updDate = updDate;
			this.updPgm = updPgm;
			this.rowVersion = rowVersion;
			this.companyID = companyID;
			this.companyCode = companyCode;
            this.BusinessArea = businessArea;
            this.ProfitCenter = profitCenter;
		}
	
		/// <summary>
		/// Minimal constructor for class DbCostCenter
		/// </summary>
		/// <param name="costCenterCode">Initial <see cref="DbCostCenter.CostCenterCode" /> value</param>
		/// <param name="valid">Initial <see cref="DbCostCenter.Valid" /> value</param>
		/// <param name="expire">Initial <see cref="DbCostCenter.Expire" /> value</param>
		/// <param name="active">Initial <see cref="DbCostCenter.Active" /> value</param>
		/// <param name="creBy">Initial <see cref="DbCostCenter.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="DbCostCenter.CreDate" /> value</param>
		/// <param name="updBy">Initial <see cref="DbCostCenter.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="DbCostCenter.UpdDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="DbCostCenter.UpdPgm" /> value</param>
		public DbCostCenter(string costCenterCode, DateTime valid, DateTime expire, bool active, long creBy, DateTime creDate, long updBy, DateTime updDate, string updPgm)
		{
			this.costCenterCode = costCenterCode;
			this.valid = valid;
			this.expire = expire;
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
		/// Gets or sets the CostCenterID for the current DbCostCenter
		/// </summary>
		public virtual long CostCenterID
		{
			get { return this.costCenterID; }
			set { this.costCenterID = value; }
		}
		
		/// <summary>
		/// Gets or sets the CostCenterCode for the current DbCostCenter
		/// </summary>
		public virtual string CostCenterCode
		{
			get { return this.costCenterCode; }
			set { this.costCenterCode = value; }
		}
		
		/// <summary>
		/// Gets or sets the Valid for the current DbCostCenter
		/// </summary>
		public virtual DateTime Valid
		{
			get { return this.valid; }
			set { this.valid = value; }
		}
		
		/// <summary>
		/// Gets or sets the Expire for the current DbCostCenter
		/// </summary>
		public virtual DateTime Expire
		{
			get { return this.expire; }
			set { this.expire = value; }
		}
		
		/// <summary>
		/// Gets or sets the Description for the current DbCostCenter
		/// </summary>
		public virtual string Description
		{
			get { return this.description; }
			set { this.description = value; }
		}
		
		/// <summary>
		/// Gets or sets the ActualPrimaryCosts for the current DbCostCenter
		/// </summary>
		public virtual bool ActualPrimaryCosts
		{
			get { return this.actualPrimaryCosts; }
			set { this.actualPrimaryCosts = value; }
		}
		
		/// <summary>
		/// Gets or sets the Active for the current DbCostCenter
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreBy for the current DbCostCenter
		/// </summary>
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreDate for the current DbCostCenter
		/// </summary>
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdBy for the current DbCostCenter
		/// </summary>
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdDate for the current DbCostCenter
		/// </summary>
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdPgm for the current DbCostCenter
		/// </summary>
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}
		
		/// <summary>
		/// Gets or sets the RowVersion for the current DbCostCenter
		/// </summary>
		public virtual Byte[] RowVersion
		{
			get { return this.rowVersion; }
			set { this.rowVersion = value; }
		}
		
		/// <summary>
		/// Gets or sets the CompanyID for the current DbCostCenter
		/// </summary>
		public virtual SCG.DB.DTO.DbCompany CompanyID
		{
			get { return this.companyID; }
			set { this.companyID = value; }
		}
		
		/// <summary>
		/// Gets or sets the CompanyCode for the current DbCostCenter
		/// </summary>
		public virtual string CompanyCode
		{
			get { return this.companyCode; }
			set { this.companyCode = value; }
		}

        /// <summary>
        /// Gets or sets the BusinessArea for the current DbCostCenter
        /// </summary>
        public virtual string BusinessArea
        {
            get { return this.businessArea; }
            set { this.businessArea = value; }
        }

        /// <summary>
        /// Gets or sets the ProfitCenter for the current DbCostCenter
        /// </summary>
        public virtual string ProfitCenter
        {
            get { return this.profitCenter; }
            set { this.profitCenter = value; }
        }
		
		#endregion
	}
}
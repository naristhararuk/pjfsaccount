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
	/// POJO for TmpDbCostCenter. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class TmpDbCostCenter
	{
		#region Fields
		
		private long costCenterID;
		private long companyID;
		private string companyCode;
		private string costCenterCode;
		private DateTime? valid;
		private DateTime? expire;
		private string description;
		private bool actualPrimaryCosts;
		private bool active;
		private long creBy;
		private DateTime creDate;
		private long updBy;
		private DateTime updDate;
		private string updPgm;
		private string rowVersion;
		private long line;
        private string businessArea;
        private string profitCenter;

		#endregion

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the TmpDbCostCenter class
		/// </summary>
		public TmpDbCostCenter()
		{
		}

		public TmpDbCostCenter(long costCenterID)
		{
			this.costCenterID = costCenterID;
		}
	
		/// <summary>
		/// Initializes a new instance of the TmpDbCostCenter class
		/// </summary>
		/// <param name="companyID">Initial <see cref="TmpDbCostCenter.CompanyID" /> value</param>
		/// <param name="companyCode">Initial <see cref="TmpDbCostCenter.CompanyCode" /> value</param>
		/// <param name="costCenterCode">Initial <see cref="TmpDbCostCenter.CostCenterCode" /> value</param>
		/// <param name="valid">Initial <see cref="TmpDbCostCenter.Valid" /> value</param>
		/// <param name="expire">Initial <see cref="TmpDbCostCenter.Expire" /> value</param>
		/// <param name="description">Initial <see cref="TmpDbCostCenter.Description" /> value</param>
		/// <param name="actualPrimaryCosts">Initial <see cref="TmpDbCostCenter.ActualPrimaryCosts" /> value</param>
		/// <param name="active">Initial <see cref="TmpDbCostCenter.Active" /> value</param>
		/// <param name="creBy">Initial <see cref="TmpDbCostCenter.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="TmpDbCostCenter.CreDate" /> value</param>
		/// <param name="updBy">Initial <see cref="TmpDbCostCenter.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="TmpDbCostCenter.UpdDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="TmpDbCostCenter.UpdPgm" /> value</param>
		/// <param name="rowVersion">Initial <see cref="TmpDbCostCenter.RowVersion" /> value</param>
		/// <param name="line">Initial <see cref="TmpDbCostCenter.Line" /> value</param>
		public TmpDbCostCenter(long companyID, string companyCode, string costCenterCode, DateTime valid, DateTime expire, string description, bool actualPrimaryCosts, bool active, long creBy, DateTime creDate, long updBy, DateTime updDate, string updPgm, string rowVersion, long line)
		{
			this.companyID = companyID;
			this.companyCode = companyCode;
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
			this.line = line;
		}
	
		/// <summary>
		/// Minimal constructor for class TmpDbCostCenter
		/// </summary>
		/// <param name="active">Initial <see cref="TmpDbCostCenter.Active" /> value</param>
		/// <param name="creBy">Initial <see cref="TmpDbCostCenter.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="TmpDbCostCenter.CreDate" /> value</param>
		/// <param name="updBy">Initial <see cref="TmpDbCostCenter.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="TmpDbCostCenter.UpdDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="TmpDbCostCenter.UpdPgm" /> value</param>
		/// <param name="line">Initial <see cref="TmpDbCostCenter.Line" /> value</param>
		public TmpDbCostCenter(bool active, long creBy, DateTime creDate, long updBy, DateTime updDate, string updPgm, long line)
		{
			this.active = active;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updBy = updBy;
			this.updDate = updDate;
			this.updPgm = updPgm;
			this.line = line;
		}
		#endregion
	
		#region Properties
		
		/// <summary>
		/// Gets or sets the CostCenterID for the current TmpDbCostCenter
		/// </summary>
		public virtual long CostCenterID
		{
			get { return this.costCenterID; }
			set { this.costCenterID = value; }
		}
		
		/// <summary>
		/// Gets or sets the CompanyID for the current TmpDbCostCenter
		/// </summary>
		public virtual long CompanyID
		{
			get { return this.companyID; }
			set { this.companyID = value; }
		}
		
		/// <summary>
		/// Gets or sets the CompanyCode for the current TmpDbCostCenter
		/// </summary>
		public virtual string CompanyCode
		{
			get { return this.companyCode; }
			set { this.companyCode = value; }
		}
		
		/// <summary>
		/// Gets or sets the CostCenterCode for the current TmpDbCostCenter
		/// </summary>
		public virtual string CostCenterCode
		{
			get { return this.costCenterCode; }
			set { this.costCenterCode = value; }
		}
		
		/// <summary>
		/// Gets or sets the Valid for the current TmpDbCostCenter
		/// </summary>
		public virtual DateTime? Valid
		{
			get { return this.valid; }
			set { this.valid = value; }
		}
		
		/// <summary>
		/// Gets or sets the Expire for the current TmpDbCostCenter
		/// </summary>
		public virtual DateTime? Expire
		{
			get { return this.expire; }
			set { this.expire = value; }
		}
		
		/// <summary>
		/// Gets or sets the Description for the current TmpDbCostCenter
		/// </summary>
		public virtual string Description
		{
			get { return this.description; }
			set { this.description = value; }
		}
		
		/// <summary>
		/// Gets or sets the ActualPrimaryCosts for the current TmpDbCostCenter
		/// </summary>
		public virtual bool ActualPrimaryCosts
		{
			get { return this.actualPrimaryCosts; }
			set { this.actualPrimaryCosts = value; }
		}
		
		/// <summary>
		/// Gets or sets the Active for the current TmpDbCostCenter
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreBy for the current TmpDbCostCenter
		/// </summary>
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreDate for the current TmpDbCostCenter
		/// </summary>
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdBy for the current TmpDbCostCenter
		/// </summary>
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdDate for the current TmpDbCostCenter
		/// </summary>
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdPgm for the current TmpDbCostCenter
		/// </summary>
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}
		
		/// <summary>
		/// Gets or sets the RowVersion for the current TmpDbCostCenter
		/// </summary>
		public virtual string RowVersion
		{
			get { return this.rowVersion; }
			set { this.rowVersion = value; }
		}
		
		/// <summary>
		/// Gets or sets the Line for the current TmpDbCostCenter
		/// </summary>
		public virtual long Line
		{
			get { return this.line; }
			set { this.line = value; }
		}

        /// <summary>
        /// Gets or sets the BusinessArea for the current TmpDbCostCenter
        /// </summary>
        public virtual string BusinessArea
        {
            get { return this.businessArea; }
            set { this.businessArea = value; }
        }

        /// <summary>
        /// Gets or sets the ProfitCenter for the current TmpDbCostCenter
        /// </summary>
        public virtual string ProfitCenter
        {
            get { return this.profitCenter; }
            set { this.profitCenter = value; }
        }
		
		#endregion
	}
}

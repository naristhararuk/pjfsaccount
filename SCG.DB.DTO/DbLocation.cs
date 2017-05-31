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
	/// POJO for DbLocation. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class DbLocation
	{
		#region Fields
		
		private long locationID;
		private string locationCode;
		private long updBy;
		private DateTime updDate;
		private long creBy;
		private DateTime creDate;
		private string updPgm;
		private Byte[] rowVersion;
		private bool active;
        private bool isAllowImportExpense;
        private string comment;
        private string locationName;
		private SCG.DB.DTO.DbCompany companyID;
        //private SCG.DB.DTO.DbCompany companyCode;
        private string companyCode;
        private long? defaultPBID;

		#endregion

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the DbLocation class
		/// </summary>
		public DbLocation()
		{
		}

		public DbLocation(long locationID)
		{
			this.locationID = locationID;
		}
	
		/// <summary>
		/// Initializes a new instance of the DbLocation class
		/// </summary>
		/// <param name="locationCode">Initial <see cref="DbLocation.LocationCode" /> value</param>
		/// <param name="updBy">Initial <see cref="DbLocation.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="DbLocation.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="DbLocation.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="DbLocation.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="DbLocation.UpdPgm" /> value</param>
		/// <param name="rowVersion">Initial <see cref="DbLocation.RowVersion" /> value</param>
		/// <param name="active">Initial <see cref="DbLocation.Active" /> value</param>
		/// <param name="company">Initial <see cref="DbLocation.Company" /> value</param>

        public DbLocation(string comment, string locationName, string locationCode, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, Byte[] rowVersion, bool active, SCG.DB.DTO.DbCompany companyID, string companyCode, bool isAllowImportExpense)
		{
            this.comment = comment;
            this.locationName = locationName;
			this.locationCode = locationCode;
			this.updBy = updBy;
			this.updDate = updDate;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updPgm = updPgm;
			this.rowVersion = rowVersion;
			this.active = active;
			this.companyID = companyID;
            this.companyCode = companyCode;
            this.isAllowImportExpense = isAllowImportExpense;
		}
	
		/// <summary>
		/// Minimal constructor for class DbLocation
		/// </summary>
		/// <param name="locationCode">Initial <see cref="DbLocation.LocationCode" /> value</param>
		/// <param name="updBy">Initial <see cref="DbLocation.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="DbLocation.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="DbLocation.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="DbLocation.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="DbLocation.UpdPgm" /> value</param>
		/// <param name="active">Initial <see cref="DbLocation.Active" /> value</param>
        public DbLocation(string locationCode, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, bool active, bool isAllowImportExpense)
		{
			this.locationCode = locationCode;
			this.updBy = updBy;
			this.updDate = updDate;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updPgm = updPgm;
			this.active = active;
            this.isAllowImportExpense = isAllowImportExpense;
		}
		#endregion
	
		#region Properties
		
		/// <summary>
		/// Gets or sets the LocationID for the current DbLocation
		/// </summary>
		public virtual long LocationID
		{
			get { return this.locationID; }
			set { this.locationID = value; }
		}
		
		/// <summary>
		/// Gets or sets the LocationCode for the current DbLocation
		/// </summary>
        /// 
        public virtual bool IsAllowImportExpense
        {
            get
            {
                return this.isAllowImportExpense;
            }
            set
            {
                this.isAllowImportExpense = value;
            }
        }
        
 
        public virtual string Comment
		{
            get { return this.comment; }
            set { this.comment = value; }
		}
        public virtual string LocationName
		{
            get { return this.locationName; }
            set { this.locationName = value; }
		}
		public virtual string LocationCode
		{
			get { return this.locationCode; }
			set { this.locationCode = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdBy for the current DbLocation
		/// </summary>
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdDate for the current DbLocation
		/// </summary>
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreBy for the current DbLocation
		/// </summary>
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreDate for the current DbLocation
		/// </summary>
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdPgm for the current DbLocation
		/// </summary>
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}
		
		/// <summary>
		/// Gets or sets the RowVersion for the current DbLocation
		/// </summary>
		public virtual Byte[] RowVersion
		{
			get { return this.rowVersion; }
			set { this.rowVersion = value; }
		}
		
		/// <summary>
		/// Gets or sets the Active for the current DbLocation
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		
		/// <summary>
        /// Gets or sets the CompanyID for the current DbCompany
		/// </summary>
		public virtual SCG.DB.DTO.DbCompany CompanyID
		{
			get { return this.companyID; }
			set { this.companyID = value; }
		}

        /// <summary>
        /// Gets or sets the CompanyCode for the current DbCompany
        /// </summary>
        public virtual string CompanyCode
        {
            get { return this.companyCode; }
            set { this.companyCode = value; }
        }

        public virtual long? DefaultPBID
        {
            get { return this.defaultPBID; }
            set { this.defaultPBID = value; }
        }
		
		#endregion
	}
}

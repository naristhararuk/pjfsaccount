using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO
{
    [Serializable]
    public class DbCompanyTax
    {
        #region Fields
        private long id;
        private long taxID;
        private long companyID;
        private double rate;
        private double rateNonDeduct;
        private bool useParentRate;
        private bool disable;
        private long updBy;
        private DateTime updDate;
        private long creBy;
        private DateTime creDate;
        private string updPgm;
        private byte[] rowVersion;
    
        #endregion

        #region Constructors
		
		/// <summary>
		/// Initializes a new instance of the DbTax class
		/// </summary>
		public DbCompanyTax()
		{
		}

        public DbCompanyTax(long id)
		{
            this.id = id;
		}
	
		/// <summary>
		/// Initializes a new instance of the DbTax class
		/// </summary>
        /// <param name="taxCode">Initial <see cref="DbTax.taxCode" /> value</param>
        /// <param name="taxName">Initial <see cref="DbTax.taxName" /> value</param>
        /// <param name="gl">Initial <see cref="DbTax.gl" /> value</param>
        /// <param name="rate">Initial <see cref="DbTax.rate" /> value</param>
        /// <param name="rateNonDeduct">Initial <see cref="DbTax.rateNonDeduct" /> value</param>
		/// <param name="updBy">Initial <see cref="DbTax.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="DbTax.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="DbTax.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="DbTax.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="DbTax.UpdPgm" /> value</param>
		/// <param name="rowVersion">Initial <see cref="DbTax.RowVersion" /> value</param>
		/// <param name="active">Initial <see cref="DbTax.Active" /> value</param>
        public DbCompanyTax(long taxID,long companyID, double rate, double rateNonDeduct, bool useParentRate, bool disable, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, byte[] rowVersion)
		{
            this.taxID = taxID;
            this.companyID = companyID;
            this.rate           = rate;
            this.rateNonDeduct  = rateNonDeduct;
            this.useParentRate = useParentRate;
            this.disable = disable;
			this.updBy          = updBy;
			this.updDate        = updDate;
			this.creBy          = creBy;
			this.creDate        = creDate;
			this.updPgm         = updPgm;
			this.rowVersion     = rowVersion;

		}
        #endregion 

        #region Properties

        public virtual long ID
        {
            get { return this.id; }
            set { this.id = value; }
        }
        /// <summary>
        /// Gets or sets the taxID for the current DbTax
        /// </summary>
        public virtual long TaxID
        {
            get { return this.taxID; }
            set { this.taxID = value; }
        }

        /// <summary>
        /// Gets or sets the CompanyID for the current DbTax
        /// </summary>
        public virtual long CompanyID
        {
            get { return this.companyID; }
            set { this.companyID = value; }
        }

        /// <summary>
        /// Gets or sets the Rate for the current DbTax
        /// </summary>
        public virtual double Rate
        {
            get { return this.rate; }
            set { this.rate = value; }
        }

        /// <summary>
        /// Gets or sets the RateNonDeduct for the current DbTax
        /// </summary>
        public virtual double RateNonDeduct
        {
            get { return this.rateNonDeduct; }
            set { this.rateNonDeduct = value; }
        }

        /// <summary>
        /// Gets or sets the Active for the current DbTax
        /// </summary>
        public virtual bool UseParentRate
        {
            get { return this.useParentRate; }
            set { this.useParentRate = value; }
        }
        /// Gets or sets the ApplyAllCompany for the current DbTax
        /// </summary>
        public virtual bool Disable
        {
            get { return this.disable; }
            set { this.disable = value; }
        }
        /// <summary>
        /// Gets or sets the UpdBy for the current DbTax
        /// </summary>
        public virtual long UpdBy
        {
            get { return this.updBy; }
            set { this.updBy = value; }
        }

        /// <summary>
        /// Gets or sets the UpdDate for the current DbTax
        /// </summary>
        public virtual DateTime UpdDate
        {
            get { return this.updDate; }
            set { this.updDate = value; }
        }

        /// <summary>
        /// Gets or sets the CreBy for the current DbTax
        /// </summary>
        public virtual long CreBy
        {
            get { return this.creBy; }
            set { this.creBy = value; }
        }

        /// <summary>
        /// Gets or sets the CreDate for the current DbTax
        /// </summary>
        public virtual DateTime CreDate
        {
            get { return this.creDate; }
            set { this.creDate = value; }
        }

        /// <summary>
        /// Gets or sets the UpdPgm for the current DbTax
        /// </summary>
        public virtual string UpdPgm
        {
            get { return this.updPgm; }
            set { this.updPgm = value; }
        }

        /// <summary>
        /// Gets or sets the RowVersion for the current DbTax
        /// </summary>
        public virtual byte[] RowVersion
        {
            get { return this.rowVersion; }
            set { this.rowVersion = value; }
        }
        #endregion
    }
}

using System;

namespace SCG.DB.DTO
{

    [Serializable]
    public partial class DbAccountCompany
    {


        #region Fields

        private long id;
        private SCG.DB.DTO.DbAccount accountID;
        private SCG.DB.DTO.DbCompany companyID;
        private bool active;
        private long updBy;
        private DateTime updDate;
        private long creBy;
        private DateTime creDate;
        private byte[] rowVersion;
        private string updPgm;
        private string companyCode;
        private bool useParent;
        private int taxCode;
        private int costCenter;
        private int internalOrder;
        private int saleOrder;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DbAccount class
        /// </summary>
        public DbAccountCompany()
        {
        }

        public DbAccountCompany(long id)
        {
            this.id = id;
        }


        public DbAccountCompany(long id, SCG.DB.DTO.DbAccount accountID, SCG.DB.DTO.DbCompany companyID,string companyCode,bool useParent,int taxCode,int costCenter,int internalOrder,int saleOrder)
        {

            this.id = id;
            this.accountID = accountID;
            this.companyID = companyID;
            this.companyCode = companyCode;
            this.useParent = useParent;
            this.taxCode = taxCode;
            this.costCenter = costCenter;
            this.internalOrder = internalOrder;
            this.saleOrder = saleOrder;

        }

        public DbAccountCompany(bool active, long updBy, DateTime updDate, long creBy, DateTime creDate, byte[] rowVersion, string updPgm)
        {
            this.active = active;
            this.updBy = updBy;
            this.updDate = updDate;
            this.creBy = creBy;
            this.creDate = creDate;
            this.rowVersion = rowVersion;
            this.updPgm = updPgm;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the accountID for the current DbAccount
        /// </summary>
        public virtual long ID
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public virtual SCG.DB.DTO.DbAccount AccountID
        {
            get { return this.accountID; }
            set { this.accountID = value; }
        }
        public virtual SCG.DB.DTO.DbCompany CompanyID
        {
            get { return this.companyID; }
            set { this.companyID = value; }
        }


        public virtual long UpdBy
        {
            get { return this.updBy; }
            set { this.updBy = value; }
        }

        /// <summary>
        /// Gets or sets the UpdDate for the current DbAccount
        /// </summary>
        public virtual DateTime UpdDate
        {
            get { return this.updDate; }
            set { this.updDate = value; }
        }

        /// <summary>
        /// Gets or sets the CreBy for the current DbAccount
        /// </summary>
        public virtual long CreBy
        {
            get { return this.creBy; }
            set { this.creBy = value; }
        }

        /// <summary>
        /// Gets or sets the CreDate for the current DbAccount
        /// </summary>
        public virtual DateTime CreDate
        {
            get { return this.creDate; }
            set { this.creDate = value; }
        }

        /// <summary>
        /// Gets or sets the UpdPgm for the current DbAccount
        /// </summary>
        public virtual string UpdPgm
        {
            get { return this.updPgm; }
            set { this.updPgm = value; }
        }

        /// <summary>
        /// Gets or sets the RowVersion for the current DbAccount
        /// </summary>
        public virtual byte[] RowVersion
        {
            get { return this.rowVersion; }
            set { this.rowVersion = value; }
        }

        /// <summary>
        /// Gets or sets the Active for the current DbAccount
        /// </summary>
        public virtual bool Active
        {
            get { return this.active; }
            set { this.active = value; }
        }
        public virtual string CompanyCode
        {
            get { return this.companyCode; }
            set { this.companyCode = value; }
        }
        public virtual bool UseParent
        {
            get { return this.useParent; }
            set { this.useParent = value; }
        }
        public virtual int TaxCode
        {
            get { return this.taxCode; }
            set { this.taxCode = value; }
        }
        public virtual int CostCenter
        {
            get { return this.costCenter; }
            set { this.costCenter = value; }
        }
        public virtual int InternalOrder
        {
            get { return this.internalOrder; }
            set { this.internalOrder = value; }
        }
        public virtual int SaleOrder
        {
            get { return this.saleOrder; }
            set { this.saleOrder = value; }
        }
        #endregion
    }
}


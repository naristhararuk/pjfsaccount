﻿//------------------------------------------------------------------------------
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
    /// POJO for DbAccount. This class is autogenerated
    /// </summary>
    [Serializable]
    public partial class DbAccount
    {


        #region Fields

        private long accountID;
        private long expenseGroupID;
        private string accountCode;     
        private bool active;
        private long updBy;
        private DateTime updDate;
        private long creBy;
        private DateTime creDate;
        private string updPgm;
        private byte[] rowVersion;

        private bool saveAsDebtor;
        private bool saveAsVendor;
        private string sAPSpecialGL;
        private string sAPSpecialGLAssignment;
        private bool domesticRecommend;
        private bool foreignRecommend;
        private int taxCode;
        private int costCenter;
        private int internalOrder;
        private int saleOrder;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DbAccount class
        /// </summary>
        public DbAccount()
        {
        }

        public DbAccount(long accountID)
        {
            this.accountID = accountID;
        }

        /// <summary>
        /// Initializes a new instance of the DbAccount class
        /// </summary>
        /// <param name="expenseGroupID">Initial <see cref="DbAccount.ExpenseGroupID" /> value</param>
        /// <param name="accountCode">Initial <see cref="DbAccount.AccountCode" /> value</param>
        /// <param name="updBy">Initial <see cref="DbAccount.UpdBy" /> value</param>
        /// <param name="updDate">Initial <see cref="DbAccount.UpdDate" /> value</param>
        /// <param name="creBy">Initial <see cref="DbAccount.CreBy" /> value</param>
        /// <param name="creDate">Initial <see cref="DbAccount.CreDate" /> value</param>
        /// <param name="updPgm">Initial <see cref="DbAccount.UpdPgm" /> value</param>
        /// <param name="rowVersion">Initial <see cref="DbAccount.RowVersion" /> value</param>
        /// <param name="active">Initial <see cref="DbAccount.Active" /> value</param>

     
        public DbAccount(long expenseGroupID, string accountCode, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, byte[] rowVersion, bool active, bool saveAsDebtor, string sAPSpecialGL, bool domesticRecommend, bool foreignRecommend, string sAPSpecialGLAssignment,int taxCode,int costCenter,int internalOrder,int saleOrder)
        {
            this.saveAsDebtor=saveAsDebtor;
            this.sAPSpecialGLAssignment = sAPSpecialGLAssignment;
            this.sAPSpecialGL = sAPSpecialGL;
            this.domesticRecommend=domesticRecommend;
            this.foreignRecommend=foreignRecommend;
            
            this.expenseGroupID = expenseGroupID;
            this.accountCode = accountCode;
            this.updBy = updBy;
            this.updDate = updDate;
            this.creBy = creBy;
            this.creDate = creDate;
            this.updPgm = updPgm;
            this.rowVersion = rowVersion;
            this.active = active;
            this.taxCode = taxCode;
            this.costCenter = costCenter;
            this.internalOrder = internalOrder;
            this.saleOrder = saleOrder;
        }

        /// <summary>
        /// Minimal constructor for class DbAccount
        /// </summary>
        /// <param name="accountCode">Initial <see cref="DbAccount.AccountCode" /> value</param>
        /// <param name="updBy">Initial <see cref="DbAccount.UpdBy" /> value</param>
        /// <param name="updDate">Initial <see cref="DbAccount.UpdDate" /> value</param>
        /// <param name="creBy">Initial <see cref="DbAccount.CreBy" /> value</param>
        /// <param name="creDate">Initial <see cref="DbAccount.CreDate" /> value</param>
        /// <param name="updPgm">Initial <see cref="DbAccount.UpdPgm" /> value</param>
        /// <param name="active">Initial <see cref="DbAccount.Active" /> value</param>
        public DbAccount(string accountCode, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, bool active)
        {
            this.accountCode = accountCode;
            this.updBy = updBy;
            this.updDate = updDate;
            this.creBy = creBy;
            this.creDate = creDate;
            this.updPgm = updPgm;
            this.active = active;
        }
        #endregion

        #region Properties

        
        public virtual bool SaveAsDebtor
        {
            get { return this.saveAsDebtor; }
            set { this.saveAsDebtor = value; }
        }
        public virtual bool SaveAsVendor
        {
            get { return this.saveAsVendor; }
            set { this.saveAsVendor = value; }
        }
        public virtual bool DomesticRecommend
        {
            get { return this.domesticRecommend; }
            set { this.domesticRecommend = value; }
        }
        public virtual bool ForeignRecommend
        {
            get { return this.foreignRecommend; }
            set { this.foreignRecommend = value; }
        }
        
          
        public virtual string SAPSpecialGLAssignment
        {
            get { return this.sAPSpecialGLAssignment; }
            set { this.sAPSpecialGLAssignment = value; }
        }

        public virtual string SAPSpecialGL
        {
            get { return this.sAPSpecialGL; }
            set { this.sAPSpecialGL = value; }
        }
        /// <summary>
        /// Gets or sets the accountID for the current DbAccount
        /// </summary>
        public virtual long AccountID
        {
            get { return this.accountID; }
            set { this.accountID = value; }
        }

        /// <summary>
        /// Gets or sets the expenseGroupID for the current DbAccount
        /// </summary>
        public virtual long ExpenseGroupID
        {
            get { return this.expenseGroupID; }
            set { this.expenseGroupID = value; }
        }

        /// <summary>
        /// Gets or sets the accountCode for the current DbAccount
        /// </summary>
        public virtual string AccountCode
        {
            get { return this.accountCode; }
            set { this.accountCode = value; }
        }

        /// <summary>
        /// Gets or sets the UpdBy for the current DbAccount
        /// </summary>
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

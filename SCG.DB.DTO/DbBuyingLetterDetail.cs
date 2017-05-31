using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO
{
    [Serializable]
    public partial class DbBuyingLetterDetail
    {
        #region Fields

        private long letterID;
        private string letterNo;
        private DateTime? buyingDate;
        private string companyName;
        private string accountType;
        private string accountNo;
        private string bankName;
        private string bankBranch;

        private long creBy;
        private DateTime creDate;
        private long updBy;
        private DateTime updDate;
        private Byte[] rowVersion;

        #endregion

        public virtual long LetterID
        {
            get { return this.letterID; }
            set { this.letterID = value; }
        }

        public virtual string LetterNo
        {
            get { return this.letterNo; }
            set { this.letterNo = value; }
        }

        public virtual DateTime? BuyingDate
        {
            get { return this.buyingDate; }
            set { this.buyingDate = value; }
        }

        public virtual string CompanyName
        {
            get { return this.companyName; }
            set { this.companyName = value; }
        }

        public virtual string AccountType
        {
            get { return this.accountType; }
            set { this.accountType = value; }
        }

        public virtual string AccountNo
        {
            get { return this.accountNo; }
            set { this.accountNo = value; }
        }

        public virtual string BankName
        {
            get { return this.bankName; }
            set { this.bankName = value; }
        }

        public virtual string BankBranch
        {
            get { return this.bankBranch; }
            set { this.bankBranch = value; }
        }

        /// <summary>
        /// Gets or sets the CreBy for the current DbCompany
        /// </summary>
        public virtual long CreBy
        {
            get { return this.creBy; }
            set { this.creBy = value; }
        }

        /// <summary>
        /// Gets or sets the CreDate for the current DbCompany
        /// </summary>
        public virtual DateTime CreDate
        {
            get { return this.creDate; }
            set { this.creDate = value; }
        }

        /// <summary>
        /// Gets or sets the UpdBy for the current DbCompany
        /// </summary>
        public virtual long UpdBy
        {
            get { return this.updBy; }
            set { this.updBy = value; }
        }

        /// <summary>
        /// Gets or sets the UpdDate for the current DbCompany
        /// </summary>
        public virtual DateTime UpdDate
        {
            get { return this.updDate; }
            set { this.updDate = value; }
        }

        /// <summary>
        /// Gets or sets the RowVersion for the current DbCompany
        /// </summary>
        public virtual Byte[] RowVersion
        {
            get { return this.rowVersion; }
            set { this.rowVersion = value; }
        }
    }

}

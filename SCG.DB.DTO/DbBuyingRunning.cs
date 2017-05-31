using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO
{
    [Serializable]
    public partial class DbBuyingRunning
    {
        #region Fields

        private long runningID;
        private long companyID;
        private int year;
        private long runningNo;

        private long creBy;
        private DateTime creDate;
        private long updBy;
        private DateTime updDate;
        private Byte[] rowVersion;

        #endregion

        public virtual long RunningID
        {
            get { return this.runningID; }
            set { this.runningID = value; }
        }

        public virtual long CompanyID
        {
            get { return this.companyID; }
            set { this.companyID = value; }
        }

        public virtual int Year
        {
            get { return this.year; }
            set { this.year = value; }
        }

        public virtual long RunningNo
        {
            get { return this.runningNo; }
            set { this.runningNo = value; }
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

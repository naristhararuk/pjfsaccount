using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.DB.DTO
{
    [Serializable]
    public partial class DbStatus
    {
        #region Fields
        private short statusid;
        private string groupstatus;
        private string status;
        private string comment;
        private long updBy;
        private DateTime updDate;
        private long creBy;
        private DateTime creDate;
        private string updPgm;
        private Byte[] rowVersion;
        private bool active;
        #endregion

        #region Constructors
		
		/// <summary>
        /// Initializes a new instance of the DbStatus class
		/// </summary>
		public DbStatus()
		{
		}

		public DbStatus(short statusid)
		{
			this.statusid = statusid;
		}
	
		/// <summary>
        /// Initializes a new instance of the DbStatus class
		/// </summary>
		/// <param name="comment">Initial <see cref="DbStatus.Comment" /> value</param>
		/// <param name="updBy">Initial <see cref="DbStatus.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="DbStatus.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="DbStatus.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="DbStatus.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="DbStatus.UpdPgm" /> value</param>
		/// <param name="rowVersion">Initial <see cref="DbStatus.RowVersion" /> value</param>
		/// <param name="active">Initial <see cref="DbStatus.Active" /> value</param>
		/// <param name="programCode">Initial <see DbStatus.ProgramCode" /> value</param>
        public DbStatus(string comment, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, Byte[] rowVersion, bool active, string groupstatus, string status)
		{
			this.comment = comment;
			this.updBy = updBy;
			this.updDate = updDate;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updPgm = updPgm;
			this.rowVersion = rowVersion;
			this.active = active;
            this.groupstatus = groupstatus;
            this.status = status;
		}
	
		/// <summary>
        /// Minimal constructor for class DbStatus
		/// </summary>
		/// <param name="updBy">Initial <see cref="DbStatus.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="DbStatus.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="DbStatus.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="DbStatus.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="DbStatus.UpdPgm" /> value</param>
		/// <param name="active">Initial <see cref="DbStatus.Active" /> value</param>
        public DbStatus(long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, bool active)
		{
			this.updBy = updBy;
			this.updDate = updDate;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updPgm = updPgm;
			this.active = active;
		}
		#endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Statusid for the current DbCity
        /// </summary>
        public virtual short StatusID
        {
            get { return this.statusid; }
            set { this.statusid = value; }
        }
        /// <summary>
        /// Gets or sets the GroupStatus for the current DbCity
        /// </summary>
        public virtual string GroupStatus
        {
            get { return this.groupstatus; }
            set { this.groupstatus = value; }
        }
        /// <summary>
        /// Gets or sets the Status for the current DbCity
        /// </summary>
        public virtual string Status
        {
            get { return this.status; }
            set { this.status = value; }
        }
        /// <summary>
        /// Gets or sets the Comment for the current DbCity
        /// </summary>
        public virtual string Comment
        {
            get { return this.comment; }
            set { this.comment = value; }
        }

        /// <summary>
        /// Gets or sets the UpdBy for the current DbCity
        /// </summary>
        public virtual long UpdBy
        {
            get { return this.updBy; }
            set { this.updBy = value; }
        }

        /// <summary>
        /// Gets or sets the UpdDate for the current DbCity
        /// </summary>
        public virtual DateTime UpdDate
        {
            get { return this.updDate; }
            set { this.updDate = value; }
        }

        /// <summary>
        /// Gets or sets the CreBy for the current DbCity
        /// </summary>
        public virtual long CreBy
        {
            get { return this.creBy; }
            set { this.creBy = value; }
        }

        /// <summary>
        /// Gets or sets the CreDate for the current DbCity
        /// </summary>
        public virtual DateTime CreDate
        {
            get { return this.creDate; }
            set { this.creDate = value; }
        }

        /// <summary>
        /// Gets or sets the UpdPgm for the current DbCity
        /// </summary>
        public virtual string UpdPgm
        {
            get { return this.updPgm; }
            set { this.updPgm = value; }
        }

        /// <summary>
        /// Gets or sets the RowVersion for the current DbCity
        /// </summary>
        public virtual Byte[] RowVersion
        {
            get { return this.rowVersion; }
            set { this.rowVersion = value; }
        }

        /// <summary>
        /// Gets or sets the Active for the current DbCity
        /// </summary>
        public virtual bool Active
        {
            get { return this.active; }
            set { this.active = value; }
        }
        #endregion
    }
}

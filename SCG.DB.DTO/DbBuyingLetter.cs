using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO
{
    [Serializable]
    public partial class DbBuyingLetter
    {
        #region Fields

        private long letterID;
        private long documentID;
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

        public virtual long DocumentID
        {
            get { return this.documentID; }
            set { this.documentID = value; }
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

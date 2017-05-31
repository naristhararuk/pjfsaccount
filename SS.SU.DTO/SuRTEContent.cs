using System;

namespace SS.SU.DTO
{
    [Serializable]
    public partial class SuRTEContent
    {
        #region Fields

        private short id;

        private string header;
        private string content;

        private string comment;
        private long updBy;
        private DateTime updDate;
        private long creBy;
        private DateTime creDate;
        private string updPgm;
        private Byte[] rowVersion;
        private bool active;
        private SS.SU.DTO.SuRTENode node;
		private SS.DB.DTO.DbLanguage language;

        #endregion

        #region Constructors
		
		/// <summary>
		/// Initializes a new instance of the SuRTEContent class
		/// </summary>
		public SuRTEContent()
		{
		}

		public SuRTEContent(short id)
		{
			this.id = id;
		}
	
		/// <summary>
		/// Initializes a new instance of the SuRTEContent class
		/// </summary>
		/// <param name="id">Initial <see cref="SuRTEContent.id" /> value</param>
		/// <param name="header">Initial <see cref="SuRTEContent.header" /> value</param>
        /// <param name="content">Initial <see cref="SuRTEContent.content" /> value</param>

		/// <param name="comment">Initial <see cref="SuRTEContent.Comment" /> value</param>
		/// <param name="updBy">Initial <see cref="SuRTEContent.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="SuRTEContent.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="SuRTEContent.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="SuRTEContent.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="SuRTEContent.UpdPgm" /> value</param>
		/// <param name="rowVersion">Initial <see cref="SuRTEContent.RowVersion" /> value</param>
		/// <param name="active">Initial <see cref="SuRTEContent.Active" /> value</param>
        public SuRTEContent(short id, string header,string content,string comment, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, Byte[] rowVersion, bool active)
		{
			this.id = id;
			this.header = header;
            this.content = content;
			this.comment = comment;
			this.updBy = updBy;
			this.updDate = updDate;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updPgm = updPgm;
			this.rowVersion = rowVersion;
			this.active = active;
		}
	
		/// <summary>
		/// Minimal constructor for class SuRTENode
		/// </summary>
		/// <param name="id">Initial <see cref="SuRTENode.id" /> value</param>
		/// <param name="updBy">Initial <see cref="SuRTENode.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="SuRTENode.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="SuRTENode.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="SuRTENode.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="SuRTENode.UpdPgm" /> value</param>
		/// <param name="active">Initial <see cref="SuRTENode.Active" /> value</param>
        public SuRTEContent(short id, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, bool active)
		{
			this.id = id;
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
        /// Gets or sets the Id for the current SuRTEContent
        /// </summary>
        public virtual short Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        /// <summary>
        /// Gets or sets the Header for the current SuRTEContent
        /// </summary>
        public virtual string Header
        {
            get { return this.header; }
            set { this.header = value; }
        }

        /// <summary>
        /// Gets or sets the Content for the current SuRTEContent
        /// </summary>
        public virtual string Content
        {
            get { return this.content; }
            set { this.content = value; }
        }

        /// <summary>
        /// Gets or sets the Comment for the current SuRTEContent
        /// </summary>
        public virtual string Comment
        {
            get { return this.comment; }
            set { this.comment = value; }
        }

        /// <summary>
        /// Gets or sets the UpdBy for the current SuRTEContent
        /// </summary>
        public virtual long UpdBy
        {
            get { return this.updBy; }
            set { this.updBy = value; }
        }

        /// <summary>
        /// Gets or sets the UpdDate for the current SuRTEContent
        /// </summary>
        public virtual DateTime UpdDate
        {
            get { return this.updDate; }
            set { this.updDate = value; }
        }

        /// <summary>
        /// Gets or sets the CreBy for the current SuRTEContent
        /// </summary>
        public virtual long CreBy
        {
            get { return this.creBy; }
            set { this.creBy = value; }
        }

        /// <summary>
        /// Gets or sets the CreDate for the current SuRTEContent
        /// </summary>
        public virtual DateTime CreDate
        {
            get { return this.creDate; }
            set { this.creDate = value; }
        }

        /// <summary>
        /// Gets or sets the UpdPgm for the current SuRTEContent
        /// </summary>
        public virtual string UpdPgm
        {
            get { return this.updPgm; }
            set { this.updPgm = value; }
        }

        /// <summary>
        /// Gets or sets the RowVersion for the current SuRTEContent
        /// </summary>
        public virtual Byte[] RowVersion
        {
            get { return this.rowVersion; }
            set { this.rowVersion = value; }
        }

        /// <summary>
        /// Gets or sets the Active for the current SuRTEContent
        /// </summary>
        public virtual bool Active
        {
            get { return this.active; }
            set { this.active = value; }
        }

        /// <summary>
        /// Gets or sets the RTENode for the current SuRTEContent
        /// </summary>
        public virtual SS.SU.DTO.SuRTENode Node
        {
            get { return this.node; }
            set { this.node = value; }
        }

        /// <summary>
        /// Gets or sets the Language for the current SuRTEContent
        /// </summary>
        public virtual SS.DB.DTO.DbLanguage Language
        {
            get { return this.language; }
            set { this.language = value; }
        }

        #endregion
    }
}

using System;

namespace SS.SU.DTO
{
    [Serializable]
    public partial class SuRTENode
    {
        #region Fields

        private short nodeid;
        private short? nodeheaderid;
        private short? nodeorderno;

        private string nodetype;
        private string imagepath;

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
		/// Initializes a new instance of the SuRTENode class
		/// </summary>
		public SuRTENode()
		{
		}

		public SuRTENode(short nodeid)
		{
			this.nodeid = nodeid;
		}
	
		/// <summary>
		/// Initializes a new instance of the SuRTENode class
		/// </summary>
		/// <param name="nodeid">Initial <see cref="SuRTENode.Nodeid" /> value</param>
		/// <param name="nodeheaderid">Initial <see cref="SuRTENode.NodeHeaderid" /> value</param>
        /// <param name="nodeorderno">Initial <see cref="SuRTENode.NodeOrderNo" /> value</param>

		/// <param name="nodetype">Initial <see cref="SuRTENode.NodeType" /> value</param>
        /// <param name="comment">Initial <see cref="SuRTENode.Comment" /> value</param>
        /// <param name="updBy">Initial <see cref="SuRTENode.UpdBy" /> value</param>
        /// <param name="updDate">Initial <see cref="SuRTENode.UpdDate" /> value</param>
        /// <param name="creBy">Initial <see cref="SuRTENode.CreBy" /> value</param>
        /// <param name="creDate">Initial <see cref="SuRTENode.CreDate" /> value</param>
        /// <param name="updPgm">Initial <see cref="SuRTENode.UpdPgm" /> value</param>
        /// <param name="rowVersion">Initial <see cref="SuRTENode.RowVersion" /> value</param>
        /// <param name="active">Initial <see cref="SuRTENode.Active" /> value</param>
        public SuRTENode(short nodeid, short? nodeheaderid, short nodeorderno, string nodetype,  string comment, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, Byte[] rowVersion, bool active)
		{
			this.nodeid = nodeid;
			this.nodeheaderid = nodeheaderid;
            this.nodeorderno = nodeorderno;
			this.nodetype = nodetype;
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
		/// <param name="nodeid">Initial <see cref="SuRTENode.nodeid" /> value</param>
		/// <param name="updBy">Initial <see cref="SuRTENode.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="SuRTENode.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="SuRTENode.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="SuRTENode.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="SuRTENode.UpdPgm" /> value</param>
		/// <param name="active">Initial <see cref="SuRTENode.Active" /> value</param>
        public SuRTENode(short nodeid, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, bool active)
		{
			this.nodeid = nodeid;
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
		/// Gets or sets the Menuid for the current SuRTENode
		/// </summary>
		public virtual short Nodeid
		{
			get { return this.nodeid; }
			set { this.nodeid = value; }
		}
		
		/// <summary>
		/// Gets or sets the Programid for the current SuRTENode
		/// </summary>
        public virtual short? NodeHeaderid
		{
            get { return this.nodeheaderid; }
            set { this.nodeheaderid = value; }
		}
		
		/// <summary>
		/// Gets or sets the MenuMainid for the current SuRTENode
		/// </summary>
        public virtual short? NodeOrderNo
		{
            get { return this.nodeorderno; }
            set { this.nodeorderno = value; }
		}

		/// <summary>
		/// Gets or sets the NodeType for the current SuRTENode
		/// </summary>
		public virtual string NodeType
		{
            get { return this.nodetype; }
            set { this.nodetype = value; }
		}

        /// <summary>
        /// Gets or sets the ImagePath for the current SuRTENode
        /// </summary>
        public virtual string ImagePath
        {
            get { return this.imagepath; }
            set { this.imagepath = value; }
        }
		
		
		/// <summary>
		/// Gets or sets the Comment for the current SuRTENode
		/// </summary>
		public virtual string Comment
		{
			get { return this.comment; }
			set { this.comment = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdBy for the current SuRTENode
		/// </summary>
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdDate for the current SuRTENode
		/// </summary>
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreBy for the current SuRTENode
		/// </summary>
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreDate for the current SuRTENode
		/// </summary>
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdPgm for the current SuRTENode
		/// </summary>
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}
		
		/// <summary>
		/// Gets or sets the RowVersion for the current SuRTENode
		/// </summary>
		public virtual Byte[] RowVersion
		{
			get { return this.rowVersion; }
			set { this.rowVersion = value; }
		}
		
		/// <summary>
		/// Gets or sets the Active for the current SuRTENode
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		
		#endregion
    }
}

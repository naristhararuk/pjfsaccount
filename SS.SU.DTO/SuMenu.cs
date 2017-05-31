//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by NHibernate.
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

using System;

namespace SS.SU.DTO
{
	/// <summary>
	/// POJO for SuMenu. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class SuMenu : ISimpleMaster
	{
		#region Fields
		
		private short menuid;
        private string menuCode;
		//private short programid;
        private SuProgram program;
		private short? menuMainid;
        private short menuLevel;

		private short menuSeq;
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
		/// Initializes a new instance of the SuMenu class
		/// </summary>
		public SuMenu()
		{
		}

		public SuMenu(short menuid)
		{
			this.menuid = menuid;
		}
	
		/// <summary>
		/// Initializes a new instance of the SuMenu class
		/// </summary>
		/// <param name="programid">Initial <see cref="SuMenu.Programid" /> value</param>
		/// <param name="menuMainid">Initial <see cref="SuMenu.MenuMainid" /> value</param>
        /// <param name="menuLevel">Initial <see cref="SuMenu.MenuLevel" /> value</param>

		/// <param name="menuSeq">Initial <see cref="SuMenu.MenuSeq" /> value</param>
		/// <param name="comment">Initial <see cref="SuMenu.Comment" /> value</param>
		/// <param name="updBy">Initial <see cref="SuMenu.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="SuMenu.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="SuMenu.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="SuMenu.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="SuMenu.UpdPgm" /> value</param>
		/// <param name="rowVersion">Initial <see cref="SuMenu.RowVersion" /> value</param>
		/// <param name="active">Initial <see cref="SuMenu.Active" /> value</param>
        public SuMenu(string menuCode,short programid, short? menuMainid, short menuLevel, short menuSeq,  string comment, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, Byte[] rowVersion, bool active)
		{
            this.menuCode = menuCode;
			//this.programid = programid;
			this.menuMainid = menuMainid;
            this.menuLevel = menuLevel;
			this.menuSeq = menuSeq;
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
		/// Minimal constructor for class SuMenu
		/// </summary>
		/// <param name="menuMainid">Initial <see cref="SuMenu.MenuMainid" /> value</param>
		/// <param name="updBy">Initial <see cref="SuMenu.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="SuMenu.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="SuMenu.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="SuMenu.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="SuMenu.UpdPgm" /> value</param>
		/// <param name="active">Initial <see cref="SuMenu.Active" /> value</param>
		public SuMenu(short? menuMainid, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, bool active)
		{
			this.menuMainid = menuMainid;
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
		/// Gets or sets the Menuid for the current SuMenu
		/// </summary>
		public virtual short Menuid
		{
			get { return this.menuid; }
			set { this.menuid = value; }
		}

        /// <summary>
        /// Gets or sets the MenuCode for the current SuMenu
        /// </summary>
        public virtual string MenuCode
        {
            get { return this.menuCode; }
            set { this.menuCode = value; }
        }
		
        ///// <summary>
        ///// Gets or sets the Programid for the current SuMenu
        ///// </summary>
        //public virtual short Programid
        //{
        //    get { return this.programid; }
        //    set { this.programid = value; }
        //}

        /// <summary>
        /// Gets or sets the Program for the current SuMenu
        /// </summary>
        public virtual SuProgram Program
        {
            get { return this.program; }
            set { this.program = value; }
        }

		/// <summary>
		/// Gets or sets the MenuMainid for the current SuMenu
		/// </summary>
		public virtual short? MenuMainid
		{
			get { return this.menuMainid; }
			set { this.menuMainid = value; }
		}
        /// <summary>
        /// Gets or sets the MenuLevel for the current SuMenu
        /// </summary>
        public virtual short MenuLevel
        {
            get { return this.menuLevel; }
            set { this.menuLevel = value; }
        }
		/// <summary>
		/// Gets or sets the MenuSeq for the current SuMenu
		/// </summary>
		public virtual short MenuSeq
		{
			get { return this.menuSeq; }
			set { this.menuSeq = value; }
		}
		
	
		
		/// <summary>
		/// Gets or sets the Comment for the current SuMenu
		/// </summary>
		public virtual string Comment
		{
			get { return this.comment; }
			set { this.comment = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdBy for the current SuMenu
		/// </summary>
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdDate for the current SuMenu
		/// </summary>
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreBy for the current SuMenu
		/// </summary>
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreDate for the current SuMenu
		/// </summary>
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdPgm for the current SuMenu
		/// </summary>
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}
		
		/// <summary>
		/// Gets or sets the RowVersion for the current SuMenu
		/// </summary>
		public virtual Byte[] RowVersion
		{
			get { return this.rowVersion; }
			set { this.rowVersion = value; }
		}
		
		/// <summary>
		/// Gets or sets the Active for the current SuMenu
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		
		#endregion
	}
}
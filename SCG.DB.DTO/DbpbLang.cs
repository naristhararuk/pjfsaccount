//------------------------------------------------------------------------------
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
	/// POJO for DbpbLang. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class DbpbLang
	{
		#region Fields
		
		private long pBLangID;
		private string description;
		private bool active;
		private long creBy;
		private DateTime creDate;
		private long updBy;
		private DateTime updDate;
		private string updPgm;
		private byte[] rowVersion;
		private SCG.DB.DTO.Dbpb pBID;
		private SS.DB.DTO.DbLanguage languageID;
        private string comment;

		#endregion

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the DbpbLang class
		/// </summary>
		public DbpbLang()
		{
		}

		public DbpbLang(long pBLangID)
		{
			this.pBLangID = pBLangID;
		}
	
		/// <summary>
		/// Initializes a new instance of the DbpbLang class
		/// </summary>
		/// <param name="description">Initial <see cref="DbpbLang.Description" /> value</param>
		/// <param name="active">Initial <see cref="DbpbLang.Active" /> value</param>
		/// <param name="creBy">Initial <see cref="DbpbLang.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="DbpbLang.CreDate" /> value</param>
		/// <param name="updBy">Initial <see cref="DbpbLang.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="DbpbLang.UpdDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="DbpbLang.UpdPgm" /> value</param>
		/// <param name="rowVersion">Initial <see cref="DbpbLang.RowVersion" /> value</param>
		/// <param name="pBID">Initial <see cref="DbpbLang.PBID" /> value</param>
		/// <param name="languageID">Initial <see cref="DbpbLang.LanguageID" /> value</param>
        public DbpbLang(string description, bool active, long creBy, DateTime creDate, long updBy, DateTime updDate, string updPgm, byte[] rowVersion, SCG.DB.DTO.Dbpb pBID, SS.DB.DTO.DbLanguage languageID, string comment)
		{
			this.description = description;
			this.active = active;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updBy = updBy;
			this.updDate = updDate;
			this.updPgm = updPgm;
			this.rowVersion = rowVersion;
			this.pBID = pBID;
			this.languageID = languageID;
            this.comment = comment;
		}
	
		/// <summary>
		/// Minimal constructor for class DbpbLang
		/// </summary>
		/// <param name="active">Initial <see cref="DbpbLang.Active" /> value</param>
		/// <param name="creBy">Initial <see cref="DbpbLang.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="DbpbLang.CreDate" /> value</param>
		/// <param name="updBy">Initial <see cref="DbpbLang.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="DbpbLang.UpdDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="DbpbLang.UpdPgm" /> value</param>
		public DbpbLang(bool active, long creBy, DateTime creDate, long updBy, DateTime updDate, string updPgm)
		{
			this.active = active;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updBy = updBy;
			this.updDate = updDate;
			this.updPgm = updPgm;
		}
		#endregion
	
		#region Properties
		
		/// <summary>
		/// Gets or sets the PBLangID for the current DbpbLang
		/// </summary>
		public virtual long PBLangID
		{
			get { return this.pBLangID; }
			set { this.pBLangID = value; }
		}
		
		/// <summary>
		/// Gets or sets the Description for the current DbpbLang
		/// </summary>
		public virtual string Description
		{
			get { return this.description; }
			set { this.description = value; }
		}
		
		/// <summary>
		/// Gets or sets the Active for the current DbpbLang
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreBy for the current DbpbLang
		/// </summary>
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreDate for the current DbpbLang
		/// </summary>
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdBy for the current DbpbLang
		/// </summary>
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdDate for the current DbpbLang
		/// </summary>
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdPgm for the current DbpbLang
		/// </summary>
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}
		
		/// <summary>
		/// Gets or sets the RowVersion for the current DbpbLang
		/// </summary>
		public virtual byte[] RowVersion
		{
			get { return this.rowVersion; }
			set { this.rowVersion = value; }
		}
		
		/// <summary>
		/// Gets or sets the PBID for the current DbpbLang
		/// </summary>
		public virtual SCG.DB.DTO.Dbpb PBID
		{
			get { return this.pBID; }
			set { this.pBID = value; }
		}
		
		/// <summary>
		/// Gets or sets the LanguageID for the current DbpbLang
		/// </summary>
		public virtual SS.DB.DTO.DbLanguage LanguageID
		{
			get { return this.languageID; }
			set { this.languageID = value; }
		}
        public virtual string Comment
        {
            get { return this.comment; }
            set { this.comment = value; }
        }

        /// <summary>
        /// Gets or sets the Active for the current DbpbLang
        /// </summary>
		
		#endregion
	}
}

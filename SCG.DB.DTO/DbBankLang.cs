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
	/// POJO for DbBankLang. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class DbBankLang
	{
		#region Fields
		
		private long id;
		private string bankName;
		private string abbrName;
		private string comment;
		private long updBy;
		private DateTime updDate;
		private long creBy;
		private DateTime creDate;
		private string updPgm;
		private byte[] rowVersion;
		private bool active;
		private SCG.DB.DTO.DbBank bank;
        private SS.DB.DTO.DbLanguage language;

		#endregion

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the DbBankLang class
		/// </summary>
		public DbBankLang()
		{
		}

		public DbBankLang(long id)
		{
			this.id = id;
		}
	
		/// <summary>
		/// Initializes a new instance of the DbBankLang class
		/// </summary>
		/// <param name="bankName">Initial <see cref="DbBankLang.BankName" /> value</param>
		/// <param name="abbrName">Initial <see cref="DbBankLang.AbbrName" /> value</param>
		/// <param name="comment">Initial <see cref="DbBankLang.Comment" /> value</param>
		/// <param name="updBy">Initial <see cref="DbBankLang.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="DbBankLang.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="DbBankLang.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="DbBankLang.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="DbBankLang.UpdPgm" /> value</param>
		/// <param name="rowVersion">Initial <see cref="DbBankLang.RowVersion" /> value</param>
		/// <param name="active">Initial <see cref="DbBankLang.Active" /> value</param>
		/// <param name="bank">Initial <see cref="DbBankLang.Bank" /> value</param>
		/// <param name="language">Initial <see cref="DbBankLang.Language" /> value</param>
        public DbBankLang(string bankName, string abbrName, string comment, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, byte[] rowVersion, bool active, SCG.DB.DTO.DbBank bank, SS.DB.DTO.DbLanguage language)
		{
			this.bankName = bankName;
			this.abbrName = abbrName;
			this.comment = comment;
			this.updBy = updBy;
			this.updDate = updDate;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updPgm = updPgm;
			this.rowVersion = rowVersion;
			this.active = active;
			this.bank = bank;
			this.language = language;
		}
	
		/// <summary>
		/// Minimal constructor for class DbBankLang
		/// </summary>
		/// <param name="updBy">Initial <see cref="DbBankLang.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="DbBankLang.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="DbBankLang.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="DbBankLang.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="DbBankLang.UpdPgm" /> value</param>
		/// <param name="active">Initial <see cref="DbBankLang.Active" /> value</param>
		public DbBankLang(long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, bool active)
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
		/// Gets or sets the Id for the current DbBankLang
		/// </summary>
		public virtual long Id
		{
			get { return this.id; }
			set { this.id = value; }
		}
		
		/// <summary>
		/// Gets or sets the BankName for the current DbBankLang
		/// </summary>
		public virtual string BankName
		{
			get { return this.bankName; }
			set { this.bankName = value; }
		}
		
		/// <summary>
		/// Gets or sets the AbbrName for the current DbBankLang
		/// </summary>
		public virtual string AbbrName
		{
			get { return this.abbrName; }
			set { this.abbrName = value; }
		}
		
		/// <summary>
		/// Gets or sets the Comment for the current DbBankLang
		/// </summary>
		public virtual string Comment
		{
			get { return this.comment; }
			set { this.comment = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdBy for the current DbBankLang
		/// </summary>
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdDate for the current DbBankLang
		/// </summary>
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreBy for the current DbBankLang
		/// </summary>
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreDate for the current DbBankLang
		/// </summary>
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdPgm for the current DbBankLang
		/// </summary>
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}
		
		/// <summary>
		/// Gets or sets the RowVersion for the current DbBankLang
		/// </summary>
		public virtual byte[] RowVersion
		{
			get { return this.rowVersion; }
			set { this.rowVersion = value; }
		}
		
		/// <summary>
		/// Gets or sets the Active for the current DbBankLang
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		
		/// <summary>
		/// Gets or sets the Bank for the current DbBankLang
		/// </summary>
		public virtual SCG.DB.DTO.DbBank Bank
		{
			get { return this.bank; }
			set { this.bank = value; }
		}
		
		/// <summary>
		/// Gets or sets the Language for the current DbBankLang
		/// </summary>
        public virtual SS.DB.DTO.DbLanguage Language
		{
			get { return this.language; }
			set { this.language = value; }
		}
		
		#endregion
	}
}

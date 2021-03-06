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
	/// POJO for SuMenuLang. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class SuMenuLang
	{
		#region Fields
		
		private long id;
		private string menuName;
		private string comment;
		private long updBy;
		private DateTime updDate;
		private long creBy;
		private DateTime creDate;
		private string updPgm;
		private Byte[] rowVersion;
		private bool active;
		private SS.SU.DTO.SuMenu menu;
		private SS.DB.DTO.DbLanguage language;

		#endregion

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the SuMenuLang class
		/// </summary>
		public SuMenuLang()
		{
		}

		public SuMenuLang(long id)
		{
			this.id = id;
		}
	
		/// <summary>
		/// Initializes a new instance of the SuMenuLang class
		/// </summary>
		/// <param name="menuName">Initial <see cref="SuMenuLang.MenuName" /> value</param>
		/// <param name="comment">Initial <see cref="SuMenuLang.Comment" /> value</param>
		/// <param name="updBy">Initial <see cref="SuMenuLang.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="SuMenuLang.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="SuMenuLang.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="SuMenuLang.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="SuMenuLang.UpdPgm" /> value</param>
		/// <param name="rowVersion">Initial <see cref="SuMenuLang.RowVersion" /> value</param>
		/// <param name="active">Initial <see cref="SuMenuLang.Active" /> value</param>
		/// <param name="menu">Initial <see cref="SuMenuLang.Menu" /> value</param>
		/// <param name="language">Initial <see cref="SuMenuLang.Language" /> value</param>
		public SuMenuLang(string menuName, string comment, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, Byte[] rowVersion, bool active, SS.SU.DTO.SuMenu menu, SS.DB.DTO.DbLanguage language)
		{
			this.menuName = menuName;
			this.comment = comment;
			this.updBy = updBy;
			this.updDate = updDate;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updPgm = updPgm;
			this.rowVersion = rowVersion;
			this.active = active;
			this.menu = menu;
			this.language = language;
		}
	
		/// <summary>
		/// Minimal constructor for class SuMenuLang
		/// </summary>
		/// <param name="menuName">Initial <see cref="SuMenuLang.MenuName" /> value</param>
		/// <param name="updBy">Initial <see cref="SuMenuLang.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="SuMenuLang.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="SuMenuLang.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="SuMenuLang.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="SuMenuLang.UpdPgm" /> value</param>
		/// <param name="active">Initial <see cref="SuMenuLang.Active" /> value</param>
		public SuMenuLang(string menuName, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, bool active)
		{
			this.menuName = menuName;
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
		/// Gets or sets the Id for the current SuMenuLang
		/// </summary>
		public virtual long Id
		{
			get { return this.id; }
			set { this.id = value; }
		}
		
		/// <summary>
		/// Gets or sets the MenuName for the current SuMenuLang
		/// </summary>
		public virtual string MenuName
		{
			get { return this.menuName; }
			set { this.menuName = value; }
		}
		
		/// <summary>
		/// Gets or sets the Comment for the current SuMenuLang
		/// </summary>
		public virtual string Comment
		{
			get { return this.comment; }
			set { this.comment = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdBy for the current SuMenuLang
		/// </summary>
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdDate for the current SuMenuLang
		/// </summary>
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreBy for the current SuMenuLang
		/// </summary>
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreDate for the current SuMenuLang
		/// </summary>
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdPgm for the current SuMenuLang
		/// </summary>
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}
		
		/// <summary>
		/// Gets or sets the RowVersion for the current SuMenuLang
		/// </summary>
		public virtual Byte[] RowVersion
		{
			get { return this.rowVersion; }
			set { this.rowVersion = value; }
		}
		
		/// <summary>
		/// Gets or sets the Active for the current SuMenuLang
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		
		/// <summary>
		/// Gets or sets the Menu for the current SuMenuLang
		/// </summary>
		public virtual SS.SU.DTO.SuMenu Menu
		{
			get { return this.menu; }
			set { this.menu = value; }
		}
		
		/// <summary>
		/// Gets or sets the Language for the current SuMenuLang
		/// </summary>
		public virtual SS.DB.DTO.DbLanguage Language
		{
			get { return this.language; }
			set { this.language = value; }
		}
		
		#endregion
	}
}

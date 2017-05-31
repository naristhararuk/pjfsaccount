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
	/// POJO for SuDivisionLang. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class SuDivisionLang
	{
		#region Fields
		
		private long id;
		private string divisionName;
		private string comment;
		private long updBy;
		private DateTime updDate;
		private long creBy;
		private DateTime creDate;
		private string updPgm;
		private Byte[] rowVersion;
		private bool active;
		private SS.DB.DTO.DbLanguage language;
		private SS.SU.DTO.SuDivision division;

		#endregion

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the SuDivisionLang class
		/// </summary>
		public SuDivisionLang()
		{
		}

		public SuDivisionLang(long id)
		{
			this.id = id;
		}
	
		/// <summary>
		/// Initializes a new instance of the SuDivisionLang class
		/// </summary>
		/// <param name="divisionName">Initial <see cref="SuDivisionLang.DivisionName" /> value</param>
		/// <param name="comment">Initial <see cref="SuDivisionLang.Comment" /> value</param>
		/// <param name="updBy">Initial <see cref="SuDivisionLang.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="SuDivisionLang.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="SuDivisionLang.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="SuDivisionLang.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="SuDivisionLang.UpdPgm" /> value</param>
		/// <param name="rowVersion">Initial <see cref="SuDivisionLang.RowVersion" /> value</param>
		/// <param name="active">Initial <see cref="SuDivisionLang.Active" /> value</param>
		/// <param name="language">Initial <see cref="SuDivisionLang.Language" /> value</param>
		/// <param name="division">Initial <see cref="SuDivisionLang.Division" /> value</param>
		public SuDivisionLang(string divisionName, string comment, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, Byte[] rowVersion, bool active, SS.DB.DTO.DbLanguage language, SS.SU.DTO.SuDivision division)
		{
			this.divisionName = divisionName;
			this.comment = comment;
			this.updBy = updBy;
			this.updDate = updDate;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updPgm = updPgm;
			this.rowVersion = rowVersion;
			this.active = active;
			this.language = language;
			this.division = division;
		}
	
		/// <summary>
		/// Minimal constructor for class SuDivisionLang
		/// </summary>
		/// <param name="divisionName">Initial <see cref="SuDivisionLang.DivisionName" /> value</param>
		/// <param name="updBy">Initial <see cref="SuDivisionLang.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="SuDivisionLang.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="SuDivisionLang.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="SuDivisionLang.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="SuDivisionLang.UpdPgm" /> value</param>
		/// <param name="active">Initial <see cref="SuDivisionLang.Active" /> value</param>
		public SuDivisionLang(string divisionName, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, bool active)
		{
			this.divisionName = divisionName;
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
		/// Gets or sets the Id for the current SuDivisionLang
		/// </summary>
		public virtual long Id
		{
			get { return this.id; }
			set { this.id = value; }
		}
		
		/// <summary>
		/// Gets or sets the DivisionName for the current SuDivisionLang
		/// </summary>
		public virtual string DivisionName
		{
			get { return this.divisionName; }
			set { this.divisionName = value; }
		}
		
		/// <summary>
		/// Gets or sets the Comment for the current SuDivisionLang
		/// </summary>
		public virtual string Comment
		{
			get { return this.comment; }
			set { this.comment = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdBy for the current SuDivisionLang
		/// </summary>
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdDate for the current SuDivisionLang
		/// </summary>
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreBy for the current SuDivisionLang
		/// </summary>
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreDate for the current SuDivisionLang
		/// </summary>
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdPgm for the current SuDivisionLang
		/// </summary>
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}
		
		/// <summary>
		/// Gets or sets the RowVersion for the current SuDivisionLang
		/// </summary>
		public virtual Byte[] RowVersion
		{
			get { return this.rowVersion; }
			set { this.rowVersion = value; }
		}
		
		/// <summary>
		/// Gets or sets the Active for the current SuDivisionLang
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		
		/// <summary>
		/// Gets or sets the Language for the current SuDivisionLang
		/// </summary>
		public virtual SS.DB.DTO.DbLanguage Language
		{
			get { return this.language; }
			set { this.language = value; }
		}
		
		/// <summary>
		/// Gets or sets the Division for the current SuDivisionLang
		/// </summary>
		public virtual SS.SU.DTO.SuDivision Division
		{
			get { return this.division; }
			set { this.division = value; }
		}
		
		#endregion
	}
}

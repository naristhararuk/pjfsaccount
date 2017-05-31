//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by NHibernate.
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

using System;

namespace SCG.eAccounting.SAP.DTO
{
	/// <summary>
	/// POJO for Bapirunning. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class Bapirunning
	{
		#region Fields
		
		private long id;
		private string year;
		private string period;
		private long runing;
		private bool active;
		private long updBy;
		private DateTime updDate;
		private long creBy;
		private DateTime creDate;
		private byte[] rowVersion;
		private string updPgm;

		#endregion

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the Bapirunning class
		/// </summary>
		public Bapirunning()
		{
		}

		public Bapirunning(long id)
		{
			this.id = id;
		}
	
		/// <summary>
		/// Initializes a new instance of the Bapirunning class
		/// </summary>
		/// <param name="id">Initial <see cref="Bapirunning.Id" /> value</param>
		/// <param name="year">Initial <see cref="Bapirunning.Year" /> value</param>
		/// <param name="period">Initial <see cref="Bapirunning.Period" /> value</param>
		/// <param name="runing">Initial <see cref="Bapirunning.Runing" /> value</param>
		/// <param name="active">Initial <see cref="Bapirunning.Active" /> value</param>
		/// <param name="updBy">Initial <see cref="Bapirunning.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="Bapirunning.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="Bapirunning.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="Bapirunning.CreDate" /> value</param>
		/// <param name="rowVersion">Initial <see cref="Bapirunning.RowVersion" /> value</param>
		/// <param name="updPgm">Initial <see cref="Bapirunning.UpdPgm" /> value</param>
		public Bapirunning(long id, string year, string period, long runing, bool active, long updBy, DateTime updDate, long creBy, DateTime creDate, byte[] rowVersion, string updPgm)
		{
			this.id = id;
			this.year = year;
			this.period = period;
			this.runing = runing;
			this.active = active;
			this.updBy = updBy;
			this.updDate = updDate;
			this.creBy = creBy;
			this.creDate = creDate;
			this.rowVersion = rowVersion;
			this.updPgm = updPgm;
		}
	
		/// <summary>
		/// Minimal constructor for class Bapirunning
		/// </summary>
		/// <param name="id">Initial <see cref="Bapirunning.Id" /> value</param>
		/// <param name="active">Initial <see cref="Bapirunning.Active" /> value</param>
		/// <param name="updBy">Initial <see cref="Bapirunning.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="Bapirunning.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="Bapirunning.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="Bapirunning.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="Bapirunning.UpdPgm" /> value</param>
		public Bapirunning(long id, bool active, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm)
		{
			this.id = id;
			this.active = active;
			this.updBy = updBy;
			this.updDate = updDate;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updPgm = updPgm;
		}
		#endregion
	
		#region Properties
		
		/// <summary>
		/// Gets or sets the Id for the current Bapirunning
		/// </summary>
		public virtual long Id
		{
			get { return this.id; }
			set { this.id = value; }
		}
		
		/// <summary>
		/// Gets or sets the Year for the current Bapirunning
		/// </summary>
		public virtual string Year
		{
			get { return this.year; }
			set { this.year = value; }
		}
		
		/// <summary>
		/// Gets or sets the Period for the current Bapirunning
		/// </summary>
		public virtual string Period
		{
			get { return this.period; }
			set { this.period = value; }
		}
		
		/// <summary>
		/// Gets or sets the Runing for the current Bapirunning
		/// </summary>
		public virtual long Runing
		{
			get { return this.runing; }
			set { this.runing = value; }
		}
		
		/// <summary>
		/// Gets or sets the Active for the current Bapirunning
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdBy for the current Bapirunning
		/// </summary>
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdDate for the current Bapirunning
		/// </summary>
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreBy for the current Bapirunning
		/// </summary>
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreDate for the current Bapirunning
		/// </summary>
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the RowVersion for the current Bapirunning
		/// </summary>
		public virtual byte[] RowVersion
		{
			get { return this.rowVersion; }
			set { this.rowVersion = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdPgm for the current Bapirunning
		/// </summary>
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}
		
		#endregion
	}
}

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
	/// POJO for Bapiparex. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class Bapiparex
	{
		#region Fields
		
		private long id;
		private string docNo;
		private string structure;
		private string valuepart1;
		private string valuepart2;
		private string valuepart3;
		private string valuepart4;
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
		/// Initializes a new instance of the Bapiparex class
		/// </summary>
		public Bapiparex()
		{
		}

		public Bapiparex(long id)
		{
			this.id = id;
		}
	
		/// <summary>
		/// Initializes a new instance of the Bapiparex class
		/// </summary>
		/// <param name="docNo">Initial <see cref="Bapiparex.DocNo" /> value</param>
		/// <param name="structure">Initial <see cref="Bapiparex.Structure" /> value</param>
		/// <param name="valuepart1">Initial <see cref="Bapiparex.Valuepart1" /> value</param>
		/// <param name="valuepart2">Initial <see cref="Bapiparex.Valuepart2" /> value</param>
		/// <param name="valuepart3">Initial <see cref="Bapiparex.Valuepart3" /> value</param>
		/// <param name="valuepart4">Initial <see cref="Bapiparex.Valuepart4" /> value</param>
		/// <param name="active">Initial <see cref="Bapiparex.Active" /> value</param>
		/// <param name="updBy">Initial <see cref="Bapiparex.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="Bapiparex.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="Bapiparex.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="Bapiparex.CreDate" /> value</param>
		/// <param name="rowVersion">Initial <see cref="Bapiparex.RowVersion" /> value</param>
		/// <param name="updPgm">Initial <see cref="Bapiparex.UpdPgm" /> value</param>
		public Bapiparex(string docNo, string structure, string valuepart1, string valuepart2, string valuepart3, string valuepart4, bool active, long updBy, DateTime updDate, long creBy, DateTime creDate, byte[] rowVersion, string updPgm)
		{
			this.docNo = docNo;
			this.structure = structure;
			this.valuepart1 = valuepart1;
			this.valuepart2 = valuepart2;
			this.valuepart3 = valuepart3;
			this.valuepart4 = valuepart4;
			this.active = active;
			this.updBy = updBy;
			this.updDate = updDate;
			this.creBy = creBy;
			this.creDate = creDate;
			this.rowVersion = rowVersion;
			this.updPgm = updPgm;
		}
	
		/// <summary>
		/// Minimal constructor for class Bapiparex
		/// </summary>
		/// <param name="docNo">Initial <see cref="Bapiparex.DocNo" /> value</param>
		/// <param name="active">Initial <see cref="Bapiparex.Active" /> value</param>
		/// <param name="updBy">Initial <see cref="Bapiparex.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="Bapiparex.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="Bapiparex.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="Bapiparex.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="Bapiparex.UpdPgm" /> value</param>
		public Bapiparex(string docNo, bool active, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm)
		{
			this.docNo = docNo;
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
		/// Gets or sets the Id for the current Bapiparex
		/// </summary>
		public virtual long Id
		{
			get { return this.id; }
			set { this.id = value; }
		}
		
		/// <summary>
		/// Gets or sets the DocNo for the current Bapiparex
		/// </summary>
		public virtual string DocNo
		{
			get { return this.docNo; }
			set { this.docNo = value; }
		}
		
		/// <summary>
		/// Gets or sets the Structure for the current Bapiparex
		/// </summary>
		public virtual string Structure
		{
			get { return this.structure; }
			set { this.structure = value; }
		}
		
		/// <summary>
		/// Gets or sets the Valuepart1 for the current Bapiparex
		/// </summary>
		public virtual string Valuepart1
		{
			get { return this.valuepart1; }
			set { this.valuepart1 = value; }
		}
		
		/// <summary>
		/// Gets or sets the Valuepart2 for the current Bapiparex
		/// </summary>
		public virtual string Valuepart2
		{
			get { return this.valuepart2; }
			set { this.valuepart2 = value; }
		}
		
		/// <summary>
		/// Gets or sets the Valuepart3 for the current Bapiparex
		/// </summary>
		public virtual string Valuepart3
		{
			get { return this.valuepart3; }
			set { this.valuepart3 = value; }
		}
		
		/// <summary>
		/// Gets or sets the Valuepart4 for the current Bapiparex
		/// </summary>
		public virtual string Valuepart4
		{
			get { return this.valuepart4; }
			set { this.valuepart4 = value; }
		}
		
		/// <summary>
		/// Gets or sets the Active for the current Bapiparex
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdBy for the current Bapiparex
		/// </summary>
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdDate for the current Bapiparex
		/// </summary>
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreBy for the current Bapiparex
		/// </summary>
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreDate for the current Bapiparex
		/// </summary>
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the RowVersion for the current Bapiparex
		/// </summary>
		public virtual byte[] RowVersion
		{
			get { return this.rowVersion; }
			set { this.rowVersion = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdPgm for the current Bapiparex
		/// </summary>
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}
		
		#endregion
	}
}

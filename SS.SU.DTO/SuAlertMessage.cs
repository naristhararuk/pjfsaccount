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
	/// POJO for SuAlertMessage. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class SuAlertMessage
	{
		#region Fields
		
		private short alertMessageid;
		private string alertMessageCode;
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
		/// Initializes a new instance of the SuAlertMessage class
		/// </summary>
		public SuAlertMessage()
		{
		}

		public SuAlertMessage(short alertMessageid)
		{
			this.alertMessageid = alertMessageid;
		}
	
		/// <summary>
		/// Initializes a new instance of the SuAlertMessage class
		/// </summary>
		/// <param name="alertMessageCode">Initial <see cref="SuAlertMessage.AlertMessageCode" /> value</param>
		/// <param name="comment">Initial <see cref="SuAlertMessage.Comment" /> value</param>
		/// <param name="updBy">Initial <see cref="SuAlertMessage.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="SuAlertMessage.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="SuAlertMessage.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="SuAlertMessage.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="SuAlertMessage.UpdPgm" /> value</param>
		/// <param name="rowVersion">Initial <see cref="SuAlertMessage.RowVersion" /> value</param>
		/// <param name="active">Initial <see cref="SuAlertMessage.Active" /> value</param>
		public SuAlertMessage(string alertMessageCode, string comment, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, Byte[] rowVersion, bool active)
		{
			this.alertMessageCode = alertMessageCode;
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
		/// Minimal constructor for class SuAlertMessage
		/// </summary>
		/// <param name="alertMessageCode">Initial <see cref="SuAlertMessage.AlertMessageCode" /> value</param>
		/// <param name="updBy">Initial <see cref="SuAlertMessage.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="SuAlertMessage.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="SuAlertMessage.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="SuAlertMessage.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="SuAlertMessage.UpdPgm" /> value</param>
		/// <param name="active">Initial <see cref="SuAlertMessage.Active" /> value</param>
		public SuAlertMessage(string alertMessageCode, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, bool active)
		{
			this.alertMessageCode = alertMessageCode;
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
		/// Gets or sets the AlertMessageid for the current SuAlertMessage
		/// </summary>
		public virtual short AlertMessageid
		{
			get { return this.alertMessageid; }
			set { this.alertMessageid = value; }
		}
		
		/// <summary>
		/// Gets or sets the AlertMessageCode for the current SuAlertMessage
		/// </summary>
		public virtual string AlertMessageCode
		{
			get { return this.alertMessageCode; }
			set { this.alertMessageCode = value; }
		}
		
		/// <summary>
		/// Gets or sets the Comment for the current SuAlertMessage
		/// </summary>
		public virtual string Comment
		{
			get { return this.comment; }
			set { this.comment = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdBy for the current SuAlertMessage
		/// </summary>
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdDate for the current SuAlertMessage
		/// </summary>
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreBy for the current SuAlertMessage
		/// </summary>
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreDate for the current SuAlertMessage
		/// </summary>
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdPgm for the current SuAlertMessage
		/// </summary>
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}
		
		/// <summary>
		/// Gets or sets the RowVersion for the current SuAlertMessage
		/// </summary>
		public virtual Byte[] RowVersion
		{
			get { return this.rowVersion; }
			set { this.rowVersion = value; }
		}
		
		/// <summary>
		/// Gets or sets the Active for the current SuAlertMessage
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		
		#endregion
	}
}

//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by NHibernate.
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

using System;

namespace SS.Standard.WorkFlow.DTO
{
	/// <summary>
	/// POJO for WorkFlowType. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class WorkFlowType
	{
		#region Fields
		
		private int workFlowTypeID;
		private string displayControlName;
		private bool active;
		private long creBy;
		private DateTime creDate;
		private long updBy;
		private DateTime updDate;
		private string updPgm;
		private byte[] rowVersion;

		#endregion

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the WorkFlowType class
		/// </summary>
		public WorkFlowType()
		{
		}

		public WorkFlowType(int workFlowTypeID)
		{
			this.workFlowTypeID = workFlowTypeID;
		}
	
		/// <summary>
		/// Initializes a new instance of the WorkFlowType class
		/// </summary>
		/// <param name="displayControlName">Initial <see cref="WorkFlowType.DisplayControlName" /> value</param>
		/// <param name="active">Initial <see cref="WorkFlowType.Active" /> value</param>
		/// <param name="creBy">Initial <see cref="WorkFlowType.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="WorkFlowType.CreDate" /> value</param>
		/// <param name="updBy">Initial <see cref="WorkFlowType.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="WorkFlowType.UpdDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="WorkFlowType.UpdPgm" /> value</param>
		/// <param name="rowVersion">Initial <see cref="WorkFlowType.RowVersion" /> value</param>
		public WorkFlowType(string displayControlName, bool active, long creBy, DateTime creDate, long updBy, DateTime updDate, string updPgm, byte[] rowVersion)
		{
			this.displayControlName = displayControlName;
			this.active = active;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updBy = updBy;
			this.updDate = updDate;
			this.updPgm = updPgm;
			this.rowVersion = rowVersion;
		}
	
		/// <summary>
		/// Minimal constructor for class WorkFlowType
		/// </summary>
		/// <param name="active">Initial <see cref="WorkFlowType.Active" /> value</param>
		/// <param name="creBy">Initial <see cref="WorkFlowType.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="WorkFlowType.CreDate" /> value</param>
		/// <param name="updBy">Initial <see cref="WorkFlowType.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="WorkFlowType.UpdDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="WorkFlowType.UpdPgm" /> value</param>
		public WorkFlowType(bool active, long creBy, DateTime creDate, long updBy, DateTime updDate, string updPgm)
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
		/// Gets or sets the workFlowTypeID for the current WorkFlowType
		/// </summary>
		public virtual int WorkFlowTypeID
		{
			get { return this.workFlowTypeID; }
			set { this.workFlowTypeID = value; }
		}
		
		/// <summary>
		/// Gets or sets the DisplayControlName for the current WorkFlowType
		/// </summary>
		public virtual string DisplayControlName
		{
			get { return this.displayControlName; }
			set { this.displayControlName = value; }
		}
		
		/// <summary>
		/// Gets or sets the Active for the current WorkFlowType
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreBy for the current WorkFlowType
		/// </summary>
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreDate for the current WorkFlowType
		/// </summary>
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdBy for the current WorkFlowType
		/// </summary>
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdDate for the current WorkFlowType
		/// </summary>
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdPgm for the current WorkFlowType
		/// </summary>
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}
		
		/// <summary>
		/// Gets or sets the RowVersion for the current WorkFlowType
		/// </summary>
		public virtual byte[] RowVersion
		{
			get { return this.rowVersion; }
			set { this.rowVersion = value; }
		}
		
		#endregion
	}
}

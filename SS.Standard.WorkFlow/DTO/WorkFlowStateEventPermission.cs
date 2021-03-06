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
	/// POJO for WorkFlowStateEventPermission. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class WorkFlowStateEventPermission
	{
		#region Fields
		
		private long workFlowStateEventPermissionID;
		private short? roleID;
		private long? userID;
		private bool active;
		private long creBy;
		private DateTime creDate;
		private long updBy;
		private DateTime updDate;
		private string updPgm;
		private byte[] rowVersion;
		private SS.Standard.WorkFlow.DTO.WorkFlow workFlow;
		private SS.Standard.WorkFlow.DTO.WorkFlowStateEvent workFlowStateEvent;

		#endregion

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the WorkFlowStateEventPermission class
		/// </summary>
		public WorkFlowStateEventPermission()
		{
		}

		public WorkFlowStateEventPermission(long workFlowStateEventPermissionID)
		{
			this.workFlowStateEventPermissionID = workFlowStateEventPermissionID;
		}
	
		/// <summary>
		/// Initializes a new instance of the WorkFlowStateEventPermission class
		/// </summary>
		/// <param name="roleID">Initial <see cref="WorkFlowStateEventPermission.RoleID" /> value</param>
		/// <param name="userID">Initial <see cref="WorkFlowStateEventPermission.UserID" /> value</param>
		/// <param name="active">Initial <see cref="WorkFlowStateEventPermission.Active" /> value</param>
		/// <param name="creBy">Initial <see cref="WorkFlowStateEventPermission.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="WorkFlowStateEventPermission.CreDate" /> value</param>
		/// <param name="updBy">Initial <see cref="WorkFlowStateEventPermission.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="WorkFlowStateEventPermission.UpdDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="WorkFlowStateEventPermission.UpdPgm" /> value</param>
		/// <param name="rowVersion">Initial <see cref="WorkFlowStateEventPermission.RowVersion" /> value</param>
		/// <param name="workFlowID">Initial <see cref="WorkFlowStateEventPermission.WorkFlowID" /> value</param>
		/// <param name="workFlowStateEventID">Initial <see cref="WorkFlowStateEventPermission.WorkFlowStateEventID" /> value</param>
		public WorkFlowStateEventPermission(short? roleID, long? userID, bool active, long creBy, DateTime creDate, long updBy, DateTime updDate, string updPgm, byte[] rowVersion, SS.Standard.WorkFlow.DTO.WorkFlow workFlowID, SS.Standard.WorkFlow.DTO.WorkFlowStateEvent workFlowStateEventID)
		{
			this.roleID = roleID;
			this.userID = userID;
			this.active = active;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updBy = updBy;
			this.updDate = updDate;
			this.updPgm = updPgm;
			this.rowVersion = rowVersion;
			this.workFlow = workFlow;
			this.workFlowStateEvent = workFlowStateEvent;
		}
	
		/// <summary>
		/// Minimal constructor for class WorkFlowStateEventPermission
		/// </summary>
		/// <param name="active">Initial <see cref="WorkFlowStateEventPermission.Active" /> value</param>
		/// <param name="creBy">Initial <see cref="WorkFlowStateEventPermission.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="WorkFlowStateEventPermission.CreDate" /> value</param>
		/// <param name="updBy">Initial <see cref="WorkFlowStateEventPermission.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="WorkFlowStateEventPermission.UpdDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="WorkFlowStateEventPermission.UpdPgm" /> value</param>
		public WorkFlowStateEventPermission(bool active, long creBy, DateTime creDate, long updBy, DateTime updDate, string updPgm)
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
		/// Gets or sets the WorkFlowStateEventPermissionID for the current WorkFlowStateEventPermission
		/// </summary>
		public virtual long WorkFlowStateEventPermissionID
		{
			get { return this.workFlowStateEventPermissionID; }
			set { this.workFlowStateEventPermissionID = value; }
		}
		
		/// <summary>
		/// Gets or sets the RoleID for the current WorkFlowStateEventPermission
		/// </summary>
		public virtual short? RoleID
		{
			get { return this.roleID; }
			set { this.roleID = value; }
		}
		
		/// <summary>
		/// Gets or sets the UserID for the current WorkFlowStateEventPermission
		/// </summary>
		public virtual long? UserID
		{
			get { return this.userID; }
			set { this.userID = value; }
		}
		
		/// <summary>
		/// Gets or sets the Active for the current WorkFlowStateEventPermission
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreBy for the current WorkFlowStateEventPermission
		/// </summary>
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreDate for the current WorkFlowStateEventPermission
		/// </summary>
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdBy for the current WorkFlowStateEventPermission
		/// </summary>
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdDate for the current WorkFlowStateEventPermission
		/// </summary>
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdPgm for the current WorkFlowStateEventPermission
		/// </summary>
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}
		
		/// <summary>
		/// Gets or sets the RowVersion for the current WorkFlowStateEventPermission
		/// </summary>
		public virtual byte[] RowVersion
		{
			get { return this.rowVersion; }
			set { this.rowVersion = value; }
		}
		
		/// <summary>
		/// Gets or sets the WorkFlow for the current WorkFlowStateEventPermission
		/// </summary>
		public virtual SS.Standard.WorkFlow.DTO.WorkFlow WorkFlow
		{
			get { return this.workFlow; }
			set { this.workFlow = value; }
		}
		
		/// <summary>
		/// Gets or sets the WorkFlowStateEvent for the current WorkFlowStateEventPermission
		/// </summary>
		public virtual SS.Standard.WorkFlow.DTO.WorkFlowStateEvent WorkFlowStateEvent
		{
			get { return this.workFlowStateEvent; }
			set { this.workFlowStateEvent = value; }
		}
		
		#endregion
	}
}

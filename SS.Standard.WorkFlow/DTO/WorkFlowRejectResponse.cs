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
	/// POJO for WorkFlowRejectResponse. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class WorkFlowRejectResponse
	{
		#region Fields
		
		private long workFlowRejectResponseID;
		private string remark;
		private bool needApproveRejection;
		private bool active;
		private long creBy;
		private DateTime creDate;
		private long updBy;
		private DateTime updDate;
		private string updPgm;
		private byte[] rowVersion;
		private SS.Standard.WorkFlow.DTO.WorkFlowResponse workFlowResponse;
		private SCG.DB.DTO.DbRejectReason reason;

		#endregion

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the WorkFlowRejectResponse class
		/// </summary>
		public WorkFlowRejectResponse()
		{
		}

		public WorkFlowRejectResponse(long workFlowRejectResponseID)
		{
			this.workFlowRejectResponseID = workFlowRejectResponseID;
		}
	
		/// <summary>
		/// Initializes a new instance of the WorkFlowRejectResponse class
		/// </summary>
		/// <param name="remark">Initial <see cref="WorkFlowRejectResponse.Remark" /> value</param>
		/// <param name="needApproveRejection">Initial <see cref="WorkFlowRejectResponse.NeedApproveRejection" /> value</param>
		/// <param name="active">Initial <see cref="WorkFlowRejectResponse.Active" /> value</param>
		/// <param name="creBy">Initial <see cref="WorkFlowRejectResponse.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="WorkFlowRejectResponse.CreDate" /> value</param>
		/// <param name="updBy">Initial <see cref="WorkFlowRejectResponse.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="WorkFlowRejectResponse.UpdDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="WorkFlowRejectResponse.UpdPgm" /> value</param>
		/// <param name="rowVersion">Initial <see cref="WorkFlowRejectResponse.RowVersion" /> value</param>
		/// <param name="workFlowResponse">Initial <see cref="WorkFlowRejectResponse.WorkFlowResponse" /> value</param>
		/// <param name="reason">Initial <see cref="WorkFlowRejectResponse.Reason" /> value</param>
        public WorkFlowRejectResponse(string remark, bool needApproveRejection, bool active, long creBy, DateTime creDate, long updBy, DateTime updDate, string updPgm, byte[] rowVersion, SS.Standard.WorkFlow.DTO.WorkFlowResponse workFlowResponse, SCG.DB.DTO.DbRejectReason reason)
		{
			this.remark = remark;
			this.needApproveRejection = needApproveRejection;
			this.active = active;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updBy = updBy;
			this.updDate = updDate;
			this.updPgm = updPgm;
			this.rowVersion = rowVersion;
			this.workFlowResponse = workFlowResponse;
			this.reason = reason;
		}
	
		/// <summary>
		/// Minimal constructor for class WorkFlowRejectResponse
		/// </summary>
		/// <param name="active">Initial <see cref="WorkFlowRejectResponse.Active" /> value</param>
		/// <param name="creBy">Initial <see cref="WorkFlowRejectResponse.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="WorkFlowRejectResponse.CreDate" /> value</param>
		/// <param name="updDate">Initial <see cref="WorkFlowRejectResponse.UpdDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="WorkFlowRejectResponse.UpdPgm" /> value</param>
		public WorkFlowRejectResponse(bool active, long creBy, DateTime creDate, DateTime updDate, string updPgm)
		{
			this.active = active;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updDate = updDate;
			this.updPgm = updPgm;
		}
		#endregion
	
		#region Properties
		
		/// <summary>
		/// Gets or sets the workFlowRejectResponseID for the current WorkFlowRejectResponse
		/// </summary>
		public virtual long WorkFlowRejectResponseID
		{
			get { return this.workFlowRejectResponseID; }
			set { this.workFlowRejectResponseID = value; }
		}
		
		/// <summary>
		/// Gets or sets the Remark for the current WorkFlowRejectResponse
		/// </summary>
		public virtual string Remark
		{
			get { return this.remark; }
			set { this.remark = value; }
		}
		
		/// <summary>
		/// Gets or sets the NeedApproveRejection for the current WorkFlowRejectResponse
		/// </summary>
		public virtual bool NeedApproveRejection
		{
			get { return this.needApproveRejection; }
			set { this.needApproveRejection = value; }
		}
		
		/// <summary>
		/// Gets or sets the Active for the current WorkFlowRejectResponse
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreBy for the current WorkFlowRejectResponse
		/// </summary>
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreDate for the current WorkFlowRejectResponse
		/// </summary>
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdBy for the current WorkFlowRejectResponse
		/// </summary>
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdDate for the current WorkFlowRejectResponse
		/// </summary>
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdPgm for the current WorkFlowRejectResponse
		/// </summary>
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}
		
		/// <summary>
		/// Gets or sets the RowVersion for the current WorkFlowRejectResponse
		/// </summary>
		public virtual byte[] RowVersion
		{
			get { return this.rowVersion; }
			set { this.rowVersion = value; }
		}
		
		/// <summary>
		/// Gets or sets the WorkFlowResponse for the current WorkFlowRejectResponse
		/// </summary>
		public virtual SS.Standard.WorkFlow.DTO.WorkFlowResponse WorkFlowResponse
		{
			get { return this.workFlowResponse; }
			set { this.workFlowResponse = value; }
		}
		
		/// <summary>
		/// Gets or sets the Reason for the current WorkFlowRejectResponse
		/// </summary>
        public virtual SCG.DB.DTO.DbRejectReason Reason
		{
			get { return this.reason; }
			set { this.reason = value; }
		}
		
		#endregion
	}
}
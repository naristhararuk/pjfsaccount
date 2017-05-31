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
	/// POJO for WorkFlowHoldResponseDetail. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class WorkFlowHoldResponseDetail
	{
		#region Fields
		
		private long workFlowHoldResponseDetailID;
		private string fieldName;
		private bool active;
		private long creBy;
		private DateTime creDate;
		private long updBy;
		private DateTime updDate;
		private string updPgm;
		private byte[] rowVersion;
		private SS.Standard.WorkFlow.DTO.WorkFlowHoldResponse workFlowHoldResponse;

		#endregion

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the WorkFlowHoldResponseDetail class
		/// </summary>
		public WorkFlowHoldResponseDetail()
		{
		}

		public WorkFlowHoldResponseDetail(long workFlowHoldResponseDetailID)
		{
			this.workFlowHoldResponseDetailID = workFlowHoldResponseDetailID;
		}
	
		/// <summary>
		/// Initializes a new instance of the WorkFlowHoldResponseDetail class
		/// </summary>
		/// <param name="fieldName">Initial <see cref="WorkFlowHoldResponseDetail.FieldName" /> value</param>
		/// <param name="active">Initial <see cref="WorkFlowHoldResponseDetail.Active" /> value</param>
		/// <param name="creBy">Initial <see cref="WorkFlowHoldResponseDetail.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="WorkFlowHoldResponseDetail.CreDate" /> value</param>
		/// <param name="updBy">Initial <see cref="WorkFlowHoldResponseDetail.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="WorkFlowHoldResponseDetail.UpdDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="WorkFlowHoldResponseDetail.UpdPgm" /> value</param>
		/// <param name="rowVersion">Initial <see cref="WorkFlowHoldResponseDetail.RowVersion" /> value</param>
		/// <param name="workFlowHoldResponse">Initial <see cref="WorkFlowHoldResponseDetail.WorkFlowHoldResponse" /> value</param>
		public WorkFlowHoldResponseDetail(string fieldName, bool active, long creBy, DateTime creDate, long updBy, DateTime updDate, string updPgm, byte[] rowVersion, SS.Standard.WorkFlow.DTO.WorkFlowHoldResponse workFlowHoldResponse)
		{
			this.fieldName = fieldName;
			this.active = active;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updBy = updBy;
			this.updDate = updDate;
			this.updPgm = updPgm;
			this.rowVersion = rowVersion;
			this.workFlowHoldResponse = workFlowHoldResponse;
		}
	
		/// <summary>
		/// Minimal constructor for class WorkFlowHoldResponseDetail
		/// </summary>
		/// <param name="active">Initial <see cref="WorkFlowHoldResponseDetail.Active" /> value</param>
		/// <param name="creBy">Initial <see cref="WorkFlowHoldResponseDetail.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="WorkFlowHoldResponseDetail.CreDate" /> value</param>
		/// <param name="updDate">Initial <see cref="WorkFlowHoldResponseDetail.UpdDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="WorkFlowHoldResponseDetail.UpdPgm" /> value</param>
		public WorkFlowHoldResponseDetail(bool active, long creBy, DateTime creDate, DateTime updDate, string updPgm)
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
		/// Gets or sets the workFlowHoldResponseDetailID for the current WorkFlowHoldResponseDetail
		/// </summary>
		public virtual long WorkFlowHoldResponseDetailID
		{
			get { return this.workFlowHoldResponseDetailID; }
			set { this.workFlowHoldResponseDetailID = value; }
		}
		
		/// <summary>
		/// Gets or sets the FieldName for the current WorkFlowHoldResponseDetail
		/// </summary>
		public virtual string FieldName
		{
			get { return this.fieldName; }
			set { this.fieldName = value; }
		}
		
		/// <summary>
		/// Gets or sets the Active for the current WorkFlowHoldResponseDetail
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreBy for the current WorkFlowHoldResponseDetail
		/// </summary>
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreDate for the current WorkFlowHoldResponseDetail
		/// </summary>
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdBy for the current WorkFlowHoldResponseDetail
		/// </summary>
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdDate for the current WorkFlowHoldResponseDetail
		/// </summary>
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdPgm for the current WorkFlowHoldResponseDetail
		/// </summary>
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}
		
		/// <summary>
		/// Gets or sets the RowVersion for the current WorkFlowHoldResponseDetail
		/// </summary>
		public virtual byte[] RowVersion
		{
			get { return this.rowVersion; }
			set { this.rowVersion = value; }
		}
		
		/// <summary>
		/// Gets or sets the WorkFlowHoldResponse for the current WorkFlowHoldResponseDetail
		/// </summary>
		public virtual SS.Standard.WorkFlow.DTO.WorkFlowHoldResponse WorkFlowHoldResponse
		{
			get { return this.workFlowHoldResponse; }
			set { this.workFlowHoldResponse = value; }
		}
		
		#endregion
	}
}
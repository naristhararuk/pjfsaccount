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
	/// POJO for WorkFlowResponseToken. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class WorkFlowResponseToken
	{
		#region Fields
		
		private long workFlowResponseTokenID;
		private string tokenCode;
		private long userID;
		private string tokenType;
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
		/// Initializes a new instance of the WorkFlowResponseToken class
		/// </summary>
		public WorkFlowResponseToken()
		{
		}

		public WorkFlowResponseToken(long workFlowResponseTokenID)
		{
			this.workFlowResponseTokenID = workFlowResponseTokenID;
		}
	
		/// <summary>
		/// Initializes a new instance of the WorkFlowResponseToken class
		/// </summary>
		/// <param name="tokenCode">Initial <see cref="WorkFlowResponseToken.TokenCode" /> value</param>
		/// <param name="userID">Initial <see cref="WorkFlowResponseToken.userID" /> value</param>
		/// <param name="tokenType">Initial <see cref="WorkFlowResponseToken.tokenType" /> value</param>
		/// <param name="workFlowStateEventID">Initial <see cref="WorkFlowResponseToken.WorkFlowStateEventID" /> value</param>
		/// <param name="active">Initial <see cref="WorkFlowResponseToken.Active" /> value</param>
		/// <param name="creBy">Initial <see cref="WorkFlowResponseToken.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="WorkFlowResponseToken.CreDate" /> value</param>
		/// <param name="updBy">Initial <see cref="WorkFlowResponseToken.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="WorkFlowResponseToken.UpdDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="WorkFlowResponseToken.UpdPgm" /> value</param>
		/// <param name="rowVersion">Initial <see cref="WorkFlowResponseToken.RowVersion" /> value</param>
		/// <param name="workFlow">Initial <see cref="WorkFlowResponseToken.workFlow" /> value</param>
		public WorkFlowResponseToken(string tokenCode, long userID, string tokenType, bool active, long creBy, DateTime creDate, long updBy, DateTime updDate, string updPgm, byte[] rowVersion, SS.Standard.WorkFlow.DTO.WorkFlow workFlow)
		{
			this.tokenCode = tokenCode;
			this.userID = userID;
			this.tokenType = tokenType;
			this.active = active;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updBy = updBy;
			this.updDate = updDate;
			this.updPgm = updPgm;
			this.rowVersion = rowVersion;
			this.workFlow = workFlow;
		}
	
		/// <summary>
		/// Minimal constructor for class WorkFlowResponseToken
		/// </summary>
		/// <param name="active">Initial <see cref="WorkFlowResponseToken.Active" /> value</param>
		/// <param name="creBy">Initial <see cref="WorkFlowResponseToken.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="WorkFlowResponseToken.CreDate" /> value</param>
		/// <param name="updDate">Initial <see cref="WorkFlowResponseToken.UpdDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="WorkFlowResponseToken.UpdPgm" /> value</param>
		public WorkFlowResponseToken(bool active, long creBy, DateTime creDate, DateTime updDate, string updPgm)
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
		/// Gets or sets the WorkFlowResponseTokenID for the current WorkFlowResponseToken
		/// </summary>
		public virtual long WorkFlowResponseTokenID
		{
			get { return this.workFlowResponseTokenID; }
			set { this.workFlowResponseTokenID = value; }
		}
		
		/// <summary>
		/// Gets or sets the TokenCode for the current WorkFlowResponseToken
		/// </summary>
		public virtual string TokenCode
		{
			get { return this.tokenCode; }
			set { this.tokenCode = value; }
		}
		
		/// <summary>
		/// Gets or sets the userID for the current WorkFlowResponseToken
		/// </summary>
		public virtual long UserID
		{
			get { return this.userID; }
			set { this.userID = value; }
		}
		
		/// <summary>
		/// Gets or sets the tokenType for the current WorkFlowResponseToken
		/// </summary>
		public virtual string TokenType
		{
			get { return this.tokenType; }
			set { this.tokenType = value; }
		}
		
		
		/// <summary>
		/// Gets or sets the Active for the current WorkFlowResponseToken
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreBy for the current WorkFlowResponseToken
		/// </summary>
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreDate for the current WorkFlowResponseToken
		/// </summary>
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdBy for the current WorkFlowResponseToken
		/// </summary>
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdDate for the current WorkFlowResponseToken
		/// </summary>
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdPgm for the current WorkFlowResponseToken
		/// </summary>
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}
		
		/// <summary>
		/// Gets or sets the RowVersion for the current WorkFlowResponseToken
		/// </summary>
		public virtual byte[] RowVersion
		{
			get { return this.rowVersion; }
			set { this.rowVersion = value; }
		}
		
		/// <summary>
		/// Gets or sets the workFlow for the current WorkFlowResponseToken
		/// </summary>
		public virtual SS.Standard.WorkFlow.DTO.WorkFlow WorkFlow
		{
			get { return this.workFlow; }
			set { this.workFlow = value; }
		}

        /// <summary>
        /// Gets or sets the WorkFlowStateEvent for the current WorkFlowResponseToken
        /// </summary>
        public virtual SS.Standard.WorkFlow.DTO.WorkFlowStateEvent WorkFlowStateEvent
        {
            get { return this.workFlowStateEvent; }
            set { this.workFlowStateEvent = value; }
        }
		
		
		#endregion
	}
}

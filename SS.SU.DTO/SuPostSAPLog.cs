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
	/// POJO for SuPostSAPLog. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class SuPostSAPLog
	{
		#region Fields
		
		private long postSAPLogID;
		private DateTime date;
		private string requestNo;
		private decimal postNo;
		private decimal documentSeqOnRequest;
		private string invoiceNo;
		private decimal year;
		private string companyCode;
		private string fiDocument;
		private string flag;
		private string message;
		private string status;
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
		/// Initializes a new instance of the SuPostSAPLog class
		/// </summary>
		public SuPostSAPLog()
		{
		}

		public SuPostSAPLog(long postSAPLogID)
		{
			this.postSAPLogID = postSAPLogID;
		}
	
		/// <summary>
		/// Initializes a new instance of the SuPostSAPLog class
		/// </summary>
		/// <param name="date">Initial <see cref="SuPostSAPLog.Date" /> value</param>
		/// <param name="requestNo">Initial <see cref="SuPostSAPLog.RequestNo" /> value</param>
		/// <param name="postNo">Initial <see cref="SuPostSAPLog.PostNo" /> value</param>
		/// <param name="documentSeqOnRequest">Initial <see cref="SuPostSAPLog.DocumentSeqOnRequest" /> value</param>
		/// <param name="invoiceNo">Initial <see cref="SuPostSAPLog.InvoiceNo" /> value</param>
		/// <param name="year">Initial <see cref="SuPostSAPLog.Year" /> value</param>
		/// <param name="companyCode">Initial <see cref="SuPostSAPLog.CompanyCode" /> value</param>
		/// <param name="fiDocument">Initial <see cref="SuPostSAPLog.FiDocument" /> value</param>
		/// <param name="flag">Initial <see cref="SuPostSAPLog.Flag" /> value</param>
		/// <param name="message">Initial <see cref="SuPostSAPLog.Message" /> value</param>
		/// <param name="status">Initial <see cref="SuPostSAPLog.Status" /> value</param>
		/// <param name="active">Initial <see cref="SuPostSAPLog.Active" /> value</param>
		/// <param name="creBy">Initial <see cref="SuPostSAPLog.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="SuPostSAPLog.CreDate" /> value</param>
		/// <param name="updBy">Initial <see cref="SuPostSAPLog.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="SuPostSAPLog.UpdDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="SuPostSAPLog.UpdPgm" /> value</param>
		/// <param name="rowVersion">Initial <see cref="SuPostSAPLog.RowVersion" /> value</param>
		public SuPostSAPLog(DateTime date, string requestNo, decimal postNo, decimal documentSeqOnRequest, string invoiceNo, decimal year, string companyCode, string fiDocument, string flag, string message, string status, bool active, long creBy, DateTime creDate, long updBy, DateTime updDate, string updPgm, byte[] rowVersion)
		{
			this.date = date;
			this.requestNo = requestNo;
			this.postNo = postNo;
			this.documentSeqOnRequest = documentSeqOnRequest;
			this.invoiceNo = invoiceNo;
			this.year = year;
			this.companyCode = companyCode;
			this.fiDocument = fiDocument;
			this.flag = flag;
			this.message = message;
			this.status = status;
			this.active = active;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updBy = updBy;
			this.updDate = updDate;
			this.updPgm = updPgm;
			this.rowVersion = rowVersion;
		}
	
		/// <summary>
		/// Minimal constructor for class SuPostSAPLog
		/// </summary>
		/// <param name="active">Initial <see cref="SuPostSAPLog.Active" /> value</param>
		/// <param name="creBy">Initial <see cref="SuPostSAPLog.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="SuPostSAPLog.CreDate" /> value</param>
		/// <param name="updBy">Initial <see cref="SuPostSAPLog.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="SuPostSAPLog.UpdDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="SuPostSAPLog.UpdPgm" /> value</param>
		/// <param name="rowVersion">Initial <see cref="SuPostSAPLog.RowVersion" /> value</param>
		public SuPostSAPLog(bool active, long creBy, DateTime creDate, long updBy, DateTime updDate, string updPgm, byte[] rowVersion)
		{
			this.active = active;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updBy = updBy;
			this.updDate = updDate;
			this.updPgm = updPgm;
			this.rowVersion = rowVersion;
		}
		#endregion
	
		#region Properties
		
		/// <summary>
		/// Gets or sets the PostSAPLogID for the current SuPostSAPLog
		/// </summary>
		public virtual long PostSAPLogID
		{
			get { return this.postSAPLogID; }
			set { this.postSAPLogID = value; }
		}
		
		/// <summary>
		/// Gets or sets the Date for the current SuPostSAPLog
		/// </summary>
		public virtual DateTime Date
		{
			get { return this.date; }
			set { this.date = value; }
		}
		
		/// <summary>
		/// Gets or sets the RequestNo for the current SuPostSAPLog
		/// </summary>
		public virtual string RequestNo
		{
			get { return this.requestNo; }
			set { this.requestNo = value; }
		}
		
		/// <summary>
		/// Gets or sets the PostNo for the current SuPostSAPLog
		/// </summary>
		public virtual decimal PostNo
		{
			get { return this.postNo; }
			set { this.postNo = value; }
		}
		
		/// <summary>
		/// Gets or sets the DocumentSeqOnRequest for the current SuPostSAPLog
		/// </summary>
		public virtual decimal DocumentSeqOnRequest
		{
			get { return this.documentSeqOnRequest; }
			set { this.documentSeqOnRequest = value; }
		}
		
		/// <summary>
		/// Gets or sets the InvoiceNo for the current SuPostSAPLog
		/// </summary>
		public virtual string InvoiceNo
		{
			get { return this.invoiceNo; }
			set { this.invoiceNo = value; }
		}
		
		/// <summary>
		/// Gets or sets the Year for the current SuPostSAPLog
		/// </summary>
		public virtual decimal Year
		{
			get { return this.year; }
			set { this.year = value; }
		}
		
		/// <summary>
		/// Gets or sets the CompanyCode for the current SuPostSAPLog
		/// </summary>
		public virtual string CompanyCode
		{
			get { return this.companyCode; }
			set { this.companyCode = value; }
		}
		
		/// <summary>
		/// Gets or sets the FiDocument for the current SuPostSAPLog
		/// </summary>
		public virtual string FiDocument
		{
			get { return this.fiDocument; }
			set { this.fiDocument = value; }
		}
		
		/// <summary>
		/// Gets or sets the Flag for the current SuPostSAPLog
		/// </summary>
		public virtual string Flag
		{
			get { return this.flag; }
			set { this.flag = value; }
		}
		
		/// <summary>
		/// Gets or sets the Message for the current SuPostSAPLog
		/// </summary>
		public virtual string Message
		{
			get { return this.message; }
			set { this.message = value; }
		}
		
		/// <summary>
		/// Gets or sets the Status for the current SuPostSAPLog
		/// </summary>
		public virtual string Status
		{
			get { return this.status; }
			set { this.status = value; }
		}
		
		/// <summary>
		/// Gets or sets the Active for the current SuPostSAPLog
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreBy for the current SuPostSAPLog
		/// </summary>
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreDate for the current SuPostSAPLog
		/// </summary>
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdBy for the current SuPostSAPLog
		/// </summary>
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdDate for the current SuPostSAPLog
		/// </summary>
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdPgm for the current SuPostSAPLog
		/// </summary>
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}
		
		/// <summary>
		/// Gets or sets the RowVersion for the current SuPostSAPLog
		/// </summary>
		public virtual byte[] RowVersion
		{
			get { return this.rowVersion; }
			set { this.rowVersion = value; }
		}
		
		#endregion
	}
}

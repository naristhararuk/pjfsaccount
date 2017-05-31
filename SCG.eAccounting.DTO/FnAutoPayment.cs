//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by NHibernate.
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

using System;

namespace SCG.eAccounting.DTO
{
	/// <summary>
	/// POJO for FnAutoPayment. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class FnAutoPayment
	{
		#region Fields
		
		private long autoPaymentID;
		private string fIDoc;
		private int status;
		private string chequeNumber;
		private string chequeBankName;
		private DateTime? chequeDate;
		private string payeeBankName;
		private string payeeBankAccountNumber;
		private double amount;
		private DateTime? paymentDate;
		private string currencyDoc;
		private string currencyPay;
		private string _companycode;
		private string _year;
		private bool active;
		private long creBy;
		private DateTime creDate;
		private long updBy;
		private DateTime updDate;
		private string updPgm;
		private Byte[] rowVersion;
		private SCG.eAccounting.DTO.SCGDocument document;

		#endregion

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the FnAutoPayment class
		/// </summary>
		public FnAutoPayment()
		{
		}

		public FnAutoPayment(long autoPaymentID)
		{
			this.autoPaymentID = autoPaymentID;
		}
	
		/// <summary>
		/// Initializes a new instance of the FnAutoPayment class
		/// </summary>
		/// <param name="fIDoc">Initial <see cref="FnAutoPayment.FIDoc" /> value</param>
		/// <param name="status">Initial <see cref="FnAutoPayment.Status" /> value</param>
		/// <param name="chequeNumber">Initial <see cref="FnAutoPayment.ChequeNumber" /> value</param>
		/// <param name="chequeBankName">Initial <see cref="FnAutoPayment.ChequeBankName" /> value</param>
		/// <param name="chequeDate">Initial <see cref="FnAutoPayment.ChequeDate" /> value</param>
		/// <param name="payeeBankName">Initial <see cref="FnAutoPayment.PayeeBankName" /> value</param>
		/// <param name="payeeBankAccountNumber">Initial <see cref="FnAutoPayment.PayeeBankAccountNumber" /> value</param>
		/// <param name="amount">Initial <see cref="FnAutoPayment.Amount" /> value</param>
		/// <param name="paymentDate">Initial <see cref="FnAutoPayment.PaymentDate" /> value</param>
		/// <param name="currencyDoc">Initial <see cref="FnAutoPayment.CurrencyDoc" /> value</param>
		/// <param name="currencyPay">Initial <see cref="FnAutoPayment.CurrencyPay" /> value</param>
		/// <param name="_companycode">Initial <see cref="FnAutoPayment.companycode" /> value</param>
		/// <param name="_year">Initial <see cref="FnAutoPayment.year" /> value</param>
		/// <param name="active">Initial <see cref="FnAutoPayment.Active" /> value</param>
		/// <param name="creBy">Initial <see cref="FnAutoPayment.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="FnAutoPayment.CreDate" /> value</param>
		/// <param name="updBy">Initial <see cref="FnAutoPayment.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="FnAutoPayment.UpdDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="FnAutoPayment.UpdPgm" /> value</param>
		/// <param name="rowVersion">Initial <see cref="FnAutoPayment.RowVersion" /> value</param>
		/// <param name="document">Initial <see cref="FnAutoPayment.Document" /> value</param>
		public FnAutoPayment(string fIDoc, int status, string chequeNumber, string chequeBankName, DateTime? chequeDate, string payeeBankName, string payeeBankAccountNumber, double amount, DateTime? paymentDate, string currencyDoc, string currencyPay, string _companycode, string _year, bool active, long creBy, DateTime creDate, long updBy, DateTime updDate, string updPgm, Byte[] rowVersion, SCG.eAccounting.DTO.SCGDocument document)
		{
			this.fIDoc = fIDoc;
			this.status = status;
			this.chequeNumber = chequeNumber;
			this.chequeBankName = chequeBankName;
			this.chequeDate = chequeDate;
			this.payeeBankName = payeeBankName;
			this.payeeBankAccountNumber = payeeBankAccountNumber;
			this.amount = amount;
			this.paymentDate = paymentDate;
			this.currencyDoc = currencyDoc;
			this.currencyPay = currencyPay;
			this._companycode = _companycode;
			this._year = _year;
			this.active = active;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updBy = updBy;
			this.updDate = updDate;
			this.updPgm = updPgm;
			this.rowVersion = rowVersion;
			this.document = document;
		}
	
		/// <summary>
		/// Minimal constructor for class FnAutoPayment
		/// </summary>
		/// <param name="amount">Initial <see cref="FnAutoPayment.Amount" /> value</param>
		/// <param name="creBy">Initial <see cref="FnAutoPayment.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="FnAutoPayment.CreDate" /> value</param>
		/// <param name="updBy">Initial <see cref="FnAutoPayment.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="FnAutoPayment.UpdDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="FnAutoPayment.UpdPgm" /> value</param>
		public FnAutoPayment(double amount, long creBy, DateTime creDate, long updBy, DateTime updDate, string updPgm)
		{
			this.amount = amount;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updBy = updBy;
			this.updDate = updDate;
			this.updPgm = updPgm;
		}
		#endregion
	
		#region Properties
		
		/// <summary>
		/// Gets or sets the AutoPaymentID for the current FnAutoPayment
		/// </summary>
		public virtual long AutoPaymentID
		{
			get { return this.autoPaymentID; }
			set { this.autoPaymentID = value; }
		}
		
		/// <summary>
		/// Gets or sets the FIDoc for the current FnAutoPayment
		/// </summary>
		public virtual string FIDoc
		{
			get { return this.fIDoc; }
			set { this.fIDoc = value; }
		}
		
		/// <summary>
		/// Gets or sets the Status for the current FnAutoPayment
		/// </summary>
		public virtual int Status
		{
			get { return this.status; }
			set { this.status = value; }
		}
		
		/// <summary>
		/// Gets or sets the ChequeNumber for the current FnAutoPayment
		/// </summary>
		public virtual string ChequeNumber
		{
			get { return this.chequeNumber; }
			set { this.chequeNumber = value; }
		}
		
		/// <summary>
		/// Gets or sets the ChequeBankName for the current FnAutoPayment
		/// </summary>
		public virtual string ChequeBankName
		{
			get { return this.chequeBankName; }
			set { this.chequeBankName = value; }
		}
		
		/// <summary>
		/// Gets or sets the ChequeDate for the current FnAutoPayment
		/// </summary>
		public virtual DateTime? ChequeDate
		{
			get { return this.chequeDate; }
			set { this.chequeDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the PayeeBankName for the current FnAutoPayment
		/// </summary>
		public virtual string PayeeBankName
		{
			get { return this.payeeBankName; }
			set { this.payeeBankName = value; }
		}
		
		/// <summary>
		/// Gets or sets the PayeeBankAccountNumber for the current FnAutoPayment
		/// </summary>
		public virtual string PayeeBankAccountNumber
		{
			get { return this.payeeBankAccountNumber; }
			set { this.payeeBankAccountNumber = value; }
		}
		
		/// <summary>
		/// Gets or sets the Amount for the current FnAutoPayment
		/// </summary>
		public virtual double Amount
		{
			get { return this.amount; }
			set { this.amount = value; }
		}
		
		/// <summary>
		/// Gets or sets the PaymentDate for the current FnAutoPayment
		/// </summary>
		public virtual DateTime? PaymentDate
		{
			get { return this.paymentDate; }
			set { this.paymentDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the CurrencyDoc for the current FnAutoPayment
		/// </summary>
		public virtual string CurrencyDoc
		{
			get { return this.currencyDoc; }
			set { this.currencyDoc = value; }
		}
		
		/// <summary>
		/// Gets or sets the CurrencyPay for the current FnAutoPayment
		/// </summary>
		public virtual string CurrencyPay
		{
			get { return this.currencyPay; }
			set { this.currencyPay = value; }
		}
		
		/// <summary>
		/// Gets or sets the companycode for the current FnAutoPayment
		/// </summary>
		public virtual string companycode
		{
			get { return this._companycode; }
			set { this._companycode = value; }
		}
		
		/// <summary>
		/// Gets or sets the year for the current FnAutoPayment
		/// </summary>
		public virtual string year
		{
			get { return this._year; }
			set { this._year = value; }
		}
		
		/// <summary>
		/// Gets or sets the Active for the current FnAutoPayment
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreBy for the current FnAutoPayment
		/// </summary>
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreDate for the current FnAutoPayment
		/// </summary>
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdBy for the current FnAutoPayment
		/// </summary>
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdDate for the current FnAutoPayment
		/// </summary>
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdPgm for the current FnAutoPayment
		/// </summary>
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}
		
		/// <summary>
		/// Gets or sets the RowVersion for the current FnAutoPayment
		/// </summary>
		public virtual Byte[] RowVersion
		{
			get { return this.rowVersion; }
			set { this.rowVersion = value; }
		}
		
		/// <summary>
		/// Gets or sets the Document for the current FnAutoPayment
		/// </summary>
		public virtual SCG.eAccounting.DTO.SCGDocument Document
		{
			get { return this.document; }
			set { this.document = value; }
		}
		
		#endregion
	}
}

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
	/// POJO for Bapiackev9. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class Bapiackev9
	{
		#region Fields
		
		private long id;
		private string docNo;
		private string itemnoAcc;
		private string fieldname;
		private string currType;
		private string currency;
		private string currencyIso;
		private decimal amtValcom;
		private string baseUom;
		private string baseUomIso;
		private decimal quaValcom;
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
		/// Initializes a new instance of the Bapiackev9 class
		/// </summary>
		public Bapiackev9()
		{
		}

		public Bapiackev9(long id)
		{
			this.id = id;
		}
	
		/// <summary>
		/// Initializes a new instance of the Bapiackev9 class
		/// </summary>
		/// <param name="docNo">Initial <see cref="Bapiackev9.DocNo" /> value</param>
		/// <param name="itemnoAcc">Initial <see cref="Bapiackev9.ItemnoAcc" /> value</param>
		/// <param name="fieldname">Initial <see cref="Bapiackev9.Fieldname" /> value</param>
		/// <param name="currType">Initial <see cref="Bapiackev9.CurrType" /> value</param>
		/// <param name="currency">Initial <see cref="Bapiackev9.Currency" /> value</param>
		/// <param name="currencyIso">Initial <see cref="Bapiackev9.CurrencyIso" /> value</param>
		/// <param name="amtValcom">Initial <see cref="Bapiackev9.AmtValcom" /> value</param>
		/// <param name="baseUom">Initial <see cref="Bapiackev9.BaseUom" /> value</param>
		/// <param name="baseUomIso">Initial <see cref="Bapiackev9.BaseUomIso" /> value</param>
		/// <param name="quaValcom">Initial <see cref="Bapiackev9.QuaValcom" /> value</param>
		/// <param name="active">Initial <see cref="Bapiackev9.Active" /> value</param>
		/// <param name="updBy">Initial <see cref="Bapiackev9.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="Bapiackev9.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="Bapiackev9.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="Bapiackev9.CreDate" /> value</param>
		/// <param name="rowVersion">Initial <see cref="Bapiackev9.RowVersion" /> value</param>
		/// <param name="updPgm">Initial <see cref="Bapiackev9.UpdPgm" /> value</param>
		public Bapiackev9(string docNo, string itemnoAcc, string fieldname, string currType, string currency, string currencyIso, decimal amtValcom, string baseUom, string baseUomIso, decimal quaValcom, bool active, long updBy, DateTime updDate, long creBy, DateTime creDate, byte[] rowVersion, string updPgm)
		{
			this.docNo = docNo;
			this.itemnoAcc = itemnoAcc;
			this.fieldname = fieldname;
			this.currType = currType;
			this.currency = currency;
			this.currencyIso = currencyIso;
			this.amtValcom = amtValcom;
			this.baseUom = baseUom;
			this.baseUomIso = baseUomIso;
			this.quaValcom = quaValcom;
			this.active = active;
			this.updBy = updBy;
			this.updDate = updDate;
			this.creBy = creBy;
			this.creDate = creDate;
			this.rowVersion = rowVersion;
			this.updPgm = updPgm;
		}
	
		/// <summary>
		/// Minimal constructor for class Bapiackev9
		/// </summary>
		/// <param name="docNo">Initial <see cref="Bapiackev9.DocNo" /> value</param>
		/// <param name="active">Initial <see cref="Bapiackev9.Active" /> value</param>
		/// <param name="updBy">Initial <see cref="Bapiackev9.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="Bapiackev9.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="Bapiackev9.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="Bapiackev9.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="Bapiackev9.UpdPgm" /> value</param>
		public Bapiackev9(string docNo, bool active, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm)
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
		/// Gets or sets the Id for the current Bapiackev9
		/// </summary>
		public virtual long Id
		{
			get { return this.id; }
			set { this.id = value; }
		}
		
		/// <summary>
		/// Gets or sets the DocNo for the current Bapiackev9
		/// </summary>
		public virtual string DocNo
		{
			get { return this.docNo; }
			set { this.docNo = value; }
		}
		
		/// <summary>
		/// Gets or sets the ItemnoAcc for the current Bapiackev9
		/// </summary>
		public virtual string ItemnoAcc
		{
			get { return this.itemnoAcc; }
			set { this.itemnoAcc = value; }
		}
		
		/// <summary>
		/// Gets or sets the Fieldname for the current Bapiackev9
		/// </summary>
		public virtual string Fieldname
		{
			get { return this.fieldname; }
			set { this.fieldname = value; }
		}
		
		/// <summary>
		/// Gets or sets the CurrType for the current Bapiackev9
		/// </summary>
		public virtual string CurrType
		{
			get { return this.currType; }
			set { this.currType = value; }
		}
		
		/// <summary>
		/// Gets or sets the Currency for the current Bapiackev9
		/// </summary>
		public virtual string Currency
		{
			get { return this.currency; }
			set { this.currency = value; }
		}
		
		/// <summary>
		/// Gets or sets the CurrencyIso for the current Bapiackev9
		/// </summary>
		public virtual string CurrencyIso
		{
			get { return this.currencyIso; }
			set { this.currencyIso = value; }
		}
		
		/// <summary>
		/// Gets or sets the AmtValcom for the current Bapiackev9
		/// </summary>
		public virtual decimal AmtValcom
		{
			get { return this.amtValcom; }
			set { this.amtValcom = value; }
		}
		
		/// <summary>
		/// Gets or sets the BaseUom for the current Bapiackev9
		/// </summary>
		public virtual string BaseUom
		{
			get { return this.baseUom; }
			set { this.baseUom = value; }
		}
		
		/// <summary>
		/// Gets or sets the BaseUomIso for the current Bapiackev9
		/// </summary>
		public virtual string BaseUomIso
		{
			get { return this.baseUomIso; }
			set { this.baseUomIso = value; }
		}
		
		/// <summary>
		/// Gets or sets the QuaValcom for the current Bapiackev9
		/// </summary>
		public virtual decimal QuaValcom
		{
			get { return this.quaValcom; }
			set { this.quaValcom = value; }
		}
		
		/// <summary>
		/// Gets or sets the Active for the current Bapiackev9
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdBy for the current Bapiackev9
		/// </summary>
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdDate for the current Bapiackev9
		/// </summary>
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreBy for the current Bapiackev9
		/// </summary>
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreDate for the current Bapiackev9
		/// </summary>
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the RowVersion for the current Bapiackev9
		/// </summary>
		public virtual byte[] RowVersion
		{
			get { return this.rowVersion; }
			set { this.rowVersion = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdPgm for the current Bapiackev9
		/// </summary>
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}
		
		#endregion
	}
}
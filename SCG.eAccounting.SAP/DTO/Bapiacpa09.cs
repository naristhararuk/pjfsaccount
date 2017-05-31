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
	/// POJO for Bapiacpa09. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class Bapiacpa09
	{
		#region Fields
		
		private long id;
        private long docId;
		private string name;
		private string name2;
		private string name3;
		private string name4;
		private string postlCode;
		private string city;
		private string country;
		private string countryIso;
		private string street;
		private string poBox;
		private string pobxPcd;
		private string pobkCurac;
		private string bankAcct;
		private string bankNo;
		private string bankCtry;
		private string bankCtryIso;
		private string taxNo1;
		private string taxNo2;
        private string taxNo3;
        private string taxNo4;
		private string tax;
		private string equalTax;
		private string region;
		private string ctrlKey;
		private string instrKey;
		private string dmeInd;
		private string languIso;
		private string anred;
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
		/// Initializes a new instance of the Bapiacpa09 class
		/// </summary>
		public Bapiacpa09()
		{
		}

		public Bapiacpa09(long id)
		{
			this.id = id;
		}
	
		/// <summary>
		/// Initializes a new instance of the Bapiacpa09 class
		/// </summary>
		/// <param name="docNo">Initial <see cref="Bapiacpa09.DocNo" /> value</param>
		/// <param name="name">Initial <see cref="Bapiacpa09.Name" /> value</param>
		/// <param name="name2">Initial <see cref="Bapiacpa09.Name2" /> value</param>
		/// <param name="name3">Initial <see cref="Bapiacpa09.Name3" /> value</param>
		/// <param name="name4">Initial <see cref="Bapiacpa09.Name4" /> value</param>
		/// <param name="postlCode">Initial <see cref="Bapiacpa09.PostlCode" /> value</param>
		/// <param name="city">Initial <see cref="Bapiacpa09.City" /> value</param>
		/// <param name="country">Initial <see cref="Bapiacpa09.Country" /> value</param>
		/// <param name="countryIso">Initial <see cref="Bapiacpa09.CountryIso" /> value</param>
		/// <param name="street">Initial <see cref="Bapiacpa09.Street" /> value</param>
		/// <param name="poBox">Initial <see cref="Bapiacpa09.PoBox" /> value</param>
		/// <param name="pobxPcd">Initial <see cref="Bapiacpa09.PobxPcd" /> value</param>
		/// <param name="pobkCurac">Initial <see cref="Bapiacpa09.PobkCurac" /> value</param>
		/// <param name="bankAcct">Initial <see cref="Bapiacpa09.BankAcct" /> value</param>
		/// <param name="bankNo">Initial <see cref="Bapiacpa09.BankNo" /> value</param>
		/// <param name="bankCtry">Initial <see cref="Bapiacpa09.BankCtry" /> value</param>
		/// <param name="bankCtryIso">Initial <see cref="Bapiacpa09.BankCtryIso" /> value</param>
		/// <param name="taxNo1">Initial <see cref="Bapiacpa09.TaxNo1" /> value</param>
		/// <param name="taxNo2">Initial <see cref="Bapiacpa09.TaxNo2" /> value</param>
		/// <param name="tax">Initial <see cref="Bapiacpa09.Tax" /> value</param>
		/// <param name="equalTax">Initial <see cref="Bapiacpa09.EqualTax" /> value</param>
		/// <param name="region">Initial <see cref="Bapiacpa09.Region" /> value</param>
		/// <param name="ctrlKey">Initial <see cref="Bapiacpa09.CtrlKey" /> value</param>
		/// <param name="instrKey">Initial <see cref="Bapiacpa09.InstrKey" /> value</param>
		/// <param name="dmeInd">Initial <see cref="Bapiacpa09.DmeInd" /> value</param>
		/// <param name="languIso">Initial <see cref="Bapiacpa09.LanguIso" /> value</param>
		/// <param name="anred">Initial <see cref="Bapiacpa09.Anred" /> value</param>
		/// <param name="active">Initial <see cref="Bapiacpa09.Active" /> value</param>
		/// <param name="updBy">Initial <see cref="Bapiacpa09.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="Bapiacpa09.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="Bapiacpa09.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="Bapiacpa09.CreDate" /> value</param>
		/// <param name="rowVersion">Initial <see cref="Bapiacpa09.RowVersion" /> value</param>
		/// <param name="updPgm">Initial <see cref="Bapiacpa09.UpdPgm" /> value</param>
		public Bapiacpa09(long docId, string name, string name2, string name3, string name4, string postlCode, string city, string country, string countryIso, string street, string poBox, string pobxPcd, string pobkCurac, string bankAcct, string bankNo, string bankCtry, string bankCtryIso, string taxNo1, string taxNo2, string tax, string equalTax, string region, string ctrlKey, string instrKey, string dmeInd, string languIso, string anred, bool active, long updBy, DateTime updDate, long creBy, DateTime creDate, byte[] rowVersion, string updPgm)
		{
			this.docId = docId;
			this.name = name;
			this.name2 = name2;
			this.name3 = name3;
			this.name4 = name4;
			this.postlCode = postlCode;
			this.city = city;
			this.country = country;
			this.countryIso = countryIso;
			this.street = street;
			this.poBox = poBox;
			this.pobxPcd = pobxPcd;
			this.pobkCurac = pobkCurac;
			this.bankAcct = bankAcct;
			this.bankNo = bankNo;
			this.bankCtry = bankCtry;
			this.bankCtryIso = bankCtryIso;
			this.taxNo1 = taxNo1;
			this.taxNo2 = taxNo2;
			this.tax = tax;
			this.equalTax = equalTax;
			this.region = region;
			this.ctrlKey = ctrlKey;
			this.instrKey = instrKey;
			this.dmeInd = dmeInd;
			this.languIso = languIso;
			this.anred = anred;
			this.active = active;
			this.updBy = updBy;
			this.updDate = updDate;
			this.creBy = creBy;
			this.creDate = creDate;
			this.rowVersion = rowVersion;
			this.updPgm = updPgm;
		}
	
		/// <summary>
		/// Minimal constructor for class Bapiacpa09
		/// </summary>
		/// <param name="docNo">Initial <see cref="Bapiacpa09.DocNo" /> value</param>
		/// <param name="active">Initial <see cref="Bapiacpa09.Active" /> value</param>
		/// <param name="updBy">Initial <see cref="Bapiacpa09.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="Bapiacpa09.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="Bapiacpa09.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="Bapiacpa09.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="Bapiacpa09.UpdPgm" /> value</param>
		public Bapiacpa09(long docId, bool active, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm)
		{
			this.docId = docId;
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
		/// Gets or sets the Id for the current Bapiacpa09
		/// </summary>
		public virtual long Id
		{
			get { return this.id; }
			set { this.id = value; }
		}
		
		/// <summary>
		/// Gets or sets the DocNo for the current Bapiacpa09
		/// </summary>
		public virtual long DocId
		{
            get { return this.docId; }
            set { this.docId = value; }
		}
		
		/// <summary>
		/// Gets or sets the Name for the current Bapiacpa09
		/// </summary>
		public virtual string Name
		{
			get { return this.name; }
			set { this.name = value; }
		}
		
		/// <summary>
		/// Gets or sets the Name2 for the current Bapiacpa09
		/// </summary>
		public virtual string Name2
		{
			get { return this.name2; }
			set { this.name2 = value; }
		}
		
		/// <summary>
		/// Gets or sets the Name3 for the current Bapiacpa09
		/// </summary>
		public virtual string Name3
		{
			get { return this.name3; }
			set { this.name3 = value; }
		}
		
		/// <summary>
		/// Gets or sets the Name4 for the current Bapiacpa09
		/// </summary>
		public virtual string Name4
		{
			get { return this.name4; }
			set { this.name4 = value; }
		}
		
		/// <summary>
		/// Gets or sets the PostlCode for the current Bapiacpa09
		/// </summary>
		public virtual string PostlCode
		{
			get { return this.postlCode; }
			set { this.postlCode = value; }
		}
		
		/// <summary>
		/// Gets or sets the City for the current Bapiacpa09
		/// </summary>
		public virtual string City
		{
			get { return this.city; }
			set { this.city = value; }
		}
		
		/// <summary>
		/// Gets or sets the Country for the current Bapiacpa09
		/// </summary>
		public virtual string Country
		{
			get { return this.country; }
			set { this.country = value; }
		}
		
		/// <summary>
		/// Gets or sets the CountryIso for the current Bapiacpa09
		/// </summary>
		public virtual string CountryIso
		{
			get { return this.countryIso; }
			set { this.countryIso = value; }
		}
		
		/// <summary>
		/// Gets or sets the Street for the current Bapiacpa09
		/// </summary>
		public virtual string Street
		{
			get { return this.street; }
			set { this.street = value; }
		}
		
		/// <summary>
		/// Gets or sets the PoBox for the current Bapiacpa09
		/// </summary>
		public virtual string PoBox
		{
			get { return this.poBox; }
			set { this.poBox = value; }
		}
		
		/// <summary>
		/// Gets or sets the PobxPcd for the current Bapiacpa09
		/// </summary>
		public virtual string PobxPcd
		{
			get { return this.pobxPcd; }
			set { this.pobxPcd = value; }
		}
		
		/// <summary>
		/// Gets or sets the PobkCurac for the current Bapiacpa09
		/// </summary>
		public virtual string PobkCurac
		{
			get { return this.pobkCurac; }
			set { this.pobkCurac = value; }
		}
		
		/// <summary>
		/// Gets or sets the BankAcct for the current Bapiacpa09
		/// </summary>
		public virtual string BankAcct
		{
			get { return this.bankAcct; }
			set { this.bankAcct = value; }
		}
		
		/// <summary>
		/// Gets or sets the BankNo for the current Bapiacpa09
		/// </summary>
		public virtual string BankNo
		{
			get { return this.bankNo; }
			set { this.bankNo = value; }
		}
		
		/// <summary>
		/// Gets or sets the BankCtry for the current Bapiacpa09
		/// </summary>
		public virtual string BankCtry
		{
			get { return this.bankCtry; }
			set { this.bankCtry = value; }
		}
		
		/// <summary>
		/// Gets or sets the BankCtryIso for the current Bapiacpa09
		/// </summary>
		public virtual string BankCtryIso
		{
			get { return this.bankCtryIso; }
			set { this.bankCtryIso = value; }
		}
		
		/// <summary>
		/// Gets or sets the TaxNo1 for the current Bapiacpa09
		/// </summary>
		public virtual string TaxNo1
		{
			get { return this.taxNo1; }
			set { this.taxNo1 = value; }
		}
		
		/// <summary>
		/// Gets or sets the TaxNo2 for the current Bapiacpa09
		/// </summary>
		public virtual string TaxNo2
		{
			get { return this.taxNo2; }
			set { this.taxNo2 = value; }
		}

        /// <summary>
        /// Gets or sets the TaxNo3 for the current Bapiacpa09
        /// </summary>
        public virtual string TaxNo3
        {
            get { return this.taxNo3; }
            set { this.taxNo3 = value; }
        }

        /// <summary>
        /// Gets or sets the TaxNo4 for the current Bapiacpa09
        /// </summary>
        public virtual string TaxNo4
        {
            get { return this.taxNo4; }
            set { this.taxNo4 = value; }
        }

		/// <summary>
		/// Gets or sets the Tax for the current Bapiacpa09
		/// </summary>
		public virtual string Tax
		{
			get { return this.tax; }
			set { this.tax = value; }
		}
		
		/// <summary>
		/// Gets or sets the EqualTax for the current Bapiacpa09
		/// </summary>
		public virtual string EqualTax
		{
			get { return this.equalTax; }
			set { this.equalTax = value; }
		}
		
		/// <summary>
		/// Gets or sets the Region for the current Bapiacpa09
		/// </summary>
		public virtual string Region
		{
			get { return this.region; }
			set { this.region = value; }
		}
		
		/// <summary>
		/// Gets or sets the CtrlKey for the current Bapiacpa09
		/// </summary>
		public virtual string CtrlKey
		{
			get { return this.ctrlKey; }
			set { this.ctrlKey = value; }
		}
		
		/// <summary>
		/// Gets or sets the InstrKey for the current Bapiacpa09
		/// </summary>
		public virtual string InstrKey
		{
			get { return this.instrKey; }
			set { this.instrKey = value; }
		}
		
		/// <summary>
		/// Gets or sets the DmeInd for the current Bapiacpa09
		/// </summary>
		public virtual string DmeInd
		{
			get { return this.dmeInd; }
			set { this.dmeInd = value; }
		}
		
		/// <summary>
		/// Gets or sets the LanguIso for the current Bapiacpa09
		/// </summary>
		public virtual string LanguIso
		{
			get { return this.languIso; }
			set { this.languIso = value; }
		}
		
		/// <summary>
		/// Gets or sets the Anred for the current Bapiacpa09
		/// </summary>
		public virtual string Anred
		{
			get { return this.anred; }
			set { this.anred = value; }
		}
		
		/// <summary>
		/// Gets or sets the Active for the current Bapiacpa09
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdBy for the current Bapiacpa09
		/// </summary>
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdDate for the current Bapiacpa09
		/// </summary>
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreBy for the current Bapiacpa09
		/// </summary>
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreDate for the current Bapiacpa09
		/// </summary>
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the RowVersion for the current Bapiacpa09
		/// </summary>
		public virtual byte[] RowVersion
		{
			get { return this.rowVersion; }
			set { this.rowVersion = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdPgm for the current Bapiacpa09
		/// </summary>
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}
		
		#endregion

        private string docSeq;
        public virtual string DocSeq
        {
            get { return this.docSeq; }
            set { this.docSeq = value; }
        }
        private string docKind;
        public virtual string DocKind
        {
            get { return this.docKind; }
            set { this.docKind = value; }
        }
	}
}

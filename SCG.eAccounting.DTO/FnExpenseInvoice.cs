//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by NHibernate.
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

using System;
using SCG.DB.DTO;
using SS.Standard.Data.NHibernate.DTO;
using System.Data;

namespace SCG.eAccounting.DTO
{
    /// <summary>
    /// POJO for FnExpenseInvoice. This class is autogenerated
    /// </summary>
    [Serializable]
    public partial class FnExpenseInvoice : INHibernateAdapterDTO<long>
    {
        #region Fields
        private long invoiceID;
        private string invoiceDocumentType;
        private string invoiceNo;
        private DateTime? invoiceDate;
        private double totalAmount;
        private double vatAmount;
        private double wHTAmount;
        private double netAmount;
        private string description;
        private bool? _isVAT;
        private bool? _isWHT;
        private long? taxID;
        private double nonDeductAmount;
        private double totalBaseAmount;
        private double? wHTRate1;
        private long? wHTTypeID1;
        private double? baseAmount1;
        private double? wHTAmount1;
        private double? wHTRate2;
        private long? wHTTypeID2;
        private double? baseAmount2;
        private double? wHTAmount2;
        private bool active;
        private long creBy;
        private DateTime creDate;
        private long updBy;
        private DateTime updDate;
        private string updPgm;
        private Byte[] rowVersion;
        private SCG.eAccounting.DTO.FnExpenseDocument expense;
        private long? vendorID;
        private long? wHTID1;
        private long? wHTID2;
        private string vendorCode;
        private string vendorBranch;
        private string vendorName;
        private string street;
        private string city;
        private string country;
        private string postalCode;
        private string vendorTaxCode;
        private string branchCode;
        private double? totalAmountLocalCurrency;
        private double? totalBaseAmountLocalCurrency;
        private double? netAmountLocalCurrency;
        private double? exchangeRateMainToTHBCurrency;
        private double? exchangeRateForLocalCurrency;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the FnExpenseInvoice class
        /// </summary>
        public FnExpenseInvoice()
        {
        }

        public FnExpenseInvoice(long invoiceID)
        {
            this.invoiceID = invoiceID;
        }

        /// <summary>
        /// Initializes a new instance of the FnExpenseInvoice class
        /// </summary>
        /// <param name="invoiceDocumentType">Initial <see cref="FnExpenseInvoice.InvoiceDocumentType" /> value</param>
        /// <param name="invoiceNo">Initial <see cref="FnExpenseInvoice.InvoiceNo" /> value</param>
        /// <param name="invoiceDate">Initial <see cref="FnExpenseInvoice.InvoiceDate" /> value</param>
        /// <param name="totalAmount">Initial <see cref="FnExpenseInvoice.TotalAmount" /> value</param>
        /// <param name="vatAmount">Initial <see cref="FnExpenseInvoice.VatAmount" /> value</param>
        /// <param name="wHTAmount">Initial <see cref="FnExpenseInvoice.WHTAmount" /> value</param>
        /// <param name="netAmount">Initial <see cref="FnExpenseInvoice.NetAmount" /> value</param>
        /// <param name="description">Initial <see cref="FnExpenseInvoice.Description" /> value</param>
        /// <param name="_isVAT">Initial <see cref="FnExpenseInvoice.isVAT" /> value</param>
        /// <param name="_isWHT">Initial <see cref="FnExpenseInvoice.isWHT" /> value</param>
        /// <param name="taxID">Initial <see cref="FnExpenseInvoice.TaxID" /> value</param>
        /// <param name="nonDeductAmount">Initial <see cref="FnExpenseInvoice.NonDeductAmount" /> value</param>
        /// <param name="totalBaseAmount">Initial <see cref="FnExpenseInvoice.TotalBaseAmount" /> value</param>
        /// <param name="wHTRate1">Initial <see cref="FnExpenseInvoice.WHTRate1" /> value</param>
        /// <param name="wHTTypeID1">Initial <see cref="FnExpenseInvoice.WHTTypeID1" /> value</param>
        /// <param name="baseAmount1">Initial <see cref="FnExpenseInvoice.BaseAmount1" /> value</param>
        /// <param name="wHTAmount1">Initial <see cref="FnExpenseInvoice.WHTAmount1" /> value</param>
        /// <param name="wHTRate2">Initial <see cref="FnExpenseInvoice.WHTRate2" /> value</param>
        /// <param name="wHTTypeID2">Initial <see cref="FnExpenseInvoice.WHTTypeID2" /> value</param>
        /// <param name="baseAmount2">Initial <see cref="FnExpenseInvoice.BaseAmount2" /> value</param>
        /// <param name="wHTAmount2">Initial <see cref="FnExpenseInvoice.WHTAmount2" /> value</param>
        /// <param name="active">Initial <see cref="FnExpenseInvoice.Active" /> value</param>
        /// <param name="creBy">Initial <see cref="FnExpenseInvoice.CreBy" /> value</param>
        /// <param name="creDate">Initial <see cref="FnExpenseInvoice.CreDate" /> value</param>
        /// <param name="updBy">Initial <see cref="FnExpenseInvoice.UpdBy" /> value</param>
        /// <param name="updDate">Initial <see cref="FnExpenseInvoice.UpdDate" /> value</param>
        /// <param name="updPgm">Initial <see cref="FnExpenseInvoice.UpdPgm" /> value</param>
        /// <param name="rowVersion">Initial <see cref="FnExpenseInvoice.RowVersion" /> value</param>
        /// <param name="expense">Initial <see cref="FnExpenseInvoice.Expense" /> value</param>
        /// <param name="vendor">Initial <see cref="FnExpenseInvoice.Vendor" /> value</param>
        public FnExpenseInvoice(string invoiceDocumentType, string invoiceNo, DateTime? invoiceDate, double totalAmount, double vatAmount, double wHTAmount, double netAmount, string description, bool _isVAT, bool _isWHT, long taxID, double nonDeductAmount, double totalBaseAmount, double? wHTRate1, long? wHTTypeID1, double? baseAmount1, double? wHTAmount1, double? wHTRate2, long? wHTTypeID2, double? baseAmount2, double? wHTAmount2, bool active, long creBy, DateTime creDate, long updBy, DateTime updDate, string updPgm, Byte[] rowVersion, SCG.eAccounting.DTO.FnExpenseDocument expense, long? vendorID, long? wHTID1, long? wHTID2)
        {
            this.invoiceDocumentType = invoiceDocumentType;
            this.invoiceNo = invoiceNo;
            this.invoiceDate = invoiceDate;
            this.totalAmount = totalAmount;
            this.vatAmount = vatAmount;
            this.wHTAmount = wHTAmount;
            this.netAmount = netAmount;
            this.description = description;
            this._isVAT = _isVAT;
            this._isWHT = _isWHT;
            this.taxID = taxID;
            this.nonDeductAmount = nonDeductAmount;
            this.totalBaseAmount = totalBaseAmount;
            this.wHTRate1 = wHTRate1;
            this.wHTTypeID1 = wHTTypeID1;
            this.baseAmount1 = baseAmount1;
            this.wHTAmount1 = wHTAmount1;
            this.wHTRate2 = wHTRate2;
            this.wHTTypeID2 = wHTTypeID2;
            this.baseAmount2 = baseAmount2;
            this.wHTAmount2 = wHTAmount2;
            this.active = active;
            this.creBy = creBy;
            this.creDate = creDate;
            this.updBy = updBy;
            this.updDate = updDate;
            this.updPgm = updPgm;
            this.rowVersion = rowVersion;
            this.expense = expense;
            this.vendorID = vendorID;
            this.wHTID1 = wHTID1;
            this.wHTID2 = wHTID2;
        }

        /// <summary>
        /// Minimal constructor for class FnExpenseInvoice
        /// </summary>
        /// <param name="active">Initial <see cref="FnExpenseInvoice.Active" /> value</param>
        /// <param name="creBy">Initial <see cref="FnExpenseInvoice.CreBy" /> value</param>
        /// <param name="creDate">Initial <see cref="FnExpenseInvoice.CreDate" /> value</param>
        /// <param name="updBy">Initial <see cref="FnExpenseInvoice.UpdBy" /> value</param>
        /// <param name="updDate">Initial <see cref="FnExpenseInvoice.UpdDate" /> value</param>
        /// <param name="updPgm">Initial <see cref="FnExpenseInvoice.UpdPgm" /> value</param>
        public FnExpenseInvoice(bool active, long creBy, DateTime creDate, long updBy, DateTime updDate, string updPgm)
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
        /// Gets or sets the InvoiceID for the current FnExpenseInvoice
        /// </summary>
        public virtual long InvoiceID
        {
            get { return this.invoiceID; }
            set { this.invoiceID = value; }
        }

        /// <summary>
        /// Gets or sets the InvoiceDocumentType for the current FnExpenseInvoice
        /// </summary>
        public virtual string InvoiceDocumentType
        {
            get { return this.invoiceDocumentType; }
            set { this.invoiceDocumentType = value; }
        }

        /// <summary>
        /// Gets or sets the InvoiceNo for the current FnExpenseInvoice
        /// </summary>
        public virtual string InvoiceNo
        {
            get { return this.invoiceNo; }
            set { this.invoiceNo = value; }
        }

        /// <summary>
        /// Gets or sets the InvoiceDate for the current FnExpenseInvoice
        /// </summary>
        public virtual DateTime? InvoiceDate
        {
            get { return this.invoiceDate; }
            set { this.invoiceDate = value; }
        }

        /// <summary>
        /// Gets or sets the TotalAmount for the current FnExpenseInvoice
        /// </summary>
        public virtual double TotalAmount
        {
            get { return this.totalAmount; }
            set { this.totalAmount = value; }
        }

        /// <summary>
        /// Gets or sets the VatAmount for the current FnExpenseInvoice
        /// </summary>
        public virtual double VatAmount
        {
            get { return this.vatAmount; }
            set { this.vatAmount = value; }
        }

        /// <summary>
        /// Gets or sets the WHTAmount for the current FnExpenseInvoice
        /// </summary>
        public virtual double WHTAmount
        {
            get { return this.wHTAmount; }
            set { this.wHTAmount = value; }
        }

        /// <summary>
        /// Gets or sets the NetAmount for the current FnExpenseInvoice
        /// </summary>
        public virtual double NetAmount
        {
            get { return this.netAmount; }
            set { this.netAmount = value; }
        }

        /// <summary>
        /// Gets or sets the Description for the current FnExpenseInvoice
        /// </summary>
        public virtual string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        public virtual string VendorCode
        {
            get { return this.vendorCode; }
            set { this.vendorCode = value; }
        }
        public virtual string VendorBranch
        {
            get { return this.vendorBranch; }
            set { this.vendorBranch = value; }
        }
        public virtual string VendorName
        {
            get { return this.vendorName; }
            set { this.vendorName = value; }
        }

        public virtual string Street
        {
            get { return this.street; }
            set { this.street = value; }
        }

        public virtual string City
        {
            get { return this.city; }
            set { this.city = value; }
        }

        public virtual string Country
        {
            get { return this.country; }
            set { this.country = value; }
        }

        public virtual string PostalCode
        {
            get { return this.postalCode; }
            set { this.postalCode = value; }
        }

        public virtual string VendorTaxCode
        {
            get { return this.vendorTaxCode; }
            set { this.vendorTaxCode = value; }
        }

        public virtual string BranchCode
        {
            get { return this.branchCode; }
            set { this.branchCode = value; }
        }
        /// <summary>
        /// Gets or sets the isVAT for the current FnExpenseInvoice
        /// </summary>
        public virtual bool? IsVAT
        {
            get { return this._isVAT; }
            set { this._isVAT = value; }
        }

        /// <summary>
        /// Gets or sets the isWHT for the current FnExpenseInvoice
        /// </summary>
        public virtual bool? IsWHT
        {
            get { return this._isWHT; }
            set { this._isWHT = value; }
        }

        /// <summary>
        /// Gets or sets the TaxID for the current FnExpenseInvoice
        /// </summary>
        public virtual long? TaxID
        {
            get { return this.taxID; }
            set { this.taxID = value; }
        }

        /// <summary>
        /// Gets or sets the NonDeductAmount for the current FnExpenseInvoice
        /// </summary>
        public virtual double NonDeductAmount
        {
            get { return this.nonDeductAmount; }
            set { this.nonDeductAmount = value; }
        }

        /// <summary>
        /// Gets or sets the TotalBaseAmount for the current FnExpenseInvoice
        /// </summary>
        public virtual double TotalBaseAmount
        {
            get { return this.totalBaseAmount; }
            set { this.totalBaseAmount = value; }
        }

        /// <summary>
        /// Gets or sets the WHTRate1 for the current FnExpenseInvoice
        /// </summary>
        public virtual double? WHTRate1
        {
            get { return this.wHTRate1; }
            set { this.wHTRate1 = value; }
        }

        /// <summary>
        /// Gets or sets the WHTTypeID1 for the current FnExpenseInvoice
        /// </summary>
        public virtual long? WHTTypeID1
        {
            get { return this.wHTTypeID1; }
            set { this.wHTTypeID1 = value; }
        }

        /// <summary>
        /// Gets or sets the BaseAmount1 for the current FnExpenseInvoice
        /// </summary>
        public virtual double? BaseAmount1
        {
            get { return this.baseAmount1; }
            set { this.baseAmount1 = value; }
        }

        /// <summary>
        /// Gets or sets the WHTAmount1 for the current FnExpenseInvoice
        /// </summary>
        public virtual double? WHTAmount1
        {
            get { return this.wHTAmount1; }
            set { this.wHTAmount1 = value; }
        }

        /// <summary>
        /// Gets or sets the WHTRate2 for the current FnExpenseInvoice
        /// </summary>
        public virtual double? WHTRate2
        {
            get { return this.wHTRate2; }
            set { this.wHTRate2 = value; }
        }

        /// <summary>
        /// Gets or sets the WHTTypeID2 for the current FnExpenseInvoice
        /// </summary>
        public virtual long? WHTTypeID2
        {
            get { return this.wHTTypeID2; }
            set { this.wHTTypeID2 = value; }
        }

        /// <summary>
        /// Gets or sets the BaseAmount2 for the current FnExpenseInvoice
        /// </summary>
        public virtual double? BaseAmount2
        {
            get { return this.baseAmount2; }
            set { this.baseAmount2 = value; }
        }

        /// <summary>
        /// Gets or sets the WHTAmount2 for the current FnExpenseInvoice
        /// </summary>
        public virtual double? WHTAmount2
        {
            get { return this.wHTAmount2; }
            set { this.wHTAmount2 = value; }
        }

        /// <summary>
        /// Gets or sets the Active for the current FnExpenseInvoice
        /// </summary>
        public virtual bool Active
        {
            get { return this.active; }
            set { this.active = value; }
        }

        /// <summary>
        /// Gets or sets the CreBy for the current FnExpenseInvoice
        /// </summary>
        public virtual long CreBy
        {
            get { return this.creBy; }
            set { this.creBy = value; }
        }

        /// <summary>
        /// Gets or sets the CreDate for the current FnExpenseInvoice
        /// </summary>
        public virtual DateTime CreDate
        {
            get { return this.creDate; }
            set { this.creDate = value; }
        }

        /// <summary>
        /// Gets or sets the UpdBy for the current FnExpenseInvoice
        /// </summary>
        public virtual long UpdBy
        {
            get { return this.updBy; }
            set { this.updBy = value; }
        }

        /// <summary>
        /// Gets or sets the UpdDate for the current FnExpenseInvoice
        /// </summary>
        public virtual DateTime UpdDate
        {
            get { return this.updDate; }
            set { this.updDate = value; }
        }

        /// <summary>
        /// Gets or sets the UpdPgm for the current FnExpenseInvoice
        /// </summary>
        public virtual string UpdPgm
        {
            get { return this.updPgm; }
            set { this.updPgm = value; }
        }

        /// <summary>
        /// Gets or sets the RowVersion for the current FnExpenseInvoice
        /// </summary>
        public virtual Byte[] RowVersion
        {
            get { return this.rowVersion; }
            set { this.rowVersion = value; }
        }

        /// <summary>
        /// Gets or sets the Expense for the current FnExpenseInvoice
        /// </summary>
        public virtual SCG.eAccounting.DTO.FnExpenseDocument Expense
        {
            get { return this.expense; }
            set { this.expense = value; }
        }

        /// <summary>
        /// Gets or sets the Vendor for the current FnExpenseInvoice
        /// </summary>
        public virtual long? VendorID
        {
            get { return this.vendorID; }
            set { this.vendorID = value; }
        }
        /// <summary>
        /// Gets or sets the WHTID1 for the current FnExpenseInvoice
        /// </summary>
        public virtual long? WHTID1
        {
            get { return this.wHTID1; }
            set { this.wHTID1 = value; }
        }

        /// <summary>
        /// Gets or sets the WHTID2 for the current FnExpenseInvoice
        /// </summary>
        public virtual long? WHTID2
        {
            get { return this.wHTID2; }
            set { this.wHTID2 = value; }
        }

        public virtual double? TotalAmountLocalCurrency
        {
            get { return this.totalAmountLocalCurrency; }
            set { this.totalAmountLocalCurrency = value; }
        }

        public virtual double? TotalBaseAmountLocalCurrency 
        {
            get { return this.totalBaseAmountLocalCurrency; }
            set { this.totalBaseAmountLocalCurrency = value; }
        }

        public virtual double? NetAmountLocalCurrency 
        {
            get { return this.netAmountLocalCurrency; }
            set { this.netAmountLocalCurrency = value; }
        }

        public virtual double? ExchangeRateForLocalCurrency
        {
            get { return this.exchangeRateForLocalCurrency; }
            set { this.exchangeRateForLocalCurrency = value; }
        }

        public virtual double? ExchangeRateMainToTHBCurrency
        {
            get { return this.exchangeRateMainToTHBCurrency; }
            set { this.exchangeRateMainToTHBCurrency = value; }
        }
        #endregion

        #region INHibernateAdapterDTO<long> Members
        public void LoadFromDataRow(DataRow dr)
        {
            if (!string.IsNullOrEmpty(dr["InvoiceID"].ToString()))
                this.InvoiceID = Convert.ToInt64(dr["InvoiceID"]);

            if (!string.IsNullOrEmpty(dr["ExpenseID"].ToString()))
                this.Expense = new FnExpenseDocument(Convert.ToInt64(dr["ExpenseID"]));

            this.InvoiceDocumentType = dr["InvoiceDocumentType"].ToString();
            this.InvoiceNo = dr["InvoiceNo"].ToString();


            this.InvoiceDate = (dr["InvoiceDate"] == System.DBNull.Value ? null : (DateTime?)dr["InvoiceDate"]);

            if (!string.IsNullOrEmpty(dr["VendorID"].ToString()))
            {
                this.VendorID = Convert.ToInt64(dr["VendorID"]);
            }
            else {
                this.VendorID = null;
            }

            if (!string.IsNullOrEmpty(dr["TotalAmount"].ToString()))
                this.TotalAmount = Convert.ToDouble(dr["TotalAmount"]);

            if (!string.IsNullOrEmpty(dr["VatAmount"].ToString()))
                this.VatAmount = Convert.ToDouble(dr["VatAmount"]);

            if (!string.IsNullOrEmpty(dr["WHTAmount"].ToString()))
                this.WHTAmount = Convert.ToDouble(dr["WHTAmount"]);

            if (!string.IsNullOrEmpty(dr["NetAmount"].ToString()))
                this.NetAmount = Convert.ToDouble(dr["NetAmount"]);

            this.Description = dr["Description"].ToString();
            this.VendorCode = dr["VendorCode"].ToString();
            this.VendorBranch = dr["VendorBranch"].ToString();
            this.VendorName = dr["VendorName"].ToString();
            this.Street = dr["Street"].ToString();
            this.City = dr["City"].ToString();
            this.Country = dr["Country"].ToString();
            this.PostalCode = dr["PostalCode"].ToString();
            this.VendorTaxCode = dr["VendorTaxCode"].ToString();

            if (!string.IsNullOrEmpty(dr["isVat"].ToString()))
                this.IsVAT = (bool)dr["isVat"];
            else
            {
                this.VatAmount = 0;
                this.IsVAT = false;
            }

            if (!string.IsNullOrEmpty(dr["isWHT"].ToString()))
                this.IsWHT = (bool)dr["isWHT"];
            else
            {
                this.WHTAmount = 0;
                this.IsWHT = false;
            }

            if (this.IsVAT == true)
            {
                if (!string.IsNullOrEmpty(dr["TaxID"].ToString()))
                    this.TaxID = Convert.ToInt64(dr["TaxID"]);
            }
            else
            {
                this.TaxID = null;
            }

            if (!string.IsNullOrEmpty(dr["NonDeductAmount"].ToString()))
                this.NonDeductAmount = Convert.ToDouble(dr["NonDeductAmount"]);

            if (!string.IsNullOrEmpty(dr["TotalBaseAmount"].ToString()))
                this.TotalBaseAmount = Convert.ToDouble(dr["TotalBaseAmount"]);

            if (!string.IsNullOrEmpty(dr["WHTID1"].ToString()))
                this.WHTID1 = Convert.ToInt64(dr["WHTID1"]);

            if (!string.IsNullOrEmpty(dr["WHTRate1"].ToString()))
                this.WHTRate1 = Convert.ToDouble(dr["WHTRate1"]);

            if (!string.IsNullOrEmpty(dr["WHTTypeID1"].ToString()))
                this.WHTTypeID1 = Convert.ToInt64(dr["WHTTypeID1"]);

            if (!string.IsNullOrEmpty(dr["BaseAmount1"].ToString()))
                this.BaseAmount1 = Convert.ToDouble(dr["BaseAmount1"]);

            if (!string.IsNullOrEmpty(dr["WHTAmount1"].ToString()))
                this.WHTAmount1 = Convert.ToDouble(dr["WHTAmount1"]);

            if (!string.IsNullOrEmpty(dr["WHTID2"].ToString()))
                this.WHTID2 = Convert.ToInt64(dr["WHTID2"]);

            if (!string.IsNullOrEmpty(dr["WHTRate2"].ToString()))
                this.WHTRate2 = Convert.ToDouble(dr["WHTRate2"]);

            if (!string.IsNullOrEmpty(dr["WHTTypeID2"].ToString()))
                this.WHTTypeID2 = Convert.ToInt64(dr["WHTTypeID2"]);

            if (!string.IsNullOrEmpty(dr["BaseAmount2"].ToString()))
                this.BaseAmount2 = Convert.ToDouble(dr["BaseAmount2"]);

            if (!string.IsNullOrEmpty(dr["WHTAmount2"].ToString()))
                this.WHTAmount2 = Convert.ToDouble(dr["WHTAmount2"]);

            this.BranchCode = dr["BranchCode"].ToString();

            if (!string.IsNullOrEmpty(dr["TotalAmountLocalCurrency"].ToString()))
                this.TotalAmountLocalCurrency = Convert.ToDouble(dr["TotalAmountLocalCurrency"]);

            if (!string.IsNullOrEmpty(dr["TotalBaseAmountLocalCurrency"].ToString()))
                this.TotalBaseAmountLocalCurrency = Convert.ToDouble(dr["TotalBaseAmountLocalCurrency"]);

            if (!string.IsNullOrEmpty(dr["NetAmountLocalCurrency"].ToString()))
                this.NetAmountLocalCurrency = Convert.ToDouble(dr["NetAmountLocalCurrency"]);

            if (!string.IsNullOrEmpty(dr["ExchangeRateMainToTHBCurrency"].ToString()))
                this.ExchangeRateMainToTHBCurrency = Convert.ToDouble(dr["ExchangeRateMainToTHBCurrency"]);

            if (!string.IsNullOrEmpty(dr["ExchangeRateForLocalCurrency"].ToString()))
                this.ExchangeRateForLocalCurrency = Convert.ToDouble(dr["ExchangeRateForLocalCurrency"]);

            this.Active = (bool)dr["Active"];
            this.CreBy = Convert.ToInt64(dr["CreBy"].ToString());
            this.CreDate = Convert.ToDateTime(dr["CreDate"]);
            this.UpdBy = Convert.ToInt64(dr["UpdBy"].ToString());
            this.UpdDate = Convert.ToDateTime(dr["UpdDate"]);
            this.UpdPgm = dr["UpdPgm"].ToString();
        }
        public void SaveIDToDataRow(DataTable dt, DataRow dr, long newID)
        {
            long oldID = dr.Field<long>("InvoiceID");

            dt.PrimaryKey[0].ReadOnly = false;
            dt.Rows.Find(oldID).BeginEdit();
            dt.Rows.Find(oldID)["InvoiceID"] = newID;
            dt.Rows.Find(oldID).EndEdit();
            dt.PrimaryKey[0].ReadOnly = true;
        }
        public long GetIDFromDataRow(DataRow dr)
        {
            return Convert.ToInt64(dr["InvoiceID"].ToString());
        }
        public long GetIDFromDataRow(DataRow dr, DataRowVersion rowState)
        {
            return Convert.ToInt64(dr["InvoiceID", rowState].ToString());
        }
        #endregion
    }
}

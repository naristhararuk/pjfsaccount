﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by NHibernate.
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------
using System;
using SCG.eAccounting.DTO.DataSet;
using SS.Standard.Data.NHibernate.DTO;
using System.Data;

namespace SCG.eAccounting.DTO
{
    [Serializable]
    public partial class TADocument : INHibernateAdapterDTO<long>
    {
        #region Fields

        private long taDocumentID;
        private DateTime fromDate;
        private DateTime toDate;
        private bool isBusinessPurpose;
        private bool isTrainningPurpose;
        private bool isOtherPurpose;
        private string otherPurposeDescription;
        private string travelBy;
        private string province;
        private string country;
        private string ticketing;
        private long updBy;
        private DateTime updDate;
        private long creBy;
        private DateTime creDate;
        private string updPgm;
        private byte[] rowVersion;
        private bool active;
        private SCG.eAccounting.DTO.SCGDocument documentID;
        private SCG.DB.DTO.DbCostCenter costCenterID;
        private SCG.DB.DTO.DbAccount account;
        private SCG.DB.DTO.DbInternalOrder ioID;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TADocument class
        /// </summary>
        public TADocument()
        {
        }

        public TADocument(long taDocumentID)
        {
            this.taDocumentID = taDocumentID;
        }

        /// <summary>
        /// Initializes a new instance of the TADocument class
        /// </summary>
        /// <param name="fromDate">Initial <see cref="TADocument.FromDate" /> value</param>
        /// <param name="toDate">Initial <see cref="TADocument.ToDate" /> value</param>
        /// <param name="isBusinessPurpose">Initial <see cref="TADocument.IsBusinessPurpose" /> value</param>
        /// <param name="isTrainningPurpose">Initial <see cref="TADocument.IsTrainningPurpose" /> value</param>
        /// <param name="isOtherPurpose">Initial <see cref="TADocument.IsOtherPurpose" /> value</param>
        /// <param name="otherPurposeDescription">Initial <see cref="TADocument.OtherPurposeDescription" /> value</param>
        /// <param name="travelBy">Initial <see cref="TADocument.TravelBy" /> value</param>
        /// <param name="province">Initial <see cref="TADocument.Province" /> value</param>
        /// <param name="country">Initial <see cref="TADocument.Country" /> value</param>
        /// <param name="ticketing">Initial <see cref="TADocument.Ticketing" /> value</param>
        /// <param name="updBy">Initial <see cref="TADocument.UpdBy" /> value</param>
        /// <param name="updDate">Initial <see cref="TADocument.UpdDate" /> value</param>
        /// <param name="creBy">Initial <see cref="TADocument.CreBy" /> value</param>
        /// <param name="creDate">Initial <see cref="TADocument.CreDate" /> value</param>
        /// <param name="updPgm">Initial <see cref="TADocument.UpdPgm" /> value</param>
        /// <param name="rowVersion">Initial <see cref="TADocument.RowVersion" /> value</param>
        /// <param name="active">Initial <see cref="TADocument.Active" /> value</param>
        /// <param name="documentID">Initial <see cref="SCGDocument.DocumentID" /> value</param>
        /// <param name="costCenterID">Initial <see cref="DbCostCenter.CostCenterID" /> value</param>
        /// <param name="account">Initial <see cref="DbExpenseGroup.Account" /> value</param>
        /// <param name="ioID">Initial <see cref="DbInternalOrder.IOID" /> value</param>
        public TADocument(DateTime fromDate, DateTime toDate, bool isBusinessPurpose, bool isTrainningPurpose, bool isOtherPurpose, string otherPurposeDescription, string travelBy, string province, string country, string ticketing, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, byte[] rowVersion, bool active, SCG.eAccounting.DTO.SCGDocument documentID, SCG.DB.DTO.DbCostCenter costCenterID, SCG.DB.DTO.DbAccount account, SCG.DB.DTO.DbInternalOrder ioID)
        {
            this.fromDate = fromDate;
            this.toDate = toDate;
            this.isBusinessPurpose = isBusinessPurpose;
            this.isTrainningPurpose = isTrainningPurpose;
            this.isOtherPurpose = isOtherPurpose;
            this.otherPurposeDescription = otherPurposeDescription;
            this.travelBy = travelBy;
            this.province = province;
            this.country = country;
            this.ticketing = ticketing;
            this.updBy = updBy;
            this.updDate = updDate;
            this.creBy = creBy;
            this.creDate = creDate;
            this.updPgm = updPgm;
            this.rowVersion = rowVersion;
            this.active = active;
            this.documentID = documentID;
            this.costCenterID = costCenterID;
            this.account = account;
            this.ioID = ioID;
        }

        /// <summary>
        /// Minimal constructor for class TADocument
        /// </summary>
        /// <param name="updBy">Initial <see cref="TADocument.UpdBy" /> value</param>
        /// <param name="updDate">Initial <see cref="TADocument.UpdDate" /> value</param>
        /// <param name="creBy">Initial <see cref="TADocument.CreBy" /> value</param>
        /// <param name="creDate">Initial <see cref="TADocument.CreDate" /> value</param>
        /// <param name="updPgm">Initial <see cref="TADocument.UpdPgm" /> value</param>
        /// <param name="active">Initial <see cref="TADocument.Active" /> value</param>
        public TADocument(long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, bool active)
        {
            this.updBy = updBy;
            this.updDate = updDate;
            this.creBy = creBy;
            this.creDate = creDate;
            this.updPgm = updPgm;
            this.active = active;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the TADocumentID for the current TADocument
        /// </summary>
        public virtual long TADocumentID
        {
            get { return this.taDocumentID; }
            set { this.taDocumentID = value; }
        }

        /// <summary>
        /// Gets or sets the FromDate for the current TADocument
        /// </summary>
        public virtual DateTime FromDate
        {
            get { return this.fromDate; }
            set { this.fromDate = value; }
        }

        /// <summary>
        /// Gets or sets the ToDate for the current TADocument
        /// </summary>
        public virtual DateTime ToDate
        {
            get { return this.toDate; }
            set { this.toDate = value; }
        }

        /// <summary>
        /// Gets or sets the IsBusinessPurpose for the current TADocument
        /// </summary>
        public virtual bool IsBusinessPurpose
        {
            get { return this.isBusinessPurpose; }
            set { this.isBusinessPurpose = value; }
        }

        /// <summary>
        /// Gets or sets the IsTrainningPurpose for the current TADocument
        /// </summary>
        public virtual bool IsTrainningPurpose
        {
            get { return this.isTrainningPurpose; }
            set { this.isTrainningPurpose = value; }
        }

        /// <summary>
        /// Gets or sets the IsOtherPurpose for the current TADocument
        /// </summary>
        public virtual bool IsOtherPurpose
        {
            get { return this.isOtherPurpose; }
            set { this.isOtherPurpose = value; }
        }

        /// <summary>
        /// Gets or sets the OtherPurposeDescription for the current TADocument
        /// </summary>
        public virtual string OtherPurposeDescription
        {
            get { return this.otherPurposeDescription; }
            set { this.otherPurposeDescription = value; }        
        }

        /// <summary>
        /// Gets or sets the TravelBy for the current TADocument
        /// </summary>
        public virtual string TravelBy
        {
            get { return this.travelBy; }
            set { this.travelBy = value; }
        }

        /// <summary>
        /// Gets or sets the Province for the current TADocument
        /// </summary>
        public virtual string Province
        {
            get { return this.province; }
            set { this.province = value; }
        }

        /// <summary>
        /// Gets or sets the Country for the current TADocument
        /// </summary>
        public virtual string Country
        {
            get { return this.country; }
            set { this.country = value; }
        }

        /// <summary>
        /// Gets or sets the Ticketing for the current TADocument
        /// </summary>
        public virtual string Ticketing
        {
            get { return this.ticketing; }
            set { this.ticketing = value; }
        }

        /// <summary>
        /// Gets or sets the UpdBy for the current TADocument
        /// </summary>
        public virtual long UpdBy
        {
            get { return this.updBy; }
            set { this.updBy = value; }
        }

        /// <summary>
        /// Gets or sets the UpdDate for the current TADocument
        /// </summary>
        public virtual DateTime UpdDate
        {
            get { return this.updDate; }
            set { this.updDate = value; }
        }

        /// <summary>
        /// Gets or sets the CreBy for the current TADocument
        /// </summary>
        public virtual long CreBy
        {
            get { return this.creBy; }
            set { this.creBy = value; }
        }

        /// <summary>
        /// Gets or sets the CreDate for the current TADocument
        /// </summary>
        public virtual DateTime CreDate
        {
            get { return this.creDate; }
            set { this.creDate = value; }
        }

        /// <summary>
        /// Gets or sets the UpdPgm for the current TADocument
        /// </summary>
        public virtual string UpdPgm
        {
            get { return this.updPgm; }
            set { this.updPgm = value; }
        }

        /// <summary>
        /// Gets or sets the RowVersion for the current TADocument
        /// </summary>
        public virtual byte[] RowVersion
        {
            get { return this.rowVersion; }
            set { this.rowVersion = value; }
        }

        /// <summary>
        /// Gets or sets the Active for the current TADocument
        /// </summary>
        public virtual bool Active
        {
            get { return this.active; }
            set { this.active = value; }
        }

        /// <summary>
        /// Gets or sets the DocumentID for the current SCGDocument
        /// </summary>
        public virtual SCG.eAccounting.DTO.SCGDocument DocumentID
        {
            get { return this.documentID; }
            set { this.documentID = value; }
        }

        /// <summary>
        /// Gets or sets the CostCenterID for the current DbCostCenter
        /// </summary>
        public virtual SCG.DB.DTO.DbCostCenter CostCenterID
        {
            get { return this.costCenterID; }
            set { this.costCenterID = value; }
        }

        /// <summary>
        /// Gets or sets the AccountID for the current DbAccount
        /// </summary>
        public virtual SCG.DB.DTO.DbAccount Account
        {
            get { return this.account; }
            set { this.account = value; }
        }

        /// <summary>
        /// Gets or sets the IOID for the current DbInternalOrder
        /// </summary>
        public virtual SCG.DB.DTO.DbInternalOrder IOID
        {
            get { return this.ioID; }
            set { this.ioID = value; }
        }
        #endregion

        #region INHibernateAdapterDTO<long> Members
        public void LoadFromDataRow(DataRow dr)
        {
            this.TADocumentID = Convert.ToInt64(dr["TADocumentID"]);
            if (!string.IsNullOrEmpty(dr["DocumentID"].ToString()))
            {
                this.DocumentID = new SCGDocument(Convert.ToInt64(dr["DocumentID"]));
            }
            this.FromDate = Convert.ToDateTime(dr["FromDate"]);
            this.ToDate = Convert.ToDateTime(dr["ToDate"]);
            this.IsBusinessPurpose = (bool)dr["IsBusinessPurpose"];
            this.IsTrainningPurpose = (bool)dr["IsTrainningPurpose"];
            this.IsOtherPurpose = (bool)dr["IsOtherPurpose"];
            this.OtherPurposeDescription = dr["OtherPurposeDescription"].ToString();
            this.TravelBy = dr["TravelBy"].ToString();
            this.Province = dr["Province"].ToString();
            this.Country = dr["Country"].ToString();
            this.Ticketing = dr["Ticketing"].ToString();
            if (!string.IsNullOrEmpty(dr["CostCenterID"].ToString()))
            {
                this.CostCenterID = new SCG.DB.DTO.DbCostCenter(Convert.ToInt64(dr["CostCenterID"]));
            }
            if (!string.IsNullOrEmpty(dr["AccountID"].ToString()))
            {
                this.Account = new SCG.DB.DTO.DbAccount(Convert.ToInt64(dr["AccountID"]));
            }
            if (!string.IsNullOrEmpty(dr["IOID"].ToString()))
            {
                this.IOID = new SCG.DB.DTO.DbInternalOrder(Convert.ToInt64(dr["IOID"]));
            }
            this.Active = (bool)dr["Active"];
            this.CreBy = Convert.ToInt64(dr["CreBy"].ToString());
            this.CreDate = Convert.ToDateTime(dr["CreDate"]);
            this.UpdBy = Convert.ToInt64(dr["UpdBy"].ToString());
            this.UpdDate = Convert.ToDateTime(dr["UpdDate"]);
            this.UpdPgm = dr["UpdPgm"].ToString();
        }
        public void SaveIDToDataRow(DataTable dt, DataRow dr, long newID)
        {
            long oldID = dr.Field<long>("TADocumentID");

            dt.PrimaryKey[0].ReadOnly = false;
            dt.Rows.Find(oldID).BeginEdit();
            dt.Rows.Find(oldID)["TADocumentID"] = newID;
            dt.Rows.Find(oldID).EndEdit();
            dt.PrimaryKey[0].ReadOnly = true;
        }
        public long GetIDFromDataRow(DataRow dr)
        {
            return Convert.ToInt64(dr["TADocumentID"].ToString());
        }
        public long GetIDFromDataRow(DataRow dr, DataRowVersion rowState)
        {
            return Convert.ToInt64(dr["TADocumentID", rowState].ToString());
        }
        #endregion
    }
}

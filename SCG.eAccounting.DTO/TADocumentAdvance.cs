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
using System.Data;
using SS.Standard.Data.NHibernate.DTO;

namespace SCG.eAccounting.DTO
{
    [Serializable]

    public partial class TADocumentAdvance : INHibernateAdapterDTO<int>
    {
        #region Fields

        private int taDocumentAdvanceID;
        private long updBy;
        private DateTime updDate;
        private long creBy;
        private DateTime creDate;
        private string updPgm;
        private byte[] rowVersion;
        private bool active;
        private SCG.eAccounting.DTO.TADocument taDocument;
        private SCG.eAccounting.DTO.AvAdvanceDocument advance;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TADocumentAdvance class
        /// </summary>
        public TADocumentAdvance()
        {
        }

        public TADocumentAdvance(int taDocumentAdvanceID)
        {
            this.taDocumentAdvanceID = taDocumentAdvanceID;
        }

        /// <summary>
        /// Initializes a new instance of the TADocumentAdvance class
        /// </summary>
        /// <param name="updBy">Initial <see cref="TADocumentAdvance.UpdBy" /> value</param>
        /// <param name="updDate">Initial <see cref="TADocumentAdvance.UpdDate" /> value</param>
        /// <param name="creBy">Initial <see cref="TADocumentAdvance.CreBy" /> value</param>
        /// <param name="creDate">Initial <see cref="TADocumentAdvance.CreDate" /> value</param>
        /// <param name="updPgm">Initial <see cref="TADocumentAdvance.UpdPgm" /> value</param>
        /// <param name="rowVersion">Initial <see cref="TADocumentAdvance.RowVersion" /> value</param>
        /// <param name="active">Initial <see cref="TADocumentAdvance.Active" /> value</param>
        /// <param name="taDocument">Initial <see cref="TADocument.TADocumentID" /> value</param>
        /// <param name="advance">Initial <see cref="AvAdvanceDocument.AdvanceID" /> value</param>
        public TADocumentAdvance(long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, byte[] rowVersion, bool active, SCG.eAccounting.DTO.TADocument taDocument, SCG.eAccounting.DTO.AvAdvanceDocument advance)
        {
            this.updBy = updBy;
            this.updDate = updDate;
            this.creBy = creBy;
            this.creDate = creDate;
            this.updPgm = updPgm;
            this.rowVersion = rowVersion;
            this.active = active;
            this.taDocument = taDocument;
            this.advance = advance;
        }

        /// <summary>
        /// Minimal constructor for class TADocumentAdvance
        /// </summary>
        /// <param name="updBy">Initial <see cref="TADocumentAdvance.UpdBy" /> value</param>
        /// <param name="updDate">Initial <see cref="TADocumentAdvance.UpdDate" /> value</param>
        /// <param name="creBy">Initial <see cref="TADocumentAdvance.CreBy" /> value</param>
        /// <param name="creDate">Initial <see cref="TADocumentAdvance.CreDate" /> value</param>
        /// <param name="updPgm">Initial <see cref="TADocumentAdvance.UpdPgm" /> value</param>
        /// <param name="active">Initial <see cref="TADocumentAdvance.Active" /> value</param>
        public TADocumentAdvance(long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, bool active)
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
        /// Gets or sets the TADocumentAdvanceID for the current TADocumentAdvance
        /// </summary>
        public virtual int TADocumentAdvanceID
        {
            get { return this.taDocumentAdvanceID; }
            set { this.taDocumentAdvanceID = value; }
        }

        /// <summary>
        /// Gets or sets the UpdBy for the current TADocumentAdvance
        /// </summary>
        public virtual long UpdBy
        {
            get { return this.updBy; }
            set { this.updBy = value; }
        }

        /// <summary>
        /// Gets or sets the UpdDate for the current TADocumentAdvance
        /// </summary>
        public virtual DateTime UpdDate
        {
            get { return this.updDate; }
            set { this.updDate = value; }
        }

        /// <summary>
        /// Gets or sets the CreBy for the current TADocumentAdvance
        /// </summary>
        public virtual long CreBy
        {
            get { return this.creBy; }
            set { this.creBy = value; }
        }

        /// <summary>
        /// Gets or sets the CreDate for the current TADocumentAdvance
        /// </summary>
        public virtual DateTime CreDate
        {
            get { return this.creDate; }
            set { this.creDate = value; }
        }

        /// <summary>
        /// Gets or sets the UpdPgm for the current TADocumentAdvance
        /// </summary>
        public virtual string UpdPgm
        {
            get { return this.updPgm; }
            set { this.updPgm = value; }
        }

        /// <summary>
        /// Gets or sets the RowVersion for the current TADocumentAdvance
        /// </summary>
        public virtual byte[] RowVersion
        {
            get { return this.rowVersion; }
            set { this.rowVersion = value; }
        }

        /// <summary>
        /// Gets or sets the Active for the current TADocumentAdvance
        /// </summary>
        public virtual bool Active
        {
            get { return this.active; }
            set { this.active = value; }
        }

        /// <summary>
        /// Gets or sets the TADocumentID for the current TADocument
        /// </summary>
        public virtual SCG.eAccounting.DTO.TADocument TADocument
        {
            get { return this.taDocument; }
            set { this.taDocument = value; }
        }

        /// <summary>
        /// Gets or sets the AdvanceID for the current AvAdvanceDocument
        /// </summary>
        public virtual SCG.eAccounting.DTO.AvAdvanceDocument Advance
        {
            get { return this.advance; }
            set { this.advance = value; }
        }
        #endregion

        #region INHibernateAdapterDTO<int> Members
        public void LoadFromDataRow(DataRow dr)
        {
            this.taDocumentAdvanceID = Convert.ToInt32(dr["TADocumentAdvanceID"]);
            if (!string.IsNullOrEmpty(dr["TADocumentID"].ToString()))
            {
                this.TADocument = new TADocument(Convert.ToInt64(dr["TADocument"]));
            }
            if (!string.IsNullOrEmpty(dr["AdvanceID"].ToString()))
            {
                this.Advance = new AvAdvanceDocument(Convert.ToInt64(dr["AdvanceID"]));
            }
            this.Active = (bool)dr["Active"];
            this.CreBy = Convert.ToInt64(dr["CreBy"].ToString());
            this.CreDate = Convert.ToDateTime(dr["CreDate"]);
            this.UpdBy = Convert.ToInt64(dr["UpdBy"].ToString());
            this.UpdDate = Convert.ToDateTime(dr["UpdDate"]);
            this.UpdPgm = dr["UpdPgm"].ToString();
        }
        public void SaveIDToDataRow(DataTable dt, DataRow dr, int newID)
        {
            int oldID = dr.Field<int>("TADocumentAdvanceID");

            dt.PrimaryKey[0].ReadOnly = false;
            dt.Rows.Find(oldID).BeginEdit();
            dt.Rows.Find(oldID)["TADocumentAdvanceID"] = newID;
            dt.Rows.Find(oldID).EndEdit();
            dt.PrimaryKey[0].ReadOnly = true;
        }
        public int GetIDFromDataRow(DataRow dr)
        {
            return Convert.ToInt32(dr["TADocumentAdvanceID"].ToString());
        }
        public int GetIDFromDataRow(DataRow dr, DataRowVersion rowState)
        {
            return Convert.ToInt32(dr["TADocumentAdvanceID", rowState].ToString());
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.DTO;
using System.Data;

namespace SCG.eAccounting.DTO
{
    [Serializable]
    public class MPADocument : INHibernateAdapterDTO<long>
    {
        #region Fields
        private long mpaDocumentID;
        private DateTime startDate;
        private DateTime endDate;
        private long updBy;
        private DateTime updDate;
        private long creBy;
        private DateTime creDate;
        private string updPgm;
        private byte[] rowVersion;
        private bool active;
        private SCG.eAccounting.DTO.SCGDocument documentID;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TADocument class
        /// </summary>
        public MPADocument()
        {
        }

        public MPADocument(long mpaDocumentID)
        {
            this.mpaDocumentID = mpaDocumentID;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the TADocumentID for the current TADocument
        /// </summary>
        public virtual long MPADocumentID
        {
            get { return this.mpaDocumentID; }
            set { this.mpaDocumentID = value; }
        }

        /// <summary>
        /// Gets or sets the FromDate for the current TADocument
        /// </summary>
        public virtual DateTime StartDate
        {
            get { return this.startDate; }
            set { this.startDate = value; }
        }

        /// <summary>
        /// Gets or sets the ToDate for the current TADocument
        /// </summary>
        public virtual DateTime EndDate
        {
            get { return this.endDate; }
            set { this.endDate = value; }
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

        #endregion

        #region INHibernateAdapterDTO<long> Members

        public void LoadFromDataRow(System.Data.DataRow dr)
        {
            this.MPADocumentID = Convert.ToInt64(dr["MPADocumentID"]);
            if (!string.IsNullOrEmpty(dr["DocumentID"].ToString()))
            {
                this.DocumentID = new SCGDocument(Convert.ToInt64(dr["DocumentID"]));
            }
            this.StartDate = Convert.ToDateTime(dr["StartDate"]);
            this.EndDate = Convert.ToDateTime(dr["EndDate"]);

            this.Active = (bool)dr["Active"];
            this.CreBy = Convert.ToInt64(dr["CreBy"].ToString());
            this.CreDate = Convert.ToDateTime(dr["CreDate"]);
            this.UpdBy = Convert.ToInt64(dr["UpdBy"].ToString());
            this.UpdDate = Convert.ToDateTime(dr["UpdDate"]);
            this.UpdPgm = dr["UpdPgm"].ToString();
        }

        public void SaveIDToDataRow(System.Data.DataTable dt, System.Data.DataRow dr, long newID)
        {
            long oldID = dr.Field<long>("MPADocumentID");

            dt.PrimaryKey[0].ReadOnly = false;
            dt.Rows.Find(oldID).BeginEdit();
            dt.Rows.Find(oldID)["MPADocumentID"] = newID;
            dt.Rows.Find(oldID).EndEdit();
            dt.PrimaryKey[0].ReadOnly = true;
        }

        public long GetIDFromDataRow(System.Data.DataRow dr)
        {
            return Convert.ToInt64(dr["MPADocumentID"].ToString());
        }

        public long GetIDFromDataRow(System.Data.DataRow dr, System.Data.DataRowVersion rowState)
        {
            return Convert.ToInt64(dr["MPADocumentID", rowState].ToString());
        }

        #endregion
    }
}

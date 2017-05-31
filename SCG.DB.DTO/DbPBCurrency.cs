using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SS.Standard.Data.NHibernate.DTO;

namespace SCG.DB.DTO
{
    [Serializable]
    public partial class DbPBCurrency : INHibernateAdapterDTO<long>
    {
        #region Fields

        private long id;
        private long pbID;
        private short currencyID;
        private long updBy;
        private DateTime updDate;
        private long creBy;
        private DateTime creDate;
        private string updPgm;
        private Byte[] rowVersion;
        private string symbol;

        #endregion

        #region Properties

        public virtual long ID
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public virtual long PBID
        {
            get { return this.pbID; }
            set { this.pbID = value; }
        }

        public virtual short CurrencyID
        {
            get { return this.currencyID; }
            set { this.currencyID = value; }
        }

        public virtual long UpdBy
        {
            get { return this.updBy; }
            set { this.updBy = value; }
        }

        public virtual DateTime UpdDate
        {
            get { return this.updDate; }
            set { this.updDate = value; }
        }

        public virtual long CreBy
        {
            get { return this.creBy; }
            set { this.creBy = value; }
        }

        public virtual DateTime CreDate
        {
            get { return this.creDate; }
            set { this.creDate = value; }
        }

        public virtual string UpdPgm
        {
            get { return this.updPgm; }
            set { this.updPgm = value; }
        }

        public virtual Byte[] RowVersion
        {
            get { return this.rowVersion; }
            set { this.rowVersion = value; }
        }

        public virtual string Symbol
        {
            get { return this.symbol; }
            set { this.symbol = value; }
        }

        #endregion

        #region INHibernateAdapterDTO<long> Members
        public void LoadFromDataRow(DataRow dr)
        {
            this.ID = Convert.ToInt64(dr["ID"]);
            this.PBID = Convert.ToInt64(dr["PBID"]);
            if (!string.IsNullOrEmpty(dr["CurrencyID"].ToString()))
                this.CurrencyID = Convert.ToInt16(dr["CurrencyID"]);
            
            this.CreBy = Convert.ToInt64(dr["CreBy"].ToString());
            this.CreDate = Convert.ToDateTime(dr["CreDate"]);
            this.UpdBy = Convert.ToInt64(dr["UpdBy"].ToString());
            this.UpdDate = Convert.ToDateTime(dr["UpdDate"]);
            this.UpdPgm = dr["UpdPgm"].ToString();
            
            
        }
        public void SaveIDToDataRow(DataTable dt, DataRow dr, long newID)
        {
            long oldID = dr.Field<long>("ID");

            dt.PrimaryKey[0].ReadOnly = false;
            dt.Rows.Find(oldID).BeginEdit();
            dt.Rows.Find(oldID)["ID"] = newID;
            dt.Rows.Find(oldID).EndEdit();
            dt.PrimaryKey[0].ReadOnly = true;
        }
        public long GetIDFromDataRow(DataRow dr)
        {
            return Convert.ToInt64(dr["ID"].ToString());
        }
        public long GetIDFromDataRow(DataRow dr, DataRowVersion rowState)
        {
            return Convert.ToInt64(dr["ID", rowState].ToString());
        }
        #endregion
    }
}
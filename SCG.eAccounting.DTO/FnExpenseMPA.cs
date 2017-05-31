using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.DTO;
using System.Data;

namespace SCG.eAccounting.DTO
{
    public partial class FnExpenseMPA : INHibernateAdapterDTO<long>
    {
        private long fnExpenseMPAID;

        public virtual long FnExpenseMPAID
        {
            get { return fnExpenseMPAID; }
            set { fnExpenseMPAID = value; }
        }

        private FnExpenseDocument expenseID;

        public virtual FnExpenseDocument ExpenseID
        {
            get { return expenseID; }
            set { expenseID = value; }
        }

        private long mPADocumentID;

        public virtual long MPADocumentID
        {
            get { return mPADocumentID; }
            set { mPADocumentID = value; }
        }

        private bool active;

        public virtual bool Active
        {
            get { return active; }
            set { active = value; }
        }    

        private Int64 creBy;

        public virtual Int64 CreBy
        {
            get { return creBy; }
            set { creBy = value; }
        }

        private DateTime creDate;

        public virtual DateTime CreDate
        {
            get { return creDate; }
            set { creDate = value; }
        }

        private Int64 updBy;

        public virtual Int64 UpdBy
        {
            get { return updBy; }
            set { updBy = value; }
        }

        private DateTime updDate;

        public virtual DateTime UpdDate
        {
            get { return updDate; }
            set { updDate = value; }
        }

        private string updPgm;

        public virtual string UpdPgm
        {
            get { return updPgm; }
            set { updPgm = value; }
        }

        private Byte[] rowVersion;

        public virtual Byte[] RowVersion
        {
            get { return rowVersion; }
            set { rowVersion = value; }
        }

        public void LoadFromDataRow(DataRow dr)
        {
            if (!string.IsNullOrEmpty(dr["FnExpenseMPAID"].ToString()))
                this.FnExpenseMPAID = Convert.ToInt64(dr["FnExpenseMPAID"]);

            if (!string.IsNullOrEmpty(dr["ExpenseID"].ToString()))
                this.ExpenseID = new FnExpenseDocument(Convert.ToInt64(dr["ExpenseID"]));

            if (!string.IsNullOrEmpty(dr["MPADocumentID"].ToString()))
                this.MPADocumentID = Convert.ToInt64(dr["MPADocumentID"]);

            this.Active = (bool)dr["Active"];
            this.CreBy = Convert.ToInt64(dr["CreBy"].ToString());
            this.CreDate = Convert.ToDateTime(dr["CreDate"]);
            this.UpdBy = Convert.ToInt64(dr["UpdBy"].ToString());
            this.UpdDate = Convert.ToDateTime(dr["UpdDate"]);
            this.UpdPgm = dr["UpdPgm"].ToString();
        }
        public void SaveIDToDataRow(DataTable dt, DataRow dr, long newID)
        {
            long oldID = dr.Field<long>("FnExpenseMPAID");

            dt.PrimaryKey[0].ReadOnly = false;
            dt.Rows.Find(oldID).BeginEdit();
            dt.Rows.Find(oldID)["FnExpenseMPAID"] = newID;
            dt.Rows.Find(oldID).EndEdit();
            dt.PrimaryKey[0].ReadOnly = true;
        }
        public long GetIDFromDataRow(DataRow dr)
        {
            return Convert.ToInt64(dr["FnExpenseMPAID"].ToString());
        }
        public long GetIDFromDataRow(DataRow dr, DataRowVersion rowState)
        {
            return Convert.ToInt64(dr["FnExpenseMPAID", rowState].ToString());
        }
    }
}

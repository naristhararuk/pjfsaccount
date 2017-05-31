﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.DTO;
using System.Data;

namespace SCG.eAccounting.DTO
{
    public partial class FnExpenseCA : INHibernateAdapterDTO<long>
    {
        private long fnExpenseCAID;

        public virtual long FnExpenseCAID
        {
            get { return fnExpenseCAID; }
            set { fnExpenseCAID = value; }
        }

        private FnExpenseDocument expenseID;

        public virtual FnExpenseDocument ExpenseID
        {
            get { return expenseID; }
            set { expenseID = value; }
        }

        private long cADocumentID;

        public virtual long CADocumentID
        {
            get { return cADocumentID; }
            set { cADocumentID = value; }
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
            if (!string.IsNullOrEmpty(dr["FnExpenseCAID"].ToString()))
                this.FnExpenseCAID = Convert.ToInt64(dr["FnExpenseCAID"]);

            if (!string.IsNullOrEmpty(dr["ExpenseID"].ToString()))
                this.ExpenseID = new FnExpenseDocument(Convert.ToInt64(dr["ExpenseID"]));

            if (!string.IsNullOrEmpty(dr["CADocumentID"].ToString()))
                this.CADocumentID = Convert.ToInt64(dr["CADocumentID"]);

            this.Active = (bool)dr["Active"];
            this.CreBy = Convert.ToInt64(dr["CreBy"].ToString());
            this.CreDate = Convert.ToDateTime(dr["CreDate"]);
            this.UpdBy = Convert.ToInt64(dr["UpdBy"].ToString());
            this.UpdDate = Convert.ToDateTime(dr["UpdDate"]);
            this.UpdPgm = dr["UpdPgm"].ToString();
        }
        public void SaveIDToDataRow(DataTable dt, DataRow dr, long newID)
        {
            long oldID = dr.Field<long>("FnExpenseCAID");

            dt.PrimaryKey[0].ReadOnly = false;
            dt.Rows.Find(oldID).BeginEdit();
            dt.Rows.Find(oldID)["FnExpenseCAID"] = newID;
            dt.Rows.Find(oldID).EndEdit();
            dt.PrimaryKey[0].ReadOnly = true;
        }
        public long GetIDFromDataRow(DataRow dr)
        {
            return Convert.ToInt64(dr["FnExpenseCAID"].ToString());
        }
        public long GetIDFromDataRow(DataRow dr, DataRowVersion rowState)
        {
            return Convert.ToInt64(dr["FnExpenseCAID", rowState].ToString());
        }
    }
}

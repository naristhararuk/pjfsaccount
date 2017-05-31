using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.DTO;
using System.Data;
using SCG.DB.DTO;

namespace SCG.eAccounting.DTO
{
    [Serializable]
    public class FixedAdvanceDocument : INHibernateAdapterDTO<long>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TADocument class
        /// </summary>
        public FixedAdvanceDocument()
        {
        }

        public FixedAdvanceDocument(long fixedAdvanceID)
        {
            this.fixedAdvanceID = fixedAdvanceID;
        }
        #endregion

        #region Fields
        private long fixedAdvanceID;
        private SCG.eAccounting.DTO.SCGDocument documentID;
        private DateTime effectiveFromDate;
        private DateTime effectiveToDate;
        private DateTime requestDate;
        private string objective;
        private long? refFixedAdvanceID;
        private byte fixedAdvanceType;
        private Double amount;
        private Double netAmount;
        private SCG.DB.DTO.DbServiceTeam serviceTeamID;
        private String paymentType;
        private SCG.DB.DTO.Dbpb pBID;
        private bool active;
        private long updBy;
        private DateTime updDate;
        private long creBy;
        private DateTime creDate;
        private string updPgm;
        private byte[] rowVersion;
        private SCG.DB.DTO.DbServiceTeam returnServiceTeamID;
        private String returnPaymentType;
        private SCG.DB.DTO.Dbpb returnPBID;
        private DateTime? returnRequestDate;
        private string branchCodeReturn;
        private long? paymentMethodIDReturn;
        private DateTime? postingDateReturn;
        private DateTime? baseLineDateReturn;
        private string postingStatusReturn;
        private string fixedAdvanceBankAccount;
        #endregion

        #region Properties

        public virtual long FixedAdvanceID
        {
            get { return this.fixedAdvanceID; }
            set { this.fixedAdvanceID = value; }
        }

        public virtual DateTime EffectiveFromDate
        {
            get { return this.effectiveFromDate; }
            set { this.effectiveFromDate = value; }
        }

        public virtual DateTime EffectiveToDate
        {
            get { return this.effectiveToDate; }
            set { this.effectiveToDate = value; }
        }

        public virtual DateTime RequestDate
        {
            get { return this.requestDate; }
            set { this.requestDate = value; }
        }

        public virtual String Objective
        {
            get { return this.objective; }
            set { this.objective = value; }
        }

        public virtual long? RefFixedAdvanceID
        {
            get { return this.refFixedAdvanceID; }
            set { this.refFixedAdvanceID = value; }
        }

        public virtual byte FixedAdvanceType
        {
            get { return this.fixedAdvanceType; }
            set { this.fixedAdvanceType = value; }
        }

        public virtual Double Amount
        {
            get { return this.amount; }
            set { this.amount = value; }
        }

        public virtual Double NetAmount
        {
            get { return this.netAmount; }
            set { this.netAmount = value; }
        }

        public virtual SCG.DB.DTO.DbServiceTeam ServiceTeamID
        {
            get { return this.serviceTeamID; }
            set { this.serviceTeamID = value; }
        }

        public virtual String PaymentType
        {
            get { return this.paymentType; }
            set { this.paymentType = value; }
        }

        public virtual SCG.DB.DTO.Dbpb PBID
        {
            get { return this.pBID; }
            set { this.pBID = value; }
        }

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

        public virtual SCG.DB.DTO.DbServiceTeam ReturnServiceTeamID
        {
            get { return this.returnServiceTeamID; }
            set { this.returnServiceTeamID = value; }
        }

        public virtual String ReturnPaymentType
        {
            get { return this.returnPaymentType; }
            set { this.returnPaymentType = value; }
        }

        public virtual SCG.DB.DTO.Dbpb ReturnPBID
        {
            get { return this.returnPBID; }
            set { this.returnPBID = value; }
        }

        public virtual DateTime? ReturnRequestDate
        {
            get { return this.returnRequestDate; }
            set { this.returnRequestDate = value; }
        }

        public virtual string BranchCodeReturn
        {
            get { return this.branchCodeReturn; }
            set { this.branchCodeReturn = value; }
        }

        public virtual long? PaymentMethodIDReturn
        {
            get { return this.paymentMethodIDReturn; }
            set { this.paymentMethodIDReturn = value; }
        }

        public virtual DateTime? PostingDateReturn
        {
            get { return this.postingDateReturn; }
            set { this.postingDateReturn = value; }
        }

        public virtual DateTime? BaseLineDateReturn
        {
            get { return this.baseLineDateReturn; }
            set { this.baseLineDateReturn = value; }
        }

        public virtual string PostingStatusReturn
        {
            get { return this.postingStatusReturn; }
            set { this.postingStatusReturn = value; }
        }

        public virtual string FixedAdvanceBankAccount
        {
            get { return this.fixedAdvanceBankAccount; }
            set { this.fixedAdvanceBankAccount = value; }
        }



        #endregion

        #region INHibernateAdapterDTO<long> Members

        public void LoadFromDataRow(System.Data.DataRow dr)
        {
            this.FixedAdvanceID = Convert.ToInt64(dr["FixedAdvanceID"]);
            if (!string.IsNullOrEmpty(dr["DocumentID"].ToString()))
            {
                this.DocumentID = new SCGDocument(Convert.ToInt64(dr["DocumentID"]));
            }
            this.EffectiveFromDate = Convert.ToDateTime(dr["EffectiveFromDate"]);
            this.EffectiveToDate = Convert.ToDateTime(dr["EffectiveToDate"]);
            this.RequestDate = Convert.ToDateTime(dr["RequestDate"]);
            this.Objective = Convert.ToString(dr["Objective"]);

            if (!string.IsNullOrEmpty(dr["RefFixedAdvanceID"].ToString()) && Convert.ToInt64(dr["RefFixedAdvanceID"]) != 0)
                this.RefFixedAdvanceID = Convert.ToInt64(dr["RefFixedAdvanceID"]);
            else
                this.RefFixedAdvanceID = null;

            this.FixedAdvanceType = Convert.ToByte(dr["FixedAdvanceType"]);
            this.Amount = Convert.ToDouble(dr["Amount"]);
            this.NetAmount = Convert.ToDouble(dr["NetAmount"]);
            this.PaymentType = Convert.ToString(dr["PaymentType"]);
            if (!string.IsNullOrEmpty(dr["PBID"].ToString()) && Convert.ToInt64(dr["PBID"]) != 0)
                this.PBID = new Dbpb(Convert.ToInt64(dr["PBID"]));
            else
                this.PBID = null;
            if (!string.IsNullOrEmpty(dr["ServiceTeamID"].ToString()) && Convert.ToInt64(dr["ServiceTeamID"]) != 0)
                this.ServiceTeamID = new DbServiceTeam(Convert.ToInt64(dr["ServiceTeamID"]));
            else
                this.ServiceTeamID = null;
            this.Active = (bool)dr["Active"];
            this.CreBy = Convert.ToInt64(dr["CreBy"].ToString());
            this.CreDate = Convert.ToDateTime(dr["CreDate"]);
            this.UpdBy = Convert.ToInt64(dr["UpdBy"].ToString());
            this.UpdDate = Convert.ToDateTime(dr["UpdDate"]);
            this.UpdPgm = dr["UpdPgm"].ToString();
            this.ReturnPaymentType = Convert.ToString(dr["ReturnPaymentType"]);
            if (!string.IsNullOrEmpty(dr["ReturnPBID"].ToString()) && Convert.ToInt64(dr["ReturnPBID"]) != 0)
                this.ReturnPBID = new Dbpb(Convert.ToInt64(dr["ReturnPBID"]));
            else
                this.ReturnPBID = null;
            if (!string.IsNullOrEmpty(dr["ReturnServiceTeamID"].ToString()) && Convert.ToInt64(dr["ReturnServiceTeamID"]) != 0)
                this.ReturnServiceTeamID = new DbServiceTeam(Convert.ToInt64(dr["ReturnServiceTeamID"]));
            else
                this.ReturnServiceTeamID = null;

            if (!string.IsNullOrEmpty(dr["ReturnRequestDate"].ToString()))
                this.ReturnRequestDate = Convert.ToDateTime(dr["ReturnRequestDate"]);
            else
                this.ReturnRequestDate = null;

            //ReturnPosting 
            this.BranchCodeReturn = dr["BranchCodeReturn"].ToString();
            this.PostingStatusReturn = dr["PostingStatusReturn"].ToString();
            this.FixedAdvanceBankAccount = dr["FixedAdvanceBankAccount"].ToString();

            if ((dr["PaymentMethodIDReturn"] != DBNull.Value) && (Convert.ToInt64(dr["PaymentMethodIDReturn"]) > 0))
                this.PaymentMethodIDReturn = Convert.ToInt64(dr["PaymentMethodIDReturn"].ToString());

            if ((dr["PostingDateReturn"] != DBNull.Value) && (Convert.ToDateTime(dr["PostingDateReturn"]) != DateTime.MinValue))
                this.PostingDateReturn = Convert.ToDateTime(dr["PostingDateReturn"]);

            if ((dr["BaseLineDateReturn"] != DBNull.Value) && (Convert.ToDateTime(dr["BaseLineDateReturn"]) != DateTime.MinValue))
                this.BaseLineDateReturn = Convert.ToDateTime(dr["BaseLineDateReturn"]);
        }

        public void SaveIDToDataRow(System.Data.DataTable dt, System.Data.DataRow dr, long newID)
        {
            long oldID = dr.Field<long>("FixedAdvanceID");

            dt.PrimaryKey[0].ReadOnly = false;
            dt.Rows.Find(oldID).BeginEdit();
            dt.Rows.Find(oldID)["FixedAdvanceID"] = newID;
            dt.Rows.Find(oldID).EndEdit();
            dt.PrimaryKey[0].ReadOnly = true;
        }

        public long GetIDFromDataRow(System.Data.DataRow dr)
        {
            return Convert.ToInt64(dr["FixedAdvanceID"].ToString());
        }

        public long GetIDFromDataRow(System.Data.DataRow dr, System.Data.DataRowVersion rowState)
        {
            return Convert.ToInt64(dr["FixedAdvanceID", rowState].ToString());
        }

        #endregion
    }
}

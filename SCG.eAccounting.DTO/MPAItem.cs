using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.DTO;
using System.Data;


namespace SCG.eAccounting.DTO
{
    public partial class MPAItem : INHibernateAdapterDTO<long>
    {

        public MPAItem()
        {
        }

        public MPAItem(long mPAItemID)
        {
            this.mPAItemID = mPAItemID;
        }


        private Int64 mPAItemID;

        public virtual Int64 MPAItemID
        {
            get { return mPAItemID; }
            set { mPAItemID = value; }
        }

        private SCG.eAccounting.DTO.MPADocument mPADocumentID;

        public virtual SCG.eAccounting.DTO.MPADocument MPADocumentID
        {
            get { return mPADocumentID; }
            set { mPADocumentID = value; }
        }

        private SS.SU.DTO.SuUser userID;

        public virtual SS.SU.DTO.SuUser UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        private string mobilePhoneNo;

        public virtual string MobilePhoneNo
        {
            get { return mobilePhoneNo; }
            set { mobilePhoneNo = value; }
        }

        private string mobileBrand;

        public virtual string MobileBrand
        {
            get { return mobileBrand; }
            set { mobileBrand = value; }
        }

        private string mobileModel;

        public virtual string MobileModel
        {
            get { return mobileModel; }
            set { mobileModel = value; }
        }

        private Decimal actualAmount;

        public virtual Decimal ActualAmount
        {
            get { return actualAmount; }
            set { actualAmount = value; }
        }


        private Decimal actualAmountNotExceed;

        public virtual Decimal ActualAmountNotExceed
        {
            get { return actualAmountNotExceed; }
            set { actualAmountNotExceed = value; }
        }

        private Decimal amountCompanyPackage;

        public virtual Decimal AmountCompanyPackage
        {
            get { return amountCompanyPackage; }
            set { amountCompanyPackage = value; }
        }

        private Decimal totalAmount;

        public virtual Decimal TotalAmount
        {
            get { return totalAmount; }
            set { totalAmount = value; }
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

        #region INHibernateAdapterDTO<long> Members

        public void LoadFromDataRow(System.Data.DataRow dr)
        {

            this.MPAItemID = Convert.ToInt64(dr["MPAItemID"].ToString());
            if (!string.IsNullOrEmpty(dr["MPADocumentID"].ToString()))
            {
                this.MPADocumentID = new SCG.eAccounting.DTO.MPADocument(Convert.ToInt64(dr["MPADocumentID"]));
            }
            this.UserID = new SS.SU.DTO.SuUser(Convert.ToInt64(dr["UserID"]));
            this.MobilePhoneNo = dr["MobilePhoneNo"].ToString();
            this.MobileBrand = dr["MobileBrand"].ToString();
            this.mobileModel = dr["mobileModel"].ToString();
            this.ActualAmount = Convert.ToDecimal(dr["ActualAmount"].ToString());
            this.ActualAmountNotExceed = Convert.ToDecimal(dr["ActualAmountNotExceed"].ToString());
            this.AmountCompanyPackage = Convert.ToDecimal(dr["AmountCompanyPackage"].ToString());
            this.TotalAmount = Convert.ToDecimal(dr["TotalAmount"].ToString());

            this.CreBy = Convert.ToInt64(dr["CreBy"].ToString());
            this.CreDate = Convert.ToDateTime(dr["CreDate"]);
            this.UpdBy = Convert.ToInt64(dr["UpdBy"].ToString());
            this.UpdDate = Convert.ToDateTime(dr["UpdDate"]);
            this.UpdPgm = dr["UpdPgm"].ToString();
        
        }

        public void SaveIDToDataRow(System.Data.DataTable dt, System.Data.DataRow dr, long newID)
        {
            long oldID = dr.Field<long>("MPAItemID");

            dt.PrimaryKey[0].ReadOnly = false;
            dt.Rows.Find(oldID).BeginEdit();
            dt.Rows.Find(oldID)["MPAItemID"] = newID;
            dt.Rows.Find(oldID).EndEdit();
            dt.PrimaryKey[0].ReadOnly = true;
        }

        public long GetIDFromDataRow(System.Data.DataRow dr)
        {
            return Convert.ToInt64(dr["MPAItemID"].ToString());
        }

        public long GetIDFromDataRow(System.Data.DataRow dr, System.Data.DataRowVersion rowState)
        {
            return Convert.ToInt64(dr["MPAItemID", rowState].ToString());
        }

        #endregion
    }
}

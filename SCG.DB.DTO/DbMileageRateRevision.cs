using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.DTO;
using System.Data;


namespace SCG.DB.DTO
{
    public partial class DbMileageRateRevision
    {

        public DbMileageRateRevision()
        {
        }

        public  DbMileageRateRevision(Guid Id)
        {
            this.id = Id;      
        }
        private Guid id;
        public virtual Guid Id 
        { 
            get { return id;}
            set { id = value;}
        }

        private DateTime? approvedDate;
        public virtual DateTime? ApprovedDate 
        {
            get {return approvedDate ;}
            set { approvedDate = value; } 
        }

        private DateTime? effectiveFromDate;
        public virtual DateTime? EffectiveFromDate 
        {
            get {return effectiveFromDate ;}
            set { effectiveFromDate = value; } 
        }
     
        private DateTime? effectiveToDate;
        public virtual DateTime? EffectiveToDate 
        {
            get {return effectiveToDate ;}
            set { effectiveToDate = value; } 
        }
        private  byte status;
        public virtual byte Status 
        {
            get { return status; }
            set { status = value; }
        }

        private string statusDesc;
        public virtual string StatusDesc 
        {
            get { return statusDesc; }
            set { statusDesc = value; }
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

        private bool active;

        public virtual bool Active
        {
            get { return active; }
            set { active = value; }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO
{
    [Serializable]
    public partial class DbHolidayProfile
    {
        public DbHolidayProfile()
        {
        }

        public DbHolidayProfile(Int32 id)
        {
            this.id = id;
        }
        private Int32 id;
        public virtual Int32 Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        private Int32 year;
        public virtual Int32 Year
        {
            get { return year; }
            set { year = value; }
        }

        private bool isApprove;
        public virtual bool IsApprove
        {
            get { return isApprove; }
            set { isApprove = value; }
        }
        private string type;
        public virtual string Type
        {
            get { return type; }
            set { type = value; }
        }
        private long updBy;
        public virtual long UpdBy
        {
            get { return this.updBy; }
            set { this.updBy = value; }
        }

        private DateTime updDate;
        public virtual DateTime UpdDate
        {
            get { return this.updDate; }
            set { this.updDate = value; }
        }

        private long creBy;
        public virtual long CreBy
        {
            get { return this.creBy; }
            set { this.creBy = value; }
        }

        private DateTime creDate;
        public virtual DateTime CreDate
        {
            get { return this.creDate; }
            set { this.creDate = value; }
        }

        private string updPgm;
        public virtual string UpdPgm
        {
            get { return this.updPgm; }
            set { this.updPgm = value; }
        }

        private byte[] rowVersion;
        public virtual byte[] RowVersion
        {
            get { return this.rowVersion; }
            set { this.rowVersion = value; }
        }
    }
}

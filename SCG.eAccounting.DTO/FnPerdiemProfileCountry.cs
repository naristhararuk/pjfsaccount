using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO
{
    public class FnPerdiemProfileCountry
    {
        private long iD;
        public virtual long ID
        {
            get { return this.iD; }
            set { this.iD = value; }
        }
        private long perdiemProfileID;
        public virtual long PerdiemProfileID
        {
            get { return this.perdiemProfileID; }
            set { this.perdiemProfileID = value; }
        }
        private short zoneID;
        public virtual short ZoneID
        {
            get { return this.zoneID; }
            set { this.zoneID = value; }
        }
        private short countryID;
        public virtual short CountryID
        {
            get { return this.countryID; }
            set { this.countryID = value; }
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

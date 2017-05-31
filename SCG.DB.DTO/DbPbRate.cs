using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO
{
    [Serializable]
    public partial class DbPbRate
    {
        public virtual long ID { get; set; }
        public virtual long PBID { get; set; }
        public virtual DateTime? EffectiveDate { get; set; }
        public virtual short MainCurrencyID { get; set; }
        public virtual double FromAmount { get; set; }
        public virtual short CurrencyID { get; set; }
        public virtual double ToAmount { get; set; }
        public virtual double ExchangeRate { get; set; }
        public virtual string UpdateBy { get; set; }
        public virtual DateTime CreDate { get; set; }
        public virtual long CreBy { get; set; }
        public virtual DateTime UpdDate { get; set; }
        public virtual long UpdBy { get; set; }
        public virtual string UpdPgm { get; set; }
        public virtual Byte[] RowVersion { get; set; }
    }
}

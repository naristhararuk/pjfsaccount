using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
    [Serializable]
    public class VOPbRate
    {
        public long ID { get; set; }
        public long PBID { get; set; }
        public DateTime EffectiveDate { get; set; }
        public short MainCurrencyID { get; set; }
        public string FromCurrencySymbol { get; set; }
        public double FromAmount { get; set; }
        public short CurrencyID { get; set; }
        public string ToCurrencySymbol { get; set; }
        public double ToAmount { get; set; }
        public double ExchangeRate { get; set; }
        public string UpdateBy { get; set; }
        public long CreBy { get; set; }
        public DateTime CreDate { get; set; }
        public long UpdBy { get; set; }
        public DateTime UpdDate { get; set; }
        public string UpdPgm { get; set; }
        public Byte[] RowVersion { get; set; }
    }
}

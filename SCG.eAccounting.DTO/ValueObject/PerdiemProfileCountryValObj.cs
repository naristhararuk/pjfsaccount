using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    public class PerdiemProfileCountryValObj
    {
        public long ID { get; set; }
        public long PerdiemProfileID { get; set; }
        public short ZoneID { get; set; }
        public string ZoneName { get; set; }
        public short CountryID { get; set; }
        public string CountryName { get; set; }
    }
}

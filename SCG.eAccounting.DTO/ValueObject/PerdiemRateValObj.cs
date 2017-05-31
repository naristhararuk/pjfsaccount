using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    public class PerdiemRateValObj
    {
        public long PerdiemRateID { get; set; }
        public long PerdiemProfileID { get; set; }
        public string PerdiemProfileName { get; set; }
        public short ZoneID { get; set; }
        public string ZoneName { get; set; }
        public long CompanyID { get; set;  }
        public string PersonalLevel { get; set; }
        public double ExtraPerdiemRate { get; set; }
        public double OfficialPerdiemRate { get; set; }
        public double StuffPerdiemRate { get; set; }
        public double InternationalStaffPerdiemRate { get; set; }
        public double SCGStaffPerdiemRate { get; set; }
        public bool Active { get; set; }
    }
}

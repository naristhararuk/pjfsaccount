using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.DB.DTO.ValueObject
{
    [Serializable]
    public class DBProvinceLovReturn
    {
        public short    ProvinceID { get; set; }
        public string   ProvinceName { get; set; }
        public short    RegionID { get; set; }
        public string   RegionName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.DB.DTO.ValueObject
{
    public class ProvinceLang
    {
        public short?  ProvinceId      { get; set; }
        public string ProvinceName { get; set; }
        public short? RegionId { get; set; }
        public string RegionName { get; set; }
        public string Comment { get; set; }
        public bool Active { get; set; }

        public short? LanguageId { get; set; }
        public string LanguageName { get; set; }

        

        public ProvinceLang()
        { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
    public class CountryLang
    {
        public short? CountryID { get; set; }
        public string CountryCode { get; set; }
        public string LanguageName { get; set; }
        public short ID { get; set; }
        public short? LanguageId { get; set; }
        public string CountryName { get; set; }

        public string Comment { get; set; }
        public bool Active { get; set; }
        public short CID { get; set; }
        public CountryLang()
        {
        }
    }
}

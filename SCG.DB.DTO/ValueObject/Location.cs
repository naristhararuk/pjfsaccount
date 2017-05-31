using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
    public class Location
    {
        public long? CompanyID { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public long? LocationID { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public bool Active { get; set; }

        public string LanguageName { get; set; }
        public string Comment { get; set; }

        public short? LanguageId { get; set; }
    }
}

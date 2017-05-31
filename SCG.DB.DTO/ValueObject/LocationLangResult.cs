using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
    public class LocationLangResult
    {
        public long LocationLangID { get; set; }
        public short LanguageID { get; set; }
        public string LanguageName { get; set; }
        public string LocationName { get; set; }
        public string Comment { get; set; }
        public bool Active { get; set; }
    }
}

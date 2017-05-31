using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.DB.DTO.ValueObject
{
    public class StatusLang
    {
        public short? StatusID { get; set; }
        public short? LanguageId { get; set; }
        public string LanguageName { get; set; }
        public string StatusDesc { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public Boolean Active { get; set; }

        public StatusLang()
        { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO.ValueObject
{
    public class DivisionLang
    {
        public long? DivisionLangId { get; set; }
        public short? DivisionId { get; set; }
        public short? LanguageId { get; set; }
        public string LanguageName { get; set; }
        public string DivisionName { get; set; }
        public string Comment { get; set; }
        public Boolean Active { get; set; }

        public DivisionLang()
        { }
    }
}

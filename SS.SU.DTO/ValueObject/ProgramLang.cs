using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO.ValueObject
{
    public class ProgramLang
    {
        public long? ProgramLangId { get; set; }
        public short? ProgramId { get; set; }
        public short? LanguageId { get; set; }
        public string LanguageName { get; set; }
        public string ProgramName { get; set; }
        public string Comment { get; set; }
        public bool Active { get; set; }


        public ProgramLang()
        {
        }
    }

}

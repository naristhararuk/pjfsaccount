using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO.ValueObject
{
    public class GlobalTranslateLang
    {
        public short? TranslateLangId { get; set; }
        public long? TranslateId { get; set; }
        public short? LanguageId { get; set; }
        public string LanguageName { get; set; }
        public string TranslateWord { get; set; }
        public string Comment { get; set; }
        public Boolean Active { get; set; }

        public GlobalTranslateLang()
        { }
    }
}

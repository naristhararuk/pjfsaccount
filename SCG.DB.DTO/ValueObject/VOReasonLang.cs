using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
    public class VOReasonLang
    {        
        public short?   ReasonID          { get; set; }
        public string   ReasonCode          { get; set; }
        public string   LanguageName        { get; set; }
        public string   DocumentTypeCode        { get; set; }

        public short?   LanguageID          { get; set; }
        public string   ReasonDetail      { get; set; }

        public string   Comment         { get; set; }
        public bool     Active          { get; set; }

        public VOReasonLang()
        {
        }
    }
}

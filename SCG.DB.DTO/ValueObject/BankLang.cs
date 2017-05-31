using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
    public class BankLang
    {        
        public short?   BankId          { get; set; }
        public string   BankNo          { get; set; }
        

        public short?   LanguageId      { get; set; }
        public string   LanguageName    { get; set; }
        public string   BankName        { get; set; }
        public string   ABBRName        { get; set; }

        public string   Comment         { get; set; }
        public bool     Active          { get; set; }
        public int?     BankCount { get; set; }

        public BankLang()
        {
        }
    }
}

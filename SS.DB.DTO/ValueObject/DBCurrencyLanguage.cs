using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.DB.DTO.ValueObject
{
    [Serializable]
    public class DBCurrencyLanguage
    {

        public string Symbol { get; set; }
        public short? CurrencyID { get; set; }
        public Boolean CurrencyActive { get; set; }
        
        public short ?Languageid{get;set;}
        public string LanguageName { get;set;}
        public string Description{get;set;}
        public string Comment{get;set;}
        public Boolean LangActive { get; set; }

       

    }
}

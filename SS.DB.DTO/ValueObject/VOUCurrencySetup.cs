using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.DB.DTO.ValueObject
{
    [Serializable]
    public class VOUCurrencySetup
    {
        #region Property
        public string Symbol{ get; set; }
        public string Comment { get; set; }
        public string Description { get; set; }
        public Boolean Active { get; set; }
        public short? CurrencyID { get; set; }
        public long? CurrencyLangID { get; set; }
        public short? CLanguageID { get; set; }
        
       
        public short? LanguageID { get; set; }
        public string LanguageName { get; set; }
       
        public Boolean LangActive { get; set; }

        public string MainUnit { get; set; }
        public string SubUnit { get; set; }

        #endregion
    }
}

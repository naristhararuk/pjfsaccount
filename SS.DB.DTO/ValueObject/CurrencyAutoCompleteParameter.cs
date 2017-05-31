using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.DB.DTO.ValueObject
{
    [Serializable]
    public class CurrencyAutoCompleteParameter
    {
        public bool IsExpense { get; set; }
        public short LanguageID { get; set; }
        public short CurrencyID { get; set; }
        public string Symbol { get; set; }
        public string Desc { get; set; }
        public bool IsAdvanceFR { get; set; }
    }
}

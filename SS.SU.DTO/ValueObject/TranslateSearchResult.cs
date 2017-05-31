using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO.ValueObject
{
    public class TranslateSearchResult
    {
        public long TranslateId { get; set; }
        public string ProgramName { get; set; }
        public string TranslateSymbol { get; set; }
        public string Comment { get; set; }
        public string TranslateControl { get; set; }
        public string ProgramCode { get; set; }

        public TranslateSearchResult()
        { }

    }
}

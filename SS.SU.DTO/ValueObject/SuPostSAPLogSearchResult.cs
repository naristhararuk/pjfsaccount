using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO.ValueObject
{
    public class SuPostSAPLogSearchResult
    {
        public DateTime? Date { get; set; }
        public String RequestNo { get; set; }
        public Double? PostNo { get; set; }
        public Double? DocumentSeqOnRequest { get; set; }
        public string InvoiceNo { get; set; }
        public Double? Year { get; set; }
        public string CompanyCode { get; set; }
        public string FiDocument { get; set; }
        public string Flag { get; set; }
        public string Message { get; set; }

        public SuPostSAPLogSearchResult()
        { }
    }
}

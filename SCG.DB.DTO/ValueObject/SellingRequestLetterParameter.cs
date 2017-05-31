using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
    public class SellingRequestLetterParameter
    {
        public long CompanyID { get; set; }
        public long LetterID { get; set; }
        public string LetterNo { get; set; }
        public bool IsIncludeGeneratedLetter { get; set; }
        public DateTime RequestDate { get; set; }
        public string CompanyCode { get; set; }
        public int Year { get; set; }
        public string DocumentNo { get; set; }
        public long DocumentID { get; set; }
    }
}

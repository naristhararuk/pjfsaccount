using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
    public class SellingDetailResult
    {
        public string RequestName { get; set; }
        public double Amount { get; set; }
        public string LetterNo { get; set; }
        public long DocumentID { get; set; }
        public string DocumentNo { get; set; }
        public string CompanyCode { get; set; }
        public int Year { get; set; }
    }
}

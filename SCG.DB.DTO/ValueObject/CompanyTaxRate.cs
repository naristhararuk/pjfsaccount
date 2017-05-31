using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
    public class CompanyTaxRate
    {
        public long ID { get; set; }
        public long TaxID { get; set; }
        public long CompanyID { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public double Rate { get; set; }
        public double RateNonDeduct { get; set; }
        public bool UseParentRate { get; set; }
        public bool Disable { get; set; }
    }
}

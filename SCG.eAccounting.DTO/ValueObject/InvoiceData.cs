using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    public class InvoiceData
    {
        public int InvoiceId { get; set; }
        public int? Seq { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public double TotalAmount { get; set; }
        public double VatAmount { get; set; }
        public double WHTAmount { get; set; }
        public double NetAmount { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public string VendorName1 { get; set; }
        public string VendorName2 { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string IONumber { get; set; }
    }
}

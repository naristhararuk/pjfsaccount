using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    public class ExportBoxID
    {
        public long DocumentID { get; set; }
        public string BoxID { get; set; }
        public DateTime DocumentDate { get; set; }
        public string CompanyCode { get; set; }
        public string Status { get; set; }
        public string FIDocNumber { get; set; }
        public string ImageDocID { get; set; }
    }
}

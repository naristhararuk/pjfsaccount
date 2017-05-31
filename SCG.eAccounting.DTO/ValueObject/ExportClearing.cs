using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    public class ExportClearing
    {
        public string DocumentID { get; set; }
        public string CompanyCode { get; set; }
        public string FIDOC { get; set; }
        public string Year { get; set; }
    }
}

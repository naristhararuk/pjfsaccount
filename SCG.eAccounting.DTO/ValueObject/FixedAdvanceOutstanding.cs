using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    [Serializable]
    public class FixedAdvanceOutstanding
    {
        //public short No { get; set; }
        public long DocumentID { get; set; }
        public string fixedAdvanceNo { get; set; }
        public long fixedAdvanceDocumentID { get; set; }
        public string subject { get; set; }
        public string fixedAdvanceStatus { get; set; }
        public DateTime? effecttiveDateFrom { get; set; }
        public DateTime? effecttiveDateTo { get; set; }
        public double? amount { get; set; }
        public string objective { get; set; }
        public FixedAdvanceOutstanding()
        { }
       
    }
}

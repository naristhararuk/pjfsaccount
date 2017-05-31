using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    public class AdvanceData
    {
        public string DocumentNo { get; set; }
        public long AdvanceID { get; set; }
        public string Subject { get; set; }
        public DateTime RequestDateOfAdvance { get; set; }
        public double Amount { get; set; }
        // For update clearing advance and remittance advance
        public long UpdBy { get; set; }
        public string ProgramCode { get; set; }
    }
}

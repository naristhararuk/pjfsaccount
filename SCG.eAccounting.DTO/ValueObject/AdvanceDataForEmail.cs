using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    public class AdvanceDataForEmail
    {
        public string DocumentNo { get; set; }
        public long AdvanceID { get; set; }
        public string Subject { get; set; }
        public DateTime RequestDateOfAdvance { get; set; }
        public double Amount { get; set; }
        // For update clearing advance and remittance advance
        public long UpdBy { get; set; }
        public string ProgramCode { get; set; }
        public bool IsRepOffice { get; set; }
        public int CurrencyID { get; set; }
        public string SymbolLocal { get; set; }
        public string SymbolMain { get; set; }
        public double? advitemMainCurrencyAmount { get; set; }
        public double? advdocMainCurrencyAmount { get; set; }
        public double? advdocLocalCurrencyAmount { get; set; }
        public string AdvanceType { get; set; }
        public long? PBID { get; set; }
        
    }
}
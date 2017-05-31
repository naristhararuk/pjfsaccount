using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    public class EmailValueObject
    {
        public string RequestID { get; set; }
        public string Subject { get; set; }


        public string TravelTo { get; set; }
        public string Fromdate { get; set; }
        public string ToDate { get; set; }

        public string RequestDateOfAdvance { get; set; }
        public string Currency { get; set; }
        public string AdvItemAmount { get; set; }

        public string Amount { get; set; }

        public string SymbolLocal { get; set; }
        public string SymbolMain { get; set; }
        public string TotalExpenseLocal { get; set; }
        public string TotalExpenseMain { get; set; }
        public bool IsRepOffice { get; set; }

        public long PBID { get; set; }
        public string DifferenceAmount { get; set; }
        public string DifferenceAmountLocalCurrency { get; set; }
        public string DifferenceAmountMainCurrency { get; set; }

        public bool IsForeign { get; set; }

        public IList<InitiatorData> InitiatorList { get; set; }
        public IList<InvoiceDataForEmail> Invoices { get; set; }
    }
}

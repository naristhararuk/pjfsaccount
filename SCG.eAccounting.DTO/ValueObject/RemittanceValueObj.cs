using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    public class RemittanceValueObj
    {
        public long? WorkflowID { get; set; }
        public long? RemittanceID { get; set; }
        public long? RemittanceItemID { get; set; }
        public string RemittanceNo { get; set; }
        public string PaymentType { get; set; }
        public string Currency { get; set; }
        public double? ForeignCurrencyAdvanced { get; set; }
        public double? ExchangeRate { get; set; }
        public double? ForeignCurrencyRemitted { get; set; }
        public double? RemittanceAmountTHB { get; set; }
        public double? RemittanceAmountMainCurrency {get;set;}
        
    }
}

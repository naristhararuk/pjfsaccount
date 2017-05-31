using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    [Serializable]
    public class VOInvoiceItem
    {
        public string ExpenseCode { get; set; }
        public string CostCenterCode { get; set; }
        public string IONumber { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public string CurrencySymbol { get; set; }
        public double CurrencyAmount { get; set; }
        public double ExchangeRate { get; set; }
        public string ReferenceNo { get; set; }
        public double LocalCurrencyAmount { get; set; }
        public double MainCurrencyAmount { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
	[Serializable]
	public class InvoiceExchangeRate
	{
		public double TotalAmount { get; set; }
		public double TotalAmountTHB { get; set; }
		public double AdvanceExchangeRateAmount { get; set; }
        public double TotalAmountMainCurrency { get; set; }
	}
}

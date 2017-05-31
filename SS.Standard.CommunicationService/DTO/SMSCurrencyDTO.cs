using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Standard.CommunicationService.DTO
{
    public class SMSCurrencyDTO
    {
        public string PaymentType { get; set; }
        public string Currency { get; set; }
        public string Amount { get; set; }
    }
}

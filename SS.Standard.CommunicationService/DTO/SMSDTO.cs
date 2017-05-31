using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Standard.CommunicationService.DTO
{
    public class SMSDTO
    {
        public string ReferenceID { get; set; }
        public string Requestor { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Amount { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Content { get; set; }
        public bool UseProxy { get; set; }
        public string DocumentNo { get; set; }
        public IList<SMSCurrencyDTO> CurrencyItemList { get; set; }

     }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
    public class CompanyPaymentMethodResult
    {
        public long CompanyPaymentMethodID { get; set; }
        public long CompanyID { get; set; }
        public string CompanyCode { get; set; }
        public bool Active { get; set; }

        public long PaymentMethodID { get; set; }
        public string PaymentMethodCode { get; set; }
        public string PaymentMethodName { get; set; }
    }
}

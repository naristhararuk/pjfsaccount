using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
    public class CompanyResult
    {
        public long CompanyID { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string PaymentType { get; set; }
        public bool Active { get; set; }

        public string PettyName { get; set; }
        public bool PettyActive { get; set; }

        public string TransferName { get; set; }
        public bool TransferActive { get; set; }

        public string ChequeName { get; set; }
        public bool ChequeActive { get; set; }

        public string StatusDesc { get; set; }
        public long DefaultTaxID { get; set; }
        public string DefaultTaxCode { get; set; }
    }
}

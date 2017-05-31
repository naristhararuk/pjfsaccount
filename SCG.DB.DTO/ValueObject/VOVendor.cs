using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
    public class VOVendor
    {
        public long? VendorID { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string Country { get; set; }
        public string VendorTaxCode { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string VendorBranch { get; set; }
        public bool ExcludeOneTime { get; set; }
    }
}

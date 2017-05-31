using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    public class TASearchCriteria
    {
        public long CompanyID { get; set; }
        public long DocumentTypeID { get; set; }
        public string RequestNo { get; set; }
        public string Subject { get; set; }
        public DateTime? RequestDateFrom { get; set; }
        public DateTime? RequestDateTo { get; set; }
        public DateTime? ApproveDateFrom { get; set; }
        public DateTime? ApproveDateTo { get; set; }
        public long CreatorID { get; set; }
        //public long RequesterID { get; set; }
        //public long TravellerID { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string TravellerNameTH { get; set; }
        public string TravellerNameEN { get; set; }
    }
}

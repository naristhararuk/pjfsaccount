using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    [Serializable]
    public class VODocumentFollowUpReport
    {
        public long DocumentID { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string Status { get; set; }
        public string RequestNo { get; set; }
        public string CreatorName { get; set; }
        public string RequesterName { get; set; }
        public string TelNo { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public double? Amount { get; set; }
        public DateTime? ResponseDate { get; set; }
        public int SendTime { get; set; }
        public long RequesterID { get; set; }
        public long CreatorID { get; set; }
        public long? CountDays { get; set; }

        public VODocumentFollowUpReport()
        {}
       
    }
}

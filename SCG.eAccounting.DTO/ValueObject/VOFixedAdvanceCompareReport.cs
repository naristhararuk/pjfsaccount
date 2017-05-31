using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    [Serializable]
    public class VOFixedAdvanceCompareReport
    {
        public long? DocumentID { get; set; }
        //For Criteria
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string RequesterID { get; set; }
        public string ApproverID { get; set; }
        public string ReportType { get; set; }
        public string DocumentNo { get; set; }
        public VOFixedAdvanceCompareReport()
        { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    [Serializable]
    public class VoTravelExpenseReport
    {
        public long? CompanyID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? FromTravelDate { get; set; }
        public DateTime? ToTravelDate { get; set; }
        public string FromTaDocumentNo { get; set; }
        public string ToTaDocumentNo { get; set; }
        public string FromTraveller { get; set; }
        public string ToTraveller { get; set; }
        public string TaStatus { get; set; }
        public string userName { get; set; }
        public string ShowParam1 { get; set; }
        public string ShowParam2 { get; set; }

        public VoTravelExpenseReport()
        {}
       
    }
}

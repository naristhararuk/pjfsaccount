using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    [Serializable]
    public class ReimbursementReportValueObj
    {
        public long? PBID { get; set; }
        public DateTime? RequestDateFrom { get; set; }
        public DateTime? RequestDateTo { get; set; }
        public DateTime? PaidDateFrom { get; set; }
        public DateTime? PaidDateTo { get; set; }
        public string RequestNoFrom { get; set; }
        public string RequestNoTo { get; set; }
        public bool? MarkDocument { get; set; }

        public long? DocumentID { get; set; }
        public long? WorkflowID { get; set; }
        public DateTime? RequestDate { get; set; }
        public string RequestNo { get; set; }
        public string Subject { get; set; }
        public double Amount { get; set; }
        public double AmountMainCurrency { get; set; }
        public double AmountTHB { get; set; }
        public string DocumentTypeName {get;set;}
        public string StateName {get;set;}
        public DateTime? PaidDate { get; set; }
        public string StatusEvent { get; set; }
        public string Currency { get; set; }
        public bool Mark { get; set; }
        public string FI_DOC { get; set; }
        public DateTime? MaxPaidDate { get; set; }
        public DateTime? MinPaidDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    public class TASearchResultData
    {
        public long DocumentID { get; set; }
        public long WorkflowID { get; set; }
        public string RequestNo { get; set; }
        public string Subject { get; set; }
        public string RequesterName { get; set; }
        public string CreatorName { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime ApproveDate { get; set; }

    }
}

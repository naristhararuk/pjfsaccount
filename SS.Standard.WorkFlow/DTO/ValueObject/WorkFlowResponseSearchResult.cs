using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Standard.WorkFlow.DTO.ValueObject
{
    public class WorkFlowResponseSearchResult
    {
        public long? DocumentID { get; set; }
        public string ResponseEventName { get; set; }
        public string EventName { get; set; }
        public string Remark { get; set; }
        public string ReasonName { get; set; }
        public string ResponseBy { get; set; }
        public string RemarkHold { get; set; }
        public string RemarkReject { get; set; }
    }
}

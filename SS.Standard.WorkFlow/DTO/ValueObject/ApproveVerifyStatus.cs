using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Standard.WorkFlow.DTO.ValueObject
{
    [Serializable]
    public class ApproveVerifyStatus
    {
        public long DocumentID { get; set; }
        public string DocumentNo { get; set; }
        public string Subject { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
        public List<string> Reason { get; set; }
    }
}

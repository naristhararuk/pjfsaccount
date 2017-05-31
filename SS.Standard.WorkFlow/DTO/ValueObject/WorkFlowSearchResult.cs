using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Standard.WorkFlow.DTO.ValueObject
{
    [Serializable]
    public class WorkFlowSearchResult
    {
        
        public long? WorkFlowID { get; set; }
        public long WorkFlowStateEventID { get; set; }
        public DateTime? ResponseDate { get; set; }
        //public string Date { get; set; }
        //public string Time { get; set; }
        public string Name { get; set; }
        public string Response { get; set; }
        public string Description { get; set; }
        public string ResponseMethod { get; set; }
        public double? AmountBeforeVerify { get; set; }
        public double? AmountVerified { get; set; }
    }
}

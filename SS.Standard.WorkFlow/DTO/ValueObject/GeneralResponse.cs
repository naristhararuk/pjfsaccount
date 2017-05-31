using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Standard.WorkFlow.DTO.ValueObject
{
    public class GeneralResponse
    {
        public int WorkFlowStateEventID { get; set; }
        public ResponseMethod ResponseMethod { get; set; }

        public GeneralResponse()
        { }

        public GeneralResponse(int workFlowStateEventID)
        {
            this.WorkFlowStateEventID = workFlowStateEventID;
        }
    }

    public enum ResponseMethod
    {
        Web = 0,
        Email = 1,
        SMS = 2,
        SAP = 3
    }
}

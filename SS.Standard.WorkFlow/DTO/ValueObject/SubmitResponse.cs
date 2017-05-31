using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Standard.WorkFlow.DTO.ValueObject
{
    public class SubmitResponse : GeneralResponse
    {
        public SubmitResponse(int workFlowStateEventID) 
        {
            this.WorkFlowStateEventID = workFlowStateEventID;
        }
    }

}

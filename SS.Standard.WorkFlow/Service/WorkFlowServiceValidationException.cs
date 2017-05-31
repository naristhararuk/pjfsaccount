using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Standard.WorkFlow.Service
{
    public class WorkFlowServiceValidationException : Exception
    {
        public Spring.Validation.ValidationErrors ValidationErrors;

        public WorkFlowServiceValidationException(Spring.Validation.ValidationErrors errors)
        {
            this.ValidationErrors = errors;
        }
    }
}

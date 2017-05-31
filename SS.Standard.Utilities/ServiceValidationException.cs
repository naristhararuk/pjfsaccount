using System;
using System.Collections.Generic;
using System.Text;

namespace SS.Standard.Utilities
{
    public class ServiceValidationException : Exception
    {
        public Spring.Validation.ValidationErrors ValidationErrors;

        public ServiceValidationException(Spring.Validation.ValidationErrors errors)
        {
            this.ValidationErrors = errors;
        }
    }
}

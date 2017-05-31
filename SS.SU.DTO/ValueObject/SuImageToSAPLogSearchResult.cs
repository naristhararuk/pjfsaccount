using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO.ValueObject
{
    public class SuImageToSAPLogSearchResult
    {   
        public string RequestNo { get; set; }
        public DateTime? SubmitDate { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }

        public SuImageToSAPLogSearchResult()
        { }
    }
}

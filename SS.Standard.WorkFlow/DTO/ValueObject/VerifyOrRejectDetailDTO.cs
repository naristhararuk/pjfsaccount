using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Standard.WorkFlow.DTO.ValueObject
{
    public class VerifyOrRejectDetailDTO
    {
        public string BranchID { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PostingDate { get; set; }
        public DateTime BaselineDate { get; set; }

        public VerifyOrRejectDetailDTO()
        {
        }
    }
}

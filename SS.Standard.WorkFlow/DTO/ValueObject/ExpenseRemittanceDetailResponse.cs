using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Standard.WorkFlow.DTO.ValueObject
{
    public class ExpenseRemittanceDetailResponse : GeneralResponse
    {
        public string GLAccount { get; set; }
        public DateTime? ValueDate { get; set; }
        public string ReceivedMethod { get; set; }
    }
}

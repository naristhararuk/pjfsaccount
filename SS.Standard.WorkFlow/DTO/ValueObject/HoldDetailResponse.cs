using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Standard.WorkFlow.DTO.ValueObject
{
    public class HoldDetailResponse : GeneralResponse
    {
        public List<object> HoldFields { get; set; }
        public string Remark { get; set; }

        public HoldDetailResponse()
        {
            HoldFields = new List<object>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Standard.WorkFlow.DTO.ValueObject
{
    public class HoldDetailDTO
    {
        public List<string> LeftIdentify { get; set; }
        public List<string> RightIdentify { get; set; }
        public string Remark { get; set; }

        public HoldDetailDTO()
        {
            LeftIdentify = new List<string>();
            RightIdentify = new List<string>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Standard.WorkFlow.DTO.ValueObject
{
    public class RejectDetailDTO
    {
        public int ReasonID { get; set;}
        public string Remark { get; set;}
        public bool NeedApproveRejection { get; set; }

        public RejectDetailDTO()
        {
        }
    }
}

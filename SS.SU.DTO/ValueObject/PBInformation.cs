using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO.ValueObject
{
    public class PBInformation
    {

        public long RolePbID { get; set; }
        public long PBID { get; set; }
       

        public string PBCode { get; set; }
        public string PBName { get; set; }
        public string Company { get; set; }
        public bool IsCheck { get; set; }
    }
}

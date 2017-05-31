using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    public class FixedAdvanceRefObjectValues
    {
        public Double NetAmount { get; set; }
        public long FixedAdvanceID { get; set; }
        public String DocNo { get; set; }
    }

    public class FixedAdvanceCanRefObjectValues
    {
        public Double NetAmount { get; set; }
        public long FixedAdvanceID { get; set; }
        public String DocNo { get; set; }
        public String RefCurrentStateName { get; set; }
    }
}

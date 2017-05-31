using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    [Serializable]
    public class ValidatePrediem
    {
        public DateTime ToDate { get; set; }
        public DateTime FromDate { get; set; }
        public string DocumentNo { get; set; }
    }
}

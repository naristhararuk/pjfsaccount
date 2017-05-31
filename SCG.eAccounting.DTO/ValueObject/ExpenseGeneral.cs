using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    public class ExpenseGeneral
    {
        public string CostCenter { get; set; }
        public string AccountCode { get; set; }
        public string InternalOrder { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string RefNo { get; set; }
    }
}

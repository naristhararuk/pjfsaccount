using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    public class RecommendExpenseData
    {
        public string CostCenterID { get; set; }
        public string ExpenseID { get; set; }
        public string InternalID { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string RefNo { get; set; }
    }
}

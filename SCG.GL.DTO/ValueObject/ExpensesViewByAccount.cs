using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.GL.DTO.ValueObject
{
    public class ExpensesViewByAccount
    {
        public string Seq { get; set; }
        public string AccountCode { get; set; }
        public string CostCenter { get; set; }
        public string InternalOrder { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
    }
}

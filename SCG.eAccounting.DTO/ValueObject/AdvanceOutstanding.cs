using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    [Serializable]
    public class AdvanceOutstanding
    {
        //public short No { get; set; }
        public long avDocumentID { get; set; }
        public long expenseDocumentID { get; set; }
        public string AdvanceNo { get; set; }
        public string Description { get; set; }
        public string AdvanceStatus { get; set; }
        public string ExpenseNo { get; set; }
        public string ExpenseStatus { get; set; }
        public DateTime? DueDate { get; set; }
        public double? Amount { get; set; }
        public AdvanceOutstanding()
        {}
       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    [Serializable]
    public class ExpensesMPA
    {
        public string DocumentNo { get; set; }
        public string Subject { get; set; }
        public DateTime DocumentDate { get; set; }
        public long? MPADocumentID { get; set; }
        public long? WorkflowID { get; set; }
    }
}

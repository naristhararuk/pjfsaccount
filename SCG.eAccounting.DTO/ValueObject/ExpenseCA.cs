using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    [Serializable]
    public class ExpenseCA
    {
        public string DocumentNo { get; set; }
        public string Subject { get; set; }
        public DateTime DocumentDate { get; set; }
        public long? CADocumentID { get; set; }
        public long? WorkflowID { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    [Serializable]
    public class VOExpenseStatementReport
    {
        public string FromExployeeCode { get; set; }
        public string ToEmployeeCode { get; set; }
        public DateTime? FromDueDate { get; set; }
        public DateTime? ToDueDate { get; set; }
        public string DocumentStatus { get; set; }
        public string ShowParam { get; set; }

        public VOExpenseStatementReport()
        {}
       
    }
}

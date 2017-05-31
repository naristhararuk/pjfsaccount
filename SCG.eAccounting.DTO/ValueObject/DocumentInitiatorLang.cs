using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    [Serializable]
    public class DocumentInitiatorLang
    {
        public long InitiatorID { get; set; }
        public long DocumentID { get; set; }
        public short Seq { get; set; }
        public long UserID { get; set; }
        public string InitiatorType { get; set; }
        public string FirstName { get; set; }
        public string EmployeeName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Boolean SMS { get; set; }
        public Boolean isSkip { get; set; }
        public string SkipReason { get; set; }
        public Boolean DoApprove { get; set; }
        public DocumentInitiatorLang()
        { }
       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    [Serializable]
    public class VOEmailLog
    {
        public long EMailLogID { get; set; }   
        public string RequestNo { get; set; }   
        public string EmailType { get; set; }   
        public DateTime? SendDate { get; set; }   
        public string ToEmail { get; set; }   
        public string CCEmail { get; set; }
        public int? Status { get; set; } 
        public string StatusName { get; set; }
        public string Remark { get; set; }

        public VOEmailLog()
        {
        }

    }
}

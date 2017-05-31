using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO.ValueObject
{
    public class SuSmsLogSearchResult
    {
        public DateTime? Date { get; set; }
        public string PhoneNo { get; set; }
        public string SendOrReceive { get; set; }
        public string Message { get; set; }
        public string TRANID { get; set; }
        public string SendMsgSMID { get; set; }

        public SuSmsLogSearchResult()
        { }
    }
}

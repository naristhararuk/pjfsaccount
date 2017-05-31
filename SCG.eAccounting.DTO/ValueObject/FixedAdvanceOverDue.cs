using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    public class FixedAdvanceOverDue
    {
        public long CacheWorkflowID { get; set; }
        public long RequesterID { get; set; }
    }
    /*เพิ่มแจ้งเตือนก่อนครบกำหนด fixedadvance*/
    public class FixedAdvanceBeforeDue
    {
        public long DocumentId { get; set; }
    }
}

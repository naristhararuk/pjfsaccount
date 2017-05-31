using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO.ValueObject
{
    public class SuUserLoginLog
    {
        public DateTime SignInDate { get; set; }
        public string UserName { get; set; }
        public long UserID { get; set; }
        public string Status { get; set; }
    }

   


}

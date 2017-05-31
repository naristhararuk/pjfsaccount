using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.CommunicationService.Implement;

namespace SS.Standard.CommunicationService.DTO
{
    [Serializable]
    public class SMSContainer
    {
        public SMS sms { get; set; }
        public long SMSLogid { get; set; }
        public bool NotifySMS { get; set; }
    }
}

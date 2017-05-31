using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.CommunicationService.DTO;
using SS.SU.DTO;
using SS.Standard.CommunicationService.Implement;
namespace SS.Standard.CommunicationService
{
   public interface ISMSService
    {
        bool Send(SMSDTO smsDTO);
        bool Send(SMSDTO smsDTO, long smsLogID);
        bool Send(SMSDTO smsDTO, bool NotifySMS);
        bool ReSend(SMS sms);
        bool ReSend(SMS sms, long smsLogID);
        bool ReSend(SMS sms, bool NotifySMS);
       bool UpdateDLVRREP(SuSmsLog smsLog);
    }
}

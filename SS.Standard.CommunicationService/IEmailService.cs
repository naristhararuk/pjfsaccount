using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.CommunicationService.Implement;
namespace SS.Standard.CommunicationService
{
    public interface IEmailService
    {
        void SetMailSender(string EmailAddress, string SenderName);
        void AddMailTO(string EmailAddress, string ReceiverName);
        void AddMailCC(string EmailAddress, string ReceiverName);
        void AddMailBCC(string EmailAddress, string ReceiverName);
        void ClearTo();
        void ClearCC();
        void ClearBCC();
        string MailSubject
        {
            get;
            set;
        }
        string MailBody
        {
            get;
            set;
        }
        string MailFileAttachmentFullPath
        {
            get;
            set;
        }
        Email.MailType MailSendingType
        {
            get;
            set;
        }
        bool IsMultipleReceiver
        {
            get;
            set;
        }
        bool SendEmail(DTO.EmailDTO email);
        bool SendEmail();

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.CommunicationService.Implement;
namespace SS.Standard.CommunicationService.DTO
{
    public class EmailDTO
    {
        public string MailSubject { get; set; }
        public string MailBody { get; set; }
        public List<AddMailSendTo> MailSendTo { get; set; }
        public List<AddMailSendToCC> MailSendToCC { get; set; }
        public List<AddMailSendToBCC> MailSendToBCC { get; set; }
        public AddSender Sender { get; set; }
        public Email.MailType MailSendingType { get; set; }
        public bool IsMultipleReceiver { get; set; }
        public string MailFileAttachmentFullPath { get; set; }

        public string RequesterNo { get; set; }
        public string EmailType { get; set; }
        public string ToEmail { get; set; }
        public string CCEmail { get; set; }
		public int? Status { get; set; }
        public string Remark { get; set; }

        public EmailDTO()
        {
            Sender = new AddSender();
            MailSendTo = new List<AddMailSendTo>();
            MailSendToCC = new List<AddMailSendToCC>();
            MailSendToBCC = new List<AddMailSendToBCC>();
        }
    }
    public class AddMailSendTo : MailSmartTag
    {

    }
    public class AddMailSendToCC : MailSmartTag
    {

    }
    public class AddMailSendToBCC : MailSmartTag
    {

    }
    public class AddSender : MailSmartTag
    {

    }
    public class MailSmartTag
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SS.Standard.Utilities
{
    public class EmailService
    {
        private SS.Standard.Utilities.Email email = null;

        #region SetMailSender
        public void SetMailSender(string EmailAddress, string SenderName)
        {
            mailSender.SenderAddress    = EmailAddress;
            mailSender.SenderName       = SenderName;
        }
        private SS.Standard.Utilities.Email.MailSender mailSender = new Email.MailSender();
        #endregion SetMailSender

        #region AddMailReceiver
        public void AddMailReceiver(string EmailAddress, string ReceiverName)
        {
            mailReceiver.ReceiverAddress.Add(EmailAddress);
            mailReceiver.ReceiverName.Add(ReceiverName);
        }
        private SS.Standard.Utilities.Email.MailReceiver mailReceiver = new Email.MailReceiver();
        #endregion AddMailReceiver

        #region Property
        private string MailSMTPServer { get; set; }
        private int MailSMTPPort { get; set; }
        private string MailSMTPUserName { get; set; }
        private string MailSMTPPassword { get; set; }

        public string MailSubject { get; set; }
        public string MailBody { get; set; }
        public string MailFileAttachmentFullPath { get; set; }
        public SS.Standard.Utilities.Email.MailType MailSendingType { get; set; }
        #endregion Property

        #region Constructor

        #region public EmailService()
        public EmailService()
        {
            email           = new Email();
            mailSender      = new Email.MailSender();
            mailReceiver.ReceiverAddress = new System.Collections.ArrayList();
            mailReceiver.ReceiverName = new System.Collections.ArrayList();

            MailSMTPServer      = ConfigurationManager.AppSettings["MailSMTPServer"].ToString();
            MailSMTPPort        = int.Parse(ConfigurationManager.AppSettings["MailSMTPPort"].ToString());
            MailSMTPUserName    = ConfigurationManager.AppSettings["MailSMTPUserName"].ToString();
            MailSMTPPassword    = ConfigurationManager.AppSettings["MailSMTPPassword"].ToString();

            MailSubject                 = "";
            MailBody                    = "";
            MailFileAttachmentFullPath  = "";
            MailSendingType = SS.Standard.Utilities.Email.MailType.TEXT;
        }
        #endregion public EmailService()

        #region public EmailService(string MailSMTPServer, int MailSMTPPort, string MailSMTPUserName, string MailSMTPPassword)
        public EmailService(string MailSMTPServer, int MailSMTPPort, string MailSMTPUserName, string MailSMTPPassword) : this()
        {
            this.MailSMTPServer     = MailSMTPServer;
            this.MailSMTPPort       = MailSMTPPort;
            this.MailSMTPUserName   = MailSMTPUserName;
            this.MailSMTPPassword   = MailSMTPPassword;
        }
        #endregion public EmailService(string MailSMTPServer, int MailSMTPPort, string MailSMTPUserName, string MailSMTPPassword)

        #region public EmailService(string MailSubject, string MailBody, string MailFileAttachmentFullPath, string MailSender, string MailReceiver, SS.Standard.Utilities.Email.MailType MailSendingType)
        public EmailService(string MailSubject, string MailBody, string MailFileAttachmentFullPath, string MailSenderName, string MailSenderAddress, string MailReceiverName, string MailReceiverAddress, SS.Standard.Utilities.Email.MailType MailSendingType)
            : this()
        {
            this.MailSubject        = MailSubject;
            this.MailBody           = MailBody;
            this.MailFileAttachmentFullPath = MailFileAttachmentFullPath;
            this.MailSendingType    = MailSendingType;

            this.SetMailSender(MailSenderAddress, MailSenderName);
            this.AddMailReceiver(MailReceiverAddress, MailReceiverName);
        }
        #endregion public EmailService(string MailSubject, string MailBody, string MailFileAttachmentFullPath, string MailSender, string MailReceiver, SS.Standard.Utilities.Email.MailType MailSendingType)

        #region public EmailService(string MailSMTPServer, int MailSMTPPort, string MailSMTPUserName, string MailSMTPPassword, string MailSubject, string MailBody, string MailFileAttachmentFullPath, string MailSender, string MailReceiver, SS.Standard.Utilities.Email.MailType MailSendingType)
        public EmailService(string MailSMTPServer, int MailSMTPPort, string MailSMTPUserName, string MailSMTPPassword, string MailSubject, string MailBody, string MailFileAttachmentFullPath, string MailSenderName, string MailSenderAddress, string MailReceiverName, string MailReceiverAddress, SS.Standard.Utilities.Email.MailType MailSendingType)
            : this()
        {
            this.MailSMTPServer = MailSMTPServer;
            this.MailSMTPPort   = MailSMTPPort;
            this.MailSMTPUserName = MailSMTPUserName;
            this.MailSMTPPassword = MailSMTPPassword;

            this.MailSubject = MailSubject;
            this.MailBody = MailBody;
            this.MailFileAttachmentFullPath = MailFileAttachmentFullPath;
            this.MailSendingType = MailSendingType;

            this.SetMailSender(MailSenderAddress, MailSenderName);
            this.AddMailReceiver(MailReceiverAddress, MailReceiverName);
        }
        #endregion public EmailService(string MailSMTPServer, int MailSMTPPort, string MailSMTPUserName, string MailSMTPPassword, string MailSubject, string MailBody, string MailFileAttachmentFullPath, string MailSender, string MailReceiver, SS.Standard.Utilities.Email.MailType MailSendingType)

        #endregion Constructor


        #region public bool SendEmail()
        public bool SendEmail()
        {
            try
            {
                email.mailSMTPServer    = MailSMTPServer;
                email.mailSMTPPort      = MailSMTPPort;
                email.mailSMTPUserName  = MailSMTPUserName;
                email.mailSMTPPassword  = MailSMTPPassword;

                email.mailSubject                   = MailSubject;
                email.mailBody                      = MailBody;
                email.mailFileAttachmentFullPath    = MailFileAttachmentFullPath;

                email.mailSender    = mailSender;
                email.mailReceiver  = mailReceiver;

                email.SendingType                   = MailSendingType;
                email.send();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion public bool SendEmail()

        #region private void ResetEmail()
        private void ResetEmail()
        {
            email = new Email();
            mailSender = new Email.MailSender();
            mailReceiver.ReceiverAddress = new System.Collections.ArrayList();
            mailReceiver.ReceiverName = new System.Collections.ArrayList();

            MailSubject = "";
            MailBody = "";
            MailFileAttachmentFullPath = "";
            MailSendingType = SS.Standard.Utilities.Email.MailType.TEXT;
        }
        #endregion private void ResetEmail()
    }
}

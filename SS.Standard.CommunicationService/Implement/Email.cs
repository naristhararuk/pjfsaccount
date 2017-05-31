using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Net.Mail;
using System.Net;

namespace SS.Standard.CommunicationService.Implement
{
    public class Email : IEmail
    {
        #region Design Variable
        private DateTime sendDateTime = DateTime.Now;

        private const string logFolder = @"C:\EmailLog\";
        private const string UnlockComponent = "APDESHMAILQ_9wwhaBWm8NpV";

        // Config for User Server
        public int mailSMTPPort { get; set; }
        public string mailSMTPServer { get; set; }
        public string mailSMTPUserName { get; set; }
        public string mailSMTPPassword { get; set; }

        // For Email
        public bool IsMultipleReceiver { get; set; }
        public string mailSubject { get; set; }
        public string mailBody { get; set; }
        public string mailFileAttachmentFullPath { get; set; }

        public MailType SendingType { get; set; }
        public MailSender mailSender = new MailSender();
        public MailReceiver mailReceiverTo = new MailReceiver();
        public MailReceiver mailReceiverBCC = new MailReceiver();
        public MailReceiver mailReceiverCC = new MailReceiver();

        // Control
        private MailMessage email;
        //private MailMessage emailForEML;
        private SmtpClient mailman;
        #endregion Design Variable

        #region Enum And Struct

        #region public enum MailType
        public enum MailType
        {
            HTML,
            TEXT
        }
        #endregion public enum MailType

        #region public struct MailSender
        public struct MailSender
        {
            public string SenderName;
            public string SenderAddress;
        }
        #endregion public struct MailSender

        #region public struct MailReceiver
        public struct MailReceiver
        {
            public ArrayList ReceiverName;
            public ArrayList ReceiverAddress;
        }
        #endregion public struct MailReceiver

        #endregion Enum And Struct

        #region Function For Set For Send User

        #region public void SetMailSender(string EmailAddress, string SenderName)
        public void SetMailSender(string EmailAddress, string SenderName)
        {
            mailSender.SenderAddress = EmailAddress;
            mailSender.SenderName = SenderName;
        }
        #endregion public void SetMailSender(string EmailAddress, string SenderName)

        #region public void AddMailTO(string EmailAddress, string ReceiverName)
        public void AddMailTO(string EmailAddress, string ReceiverName)
        {
            mailReceiverCC.ReceiverAddress.Add(EmailAddress);
            mailReceiverCC.ReceiverName.Add(ReceiverName);
        }
        #endregion public void AddMailTO(string EmailAddress, string ReceiverName)

        #region public void AddMailCC(string EmailAddress, string ReceiverName)
        public void AddMailCC(string EmailAddress, string ReceiverName)
        {
            mailReceiverCC.ReceiverAddress.Add(EmailAddress);
            mailReceiverCC.ReceiverName.Add(ReceiverName);
        }
        #endregion public void AddMailCC(string EmailAddress, string ReceiverName)

        #region public void AddMailBCC(string EmailAddress, string ReceiverName)
        public void AddMailBCC(string EmailAddress, string ReceiverName)
        {
            mailReceiverBCC.ReceiverAddress.Add(EmailAddress);
            mailReceiverBCC.ReceiverName.Add(ReceiverName);
        }
        #endregion public void AddMailBCC(string EmailAddress, string ReceiverName)

        #endregion Function For Set For Send User

        #region public Email()
        public Email()
        {
            mailSender = new MailSender();

            mailReceiverTo.ReceiverAddress = new ArrayList();
            mailReceiverTo.ReceiverName = new ArrayList();

            mailReceiverCC.ReceiverAddress = new ArrayList();
            mailReceiverCC.ReceiverName = new ArrayList();

            mailReceiverBCC.ReceiverAddress = new ArrayList();
            mailReceiverBCC.ReceiverName = new ArrayList();

            email = new MailMessage();
            //emailForEML = new MailMessage();
            mailman = new SmtpClient();
            IsMultipleReceiver = true;
        }
        #endregion public Email()

        #region public bool send()
        public bool Send()
        {
            try
            {
                #region  Set Value For 'Chilkat Mail Component'
                //mailman.UnlockComponent(UnlockComponent);
                //mailman.SmtpHost = mailSMTPServer;
                //mailman.SmtpPort = mailSMTPPort;
                //mailman.SmtpUsername = mailSMTPUserName;
                //mailman.SmtpPassword = mailSMTPPassword;
                mailman.Host = mailSMTPServer;
                mailman.Port = mailSMTPPort;
                mailman.Credentials = new NetworkCredential(mailSMTPUserName, mailSMTPPassword);
                #endregion Set Value For 'Chilkat Mail Component'

                #region prepare mail body
                email.From = new MailAddress(mailSender.SenderAddress, mailSender.SenderName);
                email.Subject = mailSubject;

                if (SendingType.Equals(MailType.HTML))
                {
                    email.IsBodyHtml = true;
                }
                else if (SendingType.Equals(MailType.TEXT))
                {
                    email.IsBodyHtml = false;
                }

                email.Body = mailBody;

                if (!string.IsNullOrEmpty(mailFileAttachmentFullPath))
                {
                    email.Attachments.Add(new Attachment(mailFileAttachmentFullPath));
                }

                #endregion prepare mail body

                #region Set Certification
                //string log = tmpEmlFolder + @"\CertLog";
                //if (null != cert)
                //{
                //    email.SetSigningCert(cert);
                //    email.SendSigned = true;
                //}
                //else
                //{
                //    certStore.SaveLastError(log + @"\ErrorLogCertIsNull.xml");
                //}
                #endregion Set Certification

                #region Sending email
                if (IsMultipleReceiver)
                {
                    #region Multiple
                    for (int i = 0; i < mailReceiverCC.ReceiverAddress.Count; i++)
                        email.CC.Add(new MailAddress(mailReceiverCC.ReceiverAddress[i].ToString(), mailReceiverCC.ReceiverName[i].ToString()));//email.AddCC(mailReceiverCC.ReceiverName[i].ToString(), mailReceiverCC.ReceiverAddress[i].ToString());
                    for (int i = 0; i < mailReceiverBCC.ReceiverAddress.Count; i++)
                        email.Bcc.Add(new MailAddress(mailReceiverBCC.ReceiverAddress[i].ToString(), mailReceiverBCC.ReceiverName[i].ToString()));//email.AddBcc(mailReceiverBCC.ReceiverName[i].ToString(), mailReceiverBCC.ReceiverAddress[i].ToString());
                    for (int i = 0; i < mailReceiverTo.ReceiverAddress.Count; i++)
                        email.To.Add(new MailAddress(mailReceiverTo.ReceiverAddress[i].ToString(), mailReceiverTo.ReceiverName[i].ToString()));//email.AddTo(mailReceiverTo.ReceiverName[i].ToString(), mailReceiverTo.ReceiverAddress[i].ToString());
                    
                    mailman.Send(email);
                    
                    email.To.Clear();
                    email.CC.Clear();
                    email.Bcc.Clear();
                    #endregion Multiple
                }
                else
                {
                    #region Single
                    for (int i = 0; i < mailReceiverCC.ReceiverAddress.Count; i++)
                        email.CC.Add(new MailAddress(mailReceiverCC.ReceiverAddress[i].ToString(), mailReceiverCC.ReceiverName[i].ToString()));//email.AddCC(mailReceiverCC.ReceiverName[i].ToString(), mailReceiverCC.ReceiverAddress[i].ToString());
                    for (int i = 0; i < mailReceiverBCC.ReceiverAddress.Count; i++)
                        email.Bcc.Add(new MailAddress(mailReceiverBCC.ReceiverAddress[i].ToString(), mailReceiverBCC.ReceiverName[i].ToString()));//email.AddBcc(mailReceiverBCC.ReceiverName[i].ToString(), mailReceiverBCC.ReceiverAddress[i].ToString());

                    for (int i = 0; i < mailReceiverTo.ReceiverAddress.Count; i++)
                    {
                        email.To.Add(new MailAddress(mailReceiverTo.ReceiverAddress[i].ToString(), mailReceiverTo.ReceiverName[i].ToString()));//email.AddTo(mailReceiverTo.ReceiverName[i].ToString(), mailReceiverTo.ReceiverAddress[i].ToString());
                        
                        mailman.Send(email);
                    }

                    email.To.Clear();
                    email.CC.Clear();
                    email.Bcc.Clear();
                    #endregion Single
                }
                #endregion Sending email
               
                #region Dispose email object
                email.Dispose();
                
                #endregion Dispose email object

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion public bool send()

        #region ClearMail
        public void ClearTo()
        {
            mailReceiverTo.ReceiverAddress = new ArrayList();
            mailReceiverTo.ReceiverName = new ArrayList();
        }

        public void ClearCC()
        {
            mailReceiverCC.ReceiverAddress = new ArrayList();
            mailReceiverCC.ReceiverName = new ArrayList();
        }

        public void ClearBCC()
        {
            mailReceiverBCC.ReceiverAddress = new ArrayList();
            mailReceiverBCC.ReceiverName = new ArrayList();
        }
        #endregion ClearMail

        #region ResetEmail
        public void ResetEmail()
        {
            mailSender = new MailSender();

            mailReceiverTo.ReceiverAddress = new ArrayList();
            mailReceiverTo.ReceiverName = new ArrayList();

            mailReceiverCC.ReceiverAddress = new ArrayList();
            mailReceiverCC.ReceiverName = new ArrayList();

            mailReceiverBCC.ReceiverAddress = new ArrayList();
            mailReceiverBCC.ReceiverName = new ArrayList();

            email = new MailMessage();
            mailman = new SmtpClient();
        }
        #endregion ResetEmail


    }
}

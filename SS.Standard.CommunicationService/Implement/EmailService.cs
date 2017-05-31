using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections;
using SS.SU.DTO;
using SS.SU.Query;

using SS.DB.Query;
using SS.Standard.CommunicationService.DTO;
using SS.Standard.Utilities;

namespace SS.Standard.CommunicationService.Implement
{
    public class EmailService : IEmailService
    {
        #region variables
        private string strMailSMTPServer = ParameterServices.SMTPMailServer;
        private string intMailSMTPPort = ParameterServices.SMTPPort;
        private string strMailSMTPUserName = ParameterServices.SMTPMailUser; 
        private string strMailSMTPPassword = ParameterServices.SMTPMailPassword;
        private string strAdminAddress = ParameterServices.AdminEmailAddress; 
        private string strAdminName = ParameterServices.AdminName;


        #endregion


        #region Design Variable
        private Email email = null;
        private Email.MailSender mailSender = new Email.MailSender();
        private Email.MailReceiver mailReceiverTO = new Email.MailReceiver();
        private Email.MailReceiver mailReceiverCC = new Email.MailReceiver();
        private Email.MailReceiver mailReceiverBCC = new Email.MailReceiver();
        #endregion Design Variable

        #region SetMailSender
        /// <summary>
        /// ทำการกำหนดชื่อของผู้ที่ทำการส่งเมลล์
        /// </summary>
        /// <param name="EmailAddress">อีเมลล์</param>
        /// <param name="SenderName">ชื่อผู้ส่ง</param>
        public void SetMailSender(string EmailAddress, string SenderName)
        {
            mailSender.SenderAddress = EmailAddress;
            mailSender.SenderName = SenderName;
        }
        #endregion SetMailSender

        #region <== AddMail ==>

        #region AddMailTO
        /// <summary>
        /// ทำการเพิ่มรายชื่อผู้รับ
        /// </summary>
        /// <param name="EmailAddress">อีเมลล์</param>
        /// <param name="ReceiverName">ชื่อผู้รับ</param>
        public void AddMailTO(string EmailAddress, string ReceiverName)
        {
            mailReceiverTO.ReceiverAddress.Add(EmailAddress);
            mailReceiverTO.ReceiverName.Add(ReceiverName);
        }
        #endregion AddMailTO

        #region AddMailCC
        /// <summary>
        /// ทำการเพิ่มชื่อผู้รับ 
        /// ทำสำเนาเมลล์ส่งถึงผู้รับ
        /// </summary>
        /// <param name="EmailAddress">อีเมลล์</param>
        /// <param name="ReceiverName">ชื่อผู้รับ</param>
        public void AddMailCC(string EmailAddress, string ReceiverName)
        {
            mailReceiverCC.ReceiverAddress.Add(EmailAddress);
            mailReceiverCC.ReceiverName.Add(ReceiverName);
        }
        #endregion AddMailCC

        #region AddMailBCC
        /// <summary>
        /// ทำการเพิ่มชื่อผู้รับ 
        /// ทำสำเนาลับเมลล์ส่งถึงผู้รับ
        /// </summary>
        /// <param name="EmailAddress">อีเมลล์</param>
        /// <param name="ReceiverName">ชื่อผู้รับ</param>
        public void AddMailBCC(string EmailAddress, string ReceiverName)
        {
            mailReceiverBCC.ReceiverAddress.Add(EmailAddress);
            mailReceiverBCC.ReceiverName.Add(ReceiverName);
        }
        #endregion AddMailBCC

        #endregion <== AddMail ==>

        #region <== ClearMail ==>

        #region ClearTo
        /// <summary>
        /// ทำการลบรายชื่อผู้รับ
        /// </summary>
        public void ClearTo()
        {
            mailReceiverTO.ReceiverAddress = new ArrayList();
            mailReceiverTO.ReceiverName = new ArrayList();
        }
        #endregion ClearTo

        #region ClearCC
        /// <summary>
        /// ทำการลบรายชื่อผู้รับที่ส่งสำเนาเมลล์ถึง
        /// </summary>
        public void ClearCC()
        {
            mailReceiverCC.ReceiverAddress = new ArrayList();
            mailReceiverCC.ReceiverName = new ArrayList();
        }
        #endregion ClearCC

        #region ClearBCC
        /// <summary>
        /// ทำการลบรายชื่อผู้รับที่ส่งสำเนาลับเมลล์ถึง
        /// </summary>
        public void ClearBCC()
        {
            mailReceiverBCC.ReceiverAddress = new ArrayList();
            mailReceiverBCC.ReceiverName = new ArrayList();
        }
        #endregion ClearBCC

        #endregion <== ClearMail ==>

        #region private void ResetEmail()
        /// <summary>
        /// ทำการกำหนดกางส่งเมลล์ให้ทั้งหมด
        /// ลบรายชื่อผู้ส่งรายชื่อผู้รับ
        /// ลบรายละเอียดเมลล์ทั้งหมด
        /// </summary>
        private void ResetEmail()
        {
            email = new Email();

            mailSender = new Email.MailSender();

            mailReceiverTO.ReceiverAddress = new ArrayList();
            mailReceiverTO.ReceiverName = new ArrayList();

            mailReceiverCC.ReceiverAddress = new ArrayList();
            mailReceiverCC.ReceiverName = new ArrayList();

            mailReceiverBCC.ReceiverAddress = new ArrayList();
            mailReceiverBCC.ReceiverName = new ArrayList();

            MailSubject = "";
            MailBody = "";
            MailFileAttachmentFullPath = "";
        }
        #endregion private void ResetEmail()

        #region Property
        private int MailSMTPPort { get; set; }
        private string MailSMTPServer { get; set; }
        private string MailSMTPUserName { get; set; }
        private string MailSMTPPassword { get; set; }

        /// <summary>
        /// หัวข้ออีเมลล์
        /// </summary>
        public string MailSubject { get; set; }
        /// <summary>
        /// รายละเอียดอีเมลล์
        /// </summary>
        public string MailBody { get; set; }
        /// <summary>
        /// Path ที่ทำการแนบไฟล์
        /// </summary>
        public string MailFileAttachmentFullPath { get; set; }
        /// <summary>
        /// ประเภทการส่งเมลล์
        /// TEXT หรือ HTML
        /// </summary>
        public Email.MailType MailSendingType { get; set; }
        /// <summary>
        /// การส่งเป็นแบบการแสดงชื่อผู้รับหลายคนหรือไม่
        /// TRUE : ผู้รับหลายคน จะแสดงชื่อผู้รับทั้งหมด
        /// เช่น nickyway@hotmail.com,nickyway@yahoo.com,nickyway@gmail.com
        /// FALSE : ผู้รับคนเด๋ว จะแสดงชื่อผู้รับเพียงทีละหนึ่งคน
        /// เช่น nickyway@hotmail.com
        /// </summary>
        public bool IsMultipleReceiver { get; set; }
        #endregion Property

        #region Constructor

        #region public EmailService()
        /// <summary>
        /// ทำการกำหนดค่าตาม Default
        /// </summary>
        public EmailService()
        {
            string strMailSMTPServer = ParameterServices.SMTPMailServer;
            string intMailSMTPPort = ParameterServices.SMTPPort;
            string strMailSMTPUserName = ParameterServices.SMTPMailUser;
            string strMailSMTPPassword = ParameterServices.SMTPMailPassword;
            string strAdminAddress = ParameterServices.AdminEmailAddress; 
            string strAdminName = ParameterServices.AdminName;

            email = new Email();

            mailSender = new Email.MailSender();

            mailReceiverTO.ReceiverAddress = new ArrayList();
            mailReceiverTO.ReceiverName = new ArrayList();

            mailReceiverCC.ReceiverAddress = new ArrayList();
            mailReceiverCC.ReceiverName = new ArrayList();

            mailReceiverBCC.ReceiverAddress = new ArrayList();
            mailReceiverBCC.ReceiverName = new ArrayList();

            MailSMTPServer = strMailSMTPServer;
            MailSMTPPort = int.Parse(intMailSMTPPort);
            MailSMTPUserName = strMailSMTPUserName;
            MailSMTPPassword = strMailSMTPPassword;

            SetMailSender(strAdminAddress, strAdminName);

            MailSubject = "";
            MailBody = "";
            MailFileAttachmentFullPath = "";

            MailSendingType = Email.MailType.TEXT;
            IsMultipleReceiver = true;
        }
        #endregion public EmailService()

        #region string MailSMTPUserName, string MailSMTPPassword, string SenderAddress, string SenderName)
        /// <summary>
        /// ทำการกำหนด User และ Password สำหรับการส่งเมลล์
        /// พร้อมกำหนดอีเมลล์และชื่อของผู้ส่ง
        /// </summary>
        /// <param name="MailSMTPUserName">MailSMTPUserName</param>
        /// <param name="MailSMTPPassword">MailSMTPPassword</param>
        /// <param name="SenderAddress">อีเมลล์ผู้ส่ง</param>
        /// <param name="SenderName">ชื่อผู้ส่ง</param>
        public EmailService(string MailSMTPUserName, string MailSMTPPassword, string SenderAddress, string SenderName)
            : this()
        {
            this.MailSMTPUserName = MailSMTPUserName;
            this.MailSMTPPassword = MailSMTPPassword;

            this.SetMailSender(SenderAddress, SenderName);
        }
        #endregion string MailSMTPUserName, string MailSMTPPassword, string SenderAddress, string SenderName)

        #region public EmailService(string MailSMTPServer, int MailSMTPPort, string MailSMTPUserName, string MailSMTPPassword)
        /// <summary>
        /// ทำการกำหนดค่า Mail Server เอง
        /// </summary>
        /// <param name="MailSMTPServer">MailSMTPServer</param>
        /// <param name="MailSMTPPort">MailSMTPPort</param>
        /// <param name="MailSMTPUserName">MailSMTPUserName</param>
        /// <param name="MailSMTPPassword">MailSMTPPassword</param>
        public EmailService(string MailSMTPServer, int MailSMTPPort, string MailSMTPUserName, string MailSMTPPassword)
            : this()
        {
            this.MailSMTPServer = MailSMTPServer;
            this.MailSMTPPort = MailSMTPPort;
            this.MailSMTPUserName = MailSMTPUserName;
            this.MailSMTPPassword = MailSMTPPassword;
        }
        #endregion public EmailService(string MailSMTPServer, int MailSMTPPort, string MailSMTPUserName, string MailSMTPPassword)

        #region public EmailService(string MailSMTPUserName, string MailSMTPPassword)
        /// <summary>
        /// ทำการกำหนดค่า User Mail Server เอง
        /// </summary>
        /// <param name="MailSMTPUserName">MailSMTPUserName</param>
        /// <param name="MailSMTPPassword">MailSMTPPassword</param>
        public EmailService(string MailSMTPUserName, string MailSMTPPassword)
            : this()
        {
            this.MailSMTPUserName = MailSMTPUserName;
            this.MailSMTPPassword = MailSMTPPassword;
        }
        #endregion public EmailService(string MailSMTPUserName, string MailSMTPPassword)

        #region public EmailService(string MailSMTPServer, int MailSMTPPort, string MailSMTPUserName, string MailSMTPPassword,string SenderAddress,string SenderName)
        /// <summary>
        /// ทำการกำหนดค่า Mail Server เอง
        /// กำหนดอีเมลล์และชื่อ ของผู้ส่ง
        /// </summary>
        /// <param name="MailSMTPServer">MailSMTPServer</param>
        /// <param name="MailSMTPPort">MailSMTPPort</param>
        /// <param name="MailSMTPUserName">MailSMTPUserName</param>
        /// <param name="MailSMTPPassword">MailSMTPPassword</param>
        /// <param name="SenderAddress">อีเมลล์ผู้ส่ง</param>
        /// <param name="SenderName">ชื่อผู้ส่ง</param>
        public EmailService(string MailSMTPServer, int MailSMTPPort, string MailSMTPUserName, string MailSMTPPassword, string SenderAddress, string SenderName)
            : this()
        {
            this.MailSMTPServer = MailSMTPServer;
            this.MailSMTPPort = MailSMTPPort;
            this.MailSMTPUserName = MailSMTPUserName;
            this.MailSMTPPassword = MailSMTPPassword;

            this.SetMailSender(SenderAddress, SenderName);
        }
        #endregion public EmailService(string MailSMTPServer, int MailSMTPPort, string MailSMTPUserName, string MailSMTPPassword,string SenderAddress,string SenderName)

        #endregion Constructor

        #region public bool SendEmail()
        /// <summary>
        /// ทำการส่งอีเมลล์
        /// </summary>
        /// <returns></returns>
        public bool SendEmail()
        {
            try
            {
                email.IsMultipleReceiver = IsMultipleReceiver;

                email.mailSMTPServer = ParameterServices.SMTPMailServer;
                email.mailSMTPPort = int.Parse(ParameterServices.SMTPPort);
                email.mailSMTPUserName = ParameterServices.SMTPMailUser;
                email.mailSMTPPassword = ParameterServices.SMTPMailPassword;

                email.mailSubject = MailSubject;
                email.mailBody = MailBody;
                email.mailFileAttachmentFullPath = MailFileAttachmentFullPath;

                email.mailSender = mailSender;
                email.mailReceiverTo = mailReceiverTO;
                email.mailReceiverCC = mailReceiverCC;
                email.mailReceiverBCC = mailReceiverBCC;

                email.SendingType = MailSendingType;

                email.Send();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion public bool SendEmail()

        #region IEmailService Members

        private bool ValidateEmailFormat(string emailAddress)
        {
            if (emailAddress.Contains('@'))
            {
                if (emailAddress.Contains('.'))
                {
                    return true;
                }
            }
            return false;
        }


        public bool SendEmail(EmailDTO emailParam)
        {
            try
            {
                email = new Email();

                mailSender = new Email.MailSender();

                mailReceiverTO.ReceiverAddress = new ArrayList();
                mailReceiverTO.ReceiverName = new ArrayList();

                mailReceiverCC.ReceiverAddress = new ArrayList();
                mailReceiverCC.ReceiverName = new ArrayList();

                mailReceiverBCC.ReceiverAddress = new ArrayList();
                mailReceiverBCC.ReceiverName = new ArrayList();

                SetMailSender(string.IsNullOrEmpty(emailParam.Sender.Email) ? strAdminAddress : emailParam.Sender.Email, string.IsNullOrEmpty(emailParam.Sender.Name) ? strAdminName : emailParam.Sender.Name);

                if (emailParam.MailSendTo != null && emailParam.MailSendTo.Count > 0)
                {
                    foreach (AddMailSendTo itemSendTo in emailParam.MailSendTo)
                    {
                        AddMailTO(itemSendTo.Email, itemSendTo.Name);
                    }
                }

                if (emailParam.MailSendToCC != null && emailParam.MailSendToCC.Count > 0)
                {
                    foreach (AddMailSendToCC itemSendToCC in emailParam.MailSendToCC)
                    {
                        AddMailCC(itemSendToCC.Email, itemSendToCC.Name);
                    }
                }

                if (emailParam.MailSendToBCC != null && emailParam.MailSendToBCC.Count > 0)
                {
                    foreach (AddMailSendToBCC itemSendToBCC in emailParam.MailSendToBCC)
                    {
                        AddMailBCC(itemSendToBCC.Email, itemSendToBCC.Name);
                    }
                }


                email.IsMultipleReceiver = emailParam.IsMultipleReceiver;

                email.mailSMTPServer = ParameterServices.SMTPMailServer; //strMailSMTPServer;
                email.mailSMTPPort = int.Parse(ParameterServices.SMTPPort); //int.Parse(intMailSMTPPort);
                email.mailSMTPUserName = ParameterServices.SMTPMailUser;  //strMailSMTPUserName;
                email.mailSMTPPassword = ParameterServices.SMTPMailPassword; //strMailSMTPPassword;

                email.mailSubject = emailParam.MailSubject;
                email.mailBody = emailParam.MailBody;
                email.mailFileAttachmentFullPath = emailParam.MailFileAttachmentFullPath;

                email.mailSender = mailSender;
                email.mailReceiverTo = mailReceiverTO;
                email.mailReceiverCC = mailReceiverCC;
                email.mailReceiverBCC = mailReceiverBCC;

                email.SendingType = emailParam.MailSendingType;

                return  email.Send();

                 
            }
            catch (Exception)
            {
                Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("Email service was failure."));
                throw new ServiceValidationException(errors);
                
            }
        }

        #endregion
    }
}

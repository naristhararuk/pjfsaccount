using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.CommunicationService.Implement;

namespace SS.Standard.CommunicationService
{
    public interface IEmail
    {

        // Config for Server
        #region Config for Mail Server
        int mailSMTPPort
        { 
            get; 
            set; 
        }
        string mailSMTPServer
        {
            get; 
            set; 
        }
        string mailSMTPUserName
        { 
            get; 
            set; 
        }
        string mailSMTPPassword
        { 
            get; 
            set;
        }
        #endregion 


        // For Email
        #region For Email Body
        bool IsMultipleReceiver
        { 
            get; 
            set; 
        }
        string mailSubject
        { 
            get; 
            set; 
        }
        string mailBody
        { 
            get; 
            set; 
        }
        string mailFileAttachmentFullPath
        { 
            get; 
            set; 
        }

        Email.MailType SendingType
        { 
            get; 
            set; 
        }
        #endregion 

       void SetMailSender(string EmailAddress, string SenderName);
       void AddMailTO(string EmailAddress, string ReceiverName);
       void AddMailCC(string EmailAddress, string ReceiverName);
       void AddMailBCC(string EmailAddress, string ReceiverName);
       bool Send();
       void ClearTo();
       void ClearCC();
       void ClearBCC();
       void ResetEmail();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Security;
using SCG.eAccounting.BLL;
using SCG.eAccounting.Interface.Utilities;
using SS.SU.Query;
using SS.Standard.CommunicationService;
using SS.Standard.CommunicationService.Implement;
using SCG.eAccounting.DAL;
using SS.SU.BLL;

namespace SCG.eAccounting.Resender.DAL
{
    public class Factory
    {
        public Factory()
        {

        }
        public static IUserAccount UserAccount { get; set; }
        public static IEmailService EmailService { get; set; }
        public static ISCGEmailService SCGEmailService { get; set; }
        public static ISuEmailResendingQuery SuEmailResendingQuery { get; set; }
        public static ISMSService SMSService { get; set; }
        public static ISuEmailLogDao SuEmailLogDao { get; set; }
        public static ISuEmailResendingService SuEmailResendingService { get; set; }

        public static void CreateObject()
        {
            UserAccount = (IUserAccount)ObjectManager.GetObject("UserAccount");
            SCGEmailService = (ISCGEmailService)ObjectManager.GetObject("SCGEmailService");
            SuEmailResendingQuery = (ISuEmailResendingQuery)ObjectManager.GetObject("SuEmailResendingQuery");
            SMSService = (ISMSService)ObjectManager.GetObject("SMSService");
            EmailService = (IEmailService)ObjectManager.GetObject("EmailService");
            SuEmailLogDao = (ISuEmailLogDao)ObjectManager.GetObject("SuEmailLogDao");
            SuEmailResendingService = (ISuEmailResendingService)ObjectManager.GetObject("SuEmailResendingService");
            UserAccount.CurrentProgramCode = "EmailResender";
        }
    }
}

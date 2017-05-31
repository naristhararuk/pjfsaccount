using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.CommunicationService.DTO;
namespace SCG.eAccounting.BLL
{
    public interface ISCGEmailService
    {
        //void SendEmail(EmailType emailType, string sendFrom, string sendTo, string CCList, long workFlowResponseID);
        void SendEmail(EmailDTO email);
        //void SendEmail(EmailDTO email, EmailType emailType);
        void SendEmailEM01(long workFlowID, long sendToUserID , string tokenCode);
        void SendEmailEM02(long workFlowResponseID, long sendToUserID, IList<long> ccList);
        void SendEmailEM03(long workFlowID , long sendToUserID);
        void SendEmailEM04(long workFlowResponseID, long sendToUserID);
        void SendEmailEM05(long workFlowID, long sendToUserID, IList<long> ccList, bool isAutoPayment);
        void SendEmailEM06(long workFlowID, long sendToUserID , IList<long> ccList);
        void SendEmailEM07(long sendToUserID);
        void SendEmailEM08(long sendToUserID, string newGenPassword);
        void SendEmailEM09(long documentID, string ccList, string comment,string note);
        void SendEmailEM10(long advDocumentID, long sendToUserID, string ccList, string remark, bool isAuto);
        void SendEmailEM11(long expDocumentID);
        void SendEmailEM12(long userID,string password);
        void SendEmailEM13(long documentID);
        void SendEmailEM14(long workFlowID, long sendToUserID, string tokenCode);
        void SendEmailEM15(long workFlowID, long sendToUserID);
        
        /*เพิ่มแจ้งเตือนก่อนครบกำหนด fixedadvance*/
        void SendEmailEM16(long documentID);
        void ResendEmail();

        
    }
}

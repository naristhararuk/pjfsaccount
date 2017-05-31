using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.BLL
{
    public interface ISCGSMSService
    {
       // void SendSMS01(long workFlowID, long sendToUserID, string tokenSMSCode);
        void SendSMS01(long workFlowID, long sendToUserID);

        void SendSMS02(long workFlowID, string RequestID, IList<long> ReciverList, bool isAutoPayment);

        void SendSMS03(long workFlowID, string RequestID, IList<long> ReciverList);

        /// <summary>
        /// กรณี ส่งมา approve ผิด format login ได้
        /// </summary>
        /// <param name="Refno"></param>
        /// <param name="UserID"></param>
        /// <param name="NotifySMS"></param>
        void SendSMS04(string Refno, long UserID,bool NotifySMS);


        /// <summary>
        /// กรณีแจ้งเตือนเมื่อ approve เรียร้อยแล้ว
        /// </summary>
        /// <param name="Refno"></param>
        /// <param name="UserID"></param>
        /// <param name="NotifySMS"></param>
        /// <param name="ApproveFlag"></param>
        void SendSMS05(string Refno, long UserID, bool NotifySMS, string ApproveFlag);


        /// <summary>
        /// กรณี login failed 
        /// </summary>
        /// <param name="Refno"></param>
        /// <param name="UserID"></param>
        /// <param name="NotifySMS"></param>
        void SendSMS06(string Refno, long UserID, bool NotifySMS);

        /// <summary>
        /// กรณี ส่งมา approve ผิด format login ไม่ได้
        /// </summary>
        /// <param name="Refno"></param>
        /// <param name="mobilePhone"></param>
        /// <param name="NotifySMS"></param>
        void SendSMS07(string Refno, string mobilePhone, bool NotifySMS);

        /// <summary>
        /// ใช้กรณี มีการ approve รอบที่สองผ่าน sms จะทำให้หา tokencode ไม่เจอ
        /// </summary>
        /// <param name="Refno"></param>
        /// <param name="mobilePhone"></param>
        /// <param name="NotifySMS"></param>
        void SendSMS08(string Refno, string mobilePhone, bool NotifySMS);


        /// <summary>
        /// ใช้กรณี มีการ approve ผ่านช่องทางอื่นที่ไม่ใช่ sms แต่ว่าเพิ่งได้รับ message แล้ว approve มาจะทำให้ work flow เกิด error ไม่สามารถ approve ได้
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Refno"></param>
        /// <param name="mobilePhone"></param>
        /// <param name="NoitfySMS"></param>
        void SendSMS09(string Message, string Refno, string mobilePhone, bool NoitfySMS);


        /// <summary>
        /// ใช้กรณี มีการ approve ผ่านเบอร์คนอื่นที่ไม่ใช่ เบอร์ที่รับ message 
        /// </summary>
        /// <param name="Refno"></param>
        /// <param name="mobilePhone"></param>
        /// <param name="NotifySMS"></param>
        void SendSMS10(string Refno, string mobilePhone, bool NotifySMS);


        bool Status
        {
            get;
        }
    }
}


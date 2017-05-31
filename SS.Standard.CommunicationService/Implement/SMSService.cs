using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.DB.Query;
using SS.Standard.Utilities;
using SS.SU.BLL;
using SS.SU.DTO;
using SS.Standard.Security;
using SS.Standard.CommunicationService.DTO;
using System.Xml;
using SS.SU.Query;

namespace SS.Standard.CommunicationService.Implement
{
    [Serializable]
    public partial class SMSService : ISMSService
    {
        public ISuSmsLogService SuSmsLogService { get; set; }
        public ISuSmsLogQuery SuSmsLogQuery { get; set; }
        public IUserAccount UserAccount { get; set; }
        public ISuEmailResendingService SuEmailResendingService { get; set; }

        #region private variables
        private bool smsSendStatus = false;
        private string content = string.Empty;
        private DateTime date;
       // private StringBuilder smsContentBody;
        #endregion

        #region porperties

        private string SMSGateWayServer
        {
            get { return ParameterServices.SMSGateWayServer; }
        }
        private string SMSGateWayPort
        {
            get { return ParameterServices.SMSGateWayPort; }
        }
        private string ProxyUserName
        {
            get { return ParameterServices.ProxyUserName; }
        }
        private string ProxyPassword
        {
            get { return ParameterServices.ProxyPassword; }
        }
        private string ProxyServer
        {
            get { return ParameterServices.ProxyServer; }
        }
        private string ProxyPort
        {
            get { return ParameterServices.ProxyPort; }
        }
        private string SMSTRANSID
        {
            get { return ParameterServices.SMSTRANSID; }
        }
        private string SMSPhoneNumber
        {
            get { return ParameterServices.SMSPhoneNumber; }
        }
        private string SMSReport
        {
            get { return ParameterServices.SMSReport; }
        }
        private string SMSCharge
        {
            get { return ParameterServices.SMSCharge; }
        }
        public bool Status
        {
            get { return this.smsSendStatus; }
        }
        private SMS smsManager = null;
        public string FROM { get; set; }
        public string TO { get; set; }
        public string CONTENT { get; set; }
        public string CMD { get; set; }
        public string TRANSID { get; set; }
        public string CTYPE { get; set; }
        #endregion

        #region constructor
        public SMSService()
        {
        }

        #endregion
        
        #region Public Methods
        public bool Send(SMSDTO smsDTO)
        {
            try
            {
                smsManager = new SMS();
                content = smsDTO.Content;
                date = DateTime.Now;
                smsManager.CONTENT = content;
                smsManager.FROM = SMSPhoneNumber;
                smsManager.TO = smsDTO.To;
                smsManager.SMSGateWayServer = SMSGateWayServer;
                smsManager.SMSGateWayPort = Utilities.Utilities.ParseInt(SMSGateWayPort);
                smsManager.REPORT = SMSReport;
                smsManager.CHARGE = SMSCharge;
                smsManager.ProxyUserName = ProxyUserName;
                smsManager.ProxyPassword = ProxyPassword;
                smsManager.TRANSID = SMSTRANSID;
                smsManager.ProxyServer = ProxyServer;
                smsManager.ProxyPort = Utilities.Utilities.ParseInt(ProxyPort);
                smsManager.useProxy = smsDTO.UseProxy;
                SMSContainer smsContainer = new SMSContainer();
                smsContainer.sms = smsManager;
                SMSSerializer serializer = new SMSSerializer();
                string serialObject = serializer.SerializeObject(smsContainer);
                SuEmailResending resending = new SuEmailResending();
                resending.Creby = 1;
                resending.CreDate = DateTime.Now;
                resending.emailtype = "SMS";
                resending.lastsenddate = DateTime.Now.AddMinutes(ParameterServices.EmailPendingDuration * -1);
                resending.mailcontent = serialObject;
                resending.retry = 0;
                resending.sendto = smsManager.TO;
                resending.status = "New";
                resending.subject = "Short Message";
                SuEmailResendingService.Save(resending);
                smsSendStatus = true;
            }
            catch (Exception ex)
            {
                smsSendStatus = false;
                SS.Standard.Utilities.Utilities.WriteLogs("SMSService : " + ex.Message.Replace("remote", "SMS remote"), "smslog", "Logs", "txt");

            }
            return smsSendStatus;

        }
        public bool Send(SMSDTO smsDTO, long smsLogID)
        {
            try
            {
                smsManager = new SMS();
                content = smsDTO.Content;
                date = DateTime.Now;
                smsManager.CONTENT = content;
                smsManager.FROM = SMSPhoneNumber;
                smsManager.TO = smsDTO.To;
                smsManager.SMSGateWayServer = SMSGateWayServer;
                smsManager.SMSGateWayPort = Utilities.Utilities.ParseInt(SMSGateWayPort);
                smsManager.REPORT = SMSReport;
                smsManager.CHARGE = SMSCharge;
                smsManager.ProxyUserName = ProxyUserName;
                smsManager.ProxyPassword = ProxyPassword;
                smsManager.TRANSID = SMSTRANSID;
                smsManager.ProxyServer = ProxyServer;
                smsManager.ProxyPort = Utilities.Utilities.ParseInt(ProxyPort);
                smsManager.useProxy = smsDTO.UseProxy;
                SMSContainer smsContainer = new SMSContainer();
                smsContainer.sms = smsManager;
                smsContainer.SMSLogid = smsLogID;
                SMSSerializer serializer = new SMSSerializer();
                string serialObject = serializer.SerializeObject(smsContainer);
                SuEmailResending resending = new SuEmailResending();
                resending.Creby = UserAccount.UserID;
                resending.CreDate = DateTime.Now.AddMinutes(ParameterServices.EmailPendingDuration * -1);
                resending.emailtype = "SMS+Log";
                resending.lastsenddate = DateTime.Now;
                resending.mailcontent = serialObject;
                resending.retry = 0;
                resending.sendto = smsManager.TO;
                resending.status = "New";
                resending.subject = content;
                SuEmailResendingService.Save(resending);
                smsSendStatus = true;

            }
            catch (Exception ex)
            {
                smsSendStatus = false;
                SS.Standard.Utilities.Utilities.WriteLogs("SMSService : " + ex.Message.Replace("remote", "SMS remote"), "smslog", "Logs", "txt");

            }
            return smsSendStatus;

        }
        public bool Send(SMSDTO smsDTO, bool NotifySMS)
        {
            try
            {
                smsManager = new SMS();
                content = smsDTO.Content;
                date = DateTime.Now;
                smsManager.CONTENT = content;
                smsManager.FROM = SMSPhoneNumber;
                smsManager.TO = smsDTO.To;
                smsManager.SMSGateWayServer = SMSGateWayServer;
                smsManager.SMSGateWayPort = Utilities.Utilities.ParseInt(SMSGateWayPort);
                smsManager.REPORT = SMSReport;
                smsManager.CHARGE = SMSCharge;
                smsManager.ProxyUserName = ProxyUserName;
                smsManager.ProxyPassword = ProxyPassword;
                smsManager.TRANSID = SMSTRANSID;
                smsManager.ProxyServer = ProxyServer;
                smsManager.ProxyPort = Utilities.Utilities.ParseInt(ProxyPort);
                smsManager.useProxy = smsDTO.UseProxy;
                SMSContainer smsContainer = new SMSContainer();
                smsContainer.sms = smsManager;
                smsContainer.NotifySMS = NotifySMS;
                SMSSerializer serializer = new SMSSerializer();
                string serialObject = serializer.SerializeObject(smsContainer);
                SuEmailResending resending = new SuEmailResending();
                resending.Creby = UserAccount.UserID;
                resending.CreDate = DateTime.Now;
                resending.emailtype = "SMS+Notify";
                resending.lastsenddate = DateTime.Now.AddMinutes(ParameterServices.EmailPendingDuration * -1);
                resending.mailcontent = serialObject;
                resending.retry = 0;
                resending.sendto = smsManager.TO;
                resending.status = "New";
                resending.subject = content;
                SuEmailResendingService.Save(resending);
                smsSendStatus = true;
       
                smsSendStatus = true;

 
            }
            catch (Exception ex)
            {
                smsSendStatus = false;
                SS.Standard.Utilities.Utilities.WriteLogs("SMSService : " + ex.Message.Replace("remote", "SMS remote"), "smslog", "Logs", "txt");

            }
            return smsSendStatus;

        }
        #endregion
        private void AddSMSLog(DateTime Date, string PhoneNo, string SendOrReceive, string Message, string SMSGateWayResult)
        {
            string SendMsgStatus = string.Empty;
            string SendMsgDetail = string.Empty;
            string SendMsgSMID = string.Empty;
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(SMSGateWayResult);
                XmlNodeList status = xDoc.GetElementsByTagName("STATUS");
                XmlNodeList detail = xDoc.GetElementsByTagName("DETAIL");
                XmlNodeList SMID = xDoc.GetElementsByTagName("SMID");

                SendMsgStatus = status.Item(0).InnerXml;
                SendMsgDetail = detail.Item(0).InnerXml;
                SendMsgSMID = SMID.Item(0).InnerXml;


            }
            catch (Exception exg)
            {

                SS.Standard.Utilities.Utilities.WriteLogs("LoadXml SMS : " + exg.Message, "smslog", "Logs", "txt");
            }
            try
            {

                long userID = UserAccount == null ? 0 : UserAccount.UserID;
                SuSmsLog smsLog = new SuSmsLog();
                string tmpPhoneNo = PhoneNo.TrimStart(new char[] { '6', '6' });

                smsLog.Active = true;
                smsLog.CreBy = userID;
                smsLog.CreDate = Date;
                smsLog.Date = Date;
                smsLog.Message = Message;
                smsLog.PhoneNo = tmpPhoneNo.PadLeft(10, '0');
                smsLog.SendOrReceive = SendOrReceive.ToString();
                smsLog.SendMsgDate = Date;
                smsLog.SendMsgDetail = SendMsgDetail;
                smsLog.SendMsgStatus = SendMsgStatus;
                smsLog.SendMsgSMID = SendMsgSMID;
                smsLog.UpdBy = userID;
                smsLog.UpdDate = Date;
                smsLog.UpdPgm = UserAccount == null ? "SMS Services Class" : UserAccount.CurrentProgramCode;
                SuSmsLogService.Save(smsLog);

            }
            catch (Exception exgs)
            {

                SS.Standard.Utilities.Utilities.WriteLogs("AddLog SMS : " + exgs.Message, "smslog", "Logs", "txt");

            }

        }

        private void AddNotifySMSLog(DateTime Date, string PhoneNo, string SendOrReceive, string Message, string SMSGateWayResult)
        {
            string SendMsgStatus = string.Empty;
            string SendMsgDetail = string.Empty;
            string SendMsgSMID = string.Empty;
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(SMSGateWayResult);
                XmlNodeList status = xDoc.GetElementsByTagName("STATUS");
                XmlNodeList detail = xDoc.GetElementsByTagName("DETAIL");
                XmlNodeList SMID = xDoc.GetElementsByTagName("SMID");

                SendMsgStatus = status.Item(0).InnerXml;
                SendMsgDetail = detail.Item(0).InnerXml;
                SendMsgSMID = SMID.Item(0).InnerXml;


            }
            catch (Exception exg)
            {

                SS.Standard.Utilities.Utilities.WriteLogs("LoadXml SMS : " + exg.Message, "smslog", "Logs", "txt");
            }
            try
            {

                long userID = UserAccount == null ? 0 : UserAccount.UserID;
                SuSmsLog smsLog = new SuSmsLog();
                string tmpPhoneNo = PhoneNo.TrimStart(new char[] { '6', '6' });

                smsLog.Active = true;
                smsLog.CreBy = userID;
                smsLog.CreDate = Date;
                smsLog.Date = Date;
                smsLog.Message = Message;
                smsLog.PhoneNo = tmpPhoneNo.PadLeft(10, '0');
                smsLog.SendOrReceive = SendOrReceive.ToString();
                smsLog.SendMsgDate = Date;
                smsLog.SendMsgDetail = SendMsgDetail;
                smsLog.SendMsgStatus = SendMsgStatus;
                smsLog.SendMsgSMID = SendMsgSMID;
                smsLog.UpdBy = userID;
                smsLog.UpdDate = Date;
                smsLog.UpdPgm = UserAccount == null ? "SMS Services Class" : UserAccount.CurrentProgramCode;
                SuSmsLogService.Save(smsLog);

            }
            catch (Exception exgs)
            {

                SS.Standard.Utilities.Utilities.WriteLogs("AddLog SMS : " + exgs.Message, "smslog", "Logs", "txt");

            }

        }

        private void UpdateSMSLog(DateTime Date, string SMSGateWayResult, long smsLogID)
        {
            string SendMsgStatus = string.Empty;
            string SendMsgDetail = string.Empty;
            string SendMsgSMID = string.Empty;
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(SMSGateWayResult);
                XmlNodeList status = xDoc.GetElementsByTagName("STATUS");
                XmlNodeList detail = xDoc.GetElementsByTagName("DETAIL");
                XmlNodeList SMID = xDoc.GetElementsByTagName("SMID");

                SendMsgStatus = status.Item(0).InnerXml;
                SendMsgDetail = detail.Item(0).InnerXml;
                SendMsgSMID = SMID.Item(0).InnerXml;


            }
            catch (Exception exg)
            {

                SS.Standard.Utilities.Utilities.WriteLogs("LoadXml SMS : " + exg.Message, "smslog", "Logs", "txt");
            }
            try
            {

                long userID = UserAccount == null ? 0 : UserAccount.UserID;
                SuSmsLog smsLog = SuSmsLogQuery.FindByID(smsLogID);
                if (smsLog != null)
                {
                    smsLog.Date = Date;
                    smsLog.SendMsgDate = Date;
                    smsLog.SendMsgDetail = SendMsgDetail;
                    smsLog.SendMsgStatus = SendMsgStatus;
                    smsLog.SendMsgSMID = SendMsgSMID;
                    smsLog.UpdBy = userID;
                    smsLog.UpdDate = Date;
                    smsLog.UpdPgm = UserAccount == null ? "SMS Services Class" : UserAccount.CurrentProgramCode;
                    SuSmsLogService.Save(smsLog);
                }

            }
            catch (Exception exgs)
            {

                SS.Standard.Utilities.Utilities.WriteLogs("AddLog SMS : " + exgs.Message, "smslog", "Logs", "txt");

            }

        }

        public bool UpdateDLVRREP(SuSmsLog smsLog)
        {
            try
            {
                SuSmsLogService.Update(smsLog);
                return true;
            }
            catch (Exception exgs)
            {

                SS.Standard.Utilities.Utilities.WriteLogs("AddLog SMS : " + exgs.Message, "smslog", "Logs", "txt");
                return false;
            }
        }

        #region ISMSService Members


        public bool ReSend(SMS sms)
        {
            try
            {
                string SMSGateWayResult = sms.Send();
                smsSendStatus = true;
                AddSMSLog(DateTime.Now, sms.TO, "Send", sms.CONTENT, SMSGateWayResult);
                return true;
            }
            catch (Exception)
            {
                
                return false;
            }

        }

        public bool ReSend(SMS sms, long smsLogID)
        {
            try
            {
                string SMSGateWayResult = sms.Send();
                UpdateSMSLog(DateTime.Now, SMSGateWayResult, smsLogID);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ReSend(SMS sms, bool NotifySMS)
        {
            try
            {
                string SMSGateWayResult = sms.Send();
                AddNotifySMSLog(DateTime.Now, sms.TO, "Send", sms.CONTENT, SMSGateWayResult);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        #endregion
    }
}

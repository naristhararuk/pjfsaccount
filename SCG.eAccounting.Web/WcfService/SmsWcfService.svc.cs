using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.Query;
using SS.SU.BLL;
using SS.Standard.WorkFlow.Service;
using SCG.eAccounting.BLL;
using Spring.Context.Support;
using SS.Standard.Security;
using SS.SU.DTO;
using SS.SU.Query;
using SS.Standard.Utilities;
using SS.DB.Query;
using SCG.eAccounting.AppService;

namespace SCG.eAccounting.Web.WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SmsWcfService" in code, svc and config file together.
    public class SmsWcfService : ISmsWcfService
    {
        public ISuSmsLogService SuSmsLogService { get; set; }
        //public IWorkFlowResponseTokenQuery WorkFlowResponseTokenQuery { get; set; }
        public IWorkFlowService WorkFlowService { get; set; }
        public ISCGSMSService SCGSMSService { get; set; }
        public IWorkFlowStateEventQuery WorkFlowStateEventQuery { get; set; }
        public IUserAccount UserAccount { get; set; }
        public IUserEngine UserEngine { get; set; }

        public void ReceiveSMS(string from, string to, string content, string transID)
        {
            SCGSMSService = (ISCGSMSService)ContextRegistry.GetContext().GetObject("SCGSMSService");

            if (from.Length > 9)
            {
                int startIndex = from.Length - 9;
                from = from.Substring(startIndex, 9);
                from = from.PadLeft(10, '0');
            }


            if (content.Length != 7)
            {
                //แจ้งเตือนกรณีใส่ข้อมูลการ approve มาผิด format
                long smsLogID = AddSMSLog(DateTime.Now, from, "Receive", content, transID);
                SCGSMSService.SendSMS07(content, from, true);

            }
            else
            {
                string ReplyApproveFlag = content.Equals(string.Empty) ? "" : content.Substring(content.Length - 1, 1);


                if (ReplyApproveFlag.ToUpper().Equals("Y") || ReplyApproveFlag.ToUpper().Equals("N"))
                {
                    //format การ approve ถูกต้องจะต้องมี 7 หลักเท่านั้น แล้วทำการ process ในการเรียก work flow

                    SMSApproveProcess(from, content, transID);
                }
                else
                {
                    //แจ้งเตือนกรณีใส่ข้อมูลการ approve มาผิด format ไม่ต้อบเป็น Y หรือ N
                    long smsLogID = AddSMSLog(DateTime.Now, from, "Receive", content, transID);
                    SCGSMSService.SendSMS07(content, from, true);
                }

            }

        }

        private void SMSApproveProcess(string FROM, string CONTENT, string TRANSID)
        {
            IWorkFlowResponseTokenQuery WorkFlowResponseTokenQuery = (IWorkFlowResponseTokenQuery)ContextRegistry.GetContext().GetObject("WorkFlowResponseTokenQuery");

            string ReplyApproveFlag = CONTENT.Equals(string.Empty) ? "" : CONTENT.Substring(CONTENT.Length - 1, 1);
            string ReplyTokenResult = CONTENT.Equals(string.Empty) ? "" : CONTENT.TrimEnd(new Char[] { 'Y', 'N', 'y', 'n' });


            if (ReplyApproveFlag.ToUpper().Equals("Y"))
            {
                ReplyApproveFlag = "Approve"; //Accept
            }
            else if (ReplyApproveFlag.ToUpper().Equals("N"))
            {
                ReplyApproveFlag = "Reject"; //Decline
            }
            long userID = 0;
            IList<WorkFlowResponseToken> responseTokens = WorkFlowResponseTokenQuery.FindByTokenCode(ReplyTokenResult);
            foreach (WorkFlowResponseToken responseToken in responseTokens)
            {
                userID = responseToken.UserID;
                break;

            }
            //1. SignIn by UserName (From)

            if (userID == 0)
            {
                //แจ้งเตือนในกรณีที่มีการ approved ผ่านมือถือ แต่ว่า approve อีกรอบจะทำให้ tokencode หาย
                long smsLogID = AddSMSLog(DateTime.Now, FROM, "Receive", CONTENT, TRANSID);
                SCGSMSService.SendSMS08(CONTENT, FROM, true);
            }
            else
            {
                try
                {
                    UserEngine = (IUserEngine)ContextRegistry.GetContext().GetObject("UserEngine");
                    ISuUserQuery SuUserQuery = (ISuUserQuery)ContextRegistry.GetContext().GetObject("SuUserQuery");
                    IDbParameterQuery DbParameterQuery = (IDbParameterQuery)ContextRegistry.GetContext().GetObject("DbParameterQuery");

                    SuUser user = SuUserQuery.FindByIdentity(userID);
                    if (user != null && user.MobilePhoneNo == FROM)
                    {

                        string userName = user.UserName;

                        UserEngine.SignIn(userName);

                        UserAccount = (IUserAccount)ContextRegistry.GetContext().GetObject("UserAccount");
                        string smsDefaultLanguageID = DbParameterQuery.GetParameterByName("SMSDefaultLanguageID");
                        UserAccount.CurrentLanguageID = string.IsNullOrEmpty(smsDefaultLanguageID) ? (short)1 : Convert.ToInt16(smsDefaultLanguageID);
                        UserAccount.CurrentProgramCode = "SmsWcfService";


                        long smsLogID = AddSMSLog(DateTime.Now, FROM, "Receive", CONTENT, TRANSID);

                        bool approveFalg = CallWorkFlow(ReplyTokenResult, ReplyApproveFlag, FROM);

                    }
                    else
                    {
                        //แจ้งเตือนในกรณีที่มีการ approved ผ่านมือถือคนอื่นที่ไม่ใช่เครื่องที่รับ message นั้น
                        long smsLogID = AddSMSLog(DateTime.Now, FROM, "Receive", CONTENT, TRANSID);
                        SCGSMSService.SendSMS10(CONTENT, FROM, true);
                    }
                }
                catch (Exception ex)
                {
                    Utilities.WriteLogs(ex.Message, "smslog", "Logs", "txt");
                    // error = ex.Message;
                }
            }
        }

        private long AddSMSLog(DateTime Date, string PhoneNo, string SendOrReceive, string Message, string TRANID)
        {
            long smslogID = 0;
            SuSmsLogService = (ISuSmsLogService)ContextRegistry.GetContext().GetObject("SuSmsLogService");
            try
            {
                long userID = UserAccount == null ? 0 : UserAccount.UserID;


                SuSmsLog smsLog = new SuSmsLog();
                smsLog.Active = true;
                smsLog.CreBy = userID;
                smsLog.CreDate = Date;
                smsLog.Date = Date;
                smsLog.Message = Message;
                smsLog.TRANID = TRANID;
                smsLog.PhoneNo = PhoneNo;
                smsLog.SendOrReceive = SendOrReceive.ToString();
                smsLog.SendMsgDate = Date;
                smsLog.SendMsgDetail = string.Empty;
                smsLog.SendMsgStatus = string.Empty;
                smsLog.SendMsgSMID = string.Empty;
                smsLog.UpdBy = userID;
                smsLog.UpdDate = Date;
                smsLog.UpdPgm = "Receive SMS Program";
                smslogID = SuSmsLogService.Save(smsLog);

            }
            catch (Exception ex)
            {
                Utilities.WriteLogs(ex.Message, "smslog", "Logs", "txt");
                // error = ex.Message;
            }
            return smslogID;
        }

        private bool CallWorkFlow(string TokenCode, string approveFlag, string From)
        {
            //1.1 If cannot SignIn , we not need to call workflow

            IWorkFlowResponseTokenQuery WorkFlowResponseTokenQuery = (IWorkFlowResponseTokenQuery)ContextRegistry.GetContext().GetObject("WorkFlowResponseTokenQuery");

            string docNo = "";
            if (UserAccount != null && UserAccount.Authentication)
            {
                try
                {
                    IList<WorkFlowResponseToken> responseTokens = WorkFlowResponseTokenQuery.FindByTokenCode(TokenCode);

                    int? eventID = null;

                    foreach (WorkFlowResponseToken responseToken in responseTokens)
                    {
                        if (responseToken.WorkFlowStateEvent.Name.ToLower() == approveFlag.ToLower())
                        {
                            eventID = responseToken.WorkFlowStateEvent.WorkFlowStateEventID;
                            break;
                        }
                        else if (responseToken.WorkFlowStateEvent.Name.ToLower() == approveFlag.ToLower())
                        {
                            eventID = responseToken.WorkFlowStateEvent.WorkFlowStateEventID;
                            break;
                        }
                    }

                    
                    WorkFlowResponseToken token = WorkFlowResponseTokenQuery.GetByTokenCode_WorkFlowStateEventID(TokenCode, eventID.Value);

                    if (token == null)
                    {
                        Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
                        string error = new Spring.Validation.ErrorMessage("InvalidTokenID").ToString();
                        //send sms notify error
                        SCGSMSService.SendSMS09(error, TokenCode, From, true);
                    }
                    else
                    {
                        docNo = token.WorkFlow.Document.DocumentNo;

                        WorkFlowService = (IWorkFlowService)ContextRegistry.GetContext().GetObject("WorkFlowService");

                        WorkFlowService.NotifyEventFromSMSToken(TokenCode, UserAccount.UserID, eventID.Value, TokenType.SMS);


                        //send sms notify approve comleted user
                        SCGSMSService.SendSMS05(TokenCode, UserAccount.UserID, true, approveFlag);

                    }

                }
                catch (ServiceValidationException ex)
                {
                    string error = ex.ValidationErrors.ToString();
                    //send sms notify error
                    SCGSMSService.SendSMS09(error, TokenCode, From, true);


                }
                catch (Exception exm)
                {
                    string error = exm.Message.ToString();
                    //send sms notify error
                    SCGSMSService.SendSMS09(error, TokenCode, From, true);
                }
                finally
                {

                    UserEngine.SignOut(UserAccount.UserID);
                }
            }
            else
            {
                //send sms notify approve comleted user
                SCGSMSService.SendSMS06(TokenCode, UserAccount.UserID, true);
            }
            return true;
        }
    }
}

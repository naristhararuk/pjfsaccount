using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Configuration;
using System.Collections;
using System.Web.Security;
using System.Web.UI.HtmlControls;

using System.IO;
using System.Net;
using System.Net.Sockets;

using SS.Standard.UI;
using SS.Standard.CommunicationService;
using SS.Standard.CommunicationService.DTO;
using SS.SU.BLL;
using SS.SU.DTO;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.Query.Hibernate;
using SS.Standard.Utilities;
using SS.Standard.WorkFlow.Service.Implement;
using SCG.eAccounting.Web.Helper;
using SS.Standard.WorkFlow.Query;
using SS.Standard.WorkFlow.Service;
using SS.DB.Query;
using SCG.eAccounting.BLL;

namespace SCG.eAccounting.Web
{
    public partial class Receive : BasePage
    {
        public ISuSmsLogService SuSmsLogService { get; set; }
        public IWorkFlowResponseTokenQuery WorkFlowResponseTokenQuery { get; set; }
        public IWorkFlowService WorkFlowService { get; set; }
        public ISCGSMSService SCGSMSService { get; set; }
        public IWorkFlowStateEventQuery WorkFlowStateEventQuery { get; set; }
        private static string ResponeOK = "<XML><STATUS>OK</STATUS><DETAIL></DETAIL></XML>";
        private string ResponeERR = "<XML><STATUS>ERR</STATUS><DETAIL>{0}</DETAIL></XML>";
        private string TRANSID = string.Empty;
        private string FROM = string.Empty;
        private string TO = string.Empty;
        private string CONTENT = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Form["TRANSID"] == null
                    || Request.Form["FROM"] == null
                    || Request.Form["TO"] == null
                    || Request.Form["CONTENT"] == null
                    )
                {
                    //Respone failed
                    string ResponseError = string.Empty;
                   if(Request.Form["TRANSID"] == null)
                       ResponseError = string.Format(ResponeERR, "Parameter TRANSID is null");
                   else if (Request.Form["FROM"] == null)
                        ResponseError = string.Format(ResponeERR, "Parameter FROM is null");
                   else if (Request.Form["TO"]==null)
                        ResponseError = string.Format(ResponeERR, "Parameter TO is null");
                   else if (Request.Form["CONTENT"] == null)
                   {
                        ResponseError = string.Format(ResponeERR, "Parameter CONTENT is null");
                   }
                   else
                   {
                       ResponseError = string.Format(ResponeERR, "Parameter is null");
                   }

                   Response.ClearHeaders();
                   Response.Clear();
                   Response.Write( ResponseError);
                   Response.End();
                }
                else
                {
                    TRANSID = Request.Form["TRANSID"] == null ? string.Empty : Request.Form["TRANSID"].Trim();
                    FROM = Request.Form["FROM"] == null ? string.Empty : Request.Form["FROM"].Trim();
                    TO = Request.Form["TO"] == null ? string.Empty : Request.Form["TO"].Trim();
                    CONTENT = Request.Form["CONTENT"] == null ? string.Empty : Request.Form["CONTENT"].Trim();
                    

                    if (FROM.Length > 9)
                    {
                        int startIndex = FROM.Length - 9;
                        FROM = FROM.Substring(startIndex, 9);
                        FROM = FROM.PadLeft(10, '0');
                    }


                    if (Request.Form["CONTENT"].Trim().Length > 6 || Request.Form["CONTENT"].Trim().Length < 6)
                    {
                        //แจ้งเตือนกรณีใส่ข้อมูลการ approve มาผิด format
                        long smsLogID = AddSMSLog(DateTime.Now, FROM, "Receive", CONTENT, TRANSID);
                        SCGSMSService.SendSMS07(CONTENT, FROM, true);

                    }
                    else if (Request.Form["CONTENT"].Trim().Length == 6)
                    {
                        string ReplyApproveFlag = CONTENT.Equals(string.Empty) ? "" : CONTENT.Substring(CONTENT.Length - 1, 1);


                        if (ReplyApproveFlag.ToUpper().Equals("Y") || ReplyApproveFlag.ToUpper().Equals("N"))
                        {
                            //format การ approve ถูกต้องจะต้องมี 6 หลักเท่านั้น แล้วทำการ process ในการเรียก work flow
                            
                            SMSApproveProcess(FROM, CONTENT);
                        }
                        else
                        {
                            //แจ้งเตือนกรณีใส่ข้อมูลการ approve มาผิด format ไม่ต้อบเป็น Y หรือ N
                            long smsLogID = AddSMSLog(DateTime.Now, FROM, "Receive", CONTENT, TRANSID);
                            SCGSMSService.SendSMS07(CONTENT, FROM, true);
                        }

                    }

                    //Respone OK ให้ทาง AIS ทราบว่าเราได้รับ message เรียบร้อยแล้ว
                    Response.ClearHeaders();
                    Response.Clear();
                    Response.Write(ResponeOK);
                    Response.End();

                }

            }
            catch (Exception ex)
            {
                Utilities.WriteLogs(ex.Message, "smslog", "Logs", "txt");

            }
        }

        private void SMSApproveProcess(string FROM, string CONTENT)
        {


            string ReplyApproveFlag = CONTENT.Equals(string.Empty) ? "" : CONTENT.Substring(CONTENT.Length - 1, 1);
            string ReplyTokenResult = CONTENT.Equals(string.Empty) ? "" : CONTENT.TrimEnd(new Char[] { 'Y', 'N','y','n' });


            if (ReplyApproveFlag.ToUpper().Equals("Y"))
            {
                ReplyApproveFlag = "Approve"; //Accept
            }
            else if (ReplyApproveFlag.ToUpper().Equals("N"))
            {
                ReplyApproveFlag = "Reject"; //Decline
            }
            long userID = 0;
            IList<WorkFlowResponseToken> responseTokens = WorkFlowQueryProvider.WorkFlowResponseTokenQuery.FindByTokenCode(ReplyTokenResult);
            foreach (WorkFlowResponseToken responseToken in responseTokens)
            {
                userID = responseToken.UserID;
                break;

            }
            //1. SignIn by UserName (From)

            if (userID == 0)
            {
                //แจ้งเตือนในกรณีที่มีการ approved ผ่านมือถือ แต่ว่า approve อีกรอบจะทำให้ tokencode หาย
                long smsLogID = AddSMSLog(DateTime.Now, FROM, "Receive", CONTENT,TRANSID);
                SCGSMSService.SendSMS08(CONTENT, FROM, true);
            }
            else
            {

                SuUser userAccount = SS.SU.Query.QueryProvider.SuUserQuery.FindByIdentity(userID);
                if (userAccount != null && userAccount.MobilePhoneNo == FROM)
                {

                    string userName = userAccount.UserName;

                    UserEngine.SignIn(userName);
                    UserAccount.CurrentLanguageID = ParameterServices.SMSDefaultLanguageID;
                    UserAccount.CurrentProgramCode = this.ProgramCode;


                    long smsLogID = AddSMSLog(DateTime.Now, FROM, "Receive", CONTENT,TRANSID);

                    bool approveFalg = CallWorkFlow(ReplyTokenResult, ReplyApproveFlag, FROM);

                }
                else
                {

                    //แจ้งเตือนในกรณีที่มีการ approved ผ่านมือถือคนอื่นที่ไม่ใช่เครื่องที่รับ message นั้น
                    long smsLogID = AddSMSLog(DateTime.Now, FROM, "Receive", CONTENT,TRANSID);
                    SCGSMSService.SendSMS10(CONTENT, FROM, true);
                }
            }
        }

        private long AddSMSLog(DateTime Date, string PhoneNo, string SendOrReceive, string Message,string TRANID)
        {
            long smslogID = 0;
            try
            {
                long userID = UserAccount==null ? 0 : UserAccount.UserID;
               

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
                smsLog.UpdPgm = UserAccount==null ? "Receive SMS Program" : UserAccount.CurrentProgramCode;
                smslogID = SuSmsLogService.Save(smsLog);

            }
            catch (Exception ex)
            {
                Utilities.WriteLogs(ex.Message, "smslog", "Logs", "txt");
               // error = ex.Message;
            }
            return smslogID;
        }

        private bool CallWorkFlow(string TokenCode,string approveFlag, string From)
        {
            //1.1 If cannot SignIn , we not need to call workflow
           
         

            string docNo = "";
            if (UserAccount != null && UserAccount.Authentication)
            {
                try
                {
                    IList<WorkFlowResponseToken> responseTokens = WorkFlowQueryProvider.WorkFlowResponseTokenQuery.FindByTokenCode(TokenCode);

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
                        //errors.AddError("General.Error", new Spring.Validation.ErrorMessage("InvalidTokenID"));

                        //send sms notify error
                        SCGSMSService.SendSMS09(error, TokenCode, From, true);

                        //throw new ServiceValidationException(errors);
                    }
                    else
                    {
                        docNo = token.WorkFlow.Document.DocumentNo;
                        string successText = GetMessage("ApproveRejectResultMessage",
                            token.WorkFlow.Document.DocumentNo,
                            WorkFlowStateEventQuery.GetTranslatedEventName(
                                token.WorkFlowStateEvent.WorkFlowStateEventID,
                                UserAccount.CurrentLanguageID));

                        WorkFlowService.NotifyEventFromSMSToken(TokenCode, UserAccount.UserID, eventID.Value, TokenType.SMS);


                        //send sms notify approve comleted user
                        SCGSMSService.SendSMS05(TokenCode, UserAccount.UserID, true, approveFlag);

                    }

                }
                catch (ServiceValidationException ex)
                {
                    this.ValidationErrors.MergeErrors(ex.ValidationErrors);

                    string error = ex.ValidationErrors.ToString();
                    //send sms notify approve comleted user
                    //SCGSMSService.SendSMS04(TokenCode + " " + approveFlag + " ", UserAccount.UserID, true);
                    //send sms notify error
                    SCGSMSService.SendSMS09(error, TokenCode, From, true);


                }
                catch (Exception exm)
                {
                    string error = exm.Message.ToString();
                    //send sms notify approve comleted user
                    //SCGSMSService.SendSMS04(TokenCode + " " + approveFlag + " ", UserAccount.UserID, true);
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
                this.ValidationErrors.AddError("General.Error",
                    new Spring.Validation.ErrorMessage("ApproveRejectResultLoginFail"));

                //send sms notify approve comleted user
                SCGSMSService.SendSMS06(TokenCode,UserAccount.UserID,true);


            }
            return true;
        }

       
        
    }
}
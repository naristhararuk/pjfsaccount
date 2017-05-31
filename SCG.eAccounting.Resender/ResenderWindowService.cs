using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using SCG.eAccounting.Resender.DAL;
using SS.SU.DTO;
using SCG.eAccounting.BLL;
using SS.Standard.CommunicationService;
using SS.Standard.CommunicationService.DTO;
using SCG.eAccounting.DTO;
using SS.DB.Query;

namespace SCG.eAccounting.Resender
{
    partial class ResenderWindowService : ServiceBase
    {
        static private bool cmdStop = false;
        Thread workerThread;
        public ResenderWindowService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
            ParameterServices.Neologize();
            ThreadStart st = new ThreadStart(ResendThread);
            workerThread = new Thread(st);
            cmdStop = false;
            workerThread.Start();
        }
        //protected override void OnPause()
        //{
        //    base.OnPause();
        //    Thread.Sleep(40000);
        //}

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
            cmdStop = true;
            workerThread.Join(new TimeSpan(0, 0, 10));
        }

        private static bool IsEmptyMailList(List<AddMailSendTo> listSend)
        {
            foreach (AddMailSendTo item in listSend)
            {
                if (item.Email.Trim() != string.Empty)
                {
                    if (IsValidEmailFormat(item.Email))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private static bool IsValidEmailFormat(string emailAddress)
        {
            if (emailAddress.Contains('@') && emailAddress.Contains('.'))
            {
                return true;
            }
            return false;
        }
        private static void AddEmailLog(EmailDTO mail)
        {
            SuEmailLog emailLog = new SuEmailLog();
            emailLog.RequestNo = mail.RequesterNo;
            emailLog.EmailType = mail.EmailType;
            emailLog.SendDate = DateTime.Now;
            emailLog.ToEmail = null;
            if ((mail.MailSendTo != null) && (mail.MailSendTo.Count > 0))
            {
                foreach (AddMailSendTo sendTo in mail.MailSendTo)
                {
                    if (string.IsNullOrEmpty(emailLog.ToEmail))
                        emailLog.ToEmail += sendTo.Email;
                    else
                        emailLog.ToEmail += ";" + sendTo.Email;
                }
            }
            emailLog.CCEmail = null;
            if ((mail.MailSendToCC != null) && (mail.MailSendToCC.Count > 0))
            {
                foreach (AddMailSendToCC cc in mail.MailSendToCC)
                {
                    if (string.IsNullOrEmpty(emailLog.CCEmail))
                        emailLog.CCEmail += cc.Email;
                    else
                        emailLog.CCEmail += ";" + cc.Email;
                }
            }
            emailLog.Status = mail.Status;
            emailLog.Active = true;
            emailLog.CreBy = Factory.UserAccount.UserID;
            emailLog.CreDate = DateTime.Now;
            emailLog.UpdBy = Factory.UserAccount.UserID;
            emailLog.UpdDate = DateTime.Now;
            emailLog.UpdPgm = Factory.UserAccount.CurrentProgramCode;
            emailLog.Remark = mail.Remark;

            Factory.SuEmailLogDao.Save(emailLog);
        }

        static private void ResendThread()
        {
            while (cmdStop == false)
            {
                try
                {
                    ParameterServices.Neologize();
                    Log.WriteMsgLogs("Email Resending Process ...");
                    Factory.CreateObject();
                    Factory.SuEmailResendingQuery.DeleteSuccessItem();
                    IList<SuEmailResending> ResendMails = Factory.SuEmailResendingQuery.FindAllEmailResending();
                    EmailSerializer serailer = new EmailSerializer();
                    SMSSerializer smsserialer = new SMSSerializer();
                    foreach (SuEmailResending item in ResendMails)
                    {
                        bool success;
                        if (item.emailtype.Contains("SMS"))
                        {
                            success = false;
                            item.status = "Retry";
                            try
                            {
                                SMSContainer sms = smsserialer.DeSerializeObject(item.mailcontent);
                                if (item.emailtype == "SMS")
                                {
                                    success = Factory.SMSService.ReSend(sms.sms);
                                }
                                else if (item.emailtype == "SMS+Log")
                                {
                                    success = Factory.SMSService.ReSend(sms.sms, sms.SMSLogid);
                                }
                                else if (item.emailtype == "SMS+Notify")
                                {
                                    success = Factory.SMSService.ReSend(sms.sms, sms.NotifySMS);
                                }
                                if (success)
                                {
                                    item.status = "Success";
                                }
                            }
                            catch (Exception e)
                            {
                                Log.WriteLogs(e);
                            }
                        }
                        else
                        {
                            try
                            {
                                EmailDTO email = serailer.DeSerializeObject(item.mailcontent);
                                email.Status = 2;
                                item.status = "Retry";
                                if (IsEmptyMailList(email.MailSendTo))
                                {
                                    AddMailSendTo sendto = new AddMailSendTo();
                                    sendto.Email = SS.DB.Query.ParameterServices.AdminEmailAddress;
                                    sendto.Name = SS.DB.Query.ParameterServices.AdminName;
                                    email.MailSendTo.Clear();
                                    email.MailSendTo.Add(sendto);
                                    email.Remark = "Send to email addresses is empty or invalid email format.";

                                }
                                email.IsMultipleReceiver = true;
                                success = Factory.EmailService.SendEmail(email);
                                if (success)
                                {
                                    email.Status = 1;
                                    item.status = "Success";
                                }
                                //send completed then must keep log
                                AddEmailLog(email);
                            }
                            catch (Exception e)
                            {
                                Log.WriteLogs(e);
                            }
                        }
                        item.lastsenddate = DateTime.Now;
                        if (item.status == "Retry")
                        {
                            item.retry++;
                            if (item.retry > ParameterServices.MaxRetry)
                            {
                                item.status = "Fail";
                            }
                        }
                        Factory.SuEmailResendingService.Update(item);
                    }
                    Log.WriteMsgLogs("Finish");
                    Thread.Sleep(SS.DB.Query.ParameterServices.EmailFlushingDuration * 60000);
                }
                catch (Exception e)
                {
                    Log.WriteLogs(e);
                    Thread.Sleep(SS.DB.Query.ParameterServices.EmailFlushingDuration * 60000);
                    continue;
                }
            }
        }
    }
}
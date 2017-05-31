using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.CommunicationService.DTO;
using SS.Standard.CommunicationService;
using SS.SU.Query;
using SS.DB.Query;
using SS.SU.DTO;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.Query;
using SS.Standard.WorkFlow.Query.Hibernate;
using SCG.eAccounting.DTO;
using SCG.eAccounting.Query;
using SS.Standard.Security;
using SS.Standard.WorkFlow.Service;
using SCG.eAccounting.DTO.ValueObject;
using SS.Standard.Utilities;
using SS.Standard.WorkFlow.Service.Implement;
using SCG.DB.Query;

namespace SCG.eAccounting.BLL.Implement
{
    
    public class SCGSMSService : ISCGSMSService
    {
        #region ประกาศตัวแปร
        public IUserAccount UserAccount { get; set; }
        public IEmailService EmailService { get; set; }
        public IWorkFlowResponseTokenQuery WorkFlowResponseTokenQuery { get; set; }
        public IWorkFlowService WorkFlowService { get; set; }
        public IWorkFlowStateEventPermissionService WorkFlowStateEventPermissionService { get; set; }
        public IWorkFlowResponseTokenService WorkFlowResponseTokenService { get; set; }
        //public ISCGSMSService SCGSMSService { get; set; }
        public IWorkFlowsmsTokenService WorkFlowsmsTokenService { get; set; }
        public ISCGDocumentService SCGDocumentService { get; set; }

        public IDbParameterQuery DbParameterQuery { get; set; }
        //public IWorkFlowsmsTokenService WorkFlowsmsTokenService { get; set; }
        public IWorkFlowStateEventQuery WorkFlowStateEventQuery { get; set; }
        public ISMSService SMSService { get; set; }
        private StringBuilder smsContentBody;
        private bool sendSMSStatus;

        #endregion ประกาศตัวแปร
        public bool Status 
        {
            get { return this.sendSMSStatus; }
        }
        #region ISCGSMSService Members

        private string SaveResponseTokenSMS(long workFlowID, long userID)
        {
            WorkFlowResponseTokenService.ClearResponseTokenByWorkFlowID(workFlowID,TokenType.SMS);

            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindProxyByIdentity(workFlowID);

            //string tokenCode = WorkFlowsmsTokenService.GetRunning().ToString().PadLeft(5,'0');
            string tokenCode = WorkFlowsmsTokenService.GetSMSTokenCode(workFlowID).Substring(0, 5);
            IList<WorkFlowStateEventPermission> stateEventPermissions = WorkFlowQueryProvider.WorkFlowStateEventPermissionQuery.FindByWorkFlowID_UserID(workFlowID, userID);
            foreach (WorkFlowStateEventPermission stateEventPermission in stateEventPermissions)
            {
                WorkFlowResponseToken responseToken = new WorkFlowResponseToken();
                responseToken.TokenCode = "1"+tokenCode;
                responseToken.UserID = stateEventPermission.UserID.Value;
                responseToken.WorkFlow = workFlow;
                responseToken.TokenType = TokenType.SMS.ToString();
                responseToken.WorkFlowStateEvent = stateEventPermission.WorkFlowStateEvent;
                responseToken.Active = true;
                responseToken.CreBy = UserAccount.UserID;
                responseToken.CreDate = DateTime.Now;
                responseToken.UpdBy = UserAccount.UserID;
                responseToken.UpdDate = DateTime.Now;
                responseToken.UpdPgm = UserAccount.CurrentProgramCode;

                WorkFlowResponseTokenService.Save(responseToken);
            }



            return tokenCode;
        }

        public void SendSMS01(long workFlowID, long sendToUserID)
        {

            try
            {
                if (ParameterServices.EnableSMS)
                {
                    string tokenSMSCode = SaveResponseTokenSMS(workFlowID, sendToUserID);

                    SuUser sendToUser = QueryProvider.SuUserQuery.FindByIdentity(sendToUserID);
                    if (sendToUser.SMSApproveOrReject)
                    {
                        IList<WorkFlowResponseToken> responseTokens = WorkFlowQueryProvider.WorkFlowResponseTokenQuery.FindByTokenCode(tokenSMSCode);
                        Document document = WorkFlowQueryProvider.WorkFlowQuery.GetDocumentByWorkFlowID(workFlowID);
                        SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.FindByIdentity(document.DocumentID);
                        SuUser RequesterUser = QueryProvider.SuUserQuery.FindByIdentity(scgDocument.RequesterID.Userid);

                        SMSDTO smsDto = new SMSDTO();
                        smsDto.From = SS.DB.Query.ParameterServices.SMSPhoneNumber;
                        string Mobile = "66" + sendToUser.MobilePhoneNo.ToString().Remove(0, 1);
                        smsDto.To = Mobile;
                        smsDto.ReferenceID = tokenSMSCode;
                        smsDto.Requestor = RequesterUser.UserName;
                        smsDto.DocumentNo = scgDocument.DocumentNo;
                        smsDto.UseProxy = true;
                        if (scgDocument.DocumentType.DocumentTypeID.Equals(DocumentTypeID.AdvanceDomesticDocument) || scgDocument.DocumentType.DocumentTypeID.Equals(DocumentTypeID.AdvanceForeignDocument))
                        {
                            AvAdvanceDocument avanceDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(scgDocument.DocumentID);
                            if (scgDocument.DocumentType.DocumentTypeID.Equals(DocumentTypeID.AdvanceForeignDocument))
                            {

                                //ต่างประเทศจะต้องไป get Currency Symbol ออกมา
                                if (avanceDocument != null)
                                {
                                    smsDto.Amount = avanceDocument.Amount.ToString();
                                    
                                }
                                else
                                {
                                    smsDto.Amount = "0";
                                }
                                IList<AvAdvanceItem> advanceItemList = ScgeAccountingQueryProvider.AvAdvanceItemQuery.FindByAvAdvanceItemAdvanceID(avanceDocument.AdvanceID);
                                IList<SMSCurrencyDTO> advItemList = new List<SMSCurrencyDTO>();
                                foreach (AvAdvanceItem advanceItem in advanceItemList)
                                {
                                    string dbStatus = ScgDbQueryProvider.SCGDbStatusLangQuery.GetStatusLang("PaymentTypeFRN", ParameterServices.DefaultLanguage, advanceItem.PaymentType);
                                    SMSCurrencyDTO advItem = new SMSCurrencyDTO();
                                    advItem.PaymentType = dbStatus;
                                    if (advanceItem.CurrencyID != null)
                                        advItem.Currency = advanceItem.CurrencyID.Symbol;
                                    advItem.Amount = advanceItem.Amount.ToString();
                                    advItemList.Add(advItem);
                                }
                                smsDto.CurrencyItemList = advItemList;

                                smsDto.Content = BuildContentBody(smsDto, SMSContenFormat.SMS01, SMSBusinessCase.AdvanceForeignDocument);

                            }
                            else
                            {
                                //ในประเทศ Currency Symbol THB เท่านั้น
                                if (avanceDocument != null)
                                {
                                    smsDto.Amount = avanceDocument.Amount.ToString();
                                }
                                else
                                {
                                    smsDto.Amount = "0";
                                }
                                smsDto.Content = BuildContentBody(smsDto, SMSContenFormat.SMS01, SMSBusinessCase.AdvanceDomesticDocument);

                            }
                            this.sendSMSStatus = SMSService.Send(smsDto);

                        }
                        else if (scgDocument.DocumentType.DocumentTypeID.Equals(DocumentTypeID.ExpenseDomesticDocument) || scgDocument.DocumentType.DocumentTypeID.Equals(DocumentTypeID.ExpenseForeignDocument))
                        {
                            FnExpenseDocument expenseDocument = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(scgDocument.DocumentID);

                            if (scgDocument.DocumentType.DocumentTypeID.Equals(DocumentTypeID.ExpenseForeignDocument))
                            {

                                FnExpenseDocument expense = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(scgDocument.DocumentID);
                                //EmailValueObject expenseDoc = new EmailValueObject();
                               // expenseDoc.RequestID = expense.Document.DocumentNo;

                                smsDto.Amount = expense.TotalExpense.ToString();
                                smsDto.Content = BuildContentBody(smsDto, SMSContenFormat.SMS01, SMSBusinessCase.ExpenseDomesticDocument);

                            }
                            else
                            {
                                if (expenseDocument != null)
                                {
                                    smsDto.Amount = expenseDocument.TotalExpense.ToString();
                                }
                                else
                                {
                                    smsDto.Amount = "0";
                                }
                                smsDto.Content = BuildContentBody(smsDto, SMSContenFormat.SMS01, SMSBusinessCase.ExpenseDomesticDocument);

                            }

                            this.sendSMSStatus = SMSService.Send(smsDto);

                        }
                        else if (scgDocument.DocumentType.DocumentTypeID.Equals(DocumentTypeID.TADocumentDomestic) || scgDocument.DocumentType.DocumentTypeID.Equals(DocumentTypeID.TADocumentForeign))
                        {
                            TADocument ta = ScgeAccountingQueryProvider.TADocumentQuery.GetTADocumentByDocumentID(scgDocument.DocumentID);
                            IList<Advance> advanceList = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindAdvanceDocumentByTADocumentID(ta.TADocumentID);
                            if (advanceList.Count > 0)
                            {
                                IList<AdvanceData> advanceDataList = new List<AdvanceData>();
                                foreach (Advance advance in advanceList)
                                {
                                    AdvanceData advanceData = new AdvanceData();
                                    AvAdvanceDocument advDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindByIdentity(advance.AdvanceID);
                                    if (advDocument != null)
                                    {
                                        //advanceData.RequestID = advDocument.DocumentID.RequesterID;
                                        advanceData.Subject = advDocument.DocumentID.Subject;
                                        advanceData.RequestDateOfAdvance = advDocument.RequestDateOfAdvance;
                                        advanceData.Amount = advDocument.Amount;
                                        if (advDocument.AdvanceType.Equals("FR"))
                                        {
                                            IList<AvAdvanceItem> advItemList = ScgeAccountingQueryProvider.AvAdvanceItemQuery.FindByAvAdvanceItemAdvanceID(advDocument.AdvanceID);
                                            IList<SMSCurrencyDTO> advanceFrList = new List<SMSCurrencyDTO>();
                                            foreach (AvAdvanceItem advItem in advItemList)
                                            {
                                                SMSCurrencyDTO advanceFR = new SMSCurrencyDTO();
                                                advanceFR.Currency = advItem.CurrencyID.Symbol;
                                                advanceFR.Amount = advItem.Amount.ToString();
                                                advanceFrList.Add(advanceFR);
                                            }
                                            smsDto.CurrencyItemList = advanceFrList;

                                        }
                                        advanceDataList.Add(advanceData);
                                    }
                                }
                               // html.SetAttribute("advList", advanceDataList);
                            }
                           // html.SetAttribute("ta", taDoc);

                                smsDto.Content = BuildContentBody(smsDto, SMSContenFormat.SMS01, SMSBusinessCase.TADocumentDomestic);

                           

                            this.sendSMSStatus = SMSService.Send(smsDto);
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                Utilities.WriteLogs("SendSMS01 : " + ex.Message, "smslog", "Logs", "txt");

            }
           
            
        }
        public void SendSMS02(long workFlowID, string RequestID, IList<long> ReciverList, bool isAutoPayment)
        {
            double netAmount = 0;
            try
            {
                if (ParameterServices.EnableSMS)
                {
                    Document document = WorkFlowQueryProvider.WorkFlowQuery.GetDocumentByWorkFlowID(workFlowID);
                    SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.FindByIdentity(document.DocumentID);
                   
                    if (!isAutoPayment) // Cash
                    {
                        #region Cash
                        if (scgDocument.DocumentType.DocumentTypeID.Equals(DocumentTypeID.ExpenseDomesticDocument) || (scgDocument.DocumentType.DocumentTypeID.Equals(DocumentTypeID.ExpenseForeignDocument)))
                        {
                            FnExpenseDocument expense = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(scgDocument.DocumentID);
                            if (expense != null)
                            {
                                netAmount = expense.DifferenceAmount;
                            }
                        }
                        else if (scgDocument.DocumentType.DocumentTypeID.Equals(DocumentTypeID.AdvanceDomesticDocument) || (scgDocument.DocumentType.DocumentTypeID.Equals(DocumentTypeID.AdvanceForeignDocument)))
                        {
                            AvAdvanceDocument advance = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(scgDocument.DocumentID);
                            
                            if (advance != null)
                            {
                               netAmount = advance.Amount;
                            }
                        }
                        #endregion Cash

                        foreach (long userID in ReciverList)
                        {
                            SuUser sendToUser = QueryProvider.SuUserQuery.FindByIdentity(userID);
                            if (sendToUser.SMSReadyToReceive)
                            {
                                string ToPhoneNo = sendToUser.MobilePhoneNo.ToString();
                                SMSDTO smsDto = new SMSDTO();
                                string Mobile = "66" + ToPhoneNo.ToString().Remove(0, 1);
                                smsDto.Requestor = RequestID;
                                smsDto.DocumentNo = scgDocument.DocumentNo;
                                smsDto.Amount = netAmount.ToString();
                                smsDto.To = Mobile;
                                smsDto.UseProxy = true;
                                smsDto.Content = BuildContentBody(smsDto, SMSContenFormat.SMS02, SMSBusinessCase.Cash);
                                this.sendSMSStatus = SMSService.Send(smsDto);
                            }
                        }
                    }
                    else // Cheque
                    {
                        #region Cheque
                        FnAutoPayment autoPayment = ScgeAccountingQueryProvider.FnAutoPaymentQuery.GetFnAutoPaymentByDocumentID(document.DocumentID);
                        netAmount = autoPayment.Amount;
                        #endregion Cheque

                        foreach (long userID in ReciverList)
                        {
                            SuUser sendToUser = QueryProvider.SuUserQuery.FindByIdentity(userID);
                            if (sendToUser.SMSReadyToReceive)
                            {
                                string ToPhoneNo = sendToUser.MobilePhoneNo.ToString();
                                SMSDTO smsDto = new SMSDTO();
                                string Mobile = "66" + ToPhoneNo.ToString().Remove(0, 1);
                                smsDto.Requestor = RequestID;
                                smsDto.DocumentNo = scgDocument.DocumentNo;
                                smsDto.PaymentDate = autoPayment.PaymentDate.Value;
                                smsDto.Amount = netAmount.ToString();
                                smsDto.To = Mobile;
                                smsDto.UseProxy = true;
                                smsDto.Content = BuildContentBody(smsDto, SMSContenFormat.SMS02, SMSBusinessCase.Cheque);
                                this.sendSMSStatus = SMSService.Send(smsDto);
                            }
                        }
                    }

                   
                }
            }
            catch (Exception ex)
            {

                Utilities.WriteLogs("SendSMS02 : ==> " + ex.Message, "smslog", "Logs", "txt");
            }
           
        }
        public void SendSMS03(long workFlowID, string RequestID, IList<long> ReciverList)
        {
            try
            {
                if (ParameterServices.EnableSMS)
                {
                    Document document = WorkFlowQueryProvider.WorkFlowQuery.GetDocumentByWorkFlowID(workFlowID);
                    SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.FindByIdentity(document.DocumentID);
                    FnAutoPayment autoPayment = ScgeAccountingQueryProvider.FnAutoPaymentQuery.GetFnAutoPaymentByDocumentID(document.DocumentID);
                   
                    foreach (long userID in ReciverList)
                    {
                        SuUser sendToUser = QueryProvider.SuUserQuery.FindByIdentity(userID);
                        if (sendToUser.SMSReadyToReceive)
                        {
                            string ToPhoneNo = sendToUser.MobilePhoneNo.ToString();
                            SMSDTO smsDto = new SMSDTO();
                            string Mobile = "66" + ToPhoneNo.ToString().Remove(0, 1);
                            smsDto.To = Mobile;
                            smsDto.PaymentDate = autoPayment.PaymentDate.Value;
                            smsDto.Amount = autoPayment.Amount.ToString();
                            smsDto.Requestor = RequestID;
                            smsDto.DocumentNo = scgDocument.DocumentNo;
                            smsDto.UseProxy = true;
                            smsDto.Content = BuildContentBody(smsDto, SMSContenFormat.SMS03, SMSBusinessCase.Cheque);
                            this.sendSMSStatus = SMSService.Send(smsDto);
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                Utilities.WriteLogs("SendSMS03 : ==> " + ex.Message, "smslog", "Logs", "txt");
            }
            
           
        }
        public void SendSMS04(string Refno, long UserID,bool NoitfySMS)
        {
            try
            {
                if (ParameterServices.EnableSMS)
                {

                    SuUser sendToUser = QueryProvider.SuUserQuery.FindByIdentity(UserID);
                    if (sendToUser.SMSApproveOrReject)
                    {
                        SMSDTO smsDto = new SMSDTO();
                        smsDto.From = SS.DB.Query.ParameterServices.SMSPhoneNumber;
                        string Mobile = "66" + sendToUser.MobilePhoneNo.ToString().Remove(0, 1);
                        smsDto.To = Mobile;
                        smsDto.UseProxy = true;
                        try
                        {
                            smsDto.Content = System.Web.HttpUtility.HtmlEncode(string.Format(ParameterServices.NotifyApproveWrongFormat, Refno));
                        }
                        catch (Exception ex)
                        {
                            string error = ex.Message;
                            smsDto.Content = System.Web.HttpUtility.HtmlEncode("To approve Refno." + Refno + " format error, Please approve again. Try send XXXXXY=Approve or XXXXXN=Reject");

                        }
                        this.sendSMSStatus = SMSService.Send(smsDto,NoitfySMS);

                    }
                }

            }
            catch (Exception ex)
            {

                Utilities.WriteLogs("To approve Failed : " + ex.Message, "smslog", "Logs", "txt");

            }
           
        }
        public void SendSMS05(string RefNo, long UserID,bool NotifySMS,string ApproveFlag)
        {
            try
            {
                if (ParameterServices.EnableSMS)
                {

                    SuUser sendToUser = QueryProvider.SuUserQuery.FindByIdentity(UserID);
                    if (sendToUser.SMSApproveOrReject)
                    {
                        SMSDTO smsDto = new SMSDTO();
                        smsDto.From = SS.DB.Query.ParameterServices.SMSPhoneNumber;
                        string Mobile = "66" + sendToUser.MobilePhoneNo.ToString().Remove(0, 1);
                        smsDto.To = Mobile;
                        smsDto.UseProxy = true;
                        try
                        {
                            smsDto.Content = System.Web.HttpUtility.HtmlEncode(string.Format(ParameterServices.NotifyApprovedCompleted, RefNo, ApproveFlag.ToLower()));

                        }
                        catch (Exception ex)
                        {
                            string error = ex.Message;
                            smsDto.Content = System.Web.HttpUtility.HtmlEncode("To approve Ref." + RefNo + " was " + ApproveFlag.ToLower() + " completed");
                        }
                        this.sendSMSStatus = SMSService.Send(smsDto,NotifySMS);

                    }
                }

            }
            catch (Exception ex)
            {

                Utilities.WriteLogs("To approve Failed : " + ex.Message, "smslog", "Logs", "txt");

            }
        }
        /// <summary>
        /// กรณี login failed  ทำให้ไม่สามารถ approve ได้
        /// </summary>
        /// <param name="Refno"></param>
        /// <param name="UserID"></param>
        /// <param name="NotifySMS"></param>
        public void SendSMS06(string Refno,long UserID,bool NotifySMS)
        {
            try
            {
                if (ParameterServices.EnableSMS)
                {

                    SuUser sendToUser = QueryProvider.SuUserQuery.FindByIdentity(UserID);
                    if (sendToUser.SMSApproveOrReject)
                    {
                        SMSDTO smsDto = new SMSDTO();
                        smsDto.From = SS.DB.Query.ParameterServices.SMSPhoneNumber;
                        string Mobile = "66" + sendToUser.MobilePhoneNo.ToString().Remove(0, 1);
                        smsDto.To = Mobile;
                        smsDto.UseProxy = true;
                        try
                        {
                            smsDto.Content = System.Web.HttpUtility.HtmlEncode(string.Format(ParameterServices.NotifyLoginFailed, Refno));

                        }
                        catch (Exception ex)
                        {
                            string error = ex.Message;
                            smsDto.Content = System.Web.HttpUtility.HtmlEncode(string.Format("To approve Ref.{0} via sms was login failed, Please contact admin.   Send by: SCG Account", Refno));

                        }
                        this.sendSMSStatus = SMSService.Send(smsDto,NotifySMS);

                    }
                }

            }
            catch (Exception ex)
            {

                Utilities.WriteLogs("To approve Failed : " + ex.Message, "smslog", "Logs", "txt");

            }
        }
        /// <summary>
        /// กรณี ส่งมา approve ผิด format login ไม่ได้
        /// </summary>
        /// <param name="Refno"></param>
        /// <param name="mobilePhone"></param>
        /// <param name="NoitfySMS"></param>
        public void SendSMS07(string Refno, string mobilePhone, bool NoitfySMS)
        {
            try
            {
                if (ParameterServices.EnableSMS)
                {

                   
                        SMSDTO smsDto = new SMSDTO();
                        smsDto.From = SS.DB.Query.ParameterServices.SMSPhoneNumber;
                        string Mobile = "66" + mobilePhone.ToString().Remove(0, 1);
                        smsDto.To = Mobile;
                        smsDto.UseProxy = true;
                        try
                        {
                        smsDto.Content = System.Web.HttpUtility.HtmlEncode(string.Format(ParameterServices.NotifyApproveWrongFormat, Refno));
                        }
                        catch (Exception ex)
                        {
                            string error = ex.Message;
                            smsDto.Content = System.Web.HttpUtility.HtmlEncode(string.Format("To approve Refno. {0} format error, Please approve again. Try send XXXXXXY=Approve or XXXXXXN=Reject", Refno));
                        }
                        this.sendSMSStatus = SMSService.Send(smsDto, NoitfySMS);

                   
                }

            }
            catch (Exception ex)
            {

                Utilities.WriteLogs("To approve Failed : " + ex.Message, "smslog", "Logs", "txt");

            }

        }
        /// <summary>
        /// ใช้กรณี มีการ approve รอบที่สอง จะทำให้หา tokencode ไม่เจอ
        /// </summary>
        /// <param name="Refno"></param>
        /// <param name="mobilePhone"></param>
        /// <param name="NoitfySMS"></param>
        public void SendSMS08(string Refno, string mobilePhone, bool NoitfySMS)
        {

            try
            {
                if (ParameterServices.EnableSMS)
                {

                    SMSDTO smsDto = new SMSDTO();
                    smsDto.From = SS.DB.Query.ParameterServices.SMSPhoneNumber;
                    string Mobile = "66" + mobilePhone.ToString().Remove(0, 1);
                    smsDto.To = Mobile;
                    smsDto.UseProxy = true;
                    try
                    {
                        smsDto.Content = System.Web.HttpUtility.HtmlEncode(string.Format(ParameterServices.NotifyTokenCodeNotFound, Refno));
                    }
                    catch (Exception ex)
                    {
                        string error = ex.Message;
                        smsDto.Content = System.Web.HttpUtility.HtmlEncode(string.Format("To approve Ref.{0} not found.", Refno));

                    }
                    this.sendSMSStatus = SMSService.Send(smsDto, NoitfySMS);


                }

            }
            catch (Exception ex)
            {

                Utilities.WriteLogs("To approve Failed : " + ex.Message, "smslog", "Logs", "txt");

            }

        }
        /// <summary>
        /// ใช้กรณี มีการ approve ผ่านช่องทางอื่นที่ไม่ใช่ sms แต่ว่าเพิ่งได้รับ message แล้ว approve มาจะทำให้ work flow เกิด error ไม่สามารถ approve ได้
        /// </summary>
        /// <param name="Refno"></param>
        /// <param name="mobilePhone"></param>
        /// <param name="NoitfySMS"></param>
        public void SendSMS09(string Message,string Refno, string mobilePhone, bool NoitfySMS)
        {

            try
            {
                if (ParameterServices.EnableSMS)
                {

                    SMSDTO smsDto = new SMSDTO();
                    smsDto.From = SS.DB.Query.ParameterServices.SMSPhoneNumber;
                    string Mobile = "66" + mobilePhone.ToString().Remove(0, 1);
                    smsDto.To = Mobile;
                    smsDto.UseProxy = true;
                    try
                    {
                        smsDto.Content = System.Web.HttpUtility.HtmlEncode(string.Format(ParameterServices.NotifyDocStatusChanged, Refno));
                    }
                    catch (Exception ex)
                    {
                        string error = ex.Message;
                        smsDto.Content = System.Web.HttpUtility.HtmlEncode(string.Format("This refno. {0} has been changed on work flow event.", Refno));

                    }
                    this.sendSMSStatus = SMSService.Send(smsDto, NoitfySMS);


                }

            }
            catch (Exception ex)
            {

                Utilities.WriteLogs("To approve Failed : " + ex.Message, "smslog", "Logs", "txt");

            }

        }
        /// <summary>
        /// ใช้กรณี มีการ approve ผ่านเบอร์คนอื่นที่ไม่ใช่ เบอร์ที่รับ message 
        /// </summary>
        /// <param name="Refno"></param>
        /// <param name="mobilePhone"></param>
        /// <param name="NoitfySMS"></param>
        public void SendSMS10(string Refno, string mobilePhone, bool NoitfySMS)
        {

            try
            {
                if (ParameterServices.EnableSMS)
                {

                    SMSDTO smsDto = new SMSDTO();
                    smsDto.From = SS.DB.Query.ParameterServices.SMSPhoneNumber;
                    string Mobile = "66" + mobilePhone.ToString().Remove(0, 1);
                    smsDto.To = Mobile;
                    smsDto.UseProxy = true;
                    try
                    {
                        smsDto.Content = System.Web.HttpUtility.HtmlEncode(string.Format(ParameterServices.NotifyWrongMobileNumber, Refno));
                    }
                    catch (Exception ex)
                    {
                        string error = ex.Message;
                        smsDto.Content = System.Web.HttpUtility.HtmlEncode(string.Format("Can use mobile number's message box owner for approve/reject refno.{0} only", Refno));

                    }
                    this.sendSMSStatus = SMSService.Send(smsDto, NoitfySMS);


                }

            }
            catch (Exception ex)
            {

                Utilities.WriteLogs("To approve Failed : " + ex.Message, "smslog", "Logs", "txt");

            }

        }
        #endregion

        private void NotifyCashReceive(long workFlowID, string RequestID, IList<long> ReciverList)
        {
        
        }
        private void NotifyChequeReceive(long workFlowID, string RequestID, IList<long> ReciverList)
        {
        
        }


        private string BuildContentBody(SMSDTO smsDTO, SMSContenFormat ContentFormat, SMSBusinessCase BusinessCase)
        {
            smsContentBody = new StringBuilder(string.Empty);
            try
            {
                if (ContentFormat == SMSContenFormat.SMS01)
                {
                    if (BusinessCase == SMSBusinessCase.AdvanceDomesticDocument)
                    {
                        try
                        {
                            smsContentBody.Append(string.Format(ParameterServices.NotifyRequestApproveAdvance, smsDTO.Requestor,smsDTO.Amount, smsDTO.DocumentNo, smsDTO.ReferenceID));

                        }
                        catch (Exception)
                        {

                            smsContentBody.AppendFormat("Please approve advance of {0} {1} {2} {3} Ref.{4}", smsDTO.Requestor,"THB "+smsDTO.Amount, smsDTO.DocumentNo, smsDTO.ReferenceID);

                        }

                    }
                    else if (BusinessCase == SMSBusinessCase.AdvanceForeignDocument)
                    {
                        IList<SMSCurrencyDTO> advnItemList = smsDTO.CurrencyItemList;
                        StringBuilder strAmount = new StringBuilder();
                        string smsAmount = string.Empty;
                        if (advnItemList != null && advnItemList.Count > 0)
                        {
                            foreach (SMSCurrencyDTO itemCurrency in advnItemList)
                            {
                                strAmount.AppendFormat("{0} {1},", itemCurrency.Currency.ToString(), itemCurrency.Amount.ToString());
                            }
                            smsAmount = strAmount.ToString().TrimEnd(',');
                        }
                        else
                        {
                            smsAmount = smsDTO.Amount;
                        }

                        try
                        {
                            smsContentBody.Append(string.Format(ParameterServices.NotifyRequestApproveAdvance, smsDTO.Requestor,  smsAmount , smsDTO.DocumentNo, smsDTO.ReferenceID));

                        }
                        catch (Exception)
                        {

                            smsContentBody.AppendFormat("Please approve advance of {0} {1} {2} {3} Ref.{4}", smsDTO.Requestor, "THB", smsAmount , smsDTO.DocumentNo, smsDTO.ReferenceID);

                        }
                    }
                    else if (BusinessCase == SMSBusinessCase.ExpenseDomesticDocument)
                    {
                        try
                        {
                            smsContentBody.Append(string.Format(ParameterServices.NotifyRequestApproveExpense, smsDTO.Requestor,smsDTO.Amount, smsDTO.DocumentNo, smsDTO.ReferenceID));

                        }
                        catch (Exception ex)
                        {
                            string error = ex.Message;
                            smsContentBody.AppendFormat("Please approve expense of {0} THB {1} {2} Ref.{3} ", smsDTO.Requestor, smsDTO.Amount, smsDTO.DocumentNo, smsDTO.ReferenceID);

                        }
                    }
                    else if (BusinessCase == SMSBusinessCase.ExpenseForeignDocument)
                    {
                        try
                        {
                            smsContentBody.Append(string.Format(ParameterServices.NotifyRequestApproveExpense, smsDTO.Requestor,  smsDTO.Amount, smsDTO.DocumentNo, smsDTO.ReferenceID));

                        }
                        catch (Exception ex)
                        {
                            string error = ex.Message;
                            smsContentBody.AppendFormat("Please approve expense of {0} THB {1} {2} Ref.{3} ", smsDTO.Requestor, smsDTO.Amount, smsDTO.DocumentNo, smsDTO.ReferenceID);

                        }
                    }
                    else if (BusinessCase == SMSBusinessCase.TADocumentDomestic || BusinessCase == SMSBusinessCase.TADocumentForeign)
                    {
                        IList<SMSCurrencyDTO> advnItemList = smsDTO.CurrencyItemList;
                        StringBuilder strAmount = new StringBuilder();
                        string smsAmount = string.Empty;
                        if (advnItemList != null && advnItemList.Count > 0)
                        {
                            foreach (SMSCurrencyDTO itemCurrency in advnItemList)
                            {
                                strAmount.AppendFormat("{0}{1},", itemCurrency.Currency.ToString(), itemCurrency.Amount.ToString());
                            }
                            smsAmount = strAmount.ToString().TrimEnd(',');

                            try
                            {
                                smsContentBody.Append(string.Format(ParameterServices.NotifyRequestApproveTAandAdvance, smsDTO.Requestor,smsAmount, smsDTO.DocumentNo, smsDTO.ReferenceID));

                            }
                            catch (Exception ex)
                            {
                                string error = ex.Message;
                                smsContentBody.AppendFormat("Please approve TA+ADV of {0}{1} {2} Ref.{3}", smsDTO.Requestor, smsAmount,smsDTO.DocumentNo, smsDTO.ReferenceID);

                            }
                        }
                        else
                        {
                            try
                            {
                                smsContentBody.Append(string.Format(ParameterServices.NotifyRequestApproveTA, smsDTO.Requestor, smsDTO.DocumentNo, smsDTO.ReferenceID));

                            }
                            catch (Exception ex)
                            {
                                string error = ex.Message;
                                smsContentBody.AppendFormat("Please approve TA of {0}{1} Ref.{2}", smsDTO.Requestor, smsDTO.DocumentNo, smsDTO.ReferenceID);

                            }
                        }


                        
                    }
                }
                else if (ContentFormat == SMSContenFormat.SMS02)
                {
                    if (BusinessCase == SMSBusinessCase.Cash)
                    {
                        try
                        {
                            smsContentBody.Append(string.Format(ParameterServices.NotifyCashReceive, smsDTO.DocumentNo,smsDTO.Amount));
                        }
                        catch (Exception ex)
                        {
                            string errer = ex.Message;
                            smsContentBody.AppendFormat("Your payment request has been made,RefID:{0} Amount {1} Please contact counter cashier.  Sent by: SCG Account", smsDTO.DocumentNo,smsDTO.Amount);
                        }
                    }
                    else if (BusinessCase == SMSBusinessCase.Cheque)
                    {
                        try
                        {
                            //smsContentBody.Append(string.Format(ParameterServices.NotifyChequeReceive, smsDTO.DocumentNo, smsDTO.Amount));
                            //comment for edit format by desh : log no. 
                            smsContentBody.Append(string.Format(ParameterServices.NotifyChequeReceive, smsDTO.PaymentDate.ToString("dd/MM/yyyy"), smsDTO.Amount, smsDTO.DocumentNo));
                        }
                        catch (Exception ex)
                        {
                            string errer = ex.Message;
                            //smsContentBody.AppendFormat("Your payment request has been made,RefID:{0} Amount {1}  Sent by: SCG Account", smsDTO.DocumentNo,smsDTO.Amount);
                            smsContentBody.AppendFormat("Your payment request has been made,Payment date: {0} Amount. {1} baht RefID: {2}. Please contact counter cashier. Send by: SCG Account", smsDTO.PaymentDate.ToString("dd/MM/yyyy"), smsDTO.Amount, smsDTO.DocumentNo);
                        }

                    }
                }
                else if (ContentFormat == SMSContenFormat.SMS03)
                {
                    if (BusinessCase == SMSBusinessCase.Cheque)
                    {
                        try
                        {
                            smsContentBody.Append(string.Format(ParameterServices.NotifyChequeReceive,smsDTO.PaymentDate.ToString("dd/MM/yyyy"),smsDTO.Amount ,smsDTO.DocumentNo ));
                        }
                        catch (Exception ex)
                        {
                            string error = ex.Message;
                            smsContentBody.AppendFormat("Your payment request has been made,Payment date: {0} Amount. {1} baht RefID: {2}. Please contact counter cashier. Send by: SCG Account",smsDTO.PaymentDate.ToString("dd/MM/yyyy"),smsDTO.Amount ,smsDTO.DocumentNo );
                        }
                    }
                    else
                    {
                        try
                        {
                            smsContentBody.Append(string.Format(ParameterServices.NotifyChequeReceive, smsDTO.DocumentNo, smsDTO.Amount, smsDTO.PaymentDate.ToString("dd/MM/yyyy")));
                        }
                        catch (Exception ex)
                        {
                            string error = ex.Message;
                            smsContentBody.AppendFormat("Your payment request has been made,Payment date: {0} Amount. {1} baht RefID: {2}.  Sent by: SCG Account", smsDTO.PaymentDate.ToString("dd/MM/yyyy"), smsDTO.Amount, smsDTO.DocumentNo);
                        }

                    }

                }
                else
                {
                    smsContentBody.AppendFormat(smsDTO.Content);
                }
            }
            catch (Exception ex)
            {

                Utilities.WriteLogs("BuildSMSContentBody : " + BusinessCase.ToString() + "==>" + ex.Message, "smslog", "Logs", "txt");

            }
           

            return System.Web.HttpUtility.HtmlEncode(smsContentBody.ToString());
        }



      
    }
}

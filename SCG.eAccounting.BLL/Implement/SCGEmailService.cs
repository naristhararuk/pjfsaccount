using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.CommunicationService;
using SS.Standard.CommunicationService.DTO;
using SS.Standard.Utilities;

using SS.SU.DTO;
using SS.SU.Query;

using SS.Standard.WorkFlow;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.Service;
using SS.Standard.WorkFlow.Query;
using SS.Standard.WorkFlow.EventUserControl;
using SS.Standard.WorkFlow.DAL;
using SS.Standard.CommunicationService.Implement;
using Antlr.StringTemplate;
using SCG.eAccounting.DTO;
using SCG.eAccounting.Query;
using SCG.eAccounting.DAL;
using SS.Standard.Security;
using SCG.eAccounting.DTO.ValueObject;
using SCG.DB.DTO;
using SCG.DB.Query;
using SS.Standard.WorkFlow.DTO.ValueObject;
using SS.DB.Query;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SS.SU.BLL;
using System.Xml.Serialization;
using SCG.DB.DTO.ValueObject;
using SS.DB.DTO;

namespace SCG.eAccounting.BLL.Implement
{

    public class SCGEmailService : ISCGEmailService
    {
        #region ประกาศตัวแปร
        public IUserAccount UserAccount { get; set; }
        public IEmailService EmailService { get; set; }
        public IWorkFlowResponseTokenQuery WorkFlowResponseTokenQuery { get; set; }
        public IWorkFlowService WorkFlowService { get; set; }
        public ISuEmailLogDao SuEmailLogDao { get; set; }
        public IDbParameterQuery DbParameterQuery { get; set; }
        public IWorkFlowStateEventQuery WorkFlowStateEventQuery { get; set; }
        public ISuEmailResendingService SuEmailResendingService { get; set; }
        public ISMSService SMSService { get; set; }
        public IDbDictionaryQuery DbDictionaryQuery { get; set; }
        #endregion ประกาศตัวแปร

        #region Other
        private long SystemUserID
        {
            get
            {
                return Convert.ToInt64(ParameterServices.SystemUserID);
            }
        }
        public void SendEmail(EmailDTO email)
        {

            EmailSerializer serializer = new EmailSerializer();
            SuEmailResending emailResend = new SuEmailResending();
            emailResend.emailtype = email.EmailType;
            emailResend.Creby = ParameterServices.SystemUserID;
            emailResend.CreDate = DateTime.Now;
            emailResend.lastsenddate = DateTime.Now.AddMinutes(ParameterServices.EmailPendingDuration * -1);
            emailResend.mailcontent = serializer.SerializeObject(email);
            emailResend.retry = 0;
            try
            {
                emailResend.sendto = email.MailSendTo[0].Email;
            }
            catch (Exception)
            {

                emailResend.sendto = "";
            }

            emailResend.status = "New";
            emailResend.subject = email.MailSubject;
            SuEmailResendingService.Save(emailResend);

        }
        public class AdvanceData
        {
            public string RequestID { get; set; }
            public string Subject { get; set; }
            public string RequestDateOfAdvance { get; set; }
            public string Amount { get; set; }
            public IList<AdvanceCurrencyData> AdvanceFrList { get; set; }
            public string IsRepOffice { get; set; }
            public int CurrencyID { get; set; }
            public string MainCurrencySymbol { get; set; }
            public string SymbolLocal { get; set; }
            public string SymbolMain { get; set; }
            public double? advitemMainCurrencyAmount { get; set; }
            public double? advdocMainCurrencyAmount { get; set; }
            public double? advdocLocalCurrencyAmount { get; set; }
            public string AdvanceType { get; set; }
            public long? PBID { get; set; }
        }


        public class FixedAdvanceData
        {
            public string RequesterName { get; set; }
            public string Subject { get; set; }
            public string RequestDateOfAdvance { get; set; }
            public string Amount { get; set; }
            public string EffectiveFromDate { get; set; }
            public string EffectiveToDate { get; set; }
            public string FixedAdvanceType { get; set; }
            public string Objective { get; set; }
            public string RequestDate { get; set; }
            public string DocNo { get; set; }
        }


        public class AdvanceCurrencyData
        {
            public string PaymentType { get; set; }
            public string Currency { get; set; }
            public string Amount { get; set; }
        }
        #endregion Other

        #region ISCGEmailService Members

        #region public StringTemplate BuildDocumentList(long documentID,string templateName )
        public StringTemplate BuildDocumentList(long documentID, string templateName)
        {
            StringTemplateGroup group = new StringTemplateGroup("group", AppDomain.CurrentDomain.BaseDirectory + "Resources\\Template\\Email");
            StringTemplate html = group.GetInstanceOf(templateName);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.FindByIdentity(documentID);
            string strNumberFormat = "#,##0.00";
            string strExchangeRateFormat = "#,##0.0000";

            if (scgDocument.DocumentType.DocumentTypeID.Equals(DocumentTypeID.TADocumentDomestic) ||
                scgDocument.DocumentType.DocumentTypeID.Equals(DocumentTypeID.TADocumentForeign))
            {
                #region Document Type ==> TA
                TADocument ta = ScgeAccountingQueryProvider.TADocumentQuery.GetTADocumentByDocumentID(scgDocument.DocumentID);
                EmailValueObject taDoc = new EmailValueObject();
                taDoc.RequestID = ta.DocumentID.DocumentNo;
                taDoc.Subject = ta.DocumentID.Subject;
                if (!string.IsNullOrEmpty(ta.Province))
                {
                    taDoc.TravelTo = ta.Province;
                }
                if (!string.IsNullOrEmpty(ta.Country))
                {
                    taDoc.TravelTo = ta.Country;
                }
                //taDoc.Fromdate = ta.FromDate.ToShortDateString();
                //taDoc.ToDate = ta.ToDate.ToShortDateString();
                taDoc.Fromdate = ta.FromDate.ToString("dd/MM/yyyy");
                taDoc.ToDate = ta.ToDate.ToString("dd/MM/yyyy");

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
                            advanceData.RequestID = advDocument.DocumentID.DocumentNo;
                            advanceData.Subject = advDocument.DocumentID.Subject;
                            //advanceData.RequestDateOfAdvance = advDocument.RequestDateOfAdvance.ToShortDateString();
                            advanceData.RequestDateOfAdvance = advDocument.RequestDateOfAdvance.ToString("dd/MM/yyyy");
                            advanceData.Amount = advDocument.Amount.ToString(strNumberFormat);
                            if (advDocument.AdvanceType.Equals("FR"))
                            {
                                IList<AvAdvanceItem> advItemList = ScgeAccountingQueryProvider.AvAdvanceItemQuery.FindByAvAdvanceItemAdvanceID(advDocument.AdvanceID);
                                IList<AdvanceCurrencyData> advanceFrList = new List<AdvanceCurrencyData>();
                                foreach (AvAdvanceItem advItem in advItemList)
                                {
                                    AdvanceCurrencyData advanceFR = new AdvanceCurrencyData();
                                    advanceFR.Currency = advItem.CurrencyID.Symbol;
                                    advanceFR.Amount = advItem.Amount.ToString(strNumberFormat);
                                    advanceFrList.Add(advanceFR);
                                }
                                advanceData.AdvanceFrList = advanceFrList;

                            }
                            advanceDataList.Add(advanceData);
                        }
                    }
                    html.SetAttribute("advList", advanceDataList);
                }
                html.SetAttribute("ta", taDoc);

                #endregion Document Type ==> TA
            }
            else if (scgDocument.DocumentType.DocumentTypeID.Equals(DocumentTypeID.AdvanceDomesticDocument) || (scgDocument.DocumentType.DocumentTypeID.Equals(DocumentTypeID.AdvanceForeignDocument)))
            {
                #region Document Type ==> Advance
                AdvanceDataForEmail advDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceforEmailByDocumentID(scgDocument.DocumentID);
                if (advDocument != null)
                {
                    IList<AdvanceData> advanceDataList = new List<AdvanceData>();
                    AdvanceData advanceDoc = new AdvanceData();
                    advanceDoc.RequestID = scgDocument.DocumentNo;
                    advanceDoc.Subject = advDocument.Subject;
                    advanceDoc.RequestDateOfAdvance = advDocument.RequestDateOfAdvance.ToString("dd/MM/yyyy");
                    advanceDoc.IsRepOffice = advDocument.IsRepOffice ? "Y" : null;
                    advanceDoc.CurrencyID = advDocument.CurrencyID;
                    advanceDoc.SymbolLocal = advDocument.SymbolLocal;
                    advanceDoc.SymbolMain = advDocument.SymbolMain;
                    advanceDoc.Amount = advDocument.Amount.ToString(strNumberFormat);
                    advanceDoc.advitemMainCurrencyAmount = advDocument.advitemMainCurrencyAmount;
                    advanceDoc.advdocMainCurrencyAmount = advDocument.advdocMainCurrencyAmount;
                    advanceDoc.advdocLocalCurrencyAmount = advDocument.advdocLocalCurrencyAmount;
                    advanceDoc.PBID = advDocument.PBID;

                    if (advDocument.AdvanceType.Equals("FR"))
                    {
                        IList<AvAdvanceItem> advItemList = ScgeAccountingQueryProvider.AvAdvanceItemQuery.FindByAvAdvanceItemAdvanceID(advDocument.AdvanceID);

                        IList<AdvanceCurrencyData> advanceFrList = new List<AdvanceCurrencyData>();
                        foreach (AvAdvanceItem advitem in advItemList)
                        {
                            AdvanceCurrencyData advanceFr = new AdvanceCurrencyData();
                            advanceFr.Currency = advitem.CurrencyID.Symbol;
                            advanceFr.Amount = advitem.Amount.ToString(strNumberFormat);
                            advanceFrList.Add(advanceFr);
                        }
                        advanceDoc.AdvanceFrList = advanceFrList;
                    }
                    advanceDataList.Add(advanceDoc);
                    // Use advance list because e-mail template will support TA with multiple advance,
                    // so, send list (length = 1) into string template.
                    html.SetAttribute("advList", advanceDataList);
                }
                #endregion Document Type ==> Advance
            }
            else if (scgDocument.DocumentType.DocumentTypeID.Equals(DocumentTypeID.ExpenseDomesticDocument) || (scgDocument.DocumentType.DocumentTypeID.Equals(DocumentTypeID.ExpenseForeignDocument)))
            {
                #region Document Type ==> Expense
                FnExpenseDataForEmail expense = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseForEmailByDocumentID(scgDocument.DocumentID);
                EmailValueObject expenseDoc = new EmailValueObject();
                expenseDoc.RequestID = scgDocument.DocumentNo;
                expenseDoc.Subject = expense.Subject;
                expenseDoc.Amount = expense.TotalExpense.ToString(strNumberFormat);
                expenseDoc.SymbolLocal = expense.SymbolLocal;
                expenseDoc.SymbolMain = expense.SymbolMain;
                expenseDoc.TotalExpenseLocal = expense.TotalExpenseLocal.ToString(strNumberFormat);
                expenseDoc.TotalExpenseMain = expense.TotalExpenseMain.ToString(strNumberFormat);
                expenseDoc.IsRepOffice = expense.IsRepOffice;
                expenseDoc.DifferenceAmount = expense.DifferenceAmount.ToString(strNumberFormat);
                expenseDoc.DifferenceAmountLocalCurrency = expense.DifferenceAmountLocalCurrency.ToString(strNumberFormat);
                expenseDoc.DifferenceAmountMainCurrency = expense.DifferenceAmountMainCurrency.ToString(strNumberFormat);
                expenseDoc.IsForeign = scgDocument.DocumentType.DocumentTypeID.Equals(DocumentTypeID.ExpenseForeignDocument);

                if (templateName == "EM01")
                {
                    IList<InitiatorData> initiators = ScgeAccountingQueryProvider.DocumentInitiatorQuery.GetDocumentInitiatorList(scgDocument.DocumentID);
                    if (initiators.Count > 0)
                        expenseDoc.InitiatorList = initiators;

                    IList<InvoiceDataForEmail> invoices = ScgeAccountingQueryProvider.FnExpenseInvoiceQuery.FindInvoiceDataByExpenseID(expense.ExpenseID);

                    if (invoices.Count > 0)
                    {
                        expenseDoc.Invoices = invoices;

                        foreach (InvoiceDataForEmail inv in invoices)
                        {
                            var items = ScgeAccountingQueryProvider.FnExpenseInvoiceItemQuery.GetInvoiceItemByInvoiceID(inv.InvoiceID);

                            foreach (var item in items)
                            {
                                if (inv.InvoiceItems == null)
                                    inv.InvoiceItems = new List<InvoiceItemDataForEmail>();

                                InvoiceItemDataForEmail invItem = new InvoiceItemDataForEmail();
                                invItem.CostCenter = item.CostCenter != null ? item.CostCenter.CostCenterCode : string.Empty;

                                if (item.Account != null)
                                {
                                    IList<AccountLang> account = ScgDbQueryProvider.DbAccountLangQuery.FindAccountLangByAccountID(item.Account.AccountID);
                                    if (account.Count > 0)
                                    {
                                        var accountTH = account.FirstOrDefault(t => t.LanguageId == 1);
                                        if (accountTH != null)
                                            invItem.ExpenseTH = String.Format("{0}-{1}", item.Account.AccountCode, accountTH.AccountName);

                                        var accountEN = account.FirstOrDefault(t => t.LanguageId == 2);
                                        if (accountEN != null)
                                            invItem.ExpenseEN = String.Format("{0}-{1}", item.Account.AccountCode, accountEN.AccountName);
                                    }
                                }

                                invItem.InternalOrder = item.IO != null ? item.IO.IONumber : string.Empty;
                                invItem.Description = item.Description;

                                if (item.CurrencyID.HasValue)
                                {
                                    DbCurrency currency = SsDbQueryProvider.DbCurrencyQuery.FindByIdentity((short)item.CurrencyID.Value);
                                    if (currency != null)
                                        invItem.Currency = currency.Symbol;
                                }

                                if (item.CurrencyAmount.HasValue)
                                    invItem.AmountCurrency = item.CurrencyAmount.Value.ToString(strNumberFormat);

                                if (item.ExchangeRate.HasValue)
                                    invItem.ExchangeRate = item.ExchangeRate.Value.ToString(strExchangeRateFormat);


                                if (item.LocalCurrencyAmount.HasValue)
                                    invItem.AmountFinalCurrency = item.LocalCurrencyAmount.Value.ToString(strNumberFormat);

                                if (item.Amount.HasValue)
                                    invItem.AmountTHB = item.Amount.Value.ToString(strNumberFormat);

                                invItem.ReferenceNo = item.ReferenceNo;

                                inv.InvoiceItems.Add(invItem);
                            }
                        }
                    }
                }

                html.SetAttribute("exp", expenseDoc);
                #endregion Document Type ==> Expense
            }
            else if (scgDocument.DocumentType.DocumentTypeID.Equals(DocumentTypeID.MPADocument))
            {
                #region Document Type ==> MPA
                MPADocument mpa = ScgeAccountingQueryProvider.MPADocumentQuery.GetMPADocumentByDocumentID(scgDocument.DocumentID);
                EmailValueObject mpadoc = new EmailValueObject();
                mpadoc.RequestID = mpa.DocumentID.DocumentNo;
                mpadoc.Subject = mpa.DocumentID.Subject;
                //if (!string.IsNullOrEmpty(mpadoc.Province))
                //{
                //    mpadoc.TravelTo = ta.Province;
                //}
                //if (!string.IsNullOrEmpty(ta.Country))
                //{
                //    mpadoc.TravelTo = ta.Country;
                //}
                //taDoc.Fromdate = ta.FromDate.ToShortDateString();
                //taDoc.ToDate = ta.ToDate.ToShortDateString();
                mpadoc.Fromdate = mpa.StartDate.ToString("dd/MM/yyyy");
                mpadoc.ToDate = mpa.EndDate.ToString("dd/MM/yyyy");

                //IList<Advance> advanceList = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindAdvanceDocumentByTADocumentID(ta.TADocumentID);
                //if (advanceList.Count > 0)
                //{
                //    IList<AdvanceData> advanceDataList = new List<AdvanceData>();
                //    foreach (Advance advance in advanceList)
                //    {
                //        AdvanceData advanceData = new AdvanceData();
                //        AvAdvanceDocument advDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindByIdentity(advance.AdvanceID);
                //        if (advDocument != null)
                //        {
                //            advanceData.RequestID = advDocument.DocumentID.DocumentNo;
                //            advanceData.Subject = advDocument.DocumentID.Subject;
                //            //advanceData.RequestDateOfAdvance = advDocument.RequestDateOfAdvance.ToShortDateString();
                //            advanceData.RequestDateOfAdvance = advDocument.RequestDateOfAdvance.ToString("dd/MM/yyyy");
                //            advanceData.Amount = advDocument.Amount.ToString(strNumberFormat);
                //            if (advDocument.AdvanceType.Equals("FR"))
                //            {
                //                IList<AvAdvanceItem> advItemList = ScgeAccountingQueryProvider.AvAdvanceItemQuery.FindByAvAdvanceItemAdvanceID(advDocument.AdvanceID);
                //                IList<AdvanceCurrencyData> advanceFrList = new List<AdvanceCurrencyData>();
                //                foreach (AvAdvanceItem advItem in advItemList)
                //                {
                //                    AdvanceCurrencyData advanceFR = new AdvanceCurrencyData();
                //                    advanceFR.Currency = advItem.CurrencyID.Symbol;
                //                    advanceFR.Amount = advItem.Amount.ToString(strNumberFormat);
                //                    advanceFrList.Add(advanceFR);
                //                }
                //                advanceData.AdvanceFrList = advanceFrList;

                //            }
                //            advanceDataList.Add(advanceData);
                //        }
                //    }
                //    html.SetAttribute("advList", advanceDataList);
                //}
                html.SetAttribute("mpa", mpadoc);

                #endregion Document Type ==> MPA
            }
            else if (scgDocument.DocumentType.DocumentTypeID.Equals(DocumentTypeID.CADocument))
            {
                #region Document Type ==> CA
                CADocument ca = ScgeAccountingQueryProvider.CADocumentQuery.GetCADocumentByDocumentID(scgDocument.DocumentID);
                EmailValueObject cadoc = new EmailValueObject();
                cadoc.RequestID = ca.DocumentID.DocumentNo;
                cadoc.Subject = ca.DocumentID.Subject;
                //if (!string.IsNullOrEmpty(mpadoc.Province))
                //{
                //    mpadoc.TravelTo = ta.Province;
                //}
                //if (!string.IsNullOrEmpty(ta.Country))
                //{
                //    mpadoc.TravelTo = ta.Country;
                //}
                //taDoc.Fromdate = ta.FromDate.ToShortDateString();
                //taDoc.ToDate = ta.ToDate.ToShortDateString();
                cadoc.Fromdate = ca.StartDate.ToString("dd/MM/yyyy");
                cadoc.ToDate = ca.EndDate.ToString("dd/MM/yyyy");

                //IList<Advance> advanceList = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindAdvanceDocumentByTADocumentID(ta.TADocumentID);
                //if (advanceList.Count > 0)
                //{
                //    IList<AdvanceData> advanceDataList = new List<AdvanceData>();
                //    foreach (Advance advance in advanceList)
                //    {
                //        AdvanceData advanceData = new AdvanceData();
                //        AvAdvanceDocument advDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindByIdentity(advance.AdvanceID);
                //        if (advDocument != null)
                //        {
                //            advanceData.RequestID = advDocument.DocumentID.DocumentNo;
                //            advanceData.Subject = advDocument.DocumentID.Subject;
                //            //advanceData.RequestDateOfAdvance = advDocument.RequestDateOfAdvance.ToShortDateString();
                //            advanceData.RequestDateOfAdvance = advDocument.RequestDateOfAdvance.ToString("dd/MM/yyyy");
                //            advanceData.Amount = advDocument.Amount.ToString(strNumberFormat);
                //            if (advDocument.AdvanceType.Equals("FR"))
                //            {
                //                IList<AvAdvanceItem> advItemList = ScgeAccountingQueryProvider.AvAdvanceItemQuery.FindByAvAdvanceItemAdvanceID(advDocument.AdvanceID);
                //                IList<AdvanceCurrencyData> advanceFrList = new List<AdvanceCurrencyData>();
                //                foreach (AvAdvanceItem advItem in advItemList)
                //                {
                //                    AdvanceCurrencyData advanceFR = new AdvanceCurrencyData();
                //                    advanceFR.Currency = advItem.CurrencyID.Symbol;
                //                    advanceFR.Amount = advItem.Amount.ToString(strNumberFormat);
                //                    advanceFrList.Add(advanceFR);
                //                }
                //                advanceData.AdvanceFrList = advanceFrList;

                //            }
                //            advanceDataList.Add(advanceData);
                //        }
                //    }
                //    html.SetAttribute("advList", advanceDataList);
                //}
                html.SetAttribute("ca", cadoc);

                #endregion Document Type ==> MPA
            }
            else if (scgDocument.DocumentType.DocumentTypeID.Equals(DocumentTypeID.FixedAdvanceDocument))
            {
                #region Document Type ==> FixedAdvance
                FixedAdvanceDocument fixedadvDocument = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.GetFixedAdvanceByDocumentID(scgDocument.DocumentID);
                if (fixedadvDocument != null)
                {
                    FixedAdvanceData fixedAdvanceDataList = new FixedAdvanceData();
                    fixedAdvanceDataList.DocNo = scgDocument.DocumentNo;
                    fixedAdvanceDataList.RequesterName = scgDocument.RequesterID.EmployeeName;
                    fixedAdvanceDataList.Subject = scgDocument.Subject;
                    fixedAdvanceDataList.EffectiveFromDate = fixedadvDocument.EffectiveFromDate.ToString("dd/MM/yyyy");
                    fixedAdvanceDataList.EffectiveToDate = fixedadvDocument.EffectiveToDate.ToString("dd/MM/yyyy");
                    fixedAdvanceDataList.FixedAdvanceType = fixedadvDocument.FixedAdvanceType.ToString();
                    fixedAdvanceDataList.Objective = fixedadvDocument.Objective;
                    fixedAdvanceDataList.RequestDate = fixedadvDocument.RequestDate.ToString("dd/MM/yyyy");
                    fixedAdvanceDataList.Amount = fixedadvDocument.Amount.ToString();

                    // Use advance list because e-mail template will support TA with multiple advance,
                    // so, send list (length = 1) into string template.
                    html.SetAttribute("fixedAdvanceDataList", fixedAdvanceDataList);
                }
                #endregion Document Type ==> FixedAdvance
            }
            return html;
        }
        #endregion public StringTemplate BuildDocumentList(long documentID,string templateName )

        #region public void SendEmailEM01(long workFlowID, long sendToUserID , string tokenCode)
        public void SendEmailEM01(long workFlowID, long sendToUserID, string tokenCode)
        {
            //1. Query from WorkFlowResponseToken Table
            IList<WorkFlowResponseToken> responseTokens = WorkFlowQueryProvider.WorkFlowResponseTokenQuery.FindByTokenCode(tokenCode);

            int? approveStateEventID = null;
            string approveStateEventDisplayName = null;
            string approveStateEventURL = null;
            int? rejectStateEventID = null;
            string rejectStateEventDisplayName = null;
            string rejectStateEventURL = null;

            foreach (WorkFlowResponseToken responseToken in responseTokens)
            {
                if (responseToken.WorkFlowStateEvent.Name == "Approve")
                {
                    approveStateEventID = responseToken.WorkFlowStateEvent.WorkFlowStateEventID;
                    approveStateEventDisplayName = WorkFlowStateEventQuery.GetTranslatedEventName(approveStateEventID.Value, ParameterServices.DefaultLanguage);
                    approveStateEventURL =
                        String.Format(ParameterServices.ApproveRejectURL,
                            tokenCode, sendToUserID, "Email", approveStateEventID.Value);
                }
                else if (responseToken.WorkFlowStateEvent.Name == "Reject")
                {
                    rejectStateEventID = responseToken.WorkFlowStateEvent.WorkFlowStateEventID;
                    rejectStateEventDisplayName = WorkFlowStateEventQuery.GetTranslatedEventName(rejectStateEventID.Value, ParameterServices.DefaultLanguage);
                    rejectStateEventURL =
                        String.Format(ParameterServices.ApproveRejectURL,
                            tokenCode, sendToUserID, "Email", rejectStateEventID.Value);
                }
            }

            SuUser sendUser = QueryProvider.SuUserQuery.FindByIdentity(SystemUserID);
            SuUser sendToUser = QueryProvider.SuUserQuery.FindByIdentity(sendToUserID);
            Document document = WorkFlowQueryProvider.WorkFlowQuery.GetDocumentByWorkFlowID(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.FindByIdentity(document.DocumentID);

            EmailDTO email = new EmailDTO();
            if (sendToUser != null)
                email.MailSendTo.Add(new AddMailSendTo() { Name = sendToUser.UserName, Email = sendToUser.Email });
            email.Sender.Email = sendUser.Email;
            email.Sender.Name = sendUser.EmployeeName;

            // Edit By Kookkla
            // แก้เรื่องการส่ง Subject Email
            if (string.IsNullOrEmpty(approveStateEventDisplayName) && string.IsNullOrEmpty(rejectStateEventDisplayName))
                email.MailSubject = string.Format("e-Xpense : Approve/Reject {0} ", scgDocument.DocumentNo);
            else
                email.MailSubject = string.Format("e-Xpense : {1}/{2} {0} ", scgDocument.DocumentNo, approveStateEventDisplayName, rejectStateEventDisplayName);

            email.MailSendingType = Email.MailType.HTML;

            #region SetAttribute
            StringTemplate html = this.BuildDocumentList(document.DocumentID, "EM01");
            // Change to use Employee name by Anuwat S on 10/05/2009
            html.SetAttribute("DearName", sendToUser == null ? string.Empty : sendToUser.EmployeeName);
            html.SetAttribute("RequesterName", scgDocument.RequesterID.EmployeeName);
            html.SetAttribute("RequestId", scgDocument.DocumentNo);
            html.SetAttribute("link", ParameterServices.UrlLink);
            html.SetAttribute("wfid", workFlowID.ToString());
            html.SetAttribute("ApproveURL", approveStateEventURL);
            html.SetAttribute("ApproveEventDisplayName", approveStateEventDisplayName);
            html.SetAttribute("RejectURL", rejectStateEventURL);
            html.SetAttribute("RejectEventDisplayName", rejectStateEventDisplayName);
            html.SetAttribute("IsApproveReject", approveStateEventDisplayName == "Approve" ? "Y" : null);
            #endregion SetAttribute

            email.MailBody = html.ToString();

            email.EmailType = "EM01";
            email.RequesterNo = scgDocument.DocumentNo;

            //SCGEmailService.SendEmail(email);
            //4. Call Email Service for send email
            if (ParameterServices.EnableEmail01)    // use for enable or disable send email
                this.SendEmail(email);

            //send completed then must keep log
            //AddEmailLog(mailBody);
        }
        #endregion public void SendEmailEM01(long workFlowID, long sendToUserID , string tokenCode)

        #region public void SendEmailEM02(long workFlowResponseID , long sendToUserID)
        public void SendEmailEM02(long workFlowResponseID, long sendToUserID, IList<long> ccList)
        {
            //Modify by tom 20/04/2010, change query because field remark.
            WorkFlowResponseSearchResult wfResponse = WorkFlowQueryProvider.WorkFlowResponseQuery.GetWorkFlowResponseAndEventAndReasonByWFResponseID(workFlowResponseID, ParameterServices.DefaultLanguage);
            //WorkFlowResponseSearchResult wfResponse = WorkFlowQueryProvider.WorkFlowResponseQuery.GetWorkFlowResponseHoldByWFResponseID(workFlowResponseID, ParameterServices.DefaultLanguage);
            WorkFlow wfid = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID((long)wfResponse.DocumentID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.FindByIdentity(wfResponse.DocumentID.Value);
            SuUser sendUser = QueryProvider.SuUserQuery.FindByIdentity(SystemUserID);
            SuUser sendToUser = QueryProvider.SuUserQuery.FindByIdentity(sendToUserID);

            //Create DTO
            EmailDTO email = new EmailDTO();
            email.Sender.Email = sendUser.Email;
            email.Sender.Name = sendUser.EmployeeName;
            email.MailSubject = string.Format("Employee Expense : {0} - Status Change", scgDocument.DocumentNo);
            email.MailSendingType = Email.MailType.HTML;

            #region SetAttribute
            StringTemplate html = this.BuildDocumentList(wfResponse.DocumentID.Value, "EM02");
            // Change to use Employee name by Anuwat S on 10/05/2009
            html.SetAttribute("DearName", sendToUser == null ? string.Empty : sendToUser.EmployeeName);
            //html.SetAttribute("DearName", sendToUser.UserName);
            html.SetAttribute("DocumentNo", scgDocument.DocumentNo);
            html.SetAttribute("RequesterName", scgDocument.RequesterID.EmployeeName);
            html.SetAttribute("Reason", wfResponse.ReasonName);
            //html.SetAttribute("remark", wfResponse.Remark);
            string remark = string.IsNullOrEmpty(wfResponse.RemarkHold) ? wfResponse.RemarkReject : wfResponse.RemarkHold;
            html.SetAttribute("remark", remark);
            html.SetAttribute("ResponseBy", wfResponse.ResponseBy);
            DbDictionary verbPassive = DbDictionaryQuery.GetVerb3ByVerb1(wfResponse.EventName);
            html.SetAttribute("WorkFlowStateEventName", string.IsNullOrEmpty(verbPassive.Verb3) ? wfResponse.ResponseEventName : verbPassive.Verb3);
            html.SetAttribute("WorkFlowStateEventName2", string.IsNullOrEmpty(verbPassive.Verb3Thai) ? wfResponse.ResponseEventName : verbPassive.Verb3Thai);
            html.SetAttribute("link", ParameterServices.UrlLink);
            html.SetAttribute("wfid", wfid.WorkFlowID.ToString());
            #endregion SetAttribute

            email.MailBody = html.ToString();

            email.EmailType = "EM02";
            email.RequesterNo = scgDocument.DocumentNo;

            if (ccList != null)
            {
                foreach (long userID in ccList)
                {
                    SuUser user = QueryProvider.SuUserQuery.FindByIdentity(userID);
                    email.MailSendToCC.Add(new AddMailSendToCC() { Email = user.Email, Name = user.EmployeeName });
                }
            }

            if (sendToUser != null)
            {
                email.MailSendTo.Add(new AddMailSendTo() { Email = sendToUser.Email, Name = sendToUser.EmployeeName });
                //Call Email Service for send email
                this.SendEmail(email);
            }

            //send completed then must keep log
            //AddEmailLog(mailBody);
        }
        #endregion public void SendEmailEM02(long workFlowResponseID , long sendToUserID)

        #region public void SendEmailEM03(long workFlowID, long sendToUserID)
        public void SendEmailEM03(long workFlowID, long sendToUserID)
        {
            //Create DTO
            SuUser sendUser = QueryProvider.SuUserQuery.FindByIdentity(SystemUserID);
            SuUser sendToUser = QueryProvider.SuUserQuery.FindByIdentity(sendToUserID);
            Document document = WorkFlowQueryProvider.WorkFlowQuery.GetDocumentByWorkFlowID(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.FindByIdentity(document.DocumentID);
            StringTemplateGroup group = new StringTemplateGroup("group", AppDomain.CurrentDomain.BaseDirectory + "Resources\\Template\\Email");

            #region SetAttribute
            StringTemplate html = group.GetInstanceOf("EM03");
            html.SetAttribute("subject", scgDocument.Subject);
            html.SetAttribute("link", ParameterServices.UrlLink); //URL In parameterService
            html.SetAttribute("wfid", workFlowID.ToString());
            html.SetAttribute("SendToUser", sendToUser == null ? string.Empty : sendToUser.EmployeeName);
            html.SetAttribute("DocumentNo", scgDocument.DocumentNo);
            #endregion SetAttribute

            EmailDTO email = new EmailDTO();
            email.Sender.Email = sendUser.Email;
            email.Sender.Name = sendUser.EmployeeName;
            email.MailSendingType = Email.MailType.HTML;
            email.MailSubject = string.Format("Employee Expense : {0} - Over Role", scgDocument.DocumentNo);
            email.MailBody = html.ToString();

            email.EmailType = "EM03";
            email.RequesterNo = scgDocument.DocumentNo;

            if (sendToUser != null)
            {
                email.MailSendTo.Add(new AddMailSendTo() { Email = sendToUser.Email, Name = sendToUser.EmployeeName });
                //Call Email Service for send email
                if (ParameterServices.EnableEmail03)    // use for enable or disable send email
                    this.SendEmail(email);
            }

            //send completed then must keep log
            //AddEmailLog(mailBody);
        }
        #endregion public void SendEmailEM03(long workFlowID, long sendToUserID)

        #region public void SendEmailEM04(long workFlowResponseID, long sendToUserID)
        public void SendEmailEM04(long workFlowResponseID, long sendToUserID)
        {
            //Create DTO
            SuUser sendUser = QueryProvider.SuUserQuery.FindByIdentity(SystemUserID);
            SuUser sendToUser = QueryProvider.SuUserQuery.FindByIdentity(sendToUserID);

            WorkFlowResponseSearchResult wfResponse = WorkFlowQueryProvider.WorkFlowResponseQuery.GetWorkFlowResponseAndEventAndReasonByWFResponseID(workFlowResponseID, ParameterServices.DefaultLanguage);
            WorkFlow wfid = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(wfResponse.DocumentID.Value);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.FindByIdentity(wfResponse.DocumentID.Value);
            StringTemplateGroup group = new StringTemplateGroup("group", AppDomain.CurrentDomain.BaseDirectory + "Resources\\Template\\Email");

            #region SetAttribute
            StringTemplate html = group.GetInstanceOf("EM04");
            // Change to display user name by sendToUser by Anuwat S on 10/05/2009
            html.SetAttribute("creatorName", sendToUser == null ? string.Empty : sendToUser.EmployeeName);
            //html.SetAttribute("creatorName", scgDocument.CreatorID.UserName);
            html.SetAttribute("subject", scgDocument.Subject);
            html.SetAttribute("requesterID", scgDocument.DocumentNo);
            html.SetAttribute("reason", wfResponse.ReasonName);
            //html.SetAttribute("remark", wfResponse.Remark);
            html.SetAttribute("remark", wfResponse.RemarkReject);
            html.SetAttribute("link", ParameterServices.UrlLink); //URL In parameterService
            html.SetAttribute("wfid", wfid.WorkFlowID);
            html.SetAttribute("WorkFlowStateEventName", wfResponse.ResponseEventName);
            #endregion SetAttribute

            #region EMAIL
            EmailDTO email = new EmailDTO();
            email.Sender.Email = sendUser.Email;
            email.Sender.Name = sendUser.EmployeeName;
            email.MailSendingType = Email.MailType.HTML;
            email.MailSubject = string.Format("Employee Expense : {0} - Rejection", scgDocument.DocumentNo);
            email.MailBody = html.ToString();

            email.EmailType = "EM04";
            email.RequesterNo = scgDocument.DocumentNo;

            if (sendToUser != null)
            {
                email.MailSendTo.Add(new AddMailSendTo() { Email = sendToUser.Email, Name = sendToUser.EmployeeName });
                //Call Email Service for send email
                if (ParameterServices.EnableEmail04)    // use for enable or disable send email
                    this.SendEmail(email);
            }
            #endregion EMAIL

            //Call Email Service for send email
            //EmailService.SendEmail(mailBody);


            //send completed then must keep log
            //AddEmailLog(mailBody);
        }
        #endregion public void SendEmailEM04(long workFlowResponseID, long sendToUserID)

        #region public void SendEmailEM05(long workFlowID, long sendToUserID, IList<long> ccList,bool isAutoPayment)
        public void SendEmailEM05(long workFlowID, long sendToUserID, IList<long> ccList, bool isAutoPayment)
        {
            //Create DTO
            string strFormat = "###,###,###,###,###,###,###,###,##0.00";
            SuUser sendToUser = QueryProvider.SuUserQuery.FindByIdentity(sendToUserID);
            SuUser sendUser = QueryProvider.SuUserQuery.FindByIdentity(SystemUserID);
            Document document = WorkFlowQueryProvider.WorkFlowQuery.GetDocumentByWorkFlowID(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.FindByIdentity(document.DocumentID);
            EmailDTO email = new EmailDTO();
            email.Sender.Email = sendUser.Email;
            email.Sender.Name = sendUser.EmployeeName;
            email.MailSubject = string.Format("Employee Expense : {0} - Payment Notification", scgDocument.DocumentNo);
            email.MailSendingType = Email.MailType.HTML;
            StringTemplateGroup group = new StringTemplateGroup("group", AppDomain.CurrentDomain.BaseDirectory + "Resources\\Template\\Email");

            #region SetAttribute
            StringTemplate html = group.GetInstanceOf("EM05");
            html.SetAttribute("receiverName", scgDocument.ReceiverID.EmployeeName);
            html.SetAttribute("requestID", scgDocument.DocumentNo);
            html.SetAttribute("subject", scgDocument.Subject);
            //html.SetAttribute("createDate", scgDocument.DocumentDate.Value.ToShortDateString());
            html.SetAttribute("createDate", scgDocument.BaseLineDate.Value.ToString("dd/MM/yyyy"));
            html.SetAttribute("wfid", workFlowID.ToString());
            html.SetAttribute("link", ParameterServices.UrlLink);
            #endregion SetAttribute

            if (!isAutoPayment) // Cash
            {
                #region Cash
                if (scgDocument.DocumentType.DocumentTypeID.Equals(DocumentTypeID.ExpenseDomesticDocument) || (scgDocument.DocumentType.DocumentTypeID.Equals(DocumentTypeID.ExpenseForeignDocument)))
                {
                    FnExpenseDataForEmail expense = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseForEmailByDocumentID(scgDocument.DocumentID);

                    if (expense != null)
                    {
                        if (expense.IsRepOffice == true)
                        {
                            html.SetAttribute("netAmount", expense.DifferenceAmountLocalCurrency.ToString(strFormat));
                            html.SetAttribute("SymbolLocal", expense.SymbolLocal);
                        }
                        else
                            html.SetAttribute("netAmount", expense.DifferenceAmount.ToString(strFormat));
                        if (expense.PBID != null)
                        {
                            DbpbLang pb = ScgDbQueryProvider.DbPbLangQuery.GetPBLangByPBIDAndLanguageID(expense.PBID, ParameterServices.DefaultLanguage);
                            html.SetAttribute("pbName", pb.Description);
                        }
                    }
                }
                else if (scgDocument.DocumentType.DocumentTypeID.Equals(DocumentTypeID.AdvanceDomesticDocument) || (scgDocument.DocumentType.DocumentTypeID.Equals(DocumentTypeID.AdvanceForeignDocument)))
                {
                    string dbStatus = string.Empty;
                    //AvAdvanceDocument advance = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(scgDocument.DocumentID);
                    AdvanceDataForEmail advance = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceforEmailByDocumentID(scgDocument.DocumentID);

                    if (advance != null)
                    {
                        if (advance.IsRepOffice == true)
                        {
                            html.SetAttribute("netAmount", advance.advdocLocalCurrencyAmount);
                            html.SetAttribute("SymbolLocal", advance.SymbolLocal);
                        }
                        else
                            html.SetAttribute("netAmount", advance.Amount.ToString(strFormat));
                        IList<AvAdvanceItem> advanceItemList = ScgeAccountingQueryProvider.AvAdvanceItemQuery.FindByAvAdvanceItemAdvanceID(advance.AdvanceID);
                        IList<AdvanceCurrencyData> advItemList = new List<AdvanceCurrencyData>();
                        if (scgDocument.DocumentType.DocumentTypeID.Equals(DocumentTypeID.AdvanceForeignDocument))
                        {
                            foreach (AvAdvanceItem advanceItem in advanceItemList)
                            {
                                dbStatus = ScgDbQueryProvider.SCGDbStatusLangQuery.GetStatusLang("PaymentTypeFRN", ParameterServices.DefaultLanguage, advanceItem.PaymentType);

                                AdvanceCurrencyData advItem = new AdvanceCurrencyData();
                                advItem.PaymentType = dbStatus;
                                if (advanceItem.CurrencyID != null)
                                    advItem.Currency = advanceItem.CurrencyID.Symbol;
                                advItem.Amount = advanceItem.Amount.ToString(strFormat);
                                advItemList.Add(advItem);
                            }

                            html.SetAttribute("advItemList", advItemList);
                        }
                        if (advance.PBID != null)
                        {
                            DbpbLang pb = ScgDbQueryProvider.DbPbLangQuery.GetPBLangByPBIDAndLanguageID(advance.PBID.Value, ParameterServices.DefaultLanguage);
                            html.SetAttribute("pbName", pb.Description);
                        }
                    }
                }
                #endregion Cash
            }
            else // Cheque
            {
                #region Cheque
                FnAutoPayment autoPayment = ScgeAccountingQueryProvider.FnAutoPaymentQuery.GetFnAutoPaymentByDocumentID(document.DocumentID);
                html.SetAttribute("isCheque", 1);
                html.SetAttribute("chequeNumber", autoPayment.ChequeNumber);
                html.SetAttribute("chequeBankName", autoPayment.ChequeBankName);
                html.SetAttribute("currencyDoc", autoPayment.CurrencyDoc);
                html.SetAttribute("amount", autoPayment.Amount.ToString(strFormat));


                if (scgDocument.DocumentType.DocumentTypeID.Equals(DocumentTypeID.ExpenseDomesticDocument) || (scgDocument.DocumentType.DocumentTypeID.Equals(DocumentTypeID.ExpenseForeignDocument)))
                {
                    FnExpenseDocument expense = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(scgDocument.DocumentID);
                    if (expense != null)
                    {
                        if (expense.PB != null)
                        {
                            DbpbLang pb = ScgDbQueryProvider.DbPbLangQuery.GetPBLangByPBIDAndLanguageID(expense.PB.Pbid, ParameterServices.DefaultLanguage);
                            html.SetAttribute("pbName", pb.Description);
                        }
                    }
                }
                else if (scgDocument.DocumentType.DocumentTypeID.Equals(DocumentTypeID.AdvanceDomesticDocument) || (scgDocument.DocumentType.DocumentTypeID.Equals(DocumentTypeID.AdvanceForeignDocument)))
                {
                    AvAdvanceDocument advance = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(scgDocument.DocumentID);
                    if (advance != null)
                    {
                        if (advance.PBID != null)
                        {
                            DbpbLang pb = ScgDbQueryProvider.DbPbLangQuery.GetPBLangByPBIDAndLanguageID(advance.PBID.Pbid, ParameterServices.DefaultLanguage);
                            html.SetAttribute("pbName", pb.Description);
                        }
                    }
                }


                #endregion Cheque
            }

            email.MailBody = html.ToString();
            email.EmailType = "EM05";
            email.RequesterNo = scgDocument.DocumentNo;

            foreach (long cc in ccList)
            {
                SuUser ccUser = QueryProvider.SuUserQuery.FindByIdentity(cc);
                if (ccUser != null) email.MailSendToCC.Add(new AddMailSendToCC() { Email = ccUser.Email, Name = ccUser.UserName });
            }

            if (sendToUser != null)
            {
                email.MailSendTo.Add(new AddMailSendTo() { Email = sendToUser.Email, Name = sendToUser.EmployeeName });
                //Call Email Service for send email
                if (ParameterServices.EnableEmail05)    // use for enable or disable send email
                    this.SendEmail(email);
            }



            //Call Email Service for send email
            //EmailService.SendEmail(mailBody);


            //send completed then must keep log
            //AddEmailLog(mailBody);
        }
        #endregion public void SendEmailEM05(long workFlowID, long sendToUserID, IList<long> ccList,bool isAutoPayment)

        #region public void SendEmailEM06(long workFlowID, long sendToUserID, IList<long> ccList)
        public void SendEmailEM06(long workFlowID, long sendToUserID, IList<long> ccList)
        {
            //Create DTO
            string strFormat = "###,###,###,###,###,###,###,###,##0.00";
            Document document = WorkFlowQueryProvider.WorkFlowQuery.GetDocumentByWorkFlowID(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.FindByIdentity(document.DocumentID);
            FnAutoPayment autoPayment = ScgeAccountingQueryProvider.FnAutoPaymentQuery.GetFnAutoPaymentByDocumentID(document.DocumentID);
            SuUser sendToUser = QueryProvider.SuUserQuery.FindByIdentity(sendToUserID);
            SuUser sendUser = QueryProvider.SuUserQuery.FindByIdentity(SystemUserID);
            EmailDTO email = new EmailDTO();
            email.Sender.Email = sendUser.Email;
            email.Sender.Name = sendUser.EmployeeName;
            email.MailSubject = string.Format("Employee Expense : {0} - Payment Notification", scgDocument.DocumentNo);
            email.MailSendingType = Email.MailType.HTML;
            StringTemplateGroup group = new StringTemplateGroup("group", AppDomain.CurrentDomain.BaseDirectory + "Resources\\Template\\Email");

            #region SetAttribute
            StringTemplate html = group.GetInstanceOf("EM06");
            html.SetAttribute("receiverName", scgDocument.ReceiverID.EmployeeName);
            html.SetAttribute("requestID", scgDocument.DocumentNo);
            html.SetAttribute("subject", scgDocument.Subject);
            try
            {
                html.SetAttribute("paymentDate", autoPayment.PaymentDate.Value.ToString("dd/MM/yyyy"));
            }
            catch (Exception)
            {

            }

            html.SetAttribute("payeeBankAccountNumber", autoPayment.PayeeBankAccountNumber);
            html.SetAttribute("payeeBankName", autoPayment.ChequeBankName);
            html.SetAttribute("chequeNumber", autoPayment.ChequeNumber);
            html.SetAttribute("chequeBankName", autoPayment.ChequeBankName);
            html.SetAttribute("amount", autoPayment.Amount.ToString(strFormat));
            html.SetAttribute("link", ParameterServices.UrlLink);
            html.SetAttribute("wfid", workFlowID.ToString());
            #endregion SetAttribute

            email.MailBody = html.ToString();
            email.EmailType = "EM06";
            email.RequesterNo = scgDocument.DocumentNo;

            foreach (long cc in ccList)
            {
                SuUser ccUser = QueryProvider.SuUserQuery.FindByIdentity(cc);
                if (ccUser != null)
                {
                    // Change to use Employee name by Anuwat S on 10/05/2009
                    email.MailSendToCC.Add(new AddMailSendToCC() { Email = ccUser.Email, Name = ccUser.EmployeeName });
                }
            }
            if (sendToUser != null)
            {
                email.MailSendTo.Add(new AddMailSendTo() { Email = sendToUser.Email, Name = sendToUser.EmployeeName });

                //Call Email Service for send email
                if (ParameterServices.EnableEmail06)    // use for enable or disable send email
                    this.SendEmail(email);
            }

            //send completed then must keep log
            //AddEmailLog(mailBody);
        }
        #endregion public void SendEmailEM06(long workFlowID, long sendToUserID, IList<long> ccList)

        #region public void SendEmailEM07(long sendToUserID)
        public void SendEmailEM07(long sendToUserID)
        {
            SuUser sendUser = QueryProvider.SuUserQuery.FindByIdentity(SystemUserID);
            //Create DTO
            EmailDTO email = new EmailDTO();
            email.Sender.Email = sendUser.Email;
            email.Sender.Name = sendUser.EmployeeName;
            email.MailSendingType = Email.MailType.HTML;
            string mailSubject = "Your e-Xpense system password will expire in {0} day(s).";
            SuUser sendToUser = QueryProvider.SuUserQuery.FindByIdentity(sendToUserID);
            StringTemplateGroup group = new StringTemplateGroup("group", AppDomain.CurrentDomain.BaseDirectory + "Resources\\Template\\Email");
            StringTemplate html = group.GetInstanceOf("EM07");

            if (sendToUser != null)
            {
                html.SetAttribute("sendToUserName", sendToUser.EmployeeName);
                if (sendToUser.PasswordExpiryDate != null)
                {
                    System.TimeSpan N = sendToUser.PasswordExpiryDate.Value - DateTime.Now;
                    N = N.Add(new TimeSpan(1, 0, 0, 0));
                    mailSubject = string.Format(mailSubject, N.Days);
                    if (N.Days == 0)
                    {

                        mailSubject = "Your e-Xpense system password was expire.";
                    }
                    if (N.Days > 0)
                    {
                        html.SetAttribute("N", N.Days);
                    }
                }
                html.SetAttribute("link", ParameterServices.UrlLink);
                email.MailSubject = mailSubject;
                email.MailBody = html.ToString();
                email.MailSendTo.Add(new AddMailSendTo() { Email = sendToUser.Email, Name = sendToUser.EmployeeName });
                email.EmailType = "EM07";

                //Call Email Service for send email
                //EmailService.SendEmail(email);
                if (ParameterServices.EnableEmail07)    // use for enable or disable send email
                    this.SendEmail(email);
            }

            //Call Email Service for send email
            //EmailService.SendEmail(mailBody);

            //send completed then must keep log
            //AddEmailLog(mailBody);
        }
        #endregion public void SendEmailEM07(long sendToUserID)

        #region public void SendEmailEM08(long sendToUserID,string newGenPassword)
        public void SendEmailEM08(long sendToUserID, string newGenPassword)
        {
            //Create DTO
            SuUser sendToUser = QueryProvider.SuUserQuery.FindByIdentity(sendToUserID);
            SuUser sendUser = QueryProvider.SuUserQuery.FindByIdentity(SystemUserID);
            StringTemplateGroup group = new StringTemplateGroup("group", AppDomain.CurrentDomain.BaseDirectory + "Resources\\Template\\Email");
            StringTemplate html = group.GetInstanceOf("EM08");
            EmailDTO email = new EmailDTO();
            email.Sender.Email = sendUser.Email;
            email.Sender.Name = sendUser.EmployeeName;
            email.MailSubject = "Welcome to e-Xpense : New User ID and Password.";
            email.MailSendingType = Email.MailType.HTML;
            html.SetAttribute("userName", sendToUser.EmployeeName);
            // Change to use Employee name by Anuwat S on 10/05/2009
            html.SetAttribute("userID", sendToUser.UserName);
            if (sendToUser.IsAdUser)
            {
                html.SetAttribute("isNotAdUser", null);
                html.SetAttribute("passwordEN", ParameterServices.EM08PasswordRemarkEN);
                html.SetAttribute("passwordTH", ParameterServices.EM08PasswordRemarkTH);
            }
            else
            {
                html.SetAttribute("isNotAdUser", "yes");
                html.SetAttribute("passwordEN", newGenPassword);
                html.SetAttribute("passwordTH", newGenPassword);
            }
            html.SetAttribute("link", ParameterServices.UrlLink);

            //html.SetAttribute("urlImage", String.Format("{0}App_Themes/Default/images/headerWelCome.png", ParameterServices.UrlLink));
            email.MailBody = html.ToString();

            if (sendToUser != null)
            {
                email.MailSendTo.Add(new AddMailSendTo() { Email = sendToUser.Email, Name = sendToUser.EmployeeName });
                email.EmailType = "EM08";
                email.RequesterNo = String.Empty;
                if (ParameterServices.EnableEmail08)    // use for enable or disable send email
                    this.SendEmail(email);
            }
        }
        #endregion public void SendEmailEM08(long sendToUserID,string newGenPassword)

        #region public void SendEmailEM09(long documentID, string ccList, string comment)
        public void SendEmailEM09(long documentID, string ccList, string comment, string note)
        {
            //Create DTO
            SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.FindByIdentity(documentID);
            //SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.FindByIdentity(document.DocumentID);
            WorkFlow wfid = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(documentID);

            //modify by oum 10-06-2009
            //change request user want to "send to" creator and receiver
            SuUser sendToUser = QueryProvider.SuUserQuery.FindByIdentity(document.CreatorID.Userid);
            SuUser userReceiver = QueryProvider.SuUserQuery.FindByIdentity(document.ReceiverID.Userid);
            //comment by oum
            DateTime? approveDate;
            EmailDTO email = new EmailDTO();
            string emailType = string.Empty;

            IList<DocumentAttachment> documentAttachments = ScgeAccountingQueryProvider.DocumentAttachmentQuery.GetDocumentAttachmentByDocumentID(document.DocumentID);
            if (documentAttachments.Count > 0)
            {
                emailType = "EM09_2";
                email.MailSubject = "Please send original documents to complete your request ";
                approveDate = WorkFlowQueryProvider.WorkFlowResponseQuery.GetApproveVerifyDateTime(wfid.WorkFlowID);

                if (note.Equals("Auto"))
                {
                    SuUser approver = QueryProvider.SuUserQuery.FindByIdentity(document.ApproverID.Userid);
                    AddMailSendToCC cc = new AddMailSendToCC();
                    cc.Email = approver.Email;
                    cc.Name = approver.EmployeeName;
                    email.MailSendToCC.Add(cc);
                }
            }
            else
            {
                emailType = "EM09_1";
                email.MailSubject = "Please send original documents for making payment of this request ";
                approveDate = WorkFlowQueryProvider.WorkFlowResponseQuery.GetApproveDateTime(wfid.WorkFlowID);
            }

            SuUser sendUser = QueryProvider.SuUserQuery.FindByIdentity(SystemUserID);
            StringTemplateGroup group = new StringTemplateGroup("group", AppDomain.CurrentDomain.BaseDirectory + "Resources\\Template\\Email");
            StringTemplate html = group.GetInstanceOf(emailType);

            email.EmailType = "EM09";
            email.Sender.Email = sendUser.Email;
            email.Sender.Name = sendUser.EmployeeName;
            email.MailSubject += string.Format("Employee Expense No: {0} ", document.DocumentNo);
            email.MailSendingType = Email.MailType.HTML;
            html.SetAttribute("receiverName", sendToUser.EmployeeName);
            html.SetAttribute("requestID", document.DocumentNo);
            html.SetAttribute("subject", document.Subject);
            html.SetAttribute("link", ParameterServices.UrlLink);
            html.SetAttribute("comment", comment);
            html.SetAttribute("wfid", wfid.WorkFlowID.ToString());
            if (note.Equals("Auto"))
            {
                html.SetAttribute("noteEN", "Automatic e-Mail issue");
                html.SetAttribute("noteTH", "ระบบออกเอกสารอัตโนมัติ");
                comment = "This is e-mail for autoreminder send document system.";
            }

            if (approveDate.HasValue)
            {
                TimeSpan numberDay;
                numberDay = DateTime.Now.Date - approveDate.Value.Date;
                html.SetAttribute("numberdate", numberDay.Days);
            }
            else
            {
                html.SetAttribute("numberdate", "0");
            }

            email.MailBody = html.ToString();

            //set vo by oum 20-04-2009
            email.RequesterNo = document.DocumentNo;
            email.Remark = comment; //for email log

            if (sendToUser != null)
            {
                // add email 'To' by oum; to creator and receiver
                email.MailSendTo.Add(new AddMailSendTo() { Email = sendToUser.Email, Name = sendToUser.EmployeeName });
                if (sendToUser.Userid != userReceiver.Userid)
                {
                    email.MailSendTo.Add(new AddMailSendTo() { Email = userReceiver.Email, Name = userReceiver.EmployeeName });
                }
                // IsMultipleReceiver = true ; ต้องการให้ทั้งหมดว่าต้องการ send ให้ใครบ้าง add by oum
                email.IsMultipleReceiver = true;

                if (!String.IsNullOrEmpty(ccList))
                {
                    String[] emailList = ccList.Split(';');
                    foreach (string e in emailList)
                    {
                        AddMailSendToCC t = new AddMailSendToCC();
                        t.Email = e;
                        t.Name = e;
                        email.MailSendToCC.Add(t);

                    }
                }
                //Call Email Service for send email
                if (ParameterServices.EnableEmail09)    // use for enable or disable send email
                    this.SendEmail(email);
            }
        }
        #endregion public void SendEmailEM09(long documentID, string ccList, string comment)

        #region public void SendEmailEM10(long advDocumentID,long sendToUserID,string ccList,string remark, isAuto)
        public void SendEmailEM10(long advDocumentID, long sendToUserID, string ccList, string remark, bool isAuto)
        {
            AvAdvanceDocument advance = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindByIdentity(advDocumentID);
            WorkFlow wfid = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(advance.DocumentID.DocumentID);
            SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.FindByIdentity(advance.DocumentID.DocumentID);
            SuUser sendToUser = QueryProvider.SuUserQuery.FindByIdentity(sendToUserID);
            SuUser sendUser = QueryProvider.SuUserQuery.FindByIdentity(SystemUserID);
            SuUser userCreator = QueryProvider.SuUserQuery.FindByIdentity(document.CreatorID.Userid);

            StringTemplateGroup group = new StringTemplateGroup("group", AppDomain.CurrentDomain.BaseDirectory + "Resources\\Template\\Email");
            StringTemplate html = group.GetInstanceOf("EM10");
            //Create DTO
            EmailDTO email = new EmailDTO();
            email.Sender.Email = sendUser.Email;
            email.Sender.Name = sendUser.EmployeeName;
            email.MailSubject = string.Format("Employee Expense : {0} - Advance Clearing Notification.", advance.DocumentID.DocumentNo);
            email.MailSendingType = Email.MailType.HTML;

            //set vo by oum 20-04-2009
            email.RequesterNo = advance.DocumentID.DocumentNo;
            email.EmailType = "EM10";

            email.Remark = remark; //for email log

            // Comment by ao 09-05-2009
            //email.ToEmail = sendToUser.Email;
            //email.CCEmail = ccList;
            //---------------------------

            if (advance != null)
            {
                html.SetAttribute("requesterName", advance.DocumentID.RequesterID.EmployeeName);

                html.SetAttribute("advanceID", advance.DocumentID.DocumentNo + " " + advance.DocumentID.Subject);
                TimeSpan numberDay;

                numberDay = DateTime.Now - (advance.RequestDateOfRemittance);
                html.SetAttribute("numberday", numberDay.Days);

                html.SetAttribute("clearDate", DateTime.Now.AddDays(7).ToString("dd/MM/yyyy"));

                if (isAuto)
                {
                    html.SetAttribute("remarkAutoEN", "Automatic e-Mail issue");
                    html.SetAttribute("remarkAutoTH", "ระบบออกเอกสารอัตโนมัติ");
                }
                else
                {
                    html.SetAttribute("remark", remark);
                }

                //html.SetAttribute("requesterName", advance.DocumentID.RequesterID.EmployeeName);
                html.SetAttribute("link", ParameterServices.UrlLink);
                html.SetAttribute("wfid", wfid.WorkFlowID.ToString());
                email.MailBody = html.ToString();
            }

            if (sendToUser != null)
            {
                //comment by fs.oat 14-01-2011
                //เช็ค Email แยกส่ง 2 ฉบับ
                email.MailSendTo.Add(new AddMailSendTo() { Email = sendToUser.Email, Name = sendToUser.EmployeeName });

                if (userCreator.Userid != sendToUser.Userid)
                {
                    email.MailSendTo.Add(new AddMailSendTo() { Email = userCreator.Email, Name = userCreator.EmployeeName });
                }
                //comment by oum 20-04-2009
                //ใช้เป็น string array แทน
                //foreach (int ccID in ccList)
                //{
                //    SuUser ccUser = QueryProvider.SuUserQuery.FindByIdentity(ccID);
                //    email.MailSendToCC.Add(new AddMailSendToCC() { Email = ccUser.Email, Name = ccUser.EmployeeName });
                //}
                String[] emailList = ccList.Split(';');
                foreach (string e in emailList)
                {
                    AddMailSendToCC t = new AddMailSendToCC();
                    t.Email = e;
                    t.Name = e;
                    email.MailSendToCC.Add(t);
                }
                //Call Email Service for send email
                //EmailService.SendEmail(email);
                if (ParameterServices.EnableEmail10)    // use for enable or disable send email
                    this.SendEmail(email);
                //this.AddEmailLog(email);
            }

            //send completed then must keep log
            //AddEmailLog(mailBody);

            //Call Email Service for send email
            //EmailService.SendEmail(mailBody);

            //send completed then must keep log
            //AddEmailLog(mailBody);
        }
        #endregion

        #region public void SendEmailEM11(long expDocumentID)
        public void SendEmailEM11(long expDocumentID)
        {
            FnExpenseDocument expense = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.FindByIdentity(expDocumentID);
            WorkFlow wfid = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(expense.Document.DocumentID);
            SuUser sendToUser = QueryProvider.SuUserQuery.FindByIdentity(expense.Document.RequesterID.Userid);
            SuUser sendUser = QueryProvider.SuUserQuery.FindByIdentity(SystemUserID);
            StringTemplateGroup group = new StringTemplateGroup("group", AppDomain.CurrentDomain.BaseDirectory + "Resources\\Template\\Email");
            StringTemplate html = group.GetInstanceOf("EM11");
            //Create DTO
            EmailDTO email = new EmailDTO();
            email.Sender.Email = sendUser.Email;
            email.Sender.Name = sendUser.EmployeeName;
            email.MailSubject = string.Format("Employee Expense : {0} - Remit outstanding advance to cashier", expense.Document.DocumentNo);
            email.MailSendingType = Email.MailType.HTML;
            html.SetAttribute("requesterName", expense.Document.RequesterID.EmployeeName);
            html.SetAttribute("wfid", wfid.WorkFlowID.ToString());
            html.SetAttribute("documentno", wfid.Document.DocumentNo.ToString());

            if (expense.PB != null)
            {
                DbpbLang pb = ScgDbQueryProvider.DbPbLangQuery.GetPBLangByPBIDAndLanguageID(expense.PB.Pbid, ParameterServices.DefaultLanguage);
                html.SetAttribute("pbName", pb.Description);
            }
            html.SetAttribute("link", ParameterServices.UrlLink);
            email.MailBody = html.ToString();
            email.RequesterNo = expense.Document.DocumentNo;
            email.EmailType = "EM11";


            if (sendToUser != null)
            {
                email.MailSendTo.Add(new AddMailSendTo() { Email = sendToUser.Email, Name = sendToUser.EmployeeName });
                if(expense.Document.RequesterID.Userid != expense.Document.CreatorID.Userid)
                {
                    SuUser sendToCreater = QueryProvider.SuUserQuery.FindByIdentity(expense.Document.CreatorID.Userid);
                    email.MailSendToCC.Add(new AddMailSendToCC() { Email = sendToCreater.Email, Name = sendToCreater.EmployeeName });
                }
                //Call Email Service for send email
                //EmailService.SendEmail(email);
                if (ParameterServices.EnableEmail11)    // use for enable or disable send email
                    this.SendEmail(email);
            }

            //Call Email Service for send email
            //EmailService.SendEmail(mailBody);

            //send completed then must keep log
            //AddEmailLog(mailBody);
        }
        #endregion public void SendEmailEM11(long expDocumentID)

        #region public void SendEmailEM12(long userID, string password)
        public void SendEmailEM12(long userID, string password)
        {
            //SuUser user = QueryProvider.SuUserQuery.FindByIdentity(userID);
            SuUser sendToUser = QueryProvider.SuUserQuery.FindByIdentity(userID);
            SuUser sendUser = QueryProvider.SuUserQuery.FindByIdentity(SystemUserID);
            StringTemplateGroup group = new StringTemplateGroup("group", AppDomain.CurrentDomain.BaseDirectory + "Resources\\Template\\Email");
            StringTemplate html = group.GetInstanceOf("EM12");
            //Create DTO
            EmailDTO email = new EmailDTO();
            email.Sender.Email = sendUser.Email;
            email.Sender.Name = sendUser.EmployeeName;
            email.MailSubject = "Your password has been reset on e-Xpense system.";
            email.MailSendingType = Email.MailType.HTML;
            html.SetAttribute("userName", sendToUser == null ? string.Empty : sendToUser.EmployeeName);
            html.SetAttribute("userID", sendToUser == null ? string.Empty : sendToUser.UserName);
            html.SetAttribute("password", password);
            html.SetAttribute("link", ParameterServices.UrlLink);
            email.MailBody = html.ToString();
            email.EmailType = "EM12";

            if (sendToUser != null)
            {
                email.MailSendTo.Add(new AddMailSendTo() { Email = sendToUser.Email, Name = sendToUser.EmployeeName });

                //Call Email Service for send email
                SendEmail(email);
            }

            //send completed then must keep log
            //AddEmailLog(mailBody);
        }
        #endregion public void SendEmailEM12(long userID, string password)

        #region public void SendEmailEM13(long documentID)
        public void SendEmailEM13(long documentID)
        {
            WorkFlow wfid = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(documentID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.FindByIdentity(documentID);
            SuUser sendUser = QueryProvider.SuUserQuery.FindByIdentity(SystemUserID);
            SuUser sendToUser = QueryProvider.SuUserQuery.FindByIdentity(scgDocument.RequesterID.Userid);

            //Create DTO
            EmailDTO email = new EmailDTO();
            email.Sender.Email = sendUser.Email;
            email.Sender.Name = sendUser.EmployeeName;
            email.MailSubject = string.Format("Employee Expense : {0} - Confirm Remittance Advance", scgDocument.DocumentNo);
            email.MailSendingType = Email.MailType.HTML;

            #region SetAttribute
            StringTemplate html = this.BuildDocumentList(documentID, "EM13");
            html.SetAttribute("DearName", sendToUser == null ? string.Empty : sendToUser.EmployeeName);
            html.SetAttribute("DocumentNo", scgDocument.DocumentNo);
            html.SetAttribute("RequesterName", scgDocument.RequesterID.EmployeeName);
            html.SetAttribute("link", ParameterServices.UrlLink);
            html.SetAttribute("wfid", wfid.WorkFlowID.ToString());
            #endregion SetAttribute

            email.MailBody = html.ToString();

            email.EmailType = "EM13";
            email.RequesterNo = scgDocument.DocumentNo;

            if (sendToUser != null)
            {
                email.MailSendTo.Add(new AddMailSendTo() { Email = sendToUser.Email, Name = sendToUser.EmployeeName });
                //Call Email Service for send email
                this.SendEmail(email);
            }
        }
        #endregion public void SendEmailEM13(long documentID)

        #region public void SendEmailEM14(long workFlowID, long sendToUserID , string tokenCode)
        public void SendEmailEM14(long workFlowID, long sendToUserID, string tokenCode)
        {
            //1. Query from WorkFlowResponseToken Table
            IList<WorkFlowResponseToken> responseTokens = WorkFlowQueryProvider.WorkFlowResponseTokenQuery.FindByTokenCode(tokenCode);

            int? approveStateEventID = null;
            string approveStateEventDisplayName = null;
            string approveStateEventURL = null;
            int? rejectStateEventID = null;
            string rejectStateEventDisplayName = null;
            string rejectStateEventURL = null;

            foreach (WorkFlowResponseToken responseToken in responseTokens)
            {
                if (responseToken.WorkFlowStateEvent.Name == "Approve")
                {
                    approveStateEventID = responseToken.WorkFlowStateEvent.WorkFlowStateEventID;
                    approveStateEventDisplayName = WorkFlowStateEventQuery.GetTranslatedEventName(approveStateEventID.Value, ParameterServices.DefaultLanguage);
                    approveStateEventURL =
                        String.Format(ParameterServices.ApproveRejectURL,
                            tokenCode, sendToUserID, "Email", approveStateEventID.Value);
                }
                else if (responseToken.WorkFlowStateEvent.Name == "Reject")
                {
                    rejectStateEventID = responseToken.WorkFlowStateEvent.WorkFlowStateEventID;
                    rejectStateEventDisplayName = WorkFlowStateEventQuery.GetTranslatedEventName(rejectStateEventID.Value, ParameterServices.DefaultLanguage);
                    rejectStateEventURL =
                        String.Format(ParameterServices.ApproveRejectURL,
                            tokenCode, sendToUserID, "Email", rejectStateEventID.Value);
                }
            }

            SuUser sendUser = QueryProvider.SuUserQuery.FindByIdentity(SystemUserID);
            SuUser sendToUser = QueryProvider.SuUserQuery.FindByIdentity(sendToUserID);
            Document document = WorkFlowQueryProvider.WorkFlowQuery.GetDocumentByWorkFlowID(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.FindByIdentity(document.DocumentID);

            EmailDTO email = new EmailDTO();
            if (sendToUser != null)
                email.MailSendTo.Add(new AddMailSendTo() { Name = sendToUser.UserName, Email = sendToUser.Email });
            email.Sender.Email = sendUser.Email;
            email.Sender.Name = sendUser.EmployeeName;

            // Edit By Kookkla
            // แก้เรื่องการส่ง Subject Email
            if (string.IsNullOrEmpty(approveStateEventDisplayName) && string.IsNullOrEmpty(rejectStateEventDisplayName))
                email.MailSubject = string.Format("Remind e-Xpense :  Approve/Reject {0}", scgDocument.DocumentNo);
            else
                email.MailSubject = string.Format("Remind e-Xpense :  {1}/{2} {0}", scgDocument.DocumentNo, approveStateEventDisplayName, rejectStateEventDisplayName);

            email.MailSendingType = Email.MailType.HTML;

            #region SetAttribute
            StringTemplate html = this.BuildDocumentList(document.DocumentID, "EM14");
            // Change to use Employee name by Anuwat S on 10/05/2009
            html.SetAttribute("DearName", sendToUser == null ? string.Empty : sendToUser.EmployeeName);
            html.SetAttribute("RequesterName", scgDocument.RequesterID.EmployeeName);
            html.SetAttribute("RequestId", scgDocument.DocumentNo);
            html.SetAttribute("link", ParameterServices.UrlLink);
            html.SetAttribute("wfid", workFlowID.ToString());
            html.SetAttribute("ApproveURL", approveStateEventURL);
            html.SetAttribute("ApproveEventDisplayName", approveStateEventDisplayName);
            html.SetAttribute("RejectURL", rejectStateEventURL);
            html.SetAttribute("RejectEventDisplayName", rejectStateEventDisplayName);
            #endregion SetAttribute

            email.MailBody = html.ToString();

            email.EmailType = "EM14";
            email.RequesterNo = scgDocument.DocumentNo;

            //SCGEmailService.SendEmail(email);
            //4. Call Email Service for send email
            if (ParameterServices.EnableEmail014)    // use for enable or disable send email
                this.SendEmail(email);

            //send completed then must keep log
            //AddEmailLog(mailBody);
        }
        #endregion public void SendEmailEM14(long workFlowID, long sendToUserID , string tokenCode)

        #region public void SendEmailEM15(long workFlowID, long sendToUserID, string tokenCode)
        public void SendEmailEM15(long workFlowID, long sendToUserID)
        {
            
            SuUser sendUser = QueryProvider.SuUserQuery.FindByIdentity(SystemUserID);
            SuUser sendToUser = QueryProvider.SuUserQuery.FindByIdentity(sendToUserID);
            Document document = WorkFlowQueryProvider.WorkFlowQuery.GetDocumentByWorkFlowID(workFlowID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.FindByIdentity(document.DocumentID);

            EmailDTO email = new EmailDTO();
            if (sendToUser != null)
                email.MailSendTo.Add(new AddMailSendTo() { Name = sendToUser.UserName, Email = sendToUser.Email });
            email.Sender.Email = sendUser.Email;
            email.Sender.Name = sendUser.EmployeeName;
            email.MailSubject = string.Format("Employee Expense : {0} - Fixed Advance Notification.", sendUser.EmployeeName);
            email.MailSendingType = Email.MailType.HTML;

            #region SetAttribute
            StringTemplate html = this.BuildDocumentList(document.DocumentID, "EM15");
            #endregion SetAttribute

            email.MailBody = html.ToString();

            email.EmailType = "EM15";
            email.RequesterNo = scgDocument.DocumentNo;

            //SCGEmailService.SendEmail(email);
            //4. Call Email Service for send email
            if (ParameterServices.EnableEmail15)    // use for enable or disable send email
                this.SendEmail(email);

            //send completed then must keep log
            //AddEmailLog(mailBody);
        }
        #endregion

        #region public void SendEmailEM16(long documentID)
        public void SendEmailEM16(long documentID)
        {
            WorkFlow wfid = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(documentID);
            SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.FindByIdentity(documentID);
            SuUser sendUser = QueryProvider.SuUserQuery.FindByIdentity(SystemUserID);
            SuUser sendToUser = QueryProvider.SuUserQuery.FindByIdentity(scgDocument.RequesterID.Userid);

            //Create DTO
            EmailDTO email = new EmailDTO();
            email.Sender.Email = sendUser.Email;
            email.Sender.Name = sendUser.EmployeeName;
            email.MailSubject = string.Format("Employee Expense : {0} - Fixed Advance Notification.", scgDocument.DocumentNo);
            email.MailSendingType = Email.MailType.HTML;

            #region SetAttribute
            StringTemplate html = this.BuildDocumentList(documentID, "EM16");
            html.SetAttribute("DearName", sendToUser == null ? string.Empty : sendToUser.EmployeeName);
            html.SetAttribute("DocumentNo", scgDocument.DocumentNo);
            html.SetAttribute("RequesterName", scgDocument.RequesterID.EmployeeName);
            html.SetAttribute("link", ParameterServices.UrlLink);
            html.SetAttribute("wfid", wfid.WorkFlowID.ToString());
            #endregion SetAttribute

            email.MailBody = html.ToString();

            email.EmailType = "EM16";
            email.RequesterNo = scgDocument.DocumentNo;

            if (sendToUser != null)
            {
                email.MailSendTo.Add(new AddMailSendTo() { Email = sendToUser.Email, Name = sendToUser.EmployeeName });
                //Call Email Service for send email
                if (ParameterServices.EnableEmail16)    // use for enable or disable send email
                    this.SendEmail(email);
                
            }
        }
        #endregion public void SendEmailEM16(long documentID)


        #endregion

        #region private void AddEmailLog(EmailDTO mail)
        private void AddEmailLog(EmailDTO mail)
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
            emailLog.CreBy = UserAccount.UserID;
            emailLog.CreDate = DateTime.Now;
            emailLog.UpdBy = UserAccount.UserID;
            emailLog.UpdDate = DateTime.Now;
            emailLog.UpdPgm = UserAccount.CurrentProgramCode;
            emailLog.Remark = mail.Remark;

            SuEmailLogDao.Save(emailLog);
        }
        #endregion private void AddEmailLog(EmailDTO mail)

        #region ISCGEmailService Members

        private bool IsEmptyMailList(List<AddMailSendTo> listSend)
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

        private bool IsValidEmailFormat(string emailAddress)
        {
            if (emailAddress.Contains('@') && emailAddress.Contains('.'))
            {
                return true;
            }
            return false;
        }

        public void ResendEmail()
        {
            QueryProvider.SuEmailResendingQuery.DeleteSuccessItem();
            IList<SuEmailResending> ResendMails = QueryProvider.SuEmailResendingQuery.FindAllEmailResending();
            EmailSerializer serailer = new EmailSerializer();
            SMSSerializer smsserialer = new SMSSerializer();
            foreach (SuEmailResending item in ResendMails)
            {
                bool success;
                if (item.emailtype.Contains("SMS"))
                {
                    SMSContainer sms = smsserialer.DeSerializeObject(item.mailcontent);
                    if (item.emailtype == "SMS")
                    {
                        success = SMSService.ReSend(sms.sms);
                    }
                    else if (item.emailtype == "SMS+Log")
                    {
                        success = SMSService.ReSend(sms.sms, sms.SMSLogid);
                    }
                    else if (item.emailtype == "SMS+Notify")
                    {
                        success = SMSService.ReSend(sms.sms, sms.NotifySMS);
                    }
                    else
                    {
                        success = false;
                    }
                }
                else
                {
                    EmailDTO email = serailer.DeSerializeObject(item.mailcontent);
                    if (IsEmptyMailList(email.MailSendTo))
                    {
                        AddMailSendTo sendto = new AddMailSendTo();
                        sendto.Email = ParameterServices.AdminEmailAddress;
                        sendto.Name = ParameterServices.AdminName;
                        email.MailSendTo.Clear();
                        email.MailSendTo.Add(sendto);
                        email.Remark = "Send to email addresses is empty or invalid email format.";
                    }

                    success = EmailService.SendEmail(email);
                    if (success)
                    {
                        email.Status = 1; // Email Status = 1 is Send Complete.
                    }
                    else
                    {
                        email.Status = 2; // Email Status = 2 is Send Incomplete.
                    }
                    //send completed then must keep log
                    AddEmailLog(email);
                }
                if (success)
                {
                    item.status = "Success";
                }
                else
                {
                    item.status = "Retry";
                }
                item.lastsenddate = DateTime.Now;
                item.retry++;
                if (item.retry > ParameterServices.MaxRetry)
                {
                    item.status = "Fail";
                }
                SuEmailResendingService.Update(item);
            }
        }

        #endregion
    }
}

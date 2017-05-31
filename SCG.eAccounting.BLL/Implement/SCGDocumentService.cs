using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SCG.eAccounting.BLL;
using SCG.eAccounting.DAL;
using SCG.eAccounting.DTO;
using System.Data;
using SS.Standard.Utilities;
using SCG.eAccounting.DTO.DataSet;
using SS.Standard.WorkFlow.Query;
using SCG.eAccounting.Query;
using SS.SU.Query;
using SS.SU.DTO;
using SCG.DB.DTO;
using SCG.DB.Query;
using SS.Standard.Security;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.Report;
using SS.DB.Query;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.Service.Implement;
using SS.Standard.WorkFlow.Service;

namespace SCG.eAccounting.BLL.Implement
{
    public class SCGDocumentService : ServiceBase<SCGDocument, long>, ISCGDocumentService
    {
        public IUserAccount UserAccount { get; set; }
        public ITransactionService TransactionService { get; set; }
        public IAvAdvanceDocumentService AvAdvanceDocumentService { get; set; }
        public IFnExpenseDocumentService FnExpenseDocumentService { get; set; }
        public IFixedAdvanceDocumentService FixedAdvanceDocumentService { get; set; }
        public IFnRemittanceService FnRemittanceService { get; set; }
        public IDocumentAttachmentService DocumentAttachmentService { get; set; }
        public IDocumentInitiatorService DocumentInitiatorService { get; set; }
        public ISCGEmailService SCGEmailService { get; set; }
        public IWorkFlowResponseTokenService WorkFlowResponseTokenService { get; set; }

        #region Override Method
        public override IDao<SCGDocument, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.SCGDocumentDao;
        }
        #endregion

        public void PrepareDataToDataset(DataSet documentDataset, long documentID)
        {
            PrepareDataInternalToDataset(documentDataset, documentID, false);
        }

        public void PrepareDataInternalToDataset(DataSet documentDataset, long documentID, bool isCopy)
        {
            SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.FindByIdentity(documentID);

            //// Set data to Document row in documentDataset.
            DataRow documentRow = documentDataset.Tables["Document"].NewRow();

            if (document.CompanyID != null)
            {
                documentRow["CompanyID"] = document.CompanyID.CompanyID;
            }
            if (document.DocumentType != null)
            {
                documentRow["DocumentTypeID"] = document.DocumentType.DocumentTypeID;
            }
            if (document.RequesterID != null)
            {
                documentRow["RequesterID"] = document.RequesterID.Userid;
            }
            if (document.ApproverID != null)
            {
                documentRow["ApproverID"] = document.ApproverID.Userid;
            }
            if (document.ReceiverID != null)
            {
                documentRow["ReceiverID"] = document.ReceiverID.Userid;
            }
            if (document.CreatorID != null)
            {
                documentRow["CreatorID"] = document.CreatorID.Userid;
            }

            if (!isCopy)
            {
                documentRow["DocumentID"] = document.DocumentID;
                documentRow["DocumentNo"] = document.DocumentNo;
                if (document.CreatorID != null)
                {
                    documentRow["CreatorID"] = document.CreatorID.Userid;
                }
                //add new field
                if (document.PaymentMethodID != null)
                {
                    documentRow["PaymentMethodID"] = document.PaymentMethodID;
                }
                if (document.PostingDate != null)
                {
                    documentRow["PostingDate"] = document.PostingDate;
                }
                else
                {
                    documentRow["PostingDate"] = DateTime.MinValue;
                }
                if (document.BaseLineDate != null)
                {
                    documentRow["BaseLineDate"] = document.BaseLineDate;
                }
                else
                {
                    documentRow["BaseLineDate"] = DateTime.MinValue;
                }
                if (document.BankAccount != null)
                {
                    documentRow["BankAccount"] = document.BankAccount;
                }
                if (!string.IsNullOrEmpty(document.BranchCode))
                    documentRow["BranchCode"] = document.BranchCode;
                else
                    documentRow["BranchCode"] = string.Empty;
                if (!string.IsNullOrEmpty(document.BusinessArea))
                    documentRow["BusinessArea"] = document.BusinessArea;
                else
                    documentRow["BusinessArea"] = string.Empty;

                if (!string.IsNullOrEmpty(document.Supplementary))
                    documentRow["Supplementary"] = document.Supplementary;
                else
                    documentRow["Supplementary"] = string.Empty;
                if (document.ApproveDate != null)
                {
                    documentRow["ApproveDate"] = document.ApproveDate;
                }
                else
                {
                    documentRow["ApproveDate"] = DateTime.MinValue;
                }
                if (!string.IsNullOrEmpty(document.PostingStatus))
                    documentRow["PostingStatus"] = document.PostingStatus;
                else
                    documentRow["PostingStatus"] = string.Empty;
                documentRow["IsVerifyImage"] = document.IsVerifyImage;
                if (document.DocumentDate != null)
                {
                    documentRow["DocumentDate"] = document.DocumentDate;
                }
                else
                {
                    documentRow["DocumentDate"] = DateTime.MinValue;
                }
            }

            documentRow["Subject"] = document.Subject;
            documentRow["Memo"] = document.Memo;
            documentRow["Active"] = document.Active;
            documentRow["CreBy"] = document.CreBy;
            documentRow["CreDate"] = document.CreDate;
            documentRow["UpdBy"] = document.UpdBy;
            documentRow["UpdDate"] = document.UpdDate;
            documentRow["UpdPgm"] = document.UpdPgm;

            // Add document row to documentDataset.
            documentDataset.Tables["Document"].Rows.Add(documentRow);

            if (!isCopy)
            {
                // Prepare data to DocumentAttachment datatable.
                DocumentAttachmentService.PrepareDataToDataset(documentDataset, documentID);
            }
            // Prepare data to DocumentInitiator datatable.
            DocumentInitiatorService.PrepareDataToDataset(documentDataset, documentID);
        }
        public void UpdateTransactionDocument(Guid TxID, SCGDocument document, bool haveReceiver, bool haveApprover)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            try
            {
                this.ValidateSCGDocument(document, haveReceiver, haveApprover);
            }
            catch (ServiceValidationException ex)
            {
                errors.MergeErrors(ex.ValidationErrors);
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            DataSet ds = TransactionService.GetDS(TxID);
            DataTable documentTable = ds.Tables["Document"];
            if (documentTable.Rows.Count > 0)
            {
                DataRow documentRow = documentTable.Rows[0];
                documentRow.BeginEdit();

                if (document.CompanyID != null)
                {
                    documentRow["CompanyID"] = document.CompanyID.CompanyID;
                }
                if (document.CreatorID != null)
                {
                    documentRow["CreatorID"] = document.CreatorID.Userid;
                }
                if (document.RequesterID != null)
                {
                    documentRow["RequesterID"] = document.RequesterID.Userid;
                }
                if (document.ApproverID != null)
                {
                    documentRow["ApproverID"] = document.ApproverID.Userid;
                }
                if (document.ReceiverID != null)
                {
                    documentRow["ReceiverID"] = document.ReceiverID.Userid;
                }
                //documentRow["DocumentNo"] = document.DocumentNo;
                if (document.DocumentType != null)
                {
                    documentRow["DocumentTypeID"] = document.DocumentType.DocumentTypeID;
                }
                IList<object> advanceEditableField = AvAdvanceDocumentService.GetEditableFields(document.DocumentID, null);
                IList<object> expenseEditableField = FnExpenseDocumentService.GetEditableFields(document.DocumentID);
                IList<object> remittanceEditableField = FnRemittanceService.GetEditableFields(document.DocumentID);
                IList<object> fixedAdvanceEditableField = FixedAdvanceDocumentService.GetEditableFields(document.DocumentID);
                if (advanceEditableField.Contains(AdvanceFieldGroup.VerifyDetail) || expenseEditableField.Contains(ExpenseFieldGroup.VerifyDetail) || remittanceEditableField.Contains(RemittanceFieldGroup.VerifyDetail) || fixedAdvanceEditableField.Contains(FixedAdvanceFieldGroup.VerifyDetail))
                {
                    //add new field
                    if (!string.IsNullOrEmpty(document.BranchCode))
                    {
                        documentRow["BranchCode"] = document.BranchCode;
                    }
                    if (!string.IsNullOrEmpty(document.BusinessArea))
                    {
                        documentRow["BusinessArea"] = document.BusinessArea;
                    }
                    else
                    {
                        documentRow["BusinessArea"] = string.Empty;
                    }

                    if (!string.IsNullOrEmpty(document.Supplementary))
                    {
                        documentRow["Supplementary"] = document.Supplementary;
                    }
                    else
                    {
                        documentRow["Supplementary"] = string.Empty;
                    }

                    if (document.PaymentMethodID.HasValue && document.PaymentMethodID > 0)
                    {
                        documentRow["PaymentMethodID"] = document.PaymentMethodID.Value;
                    }
                    if (document.PostingDate.HasValue && document.PostingDate.Value != DateTime.MinValue)
                    {
                        documentRow["PostingDate"] = document.PostingDate;
                    }
                    if (document.BaseLineDate.HasValue && document.BaseLineDate.Value != DateTime.MinValue)
                    {
                        documentRow["BaseLineDate"] = document.BaseLineDate;
                    }
                    if (!string.IsNullOrEmpty(document.BankAccount))
                    {
                        documentRow["BankAccount"] = document.BankAccount;
                    }
                }
                if (!string.IsNullOrEmpty(document.ReferenceNo))
                {
                    documentRow["ReferenceNo"] = document.ReferenceNo;
                }
                else
                {
                    if (!documentRow["DocumentNo"].ToString().StartsWith("EHR"))
                    {
                        documentRow["ReferenceNo"] = document.ReferenceNo;
                    }
                }

                documentRow["IsVerifyImage"] = document.IsVerifyImage;
                documentRow["Subject"] = document.Subject;
                documentRow["Memo"] = document.Memo;
                documentRow["Active"] = document.Active;
                documentRow["CreBy"] = UserAccount.UserID;
                documentRow["CreDate"] = DateTime.Now;
                documentRow["UpdBy"] = UserAccount.UserID;
                documentRow["UpdDate"] = DateTime.Now;
                documentRow["UpdPgm"] = UserAccount.CurrentProgramCode;

                documentRow.EndEdit();
            }
        }

        public long InsertSCGDocument(Guid txID, long documentID)
        {
            DataSet ds = TransactionService.GetDS(txID);
            DataTable documentTable = ds.Tables["Document"];
            SCGDocument document = new SCGDocument();

            long scgDocumentID = 0;

            if (documentTable.Rows.Count > 0)
            {
                this.BuildDTODocument(documentTable, document);
                scgDocumentID = ScgeAccountingDaoProvider.SCGDocumentDao.Save(document);
            }

            // Save Document Attachment to Database.
            DocumentAttachmentService.InsertDocumentAttachment(txID, scgDocumentID);

            // Save Document Initiator to Database.
            // Wait for save document initiator from transaction to database.

            return scgDocumentID;
        }
        public long UpdateSCGDocument(Guid txID, long documentID)
        {
            DataSet ds = TransactionService.GetDS(txID);
            DataTable documentTable = ds.Tables["Document"];
            SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.FindProxyByIdentity(documentID);

            if (documentTable.Rows.Count > 0)
            {
                this.BuildDTODocument(documentTable, document);
                ScgeAccountingDaoProvider.SCGDocumentDao.SaveOrUpdate(document);
            }

            // Save Document Attachment to Database.
            DocumentAttachmentService.InsertDocumentAttachment(txID, documentID);
            DocumentAttachmentService.DeleteDocumentAttachment(txID);

            // Save Document Initiator to Database.
            // Wait for save document initiator from transaction to database.
            DocumentInitiatorService.InsertDocumentInitiator(txID, documentID);
            DocumentInitiatorService.UpdateDocumentInitiator(txID, documentID);
            DocumentInitiatorService.DeleteDocumentInitiator(txID);

            return documentID;
        }

        public long SaveSCGDocument(Guid txID, long documentID)
        {
            DataSet ds = TransactionService.GetDS(txID);
            DataTable dtDocument = ds.Tables["Document"];

            // Insert, Update, Delete SCGDocument.
            documentID = ScgeAccountingDaoProvider.SCGDocumentDao.Persist(dtDocument);

            // Insert, Update, Delete DocumentInitiator.
            DocumentInitiatorService.SaveDocumentInitiator(txID, documentID);

            // Insert, Update, Delete DocumentInitiator.
            DocumentAttachmentService.SaveDocumentAttachment(txID, documentID);

            return documentID;
        }
        private void BuildDTODocument(DataTable documentTable, SCGDocument document)
        {
            DataRow documentRow = documentTable.Rows[0];
            document.CompanyID = new SCG.DB.DTO.DbCompany(Convert.ToInt64(documentRow["CompanyID"].ToString()));
            document.CreatorID = new SS.SU.DTO.SuUser(Convert.ToInt64(documentRow["CreatorID"].ToString()));
            document.RequesterID = new SS.SU.DTO.SuUser(Convert.ToInt64(documentRow["RequesterID"].ToString()));
            if (!string.IsNullOrEmpty(documentRow["ReceiverID"].ToString()))
            {
                document.ReceiverID = QueryProvider.SuUserQuery.FindProxyByIdentity(Convert.ToInt64(documentRow["ReceiverID"].ToString()));
            }
            if (!string.IsNullOrEmpty(documentRow["ApproverID"].ToString()))
            {
                document.ApproverID = QueryProvider.SuUserQuery.FindProxyByIdentity(Convert.ToInt64(documentRow["ApproverID"].ToString()));
            }
            document.DocumentNo = documentRow["DocumentNo"].ToString();
            document.DocumentType = new SS.Standard.WorkFlow.DTO.DocumentType(Convert.ToInt32(documentRow["DocumentTypeID"].ToString()));
            document.Subject = documentRow["Subject"].ToString();
            document.Memo = documentRow["Memo"].ToString();
            //add new field
            document.BranchCode = documentRow["BranchCode"].ToString();
            document.BusinessArea = documentRow["BusinessArea"].ToString();
            document.Supplementary = documentRow["Supplementary"].ToString();
            document.PaymentMethodID = Convert.ToInt64(documentRow["PaymentMethodID"]);
            document.PostingDate = DateTime.Parse(documentRow["PostingDate"].ToString());
            document.BaseLineDate = DateTime.Parse(documentRow["BaseLineDate"].ToString());
            document.BankAccount = documentRow["BankAccount"].ToString();
            document.Active = (bool)documentRow["Active"];
            document.CreBy = Convert.ToInt64(documentRow["CreBy"].ToString());
            document.CreDate = DateTime.Parse(documentRow["CreDate"].ToString());
            document.UpdBy = Convert.ToInt64(documentRow["UpdBy"].ToString());
            document.UpdDate = DateTime.Parse(documentRow["UpdDate"].ToString());
            document.UpdPgm = documentRow["UpdPgm"].ToString();
            document.DocumentDate = DateTime.Parse(documentRow["DocumentDate"].ToString());
        }

        public void ValidateSCGDocument(SCGDocument document, bool haveReceiver, bool haveApprover)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            // DocumentSubTypeID = ใบของบประมาณระหว่างงวด. Not use in eAccounting.
            // Must specific Approver.
            if (document.CompanyID == null)
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("CompanyIsRequired"));
            }
            else
            {
                DbCompany company = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(document.CompanyID.CompanyID);
                if (company == null)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("CompanyIsRequired"));
                }
            }
            if (document.PaymentMethodID == 39)
            {
                if (document.RequesterID.Userid != document.ReceiverID.Userid)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("ReceiverCannotBeDifferentRequester"));
                }
            }

            if (string.IsNullOrEmpty(document.Subject))
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("SubjectIsRequired"));
            }

            if (haveReceiver)
            {
                if (document.ReceiverID == null)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("ReceiverIsRequired"));
                }
                else
                {
                    SuUser receiver = QueryProvider.SuUserQuery.FindByIdentity(document.ReceiverID.Userid);

                    if (document.ReceiverID == null)
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("ReceiverIsRequired"));
                    }
                }
            }

            if (haveApprover)
            {
                if (document.ApproverID == null)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("ApproverIsRequired"));
                }
                else
                {
                    SuUser approver = QueryProvider.SuUserQuery.FindByIdentity(document.ApproverID.Userid);

                    if (approver == null)
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("ApproverIsRequired"));
                    }
                }
            }

            if (document.RequesterID == null)
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RequesterIsRequired"));
            }
            else
            {
                SuUser requester = QueryProvider.SuUserQuery.FindByIdentity(document.RequesterID.Userid);

                if (requester == null)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RequesterIsRequired"));
                }
            }
            //add new field
            //IList<object> advanceEditableField = AvAdvanceDocumentService.GetEditableFields(document.DocumentID, null);
            IList<object> expenseEditableField = FnExpenseDocumentService.GetEditableFields(document.DocumentID);
            IList<object> remittanceEditableField = FnRemittanceService.GetEditableFields(document.DocumentID);
            if (/*advanceEditableField.Contains(AdvanceFieldGroup.VerifyDetail) ||*/ expenseEditableField.Contains(ExpenseFieldGroup.VerifyDetail) || remittanceEditableField.Contains(RemittanceFieldGroup.VerifyDetail))
            {
                if (string.IsNullOrEmpty(document.BranchCode))
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("BranchCodeIsRequired"));
                }
                //if (!document.PaymentMethodID.HasValue)
                //{
                //    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("PaymentMethodID_IsRequired"));
                //}
                if (!document.PostingDate.HasValue)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("PostingDateIsRequired"));
                }
                if (!document.BaseLineDate.HasValue)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("BaseLineDateIsRequired"));
                }
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
        }

        #region ISCGDocumentService Members


        public byte[] GenerateReimbursementReport(string markList, string unMarkList, string pbCode, string pbName, string companyName, string username, string maxPaidDate, string minPaidDate)
        {
            ReimbursementReportValueObj obj = ScgeAccountingQueryProvider.SCGDocumentQuery.GetPeriodPaidDate(markList, unMarkList);
            if (string.IsNullOrEmpty(minPaidDate))
            {
                minPaidDate = obj.MinPaidDate.HasValue ? obj.MinPaidDate.Value.ToString("MMM dd, yyyy") : string.Empty;
            }

            if (string.IsNullOrEmpty(maxPaidDate))
            {
                maxPaidDate = obj.MaxPaidDate.HasValue ? obj.MaxPaidDate.Value.ToString("MMM dd, yyyy") : string.Empty;
            }

            List<ReportParameter> param = new List<ReportParameter>();
            ReportParameter param1 = new ReportParameter();
            ReportParameter param2 = new ReportParameter();
            ReportParameter param3 = new ReportParameter();
            ReportParameter param4 = new ReportParameter();
            ReportParameter param5 = new ReportParameter();
            ReportParameter param6 = new ReportParameter();
            ReportParameter param7 = new ReportParameter();
            ReportParameter param8 = new ReportParameter();
            param1.ParamterName = "markList";
            param1.ParamterValue = markList;
            param2.ParamterName = "unMarkList";
            param2.ParamterValue = unMarkList;
            param3.ParamterName = "pbCode";
            param3.ParamterValue = pbCode;
            param4.ParamterName = "companyName";
            param4.ParamterValue = companyName;
            param5.ParamterName = "username";
            param5.ParamterValue = username;
            param6.ParamterName = "maxPaidDate";
            param6.ParamterValue = maxPaidDate;
            param7.ParamterName = "minPaidDate";
            param7.ParamterValue = minPaidDate;
            param8.ParamterName = "pbName";
            param8.ParamterValue = pbName;
            param.Add(param1);
            param.Add(param2);
            param.Add(param3);
            param.Add(param4);
            param.Add(param5);
            param.Add(param6);
            param.Add(param7);
            param.Add(param8);

            FilesGenerator rp = new FilesGenerator();

            byte[] results = rp.GetByte(ParameterServices.ReportingURL, ParameterServices.ReportUserName, ParameterServices.ReportPassword, ParameterServices.ReportDomainName, ParameterServices.ReportFolderPath, "ReimbursementReport", param, FilesGenerator.ExportType.PDF);
            //byte[] results = rp.GetByte("http://172.20.56.116:81/ReportServer/ReportExecution2005.asmx", "RSeAccounting", "p@ssw0rd", "SCG-EACCOUNTING", "eAccountingReports", report.ReportName, param, FilesGenerator.ExportType.PDF);
            return results;
        }
        public byte[] GeneratePDF(long documentID)
        {
            DocumentReportName report = ScgeAccountingQueryProvider.SCGDocumentQuery.GetReportNameByDocumentID(documentID);
            List<ReportParameter> param = new List<ReportParameter>();
            ReportParameter param1 = new ReportParameter();
            //Hard code for parameter name.
            if (report.ReportName == "AVReportDomestic")
            {
                param1.ParamterName = "AdvanceID";
            }
            else if (report.ReportName == "AVReportForeign")
            {
                param1.ParamterName = "AdvanceID";
            }
            else if (report.ReportName == "TAReport")
            {
                param1.ParamterName = "TADocID";
            }
            else if (report.ReportName == "RMTADVReport")
            {
                param1.ParamterName = "RemittanceID";
            }
            else if (report.ReportName == "AdvReimbursementDomestic")
            {
                param1.ParamterName = "expenseId";

            }
            else if (report.ReportName == "PettyCashReimbursementDM")
            {
                param1.ParamterName = "expenseId";
            }
            else if (report.ReportName == "AdvReimbursementForeign")
            {
                param1.ParamterName = "expenseId";
            }
            else if (report.ReportName == "PettyCashReimbursementFR")
            {
                param1.ParamterName = "expenseId";
            }
            else if (report.ReportName == "MPADocumentPrintForm")
            {
                param1.ParamterName = "MPADocumentID";
            }
            else if (report.ReportName == "CADocumentPrintForm")
            {
                param1.ParamterName = "CADocumentID";
            }
            else if (report.ReportName == "FixedAdvanceDocumentPrintForm")
            {
                param1.ParamterName = "DocumentID";
            }
            else
            {
                return null;
            }

            param1.ParamterValue = report.ReportParam;
            param.Add(param1);

            FilesGenerator rp = new FilesGenerator();

            byte[] results = rp.GetByte(ParameterServices.ReportingURL, ParameterServices.ReportUserName, ParameterServices.ReportPassword, ParameterServices.ReportDomainName, ParameterServices.ReportFolderPath, report.ReportName, param, FilesGenerator.ExportType.PDF);
            //byte[] results = rp.GetByte("http://172.20.56.116:81/ReportServer/ReportExecution2005.asmx", "RSeAccounting", "p@ssw0rd", "SCG-EACCOUNTING", "eAccountingReports", report.ReportName, param, FilesGenerator.ExportType.PDF);
            return results;
        }

        #endregion

        public void ValidateVerifyDetail(Guid txId, ViewPostDocumentType docType, bool isRepOffice)
        {
            DataSet ds = TransactionService.GetDS(txId);
            DataTable documentTable = ds.Tables["Document"];

            SCGDocument document = new SCGDocument();

            if (documentTable.Rows.Count > 0)
                document.LoadFromDataRow(documentTable.Rows[0]);

            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (string.IsNullOrEmpty(document.BranchCode))
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("BranchCodeIsRequired"));
            }
            if (!document.PostingDate.HasValue)
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("PostingDateIsRequired"));
            }
            if (!document.BaseLineDate.HasValue)
            {
                if (docType.Equals(ViewPostDocumentType.AdvanceForeign))
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("ValueDateIsRequired"));
                else
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("BaseLineDateIsRequired"));
                //errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("BaseLineDate is Required."));
            }

            if (docType.Equals(ViewPostDocumentType.AdvanceForeign))
            {
                if (!isRepOffice && string.IsNullOrEmpty(document.BankAccount))
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("BankAccountIsRequired"));
                }
            }
            else if (!docType.Equals(ViewPostDocumentType.Remittance))
            {
                if (!document.PaymentMethodID.HasValue)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("PaymentMethodIDIsRequired"));
                }
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
        }
        public void MessageValidation (string chk)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (chk != "" )
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage(chk));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

        }
        #region public void DeleteDocumentByDocumentID(long documentID)

        public void DeleteDocumentByDocumentID(long documentID)
        {
            SCGDocument document = this.FindProxyByIdentity(documentID);
            ScgeAccountingDaoProvider.SCGDocumentDao.Delete(document);
        }
        #endregion public void DeleteDocumentByDocumentID(long advanceID)

        public void UpdateAdvanceSCGDocument(long taDocumentID, long advDocumentID)
        {
            SCGDocument taDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.FindProxyByIdentity(taDocumentID);
            SCGDocument avDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.FindProxyByIdentity(advDocumentID);

            if (taDocument != null)
            {
                avDocument.ApproverID = taDocument.ApproverID;
                ScgeAccountingDaoProvider.SCGDocumentDao.SaveOrUpdate(taDocument);
            }

            // Save Document Initiator to Database.
            // Wait for save document initiator from transaction to database.
            IList<DocumentInitiator> taInitiator = ScgeAccountingQueryProvider.DocumentInitiatorQuery.GetDocumentInitiatorByDocumentID(taDocumentID);

            ScgeAccountingDaoProvider.DocumentInitiatorDao.DeleteDocumentInitiatorByDocumentID(advDocumentID);

            foreach (DocumentInitiator documentInitiator in taInitiator)
            {
                DocumentInitiator advInitiator = new DocumentInitiator();

                advInitiator.DocumentID = avDocument;
                advInitiator.Seq = documentInitiator.Seq;
                advInitiator.UserID = documentInitiator.UserID;
                advInitiator.InitiatorType = documentInitiator.InitiatorType;
                advInitiator.DoApprove = false;
                advInitiator.Active = documentInitiator.Active;
                advInitiator.CreBy = documentInitiator.CreBy;
                advInitiator.CreDate = documentInitiator.CreDate;
                advInitiator.UpdBy = documentInitiator.UpdBy;
                advInitiator.UpdDate = documentInitiator.UpdDate;
                advInitiator.UpdPgm = documentInitiator.UpdPgm;

                ScgeAccountingDaoProvider.DocumentInitiatorDao.SaveOrUpdate(advInitiator);
            }
        }

        public void UpdatePostingStatusDocument(long DocumentID, string Status)
        {
            SCGDocument doc = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(DocumentID);
            if (doc != null)
            {
                doc.PostingStatus = Status;
                ScgeAccountingDaoProvider.SCGDocumentDao.SaveOrUpdate(doc);
            }
        }

        public void UpdatePostingStatusFnDocument(long DocumentID, string Status)
        {
            FnExpenseDocument docExpense = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(DocumentID);

            if (docExpense != null)
            {
                docExpense.RemittancePostingStatus = Status;
                ScgeAccountingDaoProvider.FnExpenseDocumentDao.SaveOrUpdate(docExpense);
            }
        }

        public byte[] GeneratePayIn(long documentID)
        {
            DocumentReportName report = ScgeAccountingQueryProvider.SCGDocumentQuery.GetReportNameByDocumentID(documentID);
            List<ReportParameter> param = new List<ReportParameter>();
            ReportParameter param1 = new ReportParameter();
            param1.ParamterName = "DocumentID";
            param1.ParamterValue = documentID.ToString();
            param.Add(param1);

            FilesGenerator rp = new FilesGenerator();

            byte[] results = rp.GetByte(ParameterServices.ReportingURL, ParameterServices.ReportUserName, ParameterServices.ReportPassword, ParameterServices.ReportDomainName, ParameterServices.ReportFolderPath, "PayInReport", param, FilesGenerator.ExportType.PDF);
            //byte[] results = rp.GetByte("http://172.20.56.116:81/ReportServer/ReportExecution2005.asmx", "RSeAccounting", "p@ssw0rd", "SCG-EACCOUNTING", "eAccountingReports", report.ReportName, param, FilesGenerator.ExportType.PDF);
            return results;
        }

        public void ValidateRemittanceRecievedMethod(long wfid)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            SS.Standard.WorkFlow.DTO.WorkFlow wf = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(wfid);
            if (wf != null && wf.CurrentState != null && wf.CurrentState.Name != null)
            {
                if (wf.CurrentState.Name.Equals(WorkFlowStateFlag.WaitRemittance))
                {
                    FnExpenseDocument expDoc = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(wf.Document.DocumentID);
                    if (string.IsNullOrEmpty(expDoc.ReceivedMethod))
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("ReceiveIsRequired"));
                    }
                }
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
        }
        public void UpdateMarkDocument(IList<ReimbursementReportValueObj> obj)
        {
            ScgeAccountingDaoProvider.SCGDocumentDao.UpdateMarkDocument(obj);
        }

        public void SendEmailToOverDueDate()
        {
            IList<SCGDocumentEmail> scgDocumentList = ScgeAccountingQueryProvider.SCGDocumentQuery.FindDocumentWaitApprove();

            foreach (SCGDocumentEmail item in scgDocumentList)
            {
                //SCGDocument document = FindByIdentity(item.DocumentID);
                string tokenCode = SaveResponseTokenEmail(item.CacheWorkflowID, item.UserID);
                SCGEmailService.SendEmailEM14(item.CacheWorkflowID, item.UserID, tokenCode);
            }
        }

        private string SaveResponseTokenEmail(long workFlowID, long userID)
        {
            WorkFlowResponseTokenService.ClearResponseTokenByWorkFlowID(workFlowID, TokenType.Email);

            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindProxyByIdentity(workFlowID);

            string tokenCode = Guid.NewGuid().ToString();
            IList<WorkFlowStateEventPermission> stateEventPermissions = WorkFlowQueryProvider.WorkFlowStateEventPermissionQuery.FindByWorkFlowID_UserID(workFlowID, userID);
            foreach (WorkFlowStateEventPermission stateEventPermission in stateEventPermissions)
            {
                WorkFlowResponseToken responseToken = new WorkFlowResponseToken();
                responseToken.TokenCode = tokenCode;
                responseToken.UserID = stateEventPermission.UserID.Value;
                responseToken.WorkFlow = workFlow;
                responseToken.TokenType = TokenType.Email.ToString();
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
    }
}

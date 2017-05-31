using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.eAccounting.DTO;
using System.Data;
using SCG.eAccounting.DTO.ValueObject;

namespace SCG.eAccounting.BLL
{
    public interface ISCGDocumentService : IService<SCGDocument, long>
    {
        void PrepareDataToDataset(DataSet documentDataset, long documentID);
        void UpdateTransactionDocument(Guid TxID, SCGDocument document, bool haveReceiver, bool haveApprover);
        long InsertSCGDocument(Guid txID, long documentID);
        long UpdateSCGDocument(Guid txID, long documentID);
        void ValidateSCGDocument(SCGDocument document, bool haveReceiver, bool haveApprover);
        long SaveSCGDocument(Guid txID, long documentID);
        byte[] GeneratePDF(long documentID);
        byte[] GenerateReimbursementReport(string markList, string unMarkList, string pbCode,string pbName, string companyName, string username, string maxPaidDate, string minPaidDate);
        void ValidateVerifyDetail(Guid txId, ViewPostDocumentType docType, bool isRepOffice);

        void MessageValidation(string chk);

        void DeleteDocumentByDocumentID(long documentID);

        void PrepareDataInternalToDataset(DataSet documentDataset, long documentID, bool isCopy);

        void UpdateAdvanceSCGDocument(long taDocumentID, long advDocumentID);

        void UpdatePostingStatusDocument(long DocumentID, string Status);
        void UpdatePostingStatusFnDocument(long DocumentID, string Status);

        void ValidateRemittanceRecievedMethod(long workFlowId);
        void UpdateMarkDocument(IList<ReimbursementReportValueObj> obj);

        void SendEmailToOverDueDate();
    }
}

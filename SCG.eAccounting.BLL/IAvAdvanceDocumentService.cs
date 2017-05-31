using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using SS.Standard.Data.NHibernate.Service;

using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.DataSet;

namespace SCG.eAccounting.BLL
{
	public interface IAvAdvanceDocumentService : IService<AvAdvanceDocument, long>
	{
        DataSet PrepareDS();
        DataSet PrepareDS(long documentId);
        AdvanceDataSet PrepareDataToDs(long documentId);
        AdvanceDataSet PrepareDataToDsTA(long taDocumentID);
        DataSet PrepareInternalDataToDataset(long documentId, bool isCopy);
        long AddAdvanceDocument(Guid txid, string advanceType);
        void UpdateAvDocumentTransaction(AvAdvanceDocument avAdvanceDoc, Guid txid);
        void UpdateTAdocumentIDTransaction(long advanceID);
        long SaveAvAdvance(Guid txid, long tempAdvanceId);
        void UpdateRemittanceAdvance(long remittanceId, double totalRemittanceAmt, bool isCancel);
        void UpdateRemittanceAdvanceForRepOffice(long remittanceId, double totalRemittanceAmtMain, bool isCancel);
        void UpdateClearingAdvance(long expenseId, double totalClearingAmt);
        //string GetAdvanceFromIDLatex(int advanceID);
        
        IList<object> GetVisibleFields(long? documentID);
        IList<object> GetEditableFields(long? documentID,long? taDocumentID);

        void DeleteAvAdvanceDocumentByAdvanceID(long advanceID);
        void SendEmailToOverDueDate();

        void UpdateAvAdvanceDocument(AvAdvanceDocument advance);
        void UpdateClearingAdvanceForRepOffice(long expenseId, double totalClearingAmt);
	}
}

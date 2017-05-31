using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;

using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;

using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;

namespace SCG.eAccounting.Query
{
	public interface IAvAdvanceDocumentQuery : IQuery<AvAdvanceDocument, long>
	{
        //ISQLQuery FindByAdvanceCriteria(string advanceNo, string description, bool isCount, string sortExpression);
        ISQLQuery FindByAdvanceCriteria(Advance advance, bool isCount, string sortExpression);
        //IList<Advance> GetAdvanceList(string advanceNo, string description, int firstResult, int maxResult, string sortExpression);
        IList<Advance> GetAdvanceList(Advance advance, int firstResult, int maxResult, string sortExpression);
        //int CountByAdvanceCriteria(string advanceNo, string description);

        ISQLQuery FindAdvanceRelateWithRemittanceButNotInExpenseByAdvanceCriteria(Advance advance, bool isCount, string sortExpression);
        IList<Advance> GetAdvanceListRelateWithRemittanceButNotInExpense(Advance advance, int firstResult, int maxResult, string sortExpression);

        int CountByAdvanceCriteria(Advance advance);

        IList<Advance> FindByAdvanceID(long advanceID);
        IList<AdvanceOutstanding> FindOutstandingRequest(long requesterID, long currentWorkFlowID, int documentTypeAdvance, int documentTypeExpense, IList<int> documentStatus, short languageID, int firstResult, int maxResults, string sortExpression);
        ISQLQuery FindOutstandingQuery(long requesterID, long currentWorkFlowID, int documentTypeAdvance, int documentTypeExpense, 
            IList<int> documentStatus, short languageID, bool isCount, string sortExpression);
        int CountFindOutstandingRequest(long requesterID, long currentWorkFlowID, int documentTypeAdvance, int documentTypeExpense, IList<int> documentStatus, short languageID);
        
        /*n-edited count outstanding advance for alert wanning*/
        int CountAdvanceForFixedAdvance(long companyID, long requesterID, long currentWorkFlowID, int documentTypeAdvance, DateTime curdate);
        ISQLQuery CountAdvanceOutstandingForFixedAdvance(long companyID, long requesterID, long currentWorkFlowID, int documentTypeAdvance, DateTime curdate);

        bool isSeeHistoryReject(long documentID);

        IList<Advance> FindRemittanceTAAdvanceByTADocumentID(long taID);
        AvAdvanceDocument GetAvAdvanceByDocumentID(long documentID);
        AdvanceDataForEmail GetAvAdvanceforEmailByDocumentID(long documentID);
        IList<Advance> SumAmountByPaymentTypeAndCurrency(IList<long> advanceID);
        IList<Advance> SumAmountByPaymentTypeAndCurrencyForRepOffice(IList<long> advanceID);
        //IList<AvAdvanceDocument> FindAdvanceDocumentByTADocumentID(long taDocumentID);
        IList<Advance> FindAdvanceDocumentByTADocumentID(long taDocumentID);
        IList<Advance> FindAdvanceDocumentForRequesterByTADocumentID(long requesterId, long taDocumentID);
		InvoiceExchangeRate GetPerdiemExchangeRateUSD(IList<long> advanceIDList);
        IList<Advance> FindAdvancDocumentByAdvanceIds(IList<long> advanceIdList);
        IList<AvAdvanceDocument> FindByTADocumentID(long taDocumentID);

        IList<VoAdvanceFromTA> FindAdvanceFromTA(long tadocument);
        IList<AvAdvanceDocument> FindAdvancDocumentByWorkFlowID(long workFlowID);
        AvAdvanceDocument FindAdvanceByWorkFlowID(long workFlowID);
        IList<AvAdvanceDocument> FindAdvanceReferenceTAByTADocumentID(long taDocumentID);
        bool isSeeMessage(long documentID);
        int isMessage(long documentID);
		IList<Advance> FindRemittanceAmountByAdvanceIDs(List<long> advanceIdList);
        double GetExchangeRateMainCurrencyToTHB(IList<long> advanceIdList);
        double GetExchangeRateLocalCurrencyToMainCurrency(IList<long> advanceIdList);
	}
}

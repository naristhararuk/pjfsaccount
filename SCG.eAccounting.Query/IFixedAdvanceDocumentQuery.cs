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
	public interface IFixedAdvanceDocumentQuery : IQuery<FixedAdvanceDocument, long>
	{
        ISQLQuery FindByAdvanceCriteria(Advance advance, bool isCount, string sortExpression);
        IList<Advance> GetAdvanceList(Advance advance, int firstResult, int maxResult, string sortExpression);

        ISQLQuery FindAdvanceRelateWithRemittanceButNotInExpenseByAdvanceCriteria(Advance advance, bool isCount, string sortExpression);
        IList<Advance> GetAdvanceListRelateWithRemittanceButNotInExpense(Advance advance, int firstResult, int maxResult, string sortExpression);

        int CountByAdvanceCriteria(Advance advance);
        FixedAdvanceDocument FindNetAmountQuery(long docId);

        IList<Advance> FindByAdvanceID(long advanceID);
   
        //int CountFindOutstandingRequest(long requesterID, long currentWorkFlowID, int documentTypeAdvance, int documentTypeExpense, IList<int> documentStatus, short languageID);

        bool isSeeHistoryReject(long documentID);
        FixedAdvanceDocument GetFixedAdvanceByFixedAdvanceID(long fixedAdvanceID);
        FixedAdvanceDocument GetFixedAdvanceByDocumentID(long documentID);
        IList<FixedAdvanceRefObjectValues> FindRefFixedAdvance(long comId, long userId, long requesterId, long docId, string seachType);
        /*n-edited check ref fixedadvance*/
        IList<FixedAdvanceCanRefObjectValues> FindFixedAdvanceCanRef(long comId, long userId, long requesterId, long docId, long currecurrentfixedid);
        AdvanceDataForEmail GetAvAdvanceforEmailByDocumentID(long documentID);

        IList<Advance> FindAdvanceDocumentByTADocumentID(long taDocumentID);
        IList<Advance> FindAdvanceDocumentForRequesterByTADocumentID(long requesterId, long taDocumentID);
		InvoiceExchangeRate GetPerdiemExchangeRateUSD(IList<long> advanceIDList);
        IList<Advance> FindAdvancDocumentByAdvanceIds(IList<long> advanceIdList);
        IList<AvAdvanceDocument> FindByTADocumentID(long taDocumentID);

        IList<VoAdvanceFromTA> FindAdvanceFromTA(long tadocument);
        IList<AvAdvanceDocument> FindAdvancDocumentByWorkFlowID(long workFlowID);
        AvAdvanceDocument FindAdvanceByWorkFlowID(long workFlowID);
        bool isSeeMessage(long documentID);
        int isMessage(long documentID);
        int CountFindFixedAdvanceOutstandingRequest(long requesterID, long companyID, long currentWorkFlowID, short languageID);
        IList<FixedAdvanceOutstanding> FindFixedAdvanceOutstandingRequest(long requesterID,long companyID, long currentWorkFlowID, short languageID, int firstResult, int maxResults, string sortExpression);
        ISQLQuery FindFixedAdvanceOutstandingQuery(long requesterID, long companyID, long currentWorkFlowID, short languageID, bool isCount, string sortExpression);
        bool FindFixedAdvanceOutStandingFromAleart(long requesterID, long CompanyID);

        int CountFindFixedAdvanceOutstandingRequestForExpense(long requesterID, long companyID, long currentWorkFlowID, short languageID);
        IList<FixedAdvanceOutstanding> FindFixedAdvanceOutstandingRequestForExpense(long requesterID, long companyID, long currentWorkFlowID, short languageID, int firstResult, int maxResults, string sortExpression);
        ISQLQuery FindFixedAdvanceOutstandingQueryForExpense(long requesterID, long companyID, long currentWorkFlowID, short languageID, bool isCount, string sortExpression);
        
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.Query.NHibernate;

using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;
using NHibernate;

namespace SCG.eAccounting.Query
{
    public interface ISCGDocumentQuery : IQuery<SCGDocument, long>
    {
        SCGDocument GetSCGDocumentByDocumentID(long documentID);
        string GetDocumentCurrentStateName(short languageID, long documentID);

        ISQLQuery FindByDocumentCriteria(bool isCount, string sortExpression, string documentNo, string description, long companyID, long requesterID, string travelBy, Enum queryFor, Enum withoutDocument,long? taDocumentID);
        int CountByDocumentCriteria(string documentNo, string description, long companyID, long requesterID, string travelBy, Enum queryFor, Enum withoutDocument, long? taDocumentID);
        IList<TADocumentObj> GetDocumentList(int firstResult, int maxResult, string sortExpression, string documentNo, string description, long companyID, long requesterID, string travelBy, Enum queryFor, Enum withoutDocument, long? taDocumentID);
        IList<TranslatedListItem> FindDocumentTypeCriteria();
        IList<TranslatedListItem> FindDocumentTypeCriteriaNoTA(); 
        IList<TADocumentObj> FindByDocumentIdentity(long documentId);
        IList<ExportDocumentImage> FindDocumentImage(string status, string sapCode);
        ISQLQuery FindInboxEmployeeCriteria(SearchCriteria criteria, bool isCount, string sortExpression);
        IList<SearchResultData> GetCriteriaList(SearchCriteria criteria, int firstResult, int maxResult, string sortExpression);
        int CountCriteria(SearchCriteria criteria);
        ISQLQuery FindInboxAccountantPaymentCriteria(SearchCriteria criteria, bool isCount, string sortExpression);
        IList<SearchResultData> GetAccountantPaymentCriteriaList(SearchCriteria criteria, int firstResult, int maxResult, string sortExpression);
        int CountAccountantPaymentCriteria(SearchCriteria criteria);
        int GetWorkStateEvent(int workFlowStateID, string name);
        IList<SearchResultData> FindDraftCriteria(long userID);
        ISQLQuery FindByDocumentFollowReportCriteria(bool isPosting, short languageId, long companyID, string fromLocationCode, string toLocationCode, string fromDate, string toDate, long creatorId, long requestId, long costCenterId, long serviceTeamId, int valueStatus, bool isCount, string sortExpression);
        IList<VODocumentFollowUpReport> GetDocumentReportList(int firstResult, int maxResult, string sortExpression, bool isPosting, short languageId, long companyID, string fromLocationCode, string toLocationCode, string fromDate, string toDate, long creatorId, long requestId, long costCenterId, long serviceTeamId, int valueStatus);
        int CountByDocumentReportCriteria(bool isPosting, short languageId, long companyID, string fromLocationCode, string toLocationCode, string fromDate, string toDate, long creatorId, long requestId, long costCenterId, long serviceTeamId, int valueStatus);

        ISQLQuery FindByAdvanceOverDueReportCriteria(VOAdvanceOverDueReport vo, bool isCount, string sortExpression);
        IList<VOAdvanceOverDueReport> GetAdvanceDocumentReportList(int firstResult, int maxResult, string sortExpression, VOAdvanceOverDueReport vo);
        int CountByAdvanceReportCriteria(VOAdvanceOverDueReport vo);

        IList<TranslatedListItem> FindStatusCriteria(short languageID, int documentTypeID);

        DocumentReportName GetReportNameByDocumentID(long DocumentID);

        IList<SearchResultData> FindInboxEmployeeSummaryCriteria(SearchCriteria criteria);
        IList<SearchResultData> FindInboxAccountantPaymentSummaryCriteria(SearchCriteria criteria);

        IList<SearchResultData> GetAccountantPaymentCriteriaList(SearchCriteria criteria);
        int CountDraftNoDocumentEmployeeCriteria(SearchCriteria criteria);
        IList<long> GetDocumentIDFollowUpList();

        /*FixedAdvance Report*/
        ISQLQuery FindByFixedAdvanceOverDueReportCriteria(VOFixedAdvanceOverDueReport vo, bool isCount, string sortExpression);
        IList<VOFixedAdvanceOverDueReport> GetFixedAdvanceDocumentReportList(int firstResult, int maxResult, string sortExpression, VOFixedAdvanceOverDueReport vo);
        int CountByFixedAdvanceReportCriteria(VOFixedAdvanceOverDueReport vo);

        IList<VOAdvanceOverDueReport> GetAdvanceOverdueList();
        IList<FixedAdvanceOverDue> GetFixedAdvanceOverdueList();

        /*เพิ่มแจ้งเตือนก่อนครบกำหนด fixedadvance*/
        IList<FixedAdvanceBeforeDue> GetFixedAdvanceBeforedueList();

        IList<long> GetDocumentIDByReferenceNo(string refNo);
        IList<long> GetDocumentIDByDocumentNo(string docNo);

        ISQLQuery FindReportByCriteria(ReimbursementReportValueObj report, string sortExpression, bool isCount);
        IList<ReimbursementReportValueObj> GetReportList(ReimbursementReportValueObj report, int firstResult, int maxResult, string sortExpression);
        int CountReportByCriteria(ReimbursementReportValueObj report);

        ReimbursementReportValueObj GetPeriodPaidDate(string markList, string unmarkList);

        IList<TASearchResultData> GetTADocumentCriteriaList(TASearchCriteria criteria, int firstResult, int maxResult, string sortExpression);
        int CountTADocumentCriteria(TASearchCriteria criteria);

        IList<SCGDocumentEmail> FindDocumentWaitApprove();


        
    }
}

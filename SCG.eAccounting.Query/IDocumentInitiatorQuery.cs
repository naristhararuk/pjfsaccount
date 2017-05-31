using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;
using SS.Standard.Data.Query.NHibernate;

namespace SCG.eAccounting.Query
{
    public interface IDocumentInitiatorQuery : IQuery<DocumentInitiator, long>
    {
        IList<InitiatorData> GetDocumentInitiatorList(long documentID);
        IList<DocumentInitiator> GetDocumentInitiatorByDocumentID(long documentID);
        IList<DocumentInitiator> GetResponsedInitiatorByDocumentID(long documentID);
        IList<DocumentInitiator> GetNotResponseInitiatorByDocumentID(long documentID);
        DocumentInitiator GetNextResponseInitiatorByDocumentID(long documentID);
        IList<DocumentInitiator> GetOverRoleInitiatorByDocumentID(long documentID);
        IList<DocumentInitiator> GetCCInitiatorByDocumentID(long documentID);        
        IList<DocumentInitiator> FindDocumentInitiatorByDocumentID_UserID(long documentID, long userID);
        IList<UserFavoriteInitiator> FindUserFavoriteInitiatorByUserID(InitiatorCriteria criteria, long UserID);
        IList<DocumentInitiatorLang> GetDocumentInitiatorByDocumentIDAndInitiatorType(long documentID);
    }
}

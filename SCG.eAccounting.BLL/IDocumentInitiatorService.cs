using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.DTO;
using SS.Standard.Data.NHibernate.Service;

using SCG.eAccounting.Query;
using SCG.eAccounting.DTO.ValueObject;
using System.Data;

namespace SCG.eAccounting.BLL
{
    public interface IDocumentInitiatorService : IService<DocumentInitiator, long>
    {
        void AddInitiator(DocumentInitiator Initiator);
        void DeleteInitiator(long DocumentID,long InitiatorID);
        void UpdateInitiatorSequence(int OldSequence, int NewSequence);
        void UpdateInitiator(long DocumentID,IList<DocumentInitiatorLang> DocumentInitiatorList, IList<DocumentInitiatorLang> DocumentInitiatorDeleteList);
        IList<DocumentInitiator> FindByDocumentID(long DocumentID);
        void InsertDocumentInitiator(Guid txID, long documentID);
        void UpdateDocumentInitiator(Guid txID, long documentID);
        void DeleteDocumentInitiator(Guid txID);
        void PrepareDataToDataset(DataSet documentDataset, long documentID);
        void SaveDocumentInitiator(Guid txID, long documentID);
        void PrepareDataToDatasetAdvance(DataSet documentDataset, long documentID); //create Advance from TA
        void UpdateDocumentInitiatorWhenOverRole(IList<DocumentInitiator> documentInitiators);
        void ValidateDocumentInitiator(Guid txID, long documentID);
    }
}

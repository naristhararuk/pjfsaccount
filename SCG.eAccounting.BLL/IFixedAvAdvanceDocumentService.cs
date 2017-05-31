using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using SS.Standard.Data.NHibernate.Service;

using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.DataSet;
using SCG.eAccounting.DTO.ValueObject;

namespace SCG.eAccounting.BLL
{
    public interface IFixedAdvanceDocumentService : IService<FixedAdvanceDocument, long>
	{
        DataSet PrepareDS();
        DataSet PrepareDS(long documentId, bool isCopy);
        //AdvanceDataSet PrepareDataToDs(long documentId);
        FixedAdvanceDocument FindNetAmount(long docId);
       
        DataSet PrepareInternalDataToDataset(long documentId, bool isCopy);
        long AddFixedAdvanceDocument(Guid txid, string advanceType);
        void UpdateFixedDocumentTransaction(FixedAdvanceDocument fixedAvAdvanceDoc, Guid txid);

        long SaveFixedAdvance(Guid txid, long tempfixedAdvanceId);   
        IList<object> GetVisibleFields(long? documentID);
        IList<object> GetEditableFields(long? documentID);
        IList<FixedAdvanceRefObjectValues> FindRefFixedAdvance(long comId, long userId, long requesterId, long docId, string seachType);
        /*n-edit*/
        IList<FixedAdvanceCanRefObjectValues> FindFixedAdvanceCanRef(long comId, long userId, long requesterId, long docId, long currentfixedid);
        void SendEmailToOverDueDate();
        void SendEmailToBeforeDueDate();

        void UpdateFixedAdvanceDocument(FixedAdvanceDocument fixedadvance);
        
    }
}

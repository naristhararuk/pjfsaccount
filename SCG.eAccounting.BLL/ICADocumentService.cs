using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using SCG.eAccounting.DTO;
using System.Data;

namespace SCG.eAccounting.BLL
{
    public interface ICADocumentService : IService<CADocument, long>
    {
        DataSet PrepareDS();
        DataSet PrepareDS(long documentID);
        long AddCADocumentTransaction(Guid txID);
        void UpdateCADocumentTransaction(Guid txID, CADocument caDocument);
        IList<object> GetVisibleFields(long? documentID);
        IList<object> GetEditableFields(long? documentID);
        long SaveCADocument(Guid txID, long caDocumentID);
        DataSet PrepareDataToDataset(long documentID, bool isCopy);
    }
}

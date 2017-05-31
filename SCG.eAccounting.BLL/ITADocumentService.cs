using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using SCG.eAccounting.DTO;
using System.Data;
using SCG.eAccounting.DTO.DataSet;

namespace SCG.eAccounting.BLL
{
    public interface ITADocumentService : IService<TADocument, long>
    {
        DataSet PrepareDS();
        DataSet PrepareDS(long documentID);
        long AddTADocumentTransaction(Guid txtID);
        void UpdateTADocumentTransaction(Guid txtID, TADocument taDocument);
        IList<object> GetVisibleFields(long? documentID);
        IList<object> GetEditableFields(long? documentID);
        long SaveTADocument(Guid txID, long taDocumentID);
        DataSet PrepareDataToDataset(long documentID, bool isCopy);
    }
}

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
    public interface IMPADocumentService : IService<MPADocument, long>
    {
        DataSet PrepareDS();
        DataSet PrepareDS(long documentID);
        long AddMPADocumentTransaction(Guid txID);
        void  UpdateMPADocumentTransaction(Guid txID, MPADocument mpaDocument);
        IList<object> GetVisibleFields(long? documentID);
        IList<object> GetEditableFields(long? documentID);
        long SaveMPADocument(Guid txID, long mpaDocumentID);
        DataSet PrepareDataToDataset(long documentID, bool isCopy);
    }
}

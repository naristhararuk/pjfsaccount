using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.DataSet;
using System.Data;

namespace SCG.eAccounting.BLL
{
    public interface ITADocumentAdvanceService : IService<TADocumentAdvance, int>
    {
        void PrepareDataToDataset(TADocumentDataSet taDocumentDS, long taDocumentID);
        int AddTADocumentAdvanceTransaction(Guid txID, TADocumentAdvance taDocumentAdvance);
        void UpdateTADocumentAdvanceTransaction(Guid txID, TADocumentAdvance taDocumentAdvance);
        void SaveTADocumentAdvance(Guid txID, long taDocumentID);
        void DeleteTADocumentAdvanceTransaction(Guid txID, long taDocumentID, long advanceID);
    }
}
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
    public interface IMPAItemService : IService<MPAItem , long>
    {
        void ChangeRequesterMAPItem(Guid txID, MPAItem taDocumentTraveller);
        long AddMPAItemTransaction(Guid txID, MPAItem mpaItem);
        DataTable DeleteMPAItemTransaction(Guid txID, long MPAItemID);
        void UpdateMPAItemTransaction(Guid txID, MPAItem mpaItem);
        void SaveMPAItem(Guid txID, long taDocumentID);
        void PrepareDataToDataset(MPADocumentDataSet MPAItemDS, long mpaDocumentID);
    }
}
